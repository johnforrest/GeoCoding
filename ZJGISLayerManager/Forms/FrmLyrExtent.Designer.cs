namespace ZJGISLayerManager
{
    partial class FrmLyrExtent
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
            this.DGVSource = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblTop = new DevComponents.DotNetBar.LabelX();
            this.GroupPanel2 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.GroupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.lblBottom = new DevComponents.DotNetBar.LabelX();
            this.lblRight = new DevComponents.DotNetBar.LabelX();
            this.lblLeft = new DevComponents.DotNetBar.LabelX();
            ((System.ComponentModel.ISupportInitialize)(this.DGVSource)).BeginInit();
            this.GroupPanel2.SuspendLayout();
            this.GroupPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // DGVSource
            // 
            this.DGVSource.AllowUserToAddRows = false;
            this.DGVSource.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGVSource.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DGVSource.DefaultCellStyle = dataGridViewCellStyle1;
            this.DGVSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DGVSource.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.DGVSource.Location = new System.Drawing.Point(0, 0);
            this.DGVSource.Name = "DGVSource";
            this.DGVSource.RowTemplate.Height = 23;
            this.DGVSource.Size = new System.Drawing.Size(386, 232);
            this.DGVSource.TabIndex = 0;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "属性项";
            this.Column1.Name = "Column1";
            // 
            // Column2
            // 
            this.Column2.HeaderText = "值";
            this.Column2.Name = "Column2";
            this.Column2.Width = 240;
            // 
            // lblTop
            // 
            this.lblTop.BackColor = System.Drawing.Color.Transparent;
            this.lblTop.Location = new System.Drawing.Point(130, 12);
            this.lblTop.Name = "lblTop";
            this.lblTop.Size = new System.Drawing.Size(122, 20);
            this.lblTop.TabIndex = 0;
            this.lblTop.Text = "上:Top";
            // 
            // GroupPanel2
            // 
            this.GroupPanel2.CanvasColor = System.Drawing.SystemColors.Control;
            this.GroupPanel2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.GroupPanel2.Controls.Add(this.DGVSource);
            this.GroupPanel2.Location = new System.Drawing.Point(3, 145);
            this.GroupPanel2.Name = "GroupPanel2";
            this.GroupPanel2.Size = new System.Drawing.Size(392, 256);
            // 
            // 
            // 
            this.GroupPanel2.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.GroupPanel2.Style.BackColorGradientAngle = 90;
            this.GroupPanel2.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.GroupPanel2.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.GroupPanel2.Style.BorderBottomWidth = 1;
            this.GroupPanel2.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.GroupPanel2.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.GroupPanel2.Style.BorderLeftWidth = 1;
            this.GroupPanel2.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.GroupPanel2.Style.BorderRightWidth = 1;
            this.GroupPanel2.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.GroupPanel2.Style.BorderTopWidth = 1;
            this.GroupPanel2.Style.CornerDiameter = 4;
            this.GroupPanel2.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.GroupPanel2.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.GroupPanel2.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.GroupPanel2.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            this.GroupPanel2.TabIndex = 7;
            this.GroupPanel2.Text = "数据源信息";
            // 
            // GroupPanel1
            // 
            this.GroupPanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.GroupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.GroupPanel1.Controls.Add(this.lblBottom);
            this.GroupPanel1.Controls.Add(this.lblRight);
            this.GroupPanel1.Controls.Add(this.lblLeft);
            this.GroupPanel1.Controls.Add(this.lblTop);
            this.GroupPanel1.Location = new System.Drawing.Point(3, 3);
            this.GroupPanel1.Name = "GroupPanel1";
            this.GroupPanel1.Size = new System.Drawing.Size(392, 134);
            // 
            // 
            // 
            this.GroupPanel1.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.GroupPanel1.Style.BackColorGradientAngle = 90;
            this.GroupPanel1.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.GroupPanel1.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.GroupPanel1.Style.BorderBottomWidth = 1;
            this.GroupPanel1.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.GroupPanel1.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.GroupPanel1.Style.BorderLeftWidth = 1;
            this.GroupPanel1.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.GroupPanel1.Style.BorderRightWidth = 1;
            this.GroupPanel1.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.GroupPanel1.Style.BorderTopWidth = 1;
            this.GroupPanel1.Style.CornerDiameter = 4;
            this.GroupPanel1.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.GroupPanel1.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.GroupPanel1.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.GroupPanel1.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            this.GroupPanel1.TabIndex = 6;
            this.GroupPanel1.Text = "图层范围";
            // 
            // lblBottom
            // 
            this.lblBottom.BackColor = System.Drawing.Color.Transparent;
            this.lblBottom.Location = new System.Drawing.Point(130, 74);
            this.lblBottom.Name = "lblBottom";
            this.lblBottom.Size = new System.Drawing.Size(122, 20);
            this.lblBottom.TabIndex = 2;
            this.lblBottom.Text = "下:Bottom";
            // 
            // lblRight
            // 
            this.lblRight.BackColor = System.Drawing.Color.Transparent;
            this.lblRight.Location = new System.Drawing.Point(260, 42);
            this.lblRight.Name = "lblRight";
            this.lblRight.Size = new System.Drawing.Size(122, 20);
            this.lblRight.TabIndex = 3;
            this.lblRight.Text = "右:Right";
            // 
            // lblLeft
            // 
            this.lblLeft.BackColor = System.Drawing.Color.Transparent;
            this.lblLeft.Location = new System.Drawing.Point(16, 42);
            this.lblLeft.Name = "lblLeft";
            this.lblLeft.Size = new System.Drawing.Size(122, 20);
            this.lblLeft.TabIndex = 1;
            this.lblLeft.Text = "左:Left";
            // 
            // FrmLyrExtent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(398, 405);
            this.Controls.Add(this.GroupPanel2);
            this.Controls.Add(this.GroupPanel1);
            this.Name = "FrmLyrExtent";
            this.Text = "图层属性";
            this.Load += new System.EventHandler(this.FrmLyrExtent_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DGVSource)).EndInit();
            this.GroupPanel2.ResumeLayout(false);
            this.GroupPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        internal DevComponents.DotNetBar.Controls.DataGridViewX DGVSource;
        internal System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        internal System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        internal DevComponents.DotNetBar.LabelX lblTop;
        internal DevComponents.DotNetBar.Controls.GroupPanel GroupPanel2;
        internal DevComponents.DotNetBar.Controls.GroupPanel GroupPanel1;
        internal DevComponents.DotNetBar.LabelX lblBottom;
        internal DevComponents.DotNetBar.LabelX lblRight;
        internal DevComponents.DotNetBar.LabelX lblLeft;
    }
}