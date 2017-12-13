using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using System.Windows.Forms;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using System.Resources;
using System.Reflection;

namespace SelectQuery 
{
    /// <summary>
    /// Summary description for clsMeasurePolygon.
    /// </summary>
    [Guid("0ddbebba-74ab-476c-b143-9f8762eba525")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("SelectQuery.clsMeasurePolygon")]
    public sealed class ClsMeasurePolygon : BaseTool
    {
        #region COM Registration Function(s)
        [ComRegisterFunction()]
        [ComVisible(false)]
        static void RegisterFunction(Type registerType)
        {
            // Required for ArcGIS Component Category Registrar support
            ArcGISCategoryRegistration(registerType);

            //
            
            //
        }

        [ComUnregisterFunction()]
        [ComVisible(false)]
        static void UnregisterFunction(Type registerType)
        {
            // Required for ArcGIS Component Category Registrar support
            ArcGISCategoryUnregistration(registerType);

            //
            
            //
        }

        #region ArcGIS Component Category Registrar generated code
        /// <summary>
        /// Required method for ArcGIS Component Category registration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryRegistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            GMxCommands.Register(regKey);
            MxCommands.Register(regKey);
            SxCommands.Register(regKey);
            ControlsCommands.Register(regKey);
        }
        /// <summary>
        /// Required method for ArcGIS Component Category unregistration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryUnregistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            GMxCommands.Unregister(regKey);
            MxCommands.Unregister(regKey);
            SxCommands.Unregister(regKey);
            ControlsCommands.Unregister(regKey);
        }

        #endregion
        #endregion

        private IHookHelper m_hookHelper = null;
        private IGlobeHookHelper m_globeHookHelper = null;
        private ISceneHookHelper m_sceneHookHelper = null;

        IScreenDisplay m_pScreenDisplay;
        IActiveView m_pActiveView;
        IPointCollection m_pPointCol;
        INewPolygonFeedback m_pNewPolygonFeedback;
        IPoint m_pSnapPoint;
        bool m_blnSnap;
        IPolygon m_pPolygon;

        public ClsMeasurePolygon()
        {
            //
            
            //
            base.m_category = "SelectQuery"; //localizable text 
            base.m_caption = "面积量算";  //localizable text 
            base.m_message = "点击左键开始画多边形，双击结束";   //localizable text 
            base.m_toolTip = "面积量算";  //localizable text
            base.m_name = "MeasurePolygon";   //unique id, non-localizable (e.g. "MyCategory_MyTool")
            try
            {
                //
                
                ////
                //string bitmapResourceName = GetType().Name + ".bmp";
                //base.m_bitmap = new Bitmap(GetType(), "Resources.MeasurePolygon.bmp");
                //ResourceManager resource = new ResourceManager("SelectQuery.Resources", Assembly.GetExecutingAssembly());
                //base.m_bitmap = (Bitmap)resource.GetObject("MeasurePolygon");

                ResourceManager rm = new ResourceManager("ZJGISSelectQuery.Properties.Resources", Assembly.GetExecutingAssembly());
                base.m_bitmap = (Bitmap)rm.GetObject("MeasurePolygon");
                //base.m_cursor = new System.Windows.Forms.Cursor(GetType(), "Resources.Measure.cur");

            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message, "Invalid Bitmap");
            }
        }

        #region Overriden Class Methods

        /// <summary>
        /// Occurs when this tool is created
        /// </summary>
        /// <param name="hook">Instance of the application</param>
        public override void OnCreate(object hook)
        {
            // Test the hook that calls this command and disable if nothing valid
            try
            {
                m_hookHelper = new HookHelperClass();
                m_hookHelper.Hook = hook;
                if (m_hookHelper.ActiveView == null)
                {
                    m_hookHelper = null;
                }
            }
            catch
            {
                m_hookHelper = null;
            }
            if (m_hookHelper == null)
            {
                try
                {
                    //Can be a scene or globe
                    m_sceneHookHelper = new SceneHookHelperClass();
                    m_sceneHookHelper.Hook = hook;
                    if (m_sceneHookHelper.ActiveViewer == null)
                    {
                        m_sceneHookHelper = null;
                    }
                }
                catch
                {
                    m_sceneHookHelper = null;
                }

                if (m_sceneHookHelper == null)
                {
                    //Can be globe
                    try
                    {
                        m_globeHookHelper = new GlobeHookHelperClass();
                        m_globeHookHelper.Hook = hook;
                        if (m_globeHookHelper.ActiveViewer == null)
                        {
                            //Nothing valid!
                            m_globeHookHelper = null;
                        }
                    }
                    catch
                    {
                        m_globeHookHelper = null;
                    }
                }
            }

            if (m_globeHookHelper == null && m_sceneHookHelper == null && m_hookHelper == null)
                base.m_enabled = false;
            else
                base.m_enabled = true;
            ClsDeclare.g_pMap = m_hookHelper.FocusMap;
        }

        /// <summary>
        /// Occurs when this tool is clicked
        /// </summary>
        public override void OnClick()
        {
            if (m_hookHelper != null)
            {
                
                m_pActiveView = m_hookHelper.ActiveView;
                m_pScreenDisplay = m_pActiveView.ScreenDisplay;
                m_pNewPolygonFeedback = null;

                ClsDeclare.g_txtMeasure.Clear();
                ClsMeasureSnap.g_pNowSnapPnt = null;
                ClsMeasureSnap.g_pLastSnapPnt = null;
                ClsMeasureSnap.g_pSnapSymbol =ClsMeasureSnap.SetMeasureSnapSymbol() as ISymbol;
            }
            else if (m_sceneHookHelper != null)
            {
               
            }
            else if (m_globeHookHelper != null)
            {
                
            }
        }

     

        public override void OnMouseDown(int Button, int Shift, int X, int Y)
        {
            if (m_hookHelper != null)
            {
                
                object Missing = Type.Missing;
                object Missing1 = Type.Missing;
                if (Button == 1)
                {
                    IPoint pPoint;
                    pPoint = m_pScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);
                    if (m_pNewPolygonFeedback == null)
                    {
                        m_pActiveView.PartialRefresh(esriViewDrawPhase.esriViewForeground, null, null);
                        m_pNewPolygonFeedback = new NewPolygonFeedbackClass();
                        m_pNewPolygonFeedback.Display = m_pScreenDisplay;
                        m_pPointCol = new PolygonClass();
                        if (m_blnSnap == true)
                        {
                             
                            m_pNewPolygonFeedback.Start(m_pSnapPoint);
                            m_pPointCol.AddPoint(ClsDeclare.g_UnitConverter.ConvertPointCoordinate(m_pSnapPoint),ref Missing ,ref Missing1);
                        }
                        else
                        {
                             
                            m_pNewPolygonFeedback.Start(pPoint);
                            m_pPointCol.AddPoint(ClsDeclare.g_UnitConverter.ConvertPointCoordinate(pPoint), ref Missing, ref Missing1);
                        }
                    }
                    else
                    {
                        if (m_blnSnap == true)
                        {

                            m_pNewPolygonFeedback.AddPoint(m_pSnapPoint);
                            m_pPointCol.AddPoint(ClsDeclare.g_UnitConverter.ConvertPointCoordinate(m_pSnapPoint), ref Missing, ref Missing1);
                        }
                        else
                        {
                            m_pNewPolygonFeedback.AddPoint(pPoint);
                            m_pPointCol.AddPoint(ClsDeclare.g_UnitConverter.ConvertPointCoordinate(pPoint), ref Missing, ref Missing1);
                        }
                    }
                    IPointCollection pPointCol;
                    IPolygon pPolygon;
                    IGeometry pGeometry;
                    IArea pArea;
                    ITopologicalOperator pTopo;
                    pPointCol = new PolygonClass();
                    for (int i = 0; i < m_pPointCol.PointCount; i++)
                        pPointCol.AddPoint(m_pPointCol.get_Point(i), ref Missing, ref Missing1);
                    if (pPointCol.PointCount < 3)
                        return;
                    pPolygon = (IPolygon)pPointCol;
                    if (pPolygon != null)
                    {
                        pPolygon.Close();
                        pGeometry = (IGeometry)pPolygon;
                        pTopo = (ITopologicalOperator)pGeometry;
                        pTopo.Simplify();
                        pGeometry.Project(ClsDeclare.g_pMap.SpatialReference);
                        pArea = (IArea)pGeometry;
                        if (ClsDeclare.g_UnitConverter.CurrentUnitName == "度")
                            ClsDeclare.g_UnitConverter.SetCurrentMapUnit(ClsDeclare.g_UnitConverter.DefaultMapUnit);
                        ClsDeclare.g_strUnit = ClsDeclare.g_UnitConverter.CurrentUnitName;
                        ClsDeclare.g_txtMeasure.Clear();
                        ClsDeclare.g_txtMeasure.Text = "面积量算" + "\r" + "\n" + "总面积：" + string.Format("{0,10:#######0.00}", Math.Abs(pArea.Area));
                        pPolygon = null;
                    }

                }
            }
            else if (m_sceneHookHelper != null)
            {
               
            }
            else if (m_globeHookHelper != null)
            {
                
            }
        }

        public override void OnMouseMove(int Button, int Shift, int X, int Y)
        {
            if (m_hookHelper != null)
            {
                
                IPoint pPoint;
                pPoint = m_pScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);
                m_blnSnap = false;
                if (ClsDeclare.g_blnSnap == true)
                {
                    m_pSnapPoint =ClsMeasureSnap.MeasureSnapPoint(ref pPoint);
                    if (m_pSnapPoint != null)
                        m_blnSnap = true;
                    else
                        m_blnSnap = false;
                }
                if (m_pNewPolygonFeedback != null)
                {
                    if (m_blnSnap == true)
                        m_pNewPolygonFeedback.MoveTo(m_pSnapPoint);
                    else
                        m_pNewPolygonFeedback.MoveTo(pPoint);
                }
            }
            else if (m_sceneHookHelper != null)
            {
               
            }
            else if (m_globeHookHelper != null)
            {
                
            }
        }

      

        public override void OnMouseUp(int Button, int Shift, int X, int Y)
        {
            if (m_hookHelper != null)
            {
                
                
            }
            else if (m_sceneHookHelper != null)
            {
               
            }
            else if (m_globeHookHelper != null)
            {
                
            }
        }
        public override void OnDblClick()
        {
            //base.OnDblClick();
            if (m_pNewPolygonFeedback != null)
            {
                m_pPolygon = m_pNewPolygonFeedback.Stop();
                m_pNewPolygonFeedback = null;
            }
            ISimpleFillSymbol pSimFillSym;
            IRgbColor pRGB;
            pSimFillSym = new SimpleFillSymbolClass();
            pRGB = new RgbColorClass();
            pRGB.Red = 255;
            pRGB.Green = 50;
            pRGB.Blue = 50;
            pRGB.Transparency = 0;
            pSimFillSym.Color = pRGB;
            if (m_pPolygon != null)
            {
                m_pScreenDisplay.StartDrawing(m_pScreenDisplay.hDC, -1);
                m_pScreenDisplay.SetSymbol((ISymbol)pSimFillSym);
                m_pScreenDisplay.DrawPolygon(m_pPolygon);
                m_pScreenDisplay.FinishDrawing();
            }
        }
        #endregion
    }
}
