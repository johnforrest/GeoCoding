﻿using System;
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
    class ClsIndicatorFun
    {
        //目标图层中的一条Feature
        private IFeature targetFeature;
        //源图层中的一条Feature
        private IFeature sourceFeature;


        public IFeature TargetFeature
        {
            set
            {
                targetFeature = value;
            }
        }
        public IFeature SourceFeature
        {
            set
            {
                sourceFeature = value;
            }
        }
        /// <summary>
        /// 重心相似度（1-重心距离与目标要素对角线长度比值）
        /// </summary>
        /// <returns></returns>
        public double CenterSimilarValue()
        {
            IArea pTargetArea1 = (IArea)targetFeature.Shape;
            IArea pSourceArea2 = (IArea)sourceFeature.Shape;

            IEnvelope pTargetEnv1 = targetFeature.Extent;
            IEnvelope pSourceEnv2 = sourceFeature.Extent;

            pTargetEnv1.Envelope.Union(pSourceEnv2);
            //目标要素对角线距离
            double dis1 = Math.Sqrt((pTargetEnv1.XMax - pTargetEnv1.XMin) * (pTargetEnv1.XMax - pTargetEnv1.XMin) + 
                (pTargetEnv1.YMax - pTargetEnv1.YMin) * (pTargetEnv1.YMax - pTargetEnv1.YMin));

            IPoint targetFeatureCenterPoint = new PointClass();
            //重心
            targetFeatureCenterPoint = pTargetArea1.Centroid;

            double x1, y1, x2, y2;
            x1 = targetFeatureCenterPoint.X;
            y1 = targetFeatureCenterPoint.Y;

            IPoint sourceFeatureCenterPoint = new PointClass();
            sourceFeatureCenterPoint = pSourceArea2.Centroid;
            //相似度计算
            x2 = sourceFeatureCenterPoint.X;
            y2 = sourceFeatureCenterPoint.Y;
            //重心距离
            double dis2 = Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));

            double similar = (dis1 - dis2) / dis1;
            return similar;
        }
        /// <summary>
        /// 长度比相似度（Shape比值）
        /// </summary>
        /// <returns></returns>
        public double LengthSimilarValue()
        {
            ICurve targetCurve = targetFeature.Shape as ICurve;
            ICurve sourceCurve = sourceFeature.Shape as ICurve;

            double targetLength = targetCurve.Length;
            double sourceLength = sourceCurve.Length;
            //距离差值
            double insertLength = targetLength - sourceLength;

            if (insertLength < 0)
                insertLength = -insertLength;

            double similar = (targetLength + sourceLength - insertLength) / (targetLength + sourceLength);
            return similar;
        }
        /// <summary>
        /// 面积相似值（相交面积与二者面积和比值的二倍）
        /// </summary>
        /// <returns></returns>
        public double AreaSimilarValue()
        {
            IPolygon targetPolygon = (IPolygon)targetFeature.Shape;
            IPolygon sourcePolygon = (IPolygon)sourceFeature.Shape;

            ITopologicalOperator pTargettopo = (ITopologicalOperator)targetPolygon;
            //相交面积
            IGeometry intersectArea = pTargettopo.Intersect(sourcePolygon, esriGeometryDimension.esriGeometry2Dimension);

            //计算两要素重叠区域。
            return OverlappingAreaRatio(targetPolygon, sourcePolygon, intersectArea);
        }

        /// <summary>
        /// 形状相似值
        /// </summary>
        /// <returns></returns>
        public double ShapeSimilarValue()
        {
            try
            {
                IPointCollection targetPointCollection = (IPointCollection)targetFeature.Shape;
                IPointCollection sourcePointCollection = (IPointCollection)sourceFeature.Shape;
                IPolygon targetPolygon = (IPolygon)targetFeature.Shape;
                IPolygon sourcePolygon = (IPolygon)sourceFeature.Shape;

                IGeometryCollection targetGeometryCollection = targetFeature.Shape as IGeometryCollection;
                IGeometryCollection sourceGeometryCollection = sourceFeature.Shape as IGeometryCollection;
                //返回组成该几何对象的子对象的数目。
                //如果组成几何对象都为1个
                if (targetGeometryCollection.GeometryCount == 1 && sourceGeometryCollection.GeometryCount == 1)
                {
                    //实现逆时针排序
                    targetPointCollection = SortVertexes(targetPointCollection);
                    sourcePointCollection = SortVertexes(sourcePointCollection);
                    //相似性计算
                    Dictionary<int, double> pLenth1 = new Dictionary<int, double>();
                    Dictionary<int, double> pAngle1 = new Dictionary<int, double>();
                    Dictionary<int, double> pLenth2 = new Dictionary<int, double>();
                    Dictionary<int, double> pAngle2 = new Dictionary<int, double>();
                    //计算每条边界线的长度和角度
                    angleStatics(targetPointCollection, targetPolygon.Length, ref pLenth1, ref pAngle1);
                    angleStatics(sourcePointCollection, sourcePolygon.Length, ref pLenth2, ref pAngle2);

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
                //如果组成几何对象不都为1个                
                else
                {
                    ITopologicalOperator targetTopologicalOperator = (ITopologicalOperator)targetPolygon;
                    IGeometry intersectArea = targetTopologicalOperator.Intersect(sourcePolygon, esriGeometryDimension.esriGeometry2Dimension);

                    //计算两要素重叠区域。
                    return OverlappingAreaRatio(targetPolygon, sourcePolygon, intersectArea);

                }
            }
            catch (Exception)
            {
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
                IPointCollection targetPointCollection = (IPointCollection)targetFeature.Shape;
                IPointCollection sourcePointCollection = (IPointCollection)sourceFeature.Shape;

                ICurve3 targetCurve = targetFeature.Shape as ICurve3;
                ICurve3 sourceCurve = sourceFeature.Shape as ICurve3;

                IGeometryCollection targetGeometryCollection = targetFeature.Shape as IGeometryCollection;
                IGeometryCollection sourceGeometryCollection = sourceFeature.Shape as IGeometryCollection;
                //要素（feature）不是multipart feature
                if (targetGeometryCollection.GeometryCount == 1 && sourceGeometryCollection.GeometryCount == 1)
                {
                    Dictionary<int, double> targetLengthDic = new Dictionary<int, double>();
                    Dictionary<int, double> targetAreaDic = new Dictionary<int, double>();

                    Dictionary<int, double> sourceLengthDic = new Dictionary<int, double>();
                    Dictionary<int, double> sourceAreaDic = new Dictionary<int, double>();

                    //待匹配图层
                    PolylineAngleStatistics(targetPointCollection, targetCurve.Length, ref targetLengthDic, ref targetAreaDic);
                    //源图层
                    PolylineAngleStatistics(sourcePointCollection, sourceCurve.Length, ref sourceLengthDic, ref sourceAreaDic);

                    int i = 0;
                    int j = 0;
                    Dictionary<double, double> targetLengthAngleDic = new Dictionary<double, double>();
                    Dictionary<double, double> sourceLengthAngleDic = new Dictionary<double, double>();
                    //建立长度、面积的字典映射
                    for (i = 0; i < targetLengthDic.Count; i++)
                    {
                        if (!targetLengthAngleDic.ContainsKey(targetLengthDic[i]))
                        {
                            targetLengthAngleDic.Add(targetLengthDic[i], targetAreaDic[i]);
                        }
                    }
                    for (j = 0; j < sourceLengthDic.Count; j++)
                    {
                        if (!sourceLengthAngleDic.ContainsKey(sourceLengthDic[j]))
                        {
                        	sourceLengthAngleDic.Add(sourceLengthDic[j], sourceAreaDic[j]);
                        }
                    }

                    Dictionary<int, double> tempDic = new Dictionary<int, double>();
                    tempDic = SortLengthsValue(targetLengthDic, sourceLengthDic);
                    Collection<double> area = new Collection<double>();

                    for (i = 0; i < tempDic.Count - 1; i++)
                    {
                        double ta;
                        if (targetLengthAngleDic.ContainsKey(tempDic[i]) && sourceLengthAngleDic.ContainsKey(tempDic[i]))
                        {
                            if (i == 0)
                            {
                                ta = (targetLengthAngleDic[tempDic[i]] - sourceLengthAngleDic[tempDic[i]]) * tempDic[i];
                            }
                            else
                            {
                                ta = (targetLengthAngleDic[tempDic[i]] - sourceLengthAngleDic[tempDic[i]]) * (tempDic[i] - tempDic[i - 1]);
                            }
                            if (ta < 0)
                                ta = -ta;
                            area.Add(ta);
                        }
                        else if (targetLengthAngleDic.ContainsKey(tempDic[i]) && !sourceLengthAngleDic.ContainsKey(tempDic[i]))
                        {
                            int m = i + 1;
                            while (!sourceLengthAngleDic.ContainsKey(tempDic[m]))
                            {
                                m++;
                            }

                            if (i == 0)
                            {
                                ta = (targetLengthAngleDic[tempDic[i]] - sourceLengthAngleDic[tempDic[m]]) * tempDic[i];
                            }
                            else
                            {
                                ta = (targetLengthAngleDic[tempDic[i]] - sourceLengthAngleDic[tempDic[m]]) * (tempDic[i] - tempDic[i - 1]);
                            }
                            if (ta < 0)
                                ta = -ta;
                            area.Add(ta);
                        }

                        else if (!targetLengthAngleDic.ContainsKey(tempDic[i]) && sourceLengthAngleDic.ContainsKey(tempDic[i]))
                        {
                            int n = i + 1;
                            while (!targetLengthAngleDic.ContainsKey(tempDic[n]))
                            {
                                n++;
                            }
                            if (i == 0)
                            {
                                ta = (targetLengthAngleDic[tempDic[n]] - sourceLengthAngleDic[tempDic[i]]) * tempDic[i];
                            }
                            else
                            {
                                ta = (targetLengthAngleDic[tempDic[n]] - sourceLengthAngleDic[tempDic[i]]) * (tempDic[i] - tempDic[i - 1]);
                            }
                            if (ta < 0)
                                ta = -ta;
                            area.Add(ta);
                        }
                    }
                    
                    double shapeSimilar;
                    double areaA = targetLengthDic[0] * targetAreaDic[0];
                    double areaB = sourceLengthDic[0] * sourceAreaDic[0];
                    double last;
                    if (tempDic.Count > 1)
                    {
                        if (targetLengthAngleDic[targetLengthDic[targetLengthDic.Count - 1]] > sourceLengthAngleDic[sourceLengthDic[sourceLengthDic.Count - 1]])
                        {
                            last = (targetLengthAngleDic[targetLengthDic[targetLengthDic.Count - 1]] - sourceLengthAngleDic[sourceLengthDic[sourceLengthDic.Count - 1]]) * (tempDic[tempDic.Count - 1] - tempDic[tempDic.Count - 2]);
                            area.Add(last);
                        }
                        else
                        {
                            last = (targetLengthAngleDic[targetLengthDic[targetLengthDic.Count - 1]] - sourceLengthAngleDic[sourceLengthDic[sourceLengthDic.Count - 1]]) * (tempDic[tempDic.Count - 1] - tempDic[tempDic.Count - 2]);
                            last = -last;
                            area.Add(last);
                        }

                        for (i = 1; i < targetLengthDic.Count; i++)
                        {
                            areaA = areaA + (targetLengthDic[i] - targetLengthDic[i - 1]) * targetAreaDic[i];
                        }

                        for (j = 1; j < sourceLengthDic.Count; j++)
                        {
                            areaB = areaB + (sourceLengthDic[j] - sourceLengthDic[j - 1]) * sourceAreaDic[j];
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
                //要素（feature）是multipart feature               
                else
                {
                    if (targetGeometryCollection.GeometryCount > 1 && sourceGeometryCollection.GeometryCount == 1)
                    {
                        ITopologicalOperator top = targetFeature.Shape as ITopologicalOperator;
                        IRelationalOperator relationalOperator = top.Buffer(0.0002) as IRelationalOperator;
                        if (relationalOperator.Contains(sourceFeature.Shape))
                            return 1;
                        else
                            return 0;
                    }
                    else
                    {
                        ITopologicalOperator top = sourceFeature.Shape as ITopologicalOperator;
                        IRelationalOperator relationalOperator = top.Buffer(0.0002) as IRelationalOperator;
                        if (relationalOperator.Contains(targetFeature.Shape))
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
                return 0;
                //throw;
            }

        }
        #region 形状相似度用到的函数
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
        /// 节点的相似值
        /// </summary>
        /// <param name="buffer">缓冲区</param>
        /// <returns>返回值</returns>
        public double MatchedPointsSimilarValue(double buffer)
        {
            try
            {
                IPointCollection targetPointCollection = targetFeature.Shape as IPointCollection;
                IPointCollection sourcePointCollection = sourceFeature.Shape as IPointCollection;

                Dictionary<string, string> pointDic = new Dictionary<string, string>();
                Dictionary<string, string> sourcePointDic = new Dictionary<string, string>();

                //IUnitConverter unitConverter = new UnitConverterClass();
                int i = 0, j = 0;
                int position = 0;

                int m = 0;
                int sameKey = 0;
                int outKey = 0;
                //遍历待匹配图层中所有的点
                for (i = 0; i < targetPointCollection.PointCount; i++)
                {
                    IPoint targetPoint = new PointClass();
                    //目标图层中的点cPoint
                    targetPoint = targetPointCollection.get_Point(i);

                    //IPoint unitCPoint = new PointClass();
                    //unitCPoint.X = unitConverter.ConvertUnits(cPoint.X, esriUnits.esriDecimalDegrees, esriUnits.esriMeters);
                    //unitCPoint.Y = unitConverter.ConvertUnits(cPoint.Y, esriUnits.esriDecimalDegrees, esriUnits.esriMeters);

                    ITopologicalOperator cTopOperator = targetPoint as ITopologicalOperator;

                    IGeometry5 targetGeo = cTopOperator.Buffer(buffer) as IGeometry5;
                    //遍历源图层中所有的点
                    for (j = position; j < sourcePointCollection.PointCount; j++)
                    {
                        IPoint sourcePoint = new PointClass();
                        sourcePoint = sourcePointCollection.get_Point(j);

                        //IPoint unitTUPoint = new PointClass();
                        //unitTUPoint.X = unitConverter.ConvertUnits(tuPoint.X, esriUnits.esriDecimalDegrees, esriUnits.esriMeters);
                        //unitTUPoint.Y = unitConverter.ConvertUnits(tuPoint.Y, esriUnits.esriDecimalDegrees, esriUnits.esriMeters);

                        IRelationalOperator targetRelationOperator = targetGeo as IRelationalOperator;
                        //IGeometry5 tuGeo = tuPoint as IGeometry5;
                        IGeometry sourceGeo = sourcePoint as IGeometry;
                        //如果是点集中的第一个点
                        if (j == 0)
                        {
                            if (sourceGeo != null)
                            {
                                if (targetRelationOperator.Contains(sourceGeo))
                                {
                                    if (!sourcePointDic.ContainsKey(sourcePoint.X.ToString("#0.000")))
                                    {
                                        sourcePointDic.Add(sourcePoint.X.ToString("#0.000"), sourcePoint.Y.ToString("#0.000"));
                                    }
                                    m++;
                                    position = m;
                                    break;
                                }
                                else
                                {
                                    if (pointDic.ContainsKey(sourcePoint.X.ToString("#0.000")))
                                    {
                                        if (pointDic.ContainsValue(sourcePoint.Y.ToString("#0.000")))
                                        {
                                            continue;
                                        }
                                        else
                                        {
                                            if (!sourcePointDic.ContainsKey(sourcePoint.X.ToString("#0.000")) && !pointDic.ContainsValue(sourcePoint.Y.ToString("#0.000")))
                                            {
                                                sameKey++;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (!sourcePointDic.ContainsKey(sourcePoint.X.ToString("#0.000")) && !pointDic.ContainsValue(sourcePoint.Y.ToString("#0.000")))
                                        {
                                            pointDic.Add(sourcePoint.X.ToString("#0.000"), sourcePoint.Y.ToString("#0.000"));
                                        }
                                    }
                                }
                            }

                        }
                        //如果是点集中的最后一个点                        
                        else if (j == sourcePointCollection.PointCount - 1)
                        {
                            if (targetRelationOperator.Contains(sourceGeo))
                            {
                                if (pointDic.ContainsKey(sourcePoint.X.ToString("#0.000")))
                                {
                                    if (pointDic.ContainsValue(sourcePoint.Y.ToString("#0.000")))
                                    {
                                        outKey--;
                                    }
                                }
                                if (!sourcePointDic.ContainsKey(sourcePoint.X.ToString("#0.000")))
                                {
                                    sourcePointDic.Add(sourcePoint.X.ToString("#0.000"), sourcePoint.Y.ToString("#0.000"));
                                }
                                m++;
                                position = m;
                                break;
                            }
                            else
                            {
                                if (pointDic.ContainsKey(sourcePoint.X.ToString("#0.000")))
                                {
                                    if (pointDic.ContainsValue(sourcePoint.Y.ToString("#0.000")))
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        if (!sourcePointDic.ContainsKey(sourcePoint.X.ToString("#0.000")) && !pointDic.ContainsValue(sourcePoint.Y.ToString("#0.000")))
                                        {
                                            sameKey++;
                                        }
                                    }
                                }
                                else
                                {
                                    if (!sourcePointDic.ContainsKey(sourcePoint.X.ToString("#0.000")) && !pointDic.ContainsValue(sourcePoint.Y.ToString("#0.000")))
                                    {
                                        pointDic.Add(sourcePoint.X.ToString("#0.000"), sourcePoint.Y.ToString("#0.000"));
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (targetRelationOperator.Contains(sourceGeo))
                            {
                                if (pointDic.ContainsKey(sourcePoint.X.ToString("#0.000")))
                                {
                                    if (pointDic.ContainsValue(sourcePoint.Y.ToString("#0.000")))
                                    {
                                        outKey--;
                                    }
                                }
                                if (!sourcePointDic.ContainsKey(sourcePoint.X.ToString("#0.000")))
                                {
                                    sourcePointDic.Add(sourcePoint.X.ToString("#0.000"), sourcePoint.Y.ToString("#0.000"));
                                }
                                m++;
                                position = m;
                                break;
                            }
                            else
                            {
                                ILine backLine = new LineClass();
                                ILine beforeLine = new LineClass();

                                backLine.PutCoords(sourcePointCollection.get_Point(j - 1), sourcePointCollection.get_Point(j));
                                beforeLine.PutCoords(sourcePointCollection.get_Point(j), sourcePointCollection.get_Point(j + 1));

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
                                    if (pointDic.ContainsKey(sourcePoint.X.ToString("#0.000")))
                                    {
                                        if (pointDic.ContainsValue(sourcePoint.Y.ToString("#0.000")))
                                        {
                                            continue;
                                        }
                                        else
                                        {
                                            if (!sourcePointDic.ContainsKey(sourcePoint.X.ToString("#0.000")) && !pointDic.ContainsValue(sourcePoint.Y.ToString("#0.000")))
                                            {
                                                sameKey++;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (!sourcePointDic.ContainsKey(sourcePoint.X.ToString("#0.000")) && !pointDic.ContainsValue(sourcePoint.Y.ToString("#0.000")))
                                        {
                                            pointDic.Add(sourcePoint.X.ToString("#0.000"), sourcePoint.Y.ToString("#0.000"));
                                        }
                                    }
                                }
                            }
                        }

                    }

                }
                
                int sum = targetPointCollection.PointCount + pointDic.Count + sameKey + outKey;
                double shapeSimiliarValue = Convert.ToDouble(m) / sum;
                return shapeSimiliarValue;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }
        #region 封装的函数
        /// <summary>
        /// 重叠面积比值
        /// </summary>
        /// <param name="targetPolygon">目标要素多边形</param>
        /// <param name="sourcePolygon">源要素多边形</param>
        /// <param name="intersectArea">相交多边形</param>
        /// <returns>相交面积与二者面积和比值的二倍</returns>
        private double OverlappingAreaRatio(IPolygon targetPolygon, IPolygon sourcePolygon, IGeometry intersectArea)
        {
            IArea pTargetArea1 = (IArea) targetPolygon;
            IArea pSourceArea2 = (IArea) sourcePolygon;
            IArea pIntersectArea = (IArea) intersectArea;

            double A1 = pTargetArea1.Area;
            double A2 = pSourceArea2.Area;
            double A = pIntersectArea.Area;
            //相交面积与二者面积和的二倍
            A = 2*A/(A1 + A2);
            return A;
        }

        #endregion
        #region 形状相似度
        /// <summary>
        /// 坐标排序
        /// </summary>
        /// <param name="pointCol">输入的点坐标集</param>
        /// <returns></returns>
        private IPointCollection SortVertexes(IPointCollection pointCol)
        {
            //定义一个点的集合
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
            //冒泡排序
            for (int i = 0; i < count; i++)
            {
                for (int j = i; j < count; j++)
                {
                    //如果第一个点的横坐标大于第二个点的横坐标
                    if (tempCol[i].X > tempCol[j].X)
                    {
                        tempX = tempCol[j].X;
                        tempCol[j].X = tempCol[i].X;
                        tempCol[i].X = tempX;

                        tempY = tempCol[j].Y;
                        tempCol[j].Y = tempCol[i].Y;
                        tempCol[i].Y = tempY;
                    }
                        //横坐标相同，但是纵坐标大
                    else if (tempCol[i].X == tempCol[j].X && tempCol[i].Y > tempCol[j].Y)
                    {
                        tempY = tempCol[j].Y;
                        tempCol[j].Y = tempCol[i].Y;
                        tempCol[i].Y = tempY;
                    }
                }
            }
            //找到起始点坐标（首位点坐标相同）
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
        /// <param name="targetLengthDic"></param>
        /// <param name="sourceLengthDic"></param>
        /// <returns></returns>
        private Dictionary<int, double> SortLengthsValue(Dictionary<int, double> targetLengthDic, Dictionary<int, double> sourceLengthDic)
        {
            Dictionary<int, double> pDic = new Dictionary<int, double>();

            for (int i = 0; i < targetLengthDic.Count; i++)
            {
                pDic.Add(i, targetLengthDic[i]);
            }
            int count = pDic.Count;
            double temp;

            for (int i = 0; i < sourceLengthDic.Count; i++)
            {
                if (!pDic.ContainsValue(sourceLengthDic[i]))
                {
                    pDic.Add(count, sourceLengthDic[i]);
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




        #endregion

        /// <summary>
        /// 点与点之间的欧式距离（单位待定？）
        /// </summary>
        /// <returns></returns>
        public double PointDistance()
        {
            IPoint targetPoint1 = (IPoint)targetFeature.Shape;
            IPoint sourcePoint2 = (IPoint)sourceFeature.Shape;
            double x1, y1, x2, y2;
            x1 = targetPoint1.X;
            y1 = targetPoint1.Y;
            //相似度计算
            x2 = sourcePoint2.X;
            y2 = sourcePoint2.Y;

            double distance = Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
            return distance;
        }

        /// <summary>
        /// hausdorff距离（线实体）
        /// </summary>
        /// <returns></returns>
        public double HausdorffDistance()
        {
            double hTargetDis=0.0;
            double hSourceDis=0.0;
            List<double> listSource = new List<double>();
            List<double> lisSourcetTar = new List<double>();
            List<double> listTarget = new List<double>();
            List<double> listTargetSour = new List<double>();
            try
            {
                IPointCollection targetPointCollection = (IPointCollection)targetFeature.Shape;
                IPointCollection sourcePointCollection = (IPointCollection)sourceFeature.Shape;

                for (int i=0;i<targetPointCollection.PointCount;i++)
                {
                    for (int j=0;j<sourcePointCollection.PointCount;j++)
                    {
                        listSource.Add(
                            Math.Sqrt((targetPointCollection.get_Point(i).X - sourcePointCollection.get_Point(j).X)*
                                      (targetPointCollection.get_Point(i).X - sourcePointCollection.get_Point(j).X) +
                                      (targetPointCollection.get_Point(i).Y - sourcePointCollection.get_Point(j).Y)*
                                      (targetPointCollection.get_Point(i).Y - sourcePointCollection.get_Point(j).Y)));

                    }
                    lisSourcetTar.Add(listSource.Min());
                }
                hTargetDis = lisSourcetTar.Max();

                for (int i = 0; i < sourcePointCollection.PointCount; i++)
                {
                    for (int j = 0; j < targetPointCollection.PointCount; j++)
                    {
                        listTarget.Add(
                            Math.Sqrt((sourcePointCollection.get_Point(i).X - targetPointCollection.get_Point(j).X) *
                                      (sourcePointCollection.get_Point(i).X - targetPointCollection.get_Point(j).X) +
                                      (sourcePointCollection.get_Point(i).Y - targetPointCollection.get_Point(j).Y) *
                                      (sourcePointCollection.get_Point(i).Y - targetPointCollection.get_Point(j).Y)));

                    }
                    listTargetSour.Add(listTarget.Min());
                }
                hSourceDis = listTargetSour.Max();


            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            if (hTargetDis > hSourceDis)
            {
                return hTargetDis;
            }
            else
            {
                return hSourceDis;
            }
        }
        /// <summary>
        /// 两条线之间的夹角，最后结果为角度（如60减30等于30）
        /// </summary>
        /// <returns></returns>
        public double IncludedAngle()
        {
            double kTar = 0.0;
            double kSour = 0.0;
            double result = 0.0;
            try
            {
                ICurve targetCurve = targetFeature.Shape as ICurve;
                ICurve sourceCurve = sourceFeature.Shape as ICurve;
                kTar = (targetCurve.ToPoint.Y - targetCurve.FromPoint.Y)/(targetCurve.ToPoint.X - targetCurve.FromPoint.X);
                kSour = (sourceCurve.ToPoint.Y - sourceCurve.FromPoint.Y) / (sourceCurve.ToPoint.X - sourceCurve.FromPoint.X);

                result = Math.Atan(Math.Abs((kSour - kTar)/(1 + kSour*kTar)));
                result = (180 / Math.PI) * result;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            return result;
        }




    }
}
