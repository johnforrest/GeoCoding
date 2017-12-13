using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;

namespace ZJGISEntiTable.Classes
{
    public class ClsCommon
    {


        /// <summary>
        /// 根据图层名称获取ILayer图层
        /// </summary>
        /// <param name="pMapControl"></param>
        /// <param name="LayerName"></param>
        /// <returns></returns>
        public ILayer GetLayerByName(IMap pMapControl, string LayerName)
        {
            ILayer pLayer = null;
            try
            {
                for (int i = 0; i < pMapControl.LayerCount; i++)
                {
                    if (pMapControl.get_Layer(i) is IGroupLayer)
                    {
                        ICompositeLayer pCompositeLayer = pMapControl.get_Layer(i) as ICompositeLayer;
                        for (int j = 0; j < pCompositeLayer.Count; j++)
                        {
                            if (pCompositeLayer.get_Layer(j).Name == LayerName)
                            {
                                pLayer = pCompositeLayer.get_Layer(j);
                                break;
                            }
                        }
                    }
                    else
                    {
                        if (pMapControl.get_Layer(i) is IFeatureLayer && pMapControl.get_Layer(i).Name == LayerName)
                        {
                            pLayer = pMapControl.get_Layer(i);
                            break;
                        }
                    }
                }
                return pLayer;
            }
            catch
            {
                MessageBox.Show("当前图层为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }
        }

        /// <summary>  
        /// 获得要素图层  
        /// </summary>  
        /// <param name="pMap"></param>  
        /// <returns></returns>  
        public List<IFeatureLayer> GetFeatureLayers(IMap pMap)
        {
            IFeatureLayer pFeatLayer;
            ICompositeLayer pCompLayer;
            //IRasterLayer pRasterLayer;

            List<IFeatureLayer> pList = new List<IFeatureLayer>();
            //遍历地图  
            for (int i = 0; i < pMap.LayerCount; i++)
            {
                if (pMap.get_Layer(i) is IFeatureLayer)
                {
                    //获得图层要素  
                    pFeatLayer = pMap.get_Layer(i) as IFeatureLayer;
                    pList.Add(pFeatLayer);
                }
                else if (pMap.get_Layer(i) is IGroupLayer)
                {
                    //遍历图层组  
                    pCompLayer = pMap.get_Layer(i) as ICompositeLayer;
                    for (int j = 0; j < pCompLayer.Count; j++)
                    {
                        if (pCompLayer.get_Layer(j) is IFeatureLayer)
                        {
                            pFeatLayer = pCompLayer.get_Layer(j) as IFeatureLayer;
                            pList.Add(pFeatLayer);
                        }
                    }
                }
            }
            return pList;
        }  

    }
}
