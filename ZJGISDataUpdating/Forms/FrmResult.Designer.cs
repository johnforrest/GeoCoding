namespace ZJGISDataUpdating
{
    partial class FrmResult
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
            this.dockContainerItem2 = new DevComponents.DotNetBar.DockContainerItem();
            this.barMatchedResult = new DevComponents.DotNetBar.Bar();
            this.panelDockContainerMatchedResult = new DevComponents.DotNetBar.PanelDockContainer();
            this.dockContainerItem3 = new DevComponents.DotNetBar.DockContainerItem();
            this.dockContainerItem1 = new DevComponents.DotNetBar.DockContainerItem();
            ((System.ComponentModel.ISupportInitialize)(this.barMatchedResult)).BeginInit();
            this.barMatchedResult.SuspendLayout();
            this.SuspendLayout();
            // 
            // dockContainerItem2
            // 
            this.dockContainerItem2.Name = "dockContainerItem2";
            this.dockContainerItem2.Text = "编辑匹配结果表";
            // 
            // barMatchedResult
            // 
            this.barMatchedResult.AccessibleDescription = "DotNetBar Bar (barMatchedResult)";
            this.barMatchedResult.AccessibleName = "DotNetBar Bar";
            this.barMatchedResult.AccessibleRole = System.Windows.Forms.AccessibleRole.ToolBar;
            this.barMatchedResult.CanHide = true;
            this.barMatchedResult.CloseSingleTab = true;
            this.barMatchedResult.Controls.Add(this.panelDockContainerMatchedResult);
            this.barMatchedResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.barMatchedResult.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.barMatchedResult.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.dockContainerItem3});
            this.barMatchedResult.LayoutType = DevComponents.DotNetBar.eLayoutType.DockContainer;
            this.barMatchedResult.Location = new System.Drawing.Point(0, 0);
            this.barMatchedResult.Margin = new System.Windows.Forms.Padding(4);
            this.barMatchedResult.Name = "barMatchedResult";
            this.barMatchedResult.Size = new System.Drawing.Size(1009, 559);
            this.barMatchedResult.Stretch = true;
            this.barMatchedResult.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.barMatchedResult.TabIndex = 0;
            this.barMatchedResult.TabStop = false;
            this.barMatchedResult.Text = "编辑匹配结果表";
            // 
            // panelDockContainerMatchedResult
            // 
            this.panelDockContainerMatchedResult.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.panelDockContainerMatchedResult.Location = new System.Drawing.Point(3, 3);
            this.panelDockContainerMatchedResult.Margin = new System.Windows.Forms.Padding(4);
            this.panelDockContainerMatchedResult.Name = "panelDockContainerMatchedResult";
            this.panelDockContainerMatchedResult.Size = new System.Drawing.Size(1003, 553);
            this.panelDockContainerMatchedResult.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelDockContainerMatchedResult.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.panelDockContainerMatchedResult.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder;
            this.panelDockContainerMatchedResult.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.panelDockContainerMatchedResult.Style.GradientAngle = 90;
            this.panelDockContainerMatchedResult.TabIndex = 0;
            // 
            // dockContainerItem3
            // 
            this.dockContainerItem3.Control = this.panelDockContainerMatchedResult;
            this.dockContainerItem3.Name = "dockContainerItem3";
            this.dockContainerItem3.Text = "dockContainerItem3";
            // 
            // dockContainerItem1
            // 
            this.dockContainerItem1.Name = "dockContainerItem1";
            this.dockContainerItem1.Text = "dockContainerItem1";
            // 
            // FrmResult
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1009, 559);
            this.Controls.Add(this.barMatchedResult);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmResult";
            this.Text = "编辑匹配结果表";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmResult_FormClosed);
            this.Load += new System.EventHandler(this.FrmResult_Load);
            ((System.ComponentModel.ISupportInitialize)(this.barMatchedResult)).EndInit();
            this.barMatchedResult.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.DockContainerItem dockContainerItem2;
        private DevComponents.DotNetBar.Bar barMatchedResult;
        private DevComponents.DotNetBar.DockContainerItem dockContainerItem3;
        private DevComponents.DotNetBar.DockContainerItem dockContainerItem1;
        public DevComponents.DotNetBar.PanelDockContainer panelDockContainerMatchedResult;
    }
}