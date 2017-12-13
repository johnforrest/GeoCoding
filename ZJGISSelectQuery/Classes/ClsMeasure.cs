using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using System.Windows.Forms;
using System.Resources;
using System.Reflection;
using SelectQuery;

namespace SelectQuery
{
    
    public sealed class ClsMeasure : BaseTool
    {
        

        private IHookHelper m_hookHelper = null;
        private IGlobeHookHelper m_globeHookHelper = null;
        private ISceneHookHelper m_sceneHookHelper = null;

        public ClsMeasure()
        {
            //
            
            //
            base.m_category = "SelectQuery"; //localizable text 
            base.m_caption = "量算";  //localizable text 
            base.m_message = "左键开始量算，双击结束";   //localizable text 
            base.m_toolTip = "量算";  //localizable text
            base.m_name = "A10410E";   //unique id, non-localizable (e.g. "MyCategory_MyTool")
            try
            {
                //
                
                //
                //string bitmapResourceName = GetType().Name + ".bmp";
                //base.m_bitmap = new Bitmap(GetType(), "Resources.Measure.bmp");
                //base.m_cursor = new System.Windows.Forms.Cursor(GetType(), GetType().Name + ".cur");
                //ResourceManager resource = new ResourceManager("SelectQuery.Resources", Assembly.GetExecutingAssembly());
                //base.m_bitmap = (Bitmap)resource.GetObject("Measure");
                //ResourceManager rm = new ResourceManager("ZJGISSelectQuery.Properties.Resources", Assembly.GetExecutingAssembly());
                //base.m_bitmap = (Bitmap)rm.GetObject("Measure");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message, "Invalid Bitmap");
            }
        }
        public string SysSelectStyle
        {
            set
            {
                ClsDeclare.g_SysSelectStyle = value;
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
            //ClsDeclare.g_pMap = m_hookHelper.FocusMap;
        }

        /// <summary>
        /// Occurs when this tool is clicked
        /// </summary>
        public override void OnClick()
        {
            if (m_hookHelper != null)
            {
                
                FrmMeasure frmMea = new FrmMeasure();
                IToolbarControl2 pToolbarCon;
                if (m_hookHelper.Hook is IToolbarControl)
                {
                    pToolbarCon = (IToolbarControl2)(m_hookHelper.Hook);
                    frmMea.MainMap = (IMapControl3)(pToolbarCon.Buddy);
                    
                }
                else if (m_hookHelper.Hook is IMapControl3)
                {
                    frmMea.MainMap = (IMapControl3)(m_hookHelper.Hook);
                     
                }
                frmMea.Show((Form)(ClsDeclare.g_Sys.FrmMain));
                ClsDeclare.g_Sys.MapControl.CurrentTool = null;
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
