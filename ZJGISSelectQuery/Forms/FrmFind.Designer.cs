namespace SelectQuery
{
    partial class FrmFind
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.cboMatchFind = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.cboExtentFind = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.txtStrFind = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.dgvResFind = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.bar2 = new DevComponents.DotNetBar.Bar();
            this.lblPrompt = new DevComponents.DotNetBar.LabelItem();
            this.Progress = new DevComponents.DotNetBar.ProgressBarItem();
            this.buttonX1 = new DevComponents.DotNetBar.ButtonX();
            this.buttonX2 = new DevComponents.DotNetBar.ButtonX();
            this.panelEx1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResFind)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bar2)).BeginInit();
            this.SuspendLayout();
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.panelEx1.Controls.Add(this.cboMatchFind);
            this.panelEx1.Controls.Add(this.cboExtentFind);
            this.panelEx1.Controls.Add(this.txtStrFind);
            this.panelEx1.Controls.Add(this.labelX3);
            this.panelEx1.Controls.Add(this.labelX2);
            this.panelEx1.Controls.Add(this.labelX1);
            this.panelEx1.Location = new System.Drawing.Point(0, 12);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(601, 132);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 0;
            this.panelEx1.Text = "panelEx1";
            this.panelEx1.Click += new System.EventHandler(this.panelEx1_Click);
            // 
            // cboMatchFind
            // 
            this.cboMatchFind.DisplayMember = "Text";
            this.cboMatchFind.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboMatchFind.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMatchFind.FormattingEnabled = true;
            this.cboMatchFind.ItemHeight = 15;
            this.cboMatchFind.Location = new System.Drawing.Point(103, 96);
            this.cboMatchFind.Name = "cboMatchFind";
            this.cboMatchFind.Size = new System.Drawing.Size(486, 21);
            this.cboMatchFind.TabIndex = 5;
            this.cboMatchFind.SelectedIndexChanged += new System.EventHandler(this.cboMatchFind_SelectedIndexChanged);
            // 
            // cboExtentFind
            // 
            this.cboExtentFind.DisplayMember = "Text";
            this.cboExtentFind.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboExtentFind.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboExtentFind.FormattingEnabled = true;
            this.cboExtentFind.ItemHeight = 15;
            this.cboExtentFind.Location = new System.Drawing.Point(103, 54);
            this.cboExtentFind.Name = "cboExtentFind";
            this.cboExtentFind.Size = new System.Drawing.Size(486, 21);
            this.cboExtentFind.TabIndex = 4;
            this.cboExtentFind.SelectedIndexChanged += new System.EventHandler(this.cboExtentFind_SelectedIndexChanged);
            // 
            // txtStrFind
            // 
            // 
            // 
            // 
            this.txtStrFind.Border.Class = "TextBoxBorder";
            this.txtStrFind.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtStrFind.Location = new System.Drawing.Point(103, 21);
            this.txtStrFind.Name = "txtStrFind";
            this.txtStrFind.Size = new System.Drawing.Size(486, 21);
            this.txtStrFind.TabIndex = 3;
            this.txtStrFind.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtStrFind_KeyUp);
            // 
            // labelX3
            // 
            // 
            // 
            // 
            this.labelX3.BackgroundStyle.Class = "";
            this.labelX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX3.Location = new System.Drawing.Point(14, 90);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(73, 27);
            this.labelX3.TabIndex = 2;
            this.labelX3.Text = "匹配方式：";
            this.labelX3.Click += new System.EventHandler(this.labelX3_Click);
            // 
            // labelX2
            // 
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.Class = "";
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Location = new System.Drawing.Point(14, 49);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(73, 26);
            this.labelX2.TabIndex = 1;
            this.labelX2.Text = "查询范围：";
            // 
            // labelX1
            // 
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.Class = "";
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Location = new System.Drawing.Point(14, 13);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(73, 30);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "查询值：";
            // 
            // dgvResFind
            // 
            this.dgvResFind.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvResFind.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvResFind.DefaultCellStyle = dataGridViewCellStyle6;
            this.dgvResFind.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dgvResFind.Location = new System.Drawing.Point(0, 150);
            this.dgvResFind.Name = "dgvResFind";
            this.dgvResFind.RowTemplate.Height = 23;
            this.dgvResFind.Size = new System.Drawing.Size(683, 305);
            this.dgvResFind.TabIndex = 2;
            this.dgvResFind.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.dgvResFind_MouseDoubleClick);
            // 
            // bar2
            // 
            this.bar2.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.lblPrompt,
            this.Progress});
            this.bar2.Location = new System.Drawing.Point(0, 461);
            this.bar2.Name = "bar2";
            this.bar2.ShowToolTips = false;
            this.bar2.Size = new System.Drawing.Size(683, 21);
            this.bar2.Stretch = true;
            this.bar2.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2003;
            this.bar2.TabIndex = 3;
            this.bar2.TabStop = false;
            this.bar2.Text = "bar2";
            // 
            // lblPrompt
            // 
            this.lblPrompt.Name = "lblPrompt";
            this.lblPrompt.Text = "提示：";
            // 
            // Progress
            // 
            // 
            // 
            // 
            this.Progress.BackStyle.Class = "";
            this.Progress.BackStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.Progress.ChunkGradientAngle = 0F;
            this.Progress.MenuVisibility = DevComponents.DotNetBar.eMenuVisibility.VisibleAlways;
            this.Progress.Name = "Progress";
            this.Progress.RecentlyUsed = false;
            this.Progress.Width = 200;
            // 
            // buttonX1
            // 
            this.buttonX1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX1.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonX1.Location = new System.Drawing.Point(608, 31);
            this.buttonX1.Name = "buttonX1";
            this.buttonX1.Size = new System.Drawing.Size(54, 23);
           // this.buttonX1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonX1.TabIndex = 4;
            this.buttonX1.Text = "查询";
            this.buttonX1.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // buttonX2
            // 
            this.buttonX2.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX2.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonX2.Location = new System.Drawing.Point(608, 66);
            this.buttonX2.Name = "buttonX2";
            this.buttonX2.Size = new System.Drawing.Size(54, 23);
            //this.buttonX2.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonX2.TabIndex = 5;
            this.buttonX2.Text = "停止";
            this.buttonX2.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // FrmFind
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 490);
            this.Controls.Add(this.buttonX2);
            this.Controls.Add(this.buttonX1);
            this.Controls.Add(this.bar2);
            this.Controls.Add(this.dgvResFind);
            this.Controls.Add(this.panelEx1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "FrmFind";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "查询";
            this.Load += new System.EventHandler(this.frmFind_Load);
            this.panelEx1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvResFind)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bar2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.PanelEx panelEx1;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Controls.DataGridViewX dgvResFind;
        private DevComponents.DotNetBar.Bar bar2;
        private DevComponents.DotNetBar.Controls.TextBoxX txtStrFind;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cboMatchFind;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cboExtentFind;
        private DevComponents.DotNetBar.LabelItem lblPrompt;
        private DevComponents.DotNetBar.ProgressBarItem Progress;
        private DevComponents.DotNetBar.ButtonX buttonX1;
        private DevComponents.DotNetBar.ButtonX buttonX2;
    }
}