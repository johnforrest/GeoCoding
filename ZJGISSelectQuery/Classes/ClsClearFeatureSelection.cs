﻿using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using System.Resources;
using System.Reflection;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
namespace SelectQuery
{
    /// <summary>
    /// Command that works in ArcMap/Map/PageLayout, ArcScene/SceneControl
    /// or ArcGlobe/GlobeControl
    /// </summary>
    [Guid("ee12b3df-9795-4d39-a0f6-dc76d5e8b50d")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("SelectQuery.clsClearFeatureSelection")]
    public sealed class ClsClearFeatureSelection : BaseCommand
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

        public ClsClearFeatureSelection()
        {
            //
            
            //
            base.m_category = "SelectQuery"; //localizable text
            base.m_caption = "取消选择";  //localizable text
            base.m_message = "清除所选择的要素";   //localizable text 
            base.m_toolTip = "取消选择";  //localizable text 
            base.m_name = "A10403E";   //unique id, non-localizable (e.g. "MyCategory_MyCommand")

            try
            {
                //
                
                //
                //string bitmapResourceName = GetType().Name + ".bmp";
                //base.m_bitmap = new Bitmap(GetType(), "Resources.ClearSelection1.bmp");
                //ResourceManager resource = new ResourceManager("SelectQuery.Resources", Assembly.GetExecutingAssembly());
                //base.m_bitmap = (Bitmap)resource.GetObject("ClearSelection"); 

                //ResourceManager rm = new ResourceManager("ZJGISSelectQuery.Properties.Resources", Assembly.GetExecutingAssembly());
                //base.m_bitmap = (Bitmap)rm.GetObject("ClearSelection1");
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

            
            ClsDeclare.g_pMap = m_hookHelper.FocusMap;
        }

        /// <summary>
        /// Occurs when this command is clicked
        /// </summary>
        public override bool Enabled
        {
            get
            {
                if (ClsDeclare.g_pMap.SelectionCount!=0)
                    return true;
                else
                    return false;
                //return base.Enabled;
            }
        }
        public override void OnClick()
        {
            if (m_hookHelper != null)
            {
                
                m_hookHelper.FocusMap.ClearSelection();

                IGraphicsContainer pGraphicsContainer = m_hookHelper.ActiveView as IGraphicsContainer;
                pGraphicsContainer.DeleteAllElements();

                IActiveView pActiveView;
                IEnvelope pEnv;
                pActiveView = m_hookHelper.ActiveView;
                pEnv = pActiveView.ScreenDisplay.DisplayTransformation.FittedBounds;
                pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection,null,pEnv);
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
