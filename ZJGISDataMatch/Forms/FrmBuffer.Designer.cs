namespace ZJGISDataMatch.Forms
{
    partial class FrmBuffer
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
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.comboBoxEx1 = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.comboBoxEx2 = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.textBoxX1 = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.textBoxX2 = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.buttonX1Buffer = new DevComponents.DotNetBar.ButtonX();
            this.buttonX2Cancel = new DevComponents.DotNetBar.ButtonX();
            this.buttonOpenData = new DevComponents.DotNetBar.ButtonX();
            this.textBoxX3 = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.SuspendLayout();
            // 
            // labelX1
            // 
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Location = new System.Drawing.Point(32, 43);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(75, 23);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "选择图层";
            // 
            // labelX2
            // 
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Location = new System.Drawing.Point(32, 98);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(75, 23);
            this.labelX2.TabIndex = 1;
            this.labelX2.Text = "距离";
            // 
            // labelX3
            // 
            // 
            // 
            // 
            this.labelX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX3.Location = new System.Drawing.Point(32, 155);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(75, 23);
            this.labelX3.TabIndex = 2;
            this.labelX3.Text = "输出图层";
            // 
            // comboBoxEx1
            // 
            this.comboBoxEx1.DisplayMember = "Text";
            this.comboBoxEx1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboBoxEx1.FormattingEnabled = true;
            this.comboBoxEx1.ItemHeight = 19;
            this.comboBoxEx1.Location = new System.Drawing.Point(140, 43);
            this.comboBoxEx1.Name = "comboBoxEx1";
            this.comboBoxEx1.Size = new System.Drawing.Size(236, 25);
            this.comboBoxEx1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.comboBoxEx1.TabIndex = 3;
            // 
            // comboBoxEx2
            // 
            this.comboBoxEx2.DisplayMember = "Text";
            this.comboBoxEx2.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboBoxEx2.FormattingEnabled = true;
            this.comboBoxEx2.ItemHeight = 19;
            this.comboBoxEx2.Location = new System.Drawing.Point(255, 98);
            this.comboBoxEx2.Name = "comboBoxEx2";
            this.comboBoxEx2.Size = new System.Drawing.Size(121, 25);
            this.comboBoxEx2.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.comboBoxEx2.TabIndex = 4;
            // 
            // textBoxX1
            // 
            // 
            // 
            // 
            this.textBoxX1.Border.Class = "TextBoxBorder";
            this.textBoxX1.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.textBoxX1.Location = new System.Drawing.Point(140, 98);
            this.textBoxX1.Name = "textBoxX1";
            this.textBoxX1.Size = new System.Drawing.Size(100, 25);
            this.textBoxX1.TabIndex = 5;
            this.textBoxX1.Text = "7.5";
            // 
            // textBoxX2
            // 
            // 
            // 
            // 
            this.textBoxX2.Border.Class = "TextBoxBorder";
            this.textBoxX2.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.textBoxX2.Location = new System.Drawing.Point(140, 152);
            this.textBoxX2.Name = "textBoxX2";
            this.textBoxX2.ReadOnly = true;
            this.textBoxX2.Size = new System.Drawing.Size(175, 25);
            this.textBoxX2.TabIndex = 6;
            // 
            // buttonX1Buffer
            // 
            this.buttonX1Buffer.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX1Buffer.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonX1Buffer.Location = new System.Drawing.Point(140, 199);
            this.buttonX1Buffer.Name = "buttonX1Buffer";
            this.buttonX1Buffer.Size = new System.Drawing.Size(100, 23);
            this.buttonX1Buffer.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonX1Buffer.TabIndex = 7;
            this.buttonX1Buffer.Text = "缓冲区分析";
            this.buttonX1Buffer.Click += new System.EventHandler(this.buttonX1Buffer_Click);
            // 
            // buttonX2Cancel
            // 
            this.buttonX2Cancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX2Cancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonX2Cancel.Location = new System.Drawing.Point(273, 199);
            this.buttonX2Cancel.Name = "buttonX2Cancel";
            this.buttonX2Cancel.Size = new System.Drawing.Size(91, 23);
            this.buttonX2Cancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonX2Cancel.TabIndex = 0;
            this.buttonX2Cancel.Text = "关闭";
            this.buttonX2Cancel.Click += new System.EventHandler(this.buttonX2Cancel_Click);
            // 
            // buttonOpenData
            // 
            this.buttonOpenData.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonOpenData.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonOpenData.Location = new System.Drawing.Point(334, 152);
            this.buttonOpenData.Name = "buttonOpenData";
            this.buttonOpenData.Size = new System.Drawing.Size(42, 26);
            this.buttonOpenData.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonOpenData.TabIndex = 8;
            this.buttonOpenData.Text = "...";
            this.buttonOpenData.Click += new System.EventHandler(this.buttonOpenData_Click);
            // 
            // textBoxX3
            // 
            // 
            // 
            // 
            this.textBoxX3.Border.Class = "TextBoxBorder";
            this.textBoxX3.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.textBoxX3.Location = new System.Drawing.Point(26, 256);
            this.textBoxX3.Multiline = true;
            this.textBoxX3.Name = "textBoxX3";
            this.textBoxX3.ReadOnly = true;
            this.textBoxX3.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxX3.Size = new System.Drawing.Size(338, 128);
            this.textBoxX3.TabIndex = 9;
            // 
            // FrmBuffer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(388, 430);
            this.Controls.Add(this.textBoxX3);
            this.Controls.Add(this.buttonOpenData);
            this.Controls.Add(this.buttonX2Cancel);
            this.Controls.Add(this.buttonX1Buffer);
            this.Controls.Add(this.textBoxX2);
            this.Controls.Add(this.textBoxX1);
            this.Controls.Add(this.comboBoxEx2);
            this.Controls.Add(this.comboBoxEx1);
            this.Controls.Add(this.labelX3);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.labelX1);
            this.DoubleBuffered = true;
            this.Name = "FrmBuffer";
            this.Text = "缓冲区分析";
            this.Load += new System.EventHandler(this.FrmBuffer_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.Controls.ComboBoxEx comboBoxEx1;
        private DevComponents.DotNetBar.Controls.ComboBoxEx comboBoxEx2;
        private DevComponents.DotNetBar.Controls.TextBoxX textBoxX1;
        private DevComponents.DotNetBar.Controls.TextBoxX textBoxX2;
        private DevComponents.DotNetBar.ButtonX buttonX1Buffer;
        private DevComponents.DotNetBar.ButtonX buttonX2Cancel;
        private DevComponents.DotNetBar.ButtonX buttonOpenData;
        private DevComponents.DotNetBar.Controls.TextBoxX textBoxX3;
    }
}