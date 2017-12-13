using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using System.Resources;
using ESRI.ArcGIS.Carto;

namespace ZJGISLayerManager
{
    /// <summary>
    /// Summary description for ClsUnSelectableLayer.
    /// </summary>
    [Guid("0faa91cd-e033-463a-900a-5fdfe4498136")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("ZJGISLayerManager.Classes.ClsUnSelectableLayer")]
    public sealed class ClsUnSelectableLayer : BaseCommand
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
            ControlsCommands.Register(regKey);

        }
        /// <summary>
        /// Required method for ArcGIS Component Category unregistration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryUnregistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            ControlsCommands.Unregister(regKey);

        }

        #endregion
        #endregion

        private IMapControl3 m_pMapControl;
        private ILayer m_pLayer;
        private IFeatureLayer m_pFeatLyr;


        private IHookHelper m_hookHelper = null;
        private IGlobeHookHelper m_globeHookHelper = null;
        private ISceneHookHelper m_sceneHookHelper = null;

        public ClsUnSelectableLayer()
        {
            //
            // Define values for the public properties
            //
            base.m_category = "LayerManager"; //localizable text
            base.m_caption = "";  //localizable text
            base.m_message = "设置图层为可以选择状态";  //localizable text 
            base.m_toolTip = "";  //localizable text 
            base.m_name = "ClsUnSelectableLayer";   //unique id, non-localizable (e.g. "MyCategory_MyCommand")

            try
            {
                ////
                //
                ////
                //string bitmapResourceName = GetType().Name + ".bmp";
                //base.m_bitmap = new Bitmap(GetType(), bitmapResourceName);
                ResourceManager resource = new ResourceManager("LayerManager.Properties.Resources", System.Reflection.Assembly.GetExecutingAssembly());
                base.m_bitmap = (Bitmap)resource.GetObject("不可选择");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message, "Invalid Bitmap");
            }
        }
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

        #region Overriden Class Methods

        /// <summary>
        /// Occurs when this command is created
        /// </summary>
        /// <param name="hook">Instance of the application</param>
        public override void OnCreate(object hook)
        {
            if (hook == null)
                return;

            if (m_hookHelper == null)
                m_hookHelper = new HookHelperClass();

            m_hookHelper.Hook = hook;
            m_pMapControl = (IMapControl3)m_hookHelper.Hook;
        }

        /// <summary>
        /// Occurs when this command is clicked
        /// </summary>
        public override void OnClick()
        {
            // TODO: Add ClsUnSelectableLayer.OnClick implementation
            m_pFeatLyr.Selectable = false;
        }
        public override bool Enabled
        {
            get
            {
                //对矢量图层有效
                if (m_pLayer is IFeatureLayer)
                {
                    IFeatureLayer pFeatureLayer;
                    pFeatureLayer = (IFeatureLayer)m_pLayer;

                    return pFeatureLayer.Selectable;
                }
                else
                {
                    return false;
                }
            }
        }
        public override string Caption
        {
            get
            {


                return "图层不可选";

            }
        }
        #endregion
    }
}
