namespace ZJGISLayerManager
{
    partial class FrmLabelPntChoiseField
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmLabelPntChoiseField));
            this.cboFieldsName = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.chkRation90 = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.GroupBox2 = new System.Windows.Forms.GroupBox();
            this.optArithmetic = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.optGeographic = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.PictureBox2 = new System.Windows.Forms.PictureBox();
            this.PictureBox1 = new System.Windows.Forms.PictureBox();
            this.LabelX1 = new DevComponents.DotNetBar.LabelX();
            this.btnOK = new DevComponents.DotNetBar.ButtonX();
            this.btnCancle = new DevComponents.DotNetBar.ButtonX();
            this.GroupBox1.SuspendLayout();
            this.GroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // cboFieldsName
            // 
            this.cboFieldsName.DisplayMember = "Text";
            this.cboFieldsName.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboFieldsName.FormattingEnabled = true;
            this.cboFieldsName.Location = new System.Drawing.Point(11, 54);
            this.cboFieldsName.Name = "cboFieldsName";
            this.cboFieldsName.Size = new System.Drawing.Size(237, 22);
            this.cboFieldsName.TabIndex = 1;
            // 
            // GroupBox1
            // 
            this.GroupBox1.Controls.Add(this.chkRation90);
            this.GroupBox1.Controls.Add(this.GroupBox2);
            this.GroupBox1.Controls.Add(this.cboFieldsName);
            this.GroupBox1.Controls.Add(this.LabelX1);
            this.GroupBox1.Location = new System.Drawing.Point(8, 10);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Size = new System.Drawing.Size(263, 288);
            this.GroupBox1.TabIndex = 3;
            this.GroupBox1.TabStop = false;
            // 
            // chkRation90
            // 
            this.chkRation90.Location = new System.Drawing.Point(11, 248);
            this.chkRation90.Name = "chkRation90";
            this.chkRation90.Size = new System.Drawing.Size(189, 27);
            this.chkRation90.TabIndex = 3;
            this.chkRation90.Text = "在原字段值的基础上增加90度";
            // 
            // GroupBox2
            // 
            this.GroupBox2.Controls.Add(this.optArithmetic);
            this.GroupBox2.Controls.Add(this.optGeographic);
            this.GroupBox2.Controls.Add(this.PictureBox2);
            this.GroupBox2.Controls.Add(this.PictureBox1);
            this.GroupBox2.Location = new System.Drawing.Point(11, 101);
            this.GroupBox2.Name = "GroupBox2";
            this.GroupBox2.Size = new System.Drawing.Size(237, 141);
            this.GroupBox2.TabIndex = 2;
            this.GroupBox2.TabStop = false;
            this.GroupBox2.Text = "选择参照系";
            // 
            // optArithmetic
            // 
            this.optArithmetic.CheckBoxStyle = DevComponents.DotNetBar.eCheckBoxStyle.RadioButton;
            this.optArithmetic.Location = new System.Drawing.Point(144, 113);
            this.optArithmetic.Name = "optArithmetic";
            this.optArithmetic.Size = new System.Drawing.Size(87, 22);
            this.optArithmetic.TabIndex = 3;
            this.optArithmetic.Text = "几何坐标系";
            // 
            // optGeographic
            // 
            this.optGeographic.CheckBoxStyle = DevComponents.DotNetBar.eCheckBoxStyle.RadioButton;
            this.optGeographic.Location = new System.Drawing.Point(20, 113);
            this.optGeographic.Name = "optGeographic";
            this.optGeographic.Size = new System.Drawing.Size(87, 22);
            this.optGeographic.TabIndex = 2;
            this.optGeographic.Text = "大地坐标系";
            // 
            // PictureBox2
            // 
            this.PictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("PictureBox2.Image")));
            this.PictureBox2.Location = new System.Drawing.Point(155, 29);
            this.PictureBox2.Name = "PictureBox2";
            this.PictureBox2.Size = new System.Drawing.Size(62, 62);
            this.PictureBox2.TabIndex = 1;
            this.PictureBox2.TabStop = false;
            // 
            // PictureBox1
            // 
            this.PictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("PictureBox1.Image")));
            this.PictureBox1.Location = new System.Drawing.Point(20, 29);
            this.PictureBox1.Name = "PictureBox1";
            this.PictureBox1.Size = new System.Drawing.Size(60, 62);
            this.PictureBox1.TabIndex = 0;
            this.PictureBox1.TabStop = false;
            // 
            // LabelX1
            // 
            this.LabelX1.Location = new System.Drawing.Point(11, 23);
            this.LabelX1.Name = "LabelX1";
            this.LabelX1.Size = new System.Drawing.Size(149, 25);
            this.LabelX1.TabIndex = 0;
            this.LabelX1.Text = "选择确定角度值的字段：";
            // 
            // btnOK
            // 
            this.btnOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOK.Location = new System.Drawing.Point(121, 311);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(67, 24);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "确定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancle
            // 
            this.btnCancle.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancle.Location = new System.Drawing.Point(204, 311);
            this.btnCancle.Name = "btnCancle";
            this.btnCancle.Size = new System.Drawing.Size(67, 24);
            this.btnCancle.TabIndex = 5;
            this.btnCancle.Text = "取消";
            this.btnCancle.Click += new System.EventHandler(this.btnCancle_Click);
            // 
            // FrmLabelPntChoiseField
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(278, 345);
            this.Controls.Add(this.GroupBox1);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancle);
            this.Name = "FrmLabelPntChoiseField";
            this.Text = "选择字段";
            this.Load += new System.EventHandler(this.FrmLabelPntChoiseField_Load);
            this.GroupBox1.ResumeLayout(false);
            this.GroupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        internal DevComponents.DotNetBar.Controls.ComboBoxEx cboFieldsName;
        internal System.Windows.Forms.GroupBox GroupBox1;
        internal DevComponents.DotNetBar.Controls.CheckBoxX chkRation90;
        internal System.Windows.Forms.GroupBox GroupBox2;
        internal DevComponents.DotNetBar.Controls.CheckBoxX optArithmetic;
        internal DevComponents.DotNetBar.Controls.CheckBoxX optGeographic;
        internal System.Windows.Forms.PictureBox PictureBox2;
        internal System.Windows.Forms.PictureBox PictureBox1;
        internal DevComponents.DotNetBar.LabelX LabelX1;
        internal DevComponents.DotNetBar.ButtonX btnOK;
        internal DevComponents.DotNetBar.ButtonX btnCancle;
    }
}