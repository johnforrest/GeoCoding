using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using Microsoft.VisualBasic;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using ESRI.ArcGIS.esriSystem;
using ZJGISCommon.Classes;
using ZJGISDataUpdating.Class;


namespace ZJGISDataUpdating
{
    class ClsCoreUpdateFun
    {
        private IWorkspaceEdit m_pWorkSpaceEdit;
        private IDataset m_pdataset;

        public bool ExtractDataToTEByTU(IFeatureClass pTUFeatCls, IFeatureClass pCFeatCls, IFeatureClass pTEFeatCls, ProgressBar proMain, ProgressBar proSub, LabelX stateLabel)
        {
            try
            {
                IDataset dataset = pTUFeatCls as IDataset;
                IGeoDataset geoDataset = dataset as IGeoDataset;
                IGeometry geometry = geoDataset.Extent as IGeometry;

                ISpatialFilter spatialFilter = new SpatialFilterClass();
                spatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                spatialFilter.Geometry = geometry;

                ISelectionSet selectionSet = pCFeatCls.Select(spatialFilter, esriSelectionType.esriSelectionTypeIDSet, esriSelectionOption.esriSelectionOptionNormal, null);

                ////将选择集提取到TE层
                ////提取
                IDataset pDataset = pTEFeatCls as IDataset;
                if (pDataset == null)
                    return false;
                IWorkspace tempOutWorkspace = pDataset.Workspace;
                LoadFeats(selectionSet, ref tempOutWorkspace, ref pTEFeatCls, proMain, proSub, stateLabel);
                return true;
            }
            catch (Exception)
            {

                throw;
            }

        }
        //把选择的要素写入到目标层
        public void LoadFeats(ISelectionSet pInSelSet, ref IWorkspace pOutWks, ref IFeatureClass pOutFeatCls, ProgressBar proMain, ProgressBar proSub, LabelX stateLabel)
        {
            try
            {
                ////得到传入的feature的游标
                IFeatureCursor pInfeatCursor;
                ICursor pCursor = null;

                IWorkspaceEdit pOutEditWs = pOutWks as IWorkspaceEdit;
                pOutEditWs.StartEditing(true);
                pOutEditWs.StartEditOperation();

                int i = 0;
                if (pInSelSet == null)
                    return;
                if (pInSelSet.Count == 0)
                    return;
                pInSelSet.Search(null, false, out pCursor);
                pInfeatCursor = pCursor as IFeatureCursor;
                IFeature pFeature = null;
                int featCount = pInSelSet.Count;

                if (pInfeatCursor != null)
                {
                    pFeature = pInfeatCursor.NextFeature();

                }
                string sFldName = null;
                ////数据源Field名
                int lInFldIndex = 0;
                int lOutFldIndex = 0;
                ////数据源和目标数据要素类字段索引

                IFields pFields;
                IFeatureClass pInFeatCls;
                pInFeatCls = pInSelSet.Target as IFeatureClass;
                pFields = pInFeatCls.Fields;

                IFeatureCursor pOutFeatCursor = null;
                IFeatureBuffer pOutFeatBuffer;
                int iFeatCount = 0;

                while (pFeature != null)
                {
                    pOutFeatBuffer = pOutFeatCls.CreateFeatureBuffer();
                    pOutFeatCursor = pOutFeatCls.Insert(true);

                    for (i = 0; i < pInFeatCls.Fields.FieldCount; i++)
                    {
                        sFldName = pInFeatCls.Fields.get_Field(i).Name;
                        if (sFldName != "FIXOID")
                        {
                            lInFldIndex = pFeature.Fields.FindField(sFldName);
                            lOutFldIndex = pOutFeatBuffer.Fields.FindField(sFldName);

                            if (pFeature.Fields.get_Field(lInFldIndex).Editable && lOutFldIndex >= 0 && (!Convert.IsDBNull(pFeature.get_Value(lInFldIndex))))
                            {
                                if (pOutFeatBuffer.Fields.get_Field(lOutFldIndex).Editable)
                                {
                                    pOutFeatBuffer.set_Value(lOutFldIndex, pFeature.get_Value(lInFldIndex));
                                }
                            }
                        }
                    }
                    //////记录现势库原始数据的OBJECTID
                    if (pFeature.Fields.FindField("OBJECTID") == -1)
                        lInFldIndex = pFeature.Fields.FindField("OBJECTID");
                    else
                        lInFldIndex = pFeature.Fields.FindField("OBJECTID");

                    lOutFldIndex = pOutFeatBuffer.Fields.FindField("FIXOID");
                    //string s = pFeature.get_Value(lInFldIndex).ToString();
                    pOutFeatBuffer.set_Value(lOutFldIndex, (int)pFeature.get_Value(lInFldIndex));

                    //////更新入库时要素的tagid置为4
                    //lOutFldIndex = pOutFeatBuffer.Fields.FindField("tagID");
                    //////-1 表示字段没找到
                    //if (lOutFldIndex > -1)
                    //{
                    //    pOutFeatBuffer.set_Value(lOutFldIndex, 4);
                    //}

                    if (pOutFeatBuffer.Shape != null && !pOutFeatBuffer.Shape.IsEmpty)
                    {
                        pOutFeatCursor.InsertFeature(pOutFeatBuffer);

                    }
                    //每500个要素就把缓冲区内的要素类写入目标层
                    if (iFeatCount >= 500)
                    {
                        pOutFeatCursor.Flush();
                        iFeatCount = 0;
                        //pOutEditWs.StopEditOperation();
                        //pOutEditWs.StopEditing(false);

                        //pOutEditWs.StartEditing(true);
                        //pOutEditWs.StartEditOperation();
                    }
                    sbProcessBar(featCount, proMain, proSub);

                    stateLabel.Text = "正在提取现势库中的数据到TE中，已经提取" + proMain.Value + "(/" + featCount + ")个要素。";
                    stateLabel.Refresh();

                    iFeatCount = iFeatCount + 1;
                    pFeature = pInfeatCursor.NextFeature();
                }
                if ((pOutFeatCursor != null))
                {
                    pOutFeatCursor.Flush();
                }
                if (pOutEditWs.IsBeingEdited())
                {
                    pOutEditWs.StopEditOperation();
                    pOutEditWs.StopEditing(true);

                    MessageBox.Show("提取数据成功", "提取", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    stateLabel.Text = "";
                    proMain.Value = 0;
                    proSub.Value = 0;
                }

                return;

            }
            catch (Exception)
            {

            }

        }
        #region 几何属性匹配
        /* 描述: 该函数用于在进行任意对象集成时识别出更新变化的要素
         * （任意对象集成只将新增空间对象和修改空间对象集成到数据库中，主要针对道路，水系，居民地（建构筑物）要素类，它们都有name字段作为空间对象标记）。
         * 通过将两个图层进行叠加分析来查找,首先遍历TU（pSrcFcls）图层中的每个要素通过空间自定义的关系在TE（pTarFcls）图层中查找相应的要素,
           如果找不到对应要素,则标记为4表示该要素为新增的空间对象， 
           如果找到一个要素看它们的图形和属性是否发生变化,如果图形属性都发生变化，则在TR图层中创建一个新要素，该要素获得TE图层中要素的FIXOID号，
         *                  获取TU图层中的图形属性，TAGID标记为2；
         *     只图形变化，则在TR图层中创建一个新要素，该要素获得TE图层中要素的FIXOID号及属性，获取TU图层中的图形，TAGID标记为5；
         *     只属性变化，则在TR图层中创建一个新要素，该要素获得TE图层中要素的FIXOID号及图形，获取TU图层中的属性，TAGID标记为6。 
        方法与步骤：
         * 遍历TU中的每个要素通过自定义关系查找，
         *      如果没找到则为新增；
         *      如果找到了几个要素，根据匹配属性项和几何测度求出同名实体，如果没有匹配实体则则标记为新增，找到几个要素标记为冲突要素，如果找到一个同名实体，
         *      则候选集中的其他要素标记为冲突要素；如果这些被标记为冲突的要素后来被替换更新，则取消冲突标记，冲突要素临时存放到内存中，
         *      最后这些标记的新要素都存放到TR层.
         *      图形变化为1,属性变化为2，图形属性均变化为3 新增要素为4，未变化为0， 要素冲突为5；
         */
        /// <summary>
        /// 几何匹配
        /// </summary>
        /// <param name="pSrcFcls">源图层</param>
        /// <param name="pTarFcls">待匹配图层</param>
        /// <param name="resultTable">保存结果的表TRA_PT_I_PtTable</param>
        /// <param name="matchedMode">匹配模式：设置匹配选择方式，0 代表几何匹配，1代表拓扑匹配，2代表属性匹配 </param>
        /// <param name="weight">表MatchedPointFCSetting中的（综合相似度阈值）</param>
        /// <param name="buffer">表MatchedPointFCSetting中的（搜索半径）字段值</param>
        /// <param name="fields">表MatchedPointFCSetting中的（匹配属性）字段值</param>
        /// <param name="prgMain">总进度条</param>
        /// <param name="prgSub">子进度条</param>
        /// <param name="stateLabel">状态信息</param>
        /// <returns></returns>
        public bool SearchChangedFeatures(IFeatureClass pSrcFcls, IFeatureClass pTarFcls, ITable resultTable, int matchedMode, double[] weight, double buffer, string fields, ProgressBar prgMain, ProgressBar prgSub, LabelX stateLabel)
        {
            //bool functionReturnValue = false;

            IFeatureCursor pSrcCursor = null; //源数据指针
            IFeatureCursor pTarCursor = null;//工作区数据指针

            IFeature pSrcFeature = null;
            IFeature pTarFeature = null;

            IRelationalOperator pRelOp;
            bool blnShpEqual = false;  //形状匹配
            bool blnAttriEqual = false;//属性匹配
            int i = 0;
            int intFieldIndex = -1;
            ISpatialFilter pSFilter;

            int sourceFeatCount = 0; //源图层要素个数
            int lFeatcount = 0;  //临时层要素个数
            int lIdx = 0;
            //找到的与源要素有关的目标要素
            Dictionary<int, IFeature> pDicCol;
            //Dictionary<int, IFeature> pDicChangeF = new Dictionary<int, IFeature>();

            int lOutFldIndex = 0;
            ////数据源和目标数据要素类字段索引
            string strSrcLyrName = "";
            string strTarLyrName = "";
            IDataset pDst;//临时变量，用于获取图层名
            int iFeatCount = 0; //处理的临时要素


            ////进入编辑状态
            //匹配表
            IDataset dataset = resultTable as IDataset;
            IWorkspace workspace = dataset.Workspace;
            IWorkspaceEdit workspaceEdit = workspace as IWorkspaceEdit;
            workspaceEdit.StartEditing(true);
            workspaceEdit.StartEditOperation();


            sourceFeatCount = pSrcFcls.FeatureCount(null);
            pSrcCursor = pSrcFcls.Search(null, false);

            //设置进度条
            if (sourceFeatCount > 0)
            {
                prgMain.Maximum = sourceFeatCount;
            }

            prgSub.Maximum = 100;
            prgSub.Value = 0;
            prgMain.Value = 0;

            if ((pSrcCursor != null))
            {
                pSrcFeature = pSrcCursor.NextFeature();
            }

            //指示属性值是否相等
            blnAttriEqual = true;
            //设置图层名
            pDst = pSrcFcls as IDataset; //原图层数据集
            strSrcLyrName = pDst.Name; //原图层名
            pDst = pTarFcls as IDataset;
            strTarLyrName = pDst.Name;//工作层名
            //pDst = pResultFcls as IDataset;
            //strResultLyrName = pDst.Name;
            pDst = null;

            IRowBuffer rowBuffer = null;
            ICursor rowCursor = null;
            //源要素总的循环
            try
            {
                while (pSrcFeature != null)
                {
                    rowBuffer = resultTable.CreateRowBuffer();
                    rowCursor = resultTable.Insert(true);

                    pDicCol = new Dictionary<int, IFeature>();

                    /**通过用代码构建缓冲区分析，来判断是否是同一个要素*/

                    pSFilter = new SpatialFilter();
                    if (pSrcFeature.Shape.GeometryType == esriGeometryType.esriGeometryPolyline)
                    {
                        ITopologicalOperator top = pSrcFeature.Shape as ITopologicalOperator;
                        pSFilter.Geometry = top.Buffer(buffer) as IGeometry;
                        //自定义空间查询,减少查询次数,提高查询速度
                        pSFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                    }
                    else if (pSrcFeature.Shape.GeometryType == esriGeometryType.esriGeometryPolygon)
                    {
                        pSFilter.Geometry = pSrcFeature.Shape;//查询边界形状为多边形
                        pSFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelRelation;//函数用法
                        pSFilter.SpatialRelDescription = "T********";
                    }
                    else if (pSrcFeature.Shape.GeometryType == esriGeometryType.esriGeometryPoint)
                    {
                        //创建缓冲区
                        ITopologicalOperator top = pSrcFeature.Shape as ITopologicalOperator;
                        pSFilter.Geometry = top.Buffer(buffer) as IGeometry;
                        //自定义空间查询,建立缓冲区
                        //设置缓冲区过滤条件：包含
                        pSFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;
                    }

                    //得到对应pSrcFeature的TE图层上的要素游标
                    //查询在原图层多边形边界处的工作层的元素
                    pTarCursor = pTarFcls.Search(pSFilter, false);
                    //得到对应pSrcFeature的TE图层上的要素个数
                    lFeatcount = pTarFcls.FeatureCount(pSFilter);
                    pTarFeature = pTarCursor.NextFeature();
                    lIdx = 0;
                    #region 算法说明
                    //*******************************************************************************
                    //算法说明:
                    //获取相交面的面积或相交线的长度.最后以P匹配参数来找出与pSrcFeature
                    //最匹配的pTarFeature,其中A表示pTarFeature的面积或长度,B表示pSrcFeature的面积或长度.
                    //当P值最大时,此时的pTarFeature即为找到的要素.
                    //*******************************************************************************
                    //目标次要素循环
                    #endregion

                    //设置表TRA_PT_I_PtTabl的（源OID）字段的值——cell[1]
                    rowBuffer.set_Value(rowBuffer.Fields.FindField("源OID"), pSrcFeature.OID);
                    while ((pTarFeature != null))
                    {
                        if ((pTarFeature.Shape != null))
                        {
                            if (pTarFeature.Shape.GeometryType == esriGeometryType.esriGeometryPolygon)
                            {

                                if (MatchCode(pSrcFeature, pTarFeature))
                                {
                                    rowBuffer.set_Value(4, pSrcFeature.get_Value(pSrcFeature.Fields.FindField("GCode")));
                                    rowBuffer.set_Value(5, pTarFeature.get_Value(pTarFeature.Fields.FindField("GCode")));
                                }
                         
                                double center = 0;
                                double area = 0;
                                double shape = 0;
                                double polygonRatio = 0;
                                int index = 0;
                                //判断匹配方法

                                shape = ClsIndicatorFunStatic.ShapeSimilarValue(pSrcFeature,pTarFeature);
                                area = ClsIndicatorFunStatic.AreaSimilarValue(pSrcFeature, pTarFeature);
                                center = ClsIndicatorFunStatic.CenterSimilarValue(pSrcFeature, pTarFeature);
                                polygonRatio = shape * weight[0] + area * weight[2] + center * weight[1];
                                string shape1 = string.Format("{0:0.00000000}", shape);
                                string area1 = string.Format("{0:0.00000000}", area);
                                string center1 = string.Format("{0:0.00000000}", center);
                                string polygonRatio1 = string.Format("{0:0.00000000}", polygonRatio);
                                rowBuffer.set_Value(6, center1);
                                rowBuffer.set_Value(7, area1);
                                rowBuffer.set_Value(8, shape1);
                                rowBuffer.set_Value(9, polygonRatio1);
                                if (polygonRatio > weight[3])
                                {
                                    if (rowBuffer.get_Value(2).ToString() == "")
                                    {
                                        rowBuffer.set_Value(2, pTarFeature.get_Value(index));
                                    }
                                    else
                                    {
                                        string oids = rowBuffer.get_Value(2).ToString() + ";" + pTarFeature.get_Value(index);
                                        rowBuffer.set_Value(2, oids);
                                    }
                                    pDicCol.Add(lIdx, pTarFeature);
                                    lIdx = lIdx + 1;
                                }
                            }
                            else if (pTarFeature.Shape.GeometryType == esriGeometryType.esriGeometryPolyline)
                            {
                                if (fields.Length > 0)
                                {
                                    string[] array = fields.Split(';');
                                    string test1 = array[0];

                                    for (int k = 0; k < array.Length; k++)
                                    {
                                        if (array[k].Length > 0)
                                        {
                                            //两个图层的名称字段名称不同
                                            if (array[k].Contains(':'))
                                            {
                                                string[] arr = ClsStatic.SplitStrColon(array[k]);
                                                string pSrcfield = pSrcFeature.get_Value(pSrcFeature.Fields.FindField(arr[0])).ToString();
                                                string pTarfield = pTarFeature.get_Value(pTarFeature.Fields.FindField(arr[1])).ToString();

                                                //if (StringSameOrNot(pSrcStr, pTarStr) > 0)
                                                if (pSrcfield.Length > 0 && pTarfield.Length > 0)
                                                {
                                                    //test
                                                    //int test0 = StringSameOrNot(pSrcStrName, pTarStrName);
                                                    //int test0 = StringSameOrNot2(pSrcfield, pTarfield);

                                                    string tempSrc = "";
                                                    string tempTar = "";
                                                    for (int j = 0; j < resultTable.Fields.FieldCount; j++)
                                                    {
                                                        if (resultTable.Fields.get_Field(j).AliasName == "源要素" + arr[0])
                                                        {
                                                            tempSrc = resultTable.Fields.get_Field(j).AliasName;
                                                        }
                                                        if (resultTable.Fields.get_Field(j).AliasName == "待匹配要素" + arr[1])
                                                        {
                                                            tempTar = resultTable.Fields.get_Field(j).AliasName;
                                                        }
                                                    }

                                                    //设置表TRA_PT_I_PtTabl的（待匹配编码）字段的值——cell[5]
                                                    //rowBuffer.set_Value(rowBuffer.Fields.FindField("源图层名称"), pSrcfield);
                                                    rowBuffer.set_Value(rowBuffer.Fields.FindField(tempSrc), pSrcfield);

                                                    double Similarity = ClsCosine.getSimilarity(pSrcfield, pTarfield);

                                                    //if (StringSameOrNot2(pSrcfield, pTarfield) > 2)
                                                    if (Similarity > 0.7)
                                                    {
                                                        //设置表TRA_PT_I_PtTabl的（待匹配编码）字段的值——cell[5]
                                                        //rowBuffer.set_Value(rowBuffer.Fields.FindField("待匹配图层名称"), pTarfield);
                                                        rowBuffer.set_Value(rowBuffer.Fields.FindField(tempTar), pTarfield);
                                                        //20170516注释掉
                                                        //if (MatchCode(pSrcFeature, pTarFeature))
                                                        //{
                                                        //    //设置表TRA_PT_I_PtTabl的（源编码）字段的值——cell[4]
                                                        //    rowBuffer.set_Value(4, pSrcFeature.get_Value(pSrcFeature.Fields.FindField("GCode")));
                                                        //    //设置表TRA_PT_I_PtTabl的（待匹配编码）字段的值——cell[5]
                                                        //    rowBuffer.set_Value(5, pTarFeature.get_Value(pTarFeature.Fields.FindField("GCode")));
                                                        //}

                                                     
                                                        //double matchedPoints = 0;
                                                        //double shape = 0;
                                                        //double polylineRadio = 0;

                                                        ////2010104注释掉
                                                        ////形状相似度
                                                        //shape = ClsIndicatorFunStatic.PolylineShapeSimilarValue(pSrcFeature, pTarFeature);
                                                        ////节点相似度
                                                        //matchedPoints = ClsIndicatorFunStatic.MatchedPointsSimilarValue(pSrcFeature, pTarFeature, buffer);
                                                        ////综合相似度
                                                        //polylineRadio = shape * weight[0] + matchedPoints * weight[1];

                                                        //string shape1 = string.Format("{0:0.00000000}", shape);
                                                        //string matchedPoints1 = string.Format("{0:0.00000000}", matchedPoints);
                                                        //string polygonRatio1 = string.Format("{0:0.00000000}", polylineRadio);

                                                        //rowBuffer.set_Value(rowBuffer.Fields.FindField("形状相似度"), shape1);
                                                        //rowBuffer.set_Value(rowBuffer.Fields.FindField("节点相似度"), matchedPoints1);
                                                        //rowBuffer.set_Value(rowBuffer.Fields.FindField("综合相似度"), polygonRatio1);


                                                        int index = 0;
                                                        //if (polylineRadio > weight[2])
                                                        //{
                                                        //如果两个点之间的距离小于设置的综合相似度
                                                        //if (distance < weight[0] && MatchCode(pSrcFeature, pTarFeature))
                                                        //if (MatchCode(pSrcFeature, pTarFeature))
                                                        //{
                                                        //设置表TRA_PT_I_PtTabl的（待匹配oid）字段的值为空——cell[2]
                                                        if (rowBuffer.get_Value(rowBuffer.Fields.FindField("待匹配OID")).ToString() == "")
                                                        {
                                                            //设置表TRA_PT_I_PtTabl的（待匹配oid）字段的值——cell[2]
                                                            rowBuffer.set_Value(rowBuffer.Fields.FindField("待匹配OID"), pTarFeature.get_Value(index));
                                                        }
                                                        else
                                                        {
                                                            string oids = rowBuffer.get_Value(rowBuffer.Fields.FindField("待匹配OID")).ToString() + ";" + pTarFeature.get_Value(index);
                                                            //设置表TRA_PT_I_PtTabl的（待匹配oid）字段的值——cell[2]
                                                            rowBuffer.set_Value(rowBuffer.Fields.FindField("待匹配OID"), oids);
                                                        }

                                                        //20170912
                                                        ////设置表TRA_PT_I_PtTabl的（待匹配编码）字段的值——cell[5]
                                                        //rowBuffer.set_Value(rowBuffer.Fields.FindField("源图层名称"), pSrcFeature.get_Value(pSrcFeature.Fields.FindFieldByAliasName(array[0])));
                                                        ////设置表TRA_PT_I_PtTabl的（待匹配编码）字段的值——cell[5]
                                                        //rowBuffer.set_Value(rowBuffer.Fields.FindField("待匹配图层名称"), pTarFeature.get_Value(pTarFeature.Fields.FindFieldByAliasName(array[0])));

                                                        if (!pDicCol.ContainsKey(lIdx))
                                                        {
                                                            pDicCol.Add(lIdx, pTarFeature);
                                                        }
                                                        lIdx = lIdx + 1;
                                                        //}
                                                    }
                                                    //}

                                                }
                                            }
                                            //两个图层的名称字段名称相同
                                            else
                                            {
                                                //string pSrcStrName = pSrcFeature.get_Value(pSrcFeature.Fields.FindFieldByAliasName("名称")).ToString();
                                                //string pSrcStrName = pSrcFeature.get_Value(pSrcFeature.Fields.FindField(ClsConfig.LayerConfigs[(pSrcFcls  as IDataset).Name].NameField)).ToString();
                                                //string pSrcStrName = pSrcFeature.get_Value(pSrcFeature.Fields.FindFieldByAliasName(array[0])).ToString();
                                                string pSrcfield = pSrcFeature.get_Value(pSrcFeature.Fields.FindField(array[k])).ToString();
                                                //string pTarStrName = pTarFeature.get_Value(pTarFeature.Fields.FindFieldByAliasName("名称")).ToString();
                                                //string pTarStrName = pTarFeature.get_Value(pTarFeature.Fields.FindField(ClsConfig.LayerConfigs[(pTarFcls as IDataset).Name].NameField)).ToString();
                                                //string pTarStrName = pTarFeature.get_Value(pTarFeature.Fields.FindFieldByAliasName(array[0])).ToString();
                                                string pTarfield = pTarFeature.get_Value(pTarFeature.Fields.FindField(array[k])).ToString();



                                                //if (StringSameOrNot(pSrcStr, pTarStr) > 0)
                                                if (pSrcfield.Length > 0 && pTarfield.Length > 0)
                                                {
                                                    //test
                                                    //int test0 = StringSameOrNot(pSrcStrName, pTarStrName);
                                                    int test0 = StringSameOrNot2(pSrcfield, pTarfield);

                                                    string tempSrc = "";
                                                    string tempTar = "";
                                                    for (int j = 0; j < resultTable.Fields.FieldCount; j++)
                                                    {
                                                        if (resultTable.Fields.get_Field(j).AliasName == "源要素" + array[k])
                                                        {
                                                            tempSrc = resultTable.Fields.get_Field(j).AliasName;
                                                        }
                                                        if (resultTable.Fields.get_Field(j).AliasName == "待匹配要素" + array[k])
                                                        {
                                                            tempTar = resultTable.Fields.get_Field(j).AliasName;
                                                        }
                                                    }

                                                    //设置表TRA_PT_I_PtTabl的（待匹配编码）字段的值——cell[5]
                                                    //rowBuffer.set_Value(rowBuffer.Fields.FindField("源图层名称"), pSrcfield);
                                                    rowBuffer.set_Value(rowBuffer.Fields.FindField(tempSrc), pSrcfield);
                                                    double Similarity = ClsCosine.getSimilarity(pSrcfield, pTarfield);

                                                    //if (StringSameOrNot2(pSrcfield, pTarfield) > 2)
                                                    //if (StringSameOrNot2(pSrcfield, pTarfield) > 2)
                                                    if (Similarity > 0.7)
                                                    {
                                                        //设置表TRA_PT_I_PtTabl的（待匹配编码）字段的值——cell[5]
                                                        //rowBuffer.set_Value(rowBuffer.Fields.FindField("待匹配图层名称"), pTarfield);
                                                        rowBuffer.set_Value(rowBuffer.Fields.FindField(tempTar), pTarfield);
                                                        //20170516注释掉
                                                        //if (MatchCode(pSrcFeature, pTarFeature))
                                                        //{
                                                        //    //设置表TRA_PT_I_PtTabl的（源编码）字段的值——cell[4]
                                                        //    rowBuffer.set_Value(4, pSrcFeature.get_Value(pSrcFeature.Fields.FindField("GCode")));
                                                        //    //设置表TRA_PT_I_PtTabl的（待匹配编码）字段的值——cell[5]
                                                        //    rowBuffer.set_Value(5, pTarFeature.get_Value(pTarFeature.Fields.FindField("GCode")));
                                                        //}

                                                        double matchedPoints = 0;
                                                        double shape = 0;
                                                        double polylineRadio = 0;


                                                        //20170518注释掉
                                                        //形状相似度
                                                        shape = ClsIndicatorFunStatic.PolylineShapeSimilarValue(pSrcFeature, pTarFeature);
                                                        //节点相似度
                                                        matchedPoints = ClsIndicatorFunStatic.MatchedPointsSimilarValue(pSrcFeature, pTarFeature, buffer);
                                                        //综合相似度
                                                        polylineRadio = shape * weight[0] + matchedPoints * weight[1];

                                                        string shape1 = string.Format("{0:0.00000000}", shape);
                                                        string matchedPoints1 = string.Format("{0:0.00000000}", matchedPoints);
                                                        string polygonRatio1 = string.Format("{0:0.00000000}", polylineRadio);

                                                        rowBuffer.set_Value(rowBuffer.Fields.FindField("形状相似度"), shape1);
                                                        rowBuffer.set_Value(rowBuffer.Fields.FindField("节点相似度"), matchedPoints1);
                                                        rowBuffer.set_Value(rowBuffer.Fields.FindField("综合相似度"), polygonRatio1);


                                                        int index = 0;                                                        
                                                        //if (polylineRadio > weight[2])
                                                        //{
                                                        //如果两个点之间的距离小于设置的综合相似度
                                                        //if (distance < weight[0] && MatchCode(pSrcFeature, pTarFeature))
                                                        //if (MatchCode(pSrcFeature, pTarFeature))
                                                        //{
                                                        //设置表TRA_PT_I_PtTabl的（待匹配oid）字段的值为空——cell[2]
                                                        if (rowBuffer.get_Value(rowBuffer.Fields.FindField("待匹配OID")).ToString() == "")
                                                        {
                                                            //设置表TRA_PT_I_PtTabl的（待匹配oid）字段的值——cell[2]
                                                            rowBuffer.set_Value(rowBuffer.Fields.FindField("待匹配OID"), pTarFeature.get_Value(index));
                                                        }
                                                        else
                                                        {
                                                            string oids = rowBuffer.get_Value(rowBuffer.Fields.FindField("待匹配OID")).ToString() + ";" + pTarFeature.get_Value(index);
                                                            //设置表TRA_PT_I_PtTabl的（待匹配oid）字段的值——cell[2]
                                                            rowBuffer.set_Value(rowBuffer.Fields.FindField("待匹配OID"), oids);
                                                        }

                                                        //20170912
                                                        ////设置表TRA_PT_I_PtTabl的（待匹配编码）字段的值——cell[5]
                                                        //rowBuffer.set_Value(rowBuffer.Fields.FindField("源图层名称"), pSrcFeature.get_Value(pSrcFeature.Fields.FindFieldByAliasName(array[0])));
                                                        ////设置表TRA_PT_I_PtTabl的（待匹配编码）字段的值——cell[5]
                                                        //rowBuffer.set_Value(rowBuffer.Fields.FindField("待匹配图层名称"), pTarFeature.get_Value(pTarFeature.Fields.FindFieldByAliasName(array[0])));

                                                        if (!pDicCol.ContainsKey(lIdx))
                                                        {
                                                            pDicCol.Add(lIdx, pTarFeature);
                                                        }
                                                        lIdx = lIdx + 1;
                                                        //}
                                                    }
                                                    //}

                                                }
                                            }
                                        }
                                        #region 1126注释
                                        ////tips:只有几何匹配
                                        ////没有选择属性匹配，那么只是几何匹配，需要用到反向匹配
                                        //else
                                        //{
                                        //    #region 形状相似度等
                                        //    //ClsIndicatorFun clsMatching = new ClsIndicatorFun();
                                        //    //clsMatching.SourceFeature = pSrcFeature;
                                        //    //clsMatching.TargetFeature = pTarFeature;

                                        //    //double matchedPoints = 0;
                                        //    //double shape = 0;
                                        //    //double polylineRadio = 0;

                                        //    //int index = 0;
                                        //    ////test
                                        //    //string test3 = pTarFeature.get_Value(index).ToString();


                                        //    ////20170518注释掉
                                        //    ////形状相似度
                                        //    //shape = clsMatching.PolylineShapeSimilarValue();
                                        //    ////节点相似度
                                        //    //matchedPoints = clsMatching.MatchedPointsSimilarValue(buffer);
                                        //    ////综合相似度
                                        //    //polylineRadio = shape * weight[0] + matchedPoints * weight[1];

                                        //    //string shape1 = string.Format("{0:0.00000000}", shape);
                                        //    //string matchedPoints1 = string.Format("{0:0.00000000}", matchedPoints);
                                        //    //string polygonRatio1 = string.Format("{0:0.00000000}", polylineRadio);

                                        //    //rowBuffer.set_Value(rowBuffer.Fields.FindField("形状相似度"), shape1);
                                        //    //rowBuffer.set_Value(rowBuffer.Fields.FindField("节点相似度"), matchedPoints1);
                                        //    //rowBuffer.set_Value(rowBuffer.Fields.FindField("综合相似度"), polygonRatio1);
                                        //    #endregion

                                        //    //if (polylineRadio > weight[2])
                                        //    //{
                                        //    //如果两个点之间的距离小于设置的综合相似度
                                        //    //if (distance < weight[0] && MatchCode(pSrcFeature, pTarFeature))
                                        //    //if (MatchCode(pSrcFeature, pTarFeature))
                                        //    //{
                                        //    int index = 0;

                                        //    //设置表TRA_PT_I_PtTabl的（待匹配oid）字段的值为空——cell[2]
                                        //    if (rowBuffer.get_Value(rowBuffer.Fields.FindField("待匹配OID")).ToString() == "")
                                        //    {
                                        //        //设置表TRA_PT_I_PtTabl的（待匹配oid）字段的值——cell[2]
                                        //        rowBuffer.set_Value(rowBuffer.Fields.FindField("待匹配OID"), pTarFeature.get_Value(index));
                                        //    }
                                        //    else
                                        //    {
                                        //        string oids = rowBuffer.get_Value(rowBuffer.Fields.FindField("待匹配OID")).ToString() + ";" + pTarFeature.get_Value(index);
                                        //        //设置表TRA_PT_I_PtTabl的（待匹配oid）字段的值——cell[2]
                                        //        rowBuffer.set_Value(rowBuffer.Fields.FindField("待匹配OID"), oids);
                                        //    }

                                        //    //20170912
                                        //    ////设置表TRA_PT_I_PtTabl的（待匹配编码）字段的值——cell[5]
                                        //    //rowBuffer.set_Value(rowBuffer.Fields.FindField("源图层名称"), pSrcFeature.get_Value(pSrcFeature.Fields.FindFieldByAliasName(array[0])));
                                        //    ////设置表TRA_PT_I_PtTabl的（待匹配编码）字段的值——cell[5]
                                        //    //rowBuffer.set_Value(rowBuffer.Fields.FindField("待匹配图层名称"), pTarFeature.get_Value(pTarFeature.Fields.FindFieldByAliasName(array[0])));

                                        //    if (!pDicCol.ContainsKey(lIdx))
                                        //    {
                                        //        pDicCol.Add(lIdx, pTarFeature);
                                        //    }
                                        //    lIdx = lIdx + 1;
                                        //    //}

                                        //}
                                        #endregion
                                    }
                                }
                                else
                                {
                                    double matchedPoints = 0;
                                    double shape = 0;
                                    double polylineRadio = 0;
                                    //test
                                    //string test3 = pTarFeature.get_Value(index).ToString();
                                    //20170518注释掉
                                    //形状相似度
                                    shape = ClsIndicatorFunStatic.PolylineShapeSimilarValue(pSrcFeature, pTarFeature);
                                    //节点相似度
                                    matchedPoints = ClsIndicatorFunStatic.MatchedPointsSimilarValue(pSrcFeature, pTarFeature,buffer);
                                    //综合相似度
                                    polylineRadio = shape * weight[0] + matchedPoints * weight[1];
                                    string shape1 = string.Format("{0:0.00000000}", shape);
                                    string matchedPoints1 = string.Format("{0:0.00000000}", matchedPoints);
                                    string polygonRatio1 = string.Format("{0:0.00000000}", polylineRadio);

                                    rowBuffer.set_Value(rowBuffer.Fields.FindField("形状相似度"), shape1);
                                    rowBuffer.set_Value(rowBuffer.Fields.FindField("节点相似度"), matchedPoints1);
                                    rowBuffer.set_Value(rowBuffer.Fields.FindField("综合相似度"), polygonRatio1);

                                    int index = 0;                                    
                                    //if (polylineRadio > weight[2])
                                    //{
                                    //如果两个点之间的距离小于设置的综合相似度
                                    //if (distance < weight[0] && MatchCode(pSrcFeature, pTarFeature))
                                    //if (MatchCode(pSrcFeature, pTarFeature))
                                    //{
                                    //设置表TRA_PT_I_PtTabl的（待匹配oid）字段的值为空——cell[2]
                                    if (rowBuffer.get_Value(rowBuffer.Fields.FindField("待匹配OID")).ToString() == "")
                                    {
                                        //设置表TRA_PT_I_PtTabl的（待匹配oid）字段的值——cell[2]
                                        rowBuffer.set_Value(rowBuffer.Fields.FindField("待匹配OID"), pTarFeature.get_Value(index));
                                    }
                                    else
                                    {
                                        string oids = rowBuffer.get_Value(rowBuffer.Fields.FindField("待匹配OID")).ToString() + ";" + pTarFeature.get_Value(index);
                                        //设置表TRA_PT_I_PtTabl的（待匹配oid）字段的值——cell[2]
                                        rowBuffer.set_Value(rowBuffer.Fields.FindField("待匹配OID"), oids);
                                    }

                                    //20170912
                                    ////设置表TRA_PT_I_PtTabl的（待匹配编码）字段的值——cell[5]
                                    //rowBuffer.set_Value(rowBuffer.Fields.FindField("源图层名称"), pSrcFeature.get_Value(pSrcFeature.Fields.FindFieldByAliasName(array[0])));
                                    ////设置表TRA_PT_I_PtTabl的（待匹配编码）字段的值——cell[5]
                                    //rowBuffer.set_Value(rowBuffer.Fields.FindField("待匹配图层名称"), pTarFeature.get_Value(pTarFeature.Fields.FindFieldByAliasName(array[0])));

                                    if (!pDicCol.ContainsKey(lIdx))
                                    {
                                        pDicCol.Add(lIdx, pTarFeature);
                                    }
                                    lIdx = lIdx + 1;
                                }
                            }
                            else if (pTarFeature.Shape.GeometryType == esriGeometryType.esriGeometryPoint)
                            {
                                if (fields.Length > 0)
                                {
                                    //TODO ：分号4
                                    string[] array = fields.Split(';');
                                    //string test1 = array[0];

                                    for (int k = 0; k < array.Length; k++)
                                    {
                                        //选择多个字段
                                        if (array[k].Length > 0)
                                        {
                                            //选择一个字段，但是源图层和待匹配图层的字段名称不一样
                                            if (array[k].Contains(":"))
                                            {
                                                string[] arr = ClsStatic.SplitStrColon(array[k]);
                                                string pSrcfield = pSrcFeature.get_Value(pSrcFeature.Fields.FindField(arr[0])).ToString();
                                                string pTarfield = pTarFeature.get_Value(pTarFeature.Fields.FindField(arr[1])).ToString();
                                                //if (StringSameOrNot(pSrcStr, pTarStr) > 0)
                                                //if (pSrcfield.Length > 0 && pTarfield.Length > 0 && ClsCosine.hasChinese(pSrcfield) && ClsCosine.hasChinese(pTarfield))
                                                if (pSrcfield.Length > 0 && pTarfield.Length > 0)
                                                {
                                                    //test
                                                    //int test0 = StringSameOrNot(pSrcfield, pTarfield);

                                                    string tempSrcName = "";
                                                    string tempTarName = "";
                                                    for (int j = 0; j < resultTable.Fields.FieldCount; j++)
                                                    {
                                                        if (resultTable.Fields.get_Field(j).Name == "源要素" + arr[0])
                                                        {
                                                            tempSrcName = resultTable.Fields.get_Field(j).Name;
                                                        }
                                                        if (resultTable.Fields.get_Field(j).Name == "待匹配要素" + arr[1])
                                                        {
                                                            tempTarName = resultTable.Fields.get_Field(j).Name;
                                                        }
                                                    }

                                                    //设置表TRA_PT_I_PtTabl的（待匹配编码）字段的值——cell[5]
                                                    //rowBuffer.set_Value(rowBuffer.Fields.FindField("源图层名称"), pSrcfield);
                                                    rowBuffer.set_Value(rowBuffer.Fields.FindField(tempSrcName), pSrcfield);


                                                    double Similarity = ClsCosine.getSimilarity(pSrcfield, pTarfield);

                                                    //if (StringSameOrNot(pSrcfield, pTarfield) > 1)
                                                    if (Similarity >= 0.7)
                                                    {
                                                        //设置表TRA_PT_I_PtTabl的（待匹配编码）字段的值——cell[5]
                                                        //rowBuffer.set_Value(rowBuffer.Fields.FindField("待匹配图层名称"), pTarfield);
                                                        rowBuffer.set_Value(rowBuffer.Fields.FindField(tempTarName), pTarfield);
                                                        //20170516注释掉
                                                        //if (MatchCode(pSrcFeature, pTarFeature))
                                                        //{
                                                        //    //设置表TRA_PT_I_PtTabl的（源编码）字段的值——cell[4]
                                                        //    rowBuffer.set_Value(4, pSrcFeature.get_Value(pSrcFeature.Fields.FindField("GCode")));
                                                        //    //设置表TRA_PT_I_PtTabl的（待匹配编码）字段的值——cell[5]
                                                        //    rowBuffer.set_Value(5, pTarFeature.get_Value(pTarFeature.Fields.FindField("GCode")));
                                                        //}
                                                        int index = 0;

                                                        #region 计算精度
                                                        //ClsIndicatorFun clsMatching = new ClsIndicatorFun();
                                                        //clsMatching.SourceFeature = pSrcFeature;
                                                        //clsMatching.TargetFeature = pTarFeature;

                                                        //double distance = 0;
                                                        //distance = clsMatching.PointDistance();
                                                        //string distance1 = string.Format("{0:0.0000}", 1 - distance);
                                                        ////设置表TRA_PT_I_PtTabl的（位置相似度）字段的值——cell[6]
                                                        //rowBuffer.set_Value(rowBuffer.Fields.FindField("位置相似度"), distance1);
                                                        #endregion

                                                        //如果两个点之间的距离小于设置的综合相似度
                                                        //if (distance < weight[0] && MatchCode(pSrcFeature, pTarFeature))
                                                        //if (MatchCode(pSrcFeature, pTarFeature))
                                                        //{
                                                        //设置表TRA_PT_I_PtTabl的（待匹配oid）字段的值为空——cell[2]
                                                        if (rowBuffer.get_Value(rowBuffer.Fields.FindField("待匹配OID")).ToString() == "")
                                                        {
                                                            //设置表TRA_PT_I_PtTabl的（待匹配oid）字段的值——cell[2]
                                                            rowBuffer.set_Value(rowBuffer.Fields.FindField("待匹配OID"), pTarFeature.get_Value(index));
                                                        }
                                                        else
                                                        {
                                                            string oids = rowBuffer.get_Value(rowBuffer.Fields.FindField("待匹配OID")).ToString() + ";" + pTarFeature.get_Value(index);
                                                            //设置表TRA_PT_I_PtTabl的（待匹配oid）字段的值——cell[2]
                                                            rowBuffer.set_Value(rowBuffer.Fields.FindField("待匹配OID"), oids);
                                                        }

                                                        //20170912
                                                        ////设置表TRA_PT_I_PtTabl的（待匹配编码）字段的值——cell[5]
                                                        //rowBuffer.set_Value(rowBuffer.Fields.FindField("源图层名称"), pSrcFeature.get_Value(pSrcFeature.Fields.FindFieldByAliasName(array[0])));
                                                        ////设置表TRA_PT_I_PtTabl的（待匹配编码）字段的值——cell[5]
                                                        //rowBuffer.set_Value(rowBuffer.Fields.FindField("待匹配图层名称"), pTarFeature.get_Value(pTarFeature.Fields.FindFieldByAliasName(array[0])));

                                                        pDicCol.Add(lIdx, pTarFeature);
                                                        lIdx = lIdx + 1;
                                                        //}
                                                    }
                                                }
                                            }
                                            //选择一个字段，源图层和待匹配图层的字段名称一样
                                            else
                                            {
                                                //string pSrcStrName = pSrcFeature.get_Value(pSrcFeature.Fields.FindFieldByAliasName("名称")).ToString();
                                                //string pSrcStrName = pSrcFeature.get_Value(pSrcFeature.Fields.FindField(ClsConfig.LayerConfigs[(pSrcFcls  as IDataset).Name].NameField)).ToString();
                                                //string pSrcStrName = pSrcFeature.get_Value(pSrcFeature.Fields.FindFieldByAliasName(array[0])).ToString();
                                                string pSrcfield = pSrcFeature.get_Value(pSrcFeature.Fields.FindField(array[k])).ToString();
                                                //string pTarStrName = pTarFeature.get_Value(pTarFeature.Fields.FindFieldByAliasName("名称")).ToString();
                                                //string pTarStrName = pTarFeature.get_Value(pTarFeature.Fields.FindField(ClsConfig.LayerConfigs[(pTarFcls as IDataset).Name].NameField)).ToString();
                                                //string pTarStrName = pTarFeature.get_Value(pTarFeature.Fields.FindFieldByAliasName(array[0])).ToString();
                                                string pTarfield = pTarFeature.get_Value(pTarFeature.Fields.FindField(array[k])).ToString();
                                                //test
                                                //int test0 = StringSameOrNot(pSrcStrName, pTarStrName);
                                                int test0 = StringSameOrNot(pSrcfield, pTarfield);



                                                //if (StringSameOrNot(pSrcStr, pTarStr) > 0)
                                                if (pSrcfield.Length > 0 && pTarfield.Length > 0)
                                                {
                                                    string tempSrc = "";
                                                    string tempTar = "";
                                                    for (int j = 0; j < resultTable.Fields.FieldCount; j++)
                                                    {
                                                        if (resultTable.Fields.get_Field(j).AliasName == "源要素" + array[k])
                                                        {
                                                            tempSrc = resultTable.Fields.get_Field(j).AliasName;
                                                        }
                                                        if (resultTable.Fields.get_Field(j).AliasName == "待匹配要素" + array[k])
                                                        {
                                                            tempTar = resultTable.Fields.get_Field(j).AliasName;
                                                        }
                                                    }

                                                    //设置表TRA_PT_I_PtTabl的（待匹配编码）字段的值——cell[5]
                                                    //rowBuffer.set_Value(rowBuffer.Fields.FindField("源图层名称"), pSrcfield);
                                                    rowBuffer.set_Value(rowBuffer.Fields.FindField(tempSrc), pSrcfield);

                                                    if (StringSameOrNot(pSrcfield, pTarfield) > 1)
                                                    {
                                                        //设置表TRA_PT_I_PtTabl的（待匹配编码）字段的值——cell[5]
                                                        //rowBuffer.set_Value(rowBuffer.Fields.FindField("待匹配图层名称"), pTarfield);
                                                        rowBuffer.set_Value(rowBuffer.Fields.FindField(tempTar), pTarfield);
                                                        //20170516注释掉
                                                        //if (MatchCode(pSrcFeature, pTarFeature))
                                                        //{
                                                        //    //设置表TRA_PT_I_PtTabl的（源编码）字段的值——cell[4]
                                                        //    rowBuffer.set_Value(4, pSrcFeature.get_Value(pSrcFeature.Fields.FindField("GCode")));
                                                        //    //设置表TRA_PT_I_PtTabl的（待匹配编码）字段的值——cell[5]
                                                        //    rowBuffer.set_Value(5, pTarFeature.get_Value(pTarFeature.Fields.FindField("GCode")));
                                                        //}
                                               
                                                        double distance = 0;
                                                        distance = ClsIndicatorFunStatic.EuclideanMetricDistance(pSrcFeature, pTarFeature);
                                                        string distance1 = string.Format("{0:0.0000}", 1 - distance);
                                                        //设置表TRA_PT_I_PtTabl的（位置相似度）字段的值——cell[6]
                                                        rowBuffer.set_Value(rowBuffer.Fields.FindField("位置相似度"), distance1);

                                                        int index = 0;
                                                        //如果两个点之间的距离小于设置的综合相似度
                                                        //if (distance < weight[0] && MatchCode(pSrcFeature, pTarFeature))
                                                        //if (MatchCode(pSrcFeature, pTarFeature))
                                                        //{
                                                        //设置表TRA_PT_I_PtTabl的（待匹配oid）字段的值为空——cell[2]
                                                        if (rowBuffer.get_Value(rowBuffer.Fields.FindField("待匹配OID")).ToString() == "")
                                                        {
                                                            //设置表TRA_PT_I_PtTabl的（待匹配oid）字段的值——cell[2]
                                                            rowBuffer.set_Value(rowBuffer.Fields.FindField("待匹配OID"), pTarFeature.get_Value(index));
                                                        }
                                                        else
                                                        {
                                                            string oids = rowBuffer.get_Value(rowBuffer.Fields.FindField("待匹配OID")).ToString() + ";" + pTarFeature.get_Value(index);
                                                            //设置表TRA_PT_I_PtTabl的（待匹配oid）字段的值——cell[2]
                                                            rowBuffer.set_Value(rowBuffer.Fields.FindField("待匹配OID"), oids);
                                                        }

                                                        //20170912
                                                        ////设置表TRA_PT_I_PtTabl的（待匹配编码）字段的值——cell[5]
                                                        //rowBuffer.set_Value(rowBuffer.Fields.FindField("源图层名称"), pSrcFeature.get_Value(pSrcFeature.Fields.FindFieldByAliasName(array[0])));
                                                        ////设置表TRA_PT_I_PtTabl的（待匹配编码）字段的值——cell[5]
                                                        //rowBuffer.set_Value(rowBuffer.Fields.FindField("待匹配图层名称"), pTarFeature.get_Value(pTarFeature.Fields.FindFieldByAliasName(array[0])));

                                                        pDicCol.Add(lIdx, pTarFeature);
                                                        lIdx = lIdx + 1;
                                                        //}
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    int index = 0;
                                    #region 计算精度

                                    //ClsIndicatorFun clsMatching = new ClsIndicatorFun();
                                    //clsMatching.SourceFeature = pSrcFeature;
                                    //clsMatching.TargetFeature = pTarFeature;

                                    //double distance = 0;
                                    //distance = clsMatching.PointDistance();
                                    //string distance1 = string.Format("{0:0.0000}", 1 - distance);
                                    ////设置表TRA_PT_I_PtTabl的（位置相似度）字段的值——cell[6]
                                    //rowBuffer.set_Value(rowBuffer.Fields.FindField("位置相似度"), distance1);
                                    #endregion


                                    if (rowBuffer.get_Value(rowBuffer.Fields.FindField("待匹配OID")).ToString() == "")
                                    {
                                        //设置表TRA_PT_I_PtTabl的（待匹配oid）字段的值——cell[2]
                                        rowBuffer.set_Value(rowBuffer.Fields.FindField("待匹配OID"), pTarFeature.get_Value(index));
                                    }
                                    else
                                    {
                                        string oids = rowBuffer.get_Value(rowBuffer.Fields.FindField("待匹配OID")).ToString() + ";" + pTarFeature.get_Value(index);
                                        //设置表TRA_PT_I_PtTabl的（待匹配oid）字段的值——cell[2]
                                        rowBuffer.set_Value(rowBuffer.Fields.FindField("待匹配OID"), oids);
                                    }
                                    pDicCol.Add(lIdx, pTarFeature);
                                    lIdx = lIdx + 1;
                                }
                            }
                        }
                        pTarFeature = pTarCursor.NextFeature();
                    }

                    /**情况1：通过用代码构建缓冲区分析，缓冲区分析找不到对应要素，即为新增的要素。*/
                    //找不到对应要素的情况,即为新增的情况。遍历TU找TE中的要素，与以前项目的更新程序查找相反。
                    if (pDicCol.Count == 0)
                    {
                        lOutFldIndex = rowBuffer.Fields.FindField("变化标记");
                        if (lOutFldIndex > -1)
                        {
                            //rowBuffer.set_Value(lOutFldIndex, "新增要素");
                            rowBuffer.set_Value(lOutFldIndex, ClsConstant.One2Zero);
                            ////表示新增
                        }
                        rowCursor.InsertRow(rowBuffer);
                    }

                    /**情况2：通过用代码构建缓冲区分析，找到1条对应要素的情况（源图层和待匹配图层要素是1对1的情况），然后判断属性图形是否发生变化。*/
                    if (pDicCol.Count == 1)
                    {
                        IFeature feature = pDicCol[0] as IFeature;
                        //判断形状是否发生变化，如果没有变化blnShpEqual设为True
                        if (pSrcFeature.Shape != null)
                        {
                            pRelOp = pSrcFeature.Shape as IRelationalOperator;
                            blnShpEqual = pRelOp.Equals(feature.Shape);

                            //判断线方向性变化
                            blnAttriEqual = true;
                            //提取匹配字段，查找ID与OID
                            string[] array = fields.Split(';');
                            for (i = 0; i < array.Length; i++)
                            {
                                if (!array[i].Contains("("))
                                {
                                    int teIndex = feature.Fields.FindField(array[i]);
                                    int tuIndex = pSrcFeature.Fields.FindField(array[i]);
                                    if (teIndex == -1 || tuIndex == -1)
                                    {
                                        continue;
                                    }
                                    object teObj = feature.get_Value(teIndex);
                                    object tuObj = pSrcFeature.get_Value(tuIndex);

                                    if (Convert.IsDBNull(teObj) && (!Convert.IsDBNull(tuObj)))
                                    {
                                        blnAttriEqual = false;
                                    }
                                    else if (!Convert.IsDBNull(teObj) && (!Convert.IsDBNull(tuObj)))
                                    {
                                        if (feature.get_Value(teIndex).ToString() != pSrcFeature.get_Value(tuIndex).ToString())
                                        {
                                            blnAttriEqual = false;
                                        }
                                    }
                                    else if (!Convert.IsDBNull(teObj) && (Convert.IsDBNull(tuObj)))
                                    {
                                        if (!string.IsNullOrEmpty(feature.get_Value(teIndex).ToString()))
                                        {
                                            blnAttriEqual = false;
                                        }
                                    }

                                }
                                else
                                {
                                    string tef = array[i].Substring(0, array[i].LastIndexOf('('));
                                    string tuf = array[i].Substring(array[i].LastIndexOf('(') + 1);
                                    tuf = tuf.Substring(0, tuf.LastIndexOf(')'));
                                    int teIndex = feature.Fields.FindField(tef);
                                    int tuIndex = pSrcFeature.Fields.FindField(tuf);
                                    if (teIndex == -1 || tuIndex == -1)
                                    {
                                        continue;
                                    }
                                    object teObj = feature.get_Value(teIndex);
                                    object tuObj = pSrcFeature.get_Value(tuIndex);

                                    if (Convert.IsDBNull(teObj) && (!Convert.IsDBNull(tuObj)))
                                    {
                                        blnAttriEqual = false;
                                    }
                                    else if (!Convert.IsDBNull(teObj) && (!Convert.IsDBNull(tuObj)))
                                    {
                                        if (feature.get_Value(teIndex).ToString() != pSrcFeature.get_Value(tuIndex).ToString())
                                        {
                                            blnAttriEqual = false;
                                        }
                                    }
                                    else if (!Convert.IsDBNull(teObj) && (Convert.IsDBNull(tuObj)))
                                    {
                                        if (!string.IsNullOrEmpty(feature.get_Value(teIndex).ToString()))
                                        {
                                            blnAttriEqual = false;
                                        }
                                    }
                                }
                            }

                            if (blnShpEqual == false || blnAttriEqual == false)
                            {
                                ////只图形变化
                                if (blnShpEqual == false && blnAttriEqual)
                                {
                                    intFieldIndex = rowBuffer.Fields.FindField("变化标记");
                                    if (intFieldIndex > -1)
                                    {
                                        //rowBuffer.set_Value(intFieldIndex, "图形变化");
                                        //rowBuffer.set_Value(intFieldIndex, "一对一");
                                        rowBuffer.set_Value(intFieldIndex, ClsConstant.One2One);
                                    }

                                    ////只属性变化
                                }
                                else if (blnShpEqual && blnAttriEqual == false)
                                {
                                    intFieldIndex = rowBuffer.Fields.FindField("变化标记");
                                    if (intFieldIndex > -1)
                                    {
                                        rowBuffer.set_Value(intFieldIndex, "属性变化");
                                        ////修改的打标为6   
                                    }
                                    ////图形属性都变化
                                }
                                else if (blnShpEqual == false && blnAttriEqual == false)
                                {
                                    intFieldIndex = rowBuffer.Fields.FindField("变化标记");
                                    if (intFieldIndex > -1)
                                    {
                                        rowBuffer.set_Value(intFieldIndex, "属性图形变化");
                                        ////修改的打标为2        
                                    }

                                }
                            }
                            else if (blnShpEqual == true & blnAttriEqual == true)
                            {
                                intFieldIndex = rowBuffer.Fields.FindField("变化标记");
                                if (intFieldIndex > -1)
                                {
                                    rowBuffer.set_Value(intFieldIndex, "未变化");
                                }
                            }
                            rowCursor.InsertRow(rowBuffer);
                        }
                    }
                    /**情况3：通过用代码构建缓冲区分析，找到多条对应要素的情况（源图层和待匹配图层要素是1对多的情况），然后判断属性图形是否发生变化。*/
                    else if (pDicCol.Count > 1)
                    {
                        intFieldIndex = rowBuffer.Fields.FindField("变化标记");
                        if (intFieldIndex > -1)
                        {
                            //rowBuffer.set_Value(intFieldIndex, "一对多");
                            rowBuffer.set_Value(intFieldIndex, ClsConstant.One2More);
                            rowCursor.InsertRow(rowBuffer);
                        }
                    }


                    ////进程条处理
                    sbProcessBar(sourceFeatCount, prgMain, prgSub);

                    //每1千个要素就把缓冲区内的要素类写入目标层
                    if (iFeatCount >= 500)
                    {
                        rowCursor.Flush();
                        iFeatCount = 0;
                    }

                    stateLabel.Text = "正在对" + strSrcLyrName + "图层和" + strTarLyrName + "图层进行数据变化分析，已经处理" + prgMain.Value + "(" + sourceFeatCount + ")个要素。";
                    stateLabel.Refresh();
                    prgMain.Update();
                    prgSub.Update();
                    iFeatCount = iFeatCount + 1;
                    pSrcFeature = pSrcCursor.NextFeature();
                }
            }
            catch (System.Exception ex)
            {
                //MessageBox.Show(ex.Message);
                MessageBox.Show(ex.StackTrace);
            }

            if (rowCursor != null)
            {
                rowCursor.Flush();
            }

            workspaceEdit.StopEditOperation();
            workspaceEdit.StopEditing(true);

            stateLabel.Text = "";
            prgMain.Value = 0;
            prgSub.Value = 0;

            return true;
            //return functionReturnValue;

        }
        #endregion
        /// <summary>
        /// 几何匹配
        /// </summary>
        /// <param name="pSrcFcls">源图层</param>
        /// <param name="pTarFcls">待匹配图层</param>
        /// <param name="resultTable">保存结果的表TRA_PT_I_PtTable</param>
        /// <param name="matchedMode">匹配模式：设置匹配选择方式，0 代表几何匹配，1代表拓扑匹配，2代表属性匹配 </param>
        /// <param name="weight">表MatchedPointFCSetting中的（综合相似度阈值）</param>
        /// <param name="buffer">表MatchedPointFCSetting中的（搜索半径）字段值</param>
        /// <param name="fields">表MatchedPointFCSetting中的（匹配属性）字段值</param>
        /// <param name="prgMain">总进度条</param>
        /// <param name="prgSub">子进度条</param>
        /// <param name="stateLabel">状态信息</param>
        /// <returns></returns>
        public bool SearchChangedFeaturesDifShape(IFeatureClass pSrcFcls, IFeatureClass pTarFcls, ITable resultTable, int matchedMode, double[] weight, double buffer, string fields, ProgressBar prgMain, ProgressBar prgSub, LabelX stateLabel)
        {
            //bool functionReturnValue = false;

            IFeatureCursor pSrcCursor = null; //源数据指针
            IFeatureCursor pTarCursor = null;//工作区数据指针

            IFeature pSrcFeature = null;
            IFeature pTarFeature = null;

            IRelationalOperator pRelOp;
            bool blnShpEqual = false;  //形状匹配
            bool blnAttriEqual = false;//属性匹配
            int i = 0;
            int intFieldIndex = -1;
            ISpatialFilter pSFilter;

            int sourceFeatCount = 0; //源图层要素个数
            int lFeatcount = 0;  //临时层要素个数
            int lIdx = 0;
            //找到的与源要素有关的目标要素
            Dictionary<int, IFeature> pDicCol;
            //Dictionary<int, IFeature> pDicChangeF = new Dictionary<int, IFeature>();

            int lOutFldIndex = 0;
            ////数据源和目标数据要素类字段索引
            string strSrcLyrName = "";
            string strTarLyrName = "";
            IDataset pDst;//临时变量，用于获取图层名
            int iFeatCount = 0; //处理的临时要素


            ////进入编辑状态
            //匹配表
            IDataset dataset = resultTable as IDataset;
            IWorkspace workspace = dataset.Workspace;
            IWorkspaceEdit workspaceEdit = workspace as IWorkspaceEdit;
            workspaceEdit.StartEditing(true);
            workspaceEdit.StartEditOperation();


            sourceFeatCount = pSrcFcls.FeatureCount(null);
            pSrcCursor = pSrcFcls.Search(null, false);

            //设置进度条
            if (sourceFeatCount > 0)
            {
                prgMain.Maximum = sourceFeatCount;
            }

            prgSub.Maximum = 100;
            prgSub.Value = 0;
            prgMain.Value = 0;

            if ((pSrcCursor != null))
            {
                pSrcFeature = pSrcCursor.NextFeature();
            }

            //指示属性值是否相等
            blnAttriEqual = true;
            //设置图层名
            pDst = pSrcFcls as IDataset; //原图层数据集
            strSrcLyrName = pDst.Name; //原图层名
            pDst = pTarFcls as IDataset;
            strTarLyrName = pDst.Name;//工作层名
            //pDst = pResultFcls as IDataset;
            //strResultLyrName = pDst.Name;
            pDst = null;

            IRowBuffer rowBuffer = null;
            ICursor rowCursor = null;
            //源要素总的循环
            try
            {
                while (pSrcFeature != null)
                {
                    rowBuffer = resultTable.CreateRowBuffer();
                    rowCursor = resultTable.Insert(true);

                    pDicCol = new Dictionary<int, IFeature>();

                    /**通过用代码构建缓冲区分析，来判断是否是同一个要素*/

                    pSFilter = new SpatialFilter();
                    if (pSrcFeature.Shape.GeometryType == esriGeometryType.esriGeometryPolyline)
                    {
                        ITopologicalOperator top = pSrcFeature.Shape as ITopologicalOperator;
                        pSFilter.Geometry = top.Buffer(buffer) as IGeometry;
                        //自定义空间查询,减少查询次数,提高查询速度
                        pSFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                    }
                    else if (pSrcFeature.Shape.GeometryType == esriGeometryType.esriGeometryPolygon)
                    {
                        pSFilter.Geometry = pSrcFeature.Shape;//查询边界形状为多边形
                        pSFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelRelation;//函数用法
                        pSFilter.SpatialRelDescription = "T********";
                    }
                    else if (pSrcFeature.Shape.GeometryType == esriGeometryType.esriGeometryPoint)
                    {
                        //创建缓冲区
                        ITopologicalOperator top = pSrcFeature.Shape as ITopologicalOperator;
                        pSFilter.Geometry = top.Buffer(buffer) as IGeometry;
                        //自定义空间查询,建立缓冲区
                        //设置缓冲区过滤条件：包含
                        pSFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;
                    }

                    //得到对应pSrcFeature的TE图层上的要素游标
                    //查询在原图层多边形边界处的工作层的元素
                    pTarCursor = pTarFcls.Search(pSFilter, false);
                    //得到对应pSrcFeature的TE图层上的要素个数
                    lFeatcount = pTarFcls.FeatureCount(pSFilter);
                    pTarFeature = pTarCursor.NextFeature();
                    lIdx = 0;
                    #region 算法说明
                    //*******************************************************************************
                    //算法说明:
                    //获取相交面的面积或相交线的长度.最后以P匹配参数来找出与pSrcFeature
                    //最匹配的pTarFeature,其中A表示pTarFeature的面积或长度,B表示pSrcFeature的面积或长度.
                    //当P值最大时,此时的pTarFeature即为找到的要素.
                    //*******************************************************************************
                    //目标次要素循环
                    #endregion

                    //设置表TRA_PT_I_PtTabl的（源OID）字段的值——cell[1]
                    rowBuffer.set_Value(rowBuffer.Fields.FindField("源OID"), pSrcFeature.OID);
                    while ((pTarFeature != null))
                    {
                        if ((pTarFeature.Shape != null))
                        {
                            if (pTarFeature.Shape.GeometryType == esriGeometryType.esriGeometryPolygon)
                            {

                                if (MatchCode(pSrcFeature, pTarFeature))
                                {
                                    rowBuffer.set_Value(4, pSrcFeature.get_Value(pSrcFeature.Fields.FindField("GCode")));
                                    rowBuffer.set_Value(5, pTarFeature.get_Value(pTarFeature.Fields.FindField("GCode")));
                                }
                    
                                double center = 0;
                                double area = 0;
                                double shape = 0;
                                double polygonRatio = 0;
                                //判断匹配方法

                                shape = ClsIndicatorFunStatic.ShapeSimilarValue(pSrcFeature, pTarFeature);
                                area = ClsIndicatorFunStatic.AreaSimilarValue(pSrcFeature, pTarFeature);
                                center = ClsIndicatorFunStatic.CenterSimilarValue(pSrcFeature, pTarFeature);
                                polygonRatio = shape * weight[0] + area * weight[2] + center * weight[1];
                                string shape1 = string.Format("{0:0.00000000}", shape);
                                string area1 = string.Format("{0:0.00000000}", area);
                                string center1 = string.Format("{0:0.00000000}", center);
                                string polygonRatio1 = string.Format("{0:0.00000000}", polygonRatio);
                                rowBuffer.set_Value(6, center1);
                                rowBuffer.set_Value(7, area1);
                                rowBuffer.set_Value(8, shape1);
                                rowBuffer.set_Value(9, polygonRatio1);

                                int index = 0;                                
                                if (polygonRatio > weight[3])
                                {
                                    if (rowBuffer.get_Value(2).ToString() == "")
                                    {
                                        rowBuffer.set_Value(2, pTarFeature.get_Value(index));
                                    }
                                    else
                                    {
                                        string oids = rowBuffer.get_Value(2).ToString() + ";" + pTarFeature.get_Value(index);
                                        rowBuffer.set_Value(2, oids);
                                    }
                                    pDicCol.Add(lIdx, pTarFeature);
                                    lIdx = lIdx + 1;
                                }
                            }
                            else if (pTarFeature.Shape.GeometryType == esriGeometryType.esriGeometryPolyline)
                            {
                                string[] array = fields.Split(';');
                                string test1 = array[0];

                                for (int k = 0; k < array.Length; k++)
                                {
                                    if (array[k].Length > 0)
                                    {
                                        //string pSrcStrName = pSrcFeature.get_Value(pSrcFeature.Fields.FindFieldByAliasName("名称")).ToString();
                                        //string pSrcStrName = pSrcFeature.get_Value(pSrcFeature.Fields.FindField(ClsConfig.LayerConfigs[(pSrcFcls  as IDataset).Name].NameField)).ToString();
                                        //string pSrcStrName = pSrcFeature.get_Value(pSrcFeature.Fields.FindFieldByAliasName(array[0])).ToString();
                                        string pSrcfield = pSrcFeature.get_Value(pSrcFeature.Fields.FindFieldByAliasName(array[k])).ToString();
                                        //string pTarStrName = pTarFeature.get_Value(pTarFeature.Fields.FindFieldByAliasName("名称")).ToString();
                                        //string pTarStrName = pTarFeature.get_Value(pTarFeature.Fields.FindField(ClsConfig.LayerConfigs[(pTarFcls as IDataset).Name].NameField)).ToString();
                                        //string pTarStrName = pTarFeature.get_Value(pTarFeature.Fields.FindFieldByAliasName(array[0])).ToString();
                                        string pTarfield = pTarFeature.get_Value(pTarFeature.Fields.FindFieldByAliasName(array[k])).ToString();
                                        //test
                                        //int test0 = StringSameOrNot(pSrcStrName, pTarStrName);
                                        int test0 = StringSameOrNot2(pSrcfield, pTarfield);


                                        //if (StringSameOrNot(pSrcStr, pTarStr) > 0)
                                        if (pSrcfield.Length > 0 && pTarfield.Length > 0)
                                        {
                                            string tempSrc = "";
                                            string tempTar = "";
                                            for (int j = 0; j < resultTable.Fields.FieldCount; j++)
                                            {
                                                if (resultTable.Fields.get_Field(j).AliasName == "源要素" + array[k])
                                                {
                                                    tempSrc = resultTable.Fields.get_Field(j).AliasName;
                                                }
                                                if (resultTable.Fields.get_Field(j).AliasName == "待匹配要素" + array[k])
                                                {
                                                    tempTar = resultTable.Fields.get_Field(j).AliasName;
                                                }
                                            }

                                            //设置表TRA_PT_I_PtTabl的（待匹配编码）字段的值——cell[5]
                                            //rowBuffer.set_Value(rowBuffer.Fields.FindField("源图层名称"), pSrcfield);
                                            rowBuffer.set_Value(rowBuffer.Fields.FindField(tempSrc), pSrcfield);

                                            if (StringSameOrNot2(pSrcfield, pTarfield) > 2)
                                            {
                                                //设置表TRA_PT_I_PtTabl的（待匹配编码）字段的值——cell[5]
                                                //rowBuffer.set_Value(rowBuffer.Fields.FindField("待匹配图层名称"), pTarfield);
                                                rowBuffer.set_Value(rowBuffer.Fields.FindField(tempTar), pTarfield);
                                                //20170516注释掉
                                                //if (MatchCode(pSrcFeature, pTarFeature))
                                                //{
                                                //    //设置表TRA_PT_I_PtTabl的（源编码）字段的值——cell[4]
                                                //    rowBuffer.set_Value(4, pSrcFeature.get_Value(pSrcFeature.Fields.FindField("GCode")));
                                                //    //设置表TRA_PT_I_PtTabl的（待匹配编码）字段的值——cell[5]
                                                //    rowBuffer.set_Value(5, pTarFeature.get_Value(pTarFeature.Fields.FindField("GCode")));
                                                //}


                                                double matchedPoints = 0;
                                                double shape = 0;
                                                double polylineRadio = 0;



                                                //20170518注释掉
                                                //形状相似度
                                                shape = ClsIndicatorFunStatic.PolylineShapeSimilarValue(pSrcFeature, pTarFeature);
                                                //节点相似度
                                                matchedPoints = ClsIndicatorFunStatic.MatchedPointsSimilarValue(pSrcFeature, pTarFeature,buffer);
                                                //综合相似度
                                                polylineRadio = shape * weight[0] + matchedPoints * weight[1];

                                                string shape1 = string.Format("{0:0.00000000}", shape);
                                                string matchedPoints1 = string.Format("{0:0.00000000}", matchedPoints);
                                                string polygonRatio1 = string.Format("{0:0.00000000}", polylineRadio);

                                                rowBuffer.set_Value(rowBuffer.Fields.FindField("形状相似度"), shape1);
                                                rowBuffer.set_Value(rowBuffer.Fields.FindField("节点相似度"), matchedPoints1);
                                                rowBuffer.set_Value(rowBuffer.Fields.FindField("综合相似度"), polygonRatio1);

                                                int index = 0;                                                
                                                //if (polylineRadio > weight[2])
                                                //{
                                                //如果两个点之间的距离小于设置的综合相似度
                                                //if (distance < weight[0] && MatchCode(pSrcFeature, pTarFeature))
                                                //if (MatchCode(pSrcFeature, pTarFeature))
                                                //{
                                                //设置表TRA_PT_I_PtTabl的（待匹配oid）字段的值为空——cell[2]
                                                if (rowBuffer.get_Value(rowBuffer.Fields.FindField("待匹配OID")).ToString() == "")
                                                {
                                                    //设置表TRA_PT_I_PtTabl的（待匹配oid）字段的值——cell[2]
                                                    rowBuffer.set_Value(rowBuffer.Fields.FindField("待匹配OID"), pTarFeature.get_Value(index));
                                                }
                                                else
                                                {
                                                    string oids = rowBuffer.get_Value(rowBuffer.Fields.FindField("待匹配OID")).ToString() + ";" + pTarFeature.get_Value(index);
                                                    //设置表TRA_PT_I_PtTabl的（待匹配oid）字段的值——cell[2]
                                                    rowBuffer.set_Value(rowBuffer.Fields.FindField("待匹配OID"), oids);
                                                }

                                                //20170912
                                                ////设置表TRA_PT_I_PtTabl的（待匹配编码）字段的值——cell[5]
                                                //rowBuffer.set_Value(rowBuffer.Fields.FindField("源图层名称"), pSrcFeature.get_Value(pSrcFeature.Fields.FindFieldByAliasName(array[0])));
                                                ////设置表TRA_PT_I_PtTabl的（待匹配编码）字段的值——cell[5]
                                                //rowBuffer.set_Value(rowBuffer.Fields.FindField("待匹配图层名称"), pTarFeature.get_Value(pTarFeature.Fields.FindFieldByAliasName(array[0])));

                                                if (!pDicCol.ContainsKey(lIdx))
                                                {
                                                    pDicCol.Add(lIdx, pTarFeature);
                                                }
                                                lIdx = lIdx + 1;
                                                //}
                                            }
                                            //}

                                        }
                                    }
                                    //tips:只有几何匹配
                                    else  //没有选择属性匹配，那么只是几何匹配，需要用到反向匹配
                                    {
                                        #region 形状相似度等
                                        //ClsIndicatorFun clsMatching = new ClsIndicatorFun();
                                        //clsMatching.SourceFeature = pSrcFeature;
                                        //clsMatching.TargetFeature = pTarFeature;

                                        //double matchedPoints = 0;
                                        //double shape = 0;
                                        //double polylineRadio = 0;

                                        //int index = 0;
                                        ////test
                                        //string test3 = pTarFeature.get_Value(index).ToString();


                                        ////20170518注释掉
                                        ////形状相似度
                                        //shape = clsMatching.PolylineShapeSimilarValue();
                                        ////节点相似度
                                        //matchedPoints = clsMatching.MatchedPointsSimilarValue(buffer);
                                        ////综合相似度
                                        //polylineRadio = shape * weight[0] + matchedPoints * weight[1];

                                        //string shape1 = string.Format("{0:0.00000000}", shape);
                                        //string matchedPoints1 = string.Format("{0:0.00000000}", matchedPoints);
                                        //string polygonRatio1 = string.Format("{0:0.00000000}", polylineRadio);

                                        //rowBuffer.set_Value(rowBuffer.Fields.FindField("形状相似度"), shape1);
                                        //rowBuffer.set_Value(rowBuffer.Fields.FindField("节点相似度"), matchedPoints1);
                                        //rowBuffer.set_Value(rowBuffer.Fields.FindField("综合相似度"), polygonRatio1);
                                        #endregion

                                        //if (polylineRadio > weight[2])
                                        //{
                                        //如果两个点之间的距离小于设置的综合相似度
                                        //if (distance < weight[0] && MatchCode(pSrcFeature, pTarFeature))
                                        //if (MatchCode(pSrcFeature, pTarFeature))
                                        //{
                                        int index = 0;

                                        //设置表TRA_PT_I_PtTabl的（待匹配oid）字段的值为空——cell[2]
                                        if (rowBuffer.get_Value(rowBuffer.Fields.FindField("待匹配OID")).ToString() == "")
                                        {
                                            //设置表TRA_PT_I_PtTabl的（待匹配oid）字段的值——cell[2]
                                            rowBuffer.set_Value(rowBuffer.Fields.FindField("待匹配OID"), pTarFeature.get_Value(index));
                                        }
                                        else
                                        {
                                            string oids = rowBuffer.get_Value(rowBuffer.Fields.FindField("待匹配OID")).ToString() + ";" + pTarFeature.get_Value(index);
                                            //设置表TRA_PT_I_PtTabl的（待匹配oid）字段的值——cell[2]
                                            rowBuffer.set_Value(rowBuffer.Fields.FindField("待匹配OID"), oids);
                                        }

                                        //20170912
                                        ////设置表TRA_PT_I_PtTabl的（待匹配编码）字段的值——cell[5]
                                        //rowBuffer.set_Value(rowBuffer.Fields.FindField("源图层名称"), pSrcFeature.get_Value(pSrcFeature.Fields.FindFieldByAliasName(array[0])));
                                        ////设置表TRA_PT_I_PtTabl的（待匹配编码）字段的值——cell[5]
                                        //rowBuffer.set_Value(rowBuffer.Fields.FindField("待匹配图层名称"), pTarFeature.get_Value(pTarFeature.Fields.FindFieldByAliasName(array[0])));

                                        if (!pDicCol.ContainsKey(lIdx))
                                        {
                                            pDicCol.Add(lIdx, pTarFeature);
                                        }
                                        lIdx = lIdx + 1;
                                        //}

                                    }
                                }
                            }
                            else if (pTarFeature.Shape.GeometryType == esriGeometryType.esriGeometryPoint)
                            {
                                string[] array = fields.Split(';');
                                string test1 = array[0];

                                for (int k = 0; k < array.Length; k++)
                                {
                                    if (array[k].Length > 0)
                                    {
                                        //string pSrcStrName = pSrcFeature.get_Value(pSrcFeature.Fields.FindFieldByAliasName("名称")).ToString();
                                        //string pSrcStrName = pSrcFeature.get_Value(pSrcFeature.Fields.FindField(ClsConfig.LayerConfigs[(pSrcFcls  as IDataset).Name].NameField)).ToString();
                                        //string pSrcStrName = pSrcFeature.get_Value(pSrcFeature.Fields.FindFieldByAliasName(array[0])).ToString();
                                        //string pSrcFeatureName = pSrcFeature.get_Value(pSrcFeature.Fields.FindFieldByAliasName(array[k])).ToString();
                                        string pSrcFeatureName = pSrcFeature.get_Value(pSrcFeature.Fields.FindField(ClsConfig.LayerConfigs[(pSrcFcls as IDataset).Name].NameField)).ToString();
                                        //string pTarStrName = pTarFeature.get_Value(pTarFeature.Fields.FindFieldByAliasName("名称")).ToString();
                                        //string pTarStrName = pTarFeature.get_Value(pTarFeature.Fields.FindField(ClsConfig.LayerConfigs[(pTarFcls as IDataset).Name].NameField)).ToString();
                                        //string pTarStrName = pTarFeature.get_Value(pTarFeature.Fields.FindFieldByAliasName(array[0])).ToString();
                                        //string pTarFeatureName = pTarFeature.get_Value(pTarFeature.Fields.FindFieldByAliasName(array[k])).ToString();
                                        string pTarFeatureName = pTarFeature.get_Value(pTarFeature.Fields.FindField(ClsConfig.LayerConfigs[(pTarFcls as IDataset).Name].NameField)).ToString();
                                        //test
                                        //int test0 = StringSameOrNot(pSrcStrName, pTarStrName);
                                        int test0 = StringSameOrNot(pSrcFeatureName, pTarFeatureName);



                                        //if (StringSameOrNot(pSrcStr, pTarStr) > 0)
                                        if (pSrcFeatureName.Length > 0 && pTarFeatureName.Length > 0)
                                        {
                                            string tempSrc = "";
                                            string tempTar = "";
                                            for (int j = 0; j < resultTable.Fields.FieldCount; j++)
                                            {
                                                if (resultTable.Fields.get_Field(j).AliasName == "源要素" + array[k])
                                                {
                                                    tempSrc = resultTable.Fields.get_Field(j).AliasName;
                                                }
                                                if (resultTable.Fields.get_Field(j).AliasName == "待匹配要素" + array[k])
                                                {
                                                    tempTar = resultTable.Fields.get_Field(j).AliasName;
                                                }
                                            }

                                            //设置表TRA_PT_I_PtTabl的（待匹配编码）字段的值——cell[5]
                                            //rowBuffer.set_Value(rowBuffer.Fields.FindField("源图层名称"), pSrcfield);
                                            rowBuffer.set_Value(rowBuffer.Fields.FindField(tempSrc), pSrcFeatureName);

                                            if (StringSameOrNot(pSrcFeatureName, pTarFeatureName) > 1)
                                            {
                                                //设置表TRA_PT_I_PtTabl的（待匹配编码）字段的值——cell[5]
                                                //rowBuffer.set_Value(rowBuffer.Fields.FindField("待匹配图层名称"), pTarfield);
                                                rowBuffer.set_Value(rowBuffer.Fields.FindField(tempTar), pTarFeatureName);
                                                //20170516注释掉
                                                //if (MatchCode(pSrcFeature, pTarFeature))
                                                //{
                                                //    //设置表TRA_PT_I_PtTabl的（源编码）字段的值——cell[4]
                                                //    rowBuffer.set_Value(4, pSrcFeature.get_Value(pSrcFeature.Fields.FindField("GCode")));
                                                //    //设置表TRA_PT_I_PtTabl的（待匹配编码）字段的值——cell[5]
                                                //    rowBuffer.set_Value(5, pTarFeature.get_Value(pTarFeature.Fields.FindField("GCode")));
                                                //}


                                                //ClsIndicatorFun clsMatching = new ClsIndicatorFun();
                                                //clsMatching.SourceFeature = pSrcFeature;
                                                //clsMatching.TargetFeature = pTarFeature;

                                                //double distance = 0;
                                                int index = 0;
                                                //distance = clsMatching.PointDistance();
                                                //string distance1 = string.Format("{0:0.0000}", 1 - distance);
                                                ////设置表TRA_PT_I_PtTabl的（位置相似度）字段的值——cell[6]
                                                //rowBuffer.set_Value(rowBuffer.Fields.FindField("位置相似度"), distance1);

                                                //如果两个点之间的距离小于设置的综合相似度
                                                //if (distance < weight[0] && MatchCode(pSrcFeature, pTarFeature))
                                                //if (MatchCode(pSrcFeature, pTarFeature))
                                                //{
                                                //设置表TRA_PT_I_PtTabl的（待匹配oid）字段的值为空——cell[2]
                                                if (rowBuffer.get_Value(rowBuffer.Fields.FindField("待匹配OID")).ToString() == "")
                                                {
                                                    //设置表TRA_PT_I_PtTabl的（待匹配oid）字段的值——cell[2]
                                                    rowBuffer.set_Value(rowBuffer.Fields.FindField("待匹配OID"), pTarFeature.get_Value(index));
                                                }
                                                else
                                                {
                                                    string oids = rowBuffer.get_Value(rowBuffer.Fields.FindField("待匹配OID")).ToString() + ";" + pTarFeature.get_Value(index);
                                                    //设置表TRA_PT_I_PtTabl的（待匹配oid）字段的值——cell[2]
                                                    rowBuffer.set_Value(rowBuffer.Fields.FindField("待匹配OID"), oids);
                                                }

                                                //20170912
                                                ////设置表TRA_PT_I_PtTabl的（待匹配编码）字段的值——cell[5]
                                                //rowBuffer.set_Value(rowBuffer.Fields.FindField("源图层名称"), pSrcFeature.get_Value(pSrcFeature.Fields.FindFieldByAliasName(array[0])));
                                                ////设置表TRA_PT_I_PtTabl的（待匹配编码）字段的值——cell[5]
                                                //rowBuffer.set_Value(rowBuffer.Fields.FindField("待匹配图层名称"), pTarFeature.get_Value(pTarFeature.Fields.FindFieldByAliasName(array[0])));

                                                pDicCol.Add(lIdx, pTarFeature);
                                                lIdx = lIdx + 1;
                                                //}
                                            }
                                        }
                                    }
                                }


                            }
                        }
                        pTarFeature = pTarCursor.NextFeature();
                    }

                    /**情况1：通过用代码构建缓冲区分析，缓冲区分析找不到对应要素，即为新增的要素。*/
                    //找不到对应要素的情况,即为新增的情况。遍历TU找TE中的要素，与以前项目的更新程序查找相反。
                    if (pDicCol.Count == 0)
                    {

                        lOutFldIndex = rowBuffer.Fields.FindField("变化标记");
                        if (lOutFldIndex > -1)
                        {
                            //rowBuffer.set_Value(lOutFldIndex, "新增要素");
                            rowBuffer.set_Value(lOutFldIndex, ClsConstant.One2Zero);
                            ////表示新增
                        }
                        rowCursor.InsertRow(rowBuffer);
                    }

                    /**情况2：通过用代码构建缓冲区分析，找到1条对应要素的情况（源图层和待匹配图层要素是1对1的情况），然后判断属性图形是否发生变化。*/
                    if (pDicCol.Count == 1)
                    {
                        IFeature feature = pDicCol[0] as IFeature;
                        //判断形状是否发生变化，如果没有变化blnShpEqual设为True
                        if (pSrcFeature.Shape != null)
                        {
                            pRelOp = pSrcFeature.Shape as IRelationalOperator;
                            blnShpEqual = pRelOp.Equals(feature.Shape);

                            //判断线方向性变化
                            blnAttriEqual = true;
                            //提取匹配字段，查找ID与OID
                            string[] array = fields.Split(';');
                            for (i = 0; i < array.Length; i++)
                            {
                                if (!array[i].Contains("("))
                                {
                                    int teIndex = feature.Fields.FindField(array[i]);
                                    int tuIndex = pSrcFeature.Fields.FindField(array[i]);
                                    if (teIndex == -1 || tuIndex == -1)
                                    {
                                        continue;
                                    }
                                    object teObj = feature.get_Value(teIndex);
                                    object tuObj = pSrcFeature.get_Value(tuIndex);

                                    if (Convert.IsDBNull(teObj) && (!Convert.IsDBNull(tuObj)))
                                    {
                                        blnAttriEqual = false;
                                    }
                                    else if (!Convert.IsDBNull(teObj) && (!Convert.IsDBNull(tuObj)))
                                    {
                                        if (feature.get_Value(teIndex).ToString() != pSrcFeature.get_Value(tuIndex).ToString())
                                        {
                                            blnAttriEqual = false;
                                        }
                                        else
                                        {

                                        }
                                    }
                                    else if (!Convert.IsDBNull(teObj) && (Convert.IsDBNull(tuObj)))
                                    {
                                        if (!string.IsNullOrEmpty(feature.get_Value(teIndex).ToString()))
                                        {
                                            blnAttriEqual = false;
                                        }
                                    }

                                }
                                else
                                {
                                    string tef = array[i].Substring(0, array[i].LastIndexOf('('));
                                    string tuf = array[i].Substring(array[i].LastIndexOf('(') + 1);
                                    tuf = tuf.Substring(0, tuf.LastIndexOf(')'));
                                    int teIndex = feature.Fields.FindField(tef);
                                    int tuIndex = pSrcFeature.Fields.FindField(tuf);
                                    if (teIndex == -1 || tuIndex == -1)
                                    {
                                        continue;
                                    }
                                    object teObj = feature.get_Value(teIndex);
                                    object tuObj = pSrcFeature.get_Value(tuIndex);

                                    if (Convert.IsDBNull(teObj) && (!Convert.IsDBNull(tuObj)))
                                    {
                                        blnAttriEqual = false;
                                    }
                                    else if (!Convert.IsDBNull(teObj) && (!Convert.IsDBNull(tuObj)))
                                    {
                                        if (feature.get_Value(teIndex).ToString() != pSrcFeature.get_Value(tuIndex).ToString())
                                        {
                                            blnAttriEqual = false;
                                        }
                                        else
                                        {

                                        }
                                    }
                                    else if (!Convert.IsDBNull(teObj) && (Convert.IsDBNull(tuObj)))
                                    {
                                        if (!string.IsNullOrEmpty(feature.get_Value(teIndex).ToString()))
                                        {
                                            blnAttriEqual = false;
                                        }
                                    }
                                }
                            }
                            if (blnShpEqual == false || blnAttriEqual == false)
                            {
                                ////只图形变化
                                if (blnShpEqual == false && blnAttriEqual)
                                {
                                    intFieldIndex = rowBuffer.Fields.FindField("变化标记");
                                    if (intFieldIndex > -1)
                                    {
                                        //rowBuffer.set_Value(intFieldIndex, "图形变化");
                                        //rowBuffer.set_Value(intFieldIndex, "一对一");
                                        rowBuffer.set_Value(intFieldIndex, ClsConstant.One2One);
                                    }

                                    ////只属性变化
                                }
                                else if (blnShpEqual && blnAttriEqual == false)
                                {
                                    intFieldIndex = rowBuffer.Fields.FindField("变化标记");
                                    if (intFieldIndex > -1)
                                    {
                                        rowBuffer.set_Value(intFieldIndex, "属性变化");
                                        ////修改的打标为6   
                                    }
                                    ////图形属性都变化
                                }
                                else if (blnShpEqual == false && blnAttriEqual == false)
                                {
                                    intFieldIndex = rowBuffer.Fields.FindField("变化标记");
                                    if (intFieldIndex > -1)
                                    {
                                        rowBuffer.set_Value(intFieldIndex, "属性图形变化");
                                        ////修改的打标为2        
                                    }

                                }
                            }
                            else if (blnShpEqual == true & blnAttriEqual == true)
                            {
                                intFieldIndex = rowBuffer.Fields.FindField("变化标记");
                                if (intFieldIndex > -1)
                                {
                                    rowBuffer.set_Value(intFieldIndex, "未变化");
                                }
                            }
                            rowCursor.InsertRow(rowBuffer);
                        }
                    }
                    /**情况3：通过用代码构建缓冲区分析，找到多条对应要素的情况（源图层和待匹配图层要素是1对多的情况），然后判断属性图形是否发生变化。*/
                    else if (pDicCol.Count > 1)
                    {
                        intFieldIndex = rowBuffer.Fields.FindField("变化标记");
                        if (intFieldIndex > -1)
                        {
                            //rowBuffer.set_Value(intFieldIndex, "一对多");
                            rowBuffer.set_Value(intFieldIndex, ClsConstant.One2More);
                            rowCursor.InsertRow(rowBuffer);
                        }
                    }


                    ////进程条处理
                    sbProcessBar(sourceFeatCount, prgMain, prgSub);

                    //每1千个要素就把缓冲区内的要素类写入目标层
                    if (iFeatCount >= 500)
                    {
                        rowCursor.Flush();
                        iFeatCount = 0;
                    }

                    stateLabel.Text = "正在对" + strSrcLyrName + "图层和" + strTarLyrName + "图层进行数据变化分析，已经处理" + prgMain.Value + "(" + sourceFeatCount + ")个要素。";
                    stateLabel.Refresh();
                    prgMain.Update();
                    prgSub.Update();
                    iFeatCount = iFeatCount + 1;
                    pSrcFeature = pSrcCursor.NextFeature();
                }
            }
            catch (System.Exception ex)
            {
                //MessageBox.Show(ex.Message);
                MessageBox.Show(ex.StackTrace);
            }

            if (rowCursor != null)
            {
                rowCursor.Flush();
            }

            workspaceEdit.StopEditOperation();
            workspaceEdit.StopEditing(true);

            stateLabel.Text = "";
            prgMain.Value = 0;
            prgSub.Value = 0;

            return true;
            //return functionReturnValue;

        }


        /// <summary>
        /// 判断分类码是否相同
        /// </summary>
        /// <param name="pSrcFeature">源图层</param>
        /// <param name="pTarFeature">目标图层</param>
        /// <returns></returns>
        private bool MatchCode(IFeature pSrcFeature, IFeature pTarFeature)
        {
            //int TCodeIndex = pTarFeature.Fields.FindField("GCode");
            //int SCodeIndex = pSrcFeature.Fields.FindField("GCode");
            int TCodeIndex = pTarFeature.Fields.FindField("FCODE");
            int SCodeIndex = pSrcFeature.Fields.FindField("FCODE");
            if (TCodeIndex != -1 && SCodeIndex != -1)
            {
                string SstrFirSecCode = string.Empty;
                string TstrFirSecCode = string.Empty;
                string temStr1 = pTarFeature.get_Value(TCodeIndex).ToString();
                string temStr2 = pSrcFeature.get_Value(SCodeIndex).ToString();

                SstrFirSecCode = temStr2.Substring(0, temStr2.IndexOf("_") + 10);
                TstrFirSecCode = temStr1.Substring(0, temStr1.IndexOf("_") + 10);
                if (SstrFirSecCode == TstrFirSecCode)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 反向几何匹配
        /// </summary>
        /// <param name="pSrcFcls">源图层</param>
        /// <param name="lstID"></param>
        /// <param name="pTarFcls">待匹配图层</param>
        /// <param name="resultTable">保存结果的表TRA_PT_I_PtTable</param>
        /// <param name="matchedMode">匹配模式：设置匹配选择方式，0 代表几何匹配，1代表拓扑匹配，2代表属性匹配 </param>
        /// <param name="weight">表MatchedPointFCSetting中的（综合相似度阈值）</param>
        /// <param name="buffer">表MatchedPointFCSetting中的（搜索半径）字段值</param>
        /// <param name="fields">表MatchedPointFCSetting中的（匹配属性）字段值</param>
        /// <param name="prgMain">总进度条</param>
        /// <param name="prgSub">子进度条</param>
        /// <param name="stateLabel">状态信息</param>
        /// <returns></returns>
        public bool ReverseSearchChangedFeatures(IFeatureClass pSrcFcls, List<int> lstID, IFeatureClass pTarFcls, ITable resultTable, int matchedMode, double[] weight, double buffer, string fields, ProgressBar prgMain, ProgressBar prgSub, LabelX stateLabel)
        {
            bool functionReturnValue = false;
            IFeatureCursor pTarCursor = null;//工作区数据指针

            IFeature pSrcFeature = null;
            IFeature pTarFeature = null;

            IRelationalOperator pRelOp;
            bool blnShpEqual = false;  //形状匹配
            bool blnAttriEqual = false;//属性匹配
            int i = 0;
            int intFieldIndex = -1;
            ISpatialFilter pSFilter;

            int tuFeatCount = 0; //源图层要素个数
            int lFeatcount = 0;  //临时层要素个数
            int lIdx = 0;

            Dictionary<int, IFeature> pDicCol;//找到的与源要素有关的要素
            Dictionary<int, IFeature> pDicChangeF = new Dictionary<int, IFeature>();

            int lOutFldIndex = 0;
            ////数据源和目标数据要素类字段索引
            string strSrcLyrName = "";
            string strTarLyrName = "";
            IDataset pDst;//临时变量，用于获取图层名
            int iFeatCount = 0; //处理的临时要素

            try
            {
                ////进入编辑状态
                IDataset dataset = resultTable as IDataset;//匹配表
                IWorkspace workspace = dataset.Workspace;
                IWorkspaceEdit workspaceEdit = workspace as IWorkspaceEdit;
                workspaceEdit.StartEditing(true);
                workspaceEdit.StartEditOperation();


                tuFeatCount = pSrcFcls.FeatureCount(null);
                //TU
                //pSrcCursor = pSrcFcls.Search(null, false);

                //设置进度条
                if (tuFeatCount > 0)
                {
                    prgMain.Maximum = tuFeatCount;
                }

                prgSub.Maximum = 100;
                prgSub.Value = 0;
                prgMain.Value = 0;

                //if ((pSrcCursor != null))
                //{
                //    pSrcFeature = pSrcCursor.NextFeature();
                //}

                //指示属性值是否相等
                blnAttriEqual = true;
                //设置图层名
                pDst = pSrcFcls as IDataset; //原图层数据集
                strSrcLyrName = pDst.Name; //原图层名
                pDst = pTarFcls as IDataset;
                strTarLyrName = pDst.Name;//工作层名
                //pDst = pResultFcls as IDataset;
                //strResultLyrName = pDst.Name;
                pDst = null;

                IRowBuffer rowBuffer = null;
                ICursor rowCursor = null;

                //源要素总的循环
                //while (pSrcFeature != null)
                for (int ii = 0; ii < lstID.Count; ii++)
                {
                    pSrcFeature = pSrcFcls.GetFeature(lstID[ii]);

                    rowBuffer = resultTable.CreateRowBuffer();
                    rowCursor = resultTable.Insert(true);

                    pDicCol = new Dictionary<int, IFeature>();

                    pSFilter = new SpatialFilter();
                    if (pSrcFeature.Shape.GeometryType == esriGeometryType.esriGeometryPolyline)
                    {
                        ITopologicalOperator top = pSrcFeature.Shape as ITopologicalOperator;
                        pSFilter.Geometry = top.Buffer(buffer) as IGeometry;
                        //自定义空间查询,减少查询次数,提高查询速度
                        pSFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                    }
                    else if (pSrcFeature.Shape.GeometryType == esriGeometryType.esriGeometryPolygon)
                    {
                        pSFilter.Geometry = pSrcFeature.Shape;//查询边界形状为多边形
                        pSFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelRelation;//函数用法
                        pSFilter.SpatialRelDescription = "T********";
                    }

                    //得到对应pSrcFeature的TE图层上的要素游标
                    pTarCursor = pTarFcls.Search(pSFilter, false);//查询在原图层多边形边界处的工作层的元素

                    ////得到对应pSrcFeature的TE图层上的要素个数
                    lFeatcount = pTarFcls.FeatureCount(pSFilter);
                    pTarFeature = pTarCursor.NextFeature();
                    lIdx = 0;
                    //*******************************************************************************
                    //算法说明:
                    //获取相交面的面积或相交线的长度.最后以P匹配参数来找出与pSrcFeature
                    //最匹配的pTarFeature,其中A表示pTarFeature的面积或长度,B表示pSrcFeature的面积或长度.
                    //当P值最大时,此时的pTarFeature即为找到的要素.
                    //*******************************************************************************
                    //目标次要素循环
                    rowBuffer.set_Value(2, pSrcFeature.OID);
                    while ((pTarFeature != null))
                    {
                        if ((pTarFeature.Shape != null))
                        {
                            if (pTarFeature.Shape.GeometryType == esriGeometryType.esriGeometryPolygon)
                            {

                                if (MatchCode(pSrcFeature, pTarFeature))
                                {
                                    rowBuffer.set_Value(4, pSrcFeature.get_Value(pSrcFeature.Fields.FindField("GCode")));
                                    rowBuffer.set_Value(5, pTarFeature.get_Value(pTarFeature.Fields.FindField("GCode")));
                                }

                        
                                double center = 0;
                                double area = 0;
                                double shape = 0;
                                double polygonRatio = 0;

                                //判断匹配方法

                                shape = ClsIndicatorFunStatic.ShapeSimilarValue(pSrcFeature, pTarFeature);
                                area = ClsIndicatorFunStatic.AreaSimilarValue(pSrcFeature, pTarFeature);
                                center = ClsIndicatorFunStatic.CenterSimilarValue(pSrcFeature, pTarFeature);
                                polygonRatio = shape * weight[0] + area * weight[2] + center * weight[1];
                                string shape1 = string.Format("{0:0.00000000}", shape);
                                string area1 = string.Format("{0:0.00000000}", area);
                                string center1 = string.Format("{0:0.00000000}", center);
                                string polygonRatio1 = string.Format("{0:0.00000000}", polygonRatio);
                                rowBuffer.set_Value(6, center1);
                                rowBuffer.set_Value(7, area1);
                                rowBuffer.set_Value(8, shape1);
                                rowBuffer.set_Value(9, polygonRatio1);

                                int index = 0;
                                if (polygonRatio > weight[3])
                                {
                                    if (rowBuffer.get_Value(1).ToString() == "")
                                    {
                                        rowBuffer.set_Value(1, pTarFeature.get_Value(index));
                                    }
                                    else
                                    {
                                        string oids = rowBuffer.get_Value(1).ToString() + ";" + pTarFeature.get_Value(index);
                                        rowBuffer.set_Value(1, oids);
                                    }
                                    pDicCol.Add(lIdx, pTarFeature);
                                    lIdx = lIdx + 1;
                                }
                            }
                            else if (pTarFeature.Shape.GeometryType == esriGeometryType.esriGeometryPolyline)
                            {
                                if ((pSrcFeature.Shape != null))
                                {
                                    double matchedPoints = 0;
                                    double shape = 0;
                                    double polylineRadio = 0;
                                    int index = pTarFeature.Fields.FindField("OBJECTID");
                                    string oid = pTarFeature.get_Value(index).ToString();
                                    if (matchedMode == 1)
                                    {
                                        shape = ClsIndicatorFunStatic.PolylineShapeSimilarValue(pSrcFeature, pTarFeature);
                                        polylineRadio = shape;

                                        if (polylineRadio > weight[0])
                                        {
                                            if (rowBuffer.get_Value(1).ToString() == "")
                                            {
                                                rowBuffer.set_Value(1, pTarFeature.get_Value(index));
                                            }
                                            else
                                            {
                                                string oids = rowBuffer.get_Value(1).ToString() + ";" + pTarFeature.get_Value(index);
                                                rowBuffer.set_Value(1, oids);
                                            }
                                            pDicCol.Add(lIdx, pTarFeature);
                                            lIdx = lIdx + 1;
                                        }

                                    }
                                    else if (matchedMode == 2)
                                    {
                                        matchedPoints = ClsIndicatorFunStatic.MatchedPointsSimilarValue(pSrcFeature, pTarFeature,buffer);
                                        polylineRadio = matchedPoints;

                                        if (polylineRadio > weight[1])
                                        {
                                            if (rowBuffer.get_Value(1).ToString() == "")
                                            {
                                                rowBuffer.set_Value(1, pTarFeature.get_Value(index));
                                            }
                                            else
                                            {
                                                string oids = rowBuffer.get_Value(1).ToString() + ";" + pTarFeature.get_Value(index);
                                                rowBuffer.set_Value(1, oids);
                                            }
                                            pDicCol.Add(lIdx, pTarFeature);
                                            lIdx = lIdx + 1;
                                        }
                                    }

                                }
                            }
                            else if (pTarFeature.Shape.GeometryType == esriGeometryType.esriGeometryPoint)
                            {
                                break;
                            }
                        }
                        pTarFeature = pTarCursor.NextFeature();
                    }
                    //***===方法2(自定义空间查询,减少查询次数提高查询速度)==End
                    //// 找不到对应要素的情况新增的情况。遍历TU找TE中的要素，与以前项目的更新程序查找相反。

                    if (pDicCol.Count == 0)
                    {

                        lOutFldIndex = rowBuffer.Fields.FindField("变化标记");
                        if (lOutFldIndex > -1)
                        {
                            rowBuffer.set_Value(lOutFldIndex, "新增要素");
                            ////表示新增
                        }
                        rowCursor.InsertRow(rowBuffer);
                    }

                    //*******************************************************************************
                    //// 找到对应要素的情况，然后判断属性图形是否发生变化
                    //*******************************************************************************

                    if (pDicCol.Count == 1)
                    {
                        IFeature feature = pDicCol[0] as IFeature;
                        ////判断形状是否发生变化，如果没有变化blnShpEqual设为True
                        if (pSrcFeature.Shape != null)
                        {
                            pRelOp = pSrcFeature.Shape as IRelationalOperator;
                            blnShpEqual = pRelOp.Equals(feature.Shape);

                            ////判断线方向性变化
                            blnAttriEqual = true;
                            //提取匹配字段，查找ID与OID
                            string[] array = fields.Split(';');
                            for (i = 0; i < array.Length; i++)
                            {
                                if (!array[i].Contains("("))
                                {
                                    int teIndex = feature.Fields.FindField(array[i]);
                                    int tuIndex = pSrcFeature.Fields.FindField(array[i]);
                                    if (teIndex == -1 || tuIndex == -1)
                                    {
                                        continue;
                                    }
                                    object teObj = feature.get_Value(teIndex);
                                    object tuObj = pSrcFeature.get_Value(tuIndex);

                                    if (Convert.IsDBNull(teObj) && (!Convert.IsDBNull(tuObj)))
                                    {
                                        blnAttriEqual = false;
                                    }
                                    else if (!Convert.IsDBNull(teObj) && (!Convert.IsDBNull(tuObj)))
                                    {
                                        if (feature.get_Value(teIndex).ToString() != pSrcFeature.get_Value(tuIndex).ToString())
                                        {
                                            blnAttriEqual = false;
                                        }
                                        else
                                        {

                                        }
                                    }
                                    else if (!Convert.IsDBNull(teObj) && (Convert.IsDBNull(tuObj)))
                                    {
                                        if (!string.IsNullOrEmpty(feature.get_Value(teIndex).ToString()))
                                        {
                                            blnAttriEqual = false;
                                        }
                                    }

                                }
                                else
                                {
                                    string tef = array[i].Substring(0, array[i].LastIndexOf('('));
                                    string tuf = array[i].Substring(array[i].LastIndexOf('(') + 1);
                                    tuf = tuf.Substring(0, tuf.LastIndexOf(')'));
                                    int teIndex = feature.Fields.FindField(tef);
                                    int tuIndex = pSrcFeature.Fields.FindField(tuf);
                                    if (teIndex == -1 || tuIndex == -1)
                                    {
                                        continue;
                                    }
                                    object teObj = feature.get_Value(teIndex);
                                    object tuObj = pSrcFeature.get_Value(tuIndex);

                                    if (Convert.IsDBNull(teObj) && (!Convert.IsDBNull(tuObj)))
                                    {
                                        blnAttriEqual = false;
                                    }
                                    else if (!Convert.IsDBNull(teObj) && (!Convert.IsDBNull(tuObj)))
                                    {
                                        if (feature.get_Value(teIndex).ToString() != pSrcFeature.get_Value(tuIndex).ToString())
                                        {
                                            blnAttriEqual = false;
                                        }
                                        else
                                        {

                                        }
                                    }
                                    else if (!Convert.IsDBNull(teObj) && (Convert.IsDBNull(tuObj)))
                                    {
                                        if (!string.IsNullOrEmpty(feature.get_Value(teIndex).ToString()))
                                        {
                                            blnAttriEqual = false;
                                        }
                                    }
                                }
                            }
                            if (blnShpEqual == false || blnAttriEqual == false)
                            {
                                ////只图形变化
                                if (blnShpEqual == false && blnAttriEqual)
                                {
                                    intFieldIndex = rowBuffer.Fields.FindField("变化标记");
                                    if (intFieldIndex > -1)
                                    {
                                        rowBuffer.set_Value(intFieldIndex, "图形变化");
                                    }

                                    ////只属性变化
                                }
                                else if (blnShpEqual && blnAttriEqual == false)
                                {
                                    intFieldIndex = rowBuffer.Fields.FindField("变化标记");
                                    if (intFieldIndex > -1)
                                    {
                                        rowBuffer.set_Value(intFieldIndex, "属性变化");
                                        ////修改的打标为6   
                                    }
                                    ////图形属性都变化
                                }
                                else if (blnShpEqual == false && blnAttriEqual == false)
                                {
                                    intFieldIndex = rowBuffer.Fields.FindField("变化标记");
                                    if (intFieldIndex > -1)
                                    {
                                        rowBuffer.set_Value(intFieldIndex, "属性图形变化");
                                        ////修改的打标为2        
                                    }

                                }
                            }
                            else if (blnShpEqual == true & blnAttriEqual == true)
                            {
                                intFieldIndex = rowBuffer.Fields.FindField("变化标记");
                                if (intFieldIndex > -1)
                                {
                                    rowBuffer.set_Value(intFieldIndex, "未变化");
                                }
                            }
                            rowCursor.InsertRow(rowBuffer);
                        }
                    }
                    else if (pDicCol.Count > 1)
                    {
                        intFieldIndex = rowBuffer.Fields.FindField("变化标记");
                        if (intFieldIndex > -1)
                        {
                            rowBuffer.set_Value(intFieldIndex, "一对多");
                            rowCursor.InsertRow(rowBuffer);
                        }
                    }
                    ////进程条处理
                    sbProcessBar(tuFeatCount, prgMain, prgSub);
                    //每1千个要素就把缓冲区内的要素类写入目标层
                    if (iFeatCount >= 500)
                    {
                        rowCursor.Flush();
                        iFeatCount = 0;
                    }

                    stateLabel.Text = "正在对" + strSrcLyrName + "图层和" + strTarLyrName + "图层进行数据变化分析，已经处理" + prgMain.Value + "(" + tuFeatCount + ")个要素。";
                    stateLabel.Refresh();
                    prgMain.Update();
                    prgSub.Update();
                    iFeatCount = iFeatCount + 1;
                    //pSrcFeature = pSrcCursor.NextFeature();
                }
                if (rowCursor != null)
                {
                    rowCursor.Flush();
                }

                workspaceEdit.StopEditOperation();
                workspaceEdit.StopEditing(true);

                //MessageBoxEx.Show("匹配已完成！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                stateLabel.Text = "";
                prgMain.Value = 0;
                prgSub.Value = 0;

                return true;
            }
            catch (Exception)
            {

            }
            return functionReturnValue;

        }
        #region 拓扑匹配
        /// <summary>
        /// 拓扑匹配（点）
        /// </summary>
        /// <param name="pSrcFcls">源图层</param>
        /// <param name="pTarFcls">目标图层</param>
        /// <param name="resultTable">保存结果的表TRA_PT_I_PtTable</param>
        /// <param name="buffer">表MatchedPointFCSetting中的（搜索半径）字段值</param>
        /// <param name="fields">表MatchedPointFCSetting中的（匹配属性）字段值</param>
        /// <param name="prgMain">总进度条</param>
        /// <param name="prgSub">子进度条</param>
        /// <param name="stateLabel">状态标签</param>
        /// <returns></returns>
        public bool SearchChangedFeaturesByTop(IFeatureClass pSrcFcls, IFeatureClass pTarFcls, ITable resultTable, double buffer, string fields, ProgressBar prgMain, ProgressBar prgSub, LabelX stateLabel)
        {
            bool functionReturnValue = false;

            IFeatureCursor pSrcCursor = null;
            IFeatureCursor pTarCursor = null;

            IFeature pSrcFeature = null;
            IFeature pTarFeature = null;

            IRelationalOperator pRelOp;
            bool blnShpEqual = false;
            bool blnAttriEqual = false;
            int i = 0;

            //变化标记索引
            int intFieldIndex = -1;
            ISpatialFilter pSFilter;

            int sourceFeatCount = 0;
            int targetFeatcount = 0;
            int lIdx = 0;

            Dictionary<int, IFeature> pDicCol;
            //Dictionary<string, string> pDicChangedTarOID = new Dictionary<string, string>();
            //Collection<IFeature> pColChanged = new Collection<IFeature>();

            //IPolygon pPoy;
            //IPolyline pPl1;
            //IPolyline pPl2;

            //int lInFldIndex = 0;

            //变化标记的索引
            int changeTagIndex = 0;
            ////数据源和目标数据要素类字段索引
            string strSrcLyrName = "";
            string strTarLyrName = "";
            IDataset pDst;
            int iFeatCount = 0;

            try
            {
                ////进入编辑状态
                IDataset dataset = resultTable as IDataset;
                IWorkspace workspace = dataset.Workspace;
                IWorkspaceEdit workspaceEdit = workspace as IWorkspaceEdit;
                workspaceEdit.StartEditing(true);
                workspaceEdit.StartEditOperation();

                sourceFeatCount = pSrcFcls.FeatureCount(null);
                //TU
                pSrcCursor = pSrcFcls.Search(null, false);

                //设置进度条
                if (sourceFeatCount > 0)
                {
                    prgMain.Maximum = sourceFeatCount;
                }

                prgSub.Maximum = 100;
                prgSub.Value = 0;
                prgMain.Value = 0;

                if (pSrcCursor != null)
                {
                    pSrcFeature = pSrcCursor.NextFeature();
                }

                //指示属性值是否相等
                blnAttriEqual = true;
                //设置图层名
                pDst = pSrcFcls as IDataset;
                strSrcLyrName = pDst.Name;
                pDst = pTarFcls as IDataset;
                strTarLyrName = pDst.Name;
                //pDst = pResultFcls as IDataset;
                //strResultLyrName = pDst.Name;
                pDst = null;

                IRowBuffer rowBuffer = null;
                ICursor rowCursor = null;
                while (pSrcFeature != null)
                {
                    rowBuffer = resultTable.CreateRowBuffer();
                    rowCursor = resultTable.Insert(true);

                    pDicCol = new Dictionary<int, IFeature>();

                    pSFilter = new SpatialFilter();
                    if (pSrcFeature.Shape.GeometryType == esriGeometryType.esriGeometryPolyline)
                    {
                        ITopologicalOperator top = pSrcFeature.Shape as ITopologicalOperator;
                        pSFilter.Geometry = top.Buffer(buffer) as IGeometry;
                        //自定义空间查询,减少查询次数,提高查询速度
                        //设置缓冲区过滤条件：相交
                        pSFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                    }
                    else if (pSrcFeature.Shape.GeometryType == esriGeometryType.esriGeometryPolygon)
                    {
                        pSFilter.Geometry = pSrcFeature.Shape;
                        //A与B空间关联
                        pSFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelRelation;
                        pSFilter.SpatialRelDescription = "T********";
                    }
                    else if (pSrcFeature.Shape.GeometryType == esriGeometryType.esriGeometryPoint)
                    {
                        //创建缓冲区
                        ITopologicalOperator top = pSrcFeature.Shape as ITopologicalOperator;
                        pSFilter.Geometry = top.Buffer(buffer) as IGeometry;
                        //自定义空间查询,建立缓冲区
                        //设置缓冲区过滤条件：包含
                        pSFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;
                    }
                    //得到对应pSrcFeature的TE图层上的要素游标
                    pTarCursor = pTarFcls.Search(pSFilter, false);
                    ////得到对应pSrcFeature的TE图层上的要素个数
                    targetFeatcount = pTarFcls.FeatureCount(pSFilter);
                    pTarFeature = pTarCursor.NextFeature();

                    lIdx = 0;
                    #region 算法说明
                    //*******************************************************************************
                    //算法说明:
                    //获取相交面的面积或相交线的长度.最后以P匹配参数来找出与pSrcFeature
                    //最匹配的pTarFeature,其中A表示pTarFeature的面积或长度,B表示pSrcFeature的面积或长度.
                    //当P值最大时,此时的pTarFeature即为找到的要素.
                    //*******************************************************************************
                    #endregion
                    rowBuffer.set_Value(rowBuffer.Fields.FindField("源OID"), pSrcFeature.OID);

                    while ((pTarFeature != null))
                    {
                        //int index = pTarFeature.Fields.FindField("FIXOID");
                        //int index = 0;
                        if ((pTarFeature.Shape != null))
                        {
                            if (pTarFeature.Shape.GeometryType == esriGeometryType.esriGeometryPoint)
                            {
                                //string testfieldName1 = ClsConfig.LayerConfigs[(pSrcFcls  as IDataset).Name].NameField;
                                //string pSrcStrName = pSrcFeature.get_Value(pSrcFeature.Fields.FindFieldByAliasName("名称")).ToString();
                                string pSrcStrName = pSrcFeature.get_Value(pSrcFeature.Fields.FindField(ClsConfig.LayerConfigs[(pSrcFcls as IDataset).Name].NameField)).ToString();

                                //string pTarStrName = pTarFeature.get_Value(pTarFeature.Fields.FindFieldByAliasName("名称")).ToString();
                                //string testfieldName = ClsConfig.LayerConfigs[(pTarFcls as IDataset).Name].NameField;
                                string pTarStrName = pTarFeature.get_Value(pTarFeature.Fields.FindField(ClsConfig.LayerConfigs[(pTarFcls as IDataset).Name].NameField)).ToString();

                                //test
                                int test = StringSameOrNot(pSrcStrName, pTarStrName);
                                //if (StringSameOrNot(pSrcStr, pTarStr) > 0)
                                if (pSrcStrName.Length > 0 && pTarStrName.Length > 0 && StringSameOrNot(pSrcStrName, pTarStrName) > 1)
                                {
                                    //index=0代表待匹配feature的第一个字段oid
                                    int index = 0;

                                    if (rowBuffer.get_Value(rowBuffer.Fields.FindField("待匹配OID")).ToString() == "")
                                    {
                                        rowBuffer.set_Value(rowBuffer.Fields.FindField("待匹配OID"), pTarFeature.get_Value(index));
                                    }
                                    else
                                    {
                                        string oids = rowBuffer.get_Value(rowBuffer.Fields.FindField("待匹配OID")).ToString() + ";" + pTarFeature.get_Value(index);
                                        rowBuffer.set_Value(rowBuffer.Fields.FindField("待匹配OID"), oids);
                                    }

                                    //设置表TRA_PT_I_PtTabl的（源编码）字段的值——cell[4]
                                    rowBuffer.set_Value(rowBuffer.Fields.FindField("源图层名称"), pSrcStrName);
                                    //设置表TRA_PT_I_PtTabl的（待匹配编码）字段的值——cell[5]
                                    rowBuffer.set_Value(rowBuffer.Fields.FindField("待匹配图层名称"), pTarStrName);

                             
                                    double distance = 0;
                                    distance = ClsIndicatorFunStatic.EuclideanMetricDistance(pSrcFeature, pTarFeature);
                                    string distance1 = string.Format("{0:0.0000}", 1 - distance);
                                    //设置表TRA_PT_I_PtTabl的（位置相似度）字段的值——cell[6]
                                    rowBuffer.set_Value(rowBuffer.Fields.FindField("位置相似度"), distance1);

                                    pDicCol.Add(lIdx, pTarFeature);
                                    lIdx = lIdx + 1;
                                }

                            }
                            else if (pTarFeature.Shape.GeometryType == esriGeometryType.esriGeometryPolyline)
                            {
                                #region 算法说明
                                //20170519
                                //拓扑匹配只是粗略地筛选出在缓冲区范围内的要素，要找到最匹配的要素，
                                //还需要在拓扑匹配的基础上进一步地进行几何或者拓扑相似性评价。

                                /*通过缓冲区增长法得到待匹配弧段的匹配候选集，匹配候选集中可能存在不止一个候
                                    选弧段对象，通过对待匹配弧段与每一个候选匹配弧段进行几何或者拓扑相似性评价，选
                                    取相似性最高的那一组作为最终的匹配结果。*/
                                #endregion
                                //test
                                object testobj = pTarFeature.OID;

                                string pSrcStr = pSrcFeature.get_Value(pSrcFeature.Fields.FindFieldByAliasName("名称")).ToString();
                                string pTarStr = pTarFeature.get_Value(pTarFeature.Fields.FindFieldByAliasName("名称")).ToString();
                                //test
                                int test = StringSameOrNot2(pSrcStr, pTarStr);

                                //都存在名称字段，并且名称匹配的超过2个字符
                                if (pSrcStr.Length > 0 && pTarStr.Length > 0 && StringSameOrNot2(pSrcStr, pTarStr) > 2)
                                {
                                    #region 不能写位置相似度，因为是现状实体，所以必须转换为一个代表性的点再计算
                                    ////新增
                                    //ClsIndicatorFun clsMatching = new ClsIndicatorFun();
                                    //clsMatching.SourceFeature = pSrcFeature;
                                    //clsMatching.TargetFeature = pTarFeature;

                                    //double distance = 0;
                                    //distance = clsMatching.PointDistance();
                                    //string distance1 = string.Format("{0:0.0000}", 1 - distance);
                                    ////设置表TRA_PT_I_PtTabl的（位置相似度）字段的值——cell[6]
                                    //rowBuffer.set_Value(6, distance1);
                                    #endregion
                                    int index = 0;

                                    if (rowBuffer.get_Value(rowBuffer.Fields.FindField("待匹配OID")).ToString() == "")
                                    {
                                        rowBuffer.set_Value(rowBuffer.Fields.FindField("待匹配OID"), pTarFeature.get_Value(index));
                                    }
                                    else
                                    {
                                        string oids = rowBuffer.get_Value(rowBuffer.Fields.FindField("待匹配OID")).ToString() + ";" + pTarFeature.get_Value(index);
                                        rowBuffer.set_Value(rowBuffer.Fields.FindField("待匹配OID"), oids);
                                    }

                                    //设置表TRA_PT_I_PtTabl的（源名称）字段的值——cell[4]
                                    rowBuffer.set_Value(rowBuffer.Fields.FindField("源图层名称"), pSrcFeature.get_Value(pSrcFeature.Fields.FindFieldByAliasName("名称")));
                                    //设置表TRA_PT_I_PtTabl的（待匹配名称）字段的值——cell[5]
                                    rowBuffer.set_Value(rowBuffer.Fields.FindField("待匹配图层名称"), pTarFeature.get_Value(pTarFeature.Fields.FindFieldByAliasName("名称")));

                                    pDicCol.Add(lIdx, pTarFeature);
                                    lIdx = lIdx + 1;
                                }
                            }
                            else if (pTarFeature.Shape.GeometryType == esriGeometryType.esriGeometryPolygon)
                            {

                            }
                        }
                        pTarFeature = pTarCursor.NextFeature();
                    }

                    //情况1：源图层要素缓冲区范围内没有目标图层的要素
                    if (pDicCol.Count == 0)
                    {
                        changeTagIndex = rowBuffer.Fields.FindField("变化标记");
                        if (changeTagIndex > -1)
                        {
                            ////表示新增
                            rowBuffer.set_Value(changeTagIndex, "新增要素");
                        }
                        rowCursor.InsertRow(rowBuffer);
                    }


                    //情况2：源图层要素缓冲区范围内有1个目标图层的要素
                    //找到对应要素的情况，然后判断属性图形是否发生变化
                    if (pDicCol.Count == 1)
                    {
                        IFeature feature = pDicCol[0] as IFeature;
                        ////判断形状是否发生变化，如果没有变化blnShpEqual设为True
                        if (pSrcFeature.Shape != null)
                        {
                            pRelOp = pSrcFeature.Shape as IRelationalOperator;
                            blnShpEqual = pRelOp.Equals(feature.Shape);

                            //判断线方向性变化
                            if (blnShpEqual == true)
                            {
                                //if (pTarFcls.ShapeType == esriGeometryType.esriGeometryPolyline)
                                //{
                                //    pPl1 = pSrcFeature.Shape as IPolyline;
                                //    pPl2 = feature.Shape as IPolyline;
                                //    if (pPl1.FromPoint.X != pPl2.FromPoint.X || pPl2.FromPoint.Y != pPl1.FromPoint.Y)
                                //    {
                                //        blnShpEqual = false;
                                //    }
                                //}

                            }
                            blnAttriEqual = true;

                            //提取匹配字段
                            string[] array = fields.Split(';');
                            for (i = 0; i < array.Length; i++)
                            {
                                if (!array[i].Contains("("))
                                {

                                    int teIndex = feature.Fields.FindField(array[i]);
                                    int tuIndex = pSrcFeature.Fields.FindField(array[i]);

                                    if (teIndex == -1 || tuIndex == -1)
                                    {
                                        continue;
                                    }
                                    object teObj = feature.get_Value(teIndex);
                                    object tuObj = pSrcFeature.get_Value(tuIndex);

                                    if (Convert.IsDBNull(teObj) && (!Convert.IsDBNull(tuObj)))
                                    {
                                        blnAttriEqual = false;
                                    }
                                    else if (!Convert.IsDBNull(teObj) && (!Convert.IsDBNull(tuObj)))
                                    {
                                        if (feature.get_Value(teIndex).ToString() != pSrcFeature.get_Value(tuIndex).ToString())
                                        {
                                            blnAttriEqual = false;
                                        }
                                    }
                                    else if (!Convert.IsDBNull(teObj) && (Convert.IsDBNull(tuObj)))
                                    {
                                        if (!string.IsNullOrEmpty(feature.get_Value(teIndex).ToString()))
                                        {
                                            blnAttriEqual = false;
                                        }
                                    }

                                }
                                else
                                {
                                    string tef = array[i].Substring(0, array[i].LastIndexOf('('));
                                    string tuf = array[i].Substring(array[i].LastIndexOf('(') + 1);
                                    tuf = tuf.Substring(0, tuf.LastIndexOf(')'));
                                    int teIndex = feature.Fields.FindField(tef);
                                    int tuIndex = pSrcFeature.Fields.FindField(tuf);

                                    if (teIndex == -1 || tuIndex == -1)
                                    {
                                        continue;
                                    }
                                    object teObj = feature.get_Value(teIndex);
                                    object tuObj = pSrcFeature.get_Value(tuIndex);

                                    if (Convert.IsDBNull(teObj) && (!Convert.IsDBNull(tuObj)))
                                    {
                                        blnAttriEqual = false;
                                    }
                                    else if (!Convert.IsDBNull(teObj) && (!Convert.IsDBNull(tuObj)))
                                    {
                                        if (feature.get_Value(teIndex).ToString() != pSrcFeature.get_Value(tuIndex).ToString())
                                        {
                                            blnAttriEqual = false;
                                        }
                                    }
                                    else if (!Convert.IsDBNull(teObj) && (Convert.IsDBNull(tuObj)))
                                    {
                                        if (!string.IsNullOrEmpty(feature.get_Value(teIndex).ToString()))
                                        {
                                            blnAttriEqual = false;
                                        }
                                    }
                                }
                            }
                            #region 原文注释
                            //////判断属性值是否发生变化，如果变化blnAttriEqual设为false
                            //for (i = 0; i < feature.Fields.FieldCount; i++)
                            //{
                            //    if (feature.Fields.get_Field(i).Editable == true && feature.Fields.get_Field(i).Name != "SHAPE")
                            //    {
                            //        intFieldIndex = pSrcFeature.Fields.FindField(feature.Fields.get_Field(i).Name);
                            //        if (intFieldIndex > -1)
                            //        {
                            //            ////目标字段(E)值为NULL,源字段(U)值为空字符串值,目标字段类型为字符串类型
                            //            //If (IsDBNull(pTarFeature.Value(i)) And Trim(pSrcFeature.Value(intFieldIndex)) = "" And pTarFeature.Fields.Field(i).Type = esriFieldTypeString) Then
                            //            //    '//目标字段值为空字符串值,源字段值为NULL,目标字段类型为字符串类型
                            //            //ElseIf (Trim(pTarFeature.Value(i)) = "" And IsDBNull(pSrcFeature.Value(intFieldIndex)) And pTarFeature.Fields.Field(i).Type = esriFieldTypeString) Then

                            //            ////目标字段值为NULL值,源字段值为非NULL并且不为空字符串
                            //            object obj1 = feature.get_Value(i);
                            //            object obj2 = pSrcFeature.get_Value(intFieldIndex);

                            //            if (Convert.IsDBNull(obj1) && (!Convert.IsDBNull(obj2)))
                            //            {
                            //                ////FIXID不发生变化
                            //                if (feature.Fields.get_Field(i).Name != "FIXOID" && feature.Fields.get_Field(i).Name != "变化标记")
                            //                {
                            //                    blnAttriEqual = false;
                            //                }
                            //                ////目标字段值为非NULL值,源字段值为非NULL

                            //            }
                            //            else if (!Convert.IsDBNull(obj1) && !Convert.IsDBNull(obj2))
                            //            {
                            //                if (feature.get_Value(i).ToString() != pSrcFeature.get_Value(intFieldIndex).ToString())
                            //                {
                            //                    ////FIXID不发生变化
                            //                    if (feature.Fields.get_Field(i).Name != "FIXOID" && feature.Fields.get_Field(i).Name != "变化标记")
                            //                    {
                            //                        blnAttriEqual = false;
                            //                    }
                            //                    ////字段值相同暂时放到结果集里面(两者都为null时不作处理，结果自然也是NUll)
                            //                }
                            //                else
                            //                {

                            //                }

                            //                ////目标字段值为非NULL值且不为空字符串值,源字段值为NULL
                            //            }
                            //            else if (!Convert.IsDBNull(obj1) && Convert.IsDBNull(obj2))
                            //            {
                            //                object temobj1 = feature.get_Value(i);
                            //                if (!string.IsNullOrEmpty(temobj1.ToString()))
                            //                {
                            //                    if (feature.Fields.get_Field(i).Name != "FIXOID" && feature.Fields.get_Field(i).Name != "变化标记")
                            //                    {
                            //                        blnAttriEqual = false;
                            //                    }
                            //                }

                            //            }

                            //        }

                            //    }

                            //}
                            #endregion

                            if (blnShpEqual == false || blnAttriEqual == false)
                            {
                                ////只图形变化
                                if (blnShpEqual == false && blnAttriEqual)
                                {
                                    intFieldIndex = rowBuffer.Fields.FindField("变化标记");
                                    if (intFieldIndex > -1)
                                    {
                                        // 20170906
                                        //rowBuffer.set_Value(intFieldIndex, "图形变化");
                                        rowBuffer.set_Value(intFieldIndex, "一对一");
                                    }
                                    //pColChanged.Add(pSrcFeature);
                                    //pDicChangedTarOID.Add(Convert.ToString(feature.OID), Convert.ToString(feature.OID));
                                    ////只属性变化
                                }
                                else if (blnShpEqual && blnAttriEqual == false)
                                {
                                    intFieldIndex = rowBuffer.Fields.FindField("变化标记");
                                    if (intFieldIndex > -1)
                                    {
                                        rowBuffer.set_Value(intFieldIndex, "属性变化");
                                        ////修改的打标为6   
                                    }
                                    ////图形属性都变化
                                }
                                else if (blnShpEqual == false && blnAttriEqual == false)
                                {
                                    intFieldIndex = rowBuffer.Fields.FindField("变化标记");
                                    if (intFieldIndex > -1)
                                    {
                                        rowBuffer.set_Value(intFieldIndex, "属性图形变化");
                                        ////修改的打标为2        
                                    }
                                    //pColChanged.Add(pSrcFeature);
                                    //pDicChangedTarOID.Add(Convert.ToString(feature.OID), Convert.ToString(feature.OID));
                                }
                            }
                            else if (blnShpEqual == true & blnAttriEqual == true)
                            {
                                intFieldIndex = rowBuffer.Fields.FindField("变化标记");
                                if (intFieldIndex > -1)
                                {
                                    rowBuffer.set_Value(intFieldIndex, "未变化");
                                }
                            }
                            rowCursor.InsertRow(rowBuffer);
                        }
                    }


                    //情况3：源图层要素缓冲区范围内有多个个目标图层的要素
                    else if (pDicCol.Count > 1)
                    {
                        intFieldIndex = rowBuffer.Fields.FindField("变化标记");
                        if (intFieldIndex > -1)
                        {
                            rowBuffer.set_Value(intFieldIndex, "一对多");
                            rowCursor.InsertRow(rowBuffer);
                        }
                    }

                    ////进程条处理
                    sbProcessBar(sourceFeatCount, prgMain, prgSub);
                    //每1千个要素就把缓冲区内的要素类写入目标层
                    if (iFeatCount >= 500)
                    {
                        rowCursor.Flush();
                        iFeatCount = 0;
                    }

                    stateLabel.Text = "正在对" + strSrcLyrName + "图层和" + strTarLyrName + "图层进行数据变化分析，已经处理" + prgMain.Value + "(" + sourceFeatCount + ")个要素。";
                    stateLabel.Refresh();
                    prgMain.Update();
                    prgSub.Update();
                    iFeatCount = iFeatCount + 1;
                    pSrcFeature = pSrcCursor.NextFeature();
                }


                if (rowCursor != null)
                {
                    rowCursor.Flush();
                }

                workspaceEdit.StopEditOperation();
                workspaceEdit.StopEditing(true);

                //MessageBox.Show("拓扑匹配已完成！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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


        #endregion
        /// <summary>
        /// 两字符串中有几个相同字符
        /// </summary>
        /// <param name="fir">第一个字符串</param>
        /// <param name="sec">第二个字符串</param>
        /// <returns></returns>
        public int StringSameOrNot(string fir, string sec)
        {
            int same = 0;

            if (!IsCharacterString(fir) && !IsCharacterString(sec))
            {
                //去掉指定字符
                fir = RemoveString(fir);
                sec = RemoveString(sec);
                if (fir == sec)   //第一种判断方式 
                {
                    same = 2;
                }
                else
                {
                    int flag = sec.Length;
                    StringBuilder sbfir = new StringBuilder();
                    StringBuilder sbsec = new StringBuilder();
                    sbfir.Append(fir);
                    sbsec.Append(sec);

                    int strlength = 0;
                    if (sbfir.Length == sbsec.Length)
                    {
                        strlength = sbfir.Length;
                    }
                    else if ((sbfir.Length < sbsec.Length))
                    {
                        strlength = sbfir.Length;
                    }
                    else
                    {
                        strlength = sbsec.Length;
                    }

                    try
                    {

                        if (strlength < 4)
                        {
                            for (int i = 0; i < strlength; i++)
                            {
                                if (sbfir.ToString().Substring(i, 1) == sbsec.ToString().Substring(i, 1))
                                {
                                    same++;
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < 4; i++)
                            {
                                if (sbfir.ToString().Substring(i, 1) == sbsec.ToString().Substring(i, 1))
                                {
                                    same++;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            else
            {
                if (fir == sec)
                {
                    same = 2;
                }
                else
                {
                    same = 1;
                }
            }

            return same;
        }


        public bool IsCharacterString(string str)
        {
            bool isCharcaterStr = true;
            for (int i = 0; i < str.Length; i++)
            {
                if ((int)str[i] > 127)
                {
                    isCharcaterStr = false;
                }
                else
                {
                    isCharcaterStr = true;
                }
            }
            return isCharcaterStr;
        }

        /// <summary>
        /// 两字符串中有几个相同字符
        /// </summary>
        /// <param name="fir">第一个字符串</param>
        /// <param name="sec">第二个字符串</param>
        /// <returns></returns>
        public int StringSameOrNot2(string fir, string sec)
        {
            int same = 0;
            try
            {
                if (!IsCharacterString(fir) && !IsCharacterString(sec))
                {
                    //去掉指定字符
                    fir = RemoveString(fir);
                    sec = RemoveString(sec);


                    if (fir == sec)   //第一种判断方式 
                    {
                        same = 3;
                    }
                    else
                    {
                        int flag = sec.Length;
                        StringBuilder sbfir = new StringBuilder();
                        StringBuilder sbsec = new StringBuilder();
                        sbfir.Append(fir);
                        sbsec.Append(sec);

                        int strlength = 0;
                        if (sbfir.Length == sbsec.Length)
                        {
                            strlength = sbfir.Length;
                        }
                        else if ((sbfir.Length < sbsec.Length))
                        {
                            strlength = sbfir.Length;
                        }
                        else
                        {
                            strlength = sbsec.Length;
                        }

                        try
                        {

                            if (strlength < 4)
                            {
                                for (int i = 0; i < strlength; i++)
                                {
                                    if (sbfir.ToString().Substring(i, 1) == sbsec.ToString().Substring(i, 1))
                                    {
                                        same++;
                                    }
                                }
                            }
                            else
                            {
                                for (int i = 0; i < 4; i++)
                                //for (int i = 0; i < 3; i++)
                                {
                                    if (sbfir.ToString().Substring(i, 1) == sbsec.ToString().Substring(i, 1))
                                    {
                                        same++;
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
                else
                {
                    if (fir == sec)
                    {
                        same = 3;
                    }
                    else
                    {
                        same = 2;
                    }
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return same;
        }

        /// <summary>
        /// 去掉字符串指定字符
        /// </summary>
        /// <param name="fir"></param>
        public string RemoveString(string fir)
        {
            //判断字符串是否包含指定的字符
            if (fir.IndexOf("德清县") > -1)
            {
                fir = fir.Replace("德清县", "");
            }
            else if (fir.IndexOf("德清") > -1)
            {
                fir = fir.Replace("德清", "");
            }

            if (fir.IndexOf("浙江省") > -1)
            {
                fir = fir.Replace("浙江省", "");
            }
            else if (fir.IndexOf("浙江") > -1)
            {
                fir = fir.Replace("浙江", "");
            }

            if (fir.IndexOf("浙江德清") > -1)
            {
                fir = fir.Replace("浙江德清", "");
            }
            return fir;
        }
        public bool DifScaleExtractDataToTEbyTU(IFeatureClass pTUFeatCls, IFeatureClass pCFeatCls, IFeatureClass pTEFeatCls, ProgressBar proMain, ProgressBar proSub, LabelX stateLabel)
        {
            IFeature pFeat = null;
            IFeatureCursor pCur = null;
            pCur = pTUFeatCls.Search(null, false);
            pFeat = pCur.NextFeature();

            int featureCount = 0;
            featureCount = pTUFeatCls.FeatureCount(null);
            string tuFeatClsName = pTUFeatCls.AliasName;
            string cFeatClsName = pCFeatCls.AliasName;

            IPolyline pPl = null;
            IPolygon pPy = null;

            IFeatureSelection pFeatSel = null;
            IFeatureLayer pFeatLyr = new FeatureLayer();
            pFeatLyr.FeatureClass = pCFeatCls;
            pFeatSel = pFeatLyr as IFeatureSelection;

            ISelectionSet pInSelSet = null;
            ISelectionSet pSubSelSet = null;
            ISelectionSet pSubSelSet2 = null;

            ISpatialFilter pSpatialfilter;
            pInSelSet = pFeatSel.SelectionSet;
            proSub.Value = 0;
            proMain.Value = 0;

            //ClsConvertUnit clsConvertUnit = new ClsConvertUnit();
            switch (pTUFeatCls.ShapeType)
            {
                case esriGeometryType.esriGeometryPoint:
                    //pPt = new Point();
                    //while (pFeat != null)
                    //{
                    //    pPt = pFeat.Shape as IPoint;
                    //    pSpatialfilter.Geometry = pPt;

                    //    pSubSelSet = pCFeatCls.Select(pSpatialfilter, esriSelectionType.esriSelectionTypeIDSet, esriSelectionOption.esriSelectionOptionNormal, null);
                    //    pInSelSet.Combine(pSubSelSet, esriSetOperation.esriSetUnion, out pSubSelSet2);
                    //    pInSelSet = pSubSelSet2;

                    //    pFeat = pCur.NextFeature();
                    //}
                    break;
                case esriGeometryType.esriGeometryPolyline:
                    pPl = new PolylineClass();
                    //自定义空间查询,减少查询次数,提高查询速度
                    pSpatialfilter = new SpatialFilter();
                    pSpatialfilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                    while (pFeat != null)
                    {
                        pPl = pFeat.Shape as IPolyline;
                        //IPolyline polyline = new PolylineClass();
                        //polyline = clsConvertUnit.ConvertLineCoordinate(pPl, esriUnits.esriDecimalDegrees, esriUnits.esriMeters);
                        ITopologicalOperator topol = pPl as ITopologicalOperator;
                        IGeometry geo = topol.Buffer(0.001);
                        pSpatialfilter.Geometry = geo;
                        //进度条
                        sbProcessBar(featureCount, proMain, proSub);
                        pSubSelSet = pCFeatCls.Select(pSpatialfilter, esriSelectionType.esriSelectionTypeHybrid, esriSelectionOption.esriSelectionOptionNormal, null);
                        pInSelSet.Combine(pSubSelSet, esriSetOperation.esriSetUnion, out pSubSelSet2);
                        pInSelSet = pSubSelSet2;

                        stateLabel.Text = "过滤" + cFeatClsName + "层要素，已遍历到" + tuFeatClsName + "层上第" + proMain.Value + "(" + featureCount + ")" + "个要素,请稍后!";
                        stateLabel.Refresh();
                        pFeat = pCur.NextFeature();
                    }
                    break;
                case esriGeometryType.esriGeometryPolygon:
                    pPy = new PolygonClass();
                    //自定义空间查询,减少查询次数,提高查询速度
                    pSpatialfilter = new SpatialFilterClass();
                    pSpatialfilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                    //pSpatialfilter.SpatialRelDescription = "T********";
                    ////下面的方法可以解决自相交问题，同时可以避免没必要的数据的提取。
                    while (pFeat != null)
                    {
                        pPy = pFeat.Shape as IPolygon;
                        ITopologicalOperator topol = pPy as ITopologicalOperator;
                        IGeometry geo = topol.Buffer(0.0005);
                        pSpatialfilter.Geometry = geo;
                        //进度条
                        sbProcessBar(featureCount, proMain, proSub);

                        pSubSelSet = pCFeatCls.Select(pSpatialfilter, esriSelectionType.esriSelectionTypeHybrid, esriSelectionOption.esriSelectionOptionNormal, null);
                        pInSelSet.Combine(pSubSelSet, esriSetOperation.esriSetUnion, out pSubSelSet2);
                        pInSelSet = pSubSelSet2;
                        stateLabel.Text = stateLabel.Text = "过滤" + cFeatClsName + "层要素，已遍历到" + tuFeatClsName + "层上第" + proMain.Value + "(" + featureCount + ")" + "个要素,请稍后!";
                        stateLabel.Refresh();
                        proSub.Update();
                        proMain.Update();
                        pFeat = pCur.NextFeature();
                    }
                    break;
                default:
                    return false;
            }
            try
            {
                proMain.Value = 0;
                proSub.Value = 0;

                ////将选择集提取到TE层
                ////提取
                IDataset pDataset = pTEFeatCls as IDataset;
                if (pDataset == null)
                    return false;
                IWorkspace tempOutWorkspace = pDataset.Workspace;
            }
            catch (Exception)
            {
            }
            return true;
        }

        #region 多边形正方向匹配
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
            int buffer, double area, ProgressBar prgMain, ProgressBar prgSub, LabelX stateLabel)
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
                    sbProcessBar(srcFeatCount, prgMain, prgSub);
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

        #endregion
        /// <summary>
        /// DifScaleSearchChangedPolylineFeatures 跨尺度线要素查询
        /// </summary>
        /// <param name="TUFeatCls"></param>
        /// <param name="TEFeatCls"></param>
        /// <param name="resultTable"></param>
        /// <param name="buffer"></param>
        /// <param name="area"></param>
        /// <param name="prgMain"></param>
        /// <param name="prgSub"></param>
        /// <param name="stateLabel"></param>
        /// <returns></returns>
        public bool DifScaleSearchChangedPolylineFeatures(IFeatureClass TUFeatCls, IFeatureClass TEFeatCls, ITable resultTable, double buffer, double area, ProgressBar prgMain, ProgressBar prgSub, LabelX stateLabel)
        {
            IFeatureCursor tuFeatCursor = null;
            IFeatureCursor teFeatCursor = null;

            IFeature tuFeat = null;
            IFeature teFeat = null;

            int tuFeatCount = 0;
            //bool shpEqual = true;
            //  bool attributeEqual = true;
            //记录每个tu要素查询得到的te要素个数
            int featureCount = 0;

            int targetCount = 0;

            // IPolygon tuPolygon;
            //IArea tuArea=null;

            ISpatialFilter spatialFilter;
            //反向查询tu要素
            ISpatialFilter revSpatialFilter;
            //反向查询游标
            IFeatureCursor revFeatCursor = null;
            //反向查询要素
            IFeature revFeat = null;
            //Collection<IFeature> revCol;

            //判断要素包含关系
            IRelationalOperator relationalOperator;

            //记录查询过要素
            Collection<int> targetCol;
            try
            {
                IDataset dataset = resultTable as IDataset;
                IWorkspaceEdit workspaceEdit = dataset.Workspace as IWorkspaceEdit;
                workspaceEdit.StartEditing(true);
                workspaceEdit.StartEditOperation();

                tuFeatCount = TUFeatCls.FeatureCount(null);
                tuFeatCursor = TUFeatCls.Search(null, false);

                if (tuFeatCursor != null)
                {
                    tuFeat = tuFeatCursor.NextFeature();
                }
                IRowBuffer rowBuffer = null;
                ICursor rowCursor = null;
                targetCol = new Collection<int>();

                //te层FIXOID字段的索引
                int fixoid = TEFeatCls.Fields.FindField("FIXOID");

                int index = resultTable.Fields.FindField("变化标记");
                int indexFID = resultTable.Fields.FindField("源OID");
                int indexOID = resultTable.Fields.FindField("待更新OID");

                int revFeatCount = -1;
                while (tuFeat != null)
                {
                    rowBuffer = resultTable.CreateRowBuffer();
                    rowCursor = resultTable.Insert(true);

                    ////记录反向查询要素
                    //revCol = new Collection<IFeature>();
                    //查看进度
                    sbProcessBar(tuFeatCount, prgMain, prgSub);
                    if (!targetCol.Contains(tuFeat.OID))
                    {
                        spatialFilter = new SpatialFilterClass();
                        spatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                        ITopologicalOperator top = tuFeat.Shape as ITopologicalOperator;
                        IGeometry tuBuffer = top.Buffer(buffer);
                        spatialFilter.Geometry = tuBuffer;
                        featureCount = TEFeatCls.FeatureCount(spatialFilter);

                        if (featureCount == 1)
                        {
                            teFeatCursor = TEFeatCls.Search(spatialFilter, false);
                            teFeat = teFeatCursor.NextFeature();
                            int oid = Convert.ToInt32(teFeat.get_Value(fixoid));

                            //反向查询TU层要素
                            revSpatialFilter = new SpatialFilterClass();
                            ITopologicalOperator revTop = teFeat.Shape as ITopologicalOperator;
                            IGeometry teBuffer = revTop.Buffer(buffer * 2);
                            revSpatialFilter.Geometry = teBuffer;
                            revSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;
                            revFeatCount = TUFeatCls.FeatureCount(revSpatialFilter);

                            if (revFeatCount == 1)
                            {
                                rowBuffer.set_Value(indexFID, tuFeat.OID);
                                rowBuffer.set_Value(indexOID, oid);
                                rowBuffer.set_Value(index, "一对一");
                                rowCursor.InsertRow(rowBuffer);
                                targetCount++;
                            }
                            else if (revFeatCount > 1)
                            {
                                rowBuffer = null;

                                revFeatCursor = TUFeatCls.Search(revSpatialFilter, false);
                                revFeat = revFeatCursor.NextFeature();
                                while (revFeat != null)
                                {
                                    if (!targetCol.Contains(revFeat.OID))
                                    {
                                        rowBuffer = resultTable.CreateRowBuffer();
                                        rowCursor = resultTable.Insert(true);
                                        rowBuffer.set_Value(indexFID, revFeat.OID);
                                        rowBuffer.set_Value(indexOID, oid);
                                        rowBuffer.set_Value(index, "多对一");
                                        rowCursor.InsertRow(rowBuffer);
                                        targetCount++;
                                        targetCol.Add(revFeat.OID);
                                    }
                                    revFeat = revFeatCursor.NextFeature();
                                }
                                if (!targetCol.Contains(tuFeat.OID))
                                {
                                    rowBuffer = resultTable.CreateRowBuffer();
                                    rowCursor = resultTable.Insert(true);
                                    rowBuffer.set_Value(indexFID, tuFeat.OID);
                                    rowBuffer.set_Value(index, "新增要素");
                                    rowCursor.InsertRow(rowBuffer);
                                    targetCount++;
                                }
                            }
                            else if (revFeatCount == 0)
                            {
                                rowBuffer.set_Value(indexFID, tuFeat.OID);
                                rowBuffer.set_Value(index, "新增要素");
                                rowCursor.InsertRow(rowBuffer);
                                targetCount++;
                            }
                        }
                        else if (featureCount == 0)
                        {
                            rowBuffer.set_Value(indexFID, tuFeat.OID);
                            rowBuffer.set_Value(index, "新增要素");
                            rowCursor.InsertRow(rowBuffer);
                            targetCount++;
                        }
                        else if (featureCount > 1)
                        {
                            teFeatCursor = TEFeatCls.Search(spatialFilter, false);
                            teFeat = teFeatCursor.NextFeature();

                            int tempCount = 0;
                            while (teFeat != null)
                            {
                                int oid = Convert.ToInt32(teFeat.get_Value(fixoid));
                                ITopologicalOperator revTop = teFeat.Shape as ITopologicalOperator;
                                relationalOperator = revTop.Buffer(buffer * 2) as IRelationalOperator;

                                if (relationalOperator.Contains(tuFeat.Shape))
                                {
                                    if (!targetCol.Contains(tuFeat.OID))
                                    {
                                        //反向查询TU层要素
                                        revSpatialFilter = new SpatialFilterClass();
                                        ITopologicalOperator revTop1 = teFeat.Shape as ITopologicalOperator;
                                        revSpatialFilter.Geometry = revTop1.Buffer(buffer * 2);
                                        revSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;
                                        revFeatCount = TUFeatCls.FeatureCount(revSpatialFilter);
                                        if (revFeatCount == 1)
                                        {
                                            rowBuffer.set_Value(indexFID, tuFeat.OID);
                                            rowBuffer.set_Value(indexOID, oid);
                                            rowBuffer.set_Value(index, "一对一");
                                            rowCursor.InsertRow(rowBuffer);
                                            targetCount++;
                                        }
                                        else if (revFeatCount > 1)
                                        {
                                            revFeatCursor = TUFeatCls.Search(revSpatialFilter, false);
                                            revFeat = revFeatCursor.NextFeature();
                                            rowBuffer = null;
                                            while (revFeat != null)
                                            {
                                                if (!targetCol.Contains(revFeat.OID))
                                                {
                                                    rowBuffer = resultTable.CreateRowBuffer();
                                                    rowCursor = resultTable.Insert(true);
                                                    rowBuffer.set_Value(indexFID, revFeat.OID);
                                                    rowBuffer.set_Value(indexOID, oid);
                                                    rowBuffer.set_Value(index, "多对一");
                                                    rowCursor.InsertRow(rowBuffer);
                                                    targetCount++;
                                                    targetCol.Add(revFeat.OID);
                                                }
                                                revFeat = revFeatCursor.NextFeature();
                                            }
                                            if (!targetCol.Contains(tuFeat.OID))
                                            {
                                                rowBuffer = resultTable.CreateRowBuffer();
                                                rowCursor = resultTable.Insert(true);
                                                rowBuffer.set_Value(indexFID, tuFeat.OID);
                                                rowBuffer.set_Value(index, "新增要素");
                                                rowCursor.InsertRow(rowBuffer);
                                                targetCount++;
                                            }
                                        }
                                    }
                                    tempCount++;
                                    break;
                                }
                                teFeat = teFeatCursor.NextFeature();
                            }
                            if (tempCount == 0)
                            {
                                rowBuffer.set_Value(indexFID, tuFeat.OID);
                                rowBuffer.set_Value(index, "新增要素");
                                rowCursor.InsertRow(rowBuffer);
                                targetCount++;
                            }
                        }
                    }
                    if (targetCount > 400)
                    {
                        rowCursor.Flush();
                        targetCount = 0;
                    }
                    stateLabel.Text = "已匹配" + prgMain.Value.ToString() + "个要素";
                    stateLabel.Refresh();
                    tuFeat = tuFeatCursor.NextFeature();
                }
                rowCursor.Flush();
                workspaceEdit.StopEditOperation();
                workspaceEdit.StopEditing(true);
                return true;
            }
            catch (Exception)
            {
                throw;
            }

        }

        ////取得最小值
        private double GetMinValue(double dblA, double dblB)
        {
            double dblTemp = 0;
            if (dblA >= dblB)
            {
                dblTemp = dblB;
            }
            else
            {
                dblTemp = dblA;
            }
            return dblTemp;
        }

        ////取得最大值
        private double GetMaxValue(double dblA, double dblB)
        {
            ////
            double dblTemp = 0;
            if (dblA <= dblB)
            {
                dblTemp = dblB;
            }
            else
            {
                dblTemp = dblA;
            }
            return dblTemp;
        }
        /// <summary>
        /// 处理进程条
        /// </summary>
        /// <param name="lngCount"></param>
        /// <param name="prgMain"></param>
        /// <param name="prgSub"></param>
        private void sbProcessBar(int lngCount, ProgressBar prgMain, ProgressBar prgSub)
        {
            ////进程条处理
            if (lngCount / 100 >= 1)
            {
                prgSub.Maximum = 100;
                prgMain.Maximum = lngCount;

                if (prgMain.Value == (lngCount / 100) * 100)
                {
                    prgSub.Value = 0;
                }
                else if (prgMain.Value > (lngCount / 100) * 100)
                {
                    prgSub.Maximum = (lngCount % 100);
                    prgSub.Minimum = 0;
                }

            }
            else
            {
                if (lngCount > 0)
                {
                    prgSub.Maximum = lngCount;
                    prgMain.Maximum = lngCount;
                }
            }

            if (prgSub.Value >= prgSub.Maximum)
            {
                prgSub.Value = 0;
            }

            if (prgSub.Value < prgSub.Maximum)
            {
                prgSub.Value = prgSub.Value + 1;
            }

            if (prgMain.Value < prgMain.Maximum)
            {
                prgMain.Value = prgMain.Value + 1;
            }
            else
            {
                prgMain.Value = 0;
            }

        }
        ///进入编辑状态
        private void EditData(IFeatureClass pEditFeatCls)
        {
            m_pdataset = pEditFeatCls as IDataset;
            if (m_pdataset == null)
                return;
            m_pWorkSpaceEdit = m_pdataset.Workspace as IWorkspaceEdit;

            m_pWorkSpaceEdit.StartEditing(true);
            m_pWorkSpaceEdit.StartEditOperation();
        }

        ////停止编辑

        private void StopEditData(IFeatureClass pEditFeatCls, bool blnNotSave)
        {
            m_pdataset = pEditFeatCls as IDataset;

            if (m_pdataset == null)
                return;
            m_pWorkSpaceEdit = m_pdataset.Workspace as IWorkspaceEdit;
            m_pWorkSpaceEdit.StopEditOperation();
            if (blnNotSave == true)
            {
                m_pWorkSpaceEdit.StopEditing(false);
            }
            else
            {
                m_pWorkSpaceEdit.StopEditing(true);
            }
        }

    }
}
