namespace SelectQuery
{
    partial class FrmMeasure
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMeasure));
            this.Bar1 = new DevComponents.DotNetBar.Bar();
            this.MeasureLength = new DevComponents.DotNetBar.ButtonItem();
            this.MeasurePolygon = new DevComponents.DotNetBar.ButtonItem();
            this.MeasureFeature = new DevComponents.DotNetBar.ButtonItem();
            this.Snap = new DevComponents.DotNetBar.ButtonItem();
            this.Unit = new DevComponents.DotNetBar.ButtonItem();
            this.miDistance = new DevComponents.DotNetBar.ButtonItem();
            this.miDKilometer = new DevComponents.DotNetBar.ButtonItem();
            this.miDMeter = new DevComponents.DotNetBar.ButtonItem();
            this.miDDecimeter = new DevComponents.DotNetBar.ButtonItem();
            this.miDCentimeter = new DevComponents.DotNetBar.ButtonItem();
            this.miDMillimeter = new DevComponents.DotNetBar.ButtonItem();
            this.miDecimalDegree = new DevComponents.DotNetBar.ButtonItem();
            this.miArea = new DevComponents.DotNetBar.ButtonItem();
            this.miAKilometer = new DevComponents.DotNetBar.ButtonItem();
            this.miAMeter = new DevComponents.DotNetBar.ButtonItem();
            this.miADecimeter = new DevComponents.DotNetBar.ButtonItem();
            this.miACentimeter = new DevComponents.DotNetBar.ButtonItem();
            this.miAMillimeter = new DevComponents.DotNetBar.ButtonItem();
            this.miClear = new DevComponents.DotNetBar.ButtonItem();
            this.txtMeasure = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.Bar1)).BeginInit();
            this.SuspendLayout();
            // 
            // Bar1
            // 
            this.Bar1.AccessibleDescription = "Bar1 (Bar1)";
            this.Bar1.AccessibleName = "Bar1";
            this.Bar1.AccessibleRole = System.Windows.Forms.AccessibleRole.ToolBar;
            this.Bar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.Bar1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.MeasureLength,
            this.MeasurePolygon,
            this.MeasureFeature,
            this.Snap,
            this.Unit,
            this.miClear});
            this.Bar1.Location = new System.Drawing.Point(0, 0);
            this.Bar1.Name = "Bar1";
            this.Bar1.Size = new System.Drawing.Size(334, 25);
            this.Bar1.Stretch = true;
            this.Bar1.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2003;
            this.Bar1.TabIndex = 1;
            this.Bar1.TabStop = false;
            this.Bar1.Text = "Bar1";
            // 
            // MeasureLength
            // 
            this.MeasureLength.Image = ((System.Drawing.Image)(resources.GetObject("MeasureLength.Image")));
            this.MeasureLength.Name = "MeasureLength";
            this.MeasureLength.Text = "长度量算";
            this.MeasureLength.Tooltip = "长度量算";
            this.MeasureLength.Click += new System.EventHandler(this.MeasureLength_Click);
            // 
            // MeasurePolygon
            // 
            this.MeasurePolygon.Image = ((System.Drawing.Image)(resources.GetObject("MeasurePolygon.Image")));
            this.MeasurePolygon.Name = "MeasurePolygon";
            this.MeasurePolygon.Text = "面积量算";
            this.MeasurePolygon.Tooltip = "面积量算";
            this.MeasurePolygon.Click += new System.EventHandler(this.MeasurePolygon_Click);
            // 
            // MeasureFeature
            // 
            this.MeasureFeature.Image = ((System.Drawing.Image)(resources.GetObject("MeasureFeature.Image")));
            this.MeasureFeature.Name = "MeasureFeature";
            this.MeasureFeature.Text = "要素量算";
            this.MeasureFeature.Tooltip = "要素量算";
            this.MeasureFeature.Click += new System.EventHandler(this.MeasureFeature_Click);
            // 
            // Snap
            // 
            this.Snap.BeginGroup = true;
            this.Snap.Image = ((System.Drawing.Image)(resources.GetObject("Snap.Image")));
            this.Snap.Name = "Snap";
            this.Snap.Text = "对象捕捉";
            this.Snap.Tooltip = "对象捕捉";
            this.Snap.Click += new System.EventHandler(this.Snap_Click);
            // 
            // Unit
            // 
            this.Unit.Image = ((System.Drawing.Image)(resources.GetObject("Unit.Image")));
            this.Unit.Name = "Unit";
            this.Unit.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.miDistance,
            this.miArea});
            this.Unit.Text = "地图单位";
            this.Unit.Tooltip = "地图单位";
            // 
            // miDistance
            // 
            this.miDistance.Name = "miDistance";
            this.miDistance.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.miDKilometer,
            this.miDMeter,
            this.miDDecimeter,
            this.miDCentimeter,
            this.miDMillimeter,
            this.miDecimalDegree});
            this.miDistance.Text = "距离";
            this.miDistance.Click += new System.EventHandler(this.miDistance_Click);
            // 
            // miDKilometer
            // 
            this.miDKilometer.Name = "miDKilometer";
            this.miDKilometer.Text = "公里";
            this.miDKilometer.Click += new System.EventHandler(this.miDKilometer_Click);
            // 
            // miDMeter
            // 
            this.miDMeter.Name = "miDMeter";
            this.miDMeter.Text = "米";
            this.miDMeter.Click += new System.EventHandler(this.miDMeter_Click);
            // 
            // miDDecimeter
            // 
            this.miDDecimeter.Name = "miDDecimeter";
            this.miDDecimeter.Text = "分米";
            this.miDDecimeter.Click += new System.EventHandler(this.miDDecimeter_Click);
            // 
            // miDCentimeter
            // 
            this.miDCentimeter.Name = "miDCentimeter";
            this.miDCentimeter.Text = "厘米";
            this.miDCentimeter.Click += new System.EventHandler(this.miDCentimeter_Click);
            // 
            // miDMillimeter
            // 
            this.miDMillimeter.Name = "miDMillimeter";
            this.miDMillimeter.Text = "毫米";
            this.miDMillimeter.Click += new System.EventHandler(this.miDMillimeter_Click);
            // 
            // miDecimalDegree
            // 
            this.miDecimalDegree.BeginGroup = true;
            this.miDecimalDegree.Name = "miDecimalDegree";
            this.miDecimalDegree.Text = "度";
            this.miDecimalDegree.Click += new System.EventHandler(this.miDecimalDegree_Click);
            // 
            // miArea
            // 
            this.miArea.Name = "miArea";
            this.miArea.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.miAKilometer,
            this.miAMeter,
            this.miADecimeter,
            this.miACentimeter,
            this.miAMillimeter});
            this.miArea.Text = "面积";
            this.miArea.Click += new System.EventHandler(this.miArea_Click);
            // 
            // miAKilometer
            // 
            this.miAKilometer.Name = "miAKilometer";
            this.miAKilometer.Text = "公里";
            this.miAKilometer.Click += new System.EventHandler(this.miAKilometer_Click);
            // 
            // miAMeter
            // 
            this.miAMeter.Name = "miAMeter";
            this.miAMeter.Text = "米";
            this.miAMeter.Click += new System.EventHandler(this.miAMeter_Click);
            // 
            // miADecimeter
            // 
            this.miADecimeter.Name = "miADecimeter";
            this.miADecimeter.Text = "分米";
            this.miADecimeter.Click += new System.EventHandler(this.miADecimeter_Click);
            // 
            // miACentimeter
            // 
            this.miACentimeter.Name = "miACentimeter";
            this.miACentimeter.Text = "厘米";
            this.miACentimeter.Click += new System.EventHandler(this.miACentimeter_Click);
            // 
            // miAMillimeter
            // 
            this.miAMillimeter.Name = "miAMillimeter";
            this.miAMillimeter.Text = "毫米";
            this.miAMillimeter.Click += new System.EventHandler(this.miAMillimeter_Click);
            // 
            // miClear
            // 
            this.miClear.Image = ((System.Drawing.Image)(resources.GetObject("miClear.Image")));
            this.miClear.Name = "miClear";
            this.miClear.Text = "清除";
            this.miClear.Click += new System.EventHandler(this.miClear_Click);
            // 
            // txtMeasure
            // 
            this.txtMeasure.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMeasure.Location = new System.Drawing.Point(0, 25);
            this.txtMeasure.Name = "txtMeasure";
            this.txtMeasure.Size = new System.Drawing.Size(334, 125);
            this.txtMeasure.TabIndex = 11;
            this.txtMeasure.Text = "";
            // 
            // FrmMeasure
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 150);
            this.Controls.Add(this.txtMeasure);
            this.Controls.Add(this.Bar1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "FrmMeasure";
            this.Text = "量算";
            this.Load += new System.EventHandler(this.frmMeasure_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMeasure_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.Bar1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        internal DevComponents.DotNetBar.Bar Bar1;
        internal DevComponents.DotNetBar.ButtonItem MeasureLength;
        internal DevComponents.DotNetBar.ButtonItem MeasurePolygon;
        internal DevComponents.DotNetBar.ButtonItem MeasureFeature;
        internal DevComponents.DotNetBar.ButtonItem Snap;
        internal DevComponents.DotNetBar.ButtonItem Unit;
        internal DevComponents.DotNetBar.ButtonItem miDistance;
        internal DevComponents.DotNetBar.ButtonItem miDKilometer;
        internal DevComponents.DotNetBar.ButtonItem miDMeter;
        internal DevComponents.DotNetBar.ButtonItem miDDecimeter;
        internal DevComponents.DotNetBar.ButtonItem miDCentimeter;
        internal DevComponents.DotNetBar.ButtonItem miDMillimeter;
        internal DevComponents.DotNetBar.ButtonItem miDecimalDegree;
        internal DevComponents.DotNetBar.ButtonItem miArea;
        internal DevComponents.DotNetBar.ButtonItem miAKilometer;
        internal DevComponents.DotNetBar.ButtonItem miAMeter;
        internal DevComponents.DotNetBar.ButtonItem miADecimeter;
        internal DevComponents.DotNetBar.ButtonItem miACentimeter;
        internal DevComponents.DotNetBar.ButtonItem miAMillimeter;
        internal DevComponents.DotNetBar.ButtonItem miClear;
        internal System.Windows.Forms.RichTextBox txtMeasure;
    }
}