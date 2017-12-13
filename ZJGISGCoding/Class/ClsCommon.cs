using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace ZJGISGCoding.Class
{
    public class ClsCommon
    {
        /// <summary>
        /// 检查格网字段是否存在，不存在就添加格网字段
        /// </summary>
        /// <param name="pFeatureLayer">待检查的图层</param>
        /// <param name="strField">格网字段的NameField</param>
        public void CheckGridCode(IFeatureLayer pFeatureLayer,string strField)
        {
            IClass pTable = pFeatureLayer.FeatureClass as IClass;
            try
            {
                if (pTable.Fields.FindField(strField) == -1)
                {
                    IField pField = new FieldClass();
                    IFieldEdit pFieldEdit = pField as IFieldEdit;
                    pFieldEdit.Name_2 = strField;
                    pFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
                    pTable.AddField(pField);
                }
            }
            catch
            {
                MessageBox.Show("添加字段有误,数据被占用！");
                return;
            }
        }
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
        /// 生成格网信息
        /// </summary>
        /// <param name="pFeature">Featureclass</param>
        /// <returns></returns>
        public string GetCodeString(IFeature pFeature)
        {
            double centroidX = 0;
            double centroidY = 0;
            IPoint pPoint = null;
            IPoint pPointEverrage = new PointClass();
            IPoint pPointNearest = new PointClass();

            IPoint pPointEverrage1 = new PointClass();
            IPoint pPointNearest1 = new PointClass();
            //求要素的质量中心
            switch (pFeature.Shape.GeometryType)
            {
                case esriGeometryType.esriGeometryPoint:
                    pPoint = pFeature.ShapeCopy as IPoint;
                    centroidX = pPoint.X;
                    centroidY = pPoint.Y;
                    break;
                case esriGeometryType.esriGeometryPolyline:
                    IPolyline pPolyline = pFeature.ShapeCopy as IPolyline;
                    IPointCollection pPointCollection = pPolyline as IPointCollection;

                    //20170607 先求出点集的中心点，然后再算出中心点到polyline的最近的点，之后用改点来替代整个polyline，保证了该点在改地理实体上
                    IProximityOperator proOperator = pPolyline as IProximityOperator;

                    double centerX = 0;
                    double centerY = 0;
                    for (int i = 0; i < pPointCollection.PointCount; i++)
                    {
                        pPoint = pPointCollection.get_Point(i);
                        centerX += pPoint.X;
                        centerY += pPoint.Y;
                    }
                    centroidX = centerX / pPointCollection.PointCount;
                    centroidY = centerY / pPointCollection.PointCount;

                    //20170607
                    pPointEverrage.X = centroidX;
                    pPointEverrage.Y = centroidY;
                    if (pPointEverrage != null)
                    {
                        pPointNearest = proOperator.ReturnNearestPoint(pPointEverrage, esriSegmentExtension.esriNoExtension);
                    }
                    centroidX = pPointNearest.X;
                    centroidY = pPointNearest.Y;
                    break;

                case esriGeometryType.esriGeometryPolygon:
                    //多边形获取的是中心节点的坐标
                    IPolygon pPolygon = pFeature.ShapeCopy as IPolygon;
                    IPointCollection pPolygonCollection = pPolygon as IPointCollection;

                    //20170607 先求出点集的中心点，然后再算出中心点到polyline的最近的点，之后用改点来替代整个polyline，保证了该点在改地理实体上
                    IProximityOperator pOperator = pPolygon as IProximityOperator;

                    double PolygoncenterX = 0;
                    double PolygoncenterY = 0;
                    for (int i = 0; i < pPolygonCollection.PointCount; i++)
                    {
                        pPoint = pPolygonCollection.get_Point(i);
                        PolygoncenterX += pPoint.X;
                        PolygoncenterY += pPoint.Y;
                    }
                    centroidX = PolygoncenterX / pPolygonCollection.PointCount;
                    centroidY = PolygoncenterY / pPolygonCollection.PointCount;

                    //20170607
                    pPointEverrage1.X = centroidX;
                    pPointEverrage1.Y = centroidY;
                    if (pPointEverrage != null)
                    {
                        pPointNearest1 = pOperator.ReturnNearestPoint(pPointEverrage1, esriSegmentExtension.esriNoExtension);
                    }
                    centroidX = pPointNearest1.X;
                    centroidY = pPointNearest1.Y;
                    break;
            }

            try
            {
                //加入第一级格网和第二级格网的信息
                string strCode = loadFirstGrid(centroidX, centroidY);

                return strCode;
            }
            catch (Exception ex)
            {
                ClsLog.WriteFile(ex, pFeature.OID.ToString());
                return "";
            }
        }

        /// <summary>
        /// 获取第一级格网
        /// </summary>
        /// <param name="x_centroid"></param>
        /// <param name="y_centroid"></param>
        /// <returns></returns>
        private string loadFirstGrid(double x_centroid, double y_centroid)
        {
            string FirstGridCode = string.Empty;
            if (x_centroid > 120 && y_centroid > 28)
            {
                FirstGridCode = "H51";
                FirstGridCode += loadSecondGrid(6, 4, x_centroid, y_centroid, 120, 28, 126, 32, 1);
            }
            else if (x_centroid < 120 && y_centroid > 28)
            {
                FirstGridCode = "H50";
                FirstGridCode += loadSecondGrid(6, 4, x_centroid, y_centroid, 114, 28, 120, 32, 2);
            }
            else if (x_centroid < 120 && y_centroid < 28)
            {
                FirstGridCode = "G50";
                FirstGridCode += loadSecondGrid(8, 5, x_centroid, y_centroid, 114, 24, 120, 28, 3);
            }
            else if (x_centroid > 120 && y_centroid < 28)
            {
                FirstGridCode = "G51";
                FirstGridCode += loadSecondGrid(8, 5, x_centroid, y_centroid, 120, 24, 126, 28, 4);
            }
            return FirstGridCode;

        }
        /// <summary>
        /// 获取第二级格网
        /// </summary>
        /// <param name="Xaverage"></param>
        /// <param name="Yaverage"></param>
        /// <param name="x_centroid"></param>
        /// <param name="y_centroid"></param>
        /// <param name="StartX"></param>
        /// <param name="StartY"></param>
        /// <param name="EndX"></param>
        /// <param name="EndY"></param>
        /// <param name="Quadrant"></param>
        /// <returns></returns>
        private string loadSecondGrid(int Xaverage, int Yaverage, double x_centroid, double y_centroid, double StartX, double StartY, double EndX, double EndY, int Quadrant)
        {
            //二级格网划分
            string SecondGridCode = string.Empty;
            double LengthX = (EndX - StartX) / Xaverage;//经度6或8等分
            double LengthY = (EndY - StartY) / Yaverage;//纬度4等分

            double OriginalX = StartX;
            double OriginalY = StartY;

            double DistanceX = x_centroid - OriginalX;
            double DistanceY = y_centroid - OriginalY;
            if (DistanceY < 0)
            {
                DistanceY = 0;
            }
            if (DistanceX < 0)
            {
                DistanceX = 0;
            }
            int intXCode = Convert.ToInt16(Math.Floor(DistanceX / LengthX));
            int intYCode = Convert.ToInt16(Math.Floor(DistanceY / LengthY));
            //字典
            string tempString = intXCode.ToString() + intYCode.ToString();
            string strCode = string.Empty;
            if (Quadrant == 1)
            {
                strCode = ClsDictionary.SecondGridD1[Convert.ToInt32(tempString)];
            }
            else if (Quadrant == 2)
            {
                strCode = ClsDictionary.SecondGridD2[Convert.ToInt32(tempString)];
            }
            else if (Quadrant == 3)
            {
                strCode = ClsDictionary.SecondGridD3[Convert.ToInt32(tempString)];
            }
            else if (Quadrant == 4)
            {
                strCode = ClsDictionary.SecondGridD4[Convert.ToInt32(tempString)];
            }
            SecondGridCode += strCode;

            //5级格网划分
            int GridLevel = 3;//只划分到第五级，也就是3
            Xaverage = 10;//10等分
            Yaverage = 10;//10等分

            string[] strW = new string[3];
            string[] strN = new string[3];
            //循环三次，循环一次，按10等分再分一次网格。
            for (int i = 1; i <= GridLevel; i++)
            {

                OriginalX += intXCode * LengthX;
                OriginalY += intYCode * LengthY;

                LengthX = LengthX / Xaverage;
                LengthY = LengthY / Yaverage;

                DistanceX = x_centroid - OriginalX;
                DistanceY = y_centroid - OriginalY;
                if (DistanceY < 0)
                {

                    DistanceY = 0;
                }
                if (DistanceX < 0)
                {
                    DistanceX = 0;
                }
                intXCode = Convert.ToInt16(Math.Floor(DistanceX / LengthX));
                intYCode = Convert.ToInt16(Math.Floor(DistanceY / LengthY));
                strW[i - 1] = intXCode.ToString();
                strN[i - 1] = intYCode.ToString();
                //SecondGridCode +=  intYCode.ToString() + intXCode.ToString() ;

            }
            //原来的排序规则
            //SecondGridCode += strN[0] + strN[1] + strN[2] + strW[0] + strW[1] + strW[2];

            SecondGridCode += strW[0] + strN[0] + strW[1] + strN[1] + strW[2] + strN[2];
            return SecondGridCode;
        }


    }
}
