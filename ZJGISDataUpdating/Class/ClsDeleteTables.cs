/*-------------------------------------------------------------------------
 * 版权所有：武汉大学
 * 创建人：johnforrest
 * 联系方式：571716604@qq.com
 * 创建时间： 12/13/2017 9:41:34 AM
 * 类名称：ClsDeleteTables
 * 本类主要用途描述：用于删除特定数据集下的表
 * 修改人：
 * 修改时间：
 * 修改备注：
 * @version 1.0
 *  -------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Geodatabase;

namespace ZJGISDataUpdating.Class
{
    public static class ClsDeleteTables
    {
        /// <summary>
        /// 删除特定数据集下的表
        /// </summary>
        /// <param name="gdbPath">gdb路径</param>
        /// <param name="tableName">表名称</param>
        public static void DeleteFeatureClass(string gdbPath,string tableName)
        {
            IWorkspaceFactory worFact = new FileGDBWorkspaceFactory();
            IWorkspace workspace = worFact.OpenFromFile(gdbPath, 0);
            IFeatureWorkspace featureWorkspace = workspace as IFeatureWorkspace;
            IFeatureWorkspaceManage featureWorkspaceMange = (IFeatureWorkspaceManage)featureWorkspace;
            //得到的数据集是FeatureClass
            //IEnumDatasetName enumDatasetName = workspace.get_DatasetNames(esriDatasetType.esriDTFeatureClass);
            IEnumDatasetName enumDatasetName = workspace.get_DatasetNames(esriDatasetType.esriDTTable);
            IDatasetName datasetName = null;
            while ((datasetName = enumDatasetName.Next()) != null)
            {
                if (datasetName.Name.Equals(tableName))
                {
                    //删除指定要素类(表)
                    featureWorkspaceMange.DeleteByName(datasetName);
                    break;
                }
            }
        }

    }
}
