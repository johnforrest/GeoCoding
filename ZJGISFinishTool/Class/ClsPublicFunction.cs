using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using Microsoft.VisualBasic;

namespace ZJGISFinishTool
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
                    if (Strings.InStr(1, sValue, ".", CompareMethod.Text) > 0)
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

        //功能描述:根据坐标串生成一个polygon
        //参数列表: sCoordinate (String)
        //          pSpatialReference (ISpatialReference)
        public static IPolygon GetPolygonByCoordinate(string sCoordinate, ISpatialReference pSpatialReference)
        {
            string sRealCoordinate = null;
            int i = 0;
            string sValue = null;
            IPolygon pPolygon = default(IPolygon);
            IPointCollection pPntCol = default(IPointCollection);
            IPoint pPnt = default(IPoint);
            string x = null;
            string y = null;
            string[] sArray = null;
            string[] sCoordArray = null;
            ITopologicalOperator2 pTopoOperator = default(ITopologicalOperator2);

            if (pSpatialReference == null)
                return null;
            pPolygon = null;
            try
            {
                if (!string.IsNullOrEmpty(Strings.Trim(sCoordinate)))
                {
                    if (Strings.InStr(sCoordinate, "(", CompareMethod.Text) > 0 & Strings.InStr(sCoordinate, ")", CompareMethod.Text) > 0 & Strings.InStr(sCoordinate, ",", CompareMethod.Text) > 0)
                    {
                        //去掉坐标窜中的空格
                        sRealCoordinate = "";
                        for (i = 1; i <= Strings.Len(sCoordinate); i++)
                        {
                            sValue = Strings.Mid(sCoordinate, i, 1);
                            if (!string.IsNullOrEmpty(Strings.Trim(sValue)))
                            {
                                if (string.IsNullOrEmpty(Strings.Trim(sRealCoordinate)))
                                {
                                    sRealCoordinate = sValue;
                                }
                                else
                                {
                                    sRealCoordinate = sRealCoordinate + sValue;
                                }
                            }
                        }
                        pPntCol = new Polygon();
                        //说明有多个坐标对
                        if (Strings.InStr(sRealCoordinate, "),", CompareMethod.Text) > 0)
                        {
                            //sArray = Strings.Split(sRealCoordinate, "),");
                            sArray = sRealCoordinate.Split(new char[] { ')' });
                            if (Information.UBound(sArray, 1) > 1)
                            {
                                for (i = 0; i <= Information.UBound(sArray, 1); i++)
                                {
                                    sValue = sArray[i];
                                    //分解后的坐标解析式:(x,y,   最后一项(x,y)
                                    if (Strings.InStr(sValue, "(", CompareMethod.Text) > 0 && Strings.InStr(sValue, ",", CompareMethod.Text) > 0)
                                    {
                                        //sCoordArray = Strings.Split(sValue, ",");
                                        sCoordArray = sValue.Split(new char[] { ',' });
                                        if (Information.UBound(sCoordArray, 1) == 1)
                                        {
                                            pPnt = new Point();
                                            pPnt.Project(pSpatialReference);
                                            x = sCoordArray[0];
                                            x = Convert.ToString(Convert.ToDouble(Strings.Right(x, Strings.Len(x) - 1)));
                                            y = sCoordArray[1];
                                            if (i == Information.UBound(sArray, 1))
                                            {
                                                y = Strings.Left(y, Strings.Len(y) - 1);
                                            }
                                            pPnt.PutCoords(Convert.ToDouble(x), Convert.ToDouble(y));
                                            object missing = Type.Missing;
                                            pPntCol.AddPoint(pPnt, ref missing, ref missing);
                                            //只要有一个点的坐标形式出错,就退出
                                        }
                                        else
                                        {
                                            return null;
                                        }
                                    }
                                    else
                                    {
                                        return null;
                                    }
                                }
                                pTopoOperator = (ITopologicalOperator2)pPntCol;
                                pTopoOperator.Simplify();
                                pPolygon = (IPolygon)pTopoOperator;
                                pPolygon.Project(pSpatialReference);
                                return pPolygon;

                            }
                        }
                    }
                }
                return pPolygon;
            }
            catch
            {
                return null;
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

        //3/10/2009	删除工作空间内的所有要素类 
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
