using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;

namespace ZJGISDataMatch.Classes
{
    public static class ClsCommon
    {
        /// <summary>
        /// 根据图层名称获取ILayer图层
        /// </summary>
        /// <param name="pMapControl"></param>
        /// <param name="LayerName"></param>
        /// <returns></returns>
        public static ILayer GetLayerByName(IMap pMapControl, string LayerName)
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

        public static IFeatureClass ReadFeatureClass(string path)
        {
            ESRI.ArcGIS.Geodatabase.IWorkspaceFactory workspaceFactory = new ESRI.ArcGIS.DataSourcesFile.ShapefileWorkspaceFactoryClass();
            // workspaceFactory.OpenFromFile,传入的参数是Shapefile文件所在的文件夹路径  
            string pathwithoutname = path.Substring(0, path.LastIndexOf("\\"));
            string namewithextention = path.Substring(path.LastIndexOf("\\") + 1);
            string name = namewithextention.Substring(0, namewithextention.LastIndexOf("."));
            ESRI.ArcGIS.Geodatabase.IFeatureWorkspace featureWorkspace = (ESRI.ArcGIS.Geodatabase.IFeatureWorkspace)workspaceFactory.OpenFromFile(pathwithoutname, 0);
            // OpenFeatureClass传入的参数是shape文件的文件名，不带后缀  
            ESRI.ArcGIS.Geodatabase.IFeatureClass featureClass = featureWorkspace.OpenFeatureClass(name);
            return featureClass;
        }
    }
}
