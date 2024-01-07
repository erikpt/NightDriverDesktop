namespace NightDriverDesktop
{
    partial class LightStripEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            textBoxFriendlyName = new TextBox();
            label2 = new Label();
            textBoxHostName = new TextBox();
            label3 = new Label();
            comboBoxStripType = new ComboBox();
            labelHeight = new Label();
            textBoxHeight = new TextBox();
            labelWIdth = new Label();
            textBoxWidth = new TextBox();
            label4 = new Label();
            buttonSave = new Button();
            buttonCancel = new Button();
            checkBoxCompression = new CheckBox();
            tableLayoutPanel2 = new TableLayoutPanel();
            comboBoxChannel = new ComboBox();
            label5 = new Label();
            checkBoxReverseOrder = new CheckBox();
            label7 = new Label();
            textBoxPixelOffset = new TextBox();
            label6 = new Label();
            comboBoxColorOrder = new ComboBox();
            checkBoxSkipNetTest = new CheckBox();
            tableLayoutPanel2.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Left;
            label1.AutoSize = true;
            label1.Location = new Point(3, 7);
            label1.Name = "label1";
            label1.Size = new Size(84, 15);
            label1.TabIndex = 0;
            label1.Text = "Friendly Name";
            // 
            // textBoxFriendlyName
            // 
            textBoxFriendlyName.CharacterCasing = CharacterCasing.Upper;
            textBoxFriendlyName.Location = new Point(148, 3);
            textBoxFriendlyName.Name = "textBoxFriendlyName";
            textBoxFriendlyName.Size = new Size(155, 23);
            textBoxFriendlyName.TabIndex = 1;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Left;
            label2.AutoSize = true;
            label2.Location = new Point(3, 36);
            label2.Name = "label2";
            label2.Size = new Size(139, 15);
            label2.TabIndex = 2;
            label2.Text = "Host Name or IP Address";
            // 
            // textBoxHostName
            // 
            textBoxHostName.Location = new Point(148, 32);
            textBoxHostName.Name = "textBoxHostName";
            textBoxHostName.Size = new Size(155, 23);
            textBoxHostName.TabIndex = 3;
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Left;
            label3.AutoSize = true;
            label3.Location = new Point(3, 65);
            label3.Name = "label3";
            label3.Size = new Size(58, 15);
            label3.TabIndex = 4;
            label3.Text = "Strip Type";
            // 
            // comboBoxStripType
            // 
            comboBoxStripType.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBoxStripType.AutoCompleteSource = AutoCompleteSource.ListItems;
            comboBoxStripType.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxStripType.FormattingEnabled = true;
            comboBoxStripType.Items.AddRange(new object[] { "Single Strip", "LED Matrix" });
            comboBoxStripType.Location = new Point(148, 61);
            comboBoxStripType.Name = "comboBoxStripType";
            comboBoxStripType.Size = new Size(155, 23);
            comboBoxStripType.TabIndex = 5;
            comboBoxStripType.SelectedIndexChanged += comboBoxStripType_SelectedIndexChanged;
            // 
            // labelHeight
            // 
            labelHeight.Anchor = AnchorStyles.Left;
            labelHeight.AutoSize = true;
            labelHeight.Location = new Point(3, 123);
            labelHeight.Name = "labelHeight";
            labelHeight.Size = new Size(43, 15);
            labelHeight.TabIndex = 6;
            labelHeight.Text = "Height";
            // 
            // textBoxHeight
            // 
            textBoxHeight.Location = new Point(148, 119);
            textBoxHeight.Name = "textBoxHeight";
            textBoxHeight.Size = new Size(49, 23);
            textBoxHeight.TabIndex = 7;
            textBoxHeight.Text = "1";
            textBoxHeight.TextChanged += textBoxHeight_TextChanged;
            // 
            // labelWIdth
            // 
            labelWIdth.Anchor = AnchorStyles.Left;
            labelWIdth.AutoSize = true;
            labelWIdth.Location = new Point(3, 152);
            labelWIdth.Name = "labelWIdth";
            labelWIdth.Size = new Size(39, 15);
            labelWIdth.TabIndex = 8;
            labelWIdth.Text = "Width";
            // 
            // textBoxWidth
            // 
            textBoxWidth.Location = new Point(148, 148);
            textBoxWidth.Name = "textBoxWidth";
            textBoxWidth.Size = new Size(49, 23);
            textBoxWidth.TabIndex = 9;
            textBoxWidth.TextChanged += textBoxWidth_TextChanged;
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.Top;
            label4.AutoSize = true;
            tableLayoutPanel2.SetColumnSpan(label4, 2);
            label4.Location = new Point(10, 87);
            label4.Name = "label4";
            label4.Padding = new Padding(0, 10, 0, 3);
            label4.Size = new Size(286, 28);
            label4.TabIndex = 10;
            label4.Text = "For LED Strips, Width is equal to the number of pixels";
            // 
            // buttonSave
            // 
            buttonSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonSave.Location = new Point(539, 204);
            buttonSave.Margin = new Padding(3, 30, 3, 3);
            buttonSave.Name = "buttonSave";
            buttonSave.Size = new Size(75, 23);
            buttonSave.TabIndex = 13;
            buttonSave.TabStop = false;
            buttonSave.Text = "&Save";
            buttonSave.UseVisualStyleBackColor = true;
            buttonSave.Click += buttonSave_Click;
            // 
            // buttonCancel
            // 
            buttonCancel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonCancel.DialogResult = DialogResult.Cancel;
            buttonCancel.Location = new Point(457, 204);
            buttonCancel.Margin = new Padding(3, 30, 3, 3);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(75, 23);
            buttonCancel.TabIndex = 14;
            buttonCancel.Text = "&Cancel";
            buttonCancel.UseVisualStyleBackColor = true;
            buttonCancel.Click += buttonCancel_Click;
            // 
            // checkBoxCompression
            // 
            checkBoxCompression.Anchor = AnchorStyles.Left;
            checkBoxCompression.AutoSize = true;
            checkBoxCompression.Location = new Point(384, 63);
            checkBoxCompression.Name = "checkBoxCompression";
            checkBoxCompression.Size = new Size(111, 19);
            checkBoxCompression.TabIndex = 15;
            checkBoxCompression.Text = "Compress Data?";
            checkBoxCompression.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.AutoSize = true;
            tableLayoutPanel2.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tableLayoutPanel2.ColumnCount = 5;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25.7234688F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 24.7588444F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel2.Controls.Add(textBoxWidth, 1, 5);
            tableLayoutPanel2.Controls.Add(labelWIdth, 0, 5);
            tableLayoutPanel2.Controls.Add(textBoxHeight, 1, 4);
            tableLayoutPanel2.Controls.Add(labelHeight, 0, 4);
            tableLayoutPanel2.Controls.Add(label3, 0, 2);
            tableLayoutPanel2.Controls.Add(textBoxHostName, 1, 1);
            tableLayoutPanel2.Controls.Add(label2, 0, 1);
            tableLayoutPanel2.Controls.Add(textBoxFriendlyName, 1, 0);
            tableLayoutPanel2.Controls.Add(label1, 0, 0);
            tableLayoutPanel2.Controls.Add(comboBoxChannel, 3, 0);
            tableLayoutPanel2.Controls.Add(label5, 2, 0);
            tableLayoutPanel2.Controls.Add(comboBoxStripType, 1, 2);
            tableLayoutPanel2.Controls.Add(label4, 0, 3);
            tableLayoutPanel2.Controls.Add(buttonCancel, 3, 6);
            tableLayoutPanel2.Controls.Add(buttonSave, 4, 6);
            tableLayoutPanel2.Controls.Add(checkBoxReverseOrder, 3, 5);
            tableLayoutPanel2.Controls.Add(label7, 2, 4);
            tableLayoutPanel2.Controls.Add(textBoxPixelOffset, 3, 4);
            tableLayoutPanel2.Controls.Add(label6, 2, 3);
            tableLayoutPanel2.Controls.Add(comboBoxColorOrder, 3, 3);
            tableLayoutPanel2.Controls.Add(checkBoxCompression, 3, 2);
            tableLayoutPanel2.Controls.Add(checkBoxSkipNetTest, 3, 1);
            tableLayoutPanel2.Location = new Point(12, 12);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 7;
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.Size = new Size(617, 230);
            tableLayoutPanel2.TabIndex = 1;
            // 
            // comboBoxChannel
            // 
            comboBoxChannel.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBoxChannel.AutoCompleteSource = AutoCompleteSource.ListItems;
            comboBoxChannel.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxChannel.FormattingEnabled = true;
            comboBoxChannel.Items.AddRange(new object[] { "0", "1", "2", "3", "4", "5", "6", "7" });
            comboBoxChannel.Location = new Point(384, 3);
            comboBoxChannel.Name = "comboBoxChannel";
            comboBoxChannel.Size = new Size(70, 23);
            comboBoxChannel.TabIndex = 7;
            comboBoxChannel.SelectedIndexChanged += comboBoxChannel_SelectedIndexChanged;
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.Left;
            label5.AutoSize = true;
            label5.Location = new Point(309, 7);
            label5.Name = "label5";
            label5.Size = new Size(51, 15);
            label5.TabIndex = 6;
            label5.Text = "Channel";
            // 
            // checkBoxReverseOrder
            // 
            checkBoxReverseOrder.AutoSize = true;
            checkBoxReverseOrder.Location = new Point(384, 148);
            checkBoxReverseOrder.Name = "checkBoxReverseOrder";
            checkBoxReverseOrder.Size = new Size(132, 19);
            checkBoxReverseOrder.TabIndex = 18;
            checkBoxReverseOrder.Text = "Reverse Pixel Order?";
            checkBoxReverseOrder.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            label7.Anchor = AnchorStyles.Left;
            label7.AutoSize = true;
            label7.Location = new Point(309, 123);
            label7.Name = "label7";
            label7.Size = new Size(67, 15);
            label7.TabIndex = 19;
            label7.Text = "Pixel Offset";
            // 
            // textBoxPixelOffset
            // 
            textBoxPixelOffset.Location = new Point(384, 119);
            textBoxPixelOffset.Name = "textBoxPixelOffset";
            textBoxPixelOffset.Size = new Size(70, 23);
            textBoxPixelOffset.TabIndex = 20;
            textBoxPixelOffset.Text = "0";
            textBoxPixelOffset.TextChanged += textBoxPixelOffset_TextChanged;
            // 
            // label6
            // 
            label6.Anchor = AnchorStyles.Left;
            label6.AutoSize = true;
            label6.Location = new Point(309, 94);
            label6.Name = "label6";
            label6.Size = new Size(69, 15);
            label6.TabIndex = 16;
            label6.Text = "Color Order";
            // 
            // comboBoxColorOrder
            // 
            comboBoxColorOrder.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBoxColorOrder.AutoCompleteSource = AutoCompleteSource.ListItems;
            comboBoxColorOrder.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxColorOrder.FormattingEnabled = true;
            comboBoxColorOrder.Items.AddRange(new object[] { "GRB", "RGB" });
            comboBoxColorOrder.Location = new Point(384, 90);
            comboBoxColorOrder.Name = "comboBoxColorOrder";
            comboBoxColorOrder.Size = new Size(121, 23);
            comboBoxColorOrder.TabIndex = 17;
            comboBoxColorOrder.SelectedIndexChanged += comboBoxColorOrder_SelectedIndexChanged;
            // 
            // checkBoxSkipNetTest
            // 
            checkBoxSkipNetTest.Anchor = AnchorStyles.Left;
            checkBoxSkipNetTest.AutoSize = true;
            tableLayoutPanel2.SetColumnSpan(checkBoxSkipNetTest, 2);
            checkBoxSkipNetTest.Location = new Point(384, 34);
            checkBoxSkipNetTest.Name = "checkBoxSkipNetTest";
            checkBoxSkipNetTest.Size = new Size(189, 19);
            checkBoxSkipNetTest.TabIndex = 21;
            checkBoxSkipNetTest.Text = "Skip Network Connectivity Test";
            checkBoxSkipNetTest.UseVisualStyleBackColor = true;
            // 
            // LightStripEditor
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = buttonCancel;
            ClientSize = new Size(642, 251);
            Controls.Add(tableLayoutPanel2);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "LightStripEditor";
            ShowIcon = false;
            ShowInTaskbar = false;
            Text = "Add New Light Strip";
            Load += this.LightStripEditor_Load;
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label1;
        private TextBox textBoxFriendlyName;
        private Label label2;
        private TextBox textBoxHostName;
        private Label label3;
        private ComboBox comboBoxStripType;
        private Label labelHeight;
        private TextBox textBoxHeight;
        private Label labelWIdth;
        private TextBox textBoxWidth;
        private Label label4;
        private Button buttonSave;
        private Button buttonCancel;
        private CheckBox checkBoxCompression;
        private TableLayoutPanel tableLayoutPanel2;
        private Label label5;
        private ComboBox comboBoxChannel;
        private Label label6;
        private ComboBox comboBoxColorOrder;
        private CheckBox checkBoxReverseOrder;
        private Label label7;
        private TextBox textBoxPixelOffset;
        private CheckBox checkBoxSkipNetTest;
    }
}