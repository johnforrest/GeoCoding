using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Carto;
using System.Resources;

namespace ZJGISLayerManager
{
    /// <summary>
    /// Summary description for ClsLayerSelectable.
    /// </summary>
    [Guid("615bd19d-f1b1-481f-a5c6-9e845f36352b")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("ZJGISLayerManager.Classes.ClsLayerSelectable")]
    public sealed class ClsLayerSelectable : BaseCommand,ICommandSubType
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
        private int m_lSubType;
        private ILayer m_pLayer;
        private IFeatureLayer m_pFeatLyr;
        ITOCControl2 m_TocControl;

        private IHookHelper m_hookHelper = null;
        private IGlobeHookHelper m_globeHookHelper = null;
        private ISceneHookHelper m_sceneHookHelper = null;

        public ClsLayerSelectable()
        {
            //
            
            //
            base.m_category = "LayerManager"; //localizable text
            base.m_caption = "设置图层可选或不可选";  //localizable text
            base.m_message = "";  //localizable text 
            base.m_toolTip = "";  //localizable text 
            base.m_name = "ClsLayerSelectable";   //unique id, non-localizable (e.g. "MyCategory_MyCommand")

            //try
            //{
            //    //
            //    
            //    //
            //    //string bitmapResourceName = GetType().Name + ".bmp";
            //    //base.m_bitmap = new Bitmap(GetType(), bitmapResourceName);
            //}
            //catch (Exception ex)
            //{
            //    System.Diagnostics.Trace.WriteLine(ex.Message, "Invalid Bitmap");
            //}
        }

        public ITOCControl2 TocControl
        {
            get
            {
                return m_TocControl;
            }
            set
            {
                m_TocControl=value;
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
            //  Add other initialization code
        }

        /// <summary>
        /// Occurs when this command is clicked
        /// </summary>
        public override void OnClick()
        {
            // TODO: Add ClsLayerSelectable.OnClick implementation
            m_pFeatLyr = (IFeatureLayer)m_pLayer;
            if (m_lSubType == 1)
            {
                m_pFeatLyr.Selectable = true;           //可选
            }
            if (m_lSubType == 2)
            {
                m_pFeatLyr.Selectable = false;          //不可选
            }
          
            //m_TocControl.ActiveView.Refresh();
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
                    if (m_lSubType == 1)
                    {
                        return !pFeatureLayer.Selectable;
                    }
                    else if (m_lSubType == 2)
                    {
                        return pFeatureLayer.Selectable;
                    }
                    else
                    {
                        return false;
                    }
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
                if (m_lSubType == 1)
                {
                    return "图层可选";
                }
                else
                {
                    return "图层不可选";
                }
            }
        }

        public int GetCount()
        {
            //throw new NotImplementedException();
            return 2;
        }

        public void SetSubType(int SubType)
        {
            //throw new NotImplementedException();
            m_lSubType = SubType;

            ResourceManager resource = new ResourceManager("ZJGISLayerManager.Properties.Resources", System.Reflection.Assembly.GetExecutingAssembly());

            if (m_lSubType == 1)
            {
                base.m_bitmap = (Bitmap)resource.GetObject("可选择");
                base.m_message = "设置图层为不可选择状态";
            }
            else if (m_lSubType == 2)
            {
                base.m_bitmap = (Bitmap)resource.GetObject("不可选择");
                base.m_message = "设置图层为可以选择状态";
            }
        }
        #endregion
    }
}
