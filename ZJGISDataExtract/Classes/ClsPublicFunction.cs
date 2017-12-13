using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using Microsoft.VisualBasic;

namespace ZJGISDataExtract
{
    class ClsPublicFunction
    {
        public static void ControlNumKey(ref short keyascii, ref string sValue)
        {
            sValue = "";
            if (!string.IsNullOrEmpty(sValue))
            {
                //小数点键".",检查小数点是否已经存在
                if (keyascii == 46)
                {
                    //if (Strings.InStr(1, sValue, ".", CompareMethod.Text) > 0)
                    if(sValue.IndexOf(".")>0)
                    {
                        keyascii = 0;
                    }
                    return;
                }
            }
            //数字键(48~57),8中退格键
            if ((keyascii > 57 | keyascii < 48) & keyascii != 8)
            {
                keyascii = 0;
                return;
            }
        }

        //功能描述: 根据多边形生成坐标串并写到多边形列表中
        //坐标窜的格式:(x,y),(x,y)
        public static string GeneratePntCol(IPolygon pPolygon)
        {
            int l = 0;
            IPointCollection pPntCol = default(IPointCollection);
            string sCoordinate = null;
            IPoint pPnt = default(IPoint);
            sCoordinate = "";
            if ((pPolygon != null))
            {
                if (pPolygon.IsEmpty == false)
                {
                    pPntCol = (IPointCollection)pPolygon;
                    for (l = 0; l <= pPntCol.PointCount - 1; l++)
                    {
                        pPnt = pPntCol.get_Point(l);
                        if (string.IsNullOrEmpty(sCoordinate))
                        {
                            sCoordinate = "(" + pPnt.X + "," + pPnt.Y + ")";
                        }
                        else
                        {
                            sCoordinate = sCoordinate + "," + "(" + pPnt.X + "," + pPnt.Y + ")";
                        }
                    }
                }
            }
            return sCoordinate;
        }

        ////印骅 20081125 添加
        //功能描述: 从PRJ文件获取投影坐标系统
        //参数列表: prj (string)
        public static ISpatialReference LoadPRJ(string sPrj)
        {
            ISpatialReferenceFactory2 pSpatRefFact = default(ISpatialReferenceFactory2);
            //pSpatRefFact = new SpatialReferenceEnvironment();
            pSpatRefFact = new SpatialReferenceEnvironmentClass();
            ISpatialReference pSpatRef = default(ISpatialReference);
            pSpatRef = pSpatRefFact.CreateESRISpatialReferenceFromPRJFile(sPrj);
            return pSpatRef;
        }

        //3/10/2009	删除工作空间内的所有要素类 yh
        public static void DeleteAllFeatCls(IWorkspace pworkspace)
        {
            IEnumDataset pEnumFeatDataset = default(IEnumDataset);
            IDataset pDataset = default(IDataset);
            IFeatureDataset pFeatDataset = default(IFeatureDataset);

            pEnumFeatDataset = pworkspace.get_Datasets(esriDatasetType.esriDTAny);
            pEnumFeatDataset.Reset();
            pDataset = pEnumFeatDataset.Next();
            while ((pDataset != null))
            {
                if (pDataset.CanDelete())
                {
                    if (pDataset.Type == esriDatasetType.esriDTFeatureDataset)
                    {
                        pFeatDataset = (IFeatureDataset)pDataset;
                        pFeatDataset.Delete();
                    }
                    else
                    {
                        pDataset.Delete();
                    }
                    //pDataset.Delete()
                    pDataset = pEnumFeatDataset.Next();
                }
            }
        }

    }
}
