using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System.Windows.Forms;
using DevComponents.DotNetBar;
namespace ZJGISFinishTool
{
    class ClsSelectAndQuery
    {
        //<CSCM>
        //********************************************************************************
        //** 函 数 名: getFeatSelection
        //** 版    权: CopyRight (C) 
        //** 创 建 人: 杨旭斌
        //** 功能描述: 由非标准的sSQL表达式获得选择集,如果SQL为空的话,默认选择要素类内的所有要素
        //** 创建日期:
        //** 修 改 人:
        //** 修改日期:
        //** 修改时间: 20070813
        //** 参数列表: pFeatSelection (IFeatureSelection)传入的选择集,
        //             sSQLExpress (String)
        //             pDomainGeometry  如果有范围的时和范围一起进行查寻,提高效率
        //             EnumSelectMethod 选择方式:添加,新建等
        //** 版    本:1.0
        //*********************************************************************************
        //</CSCM>
        public static IFeatureSelection GetFeatSelection(ref IFeatureLayer pFeatLayer, string sSQLExpress, esriSelectionResultEnum EnumSelectMethod, esriSpatialRelEnum EnumSpatialRel, ref IGeometry pDomainGeometry)
        {
            //sSQLExpress = "";
            //EnumSelectMethod = esriSelectionResultEnum.esriSelectionResultNew;
            //EnumSpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            //pDomainGeometry = null;

            IFeatureSelection functionReturnValue = default(IFeatureSelection);
            ISpatialFilter pSpatialfilter = default(ISpatialFilter);
            ITopologicalOperator2 pTopoOperator = default(ITopologicalOperator2);
            IFeatureSelection pFeatSelection = default(IFeatureSelection);
            // ERROR: Not supported in C#: OnErrorStatement

            try
            {
                if (pFeatLayer == null)
                {
                    functionReturnValue = null;
                    return functionReturnValue;
                }

                pFeatSelection = (IFeatureSelection)pFeatLayer;
                pSpatialfilter = new SpatialFilter();
                pSpatialfilter.WhereClause = sSQLExpress;
                //范围一起查询,提高查询效率
                if (pDomainGeometry != null)
                {
                    pTopoOperator = (ITopologicalOperator2)pDomainGeometry;
                    pTopoOperator.IsKnownSimple_2 = false;
                    pTopoOperator.Simplify();
                    pSpatialfilter.Geometry = (IGeometry)pTopoOperator;
                    pSpatialfilter.SpatialRel = EnumSpatialRel;
                }
                //获得一个新的选择集
                pFeatSelection.SelectFeatures(pSpatialfilter, EnumSelectMethod, false);
                functionReturnValue = pFeatSelection;
                return functionReturnValue;
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {
                if (ex.ErrorCode == -2147216117)
                {
                    MessageBoxEx.Show("失去了与SDE服务器的连接,提取失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                }
                functionReturnValue = null;
                return functionReturnValue;
            }

        }


        //<CSCM>
        //********************************************************************************
        //** 函 数 名: GetCursorByFieldAndValue
        //** 版    权: CopyRight (C) 
        //** 创 建 人: 杨旭斌
        //** 功能描述: 通过某字段的值获得与该字段对应的要素,比如通过图幅号获得某个图幅,
        //             该函数返回一个光标,用户从光标中取需要的要素,如果是多个字段,则调用 GetFeatCursor 即可
        //** 创建日期:
        //** 修 改 人:
        //** 修改日期:
        //** 修改时间: 20070817
        //** 参数列表: sValue (String)
        //         sFieldName (String)
        //         sFieldNameLeftgBrace (String)
        //         sFieldNameRightBrace (String)
        //         pFeatCls (IFeatureClass)
        //** 版    本:1.0
        //*********************************************************************************
        //</CSCM>
        public static IFeatureCursor GetCursorByFieldAndValue(string sValue, string sFieldName, ref IFeatureClass pFeatCls, bool bIsOID)
        {
            //bIsOID = false;

            IFeatureCursor functionReturnValue = default(IFeatureCursor);
            IQueryFilter pQueryFilter = default(IQueryFilter);
            IFeatureCursor pFeatCursor = default(IFeatureCursor);
            string sSQL = null;
            string sOIDName = null;
            int lFieldIndex = 0;
            IField pField = default(IField);

            // ERROR: Not supported in C#: OnErrorStatement
            try
            {
                if (bIsOID == true)
                {
                    sOIDName = pFeatCls.OIDFieldName;
                    sSQL = sOIDName + "=" + sValue;
                }
                else
                {
                    lFieldIndex = pFeatCls.Fields.FindField(sFieldName);
                    pField = pFeatCls.Fields.get_Field(lFieldIndex);
                    if (pField.Type == ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeString)
                    {
                        sSQL = sFieldName + "='" + sValue + "'";
                    }
                    else if ((int)pField.Type < 4 | (int)pField.Type == 6)
                    {
                        sSQL = sFieldName + "=" + sValue;
                    }
                    else
                    {
                        functionReturnValue = null;
                        return functionReturnValue;
                    }
                }
                pQueryFilter = new ESRI.ArcGIS.Geodatabase.QueryFilter();
                pQueryFilter.WhereClause = sSQL;
                pFeatCursor = pFeatCls.Search(pQueryFilter, false);
                functionReturnValue = pFeatCursor;
                return functionReturnValue;
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {
                if (ex.ErrorCode == -2147216117)
                {
                    MessageBoxEx.Show("失去了与SDE服务器的连接,提取失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                }
                functionReturnValue = null;
                return functionReturnValue;
            }
        }
        //<CSCM>
        //********************************************************************************
        //** 函 数 名: GetCursorByFieldAndValue
        //** 版    权: CopyRight (C) 
        //** 创 建 人: 杨旭斌
        //** 功能描述: 给出查询信息后,返回一个满足范围的光标
        //             
        //** 创建日期:
        //** 修 改 人:
        //** 修改日期:
        //** 修改时间: 20070817
        //** 参数列表: sValue (String)
        //         sFieldName (String)
        //         pFeatCls (IFeatureClass)
        //** 版    本:1.0
        //*********************************************************************************
        //</CSCM>
        public static IFeatureCursor GetFeatCursor(IFeatureClass pFeatCls, string sSQLExpress, esriSpatialRelEnum EnumSpatialRel,
                                            ref IGeometry pDomainGeometry, bool bGetFeatCount, ref long lFeatCount)
        {
            //sSQLExpress = "";
            //EnumSpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            //pDomainGeometry = null;
            //bGetFeatCount = false;
            //lFeatCount = 0;

            IFeatureCursor functionReturnValue = default(IFeatureCursor);
            ISpatialFilter pSpatialfilter = default(ISpatialFilter);
            ITopologicalOperator2 pTopoOperator = default(ITopologicalOperator2);
            IFeatureCursor pFeatCursor = default(IFeatureCursor);
            // ERROR: Not supported in C#: OnErrorStatement

            try
            {
                if (pFeatCls == null)
                {
                    functionReturnValue = null;
                    return functionReturnValue;
                }
                pSpatialfilter = new SpatialFilter();
                pSpatialfilter.WhereClause = sSQLExpress;
                //范围一起查询,提高查询效率
                if (pDomainGeometry != null)
                {
                    pTopoOperator = (ITopologicalOperator2)pDomainGeometry;
                    pTopoOperator.IsKnownSimple_2 = false;
                    pTopoOperator.Simplify();
                    pSpatialfilter.Geometry = (IGeometry)pTopoOperator;
                    pSpatialfilter.SpatialRel = EnumSpatialRel;
                }
                //获得一个新的选择集
                pFeatCursor = pFeatCls.Search(pSpatialfilter, false);
                //如果需要获得要满足条件的个数，则获得
                if (bGetFeatCount)
                {
                    lFeatCount = pFeatCls.FeatureCount(pSpatialfilter);
                }
                functionReturnValue = pFeatCursor;
                return functionReturnValue;
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {
                if (ex.ErrorCode == -2147216117)
                {
                    MessageBoxEx.Show("失去了与SDE服务器的连接,提取失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                }
                functionReturnValue = null;
                return functionReturnValue;
            }

        }

        //<CSCM>
        //********************************************************************************
        //** 函 数 名: ClassifyFeatCursorByGeometry
        //** 版    权: CopyRight (C) 
        //** 创 建 人: 杨旭斌
        //** 功能描述: 把位于某个选择集内的与geometry图形包含或相交的要素集分开,
        //             分别返回位于内部与相交的FeatureCursor
        //             主要是辅助完成剪切功能
        //** 创建日期:
        //** 修 改 人:
        //** 修改日期:
        //** 修改时间:20070848
        //** 参数列表: pFeatSelection (IFeatureSelection)
        //             pGeometry (IGeometry)
        //         pWithInFeatCursor (IFeatureCursor),位于传入的geometry内部的要素集
        //         pCrossFeatCursor (IFeatureCursor)与传入的geometry相交的要素集
        //         EnumGeometrType (esriGeometryType) ,pFeatSelection对应的要素类的类型:点,线,面等
        //** 版    本:1.0
        //*********************************************************************************
        //</CSCM>
        //使用geometry获得要素类的设计思想:
        //首选获得用户需要的geometry,如果有多个的话进行合并并形成GeometryCollection:
        //其次获得与某个geometry相交,相切,包含等关系的所有要素,
        //如果用户需要进行剪切的话,则首行获得位于内部的,其次再获得相交的,再使用iTopoOperator接口进行剪切

        //Public Function ClassifyFeatCursorByGeometry(ByRef pFeatSelection As ESRI.ArcGIS.Carto.IFeatureSelection, ByVal pGeometry As ESRI.ArcGIS.Geometry.IGeometry, _
        //                ByRef pWithINFeatCursor As ESRI.ArcGIS.Geodatabase.IFeatureCursor, ByRef pCrossFeatCursor As ESRI.ArcGIS.Geodatabase.IFeatureCursor, _
        //                ByVal EnumGeometrType As ESRI.ArcGIS.Geometry.esriGeometryType) As Boolean
        public static bool ClassifyFeatCursorByGeometry(ref IFeatureSelection pFeatSelection, IGeometry pGeometry, ref IFeatureCursor pFeatCursor, esriGeometryType EnumGeometrType)
        {
            bool functionReturnValue = false;
            ISpatialFilter pSpatialfilter = default(ISpatialFilter);
            IFeature pFeat = default(IFeature);

            // ERROR: Not supported in C#: OnErrorStatement

            try
            {
                pSpatialfilter = new SpatialFilter();
                pSpatialfilter.Geometry = pGeometry;
                pSpatialfilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                pFeatSelection.SelectFeatures(pSpatialfilter, esriSelectionResultEnum.esriSelectionResultNew, false);
                // 陈昉  2009-3-12  修改 修改原因 获得与此Geometry相交的要素，
                //原来的方法不能搜索到包含此Geometry的要素
                //pSpatialfilter.SpatialRel = ESRI.ArcGIS.Geodatabase.esriSpatialRelEnum.esriSpatialRelContains
                pSpatialfilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelRelation;
                pSpatialfilter.SpatialRelDescription = "T********";

                ICursor pCursor = (ICursor)pFeatCursor;
                pFeatSelection.SelectionSet.Search(pSpatialfilter, false, out pCursor);
                pFeatCursor = (IFeatureCursor)pCursor; 
                //获得位于geometry内部的要素集
                //pSpatialfilter = Nothing
                //'获得与geometry相交的要素
                //pSpatialfilter = New ESRI.ArcGIS.Geodatabase.SpatialFilter
                //pSpatialfilter.Geometry = pGeometry

                //'线要素 交叉
                //If EnumGeometrType = ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryLine Or EnumGeometrType = ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolyline Or EnumGeometrType = ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryRing Or EnumGeometrType = ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPath Then
                //    pSpatialfilter.SpatialRel = ESRI.ArcGIS.Geodatabase.esriSpatialRelEnum.esriSpatialRelCrosses
                //    '面要素,相交表现的是重叠的关系
                //ElseIf EnumGeometrType = ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon Then
                //    pSpatialfilter.SpatialRel = ESRI.ArcGIS.Geodatabase.esriSpatialRelEnum.esriSpatialRelOverlaps
                //End If

                //pFeatSelection.SelectionSet.Search(pSpatialfilter, False, pCrossFeatCursor)
                functionReturnValue = true;
                return functionReturnValue;
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {
                if (ex.ErrorCode == -2147216117)
                {
                    MessageBoxEx.Show("失去了与SDE服务器的连接,提取失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                }
                functionReturnValue = false;
                return functionReturnValue;
            }

        }

        public static IFeatureCursor GetFeatCursorBySql(string sValue, string sFieldName, string sFieldNameLeftgBrace, string sFieldNameRightBrace, ref IFeatureClass pFeatCls, ref bool bIsOID, ref bool bShowErrInfo)
        {
            //bIsOID = false;
            //bShowErrInfo = true;

            IFeatureCursor functionReturnValue = default(IFeatureCursor);
            IQueryFilter pQueryFilter = default(IQueryFilter);
            IFeatureCursor pFeatCursor = default(IFeatureCursor);
            string sSQL = null;
            string sOIDName = null;
            IField pField = default(IField);
            int iFieldIndex = 0;

            //On Error GoTo GetFeatCursor_ERR
            if (bIsOID == true)
            {
                sOIDName = pFeatCls.OIDFieldName;
                sSQL = sOIDName + "=" + sValue;
            }
            else
            {
                iFieldIndex = pFeatCls.Fields.FindField(sFieldName);
                if (iFieldIndex != -1)
                {
                    pField = pFeatCls.Fields.get_Field(iFieldIndex);
                    if (pField.Type == esriFieldType.esriFieldTypeString)
                    {
                        sSQL = sFieldName + "='" + sValue + "'";
                    }
                    else
                    {
                        sSQL = sFieldName + "=" + sValue;
                    }
                }
                else
                {
                    functionReturnValue = null;
                    return functionReturnValue;
                }
            }
            try
            {
                pQueryFilter = new QueryFilter();
                pQueryFilter.WhereClause = sSQL;
                pFeatCursor = pFeatCls.Search(pQueryFilter, false);
                functionReturnValue = pFeatCursor;
            }
            catch
            {
                if (bShowErrInfo)
                {
                    MessageBoxEx.Show("失去了与SDE服务器的连接,提取失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                }
                functionReturnValue = null;
            }
            return functionReturnValue;

        }

    }
}
