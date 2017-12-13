namespace ZJGISLayerManager
{
    partial class FrmAttributeTB
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAttributeTB));
            this.DockSite1 = new DevComponents.DotNetBar.DockSite();
            this.dgvTable = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.contextMenuStripRightClick = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.btnRightClickGeocoding = new System.Windows.Forms.ToolStripMenuItem();
            this.barTool = new DevComponents.DotNetBar.Bar();
            this.biDisplayAll = new DevComponents.DotNetBar.ButtonItem();
            this.biUnDisplayAll = new DevComponents.DotNetBar.ButtonItem();
            this.biDisplaySel = new DevComponents.DotNetBar.ButtonItem();
            this.biExportExcel = new DevComponents.DotNetBar.ButtonItem();
            this.biHide = new DevComponents.DotNetBar.ButtonItem();
            this.biSave = new DevComponents.DotNetBar.ButtonItem();
            this.DockSite5 = new DevComponents.DotNetBar.DockSite();
            this.DockSite6 = new DevComponents.DotNetBar.DockSite();
            this.DockSite7 = new DevComponents.DotNetBar.DockSite();
            this.DockSite8 = new DevComponents.DotNetBar.DockSite();
            this.DockSite2 = new DevComponents.DotNetBar.DockSite();
            this.lblCount = new DevComponents.DotNetBar.LabelItem();
            this.DockSite3 = new DevComponents.DotNetBar.DockSite();
            this.barStatus = new DevComponents.DotNetBar.Bar();
            this.labelXSelectItems = new DevComponents.DotNetBar.LabelX();
            this.ProgressBar = new DevComponents.DotNetBar.ProgressBarItem();
            this.DockContainerItem1 = new DevComponents.DotNetBar.DockContainerItem();
            this.PanelDockContainer1 = new DevComponents.DotNetBar.PanelDockContainer();
            this.ContextMenuBar = new DevComponents.DotNetBar.ContextMenuBar();
            this.barSelectType = new DevComponents.DotNetBar.ButtonItem();
            this.biSelectAll = new DevComponents.DotNetBar.ButtonItem();
            this.biUnSelectAll = new DevComponents.DotNetBar.ButtonItem();
            this.lstFields = new DevComponents.DotNetBar.Controls.ListViewEx();
            this.Field = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Bar1 = new DevComponents.DotNetBar.Bar();
            this.barFields = new DevComponents.DotNetBar.Bar();
            this.dockSite4 = new DevComponents.DotNetBar.DockSite();
            this.dockSite9 = new DevComponents.DotNetBar.DockSite();
            this.barManager = new DevComponents.DotNetBar.DotNetBarManager(this.components);
            this.buttonItem1 = new DevComponents.DotNetBar.ButtonItem();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTable)).BeginInit();
            this.contextMenuStripRightClick.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.barTool)).BeginInit();
            this.DockSite7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.barStatus)).BeginInit();
            this.barStatus.SuspendLayout();
            this.PanelDockContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ContextMenuBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Bar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barFields)).BeginInit();
            this.barFields.SuspendLayout();
            this.dockSite4.SuspendLayout();
            this.SuspendLayout();
            // 
            // DockSite1
            // 
            this.DockSite1.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            this.DockSite1.Location = new System.Drawing.Point(0, 0);
            this.DockSite1.Name = "DockSite1";
            this.DockSite1.Size = new System.Drawing.Size(0, 0);
            this.DockSite1.TabIndex = 0;
            this.DockSite1.TabStop = false;
            // 
            // dgvTable
            // 
            this.dgvTable.AllowUserToAddRows = false;
            this.dgvTable.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvTable.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTable.ContextMenuStrip = this.contextMenuStripRightClick;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvTable.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTable.EnableHeadersVisualStyles = false;
            this.dgvTable.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dgvTable.Location = new System.Drawing.Point(223, 29);
            this.dgvTable.Margin = new System.Windows.Forms.Padding(4);
            this.dgvTable.Name = "dgvTable";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvTable.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvTable.RowTemplate.Height = 23;
            this.dgvTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTable.Size = new System.Drawing.Size(824, 569);
            this.dgvTable.TabIndex = 19;
            this.dgvTable.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTable_CellValueChanged);
            this.dgvTable.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvTable_ColumnHeaderMouseClick);
            this.dgvTable.RowHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvTable_RowHeaderMouseClick);
            this.dgvTable.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.dgvTable_SortCompare);
            // 
            // contextMenuStripRightClick
            // 
            this.contextMenuStripRightClick.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnRightClickGeocoding});
            this.contextMenuStripRightClick.Name = "contextMenuStripRightClick";
            this.contextMenuStripRightClick.Size = new System.Drawing.Size(139, 28);
            // 
            // btnRightClickGeocoding
            // 
            this.btnRightClickGeocoding.Name = "btnRightClickGeocoding";
            this.btnRightClickGeocoding.Size = new System.Drawing.Size(138, 24);
            this.btnRightClickGeocoding.Text = "地理编码";
            this.btnRightClickGeocoding.Click += new System.EventHandler(this.btnRightClickGeocoding_Click);
            // 
            // barTool
            // 
            this.barTool.AccessibleDescription = "DotNetBar Bar (barTool)";
            this.barTool.AccessibleName = "DotNetBar Bar";
            this.barTool.AccessibleRole = System.Windows.Forms.AccessibleRole.ToolBar;
            this.barTool.DockSide = DevComponents.DotNetBar.eDockSide.Top;
            this.barTool.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.barTool.GrabHandleStyle = DevComponents.DotNetBar.eGrabHandleStyle.Office2003;
            this.barTool.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.biDisplayAll,
            this.biUnDisplayAll,
            this.biDisplaySel,
            this.biExportExcel,
            this.biHide,
            this.biSave});
            this.barTool.Location = new System.Drawing.Point(0, 0);
            this.barTool.Margin = new System.Windows.Forms.Padding(4);
            this.barTool.Name = "barTool";
            this.barTool.Size = new System.Drawing.Size(624, 29);
            this.barTool.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2003;
            this.barTool.TabIndex = 0;
            this.barTool.TabStop = false;
            this.barTool.Text = "Bar1";
            // 
            // biDisplayAll
            // 
            this.biDisplayAll.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.biDisplayAll.Image = ((System.Drawing.Image)(resources.GetObject("biDisplayAll.Image")));
            this.biDisplayAll.Name = "biDisplayAll";
            this.biDisplayAll.Text = "显示所有字段";
            this.biDisplayAll.Click += new System.EventHandler(this.biDisplayAll_Click);
            // 
            // biUnDisplayAll
            // 
            this.biUnDisplayAll.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.biUnDisplayAll.Image = global::ZJGISLayerManager.Properties.Resources.biUnDisplayAll1;
            this.biUnDisplayAll.ImageListSizeSelection = DevComponents.DotNetBar.eButtonImageListSelection.Default;
            this.biUnDisplayAll.Name = "biUnDisplayAll";
            this.biUnDisplayAll.Text = "隐藏所有字段";
            this.biUnDisplayAll.Click += new System.EventHandler(this.biUnDisplayAll_Click);
            // 
            // biDisplaySel
            // 
            this.biDisplaySel.BeginGroup = true;
            this.biDisplaySel.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.biDisplaySel.Image = ((System.Drawing.Image)(resources.GetObject("biDisplaySel.Image")));
            this.biDisplaySel.Name = "biDisplaySel";
            this.biDisplaySel.Text = "显示选择字段";
            this.biDisplaySel.Click += new System.EventHandler(this.biDisplaySel_Click);
            // 
            // biExportExcel
            // 
            this.biExportExcel.BeginGroup = true;
            this.biExportExcel.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.biExportExcel.Image = ((System.Drawing.Image)(resources.GetObject("biExportExcel.Image")));
            this.biExportExcel.Name = "biExportExcel";
            this.biExportExcel.Text = "导出到Excel";
            this.biExportExcel.Click += new System.EventHandler(this.biExportExcel_Click);
            // 
            // biHide
            // 
            this.biHide.BeginGroup = true;
            this.biHide.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.biHide.Image = ((System.Drawing.Image)(resources.GetObject("biHide.Image")));
            this.biHide.Name = "biHide";
            this.biHide.Text = "隐藏列表框";
            this.biHide.Click += new System.EventHandler(this.biHide_Click);
            // 
            // biSave
            // 
            this.biSave.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.biSave.Image = ((System.Drawing.Image)(resources.GetObject("biSave.Image")));
            this.biSave.Name = "biSave";
            this.biSave.Text = "保存";
            this.biSave.Click += new System.EventHandler(this.biSave_Click);
            // 
            // DockSite5
            // 
            this.DockSite5.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            this.DockSite5.Dock = System.Windows.Forms.DockStyle.Left;
            this.DockSite5.Location = new System.Drawing.Point(223, 29);
            this.DockSite5.Margin = new System.Windows.Forms.Padding(4);
            this.DockSite5.Name = "DockSite5";
            this.DockSite5.Size = new System.Drawing.Size(0, 592);
            this.DockSite5.TabIndex = 14;
            this.DockSite5.TabStop = false;
            // 
            // DockSite6
            // 
            this.DockSite6.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            this.DockSite6.Dock = System.Windows.Forms.DockStyle.Right;
            this.DockSite6.Location = new System.Drawing.Point(1047, 29);
            this.DockSite6.Margin = new System.Windows.Forms.Padding(4);
            this.DockSite6.Name = "DockSite6";
            this.DockSite6.Size = new System.Drawing.Size(0, 592);
            this.DockSite6.TabIndex = 15;
            this.DockSite6.TabStop = false;
            // 
            // DockSite7
            // 
            this.DockSite7.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            this.DockSite7.Controls.Add(this.barTool);
            this.DockSite7.Dock = System.Windows.Forms.DockStyle.Top;
            this.DockSite7.Location = new System.Drawing.Point(223, 0);
            this.DockSite7.Margin = new System.Windows.Forms.Padding(4);
            this.DockSite7.Name = "DockSite7";
            this.DockSite7.Size = new System.Drawing.Size(824, 29);
            this.DockSite7.TabIndex = 16;
            this.DockSite7.TabStop = false;
            // 
            // DockSite8
            // 
            this.DockSite8.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            this.DockSite8.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.DockSite8.Location = new System.Drawing.Point(223, 621);
            this.DockSite8.Margin = new System.Windows.Forms.Padding(4);
            this.DockSite8.Name = "DockSite8";
            this.DockSite8.Size = new System.Drawing.Size(824, 0);
            this.DockSite8.TabIndex = 17;
            this.DockSite8.TabStop = false;
            // 
            // DockSite2
            // 
            this.DockSite2.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            this.DockSite2.Dock = System.Windows.Forms.DockStyle.Right;
            this.DockSite2.Location = new System.Drawing.Point(1047, 0);
            this.DockSite2.Margin = new System.Windows.Forms.Padding(4);
            this.DockSite2.Name = "DockSite2";
            this.DockSite2.Size = new System.Drawing.Size(0, 621);
            this.DockSite2.TabIndex = 11;
            this.DockSite2.TabStop = false;
            // 
            // lblCount
            // 
            this.lblCount.Name = "lblCount";
            this.lblCount.Stretch = true;
            this.lblCount.Text = "记录总数：";
            // 
            // DockSite3
            // 
            this.DockSite3.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            this.DockSite3.Dock = System.Windows.Forms.DockStyle.Top;
            this.DockSite3.Location = new System.Drawing.Point(223, 0);
            this.DockSite3.Margin = new System.Windows.Forms.Padding(4);
            this.DockSite3.Name = "DockSite3";
            this.DockSite3.Size = new System.Drawing.Size(824, 0);
            this.DockSite3.TabIndex = 12;
            this.DockSite3.TabStop = false;
            // 
            // barStatus
            // 
            this.barStatus.Controls.Add(this.labelXSelectItems);
            this.barStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barStatus.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.barStatus.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.lblCount,
            this.ProgressBar});
            this.barStatus.Location = new System.Drawing.Point(223, 598);
            this.barStatus.Margin = new System.Windows.Forms.Padding(4);
            this.barStatus.Name = "barStatus";
            this.barStatus.Size = new System.Drawing.Size(824, 23);
            this.barStatus.Stretch = true;
            this.barStatus.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2003;
            this.barStatus.TabIndex = 18;
            this.barStatus.TabStop = false;
            this.barStatus.Text = "Bar1";
            // 
            // labelXSelectItems
            // 
            this.labelXSelectItems.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelXSelectItems.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelXSelectItems.ForeColor = System.Drawing.Color.Black;
            this.labelXSelectItems.Location = new System.Drawing.Point(335, 3);
            this.labelXSelectItems.Name = "labelXSelectItems";
            this.labelXSelectItems.Size = new System.Drawing.Size(389, 23);
            this.labelXSelectItems.TabIndex = 0;
            // 
            // ProgressBar
            // 
            // 
            // 
            // 
            this.ProgressBar.BackStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.ProgressBar.ChunkGradientAngle = 0F;
            this.ProgressBar.MenuVisibility = DevComponents.DotNetBar.eMenuVisibility.VisibleAlways;
            this.ProgressBar.Name = "ProgressBar";
            this.ProgressBar.RecentlyUsed = false;
            // 
            // DockContainerItem1
            // 
            this.DockContainerItem1.Control = this.PanelDockContainer1;
            this.DockContainerItem1.Name = "DockContainerItem1";
            this.DockContainerItem1.Text = "DockContainerItem1";
            // 
            // PanelDockContainer1
            // 
            this.PanelDockContainer1.Controls.Add(this.ContextMenuBar);
            this.PanelDockContainer1.Controls.Add(this.lstFields);
            this.PanelDockContainer1.Location = new System.Drawing.Point(3, 3);
            this.PanelDockContainer1.Margin = new System.Windows.Forms.Padding(4);
            this.PanelDockContainer1.Name = "PanelDockContainer1";
            this.PanelDockContainer1.Size = new System.Drawing.Size(214, 615);
            this.PanelDockContainer1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.PanelDockContainer1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.PanelDockContainer1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground2;
            this.PanelDockContainer1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder;
            this.PanelDockContainer1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.PanelDockContainer1.Style.GradientAngle = 90;
            this.PanelDockContainer1.TabIndex = 0;
            // 
            // ContextMenuBar
            // 
            this.ContextMenuBar.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.ContextMenuBar.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.barSelectType});
            this.ContextMenuBar.Location = new System.Drawing.Point(67, 258);
            this.ContextMenuBar.Margin = new System.Windows.Forms.Padding(4);
            this.ContextMenuBar.Name = "ContextMenuBar";
            this.ContextMenuBar.Size = new System.Drawing.Size(108, 29);
            this.ContextMenuBar.Stretch = true;
            this.ContextMenuBar.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2003;
            this.ContextMenuBar.TabIndex = 1;
            this.ContextMenuBar.TabStop = false;
            this.ContextMenuBar.Text = "ContextMenuBar1";
            // 
            // barSelectType
            // 
            this.barSelectType.AutoExpandOnClick = true;
            this.barSelectType.Name = "barSelectType";
            this.barSelectType.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.biSelectAll,
            this.biUnSelectAll});
            this.barSelectType.Text = "选择方式";
            // 
            // biSelectAll
            // 
            this.biSelectAll.Name = "biSelectAll";
            this.biSelectAll.Text = "全部选择";
            this.biSelectAll.Click += new System.EventHandler(this.biSelectAll_Click);
            // 
            // biUnSelectAll
            // 
            this.biUnSelectAll.Name = "biUnSelectAll";
            this.biUnSelectAll.Text = "全部不选";
            this.biUnSelectAll.Click += new System.EventHandler(this.biUnSelectAll_Click);
            // 
            // lstFields
            // 
            // 
            // 
            // 
            this.lstFields.Border.Class = "ListViewBorder";
            this.lstFields.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lstFields.CheckBoxes = true;
            this.lstFields.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Field});
            this.lstFields.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstFields.Location = new System.Drawing.Point(0, 0);
            this.lstFields.Margin = new System.Windows.Forms.Padding(4);
            this.lstFields.Name = "lstFields";
            this.lstFields.Size = new System.Drawing.Size(214, 615);
            this.lstFields.TabIndex = 0;
            this.lstFields.UseCompatibleStateImageBehavior = false;
            this.lstFields.View = System.Windows.Forms.View.Details;
            this.lstFields.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lstFields_MouseUp);
            // 
            // Field
            // 
            this.Field.Text = "字段列表";
            this.Field.Width = 140;
            // 
            // Bar1
            // 
            this.Bar1.AccessibleDescription = "DotNetBar Bar (Bar1)";
            this.Bar1.AccessibleName = "DotNetBar Bar";
            this.Bar1.AccessibleRole = System.Windows.Forms.AccessibleRole.ToolBar;
            this.Bar1.AutoSyncBarCaption = true;
            this.Bar1.CloseSingleTab = true;
            this.Bar1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Bar1.LayoutType = DevComponents.DotNetBar.eLayoutType.DockContainer;
            this.Bar1.Location = new System.Drawing.Point(0, 0);
            this.Bar1.Margin = new System.Windows.Forms.Padding(4);
            this.Bar1.Name = "Bar1";
            this.Bar1.Size = new System.Drawing.Size(3, 560);
            this.Bar1.Stretch = true;
            this.Bar1.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2003;
            this.Bar1.TabIndex = 0;
            this.Bar1.TabStop = false;
            this.Bar1.Text = "DockContainerItem1";
            // 
            // barFields
            // 
            this.barFields.AccessibleDescription = "DotNetBar Bar (barFields)";
            this.barFields.AccessibleName = "DotNetBar Bar";
            this.barFields.AccessibleRole = System.Windows.Forms.AccessibleRole.ToolBar;
            this.barFields.AutoSyncBarCaption = true;
            this.barFields.CloseSingleTab = true;
            this.barFields.Controls.Add(this.PanelDockContainer1);
            this.barFields.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.barFields.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.DockContainerItem1});
            this.barFields.LayoutType = DevComponents.DotNetBar.eLayoutType.DockContainer;
            this.barFields.Location = new System.Drawing.Point(0, 0);
            this.barFields.Margin = new System.Windows.Forms.Padding(4);
            this.barFields.Name = "barFields";
            this.barFields.Size = new System.Drawing.Size(220, 621);
            this.barFields.Stretch = true;
            this.barFields.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2003;
            this.barFields.TabIndex = 1;
            this.barFields.TabStop = false;
            this.barFields.Text = "DockContainerItem1";
            // 
            // dockSite4
            // 
            this.dockSite4.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            this.dockSite4.Controls.Add(this.Bar1);
            this.dockSite4.Controls.Add(this.barFields);
            this.dockSite4.Dock = System.Windows.Forms.DockStyle.Left;
            this.dockSite4.DocumentDockContainer = new DevComponents.DotNetBar.DocumentDockContainer(new DevComponents.DotNetBar.DocumentBaseContainer[] {
            ((DevComponents.DotNetBar.DocumentBaseContainer)(new DevComponents.DotNetBar.DocumentBarContainer(this.barFields, 220, 621)))}, DevComponents.DotNetBar.eOrientation.Horizontal);
            this.dockSite4.Location = new System.Drawing.Point(0, 0);
            this.dockSite4.Margin = new System.Windows.Forms.Padding(4);
            this.dockSite4.Name = "dockSite4";
            this.dockSite4.Size = new System.Drawing.Size(223, 621);
            this.dockSite4.TabIndex = 10;
            this.dockSite4.TabStop = false;
            // 
            // dockSite9
            // 
            this.dockSite9.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            this.dockSite9.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dockSite9.Location = new System.Drawing.Point(0, 621);
            this.dockSite9.Margin = new System.Windows.Forms.Padding(4);
            this.dockSite9.Name = "dockSite9";
            this.dockSite9.Size = new System.Drawing.Size(1047, 0);
            this.dockSite9.TabIndex = 13;
            this.dockSite9.TabStop = false;
            // 
            // barManager
            // 
            this.barManager.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.F1);
            this.barManager.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlC);
            this.barManager.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlA);
            this.barManager.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlV);
            this.barManager.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlX);
            this.barManager.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlZ);
            this.barManager.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlY);
            this.barManager.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.Del);
            this.barManager.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.Ins);
            this.barManager.BottomDockSite = this.dockSite9;
            this.barManager.EnableFullSizeDock = false;
            this.barManager.LeftDockSite = this.dockSite4;
            this.barManager.ParentForm = this;
            this.barManager.RightDockSite = this.DockSite2;
            this.barManager.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2003;
            this.barManager.ToolbarBottomDockSite = this.DockSite8;
            this.barManager.ToolbarLeftDockSite = this.DockSite5;
            this.barManager.ToolbarRightDockSite = this.DockSite6;
            this.barManager.ToolbarTopDockSite = this.DockSite7;
            this.barManager.TopDockSite = this.DockSite3;
            // 
            // buttonItem1
            // 
            this.buttonItem1.BeginGroup = true;
            this.buttonItem1.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.buttonItem1.Image = ((System.Drawing.Image)(resources.GetObject("buttonItem1.Image")));
            this.buttonItem1.Name = "buttonItem1";
            this.buttonItem1.Text = "隐藏列表框";
            // 
            // FrmAttributeTB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1047, 621);
            this.Controls.Add(this.dgvTable);
            this.Controls.Add(this.barStatus);
            this.Controls.Add(this.DockSite5);
            this.Controls.Add(this.DockSite6);
            this.Controls.Add(this.DockSite7);
            this.Controls.Add(this.DockSite8);
            this.Controls.Add(this.DockSite2);
            this.Controls.Add(this.DockSite3);
            this.Controls.Add(this.dockSite4);
            this.Controls.Add(this.dockSite9);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FrmAttributeTB";
            this.Text = "frmAttributeTB";
            this.Load += new System.EventHandler(this.FrmAttributeTB_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FrmAttributeTB_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTable)).EndInit();
            this.contextMenuStripRightClick.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.barTool)).EndInit();
            this.DockSite7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.barStatus)).EndInit();
            this.barStatus.ResumeLayout(false);
            this.PanelDockContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ContextMenuBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Bar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barFields)).EndInit();
            this.barFields.ResumeLayout(false);
            this.dockSite4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.DockSite DockSite1;
        internal DevComponents.DotNetBar.Controls.DataGridViewX dgvTable;
        internal DevComponents.DotNetBar.Bar barTool;
        internal DevComponents.DotNetBar.ButtonItem biDisplayAll;
        internal DevComponents.DotNetBar.ButtonItem biDisplaySel;
        internal DevComponents.DotNetBar.ButtonItem biExportExcel;
        internal DevComponents.DotNetBar.ButtonItem biHide;

        //internal DevComponents.DotNetBar.ButtonItem biSave;
        internal DevComponents.DotNetBar.DockSite DockSite5;
        internal DevComponents.DotNetBar.DockSite DockSite6;
        internal DevComponents.DotNetBar.DockSite DockSite7;
        internal DevComponents.DotNetBar.DockSite DockSite8;
        internal DevComponents.DotNetBar.DockSite DockSite2;
        internal DevComponents.DotNetBar.LabelItem lblCount;
        internal DevComponents.DotNetBar.DockSite DockSite3;
        internal DevComponents.DotNetBar.Bar barStatus;
        internal DevComponents.DotNetBar.ProgressBarItem ProgressBar;
        internal DevComponents.DotNetBar.DockContainerItem DockContainerItem1;
        internal DevComponents.DotNetBar.PanelDockContainer PanelDockContainer1;
        internal DevComponents.DotNetBar.ContextMenuBar ContextMenuBar;
        internal DevComponents.DotNetBar.ButtonItem barSelectType;
        internal DevComponents.DotNetBar.ButtonItem biSelectAll;
        internal DevComponents.DotNetBar.ButtonItem biUnSelectAll;
        internal DevComponents.DotNetBar.Controls.ListViewEx lstFields;
        internal System.Windows.Forms.ColumnHeader Field;
        internal DevComponents.DotNetBar.Bar Bar1;
        internal DevComponents.DotNetBar.Bar barFields;
        internal DevComponents.DotNetBar.DockSite dockSite4;
        internal DevComponents.DotNetBar.DockSite dockSite9;
        internal DevComponents.DotNetBar.DotNetBarManager barManager;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripRightClick;
        private System.Windows.Forms.ToolStripMenuItem btnRightClickGeocoding;
        internal DevComponents.DotNetBar.ButtonItem buttonItem1;
        private DevComponents.DotNetBar.ButtonItem biSave;
        private DevComponents.DotNetBar.ButtonItem biUnDisplayAll;
        private DevComponents.DotNetBar.LabelX labelXSelectItems;
    }
}