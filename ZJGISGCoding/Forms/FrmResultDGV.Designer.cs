namespace ZJGISGCoding.Forms
{
    partial class FrmResultDGV
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
            this.dataChild = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataChild)).BeginInit();
            this.SuspendLayout();
            // 
            // dataChild
            // 
            this.dataChild.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataChild.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataChild.Location = new System.Drawing.Point(0, 0);
            this.dataChild.Name = "dataChild";
            this.dataChild.RowTemplate.Height = 27;
            this.dataChild.Size = new System.Drawing.Size(282, 253);
            this.dataChild.TabIndex = 0;
            // 
            // FrmResultDGV
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(282, 253);
            this.Controls.Add(this.dataChild);
            this.DoubleBuffered = true;
            this.Name = "FrmResultDGV";
            this.Text = "重复的实体记录";
            ((System.ComponentModel.ISupportInitialize)(this.dataChild)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.DataGridView dataChild;
    }
}