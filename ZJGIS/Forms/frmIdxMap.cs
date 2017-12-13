using System.Collections.Generic;
using System.Drawing;
using System;
using System.Collections;
using System.Diagnostics;
using Microsoft.VisualBasic;
using System.Data;
using System.Windows.Forms;
using ESRI.ArcGIS.Geometry;
using ZJGIS;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Carto;


namespace ZJGIS
{
	public partial class frmIdxMap
	{
		//private bool m_Navigate;
		private IEnvelope RectEye; //显示的鹰眼矩形
		private string m_strCommand;
		public IPoint PtStart; //标识移动鹰眼矩形的起始点
		public IEnvelope EyeMove; //移动的鹰眼矩形
		private frmMain m_FrmActive; //传进来要处理的窗体
		public frmIdxMap()
		{
			// 此调用是 Windows 窗体设计器所必需的。
			InitializeComponent();
			// 在 InitializeComponent() 调用之后添加任何初始化。
			this.SetTopLevel(false);
		}
		public frmMain mainForm
		{
			get
			{
				return m_FrmActive;
			}
            set
            {
                m_FrmActive = value;
            }
		}
		/// <summary>
		/// load事件
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void frmIdxMap_Load(object sender, System.EventArgs e)
		{
			//m_Navigate = true;
			this.MapIndex.Map.Name = "图层";

            //初始化加载图层
            InitSketchMap((IMapControl3)m_FrmActive.mapMain.Object, (IMapControl3)this.MapIndex.Object);
            this.MapIndex.Extent = m_FrmActive.mapMain.FullExtent;
			
			EyeMove = new EnvelopeClass();
			//Application.SetUnhandledExceptionMode(UnhandledExceptionMode.Automatic)
		}
		
		private void miFence_Click(System.Object sender, System.EventArgs e)
		{
			
			m_strCommand = "Fence";
			MapIndex.MousePointer = esriControlsMousePointer.esriPointerCrosshair;
		}
		
		private void miZoomToEnvlope_Click(System.Object sender, System.EventArgs e)
		{
			MapIndex.MousePointer = esriControlsMousePointer.esriPointerPagePan;
			m_strCommand = "ZoomToEnvlope";
			
			
		}
		
		private void miZoomIn_Click(System.Object sender, System.EventArgs e)
		{
			MapIndex.MousePointer = esriControlsMousePointer.esriPointerZoomIn;
			
			m_strCommand = "ZoomIn";
		}
		
		private void miZoomOut_Click(System.Object sender, System.EventArgs e)
		{
			
			MapIndex.MousePointer = esriControlsMousePointer.esriPointerZoomOut;
			m_strCommand = "ZoomOut";
		}
		
		private void miPan_Click(System.Object sender, System.EventArgs e)
		{
			MapIndex.MousePointer = esriControlsMousePointer.esriPointerPan;
			
			m_strCommand = "Pan";
		}
		
		private void miFullExtend_Click(System.Object sender, System.EventArgs e)
		{
			
			m_strCommand = "FullExtent";
			this.MapIndex.Extent = this.MapIndex.FullExtent;
		}
		
		
		////设置鹰眼矩形符号
		public ISimpleFillSymbol EyeRectSym
		{
			
			get
			{
				ISimpleFillSymbol returnValue;
				
				ISimpleFillSymbol pSym;
				pSym = new SimpleFillSymbol();
				ISimpleLineSymbol pLnSym;
				pLnSym = new SimpleLineSymbol();
				
				pLnSym.Color = SDCNCgisGetRGBColor(80, 100, 255);
				pLnSym.Style = esriSimpleLineStyle.esriSLSSolid;
				pLnSym.Width = 1;
				
				pSym.Color = SDCNCgisGetRGBColor(80, 100, 255);
				pSym.Style = esriSimpleFillStyle.esriSFSBackwardDiagonal;
				pSym.Outline = pLnSym;
				
				returnValue = pSym;
				pSym = null;
				
				return returnValue;
			}
		}
		
		////得到鹰眼矩形
		public IEnvelope EyeRect
		{
			get
			{
				IEnvelope returnValue;
				IEnvelope pEnvEyeExt;
				IEnvelope pEnvEyeFull;
				IEnvelope pEnvMainFull;
				IEnvelope pEnvMainExt;
				
				pEnvMainFull = m_FrmActive.mapMain.FullExtent;
                pEnvMainExt = m_FrmActive.mapMain.Extent;
				pEnvEyeFull = this.MapIndex.FullExtent;
				pEnvEyeExt = new EnvelopeClass();
				
				pEnvEyeExt = pEnvMainExt;
                returnValue = new EnvelopeClass();
				returnValue = pEnvEyeExt;
				RectEye = pEnvEyeExt;
				
				pEnvEyeExt = null;
				pEnvEyeFull = null;
				pEnvMainFull = null;
				pEnvMainExt = null;
				
				return returnValue;
			}
		}
		
