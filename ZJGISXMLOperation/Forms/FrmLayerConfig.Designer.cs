namespace ZJGISXMLOperation.Forms
{
    partial class FrmLayerConfig
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.Editbt = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.Addbt = new System.Windows.Forms.Button();
            this.Delbt = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Editbt
            // 
            this.Editbt.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.Editbt.Location = new System.Drawing.Point(296, 0);
            this.Editbt.Margin = new System.Windows.Forms.Padding(4);
            this.Editbt.Name = "Editbt";
            this.Editbt.Size = new System.Drawing.Size(100, 29);
            this.Editbt.TabIndex = 1;
            this.Editbt.Text = "编辑";
            this.Editbt.UseVisualStyleBackColor = true;
            this.Editbt.Click += new System.EventHandler(this.Editbt_Click);
            // 
            // listView1
            // 
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Margin = new System.Windows.Forms.Padding(4);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(778, 466);
            this.listView1.TabIndex = 2;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.KeyROpt);
            this.listView1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.MouseROpt);
            // 
            // Addbt
            // 
            this.Addbt.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.Addbt.Location = new System.Drawing.Point(117, 0);
            this.Addbt.Margin = new System.Windows.Forms.Padding(4);
            this.Addbt.Name = "Addbt";
            this.Addbt.Size = new System.Drawing.Size(100, 29);
            this.Addbt.TabIndex = 1;
            this.Addbt.Text = "新增";
            this.Addbt.UseVisualStyleBackColor = true;
            this.Addbt.Click += new System.EventHandler(this.Addbt_Click);
            // 
            // Delbt
            // 
            this.Delbt.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.Delbt.Location = new System.Drawing.Point(473, 1);
            this.Delbt.Margin = new System.Windows.Forms.Padding(4);
            this.Delbt.Name = "Delbt";
            this.Delbt.Size = new System.Drawing.Size(100, 29);
            this.Delbt.TabIndex = 1;
            this.Delbt.Text = "删除";
            this.Delbt.UseVisualStyleBackColor = true;
            this.Delbt.Click += new System.EventHandler(this.Delbt_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.Editbt);
            this.splitContainer1.Panel1.Controls.Add(this.Addbt);
            this.splitContainer1.Panel1.Controls.Add(this.Delbt);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.listView1);
            this.splitContainer1.Size = new System.Drawing.Size(778, 505);
            this.splitContainer1.SplitterDistance = 35;
            this.splitContainer1.TabIndex = 3;
            // 
            // FrmLayerConfig
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(778, 505);
            this.Controls.Add(this.splitContainer1);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FrmLayerConfig";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "图层配置";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Editbt;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Button Addbt;
        private System.Windows.Forms.Button Delbt;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}

