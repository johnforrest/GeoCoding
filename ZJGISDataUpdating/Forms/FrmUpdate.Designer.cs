namespace ZJGISDataUpdating
{
    partial class FrmUpdate
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
            this.wizardUpdate = new DevComponents.DotNetBar.Wizard();
            this.wizardPage1 = new DevComponents.DotNetBar.WizardPage();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.buttonXBrowseLayer = new DevComponents.DotNetBar.ButtonX();
            this.textBoxXLayerPath = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.buttonXBrowseTable = new DevComponents.DotNetBar.ButtonX();
            this.textBoxXTablePath = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.progressBarXUpdate = new DevComponents.DotNetBar.Controls.ProgressBarX();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.textBoxXTargetLayer = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.buttonXBrowserTarlayer = new DevComponents.DotNetBar.ButtonX();
            this.wizardUpdate.SuspendLayout();
            this.wizardPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // wizardUpdate
            // 
            this.wizardUpdate.BackButtonText = "";
            this.wizardUpdate.BackButtonWidth = 99;
            this.wizardUpdate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(229)))), ((int)(((byte)(253)))));
            this.wizardUpdate.ButtonStyle = DevComponents.DotNetBar.eWizardStyle.Office2007;
            this.wizardUpdate.CancelButtonText = "关闭";
            this.wizardUpdate.CancelButtonWidth = 99;
            this.wizardUpdate.Cursor = System.Windows.Forms.Cursors.Default;
            this.wizardUpdate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardUpdate.FinishButtonTabIndex = 3;
            this.wizardUpdate.FinishButtonText = "编码赋值";
            this.wizardUpdate.FinishButtonWidth = 99;
            this.wizardUpdate.FooterHeight = 58;
            // 
            // 
            // 
            this.wizardUpdate.FooterStyle.BackColor = System.Drawing.Color.Transparent;
            this.wizardUpdate.FooterStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.wizardUpdate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(57)))), ((int)(((byte)(129)))));
            this.wizardUpdate.HeaderCaptionFont = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Bold);
            this.wizardUpdate.HeaderDescriptionFont = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.wizardUpdate.HeaderDescriptionIndent = 78;
            this.wizardUpdate.HeaderDescriptionVisible = false;
            this.wizardUpdate.HeaderHeight = 112;
            this.wizardUpdate.HeaderImageAlignment = DevComponents.DotNetBar.eWizardTitleImageAlignment.Left;
            this.wizardUpdate.HeaderImageSize = new System.Drawing.Size(64, 60);
            // 
            // 
            // 
            this.wizardUpdate.HeaderStyle.BackColor = System.Drawing.Color.Transparent;
            this.wizardUpdate.HeaderStyle.BackColorGradientAngle = 90;
            this.wizardUpdate.HeaderStyle.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.wizardUpdate.HeaderStyle.BorderBottomColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(157)))), ((int)(((byte)(182)))));
            this.wizardUpdate.HeaderStyle.BorderBottomWidth = 1;
            this.wizardUpdate.HeaderStyle.BorderColor = System.Drawing.SystemColors.Control;
            this.wizardUpdate.HeaderStyle.BorderLeftWidth = 1;
            this.wizardUpdate.HeaderStyle.BorderRightWidth = 1;
            this.wizardUpdate.HeaderStyle.BorderTopWidth = 1;
            this.wizardUpdate.HeaderStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.wizardUpdate.HeaderStyle.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.wizardUpdate.HeaderStyle.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.wizardUpdate.HeaderTitleIndent = 78;
            this.wizardUpdate.HelpButtonVisible = false;
            this.wizardUpdate.HelpButtonWidth = 99;
            this.wizardUpdate.Location = new System.Drawing.Point(0, 0);
            this.wizardUpdate.Margin = new System.Windows.Forms.Padding(4);
            this.wizardUpdate.Name = "wizardUpdate";
            this.wizardUpdate.NextButtonText = "";
            this.wizardUpdate.NextButtonWidth = 99;
            this.wizardUpdate.Size = new System.Drawing.Size(719, 508);
            this.wizardUpdate.TabIndex = 0;
            this.wizardUpdate.WizardPages.AddRange(new DevComponents.DotNetBar.WizardPage[] {
            this.wizardPage1});
            this.wizardUpdate.FinishButtonClick += new System.ComponentModel.CancelEventHandler(this.wizardUpdate_FinishButtonClick);
            this.wizardUpdate.CancelButtonClick += new System.ComponentModel.CancelEventHandler(this.wizardUpdate_CancelButtonClick);
            // 
            // wizardPage1
            // 
            this.wizardPage1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.wizardPage1.BackButtonEnabled = DevComponents.DotNetBar.eWizardButtonState.False;
            this.wizardPage1.BackButtonVisible = DevComponents.DotNetBar.eWizardButtonState.False;
            this.wizardPage1.BackColor = System.Drawing.Color.Transparent;
            this.wizardPage1.CancelButtonEnabled = DevComponents.DotNetBar.eWizardButtonState.True;
            this.wizardPage1.CancelButtonVisible = DevComponents.DotNetBar.eWizardButtonState.True;
            this.wizardPage1.Controls.Add(this.buttonXBrowserTarlayer);
            this.wizardPage1.Controls.Add(this.textBoxXTargetLayer);
            this.wizardPage1.Controls.Add(this.labelX4);
            this.wizardPage1.Controls.Add(this.labelX3);
            this.wizardPage1.Controls.Add(this.labelX2);
            this.wizardPage1.Controls.Add(this.labelX1);
            this.wizardPage1.Controls.Add(this.buttonXBrowseLayer);
            this.wizardPage1.Controls.Add(this.textBoxXLayerPath);
            this.wizardPage1.Controls.Add(this.buttonXBrowseTable);
            this.wizardPage1.Controls.Add(this.textBoxXTablePath);
            this.wizardPage1.Controls.Add(this.progressBarXUpdate);
            this.wizardPage1.FinishButtonEnabled = DevComponents.DotNetBar.eWizardButtonState.True;
            this.wizardPage1.Location = new System.Drawing.Point(7, 124);
            this.wizardPage1.Margin = new System.Windows.Forms.Padding(4);
            this.wizardPage1.Name = "wizardPage1";
            this.wizardPage1.NextButtonEnabled = DevComponents.DotNetBar.eWizardButtonState.False;
            this.wizardPage1.NextButtonVisible = DevComponents.DotNetBar.eWizardButtonState.False;
            this.wizardPage1.PageTitle = "编码赋值";
            this.wizardPage1.Size = new System.Drawing.Size(705, 314);
            // 
            // 
            // 
            this.wizardPage1.Style.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.wizardPage1.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.wizardPage1.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.wizardPage1.TabIndex = 7;
            this.wizardPage1.CancelButtonClick += new System.ComponentModel.CancelEventHandler(this.wizardPage1_CancelButtonClick);
            // 
            // labelX3
            // 
            // 
            // 
            // 
            this.labelX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX3.Location = new System.Drawing.Point(44, 229);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(140, 23);
            this.labelX3.TabIndex = 8;
            this.labelX3.Text = "进度条：";
            // 
            // labelX2
            // 
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Location = new System.Drawing.Point(44, 154);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(140, 23);
            this.labelX2.TabIndex = 7;
            this.labelX2.Text = "匹配结果表：";
            // 
            // labelX1
            // 
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Location = new System.Drawing.Point(44, 4);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(140, 23);
            this.labelX1.TabIndex = 6;
            this.labelX1.Text = "已编码图层：";
            // 
            // buttonXBrowseLayer
            // 
            this.buttonXBrowseLayer.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonXBrowseLayer.ColorTable = DevComponents.DotNetBar.eButtonColor.Office2007WithBackground;
            this.buttonXBrowseLayer.Location = new System.Drawing.Point(512, 40);
            this.buttonXBrowseLayer.Margin = new System.Windows.Forms.Padding(4);
            this.buttonXBrowseLayer.Name = "buttonXBrowseLayer";
            this.buttonXBrowseLayer.Size = new System.Drawing.Size(64, 29);
            this.buttonXBrowseLayer.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonXBrowseLayer.TabIndex = 3;
            this.buttonXBrowseLayer.Text = "浏览";
            this.buttonXBrowseLayer.Click += new System.EventHandler(this.buttonXBrowseLayer_Click);
            // 
            // textBoxXLayerPath
            // 
            // 
            // 
            // 
            this.textBoxXLayerPath.Border.Class = "TextBoxBorder";
            this.textBoxXLayerPath.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.textBoxXLayerPath.Location = new System.Drawing.Point(44, 40);
            this.textBoxXLayerPath.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxXLayerPath.Name = "textBoxXLayerPath";
            this.textBoxXLayerPath.ReadOnly = true;
            this.textBoxXLayerPath.Size = new System.Drawing.Size(460, 25);
            this.textBoxXLayerPath.TabIndex = 2;
            // 
            // buttonXBrowseTable
            // 
            this.buttonXBrowseTable.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonXBrowseTable.ColorTable = DevComponents.DotNetBar.eButtonColor.Office2007WithBackground;
            this.buttonXBrowseTable.Location = new System.Drawing.Point(512, 184);
            this.buttonXBrowseTable.Margin = new System.Windows.Forms.Padding(4);
            this.buttonXBrowseTable.Name = "buttonXBrowseTable";
            this.buttonXBrowseTable.Size = new System.Drawing.Size(64, 29);
            this.buttonXBrowseTable.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonXBrowseTable.TabIndex = 5;
            this.buttonXBrowseTable.Text = "浏览";
            this.buttonXBrowseTable.Click += new System.EventHandler(this.buttonXBrowseTable_Click);
            // 
            // textBoxXTablePath
            // 
            // 
            // 
            // 
            this.textBoxXTablePath.Border.Class = "TextBoxBorder";
            this.textBoxXTablePath.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.textBoxXTablePath.Location = new System.Drawing.Point(44, 188);
            this.textBoxXTablePath.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxXTablePath.Name = "textBoxXTablePath";
            this.textBoxXTablePath.ReadOnly = true;
            this.textBoxXTablePath.Size = new System.Drawing.Size(457, 25);
            this.textBoxXTablePath.TabIndex = 4;
            // 
            // progressBarXUpdate
            // 
            // 
            // 
            // 
            this.progressBarXUpdate.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.progressBarXUpdate.Location = new System.Drawing.Point(44, 262);
            this.progressBarXUpdate.Margin = new System.Windows.Forms.Padding(4);
            this.progressBarXUpdate.Name = "progressBarXUpdate";
            this.progressBarXUpdate.Size = new System.Drawing.Size(532, 25);
            this.progressBarXUpdate.TabIndex = 2;
            this.progressBarXUpdate.Text = "progressBarX1";
            // 
            // labelX4
            // 
            // 
            // 
            // 
            this.labelX4.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX4.Location = new System.Drawing.Point(44, 79);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(140, 23);
            this.labelX4.TabIndex = 9;
            this.labelX4.Text = "待编码图层：";
            // 
            // textBoxXTargetLayer
            // 
            this.textBoxXTargetLayer.BackColor = System.Drawing.SystemColors.Control;
            // 
            // 
            // 
            this.textBoxXTargetLayer.Border.Class = "TextBoxBorder";
            this.textBoxXTargetLayer.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.textBoxXTargetLayer.Location = new System.Drawing.Point(44, 114);
            this.textBoxXTargetLayer.Name = "textBoxXTargetLayer";
            this.textBoxXTargetLayer.Size = new System.Drawing.Size(457, 25);
            this.textBoxXTargetLayer.TabIndex = 10;
            // 
            // buttonXBrowserTarlayer
            // 
            this.buttonXBrowserTarlayer.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonXBrowserTarlayer.ColorTable = DevComponents.DotNetBar.eButtonColor.Office2007WithBackground;
            this.buttonXBrowserTarlayer.Location = new System.Drawing.Point(512, 115);
            this.buttonXBrowserTarlayer.Name = "buttonXBrowserTarlayer";
            this.buttonXBrowserTarlayer.Size = new System.Drawing.Size(64, 23);
            this.buttonXBrowserTarlayer.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonXBrowserTarlayer.TabIndex = 11;
            this.buttonXBrowserTarlayer.Text = "浏览";
            this.buttonXBrowserTarlayer.Click += new System.EventHandler(this.buttonXBrowserTarlayer_Click);
            // 
            // FrmUpdate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(719, 508);
            this.Controls.Add(this.wizardUpdate);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FrmUpdate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "矢量数据图层匹配向导";
            this.Load += new System.EventHandler(this.FrmUpdate_Load);
            this.wizardUpdate.ResumeLayout(false);
            this.wizardPage1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Wizard wizardUpdate;
        private DevComponents.DotNetBar.WizardPage wizardPage1;
        private DevComponents.DotNetBar.ButtonX buttonXBrowseLayer;
        private DevComponents.DotNetBar.Controls.TextBoxX textBoxXLayerPath;
        private DevComponents.DotNetBar.ButtonX buttonXBrowseTable;
        private DevComponents.DotNetBar.Controls.TextBoxX textBoxXTablePath;
        private DevComponents.DotNetBar.Controls.ProgressBarX progressBarXUpdate;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.ButtonX buttonXBrowserTarlayer;
        private DevComponents.DotNetBar.Controls.TextBoxX textBoxXTargetLayer;
        private DevComponents.DotNetBar.LabelX labelX4;
    }
}