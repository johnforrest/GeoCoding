namespace SelectQuery
{
    partial class FrmSelectSet
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSelectSet));
            this.groGroupBox1 = new System.Windows.Forms.GroupBox();
            this.dgvGridList = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.Is_Checked = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.LayerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnInvertSelect = new DevComponents.DotNetBar.ButtonX();
            this.btnSelectAll = new DevComponents.DotNetBar.ButtonX();
            this.groGroupBox4 = new System.Windows.Forms.GroupBox();
            this.cColorPicker = new DevComponents.DotNetBar.ColorPickerButton();
            this.LabelX3 = new DevComponents.DotNetBar.LabelX();
            this.groGroupBox2 = new System.Windows.Forms.GroupBox();
            this.txtTolerance = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.LabelX1 = new DevComponents.DotNetBar.LabelX();
            this.groGroupBox3 = new System.Windows.Forms.GroupBox();
            this.txtSelectCount = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.LabelX2 = new DevComponents.DotNetBar.LabelX();
            this.lblWarning = new DevComponents.DotNetBar.LabelX();
            this.btnOK = new DevComponents.DotNetBar.ButtonX();
            this.btnCancle = new DevComponents.DotNetBar.ButtonX();
            this.groGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGridList)).BeginInit();
            this.groGroupBox4.SuspendLayout();
            this.groGroupBox2.SuspendLayout();
            this.groGroupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groGroupBox1
            // 
            this.groGroupBox1.Controls.Add(this.dgvGridList);
            this.groGroupBox1.Controls.Add(this.btnInvertSelect);
            this.groGroupBox1.Controls.Add(this.btnSelectAll);
            this.groGroupBox1.Location = new System.Drawing.Point(12, 12);
            this.groGroupBox1.Name = "groGroupBox1";
            this.groGroupBox1.Size = new System.Drawing.Size(360, 232);
            this.groGroupBox1.TabIndex = 1;
            this.groGroupBox1.TabStop = false;
            this.groGroupBox1.Text = "设置可选择的图层";
            // 
            // dgvGridList
            // 
            this.dgvGridList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvGridList.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dgvGridList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvGridList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Is_Checked,
            this.LayerName});
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvGridList.DefaultCellStyle = dataGridViewCellStyle7;
            this.dgvGridList.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dgvGridList.Location = new System.Drawing.Point(6, 22);
            this.dgvGridList.Name = "dgvGridList";
            this.dgvGridList.ReadOnly = true;
            this.dgvGridList.RowTemplate.Height = 23;
            this.dgvGridList.Size = new System.Drawing.Size(348, 164);
            this.dgvGridList.TabIndex = 3;
            // 
            // Is_Checked
            // 
            this.Is_Checked.FillWeight = 42.63959F;
            this.Is_Checked.HeaderText = "设置选择";
            this.Is_Checked.Name = "Is_Checked";
            this.Is_Checked.ReadOnly = true;
            this.Is_Checked.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Is_Checked.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // LayerName
            // 
            this.LayerName.FillWeight = 157.3604F;
            this.LayerName.HeaderText = "图层名";
            this.LayerName.Name = "LayerName";
            this.LayerName.ReadOnly = true;
            // 
            // btnInvertSelect
            // 
            this.btnInvertSelect.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnInvertSelect.Location = new System.Drawing.Point(284, 192);
            this.btnInvertSelect.Name = "btnInvertSelect";
            this.btnInvertSelect.Size = new System.Drawing.Size(60, 25);
            this.btnInvertSelect.TabIndex = 2;
            this.btnInvertSelect.Text = "反选";
            this.btnInvertSelect.Click += new System.EventHandler(this.btnInvertSelect_Click);
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSelectAll.Location = new System.Drawing.Point(214, 192);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(60, 25);
            this.btnSelectAll.TabIndex = 1;
            this.btnSelectAll.Text = "全选";
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // groGroupBox4
            // 
            this.groGroupBox4.Controls.Add(this.cColorPicker);
            this.groGroupBox4.Controls.Add(this.LabelX3);
            this.groGroupBox4.Location = new System.Drawing.Point(12, 250);
            this.groGroupBox4.Name = "groGroupBox4";
            this.groGroupBox4.Size = new System.Drawing.Size(357, 69);
            this.groGroupBox4.TabIndex = 6;
            this.groGroupBox4.TabStop = false;
            this.groGroupBox4.Text = "设置选择的颜色";
            // 
            // cColorPicker
            // 
            this.cColorPicker.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.cColorPicker.Image = ((System.Drawing.Image)(resources.GetObject("cColorPicker.Image")));
            this.cColorPicker.Location = new System.Drawing.Point(200, 22);
            this.cColorPicker.Name = "cColorPicker";
            this.cColorPicker.SelectedColorImageRectangle = new System.Drawing.Rectangle(2, 2, 12, 12);
            this.cColorPicker.Size = new System.Drawing.Size(96, 23);
            this.cColorPicker.TabIndex = 2;
            // 
            // LabelX3
            // 
            this.LabelX3.Location = new System.Drawing.Point(16, 23);
            this.LabelX3.Name = "LabelX3";
            this.LabelX3.Size = new System.Drawing.Size(144, 21);
            this.LabelX3.TabIndex = 0;
            this.LabelX3.Text = "设置当地物选择后的颜色";
            // 
            // groGroupBox2
            // 
            this.groGroupBox2.Controls.Add(this.txtTolerance);
            this.groGroupBox2.Controls.Add(this.LabelX1);
            this.groGroupBox2.Location = new System.Drawing.Point(10, 325);
            this.groGroupBox2.Name = "groGroupBox2";
            this.groGroupBox2.Size = new System.Drawing.Size(359, 62);
            this.groGroupBox2.TabIndex = 7;
            this.groGroupBox2.TabStop = false;
            this.groGroupBox2.Text = "设置容差";
            // 
            // txtTolerance
            // 
            // 
            // 
            // 
            this.txtTolerance.Border.Class = "TextBoxBorder";
            this.txtTolerance.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtTolerance.Location = new System.Drawing.Point(84, 24);
            this.txtTolerance.Name = "txtTolerance";
            this.txtTolerance.Size = new System.Drawing.Size(77, 21);
            this.txtTolerance.TabIndex = 1;
            this.txtTolerance.Text = "3";
            this.txtTolerance.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTolerance_KeyPress);
            // 
            // LabelX1
            // 
            this.LabelX1.Location = new System.Drawing.Point(12, 24);
            this.LabelX1.Name = "LabelX1";
            this.LabelX1.Size = new System.Drawing.Size(69, 25);
            this.LabelX1.TabIndex = 0;
            this.LabelX1.Text = "半径";
            // 
            // groGroupBox3
            // 
            this.groGroupBox3.Controls.Add(this.txtSelectCount);
            this.groGroupBox3.Controls.Add(this.LabelX2);
            this.groGroupBox3.Controls.Add(this.lblWarning);
            this.groGroupBox3.Location = new System.Drawing.Point(12, 393);
            this.groGroupBox3.Name = "groGroupBox3";
            this.groGroupBox3.Size = new System.Drawing.Size(360, 79);
            this.groGroupBox3.TabIndex = 8;
            this.groGroupBox3.TabStop = false;
            this.groGroupBox3.Text = "设置数目";
            // 
            // txtSelectCount
            // 
            // 
            // 
            // 
            this.txtSelectCount.Border.Class = "TextBoxBorder";
            this.txtSelectCount.Location = new System.Drawing.Point(83, 46);
            this.txtSelectCount.Name = "txtSelectCount";
            this.txtSelectCount.Size = new System.Drawing.Size(77, 21);
            this.txtSelectCount.TabIndex = 2;
            this.txtSelectCount.Text = "10";
            // 
            // LabelX2
            // 
            this.LabelX2.Location = new System.Drawing.Point(12, 46);
            this.LabelX2.Name = "LabelX2";
            this.LabelX2.Size = new System.Drawing.Size(58, 24);
            this.LabelX2.TabIndex = 1;
            this.LabelX2.Text = "记录数";
            // 
            // lblWarning
            // 
            this.lblWarning.Location = new System.Drawing.Point(6, 19);
            this.lblWarning.Name = "lblWarning";
            this.lblWarning.Size = new System.Drawing.Size(306, 21);
            this.lblWarning.TabIndex = 0;
            this.lblWarning.Text = "当选择结果超过如下记录时候，弹出警告窗口";
            // 
            // btnOK
            // 
            this.btnOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOK.Location = new System.Drawing.Point(226, 491);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(66, 23);
            this.btnOK.TabIndex = 9;
            this.btnOK.Text = "确定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancle
            // 
            this.btnCancle.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancle.Location = new System.Drawing.Point(306, 491);
            this.btnCancle.Name = "btnCancle";
            this.btnCancle.Size = new System.Drawing.Size(66, 23);
            this.btnCancle.TabIndex = 10;
            this.btnCancle.Text = "取消";
            this.btnCancle.Click += new System.EventHandler(this.btnCancle_Click);
            // 
            // frmSelectSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(381, 526);
            this.Controls.Add(this.btnCancle);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.groGroupBox3);
            this.Controls.Add(this.groGroupBox2);
            this.Controls.Add(this.groGroupBox4);
            this.Controls.Add(this.groGroupBox1);
            this.Name = "frmSelectSet";
            this.Text = "frmSelectSet";
            this.Load += new System.EventHandler(this.frmSelectSet_Load);
            this.groGroupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvGridList)).EndInit();
            this.groGroupBox4.ResumeLayout(false);
            this.groGroupBox2.ResumeLayout(false);
            this.groGroupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.GroupBox groGroupBox1;
        internal DevComponents.DotNetBar.Controls.DataGridViewX dgvGridList;
        internal System.Windows.Forms.DataGridViewCheckBoxColumn Is_Checked;
        internal System.Windows.Forms.DataGridViewTextBoxColumn LayerName;
        internal DevComponents.DotNetBar.ButtonX btnInvertSelect;
        internal DevComponents.DotNetBar.ButtonX btnSelectAll;
        internal System.Windows.Forms.GroupBox groGroupBox4;
        internal DevComponents.DotNetBar.ColorPickerButton cColorPicker;
        internal DevComponents.DotNetBar.LabelX LabelX3;
        internal System.Windows.Forms.GroupBox groGroupBox2;
        internal DevComponents.DotNetBar.Controls.TextBoxX txtTolerance;
        internal DevComponents.DotNetBar.LabelX LabelX1;
        internal System.Windows.Forms.GroupBox groGroupBox3;
        internal DevComponents.DotNetBar.Controls.TextBoxX txtSelectCount;
        internal DevComponents.DotNetBar.LabelX LabelX2;
        internal DevComponents.DotNetBar.LabelX lblWarning;
        internal DevComponents.DotNetBar.ButtonX btnOK;
        internal DevComponents.DotNetBar.ButtonX btnCancle;
    }
}