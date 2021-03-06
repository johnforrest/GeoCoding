﻿/*-------------------------------------------------------------------------
 * 版权所有：武汉大学
 * 创建人：johnforrest
 * 联系方式：571716604@qq.com
 * 创建时间： 4/3/2018 2:58:50 PM
 * 类名称：ClsPolygonMatch
 * 本类主要用途描述：
 * 修改人：
 * 修改时间：
 * 修改备注：
 * @version 1.0
 *  -------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace ZJGISDataUpdating.Class
{
    class ClsPolygonMatch
    {
        /// <summary>
        /// 跨尺度匹配要素--算法核心-填充xxx_DifPyTable（面状）
        /// </summary>
        /// <param name="TUFeatCls">源图层</param>
        /// <param name="TEFeatCls">待匹配更新的图层</param>
        /// <param name="resultTable">匹配结果表</param>
        /// <param name="buffer">缓冲区距离</param>
        /// <param name="area">缓冲区面积</param>
        /// <param name="prgMain">总进度条</param>
        /// <param name="prgSub">当前进度条</param>
        /// <param name="stateLabel">状体提示信息</param>
        /// <returns>返回类型，布尔型</returns>
        public bool DifScaleSearchChangedPolygonFeatures(IFeatureClass SrcFeatCls, IFeatureClass TarFeatCls, ITable resultTable,
            double buffer, double area, ProgressBar prgMain, ProgressBar prgSub, LabelX stateLabel)
        {

            //源图层的要素个数
            int srcFeatCount = 0;
            //源图层的游标
            IFeatureCursor srcFeatCursor = null;
            //源图层通过游标遍历的下一个feature
            IFeature srcFeature = null;

            //根据特定的控件过滤条件，源图层查询到的待匹配图层的游标
            IFeatureCursor tarFeatCursor = null;
            bool functionReturnValue = false;
            //teFeat是根据游标查询出的待匹配的要素
            IFeature tarFeature = null;

            // bool shpEqual = true;
            // bool attributeEqual = true;

            //featureCount记录每个源图层要素查询得到的待匹配图层的要素个数
            int featureCount = 0;

            //IPolygon tuPolygon;
            //IArea tuArea=null;

            //反向查询：根据待匹配图层查询源图层
            //ISpatialFilter用于返回和修改filter使用的空间关系。查询tu要素
            ISpatialFilter spatialFilter;
            //反向查询源图层要素
            ISpatialFilter revSpatialFilter;
            //反向查询源图层游标，也就是源图层游标
            IFeatureCursor revFeatCursor = null;
            //反向查询要素，也就是源图层要素
            IFeature revFeat = null;
            //记录反向查询过要素
            //记录待匹配图层要素包含源图层要素时，把包含的源图层要素添加到集合中
            Collection<int> revCol = new Collection<int>();

            //记录查询过要素
            Collection<int> targetCol = new Collection<int>();

            try
            {
                IDataset dataset = resultTable as IDataset;
                IWorkspaceEdit workspaceEdit = dataset.Workspace as IWorkspaceEdit;
                workspaceEdit.StartEditing(true);
                workspaceEdit.StartEditOperation();
                //源图层的要素个数及游标
                srcFeatCount = SrcFeatCls.FeatureCount(null);
                srcFeatCursor = SrcFeatCls.Search(null, false);
                int featCount = 0;
                // int targetFeatCount = 0;

                if (srcFeatCursor != null)
                {
                    srcFeature = srcFeatCursor.NextFeature();
                }
                IRowBuffer rowBuffer = null;
                ICursor rowCursor = null;

                //待匹配图层FIXOID（OID）字段的索引
                int fixoid = 0;

                //记录第一次为匹配的要素OID
                Collection<int> unmatchedCol = new Collection<int>();

                //遍历每一条源图层要素（Feature）
                while (srcFeature != null)
                {
                    //Creates a row buffer that can be used with an insert cursor.
                    rowBuffer = resultTable.CreateRowBuffer();
                    rowCursor = resultTable.Insert(true);

                    spatialFilter = new SpatialFilterClass();
                    //esriSpatialRelIntersects：Returns a feature if any spatial relationship is found. Applies to all shape type combinations. 
                    spatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                    //查询tu要素
                    spatialFilter.Geometry = srcFeature.Shape;

                    int indexFID = rowBuffer.Fields.FindField("源OID");
                    int indexOID = rowBuffer.Fields.FindField("待匹配OID");
                    //rowBuffer.set_Value(indexFID, tuFeat.OID);
                    int index = rowBuffer.Fields.FindField("变化标记");
                    //记录在由源图层查询待匹配图层是1对1的情况下，再由待匹配的图层反向查询源图层获得的源图层的个数
                    int revFeatCount = 0;

                    //查看进度
                    ClsStatic.AboutProcessBar(srcFeatCount, prgMain, prgSub);
                    //不包含
                    if (!revCol.Contains(srcFeature.OID))
                    {
                        //The number of features selected by the specified query.
                        //featureCount记录在特定的查询条件下，每个源图层要素查询得到的待匹配图层的要素个数
                        featureCount = TarFeatCls.FeatureCount(spatialFilter);

                        //源图层一个要素对应1个目标图层要素（一对一关系）
                        if (featureCount == 1)
                        {
                            //Returns an object cursor that can be used to fetch feature objects selected by the specified query.
                            //teFeatCursor是根据特定的控件过滤条件，源图层查询到的待匹配图层的游标
                            tarFeatCursor = TarFeatCls.Search(spatialFilter, false);
                            tarFeature = tarFeatCursor.NextFeature();
                            int oid = Convert.ToInt32(tarFeature.get_Value(fixoid));

                            //TODO :正反向匹配的反向查询：根据待匹配图层查询源图层

                            revSpatialFilter = new SpatialFilterClass();
                            revSpatialFilter.Geometry = tarFeature.Shape;
                            revSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                            revFeatCount = SrcFeatCls.FeatureCount(revSpatialFilter);

                            //反向查询也是一对一的关系
                            if (revFeatCount == 1)
                            {
                                //插入分类码
                                if (srcFeature != null)
                                {
                                    rowBuffer.set_Value(rowBuffer.Fields.FindField("源要素分类码"), srcFeature.get_Value(srcFeature.Fields.FindField("FCODE")));
                                }
                                if (tarFeature != null)
                                {
                                    rowBuffer.set_Value(rowBuffer.Fields.FindField("待匹配要素分类码"), tarFeature.get_Value(tarFeature.Fields.FindField("FCODE")));
                                }

                                rowBuffer.set_Value(indexFID, srcFeature.OID);
                                rowBuffer.set_Value(indexOID, oid);
                                //rowBuffer.set_Value(index, "一对一");
                                rowBuffer.set_Value(index, ClsConstant.One2One);
                                rowCursor.InsertRow(rowBuffer);
                                featCount++;
                            }
                            //反向查询不是一对一的关系
                            else
                            {
                                //删除row kenen
                                //rowCursor.DeleteRow();

                                //20170522注释掉
                                //rowBuffer = null;

                                revFeatCursor = SrcFeatCls.Search(revSpatialFilter, false);
                                revFeat = revFeatCursor.NextFeature();

                                //记录待匹配图层与源图层在重叠关系下，源图层的个数
                                Collection<IFeature> tempCol = new Collection<IFeature>();
                                //记录待匹配图层与源图层之间包含关系的个数
                                int tempCount = 0;

                                IRelationalOperator teRel = tarFeature.Shape as IRelationalOperator;
                                ITopologicalOperator top = tarFeature.Shape as ITopologicalOperator;
                                IArea teArea = tarFeature.Shape as IArea;

                                while (revFeat != null)
                                {
                                    IGeometry revGeo = revFeat.Shape as IGeometry;

                                    if (!revCol.Contains(revFeat.OID))
                                    {
                                        //首先判断两个几何图形是否是重叠关系：也就是待匹配图层是否与源图层重叠
                                        //如果有重叠，那么进一步判断重叠面积的比值
                                        if (teRel.Overlaps(revGeo))
                                        {
                                            IArea revArea = revFeat.Shape as IArea;
                                            IGeometry insertGeo = top.Intersect(revGeo, esriGeometryDimension.esriGeometry2Dimension);
                                            IArea insertArea = (IArea)insertGeo;
                                            //源图层和待匹配图层的交集面积与源图层面积的比值
                                            //如果比值小于0.1，那么久认为二者不匹配
                                            double d = insertArea.Area / revArea.Area;
                                            if (d < 0.1)
                                            {
                                                if (!unmatchedCol.Contains(revFeat.OID))
                                                {
                                                    unmatchedCol.Add(revFeat.OID);
                                                }
                                                //revFeat = revFeatCursor.NextFeature();
                                                //continue;
                                            }
                                            //d>0.1
                                            else
                                            {
                                                tempCol.Add(revFeat);
                                            }
                                        }
                                        //其次判断两个几何图形是否是包含关系：也就是待匹配图层是否包含源图层，如果包含
                                        else if (teRel.Contains(revGeo))
                                        {
                                            rowBuffer = resultTable.CreateRowBuffer();
                                            rowCursor = resultTable.Insert(true);
                                            rowBuffer.set_Value(indexFID, revFeat.OID);
                                            rowBuffer.set_Value(indexOID, oid);
                                            ////rowBuffer.set_Value(index, "多对一");
                                            rowBuffer.set_Value(index, ClsConstant.More2One);
                                            rowCursor.InsertRow(rowBuffer);
                                            //记录待匹配图层要素包含源图层要素时，把包含的源图层要素添加到集合中
                                            revCol.Add(revFeat.OID);
                                            tempCount++;
                                            featCount++;
                                        }
                                    }

                                    revFeat = revFeatCursor.NextFeature();
                                }

                                //有1个待匹配图层与源图层是重叠关系并且不是包含关系
                                //（重叠关系下面积比值都大于0.1）
                                if (tempCol.Count == 1 && tempCount == 0)
                                {
                                    if (!targetCol.Contains(tempCol[0].OID))
                                    {
                                        //插入分类码
                                        if (srcFeature != null)
                                        {
                                            rowBuffer.set_Value(rowBuffer.Fields.FindField("源要素分类码"), srcFeature.get_Value(srcFeature.Fields.FindField("FCODE")));
                                        }
                                        if (tarFeature != null)
                                        {
                                            rowBuffer.set_Value(rowBuffer.Fields.FindField("待匹配要素分类码"), tarFeature.get_Value(tarFeature.Fields.FindField("FCODE")));
                                        }

                                        rowBuffer = resultTable.CreateRowBuffer();
                                        rowCursor = resultTable.Insert(true);
                                        rowBuffer.set_Value(indexFID, tempCol[0].OID);
                                        rowBuffer.set_Value(indexOID, oid);
                                        //rowBuffer.set_Value(index, "一对一");
                                        rowBuffer.set_Value(index, ClsConstant.One2One);
                                        rowCursor.InsertRow(rowBuffer);

                                        revCol.Add(tempCol[0].OID);
                                        targetCol.Add(tempCol[0].OID);
                                        featCount++;
                                    }
                                }
                                //有1个待匹配图层与源图层是重叠关系并且是包含关系
                                //（重叠关系下面积比值都大于0.1）
                                else if (tempCol.Count == 1 && tempCount > 0)
                                {
                                    if (!targetCol.Contains(tempCol[0].OID))
                                    {
                                        rowBuffer = resultTable.CreateRowBuffer();
                                        rowCursor = resultTable.Insert(true);
                                        rowBuffer.set_Value(indexFID, tempCol[0].OID);
                                        rowBuffer.set_Value(indexOID, oid);
                                        //rowBuffer.set_Value(index, "多对一");
                                        rowBuffer.set_Value(index, ClsConstant.More2One);
                                        rowCursor.InsertRow(rowBuffer);

                                        revCol.Add(tempCol[0].OID);
                                        targetCol.Add(tempCol[0].OID);
                                        featCount++;
                                    }

                                }
                                //有多个待匹配图层与源图层是重叠关系（重叠关系下面积比值都大于0.1）
                                else if (tempCol.Count > 1)
                                {
                                    for (int k = 0; k < tempCol.Count; k++)
                                    {
                                        rowBuffer = resultTable.CreateRowBuffer();
                                        rowCursor = resultTable.Insert(true);
                                        rowBuffer.set_Value(indexFID, tempCol[k].OID);
                                        rowBuffer.set_Value(indexOID, oid);
                                        //rowBuffer.set_Value(index, "多对一");
                                        rowBuffer.set_Value(index, ClsConstant.More2One);
                                        rowCursor.InsertRow(rowBuffer);

                                        revCol.Add(tempCol[k].OID);
                                        featCount++;
                                    }
                                }
                            }
                        }
                        //源图层一个要素对应0个目标图层要素（1对0关系）                        
                        else if (featureCount == 0)
                        {
                            rowBuffer.set_Value(indexFID, srcFeature.OID);
                            //rowBuffer.set_Value(index, "新增要素");
                            rowBuffer.set_Value(index, ClsConstant.One2Zero);
                            rowCursor.InsertRow(rowBuffer);
                        }
                        //源图层一个要素对应多个目标图层要素（1对多关系）                        
                        else if (featureCount > 1)
                        {
                            //teFeatCursor是根据空间关系找到的与源图层要素对应的一对多的所有要素集
                            tarFeatCursor = TarFeatCls.Search(spatialFilter, false);
                            IPolygon tuPolygon = srcFeature.Shape as IPolygon;
                            IFeature maxAreaFeat = null;
                            double maxRadio = 0;
                            tarFeature = tarFeatCursor.NextFeature();

                            while (tarFeature != null)
                            {
                                IPolygon tePolygon = tarFeature.Shape as IPolygon;
                                ITopologicalOperator ptopo = (ITopologicalOperator)tuPolygon;
                                //求解源图层要素与待匹配要素的拓扑交集
                                IGeometry geometry = ptopo.Intersect(tePolygon, esriGeometryDimension.esriGeometry2Dimension);

                                IArea tuArea = tuPolygon as IArea;
                                IArea teArea = tePolygon as IArea;
                                IArea area1 = geometry as IArea;
                                //源要素面积和待匹配要素面积的交集面积 与源图层要素面积的比值
                                double radio = area1.Area / tuArea.Area;
                                if (radio > maxRadio)
                                {
                                    maxRadio = radio;
                                    maxAreaFeat = tarFeature;
                                }
                                tarFeature = tarFeatCursor.NextFeature();
                            }

                            //插入分类码
                            if (srcFeature != null)
                            {
                                rowBuffer.set_Value(rowBuffer.Fields.FindField("源要素分类码"), srcFeature.get_Value(srcFeature.Fields.FindField("FCODE")));
                            }
                            if (maxAreaFeat != null)
                            {
                                rowBuffer.set_Value(rowBuffer.Fields.FindField("待匹配要素分类码"), maxAreaFeat.get_Value(maxAreaFeat.Fields.FindField("FCODE")));
                            }

                            rowBuffer.set_Value(indexFID, srcFeature.OID);
                            rowBuffer.set_Value(indexOID, maxAreaFeat.get_Value(fixoid));
                            //rowBuffer.set_Value(index, "一对一");
                            rowBuffer.set_Value(index, ClsConstant.One2One);
                            rowCursor.InsertRow(rowBuffer);

                            targetCol.Add(srcFeature.OID);
                            featCount++;
                        }
                    }

                    //源图层一个要素对应多于500个目标图层要素，清零                                            
                    if (featCount > 500)
                    {
                        rowCursor.Flush();
                        featCount = 0;
                    }

                    stateLabel.Text = "图层" + SrcFeatCls.AliasName + "已匹配到" + prgMain.Value.ToString() + "(" + srcFeatCount.ToString() + ")" + "个要素";
                    stateLabel.Refresh();
                    srcFeature = srcFeatCursor.NextFeature();
                }

                //待匹配图层与源图层之间是重叠关系（重叠面积的比值小于0.1）
                for (int m = 0; m < unmatchedCol.Count; m++)
                {
                    if (!revCol.Contains(unmatchedCol[m]) && !targetCol.Contains(unmatchedCol[m]))
                    {
                        rowBuffer = resultTable.CreateRowBuffer();
                        rowCursor = resultTable.Insert(true);
                        rowBuffer.set_Value(rowBuffer.Fields.FindField("源OID"), unmatchedCol[m]);
                        //rowBuffer.set_Value(rowBuffer.Fields.FindField("变化标记"), "新增要素");
                        rowBuffer.set_Value(rowBuffer.Fields.FindField("变化标记"), ClsConstant.One2Zero);
                        rowCursor.InsertRow(rowBuffer);
                    }
                }

                rowCursor.Flush();
                workspaceEdit.StopEditOperation();
                workspaceEdit.StopEditing(true);

                //MessageBoxEx.Show("匹配已完成！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //进度条

                stateLabel.Text = "";
                prgMain.Value = 0;
                prgSub.Value = 0;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return functionReturnValue;
        }


        /// <summary>
        /// 跨尺度匹配要素--算法核心-填充xxx_DifPyTable（面状）
        /// </summary>
        /// <param name="sourceFeatureClass">源图层</param>
        /// <param name="targetFeatureClass">待匹配更新的图层</param>
        /// <param name="resultTable">匹配结果表</param>
        /// <param name="buffer">缓冲区距离</param>
        /// <param name="minArea">缓冲区面积</param>
        /// <param name="prgMain">总进度条</param>
        /// <param name="prgSub">当前进度条</param>
        /// <param name="stateLabel">状体提示信息</param>
        /// <returns>返回类型，布尔型</returns>
        public bool SearchChangedPolygonFeaturesArea(IFeatureClass sourceFeatureClass, IFeatureClass targetFeatureClass,
            ITable resultTable, double buffer, double minArea, ProgressBar prgMain, ProgressBar prgSub, LabelX stateLabel)
        {
            //源图层的要素个数
            int srcFeatCount = 0;
            //源图层的游标
            IFeatureCursor srcFeatCursor = null;
            //源图层通过游标遍历的下一个feature
            IFeature srcFeature = null;

            //根据特定的控件过滤条件，源图层查询到的待匹配图层的游标
            IFeatureCursor tarFeatCursor = null;
            IFeatureCursor revSourceFeatCursor = null;

            //teFeat是根据游标查询出的待匹配的要素
            IFeature tarFeature = null;
            IFeature revSourceFeature = null;


            //记录每个源图层要素查询得到的待匹配图层的要素个数
            int featureCount = 0;
            //记录由待匹配的图层反向查询源图层获得的源图层的个数
            int revSourFeatCount = 0;


            //ISpatialFilter用于返回和修改filter使用的空间关系。
            ISpatialFilter spatialFilter;

            //反向查询：根据待匹配图层查询源图层
            //反向查询源图层要素
            ISpatialFilter revSpatialFilter;
            //反向查询源图层游标，也就是源图层游标

            //记录反向查询过要素
            //记录待匹配图层要素包含源图层要素时，把包含的源图层要素添加到集合中
            Collection<int> revCol = new Collection<int>();

            List<IFeature> revListMore2More = null;
            Dictionary<IFeature, IFeature> revListMore2One = null;
            List<IFeature> revList = null;

            List<IFeature> revListOne2One = null;
            List<IFeature> revListOne2More = null;

            //记录查询过要素
            Collection<int> targetCol = new Collection<int>();

            try
            {
                IDataset dataset = resultTable as IDataset;
                IWorkspaceEdit workspaceEdit = dataset.Workspace as IWorkspaceEdit;
                workspaceEdit.StartEditing(true);
                workspaceEdit.StartEditOperation();
                //源图层的要素个数及游标
                srcFeatCount = sourceFeatureClass.FeatureCount(null);
                srcFeatCursor = sourceFeatureClass.Search(null, false);

                if (srcFeatCursor != null)
                {
                    srcFeature = srcFeatCursor.NextFeature();
                }
                IRowBuffer rowBuffer = null;
                ICursor rowCursor = null;

                //待匹配图层FIXOID（OID）字段的索引
                int fixoid = 0;

                //记录第一次为匹配的要素OID
                Collection<int> unmatchedCol = new Collection<int>();

                //遍历每一条源图层要素（Feature）
                while (srcFeature != null)
                {
                    rowBuffer = resultTable.CreateRowBuffer();
                    rowCursor = resultTable.Insert(true);

                    int sourceOID = rowBuffer.Fields.FindField("源OID");
                    int targetOID = rowBuffer.Fields.FindField("待匹配OID");

                    int sourceFcode = rowBuffer.Fields.FindField("源要素分类码");
                    int targetFcode = rowBuffer.Fields.FindField("待匹配要素分类码");

                    //rowBuffer.set_Value(indexFID, tuFeat.OID);
                    int changeIndex = rowBuffer.Fields.FindField("变化标记");

                    rowBuffer.set_Value(sourceOID, srcFeature.OID);

                    //查看进度
                    ClsStatic.AboutProcessBar(srcFeatCount, prgMain, prgSub);

                    spatialFilter = new SpatialFilterClass();
                    spatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                    spatialFilter.Geometry = srcFeature.Shape;
                    featureCount = targetFeatureClass.FeatureCount(spatialFilter);
                    //源图层一个要素对应0个目标图层要素（1对0关系）                        
                    if (featureCount == 0)
                    {
                        rowBuffer.set_Value(changeIndex, ClsConstant.One2Zero);
                    }
                    //源图层一个要素对应1个目标图层要素（一对一关系）
                    else if (featureCount == 1)
                    {
                        revListOne2More = new List<IFeature>();
                        revListOne2One = new List<IFeature>();
                        //teFeatCursor是根据特定的控件过滤条件，源图层查询到的待匹配图层的游标
                        tarFeatCursor = targetFeatureClass.Search(spatialFilter, false);
                        tarFeature = tarFeatCursor.NextFeature();
                        if (ClsIndicatorFunStatic.AreaRatio(srcFeature, tarFeature)>minArea)
                        {
                            //TODO :反向查询：根据待匹配图层查询源图层
                            revSpatialFilter = new SpatialFilterClass();
                            revSpatialFilter.Geometry = tarFeature.Shape;
                            revSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                            revSourFeatCount = sourceFeatureClass.FeatureCount(revSpatialFilter);
                            revSourceFeatCursor = sourceFeatureClass.Search(revSpatialFilter, false);
                            revSourceFeature = revSourceFeatCursor.NextFeature();
                            //反向查询也是一对一的关系
                            if (revSourFeatCount == 1)
                            {
                                rowBuffer.set_Value(targetOID, tarFeature.OID);
                                rowBuffer.set_Value(changeIndex, ClsConstant.One2One);
                                SetFcode(srcFeature, rowBuffer, sourceFcode, tarFeature, targetFcode);
                                //revListOne2One.Add(revSourceFeature);
                            }
                            //反向查询是一对多的关系
                            else
                            {
                                //Dictionary<IFeature, IFeature> pDicOverlap = new Dictionary<IFeature, IFeature>();
                                List<IFeature> pDicOverlap = new List<IFeature>();
                                List<IFeature> plistContains = new List<IFeature>();

                                IRelationalOperator tarFeatureRelation = tarFeature.Shape as IRelationalOperator;

                                while (revSourceFeature != null)
                                {
                                    IGeometry revGeometry = revSourceFeature.Shape as IGeometry;
                                    //首先判断两个几何图形是否是重叠关系：也就是待匹配图层是否与源图层重叠
                                    //如果有重叠，那么进一步判断重叠面积的比值
                                    if (tarFeatureRelation.Overlaps(revGeometry))
                                    {
                                        if (ClsIndicatorFunStatic.AreaRatio(revSourceFeature,tarFeature)> minArea)
                                        {
                                            //TODO:添加的判断oid,对结果影响很大
                                            //if (revSourceFeature.OID == srcFeature.OID)
                                            //{

                                            //源图层要素重心是否落入待匹配图层中
                                            if (ClsIndicatorFunStatic.PolygonContainsPoint(revSourceFeature, tarFeature))
                                            {
                                                if (!pDicOverlap.Contains(revSourceFeature))
                                                {
                                                    pDicOverlap.Add(revSourceFeature);
                                                }
                                            }
                                            //}
                                        }
                                    }
                                    //其次判断两个几何图形是否是包含关系：也就是待匹配图层是否包含源图层，如果包含
                                    else if (tarFeatureRelation.Contains(revGeometry))
                                    {
                                        if (!plistContains.Contains(revSourceFeature))
                                        {
                                            plistContains.Add(revSourceFeature);
                                        }
                                    }
                                    revSourceFeature = revSourceFeatCursor.NextFeature();
                                }
                                //未匹配
                                if (pDicOverlap.Count == 0 && plistContains.Count == 0)
                                {
                                    rowBuffer.set_Value(changeIndex, ClsConstant.One2Zero);
                                }
                                //TODO:更改
                                //一对一
                                //有1个待匹配图层与源图层是重叠关系并且不是包含关系
                                //（重叠关系下面积比值都大于0.1）
                                if ((pDicOverlap.Count == 1 && plistContains.Count == 0) ||
                                    (pDicOverlap.Count == 0 && plistContains.Count == 1))
                                {

                                    SetFcode(srcFeature, rowBuffer, sourceFcode, tarFeature, targetFcode);
                                    rowBuffer.set_Value(changeIndex, ClsConstant.One2One);
                                    rowBuffer.set_Value(targetOID, tarFeature.OID);
                                }
                                //多对一
                                //有1个待匹配图层与源图层是重叠关系并且是包含关系,
                                //（重叠关系下面积比值都大于0.1）
                                else
                                {
                                    rowBuffer.set_Value(changeIndex, ClsConstant.More2One);
                                    rowBuffer.set_Value(targetOID, tarFeature.OID);

                                    foreach (IFeature pfeature in pDicOverlap)
                                    {
                                        if (!rowBuffer.get_Value(sourceOID).ToString().Contains(pfeature.OID.ToString()))
                                        {
                                            string oids = rowBuffer.get_Value(sourceOID).ToString() + ";" + pfeature.OID.ToString();
                                            rowBuffer.set_Value(sourceOID, oids);
                                        }
                                    }

                                    foreach (IFeature pfeature in plistContains)
                                    {
                                        if (!rowBuffer.get_Value(sourceOID).ToString().Contains(pfeature.OID.ToString()))
                                        {
                                            string oids = rowBuffer.get_Value(sourceOID).ToString() + ";" + pfeature.OID.ToString();
                                            rowBuffer.set_Value(sourceOID, oids);
                                        }
                                    }

                                }

                            }
                        }
                        else
                        {
                            rowBuffer.set_Value(changeIndex, ClsConstant.One2Zero);
                        }
                    }
                    //源图层一个要素对应多个目标图层要素（1对多关系）                        
                    else if (featureCount > 1)
                    {
                        revListMore2More = new List<IFeature>();
                        revListMore2One = new Dictionary<IFeature, IFeature>();
                        revList = new List<IFeature>();

                        tarFeatCursor = targetFeatureClass.Search(spatialFilter, false);
                        tarFeature = tarFeatCursor.NextFeature();

                        while (tarFeature != null)
                        {
                            revList.Add(tarFeature);
                            //TODO :反向查询：根据待匹配图层查询源图层
                            revSpatialFilter = new SpatialFilterClass();
                            revSpatialFilter.Geometry = tarFeature.Shape;
                            revSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                            revSourFeatCount = sourceFeatureClass.FeatureCount(revSpatialFilter);

                            revSourceFeatCursor = sourceFeatureClass.Search(revSpatialFilter, false);
                            revSourceFeature = revSourceFeatCursor.NextFeature();

                            //反向查询也是一对一的关系
                            if (revSourFeatCount == 1)
                            {
                                if (ClsIndicatorFunStatic.AreaRatio(revSourceFeature, tarFeature) > minArea)
                                {
                                    if (!revListMore2One.ContainsValue(revSourceFeature))
                                    {
                                        revListMore2One.Add(tarFeature, revSourceFeature);
                                    }
                                }
                            }
                            else
                            {

                                while (revSourceFeature != null)
                                {
                                    //TODO:20180409要不要比较要素
                                    //if (ClsIndicatorFunStatic.AreaRatio(revSourceFeature, tarFeature) > minArea)
                                    if (ClsIndicatorFunStatic.AreaRatio(revSourceFeature, tarFeature) > minArea
                                        && revSourceFeature.OID == srcFeature.OID)
                                    {
                                        if (!revListMore2More.Contains(revSourceFeature))
                                        {
                                            revListMore2More.Add(revSourceFeature);
                                        }
                                    }
                                    revSourceFeature = revSourceFeatCursor.NextFeature();
                                }

                            }

                            tarFeature = tarFeatCursor.NextFeature();
                        }
                        //一对0
                        if (revListMore2One.Count == 0 && revListMore2More.Count == 0)
                        {
                            rowBuffer.set_Value(changeIndex, ClsConstant.One2Zero);
                            //SetFcode(srcFeature, rowBuffer, sourceFcode, tarFeature, targetFcode);
                        }
                        //一对一
                        if (revListMore2One.Count == 1 && revListMore2More.Count == 0)
                        {
                            rowBuffer.set_Value(changeIndex, ClsConstant.One2One);
                            IFeature tempFeature = null;
                            foreach (IFeature key in revListMore2One.Keys)
                            {
                                tempFeature = key;
                            }
                            rowBuffer.set_Value(targetOID, tempFeature.OID);
                            SetFcode(srcFeature, rowBuffer, sourceFcode, tempFeature, targetFcode);
                        }
                        //一对多
                        if (revListMore2One.Count > 1 && revListMore2More.Count == 0)
                        {
                            rowBuffer.set_Value(changeIndex, ClsConstant.One2More);
                            foreach (IFeature pfeature in revList)
                            {
                                if (rowBuffer.get_Value(targetOID).ToString() != "")
                                {
                                    string oids = rowBuffer.get_Value(targetOID).ToString() + ";" + pfeature.OID.ToString();
                                    rowBuffer.set_Value(targetOID, oids);
                                }
                            }

                        }
                        //多对多
                        if (revListMore2More.Count > 0)
                        {
                            rowBuffer.set_Value(changeIndex, ClsConstant.More2More);
                            foreach (IFeature pfeature in revList)
                            {
                                string oids = null;
                                if (rowBuffer.get_Value(targetOID).ToString() == "")
                                {
                                    oids = pfeature.OID.ToString();
                                }
                                else
                                {
                                    oids = rowBuffer.get_Value(targetOID).ToString() + ";" + pfeature.OID.ToString();
                                }
                                rowBuffer.set_Value(targetOID, oids);
                            }
                            foreach (IFeature pfeature in revListMore2More)
                            {
                                if (!rowBuffer.get_Value(sourceOID).ToString().Contains(pfeature.OID.ToString()))
                                {
                                    string oids = rowBuffer.get_Value(sourceOID).ToString() + ";" + pfeature.OID.ToString();
                                    rowBuffer.set_Value(sourceOID, oids);
                                }
                            }
                        }
                    }
                    rowCursor.InsertRow(rowBuffer);
                    stateLabel.Text = "图层" + sourceFeatureClass.AliasName + "已匹配到" + prgMain.Value.ToString() + "(" + srcFeatCount.ToString() + ")" + "个要素";
                    stateLabel.Refresh();
                    srcFeature = srcFeatCursor.NextFeature();
                }
                workspaceEdit.StopEditOperation();
                workspaceEdit.StopEditing(true);
                stateLabel.Text = "";
                prgMain.Value = 0;
                prgSub.Value = 0;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
            }
            return true;

        }
        /// <summary>
        /// 跨尺度匹配要素--算法核心-填充xxx_DifPyTable（面状）
        /// </summary>
        /// <param name="sourceFeatureClass">源图层</param>
        /// <param name="targetFeatureClass">待匹配更新的图层</param>
        /// <param name="resultTable">匹配结果表</param>
        /// <param name="buffer">缓冲区距离</param>
        /// <param name="minArea">缓冲区面积</param>
        /// <param name="prgMain">总进度条</param>
        /// <param name="prgSub">当前进度条</param>
        /// <param name="stateLabel">状体提示信息</param>
        /// <returns>返回类型，布尔型</returns>
        public bool SearchChangedPolygonFeaturesArea2(IFeatureClass sourceFeatureClass, IFeatureClass targetFeatureClass,
            ITable resultTable, double buffer, double minArea, ProgressBar prgMain, ProgressBar prgSub, LabelX stateLabel)
        {
            //源图层的要素个数
            int srcFeatCount = 0;
            //源图层的游标
            IFeatureCursor srcFeatCursor = null;
            //源图层通过游标遍历的下一个feature
            IFeature srcFeature = null;

            //根据特定的控件过滤条件，源图层查询到的待匹配图层的游标
            IFeatureCursor tarFeatCursor = null;
            IFeatureCursor revSourceFeatCursor = null;

            //teFeat是根据游标查询出的待匹配的要素
            IFeature tarFeature = null;
            IFeature revSourceFeature = null;


            //记录每个源图层要素查询得到的待匹配图层的要素个数
            int featureCount = 0;
            //记录由待匹配的图层反向查询源图层获得的源图层的个数
            int revSourFeatCount = 0;


            //ISpatialFilter用于返回和修改filter使用的空间关系。
            ISpatialFilter spatialFilter;

            //反向查询：根据待匹配图层查询源图层
            //反向查询源图层要素
            ISpatialFilter revSpatialFilter;
            //反向查询源图层游标，也就是源图层游标

            //记录反向查询过要素
            //记录待匹配图层要素包含源图层要素时，把包含的源图层要素添加到集合中
            Collection<int> revCol = new Collection<int>();

            List<IFeature> revListMore2More = null;
            Dictionary<IFeature, IFeature> revListMore2One = null;
            List<IFeature> revList = null;

            List<IFeature> revListOne2One = null;
            List<IFeature> revListOne2More = null;

            //记录查询过要素
            Collection<int> targetCol = new Collection<int>();

            try
            {
                IDataset dataset = resultTable as IDataset;
                IWorkspaceEdit workspaceEdit = dataset.Workspace as IWorkspaceEdit;
                workspaceEdit.StartEditing(true);
                workspaceEdit.StartEditOperation();
                //源图层的要素个数及游标
                srcFeatCount = sourceFeatureClass.FeatureCount(null);
                srcFeatCursor = sourceFeatureClass.Search(null, false);

                if (srcFeatCursor != null)
                {
                    srcFeature = srcFeatCursor.NextFeature();
                }
                IRowBuffer rowBuffer = null;
                ICursor rowCursor = null;

                //待匹配图层FIXOID（OID）字段的索引
                int fixoid = 0;

                //记录第一次为匹配的要素OID
                Collection<int> unmatchedCol = new Collection<int>();

                //遍历每一条源图层要素（Feature）
                while (srcFeature != null)
                {
                    rowBuffer = resultTable.CreateRowBuffer();
                    rowCursor = resultTable.Insert(true);

                    int sourceOID = rowBuffer.Fields.FindField("源OID");
                    int targetOID = rowBuffer.Fields.FindField("待匹配OID");

                    int sourceFcode = rowBuffer.Fields.FindField("源要素分类码");
                    int targetFcode = rowBuffer.Fields.FindField("待匹配要素分类码");

                    //rowBuffer.set_Value(indexFID, tuFeat.OID);
                    int changeIndex = rowBuffer.Fields.FindField("变化标记");

                    rowBuffer.set_Value(sourceOID, srcFeature.OID);

                    //查看进度
                    ClsStatic.AboutProcessBar(srcFeatCount, prgMain, prgSub);

                    spatialFilter = new SpatialFilterClass();
                    spatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                    spatialFilter.Geometry = srcFeature.Shape;
                    featureCount = targetFeatureClass.FeatureCount(spatialFilter);
                    //源图层一个要素对应0个目标图层要素（1对0关系）                        
                    if (featureCount == 0)
                    {
                        rowBuffer.set_Value(changeIndex, ClsConstant.One2Zero);
                    }
                    //源图层一个要素对应1个目标图层要素（一对一关系）
                    else if (featureCount == 1)
                    {
                        revListOne2More = new List<IFeature>();
                        revListOne2One = new List<IFeature>();

                        //teFeatCursor是根据特定的控件过滤条件，源图层查询到的待匹配图层的游标
                        tarFeatCursor = targetFeatureClass.Search(spatialFilter, false);
                        tarFeature = tarFeatCursor.NextFeature();

                        //TODO :反向查询：根据待匹配图层查询源图层
                        revSpatialFilter = new SpatialFilterClass();
                        revSpatialFilter.Geometry = tarFeature.Shape;
                        revSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                        revSourFeatCount = sourceFeatureClass.FeatureCount(revSpatialFilter);
                        revSourceFeatCursor = sourceFeatureClass.Search(revSpatialFilter, false);
                        revSourceFeature = revSourceFeatCursor.NextFeature();
                        //反向查询也是一对一的关系
                        if (revSourFeatCount == 1)
                        {
                            rowBuffer.set_Value(targetOID, tarFeature.OID);
                            rowBuffer.set_Value(changeIndex, ClsConstant.One2One);
                            SetFcode(srcFeature, rowBuffer, sourceFcode, tarFeature, targetFcode);
                            //revListOne2One.Add(revSourceFeature);
                        }
                        //反向查询是一对多的关系
                        else
                        {
                            //Dictionary<IFeature, IFeature> pDicOverlap = new Dictionary<IFeature, IFeature>();
                            List<IFeature> pDicOverlap = new List<IFeature>();
                            List<IFeature> plistContains = new List<IFeature>();

                            IRelationalOperator tarFeatureRelation = tarFeature.Shape as IRelationalOperator;
                            IArea targetArea = tarFeature.Shape as IArea;
                            ITopologicalOperator tarTopo = tarFeature.Shape as ITopologicalOperator;
                            IArea teArea = tarFeature.Shape as IArea;

                            while (revSourceFeature != null)
                            {
                                IGeometry revGeometry = revSourceFeature.Shape as IGeometry;
                                //首先判断两个几何图形是否是重叠关系：也就是待匹配图层是否与源图层重叠
                                //如果有重叠，那么进一步判断重叠面积的比值
                                if (tarFeatureRelation.Overlaps(revGeometry))
                                {
                                    IArea revArea = revSourceFeature.Shape as IArea;
                                    IGeometry insertGeo = tarTopo.Intersect(revGeometry, esriGeometryDimension.esriGeometry2Dimension);
                                    IArea insertArea = (IArea)insertGeo;

                                    //源图层和待匹配图层的交集面积与源图层面积的比值
                                    //如果比值小于0.1，那么久认为二者不匹配
                                    double d = insertArea.Area / revArea.Area;
                                    //double d = insertArea.Area / targetArea.Area;
                                    if (d > minArea)
                                    {
                                        //TODO:添加的判断oid,对结果影响很大
                                        //if (revSourceFeature.OID == srcFeature.OID)
                                        //{

                                        //源图层要素重心是否落入待匹配图层中
                                        if (ClsIndicatorFunStatic.PolygonContainsPoint(revSourceFeature, tarFeature))
                                        {
                                            if (!pDicOverlap.Contains(revSourceFeature))
                                            {
                                                pDicOverlap.Add(revSourceFeature);
                                            }
                                        }

                                        //}
                                    }
                                }
                                //其次判断两个几何图形是否是包含关系：也就是待匹配图层是否包含源图层，如果包含
                                else if (tarFeatureRelation.Contains(revGeometry))
                                {
                                    if (!plistContains.Contains(revSourceFeature))
                                    {
                                        plistContains.Add(revSourceFeature);
                                    }
                                }
                                revSourceFeature = revSourceFeatCursor.NextFeature();
                            }
                            //未匹配
                            if (pDicOverlap.Count == 0 && plistContains.Count == 0)
                            {
                                rowBuffer.set_Value(changeIndex, ClsConstant.One2Zero);
                            }
                            //TODO:更改
                            //一对一
                            //有1个待匹配图层与源图层是重叠关系并且不是包含关系
                            //（重叠关系下面积比值都大于0.1）
                            if ((pDicOverlap.Count == 1 && plistContains.Count == 0) ||
                                (pDicOverlap.Count == 0 && plistContains.Count == 1))
                            {

                                SetFcode(srcFeature, rowBuffer, sourceFcode, tarFeature, targetFcode);
                                rowBuffer.set_Value(changeIndex, ClsConstant.One2One);
                                rowBuffer.set_Value(targetOID, tarFeature.OID);
                            }
                            //多对一
                            //有1个待匹配图层与源图层是重叠关系并且是包含关系,
                            //（重叠关系下面积比值都大于0.1）
                            else
                            {
                                rowBuffer.set_Value(changeIndex, ClsConstant.More2One);
                                rowBuffer.set_Value(targetOID, tarFeature.OID);

                                foreach (IFeature pfeature in pDicOverlap)
                                {
                                    if (!rowBuffer.get_Value(sourceOID).ToString().Contains(pfeature.OID.ToString()))
                                    {
                                        string oids = rowBuffer.get_Value(sourceOID).ToString() + ";" + pfeature.OID.ToString();
                                        rowBuffer.set_Value(sourceOID, oids);
                                    }
                                }

                                foreach (IFeature pfeature in plistContains)
                                {
                                    if (!rowBuffer.get_Value(sourceOID).ToString().Contains(pfeature.OID.ToString()))
                                    {
                                        string oids = rowBuffer.get_Value(sourceOID).ToString() + ";" + pfeature.OID.ToString();
                                        rowBuffer.set_Value(sourceOID, oids);
                                    }
                                }

                            }

                        }
                    }
                    //源图层一个要素对应多个目标图层要素（1对多关系）                        
                    else if (featureCount > 1)
                    {
                        revListMore2More = new List<IFeature>();
                        revListMore2One = new Dictionary<IFeature, IFeature>();
                        revList = new List<IFeature>();

                        tarFeatCursor = targetFeatureClass.Search(spatialFilter, false);
                        tarFeature = tarFeatCursor.NextFeature();

                        while (tarFeature != null)
                        {
                            revList.Add(tarFeature);
                            //TODO :反向查询：根据待匹配图层查询源图层
                            revSpatialFilter = new SpatialFilterClass();
                            revSpatialFilter.Geometry = tarFeature.Shape;
                            revSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                            revSourFeatCount = sourceFeatureClass.FeatureCount(revSpatialFilter);

                            revSourceFeatCursor = sourceFeatureClass.Search(revSpatialFilter, false);
                            revSourceFeature = revSourceFeatCursor.NextFeature();

                            //反向查询也是一对一的关系
                            if (revSourFeatCount == 1)
                            {
                                if (ClsIndicatorFunStatic.AreaRatio(revSourceFeature, tarFeature) > minArea)
                                {
                                    if (!revListMore2One.ContainsValue(revSourceFeature))
                                    {
                                        revListMore2One.Add(tarFeature, revSourceFeature);
                                    }
                                }
                            }
                            else
                            {

                                while (revSourceFeature != null)
                                {
                                    //TODO:20180409要不要比较要素
                                    //if (ClsIndicatorFunStatic.AreaRatio(revSourceFeature, tarFeature) > minArea)
                                    if (ClsIndicatorFunStatic.AreaRatio(revSourceFeature, tarFeature) > minArea
                                        && revSourceFeature.OID == srcFeature.OID)
                                    {
                                        if (!revListMore2More.Contains(revSourceFeature))
                                        {
                                            revListMore2More.Add(revSourceFeature);
                                        }
                                    }
                                    revSourceFeature = revSourceFeatCursor.NextFeature();
                                }

                            }

                            tarFeature = tarFeatCursor.NextFeature();
                        }
                        //一对0
                        if (revListMore2One.Count == 0 && revListMore2More.Count == 0)
                        {
                            rowBuffer.set_Value(changeIndex, ClsConstant.One2Zero);
                            //SetFcode(srcFeature, rowBuffer, sourceFcode, tarFeature, targetFcode);
                        }
                        //一对一
                        if (revListMore2One.Count == 1 && revListMore2More.Count == 0)
                        {
                            rowBuffer.set_Value(changeIndex, ClsConstant.One2One);
                            IFeature tempFeature = null;
                            foreach (IFeature key in revListMore2One.Keys)
                            {
                                tempFeature = key;
                            }
                            rowBuffer.set_Value(targetOID, tempFeature.OID);
                            SetFcode(srcFeature, rowBuffer, sourceFcode, tempFeature, targetFcode);
                        }
                        //一对多
                        if (revListMore2One.Count > 1 && revListMore2More.Count == 0)
                        {
                            rowBuffer.set_Value(changeIndex, ClsConstant.One2More);
                            foreach (IFeature pfeature in revList)
                            {
                                if (rowBuffer.get_Value(targetOID).ToString() != "")
                                {
                                    string oids = rowBuffer.get_Value(targetOID).ToString() + ";" + pfeature.OID.ToString();
                                    rowBuffer.set_Value(targetOID, oids);
                                }
                            }

                        }
                        //多对多
                        if (revListMore2More.Count > 0)
                        {
                            rowBuffer.set_Value(changeIndex, ClsConstant.More2More);
                            foreach (IFeature pfeature in revList)
                            {
                                string oids = null;
                                if (rowBuffer.get_Value(targetOID).ToString() == "")
                                {
                                    oids = pfeature.OID.ToString();
                                }
                                else
                                {
                                    oids = rowBuffer.get_Value(targetOID).ToString() + ";" + pfeature.OID.ToString();
                                }
                                rowBuffer.set_Value(targetOID, oids);
                            }
                            foreach (IFeature pfeature in revListMore2More)
                            {
                                if (!rowBuffer.get_Value(sourceOID).ToString().Contains(pfeature.OID.ToString()))
                                {
                                    string oids = rowBuffer.get_Value(sourceOID).ToString() + ";" + pfeature.OID.ToString();
                                    rowBuffer.set_Value(sourceOID, oids);
                                }
                            }
                        }
                    }
                    rowCursor.InsertRow(rowBuffer);
                    stateLabel.Text = "图层" + sourceFeatureClass.AliasName + "已匹配到" + prgMain.Value.ToString() + "(" + srcFeatCount.ToString() + ")" + "个要素";
                    stateLabel.Refresh();
                    srcFeature = srcFeatCursor.NextFeature();
                }
                workspaceEdit.StopEditOperation();
                workspaceEdit.StopEditing(true);
                stateLabel.Text = "";
                prgMain.Value = 0;
                prgSub.Value = 0;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
            }
            return true;

        }
        /// <summary>
        /// 跨尺度匹配要素--算法核心-填充xxx_DifPyTable（面状）
        /// </summary>
        /// <param name="sourceFeatureClass">源图层</param>
        /// <param name="targetFeatureClass">待匹配更新的图层</param>
        /// <param name="resultTable">匹配结果表</param>
        /// <param name="buffer">缓冲区距离</param>
        /// <param name="minArea">缓冲区面积</param>
        /// <param name="prgMain">总进度条</param>
        /// <param name="prgSub">当前进度条</param>
        /// <param name="stateLabel">状体提示信息</param>
        /// <returns>返回类型，布尔型</returns>
        public bool SearchChangedPolygonFeaturesAreaDirec(IFeatureClass sourceFeatureClass, IFeatureClass targetFeatureClass,
            ITable resultTable, double buffer, double minArea, ProgressBar prgMain, ProgressBar prgSub, LabelX stateLabel)
        {
            //源图层的要素个数
            int srcFeatCount = 0;
            //源图层的游标
            IFeatureCursor srcFeatCursor = null;
            //源图层通过游标遍历的下一个feature
            IFeature srcFeature = null;

            //根据特定的控件过滤条件，源图层查询到的待匹配图层的游标
            IFeatureCursor tarFeatCursor = null;
            IFeatureCursor revSourceFeatCursor = null;

            //teFeat是根据游标查询出的待匹配的要素
            IFeature tarFeature = null;
            IFeature revSourceFeature = null;


            //记录每个源图层要素查询得到的待匹配图层的要素个数
            int featureCount = 0;
            //记录由待匹配的图层反向查询源图层获得的源图层的个数
            int revSourFeatCount = 0;


            //ISpatialFilter用于返回和修改filter使用的空间关系。
            ISpatialFilter spatialFilter;

            //反向查询：根据待匹配图层查询源图层
            //反向查询源图层要素
            ISpatialFilter revSpatialFilter;
            //反向查询源图层游标，也就是源图层游标

            //记录反向查询过要素
            //记录待匹配图层要素包含源图层要素时，把包含的源图层要素添加到集合中
            Collection<int> revCol = new Collection<int>();

            List<IFeature> revListMore2More = null;
            Dictionary<IFeature, IFeature> revListMore2One = null;
            List<IFeature> revList = null;

            List<IFeature> revListOne2One = null;
            List<IFeature> revListOne2More = null;

            //记录查询过要素
            Collection<int> targetCol = new Collection<int>();

            try
            {
                IDataset dataset = resultTable as IDataset;
                IWorkspaceEdit workspaceEdit = dataset.Workspace as IWorkspaceEdit;
                workspaceEdit.StartEditing(true);
                workspaceEdit.StartEditOperation();
                //源图层的要素个数及游标
                srcFeatCount = sourceFeatureClass.FeatureCount(null);
                srcFeatCursor = sourceFeatureClass.Search(null, false);

                if (srcFeatCursor != null)
                {
                    srcFeature = srcFeatCursor.NextFeature();
                }
                IRowBuffer rowBuffer = null;
                ICursor rowCursor = null;

                //待匹配图层FIXOID（OID）字段的索引
                int fixoid = 0;

                //记录第一次为匹配的要素OID
                Collection<int> unmatchedCol = new Collection<int>();

                //遍历每一条源图层要素（Feature）
                while (srcFeature != null)
                {
                    rowBuffer = resultTable.CreateRowBuffer();
                    rowCursor = resultTable.Insert(true);

                    int sourceOID = rowBuffer.Fields.FindField("源OID");
                    int targetOID = rowBuffer.Fields.FindField("待匹配OID");

                    int sourceFcode = rowBuffer.Fields.FindField("源要素分类码");
                    int targetFcode = rowBuffer.Fields.FindField("待匹配要素分类码");

                    //rowBuffer.set_Value(indexFID, tuFeat.OID);
                    int changeIndex = rowBuffer.Fields.FindField("变化标记");

                    rowBuffer.set_Value(sourceOID, srcFeature.OID);

                    //查看进度
                    ClsStatic.AboutProcessBar(srcFeatCount, prgMain, prgSub);

                    spatialFilter = new SpatialFilterClass();
                    spatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                    spatialFilter.Geometry = srcFeature.Shape;
                    featureCount = targetFeatureClass.FeatureCount(spatialFilter);
                    //源图层一个要素对应0个目标图层要素（1对0关系）                        
                    if (featureCount == 0)
                    {
                        rowBuffer.set_Value(changeIndex, ClsConstant.One2Zero);
                    }
                    //源图层一个要素对应1个目标图层要素（一对一关系）
                    else if (featureCount == 1)
                    {
                        revListOne2More = new List<IFeature>();
                        revListOne2One = new List<IFeature>();

                        //teFeatCursor是根据特定的控件过滤条件，源图层查询到的待匹配图层的游标
                        tarFeatCursor = targetFeatureClass.Search(spatialFilter, false);
                        tarFeature = tarFeatCursor.NextFeature();

                        //TODO :反向查询：根据待匹配图层查询源图层
                        revSpatialFilter = new SpatialFilterClass();
                        revSpatialFilter.Geometry = tarFeature.Shape;
                        revSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                        revSourFeatCount = sourceFeatureClass.FeatureCount(revSpatialFilter);
                        revSourceFeatCursor = sourceFeatureClass.Search(revSpatialFilter, false);
                        revSourceFeature = revSourceFeatCursor.NextFeature();
                        //反向查询也是一对一的关系
                        if (revSourFeatCount == 1)
                        {
                            rowBuffer.set_Value(targetOID, tarFeature.OID);
                            rowBuffer.set_Value(changeIndex, ClsConstant.One2One);
                            SetFcode(srcFeature, rowBuffer, sourceFcode, tarFeature, targetFcode);
                            //revListOne2One.Add(revSourceFeature);
                        }
                        //反向查询是一对多的关系
                        else
                        {
                            Dictionary<IFeature, IFeature> pDicOverlap = new Dictionary<IFeature, IFeature>();
                            List<IFeature> plistContains = new List<IFeature>();

                            IRelationalOperator tarFeatureRelation = tarFeature.Shape as IRelationalOperator;
                            IArea targetArea = tarFeature.Shape as IArea;
                            ITopologicalOperator tarTopo = tarFeature.Shape as ITopologicalOperator;
                            IArea teArea = tarFeature.Shape as IArea;

                            while (revSourceFeature != null)
                            {
                                IGeometry revGeometry = revSourceFeature.Shape as IGeometry;
                                //首先判断两个几何图形是否是重叠关系：也就是待匹配图层是否与源图层重叠
                                //如果有重叠，那么进一步判断重叠面积的比值
                                if (tarFeatureRelation.Overlaps(revGeometry))
                                {
                                    IArea revArea = revSourceFeature.Shape as IArea;
                                    IGeometry insertGeo = tarTopo.Intersect(revGeometry, esriGeometryDimension.esriGeometry2Dimension);
                                    IArea insertArea = (IArea)insertGeo;

                                    //源图层和待匹配图层的交集面积与源图层面积的比值
                                    //如果比值小于0.1，那么久认为二者不匹配
                                    double d = insertArea.Area / revArea.Area;
                                    //double d = insertArea.Area / targetArea.Area;
                                    if (d > minArea)
                                    {
                                        //TODO:添加的判断oid,对结果影响很大
                                        //if (revSourceFeature.OID == srcFeature.OID)
                                        //{

                                        ////源图层要素重心是否落入待匹配图层中
                                        //if (ClsIndicatorFunStatic.PolygonContainsPoint(revSourceFeature, tarFeature))
                                        //{
                                        if (!pDicOverlap.ContainsKey(revSourceFeature))
                                        {
                                            pDicOverlap.Add(revSourceFeature, srcFeature);
                                        }
                                        //}

                                        //}
                                    }
                                }
                                //其次判断两个几何图形是否是包含关系：也就是待匹配图层是否包含源图层，如果包含
                                else if (tarFeatureRelation.Contains(revGeometry))
                                {
                                    if (!plistContains.Contains(revSourceFeature))
                                    {
                                        plistContains.Add(revSourceFeature);
                                    }
                                }
                                revSourceFeature = revSourceFeatCursor.NextFeature();
                            }
                            //未匹配
                            if (pDicOverlap.Count == 0 && plistContains.Count == 0)
                            {
                                rowBuffer.set_Value(changeIndex, ClsConstant.One2Zero);
                            }
                            //TODO:更改
                            //一对一
                            //有1个待匹配图层与源图层是重叠关系并且不是包含关系
                            //（重叠关系下面积比值都大于0.1）
                            if (pDicOverlap.Count == 1 && plistContains.Count == 0)
                            {
                                if (pDicOverlap.FirstOrDefault().Key.OID == pDicOverlap.FirstOrDefault().Key.OID)
                                {
                                    SetFcode(srcFeature, rowBuffer, sourceFcode, tarFeature, targetFcode);
                                    rowBuffer.set_Value(changeIndex, ClsConstant.One2One);
                                    rowBuffer.set_Value(targetOID, tarFeature.OID);
                                }
                                else
                                {
                                    rowBuffer.set_Value(changeIndex, ClsConstant.One2Zero);
                                }
                            }
                            if (pDicOverlap.Count == 0 && plistContains.Count == 1)
                            {
                                SetFcode(srcFeature, rowBuffer, sourceFcode, tarFeature, targetFcode);

                                rowBuffer.set_Value(changeIndex, ClsConstant.One2One);

                                rowBuffer.set_Value(targetOID, tarFeature.OID);
                            }

                            //多对一
                            //有1个待匹配图层与源图层是重叠关系并且是包含关系,
                            //（重叠关系下面积比值都大于0.1）
                            else
                            {
                                rowBuffer.set_Value(changeIndex, ClsConstant.More2One);
                                rowBuffer.set_Value(targetOID, tarFeature.OID);

                                foreach (IFeature pfeature in pDicOverlap.Keys)
                                {
                                    if (!rowBuffer.get_Value(sourceOID).ToString().Contains(pfeature.OID.ToString()))
                                    {
                                        string oids = rowBuffer.get_Value(sourceOID).ToString() + ";" + pfeature.OID.ToString();
                                        rowBuffer.set_Value(sourceOID, oids);
                                    }
                                }

                                foreach (IFeature pfeature in plistContains)
                                {
                                    if (!rowBuffer.get_Value(sourceOID).ToString().Contains(pfeature.OID.ToString()))
                                    {
                                        string oids = rowBuffer.get_Value(sourceOID).ToString() + ";" + pfeature.OID.ToString();
                                        rowBuffer.set_Value(sourceOID, oids);
                                    }
                                }

                            }

                        }
                    }
                    //源图层一个要素对应多个目标图层要素（1对多关系）                        
                    else if (featureCount > 1)
                    {
                        revListMore2More = new List<IFeature>();
                        revListMore2One = new Dictionary<IFeature, IFeature>();
                        revList = new List<IFeature>();

                        tarFeatCursor = targetFeatureClass.Search(spatialFilter, false);
                        tarFeature = tarFeatCursor.NextFeature();

                        while (tarFeature != null)
                        {
                            revList.Add(tarFeature);
                            //TODO :反向查询：根据待匹配图层查询源图层
                            revSpatialFilter = new SpatialFilterClass();
                            revSpatialFilter.Geometry = tarFeature.Shape;
                            revSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                            revSourFeatCount = sourceFeatureClass.FeatureCount(revSpatialFilter);

                            revSourceFeatCursor = sourceFeatureClass.Search(revSpatialFilter, false);
                            revSourceFeature = revSourceFeatCursor.NextFeature();

                            //反向查询也是一对一的关系
                            if (revSourFeatCount == 1)
                            {
                                if (ClsIndicatorFunStatic.AreaRatio(revSourceFeature, tarFeature) > minArea)
                                {
                                    if (!revListMore2One.ContainsValue(revSourceFeature))
                                    {
                                        revListMore2One.Add(tarFeature, revSourceFeature);
                                    }
                                }
                            }
                            else
                            {

                                while (revSourceFeature != null)
                                {
                                    //TODO:20180409要不要比较要素
                                    //if (ClsIndicatorFunStatic.AreaRatio(revSourceFeature, tarFeature) > minArea)
                                    if (ClsIndicatorFunStatic.AreaRatio(revSourceFeature, tarFeature) > minArea
                                        && revSourceFeature.OID == srcFeature.OID)
                                    {
                                        if (!revListMore2More.Contains(revSourceFeature))
                                        {
                                            revListMore2More.Add(revSourceFeature);
                                        }
                                    }
                                    revSourceFeature = revSourceFeatCursor.NextFeature();
                                }

                            }

                            tarFeature = tarFeatCursor.NextFeature();
                        }
                        //一对0
                        if (revListMore2One.Count == 0 && revListMore2More.Count == 0)
                        {
                            rowBuffer.set_Value(changeIndex, ClsConstant.One2Zero);
                            //SetFcode(srcFeature, rowBuffer, sourceFcode, tarFeature, targetFcode);
                        }
                        //一对一
                        if (revListMore2One.Count == 1 && revListMore2More.Count == 0)
                        {
                            rowBuffer.set_Value(changeIndex, ClsConstant.One2One);
                            IFeature tempFeature = null;
                            foreach (IFeature key in revListMore2One.Keys)
                            {
                                tempFeature = key;
                            }
                            rowBuffer.set_Value(targetOID, tempFeature.OID);
                            SetFcode(srcFeature, rowBuffer, sourceFcode, tempFeature, targetFcode);
                        }
                        //一对多
                        if (revListMore2One.Count > 1 && revListMore2More.Count == 0)
                        {
                            rowBuffer.set_Value(changeIndex, ClsConstant.One2More);
                            foreach (IFeature pfeature in revList)
                            {
                                if (rowBuffer.get_Value(targetOID).ToString() != "")
                                {
                                    string oids = rowBuffer.get_Value(targetOID).ToString() + ";" + pfeature.OID.ToString();
                                    rowBuffer.set_Value(targetOID, oids);
                                }
                            }

                        }
                        //多对多
                        if (revListMore2More.Count > 0)
                        {
                            rowBuffer.set_Value(changeIndex, ClsConstant.More2More);
                            foreach (IFeature pfeature in revList)
                            {
                                string oids = null;
                                if (rowBuffer.get_Value(targetOID).ToString() == "")
                                {
                                    oids = pfeature.OID.ToString();
                                }
                                else
                                {
                                    oids = rowBuffer.get_Value(targetOID).ToString() + ";" + pfeature.OID.ToString();
                                }
                                rowBuffer.set_Value(targetOID, oids);
                            }
                            foreach (IFeature pfeature in revListMore2More)
                            {
                                if (!rowBuffer.get_Value(sourceOID).ToString().Contains(pfeature.OID.ToString()))
                                {
                                    string oids = rowBuffer.get_Value(sourceOID).ToString() + ";" + pfeature.OID.ToString();
                                    rowBuffer.set_Value(sourceOID, oids);
                                }
                            }
                        }
                    }
                    rowCursor.InsertRow(rowBuffer);
                    stateLabel.Text = "图层" + sourceFeatureClass.AliasName + "已匹配到" + prgMain.Value.ToString() + "(" + srcFeatCount.ToString() + ")" + "个要素";
                    stateLabel.Refresh();
                    srcFeature = srcFeatCursor.NextFeature();
                }
                workspaceEdit.StopEditOperation();
                workspaceEdit.StopEditing(true);
                stateLabel.Text = "";
                prgMain.Value = 0;
                prgSub.Value = 0;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
            }
            return true;

        }
        /// <summary>
        /// 设置分类码
        /// </summary>
        /// <param name="srcFeature"></param>
        /// <param name="rowBuffer"></param>
        /// <param name="sourceFcode"></param>
        /// <param name="tarFeature"></param>
        /// <param name="targetFcode"></param>
        private static void SetFcode(IFeature srcFeature, IRowBuffer rowBuffer, int sourceFcode, IFeature tarFeature, int targetFcode)
        {
            //插入分类码
            if (srcFeature != null && tarFeature != null)
            {
                rowBuffer.set_Value(sourceFcode, srcFeature.get_Value(srcFeature.Fields.FindField("FCODE")));
                rowBuffer.set_Value(targetFcode, tarFeature.get_Value(tarFeature.Fields.FindField("FCODE")));
            }
            else if (srcFeature != null)
            {
                rowBuffer.set_Value(sourceFcode, srcFeature.get_Value(srcFeature.Fields.FindField("FCODE")));
            }
            else if (tarFeature != null)
            {
                rowBuffer.set_Value(targetFcode, tarFeature.get_Value(tarFeature.Fields.FindField("FCODE")));
            }
        }
    }
}
