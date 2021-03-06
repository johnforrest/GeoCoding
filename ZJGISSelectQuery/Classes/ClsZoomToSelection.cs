﻿using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using System.Resources;
using System.Reflection;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using ZJGISCommon;


namespace SelectQuery 
{
    /// <summary>
    /// Command that works in ArcMap/Map/PageLayout, ArcScene/SceneControl
    /// or ArcGlobe/GlobeControl
    /// </summary>
    [Guid("8f5c699f-080b-4f65-901d-64e1dec79ad0")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("SelectQuery.clsZoomToSelection")]
    public sealed class ClsZoomToSelection : BaseCommand
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

        public ClsZoomToSelection()
        {
            //
            
            //
            base.m_category = "SelectQuery"; //localizable text
            base.m_caption = "放大到选择要素";  //localizable text
            base.m_message = "放大到选择要素";   //localizable text 
            base.m_toolTip = "放大到选择要素";  //localizable text 
            base.m_name = "A10402E";   //unique id, non-localizable (e.g. "MyCategory_MyCommand")

            try
            {
                //
                
                //
                //string bitmapResourceName = GetType().Name + ".bmp";
                //base.m_bitmap = new Bitmap(GetType(), "Resources.放大到选择.bmp");

                //ResourceManager rm = new ResourceManager("ZJGISSelectQuery.Properties.Resources", Assembly.GetExecutingAssembly());
                //base.m_bitmap = (Bitmap)rm.GetObject("放大到选择");

                //ResourceManager resource = new ResourceManager("SelectQuery.Resources", Assembly.GetExecutingAssembly());
                //base.m_bitmap = (Bitmap)resource.GetObject("放大到选择");
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
        public override void OnClick()
        {
            if (m_hookHelper != null)
            {
                
                ClsMapLayer pConvertPixelDistanceToMapDistance = new ClsMapLayer();
                IEnvelope pEnv;
                IFeature pFeat=null;
                IActiveView pActiveView;
                ICursor pCursor=null;
                IFeatureCursor pFeatCursor;
                IGeometry pGeo;
                IFeatureLayer pFeatLyr;
                ICompositeLayer pGroupLyr;
                IFeatureSelection pFeatSel;
                pEnv = new EnvelopeClass();

                if (ClsDeclare.g_pMap.SelectionCount < 1)
                {
                    //ClsDeclare.g_ErrorHandler.DisplayInformation("图面上没有选择地物，无法放大", false, "确定",null);
                    return;
                }
                try
                {
                    for (int i = 0; i < ClsDeclare.g_pMap.LayerCount; i++)
                    {
                        if (ClsDeclare.g_pMap.get_Layer(i) is IGroupLayer && (ClsDeclare.g_pMap.get_Layer(i).Name != "示意图"))
                        {
                            pGroupLyr = ClsDeclare.g_pMap.get_Layer(i) as ICompositeLayer;
                            for (int j = 0; j < pGroupLyr.Count; j++)
                            {
                                if (pGroupLyr.get_Layer(j) is IGeoFeatureLayer)
                                {
                                    pFeatLyr = pGroupLyr.get_Layer(j) as IFeatureLayer;
                                    pFeatSel = (IFeatureSelection)pFeatLyr;
                                    if (pFeatSel.SelectionSet.Count == 0)
                                        continue;
                                    pFeatSel.SelectionSet.Search(null, false, out pCursor);
                                    pFeatCursor = (IFeatureCursor)pCursor;
                                    pFeat = pFeatCursor.NextFeature();
                                    while (pFeat != null)
                                    {
                                        pGeo = pFeat.Shape;
                                        pEnv.Union(pGeo.Envelope);
                                        pFeat = pFeatCursor.NextFeature();
                                    }
                                }
                            }
                        }
                        else if (ClsDeclare.g_pMap.get_Layer(i) is IGeoFeatureLayer)
                        {
                            pFeatLyr = ClsDeclare.g_pMap.get_Layer(i) as IFeatureLayer;
                            pFeatSel = (IFeatureSelection)pFeatLyr;
                            if (pFeatSel.SelectionSet.Count == 0)
                                continue;
                            pFeatSel.SelectionSet.Search(null, false, out pCursor);
                            pFeatCursor = (IFeatureCursor)pCursor;
                            pFeat = pFeatCursor.NextFeature();
                            while (pFeat != null)
                            {
                                pGeo = pFeat.Shape;
                                pEnv.Union(pGeo.Envelope);
                                pFeat = pFeatCursor.NextFeature();
                            }
                        }
                    }
                    pActiveView = (IActiveView)ClsDeclare.g_pMap;
                    double dblLen;
                    dblLen = pConvertPixelDistanceToMapDistance.ConvertPixelDistanceToMapDistance(pActiveView, 30);
                    pEnv.Expand(dblLen, dblLen, false);
                    pActiveView.Extent = pEnv;
                    pActiveView.Refresh();
                }
                catch (Exception)
                {
                    //ClsDeclare.g_ErrorHandler.DisplayInformation(ex.Message, false, "确定",null);
                    throw;
                }
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
                if (ClsDeclare.g_pMap.SelectionCount != 0)
                    return true;
                else
                    return false;
            }
        }

        #endregion
    }
}
