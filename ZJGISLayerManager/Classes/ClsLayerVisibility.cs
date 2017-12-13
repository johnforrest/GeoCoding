//<CSCC>
//-------------------------------------------------------------------------------------
//    部件名：ClsLayerVisibility
//    工程名：LayerManager
//    版权: CopyRight (c) 2010
//    创建人：
//    描述：当前图层是否可见
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
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using System.Resources;

namespace ZJGISLayerManager
{
    /// <summary>
    /// Command that works in ArcMap/Map/PageLayout, ArcScene/SceneControl
    /// or ArcGlobe/GlobeControl
    /// </summary>
    [Guid("f0525d79-73f8-4f41-ae49-e8f8157bee89")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("ZJGISLayerManager.Classes.ClsLayerVisibility")]
    public sealed class ClsLayerVisibility : BaseCommand,ICommandSubType
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

        private int m_lSubType;

        private IHookHelper m_hookHelper = null;
        private IGlobeHookHelper m_globeHookHelper = null;
        private ISceneHookHelper m_sceneHookHelper = null;

        public ClsLayerVisibility()
        {
            //
            
            //
            //base.m_category = ""; //localizable text
            //base.m_caption = "";  //localizable text
            //base.m_message = "This should work in ArcMap/MapControl/PageLayoutControl, " +
            //                 "ArcScene/SceneControl, ArcGlobe/GlobeControl";   //localizable text 
            //base.m_toolTip = "";  //localizable text 
            //base.m_name = "";   //unique id, non-localizable (e.g. "MyCategory_MyCommand")

            //try
            //{
            //    //
            //    
            //    //
            //    string bitmapResourceName = GetType().Name + ".bmp";
            //    base.m_bitmap = new Bitmap(GetType(), bitmapResourceName);
            //}
            //catch (Exception ex)
            //{
            //    System.Diagnostics.Trace.WriteLine(ex.Message, "Invalid Bitmap");
            //}
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

            int i, j;
            IGroupLayer pGroupLayer;
            ICompositeLayer pCompositeLayer;

            for (i = 0; i <= m_hookHelper.FocusMap.LayerCount-1; i++)
            {
                if (m_lSubType==1)
                {
                    m_hookHelper.FocusMap.get_Layer(i).Visible = true;
                    if (m_hookHelper.FocusMap.get_Layer(i) is IGroupLayer)
                    {
                        pGroupLayer = (IGroupLayer)m_hookHelper.FocusMap.get_Layer(i);
                        pCompositeLayer = (ICompositeLayer)pGroupLayer;
                        for (j = 0; j <= pCompositeLayer.Count-1; j++)
                        {
                            pCompositeLayer.get_Layer(j).Visible = true;
                        }
                    }
                    else if (m_lSubType==2)
                    {
                        m_hookHelper.FocusMap.get_Layer(i).Visible = false;
                        if (m_hookHelper.FocusMap.get_Layer(i) is IGroupLayer)
                        {
                            pGroupLayer = (IGroupLayer)m_hookHelper.FocusMap.get_Layer(i);
                            pCompositeLayer = (ICompositeLayer)pGroupLayer;
                            for (j = 0; j <= pCompositeLayer.Count-1; j++)
                            {
                                pCompositeLayer.get_Layer(j).Visible = false;
                            }
                        }
                    }
                }
            }
            m_hookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
        }

        //判断命令的有效性
        public override bool Enabled
        {
            get
            {
                int i;
                bool bEnabled = false;

                if (m_lSubType==1)
                {
                    for (i = 0; i <= m_hookHelper.FocusMap.LayerCount-1; i++)
                    {
                        if (m_hookHelper.FocusMap.get_Layer(i).Visible==false)
                        {
                            bEnabled = true;
                            break;
                        }
                    }
                }
                else if (m_lSubType==2)
                {
                    for (i = 0; i <= m_hookHelper.FocusMap.LayerCount-1; i++)
                    {
                        if (m_hookHelper.FocusMap.get_Layer(i).Visible==true)
                        {
                            bEnabled = true;
                            break;
                        }
                    }
                }
                return bEnabled;
            }

        }

        //protected override void Finalize()
        //{
        //    m_hookHelper = null;
        //}

        #endregion

        #region ICommandSubType Members

        public int GetCount()
        {
            //throw new NotImplementedException();
            return 2;
        }

        public void SetSubType(int SubType)
        {
            //throw new NotImplementedException();
            m_lSubType = SubType;
            ResourceManager resource = new ResourceManager("LayerManager.Properties.Resources", System.Reflection.Assembly.GetExecutingAssembly());

            if (m_lSubType==1)
            {
                base.m_category = "LayerManager"; //localizable text
                base.m_caption = "打开所有图层";  //localizable text
                base.m_message = "设置图层为可见状态";   //localizable text 
                base.m_toolTip = "图层可见";  //localizable text 
                base.m_name = "IsVisible_false";   //unique id, non-localizable (e.g. "MyCategory_MyCommand")

                base.m_bitmap = (Bitmap)resource.GetObject("IsVisible_false");
            }
            else if (m_lSubType==2)
            {
                base.m_category = "LayerManager"; //localizable text
                base.m_caption = "关闭所有图层";  //localizable text
                base.m_message = "设置图层为不可见状态";   //localizable text 
                base.m_toolTip = "图层不可见";  //localizable text 
                base.m_name = "IsVisible_true";   //unique id, non-localizable (e.g. "MyCategory_MyCommand")

                //base.m_bitmap = (Bitmap)resource.GetObject("IsVisible_true");
            }
        }

        #endregion
    }
}
