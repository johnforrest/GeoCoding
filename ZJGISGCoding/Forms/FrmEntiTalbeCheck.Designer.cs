namespace ZJGISGCoding.Forms
{
    partial class FrmEntiTalbeCheck
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
            this.textBoxX1 = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.buttonXPath = new DevComponents.DotNetBar.ButtonX();
            this.buttonXCheck = new DevComponents.DotNetBar.ButtonX();
            this.progressBarX1 = new DevComponents.DotNetBar.Controls.ProgressBarX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.SuspendLayout();
            // 
            // textBoxX1
            // 
            // 
            // 
            // 
            this.textBoxX1.Border.Class = "TextBoxBorder";
            this.textBoxX1.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.textBoxX1.Location = new System.Drawing.Point(13, 49);
            this.textBoxX1.Name = "textBoxX1";
            this.textBoxX1.Size = new System.Drawing.Size(307, 25);
            this.textBoxX1.TabIndex = 0;
            // 
            // labelX1
            // 
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Location = new System.Drawing.Point(13, 20);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(75, 23);
            this.labelX1.TabIndex = 1;
            this.labelX1.Text = "实体表路径";
            // 
            // buttonXPath
            // 
            this.buttonXPath.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonXPath.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonXPath.Location = new System.Drawing.Point(326, 49);
            this.buttonXPath.Name = "buttonXPath";
            this.buttonXPath.Size = new System.Drawing.Size(33, 23);
            this.buttonXPath.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonXPath.TabIndex = 2;
            this.buttonXPath.Text = "...";
            this.buttonXPath.Click += new System.EventHandler(this.buttonXPath_Click);
            // 
            // buttonXCheck
            // 
            this.buttonXCheck.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonXCheck.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonXCheck.Location = new System.Drawing.Point(117, 98);
            this.buttonXCheck.Name = "buttonXCheck";
            this.buttonXCheck.Size = new System.Drawing.Size(80, 23);
            this.buttonXCheck.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonXCheck.TabIndex = 3;
            this.buttonXCheck.Text = "查询";
            this.buttonXCheck.Click += new System.EventHandler(this.buttonXCheck_Click);
            // 
            // progressBarX1
            // 
            // 
            // 
            // 
            this.progressBarX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.progressBarX1.Location = new System.Drawing.Point(13, 155);
            this.progressBarX1.Name = "progressBarX1";
            this.progressBarX1.Size = new System.Drawing.Size(346, 23);
            this.progressBarX1.TabIndex = 4;
            this.progressBarX1.Text = "progressBarX1";
            // 
            // labelX2
            // 
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Location = new System.Drawing.Point(13, 126);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(75, 23);
            this.labelX2.TabIndex = 5;
            this.labelX2.Text = "进度条：";
            // 
            // FrmEntiTalbeCheck
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(386, 194);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.progressBarX1);
            this.Controls.Add(this.buttonXCheck);
            this.Controls.Add(this.buttonXPath);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.textBoxX1);
            this.DoubleBuffered = true;
            this.Name = "FrmEntiTalbeCheck";
            this.Text = "实体表编码唯一性检查";
            this.Load += new System.EventHandler(this.FrmEntiTalbeCheck_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.TextBoxX textBoxX1;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.ButtonX buttonXPath;
        private DevComponents.DotNetBar.ButtonX buttonXCheck;
        private DevComponents.DotNetBar.Controls.ProgressBarX progressBarX1;
        private DevComponents.DotNetBar.LabelX labelX2;
    }
}