		////得到移动鹰眼符号
		public ISimpleFillSymbol MoveRectSym
		{
			get
			{
				ISimpleFillSymbol returnValue;
				
				ISimpleFillSymbol pSym;
				pSym = new SimpleFillSymbol();
				ISimpleLineSymbol pLnSym;
				pLnSym = new SimpleLineSymbol();
				
				pLnSym.Color = SDCNCgisGetRGBColor(80, 100, 255);
				pLnSym.Style = esriSimpleLineStyle.esriSLSSolid;
				pLnSym.Width = 1;
				
				pSym.Outline = pLnSym;
				pSym.Style = esriSimpleFillStyle.esriSFSHollow;
				
				returnValue = pSym;
				pSym = null;
				
				return returnValue;
			}
		}

        public IRgbColor SDCNCgisGetRGBColor(int pRed, int pGreen, int pBlue)
		{
			IRgbColor returnValue;
			IRgbColor pRGB;
			pRGB = new RgbColor();
			
			pRGB.Red = pRed;
			pRGB.Green = pGreen;
			pRGB.Blue = pBlue;
            //pRGB.Transparency = 0;
			pRGB.UseWindowsDithering = true;
			
			returnValue = pRGB;
			return returnValue;
		}
		
		
		//绘制鹰眼矩形
		private void MapIndex_OnAfterDraw(object sender, IMapControlEvents2_OnAfterDrawEvent e)
		{
            if (e.viewDrawPhase != 32)
			{
				return;
			}
			IEnvelope pRect;
			ISimpleFillSymbol pSym;
			IEnvelope pRect2;
			ISimpleFillSymbol pSym2;
			
			pRect = EyeRect;
			pSym = EyeRectSym;
            object obj = pSym;
			//pSym = MoveRectSym
			if (pRect != null&& pSym != null)
			{
				this.MapIndex.DrawShape(pRect,ref obj);
			}
			pRect2 = EyeMove;
			pSym2 = MoveRectSym;
            object obj2 = pSym2;
			if (pRect2 != null&& pSym2 != null)
			{
				this.MapIndex.DrawShape(pRect2,ref obj2);
			}
		}
		
		private void MapIndex_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
		{
			IEnvelope pEnv;
			IActiveView pActView;
			IRubberBand pRubberBand;
			double dWidth;
			double dHeight;
			double dXmin;
			double dYmin;
			double dXmax;
			double dYmax;
			
			if (m_strCommand == "Fence")
			{
				
				pRubberBand = new RubberEnvelope();
				IActiveView pActiveViewFrmMain;
				pActiveViewFrmMain = m_FrmActive.mapMain.ActiveView;
				pActView = MapIndex.ActiveView;
				
				pEnv = MapIndex.TrackRectangle();
				if (pEnv.IsEmpty)
				{
					IPoint pPt;
					pPt = pActView.ScreenDisplay.DisplayTransformation.ToMapPoint(e.x, e.y);
					pEnv = pActiveViewFrmMain.Extent;
					pEnv.CenterAt(pPt);
				}
				pActiveViewFrmMain.Extent = pEnv;
				pActiveViewFrmMain.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, pActiveViewFrmMain.Extent);
				pActView.Refresh();
				
			}
			else if (m_strCommand == "ZoomToEnvlope")
			{
				IPoint PtDown;
				PtDown = new ESRI.ArcGIS.Geometry.Point();
				PtDown = MapIndex.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(e.x, e.y);
				PtStart = PtDown;
				
				EyeMove = new EnvelopeClass();
				EyeMove.PutCoords(EyeRect.XMin, EyeRect.YMin, EyeRect.XMax, EyeRect.YMax);
				
			}
			else if (m_strCommand == "ZoomIn")
			{
				MapIndex.Extent = MapIndex.TrackRectangle();
				
			}
			else if (m_strCommand == "ZoomOut")
			{
				pRubberBand = new RubberEnvelope();
				pActView = MapIndex.ActiveView; //pActView为当前窗口
				pEnv = pRubberBand.TrackNew(pActView.ScreenDisplay, null).Envelope; //pEnv为拉框范围
				if (pEnv.IsEmpty)
				{
					return;
				}
				if (pEnv.Width == 0 || pEnv.Height == 0)
				{
					return;
				}
				
				dWidth = pActView.Extent.Width *(pActView.Extent.Width / pEnv.Width);
				dHeight = pActView.Extent.Height *(pActView.Extent.Height / pEnv.Height);
				
				dXmin = pActView.Extent.XMin -((pEnv.XMin - pActView.Extent.XMin) *(pActView.Extent.Width / pEnv.Width));
				dYmin = pActView.Extent.YMin -((pEnv.YMin - pActView.Extent.YMin) *(pActView.Extent.Height / pEnv.Height));
				dXmax = (pActView.Extent.XMin -((pEnv.XMin - pActView.Extent.XMin) *(pActView.Extent.Width / pEnv.Width))) + dWidth;
				dYmax = (pActView.Extent.YMin -((pEnv.YMin - pActView.Extent.YMin) *(pActView.Extent.Height / pEnv.Height))) + dHeight;
				
				pEnv.PutCoords(dXmin, dYmin, dXmax, dYmax);
				pActView.Extent = pEnv;
				pActView.Refresh();
				
			}
			else if (m_strCommand == "Pan")
			{
				this.MapIndex.Pan();
			}
		}
		
