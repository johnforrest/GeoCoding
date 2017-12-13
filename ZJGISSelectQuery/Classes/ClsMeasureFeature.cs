using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using System.Windows.Forms;
using System.Resources;
using System.Reflection;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using System.Collections;
using ESRI.ArcGIS.Geodatabase;
using Microsoft.VisualBasic;

namespace SelectQuery 
{
    /// <summary>
    /// Summary description for clsMeasureFeasure.
    /// </summary>
    [Guid("f6495cd0-67ed-4605-9884-de5c0d5d5171")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("SelectQuery.clsMeasureFeature")]
    public sealed class ClsMeasureFeature : BaseTool
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
        private double m_dArea;
        IMap m_Map;
        public ClsMeasureFeature()
        {
            //
            
            //
            base.m_category = "SelectQuery"; //localizable text 
            base.m_caption = "要素测量";  //localizable text 
            base.m_message = "选择地图上要素进行量算";   //localizable text 
            base.m_toolTip = "要素测量";  //localizable text
            base.m_name = "MeasureFeature";   //unique id, non-localizable (e.g. "MyCategory_MyTool")
            try
            {
                //
                
                //
                //string bitmapResourceName = GetType().Name + ".bmp";
                //base.m_bitmap = new Bitmap(GetType(), "Resources.MeasureFeature.bmp");
                //ResourceManager resource = new ResourceManager("SelectQuery.Resources", Assembly.GetExecutingAssembly());
                //base.m_bitmap = (Bitmap)resource.GetObject("MeasureFeature");

                ResourceManager rm = new ResourceManager("ZJGISSelectQuery.Properties.Resources", Assembly.GetExecutingAssembly());
                base.m_bitmap = (Bitmap)rm.GetObject("MeasureFeature");

                //base.m_cursor = new System.Windows.Forms.Cursor(GetType(),"Resources.Measure.cur");
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
            //ClsDeclare.g_pMap = m_hookHelper.ActiveView;
        }

        /// <summary>
        /// Occurs when this tool is clicked
        /// </summary>
        public override void OnClick()
        {
            if (m_hookHelper != null)
            {
                
                 m_Map = m_hookHelper.FocusMap;
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
                
                IPoint pPt;
                IActiveView pActiveView;
                ClsDeclare.g_strUnit = ClsDeclare.g_UnitConverter.CurrentUnitName;
                pActiveView = m_hookHelper.ActiveView;
                pPt = pActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(X,Y);
                //得到最近地物 
                Collection pFeatures=null;
                IFeature pFeature=null;
                ClsSearch.PfgisGetClosestFeature(m_Map, pPt, ref pFeatures, ref pFeature);
                if (pFeature == null)
                    return;
                if (pFeature.Shape.GeometryType == esriGeometryType.esriGeometryPolygon)
                {
                    ClsSearch.FlashFeature(pActiveView, pFeature);
                    IPolygon pPolygon;
                    pPolygon = (IPolygon)pFeature.ShapeCopy;
                    IArea pArea;
                    if (ClsDeclare.g_strUnit == "度")
                    {
                        ClsDeclare.g_UnitConverter.SetCurrentMapUnit(ClsDeclare.g_UnitConverter.DefaultMapUnit);
                        ClsDeclare.g_strUnit = ClsDeclare.g_UnitConverter.CurrentUnitName;
                    }
                    pArea = ClsDeclare.g_UnitConverter.ConvertGeometryCoordinate(pPolygon) as IArea;
                    m_dArea = pArea.Area;
                    m_dArea = Convert.ToDouble(string.Format("{0:#######0.00}", m_dArea));
                    ClsDeclare.g_txtMeasure.Clear();
                    ClsDeclare.g_txtMeasure.Text = "面要素量算" + "\r" + "\n" + "要素面积：" + Math.Abs(m_dArea) + "平方" + ClsDeclare.g_strUnit + "\r" + "\n" + "要素长度：" + string.Format("{0:#######0.00}", ClsDeclare.g_UnitConverter.ConvertPolyonCoordinate(pPolygon).Length) + ClsDeclare.g_strUnit;
                }
                else if (pFeature.Shape.GeometryType == esriGeometryType.esriGeometryPolyline)
                {
                    ClsSearch.FlashFeature(pActiveView, pFeature);
                    IPolyline pPolyline;
                    pPolyline = ClsDeclare.g_UnitConverter.ConvertGeometryCoordinate(pFeature.ShapeCopy) as IPolyline;
                    ClsDeclare.g_txtMeasure.Clear();
                    ClsDeclare.g_txtMeasure.Text = "线要素量算" + "\r" + "\n" + "要素长度：" + string.Format("{0:#######0.00}", pPolyline.Length) + ClsDeclare.g_strUnit;

                }
                else if (pFeature.Shape.GeometryType == esriGeometryType.esriGeometryPoint)
                {
                    ClsSearch.FlashFeature(pActiveView, pFeature);
                    IPoint pPointFeature;
                    pPointFeature = ClsDeclare.g_UnitConverter.ConvertGeometryCoordinate(pFeature.ShapeCopy) as IPoint;
                    ClsDeclare.g_txtMeasure.Clear();
                    ClsDeclare.g_txtMeasure.Text = "点要素" + "\r" + "\n" + "X:  " + pPointFeature.X + ClsDeclare.g_strUnit + "\r" + "\n" + "Y:  " + pPointFeature.Y + ClsDeclare.g_strUnit;
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
        #endregion
    }
}
