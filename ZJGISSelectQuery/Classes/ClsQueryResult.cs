﻿using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using System.Windows.Forms;
using ESRI.ArcGIS.Geometry;
using System.Resources;
using System.Reflection;
using ESRI.ArcGIS.Carto;
using Microsoft.VisualBasic;
using ZJGISCommon;

namespace SelectQuery 
{
    /// <summary>
    /// Summary description for clsQueryResult.
    /// </summary>
    [Guid("14f2a3b7-dd99-4a52-a36b-d089c6a3214e")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("SelectQuery.clsQueryResult")]
    public sealed class ClsQueryResult : BaseTool
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

        IMapControl3 m_pMapControl;
        string mvarSelectMethod;
        //IGeometry m_pGeometry;
        Collection m_FeatClsCln=null;
        Collection m_pQueryResultCln;
        FrmQuery m_frmQuery;

        public ClsQueryResult()
        {
            //
            
            //
            base.m_category = "SelectQuery"; //localizable text 
            base.m_caption = "查询结果";  //localizable text 
            base.m_message = "点选或框选查询得到的要素";   //localizable text 
            base.m_toolTip = "查询结果";  //localizable text
            base.m_name = "A10406E";   //unique id, non-localizable (e.g. "MyCategory_MyTool")
            try
            {
                //
                
                ////
                //string bitmapResourceName = GetType().Name + ".bmp";
                //base.m_bitmap = new Bitmap(GetType(), "Resources.Identify.bmp");
                //ResourceManager resource = new ResourceManager("SelectQuery.Resources", Assembly.GetExecutingAssembly());
                //base.m_bitmap = (Bitmap)resource.GetObject("Identify");
                ResourceManager rm = new ResourceManager("ZJGISSelectQuery.Properties.Resources", Assembly.GetExecutingAssembly());
                base.m_bitmap = (Bitmap)rm.GetObject("Identify");

                //base.m_cursor = new System.Windows.Forms.Cursor(GetType(), "Resources.Identify.cur");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message, "Invalid Bitmap");
            }
            m_frmQuery = new FrmQuery();
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
                
                IToolbarControl pTC = new ToolbarControlClass();
                //IMapControl3 pMC = new MapControlClass();
                if (m_hookHelper.Hook is IToolbarControl)
                {
                    
                    pTC = (IToolbarControl)(m_hookHelper.Hook);
                    m_pMapControl= (IMapControl3)(pTC.Buddy);
                    //m_pMapControl = (IMapControl3)pMC;
                }
                else if (m_hookHelper.Hook is IMapControl3)
                {
                    m_pMapControl = (IMapControl3)(m_hookHelper.Hook);
                }
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
                
                ClsMapLayer pConvert=new ClsMapLayer();
                IGeometry pGeometry;
                IActiveView pActiveView;
                pActiveView=m_hookHelper.ActiveView;
                IPoint pPoint;
                pPoint=pActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(X,Y);
                pGeometry=(IGeometry)pPoint;

                if(ClsDeclare.g_pMap.SelectionCount!=0)
                    ClsDeclare.g_pMap.ClearSelection();
                pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, null);

                ISelectionEnvironment pSelectEnv=new SelectionEnvironmentClass();
                double Length;
                Length = pConvert.ConvertPixelDistanceToMapDistance(pActiveView, pSelectEnv.SearchTolerance);

                ITopologicalOperator pTopoOpera;
                IGeometry pBuffer;
                pTopoOpera=(ITopologicalOperator)pGeometry;
                pBuffer=pTopoOpera.Buffer(Length);

                IEnvelope pEnvelope;
                pEnvelope = m_pMapControl.TrackRectangle();
                pGeometry = (IGeometry)pEnvelope;

                Application.DoEvents();
                m_pMapControl.MousePointer = esriControlsMousePointer.esriPointerHourglass;

                if (pGeometry != null)
                {
                    if (pEnvelope.IsEmpty)
                        pGeometry = pBuffer.Envelope;
                }
                esriSelectionResultEnum pselecttype=new esriSelectionResultEnum();
                switch(Shift)
                {
                    case 0:
                        {
                            pselecttype = esriSelectionResultEnum.esriSelectionResultNew;
                            switch(mvarSelectMethod)
                            {
                                case "New":
                                    pselecttype = esriSelectionResultEnum.esriSelectionResultNew;
                                    break;
                                case "Add":
                                    pselecttype = esriSelectionResultEnum.esriSelectionResultAnd;
                                    break;
                                case "Sub":
                                    pselecttype = esriSelectionResultEnum.esriSelectionResultSubtract;
                                    break;
                                    
                                case "Xor":
                                    pselecttype = esriSelectionResultEnum.esriSelectionResultXOR;
                                    break;
                            }
                            break;
                        }
                    case 1:
                        pselecttype = esriSelectionResultEnum.esriSelectionResultAdd;
                        break;
                    case 2:
                        pselecttype = esriSelectionResultEnum.esriSelectionResultXOR;
                        break;
                    case 3:
                        pselecttype = esriSelectionResultEnum.esriSelectionResultNew;
                        break;
                    case 4:
                        pselecttype = esriSelectionResultEnum.esriSelectionResultSubtract;
                        break;
                }
                if(m_FeatClsCln==null)
                {
                    bool blntemp = false;
                    m_pQueryResultCln = new Collection();
                    ClsSelectQuery.SelectGeometry(ref blntemp, pGeometry, pselecttype, ref m_pQueryResultCln);
                    pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection,null,null);
                    if(m_frmQuery==null)
                        m_frmQuery = new FrmQuery();
                    if(m_frmQuery.IsDisposed)
                        m_frmQuery = new FrmQuery();
                    m_frmQuery.QueryResultCln = m_pQueryResultCln;
                    m_frmQuery.ReLoadQueryResult();
                    m_frmQuery.Visible = false;
                    m_frmQuery.Show((Form)(ClsDeclare.g_Sys.FrmMain));
                }
               m_pQueryResultCln = null;
               m_pMapControl.MousePointer = esriControlsMousePointer.esriPointerDefault;
            }
            else if (m_sceneHookHelper != null)
            {
               
            }
            else if (m_globeHookHelper != null)
            {
                
            }
        }

        //private void SelectGeometry(bool p, IGeometry pGeometry, esriSelectionResultEnum pselecttype, Collection m_pQueryResultCln)
        //{
        //    throw new NotImplementedException();
        //}

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
        public override bool Enabled
        {
            get
            {
                if (m_hookHelper.FocusMap == null)
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
        #endregion
    }
}
