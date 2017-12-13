using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using System.Windows.Forms;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using System.Resources;
using System.Reflection;

namespace SelectQuery 
{
    /// <summary>
    /// Summary description for clsMeasureLength.
    /// </summary>
    [Guid("cf912aeb-d54e-4d2d-98ee-55ff3f2e2789")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("SelectQuery.clsMeasureLength")]
    public sealed class ClsMeasureLength : BaseTool
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

        IPoint m_pPoint;
        IActiveView m_pActiveView;
        INewLineFeedback m_pNewLineFeedback;
        IScreenDisplay m_pScreenDisplay;

        double m_douTotal;
        double m_douDistance;
        bool m_blnSnap;
        bool m_blnInMouseDown;

        IPoint m_pSnapPoint;
        IGeometry m_pGeometry;
        IGraphicsContainer m_pGraCont;

        public IGraphicsContainer DrawLineElement
        {
            get
            {
                return m_pGraCont;
            }
        }
        public IActiveView ActiveView
        {
            get
            {
                return m_pActiveView;
            }
        }
        public ClsMeasureLength()
        {
            //
            
            //
            base.m_category = "SelectQuery"; //localizable text 
            base.m_caption = "长度量算";  //localizable text 
            base.m_message = "点击左键开始画线，双击结束";   //localizable text 
            base.m_toolTip = "长度量算";  //localizable text
            base.m_name = "MeasureLength";   //unique id, non-localizable (e.g. "MyCategory_MyTool")
            try
            {
                //
                
                //
                //string bitmapResourceName = GetType().Name + ".bmp";
                //base.m_bitmap = new Bitmap(GetType(), "Resources.MeasureLength.bmp");
                //ResourceManager resource = new ResourceManager("SelectQuery.Resources", Assembly.GetExecutingAssembly());
                //base.m_bitmap = (Bitmap)resource.GetObject("MeasureLength");

                ResourceManager rm = new ResourceManager("ZJGISSelectQuery.Properties.Resources", Assembly.GetExecutingAssembly());
                base.m_bitmap = (Bitmap)rm.GetObject("MeasureLength");
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
                m_pNewLineFeedback = null;

                m_douTotal = 0d;
                m_douDistance = 0d;
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
                
                if (Button == 1)
                {
                    m_blnInMouseDown = true;
                    m_pPoint = m_pActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);
                    //此处可能要加m_blnSnap=false;
                    if (m_pNewLineFeedback == null)
                    {
                        m_pNewLineFeedback = new NewLineFeedback();
                        m_pNewLineFeedback.Display = m_pScreenDisplay;
                        m_douTotal = 0d;
                        if (m_blnSnap == true)
                            m_pNewLineFeedback.Start(m_pSnapPoint);
                        else
                            m_pNewLineFeedback.Start(m_pPoint);
                    }
                    else
                    {
                        if (m_blnSnap == true)
                            m_pNewLineFeedback.AddPoint(m_pSnapPoint);
                        else
                            m_pNewLineFeedback.AddPoint(m_pPoint);
                    }
                    m_pGeometry = m_pPoint;
                    if (m_douDistance != 0d)
                        m_douTotal = m_douTotal + m_douDistance;
                }
                else if (Button == 2)
                    m_blnInMouseDown = false;   

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
                
                if (m_blnInMouseDown == false)
                    return;
                IPoint pPt;
                pPt = m_pScreenDisplay.DisplayTransformation.ToMapPoint(X,Y);
                m_blnSnap = false;
                if (m_pNewLineFeedback != null)
                    m_pNewLineFeedback.MoveTo(pPt);
                if (ClsDeclare.g_blnSnap == true)
                {
                    m_pSnapPoint = MeasureSnapPoint(pPt);
                    if (m_pSnapPoint != null)
                        m_blnSnap = true;
                    else
                        m_blnSnap = false;
                }
                if (m_pNewLineFeedback != null)
                {
                    if (m_blnSnap == true)
                        m_pNewLineFeedback.MoveTo(m_pSnapPoint);
                    else
                        m_pNewLineFeedback.MoveTo(pPt);
                }
                double deltaX;
                double deltaY;
                if (m_pPoint != null && m_pNewLineFeedback != null)
                {
                    m_pGeometry = pPt;
                    deltaX = ClsDeclare.g_UnitConverter.ConvertPointCoordinate(pPt).X - ClsDeclare.g_UnitConverter.ConvertPointCoordinate(m_pPoint).X;
                    deltaY = ClsDeclare.g_UnitConverter.ConvertPointCoordinate(pPt).Y - ClsDeclare.g_UnitConverter.ConvertPointCoordinate(m_pPoint).Y;
                    m_douDistance = System.Math.Round(System.Math.Sqrt((deltaX * deltaX) + (deltaY * deltaY)),3);
                    m_douTotal = m_douTotal + m_douDistance;
                    ClsDeclare.g_strUnit = ClsDeclare.g_UnitConverter.CurrentUnitName;
                    ClsDeclare.g_txtMeasure.Clear();
                    ClsDeclare.g_txtMeasure.Text = "长度量算" + "\r" + "\n" + "段长度：" + m_douDistance + ClsDeclare.g_strUnit + "\r" + "\n" + "总长度：" + m_douTotal + ClsDeclare.g_strUnit;
                    m_douTotal = m_douTotal - m_douDistance;
                }
                    

            }
            else if (m_sceneHookHelper != null)
            {
               
            }
            else if (m_globeHookHelper != null)
            {
                
            }
        }

        private IPoint MeasureSnapPoint(IPoint pPt)
        {
            throw new NotImplementedException();
        }

        //private decimal Sqrt(double p)
        //{
        //    throw new NotImplementedException();
        //}

        public override void OnMouseUp(int Button, int Shift, int X, int Y)
        {
            if (m_hookHelper != null)
            {
                
                if (Button == 1)
                {
                    IActiveView pActiveView;
                    pActiveView = m_hookHelper.ActiveView;
                    pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, null);
                }
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
            if (m_pGraCont != null)
                m_pGraCont.DeleteAllElements();
            m_pGeometry = m_pNewLineFeedback.Stop();
            if (m_pGeometry != null)
            {
                AddCreateElement(m_pGeometry, m_pActiveView);
                m_pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography,null,null);
                
            }
            m_pNewLineFeedback = null;

            m_douTotal = 0d;
            m_douDistance = 0d;

        }
        public void AddCreateElement(IGeometry pGeomLine, IActiveView pAV)
        {
            ILineElement pLineElem;
            IElement pElem;
            ISimpleLineSymbol pSLnSym;
            IRgbColor pRGB;

            pElem = new LineElementClass();
            pElem.Geometry = pGeomLine;
            pLineElem = (ILineElement)pElem;
            pRGB = new RgbColorClass();
            pRGB.Red = 50;
            pRGB.Green = 50;
            pRGB.Blue = 50;

            pSLnSym = new SimpleLineSymbolClass();
            pSLnSym.Color = pRGB;
            pSLnSym.Style = esriSimpleLineStyle.esriSLSSolid;
            pLineElem.Symbol = pSLnSym;

            m_pGraCont = (IGraphicsContainer)pAV;
            m_pGraCont.AddElement((IElement)pLineElem, 0);
        }
       
         
        #endregion
    }
}
