namespace ZJGISEntiTable.Froms
{
    partial class FrmEntitySearch
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
            this.EntityCode = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.VersionTimeSld = new System.Windows.Forms.TrackBar();
            this.label3 = new System.Windows.Forms.Label();
            this.EntityTbPath = new System.Windows.Forms.TextBox();
            this.EntityTbPathBtn = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.VersionTbPath = new System.Windows.Forms.TextBox();
            this.VersionTbPathBtn = new System.Windows.Forms.Button();
            this.EntityCodeBtn = new System.Windows.Forms.Button();
            this.EntityTbName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.VersionTimeSld)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 220);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(127, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "请输入实体编码：";
            // 
            // EntityCode
            // 
            this.EntityCode.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.EntityCode.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.EntityCode.Location = new System.Drawing.Point(160, 216);
            this.EntityCode.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.EntityCode.Name = "EntityCode";
            this.EntityCode.Size = new System.Drawing.Size(276, 25);
            this.EntityCode.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 281);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "版本时间：";
            // 
            // VersionTimeSld
            // 
            this.VersionTimeSld.LargeChange = 2;
            this.VersionTimeSld.Location = new System.Drawing.Point(149, 262);
            this.VersionTimeSld.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.VersionTimeSld.Name = "VersionTimeSld";
            this.VersionTimeSld.Size = new System.Drawing.Size(451, 56);
            this.VersionTimeSld.TabIndex = 3;
            this.VersionTimeSld.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.VersionTimeSld.ValueChanged += new System.EventHandler(this.VersionTimeSld_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 98);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "实体表路径：";
            // 
            // EntityTbPath
            // 
            this.EntityTbPath.Location = new System.Drawing.Point(160, 95);
            this.EntityTbPath.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.EntityTbPath.Name = "EntityTbPath";
            this.EntityTbPath.Size = new System.Drawing.Size(276, 25);
            this.EntityTbPath.TabIndex = 5;
            // 
            // EntityTbPathBtn
            // 
            this.EntityTbPathBtn.Location = new System.Drawing.Point(463, 92);
            this.EntityTbPathBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.EntityTbPathBtn.Name = "EntityTbPathBtn";
            this.EntityTbPathBtn.Size = new System.Drawing.Size(84, 31);
            this.EntityTbPathBtn.TabIndex = 6;
            this.EntityTbPathBtn.Text = " ...";
            this.EntityTbPathBtn.UseVisualStyleBackColor = true;
            this.EntityTbPathBtn.Click += new System.EventHandler(this.EntityTbPathBtn_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 159);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(127, 15);
            this.label4.TabIndex = 7;
            this.label4.Text = "版本记录表路径：";
            // 
            // VersionTbPath
            // 
            this.VersionTbPath.Location = new System.Drawing.Point(160, 155);
            this.VersionTbPath.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.VersionTbPath.Name = "VersionTbPath";
            this.VersionTbPath.Size = new System.Drawing.Size(276, 25);
            this.VersionTbPath.TabIndex = 8;
            // 
            // VersionTbPathBtn
            // 
            this.VersionTbPathBtn.Location = new System.Drawing.Point(463, 152);
            this.VersionTbPathBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.VersionTbPathBtn.Name = "VersionTbPathBtn";
            this.VersionTbPathBtn.Size = new System.Drawing.Size(84, 31);
            this.VersionTbPathBtn.TabIndex = 9;
            this.VersionTbPathBtn.Text = " ...";
            this.VersionTbPathBtn.UseVisualStyleBackColor = true;
            this.VersionTbPathBtn.Click += new System.EventHandler(this.VersionTbPathBtn_Click);
            // 
            // EntityCodeBtn
            // 
            this.EntityCodeBtn.Location = new System.Drawing.Point(463, 214);
            this.EntityCodeBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.EntityCodeBtn.Name = "EntityCodeBtn";
            this.EntityCodeBtn.Size = new System.Drawing.Size(84, 31);
            this.EntityCodeBtn.TabIndex = 10;
            this.EntityCodeBtn.Text = "查找实体";
            this.EntityCodeBtn.UseVisualStyleBackColor = true;
            this.EntityCodeBtn.Click += new System.EventHandler(this.EntityCodeBtn_Click);
            // 
            // EntityTbName
            // 
            this.EntityTbName.Location = new System.Drawing.Point(160, 32);
            this.EntityTbName.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.EntityTbName.Name = "EntityTbName";
            this.EntityTbName.Size = new System.Drawing.Size(276, 25);
            this.EntityTbName.TabIndex = 12;
            this.EntityTbName.Text = "EntiTable";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 36);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(97, 15);
            this.label5.TabIndex = 11;
            this.label5.Text = "实体表名称：";
            // 
            // FrmEntitySearch
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(620, 378);
            this.Controls.Add(this.EntityTbName);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.EntityCodeBtn);
            this.Controls.Add(this.VersionTbPathBtn);
            this.Controls.Add(this.VersionTbPath);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.EntityTbPathBtn);
            this.Controls.Add(this.EntityTbPath);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.EntityCode);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.VersionTimeSld);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FrmEntitySearch";
            this.Text = "实体查询";
            this.Load += new System.EventHandler(this.FrmEntitySearch_Load);
            ((System.ComponentModel.ISupportInitialize)(this.VersionTimeSld)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox EntityCode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TrackBar VersionTimeSld;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox EntityTbPath;
        private System.Windows.Forms.Button EntityTbPathBtn;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox VersionTbPath;
        private System.Windows.Forms.Button VersionTbPathBtn;
        private System.Windows.Forms.Button EntityCodeBtn;
        private System.Windows.Forms.TextBox EntityTbName;
        private System.Windows.Forms.Label label5;
    }
}