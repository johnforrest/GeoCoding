namespace ZJGISDataExtract
{
    partial class FrmPolygonExtract
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
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.txtFileRoute = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.cmbFileStyle = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.btnBrowse = new DevComponents.DotNetBar.ButtonX();
            this.btnOK = new DevComponents.DotNetBar.ButtonX();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.ChkClip = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.FolderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.dgvChoose = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column4 = new DevComponents.DotNetBar.Controls.DataGridViewButtonXColumn();
            this.lblInfo = new DevComponents.DotNetBar.LabelX();
            this.SaveFile = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChoose)).BeginInit();
            this.SuspendLayout();
            // 
            // labelX1
            // 
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Location = new System.Drawing.Point(385, 98);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(75, 23);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "保存格式：";
            // 
            // txtFileRoute
            // 
            // 
            // 
            // 
            this.txtFileRoute.Border.Class = "TextBoxBorder";
            this.txtFileRoute.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtFileRoute.Location = new System.Drawing.Point(385, 188);
            this.txtFileRoute.Name = "txtFileRoute";
            this.txtFileRoute.Size = new System.Drawing.Size(212, 21);
            this.txtFileRoute.TabIndex = 1;
            // 
            // cmbFileStyle
            // 
            this.cmbFileStyle.DisplayMember = "Text";
            this.cmbFileStyle.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbFileStyle.FormattingEnabled = true;
            this.cmbFileStyle.ItemHeight = 15;
            this.cmbFileStyle.Location = new System.Drawing.Point(385, 122);
            this.cmbFileStyle.Name = "cmbFileStyle";
            this.cmbFileStyle.Size = new System.Drawing.Size(212, 21);
            this.cmbFileStyle.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cmbFileStyle.TabIndex = 2;
            this.cmbFileStyle.TextChanged += new System.EventHandler(this.cmbFileStyle_TextChanged);
            // 
            // labelX2
            // 
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Location = new System.Drawing.Point(385, 164);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(75, 23);
            this.labelX2.TabIndex = 3;
            this.labelX2.Text = "输出路径：";
            // 
            // btnBrowse
            // 
            this.btnBrowse.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnBrowse.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnBrowse.Location = new System.Drawing.Point(608, 189);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(47, 23);
            this.btnBrowse.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnBrowse.TabIndex = 4;
            this.btnBrowse.Text = "浏览";
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // btnOK
            // 
            this.btnOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOK.Location = new System.Drawing.Point(467, 268);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "确定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCancel.Location = new System.Drawing.Point(579, 268);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // ChkClip
            // 
            // 
            // 
            // 
            this.ChkClip.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.ChkClip.Location = new System.Drawing.Point(385, 226);
            this.ChkClip.Name = "ChkClip";
            this.ChkClip.Size = new System.Drawing.Size(119, 23);
            this.ChkClip.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ChkClip.TabIndex = 7;
            this.ChkClip.Text = "剪切";
            // 
            // dgvChoose
            // 
            this.dgvChoose.AllowUserToAddRows = false;
            this.dgvChoose.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvChoose.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvChoose.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvChoose.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dgvChoose.Location = new System.Drawing.Point(-1, 1);
            this.dgvChoose.Name = "dgvChoose";
            this.dgvChoose.RowTemplate.Height = 23;
            this.dgvChoose.Size = new System.Drawing.Size(373, 316);
            this.dgvChoose.TabIndex = 8;
            this.dgvChoose.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewX1_CellMouseClick);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "序号";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 60;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "图层名称";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 150;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "提取";
            this.Column3.Name = "Column3";
            this.Column3.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column3.Width = 60;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "属性筛选";
            this.Column4.Name = "Column4";
            this.Column4.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column4.Text = "进入";
            this.Column4.Width = 60;
            // 
            // lblInfo
            // 
            // 
            // 
            // 
            this.lblInfo.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblInfo.Location = new System.Drawing.Point(429, 16);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(196, 47);
            this.lblInfo.TabIndex = 15;
            this.lblInfo.WordWrap = true;
            // 
            // FrmPolygonExtract
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(658, 343);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.dgvChoose);
            this.Controls.Add(this.ChkClip);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.cmbFileStyle);
            this.Controls.Add(this.txtFileRoute);
            this.Controls.Add(this.labelX1);
            this.DoubleBuffered = true;
            this.MaximizeBox = false;
            this.Name = "FrmPolygonExtract";
            this.Text = "自定义范围提取";
            this.Load += new System.EventHandler(this.FrmPolygonExtract_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvChoose)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Controls.TextBoxX txtFileRoute;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbFileStyle;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.ButtonX btnBrowse;
        private DevComponents.DotNetBar.ButtonX btnOK;
        private DevComponents.DotNetBar.ButtonX btnCancel;
        private DevComponents.DotNetBar.Controls.CheckBoxX ChkClip;
        private System.Windows.Forms.FolderBrowserDialog FolderBrowser;
        private DevComponents.DotNetBar.Controls.DataGridViewX dgvChoose;
        internal DevComponents.DotNetBar.LabelX lblInfo;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column3;
        private DevComponents.DotNetBar.Controls.DataGridViewButtonXColumn Column4;
        private System.Windows.Forms.SaveFileDialog SaveFile;
    }
}