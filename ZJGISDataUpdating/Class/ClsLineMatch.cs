/*-------------------------------------------------------------------------
 * 版权所有：武汉大学
 * 创建人：johnforrest
 * 联系方式：571716604@qq.com
 * 创建时间： 4/3/2018 9:36:18 AM
 * 类名称：ClsLineMatch
 * 本类主要用途描述：
 * 修改人：
 * 修改时间：
 * 修改备注：
 * @version 1.0
 *  -------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ZJGISCommon.Classes;

namespace ZJGISDataUpdating.Class
{
    class ClsLineMatch
    {
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
        public bool SearchChangedFeaturesGeoAttr(IFeatureClass pSrcFcls, IFeatureClass pTarFcls, ITable resultTable, double[] weight,
            double buffer, string fields, ProgressBar prgMain, ProgressBar prgSub, LabelX stateLabel, CheckBox chkBoxIndicator)
        {
            IFeature pSrcFeature = null;
            IFeature pTarFeature = null;
            IFeatureCursor pTarCursor = null;//工作区数据指针

            IRelationalOperator pRelOp;
            bool blnShpEqual = false;  //形状匹配
            bool blnAttriEqual = false;//属性匹配

            int i = 0;
            int indexChangeMaker = -1;

            int tempfeatcount = 0;  //临时层要素个数

            int idx = 0;
            //找到的与源要素有关的目标要素
            Dictionary<int, IFeature> pDicCol = null;
            ISpatialFilter pSFilter = null;

            //处理的临时要素
            int tempFeatCount = 0;

            ////进入编辑状态
            //匹配表
            IWorkspace workspace = (resultTable as IDataset).Workspace;
            IWorkspaceEdit workspaceEdit = workspace as IWorkspaceEdit;
            workspaceEdit.StartEditing(true);
            workspaceEdit.StartEditOperation();
            //源图层要素个数
            int sourceFeatCount = pSrcFcls.FeatureCount(null);
            //源图层要素指针
            IFeatureCursor pSrcCursor = pSrcFcls.Search(null, false);

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

            IRowBuffer resultTableRowBuffer = null;
            ICursor resultTableRowCursor = null;
            //源要素总的循环
            try
            {
                while (pSrcFeature != null)
                {
                    pDicCol = new Dictionary<int, IFeature>();
                    pSFilter = new SpatialFilter();
                    resultTableRowBuffer = resultTable.CreateRowBuffer();
                    resultTableRowCursor = resultTable.Insert(true);
                    indexChangeMaker = resultTableRowBuffer.Fields.FindField("变化标记");
                    /**通过用代码构建缓冲区分析，来判断是否是同一个要素*/
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
                    tempfeatcount = pTarFcls.FeatureCount(pSFilter);
                    pTarFeature = pTarCursor.NextFeature();
                    idx = 0;

                    //设置表TRA_PT_I_PtTabl的（源OID）字段的值——cell[1]
                    resultTableRowBuffer.set_Value(resultTableRowBuffer.Fields.FindField("源OID"), pSrcFeature.OID);

                    while ((pTarFeature != null))
                    {
                        if ((pTarFeature.Shape != null))
                        {
                            if (pTarFeature.Shape.GeometryType == esriGeometryType.esriGeometryPolyline)
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
                                                    resultTableRowBuffer.set_Value(resultTableRowBuffer.Fields.FindField(tempSrc), pSrcfield);

                                                    double Similarity = ClsCosine.getSimilarity(pSrcfield, pTarfield);

                                                    //if (StringSameOrNot2(pSrcfield, pTarfield) > 2)
                                                    if (Similarity > 0.7)
                                                    {
                                                        //设置表TRA_PT_I_PtTabl的（待匹配编码）字段的值——cell[5]
                                                        resultTableRowBuffer.set_Value(resultTableRowBuffer.Fields.FindField(tempTar), pTarfield);

                                                        CalculateIndcators(weight, buffer, chkBoxIndicator, pSrcFeature, pTarFeature, resultTableRowBuffer);

                                                        int index = 0;

                                                        //if (polylineRadio > weight[2])
                                                        //{
                                                        //如果两个点之间的距离小于设置的综合相似度
                                                        //if (distance < weight[0] && MatchCode(pSrcFeature, pTarFeature))
                                                        //if (MatchCode(pSrcFeature, pTarFeature))
                                                        //{
                                                        //设置表TRA_PT_I_PtTabl的（待匹配oid）字段的值为空——cell[2]
                                                        if (resultTableRowBuffer.get_Value(resultTableRowBuffer.Fields.FindField("待匹配OID")).ToString() == "")
                                                        {
                                                            //设置表TRA_PT_I_PtTabl的（待匹配oid）字段的值——cell[2]
                                                            resultTableRowBuffer.set_Value(resultTableRowBuffer.Fields.FindField("待匹配OID"), pTarFeature.get_Value(index));
                                                        }
                                                        else
                                                        {
                                                            string oids = resultTableRowBuffer.get_Value(resultTableRowBuffer.Fields.FindField("待匹配OID")).ToString() + ";" + pTarFeature.get_Value(index);
                                                            //设置表TRA_PT_I_PtTabl的（待匹配oid）字段的值——cell[2]
                                                            resultTableRowBuffer.set_Value(resultTableRowBuffer.Fields.FindField("待匹配OID"), oids);
                                                        }

                                                        if (!pDicCol.ContainsKey(idx))
                                                        {
                                                            pDicCol.Add(idx, pTarFeature);
                                                        }
                                                        idx += 1;
                                                        //}
                                                    }
                                                    //}

                                                }
                                            }
                                            //两个图层的名称字段名称相同
                                            else
                                            {
                                                string pSrcfield = pSrcFeature.get_Value(pSrcFeature.Fields.FindField(array[k])).ToString();
                                                string pTarfield = pTarFeature.get_Value(pTarFeature.Fields.FindField(array[k])).ToString();

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
                                                    resultTableRowBuffer.set_Value(resultTableRowBuffer.Fields.FindField(tempSrc), pSrcfield);
                                                    double Similarity = ClsCosine.getSimilarity(pSrcfield, pTarfield);

                                                    //if (StringSameOrNot2(pSrcfield, pTarfield) > 2)
                                                    //if (StringSameOrNot2(pSrcfield, pTarfield) > 2)
                                                    if (Similarity > 0.7)
                                                    {
                                                        //设置表TRA_PT_I_PtTabl的（待匹配编码）字段的值——cell[5]
                                                        resultTableRowBuffer.set_Value(resultTableRowBuffer.Fields.FindField(tempTar), pTarfield);
                                                        //20170516注释掉
                                                        //if (MatchCode(pSrcFeature, pTarFeature))
                                                        //{
                                                        //    //设置表TRA_PT_I_PtTabl的（源编码）字段的值——cell[4]
                                                        //    rowBuffer.set_Value(4, pSrcFeature.get_Value(pSrcFeature.Fields.FindField("GCode")));
                                                        //    //设置表TRA_PT_I_PtTabl的（待匹配编码）字段的值——cell[5]
                                                        //    rowBuffer.set_Value(5, pTarFeature.get_Value(pTarFeature.Fields.FindField("GCode")));
                                                        //}

                                                        CalculateIndcators(weight, buffer, chkBoxIndicator, pSrcFeature, pTarFeature, resultTableRowBuffer);

                                                        int index = 0;
                                                        //if (polylineRadio > weight[2])
                                                        //{
                                                        //如果两个点之间的距离小于设置的综合相似度
                                                        //if (distance < weight[0] && MatchCode(pSrcFeature, pTarFeature))
                                                        //if (MatchCode(pSrcFeature, pTarFeature))
                                                        //{
                                                        //设置表TRA_PT_I_PtTabl的（待匹配oid）字段的值为空——cell[2]
                                                        if (resultTableRowBuffer.get_Value(resultTableRowBuffer.Fields.FindField("待匹配OID")).ToString() == "")
                                                        {
                                                            //设置表TRA_PT_I_PtTabl的（待匹配oid）字段的值——cell[2]
                                                            resultTableRowBuffer.set_Value(resultTableRowBuffer.Fields.FindField("待匹配OID"), pTarFeature.get_Value(index));
                                                        }
                                                        else
                                                        {
                                                            string oids = resultTableRowBuffer.get_Value(resultTableRowBuffer.Fields.FindField("待匹配OID")).ToString() + ";" + pTarFeature.get_Value(index);
                                                            //设置表TRA_PT_I_PtTabl的（待匹配oid）字段的值——cell[2]
                                                            resultTableRowBuffer.set_Value(resultTableRowBuffer.Fields.FindField("待匹配OID"), oids);
                                                        }

                                                        //20170912
                                                        ////设置表TRA_PT_I_PtTabl的（待匹配编码）字段的值——cell[5]
                                                        //rowBuffer.set_Value(rowBuffer.Fields.FindField("源图层名称"), pSrcFeature.get_Value(pSrcFeature.Fields.FindFieldByAliasName(array[0])));
                                                        ////设置表TRA_PT_I_PtTabl的（待匹配编码）字段的值——cell[5]
                                                        //rowBuffer.set_Value(rowBuffer.Fields.FindField("待匹配图层名称"), pTarFeature.get_Value(pTarFeature.Fields.FindFieldByAliasName(array[0])));

                                                        if (!pDicCol.ContainsKey(idx))
                                                        {
                                                            pDicCol.Add(idx, pTarFeature);
                                                        }
                                                        idx = idx + 1;
                                                        //}
                                                    }
                                                    //}

                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    //CalculateIndcators(weight, buffer, chkBoxIndicator, pSrcFeature, pTarFeature, resultTableRowBuffer);

                                    //形状相似度
                                    double shape = ClsIndicatorFunStatic.PolylineShapeSimilarValue(pSrcFeature, pTarFeature);
                                    //节点相似度
                                    double matchedPoints = ClsIndicatorFunStatic.MatchedPointsSimilarValue(pSrcFeature, pTarFeature, buffer);
                                    //综合相似度
                                    double polylineRadio = shape * weight[0] + matchedPoints * weight[1];

                                    string shape1 = string.Format("{0:0.00000000}", shape);
                                    string matchedPoints1 = string.Format("{0:0.00000000}", matchedPoints);
                                    string polygonRatio1 = string.Format("{0:0.00000000}", polylineRadio);

                                    //resultTableRowBuffer.set_Value(resultTableRowBuffer.Fields.FindField("形状相似度"), shape1);
                                    //resultTableRowBuffer.set_Value(resultTableRowBuffer.Fields.FindField("节点相似度"), matchedPoints1);
                                    //resultTableRowBuffer.set_Value(resultTableRowBuffer.Fields.FindField("综合相似度"), polygonRatio1);

                                    //if (polylineRadio > weight[2])
                                    //{
                                    //如果两个点之间的距离小于设置的综合相似度
                                    //if (distance < weight[0] && MatchCode(pSrcFeature, pTarFeature))
                                    //if (MatchCode(pSrcFeature, pTarFeature))
                                    //{
                                    //设置表TRA_PT_I_PtTabl的（待匹配oid）字段的值为空——cell[2]

                                    //角度判断30
                                    if (ClsIndicatorFunStatic.IncludedAngle(pSrcFeature, pTarFeature) < 30)
                                    {
                                        if (polylineRadio > 0.5)
                                        {
                                            resultTableRowBuffer.set_Value(resultTableRowBuffer.Fields.FindField("形状相似度"), shape1);
                                            resultTableRowBuffer.set_Value(resultTableRowBuffer.Fields.FindField("节点相似度"), matchedPoints1);
                                            resultTableRowBuffer.set_Value(resultTableRowBuffer.Fields.FindField("综合相似度"), polygonRatio1);

                                            int index = 0;
                                            if (resultTableRowBuffer.get_Value(resultTableRowBuffer.Fields.FindField("待匹配OID")).ToString() == "")
                                            {
                                                //设置表TRA_PT_I_PtTabl的（待匹配oid）字段的值——cell[2]
                                                resultTableRowBuffer.set_Value(resultTableRowBuffer.Fields.FindField("待匹配OID"), pTarFeature.get_Value(index));
                                            }
                                            else
                                            {
                                                string oids = resultTableRowBuffer.get_Value(resultTableRowBuffer.Fields.FindField("待匹配OID")).ToString() + ";" + pTarFeature.get_Value(index);
                                                //设置表TRA_PT_I_PtTabl的（待匹配oid）字段的值——cell[2]
                                                resultTableRowBuffer.set_Value(resultTableRowBuffer.Fields.FindField("待匹配OID"), oids);
                                            }
                                            if (!pDicCol.ContainsKey(idx))
                                            {
                                                pDicCol.Add(idx, pTarFeature);
                                            }
                                            idx = idx + 1;
                                        }
                                    }
                                }
                            }
                        }
                        pTarFeature = pTarCursor.NextFeature();
                    }

                    /**情况1：通过用代码构建缓冲区分析，缓冲区分析找不到对应要素，即为未匹配的要素。*/
                    if (pDicCol.Count == 0)
                    {
                        if (indexChangeMaker > -1)
                        {
                            resultTableRowBuffer.set_Value(indexChangeMaker, ClsConstant.One2Zero);
                        }
                    }
                    /**情况2：通过用代码构建缓冲区分析，找到1条对应要素的情况（源图层和待匹配图层要素是1对1的情况），
                     * 需要判断属性图形是否发生变化。*/
                    else if (pDicCol.Count == 1)
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
                                    if (indexChangeMaker > -1)
                                    {
                                        //rowBuffer.set_Value(intFieldIndex, "图形变化");
                                        //rowBuffer.set_Value(intFieldIndex, "一对一");
                                        resultTableRowBuffer.set_Value(indexChangeMaker, ClsConstant.One2One);
                                    }

                                    ////只属性变化
                                }
                                else if (blnShpEqual && blnAttriEqual == false)
                                {
                                    if (indexChangeMaker > -1)
                                    {
                                        resultTableRowBuffer.set_Value(indexChangeMaker, "属性变化");
                                        ////修改的打标为6   
                                    }
                                    ////图形属性都变化
                                }
                                else if (blnShpEqual == false && blnAttriEqual == false)
                                {
                                    if (indexChangeMaker > -1)
                                    {
                                        resultTableRowBuffer.set_Value(indexChangeMaker, "属性图形变化");
                                        ////修改的打标为2        
                                    }

                                }
                            }
                            else if (blnShpEqual == true & blnAttriEqual == true)
                            {
                                //indexChangeMaker = resultTableRowBuffer.Fields.FindField("变化标记");
                                if (indexChangeMaker > -1)
                                {
                                    resultTableRowBuffer.set_Value(indexChangeMaker, "未变化");
                                }
                            }
                            //resultTableRowCursor.InsertRow(resultTableRowBuffer);
                        }
                    }
                    /**情况3：通过用代码构建缓冲区分析，找到多条对应要素的情况（源图层和待匹配图层要素是1对多的情况），
                     * 需要判断属性图形是否发生变化。*/
                    else if (pDicCol.Count > 1)
                    {
                        //indexChangeMaker = resultTableRowBuffer.Fields.FindField("变化标记");
                        if (indexChangeMaker > -1)
                        {
                            resultTableRowBuffer.set_Value(indexChangeMaker, ClsConstant.One2More);
                            //resultTableRowCursor.InsertRow(resultTableRowBuffer);
                        }
                    }

                    resultTableRowCursor.InsertRow(resultTableRowBuffer);


                    //进程条处理
                    ClsStatic.AboutProcessBar(sourceFeatCount, prgMain, prgSub);

                    //每1千个要素就把缓冲区内的要素类写入目标层
                    if (tempFeatCount >= 500)
                    {
                        resultTableRowCursor.Flush();
                        tempFeatCount = 0;
                    }

                    stateLabel.Text = "正在对" + (pSrcFcls as IDataset).Name + "图层和" + (pTarFcls as IDataset).Name + "图层进行数据变化分析，已经处理" + prgMain.Value + "(" + sourceFeatCount + ")个要素。";
                    stateLabel.Refresh();
                    prgMain.Update();
                    prgSub.Update();
                    tempFeatCount = tempFeatCount + 1;
                    pSrcFeature = pSrcCursor.NextFeature();
                }
            }
            catch (System.Exception ex)
            {
                //MessageBox.Show(ex.Message);
                MessageBox.Show(ex.StackTrace);
            }
            if (resultTableRowCursor != null)
            {
                resultTableRowCursor.Flush();
            }

            workspaceEdit.StopEditOperation();
            workspaceEdit.StopEditing(true);

            stateLabel.Text = "";
            prgMain.Value = 0;
            prgSub.Value = 0;
            return true;
        }
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
        public bool SearchChangedFeaturesGeo(IFeatureClass pSrcFcls, IFeatureClass pTarFcls, ITable resultTable, double[] weight,
            double buffer, string fields, ProgressBar prgMain, ProgressBar prgSub, LabelX stateLabel, CheckBox chkBoxIndicator,
            double includeAngel,double hausdorff)
        {
            IFeature srcFeature = null;
            IFeatureCursor pSourCursor = null;//工作区数据指针

            //根据特定的控件过滤条件，源图层查询到的待匹配图层的游标
            IFeatureCursor pTargetCursor = null;
            IFeature tarFeature = null;
            IFeature revSrcFeature = null;

            int i = 0;
            int indexChangeMaker = -1;
            int targetOID = 0;
            int sourceOID = 0;

            int targetFeatcount = 0;  //待匹配图层要素个数

            int idx = 0;
            //找到的与源要素有关的目标要素
            List<IFeature> pListTarOne2One = null;
            List<IFeature> pListTarMore2One = null;
            List<IFeature> pListTarMore2More = null;

            ISpatialFilter spatialFilter = null;
            //反向查询源图层要素
            ISpatialFilter revSpatialFilter;

            //处理的临时要素
            int tempFeatCount = 0;

            ////进入编辑状态
            IWorkspace workspace = (resultTable as IDataset).Workspace;
            IWorkspaceEdit workspaceEdit = workspace as IWorkspaceEdit;
            workspaceEdit.StartEditing(true);
            workspaceEdit.StartEditOperation();

            //源图层要素个数
            int sourceFeatCount = pSrcFcls.FeatureCount(null);
            //源图层要素指针
            IFeatureCursor pOutSrcCursor = pSrcFcls.Search(null, false);

            //设置进度条
            if (sourceFeatCount > 0)
            {
                prgMain.Maximum = sourceFeatCount;
            }
            prgSub.Maximum = 100;
            prgSub.Value = 0;
            prgMain.Value = 0;

            if ((pOutSrcCursor != null))
            {
                srcFeature = pOutSrcCursor.NextFeature();
            }

            IRowBuffer resultTableRowBuffer = null;
            ICursor resultTableRowCursor = null;
            //源要素总的循环
            try
            {
                while (srcFeature != null)
                {
                    pListTarMore2One = new List<IFeature>();
                    pListTarMore2More = new List<IFeature>();
                    pListTarOne2One = new List<IFeature>();

                    spatialFilter = new SpatialFilter();

                    resultTableRowBuffer = resultTable.CreateRowBuffer();
                    resultTableRowCursor = resultTable.Insert(true);

                    sourceOID = resultTableRowBuffer.Fields.FindField("源OID");
                    targetOID = resultTableRowBuffer.Fields.FindField("待匹配OID");
                    indexChangeMaker = resultTableRowBuffer.Fields.FindField("变化标记");

                    //记录在由源图层查询待匹配图层是1对1的情况下，
                    //再由待匹配的图层反向查询源图层获得的源图层的个数
                    int revSourFeatCount = 0;

                    /**通过用代码构建缓冲区分析，来判断是否是同一个要素*/
                    if (srcFeature.Shape.GeometryType == esriGeometryType.esriGeometryPolyline)
                    {
                        ITopologicalOperator top = srcFeature.Shape as ITopologicalOperator;
                        spatialFilter.Geometry = top.Buffer(buffer) as IGeometry;
                        //自定义空间查询,减少查询次数,提高查询速度
                        spatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                    }

                    //得到对应pSrcFeature的TE图层上的要素个数
                    targetFeatcount = pTarFcls.FeatureCount(spatialFilter);
                    //设置表TRA_PT_I_PtTabl的（源OID）字段的值——cell[1]
                    resultTableRowBuffer.set_Value(sourceOID, srcFeature.OID);
                    //Returns an object cursor that can be used to fetch feature objects selected by the specified query.
                    //pTargetCursor是根据特定的控件过滤条件，源图层查询到的待匹配图层的游标
                    pTargetCursor = pTarFcls.Search(spatialFilter, false);
                    tarFeature = pTargetCursor.NextFeature();
                    if (targetFeatcount == 0)
                    {
                        resultTableRowBuffer.set_Value(indexChangeMaker, ClsConstant.One2Zero);
                    }
                    else
                    {
                        #region 综合相似度
                        ////形状相似度
                        //double shape = ClsIndicatorFunStatic.PolylineShapeSimilarValue(srcFeature, tarFeature);
                        ////节点相似度
                        //double matchedPoints = ClsIndicatorFunStatic.MatchedPointsSimilarValue(srcFeature, tarFeature, buffer);
                        ////综合相似度
                        //double polylineRadio = shape * weight[0] + matchedPoints * weight[1];

                        //string shape1 = string.Format("{0:0.00000000}", shape);
                        //string matchedPoints1 = string.Format("{0:0.00000000}", matchedPoints);
                        //string polygonRatio1 = string.Format("{0:0.00000000}", polylineRadio);

                        //resultTableRowBuffer.set_Value(resultTableRowBuffer.Fields.FindField("形状相似度"), shape1);
                        //resultTableRowBuffer.set_Value(resultTableRowBuffer.Fields.FindField("节点相似度"), matchedPoints1);
                        //resultTableRowBuffer.set_Value(resultTableRowBuffer.Fields.FindField("综合相似度"), polygonRatio1);
                        //if (polylineRadio > 0.5)
                        //double test = ClsIndicatorFunStatic.(int)hausdorffDistance(pSrcFeature, pTarFeature);
                        //IPolyline planeSour = ClsConvertUnit.ConvertLineCoordinate(
                        //    (IPolyline) pSrcFeature, ESRI.ArcGIS.esriSystem.esriUnits.esriDecimalDegrees
                        //    , ESRI.ArcGIS.esriSystem.esriUnits.esriMeters);
                        //}
                        #endregion
                        //角度判断30
                        if (targetFeatcount == 1)
                        {
                            if (ClsIndicatorFunStatic.IncludedAngle(srcFeature, tarFeature) < (int)includeAngel)
                            {
                                if (ClsIndicatorFunStatic.HausdorffDistance(srcFeature, tarFeature) < (int)hausdorff)
                                {
                                    //TODO :正反向匹配的反向查询：根据待匹配图层查询源图层
                                    revSpatialFilter = new SpatialFilterClass();
                                    ITopologicalOperator top = tarFeature.Shape as ITopologicalOperator;
                                    revSpatialFilter.Geometry = top.Buffer(buffer) as IGeometry;
                                    //revSpatialFilter.Geometry = tarFeature.Shape;
                                    revSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                                    revSourFeatCount = pSrcFcls.FeatureCount(revSpatialFilter);
                                    pSourCursor = pSrcFcls.Search(revSpatialFilter, false);
                                    revSrcFeature = pSourCursor.NextFeature();


                                    if (revSourFeatCount == 1)
                                    {
                                        resultTableRowBuffer.set_Value(targetOID, tarFeature.get_Value(0));
                                        //rowBuffer.set_Value(index, "一对一");
                                        resultTableRowBuffer.set_Value(indexChangeMaker, ClsConstant.One2One);
                                    }
                                    else
                                    {
                                        while (revSrcFeature != null)
                                        {
                                            if (ClsIndicatorFunStatic.IncludedAngle(revSrcFeature, tarFeature) < (int)includeAngel)
                                            {
                                                //if (polylineRadio > 0.5)
                                                //double test = ClsIndicatorFunStatic.(int)hausdorffDistance(pSrcFeature, pTarFeature);
                                                //IPolyline planeSour = ClsConvertUnit.ConvertLineCoordinate(
                                                //    (IPolyline) pSrcFeature, ESRI.ArcGIS.esriSystem.esriUnits.esriDecimalDegrees
                                                //    , ESRI.ArcGIS.esriSystem.esriUnits.esriMeters);
                                                if (ClsIndicatorFunStatic.HausdorffDistance(revSrcFeature, tarFeature) < (int)hausdorff)
                                                {
                                                    pListTarOne2One.Add(revSrcFeature);
                                                }
                                            }
                                            revSrcFeature = pSourCursor.NextFeature();
                                        }

                                        resultTableRowBuffer.set_Value(targetOID, tarFeature.get_Value(0));

                                        if (pListTarOne2One.Count == 1)
                                        {
                                            resultTableRowBuffer.set_Value(indexChangeMaker, ClsConstant.One2One);
                                        }
                                        else
                                        {
                                            resultTableRowBuffer.set_Value(indexChangeMaker, ClsConstant.More2One);
                                        }
                                    }
                                }
                                else
                                {
                                    resultTableRowBuffer.set_Value(indexChangeMaker, ClsConstant.One2Zero);
                                }
                            }
                            else
                            {
                                resultTableRowBuffer.set_Value(indexChangeMaker, ClsConstant.One2Zero);
                            }

                        }
                        else if (targetFeatcount > 1)
                        {
                            ////Returns an object cursor that can be used to fetch feature objects selected by the specified query.
                            ////teFeatCursor是根据特定的控件过滤条件，源图层查询到的待匹配图层的游标
                            //pTargetCursor = pTarFcls.Search(spatialFilter, false);
                            //tarFeature = pTargetCursor.NextFeature();
                            while (tarFeature != null)
                            {
                                if (ClsIndicatorFunStatic.IncludedAngle(srcFeature, tarFeature) < (int)includeAngel)
                                {
                                    //if (polylineRadio > 0.5)
                                    //double test = ClsIndicatorFunStatic.(int)hausdorffDistance(pSrcFeature, pTarFeature);
                                    //IPolyline planeSour = ClsConvertUnit.ConvertLineCoordinate(
                                    //    (IPolyline) pSrcFeature, ESRI.ArcGIS.esriSystem.esriUnits.esriDecimalDegrees
                                    //    , ESRI.ArcGIS.esriSystem.esriUnits.esriMeters);
                                    if (ClsIndicatorFunStatic.HausdorffDistance(srcFeature, tarFeature) < (int)hausdorff)
                                    {
                                        pListTarMore2More.Add(tarFeature);

                                        //TODO :正反向匹配的反向查询：根据待匹配图层查询源图层
                                        revSpatialFilter = new SpatialFilterClass();
                                        ITopologicalOperator top = tarFeature.Shape as ITopologicalOperator;
                                        revSpatialFilter.Geometry = top.Buffer(buffer) as IGeometry;
                                        //revSpatialFilter.Geometry = tarFeature.Shape;
                                        revSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                                        revSourFeatCount = pSrcFcls.FeatureCount(revSpatialFilter);
                                        pSourCursor = pSrcFcls.Search(revSpatialFilter, false);
                                        revSrcFeature = pSourCursor.NextFeature();

                                        if (revSourFeatCount == 1)
                                        {
                                            //if (!pListTarOne2One.Contains(tarFeature))
                                            //{
                                            //    pListTarOne2One.Add(tarFeature);
                                            //}
                                            resultTableRowBuffer.set_Value(indexChangeMaker, ClsConstant.One2One);

                                        }
                                        else if (revSourFeatCount > 1)
                                        {
                                            while (revSrcFeature != null)
                                            {
                                                if (ClsIndicatorFunStatic.IncludedAngle(revSrcFeature, tarFeature) < (int)includeAngel)
                                                {
                                                    if (ClsIndicatorFunStatic.HausdorffDistance(revSrcFeature,tarFeature) < (int)hausdorff)
                                                    {
                                                        if (!pListTarMore2One.Contains(revSrcFeature))
                                                        {
                                                            pListTarMore2One.Add(revSrcFeature);
                                                        }
                                                    }
                                                }
                                                revSrcFeature = pSourCursor.NextFeature();
                                            }
                                        }

                                        if (resultTableRowBuffer.get_Value(targetOID).ToString() == "")
                                        {
                                            //设置表TRA_PT_I_PtTabl的（待匹配oid）字段的值——cell[2]
                                            resultTableRowBuffer.set_Value(targetOID, tarFeature.get_Value(0).ToString());
                                        }
                                        else
                                        {
                                            string oids = resultTableRowBuffer.get_Value(targetOID).ToString() + ";" +
                                                          tarFeature.get_Value(0).ToString();
                                            //设置表TRA_PT_I_PtTabl的（待匹配oid）字段的值——cell[2]
                                            resultTableRowBuffer.set_Value(targetOID, oids);
                                        }

                                        tarFeature = pTargetCursor.NextFeature();
                                    }

                                    foreach (IFeature feature in pListTarMore2One)
                                    {
                                        if (!resultTableRowBuffer.get_Value(sourceOID).ToString().Contains(feature.OID.ToString()))
                                        {
                                            string oids = resultTableRowBuffer.get_Value(sourceOID).ToString() + ";" +feature.OID.ToString();
                                            resultTableRowBuffer.set_Value(sourceOID, oids);
                                        }
                                    }
                                }
                                tarFeature = pTargetCursor.NextFeature();
                            }
                           
                            //当且仅当所有反向匹配，匹配的源图层要素都为1时，说明匹配对应关系为一对多
                            if (pListTarMore2One.Count == 1 && pListTarMore2More.Count == 1)
                            {
                                resultTableRowBuffer.set_Value(indexChangeMaker, ClsConstant.One2One);
                            }
                            //否则说明匹配关系为多对多
                            else if (pListTarMore2One.Count > 1 && pListTarMore2More.Count == 1)
                            {
                                resultTableRowBuffer.set_Value(indexChangeMaker, ClsConstant.More2One);

                            }
                            else if (pListTarMore2One.Count==1&& pListTarMore2More.Count>1)
                            {
                                resultTableRowBuffer.set_Value(indexChangeMaker, ClsConstant.One2More);
                            }
                            else
                            {
                                resultTableRowBuffer.set_Value(indexChangeMaker, ClsConstant.More2More);
                            }

                            if (pListTarMore2More.Count == 0)
                            {
                                resultTableRowBuffer.set_Value(indexChangeMaker, ClsConstant.One2Zero);
                            }
                        }
                    }

                    resultTableRowCursor.InsertRow(resultTableRowBuffer);

                    //进程条处理
                    ClsStatic.AboutProcessBar(sourceFeatCount, prgMain, prgSub);

                    //每1千个要素就把缓冲区内的要素类写入目标层
                    if (tempFeatCount >= 500)
                    {
                        resultTableRowCursor.Flush();
                        tempFeatCount = 0;
                    }

                    stateLabel.Text = "正在对" + (pSrcFcls as IDataset).Name + "图层和" + (pTarFcls as IDataset).Name + "图层进行数据变化分析，已经处理" + prgMain.Value + "(" + sourceFeatCount + ")个要素。";
                    stateLabel.Refresh();
                    prgMain.Update();
                    prgSub.Update();
                    tempFeatCount = tempFeatCount + 1;
                    srcFeature = pOutSrcCursor.NextFeature();
                }
            }
            catch (System.Exception ex)
            {
                //MessageBox.Show(ex.Message);
                MessageBox.Show(ex.StackTrace);
            }
            if (resultTableRowCursor != null)
            {
                resultTableRowCursor.Flush();
            }

            workspaceEdit.StopEditOperation();
            workspaceEdit.StopEditing(true);

            stateLabel.Text = "";
            prgMain.Value = 0;
            prgSub.Value = 0;
            return true;
        }
        /// <summary>
        /// 计算指标值
        /// </summary>
        /// <param name="weight"></param>
        /// <param name="buffer"></param>
        /// <param name="chkBoxIndicator"></param>
        /// <param name="pSrcFeature"></param>
        /// <param name="pTarFeature"></param>
        /// <param name="rowBuffer"></param>
        private static void CalculateIndcators(double[] weight, double buffer, CheckBox chkBoxIndicator, IFeature pSrcFeature,
            IFeature pTarFeature, IRowBuffer resultTableRowBuffer)
        {
            if (chkBoxIndicator.Checked == true)
            {
                //形状相似度
                double shape = ClsIndicatorFunStatic.PolylineShapeSimilarValue(pSrcFeature, pTarFeature);
                //节点相似度
                double matchedPoints = ClsIndicatorFunStatic.MatchedPointsSimilarValue(pSrcFeature, pTarFeature, buffer);
                //综合相似度
                double polylineRadio = shape * weight[0] + matchedPoints * weight[1];

                string shape1 = string.Format("{0:0.00000000}", shape);
                string matchedPoints1 = string.Format("{0:0.00000000}", matchedPoints);
                string polygonRatio1 = string.Format("{0:0.00000000}", polylineRadio);

                resultTableRowBuffer.set_Value(resultTableRowBuffer.Fields.FindField("形状相似度"), shape1);
                resultTableRowBuffer.set_Value(resultTableRowBuffer.Fields.FindField("节点相似度"), matchedPoints1);
                resultTableRowBuffer.set_Value(resultTableRowBuffer.Fields.FindField("综合相似度"), polygonRatio1);
            }
        }

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

                    rowBuffer.set_Value(rowBuffer.Fields.FindField("源OID"), pSrcFeature.OID);

                    while ((pTarFeature != null))
                    {
                        //int index = pTarFeature.Fields.FindField("FIXOID");
                        //int index = 0;
                        if ((pTarFeature.Shape != null))
                        {
                            if (pTarFeature.Shape.GeometryType == esriGeometryType.esriGeometryPoint)
                            {
                                string pSrcStrName = pSrcFeature.get_Value(pSrcFeature.Fields.FindField(ClsConfig.LayerConfigs[(pSrcFcls as IDataset).Name].NameField)).ToString();
                                string pTarStrName = pTarFeature.get_Value(pTarFeature.Fields.FindField(ClsConfig.LayerConfigs[(pTarFcls as IDataset).Name].NameField)).ToString();

                                //test
                                //int test = StringSameOrNot(pSrcStrName, pTarStrName);
                                //if (StringSameOrNot(pSrcStr, pTarStr) > 0)
                                if (pSrcStrName.Length > 0 && pTarStrName.Length > 0 && ClsStatic.StringSameOrNot(pSrcStrName, pTarStrName) > 1)
                                {
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
                                //int test = StringSameOrNot2(pSrcStr, pTarStr);

                                //都存在名称字段，并且名称匹配的超过2个字符
                                if (pSrcStr.Length > 0 && pTarStr.Length > 0 && ClsStatic.StringSameOrNot2(pSrcStr, pTarStr) > 2)
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
                    ClsStatic.AboutProcessBar(sourceFeatCount, prgMain, prgSub);
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



    }
}
