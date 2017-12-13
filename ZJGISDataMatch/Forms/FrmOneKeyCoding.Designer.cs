namespace ZJGISDataMatch.Forms
{
    partial class FrmOneKeyCoding
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
            this.comboBoxExOriLayer = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.comboBoxExMatLayer = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.buttonXOneKeyCoding = new DevComponents.DotNetBar.ButtonX();
            this.buttonXClose = new DevComponents.DotNetBar.ButtonX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.textBoxX1 = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.comboBoxExDistance = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.SuspendLayout();
            // 
            // labelX1
            // 
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Location = new System.Drawing.Point(2, 36);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(135, 25);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "源图层：";
            // 
            // labelX2
            // 
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Location = new System.Drawing.Point(2, 136);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(135, 25);
            this.labelX2.TabIndex = 1;
            this.labelX2.Text = "待编码图层：";
            // 
            // comboBoxExOriLayer
            // 
            this.comboBoxExOriLayer.DisplayMember = "Text";
            this.comboBoxExOriLayer.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboBoxExOriLayer.FormattingEnabled = true;
            this.comboBoxExOriLayer.ItemHeight = 19;
            this.comboBoxExOriLayer.Location = new System.Drawing.Point(156, 34);
            this.comboBoxExOriLayer.Name = "comboBoxExOriLayer";
            this.comboBoxExOriLayer.Size = new System.Drawing.Size(192, 25);
            this.comboBoxExOriLayer.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.comboBoxExOriLayer.TabIndex = 2;
            // 
            // comboBoxExMatLayer
            // 
            this.comboBoxExMatLayer.DisplayMember = "Text";
            this.comboBoxExMatLayer.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboBoxExMatLayer.FormattingEnabled = true;
            this.comboBoxExMatLayer.ItemHeight = 19;
            this.comboBoxExMatLayer.Location = new System.Drawing.Point(156, 136);
            this.comboBoxExMatLayer.Name = "comboBoxExMatLayer";
            this.comboBoxExMatLayer.Size = new System.Drawing.Size(189, 25);
            this.comboBoxExMatLayer.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.comboBoxExMatLayer.TabIndex = 3;
            // 
            // buttonXOneKeyCoding
            // 
            this.buttonXOneKeyCoding.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonXOneKeyCoding.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonXOneKeyCoding.Location = new System.Drawing.Point(38, 195);
            this.buttonXOneKeyCoding.Name = "buttonXOneKeyCoding";
            this.buttonXOneKeyCoding.Size = new System.Drawing.Size(103, 34);
            this.buttonXOneKeyCoding.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonXOneKeyCoding.TabIndex = 4;
            this.buttonXOneKeyCoding.Text = "编码";
            this.buttonXOneKeyCoding.Click += new System.EventHandler(this.buttonXOneKeyCoding_Click);
            // 
            // buttonXClose
            // 
            this.buttonXClose.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonXClose.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonXClose.Location = new System.Drawing.Point(199, 195);
            this.buttonXClose.Name = "buttonXClose";
            this.buttonXClose.Size = new System.Drawing.Size(103, 34);
            this.buttonXClose.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonXClose.TabIndex = 5;
            this.buttonXClose.Text = "关闭";
            this.buttonXClose.Click += new System.EventHandler(this.buttonXClose_Click);
            // 
            // labelX3
            // 
            // 
            // 
            // 
            this.labelX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX3.Location = new System.Drawing.Point(2, 84);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(135, 25);
            this.labelX3.TabIndex = 6;
            this.labelX3.Text = "缓冲区距离：";
            // 
            // textBoxX1
            // 
            // 
            // 
            // 
            this.textBoxX1.Border.Class = "TextBoxBorder";
            this.textBoxX1.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.textBoxX1.Location = new System.Drawing.Point(156, 84);
            this.textBoxX1.Name = "textBoxX1";
            this.textBoxX1.Size = new System.Drawing.Size(113, 25);
            this.textBoxX1.TabIndex = 7;
            this.textBoxX1.Text = "7.5";
            // 
            // comboBoxExDistance
            // 
            this.comboBoxExDistance.DisplayMember = "Text";
            this.comboBoxExDistance.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboBoxExDistance.FormattingEnabled = true;
            this.comboBoxExDistance.ItemHeight = 19;
            this.comboBoxExDistance.Location = new System.Drawing.Point(275, 84);
            this.comboBoxExDistance.Name = "comboBoxExDistance";
            this.comboBoxExDistance.Size = new System.Drawing.Size(72, 25);
            this.comboBoxExDistance.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.comboBoxExDistance.TabIndex = 8;
            // 
            // FrmOneKeyCoding
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(356, 359);
            this.Controls.Add(this.comboBoxExDistance);
            this.Controls.Add(this.textBoxX1);
            this.Controls.Add(this.labelX3);
            this.Controls.Add(this.buttonXClose);
            this.Controls.Add(this.buttonXOneKeyCoding);
            this.Controls.Add(this.comboBoxExMatLayer);
            this.Controls.Add(this.comboBoxExOriLayer);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.labelX1);
            this.DoubleBuffered = true;
            this.Name = "FrmOneKeyCoding";
            this.Text = "编码";
            this.Load += new System.EventHandler(this.FrmOneKeyCoding_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.Controls.ComboBoxEx comboBoxExOriLayer;
        private DevComponents.DotNetBar.Controls.ComboBoxEx comboBoxExMatLayer;
        private DevComponents.DotNetBar.ButtonX buttonXOneKeyCoding;
        private DevComponents.DotNetBar.ButtonX buttonXClose;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.Controls.TextBoxX textBoxX1;
        private DevComponents.DotNetBar.Controls.ComboBoxEx comboBoxExDistance;
    }
}