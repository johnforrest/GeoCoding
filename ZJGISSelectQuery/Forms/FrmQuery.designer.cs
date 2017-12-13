namespace SelectQuery
{
    partial class FrmQuery:DevComponents.DotNetBar.Office2007Form
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
            this.lblFeatCount = new DevComponents.DotNetBar.LabelX();
            this.tvwFeatures = new System.Windows.Forms.TreeView();
            this.ExpandableSplitter = new DevComponents.DotNetBar.ExpandableSplitter();
            this.dgvAttributes = new DevComponents.DotNetBar.Controls.DataGridViewX();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAttributes)).BeginInit();
            this.SuspendLayout();
            // 
            // lblFeatCount
            // 
            // 
            // 
            // 
            this.lblFeatCount.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblFeatCount.BorderType = DevComponents.DotNetBar.eBorderType.Sunken;
            this.lblFeatCount.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblFeatCount.Location = new System.Drawing.Point(0, 332);
            this.lblFeatCount.Margin = new System.Windows.Forms.Padding(4);
            this.lblFeatCount.Name = "lblFeatCount";
            this.lblFeatCount.Size = new System.Drawing.Size(565, 26);
            this.lblFeatCount.TabIndex = 9;
            this.lblFeatCount.Text = "要素个数：";
            // 
            // tvwFeatures
            // 
            this.tvwFeatures.Dock = System.Windows.Forms.DockStyle.Left;
            this.tvwFeatures.Location = new System.Drawing.Point(0, 0);
            this.tvwFeatures.Margin = new System.Windows.Forms.Padding(4);
            this.tvwFeatures.Name = "tvwFeatures";
            this.tvwFeatures.Size = new System.Drawing.Size(203, 332);
            this.tvwFeatures.TabIndex = 11;
            this.tvwFeatures.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvwFeatures_NodeMouseClick);
            // 
            // ExpandableSplitter
            // 
            this.ExpandableSplitter.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this.ExpandableSplitter.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.ExpandableSplitter.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.ExpandableSplitter.ExpandFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this.ExpandableSplitter.ExpandFillColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.ExpandableSplitter.ExpandLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ExpandableSplitter.ExpandLineColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.ExpandableSplitter.GripDarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ExpandableSplitter.GripDarkColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.ExpandableSplitter.GripLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.ExpandableSplitter.GripLightColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.ExpandableSplitter.HotBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(151)))), ((int)(((byte)(61)))));
            this.ExpandableSplitter.HotBackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(184)))), ((int)(((byte)(94)))));
            this.ExpandableSplitter.HotBackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground2;
            this.ExpandableSplitter.HotBackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground;
            this.ExpandableSplitter.HotExpandFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this.ExpandableSplitter.HotExpandFillColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.ExpandableSplitter.HotExpandLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ExpandableSplitter.HotExpandLineColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.ExpandableSplitter.HotGripDarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this.ExpandableSplitter.HotGripDarkColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.ExpandableSplitter.HotGripLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.ExpandableSplitter.HotGripLightColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.ExpandableSplitter.Location = new System.Drawing.Point(203, 0);
            this.ExpandableSplitter.Margin = new System.Windows.Forms.Padding(4);
            this.ExpandableSplitter.Name = "ExpandableSplitter";
            this.ExpandableSplitter.Size = new System.Drawing.Size(1, 332);
            this.ExpandableSplitter.Style = DevComponents.DotNetBar.eSplitterStyle.Office2007;
            this.ExpandableSplitter.TabIndex = 12;
            this.ExpandableSplitter.TabStop = false;
            // 
            // dgvAttributes
            // 
            this.dgvAttributes.AllowUserToAddRows = false;
            this.dgvAttributes.AllowUserToDeleteRows = false;
            this.dgvAttributes.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvAttributes.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvAttributes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvAttributes.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvAttributes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvAttributes.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dgvAttributes.Location = new System.Drawing.Point(204, 0);
            this.dgvAttributes.Margin = new System.Windows.Forms.Padding(4);
            this.dgvAttributes.Name = "dgvAttributes";
            this.dgvAttributes.RowHeadersWidth = 80;
            this.dgvAttributes.RowTemplate.Height = 23;
            this.dgvAttributes.Size = new System.Drawing.Size(361, 332);
            this.dgvAttributes.TabIndex = 12;
            this.dgvAttributes.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewX1_CellContentClick);
            // 
            // FrmQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(565, 358);
            this.Controls.Add(this.dgvAttributes);
            this.Controls.Add(this.ExpandableSplitter);
            this.Controls.Add(this.tvwFeatures);
            this.Controls.Add(this.lblFeatCount);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FrmQuery";
            this.Text = "图元属性";
            this.Load += new System.EventHandler(this.frmQuery_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAttributes)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        internal DevComponents.DotNetBar.LabelX lblFeatCount;
        internal System.Windows.Forms.TreeView tvwFeatures;
        internal DevComponents.DotNetBar.ExpandableSplitter ExpandableSplitter;
        public DevComponents.DotNetBar.Controls.DataGridViewX dgvAttributes;

    }
}