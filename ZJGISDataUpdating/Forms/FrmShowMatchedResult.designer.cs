namespace ZJGISDataUpdating
{
    partial class FrmShowMatchedResult
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmShowMatchedResult));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.bar3 = new DevComponents.DotNetBar.Bar();
            this.btnAll = new DevComponents.DotNetBar.ButtonItem();
            this.btnMatched = new DevComponents.DotNetBar.ButtonItem();
            this.btnNotMatched = new DevComponents.DotNetBar.ButtonItem();
            this.btnAttribute = new DevComponents.DotNetBar.ButtonItem();
            this.btnShape = new DevComponents.DotNetBar.ButtonItem();
            this.btnShapeAttribute = new DevComponents.DotNetBar.ButtonItem();
            this.btnDifScaleMatched = new DevComponents.DotNetBar.ButtonItem();
            this.btnNew = new DevComponents.DotNetBar.ButtonItem();
            this.btnOneToOne = new DevComponents.DotNetBar.ButtonItem();
            this.btnOneToMore = new DevComponents.DotNetBar.ButtonItem();
            this.btnMoreToOne = new DevComponents.DotNetBar.ButtonItem();
            this.btnMoreToMore = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItemDelRow = new DevComponents.DotNetBar.ButtonItem();
            this.btnSave = new DevComponents.DotNetBar.ButtonItem();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.bindingNavigator1 = new System.Windows.Forms.BindingNavigator(this.components);
            this.bindingNavigatorCountItem = new System.Windows.Forms.ToolStripLabel();
            this.bindingNavigatorMoveLastItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveNextItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorPositionItem = new System.Windows.Forms.ToolStripTextBox();
            this.bindingNavigatorSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorMovePreviousItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveFirstItem = new System.Windows.Forms.ToolStripButton();
            this.dataGridViewX1 = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmMenuSave = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.bar3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).BeginInit();
            this.bindingNavigator1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewX1)).BeginInit();
            this.dataGridViewX1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // bar3
            // 
            this.bar3.AntiAlias = true;
            this.bar3.Dock = System.Windows.Forms.DockStyle.Top;
            this.bar3.Font = new System.Drawing.Font("Microsoft YaHei", 9F);
            this.bar3.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.btnAll,
            this.btnMatched,
            this.btnNotMatched,
            this.btnDifScaleMatched,
            this.buttonItemDelRow,
            this.btnSave});
            this.bar3.Location = new System.Drawing.Point(0, 0);
            this.bar3.Margin = new System.Windows.Forms.Padding(4);
            this.bar3.Name = "bar3";
            this.bar3.Size = new System.Drawing.Size(897, 29);
            this.bar3.Stretch = true;
            this.bar3.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.bar3.TabIndex = 22;
            this.bar3.TabStop = false;
            this.bar3.Text = "bar3";
            // 
            // btnAll
            // 
            this.btnAll.Name = "btnAll";
            this.btnAll.Text = "全部";
            this.btnAll.Click += new System.EventHandler(this.btnAll_Click);
            // 
            // btnMatched
            // 
            this.btnMatched.Name = "btnMatched";
            this.btnMatched.Text = "未变化";
            this.btnMatched.Click += new System.EventHandler(this.btnMatched_Click);
            // 
            // btnNotMatched
            // 
            this.btnNotMatched.Name = "btnNotMatched";
            this.btnNotMatched.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.btnAttribute,
            this.btnShape,
            this.btnShapeAttribute});
            this.btnNotMatched.Text = "变化";
            // 
            // btnAttribute
            // 
            this.btnAttribute.Name = "btnAttribute";
            this.btnAttribute.Text = "属性变化";
            this.btnAttribute.Click += new System.EventHandler(this.btnAttribute_Click);
            // 
            // btnShape
            // 
            this.btnShape.Name = "btnShape";
            this.btnShape.Text = "图形变化";
            this.btnShape.Click += new System.EventHandler(this.btnShape_Click);
            // 
            // btnShapeAttribute
            // 
            this.btnShapeAttribute.Name = "btnShapeAttribute";
            this.btnShapeAttribute.Text = "属性图形变化";
            this.btnShapeAttribute.Click += new System.EventHandler(this.btnShapeAttribute_Click);
            // 
            // btnDifScaleMatched
            // 
            this.btnDifScaleMatched.Name = "btnDifScaleMatched";
            this.btnDifScaleMatched.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.btnNew,
            this.btnOneToOne,
            this.btnOneToMore,
            this.btnMoreToOne,
            this.btnMoreToMore});
            this.btnDifScaleMatched.Text = "跨尺度变化";
            // 
            // btnNew
            // 
            this.btnNew.Name = "btnNew";
            this.btnNew.Text = "一对零";
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnOneToOne
            // 
            this.btnOneToOne.Name = "btnOneToOne";
            this.btnOneToOne.Text = "一对一";
            this.btnOneToOne.Click += new System.EventHandler(this.btnOneToOne_Click);
            // 
            // btnOneToMore
            // 
            this.btnOneToMore.Name = "btnOneToMore";
            this.btnOneToMore.Text = "一对多";
            this.btnOneToMore.Click += new System.EventHandler(this.btnOneToMore_Click);
            // 
            // btnMoreToOne
            // 
            this.btnMoreToOne.Name = "btnMoreToOne";
            this.btnMoreToOne.Text = "多对一";
            this.btnMoreToOne.Click += new System.EventHandler(this.btnMoreToOne_Click);
            // 
            // btnMoreToMore
            // 
            this.btnMoreToMore.Name = "btnMoreToMore";
            this.btnMoreToMore.Text = "多对多";
            this.btnMoreToMore.Click += new System.EventHandler(this.btnMoreToMore_Click);
            // 
            // buttonItemDelRow
            // 
            this.buttonItemDelRow.ItemAlignment = DevComponents.DotNetBar.eItemAlignment.Far;
            this.buttonItemDelRow.Name = "buttonItemDelRow";
            this.buttonItemDelRow.Text = "删除";
            this.buttonItemDelRow.Click += new System.EventHandler(this.buttonItemDelRow_Click);
            // 
            // btnSave
            // 
            this.btnSave.Name = "btnSave";
            this.btnSave.Text = "保存";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // bindingNavigator1
            // 
            this.bindingNavigator1.AddNewItem = null;
            this.bindingNavigator1.CountItem = this.bindingNavigatorCountItem;
            this.bindingNavigator1.DeleteItem = null;
            this.bindingNavigator1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bindingNavigator1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bindingNavigatorMoveLastItem,
            this.bindingNavigatorMoveNextItem,
            this.bindingNavigatorSeparator1,
            this.bindingNavigatorCountItem,
            this.bindingNavigatorPositionItem,
            this.bindingNavigatorSeparator2,
            this.bindingNavigatorMovePreviousItem,
            this.bindingNavigatorMoveFirstItem});
            this.bindingNavigator1.Location = new System.Drawing.Point(0, 419);
            this.bindingNavigator1.MoveFirstItem = this.bindingNavigatorMoveFirstItem;
            this.bindingNavigator1.MoveLastItem = this.bindingNavigatorMoveLastItem;
            this.bindingNavigator1.MoveNextItem = this.bindingNavigatorMoveNextItem;
            this.bindingNavigator1.MovePreviousItem = this.bindingNavigatorMovePreviousItem;
            this.bindingNavigator1.Name = "bindingNavigator1";
            this.bindingNavigator1.PositionItem = this.bindingNavigatorPositionItem;
            this.bindingNavigator1.Size = new System.Drawing.Size(897, 27);
            this.bindingNavigator1.TabIndex = 23;
            this.bindingNavigator1.Text = "bindingNavigator1";
            // 
            // bindingNavigatorCountItem
            // 
            this.bindingNavigatorCountItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.bindingNavigatorCountItem.Name = "bindingNavigatorCountItem";
            this.bindingNavigatorCountItem.Size = new System.Drawing.Size(47, 24);
            this.bindingNavigatorCountItem.Text = "of {0}";
            this.bindingNavigatorCountItem.ToolTipText = "Total number of items";
            // 
            // bindingNavigatorMoveLastItem
            // 
            this.bindingNavigatorMoveLastItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.bindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveLastItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveLastItem.Image")));
            this.bindingNavigatorMoveLastItem.Name = "bindingNavigatorMoveLastItem";
            this.bindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveLastItem.Size = new System.Drawing.Size(23, 24);
            this.bindingNavigatorMoveLastItem.Text = "Move last";
            // 
            // bindingNavigatorMoveNextItem
            // 
            this.bindingNavigatorMoveNextItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.bindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveNextItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveNextItem.Image")));
            this.bindingNavigatorMoveNextItem.Name = "bindingNavigatorMoveNextItem";
            this.bindingNavigatorMoveNextItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveNextItem.Size = new System.Drawing.Size(23, 24);
            this.bindingNavigatorMoveNextItem.Text = "Move next";
            // 
            // bindingNavigatorSeparator1
            // 
            this.bindingNavigatorSeparator1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.bindingNavigatorSeparator1.Name = "bindingNavigatorSeparator1";
            this.bindingNavigatorSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // bindingNavigatorPositionItem
            // 
            this.bindingNavigatorPositionItem.AccessibleName = "Position";
            this.bindingNavigatorPositionItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.bindingNavigatorPositionItem.AutoSize = false;
            this.bindingNavigatorPositionItem.Name = "bindingNavigatorPositionItem";
            this.bindingNavigatorPositionItem.Size = new System.Drawing.Size(65, 27);
            this.bindingNavigatorPositionItem.Text = "0";
            this.bindingNavigatorPositionItem.ToolTipText = "Current position";
            // 
            // bindingNavigatorSeparator2
            // 
            this.bindingNavigatorSeparator2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.bindingNavigatorSeparator2.Name = "bindingNavigatorSeparator2";
            this.bindingNavigatorSeparator2.Size = new System.Drawing.Size(6, 27);
            // 
            // bindingNavigatorMovePreviousItem
            // 
            this.bindingNavigatorMovePreviousItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.bindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMovePreviousItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMovePreviousItem.Image")));
            this.bindingNavigatorMovePreviousItem.Name = "bindingNavigatorMovePreviousItem";
            this.bindingNavigatorMovePreviousItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMovePreviousItem.Size = new System.Drawing.Size(23, 24);
            this.bindingNavigatorMovePreviousItem.Text = "Move previous";
            // 
            // bindingNavigatorMoveFirstItem
            // 
            this.bindingNavigatorMoveFirstItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.bindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveFirstItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveFirstItem.Image")));
            this.bindingNavigatorMoveFirstItem.Name = "bindingNavigatorMoveFirstItem";
            this.bindingNavigatorMoveFirstItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveFirstItem.Size = new System.Drawing.Size(23, 24);
            this.bindingNavigatorMoveFirstItem.Text = "Move first";
            // 
            // dataGridViewX1
            // 
            this.dataGridViewX1.AllowUserToAddRows = false;
            this.dataGridViewX1.AllowUserToOrderColumns = true;
            this.dataGridViewX1.BackgroundColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.dataGridViewX1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridViewX1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewX1.ContextMenuStrip = this.contextMenuStrip1;
            this.dataGridViewX1.Controls.Add(this.comboBox1);
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewX1.DefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewX1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewX1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dataGridViewX1.Location = new System.Drawing.Point(0, 29);
            this.dataGridViewX1.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridViewX1.Name = "dataGridViewX1";
            this.dataGridViewX1.RowHeadersWidth = 20;
            this.dataGridViewX1.RowTemplate.Height = 23;
            this.dataGridViewX1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewX1.Size = new System.Drawing.Size(897, 390);
            this.dataGridViewX1.TabIndex = 25;
            this.dataGridViewX1.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewX1_CellMouseDown);
            this.dataGridViewX1.CurrentCellChanged += new System.EventHandler(this.dataGridViewX1_CurrentCellChanged);
            this.dataGridViewX1.RowHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewX1_RowHeaderMouseClick);
            this.dataGridViewX1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dataGridViewX1_MouseUp);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "一对一",
            "一对多",
            "多对一",
            "多对多",
            "未匹配"});
            this.comboBox1.Location = new System.Drawing.Point(455, 190);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 23);
            this.comboBox1.TabIndex = 26;
            this.comboBox1.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboBox1_DrawItem);
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmMenuSave});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(109, 28);
            // 
            // cmMenuSave
            // 
            this.cmMenuSave.Name = "cmMenuSave";
            this.cmMenuSave.Size = new System.Drawing.Size(108, 24);
            this.cmMenuSave.Text = "保存";
            this.cmMenuSave.Click += new System.EventHandler(this.cmMenuSave_Click);
            // 
            // FrmShowMatchedResult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(897, 446);
            this.Controls.Add(this.dataGridViewX1);
            this.Controls.Add(this.bindingNavigator1);
            this.Controls.Add(this.bar3);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FrmShowMatchedResult";
            this.Load += new System.EventHandler(this.FrmShowMatchedResult_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bar3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).EndInit();
            this.bindingNavigator1.ResumeLayout(false);
            this.bindingNavigator1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewX1)).EndInit();
            this.dataGridViewX1.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevComponents.DotNetBar.Bar bar3;
        private DevComponents.DotNetBar.ButtonItem btnAll;
        private DevComponents.DotNetBar.ButtonItem btnMatched;
        private DevComponents.DotNetBar.ButtonItem btnNotMatched;
        private DevComponents.DotNetBar.ButtonItem btnAttribute;
        private DevComponents.DotNetBar.ButtonItem btnShape;
        private DevComponents.DotNetBar.ButtonItem btnShapeAttribute;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.BindingNavigator bindingNavigator1;
        private System.Windows.Forms.ToolStripLabel bindingNavigatorCountItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveFirstItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMovePreviousItem;
        private System.Windows.Forms.ToolStripTextBox bindingNavigatorPositionItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator1;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveNextItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveLastItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator2;
        private DevComponents.DotNetBar.Controls.DataGridViewX dataGridViewX1;
        private DevComponents.DotNetBar.ButtonItem btnMoreToOne;
        private DevComponents.DotNetBar.ButtonItem btnDifScaleMatched;
        private DevComponents.DotNetBar.ButtonItem btnOneToOne;
        private DevComponents.DotNetBar.ButtonItem btnMoreToMore;
        private DevComponents.DotNetBar.ButtonItem btnNew;
        private DevComponents.DotNetBar.ButtonItem buttonItemDelRow;
        private System.Windows.Forms.ComboBox comboBox1;
        private DevComponents.DotNetBar.ButtonItem btnOneToMore;
        private DevComponents.DotNetBar.ButtonItem btnSave;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem cmMenuSave;
    }
}