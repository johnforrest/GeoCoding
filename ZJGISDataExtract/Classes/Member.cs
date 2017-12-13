using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;

namespace ZJGISDataExtract
{
    /// <summary>
    ///2013.11.06  llj创建，用于工作空间操作
    /// </summary>
  public  class Member
    {
      /// <summary>
      /// 删除工作空间里的所有要素
      /// </summary>
      /// <param name="workspace">工作空间</param>
      public void DeleteAllFeatureClass(IWorkspace workspace)
      {
          IEnumDataset enumDataset = workspace.get_Datasets(esriDatasetType.esriDTAny);
          enumDataset.Reset();
          IDataset dataset = enumDataset.Next();
          while (dataset != null)
          {
              if (dataset.CanDelete())
              {
                  if (dataset.Type == esriDatasetType.esriDTFeatureDataset)
                  {
                      IFeatureDataset featureDataset = dataset as IFeatureDataset;
                      featureDataset.Delete();
                  }
                  else
                      dataset.Delete();
                  dataset = enumDataset.Next();
              }
          }
      }
      /// <summary>
      /// 根据路径获得工作空间
      /// </summary>
      /// <param name="strPath"></param>
      /// <returns></returns>
      //public abstract IWorkspace GetWorkspace(string strPath);

      /// <summary>
      /// 添加图层到地图上
      /// </summary>
      /// <param name="workspace"></param>
      /// <param name="map"></param>
      public void AddLayerToMap(IWorkspace workspace,ESRI.ArcGIS.Carto.IMap map)
      {
          //IEnumDataset enumDataset = workspace.get_Datasets(esriDatasetType.esriDTAny);
          //IFeatureWorkspace featureWorkspace = workspace as IFeatureWorkspace;
          //IFeatureClassContainer featureClassContainer2 = featureWorkspace as IFeatureClassContainer;
          //IFeatureClass fea = featureClassContainer2.get_Class(0);
          //enumDataset.Reset();
          //IDataset dataset = enumDataset.Next();
          //while (dataset != null)
          //{                
          //      IFeatureDataset featureDataset = dataset as IFeatureDataset;
          //      if (featureDataset != null)
          //      {
          //          IFeatureClassContainer featureClassContainer = (IFeatureClassContainer)featureDataset;
          //          IEnumFeatureClass enumFeatureClass = (IEnumFeatureClass)featureClassContainer.Classes;
          //          IFeatureClass featureClass = enumFeatureClass.Next();
          //          while (featureClass != null)
          //          {
          //              map.AddLayer((ESRI.ArcGIS.Carto.ILayer)featureClass);//加载到地图上
          //              featureClass = enumFeatureClass.Next();                                             
          //          }                
          //      }
          //  dataset = enumDataset.Next();
          //}
          //ESRI.ArcGIS.Carto.IActiveView activeView = map as ESRI.ArcGIS.Carto.IActiveView;
          //activeView.Refresh();
      }
    }
}
