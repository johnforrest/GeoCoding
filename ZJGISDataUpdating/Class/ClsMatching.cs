using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Carto;
using System.Collections.ObjectModel;
using ESRI.ArcGIS.esriSystem;
using System.Windows.Forms;


namespace ZJGISDataUpdating
{
    class ClsMatching
    {
        //目标图层中的一条Feature
        private IFeature m_CFeature;
        //源图层中的一条Feature
        private IFeature m_TUFeature;


        public IFeature CFeature
        {
            set
            {
                m_CFeature = value;
            }
        }
        public IFeature TUFeature
        {
            set
            {
                m_TUFeature = value;
            }
        }
        /// <summary>
        /// 位置相似度
        /// </summary>
        /// <returns></returns>
        public double CenterSimilarValue()
        {
            IArea pArea1 = (IArea)m_CFeature.Shape;
            IArea pArea2 = (IArea)m_TUFeature.Shape;

            IEnvelope pEnv1 = m_CFeature.Extent;
            IEnvelope pEnv2 = m_TUFeature.Extent;

            pEnv1.Envelope.Union(pEnv2);

            double dis1 = Math.Sqrt((pEnv1.XMax - pEnv1.XMin) * (pEnv1.XMax - pEnv1.XMin) + (pEnv1.YMax - pEnv1.YMin) * (pEnv1.YMax - pEnv1.YMin));

            IPoint centerPoint = new PointClass();
            centerPoint = pArea1.Centroid; //重心

            double x1, y1, x2, y2;
            x1 = centerPoint.X;
            y1 = centerPoint.Y;
            centerPoint = pArea2.Centroid;
            //相似度计算
            x2 = centerPoint.X;
            y2 = centerPoint.Y;

            double dis2 = Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));

