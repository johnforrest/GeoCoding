namespace ZJGISLayerManager
{
    partial class FrmRotateRenderer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmRotateRenderer));
            this.rdoMath = new System.Windows.Forms.RadioButton();
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.rdoGeographic = new System.Windows.Forms.RadioButton();
            this.PictureBox2 = new System.Windows.Forms.PictureBox();
            this.PictureBox1 = new System.Windows.Forms.PictureBox();
            this.CmbField = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.LabelX1 = new DevComponents.DotNetBar.LabelX();
            this.btnOK = new DevComponents.DotNetBar.ButtonX();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.GroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // rdoMath
            // 
            this.rdoMath.AutoSize = true;
            this.rdoMath.Location = new System.Drawing.Point(168, 168);
            this.rdoMath.Name = "rdoMath";
            this.rdoMath.Size = new System.Drawing.Size(47, 16);
            this.rdoMath.TabIndex = 5;
            this.rdoMath.TabStop = true;
            this.rdoMath.Text = "数学";
            this.rdoMath.UseVisualStyleBackColor = true;
            // 
            // GroupBox1
            // 
            this.GroupBox1.Controls.Add(this.rdoMath);
            this.GroupBox1.Controls.Add(this.rdoGeographic);
            this.GroupBox1.Controls.Add(this.PictureBox2);
            this.GroupBox1.Controls.Add(this.PictureBox1);
            this.GroupBox1.Controls.Add(this.CmbField);
            this.GroupBox1.Controls.Add(this.LabelX1);
            this.GroupBox1.Location = new System.Drawing.Point(1, 2);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Size = new System.Drawing.Size(286, 201);
            this.GroupBox1.TabIndex = 3;
            this.GroupBox1.TabStop = false;
            // 
            // rdoGeographic
            // 
            this.rdoGeographic.AutoSize = true;
            this.rdoGeographic.Location = new System.Drawing.Point(62, 168);
            this.rdoGeographic.Name = "rdoGeographic";
            this.rdoGeographic.Size = new System.Drawing.Size(47, 16);
            this.rdoGeographic.TabIndex = 4;
            this.rdoGeographic.TabStop = true;
            this.rdoGeographic.Text = "地理";
            this.rdoGeographic.UseVisualStyleBackColor = true;
            // 
            // PictureBox2
            // 
            this.PictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("PictureBox2.Image")));
            this.PictureBox2.Location = new System.Drawing.Point(160, 91);
            this.PictureBox2.Name = "PictureBox2";
            this.PictureBox2.Size = new System.Drawing.Size(61, 60);
            this.PictureBox2.TabIndex = 3;
            this.PictureBox2.TabStop = false;
            // 
            // PictureBox1
            // 
            this.PictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("PictureBox1.Image")));
            this.PictureBox1.Location = new System.Drawing.Point(54, 91);
            this.PictureBox1.Name = "PictureBox1";
            this.PictureBox1.Size = new System.Drawing.Size(62, 60);
            this.PictureBox1.TabIndex = 2;
            this.PictureBox1.TabStop = false;
            // 
            // CmbField
            // 
            this.CmbField.DisplayMember = "Text";
            this.CmbField.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.CmbField.FormattingEnabled = true;
            this.CmbField.Location = new System.Drawing.Point(13, 45);
            this.CmbField.Name = "CmbField";
            this.CmbField.Size = new System.Drawing.Size(256, 22);
            this.CmbField.TabIndex = 1;
            // 
            // LabelX1
            // 
            this.LabelX1.Location = new System.Drawing.Point(12, 20);
            this.LabelX1.Name = "LabelX1";
            this.LabelX1.Size = new System.Drawing.Size(165, 18);
            this.LabelX1.TabIndex = 0;
            this.LabelX1.Text = "旋转角度的字段:";
            // 
            // btnOK
            // 
            this.btnOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOK.Location = new System.Drawing.Point(120, 210);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "确定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.Location = new System.Drawing.Point(212, 209);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // FrmRotateRenderer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(289, 234);
            this.Controls.Add(this.GroupBox1);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Name = "FrmRotateRenderer";
            this.Text = "符号旋转设置";
            this.Load += new System.EventHandler(this.FrmRotateRenderer_Load);
            this.GroupBox1.ResumeLayout(false);
            this.GroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.RadioButton rdoMath;
        internal System.Windows.Forms.GroupBox GroupBox1;
        internal System.Windows.Forms.RadioButton rdoGeographic;
        internal System.Windows.Forms.PictureBox PictureBox2;
        internal System.Windows.Forms.PictureBox PictureBox1;
        internal DevComponents.DotNetBar.Controls.ComboBoxEx CmbField;
        internal DevComponents.DotNetBar.LabelX LabelX1;
        internal DevComponents.DotNetBar.ButtonX btnOK;
        internal DevComponents.DotNetBar.ButtonX btnCancel;
    }
}