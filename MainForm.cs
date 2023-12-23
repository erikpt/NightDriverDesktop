using NightDriver;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TreeView;

namespace NightDriver
{
    public partial class MainForm : Form, IMessageFilter
    {
        public enum DWMWINDOWATTRIBUTE : uint
        {
            DWMWA_USE_IMMERSIVE_DARK_MODE = 20,
            DWMWA_WINDOW_CORNER_PREFERENCE = 33,
        }

        [DllImport("dwmapi.dll", CharSet = CharSet.Unicode, PreserveSig = false)]
        public static extern void DwmSetWindowAttribute(IntPtr hwnd,
                                                DWMWINDOWATTRIBUTE attribute,
                                                ref int pvAttribute,
                                                uint cbAttribute);

        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == 0x20a)
            {
                // WM_MOUSEWHEEL, find the control at screen position m.LParam
                Point pos = new Point(m.LParam.ToInt32() & 0xffff, m.LParam.ToInt32() >> 16);
                IntPtr hWnd = WindowFromPoint(pos);
                if (hWnd != IntPtr.Zero && hWnd != m.HWnd && Control.FromHandle(hWnd) != null)
                {
                    SendMessage(hWnd, m.Msg, m.WParam, m.LParam);
                    return true;
                }
            }
            return false;
        }

        // P/Invoke declarations
        [DllImport("user32.dll")]
        private static extern IntPtr WindowFromPoint(Point pt);
        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);

        LEDServer _server = new LEDServer();

        private Task backgroundTask;
        private CancellationTokenSource cancellationTokenSource;

        private void StartButton_Click(object sender, EventArgs e)
        {
            NightDriver.ConsoleApp.Stats.WriteLine("Start Server");
            StartButton.Enabled = false;
            cancellationTokenSource = new CancellationTokenSource();
            var token = cancellationTokenSource.Token;

            backgroundTask = Task.Run(() =>
            {
                _server.Start(token);
            }, token);
            StopButton.Enabled = true;

            FillListView();
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            NightDriver.ConsoleApp.Stats.WriteLine("Stop Server");
            _server.Stop(cancellationTokenSource);
            StartButton.Enabled = true;
            StopButton.Enabled = false;
        }

        public MainForm()
        {
            Application.AddMessageFilter(this);

            var preference = Convert.ToInt32(true);
            DwmSetWindowAttribute(this.Handle,
                                  DWMWINDOWATTRIBUTE.DWMWA_USE_IMMERSIVE_DARK_MODE,
                                  ref preference, sizeof(uint));

            InitializeComponent();
            _server.LoadStrips();
            FillListView();
            timerListView.Start();
            timerVisualizer.Start();
        }

        private void tabControl_DrawItem(object? sender, DrawItemEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = _server.AllStrips.Count().ToString() + " Strips, " + Process.GetCurrentProcess().Threads.Count.ToString() + " Threads";
            stripList.Invalidate();
            textLog.Text = NightDriver.ConsoleApp.Stats.Text;
        }

        internal void FillListView()
        {
            stripList.VirtualListSize = _server.AllStrips.Count();
        }

        class LEDServer
        {
            public Statistics Stats = new Statistics();

            // Main
            //
            // Application main loop - starts worker threads

            public List<Location> AllLocations = new List<Location>
            {
              new Tree()             { FramesPerSecond = 28 },

              new CeilingStrip        { FramesPerSecond = 30 },

              new Cabana()            { FramesPerSecond = 20 },

              new TV()                { FramesPerSecond = 20 },
              new ShopCupboards()     { FramesPerSecond = 20 },

              new ShopSouthWindows1() { FramesPerSecond = 2 },
              new ShopSouthWindows2() { FramesPerSecond = 2 },
              new ShopSouthWindows3() { FramesPerSecond = 2 },
            };

            public readonly List<LightStrip> AllStrips = new List<LightStrip>();

            internal void LoadStrips()
            {
                AllStrips.Clear();
                foreach (var location in AllLocations)
                {
                    foreach (var strip in location.LightStrips)
                    {
                        strip.Location = location;
                        AllStrips.Add(strip);
                    }
                }
            }

            internal void StartCommunications()
            {
                foreach (var location in AllLocations)
                    foreach (var strip in location.LightStrips)
                        strip.Start();
            }

            internal void StopCommunications()
            {
                foreach (var location in AllLocations)
                    foreach (var strip in location.LightStrips)
                        strip.Stop();
            }


            internal void Start(CancellationToken token)
            {
                foreach (var location in AllLocations)
                    location.StartWorkerThread(token);
                StartCommunications();
            }

            internal void Stop(CancellationTokenSource cancelSource)
            {
                cancelSource.Cancel();
                StopCommunications();
            }

        }

        private void stripList_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            double epoch = (DateTime.UtcNow.Ticks - DateTime.UnixEpoch.Ticks) / (double)TimeSpan.TicksPerSecond;

            var strip = _server.AllStrips[e.ItemIndex];
            e.Item = new ListViewItem(strip.FriendlyName);
            e.Item.SubItems.Add(new ListViewItem.ListViewSubItem(e.Item, strip.HostName));

            if (strip.HasSocket)
            {
                e.Item.SubItems.Add(new ListViewItem.ListViewSubItem(e.Item, strip.HasSocket ? "Open" : "No"));
                e.Item.SubItems.Add(new ListViewItem.ListViewSubItem(e.Item, strip.Response.wifiSignal.ToString()));
                e.Item.SubItems.Add(new ListViewItem.ListViewSubItem(e.Item, strip.ReadyForData ? "Ready" : "No"));
                e.Item.SubItems.Add(new ListViewItem.ListViewSubItem(e.Item, strip.BytesPerSecond.ToString()));
                e.Item.SubItems.Add(new ListViewItem.ListViewSubItem(e.Item, strip.Response.currentClock > 8 ? (strip.Response.currentClock - epoch).ToString("F2") : "UNSET"));
                e.Item.SubItems.Add(new ListViewItem.ListViewSubItem(e.Item, strip.Response.bufferPos.ToString() + "/" + strip.Response.bufferSize.ToString()));
                e.Item.SubItems.Add(new ListViewItem.ListViewSubItem(e.Item, strip.Response.watts.ToString()));
                e.Item.SubItems.Add(new ListViewItem.ListViewSubItem(e.Item, strip.Response.fpsDrawing.ToString()));
                e.Item.SubItems.Add(new ListViewItem.ListViewSubItem(e.Item, strip.TimeOffset.ToString()));

            }
            else
            {
                // Without an active socket, these next columns are all empty
                for (int i = 0; i < 9; i++)
                    e.Item.SubItems.Add(new ListViewItem.ListViewSubItem(e.Item, "---"));
            }
            e.Item.SubItems.Add(new ListViewItem.ListViewSubItem(e.Item, strip.Connects.ToString()));
            e.Item.SubItems.Add(new ListViewItem.ListViewSubItem(e.Item, strip.QueueDepth.ToString()));
            e.Item.SubItems.Add(new ListViewItem.ListViewSubItem(e.Item, strip.Location.CurrentEffectName));
        }

        private void stripList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (stripList.SelectedIndices.Count > 0)
            {
                var strip = _server.AllStrips[stripList.SelectedIndices[0]];
                panelVisualizer.ColorData = strip.Location.LEDs;
                timerVisualizer.Interval = Math.Clamp(1000 / strip.Location.FramesPerSecond, 20, 500);
            }
        }

        private void timerVisualizer_Tick(object sender, EventArgs e)
        {
            panelVisualizer.Invalidate();
        }
    }
}
