using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;

namespace ZJGISDataMatch.Classes
{
    class ClsCoding
    {
        public void PointCoding(IFeatureLayer pMatchLayer, Dictionary<int, int> pdicOid, Dictionary<int, string> pdiOri, string pEntiid)
        {
            //遍历Feature
            IDataset pDatasetMat = pMatchLayer.FeatureClass as IDataset;
            IWorkspaceEdit pWorkspaceEditMat = null;
            if (pDatasetMat != null)
            {
                pWorkspaceEditMat = pDatasetMat.Workspace as IWorkspaceEdit;
                if (pWorkspaceEditMat != null || pWorkspaceEditMat.IsBeingEdited() == false)
                {
                    pWorkspaceEditMat.StartEditing(true);
                    pWorkspaceEditMat.StartEditOperation();
                }
                IFeatureCursor pFeatureCursor = pMatchLayer.FeatureClass.Search(new QueryFilterClass(), false);
                //test
                int test = pMatchLayer.FeatureClass.FeatureCount(null);
                IFeature pFeature = pFeatureCursor.NextFeature();
                while (pFeature != null)
                {
                    foreach (int key in pdicOid.Keys)
                    {
                        if (pFeature.OID == key)
                        {
                            int pIndex = pFeature.Fields.FindField(pEntiid);
                            //test
                            string test1 = pdiOri[pdicOid[key]];
                            pFeature.set_Value(pIndex, pdiOri[pdicOid[key]]);
                            pFeature.Store();
                        }

                    }
                    pFeature = pFeatureCursor.NextFeature();
                }
                pWorkspaceEditMat.StopEditing(true);
                pWorkspaceEditMat.StopEditOperation();

            }
        }

        public void PointCodingRest(IFeatureLayer pMatchLayer, Dictionary<int, string> pdiOri, string pEntiid)
        {
            int i = 0;
            string pPoiOneTwo = "1330521000000";

            List<string> plist = new List<string>();
            //遍历Feature
            IDataset pDatasetMat = pMatchLayer.FeatureClass as IDataset;
            IWorkspaceEdit pWorkspaceEditMat = null;
            if (pDatasetMat != null)
            {
                pWorkspaceEditMat = pDatasetMat.Workspace as IWorkspaceEdit;
                if (pWorkspaceEditMat != null || pWorkspaceEditMat.IsBeingEdited() == false)
                {
                    pWorkspaceEditMat.StartEditing(true);
                    pWorkspaceEditMat.StartEditOperation();
                }
                IFeatureCursor pFeatureCursor = pMatchLayer.FeatureClass.Search(new QueryFilterClass(), false);
                //test
                int test = pMatchLayer.FeatureClass.FeatureCount(null);
                IFeature pFeature = pFeatureCursor.NextFeature();
                while (pFeature != null)
                {
                    int pIndex = pFeature.Fields.FindField(pEntiid);
                    //test
                    string test1 = pFeature.get_Value(pIndex).ToString();
                    object testoid = pFeature.OID;
                    if (pFeature.get_Value(pIndex).ToString().Trim().Length == 0)
                    {
                        //z这两个变量只能放到if语句里面。
                        string pResult = null;
                        bool isContainValue = false;


                        //获取单条Feature的某个字段值
                        while (i < 10000000)
                        {
                            i++;
                            break;
                        }
                        for (int k = 0; k < 7 - i.ToString().Length; k++)
                        {
                            pResult += "0";
                        }
                        //string pEntiid = pPoiOneTwo + ReturnNumberCharacter(i);
                       
                        string pentiid = pPoiOneTwo + pResult + i.ToString();

                        foreach (string sValue in pdiOri.Values)
                        {
                            if (pentiid != sValue)
                            {
                                continue;
                            }
                            else
                            {
                                pFeature.set_Value(pIndex, sValue);
                                pFeature.Store();
                                isContainValue = true;
                                break;
                            }
                        }

                        if (!isContainValue)
                        {
                            pFeature.set_Value(pIndex, pentiid);
                            pFeature.Store();
                        }
                    }
                    else
                    {
                        pFeature.Store();
                    }

                    pFeature = pFeatureCursor.NextFeature();
                }
                pWorkspaceEditMat.StopEditing(true);
                pWorkspaceEditMat.StopEditOperation();
            }
        }




    }
}
