using System;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using System.Windows.Forms;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using Microsoft.VisualBasic;
using ZJGISCommon;

namespace SelectQuery 
{
    /// <summary>
    /// Summary description for clsSelectByPoint.
    /// </summary>
    [Guid("c2882945-0817-457c-8a5e-7ced01ba2fac")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("SelectQuery.clsSelectByPoint")]
    public sealed class ClsSelectByPoint : BaseTool
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

        IGeometry m_pGeometry;
        Collection m_FeatClsCln=null;
        Collection m_pQueryResultCln;
        string mvarSelectMethod;
        FrmQuery m_frmQuery;

        IMapControl3 m_pMapControl;
        //System.Windows.Forms.Cursor m_SelCursor;

        public ClsSelectByPoint()
        {
            //
            //  Define values for the public properties
            //
            base.m_category = "SelectQuery"; //localizable text 
            base.m_caption = "点选择";  //localizable text 
            base.m_message = "点击鼠标选择要素";   //localizable text 
            base.m_toolTip = "点选择";  //localizable text
            base.m_name = "A1040000E";   //unique id, non-localizable (e.g. "MyCategory_MyTool")
            try
            {
                //
                
                //
                //string bitmapResourceName = GetType().Name + ".bmp";
                //base.m_bitmap = new Bitmap(GetType(), "Resources.点选择.bmp");
                //ResourceManager resource = new ResourceManager("SelectQuery.Resources", Assembly.GetExecutingAssembly());
                //base.m_bitmap = (Bitmap)resource.GetObject("点选择");
                base.m_cursor = System.Windows.Forms.Cursors.Cross;
                //ResourceManager rm = new ResourceManager("ZJGISSelectQuery.Properties.Resources", Assembly.GetExecutingAssembly());
                //base.m_bitmap = (Bitmap)rm.GetObject("点选择");
                //System.Windows.Forms.Cursors.w

                
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
            
     
            if (m_hookHelper == null)
            {
                m_hookHelper = new HookHelperClass();
            }
            if (hook!=null)
            {
                m_hookHelper.Hook = hook;
            }

            //ClsDeclare.g_Sys = new Common.ClsSysSet();
            //ClsDeclare.g_Sys.MapControl = m_hookHelper.Hook as IMapControl4;

            //m_SelCursor = new System.Windows.Forms.Cursor(GetType(), "Resources.PolygonSelect.cur");  //改嵌入式，，不然出现未实例化错误！！
        }

        /// <summary>
        /// Occurs when this tool is clicked
        /// </summary>
        /// 
        //public override int Cursor
        //{
        //    get
        //    {
        //        return m_SelCursor.Handle.ToInt32();
        //    }
        //}
        public override bool Enabled
        {
            get
            {
                if (m_hookHelper.FocusMap==null)
                    return false;
                if (m_hookHelper.FocusMap.LayerCount<=0)
                    return false;
                else
                    return true;
            }
        }
        public string SelectMethod
        {
            get
            {
                return mvarSelectMethod;
            }
            set
            {
                mvarSelectMethod = value;
            }
        }
        public override void OnClick()
        {
                
                IToolbarControl2 pToolbarControl;
                if (m_hookHelper.Hook is IToolbarControl)
                {

                    pToolbarControl = m_hookHelper.Hook as IToolbarControl2;
                    m_pMapControl = pToolbarControl.Buddy as IMapControl3;
                }
                else if (m_hookHelper.Hook is IMapControl3)
                    m_pMapControl = (IMapControl3)(m_hookHelper.Hook);
                ClsDeclare.g_pMap = m_pMapControl.Map;
        }

        public override void OnMouseDown(int Button, int Shift, int X, int Y)
        {
            try
            {
                //if (ClsDeclare.g_pMap.SelectionCount != 0)
                //    ClsDeclare.g_pMap.ClearSelection();
                ClsMapLayer pConvert = new ClsMapLayer();
                IActiveView pActiveView;
                pActiveView = m_hookHelper.ActiveView;

                IPoint pPoint;
                pPoint = pActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);

                ISelectionEnvironment pSelectEnv = new SelectionEnvironmentClass();
                double Length;
                Length = pConvert.ConvertPixelDistanceToMapDistance(pActiveView, pSelectEnv.SearchTolerance);

                ITopologicalOperator pTopoOpera;
                IGeometry pBuffer;
                pTopoOpera = (ITopologicalOperator)pPoint;
                pBuffer = pTopoOpera.Buffer(Length);
                m_pGeometry = pBuffer.Envelope;
                esriSelectionResultEnum pSelectType = new esriSelectionResultEnum();
                switch (Shift)
                {
                    case 0:
                        {
                            pSelectType = esriSelectionResultEnum.esriSelectionResultNew;
                            switch (mvarSelectMethod)
                            {
                                case "New":   //新建选择结果集
                                    pSelectType = esriSelectionResultEnum.esriSelectionResultNew;
                                    break;
                                case "Add":   //增加选择结果集 按下Shift键
                                    pSelectType = esriSelectionResultEnum.esriSelectionResultAnd;
                                    break;
                                case "Sub":  //减少选择结果集 按下Alt键
                                    pSelectType = esriSelectionResultEnum.esriSelectionResultSubtract;
                                    break;
                                case "Xor":   //对已有的选择集做异或 按下Ctrl键
                                    pSelectType = esriSelectionResultEnum.esriSelectionResultXOR;
                                    break;
                            }
                            break;
                        }
                    case 1:
                        pSelectType = esriSelectionResultEnum.esriSelectionResultAdd;
                        break;
                    case 2:
                        pSelectType = esriSelectionResultEnum.esriSelectionResultXOR;
                        break;
                    case 3:
                        pSelectType = esriSelectionResultEnum.esriSelectionResultNew;
                        break;
                    case 4:
                        pSelectType = esriSelectionResultEnum.esriSelectionResultSubtract;
                        break;
                }
                if (m_FeatClsCln == null)
                {
                    bool blntemp = true;
                    m_pQueryResultCln = new Collection();
                    ClsSelectQuery.SelectGeometry(ref blntemp, m_pGeometry, pSelectType, ref m_pQueryResultCln);
                    pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, null);
                    //TODO: FrmQuery的用法
                    if (m_frmQuery == null)
                        m_frmQuery = new FrmQuery();
                    if (m_frmQuery.IsDisposed)
                        m_frmQuery = new FrmQuery();

                    m_frmQuery.QueryResultCln = m_pQueryResultCln;
                    m_frmQuery.ReLoadQueryResult();
                    m_frmQuery.Visible = false;
                    m_frmQuery.Show((Form)(ClsDeclare.g_Sys.FrmMain));
                }
                m_pQueryResultCln = null;
                if ((ClsDeclare.g_pMap.SelectionCount > ClsDeclare.g_Sys.MaxSelectCount) && (ClsDeclare.g_Sys.MaxSelectCount != 0))
                {
                    //if (ClsDeclare.g_ErrorHandler.DisplayInformation("你选择的结果大于" + Convert.ToString(ClsDeclare.g_Sys.MaxSelectCount) + "个地物，是否继续选择操作?", true, "确定", "取消") == false)
                    {
                        pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, null);
                        ClsDeclare.g_pMap.ClearSelection();
                        pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, null);
                    }
                }
                
            }
            catch (Exception)
            {
                //ClsDeclare.g_ErrorHandler.HandleError(true, null, 0, null, ex.StackTrace);
                throw;
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
