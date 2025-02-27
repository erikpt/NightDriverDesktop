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


using NightDriverDesktop;
using System.Diagnostics;
using System.Runtime.CompilerServices;
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

        private SaveFileDialog saveDialog = new SaveFileDialog();
        private OpenFileDialog openDialog = new OpenFileDialog();
        private string activeFileName = string.Empty;

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
            FillListView();
            timerListView.Start();
            timerVisualizer.Start();

            //Setup the SaveFileDialog
            saveDialog.InitialDirectory = Application.StartupPath;
            saveDialog.RestoreDirectory = true;
            saveDialog.FileName = string.Empty;
            saveDialog.Filter = "JSON LED Strip Data File (*.json)|*.json";
            saveDialog.CheckWriteAccess = true;
            saveDialog.CheckFileExists = false;
            saveDialog.CheckPathExists = true;
            saveDialog.AutoUpgradeEnabled = true;
            saveDialog.DefaultExt = ".json";
            saveDialog.Title = Application.ProductName + " - Save LED Strip List";

            //Setup the OpenFileDialog
            openDialog.InitialDirectory = Application.StartupPath;
            openDialog.RestoreDirectory = true;
            openDialog.FileName = "AllStrips.json";
            openDialog.Filter = "JSON LED Strip Data File (*.json)|*.json";
            openDialog.CheckFileExists = true;
            openDialog.CheckPathExists = true;
            openDialog.AutoUpgradeEnabled = true;
            openDialog.DefaultExt = ".json";
            openDialog.Title = Application.ProductName + " - Open LED Strip List";

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
            stripList.Items.Clear();

            foreach (var strip in _server.AllStrips)
            {
                var name = strip.StripSite.Name;
                ListViewGroup group = stripList.Groups[name];
                if (group == null)
                    group = stripList.Groups.Add(name, name);
                stripList.Items.Add(new StripListItem(group, strip.FriendlyName, strip));
            }
            if (stripList.Items.Count > 0)
                stripList.Items[0].Selected = true;
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

            buttonDeleteStrip.Enabled = !_server.IsRunning && stripList.SelectedIndices.Count > 0;
            buttonEditStrip.Enabled = !_server.IsRunning && stripList.SelectedIndices.Count == 1;
            buttonNewStrip.Enabled = !_server.IsRunning;
        }

        private void AddNewStrip()
        {
            LightStripEditor newStrip = new();
            newStrip.DialogMode = LightStripEditor.DialogOperatingMode.New;

            DialogResult result = newStrip.ShowDialog();

            if (!(result == DialogResult.OK))
            {
                return;
            }
            else if (newStrip.Tag is LightStrip)
            {
                if (newStrip.Tag == null)
                {
                    MessageBox.Show(
                        "Error while adding new LED strip.",
                        "Invalid LED Strip Data",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                        );
                }
                LightStrip strip = (LightStrip)newStrip.Tag;
                _server.AllStrips.Add(strip);
                FillListView();
            }
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
            var selectedStripName = stripList.SelectedItems[0].Text;
            var strip = stripList.SelectedItems[0].Tag as LightStrip;
            DialogResult result = MessageBox.Show(String.Format("Are you sure you want to delete the {0} strip", selectedStripName), Application.ProductName + " - Are you sure?", MessageBoxButtons.YesNoCancel);
            if (result == DialogResult.Yes)
            {
                _server.RemoveStrip(strip);
                FillListView();
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Default this to the standard file name
            var fileName = "AllStrips.json";

            // we know this should be a string, treat it as one
            if (activeFileName.Trim() == string.Empty)
            {
                DialogResult saveResult = saveDialog.ShowDialog();

                if (saveResult == DialogResult.OK && saveDialog.FileName.ToLower().EndsWith(".json"))
                {
                    fileName = saveDialog.FileName;
                }
            }
            else if (activeFileName.Trim().ToLower().EndsWith(".json"))
            {
                fileName = activeFileName.Trim();
            }
            _server.SaveStripsAs(fileName);
            //_server.SaveStrips();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

            DialogResult openResult = openDialog.ShowDialog();

            if (openResult == DialogResult.OK && openDialog.FileName.ToLower().Trim().EndsWith(".json"))
            {
                activeFileName = openDialog.FileName;
                _server.LoadStripsFromFile(openDialog.FileName);
                FillListView();
                panelVisualizer.ColorData = null;
                panelVisualizer.Invalidate();
            }
            else
            {
                MessageBox.Show("Could not open the selected file.", Application.ProductName + " - Open File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void loadDemoFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _server.LoadStripsFromTable();
            FillListView();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Save before closing?", Application.ProductName + " - Save LED Strip List", MessageBoxButtons.YesNoCancel);
            if (result == DialogResult.Yes)
            {
                //Default this to the standard file name
                var fileName = "AllStrips.json";

                // we know this should be a string, treat it as one
                if (activeFileName.Trim().EndsWith(".json"))
                {
                    fileName = activeFileName.Trim();
                }
                _server.SaveStripsAs(fileName);
            }
            _server.FreshStart();
            stripList.Clear();
            FillListView();
            panelVisualizer.ColorData = null;
            panelVisualizer.Invalidate();
            activeFileName = string.Empty;
            openDialog.FileName = string.Empty;
            saveDialog.FileName = string.Empty;
        }

        private void buttonNewStrip_Click(object sender, EventArgs e)
        {
            AddNewStrip();
        }

        private void newEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNewStrip();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult saveResult = saveDialog.ShowDialog();

            if (saveResult == DialogResult.OK && saveDialog.FileName.ToLower().EndsWith(".json"))
            {
                _server.SaveStripsAs(saveDialog.FileName);
            }
        }

        private void buttonEditStrip_Click(object sender, EventArgs e)
        {
            var selectedStripName = stripList.SelectedItems[0].Text;
            var strip = stripList.SelectedItems[0].Tag as LightStrip;

            LightStripEditor editStrip = new();
            editStrip.DialogMode = LightStripEditor.DialogOperatingMode.Edit;
            editStrip.StripToEdit = strip;

            DialogResult result = editStrip.ShowDialog();

            if (!(result == DialogResult.OK))
            {
                return;
            }
            else if (editStrip.Tag is LightStrip)
            {
                if (editStrip.Tag == null)
                {
                    MessageBox.Show(
                        "Error while adding new LED strip.",
                        "Invalid LED Strip Data",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                        );
                }
                LightStrip editedStrip = (LightStrip)editStrip.Tag;
                _server.RemoveStrip(strip);
                _server.AllStrips.Add(editedStrip);
                FillListView();
            }
        }
    }
}
