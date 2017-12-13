namespace ZJGISEntiTable.Froms
{
    partial class FrmEntiUpdate
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
            this.label1 = new System.Windows.Forms.Label();
            this.toUpdateBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.updatedBox = new System.Windows.Forms.ComboBox();
            this.update = new System.Windows.Forms.Button();
            this.EntityTbName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.VersionTbPathBtn = new System.Windows.Forms.Button();
            this.VersionTbPath = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.EntityTbPathBtn = new System.Windows.Forms.Button();
            this.EntityTbPath = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.progressBarXEntiUpdate = new DevComponents.DotNetBar.Controls.ProgressBarX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 271);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "待更新图层：";
            // 
            // toUpdateBox
            // 
            this.toUpdateBox.FormattingEnabled = true;
            this.toUpdateBox.Location = new System.Drawing.Point(167, 265);
            this.toUpdateBox.Margin = new System.Windows.Forms.Padding(4);
            this.toUpdateBox.Name = "toUpdateBox";
            this.toUpdateBox.Size = new System.Drawing.Size(276, 23);
            this.toUpdateBox.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 210);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "更新后图层：";
            // 
            // updatedBox
            // 
            this.updatedBox.FormattingEnabled = true;
            this.updatedBox.Location = new System.Drawing.Point(167, 205);
            this.updatedBox.Margin = new System.Windows.Forms.Padding(4);
            this.updatedBox.Name = "updatedBox";
            this.updatedBox.Size = new System.Drawing.Size(276, 23);
            this.updatedBox.TabIndex = 3;
            // 
            // update
            // 
            this.update.Location = new System.Drawing.Point(257, 324);
            this.update.Margin = new System.Windows.Forms.Padding(4);
            this.update.Name = "update";
            this.update.Size = new System.Drawing.Size(87, 31);
            this.update.TabIndex = 4;
            this.update.Text = "开始";
            this.update.UseVisualStyleBackColor = true;
            this.update.Click += new System.EventHandler(this.update_Click);
            // 
            // EntityTbName
            // 
            this.EntityTbName.Location = new System.Drawing.Point(167, 26);
            this.EntityTbName.Margin = new System.Windows.Forms.Padding(4);
            this.EntityTbName.Name = "EntityTbName";
            this.EntityTbName.Size = new System.Drawing.Size(276, 25);
            this.EntityTbName.TabIndex = 20;
            this.EntityTbName.Text = "EntiTable";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(24, 30);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(97, 15);
            this.label5.TabIndex = 19;
            this.label5.Text = "实体表名称：";
            // 
            // VersionTbPathBtn
            // 
            this.VersionTbPathBtn.Location = new System.Drawing.Point(469, 146);
            this.VersionTbPathBtn.Margin = new System.Windows.Forms.Padding(4);
            this.VersionTbPathBtn.Name = "VersionTbPathBtn";
            this.VersionTbPathBtn.Size = new System.Drawing.Size(87, 31);
            this.VersionTbPathBtn.TabIndex = 18;
            this.VersionTbPathBtn.Text = " ...";
            this.VersionTbPathBtn.UseVisualStyleBackColor = true;
            this.VersionTbPathBtn.Click += new System.EventHandler(this.VersionTbPathBtn_Click);
            // 
            // VersionTbPath
            // 
            this.VersionTbPath.Location = new System.Drawing.Point(167, 149);
            this.VersionTbPath.Margin = new System.Windows.Forms.Padding(4);
            this.VersionTbPath.Name = "VersionTbPath";
            this.VersionTbPath.Size = new System.Drawing.Size(276, 25);
            this.VersionTbPath.TabIndex = 17;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 152);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(127, 15);
            this.label4.TabIndex = 16;
            this.label4.Text = "版本记录表路径：";
            // 
            // EntityTbPathBtn
            // 
            this.EntityTbPathBtn.Location = new System.Drawing.Point(469, 86);
            this.EntityTbPathBtn.Margin = new System.Windows.Forms.Padding(4);
            this.EntityTbPathBtn.Name = "EntityTbPathBtn";
            this.EntityTbPathBtn.Size = new System.Drawing.Size(87, 31);
            this.EntityTbPathBtn.TabIndex = 15;
            this.EntityTbPathBtn.Text = " ...";
            this.EntityTbPathBtn.UseVisualStyleBackColor = true;
            this.EntityTbPathBtn.Click += new System.EventHandler(this.EntityTbPathBtn_Click);
            // 
            // EntityTbPath
            // 
            this.EntityTbPath.Location = new System.Drawing.Point(167, 89);
            this.EntityTbPath.Margin = new System.Windows.Forms.Padding(4);
            this.EntityTbPath.Name = "EntityTbPath";
            this.EntityTbPath.Size = new System.Drawing.Size(276, 25);
            this.EntityTbPath.TabIndex = 14;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(24, 91);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 15);
            this.label3.TabIndex = 13;
            this.label3.Text = "实体表路径：";
            // 
            // progressBarXEntiUpdate
            // 
            // 
            // 
            // 
            this.progressBarXEntiUpdate.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.progressBarXEntiUpdate.Location = new System.Drawing.Point(125, 391);
            this.progressBarXEntiUpdate.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.progressBarXEntiUpdate.Name = "progressBarXEntiUpdate";
            this.progressBarXEntiUpdate.Size = new System.Drawing.Size(419, 22);
            this.progressBarXEntiUpdate.TabIndex = 21;
            this.progressBarXEntiUpdate.Text = "progressBarX1";
            // 
            // labelX1
            // 
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Location = new System.Drawing.Point(27, 391);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(94, 23);
            this.labelX1.TabIndex = 22;
            this.labelX1.Text = "进度条：";
            // 
            // FrmEntiUpdate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(573, 434);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.progressBarXEntiUpdate);
            this.Controls.Add(this.EntityTbName);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.VersionTbPathBtn);
            this.Controls.Add(this.VersionTbPath);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.EntityTbPathBtn);
            this.Controls.Add(this.EntityTbPath);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.update);
            this.Controls.Add(this.updatedBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.toUpdateBox);
            this.Controls.Add(this.label1);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FrmEntiUpdate";
            this.Text = "驱动更新机制";
            this.Load += new System.EventHandler(this.FrmEntiUpdate_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox toUpdateBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox updatedBox;
        private System.Windows.Forms.Button update;
        private System.Windows.Forms.TextBox EntityTbName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button VersionTbPathBtn;
        private System.Windows.Forms.TextBox VersionTbPath;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button EntityTbPathBtn;
        private System.Windows.Forms.TextBox EntityTbPath;
        private System.Windows.Forms.Label label3;
        private DevComponents.DotNetBar.Controls.ProgressBarX progressBarXEntiUpdate;
        private DevComponents.DotNetBar.LabelX labelX1;
    }
}