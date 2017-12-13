namespace ZJGISDataExtract
{
    partial class FrmRegionExtract
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvRegionChoose = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.btnOK = new DevComponents.DotNetBar.ButtonX();
            this.btnBrowse = new DevComponents.DotNetBar.ButtonX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.cmbFileStyle = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.txtFileRoute = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dgvLayerChoose = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.ChkClip = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.lblInfo = new DevComponents.DotNetBar.LabelX();
            this.SaveFile = new System.Windows.Forms.SaveFileDialog();
            this.FolderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.colID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRegionName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colChoose = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chkLayer = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewButtonXColumn1 = new DevComponents.DotNetBar.Controls.DataGridViewButtonXColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRegionChoose)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLayerChoose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvRegionChoose
            // 
            this.dgvRegionChoose.AllowUserToAddRows = false;
            this.dgvRegionChoose.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRegionChoose.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colID,
            this.colRegionName,
            this.colChoose});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvRegionChoose.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvRegionChoose.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dgvRegionChoose.Location = new System.Drawing.Point(10, 18);
            this.dgvRegionChoose.Name = "dgvRegionChoose";
            this.dgvRegionChoose.RowTemplate.Height = 23;
            this.dgvRegionChoose.Size = new System.Drawing.Size(364, 340);
            this.dgvRegionChoose.TabIndex = 9;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dgvRegionChoose);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(388, 366);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "行政区选择";
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCancel.Location = new System.Drawing.Point(765, 62);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnCancel.TabIndex = 22;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOK.Location = new System.Drawing.Point(645, 63);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnOK.TabIndex = 21;
            this.btnOK.Text = "确定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnBrowse
            // 
            this.btnBrowse.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnBrowse.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnBrowse.Location = new System.Drawing.Point(793, 27);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(47, 23);
            this.btnBrowse.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnBrowse.TabIndex = 20;
            this.btnBrowse.Text = "浏览";
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // labelX2
            // 
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Location = new System.Drawing.Point(419, 25);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(71, 23);
            this.labelX2.TabIndex = 19;
            this.labelX2.Text = "输出路径：";
            // 
            // cmbFileStyle
            // 
            this.cmbFileStyle.DisplayMember = "Text";
            this.cmbFileStyle.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbFileStyle.FormattingEnabled = true;
            this.cmbFileStyle.ItemHeight = 15;
            this.cmbFileStyle.Location = new System.Drawing.Point(101, 25);
            this.cmbFileStyle.Name = "cmbFileStyle";
            this.cmbFileStyle.Size = new System.Drawing.Size(214, 21);
            this.cmbFileStyle.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cmbFileStyle.TabIndex = 18;
            this.cmbFileStyle.TextChanged += new System.EventHandler(this.cmbFileStyle_TextChanged);
            // 
            // txtFileRoute
            // 
            // 
            // 
            // 
            this.txtFileRoute.Border.Class = "TextBoxBorder";
            this.txtFileRoute.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtFileRoute.Location = new System.Drawing.Point(496, 27);
            this.txtFileRoute.Name = "txtFileRoute";
            this.txtFileRoute.Size = new System.Drawing.Size(288, 21);
            this.txtFileRoute.TabIndex = 17;
            // 
            // labelX1
            // 
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Location = new System.Drawing.Point(24, 25);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(71, 23);
            this.labelX1.TabIndex = 16;
            this.labelX1.Text = "保存格式：";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.dgvLayerChoose);
            this.groupBox3.Location = new System.Drawing.Point(406, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(421, 366);
            this.groupBox3.TabIndex = 14;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "图层选取";
            // 
            // dgvLayerChoose
            // 
            this.dgvLayerChoose.AllowUserToAddRows = false;
            this.dgvLayerChoose.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLayerChoose.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.chkLayer,
            this.dataGridViewButtonXColumn1});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvLayerChoose.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvLayerChoose.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dgvLayerChoose.Location = new System.Drawing.Point(10, 13);
            this.dgvLayerChoose.Name = "dgvLayerChoose";
            this.dgvLayerChoose.RowTemplate.Height = 23;
            this.dgvLayerChoose.Size = new System.Drawing.Size(404, 345);
            this.dgvLayerChoose.TabIndex = 10;
            this.dgvLayerChoose.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvLayerChoose_CellMouseClick);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox3);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.ChkClip);
            this.splitContainer1.Panel2.Controls.Add(this.lblInfo);
            this.splitContainer1.Panel2.Controls.Add(this.labelX1);
            this.splitContainer1.Panel2.Controls.Add(this.btnCancel);
            this.splitContainer1.Panel2.Controls.Add(this.cmbFileStyle);
            this.splitContainer1.Panel2.Controls.Add(this.btnBrowse);
            this.splitContainer1.Panel2.Controls.Add(this.btnOK);
            this.splitContainer1.Panel2.Controls.Add(this.labelX2);
            this.splitContainer1.Panel2.Controls.Add(this.txtFileRoute);
            this.splitContainer1.Size = new System.Drawing.Size(865, 491);
            this.splitContainer1.SplitterDistance = 390;
            this.splitContainer1.TabIndex = 15;
            // 
            // ChkClip
            // 
            // 
            // 
            // 
            this.ChkClip.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.ChkClip.Location = new System.Drawing.Point(459, 62);
            this.ChkClip.Name = "ChkClip";
            this.ChkClip.Size = new System.Drawing.Size(63, 23);
            this.ChkClip.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ChkClip.TabIndex = 24;
            this.ChkClip.Text = "剪切";
            // 
            // lblInfo
            // 
            // 
            // 
            // 
            this.lblInfo.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblInfo.Location = new System.Drawing.Point(24, 63);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(281, 23);
            this.lblInfo.TabIndex = 23;
            // 
            // colID
            // 
            this.colID.HeaderText = "序号";
            this.colID.Name = "colID";
            this.colID.Width = 60;
            // 
            // colRegionName
            // 
            this.colRegionName.HeaderText = "行政区名称";
            this.colRegionName.Name = "colRegionName";
            this.colRegionName.Width = 180;
            // 
            // colChoose
            // 
            this.colChoose.HeaderText = "提取";
            this.colChoose.Name = "colChoose";
            this.colChoose.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colChoose.Width = 80;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "序号";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Width = 60;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "图层名称";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Width = 180;
            // 
            // chkLayer
            // 
            this.chkLayer.HeaderText = "提取";
            this.chkLayer.Name = "chkLayer";
            this.chkLayer.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.chkLayer.Width = 60;
            // 
            // dataGridViewButtonXColumn1
            // 
            this.dataGridViewButtonXColumn1.HeaderText = "属性选择";
            this.dataGridViewButtonXColumn1.Name = "dataGridViewButtonXColumn1";
            this.dataGridViewButtonXColumn1.Text = null;
            this.dataGridViewButtonXColumn1.Width = 60;
            // 
            // FrmRegionExtract
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(865, 491);
            this.Controls.Add(this.splitContainer1);
            this.DoubleBuffered = true;
            this.Name = "FrmRegionExtract";
            this.Text = "行政区域提取";
            this.Load += new System.EventHandler(this.FrmRegionExtract_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRegionChoose)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLayerChoose)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.DataGridViewX dgvRegionChoose;
        private System.Windows.Forms.GroupBox groupBox1;
        private DevComponents.DotNetBar.ButtonX btnCancel;
        private DevComponents.DotNetBar.ButtonX btnOK;
        private DevComponents.DotNetBar.ButtonX btnBrowse;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbFileStyle;
        private DevComponents.DotNetBar.Controls.TextBoxX txtFileRoute;
        private DevComponents.DotNetBar.LabelX labelX1;
        private System.Windows.Forms.GroupBox groupBox3;
        private DevComponents.DotNetBar.Controls.DataGridViewX dgvLayerChoose;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private DevComponents.DotNetBar.LabelX lblInfo;
        private System.Windows.Forms.SaveFileDialog SaveFile;
        private System.Windows.Forms.FolderBrowserDialog FolderBrowser;
        private DevComponents.DotNetBar.Controls.CheckBoxX ChkClip;
        private System.Windows.Forms.DataGridViewTextBoxColumn colID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRegionName;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colChoose;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewCheckBoxColumn chkLayer;
        private DevComponents.DotNetBar.Controls.DataGridViewButtonXColumn dataGridViewButtonXColumn1;
    }
}