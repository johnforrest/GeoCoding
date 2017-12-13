namespace ZJGISFinishTool
{
    partial class FrmNewProj
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmNewProj));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.ControlContainerItem1 = new DevComponents.DotNetBar.ControlContainerItem();
            this.cmbFileStyle = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.btOpenBrowser = new DevComponents.DotNetBar.ButtonX();
            this.btDelete = new DevComponents.DotNetBar.ButtonX();
            this.btSaveBrowser = new DevComponents.DotNetBar.ButtonX();
            this.btBrowser = new DevComponents.DotNetBar.ButtonX();
            this.txtSaveFileRoute = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtPreDefinedPrj = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.LabelX4 = new DevComponents.DotNetBar.LabelX();
            this.LabelX3 = new DevComponents.DotNetBar.LabelX();
            this.LabelX2 = new DevComponents.DotNetBar.LabelX();
            this.colCoordinateSys = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFeatClsPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SaveFile = new System.Windows.Forms.SaveFileDialog();
            this.dgvData = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.GroupPanel2 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.ExpandableSplitter = new DevComponents.DotNetBar.ExpandableSplitter();
            this.GroupPanel3 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.ControlContainerItem3 = new DevComponents.DotNetBar.ControlContainerItem();
            this.ControlContainerItem4 = new DevComponents.DotNetBar.ControlContainerItem();
            this.ControlContainerItem2 = new DevComponents.DotNetBar.ControlContainerItem();
            this.lblInfo = new DevComponents.DotNetBar.LabelX();
            this.ProgressBar = new DevComponents.DotNetBar.Controls.ProgressBarX();
            this.GroupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.btQuit = new DevComponents.DotNetBar.ButtonX();
            this.btStart = new DevComponents.DotNetBar.ButtonX();
            this.FolderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.GroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).BeginInit();
            this.GroupPanel2.SuspendLayout();
            this.GroupPanel3.SuspendLayout();
            this.GroupPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ControlContainerItem1
            // 
            this.ControlContainerItem1.AllowItemResize = false;
            this.ControlContainerItem1.MenuVisibility = DevComponents.DotNetBar.eMenuVisibility.VisibleAlways;
            this.ControlContainerItem1.Name = "ControlContainerItem1";
            // 
            // cmbFileStyle
            // 
            this.cmbFileStyle.DisplayMember = "Text";
            this.cmbFileStyle.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbFileStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFileStyle.FormattingEnabled = true;
            this.cmbFileStyle.ItemHeight = 17;
            this.cmbFileStyle.Location = new System.Drawing.Point(6, 108);
            this.cmbFileStyle.Name = "cmbFileStyle";
            this.cmbFileStyle.Size = new System.Drawing.Size(134, 23);
            this.cmbFileStyle.TabIndex = 0;
            this.cmbFileStyle.TextChanged += new System.EventHandler(this.cmbFileStyle_TextChanged);
            // 
            // GroupBox1
            // 
            this.GroupBox1.Controls.Add(this.btOpenBrowser);
            this.GroupBox1.Controls.Add(this.cmbFileStyle);
            this.GroupBox1.Controls.Add(this.btDelete);
            this.GroupBox1.Controls.Add(this.btSaveBrowser);
            this.GroupBox1.Controls.Add(this.btBrowser);
            this.GroupBox1.Controls.Add(this.txtSaveFileRoute);
            this.GroupBox1.Controls.Add(this.txtPreDefinedPrj);
            this.GroupBox1.Controls.Add(this.LabelX4);
            this.GroupBox1.Controls.Add(this.LabelX3);
            this.GroupBox1.Controls.Add(this.LabelX2);
            this.GroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GroupBox1.Location = new System.Drawing.Point(0, 0);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Size = new System.Drawing.Size(218, 303);
            this.GroupBox1.TabIndex = 0;
            this.GroupBox1.TabStop = false;
            this.GroupBox1.Text = "设置";
            // 
            // btOpenBrowser
            // 
            this.btOpenBrowser.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btOpenBrowser.Image = global::ZJGISFinishTool.Properties.Resources.btOpenBrowser;
            this.btOpenBrowser.Location = new System.Drawing.Point(28, 34);
            this.btOpenBrowser.Name = "btOpenBrowser";
            this.btOpenBrowser.Size = new System.Drawing.Size(75, 23);
            this.btOpenBrowser.TabIndex = 0;
            this.btOpenBrowser.Text = "添加";
            this.btOpenBrowser.Click += new System.EventHandler(this.btOpenBrowser_Click);
            // 
            // btDelete
            // 
            this.btDelete.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btDelete.Image = global::ZJGISFinishTool.Properties.Resources.btDelete;
            this.btDelete.Location = new System.Drawing.Point(119, 34);
            this.btDelete.Name = "btDelete";
            this.btDelete.Size = new System.Drawing.Size(75, 23);
            this.btDelete.TabIndex = 1;
            this.btDelete.Text = "删除";
            this.btDelete.Click += new System.EventHandler(this.btDelete_Click);
            // 
            // btSaveBrowser
            // 
            this.btSaveBrowser.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btSaveBrowser.Image = ((System.Drawing.Image)(resources.GetObject("btSaveBrowser.Image")));
            this.btSaveBrowser.Location = new System.Drawing.Point(146, 243);
            this.btSaveBrowser.Name = "btSaveBrowser";
            this.btSaveBrowser.Size = new System.Drawing.Size(64, 23);
            this.btSaveBrowser.TabIndex = 8;
            this.btSaveBrowser.Text = "浏览";
            this.btSaveBrowser.Click += new System.EventHandler(this.btSaveBrowser_Click);
            // 
            // btBrowser
            // 
            this.btBrowser.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btBrowser.Image = ((System.Drawing.Image)(resources.GetObject("btBrowser.Image")));
            this.btBrowser.Location = new System.Drawing.Point(146, 176);
            this.btBrowser.Name = "btBrowser";
            this.btBrowser.Size = new System.Drawing.Size(64, 23);
            this.btBrowser.TabIndex = 7;
            this.btBrowser.Text = "浏览";
            this.btBrowser.Click += new System.EventHandler(this.btBrowser_Click);
            // 
            // txtSaveFileRoute
            // 
            // 
            // 
            // 
            this.txtSaveFileRoute.Border.Class = "TextBoxBorder";
            this.txtSaveFileRoute.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtSaveFileRoute.Enabled = false;
            this.txtSaveFileRoute.Location = new System.Drawing.Point(6, 243);
            this.txtSaveFileRoute.Name = "txtSaveFileRoute";
            this.txtSaveFileRoute.Size = new System.Drawing.Size(134, 21);
            this.txtSaveFileRoute.TabIndex = 5;
            // 
            // txtPreDefinedPrj
            // 
            // 
            // 
            // 
            this.txtPreDefinedPrj.Border.Class = "TextBoxBorder";
            this.txtPreDefinedPrj.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtPreDefinedPrj.Enabled = false;
            this.txtPreDefinedPrj.Location = new System.Drawing.Point(6, 176);
            this.txtPreDefinedPrj.Name = "txtPreDefinedPrj";
            this.txtPreDefinedPrj.Size = new System.Drawing.Size(134, 21);
            this.txtPreDefinedPrj.TabIndex = 4;
            // 
            // LabelX4
            // 
            // 
            // 
            // 
            this.LabelX4.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.LabelX4.Location = new System.Drawing.Point(6, 214);
            this.LabelX4.Name = "LabelX4";
            this.LabelX4.Size = new System.Drawing.Size(75, 23);
            this.LabelX4.TabIndex = 2;
            this.LabelX4.Text = "输出路径";
            // 
            // LabelX3
            // 
            // 
            // 
            // 
            this.LabelX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.LabelX3.Location = new System.Drawing.Point(6, 147);
            this.LabelX3.Name = "LabelX3";
            this.LabelX3.Size = new System.Drawing.Size(111, 23);
            this.LabelX3.TabIndex = 1;
            this.LabelX3.Text = "坐标文件(.prj)";
            // 
            // LabelX2
            // 
            // 
            // 
            // 
            this.LabelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.LabelX2.Location = new System.Drawing.Point(6, 79);
            this.LabelX2.Name = "LabelX2";
            this.LabelX2.Size = new System.Drawing.Size(75, 23);
            this.LabelX2.TabIndex = 0;
            this.LabelX2.Text = "输出格式";
            // 
            // colCoordinateSys
            // 
            this.colCoordinateSys.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colCoordinateSys.HeaderText = "坐标系";
            this.colCoordinateSys.Name = "colCoordinateSys";
            this.colCoordinateSys.ReadOnly = true;
            // 
            // colFeatClsPath
            // 
            this.colFeatClsPath.HeaderText = "要素类路径";
            this.colFeatClsPath.Name = "colFeatClsPath";
            this.colFeatClsPath.ReadOnly = true;
            this.colFeatClsPath.Width = 140;
            // 
            // colNum
            // 
            this.colNum.HeaderText = "记录数";
            this.colNum.Name = "colNum";
            this.colNum.ReadOnly = true;
            this.colNum.Width = 74;
            // 
            // dgvData
            // 
            this.dgvData.AllowUserToAddRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colNum,
            this.colFeatClsPath,
            this.colCoordinateSys});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvData.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvData.EnableHeadersVisualStyles = false;
            this.dgvData.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dgvData.Location = new System.Drawing.Point(0, 0);
            this.dgvData.Name = "dgvData";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvData.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvData.RowTemplate.Height = 23;
            this.dgvData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvData.Size = new System.Drawing.Size(435, 309);
            this.dgvData.TabIndex = 2;
            // 
            // GroupPanel2
            // 
            this.GroupPanel2.CanvasColor = System.Drawing.SystemColors.Control;
            this.GroupPanel2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.GroupPanel2.Controls.Add(this.dgvData);
            this.GroupPanel2.Controls.Add(this.ExpandableSplitter);
            this.GroupPanel2.Controls.Add(this.GroupPanel3);
            this.GroupPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GroupPanel2.Location = new System.Drawing.Point(0, 0);
            this.GroupPanel2.Name = "GroupPanel2";
            this.GroupPanel2.Size = new System.Drawing.Size(668, 315);
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
            // 
            // 
            // 
            this.GroupPanel2.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.GroupPanel2.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.GroupPanel2.TabIndex = 8;
            // 
            // ExpandableSplitter
            // 
            this.ExpandableSplitter.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(45)))), ((int)(((byte)(150)))));
            this.ExpandableSplitter.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.ExpandableSplitter.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.ExpandableSplitter.Dock = System.Windows.Forms.DockStyle.Right;
            this.ExpandableSplitter.ExpandFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(45)))), ((int)(((byte)(150)))));
            this.ExpandableSplitter.ExpandFillColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.ExpandableSplitter.ExpandLineColor = System.Drawing.SystemColors.ControlText;
            this.ExpandableSplitter.ExpandLineColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.ExpandableSplitter.GripDarkColor = System.Drawing.SystemColors.ControlText;
            this.ExpandableSplitter.GripDarkColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.ExpandableSplitter.GripLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(237)))), ((int)(((byte)(254)))));
            this.ExpandableSplitter.GripLightColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.ExpandableSplitter.HotBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(142)))), ((int)(((byte)(75)))));
            this.ExpandableSplitter.HotBackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(207)))), ((int)(((byte)(139)))));
            this.ExpandableSplitter.HotBackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground2;
            this.ExpandableSplitter.HotBackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground;
            this.ExpandableSplitter.HotExpandFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(45)))), ((int)(((byte)(150)))));
            this.ExpandableSplitter.HotExpandFillColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.ExpandableSplitter.HotExpandLineColor = System.Drawing.SystemColors.ControlText;
            this.ExpandableSplitter.HotExpandLineColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.ExpandableSplitter.HotGripDarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(45)))), ((int)(((byte)(150)))));
            this.ExpandableSplitter.HotGripDarkColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.ExpandableSplitter.HotGripLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(237)))), ((int)(((byte)(254)))));
            this.ExpandableSplitter.HotGripLightColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.ExpandableSplitter.Location = new System.Drawing.Point(435, 0);
            this.ExpandableSplitter.Name = "ExpandableSplitter";
            this.ExpandableSplitter.Size = new System.Drawing.Size(3, 309);
            this.ExpandableSplitter.TabIndex = 1;
            this.ExpandableSplitter.TabStop = false;
            // 
            // GroupPanel3
            // 
            this.GroupPanel3.CanvasColor = System.Drawing.SystemColors.Control;
            this.GroupPanel3.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.GroupPanel3.Controls.Add(this.GroupBox1);
            this.GroupPanel3.Dock = System.Windows.Forms.DockStyle.Right;
            this.GroupPanel3.Location = new System.Drawing.Point(438, 0);
            this.GroupPanel3.Name = "GroupPanel3";
            this.GroupPanel3.Size = new System.Drawing.Size(224, 309);
            // 
            // 
            // 
            this.GroupPanel3.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.GroupPanel3.Style.BackColorGradientAngle = 90;
            this.GroupPanel3.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.GroupPanel3.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.GroupPanel3.Style.BorderBottomWidth = 1;
            this.GroupPanel3.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.GroupPanel3.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.GroupPanel3.Style.BorderLeftWidth = 1;
            this.GroupPanel3.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.GroupPanel3.Style.BorderRightWidth = 1;
            this.GroupPanel3.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.GroupPanel3.Style.BorderTopWidth = 1;
            this.GroupPanel3.Style.CornerDiameter = 4;
            this.GroupPanel3.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.GroupPanel3.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.GroupPanel3.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.GroupPanel3.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            // 
            // 
            // 
            this.GroupPanel3.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.GroupPanel3.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.GroupPanel3.TabIndex = 0;
            // 
            // ControlContainerItem3
            // 
            this.ControlContainerItem3.AllowItemResize = false;
            this.ControlContainerItem3.MenuVisibility = DevComponents.DotNetBar.eMenuVisibility.VisibleAlways;
            this.ControlContainerItem3.Name = "ControlContainerItem3";
            // 
            // ControlContainerItem4
            // 
            this.ControlContainerItem4.AllowItemResize = false;
            this.ControlContainerItem4.MenuVisibility = DevComponents.DotNetBar.eMenuVisibility.VisibleAlways;
            this.ControlContainerItem4.Name = "ControlContainerItem4";
            // 
            // ControlContainerItem2
            // 
            this.ControlContainerItem2.AllowItemResize = false;
            this.ControlContainerItem2.MenuVisibility = DevComponents.DotNetBar.eMenuVisibility.VisibleAlways;
            this.ControlContainerItem2.Name = "ControlContainerItem2";
            // 
            // lblInfo
            // 
            this.lblInfo.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblInfo.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblInfo.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblInfo.Location = new System.Drawing.Point(0, 0);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(491, 24);
            this.lblInfo.TabIndex = 5;
            this.lblInfo.WordWrap = true;
            // 
            // ProgressBar
            // 
            // 
            // 
            // 
            this.ProgressBar.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.ProgressBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ProgressBar.Location = new System.Drawing.Point(0, 315);
            this.ProgressBar.Name = "ProgressBar";
            this.ProgressBar.Size = new System.Drawing.Size(668, 10);
            this.ProgressBar.TabIndex = 7;
            this.ProgressBar.Text = "ProgressBar";
            // 
            // GroupPanel1
            // 
            this.GroupPanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.GroupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.GroupPanel1.Controls.Add(this.btQuit);
            this.GroupPanel1.Controls.Add(this.btStart);
            this.GroupPanel1.Controls.Add(this.lblInfo);
            this.GroupPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.GroupPanel1.Location = new System.Drawing.Point(0, 325);
            this.GroupPanel1.Name = "GroupPanel1";
            this.GroupPanel1.Size = new System.Drawing.Size(668, 30);
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
            // 
            // 
            // 
            this.GroupPanel1.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.GroupPanel1.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.GroupPanel1.TabIndex = 6;
            // 
            // btQuit
            // 
            this.btQuit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btQuit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btQuit.Location = new System.Drawing.Point(587, 0);
            this.btQuit.Name = "btQuit";
            this.btQuit.Size = new System.Drawing.Size(75, 24);
            this.btQuit.TabIndex = 7;
            this.btQuit.Text = "退出";
            this.btQuit.Click += new System.EventHandler(this.btQuit_Click);
            // 
            // btStart
            // 
            this.btStart.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btStart.Dock = System.Windows.Forms.DockStyle.Left;
            this.btStart.Location = new System.Drawing.Point(491, 0);
            this.btStart.Name = "btStart";
            this.btStart.Size = new System.Drawing.Size(75, 24);
            this.btStart.TabIndex = 6;
            this.btStart.Text = "确定";
            this.btStart.Click += new System.EventHandler(this.btStart_Click);
            // 
            // FrmNewProj
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(668, 355);
            this.Controls.Add(this.GroupPanel2);
            this.Controls.Add(this.ProgressBar);
            this.Controls.Add(this.GroupPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "FrmNewProj";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "投影转换";
            this.Load += new System.EventHandler(this.FrmNewProj_Load);
            this.GroupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).EndInit();
            this.GroupPanel2.ResumeLayout(false);
            this.GroupPanel3.ResumeLayout(false);
            this.GroupPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        internal DevComponents.DotNetBar.ControlContainerItem ControlContainerItem1;
        internal DevComponents.DotNetBar.Controls.ComboBoxEx cmbFileStyle;
        internal System.Windows.Forms.GroupBox GroupBox1;
        internal DevComponents.DotNetBar.ButtonX btSaveBrowser;
        internal DevComponents.DotNetBar.ButtonX btBrowser;
        internal DevComponents.DotNetBar.Controls.TextBoxX txtSaveFileRoute;
        internal DevComponents.DotNetBar.Controls.TextBoxX txtPreDefinedPrj;
        internal DevComponents.DotNetBar.LabelX LabelX4;
        internal DevComponents.DotNetBar.LabelX LabelX3;
        internal DevComponents.DotNetBar.LabelX LabelX2;
        internal System.Windows.Forms.DataGridViewTextBoxColumn colCoordinateSys;
        internal System.Windows.Forms.DataGridViewTextBoxColumn colFeatClsPath;
        internal System.Windows.Forms.DataGridViewTextBoxColumn colNum;
        internal System.Windows.Forms.SaveFileDialog SaveFile;
        internal DevComponents.DotNetBar.Controls.DataGridViewX dgvData;
        internal DevComponents.DotNetBar.ButtonX btOpenBrowser;
        internal DevComponents.DotNetBar.Controls.GroupPanel GroupPanel2;
        internal DevComponents.DotNetBar.ExpandableSplitter ExpandableSplitter;
        internal DevComponents.DotNetBar.Controls.GroupPanel GroupPanel3;
        internal DevComponents.DotNetBar.ButtonX btDelete;
        internal DevComponents.DotNetBar.ControlContainerItem ControlContainerItem3;
        internal DevComponents.DotNetBar.ControlContainerItem ControlContainerItem4;
        internal DevComponents.DotNetBar.ControlContainerItem ControlContainerItem2;
        internal DevComponents.DotNetBar.LabelX lblInfo;
        internal DevComponents.DotNetBar.Controls.ProgressBarX ProgressBar;
        internal DevComponents.DotNetBar.Controls.GroupPanel GroupPanel1;
        internal DevComponents.DotNetBar.ButtonX btQuit;
        internal DevComponents.DotNetBar.ButtonX btStart;
        internal System.Windows.Forms.FolderBrowserDialog FolderBrowser;
    }
}