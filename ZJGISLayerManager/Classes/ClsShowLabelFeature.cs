//<CSCC>
//-------------------------------------------------------------------------------------
//    部件名：ClsShowLabelFeature
//    工程名：LayerManager
//    版权: CopyRight (c) 2010
//    创建人：
//    描述：显示标注
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
using ESRI.ArcGIS.Display;
using System.Resources;

namespace ZJGISLayerManager
{
    /// <summary>
    /// Command that works in ArcMap/Map/PageLayout, ArcScene/SceneControl
    /// or ArcGlobe/GlobeControl
    /// </summary>
    [Guid("1cc0df8c-b280-4021-9483-0f5b297a735c")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("ZJGISLayerManager.Classes.ClsShowLabelFeature")]
    public sealed class ClsShowLabelFeature : BaseCommand
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

        bool m_bShow;               //是否对要素进行标注
        ILayer m_pLayer;

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

        public ClsShowLabelFeature()
        {
            //
            //Define values for the public properties
            //
            base.m_category = "LayerManager"; //localizable text
            base.m_caption = "显示标注";  //localizable text
            base.m_message = "显示标注";   //localizable text 
            base.m_toolTip = "显示标注";  //localizable text 
            base.m_name = "ShowLabelFeature";   //unique id, non-localizable (e.g. "MyCategory_MyCommand")

            try
            {
                //
                
                //
                ResourceManager resBitmap = new ResourceManager("ZJGISLayerManager.Properties.Resources", System.Reflection.Assembly.GetExecutingAssembly());
                base.m_bitmap = (Bitmap)resBitmap.GetObject("ShowLabelFeature");
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
                bool bEnabled = false;
                if (m_pLayer is IGeoFeatureLayer)
                {
                    bEnabled = true;
                }
                else
                {
                    bEnabled = false;
                }
                return bEnabled;
            }
        }

        public override string Caption
        {
            get
            {
                IGeoFeatureLayer pGeoFeatureLayer;
                if (m_pLayer is IGeoFeatureLayer)
                {
                    pGeoFeatureLayer = (IGeoFeatureLayer)m_pLayer;
                    if (pGeoFeatureLayer.DisplayAnnotation==true)
                    {
                        return "取消标注";
                    }
                    else
                    {
                        return "显示标注";
                    }
                }
                else
                {
                    return "显示标注";
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

            
            m_bShow = false;
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

            IActiveView pActiveView;
            IGeoFeatureLayer pGeoFeatureLayer;

            if (m_pLayer is IGeoFeatureLayer)
            {
                pGeoFeatureLayer = (IGeoFeatureLayer)m_pLayer;
                //设置标注已经有别外的模块单独完成

                if (pGeoFeatureLayer.DisplayAnnotation == false)
                {
                    pGeoFeatureLayer.DisplayAnnotation = true;
                    pActiveView = m_hookHelper.ActiveView;
                    //pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
                    pActiveView.Refresh();
                    return;
                }
                if (pGeoFeatureLayer.DisplayAnnotation == true)
                {
                    pGeoFeatureLayer.DisplayAnnotation = false;
                    pActiveView = m_hookHelper.ActiveView;
                    //pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
                    pActiveView.Refresh();
                    return;
                }
               
            }

        }

        #endregion
    }
}
