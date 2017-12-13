namespace SelectQuery
{
    partial class FrmQueryByAttribute
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
            this.cboSelectMethod = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.cboLayerList = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.lblMethod = new DevComponents.DotNetBar.LabelX();
            this.lblLayer = new DevComponents.DotNetBar.LabelX();
            this.groGroupBox2 = new System.Windows.Forms.GroupBox();
            this.lstField = new System.Windows.Forms.ListBox();
            this.groGroupBox3 = new System.Windows.Forms.GroupBox();
            this.btnOperate11 = new DevComponents.DotNetBar.ButtonX();
            this.btnOperate10 = new DevComponents.DotNetBar.ButtonX();
            this.btnOperate14 = new DevComponents.DotNetBar.ButtonX();
            this.btnOperate13 = new DevComponents.DotNetBar.ButtonX();
            this.btnOperate12 = new DevComponents.DotNetBar.ButtonX();
            this.btnOperate9 = new DevComponents.DotNetBar.ButtonX();
            this.btnOperate8 = new DevComponents.DotNetBar.ButtonX();
            this.btnOperate7 = new DevComponents.DotNetBar.ButtonX();
            this.btnOperate6 = new DevComponents.DotNetBar.ButtonX();
            this.btnOperate5 = new DevComponents.DotNetBar.ButtonX();
            this.btnOperate4 = new DevComponents.DotNetBar.ButtonX();
            this.btnOperate3 = new DevComponents.DotNetBar.ButtonX();
            this.btnOperate2 = new DevComponents.DotNetBar.ButtonX();
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
            this.groGroupBox1.Controls.Add(this.cboLayerList);
            this.groGroupBox1.Controls.Add(this.lblMethod);
            this.groGroupBox1.Controls.Add(this.lblLayer);
            this.groGroupBox1.Location = new System.Drawing.Point(2, 2);
            this.groGroupBox1.Name = "groGroupBox1";
            this.groGroupBox1.Size = new System.Drawing.Size(417, 86);
            this.groGroupBox1.TabIndex = 1;
            this.groGroupBox1.TabStop = false;
            // 
            // cboSelectMethod
            // 
            this.cboSelectMethod.DisplayMember = "Text";
            this.cboSelectMethod.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboSelectMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSelectMethod.FormattingEnabled = true;
            this.cboSelectMethod.Location = new System.Drawing.Point(117, 55);
            this.cboSelectMethod.Name = "cboSelectMethod";
            this.cboSelectMethod.Size = new System.Drawing.Size(267, 22);
            this.cboSelectMethod.TabIndex = 3;
            // 
            // cboLayerList
            // 
            this.cboLayerList.DisplayMember = "Text";
            this.cboLayerList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboLayerList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLayerList.FormattingEnabled = true;
            this.cboLayerList.Location = new System.Drawing.Point(117, 14);
            this.cboLayerList.Name = "cboLayerList";
            this.cboLayerList.Size = new System.Drawing.Size(267, 22);
            this.cboLayerList.TabIndex = 2;
            this.cboLayerList.SelectedIndexChanged += new System.EventHandler(this.cboLayerList_SelectedIndexChanged);
            // 
            // lblMethod
            // 
            this.lblMethod.AutoSize = true;
            // 
            // 
            // 
            this.lblMethod.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblMethod.Location = new System.Drawing.Point(13, 55);
            this.lblMethod.Name = "lblMethod";
            this.lblMethod.Size = new System.Drawing.Size(56, 18);
            this.lblMethod.TabIndex = 1;
            this.lblMethod.Text = "选择方式";
            // 
            // lblLayer
            // 
            this.lblLayer.AutoSize = true;
            // 
            // 
            // 
            this.lblLayer.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblLayer.Location = new System.Drawing.Point(13, 20);
            this.lblLayer.Name = "lblLayer";
            this.lblLayer.Size = new System.Drawing.Size(56, 18);
            this.lblLayer.TabIndex = 0;
            this.lblLayer.Text = "选择图层";
            // 
            // groGroupBox2
            // 
            this.groGroupBox2.Controls.Add(this.lstField);
            this.groGroupBox2.Location = new System.Drawing.Point(2, 94);
            this.groGroupBox2.Name = "groGroupBox2";
            this.groGroupBox2.Size = new System.Drawing.Size(148, 199);
            this.groGroupBox2.TabIndex = 2;
            this.groGroupBox2.TabStop = false;
            this.groGroupBox2.Text = "字段";
            // 
            // lstField
            // 
            this.lstField.FormattingEnabled = true;
            this.lstField.ItemHeight = 12;
            this.lstField.Location = new System.Drawing.Point(9, 18);
            this.lstField.Name = "lstField";
            this.lstField.Size = new System.Drawing.Size(133, 172);
            this.lstField.TabIndex = 0;
            this.lstField.SelectedIndexChanged += new System.EventHandler(this.lstField_SelectedIndexChanged);
            this.lstField.DoubleClick += new System.EventHandler(this.lstField_DoubleClick);
            // 
            // groGroupBox3
            // 
            this.groGroupBox3.Controls.Add(this.btnOperate11);
            this.groGroupBox3.Controls.Add(this.btnOperate10);
            this.groGroupBox3.Controls.Add(this.btnOperate14);
            this.groGroupBox3.Controls.Add(this.btnOperate13);
            this.groGroupBox3.Controls.Add(this.btnOperate12);
            this.groGroupBox3.Controls.Add(this.btnOperate9);
            this.groGroupBox3.Controls.Add(this.btnOperate8);
            this.groGroupBox3.Controls.Add(this.btnOperate7);
            this.groGroupBox3.Controls.Add(this.btnOperate6);
            this.groGroupBox3.Controls.Add(this.btnOperate5);
            this.groGroupBox3.Controls.Add(this.btnOperate4);
            this.groGroupBox3.Controls.Add(this.btnOperate3);
            this.groGroupBox3.Controls.Add(this.btnOperate2);
            this.groGroupBox3.Controls.Add(this.btnOperate1);
            this.groGroupBox3.Location = new System.Drawing.Point(156, 103);
            this.groGroupBox3.Name = "groGroupBox3";
            this.groGroupBox3.Size = new System.Drawing.Size(124, 199);
            this.groGroupBox3.TabIndex = 3;
            this.groGroupBox3.TabStop = false;
            this.groGroupBox3.Text = "操作符";
            // 
            // btnOperate11
            // 
            this.btnOperate11.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOperate11.Location = new System.Drawing.Point(24, 127);
            this.btnOperate11.Name = "btnOperate11";
            this.btnOperate11.Size = new System.Drawing.Size(18, 23);
            this.btnOperate11.TabIndex = 10;
            this.btnOperate11.Text = "%";
            this.btnOperate11.Click += new System.EventHandler(this.btnOperate11_Click);
            // 
            // btnOperate10
            // 
            this.btnOperate10.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOperate10.Location = new System.Drawing.Point(7, 127);
            this.btnOperate10.Name = "btnOperate10";
            this.btnOperate10.Size = new System.Drawing.Size(18, 23);
            this.btnOperate10.TabIndex = 9;
            this.btnOperate10.Text = "_";
            this.btnOperate10.Click += new System.EventHandler(this.btnOperate10_Click);
            // 
            // btnOperate14
            // 
            this.btnOperate14.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOperate14.Location = new System.Drawing.Point(6, 159);
            this.btnOperate14.Name = "btnOperate14";
            this.btnOperate14.Size = new System.Drawing.Size(36, 22);
            this.btnOperate14.TabIndex = 13;
            this.btnOperate14.Text = "is";
            this.btnOperate14.Click += new System.EventHandler(this.btnOperate14_Click);
            // 
            // btnOperate13
            // 
            this.btnOperate13.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOperate13.Location = new System.Drawing.Point(82, 129);
            this.btnOperate13.Name = "btnOperate13";
            this.btnOperate13.Size = new System.Drawing.Size(36, 22);
            this.btnOperate13.TabIndex = 12;
            this.btnOperate13.Text = "not";
            this.btnOperate13.Click += new System.EventHandler(this.btnOperate13_Click);
            // 
            // btnOperate12
            // 
            this.btnOperate12.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOperate12.Location = new System.Drawing.Point(44, 129);
            this.btnOperate12.Name = "btnOperate12";
            this.btnOperate12.Size = new System.Drawing.Size(36, 22);
            this.btnOperate12.TabIndex = 11;
            this.btnOperate12.Text = "()";
            this.btnOperate12.Click += new System.EventHandler(this.btnOperate12_Click);
            // 
            // btnOperate9
            // 
            this.btnOperate9.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOperate9.Location = new System.Drawing.Point(82, 93);
            this.btnOperate9.Name = "btnOperate9";
            this.btnOperate9.Size = new System.Drawing.Size(36, 22);
            this.btnOperate9.TabIndex = 8;
            this.btnOperate9.Text = "or";
            this.btnOperate9.Click += new System.EventHandler(this.btnOperate9_Click);
            // 
            // btnOperate8
            // 
            this.btnOperate8.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOperate8.Location = new System.Drawing.Point(44, 93);
            this.btnOperate8.Name = "btnOperate8";
            this.btnOperate8.Size = new System.Drawing.Size(36, 22);
            this.btnOperate8.TabIndex = 7;
            this.btnOperate8.Text = "<=";
            this.btnOperate8.Click += new System.EventHandler(this.btnOperate8_Click);
            // 
            // btnOperate7
            // 
            this.btnOperate7.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOperate7.Location = new System.Drawing.Point(6, 93);
            this.btnOperate7.Name = "btnOperate7";
            this.btnOperate7.Size = new System.Drawing.Size(36, 22);
            this.btnOperate7.TabIndex = 6;
            this.btnOperate7.Text = "<";
            this.btnOperate7.Click += new System.EventHandler(this.btnOperate7_Click);
            // 
            // btnOperate6
            // 
            this.btnOperate6.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOperate6.Location = new System.Drawing.Point(82, 57);
            this.btnOperate6.Name = "btnOperate6";
            this.btnOperate6.Size = new System.Drawing.Size(36, 22);
            this.btnOperate6.TabIndex = 5;
            this.btnOperate6.Text = "and";
            this.btnOperate6.Click += new System.EventHandler(this.btnOperate6_Click);
            // 
            // btnOperate5
            // 
            this.btnOperate5.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOperate5.Location = new System.Drawing.Point(44, 57);
            this.btnOperate5.Name = "btnOperate5";
            this.btnOperate5.Size = new System.Drawing.Size(36, 22);
            this.btnOperate5.TabIndex = 4;
            this.btnOperate5.Text = ">=";
            this.btnOperate5.Click += new System.EventHandler(this.btnOperate5_Click);
            // 
            // btnOperate4
            // 
            this.btnOperate4.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOperate4.Location = new System.Drawing.Point(6, 57);
            this.btnOperate4.Name = "btnOperate4";
            this.btnOperate4.Size = new System.Drawing.Size(36, 22);
            this.btnOperate4.TabIndex = 3;
            this.btnOperate4.Text = ">";
            this.btnOperate4.Click += new System.EventHandler(this.btnOperate4_Click);
            // 
            // btnOperate3
            // 
            this.btnOperate3.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOperate3.Location = new System.Drawing.Point(82, 29);
            this.btnOperate3.Name = "btnOperate3";
            this.btnOperate3.Size = new System.Drawing.Size(36, 22);
            this.btnOperate3.TabIndex = 2;
            this.btnOperate3.Text = "like";
            this.btnOperate3.Click += new System.EventHandler(this.btnOperate3_Click);
            // 
            // btnOperate2
            // 
            this.btnOperate2.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOperate2.Location = new System.Drawing.Point(44, 29);
            this.btnOperate2.Name = "btnOperate2";
            this.btnOperate2.Size = new System.Drawing.Size(36, 22);
            this.btnOperate2.TabIndex = 1;
            this.btnOperate2.Text = "<>";
            this.btnOperate2.Click += new System.EventHandler(this.btnOperate2_Click);
            // 
            // btnOperate1
            // 
            this.btnOperate1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOperate1.Location = new System.Drawing.Point(6, 29);
            this.btnOperate1.Name = "btnOperate1";
            this.btnOperate1.Size = new System.Drawing.Size(36, 22);
            this.btnOperate1.TabIndex = 0;
            this.btnOperate1.Text = "=";
            this.btnOperate1.Click += new System.EventHandler(this.btnOperate1_Click);
            // 
            // groGroupBox4
            // 
            this.groGroupBox4.Controls.Add(this.btnValue);
            this.groGroupBox4.Controls.Add(this.lstValue);
            this.groGroupBox4.Location = new System.Drawing.Point(286, 103);
            this.groGroupBox4.Name = "groGroupBox4";
            this.groGroupBox4.Size = new System.Drawing.Size(133, 199);
            this.groGroupBox4.TabIndex = 4;
            this.groGroupBox4.TabStop = false;
            this.groGroupBox4.Text = "值";
            // 
            // btnValue
            // 
            this.btnValue.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnValue.Location = new System.Drawing.Point(9, 165);
            this.btnValue.Name = "btnValue";
            this.btnValue.Size = new System.Drawing.Size(114, 26);
            this.btnValue.TabIndex = 1;
            this.btnValue.Text = "列出可能的值";
            this.btnValue.Click += new System.EventHandler(this.btnValue_Click);
            // 
            // lstValue
            // 
            this.lstValue.FormattingEnabled = true;
            this.lstValue.ItemHeight = 12;
            this.lstValue.Location = new System.Drawing.Point(9, 20);
            this.lstValue.Name = "lstValue";
            this.lstValue.Size = new System.Drawing.Size(114, 136);
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
            this.groGroupBox5.Location = new System.Drawing.Point(6, 310);
            this.groGroupBox5.Name = "groGroupBox5";
            this.groGroupBox5.Size = new System.Drawing.Size(403, 132);
            this.groGroupBox5.TabIndex = 5;
            this.groGroupBox5.TabStop = false;
            this.groGroupBox5.Text = "生成表达式";
            // 
            // btnVerify
            // 
            this.btnVerify.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnVerify.Location = new System.Drawing.Point(88, 97);
            this.btnVerify.Name = "btnVerify";
            this.btnVerify.Size = new System.Drawing.Size(69, 24);
            this.btnVerify.TabIndex = 9;
            this.btnVerify.Text = "验证表达式";
            this.btnVerify.Click += new System.EventHandler(this.btnVerify_Click);
            // 
            // txtExpress
            // 
            this.txtExpress.Location = new System.Drawing.Point(8, 16);
            this.txtExpress.Name = "txtExpress";
            this.txtExpress.Size = new System.Drawing.Size(386, 76);
            this.txtExpress.TabIndex = 8;
            this.txtExpress.Text = "";
            // 
            // btnOK
            // 
            this.btnOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOK.Location = new System.Drawing.Point(249, 97);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(69, 24);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "属性查询";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.Location = new System.Drawing.Point(327, 97);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(69, 24);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "取  消 ";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnClear
            // 
            this.btnClear.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnClear.Location = new System.Drawing.Point(8, 97);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(69, 24);
            this.btnClear.TabIndex = 1;
            this.btnClear.Text = "清除表达式";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnApply
            // 
            this.btnApply.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnApply.Location = new System.Drawing.Point(169, 97);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(69, 24);
            this.btnApply.TabIndex = 5;
            this.btnApply.Text = "空间定位";
            this.btnApply.ClientSizeChanged += new System.EventHandler(this.btnApply_ClientSizeChanged);
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // FrmQueryByAttribute
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(431, 454);
            this.Controls.Add(this.groGroupBox5);
            this.Controls.Add(this.groGroupBox4);
            this.Controls.Add(this.groGroupBox3);
            this.Controls.Add(this.groGroupBox2);
            this.Controls.Add(this.groGroupBox1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "FrmQueryByAttribute";
            this.Text = "属性查询";
            this.Load += new System.EventHandler(this.frmQueryByAttribute_Load);
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
        internal DevComponents.DotNetBar.Controls.ComboBoxEx cboSelectMethod;
        internal DevComponents.DotNetBar.LabelX lblMethod;
        internal DevComponents.DotNetBar.LabelX lblLayer;
        internal System.Windows.Forms.GroupBox groGroupBox2;
        internal System.Windows.Forms.ListBox lstField;
        internal System.Windows.Forms.GroupBox groGroupBox3;
        internal DevComponents.DotNetBar.ButtonX btnOperate11;
        internal DevComponents.DotNetBar.ButtonX btnOperate10;
        internal DevComponents.DotNetBar.ButtonX btnOperate14;
        internal DevComponents.DotNetBar.ButtonX btnOperate13;
        internal DevComponents.DotNetBar.ButtonX btnOperate12;
        internal DevComponents.DotNetBar.ButtonX btnOperate9;
        internal DevComponents.DotNetBar.ButtonX btnOperate8;
        internal DevComponents.DotNetBar.ButtonX btnOperate7;
        internal DevComponents.DotNetBar.ButtonX btnOperate6;
        internal DevComponents.DotNetBar.ButtonX btnOperate5;
        internal DevComponents.DotNetBar.ButtonX btnOperate4;
        internal DevComponents.DotNetBar.ButtonX btnOperate3;
        internal DevComponents.DotNetBar.ButtonX btnOperate2;
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
    }
}