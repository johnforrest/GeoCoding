//<CSCC>
//-------------------------------------------------------------------------------------
//    部件名：ClsLyrExtent
//    工程名：LayerManager
//    版权: CopyRight (c) 2010
//    创建人：
//    描述：查看图层范围及属性
//    创建日期：
//    修改人：
//    修改说明：
//    修改日期：
//-------------------------------------------------------------------------------------
//</CSCC>


using System;
using System.Resources;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ZJGISLayerManager;
using System.Windows.Forms;

namespace ZJGISLayerManager
{
    /// <summary>
    /// Command that works in ArcMap/Map/PageLayout, ArcScene/SceneControl
    /// or ArcGlobe/GlobeControl
    /// </summary>
    [Guid("d50549fc-6d32-4729-ae84-2a803c2c29ae")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("ZJGISLayerManager.Classes.ClsLyrExtent")]
    public sealed class ClsLyrExtent : BaseCommand
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

        private Form m_pFrmMain;
        private ILayer m_pLayer;
        private FrmLyrExtent m_pFrmLyrExtent = new FrmLyrExtent();

        private IHookHelper m_hookHelper = null;
        private IGlobeHookHelper m_globeHookHelper = null;
        private ISceneHookHelper m_sceneHookHelper = null;

        public ILayer Layer
        {
            get
            {
                return m_pLayer;
            }
            set
            {
                m_pLayer = value;
            }
        }

        public ClsLyrExtent(Form FrmMain)
        {
            //
            
            //
            m_pFrmMain = FrmMain;

            base.m_category = "LayerManager"; //localizable text
            base.m_caption = "图层属性";  //localizable text
            base.m_message = "查看图层范围及属性";   //localizable text 
            base.m_toolTip = "图层属性";  //localizable text 
            base.m_name = "LyrExtent";   //unique id, non-localizable (e.g. "MyCategory_MyCommand")

            ResourceManager resBitmap = new ResourceManager("ZJGISLayerManager.Properties.Resources", System.Reflection.Assembly.GetExecutingAssembly());
            try
            {
                //
                
                ////
                base.m_bitmap = (Bitmap)resBitmap.GetObject("LayerProperty");
                //string bitmapResourceName = GetType().Name + ".bmp";
                //base.m_bitmap = new Bitmap(GetType(), bitmapResourceName);

            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message, "Invalid Bitmap");
            }
        }

        public override bool Enabled
        {
            get
            {
                if ((m_pLayer is IFeatureLayer)||(m_pLayer is ITinLayer)||(m_pLayer is IRasterLayer))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }

        #region Overriden Class Methods

        /// <summary>
        /// Occurs when this command is created
        /// </summary>
        /// <param name="hook">Instance of the application</param>
        public override void OnCreate(object hook)
        {
            if (hook == null)
                return;

            // Test the hook that calls this command and disable if nothing is valid
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
                //Can be scene or globe
                try
                {
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

            
        }

        /// <summary>
        /// Occurs when this command is clicked
        /// </summary>
        public override void OnClick()
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

            IGeoDataset pGeoDataSet;
            IMapControl3 pMapControl;

            try
            {
                if (m_pFrmLyrExtent==null)
                {
                    m_pFrmLyrExtent = new FrmLyrExtent();
                }
                else
                {
                    if (m_pFrmLyrExtent.IsDisposed)
                    {
                        m_pFrmLyrExtent = new FrmLyrExtent();
                    }
                }
                pMapControl = (IMapControl3)m_hookHelper.Hook;

                if (m_pLayer==null)
                {
                    return;
                }
                m_pFrmLyrExtent.Layer = m_pLayer;
                m_pFrmLyrExtent.Show(m_pFrmMain);
            }
            catch (Exception)
            {
                m_pLayer = null;
                pGeoDataSet = null;
            }
        }

        #endregion
    }
}