            double similar = (dis1 - dis2) / dis1;
            return similar;
        }
        /// <summary>
        /// 长度相似度
        /// </summary>
        /// <returns></returns>
        public double LengthSimilarValue()
        {
            ICurve cCurve = m_CFeature.Shape as ICurve;
            ICurve tuCurve = m_TUFeature.Shape as ICurve;

            double cLength = cCurve.Length;
            double tuLength = tuCurve.Length;

            double insertLength = cLength - tuLength;
            if (insertLength < 0)
                insertLength = -insertLength;
            double similar = (cLength + tuLength - insertLength) / (cLength + tuLength);
            return similar;
        }
        /// <summary>
        /// 大小相似值
        /// </summary>
        /// <returns></returns>
        public double AreaSimilarValue()
        {

            IPolygon pPolygon1 = (IPolygon)m_CFeature.Shape;
            IPolygon pPolygon2 = (IPolygon)m_TUFeature.Shape;

            ITopologicalOperator ptopo = (ITopologicalOperator)pPolygon1;
            IGeometry pPolygon = ptopo.Intersect(pPolygon2, esriGeometryDimension.esriGeometry2Dimension);

            //计算两要素重叠区域。

            IArea pArea1 = (IArea)pPolygon1;
            IArea pArea2 = (IArea)pPolygon2;
            IArea pArea = (IArea)pPolygon;
            double A1 = pArea1.Area;
            double A2 = pArea2.Area;
            double A = pArea.Area;
            A = 2 * A / (A1 + A2);
            return A;
        }
        /// <summary>
        /// 形状相似值
        /// </summary>
        /// <returns></returns>
        public double ShapeSimilarValue()
        {
            try
            {
                IPointCollection pPointCol1 = (IPointCollection)m_CFeature.Shape;
                IPointCollection pPointCol2 = (IPointCollection)m_TUFeature.Shape;
                IPolygon pPolygon1 = (IPolygon)m_CFeature.Shape;
                IPolygon pPolygon2 = (IPolygon)m_TUFeature.Shape;

                IGeometryCollection cgeometryCol = m_CFeature.Shape as IGeometryCollection;
                IGeometryCollection tugeometryCol = m_TUFeature.Shape as IGeometryCollection;

                if (cgeometryCol.GeometryCount == 1 && tugeometryCol.GeometryCount == 1)
                {
                    //实现顺时针或者逆时针排序
                    pPointCol1 = SortVertexes(pPointCol1);
                    pPointCol2 = SortVertexes(pPointCol2);
                    //相似性计算
                    Dictionary<int, double> pLenth1 = new Dictionary<int, double>();
                    Dictionary<int, double> pAngle1 = new Dictionary<int, double>();
                    Dictionary<int, double> pLenth2 = new Dictionary<int, double>();
                    Dictionary<int, double> pAngle2 = new Dictionary<int, double>();
                    //计算每条边界线的长度和角度
                    angleStatics(pPointCol1, pPolygon1.Length, ref pLenth1, ref pAngle1);
                    angleStatics(pPointCol2, pPolygon2.Length, ref pLenth2, ref pAngle2);

                    int i = 0;
                    int j = 0;
                    Dictionary<double, double> LengthAngleDic1 = new Dictionary<double, double>();
                    Dictionary<double, double> LengthAngleDic2 = new Dictionary<double, double>();

                    for (i = 0; i < pLenth1.Count; i++)
                    {
                        LengthAngleDic1.Add(pLenth1[i], pAngle1[i]);
                    }

                    for (j = 0; j < pLenth2.Count; j++)
                    {
                        LengthAngleDic2.Add(pLenth2[j], pAngle2[j]);
                    }

                    Dictionary<int, double> tempDic = new Dictionary<int, double>();
                    tempDic = SortLengthsValue(pLenth1, pLenth2);
                    Collection<double> area = new Collection<double>();

                    for (i = 0; i < tempDic.Count - 1; i++)
                    {
                        double ta;
                        if (LengthAngleDic1.ContainsKey(tempDic[i]) && LengthAngleDic2.ContainsKey(tempDic[i]))
                        {
                            if (i == 0)
                            {
                                ta = (LengthAngleDic1[tempDic[i]] - LengthAngleDic2[tempDic[i]]) * tempDic[i];
                            }
                            else
                            {
                                ta = (LengthAngleDic1[tempDic[i]] - LengthAngleDic2[tempDic[i]]) * (tempDic[i] - tempDic[i - 1]);
                            }
                            if (ta < 0)
                                ta = -ta;
                            area.Add(ta);

                        }
                        else if (LengthAngleDic1.ContainsKey(tempDic[i]) && !LengthAngleDic2.ContainsKey(tempDic[i]))
                        {
                            int m = i + 1;
                            while (!LengthAngleDic2.ContainsKey(tempDic[m]))
                            {
                                m++;
                            }

                            if (i == 0)
                            {
                                ta = (LengthAngleDic1[tempDic[i]] - LengthAngleDic2[tempDic[m]]) * tempDic[i];
                            }
                            else
                            {
                                ta = (LengthAngleDic1[tempDic[i]] - LengthAngleDic2[tempDic[m]]) * (tempDic[i] - tempDic[i - 1]);
                            }
                            if (ta < 0)
                                ta = -ta;
                            area.Add(ta);

                        }
                        else if (!LengthAngleDic1.ContainsKey(tempDic[i]) && LengthAngleDic2.ContainsKey(tempDic[i]))
                        {
                            int n = i + 1;
                            while (!LengthAngleDic1.ContainsKey(tempDic[n]))
                            {
                                n++;
                            }
                            if (i == 0)
                            {
                                ta = (LengthAngleDic1[tempDic[n]] - LengthAngleDic2[tempDic[i]]) * tempDic[i];
                            }
                            else
                            {
                                ta = (LengthAngleDic1[tempDic[n]] - LengthAngleDic2[tempDic[i]]) * (tempDic[i] - tempDic[i - 1]);
                            }
                            if (ta < 0)
                                ta = -ta;

                            area.Add(ta);
                        }
                    }
                    double last;
                    if (LengthAngleDic1[pLenth1[pLenth1.Count - 1]] > LengthAngleDic2[pLenth2[pLenth2.Count - 1]])
                    {
                        last = (LengthAngleDic1[pLenth1[pLenth1.Count - 1]] - LengthAngleDic2[pLenth2[pLenth2.Count - 1]]) * (tempDic[tempDic.Count - 1] - tempDic[tempDic.Count - 2]);
                        area.Add(last);
                    }
                    else
                    {
                        last = (LengthAngleDic1[pLenth1[pLenth1.Count - 1]] - LengthAngleDic2[pLenth2[pLenth2.Count - 1]]) * (tempDic[tempDic.Count - 1] - tempDic[tempDic.Count - 2]);
                        last = -last;
                        area.Add(last);
                    }

                    double areaA = pLenth1[0] * pAngle1[0];
                    for (i = 1; i < pLenth1.Count; i++)
                    {
                        areaA = areaA + (pLenth1[i] - pLenth1[i - 1]) * pAngle1[i];
                    }
                    double areaB = pLenth2[0] * pAngle2[0];
                    for (j = 1; j < pLenth2.Count; j++)
                    {
                        areaB = areaB + (pLenth2[j] - pLenth2[j - 1]) * pAngle2[j];
                    }

                    double areaSum = 0;
                    for (i = 0; i < area.Count; i++)
                    {
                        areaSum = areaSum + area[i];
                    }

                    double areaU = (areaA + areaB - areaSum) / 2;
                    double shapeSimilar = areaU / (areaU + areaSum);
                    return shapeSimilar;
                }
                else
                {
                    ITopologicalOperator ptopo = (ITopologicalOperator)pPolygon1;
                    IGeometry pPolygon = ptopo.Intersect(pPolygon2, esriGeometryDimension.esriGeometry2Dimension);

                    //计算两要素重叠区域。

                    IArea pArea1 = (IArea)pPolygon1;
                    IArea pArea2 = (IArea)pPolygon2;
                    IArea pArea = (IArea)pPolygon;
                    double A1 = pArea1.Area;
                    double A2 = pArea2.Area;
                    double A = pArea.Area;
                    A = 2 * A / (A1 + A2);
                    return A;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        #region shapesimilar
        /// <summary>
        /// 坐标排序
        /// </summary>
        /// <param name="pointCol">输入的点坐标集</param>
        /// <returns></returns>
        private IPointCollection SortVertexes(IPointCollection pointCol)
        {
            Collection<IPoint> tempCol = new Collection<IPoint>();
            IPointCollection retPointCol = new MultipointClass();

            double tempX = 0;
            double tempY = 0;
            object missing = Type.Missing;
            int count = pointCol.PointCount - 1;
            int startLocation = -1;

            for (int k = 0; k < count; k++)
            {
                tempCol.Add(pointCol.get_Point(k));
            }
            for (int i = 0; i < count; i++)
            {
                for (int j = i; j < count; j++)
                {
                    if (tempCol[i].X > tempCol[j].X)
                    {
                        tempX = tempCol[j].X;
                        tempCol[j].X = tempCol[i].X;
                        tempCol[i].X = tempX;

                        tempY = tempCol[j].Y;
                        tempCol[j].Y = tempCol[i].Y;
                        tempCol[i].Y = tempY;
                    }
                    else if (tempCol[i].X == tempCol[j].X && tempCol[i].Y > tempCol[j].Y)
                    {
                        tempY = tempCol[j].Y;
                        tempCol[j].Y = tempCol[i].Y;
                        tempCol[i].Y = tempY;
                    }
                }
            }
            for (int m = 0; m < count; m++)
            {
                if (pointCol.get_Point(m).X == tempCol[count - 1].X && pointCol.get_Point(m).Y == tempCol[count - 1].Y)
                {
                    startLocation = m;
                    break;
                }
            }

            int te = startLocation + count;
            while (startLocation < te)
            {
                if (startLocation < count)
                {
                    retPointCol.AddPoint(pointCol.get_Point(startLocation), ref missing, ref missing);
                }
                else
                {
                    retPointCol.AddPoint(pointCol.get_Point(startLocation - count), ref missing, ref missing);
                }
                startLocation++;
            }
            return retPointCol;
        }

        /// <summary>
        /// 角度距离计算
        /// </summary>
        /// <param name="pointCol"></param>
        /// <param name="TotalLenth"></param>
        /// <param name="pLenthDic"></param>
        /// <param name="pAngleDic"></param>
        private void angleStatics(IPointCollection pointCol, double TotalLenth, ref Dictionary<int, double> pLenthDic, ref Dictionary<int, double> pAngleDic)
        {
            int Count = pointCol.PointCount;

            double tempAngle;
            double angle;

            double[] dangle = new double[Count];
            double l = 0;
            double t = 0;

            for (int i = 0; i <= Count - 1; i++)
            {
                ILine pLine = new LineClass();
                if (i == Count - 1)
                {
                    pLine.PutCoords(pointCol.get_Point(i), pointCol.get_Point(0));
                }
                else
                {
                    pLine.PutCoords(pointCol.get_Point(i), pointCol.get_Point(i + 1));
                }

                double Li = pLine.Length / TotalLenth;//计算每条线和总长度的比值
                l = Li + l;
                pLenthDic.Add(i, l);

                angle = (pLine.Angle * 180) / Math.PI;
                if (i == 0)
                {
                    dangle[i] = (pLine.Angle * 180) / Math.PI;
                    if (dangle[i] < 0)
                    {
                        dangle[i] = -dangle[i];
                    }
                }
                else
                {
                    ILine tempLine = new LineClass();
                    tempLine.PutCoords(pointCol.get_Point(i - 1), pointCol.get_Point(i));
                    tempAngle = (tempLine.Angle * 180) / Math.PI;

                    if (tempAngle > 0 && angle > 0)
                    {
                        dangle[i] = tempAngle - angle;
                    }
                    else if (tempAngle < 0 && angle < 0)
                    {
                        dangle[i] = tempAngle - angle;
                    }
                    else if (tempAngle > 0 && angle < 0)
                    {
                        angle = -angle;
                        double sum = angle + tempAngle;
                        if (sum < 180)
                        {
                            dangle[i] = sum;
                        }
                        else
                        {
                            dangle[i] = sum - 360;
                        }
                    }
                    else if (tempAngle < 0 && angle > 0)
                    {
                        tempAngle = -tempAngle;
                        double sum = angle + tempAngle;
                        if (sum < 180)
                        {
                            dangle[i] = -sum;
                        }
                        else
                        {
                            dangle[i] = 360 - sum;
                        }
                    }
                }
                t = t + dangle[i];
                pAngleDic.Add(i, t);
            }
        }
        /// <summary>
        /// 所有长度排序
        /// </summary>
        /// <param name="pLength1"></param>
        /// <param name="pLength2"></param>
        /// <returns></returns>
        private Dictionary<int, double> SortLengthsValue(Dictionary<int, double> pLength1, Dictionary<int, double> pLength2)
        {
            Dictionary<int, double> pDic = new Dictionary<int, double>();

            for (int i = 0; i < pLength1.Count; i++)
            {
                pDic.Add(i, pLength1[i]);
            }
            int count = pDic.Count;
            double temp;

            for (int i = 0; i < pLength2.Count; i++)
            {
                if (!pDic.ContainsValue(pLength2[i]))
                {
                    pDic.Add(count, pLength2[i]);
                    count++;
                }
            }

            for (int i = 0; i < pDic.Count; i++)
            {
                for (int j = i; j < pDic.Count; j++)
                {
                    if (pDic[i] > pDic[j])
                    {
                        temp = pDic[j];
                        pDic[j] = pDic[i];
                        pDic[i] = temp;
                    }
                }
            }
            return pDic;
        }




        /// <summary>
        /// 匹配点的相似值
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns>返回值</returns>
        public double MatchedPointsSimilarValue(double buffer)
        {
            try
            {
                //待匹配图层要素
                IPointCollection CPointCol = m_CFeature.Shape as IPointCollection;
                //源图层要素
                IPointCollection TUPointCol = m_TUFeature.Shape as IPointCollection;
                Dictionary<string, string> pointDic = new Dictionary<string, string>();
                Dictionary<string, string> inputPointDic = new Dictionary<string, string>();

                //IUnitConverter unitConverter = new UnitConverterClass();
                int i = 0, j = 0;
                int m = 0;
                int position = 0;
                int sameKey = 0;
                int outKey = 0;
                //遍历待匹配图层中所有的点
                for (i = 0; i < CPointCol.PointCount; i++)
                {
                    IPoint cPoint = new PointClass();
                    //目标图层中的点cPoint
                    cPoint = CPointCol.get_Point(i);

                    //IPoint unitCPoint = new PointClass();
                    //unitCPoint.X = unitConverter.ConvertUnits(cPoint.X, esriUnits.esriDecimalDegrees, esriUnits.esriMeters);
                    //unitCPoint.Y = unitConverter.ConvertUnits(cPoint.Y, esriUnits.esriDecimalDegrees, esriUnits.esriMeters);

                    ITopologicalOperator cTopOperator = cPoint as ITopologicalOperator;

                    IGeometry5 cGeo = cTopOperator.Buffer(buffer) as IGeometry5;
                    //遍历源图层中所有的点
                    for (j = position; j < TUPointCol.PointCount; j++)
                    {
                        IPoint tuPoint = new PointClass();
                        tuPoint = TUPointCol.get_Point(j);

                        //IPoint unitTUPoint = new PointClass();
                        //unitTUPoint.X = unitConverter.ConvertUnits(tuPoint.X, esriUnits.esriDecimalDegrees, esriUnits.esriMeters);
                        //unitTUPoint.Y = unitConverter.ConvertUnits(tuPoint.Y, esriUnits.esriDecimalDegrees, esriUnits.esriMeters);

                        IRelationalOperator relationOperator = cGeo as IRelationalOperator;
                        //IGeometry5 tuGeo = tuPoint as IGeometry5;
                        IGeometry tuGeo = tuPoint as IGeometry;

                        //test


                        if (j == 0)
                        {
                            if (tuGeo != null)
                            {
                                if (relationOperator.Contains(tuGeo))
                                {
                                    if (!inputPointDic.ContainsKey(tuPoint.X.ToString("#0.000")))
                                    {
                                        inputPointDic.Add(tuPoint.X.ToString("#0.000"), tuPoint.Y.ToString("#0.000"));
                                    }
                                    m++;
                                    position = m;
                                    break;
                                }
                                else
                                {
                                    if (pointDic.ContainsKey(tuPoint.X.ToString("#0.000")))
                                    {
                                        if (pointDic.ContainsValue(tuPoint.Y.ToString("#0.000")))
                                        {
                                            continue;
                                        }
                                        else
                                        {
                                            if (!inputPointDic.ContainsKey(tuPoint.X.ToString("#0.000")) && !pointDic.ContainsValue(tuPoint.Y.ToString("#0.000")))
                                            {
                                                sameKey++;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (!inputPointDic.ContainsKey(tuPoint.X.ToString("#0.000")) && !pointDic.ContainsValue(tuPoint.Y.ToString("#0.000")))
                                        {
                                            pointDic.Add(tuPoint.X.ToString("#0.000"), tuPoint.Y.ToString("#0.000"));
                                        }
                                    }
                                }
                            }

                        }
                        else if (j == TUPointCol.PointCount - 1)
                        {
                            if (relationOperator.Contains(tuGeo))
                            {
                                if (pointDic.ContainsKey(tuPoint.X.ToString("#0.000")))
                                {
                                    if (pointDic.ContainsValue(tuPoint.Y.ToString("#0.000")))
                                    {
                                        outKey--;
                                    }
                                }
                                if (!inputPointDic.ContainsKey(tuPoint.X.ToString("#0.000")))
                                {
                                    inputPointDic.Add(tuPoint.X.ToString("#0.000"), tuPoint.Y.ToString("#0.000"));
                                }
                                m++;
                                position = m;
                                break;
                            }
                            else
                            {
                                if (pointDic.ContainsKey(tuPoint.X.ToString("#0.000")))
                                {
                                    if (pointDic.ContainsValue(tuPoint.Y.ToString("#0.000")))
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        if (!inputPointDic.ContainsKey(tuPoint.X.ToString("#0.000")) && !pointDic.ContainsValue(tuPoint.Y.ToString("#0.000")))
                                        {
                                            sameKey++;
                                        }
                                    }
                                }
                                else
                                {
                                    if (!inputPointDic.ContainsKey(tuPoint.X.ToString("#0.000")) && !pointDic.ContainsValue(tuPoint.Y.ToString("#0.000")))
                                    {
                                        pointDic.Add(tuPoint.X.ToString("#0.000"), tuPoint.Y.ToString("#0.000"));
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (relationOperator.Contains(tuGeo))
                            {
                                if (pointDic.ContainsKey(tuPoint.X.ToString("#0.000")))
                                {
                                    if (pointDic.ContainsValue(tuPoint.Y.ToString("#0.000")))
                                    {
                                        outKey--;
                                    }
                                }
                                if (!inputPointDic.ContainsKey(tuPoint.X.ToString("#0.000")))
                                {
                                    inputPointDic.Add(tuPoint.X.ToString("#0.000"), tuPoint.Y.ToString("#0.000"));
                                }
                                m++;
                                position = m;
                                break;
                            }
                            else
                            {
                                ILine backLine = new LineClass();
                                ILine beforeLine = new LineClass();

                                backLine.PutCoords(TUPointCol.get_Point(j - 1), TUPointCol.get_Point(j));
                                beforeLine.PutCoords(TUPointCol.get_Point(j), TUPointCol.get_Point(j + 1));

                                double backAngle = backLine.Angle * 180 / Math.PI;
                                double beforeAngle = beforeLine.Angle * 180 / Math.PI;
                                double t = 0;

                                if (backAngle > 0 && beforeAngle > 0)
                                {
                                    t = beforeAngle - backAngle;
                                    if (t < 0)
                                        t = -t;
                                }
                                else if (backAngle < 0 && beforeAngle < 0)
                                {
                                    t = beforeAngle - backAngle;
                                    if (t < 0)
                                        t = -t;
                                }
                                else if (backAngle > 0 && beforeAngle < 0)
                                {
                                    beforeAngle = -beforeAngle;
                                    t = backAngle + beforeAngle;
                                    if (t > 180)
                                        t = 360 - 180;
                                }
                                else if (backAngle < 0 && beforeAngle > 0)
                                {
                                    backAngle = -backAngle;
                                    t = backAngle + beforeAngle;
                                    if (t > 180)
                                        t = 360 - t;
                                }

                                if (t < 10)
                                {
                                    continue;
                                }
                                else
                                {
                                    if (pointDic.ContainsKey(tuPoint.X.ToString("#0.000")))
                                    {
                                        if (pointDic.ContainsValue(tuPoint.Y.ToString("#0.000")))
                                        {
                                            continue;
                                        }
                                        else
                                        {
                                            if (!inputPointDic.ContainsKey(tuPoint.X.ToString("#0.000")) && !pointDic.ContainsValue(tuPoint.Y.ToString("#0.000")))
                                            {
                                                sameKey++;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (!inputPointDic.ContainsKey(tuPoint.X.ToString("#0.000")) && !pointDic.ContainsValue(tuPoint.Y.ToString("#0.000")))
                                        {
                                            pointDic.Add(tuPoint.X.ToString("#0.000"), tuPoint.Y.ToString("#0.000"));
                                        }
                                    }
                                }
                            }
                        }

                    }

                }
                int sum = CPointCol.PointCount + pointDic.Count + sameKey + outKey;
                double shapeSimiliarValue = Convert.ToDouble(m) / sum;
                return shapeSimiliarValue;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }
        /// <summary>
        /// 形状相似值
        /// </summary>
        /// <returns></returns>
        public double PolylineShapeSimilarValue()
        {
            try
            {
                IPointCollection pPointCol1 = (IPointCollection)m_CFeature.Shape;
                IPointCollection pPointCol2 = (IPointCollection)m_TUFeature.Shape;

                ICurve3 curve1 = m_CFeature.Shape as ICurve3;
                ICurve3 curve2 = m_TUFeature.Shape as ICurve3;

                IGeometryCollection cgeometryCol = m_CFeature.Shape as IGeometryCollection;
                IGeometryCollection tugeometryCol = m_TUFeature.Shape as IGeometryCollection;

                if (cgeometryCol.GeometryCount == 1 && tugeometryCol.GeometryCount == 1)
                {
                    Dictionary<int, double> lengthDic1 = new Dictionary<int, double>();
                    Dictionary<int, double> areaDic1 = new Dictionary<int, double>();
                    Dictionary<int, double> lengthDic2 = new Dictionary<int, double>();
                    Dictionary<int, double> areaDic2 = new Dictionary<int, double>();

                    //待匹配图层
                    PolylineAngleStatistics(pPointCol1, curve1.Length, ref lengthDic1, ref areaDic1);
                    //源图层
                    PolylineAngleStatistics(pPointCol2, curve2.Length, ref lengthDic2, ref areaDic2);

                    int i = 0;
                    int j = 0;
                    Dictionary<double, double> LengthAngleDic1 = new Dictionary<double, double>();
                    Dictionary<double, double> LengthAngleDic2 = new Dictionary<double, double>();

                    for (i = 0; i < lengthDic1.Count; i++)
                    {
                        if (!LengthAngleDic1.ContainsKey(lengthDic1[i]))
                        {
                            LengthAngleDic1.Add(lengthDic1[i], areaDic1[i]);
                        }
                    }
                    for (j = 0; j < lengthDic2.Count; j++)
                    {
                        if (!LengthAngleDic2.ContainsKey(lengthDic2[j]))
                        {
                        	LengthAngleDic2.Add(lengthDic2[j], areaDic2[j]);
                        }
                    }

                    Dictionary<int, double> tempDic = new Dictionary<int, double>();
                    tempDic = SortLengthsValue(lengthDic1, lengthDic2);
                    Collection<double> area = new Collection<double>();

                    for (i = 0; i < tempDic.Count - 1; i++)
                    {
                        double ta;
                        if (LengthAngleDic1.ContainsKey(tempDic[i]) && LengthAngleDic2.ContainsKey(tempDic[i]))
                        {
                            if (i == 0)
                            {
                                ta = (LengthAngleDic1[tempDic[i]] - LengthAngleDic2[tempDic[i]]) * tempDic[i];
                            }
                            else
                            {
                                ta = (LengthAngleDic1[tempDic[i]] - LengthAngleDic2[tempDic[i]]) * (tempDic[i] - tempDic[i - 1]);
                            }
                            if (ta < 0)
                                ta = -ta;
                            area.Add(ta);

                        }
                        else if (LengthAngleDic1.ContainsKey(tempDic[i]) && !LengthAngleDic2.ContainsKey(tempDic[i]))
                        {
                            int m = i + 1;
                            while (!LengthAngleDic2.ContainsKey(tempDic[m]))
                            {
                                m++;
                            }

                            if (i == 0)
                            {
                                ta = (LengthAngleDic1[tempDic[i]] - LengthAngleDic2[tempDic[m]]) * tempDic[i];
                            }
                            else
                            {
                                ta = (LengthAngleDic1[tempDic[i]] - LengthAngleDic2[tempDic[m]]) * (tempDic[i] - tempDic[i - 1]);
                            }
                            if (ta < 0)
                                ta = -ta;
                            area.Add(ta);

                        }
                        else if (!LengthAngleDic1.ContainsKey(tempDic[i]) && LengthAngleDic2.ContainsKey(tempDic[i]))
                        {
                            int n = i + 1;
                            while (!LengthAngleDic1.ContainsKey(tempDic[n]))
                            {
                                n++;
                            }
                            if (i == 0)
                            {
                                ta = (LengthAngleDic1[tempDic[n]] - LengthAngleDic2[tempDic[i]]) * tempDic[i];
                            }
                            else
                            {
                                ta = (LengthAngleDic1[tempDic[n]] - LengthAngleDic2[tempDic[i]]) * (tempDic[i] - tempDic[i - 1]);
                            }
                            if (ta < 0)
                                ta = -ta;
                            area.Add(ta);
                        }
                    }
                    double shapeSimilar;
                    double areaA = lengthDic1[0] * areaDic1[0];
                    double areaB = lengthDic2[0] * areaDic2[0];
                    double last;
                    if (tempDic.Count > 1)
                    {
                        if (LengthAngleDic1[lengthDic1[lengthDic1.Count - 1]] > LengthAngleDic2[lengthDic2[lengthDic2.Count - 1]])
                        {
                            last = (LengthAngleDic1[lengthDic1[lengthDic1.Count - 1]] - LengthAngleDic2[lengthDic2[lengthDic2.Count - 1]]) * (tempDic[tempDic.Count - 1] - tempDic[tempDic.Count - 2]);
                            area.Add(last);
                        }
                        else
                        {
                            last = (LengthAngleDic1[lengthDic1[lengthDic1.Count - 1]] - LengthAngleDic2[lengthDic2[lengthDic2.Count - 1]]) * (tempDic[tempDic.Count - 1] - tempDic[tempDic.Count - 2]);
                            last = -last;
                            area.Add(last);
                        }

                        for (i = 1; i < lengthDic1.Count; i++)
                        {
                            areaA = areaA + (lengthDic1[i] - lengthDic1[i - 1]) * areaDic1[i];
                        }

                        for (j = 1; j < lengthDic2.Count; j++)
                        {
                            areaB = areaB + (lengthDic2[j] - lengthDic2[j - 1]) * areaDic2[j];
                        }

                        double areaSum = 0;
                        for (i = 0; i < area.Count; i++)
                        {
                            areaSum = areaSum + area[i];
                        }

                        double areaU = (areaA + areaB - areaSum) / 2;
                        shapeSimilar = areaU / (areaU + areaSum);
                        return shapeSimilar;
                    }
                    else
                    {
                        if (areaA > areaB)
                        {
                            shapeSimilar = areaB / areaA;
                        }
                        else
                        {
                            shapeSimilar = areaA / areaB;
                        }
                        return shapeSimilar;
                    }
                }
                else
                {
                    if (cgeometryCol.GeometryCount > 1 && tugeometryCol.GeometryCount == 1)
                    {
                        ITopologicalOperator top = m_CFeature.Shape as ITopologicalOperator;
                        IRelationalOperator relationalOperator = top.Buffer(0.0002) as IRelationalOperator;
                        if (relationalOperator.Contains(m_TUFeature.Shape))
                            return 1;
                        else
                            return 0;
                    }
                    else
                    {
                        ITopologicalOperator top = m_TUFeature.Shape as ITopologicalOperator;
                        IRelationalOperator relationalOperator = top.Buffer(0.0002) as IRelationalOperator;
                        if (relationalOperator.Contains(m_CFeature.Shape))
                            return 1;
                        else
                            return 0;
                    }

                }
            }
            catch (Exception ex)
            {
                //20170916
                MessageBox.Show(ex.Message);
                return 0.0;
                //throw;
            }

        }
        /// <summary>
        /// 折线角度统计
        /// </summary>
        /// <param name="pointCol">线状目标转换的点集合</param>
        /// <param name="totalLength">线状目标转化的曲线长度</param>
        /// <param name="lengthDic">线的长度字典</param>
        /// <param name="areaDic">线的角度字典</param>
        private void PolylineAngleStatistics(IPointCollection pointCol, double totalLength, ref Dictionary<int, double> lengthDic, ref Dictionary<int, double> areaDic)
        {
            int count = pointCol.PointCount;

            double beforfAngle = 0;
            double backAngle = 0;
            double[] length = new double[count - 1];
            double[] angle = new double[count - 1];

            double l = 0;
            double a = 0;

            if (count == 2)
            {
                ILine line = new LineClass();
                line.PutCoords(pointCol.get_Point(0), pointCol.get_Point(1));
                lengthDic.Add(0, 1.0);
                //弧度转角度
                backAngle = line.Angle * 180 / Math.PI;
                if (backAngle < 0)
                    backAngle = -backAngle;
                areaDic.Add(0, backAngle);

            }
            else
            {
                for (int i = 0; i < count - 1; i++)
                {
                    ILine line = new LineClass();
                    line.PutCoords(pointCol.get_Point(i), pointCol.get_Point(i + 1));

                    length[i] = line.Length / totalLength;
                    l = l + length[i];
                    lengthDic.Add(i, l);

                    beforfAngle = line.Angle * 180 / Math.PI;
                    if (i == 0)
                    {
                        if (beforfAngle < 0)
                            beforfAngle = -beforfAngle;
                        angle[i] = beforfAngle;
                    }
                    else
                    {
                        ILine aline = new LineClass();
                        aline.PutCoords(pointCol.get_Point(i - 1), pointCol.get_Point(i));
                        backAngle = aline.Angle * 180 / Math.PI;

                        if (backAngle > 0 && beforfAngle > 0)
                        {
                            angle[i] = beforfAngle - backAngle;
                            if (angle[i] < 0)
                                angle[i] = -angle[i];
                        }
                        else if (backAngle < 0 && beforfAngle < 0)
                        {
                            angle[i] = beforfAngle - backAngle;
                            if (angle[i] < 0)
                                angle[i] = -angle[i];
                        }
                        else if (backAngle > 0 && beforfAngle < 0)
                        {
                            beforfAngle = -beforfAngle;
                            angle[i] = backAngle + beforfAngle;
                            if (angle[i] > 180)
                                angle[i] = 360 - angle[i];
                        }
                        else if (backAngle < 0 && beforfAngle > 0)
                        {
                            backAngle = -backAngle;
                            angle[i] = backAngle + beforfAngle;
                            if (angle[i] > 180)
                                angle[i] = 360 - angle[i];
                        }
                    }

                    a = a + angle[i];
                    areaDic.Add(i, a);
                }
            }
        }
        #endregion

        /// <summary>
        /// 返回点与点之间的直线距离（单位待定？）
        /// </summary>
        /// <returns></returns>
        public double PointDistance()
        {

            IPoint Point1 = (IPoint)m_CFeature.Shape;
            IPoint Point2 = (IPoint)m_TUFeature.Shape;
            double x1, y1, x2, y2;
            x1 = Point1.X;
            y1 = Point1.Y;

            //相似度计算
            x2 = Point2.X;
            y2 = Point2.Y;

            double distance = Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
            return distance;
        }
    }
}
