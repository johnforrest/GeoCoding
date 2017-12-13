//<CSCC>
//-------------------------------------------------------------------------------------
//    部件名：ClsRemoveAllLayer
//    工程名：LayerManager
//    版权: CopyRight (c) 2010
//    创建人：
//    描述：移除全部图层
//    创建日期：
//    修改人：
//    修改说明：
//    修改日期：
//-------------------------------------------------------------------------------------
//</CSCC>


using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using System.Resources;
using ZJGISLayerManager;

namespace ZJGISLayerManager
{
    /// <summary>
    /// Command that works in ArcMap/Map/PageLayout, ArcScene/SceneControl
    /// or ArcGlobe/GlobeControl
    /// </summary>
    [Guid("7d99ad55-1ab6-4435-a6b0-821177202dc1")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("ZJGISLayerManager.Classes.ClsRemoveAllLayer")]
    public sealed class ClsRemoveAllLayer : BaseCommand
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
        private AxTOCControl m_pTocControl;
        private IMapControl4 m_pMapControl;

        public ClsRemoveAllLayer(AxTOCControl pTocControl,IMapControl4 pMapControl)
        {
            //
            
            //
            m_pTocControl = pTocControl;
            m_pMapControl = pMapControl;

            base.m_category = "LayerManager"; //localizable text
            base.m_caption = "删除所有图层";  //localizable text
            base.m_message = "删除所有图层";   //localizable text 
            base.m_toolTip = "删除所有图层";  //localizable text 
            base.m_name = "RemoveAllLayer";   //unique id, non-localizable (e.g. "MyCategory_MyCommand")

            try
            {
                //
                

                ResourceManager resBitmap = new ResourceManager("ZJGISLayerManager.Properties.Resources", System.Reflection.Assembly.GetExecutingAssembly());
                base.m_bitmap = (Bitmap)resBitmap.GetObject("RemoveLayer");
                ////string bitmapResourceName = GetType().Name + ".bmp";
                //base.m_bitmap = new Bitmap(GetType(), bitmapResourceName);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message, "Invalid Bitmap");
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

            int i;
            IMapControl3 pMapControl = (IMapControl3)m_hookHelper.Hook;

            //ModDeclare.g_Sys.TocControl.SetBuddyControl(null);
            m_pTocControl.SetBuddyControl(null);
            for (i = pMapControl.Map.LayerCount-1; i >= 0; i--)
            {
                //pMapControl.Map.DeleteLayer(pMapControl.Map.get_Layer(i));
                m_pMapControl.Map.DeleteLayer(pMapControl.Map.get_Layer(i));
            }
            pMapControl.SpatialReference = null;

            //ModDeclare.g_Sys.TocControl.SetBuddyControl(ModDeclare.g_Sys.MapControl);
            //ModDeclare.g_Sys.TocControl.ActiveView.Refresh();
            m_pTocControl.SetBuddyControl(m_pMapControl);
            m_pTocControl.ActiveView.Refresh();
        }

        #endregion
    }
}
