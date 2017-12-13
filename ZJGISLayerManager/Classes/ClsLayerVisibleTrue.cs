using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using System.Resources;
using ESRI.ArcGIS.Carto;

namespace ZJGISLayerManager.Classes
{
    /// <summary>
    /// Command that works in ArcMap/Map/PageLayout
    /// </summary>
    [Guid("1e9fcb8a-c4a8-4b92-8cf8-979fb4916862")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("ZJGISLayerManager.Classes.ClsLayerVisibleTrue")]
    public sealed class ClsLayerVisibleTrue : BaseCommand
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
            MxCommands.Register(regKey);
            ControlsCommands.Register(regKey);
        }
        /// <summary>
        /// Required method for ArcGIS Component Category unregistration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryUnregistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            MxCommands.Unregister(regKey);
            ControlsCommands.Unregister(regKey);
        }

        #endregion
        #endregion

        private IHookHelper m_hookHelper = null;
        public ClsLayerVisibleTrue()
        {
            //
            
            //
            base.m_category = "ZJGISMapLayerManage"; //localizable text
            base.m_caption = "打开所有图层";  //localizable text 
            base.m_message = "设置图层为可见状态";  //localizable text
            base.m_toolTip = "图层可见";  //localizable text
            base.m_name = "IsVisible_true";   //unique id, non-localizable (e.g. "MyCategory_MyCommand")

            try
            {
                //
                
                //
                ResourceManager resource = new ResourceManager("ZJGISLayerManager.Properties.Resources", System.Reflection.Assembly.GetExecutingAssembly());
                base.m_bitmap = (Bitmap)resource.GetObject("IsVisible_true");
                //string bitmapResourceName = GetType().Name + ".bmp";
                //base.m_bitmap = new Bitmap(GetType(), bitmapResourceName);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message, "图层可见图标不可用");
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

            try
            {
                m_hookHelper = new HookHelperClass();
                m_hookHelper.Hook = hook;
                if (m_hookHelper.ActiveView == null)
                    m_hookHelper = null;
            }
            catch
            {
                m_hookHelper = null;
            }

            if (m_hookHelper == null)
                base.m_enabled = false;
            else
                base.m_enabled = true;

        }

        /// <summary>
        /// Occurs when this command is clicked
        /// </summary>
        public override void OnClick()
        {
            // TODO: Add ClsLayerVisibleTrue.OnClick implementation
            IGroupLayer pGroupLayer;
            ICompositeLayer pCompositeLayer;

            for (int i = 0; i < m_hookHelper.FocusMap.LayerCount; i++)
            {
                m_hookHelper.FocusMap.get_Layer(i).Visible = true;
                if (m_hookHelper.FocusMap.get_Layer(i) is IGroupLayer)
                {
                    pGroupLayer = (IGroupLayer)m_hookHelper.FocusMap.get_Layer(i);
                    pCompositeLayer = (ICompositeLayer)pGroupLayer;
                    for (int j = 0; j < pCompositeLayer.Count; j++)
                    {
                        pCompositeLayer.get_Layer(j).Visible = true;
                    }
                }
            }
            m_hookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);

        }

        #endregion
    }
}