		////鼠标移动时移动鹰眼矩形
		private void MapIndex_OnMouseMove(object sender,IMapControlEvents2_OnMouseMoveEvent e)
		{
			
			IPoint PtMove;
			PtMove = new PointClass();
			PtMove.PutCoords(e.mapX, e.mapY);
			try
			{
				//====
				if (PtStart != null)
				{
					EyeMove.PutCoords(PtMove.X + EyeRect.XMin - PtStart.X, PtMove.Y + EyeRect.YMin - PtStart.Y, PtMove.X + EyeRect.XMax - PtStart.X, PtMove.Y + EyeRect.YMax - PtStart.Y);
					MapIndex.Refresh();
				}
			}
			catch (Exception ex)
			{
				
				MessageBox.Show(ex.Message);
			}
			
		}
		
		////最后定位鹰眼矩形
		private void MapIndex_OnMouseUp(object sender, IMapControlEvents2_OnMouseUpEvent e)
		{
			if (RectEye == null || EyeMove == null || PtStart == null)
			{
				return;
			}
			
			RectEye.PutCoords(EyeMove.XMin, EyeMove.YMin, EyeMove.XMax, EyeMove.YMax);
			m_FrmActive.mapMain.Extent = EyeMove;
			MapIndex.Refresh();
			PtStart = null;
			EyeMove = null;
			
		}
		/// <summary>
        /// 初始化Mapcontrol
		/// </summary>
		/// <param name="frmMainMapControl"></param>
		/// <param name="frmIdxMapControl"></param>
		private void InitSketchMap(IMapControl3 frmMainMapControl, IMapControl3 frmIdxMapControl)
		{
			
			IMap pMap;
			ILayer pLayer;
			
			
			int i;
			bool blnExistMain;
			bool blnExistIdx;
			blnExistMain = false;
			blnExistIdx = false;
			
			
			
			IMapDocument pMapDoc;
			//20080906
			//项目启动时当前上当的路径(即.exe所有目录的路径)
			string sCurrentPath;
			
			sCurrentPath = System.Windows.Forms.Application.StartupPath;
			
			pMapDoc = new MapDocument();

            //pMapDoc.Open(sCurrentPath + "\\..\\Res\\mdb\\全国县级区域矢量化地图\\实验数据.mxd");
            //pMapDoc.Open(sCurrentPath + "\\..\\Res\\mxd\\res.mxd");
			pMap = pMapDoc.Map[0];
			for (i = pMap.LayerCount - 1; i >= 0; i--)
			{
				//For i = 0 To pMap.LayerCount - 1
				pLayer = pMap.Layer[i];
				////索引图MapControl加载加载示意图
				if (blnExistIdx == false)
				{
					frmIdxMapControl.AddLayer(pLayer);
				}
			}
            this.MapIndex.Extent = this.MapIndex.FullExtent;
			//System.Windows.Forms.Application.StartupPath
			IMapDocument pMapDoc2;
			pMapDoc2 = new MapDocument();

			////打开示意图
            //pMapDoc2.Open(sCurrentPath + "\\..\\Res\\mdb\\全国县级区域矢量化地图\\实验数据.mxd");
            //pMapDoc2.Open(sCurrentPath + "\\..\\Res\\mxd\\res.mxd");
			pMap = pMapDoc2.Map[0];
			
			if (blnExistMain == false)
			{
				frmMainMapControl.Map = pMap;
			}
            frmMainMapControl.Extent = frmMainMapControl.FullExtent; 
			////释放变量
			pMapDoc = null;
			
		}
		
	}
	
}
