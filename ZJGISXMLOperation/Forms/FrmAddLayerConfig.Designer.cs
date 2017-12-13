namespace ZJGISXMLOperation.Forms
{
    partial class FrmAddLayerConfig
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
            this.layerName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.endVersion = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.Savebt = new System.Windows.Forms.Button();
            this.Cancelbt = new System.Windows.Forms.Button();
            this.sourceName = new System.Windows.Forms.ComboBox();
            this.nameField = new System.Windows.Forms.TextBox();
            this.startVersion = new System.Windows.Forms.TextBox();
            this.guid = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.sourceType = new System.Windows.Forms.ComboBox();
            this.shapeType = new System.Windows.Forms.ComboBox();
            this.entiID = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 44);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "图层名称";
            // 
            // layerName
            // 
            this.layerName.Location = new System.Drawing.Point(88, 41);
            this.layerName.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.layerName.Name = "layerName";
            this.layerName.Size = new System.Drawing.Size(169, 25);
            this.layerName.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 88);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 15);
            this.label2.TabIndex = 0;
            this.label2.Text = "名称字段";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 128);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 15);
            this.label3.TabIndex = 0;
            this.label3.Text = "起始版本";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 170);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 15);
            this.label4.TabIndex = 0;
            this.label4.Text = "终止版本";
            // 
            // endVersion
            // 
            this.endVersion.Location = new System.Drawing.Point(88, 167);
            this.endVersion.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.endVersion.Name = "endVersion";
            this.endVersion.Size = new System.Drawing.Size(169, 25);
            this.endVersion.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 213);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 15);
            this.label5.TabIndex = 0;
            this.label5.Text = "图元字段";
            // 
            // Savebt
            // 
            this.Savebt.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Savebt.Location = new System.Drawing.Point(297, 60);
            this.Savebt.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Savebt.Name = "Savebt";
            this.Savebt.Size = new System.Drawing.Size(100, 29);
            this.Savebt.TabIndex = 3;
            this.Savebt.Text = "保存";
            this.Savebt.UseVisualStyleBackColor = true;
            this.Savebt.Click += new System.EventHandler(this.Savebt_Click);
            // 
            // Cancelbt
            // 
            this.Cancelbt.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancelbt.Location = new System.Drawing.Point(297, 295);
            this.Cancelbt.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Cancelbt.Name = "Cancelbt";
            this.Cancelbt.Size = new System.Drawing.Size(100, 29);
            this.Cancelbt.TabIndex = 4;
            this.Cancelbt.Text = "取消";
            this.Cancelbt.UseVisualStyleBackColor = true;
            this.Cancelbt.Click += new System.EventHandler(this.Cancelbt_Click);
            // 
            // sourceName
            // 
            this.sourceName.FormattingEnabled = true;
            this.sourceName.Location = new System.Drawing.Point(88, 256);
            this.sourceName.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.sourceName.Name = "sourceName";
            this.sourceName.Size = new System.Drawing.Size(169, 23);
            this.sourceName.TabIndex = 5;
            // 
            // nameField
            // 
            this.nameField.Location = new System.Drawing.Point(88, 85);
            this.nameField.Name = "nameField";
            this.nameField.Size = new System.Drawing.Size(169, 25);
            this.nameField.TabIndex = 1;
            // 
            // startVersion
            // 
            this.startVersion.Location = new System.Drawing.Point(88, 125);
            this.startVersion.Name = "startVersion";
            this.startVersion.Size = new System.Drawing.Size(169, 25);
            this.startVersion.TabIndex = 2;
            // 
            // guid
            // 
            this.guid.Location = new System.Drawing.Point(88, 210);
            this.guid.Name = "guid";
            this.guid.Size = new System.Drawing.Size(169, 25);
            this.guid.TabIndex = 4;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 258);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 15);
            this.label6.TabIndex = 9;
            this.label6.Text = "图层来源";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(13, 301);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(67, 15);
            this.label7.TabIndex = 10;
            this.label7.Text = "图层类型";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(13, 346);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(67, 15);
            this.label8.TabIndex = 11;
            this.label8.Text = "编码字段";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(13, 387);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(67, 15);
            this.label9.TabIndex = 12;
            this.label9.Text = "几何形状";
            // 
            // sourceType
            // 
            this.sourceType.FormattingEnabled = true;
            this.sourceType.Location = new System.Drawing.Point(88, 299);
            this.sourceType.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.sourceType.Name = "sourceType";
            this.sourceType.Size = new System.Drawing.Size(169, 23);
            this.sourceType.TabIndex = 6;
            // 
            // shapeType
            // 
            this.shapeType.FormattingEnabled = true;
            this.shapeType.Location = new System.Drawing.Point(88, 385);
            this.shapeType.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.shapeType.Name = "shapeType";
            this.shapeType.Size = new System.Drawing.Size(169, 23);
            this.shapeType.TabIndex = 8;
            // 
            // entiID
            // 
            this.entiID.Location = new System.Drawing.Point(88, 343);
            this.entiID.Name = "entiID";
            this.entiID.Size = new System.Drawing.Size(169, 25);
            this.entiID.TabIndex = 7;
            // 
            // Form2
            // 
            this.AcceptButton = this.Savebt;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Cancelbt;
            this.ClientSize = new System.Drawing.Size(411, 426);
            this.Controls.Add(this.entiID);
            this.Controls.Add(this.shapeType);
            this.Controls.Add(this.sourceType);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.guid);
            this.Controls.Add(this.startVersion);
            this.Controls.Add(this.nameField);
            this.Controls.Add(this.sourceName);
            this.Controls.Add(this.Cancelbt);
            this.Controls.Add(this.Savebt);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.endVersion);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.layerName);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "Form2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form2";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.TextBox layerName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.TextBox endVersion;
        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.Button Savebt;
        private System.Windows.Forms.Button Cancelbt;
        public System.Windows.Forms.ComboBox sourceName;
        public System.Windows.Forms.TextBox nameField;
        public System.Windows.Forms.TextBox startVersion;
        public System.Windows.Forms.TextBox guid;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        public System.Windows.Forms.ComboBox sourceType;
        public System.Windows.Forms.ComboBox shapeType;
        public System.Windows.Forms.TextBox entiID;
    }
}