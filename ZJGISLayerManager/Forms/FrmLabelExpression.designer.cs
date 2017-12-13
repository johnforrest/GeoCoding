namespace ZJGISLayerManager
{
    partial class FrmLabelExpression
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
            this.btnVerify = new DevComponents.DotNetBar.ButtonX();
            this.txtExpression = new System.Windows.Forms.RichTextBox();
            this.btnCancle = new DevComponents.DotNetBar.ButtonX();
            this.btnOK = new DevComponents.DotNetBar.ButtonX();
            this.btnClear = new DevComponents.DotNetBar.ButtonX();
            this.btnAppendix = new DevComponents.DotNetBar.ButtonX();
            this.GroupBox2 = new System.Windows.Forms.GroupBox();
            this.lstFieldName = new DevComponents.DotNetBar.Controls.ListViewEx();
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.GroupBox2.SuspendLayout();
            this.GroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnVerify
            // 
            this.btnVerify.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnVerify.Location = new System.Drawing.Point(82, 165);
            this.btnVerify.Name = "btnVerify";
            this.btnVerify.Size = new System.Drawing.Size(55, 19);
            this.btnVerify.TabIndex = 4;
            this.btnVerify.Text = "检查";
            this.btnVerify.Click += new System.EventHandler(this.btnVerify_Click);
            // 
            // txtExpression
            // 
            this.txtExpression.Location = new System.Drawing.Point(9, 20);
            this.txtExpression.Name = "txtExpression";
            this.txtExpression.Size = new System.Drawing.Size(224, 139);
            this.txtExpression.TabIndex = 3;
            this.txtExpression.Text = "";
            // 
            // btnCancle
            // 
            this.btnCancle.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancle.Location = new System.Drawing.Point(195, 402);
            this.btnCancle.Name = "btnCancle";
            this.btnCancle.Size = new System.Drawing.Size(55, 19);
            this.btnCancle.TabIndex = 7;
            this.btnCancle.Text = "取消";
            this.btnCancle.Click += new System.EventHandler(this.btnCancle_Click);
            // 
            // btnOK
            // 
            this.btnOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOK.Location = new System.Drawing.Point(129, 402);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(55, 19);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "确定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnClear
            // 
            this.btnClear.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnClear.Location = new System.Drawing.Point(9, 165);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(55, 19);
            this.btnClear.TabIndex = 1;
            this.btnClear.Text = "清空";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnAppendix
            // 
            this.btnAppendix.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnAppendix.Location = new System.Drawing.Point(9, 148);
            this.btnAppendix.Name = "btnAppendix";
            this.btnAppendix.Size = new System.Drawing.Size(55, 19);
            this.btnAppendix.TabIndex = 1;
            this.btnAppendix.Text = "附加";
            this.btnAppendix.Click += new System.EventHandler(this.btnAppendix_Click);
            // 
            // GroupBox2
            // 
            this.GroupBox2.Controls.Add(this.btnVerify);
            this.GroupBox2.Controls.Add(this.txtExpression);
            this.GroupBox2.Controls.Add(this.btnClear);
            this.GroupBox2.Location = new System.Drawing.Point(7, 194);
            this.GroupBox2.Name = "GroupBox2";
            this.GroupBox2.Size = new System.Drawing.Size(243, 192);
            this.GroupBox2.TabIndex = 5;
            this.GroupBox2.TabStop = false;
            this.GroupBox2.Text = "表达式";
            // 
            // lstFieldName
            // 
            // 
            // 
            // 
            this.lstFieldName.Border.Class = "ListViewBorder";
            this.lstFieldName.Location = new System.Drawing.Point(9, 24);
            this.lstFieldName.Name = "lstFieldName";
            this.lstFieldName.Size = new System.Drawing.Size(226, 118);
            this.lstFieldName.TabIndex = 0;
            this.lstFieldName.UseCompatibleStateImageBehavior = false;
            this.lstFieldName.View = System.Windows.Forms.View.Details;
            // 
            // GroupBox1
            // 
            this.GroupBox1.Controls.Add(this.btnAppendix);
            this.GroupBox1.Controls.Add(this.lstFieldName);
            this.GroupBox1.Location = new System.Drawing.Point(5, 10);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Size = new System.Drawing.Size(245, 173);
            this.GroupBox1.TabIndex = 4;
            this.GroupBox1.TabStop = false;
            this.GroupBox1.Text = "可选字段";
            // 
            // FrmLabelExpression
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(254, 431);
            this.Controls.Add(this.btnCancle);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.GroupBox2);
            this.Controls.Add(this.GroupBox1);
            this.Name = "FrmLabelExpression";
            this.Text = "设置标注的表达式";
            this.Load += new System.EventHandler(this.FrmLabelExpression_Load);
            this.GroupBox2.ResumeLayout(false);
            this.GroupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        internal DevComponents.DotNetBar.ButtonX btnVerify;
        internal System.Windows.Forms.RichTextBox txtExpression;
        internal DevComponents.DotNetBar.ButtonX btnCancle;
        internal DevComponents.DotNetBar.ButtonX btnOK;
        internal DevComponents.DotNetBar.ButtonX btnClear;
        internal DevComponents.DotNetBar.ButtonX btnAppendix;
        internal System.Windows.Forms.GroupBox GroupBox2;
        internal DevComponents.DotNetBar.Controls.ListViewEx lstFieldName;
        internal System.Windows.Forms.GroupBox GroupBox1;
    }
}