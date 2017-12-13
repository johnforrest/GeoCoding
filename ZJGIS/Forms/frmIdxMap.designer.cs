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
	public partial class frmIdxMap : System.Windows.Forms.Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmIdxMap));
            this.DotNetBarManager1 = new DevComponents.DotNetBar.DotNetBarManager(this.components);
            this.DockSite4 = new DevComponents.DotNetBar.DockSite();
            this.ImageList1 = new System.Windows.Forms.ImageList(this.components);
            this.DockSite1 = new DevComponents.DotNetBar.DockSite();
            this.DockSite2 = new DevComponents.DotNetBar.DockSite();
            this.DockSite8 = new DevComponents.DotNetBar.DockSite();
            this.Bar1 = new DevComponents.DotNetBar.Bar();
            this.miFence = new DevComponents.DotNetBar.ButtonItem();
            this.miZoomToEnvlope = new DevComponents.DotNetBar.ButtonItem();
            this.miZoomIn = new DevComponents.DotNetBar.ButtonItem();
            this.miZoomOut = new DevComponents.DotNetBar.ButtonItem();
            this.miPan = new DevComponents.DotNetBar.ButtonItem();
            this.miFullExtend = new DevComponents.DotNetBar.ButtonItem();
            this.DockSite5 = new DevComponents.DotNetBar.DockSite();
            this.DockSite6 = new DevComponents.DotNetBar.DockSite();
            this.DockSite7 = new DevComponents.DotNetBar.DockSite();
            this.DockSite3 = new DevComponents.DotNetBar.DockSite();
            this.MapIndex = new ESRI.ArcGIS.Controls.AxMapControl();
            this.DockSite8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Bar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MapIndex)).BeginInit();
            this.SuspendLayout();
            // 
            // DotNetBarManager1
            // 
            this.DotNetBarManager1.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.F1);
            this.DotNetBarManager1.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlC);
            this.DotNetBarManager1.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlA);
            this.DotNetBarManager1.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlV);
            this.DotNetBarManager1.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlX);
            this.DotNetBarManager1.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlZ);
            this.DotNetBarManager1.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlY);
            this.DotNetBarManager1.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.Del);
            this.DotNetBarManager1.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.Ins);
            this.DotNetBarManager1.BottomDockSite = this.DockSite4;
            this.DotNetBarManager1.EnableFullSizeDock = false;
            this.DotNetBarManager1.Images = this.ImageList1;
            this.DotNetBarManager1.LeftDockSite = this.DockSite1;
            this.DotNetBarManager1.ParentForm = this;
            this.DotNetBarManager1.RightDockSite = this.DockSite2;
            this.DotNetBarManager1.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.DotNetBarManager1.ToolbarBottomDockSite = this.DockSite8;
            this.DotNetBarManager1.ToolbarLeftDockSite = this.DockSite5;
            this.DotNetBarManager1.ToolbarRightDockSite = this.DockSite6;
            this.DotNetBarManager1.ToolbarTopDockSite = this.DockSite7;
            this.DotNetBarManager1.TopDockSite = this.DockSite3;
            // 
            // DockSite4
            // 
            this.DockSite4.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            this.DockSite4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.DockSite4.DocumentDockContainer = new DevComponents.DotNetBar.DocumentDockContainer();
            this.DockSite4.Location = new System.Drawing.Point(0, 223);
            this.DockSite4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.DockSite4.Name = "DockSite4";
            this.DockSite4.Size = new System.Drawing.Size(271, 0);
            this.DockSite4.TabIndex = 3;
            this.DockSite4.TabStop = false;
            // 
            // ImageList1
            // 
            this.ImageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ImageList1.ImageStream")));
            this.ImageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.ImageList1.Images.SetKeyName(0, "s1.bmp");
            this.ImageList1.Images.SetKeyName(1, "s2.bmp");
            this.ImageList1.Images.SetKeyName(2, "s3.bmp");
            this.ImageList1.Images.SetKeyName(3, "s4.bmp");
            this.ImageList1.Images.SetKeyName(4, "s5.bmp");
            this.ImageList1.Images.SetKeyName(5, "s6.bmp");
            // 
            // DockSite1
            // 
            this.DockSite1.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            this.DockSite1.Dock = System.Windows.Forms.DockStyle.Left;
            this.DockSite1.DocumentDockContainer = new DevComponents.DotNetBar.DocumentDockContainer();
            this.DockSite1.Location = new System.Drawing.Point(0, 0);
            this.DockSite1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.DockSite1.Name = "DockSite1";
            this.DockSite1.Size = new System.Drawing.Size(0, 223);
            this.DockSite1.TabIndex = 0;
            this.DockSite1.TabStop = false;
            // 
            // DockSite2
            // 
            this.DockSite2.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            this.DockSite2.Dock = System.Windows.Forms.DockStyle.Right;
            this.DockSite2.DocumentDockContainer = new DevComponents.DotNetBar.DocumentDockContainer();
            this.DockSite2.Location = new System.Drawing.Point(271, 0);
            this.DockSite2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.DockSite2.Name = "DockSite2";
            this.DockSite2.Size = new System.Drawing.Size(0, 223);
            this.DockSite2.TabIndex = 1;
            this.DockSite2.TabStop = false;
            // 
            // DockSite8
            // 
            this.DockSite8.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            this.DockSite8.Controls.Add(this.Bar1);
            this.DockSite8.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.DockSite8.Location = new System.Drawing.Point(0, 223);
            this.DockSite8.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.DockSite8.Name = "DockSite8";
            this.DockSite8.Size = new System.Drawing.Size(271, 21);
            this.DockSite8.TabIndex = 7;
            this.DockSite8.TabStop = false;
            // 
            // Bar1
            // 
            this.Bar1.AccessibleDescription = "Bar1 (Bar1)";
            this.Bar1.AccessibleName = "Bar1";
            this.Bar1.AccessibleRole = System.Windows.Forms.AccessibleRole.ToolBar;
            this.Bar1.DockSide = DevComponents.DotNetBar.eDockSide.Bottom;
            this.Bar1.Font = new System.Drawing.Font("SimSun", 9F);
            this.Bar1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.miFence,
            this.miZoomToEnvlope,
            this.miZoomIn,
            this.miZoomOut,
            this.miPan,
            this.miFullExtend});
            this.Bar1.Location = new System.Drawing.Point(0, 0);
            this.Bar1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Bar1.Name = "Bar1";
            this.Bar1.Size = new System.Drawing.Size(122, 21);
            this.Bar1.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.Bar1.TabIndex = 0;
            this.Bar1.TabStop = false;
            this.Bar1.Text = "Bar1";
            // 
            // miFence
            // 
            this.miFence.ImageIndex = 0;
            this.miFence.Name = "miFence";
            this.miFence.Text = "开窗";
            this.miFence.Tooltip = "开窗";
            this.miFence.Click += new System.EventHandler(this.miFence_Click);
            // 
            // miZoomToEnvlope
            // 
            this.miZoomToEnvlope.ImageIndex = 1;
            this.miZoomToEnvlope.Name = "miZoomToEnvlope";
            this.miZoomToEnvlope.Text = "缩放到拉框矩形";
            this.miZoomToEnvlope.Tooltip = "拉框";
            this.miZoomToEnvlope.Click += new System.EventHandler(this.miZoomToEnvlope_Click);
            // 
            // miZoomIn
            // 
            this.miZoomIn.ImageIndex = 2;
            this.miZoomIn.Name = "miZoomIn";
            this.miZoomIn.Text = "放大";
            this.miZoomIn.Tooltip = "放大";
            this.miZoomIn.Click += new System.EventHandler(this.miZoomIn_Click);
            // 
            // miZoomOut
            // 
            this.miZoomOut.ImageIndex = 3;
            this.miZoomOut.Name = "miZoomOut";
            this.miZoomOut.Text = "缩小";
            this.miZoomOut.Tooltip = "缩小";
            this.miZoomOut.Click += new System.EventHandler(this.miZoomOut_Click);
            // 
            // miPan
            // 
            this.miPan.ImageIndex = 4;
            this.miPan.Name = "miPan";
            this.miPan.Text = "漫游";
            this.miPan.Tooltip = "漫游";
            this.miPan.Click += new System.EventHandler(this.miPan_Click);
            // 
            // miFullExtend
            // 
            this.miFullExtend.ImageIndex = 5;
            this.miFullExtend.Name = "miFullExtend";
            this.miFullExtend.Text = "移动矩形";
            this.miFullExtend.Tooltip = "全图显示";
            this.miFullExtend.Click += new System.EventHandler(this.miFullExtend_Click);
            // 
            // DockSite5
            // 
            this.DockSite5.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            this.DockSite5.Dock = System.Windows.Forms.DockStyle.Left;
            this.DockSite5.Location = new System.Drawing.Point(0, 0);
            this.DockSite5.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.DockSite5.Name = "DockSite5";
            this.DockSite5.Size = new System.Drawing.Size(0, 223);
            this.DockSite5.TabIndex = 4;
            this.DockSite5.TabStop = false;
            // 
            // DockSite6
            // 
            this.DockSite6.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            this.DockSite6.Dock = System.Windows.Forms.DockStyle.Right;
            this.DockSite6.Location = new System.Drawing.Point(271, 0);
            this.DockSite6.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.DockSite6.Name = "DockSite6";
            this.DockSite6.Size = new System.Drawing.Size(0, 223);
            this.DockSite6.TabIndex = 5;
            this.DockSite6.TabStop = false;
            // 
            // DockSite7
            // 
            this.DockSite7.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            this.DockSite7.Dock = System.Windows.Forms.DockStyle.Top;
            this.DockSite7.Location = new System.Drawing.Point(0, 0);
            this.DockSite7.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.DockSite7.Name = "DockSite7";
            this.DockSite7.Size = new System.Drawing.Size(271, 0);
            this.DockSite7.TabIndex = 6;
            this.DockSite7.TabStop = false;
            // 
            // DockSite3
            // 
            this.DockSite3.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            this.DockSite3.Dock = System.Windows.Forms.DockStyle.Top;
            this.DockSite3.DocumentDockContainer = new DevComponents.DotNetBar.DocumentDockContainer();
            this.DockSite3.Location = new System.Drawing.Point(0, 0);
            this.DockSite3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.DockSite3.Name = "DockSite3";
            this.DockSite3.Size = new System.Drawing.Size(271, 0);
            this.DockSite3.TabIndex = 2;
            this.DockSite3.TabStop = false;
            // 
            // MapIndex
            // 
            this.MapIndex.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MapIndex.Location = new System.Drawing.Point(0, 0);
            this.MapIndex.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MapIndex.Name = "MapIndex";
            this.MapIndex.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("MapIndex.OcxState")));
            this.MapIndex.Size = new System.Drawing.Size(271, 244);
            this.MapIndex.TabIndex = 8;
            this.MapIndex.OnMouseDown += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnMouseDownEventHandler(this.MapIndex_OnMouseDown);
            this.MapIndex.OnMouseUp += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnMouseUpEventHandler(this.MapIndex_OnMouseUp);
            this.MapIndex.OnMouseMove += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnMouseMoveEventHandler(this.MapIndex_OnMouseMove);
            this.MapIndex.OnAfterDraw += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnAfterDrawEventHandler(this.MapIndex_OnAfterDraw);
            // 
            // frmIdxMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(271, 244);
            this.Controls.Add(this.DockSite1);
            this.Controls.Add(this.DockSite2);
            this.Controls.Add(this.DockSite3);
            this.Controls.Add(this.DockSite4);
            this.Controls.Add(this.DockSite5);
            this.Controls.Add(this.DockSite6);
            this.Controls.Add(this.DockSite7);
            this.Controls.Add(this.DockSite8);
            this.Controls.Add(this.MapIndex);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "frmIdxMap";
            this.Load += new System.EventHandler(this.frmIdxMap_Load);
            this.DockSite8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Bar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MapIndex)).EndInit();
            this.ResumeLayout(false);

		}
		internal DevComponents.DotNetBar.DotNetBarManager DotNetBarManager1;
		internal DevComponents.DotNetBar.DockSite DockSite4;
		internal DevComponents.DotNetBar.DockSite DockSite1;
		internal DevComponents.DotNetBar.DockSite DockSite2;
		internal DevComponents.DotNetBar.DockSite DockSite3;
		internal DevComponents.DotNetBar.DockSite DockSite5;
		internal DevComponents.DotNetBar.DockSite DockSite6;
		internal DevComponents.DotNetBar.DockSite DockSite7;
		internal DevComponents.DotNetBar.DockSite DockSite8;
		internal DevComponents.DotNetBar.Bar Bar1;
		internal DevComponents.DotNetBar.ButtonItem miFence;
		internal ESRI.ArcGIS.Controls.AxMapControl MapIndex;
		internal System.Windows.Forms.ImageList ImageList1;
		internal DevComponents.DotNetBar.ButtonItem miZoomToEnvlope;
		internal DevComponents.DotNetBar.ButtonItem miZoomIn;
		internal DevComponents.DotNetBar.ButtonItem miZoomOut;
		internal DevComponents.DotNetBar.ButtonItem miPan;
		internal DevComponents.DotNetBar.ButtonItem miFullExtend;
        private System.ComponentModel.IContainer components;
	}
	
}
