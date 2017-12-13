namespace SelectQuery
{
    partial class FrmQueryByEntiTable
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
            this.groGroupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonXOpenEntiTalbe = new DevComponents.DotNetBar.ButtonX();
            this.cboLayerList = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.lblLayer = new DevComponents.DotNetBar.LabelX();
            this.groGroupBox2 = new System.Windows.Forms.GroupBox();
            this.lstField = new System.Windows.Forms.ListBox();
            this.groGroupBox3 = new System.Windows.Forms.GroupBox();
            this.btnOperate1 = new DevComponents.DotNetBar.ButtonX();
            this.groGroupBox4 = new System.Windows.Forms.GroupBox();
            this.btnValue = new DevComponents.DotNetBar.ButtonX();
            this.lstValue = new System.Windows.Forms.ListBox();
            this.groGroupBox5 = new System.Windows.Forms.GroupBox();
            this.btnVerify = new DevComponents.DotNetBar.ButtonX();
            this.txtExpress = new System.Windows.Forms.RichTextBox();
            this.btnOK = new DevComponents.DotNetBar.ButtonX();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.btnClear = new DevComponents.DotNetBar.ButtonX();
            this.btnApply = new DevComponents.DotNetBar.ButtonX();
            this.cboSelectMethod = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.lblMethod = new DevComponents.DotNetBar.LabelX();
            this.groGroupBox1.SuspendLayout();
            this.groGroupBox2.SuspendLayout();
            this.groGroupBox3.SuspendLayout();
            this.groGroupBox4.SuspendLayout();
            this.groGroupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // groGroupBox1
            // 
            this.groGroupBox1.Controls.Add(this.cboSelectMethod);
            this.groGroupBox1.Controls.Add(this.lblMethod);
            this.groGroupBox1.Controls.Add(this.buttonXOpenEntiTalbe);
            this.groGroupBox1.Controls.Add(this.cboLayerList);
            this.groGroupBox1.Controls.Add(this.lblLayer);
            this.groGroupBox1.Location = new System.Drawing.Point(3, 2);
            this.groGroupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groGroupBox1.Name = "groGroupBox1";
            this.groGroupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groGroupBox1.Size = new System.Drawing.Size(556, 108);
            this.groGroupBox1.TabIndex = 1;
            this.groGroupBox1.TabStop = false;
            // 
            // buttonXOpenEntiTalbe
            // 
            this.buttonXOpenEntiTalbe.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonXOpenEntiTalbe.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonXOpenEntiTalbe.Location = new System.Drawing.Point(518, 25);
            this.buttonXOpenEntiTalbe.Name = "buttonXOpenEntiTalbe";
            this.buttonXOpenEntiTalbe.Size = new System.Drawing.Size(41, 23);
            this.buttonXOpenEntiTalbe.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonXOpenEntiTalbe.TabIndex = 4;
            this.buttonXOpenEntiTalbe.Text = "...";
            this.buttonXOpenEntiTalbe.Click += new System.EventHandler(this.buttonXOpenEntiTalbe_Click);
            // 
            // cboLayerList
            // 
            this.cboLayerList.DisplayMember = "Text";
            this.cboLayerList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboLayerList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple;
            this.cboLayerList.FormattingEnabled = true;
            this.cboLayerList.Location = new System.Drawing.Point(155, 20);
            this.cboLayerList.Margin = new System.Windows.Forms.Padding(4);
            this.cboLayerList.Name = "cboLayerList";
            this.cboLayerList.Size = new System.Drawing.Size(355, 32);
            this.cboLayerList.TabIndex = 2;
            this.cboLayerList.SelectedIndexChanged += new System.EventHandler(this.cboLayerList_SelectedIndexChanged);
            // 
            // lblLayer
            // 
            this.lblLayer.AutoSize = true;
            // 
            // 
            // 
            this.lblLayer.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblLayer.Location = new System.Drawing.Point(16, 26);
            this.lblLayer.Margin = new System.Windows.Forms.Padding(4);
            this.lblLayer.Name = "lblLayer";
            this.lblLayer.Size = new System.Drawing.Size(77, 20);
            this.lblLayer.TabIndex = 0;
            this.lblLayer.Text = "选择实体表";
            // 
            // groGroupBox2
            // 
            this.groGroupBox2.Controls.Add(this.lstField);
            this.groGroupBox2.Location = new System.Drawing.Point(3, 118);
            this.groGroupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groGroupBox2.Name = "groGroupBox2";
            this.groGroupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groGroupBox2.Size = new System.Drawing.Size(197, 249);
            this.groGroupBox2.TabIndex = 2;
            this.groGroupBox2.TabStop = false;
            this.groGroupBox2.Text = "字段";
            // 
            // lstField
            // 
            this.lstField.FormattingEnabled = true;
            this.lstField.ItemHeight = 15;
            this.lstField.Location = new System.Drawing.Point(12, 22);
            this.lstField.Margin = new System.Windows.Forms.Padding(4);
            this.lstField.Name = "lstField";
            this.lstField.Size = new System.Drawing.Size(176, 214);
            this.lstField.TabIndex = 0;
            this.lstField.SelectedIndexChanged += new System.EventHandler(this.lstField_SelectedIndexChanged);
            this.lstField.DoubleClick += new System.EventHandler(this.lstField_DoubleClick);
            // 
            // groGroupBox3
            // 
            this.groGroupBox3.Controls.Add(this.btnOperate1);
            this.groGroupBox3.Location = new System.Drawing.Point(208, 129);
            this.groGroupBox3.Margin = new System.Windows.Forms.Padding(4);
            this.groGroupBox3.Name = "groGroupBox3";
            this.groGroupBox3.Padding = new System.Windows.Forms.Padding(4);
            this.groGroupBox3.Size = new System.Drawing.Size(165, 249);
            this.groGroupBox3.TabIndex = 3;
            this.groGroupBox3.TabStop = false;
            this.groGroupBox3.Text = "操作符";
            // 
            // btnOperate1
            // 
            this.btnOperate1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOperate1.Location = new System.Drawing.Point(8, 36);
            this.btnOperate1.Margin = new System.Windows.Forms.Padding(4);
            this.btnOperate1.Name = "btnOperate1";
            this.btnOperate1.Size = new System.Drawing.Size(48, 28);
            this.btnOperate1.TabIndex = 0;
            this.btnOperate1.Text = "=";
            this.btnOperate1.Click += new System.EventHandler(this.btnOperate1_Click);
            // 
            // groGroupBox4
            // 
            this.groGroupBox4.Controls.Add(this.btnValue);
            this.groGroupBox4.Controls.Add(this.lstValue);
            this.groGroupBox4.Location = new System.Drawing.Point(381, 129);
            this.groGroupBox4.Margin = new System.Windows.Forms.Padding(4);
            this.groGroupBox4.Name = "groGroupBox4";
            this.groGroupBox4.Padding = new System.Windows.Forms.Padding(4);
            this.groGroupBox4.Size = new System.Drawing.Size(177, 249);
            this.groGroupBox4.TabIndex = 4;
            this.groGroupBox4.TabStop = false;
            this.groGroupBox4.Text = "值";
            // 
            // btnValue
            // 
            this.btnValue.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnValue.Location = new System.Drawing.Point(12, 206);
            this.btnValue.Margin = new System.Windows.Forms.Padding(4);
            this.btnValue.Name = "btnValue";
            this.btnValue.Size = new System.Drawing.Size(152, 32);
            this.btnValue.TabIndex = 1;
            this.btnValue.Text = "列出可能的值";
            this.btnValue.Click += new System.EventHandler(this.btnValue_Click);
            // 
            // lstValue
            // 
            this.lstValue.FormattingEnabled = true;
            this.lstValue.ItemHeight = 15;
            this.lstValue.Location = new System.Drawing.Point(12, 25);
            this.lstValue.Margin = new System.Windows.Forms.Padding(4);
            this.lstValue.Name = "lstValue";
            this.lstValue.Size = new System.Drawing.Size(151, 169);
            this.lstValue.TabIndex = 0;
            this.lstValue.DoubleClick += new System.EventHandler(this.lstValue_DoubleClick);
            // 
            // groGroupBox5
            // 
            this.groGroupBox5.Controls.Add(this.btnVerify);
            this.groGroupBox5.Controls.Add(this.txtExpress);
            this.groGroupBox5.Controls.Add(this.btnOK);
            this.groGroupBox5.Controls.Add(this.btnCancel);
            this.groGroupBox5.Controls.Add(this.btnClear);
            this.groGroupBox5.Controls.Add(this.btnApply);
            this.groGroupBox5.Location = new System.Drawing.Point(8, 388);
            this.groGroupBox5.Margin = new System.Windows.Forms.Padding(4);
            this.groGroupBox5.Name = "groGroupBox5";
            this.groGroupBox5.Padding = new System.Windows.Forms.Padding(4);
            this.groGroupBox5.Size = new System.Drawing.Size(537, 165);
            this.groGroupBox5.TabIndex = 5;
            this.groGroupBox5.TabStop = false;
            this.groGroupBox5.Text = "生成表达式";
            // 
            // btnVerify
            // 
            this.btnVerify.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnVerify.Location = new System.Drawing.Point(117, 122);
            this.btnVerify.Margin = new System.Windows.Forms.Padding(4);
            this.btnVerify.Name = "btnVerify";
            this.btnVerify.Size = new System.Drawing.Size(92, 30);
            this.btnVerify.TabIndex = 9;
            this.btnVerify.Text = "验证表达式";
            this.btnVerify.Click += new System.EventHandler(this.btnVerify_Click);
            // 
            // txtExpress
            // 
            this.txtExpress.Location = new System.Drawing.Point(11, 20);
            this.txtExpress.Margin = new System.Windows.Forms.Padding(4);
            this.txtExpress.Name = "txtExpress";
            this.txtExpress.Size = new System.Drawing.Size(513, 94);
            this.txtExpress.TabIndex = 8;
            this.txtExpress.Text = "";
            // 
            // btnOK
            // 
            this.btnOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOK.Location = new System.Drawing.Point(232, 122);
            this.btnOK.Margin = new System.Windows.Forms.Padding(4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(92, 30);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "属性查询";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.Location = new System.Drawing.Point(436, 122);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(92, 30);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "取  消 ";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnClear
            // 
            this.btnClear.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnClear.Location = new System.Drawing.Point(11, 122);
            this.btnClear.Margin = new System.Windows.Forms.Padding(4);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(92, 30);
            this.btnClear.TabIndex = 1;
            this.btnClear.Text = "清除表达式";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnApply
            // 
            this.btnApply.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnApply.Location = new System.Drawing.Point(336, 122);
            this.btnApply.Margin = new System.Windows.Forms.Padding(4);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(92, 30);
            this.btnApply.TabIndex = 5;
            this.btnApply.Text = "空间定位";
            this.btnApply.ClientSizeChanged += new System.EventHandler(this.btnApply_ClientSizeChanged);
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // cboSelectMethod
            // 
            this.cboSelectMethod.DisplayMember = "Text";
            this.cboSelectMethod.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboSelectMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSelectMethod.FormattingEnabled = true;
            this.cboSelectMethod.Location = new System.Drawing.Point(156, 67);
            this.cboSelectMethod.Margin = new System.Windows.Forms.Padding(4);
            this.cboSelectMethod.Name = "cboSelectMethod";
            this.cboSelectMethod.Size = new System.Drawing.Size(355, 26);
            this.cboSelectMethod.TabIndex = 6;
            // 
            // lblMethod
            // 
            this.lblMethod.AutoSize = true;
            // 
            // 
            // 
            this.lblMethod.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblMethod.Location = new System.Drawing.Point(17, 67);
            this.lblMethod.Margin = new System.Windows.Forms.Padding(4);
            this.lblMethod.Name = "lblMethod";
            this.lblMethod.Size = new System.Drawing.Size(62, 20);
            this.lblMethod.TabIndex = 5;
            this.lblMethod.Text = "选择方式";
            // 
            // FrmQueryByEntiTable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(575, 568);
            this.Controls.Add(this.groGroupBox5);
            this.Controls.Add(this.groGroupBox4);
            this.Controls.Add(this.groGroupBox3);
            this.Controls.Add(this.groGroupBox2);
            this.Controls.Add(this.groGroupBox1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "FrmQueryByEntiTable";
            this.Text = "实体表查询";
            this.Load += new System.EventHandler(this.FrmQueryByEntiTable_Load);
            this.groGroupBox1.ResumeLayout(false);
            this.groGroupBox1.PerformLayout();
            this.groGroupBox2.ResumeLayout(false);
            this.groGroupBox3.ResumeLayout(false);
            this.groGroupBox4.ResumeLayout(false);
            this.groGroupBox5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.GroupBox groGroupBox1;
        internal DevComponents.DotNetBar.LabelX lblLayer;
        internal System.Windows.Forms.GroupBox groGroupBox2;
        internal System.Windows.Forms.ListBox lstField;
        internal System.Windows.Forms.GroupBox groGroupBox3;
        internal DevComponents.DotNetBar.ButtonX btnOperate1;
        internal System.Windows.Forms.GroupBox groGroupBox4;
        internal DevComponents.DotNetBar.ButtonX btnValue;
        internal System.Windows.Forms.ListBox lstValue;
        internal System.Windows.Forms.GroupBox groGroupBox5;
        internal DevComponents.DotNetBar.ButtonX btnVerify;
        internal System.Windows.Forms.RichTextBox txtExpress;
        internal DevComponents.DotNetBar.ButtonX btnOK;
        internal DevComponents.DotNetBar.ButtonX btnCancel;
        internal DevComponents.DotNetBar.ButtonX btnClear;
        internal DevComponents.DotNetBar.ButtonX btnApply;
        public DevComponents.DotNetBar.Controls.ComboBoxEx cboLayerList;
        private DevComponents.DotNetBar.ButtonX buttonXOpenEntiTalbe;
        internal DevComponents.DotNetBar.Controls.ComboBoxEx cboSelectMethod;
        internal DevComponents.DotNetBar.LabelX lblMethod;
    }
}