namespace ZJGISLayerManager
{
    partial class FrmSetLabel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSetLabel));
            this.LabelX7 = new DevComponents.DotNetBar.LabelX();
            this.txtMaxScale = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.LabelX6 = new DevComponents.DotNetBar.LabelX();
            this.btnOK = new DevComponents.DotNetBar.ButtonX();
            this.GroupBox3 = new System.Windows.Forms.GroupBox();
            this.txtMinScale = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.LabelX5 = new DevComponents.DotNetBar.LabelX();
            this.LabelX4 = new DevComponents.DotNetBar.LabelX();
            this.optUserDefined = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.optScaleNameWithLayer = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.GroupBox4 = new System.Windows.Forms.GroupBox();
            this.btnPosition = new DevComponents.DotNetBar.ButtonX();
            this.LabelX3 = new DevComponents.DotNetBar.LabelX();
            this.btnFontUnderline = new DevComponents.DotNetBar.ButtonItem();
            this.btnFontItalic = new DevComponents.DotNetBar.ButtonItem();
            this.cmbFieldName = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.btnFontBold = new DevComponents.DotNetBar.ButtonItem();
            this.LabelX1 = new DevComponents.DotNetBar.LabelX();
            this.GroupBox2 = new System.Windows.Forms.GroupBox();
            this.btnColorPick = new DevComponents.DotNetBar.ColorPickerButton();
            this.cmbFontSize = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.cmbFontName = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.lblText = new DevComponents.DotNetBar.LabelX();
            this.Bar1 = new DevComponents.DotNetBar.Bar();
            this.btnExpresion = new DevComponents.DotNetBar.ButtonX();
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.GroupBox3.SuspendLayout();
            this.GroupBox4.SuspendLayout();
            this.GroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Bar1)).BeginInit();
            this.GroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // LabelX7
            // 
            this.LabelX7.Location = new System.Drawing.Point(189, 106);
            this.LabelX7.Name = "LabelX7";
            this.LabelX7.Size = new System.Drawing.Size(30, 24);
            this.LabelX7.TabIndex = 7;
            this.LabelX7.Text = "1：";
            // 
            // txtMaxScale
            // 
            // 
            // 
            // 
            this.txtMaxScale.Border.Class = "TextBoxBorder";
            this.txtMaxScale.Location = new System.Drawing.Point(219, 106);
            this.txtMaxScale.Name = "txtMaxScale";
            this.txtMaxScale.Size = new System.Drawing.Size(95, 21);
            this.txtMaxScale.TabIndex = 5;
            this.txtMaxScale.TextChanged += new System.EventHandler(this.txtMaxScale_TextChanged);
            this.txtMaxScale.Leave += new System.EventHandler(this.txtMaxScale_Leave);
            // 
            // LabelX6
            // 
            this.LabelX6.Location = new System.Drawing.Point(17, 106);
            this.LabelX6.Name = "LabelX6";
            this.LabelX6.Size = new System.Drawing.Size(30, 26);
            this.LabelX6.TabIndex = 6;
            this.LabelX6.Text = "1：";
            // 
            // btnOK
            // 
            this.btnOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOK.Location = new System.Drawing.Point(168, 427);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(63, 25);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "确 定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // GroupBox3
            // 
            this.GroupBox3.Controls.Add(this.LabelX7);
            this.GroupBox3.Controls.Add(this.LabelX6);
            this.GroupBox3.Controls.Add(this.txtMaxScale);
            this.GroupBox3.Controls.Add(this.txtMinScale);
            this.GroupBox3.Controls.Add(this.LabelX5);
            this.GroupBox3.Controls.Add(this.LabelX4);
            this.GroupBox3.Controls.Add(this.optUserDefined);
            this.GroupBox3.Controls.Add(this.optScaleNameWithLayer);
            this.GroupBox3.Location = new System.Drawing.Point(11, 279);
            this.GroupBox3.Name = "GroupBox3";
            this.GroupBox3.Size = new System.Drawing.Size(331, 142);
            this.GroupBox3.TabIndex = 10;
            this.GroupBox3.TabStop = false;
            this.GroupBox3.Text = "参考比例尺设置";
            // 
            // txtMinScale
            // 
            // 
            // 
            // 
            this.txtMinScale.Border.Class = "TextBoxBorder";
            this.txtMinScale.Location = new System.Drawing.Point(48, 106);
            this.txtMinScale.Name = "txtMinScale";
            this.txtMinScale.Size = new System.Drawing.Size(95, 21);
            this.txtMinScale.TabIndex = 4;
            this.txtMinScale.TextChanged += new System.EventHandler(this.txtMinScale_TextChanged);
            this.txtMinScale.Leave += new System.EventHandler(this.txtMinScale_Leave);
            // 
            // LabelX5
            // 
            this.LabelX5.Location = new System.Drawing.Point(177, 81);
            this.LabelX5.Name = "LabelX5";
            this.LabelX5.Size = new System.Drawing.Size(97, 19);
            this.LabelX5.TabIndex = 3;
            this.LabelX5.Text = "最大比例尺：";
            // 
            // LabelX4
            // 
            this.LabelX4.Location = new System.Drawing.Point(17, 79);
            this.LabelX4.Name = "LabelX4";
            this.LabelX4.Size = new System.Drawing.Size(94, 22);
            this.LabelX4.TabIndex = 2;
            this.LabelX4.Text = "最小比例尺：";
            // 
            // optUserDefined
            // 
            this.optUserDefined.CheckBoxStyle = DevComponents.DotNetBar.eCheckBoxStyle.RadioButton;
            this.optUserDefined.Location = new System.Drawing.Point(22, 46);
            this.optUserDefined.Name = "optUserDefined";
            this.optUserDefined.Size = new System.Drawing.Size(182, 21);
            this.optUserDefined.TabIndex = 1;
            this.optUserDefined.Text = "自定义显示注记的参考比例尺的范围";
            this.optUserDefined.CheckedChanged += new System.EventHandler(this.optUserDefined_CheckedChanged);
            // 
            // optScaleNameWithLayer
            // 
            this.optScaleNameWithLayer.CheckBoxStyle = DevComponents.DotNetBar.eCheckBoxStyle.RadioButton;
            this.optScaleNameWithLayer.Location = new System.Drawing.Point(22, 20);
            this.optScaleNameWithLayer.Name = "optScaleNameWithLayer";
            this.optScaleNameWithLayer.Size = new System.Drawing.Size(179, 20);
            this.optScaleNameWithLayer.TabIndex = 0;
            this.optScaleNameWithLayer.Text = "与当前层的参考比例尺相同";
            this.optScaleNameWithLayer.CheckedChanged += new System.EventHandler(this.optScaleNameWithLayer_CheckedChanged);
            // 
            // GroupBox4
            // 
            this.GroupBox4.Controls.Add(this.btnPosition);
            this.GroupBox4.Controls.Add(this.LabelX3);
            this.GroupBox4.Location = new System.Drawing.Point(11, 199);
            this.GroupBox4.Name = "GroupBox4";
            this.GroupBox4.Size = new System.Drawing.Size(331, 68);
            this.GroupBox4.TabIndex = 11;
            this.GroupBox4.TabStop = false;
            this.GroupBox4.Text = "位置属性相关";
            // 
            // btnPosition
            // 
            this.btnPosition.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnPosition.Location = new System.Drawing.Point(250, 29);
            this.btnPosition.Name = "btnPosition";
            this.btnPosition.Size = new System.Drawing.Size(66, 28);
            this.btnPosition.TabIndex = 1;
            this.btnPosition.Text = "设    置";
            this.btnPosition.Click += new System.EventHandler(this.btnPosition_Click);
            // 
            // LabelX3
            // 
            this.LabelX3.Location = new System.Drawing.Point(48, 20);
            this.LabelX3.Name = "LabelX3";
            this.LabelX3.Size = new System.Drawing.Size(183, 37);
            this.LabelX3.TabIndex = 0;
            this.LabelX3.Text = "包括标注与要素的相对位置，\r\n偏转方向等属性。";
            // 
            // btnFontUnderline
            // 
            this.btnFontUnderline.FontBold = true;
            this.btnFontUnderline.FontUnderline = true;
            this.btnFontUnderline.ImagePaddingHorizontal = 8;
            this.btnFontUnderline.Name = "btnFontUnderline";
            this.btnFontUnderline.Text = "U";
            this.btnFontUnderline.Click += new System.EventHandler(this.btnFontUnderline_Click);
            // 
            // btnFontItalic
            // 
            this.btnFontItalic.FontBold = true;
            this.btnFontItalic.FontItalic = true;
            this.btnFontItalic.ImagePaddingHorizontal = 8;
            this.btnFontItalic.Name = "btnFontItalic";
            this.btnFontItalic.Text = "I";
            this.btnFontItalic.Click += new System.EventHandler(this.btnFontItalic_Click);
            // 
            // cmbFieldName
            // 
            this.cmbFieldName.DisplayMember = "Text";
            this.cmbFieldName.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbFieldName.FormattingEnabled = true;
            this.cmbFieldName.Location = new System.Drawing.Point(80, 34);
            this.cmbFieldName.Name = "cmbFieldName";
            this.cmbFieldName.Size = new System.Drawing.Size(141, 22);
            this.cmbFieldName.TabIndex = 2;
            this.cmbFieldName.SelectedIndexChanged += new System.EventHandler(this.cmbFieldName_SelectedIndexChanged);
            this.cmbFieldName.SelectedValueChanged += new System.EventHandler(this.cmbFieldName_SelectedValueChanged);
            this.cmbFieldName.Click += new System.EventHandler(this.cmbFieldName_Click);
            // 
            // btnFontBold
            // 
            this.btnFontBold.FontBold = true;
            this.btnFontBold.ImagePaddingHorizontal = 8;
            this.btnFontBold.Name = "btnFontBold";
            this.btnFontBold.Text = "B";
            this.btnFontBold.Click += new System.EventHandler(this.btnFontBold_Click);
            // 
            // LabelX1
            // 
            this.LabelX1.Location = new System.Drawing.Point(6, 35);
            this.LabelX1.Name = "LabelX1";
            this.LabelX1.Size = new System.Drawing.Size(77, 21);
            this.LabelX1.TabIndex = 1;
            this.LabelX1.Text = "字段名称：";
            // 
            // GroupBox2
            // 
            this.GroupBox2.Controls.Add(this.btnColorPick);
            this.GroupBox2.Controls.Add(this.cmbFontSize);
            this.GroupBox2.Controls.Add(this.cmbFontName);
            this.GroupBox2.Controls.Add(this.lblText);
            this.GroupBox2.Controls.Add(this.Bar1);
            this.GroupBox2.Location = new System.Drawing.Point(10, 100);
            this.GroupBox2.Name = "GroupBox2";
            this.GroupBox2.Size = new System.Drawing.Size(332, 93);
            this.GroupBox2.TabIndex = 9;
            this.GroupBox2.TabStop = false;
            this.GroupBox2.Text = "字体样式";
            // 
            // btnColorPick
            // 
            this.btnColorPick.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnColorPick.Image = ((System.Drawing.Image)(resources.GetObject("btnColorPick.Image")));
            this.btnColorPick.Location = new System.Drawing.Point(116, 64);
            this.btnColorPick.Name = "btnColorPick";
            this.btnColorPick.SelectedColorImageRectangle = new System.Drawing.Rectangle(2, 2, 12, 12);
            this.btnColorPick.Size = new System.Drawing.Size(50, 23);
            this.btnColorPick.TabIndex = 4;
            this.btnColorPick.SelectedColorChanged += new System.EventHandler(this.btnColorPick_SelectedColorChanged);
            // 
            // cmbFontSize
            // 
            this.cmbFontSize.DisplayMember = "Text";
            this.cmbFontSize.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbFontSize.FormattingEnabled = true;
            this.cmbFontSize.Location = new System.Drawing.Point(245, 26);
            this.cmbFontSize.Name = "cmbFontSize";
            this.cmbFontSize.Size = new System.Drawing.Size(71, 22);
            this.cmbFontSize.TabIndex = 3;
            this.cmbFontSize.SelectedIndexChanged += new System.EventHandler(this.cmbFontSize_SelectedIndexChanged);
            this.cmbFontSize.SelectedValueChanged += new System.EventHandler(this.cmbFontSize_SelectedValueChanged);
            this.cmbFontSize.Click += new System.EventHandler(this.cmbFontSize_Click);
            // 
            // cmbFontName
            // 
            this.cmbFontName.DisplayMember = "Text";
            this.cmbFontName.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbFontName.FormattingEnabled = true;
            this.cmbFontName.Location = new System.Drawing.Point(113, 27);
            this.cmbFontName.Name = "cmbFontName";
            this.cmbFontName.Size = new System.Drawing.Size(108, 22);
            this.cmbFontName.TabIndex = 2;
            this.cmbFontName.SelectedIndexChanged += new System.EventHandler(this.cmbFontName_SelectedIndexChanged);
            this.cmbFontName.SelectedValueChanged += new System.EventHandler(this.cmbFontName_SelectedValueChanged);
            this.cmbFontName.Click += new System.EventHandler(this.cmbFontName_Click);
            // 
            // lblText
            // 
            this.lblText.Location = new System.Drawing.Point(11, 27);
            this.lblText.Name = "lblText";
            this.lblText.Size = new System.Drawing.Size(96, 47);
            this.lblText.TabIndex = 1;
            this.lblText.Text = "浙江数据";
            this.lblText.TextLineAlignment = System.Drawing.StringAlignment.Near;
            // 
            // Bar1
            // 
            this.Bar1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.btnFontBold,
            this.btnFontItalic,
            this.btnFontUnderline});
            this.Bar1.Location = new System.Drawing.Point(245, 62);
            this.Bar1.Name = "Bar1";
            this.Bar1.Size = new System.Drawing.Size(57, 25);
            this.Bar1.Stretch = true;
            this.Bar1.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.Bar1.TabIndex = 0;
            this.Bar1.TabStop = false;
            this.Bar1.Text = "Bar1";
            // 
            // btnExpresion
            // 
            this.btnExpresion.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnExpresion.Location = new System.Drawing.Point(242, 30);
            this.btnExpresion.Name = "btnExpresion";
            this.btnExpresion.Size = new System.Drawing.Size(75, 26);
            this.btnExpresion.TabIndex = 0;
            this.btnExpresion.Text = "<b>表  达  式</b>";
            this.btnExpresion.Click += new System.EventHandler(this.btnExpresion_Click);
            // 
            // GroupBox1
            // 
            this.GroupBox1.Controls.Add(this.cmbFieldName);
            this.GroupBox1.Controls.Add(this.LabelX1);
            this.GroupBox1.Controls.Add(this.btnExpresion);
            this.GroupBox1.Location = new System.Drawing.Point(10, 10);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Size = new System.Drawing.Size(332, 84);
            this.GroupBox1.TabIndex = 8;
            this.GroupBox1.TabStop = false;
            this.GroupBox1.Text = "标注字段";
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.Location = new System.Drawing.Point(261, 427);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(66, 26);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "取  消 ";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // FrmSetLabel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(352, 463);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.GroupBox3);
            this.Controls.Add(this.GroupBox4);
            this.Controls.Add(this.GroupBox2);
            this.Controls.Add(this.GroupBox1);
            this.Controls.Add(this.btnCancel);
            this.Name = "FrmSetLabel";
            this.Text = "FrmSetLabel";
            this.Load += new System.EventHandler(this.FrmSetLabel_Load);
            this.GroupBox3.ResumeLayout(false);
            this.GroupBox4.ResumeLayout(false);
            this.GroupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Bar1)).EndInit();
            this.GroupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        internal DevComponents.DotNetBar.LabelX LabelX7;
        internal DevComponents.DotNetBar.Controls.TextBoxX txtMaxScale;
        internal DevComponents.DotNetBar.LabelX LabelX6;
        internal DevComponents.DotNetBar.ButtonX btnOK;
        internal System.Windows.Forms.GroupBox GroupBox3;
        internal DevComponents.DotNetBar.Controls.TextBoxX txtMinScale;
        internal DevComponents.DotNetBar.LabelX LabelX5;
        internal DevComponents.DotNetBar.LabelX LabelX4;
        internal DevComponents.DotNetBar.Controls.CheckBoxX optUserDefined;
        internal DevComponents.DotNetBar.Controls.CheckBoxX optScaleNameWithLayer;
        internal System.Windows.Forms.GroupBox GroupBox4;
        internal DevComponents.DotNetBar.ButtonX btnPosition;
        internal DevComponents.DotNetBar.LabelX LabelX3;
        internal DevComponents.DotNetBar.ButtonItem btnFontUnderline;
        internal DevComponents.DotNetBar.ButtonItem btnFontItalic;
        internal DevComponents.DotNetBar.Controls.ComboBoxEx cmbFieldName;
        internal DevComponents.DotNetBar.ButtonItem btnFontBold;
        internal DevComponents.DotNetBar.LabelX LabelX1;
        internal System.Windows.Forms.GroupBox GroupBox2;
        internal DevComponents.DotNetBar.ColorPickerButton btnColorPick;
        internal DevComponents.DotNetBar.Controls.ComboBoxEx cmbFontSize;
        internal DevComponents.DotNetBar.Controls.ComboBoxEx cmbFontName;
        internal DevComponents.DotNetBar.LabelX lblText;
        internal DevComponents.DotNetBar.Bar Bar1;
        internal DevComponents.DotNetBar.ButtonX btnExpresion;
        internal System.Windows.Forms.GroupBox GroupBox1;
        internal DevComponents.DotNetBar.ButtonX btnCancel;
    }
}