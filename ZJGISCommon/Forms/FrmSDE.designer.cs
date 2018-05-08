namespace ZJGISCommon
{
    partial class FrmSDE
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
            this.cboVersion = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.comboBoxEx1 = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.btnAddSDE = new DevComponents.DotNetBar.ButtonX();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.txtSet5 = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtSet4 = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtSet3 = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtSet2 = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtSet1 = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.LabelX6 = new DevComponents.DotNetBar.LabelX();
            this.LabelX5 = new DevComponents.DotNetBar.LabelX();
            this.LabelX4 = new DevComponents.DotNetBar.LabelX();
            this.LabelX3 = new DevComponents.DotNetBar.LabelX();
            this.LabelX2 = new DevComponents.DotNetBar.LabelX();
            this.LabelX1 = new DevComponents.DotNetBar.LabelX();
            this.comboBoxEx2 = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX7 = new DevComponents.DotNetBar.LabelX();
            this.SuspendLayout();
            // 
            // cboVersion
            // 
            this.cboVersion.DisplayMember = "Text";
            this.cboVersion.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboVersion.FormattingEnabled = true;
            this.cboVersion.Location = new System.Drawing.Point(108, 187);
            this.cboVersion.Name = "cboVersion";
            this.cboVersion.Size = new System.Drawing.Size(181, 26);
            this.cboVersion.TabIndex = 15;
            this.cboVersion.Text = "SDE.DEFAULT";
            // 
            // comboBoxEx1
            // 
            this.comboBoxEx1.DisplayMember = "Text";
            this.comboBoxEx1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboBoxEx1.FormattingEnabled = true;
            this.comboBoxEx1.Location = new System.Drawing.Point(124, 221);
            this.comboBoxEx1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comboBoxEx1.Name = "comboBoxEx1";
            this.comboBoxEx1.Size = new System.Drawing.Size(240, 26);
            this.comboBoxEx1.TabIndex = 29;
            this.comboBoxEx1.Text = "DBO.DEFAULT";
            // 
            // btnAddSDE
            // 
            this.btnAddSDE.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnAddSDE.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnAddSDE.Location = new System.Drawing.Point(159, 319);
            this.btnAddSDE.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnAddSDE.Name = "btnAddSDE";
            this.btnAddSDE.Size = new System.Drawing.Size(100, 29);
            this.btnAddSDE.TabIndex = 28;
            this.btnAddSDE.Text = "连  接";
            this.btnAddSDE.Click += new System.EventHandler(this.btnAddSDE_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.Location = new System.Drawing.Point(267, 319);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 29);
            this.btnCancel.TabIndex = 27;
            this.btnCancel.Text = "取  消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // txtSet5
            // 
            // 
            // 
            // 
            this.txtSet5.Border.Class = "TextBoxBorder";
            this.txtSet5.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtSet5.Location = new System.Drawing.Point(124, 179);
            this.txtSet5.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtSet5.Name = "txtSet5";
            this.txtSet5.PasswordChar = '*';
            this.txtSet5.Size = new System.Drawing.Size(243, 25);
            this.txtSet5.TabIndex = 26;
            // 
            // txtSet4
            // 
            // 
            // 
            // 
            this.txtSet4.Border.Class = "TextBoxBorder";
            this.txtSet4.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtSet4.Location = new System.Drawing.Point(124, 139);
            this.txtSet4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtSet4.Name = "txtSet4";
            this.txtSet4.Size = new System.Drawing.Size(243, 25);
            this.txtSet4.TabIndex = 25;
            // 
            // txtSet3
            // 
            // 
            // 
            // 
            this.txtSet3.Border.Class = "TextBoxBorder";
            this.txtSet3.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtSet3.Location = new System.Drawing.Point(124, 96);
            this.txtSet3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtSet3.Name = "txtSet3";
            this.txtSet3.Size = new System.Drawing.Size(243, 25);
            this.txtSet3.TabIndex = 24;
            // 
            // txtSet2
            // 
            // 
            // 
            // 
            this.txtSet2.Border.Class = "TextBoxBorder";
            this.txtSet2.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtSet2.Location = new System.Drawing.Point(124, 56);
            this.txtSet2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtSet2.Name = "txtSet2";
            this.txtSet2.Size = new System.Drawing.Size(243, 25);
            this.txtSet2.TabIndex = 23;
            // 
            // txtSet1
            // 
            // 
            // 
            // 
            this.txtSet1.Border.Class = "TextBoxBorder";
            this.txtSet1.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtSet1.Location = new System.Drawing.Point(124, 15);
            this.txtSet1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtSet1.Name = "txtSet1";
            this.txtSet1.Size = new System.Drawing.Size(243, 25);
            this.txtSet1.TabIndex = 22;
            // 
            // LabelX6
            // 
            // 
            // 
            // 
            this.LabelX6.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.LabelX6.Location = new System.Drawing.Point(16, 216);
            this.LabelX6.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.LabelX6.Name = "LabelX6";
            this.LabelX6.Size = new System.Drawing.Size(140, 32);
            this.LabelX6.TabIndex = 21;
            this.LabelX6.Text = "版      本：";
            // 
            // LabelX5
            // 
            // 
            // 
            // 
            this.LabelX5.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.LabelX5.Location = new System.Drawing.Point(16, 175);
            this.LabelX5.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.LabelX5.Name = "LabelX5";
            this.LabelX5.Size = new System.Drawing.Size(157, 32);
            this.LabelX5.TabIndex = 20;
            this.LabelX5.Text = "密      码：";
            // 
            // LabelX4
            // 
            // 
            // 
            // 
            this.LabelX4.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.LabelX4.Location = new System.Drawing.Point(16, 134);
            this.LabelX4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.LabelX4.Name = "LabelX4";
            this.LabelX4.Size = new System.Drawing.Size(119, 32);
            this.LabelX4.TabIndex = 19;
            this.LabelX4.Text = "用  户  名：";
            // 
            // LabelX3
            // 
            // 
            // 
            // 
            this.LabelX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.LabelX3.Location = new System.Drawing.Point(16, 94);
            this.LabelX3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.LabelX3.Name = "LabelX3";
            this.LabelX3.Size = new System.Drawing.Size(119, 32);
            this.LabelX3.TabIndex = 18;
            this.LabelX3.Text = "数  据  库：";
            // 
            // LabelX2
            // 
            // 
            // 
            // 
            this.LabelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.LabelX2.Location = new System.Drawing.Point(16, 54);
            this.LabelX2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.LabelX2.Name = "LabelX2";
            this.LabelX2.Size = new System.Drawing.Size(119, 32);
            this.LabelX2.TabIndex = 17;
            this.LabelX2.Text = "服务实例：";
            // 
            // LabelX1
            // 
            // 
            // 
            // 
            this.LabelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.LabelX1.Location = new System.Drawing.Point(16, 15);
            this.LabelX1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.LabelX1.Name = "LabelX1";
            this.LabelX1.Size = new System.Drawing.Size(119, 32);
            this.LabelX1.TabIndex = 16;
            this.LabelX1.Text = "服  务  器：";
            // 
            // comboBoxEx2
            // 
            this.comboBoxEx2.DisplayMember = "Text";
            this.comboBoxEx2.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboBoxEx2.FormattingEnabled = true;
            this.comboBoxEx2.Location = new System.Drawing.Point(124, 263);
            this.comboBoxEx2.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxEx2.Name = "comboBoxEx2";
            this.comboBoxEx2.Size = new System.Drawing.Size(240, 26);
            this.comboBoxEx2.TabIndex = 31;
            this.comboBoxEx2.Text = "DBMS";
            // 
            // labelX7
            // 
            // 
            // 
            // 
            this.labelX7.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX7.Location = new System.Drawing.Point(16, 258);
            this.labelX7.Margin = new System.Windows.Forms.Padding(4);
            this.labelX7.Name = "labelX7";
            this.labelX7.Size = new System.Drawing.Size(140, 32);
            this.labelX7.TabIndex = 30;
            this.labelX7.Text = "授权方式：";
            // 
            // FrmSDE
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(388, 384);
            this.Controls.Add(this.comboBoxEx2);
            this.Controls.Add(this.labelX7);
            this.Controls.Add(this.comboBoxEx1);
            this.Controls.Add(this.btnAddSDE);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.txtSet5);
            this.Controls.Add(this.txtSet4);
            this.Controls.Add(this.txtSet3);
            this.Controls.Add(this.txtSet2);
            this.Controls.Add(this.txtSet1);
            this.Controls.Add(this.LabelX6);
            this.Controls.Add(this.LabelX5);
            this.Controls.Add(this.LabelX4);
            this.Controls.Add(this.LabelX3);
            this.Controls.Add(this.LabelX2);
            this.Controls.Add(this.LabelX1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmSDE";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SDE连接信息";
            this.Load += new System.EventHandler(this.FrmSDE_Load);
            this.ResumeLayout(false);

        }

        #endregion

        internal DevComponents.DotNetBar.Controls.ComboBoxEx cboVersion;
        internal DevComponents.DotNetBar.Controls.ComboBoxEx comboBoxEx1;
        internal DevComponents.DotNetBar.ButtonX btnAddSDE;
        internal DevComponents.DotNetBar.ButtonX btnCancel;
        internal DevComponents.DotNetBar.Controls.TextBoxX txtSet5;
        internal DevComponents.DotNetBar.Controls.TextBoxX txtSet4;
        internal DevComponents.DotNetBar.Controls.TextBoxX txtSet3;
        internal DevComponents.DotNetBar.Controls.TextBoxX txtSet2;
        internal DevComponents.DotNetBar.Controls.TextBoxX txtSet1;
        internal DevComponents.DotNetBar.LabelX LabelX6;
        internal DevComponents.DotNetBar.LabelX LabelX5;
        internal DevComponents.DotNetBar.LabelX LabelX4;
        internal DevComponents.DotNetBar.LabelX LabelX3;
        internal DevComponents.DotNetBar.LabelX LabelX2;
        internal DevComponents.DotNetBar.LabelX LabelX1;
        internal DevComponents.DotNetBar.Controls.ComboBoxEx comboBoxEx2;
        internal DevComponents.DotNetBar.LabelX labelX7;
    }
}