namespace ZJGISEntiTable.Froms
{
    partial class FrmUpdateResult
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.updateState = new System.Windows.Forms.TreeView();
            this.updateContent = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.updateContent)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.updateState);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.updateContent);
            this.splitContainer1.Size = new System.Drawing.Size(1235, 178);
            this.splitContainer1.SplitterDistance = 106;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 0;
            // 
            // updateState
            // 
            this.updateState.Dock = System.Windows.Forms.DockStyle.Fill;
            this.updateState.Location = new System.Drawing.Point(0, 0);
            this.updateState.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.updateState.Name = "updateState";
            this.updateState.Size = new System.Drawing.Size(106, 178);
            this.updateState.TabIndex = 0;
            this.updateState.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.updateState_NodeMouseClick);
            // 
            // updateContent
            // 
            this.updateContent.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.updateContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.updateContent.Location = new System.Drawing.Point(0, 0);
            this.updateContent.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.updateContent.Name = "updateContent";
            this.updateContent.RowTemplate.Height = 23;
            this.updateContent.Size = new System.Drawing.Size(1124, 178);
            this.updateContent.TabIndex = 0;
            this.updateContent.RowHeaderMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.updateContent_RowHeaderMouseDoubleClick);
            // 
            // FrmUpdateResult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1235, 178);
            this.Controls.Add(this.splitContainer1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FrmUpdateResult";
            this.Text = "更新结果";
            this.TopMost = true;
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.updateContent)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView updateState;
        private System.Windows.Forms.DataGridView updateContent;
    }
}