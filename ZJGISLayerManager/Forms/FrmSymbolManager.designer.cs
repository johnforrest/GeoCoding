namespace ZJGISLayerManager
{
    partial class FrmSymbolManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSymbolManager));
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.SymbolCtrl1 = new ESRI.ArcGIS.Controls.AxSymbologyControl();
            this.cboSymbolType = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.lblSymbolType = new DevComponents.DotNetBar.LabelX();
            this.cboSymbolFile = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.lblSymbolFile = new DevComponents.DotNetBar.LabelX();
            this.upLnWidth = new System.Windows.Forms.NumericUpDown();
            this.chkNullColor = new System.Windows.Forms.CheckBox();
            this.lblLnWidth = new DevComponents.DotNetBar.LabelX();
            this.cpbLnColor = new DevComponents.DotNetBar.ColorPickerButton();
            this.lblLnColor = new DevComponents.DotNetBar.LabelX();
            this.gbPoint = new System.Windows.Forms.GroupBox();
            this.upPointAngle = new System.Windows.Forms.NumericUpDown();
            this.upPointSize = new System.Windows.Forms.NumericUpDown();
            this.cpbPtColor = new DevComponents.DotNetBar.ColorPickerButton();
            this.lblPtAngle = new DevComponents.DotNetBar.LabelX();
            this.lblPtSize = new DevComponents.DotNetBar.LabelX();
            this.lblPtColor = new DevComponents.DotNetBar.LabelX();
            this.btnOK = new DevComponents.DotNetBar.ButtonX();
            this.gbLine = new System.Windows.Forms.GroupBox();
            this.pbPreview = new System.Windows.Forms.PictureBox();
            this.gbPreview = new System.Windows.Forms.GroupBox();
            this.gbArea = new System.Windows.Forms.GroupBox();
            this.chkNoColor = new System.Windows.Forms.CheckBox();
            this.upAreaBoardWidth = new System.Windows.Forms.NumericUpDown();
            this.cpbAreaBoardWidth = new DevComponents.DotNetBar.ColorPickerButton();
            this.lblAreaBoardWidth = new DevComponents.DotNetBar.LabelX();
            this.lblAreaBoardColor = new DevComponents.DotNetBar.LabelX();
            this.cpbAreaColor = new DevComponents.DotNetBar.ColorPickerButton();
            this.lblAreaColor = new DevComponents.DotNetBar.LabelX();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.GroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SymbolCtrl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.upLnWidth)).BeginInit();
            this.gbPoint.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.upPointAngle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.upPointSize)).BeginInit();
            this.gbLine.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbPreview)).BeginInit();
            this.gbPreview.SuspendLayout();
            this.gbArea.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.upAreaBoardWidth)).BeginInit();
            this.SuspendLayout();
            // 
            // GroupBox1
            // 
            this.GroupBox1.Controls.Add(this.SymbolCtrl1);
            this.GroupBox1.Controls.Add(this.cboSymbolType);
            this.GroupBox1.Controls.Add(this.lblSymbolType);
            this.GroupBox1.Controls.Add(this.cboSymbolFile);
            this.GroupBox1.Controls.Add(this.lblSymbolFile);
            this.GroupBox1.Location = new System.Drawing.Point(1, 7);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Size = new System.Drawing.Size(345, 246);
            this.GroupBox1.TabIndex = 9;
            this.GroupBox1.TabStop = false;
            // 
            // SymbolCtrl1
            // 
            this.SymbolCtrl1.Location = new System.Drawing.Point(12, 79);
            this.SymbolCtrl1.Name = "SymbolCtrl1";
            this.SymbolCtrl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("SymbolCtrl1.OcxState")));
            this.SymbolCtrl1.Size = new System.Drawing.Size(265, 265);
            this.SymbolCtrl1.TabIndex = 4;
            this.SymbolCtrl1.OnItemSelected += new ESRI.ArcGIS.Controls.ISymbologyControlEvents_Ax_OnItemSelectedEventHandler(this.SymbolCtrl1_OnItemSelected);
            // 
            // cboSymbolType
            // 
            this.cboSymbolType.DisplayMember = "Text";
            this.cboSymbolType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboSymbolType.FormattingEnabled = true;
            this.cboSymbolType.Location = new System.Drawing.Point(73, 55);
            this.cboSymbolType.Name = "cboSymbolType";
            this.cboSymbolType.Size = new System.Drawing.Size(266, 22);
            this.cboSymbolType.TabIndex = 3;
            // 
            // lblSymbolType
            // 
            this.lblSymbolType.Location = new System.Drawing.Point(6, 58);
            this.lblSymbolType.Name = "lblSymbolType";
            this.lblSymbolType.Size = new System.Drawing.Size(60, 19);
            this.lblSymbolType.TabIndex = 2;
            this.lblSymbolType.Text = "符号类型:";
            // 
            // cboSymbolFile
            // 
            this.cboSymbolFile.DisplayMember = "Text";
            this.cboSymbolFile.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboSymbolFile.FormattingEnabled = true;
            this.cboSymbolFile.Location = new System.Drawing.Point(75, 17);
            this.cboSymbolFile.Name = "cboSymbolFile";
            this.cboSymbolFile.Size = new System.Drawing.Size(264, 22);
            this.cboSymbolFile.TabIndex = 1;
            this.cboSymbolFile.SelectedIndexChanged += new System.EventHandler(this.cboSymbolFile_SelectedIndexChanged);
            // 
            // lblSymbolFile
            // 
            this.lblSymbolFile.Location = new System.Drawing.Point(6, 17);
            this.lblSymbolFile.Name = "lblSymbolFile";
            this.lblSymbolFile.Size = new System.Drawing.Size(63, 22);
            this.lblSymbolFile.TabIndex = 0;
            this.lblSymbolFile.Text = "符号文件:";
            // 
            // upLnWidth
            // 
            this.upLnWidth.DecimalPlaces = 1;
            this.upLnWidth.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.upLnWidth.Location = new System.Drawing.Point(63, 65);
            this.upLnWidth.Name = "upLnWidth";
            this.upLnWidth.Size = new System.Drawing.Size(84, 21);
            this.upLnWidth.TabIndex = 3;
            this.upLnWidth.ValueChanged += new System.EventHandler(this.upLnWidth_ValueChanged);
            // 
            // chkNullColor
            // 
            this.chkNullColor.AutoSize = true;
            this.chkNullColor.Location = new System.Drawing.Point(8, 52);
            this.chkNullColor.Name = "chkNullColor";
            this.chkNullColor.Size = new System.Drawing.Size(60, 16);
            this.chkNullColor.TabIndex = 9;
            this.chkNullColor.Text = "无颜色";
            this.chkNullColor.UseVisualStyleBackColor = true;
            // 
            // lblLnWidth
            // 
            this.lblLnWidth.Location = new System.Drawing.Point(14, 66);
            this.lblLnWidth.Name = "lblLnWidth";
            this.lblLnWidth.Size = new System.Drawing.Size(50, 20);
            this.lblLnWidth.TabIndex = 2;
            this.lblLnWidth.Text = "线宽:";
            // 
            // cpbLnColor
            // 
            this.cpbLnColor.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.cpbLnColor.Image = ((System.Drawing.Image)(resources.GetObject("cpbLnColor.Image")));
            this.cpbLnColor.Location = new System.Drawing.Point(61, 27);
            this.cpbLnColor.Name = "cpbLnColor";
            this.cpbLnColor.SelectedColorImageRectangle = new System.Drawing.Rectangle(2, 2, 12, 12);
            this.cpbLnColor.Size = new System.Drawing.Size(83, 23);
            this.cpbLnColor.TabIndex = 1;
            this.cpbLnColor.SelectedColorChanged += new System.EventHandler(this.cpbLnColor_SelectedColorChanged);
            // 
            // lblLnColor
            // 
            this.lblLnColor.Location = new System.Drawing.Point(14, 31);
            this.lblLnColor.Name = "lblLnColor";
            this.lblLnColor.Size = new System.Drawing.Size(52, 21);
            this.lblLnColor.TabIndex = 0;
            this.lblLnColor.Text = "颜色:";
            // 
            // gbPoint
            // 
            this.gbPoint.Controls.Add(this.chkNullColor);
            this.gbPoint.Controls.Add(this.upPointAngle);
            this.gbPoint.Controls.Add(this.upPointSize);
            this.gbPoint.Controls.Add(this.cpbPtColor);
            this.gbPoint.Controls.Add(this.lblPtAngle);
            this.gbPoint.Controls.Add(this.lblPtSize);
            this.gbPoint.Controls.Add(this.lblPtColor);
            this.gbPoint.Location = new System.Drawing.Point(391, 220);
            this.gbPoint.Name = "gbPoint";
            this.gbPoint.Size = new System.Drawing.Size(171, 175);
            this.gbPoint.TabIndex = 13;
            this.gbPoint.TabStop = false;
            // 
            // upPointAngle
            // 
            this.upPointAngle.Location = new System.Drawing.Point(62, 109);
            this.upPointAngle.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.upPointAngle.Name = "upPointAngle";
            this.upPointAngle.Size = new System.Drawing.Size(85, 21);
            this.upPointAngle.TabIndex = 7;
            this.upPointAngle.ValueChanged += new System.EventHandler(this.upPointAngle_ValueChanged);
            // 
            // upPointSize
            // 
            this.upPointSize.DecimalPlaces = 1;
            this.upPointSize.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.upPointSize.Location = new System.Drawing.Point(64, 77);
            this.upPointSize.Name = "upPointSize";
            this.upPointSize.Size = new System.Drawing.Size(83, 21);
            this.upPointSize.TabIndex = 6;
            this.upPointSize.ValueChanged += new System.EventHandler(this.upPointSize_ValueChanged);
            // 
            // cpbPtColor
            // 
            this.cpbPtColor.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.cpbPtColor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(217)))), ((int)(((byte)(247)))));
            this.cpbPtColor.Image = ((System.Drawing.Image)(resources.GetObject("cpbPtColor.Image")));
            this.cpbPtColor.Location = new System.Drawing.Point(64, 24);
            this.cpbPtColor.Name = "cpbPtColor";
            this.cpbPtColor.SelectedColorImageRectangle = new System.Drawing.Rectangle(2, 2, 12, 12);
            this.cpbPtColor.Size = new System.Drawing.Size(83, 23);
            this.cpbPtColor.TabIndex = 3;
            this.cpbPtColor.SelectedColorChanged += new System.EventHandler(this.cpbPtColor_SelectedColorChanged);
            // 
            // lblPtAngle
            // 
            this.lblPtAngle.Location = new System.Drawing.Point(8, 106);
            this.lblPtAngle.Name = "lblPtAngle";
            this.lblPtAngle.Size = new System.Drawing.Size(57, 22);
            this.lblPtAngle.TabIndex = 2;
            this.lblPtAngle.Text = "角度:";
            // 
            // lblPtSize
            // 
            this.lblPtSize.Location = new System.Drawing.Point(8, 75);
            this.lblPtSize.Name = "lblPtSize";
            this.lblPtSize.Size = new System.Drawing.Size(53, 25);
            this.lblPtSize.TabIndex = 1;
            this.lblPtSize.Text = "大小:";
            // 
            // lblPtColor
            // 
            this.lblPtColor.Location = new System.Drawing.Point(8, 27);
            this.lblPtColor.Name = "lblPtColor";
            this.lblPtColor.Size = new System.Drawing.Size(56, 20);
            this.lblPtColor.TabIndex = 0;
            this.lblPtColor.Text = "颜色:";
            // 
            // btnOK
            // 
            this.btnOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOK.Location = new System.Drawing.Point(164, 434);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(77, 29);
            this.btnOK.TabIndex = 14;
            this.btnOK.Text = "确定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // gbLine
            // 
            this.gbLine.Controls.Add(this.upLnWidth);
            this.gbLine.Controls.Add(this.lblLnWidth);
            this.gbLine.Controls.Add(this.cpbLnColor);
            this.gbLine.Controls.Add(this.lblLnColor);
            this.gbLine.Location = new System.Drawing.Point(391, 24);
            this.gbLine.Name = "gbLine";
            this.gbLine.Size = new System.Drawing.Size(171, 175);
            this.gbLine.TabIndex = 12;
            this.gbLine.TabStop = false;
            // 
            // pbPreview
            // 
            this.pbPreview.BackColor = System.Drawing.Color.White;
            this.pbPreview.Location = new System.Drawing.Point(6, 13);
            this.pbPreview.Name = "pbPreview";
            this.pbPreview.Size = new System.Drawing.Size(156, 156);
            this.pbPreview.TabIndex = 0;
            this.pbPreview.TabStop = false;
            // 
            // gbPreview
            // 
            this.gbPreview.Controls.Add(this.pbPreview);
            this.gbPreview.Location = new System.Drawing.Point(178, 253);
            this.gbPreview.Name = "gbPreview";
            this.gbPreview.Size = new System.Drawing.Size(168, 175);
            this.gbPreview.TabIndex = 11;
            this.gbPreview.TabStop = false;
            // 
            // gbArea
            // 
            this.gbArea.Controls.Add(this.chkNoColor);
            this.gbArea.Controls.Add(this.upAreaBoardWidth);
            this.gbArea.Controls.Add(this.cpbAreaBoardWidth);
            this.gbArea.Controls.Add(this.lblAreaBoardWidth);
            this.gbArea.Controls.Add(this.lblAreaBoardColor);
            this.gbArea.Controls.Add(this.cpbAreaColor);
            this.gbArea.Controls.Add(this.lblAreaColor);
            this.gbArea.Location = new System.Drawing.Point(1, 253);
            this.gbArea.Name = "gbArea";
            this.gbArea.Size = new System.Drawing.Size(171, 175);
            this.gbArea.TabIndex = 10;
            this.gbArea.TabStop = false;
            // 
            // chkNoColor
            // 
            this.chkNoColor.AutoSize = true;
            this.chkNoColor.Location = new System.Drawing.Point(87, 13);
            this.chkNoColor.Name = "chkNoColor";
            this.chkNoColor.Size = new System.Drawing.Size(48, 16);
            this.chkNoColor.TabIndex = 7;
            this.chkNoColor.Text = "空值";
            this.chkNoColor.UseVisualStyleBackColor = true;
            this.chkNoColor.CheckedChanged += new System.EventHandler(this.chkNoColor_CheckedChanged);
            // 
            // upAreaBoardWidth
            // 
            this.upAreaBoardWidth.DecimalPlaces = 1;
            this.upAreaBoardWidth.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.upAreaBoardWidth.Location = new System.Drawing.Point(87, 64);
            this.upAreaBoardWidth.Name = "upAreaBoardWidth";
            this.upAreaBoardWidth.Size = new System.Drawing.Size(69, 21);
            this.upAreaBoardWidth.TabIndex = 5;
            this.upAreaBoardWidth.ValueChanged += new System.EventHandler(this.upAreaBoardWidth_ValueChanged);
            // 
            // cpbAreaBoardWidth
            // 
            this.cpbAreaBoardWidth.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.cpbAreaBoardWidth.Image = ((System.Drawing.Image)(resources.GetObject("cpbAreaBoardWidth.Image")));
            this.cpbAreaBoardWidth.Location = new System.Drawing.Point(87, 94);
            this.cpbAreaBoardWidth.Name = "cpbAreaBoardWidth";
            this.cpbAreaBoardWidth.SelectedColorImageRectangle = new System.Drawing.Rectangle(2, 2, 12, 12);
            this.cpbAreaBoardWidth.Size = new System.Drawing.Size(69, 23);
            this.cpbAreaBoardWidth.TabIndex = 4;
            this.cpbAreaBoardWidth.SelectedColorChanged += new System.EventHandler(this.cpbAreaBoardWidth_SelectedColorChanged);
            // 
            // lblAreaBoardWidth
            // 
            this.lblAreaBoardWidth.Location = new System.Drawing.Point(6, 61);
            this.lblAreaBoardWidth.Name = "lblAreaBoardWidth";
            this.lblAreaBoardWidth.Size = new System.Drawing.Size(75, 23);
            this.lblAreaBoardWidth.TabIndex = 2;
            this.lblAreaBoardWidth.Text = "轮廓线宽度:";
            // 
            // lblAreaBoardColor
            // 
            this.lblAreaBoardColor.Location = new System.Drawing.Point(6, 94);
            this.lblAreaBoardColor.Name = "lblAreaBoardColor";
            this.lblAreaBoardColor.Size = new System.Drawing.Size(75, 23);
            this.lblAreaBoardColor.TabIndex = 2;
            this.lblAreaBoardColor.Text = "轮廓线颜色:";
            // 
            // cpbAreaColor
            // 
            this.cpbAreaColor.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.cpbAreaColor.Image = ((System.Drawing.Image)(resources.GetObject("cpbAreaColor.Image")));
            this.cpbAreaColor.Location = new System.Drawing.Point(87, 32);
            this.cpbAreaColor.Name = "cpbAreaColor";
            this.cpbAreaColor.SelectedColorImageRectangle = new System.Drawing.Rectangle(2, 2, 12, 12);
            this.cpbAreaColor.Size = new System.Drawing.Size(69, 23);
            this.cpbAreaColor.TabIndex = 1;
            this.cpbAreaColor.SelectedColorChanged += new System.EventHandler(this.cpbAreaColor_SelectedColorChanged);
            // 
            // lblAreaColor
            // 
            this.lblAreaColor.Location = new System.Drawing.Point(6, 23);
            this.lblAreaColor.Name = "lblAreaColor";
            this.lblAreaColor.Size = new System.Drawing.Size(66, 23);
            this.lblAreaColor.TabIndex = 0;
            this.lblAreaColor.Text = "填充颜色:";
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.Location = new System.Drawing.Point(258, 434);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(77, 29);
            this.btnCancel.TabIndex = 15;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // FrmSymbolManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(346, 470);
            this.Controls.Add(this.GroupBox1);
            this.Controls.Add(this.gbPoint);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.gbLine);
            this.Controls.Add(this.gbPreview);
            this.Controls.Add(this.gbArea);
            this.Controls.Add(this.btnCancel);
            this.Name = "FrmSymbolManager";
            this.Text = "符号配置";
            this.Load += new System.EventHandler(this.FrmSymbolManager_Load);
            this.GroupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SymbolCtrl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.upLnWidth)).EndInit();
            this.gbPoint.ResumeLayout(false);
            this.gbPoint.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.upPointAngle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.upPointSize)).EndInit();
            this.gbLine.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbPreview)).EndInit();
            this.gbPreview.ResumeLayout(false);
            this.gbArea.ResumeLayout(false);
            this.gbArea.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.upAreaBoardWidth)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.GroupBox GroupBox1;
        private ESRI.ArcGIS.Controls.AxSymbologyControl SymbolCtrl1;
        internal DevComponents.DotNetBar.Controls.ComboBoxEx cboSymbolType;
        internal DevComponents.DotNetBar.LabelX lblSymbolType;
        internal DevComponents.DotNetBar.Controls.ComboBoxEx cboSymbolFile;
        internal DevComponents.DotNetBar.LabelX lblSymbolFile;
        internal System.Windows.Forms.NumericUpDown upLnWidth;
        internal System.Windows.Forms.CheckBox chkNullColor;
        internal DevComponents.DotNetBar.LabelX lblLnWidth;
        internal DevComponents.DotNetBar.ColorPickerButton cpbLnColor;
        internal DevComponents.DotNetBar.LabelX lblLnColor;
        internal System.Windows.Forms.GroupBox gbPoint;
        internal System.Windows.Forms.NumericUpDown upPointAngle;
        internal System.Windows.Forms.NumericUpDown upPointSize;
        internal DevComponents.DotNetBar.ColorPickerButton cpbPtColor;
        internal DevComponents.DotNetBar.LabelX lblPtAngle;
        internal DevComponents.DotNetBar.LabelX lblPtSize;
        internal DevComponents.DotNetBar.LabelX lblPtColor;
        internal DevComponents.DotNetBar.ButtonX btnOK;
        internal System.Windows.Forms.GroupBox gbLine;
        internal System.Windows.Forms.PictureBox pbPreview;
        internal System.Windows.Forms.GroupBox gbPreview;
        internal System.Windows.Forms.GroupBox gbArea;
        internal System.Windows.Forms.CheckBox chkNoColor;
        internal System.Windows.Forms.NumericUpDown upAreaBoardWidth;
        internal DevComponents.DotNetBar.ColorPickerButton cpbAreaBoardWidth;
        internal DevComponents.DotNetBar.LabelX lblAreaBoardWidth;
        internal DevComponents.DotNetBar.LabelX lblAreaBoardColor;
        internal DevComponents.DotNetBar.ColorPickerButton cpbAreaColor;
        internal DevComponents.DotNetBar.LabelX lblAreaColor;
        internal DevComponents.DotNetBar.ButtonX btnCancel;

    }
}