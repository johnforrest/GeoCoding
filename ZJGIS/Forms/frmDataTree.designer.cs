using System.Collections.Generic;
using System.Drawing;
using System;
using System.Collections;
using System.Diagnostics;
using Microsoft.VisualBasic;
using System.Data;
using System.Windows.Forms;

namespace ZJGIS
{
	public partial class frmDataTree : DevComponents.DotNetBar.Office2007Form
	{
		
		//Form 重写 Dispose，以清理组件列表。
		[System.Diagnostics.DebuggerNonUserCode()]protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

        //Windows 窗体设计器所必需的
		
		//注意: 以下过程是 Windows 窗体设计器所必需的
		//可以使用 Windows 窗体设计器修改它。
		//不要使用代码编辑器修改它。
		[System.Diagnostics.DebuggerStepThrough()]private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDataTree));
            this.tabControl = new DevComponents.DotNetBar.TabControl();
            this.TabControlPanel1 = new DevComponents.DotNetBar.TabControlPanel();
            this.TOCLayer = new ESRI.ArcGIS.Controls.AxTOCControl();
            this.tiToc = new DevComponents.DotNetBar.TabItem(this.components);
            this.ImageList = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.tabControl)).BeginInit();
            this.tabControl.SuspendLayout();
            this.TabControlPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TOCLayer)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(217)))), ((int)(((byte)(247)))));
            this.tabControl.CanReorderTabs = true;
            this.tabControl.Controls.Add(this.TabControlPanel1);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabControl.Name = "tabControl";
            this.tabControl.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tabControl.SelectedTabFont = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold);
            this.tabControl.SelectedTabIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(284, 369);
            this.tabControl.Style = DevComponents.DotNetBar.eTabStripStyle.Office2007Document;
            this.tabControl.TabAlignment = DevComponents.DotNetBar.eTabStripAlignment.Bottom;
            this.tabControl.TabIndex = 0;
            this.tabControl.TabLayoutType = DevComponents.DotNetBar.eTabLayoutType.FixedWithNavigationBox;
            this.tabControl.Tabs.Add(this.tiToc);
            this.tabControl.Text = "TabControl1";
            // 
            // TabControlPanel1
            // 
            this.TabControlPanel1.Controls.Add(this.TOCLayer);
            this.TabControlPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabControlPanel1.Location = new System.Drawing.Point(0, 0);
            this.TabControlPanel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.TabControlPanel1.Name = "TabControlPanel1";
            this.TabControlPanel1.Padding = new System.Windows.Forms.Padding(1);
            this.TabControlPanel1.Size = new System.Drawing.Size(284, 344);
            this.TabControlPanel1.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(253)))), ((int)(((byte)(254)))));
            this.TabControlPanel1.Style.BackColor2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(157)))), ((int)(((byte)(188)))), ((int)(((byte)(227)))));
            this.TabControlPanel1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.TabControlPanel1.Style.BorderColor.Color = System.Drawing.Color.FromArgb(((int)(((byte)(146)))), ((int)(((byte)(165)))), ((int)(((byte)(199)))));
            this.TabControlPanel1.Style.BorderSide = ((DevComponents.DotNetBar.eBorderSide)(((DevComponents.DotNetBar.eBorderSide.Left | DevComponents.DotNetBar.eBorderSide.Right) 
            | DevComponents.DotNetBar.eBorderSide.Top)));
            this.TabControlPanel1.Style.GradientAngle = -90;
            this.TabControlPanel1.TabIndex = 1;
            this.TabControlPanel1.TabItem = this.tiToc;
            // 
            // TOCLayer
            // 
            this.TOCLayer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TOCLayer.Location = new System.Drawing.Point(1, 1);
            this.TOCLayer.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.TOCLayer.Name = "TOCLayer";
            this.TOCLayer.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("TOCLayer.OcxState")));
            this.TOCLayer.Size = new System.Drawing.Size(282, 342);
            this.TOCLayer.TabIndex = 0;
            this.TOCLayer.OnMouseDown += new ESRI.ArcGIS.Controls.ITOCControlEvents_Ax_OnMouseDownEventHandler(this.TOCLayer_OnMouseDown);
            this.TOCLayer.OnDoubleClick += new ESRI.ArcGIS.Controls.ITOCControlEvents_Ax_OnDoubleClickEventHandler(this.TOCLayer_OnDoubleClick);
            // 
            // tiToc
            // 
            this.tiToc.AttachedControl = this.TabControlPanel1;
            this.tiToc.Image = ((System.Drawing.Image)(resources.GetObject("tiToc.Image")));
            this.tiToc.Name = "tiToc";
            this.tiToc.Text = "图层控制";
            // 
            // ImageList
            // 
            this.ImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ImageList.ImageStream")));
            this.ImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.ImageList.Images.SetKeyName(0, "BMP");
            this.ImageList.Images.SetKeyName(1, "DATA");
            this.ImageList.Images.SetKeyName(2, "DOM");
            this.ImageList.Images.SetKeyName(3, "DWG");
            this.ImageList.Images.SetKeyName(4, "FC");
            this.ImageList.Images.SetKeyName(5, "FD");
            this.ImageList.Images.SetKeyName(6, "File");
            this.ImageList.Images.SetKeyName(7, "JPG");
            this.ImageList.Images.SetKeyName(8, "LayCtrl");
            this.ImageList.Images.SetKeyName(9, "LibCtrl");
            this.ImageList.Images.SetKeyName(10, "LOCAL");
            this.ImageList.Images.SetKeyName(11, "MXD");
            this.ImageList.Images.SetKeyName(12, "PDB");
            this.ImageList.Images.SetKeyName(13, "Point");
            this.ImageList.Images.SetKeyName(14, "Polygon");
            this.ImageList.Images.SetKeyName(15, "Polyline");
            this.ImageList.Images.SetKeyName(16, "RC");
            this.ImageList.Images.SetKeyName(17, "RD");
            this.ImageList.Images.SetKeyName(18, "ROOT");
            this.ImageList.Images.SetKeyName(19, "SCALE");
            this.ImageList.Images.SetKeyName(20, "SDE");
            this.ImageList.Images.SetKeyName(21, "SID");
            this.ImageList.Images.SetKeyName(22, "STYLE");
            this.ImageList.Images.SetKeyName(23, "TIF");
            this.ImageList.Images.SetKeyName(24, "AN");
            this.ImageList.Images.SetKeyName(25, "ANGry");
            this.ImageList.Images.SetKeyName(26, "ANRGB");
            this.ImageList.Images.SetKeyName(27, "PointGry");
            this.ImageList.Images.SetKeyName(28, "PointRGB");
            this.ImageList.Images.SetKeyName(29, "PolygonGry");
            this.ImageList.Images.SetKeyName(30, "PolygonRGB");
            this.ImageList.Images.SetKeyName(31, "PolylineGry");
            this.ImageList.Images.SetKeyName(32, "PolylineRGB");
            this.ImageList.Images.SetKeyName(33, "RasterGry");
            this.ImageList.Images.SetKeyName(34, "RasterRGB");
            // 
            // frmDataTree
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(284, 369);
            this.Controls.Add(this.tabControl);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "frmDataTree";
            this.Text = "frmDataTree";
            this.Load += new System.EventHandler(this.frmDataTree_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tabControl)).EndInit();
            this.tabControl.ResumeLayout(false);
            this.TabControlPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.TOCLayer)).EndInit();
            this.ResumeLayout(false);

		}
		internal DevComponents.DotNetBar.TabControl tabControl;
		internal DevComponents.DotNetBar.TabControlPanel TabControlPanel1;
		internal DevComponents.DotNetBar.TabItem tiToc;
		internal ESRI.ArcGIS.Controls.AxTOCControl TOCLayer;
		internal System.Windows.Forms.ImageList ImageList;
        private System.ComponentModel.IContainer components;
	}
	
}
