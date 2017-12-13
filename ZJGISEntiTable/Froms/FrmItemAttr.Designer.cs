namespace ZJGISEntiTable.Froms
{
    partial class FrmItemAttr
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmItemAttr));
            this.dataChild = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataChild)).BeginInit();
            this.SuspendLayout();
            // 
            // dataChild
            // 
            this.dataChild.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataChild.BackgroundColor = System.Drawing.Color.White;
            this.dataChild.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataChild.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataChild.Location = new System.Drawing.Point(0, 0);
            this.dataChild.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dataChild.Name = "dataChild";
            this.dataChild.RowTemplate.Height = 23;
            this.dataChild.Size = new System.Drawing.Size(312, 540);
            this.dataChild.TabIndex = 0;
            // 
            // frmItemAttr
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(312, 540);
            this.Controls.Add(this.dataChild);
            this.DoubleBuffered = true;
            this.EnableGlass = false;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "frmItemAttr";
            this.Text = "实体属性";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.dataChild)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.DataGridView dataChild;
    }
}