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
            UpdateListView();
            toolStripStatusLabel1.Text = _server.AllStrips.Count().ToString() + " Strips, " + Process.GetCurrentProcess().Threads.Count.ToString() + " Threads";
            textLog.Text = NightDriver.ConsoleApp.Stats.Text;
        }

        internal void FillListView()
        {
            stripList.Groups.Clear();
            //foreach (var location in _server.AllLocations)
            //    stripList.Groups.Add(new ListViewGroup(location.GetType().Name));

            stripList.Items.Clear();
            foreach (var strip in _server.AllStrips)
            {
                var typename = strip.Location.GetType().Name;
                ListViewGroup group = stripList.Groups[typename];
                if (group == null)
                    group = stripList.Groups.Add(typename, typename);
                stripList.Items.Add(new StripListItem(group, strip.FriendlyName, strip));
            }
        }

        internal void UpdateListView()
        {
            foreach (var strip in _server.AllStrips)
            {
                var item = stripList.FindItemWithText(strip.FriendlyName, true, 0) as StripListItem;
                var newItem = StripListItem.CreateForStrip(null, strip);
                if (item.UpdateColumnText(newItem))
                    stripList.Invalidate();
            }
        }

        class LEDServer
        {
            public Statistics Stats = new Statistics();

            // Main
            //
            // Application main loop - starts worker threads

            public List<Location> AllLocations = new List<Location>
            {
              new Tree()              { FramesPerSecond = 28 },

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

        private void stripList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (stripList.SelectedIndices.Count > 0)
            {
                var strip = stripList.SelectedItems[0].Tag as LightStrip;
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

class StripListItem : ListViewItem
{
    public enum ColumnIndex
    {
        INVALID = -1,
        FIRST = 1,
        iHost = 1,
        iHasSocket,
        iWiFiSignal,
        iReadyForData,
        iBytesPerSecond,
        iCurrentClock,
        iBufferPos,
        iWatts,
        iFpsDrawing,
        iTimeOffset,
        iConnects,
        iQueueDepth,
        iCurrentEffect,
        MAX = iCurrentEffect
    };

    public string Host
    {
        get
        {
            return SubItems[(int)ColumnIndex.iHost].Text;
        }
        set
        {
            SubItems[(int)ColumnIndex.iHost].Text = value;
        }
    }

    public string HasSocket
    {
        get
        {
            return SubItems[(int)ColumnIndex.iHasSocket].Text;
        }
        set
        {
            SubItems[(int)ColumnIndex.iHasSocket].Text = value;
        }
    }

    public string WiFiSignal
    {
        get
        {
            return SubItems[(int)ColumnIndex.iWiFiSignal].Text;
        }
        set
        {
            SubItems[(int)ColumnIndex.iWiFiSignal].Text = value;
        }
    }

    public string ReadyForData
    {
        get
        {
            return SubItems[(int)ColumnIndex.iReadyForData].Text;
        }
        set
        {
            SubItems[(int)ColumnIndex.iReadyForData].Text = value;
        }
    }

    public string BytesPerSecond
    {
        get
        {
            return SubItems[(int)ColumnIndex.iBytesPerSecond].Text;
        }
        set
        {
            SubItems[(int)ColumnIndex.iBytesPerSecond].Text = value;
        }
    }

    public string CurrentClock
    {
        get
        {
            return SubItems[(int)ColumnIndex.iCurrentClock].Text;
        }
        set
        {
            SubItems[(int)ColumnIndex.iCurrentClock].Text = value;
        }
    }

    public string BufferPos
    {
        get
        {
            return SubItems[(int)ColumnIndex.iBufferPos].Text;
        }
        set
        {
            SubItems[(int)ColumnIndex.iBufferPos].Text = value;
        }
    }

    public string Watts
    {
        get
        {
            return SubItems[(int)ColumnIndex.iWatts].Text;
        }
        set
        {
            SubItems[(int)ColumnIndex.iWatts].Text = value;
        }
    }

    public string FpsDrawing
    {
        get
        {
            return SubItems[(int)ColumnIndex.iFpsDrawing].Text;
        }
        set
        {
            SubItems[(int)ColumnIndex.iFpsDrawing].Text = value;
        }
    }

    public string TimeOffset
    {
        get
        {
            return SubItems[(int)ColumnIndex.iTimeOffset].Text;
        }
        set
        {
            SubItems[(int)ColumnIndex.iTimeOffset].Text = value;
        }
    }

    public string Connects
    {
        get
        {
            return SubItems[(int)ColumnIndex.iConnects].Text;
        }
        set
        {
            SubItems[(int)ColumnIndex.iConnects].Text = value;
        }
    }

    public string QueueDepth
    {
        get
        {
            return SubItems[(int)ColumnIndex.iQueueDepth].Text;
        }
        set
        {
            SubItems[(int)ColumnIndex.iQueueDepth].Text = value;
        }
    }

    public string CurrentEffect
    {
        get
        {
            return SubItems[(int)ColumnIndex.iCurrentEffect].Text;
        }
        set
        {
            SubItems[(int)ColumnIndex.iCurrentEffect].Text = value;
        }
    }

    public string Location
    {
        get
        {
            return Text;
        }
        set
        {
            Text = value;
            Group = new ListViewGroup(value);
        }
    }


    // UpdateColumnText

    public bool UpdateColumnText(StripListItem other, ColumnIndex idx = ColumnIndex.INVALID)
    {
        if (idx == ColumnIndex.INVALID)
        {
            bool bChanged = false;
            for (int i = 0; i < SubItems.Count; i++)
                bChanged |= UpdateColumnText(other, (ColumnIndex)i);
            return bChanged;
        }
        else
        {
            if (SubItems[(int)idx].Text != other.SubItems[(int)idx].Text)
            {
                SubItems[(int)idx].Text = other.SubItems[(int)idx].Text;
                return true;
            }
            return false;
        }
    }

    public StripListItem(ListViewGroup group, String text, LightStrip strip)
    {
        Text = text;
        Group = group;
        Tag = strip;

        for (ColumnIndex i = 0; i < ColumnIndex.MAX; i++)
            SubItems.Add(new ListViewItem.ListViewSubItem(this, "---"));
    }

    public static StripListItem CreateForStrip(ListViewGroup group, LightStrip strip)
    {
        double epoch = (DateTime.UtcNow.Ticks - DateTime.UnixEpoch.Ticks) / (double)TimeSpan.TicksPerSecond;
        var item = new StripListItem(group, strip.FriendlyName, strip);

        item.Host           = strip.HostName;
        item.HasSocket      = !strip.HasSocket ? "No"   : "Open";
        item.WiFiSignal     = !strip.HasSocket ? "--- " : strip.Response.wifiSignal.ToString();
        item.ReadyForData   = !strip.HasSocket ? "--- " : strip.ReadyForData ? "Ready" : "No";
        item.BytesPerSecond = !strip.HasSocket ? "--- " : strip.BytesPerSecond.ToString();
        item.CurrentClock   = !strip.HasSocket ? "--- " : strip.Response.currentClock > 8 ? (strip.Response.currentClock - epoch).ToString("F2") : "UNSET";
        item.BufferPos      = !strip.HasSocket ? "--- " : strip.Response.bufferPos.ToString() + "/" + strip.Response.bufferSize.ToString();
        item.Watts          = !strip.HasSocket ? "--- " : strip.Response.watts.ToString();
        item.FpsDrawing     = !strip.HasSocket ? "--- " : strip.Response.fpsDrawing.ToString();
        item.TimeOffset     = !strip.HasSocket ? "--- " : strip.TimeOffset.ToString();
        item.Connects       = strip.Connects.ToString();
        item.QueueDepth     = strip.QueueDepth.ToString();
        item.CurrentEffect  = strip.Location.CurrentEffectName;
        item.Group = group;

        Debug.Assert(item.Tag == strip);
        
        return item;
    }
}
