namespace ZJGISEntiTable.Froms
{
    partial class FrmEntiDB
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
            this.txbGdbPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnOpenPath = new System.Windows.Forms.Button();
            this.btnCreateEntiTable = new System.Windows.Forms.Button();
            this.btnOpenVersionPath = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txbVersionPath = new System.Windows.Forms.TextBox();
            this.progressBarXEntiDB = new DevComponents.DotNetBar.Controls.ProgressBarX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.SuspendLayout();
            // 
            // txbGdbPath
            // 
            this.txbGdbPath.Location = new System.Drawing.Point(17, 44);
            this.txbGdbPath.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txbGdbPath.Name = "txbGdbPath";
            this.txbGdbPath.Size = new System.Drawing.Size(341, 25);
            this.txbGdbPath.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "实体表路径：";
            // 
            // btnOpenPath
            // 
            this.btnOpenPath.Location = new System.Drawing.Point(363, 44);
            this.btnOpenPath.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnOpenPath.Name = "btnOpenPath";
            this.btnOpenPath.Size = new System.Drawing.Size(75, 25);
            this.btnOpenPath.TabIndex = 4;
            this.btnOpenPath.Text = "...";
            this.btnOpenPath.UseVisualStyleBackColor = true;
            this.btnOpenPath.Click += new System.EventHandler(this.btnOpenPath_Click);
            // 
            // btnCreateEntiTable
            // 
            this.btnCreateEntiTable.Location = new System.Drawing.Point(177, 168);
            this.btnCreateEntiTable.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCreateEntiTable.Name = "btnCreateEntiTable";
            this.btnCreateEntiTable.Size = new System.Drawing.Size(75, 25);
            this.btnCreateEntiTable.TabIndex = 5;
            this.btnCreateEntiTable.Text = "创建表";
            this.btnCreateEntiTable.UseVisualStyleBackColor = true;
            this.btnCreateEntiTable.Click += new System.EventHandler(this.btnCreateEntiTable_Click);
            // 
            // btnOpenVersionPath
            // 
            this.btnOpenVersionPath.Location = new System.Drawing.Point(363, 107);
            this.btnOpenVersionPath.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnOpenVersionPath.Name = "btnOpenVersionPath";
            this.btnOpenVersionPath.Size = new System.Drawing.Size(75, 25);
            this.btnOpenVersionPath.TabIndex = 8;
            this.btnOpenVersionPath.Text = "...";
            this.btnOpenVersionPath.UseVisualStyleBackColor = true;
            this.btnOpenVersionPath.Click += new System.EventHandler(this.btnOpenVersionPath_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(127, 15);
            this.label3.TabIndex = 7;
            this.label3.Text = "版本记录表路径：";
            // 
            // txbVersionPath
            // 
            this.txbVersionPath.Location = new System.Drawing.Point(17, 109);
            this.txbVersionPath.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txbVersionPath.Name = "txbVersionPath";
            this.txbVersionPath.Size = new System.Drawing.Size(341, 25);
            this.txbVersionPath.TabIndex = 6;
            // 
            // progressBarXEntiDB
            // 
            // 
            // 
            // 
            this.progressBarXEntiDB.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.progressBarXEntiDB.Location = new System.Drawing.Point(91, 241);
            this.progressBarXEntiDB.Name = "progressBarXEntiDB";
            this.progressBarXEntiDB.Size = new System.Drawing.Size(338, 23);
            this.progressBarXEntiDB.TabIndex = 9;
            this.progressBarXEntiDB.Text = "progressBarX1";
            // 
            // labelX1
            // 
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Location = new System.Drawing.Point(17, 242);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(75, 23);
            this.labelX1.TabIndex = 10;
            this.labelX1.Text = "进度条：";
            // 
            // FrmEntiDB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(466, 289);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.progressBarXEntiDB);
            this.Controls.Add(this.btnOpenVersionPath);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txbVersionPath);
            this.Controls.Add(this.btnCreateEntiTable);
            this.Controls.Add(this.btnOpenPath);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txbGdbPath);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FrmEntiDB";
            this.Text = "创建实体表";
            this.Load += new System.EventHandler(this.FrmEntiDB_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txbGdbPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnOpenPath;
        private System.Windows.Forms.Button btnCreateEntiTable;
        private System.Windows.Forms.Button btnOpenVersionPath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txbVersionPath;
        private DevComponents.DotNetBar.Controls.ProgressBarX progressBarXEntiDB;
        private DevComponents.DotNetBar.LabelX labelX1;
    }
}