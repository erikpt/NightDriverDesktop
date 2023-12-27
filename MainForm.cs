//+--------------------------------------------------------------------------
//
// NightDriver.Net - (c) 2019 Dave Plummer.  All Rights Reserved.
//
// File:        MainForm.cs
//
// Description:
//
//   The main WinForms app window, which in turn contains a tab control, 
//   and those tabs contains the main UI for the app as well as logging etc.
//
// History:     Dec-23-2023        Davepl      Created
//
//---------------------------------------------------------------------------


using System.Diagnostics;
using System.Runtime.InteropServices;

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
                var typename = strip.StripSite.GetType().Name;
                ListViewGroup group = stripList.Groups[typename];
                if (group == null)
                    group = stripList.Groups.Add(typename, typename);
                stripList.Items.Add(new StripListItem(group, strip.FriendlyName, strip));
            }
            UpdateUIStates();
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
            var comparer = stripList.ListViewItemSorter as StripListItemComparer;
            if (comparer == null)
                stripList.ListViewItemSorter = new StripListItemComparer(StripListViewColumnIndex.FIRST);
            stripList.Sort();
            UpdateUIStates();
        }

        private void stripList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (stripList.SelectedIndices.Count > 0)
            {
                var strip = stripList.SelectedItems[0].Tag as LightStrip;
                panelVisualizer.ColorData = strip.StripSite.LEDs;
                timerVisualizer.Interval = Math.Clamp(1000 / strip.StripSite.FramesPerSecond, 20, 500);
            }
            UpdateUIStates();
        }

        private void timerVisualizer_Tick(object sender, EventArgs e)
        {
            panelVisualizer.Invalidate();
        }

        private void checkGroupItems_CheckedChanged(object sender, EventArgs e)
        {
            stripList.ShowGroups = checkGroupItems.Checked;
            FillListView();
        }

        private void stripList_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            var comparer = stripList.ListViewItemSorter as StripListItemComparer;
            if (comparer != null && comparer.Column == (StripListViewColumnIndex)e.Column)
                comparer.ToggleSortOrder();
            else
                stripList.ListViewItemSorter = new StripListItemComparer((StripListViewColumnIndex)e.Column);

            stripList.Sort();
        }

        private void UpdateUIStates()
        {
            checkGroupItems.Checked = stripList.ShowGroups;
            checkGroupItems.Enabled = stripList.Groups.Count > 0;

            buttonPreviousEffect.Enabled = stripList.SelectedIndices.Count == 1;
            buttonNextEffect.Enabled = stripList.SelectedIndices.Count == 1;

            buttonDeleteStrip.Enabled = stripList.SelectedIndices.Count > 0;
            buttonEditStrip.Enabled = stripList.SelectedIndices.Count == 1;

            buttonNewStrip.Enabled = true;

        }

        private void buttonPreviousEffect_Click(object sender, EventArgs e)
        {
            var strip = stripList.SelectedItems[0].Tag as LightStrip;
            strip.StripSite.PreviousEffect();
        }

        private void buttonNextEffect_Click(object sender, EventArgs e)
        {
            var strip = stripList.SelectedItems[0].Tag as LightStrip;
            strip.StripSite.NextEffect();

        }

        private void buttonDeleteStrip_Click(object sender, EventArgs e)
        {
            var strip = stripList.SelectedItems[0].Tag as LightStrip;
            _server.RemoveStrip(strip);
            FillListView();
        }
    }
}
