namespace NightDriver
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            StartButton = new Button();
            StopButton = new Button();
            statusStrip1 = new StatusStrip();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            timerListView = new System.Windows.Forms.Timer(components);
            stripList = new StripListView();
            colName = new ColumnHeader();
            colHost = new ColumnHeader();
            colSocket = new ColumnHeader();
            colWiFi = new ColumnHeader();
            colStatus = new ColumnHeader();
            colBPS = new ColumnHeader();
            colClock = new ColumnHeader();
            colBuffer = new ColumnHeader();
            colPower = new ColumnHeader();
            colFPS = new ColumnHeader();
            colOffset = new ColumnHeader();
            colConnects = new ColumnHeader();
            colQueue = new ColumnHeader();
            colEffect = new ColumnHeader();
            tabControl = new TabControl();
            tabMain = new TabPage();
            splitContainer1 = new SplitContainer();
            panelVisualizer = new LEDVisualizer();
            tabLogging = new TabPage();
            textLog = new TextBox();
            timer1 = new System.Windows.Forms.Timer(components);
            timerVisualizer = new System.Windows.Forms.Timer(components);
            statusStrip1.SuspendLayout();
            tabControl.SuspendLayout();
            tabMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            tabLogging.SuspendLayout();
            SuspendLayout();
            // 
            // StartButton
            // 
            StartButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            StartButton.Location = new Point(1115, 676);
            StartButton.Name = "StartButton";
            StartButton.Size = new Size(75, 23);
            StartButton.TabIndex = 0;
            StartButton.Text = "Start";
            StartButton.UseVisualStyleBackColor = true;
            StartButton.Click += StartButton_Click;
            // 
            // StopButton
            // 
            StopButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            StopButton.Enabled = false;
            StopButton.Location = new Point(1034, 676);
            StopButton.Name = "StopButton";
            StopButton.Size = new Size(75, 23);
            StopButton.TabIndex = 0;
            StopButton.Text = "Stop";
            StopButton.UseVisualStyleBackColor = true;
            StopButton.Click += StopButton_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1 });
            statusStrip1.Location = new Point(0, 739);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(1206, 22);
            statusStrip1.TabIndex = 1;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.BackColor = SystemColors.ButtonFace;
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(118, 17);
            toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // timerListView
            // 
            timerListView.Tick += timer1_Tick;
            // 
            // stripList
            // 
            stripList.BackColor = Color.FromArgb(26, 26, 26);
            stripList.Columns.AddRange(new ColumnHeader[] { colName, colHost, colSocket, colWiFi, colStatus, colBPS, colClock, colBuffer, colPower, colFPS, colOffset, colConnects, colQueue, colEffect });
            stripList.Dock = DockStyle.Fill;
            stripList.ForeColor = Color.White;
            stripList.Location = new Point(0, 0);
            stripList.Name = "stripList";
            stripList.Size = new Size(1192, 331);
            stripList.Sorting = SortOrder.Descending;
            stripList.TabIndex = 2;
            stripList.UseCompatibleStateImageBehavior = false;
            stripList.View = View.Details;
            stripList.SelectedIndexChanged += stripList_SelectedIndexChanged;
            // 
            // colName
            // 
            colName.Text = "Name";
            colName.Width = 100;
            // 
            // colHost
            // 
            colHost.Text = "Host";
            colHost.Width = 100;
            // 
            // colSocket
            // 
            colSocket.Text = "Socket";
            colSocket.Width = 50;
            // 
            // colWiFi
            // 
            colWiFi.Text = "WiFi";
            colWiFi.Width = 40;
            // 
            // colStatus
            // 
            colStatus.Text = "Status";
            colStatus.Width = 50;
            // 
            // colBPS
            // 
            colBPS.Text = "Bytes/Sec";
            colBPS.Width = 80;
            // 
            // colClock
            // 
            colClock.Text = "Clock";
            colClock.Width = 50;
            // 
            // colBuffer
            // 
            colBuffer.Text = "Buffer";
            colBuffer.Width = 50;
            // 
            // colPower
            // 
            colPower.Text = "Power";
            colPower.Width = 50;
            // 
            // colFPS
            // 
            colFPS.Text = "FPS";
            colFPS.Width = 40;
            // 
            // colOffset
            // 
            colOffset.DisplayIndex = 12;
            colOffset.Text = "Offset";
            colOffset.Width = 50;
            // 
            // colConnects
            // 
            colConnects.DisplayIndex = 10;
            colConnects.Text = "Connects";
            colConnects.Width = 70;
            // 
            // colQueue
            // 
            colQueue.DisplayIndex = 11;
            colQueue.Text = "Queue";
            colQueue.Width = 50;
            // 
            // colEffect
            // 
            colEffect.Text = "Effect";
            colEffect.Width = 190;
            // 
            // tabControl
            // 
            tabControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabControl.Controls.Add(tabMain);
            tabControl.Controls.Add(tabLogging);
            tabControl.Location = new Point(0, 3);
            tabControl.Margin = new Padding(0);
            tabControl.Name = "tabControl";
            tabControl.Padding = new Point(0, 0);
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(1206, 733);
            tabControl.TabIndex = 3;
            // 
            // tabMain
            // 
            tabMain.BackColor = Color.Transparent;
            tabMain.Controls.Add(splitContainer1);
            tabMain.Controls.Add(StopButton);
            tabMain.Controls.Add(StartButton);
            tabMain.ForeColor = Color.Black;
            tabMain.Location = new Point(4, 24);
            tabMain.Name = "tabMain";
            tabMain.Padding = new Padding(3);
            tabMain.Size = new Size(1198, 705);
            tabMain.TabIndex = 0;
            tabMain.Text = "WiFi Control";
            // 
            // splitContainer1
            // 
            splitContainer1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            splitContainer1.Location = new Point(3, 6);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(stripList);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(panelVisualizer);
            splitContainer1.Size = new Size(1192, 664);
            splitContainer1.SplitterDistance = 331;
            splitContainer1.TabIndex = 4;
            // 
            // panelVisualizer
            // 
            panelVisualizer.ColorData = null;
            panelVisualizer.Dock = DockStyle.Fill;
            panelVisualizer.Location = new Point(0, 0);
            panelVisualizer.Name = "panelVisualizer";
            panelVisualizer.Size = new Size(1192, 329);
            panelVisualizer.TabIndex = 3;
            // 
            // tabLogging
            // 
            tabLogging.Controls.Add(textLog);
            tabLogging.Location = new Point(4, 24);
            tabLogging.Name = "tabLogging";
            tabLogging.Padding = new Padding(3);
            tabLogging.Size = new Size(991, 462);
            tabLogging.TabIndex = 1;
            tabLogging.Text = "Logging";
            tabLogging.UseVisualStyleBackColor = true;
            // 
            // textLog
            // 
            textLog.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            textLog.BackColor = Color.Black;
            textLog.ForeColor = Color.Lime;
            textLog.Location = new Point(0, 0);
            textLog.Multiline = true;
            textLog.Name = "textLog";
            textLog.ScrollBars = ScrollBars.Vertical;
            textLog.Size = new Size(991, 466);
            textLog.TabIndex = 0;
            // 
            // timerVisualizer
            // 
            timerVisualizer.Tick += timerVisualizer_Tick;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveBorder;
            ClientSize = new Size(1206, 761);
            Controls.Add(tabControl);
            Controls.Add(statusStrip1);
            Name = "MainForm";
            Text = "Form1";
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            tabControl.ResumeLayout(false);
            tabMain.ResumeLayout(false);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            tabLogging.ResumeLayout(false);
            tabLogging.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button StartButton;
        private Button StopButton;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Timer timerListView;
        private StripListView stripList;
        private ColumnHeader colName;
        private ColumnHeader colHost;
        private ColumnHeader colSocket;
        private ColumnHeader colWiFi;
        private ColumnHeader colStatus;
        private ColumnHeader colBPS;
        private ColumnHeader colClock;
        private ColumnHeader colBuffer;
        private ColumnHeader colPower;
        private ColumnHeader colFPS;
        private ColumnHeader colConnects;
        private ColumnHeader colQueue;
        private ColumnHeader colOffset;
        private ColumnHeader colEffect;
        private TabControl tabControl;
        private TabPage tabMain;
        private TabPage tabLogging;
        private TextBox textLog;
        private LEDVisualizer panelVisualizer;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer timerVisualizer;
        private SplitContainer splitContainer1;
    }
}
