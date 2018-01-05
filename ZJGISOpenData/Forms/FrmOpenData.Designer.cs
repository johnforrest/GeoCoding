namespace ZJGISOpenData.Forms
{
    partial class FrmOpenData
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
            this.comboBoxExFileType = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.textBoxXPath = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.buttonXOpen = new DevComponents.DotNetBar.ButtonX();
            this.buttonXCancel = new DevComponents.DotNetBar.ButtonX();
            this.imageListFiles = new System.Windows.Forms.ImageList(this.components);
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.listViewExFiles = new DevComponents.DotNetBar.Controls.ListViewEx();
            this.columnHeaderName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.advTreeFiles = new DevComponents.AdvTree.AdvTree();
            this.elementStyle1 = new DevComponents.DotNetBar.ElementStyle();
            this.buttonXBack = new DevComponents.DotNetBar.ButtonX();
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.imageList3 = new System.Windows.Forms.ImageList(this.components);
            this.panelEx1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.advTreeFiles)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBoxExFileType
            // 
            this.comboBoxExFileType.DisplayMember = "Text";
            this.comboBoxExFileType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboBoxExFileType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxExFileType.FormattingEnabled = true;
            this.comboBoxExFileType.IsStandalone = false;
            this.comboBoxExFileType.ItemHeight = 15;
            this.comboBoxExFileType.Location = new System.Drawing.Point(190, 432);
            this.comboBoxExFileType.Name = "comboBoxExFileType";
            this.comboBoxExFileType.Size = new System.Drawing.Size(336, 21);
            this.comboBoxExFileType.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.comboBoxExFileType.TabIndex = 2;
            this.comboBoxExFileType.SelectedIndexChanged += new System.EventHandler(this.comboBoxExFileType_SelectedIndexChanged);
            // 
            // textBoxXPath
            // 
            // 
            // 
            // 
            this.textBoxXPath.Border.Class = "TextBoxBorder";
            this.textBoxXPath.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.textBoxXPath.Location = new System.Drawing.Point(190, 403);
            this.textBoxXPath.Name = "textBoxXPath";
            this.textBoxXPath.PreventEnterBeep = true;
            this.textBoxXPath.Size = new System.Drawing.Size(336, 25);
            this.textBoxXPath.TabIndex = 3;
            this.textBoxXPath.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxXPath_KeyPress);
            // 
            // labelX1
            // 
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Location = new System.Drawing.Point(117, 403);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(56, 23);
            this.labelX1.TabIndex = 4;
            this.labelX1.Text = "文件名：";
            // 
            // buttonXOpen
            // 
            this.buttonXOpen.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonXOpen.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonXOpen.Location = new System.Drawing.Point(532, 401);
            this.buttonXOpen.Name = "buttonXOpen";
            this.buttonXOpen.Size = new System.Drawing.Size(75, 23);
            this.buttonXOpen.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonXOpen.TabIndex = 5;
            this.buttonXOpen.Text = "打开";
            this.buttonXOpen.Click += new System.EventHandler(this.buttonXOpen_Click);
            // 
            // buttonXCancel
            // 
            this.buttonXCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonXCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonXCancel.Location = new System.Drawing.Point(532, 434);
            this.buttonXCancel.Name = "buttonXCancel";
            this.buttonXCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonXCancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonXCancel.TabIndex = 6;
            this.buttonXCancel.Text = "取消";
            this.buttonXCancel.Click += new System.EventHandler(this.buttonXCancel_Click);
            // 
            // imageListFiles
            // 
            this.imageListFiles.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imageListFiles.ImageSize = new System.Drawing.Size(32, 32);
            this.imageListFiles.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // labelX2
            // 
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Location = new System.Drawing.Point(117, 432);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(67, 23);
            this.labelX2.TabIndex = 8;
            this.labelX2.Text = "文件类型：";
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx1.Controls.Add(this.listViewExFiles);
            this.panelEx1.Controls.Add(this.splitter1);
            this.panelEx1.Controls.Add(this.advTreeFiles);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelEx1.Location = new System.Drawing.Point(0, 0);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(653, 395);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 9;
            this.panelEx1.Text = "panelEx1";
            // 
            // listViewExFiles
            // 
            this.listViewExFiles.AccessibleRole = System.Windows.Forms.AccessibleRole.ListItem;
            this.listViewExFiles.Activation = System.Windows.Forms.ItemActivation.OneClick;
            // 
            // 
            // 
            this.listViewExFiles.Border.Class = "ListViewBorder";
            this.listViewExFiles.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.listViewExFiles.Border.Name = "test";
            this.listViewExFiles.Border.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemCheckedBackground2;
            this.listViewExFiles.Border.WordWrap = true;
            this.listViewExFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderName});
            this.listViewExFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewExFiles.Font = new System.Drawing.Font("Microsoft YaHei", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.listViewExFiles.FullRowSelect = true;
            this.listViewExFiles.LargeImageList = this.imageListFiles;
            this.listViewExFiles.Location = new System.Drawing.Point(186, 0);
            this.listViewExFiles.Name = "listViewExFiles";
            this.listViewExFiles.ShowItemToolTips = true;
            this.listViewExFiles.Size = new System.Drawing.Size(467, 395);
            this.listViewExFiles.SmallImageList = this.imageListFiles;
            this.listViewExFiles.TabIndex = 10;
            this.listViewExFiles.TileSize = new System.Drawing.Size(100, 100);
            this.listViewExFiles.UseCompatibleStateImageBehavior = false;
            this.listViewExFiles.DoubleClick += new System.EventHandler(this.listViewExFiles_DoubleClick);
            this.listViewExFiles.MouseUp += new System.Windows.Forms.MouseEventHandler(this.listViewExFiles_MouseUp);
            // 
            // columnHeaderName
            // 
            this.columnHeaderName.Width = 96;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(183, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 395);
            this.splitter1.TabIndex = 9;
            this.splitter1.TabStop = false;
            // 
            // advTreeFiles
            // 
            this.advTreeFiles.AllowDrop = true;
            this.advTreeFiles.BackColor = System.Drawing.SystemColors.Window;
            // 
            // 
            // 
            this.advTreeFiles.BackgroundStyle.Class = "TreeBorderKey";
            this.advTreeFiles.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.advTreeFiles.Dock = System.Windows.Forms.DockStyle.Left;
            this.advTreeFiles.DragDropEnabled = false;
            this.advTreeFiles.DragDropNodeCopyEnabled = false;
            this.advTreeFiles.ExpandButtonType = DevComponents.AdvTree.eExpandButtonType.Triangle;
            this.advTreeFiles.ExpandWidth = 14;
            this.advTreeFiles.GroupNodeStyle = this.elementStyle1;
            this.advTreeFiles.HotTracking = true;
            this.advTreeFiles.Location = new System.Drawing.Point(0, 0);
            this.advTreeFiles.Name = "advTreeFiles";
            this.advTreeFiles.NodeStyle = this.elementStyle1;
            this.advTreeFiles.PathSeparator = ";";
            this.advTreeFiles.Size = new System.Drawing.Size(183, 395);
            this.advTreeFiles.Styles.Add(this.elementStyle1);
            this.advTreeFiles.TabIndex = 8;
            this.advTreeFiles.BeforeExpand += new DevComponents.AdvTree.AdvTreeNodeCancelEventHandler(this.advTreeFiles_BeforeExpand);
            this.advTreeFiles.NodeClick += new DevComponents.AdvTree.TreeNodeMouseEventHandler(this.advTreeFiles_NodeClick);
            // 
            // elementStyle1
            // 
            this.elementStyle1.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.elementStyle1.Name = "elementStyle1";
            this.elementStyle1.TextColor = System.Drawing.SystemColors.ControlText;
            // 
            // buttonXBack
            // 
            this.buttonXBack.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonXBack.BackColor = System.Drawing.Color.Transparent;
            this.buttonXBack.ColorTable = DevComponents.DotNetBar.eButtonColor.BlueOrb;
            this.buttonXBack.FocusCuesEnabled = false;
            this.buttonXBack.Image = global::ZJGISOpenData.Properties.Resources.btnreturn;
            this.buttonXBack.Location = new System.Drawing.Point(12, 406);
            this.buttonXBack.Name = "buttonXBack";
            this.buttonXBack.Shape = new DevComponents.DotNetBar.EllipticalShapeDescriptor();
            this.buttonXBack.Size = new System.Drawing.Size(44, 42);
            this.buttonXBack.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonXBack.TabIndex = 10;
            this.buttonXBack.Tooltip = "返回上一层";
            this.buttonXBack.Click += new System.EventHandler(this.buttonXBack_Click);
            // 
            // imageList2
            // 
            this.imageList2.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList2.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // imageList3
            // 
            this.imageList3.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList3.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList3.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // FrmOpenData
            // 
            this.ClientSize = new System.Drawing.Size(653, 466);
            this.Controls.Add(this.buttonXBack);
            this.Controls.Add(this.panelEx1);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.buttonXCancel);
            this.Controls.Add(this.buttonXOpen);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.textBoxXPath);
            this.Controls.Add(this.comboBoxExFileType);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "FrmOpenData";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "打开图层";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmOpenData_FormClosed);
            this.Load += new System.EventHandler(this.FrmOpenData_Load);
            this.panelEx1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.advTreeFiles)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.TextBoxX textBoxXPath;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.ButtonX buttonXOpen;
        private DevComponents.DotNetBar.ButtonX buttonXCancel;
        private System.Windows.Forms.ImageList imageListFiles;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.PanelEx panelEx1;
        internal DevComponents.DotNetBar.Controls.ListViewEx listViewExFiles;
        private System.Windows.Forms.ColumnHeader columnHeaderName;
        private System.Windows.Forms.Splitter splitter1;
        private DevComponents.AdvTree.AdvTree advTreeFiles;
        private DevComponents.DotNetBar.ElementStyle elementStyle1;
        private DevComponents.DotNetBar.ButtonX buttonXBack;
        private DevComponents.DotNetBar.Controls.ComboBoxEx comboBoxExFileType;
        private System.Windows.Forms.ImageList imageList2;
        private System.Windows.Forms.ImageList imageList3;
    }
}