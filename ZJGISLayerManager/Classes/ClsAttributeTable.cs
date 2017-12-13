//<CSCC>
//-------------------------------------------------------------------------------------
//    部件名：ClsAttributeTable
//    工程名：LayerManager
//    版权: CopyRight (c) 2010
//    创建人：
//    描述：查看属性表
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
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.Geodatabase;
using ZJGISLayerManager;
using System.Resources;
using System.Windows.Forms;

namespace ZJGISLayerManager
{
   
    public sealed class ClsAttributeTable : BaseCommand
    {
      
        private IHookHelper m_hookHelper = null;
        private IGlobeHookHelper m_globeHookHelper = null;
        private ISceneHookHelper m_sceneHookHelper = null;

        private IMapControl3 m_pMapControl;
        private ILayer m_pLayer;
        private FrmAttributeTB m_FrmAttributeTB = new FrmAttributeTB();
        private Form m_FrmMap;

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

        public Form FrmMap
        {
            set
            {
                m_FrmMap = value;
            }
        }

        public ClsAttributeTable()
        {
            //
            
            //
            base.m_category = "LayerManager"; //localizable text
            base.m_caption = "查看属性表";  //localizable text
            base.m_message = "查看图层要素的属性";   //localizable text 
            base.m_toolTip = "查看属性表";  //localizable text 
            base.m_name = "AttributeTable";   //unique id, non-localizable (e.g. "MyCategory_MyCommand")

            try
            {
                //
                
                ////
                ResourceManager resource = new ResourceManager("ZJGISLayerManager.Properties.Resources", System.Reflection.Assembly.GetExecutingAssembly());
                base.m_bitmap = (Bitmap)resource.GetObject("AttributeTable");
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

            
            m_pMapControl = (IMapControl3)m_hookHelper.Hook;
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

            if (m_FrmAttributeTB.IsDisposed)
            {
                m_FrmAttributeTB = new FrmAttributeTB();
            }
            m_FrmAttributeTB.SetMap = m_pMapControl.Map;
            m_FrmAttributeTB.SelLayer = m_pLayer as ILayer;                             //将TOC中选择的图层传递到窗体中
            m_FrmAttributeTB.Show();                                          //显示图层属性表窗体

            m_FrmAttributeTB.AddDataToTable(m_pLayer);                        //这样调用便于显示进度条

        }
        #endregion

        public override bool Enabled
        {
            get
            {
                //判断图层类型
                if (m_pLayer is IFeatureLayer)
                {
                    return true;
                }
                else if (m_pLayer is IRasterLayer)
                {
                    IRasterLayer pRasterLayer;
                    pRasterLayer = (IRasterLayer)m_pLayer;
                    if (pRasterLayer.BandCount > 1)
                    {
                        return false;
                    }
                    //获取第一个波段
                    IRaster pRaster = pRasterLayer.Raster;
                    IRasterBandCollection pBandCol;
                    pBandCol = (IRasterBandCollection)pRaster;
                    IRasterBand pBand = pBandCol.Item(0);
                    //判断有无属性表
                    bool bHasTable;
                    pBand.HasTable(out bHasTable);
                    //没有属性表
                    if (bHasTable == false)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }

        }

        public bool Enabled2
        {
            set
            {
                base.m_enabled = value;
            }
        }
    }
}
