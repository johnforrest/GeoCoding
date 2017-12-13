using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevComponents.DotNetBar.Controls;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using Microsoft.VisualBasic;
using System.IO;

namespace ZJGISDataExtract
{
    class ClsCreateFeat
    {
        public static long OutPutFeat(ref IFeatureClass pDesFeatCls, ref IFeatureClass pOriFeatCls, string sSql, IGeometry pDomainGeometry, bool bIsCut, ref ISpatialReference pSpatialReference, Dictionary<string, string> pDicField, ref LabelX lblInfo, string sInfo)
        {
            //sSql = "";
            //pDomainGeometry = null;
            //bIsCut = false;
            //pSpatialReference = null;
            //pDicField = null;
            //vProgressBar = null;
            //lblInfo = null;
            //sInfo = null;

            long functionReturnValue = 0;
            //DevComponents.DotNetBar.LabelItem
            IFeatureCursor pWithINFeatCursor = null;
            IFeatureCursor pCrossFeatCursor = null;
            IFeatureCursor pFeatCursor = default(IFeatureCursor);
            IFeatureLayer pFeatLayer = default(IFeatureLayer);
            IFeatureSelection pFeatSelection = default(IFeatureSelection);

            int lFeatCount = 0;
            int l = 0;
            int j = 0;

            bool bInsertRight = true;
            //Insert是否成功

            // ERROR: Not supported in C#: OnErrorStatement


            if (pDesFeatCls == null | pOriFeatCls == null)
                return functionReturnValue;

            if (pDesFeatCls.ShapeType == esriGeometryType.esriGeometryPoint)
            {
                bIsCut = false;
            }

            if (pDomainGeometry == null)
            {
                bIsCut = false;
            }

            //如果需要剪切,则对面要素类和线要素类分成范围内的要素和与范围相交的要素,提高提取效率
            //否则,使用featurecursor进行提取,不进行选择,相对要快

            if (lblInfo != null)
            {
                lblInfo.Text = sInfo + "正在获得满足条件的要素，请稍候.....";
                lblInfo.Refresh();
                Application.DoEvents();
            }

            if (bIsCut)
            {
                pFeatLayer = new FeatureLayer();
                pFeatLayer.FeatureClass = pOriFeatCls;
                pFeatLayer.Name = pOriFeatCls.AliasName;
              
                //获得选择集
                pFeatSelection = ClsSelectAndQuery.GetFeatSelection(ref pFeatLayer, sSql, esriSelectionResultEnum.esriSelectionResultNew, esriSpatialRelEnum.esriSpatialRelIntersects, ref pDomainGeometry);
                lFeatCount = pFeatSelection.SelectionSet.Count;          

                //获得位范围内的以及与范围相交的要素集
                ClsSelectAndQuery.ClassifyFeatCursorByGeometry(ref pFeatSelection, pDomainGeometry, ref pWithINFeatCursor, pDesFeatCls.ShapeType);
                // 陈昉  2009-3-12  修改 修改原因 原来的方法不能搜索到包含一此Geometry的要素

                //ClassifyFeatCursorByGeometry(pFeatSelection, pDomainGeometry, pWithINFeatCursor, pCrossFeatCursor, pDesFeatCls.ShapeType)

                if (lblInfo != null)
                {
                    lblInfo.Text = sInfo + "正在输出要素，请稍候.....";
                    lblInfo.Refresh();
                    Application.DoEvents();
                }

                InsertFeatIntoFeatClsByCursor(ref pDesFeatCls, ref pWithINFeatCursor, true, pDomainGeometry, ref pSpatialReference, pDicField);
                //首先把位于图幅内的要素插入

                //InsertFeatIntoFeatClsByCursor(pDesFeatCls, pWithINFeatCursor, True, pDomainGeometry, pSpatialReference, _
                //                              pDicField, vProgressBar)
                //'再把与范围相交的要素插入,(插入时需要进行剪切)
                //InsertFeatIntoFeatClsByCursor(pDesFeatCls, pCrossFeatCursor, True, pDomainGeometry, pSpatialReference, _
                //                              pDicField, vProgressBar)
            }
            else
            {
                //获得需要提取的要素
                long lFeatCount2 = (long)lFeatCount;
                pFeatCursor = ClsSelectAndQuery.GetFeatCursor(pOriFeatCls, sSql, esriSpatialRelEnum.esriSpatialRelIntersects, ref pDomainGeometry, true, ref lFeatCount2);
                lFeatCount = (int)lFeatCount2;
                if (lblInfo != null)
                {
                    lblInfo.Text = sInfo + "正在输出要素，请稍候.....";
                    lblInfo.Refresh();
                    Application.DoEvents();
                }

                bInsertRight = InsertFeatIntoFeatClsByCursor(ref pDesFeatCls, ref pFeatCursor, false, pDomainGeometry, ref pSpatialReference, pDicField);

                //印骅 20081202 Insert失败，退出函数
                if (bInsertRight == false)
                {
                    functionReturnValue = -1;
                    return functionReturnValue;
                }
            }
            //杨旭斌于20080825添加，返回提取的要素个数
            functionReturnValue = lFeatCount;

            pWithINFeatCursor = null;
            pCrossFeatCursor = null;
            return functionReturnValue;
        }


        //<CSCM>
        //********************************************************************************
        //** 函 数 名: InsertFeatIntoFeatClsByCursor
        //** 版    权: CopyRight (C) 
        //** 创 建 人: 杨旭斌
        //** 功能描述: 通过FeatureCursor向目标要素类中插入要素
        //** 创建日期:20070818
        //** 修 改 人:
        //** 修改日期:
        //** 修改时间: 
        //** 参数列表: pDesFeatCls (IFeatureClass)
        //             pFeatCursor (IFeatureCursor)
        //** 版    本:1.0
        //*********************************************************************************
        //</CSCM>
        public static bool InsertFeatIntoFeatClsByCursor(ref IFeatureClass pDesFeatCls, ref IFeatureCursor pFeatCursor, bool bIsCut, IGeometry pDomainGeometry, ref ISpatialReference pSpatialReference, Dictionary<string, string> pDicField)
        {
            //bIsCut = false;
            //pDomainGeometry = null;
            //pSpatialReference = null;
            //pDicField = null;
            //vProgressBar = null;

            bool functionReturnValue = false;
            //要素类,用于循环
            IFeature pFeat = default(IFeature);
            //用新建要素类的游标,使用该游标向新要素类中插入要素,并写入数据库
            IFeatureCursor pDesFeatCursor = default(IFeatureCursor);
            //新建要素类创建的要素缓冲区
            IFeatureBuffer pDesFeatBuffer = default(IFeatureBuffer);

            int iFeatCount = 0;
            IGeometry pGeometry = default(IGeometry);
            esriGeometryDimension EnumGeometryDimension = default(esriGeometryDimension);
            IGeometry pDesGeometry = default(IGeometry);

            bool bTransferRight = true;
            //记录InsertOneFeatIntoCursor的返回值

            //On Error Resume Next
            if (pDomainGeometry == null)
            {
                bIsCut = false;
            }

            if (pDesFeatCls.ShapeType == esriGeometryType.esriGeometryPolyline)
            {
                EnumGeometryDimension = esriGeometryDimension.esriGeometry1Dimension;
            }
            else if (pDesFeatCls.ShapeType == esriGeometryType.esriGeometryPolygon)
            {
                EnumGeometryDimension = esriGeometryDimension.esriGeometry2Dimension;
            }
            else
            {
                bIsCut = false;
            }

            try
            {
                pFeat = pFeatCursor.NextFeature();
                if (pFeat != null)
                {
                    //从新建要素类中获得新游标,用于插入新的要素
                    pDesFeatCursor = pDesFeatCls.Insert(true);
                    //创建要素缓冲区,和insert cursor 配合,创建新要素
                    pDesFeatBuffer = pDesFeatCls.CreateFeatureBuffer();
                    while (pFeat != null)
                    {
                        pGeometry = pFeat.ShapeCopy;
                        if (pGeometry != null)
                        {
                            if (pGeometry.IsEmpty == false)
                            {
                                iFeatCount = iFeatCount + 1;

                                //进行剪切或不剪切
                                if (bIsCut)
                                {
                                    pDesGeometry = ClsGeometryOperator.CutFeatByGeometry(ref pGeometry, ref pDomainGeometry, ref EnumGeometryDimension);
                                }
                                else
                                {
                                    pDesGeometry = pGeometry;
                                }

                                bTransferRight = InsertOneFeatIntoCursor(ref pFeat, pDesGeometry, ref pDesFeatCursor, ref pDesFeatBuffer, pSpatialReference, ref pDicField);
                                //印骅 20081202 如果投影失败 则跳出函数
                                if (bTransferRight == false)
                                {
                                    return functionReturnValue;
                                }
                                //每一千个要素就把缓冲区内的要素类写入数据库
                                if (iFeatCount >= 1000)
                                {
                                    pDesFeatCursor.Flush();
                                    iFeatCount = 0;
                                    System.Windows.Forms.Application.DoEvents();
                                }                      
                            }
                        }
                        pFeat = pFeatCursor.NextFeature();
                    }
                    if (iFeatCount % 1000 != 0)
                    {
                        pDesFeatCursor.Flush();
                    }
                    pDesFeatCursor = null;
                    pDesFeatBuffer = null;
                }
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {
                if (ex.ErrorCode == -2147216556)
                {
                    functionReturnValue = false;
                    //g_clsErrorHandle.DisplayInformation(string.Format("目标数据{0}正被其他程序使用，请检查", ((IDataset)pDesFeatCls).Name), false);
                    MessageBoxEx.Show(string.Format("目标数据{0}正被其他程序使用，请检查", ((IDataset)pDesFeatCls).Name), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            functionReturnValue = true;
            return functionReturnValue;
            //正常退出，返回true
        }


        //<CSCM>
        //********************************************************************************
        //** 函 数 名: InsertFeatIntoFeatClsByEnumIDs
        //** 版    权: CopyRight (C) 
        //** 创 建 人: 杨旭斌
        //** 功能描述: 把EnumIDs中的要素插入到目标要素类中
        //** 创建日期:
        //** 修 改 人:
        //** 修改日期:
        //** 修改时间:20070818
        //** 参数列表: pDesFeatCls (IFeatureClass)
        //             pEnumIDs (IEnumIDs)
        //             pOriFeatCls "原要素类(把该要素类中的要素插入到目标要素类中)
        //** 版    本:1.0
        //*********************************************************************************
        //</CSCM>
        public static long InsertFeatIntoFeatClsByEnumIDs(ref IFeatureClass pDesFeatCls, ref IFeatureClass pOriFeatCls, ref IEnumIDs pEnumIDs, bool bIsCut, IGeometry pDomainGeometry, ref ISpatialReference pSpatialReference, ref Dictionary<string, string> pDicField, ref ProgressBar vProgressBar)
        {
            //bIsCut = false;
            //pDomainGeometry = null;
            //pSpatialReference = null;
            //pDicField = null;
            //vProgressBar = null;

            long functionReturnValue = 0;
            IFeature pFeat = default(IFeature);
            //要素类,用于循环
            IFeatureCursor pDesFeatCursor = default(IFeatureCursor);
            //用新建要素类的游标,使用该游标向新要素类中插入要素,并写入数据库
            IFeatureBuffer pDesFeatBuffer = default(IFeatureBuffer);
            //新建要素类创建的要素缓冲区,

            int iFeatCount = 0;
            IGeometry pGeometry = default(IGeometry);
            long lID = 0;
            //Dim pGeometryCol As ESRI.ArcGIS.Geometry.IGeometryCollection
            //Dim j As Integer
            //Dim pSubGeometry As ESRI.ArcGIS.Geometry.IGeometry
            //Dim pOriPntCol As ESRI.ArcGIS.Geometry.IPointCollection
            //Dim pPntCol As ESRI.ArcGIS.Geometry.IPointCollection
            IPoint pPnt = default(IPoint);
            IGeometry pDesGeometry = default(IGeometry);
            esriGeometryDimension EnumGeometryDimension = default(esriGeometryDimension);

            // ERROR: Not supported in C#: OnErrorStatement

            if (pDomainGeometry == null)
                bIsCut = false;

            if (pDesFeatCls.ShapeType == esriGeometryType.esriGeometryPolyline)
            {
                EnumGeometryDimension = esriGeometryDimension.esriGeometry1Dimension;
            }
            else if (pDesFeatCls.ShapeType == esriGeometryType.esriGeometryPolygon)
            {
                EnumGeometryDimension = esriGeometryDimension.esriGeometry2Dimension;
            }
            else
            {
                bIsCut = false;
            }

            pEnumIDs.Reset();
            lID = pEnumIDs.Next();
            if (lID != -1)
            {
                //从新建要素类中获得新游标,用于插入新的要素
                pDesFeatCursor = pDesFeatCls.Insert(true);
                //创建要素缓冲区,和insert cursor 配合,创建新要素
                pDesFeatBuffer = pDesFeatCls.CreateFeatureBuffer();
            }
            else
            {
                return functionReturnValue;
            }

            if (lID > -1)
            {
                while (lID > -1)
                {
                    pFeat = pOriFeatCls.GetFeature((int)lID);
                    pGeometry = pFeat.ShapeCopy;
                    if ((pGeometry != null))
                    {
                        if (pGeometry.IsEmpty == false)
                        {
                            //进行剪切或不剪切
                            if (bIsCut)
                            {
                                pDesGeometry = ClsGeometryOperator.CutFeatByGeometry(ref pGeometry, ref pDomainGeometry, ref EnumGeometryDimension);
                            }
                            else
                            {
                                pDesGeometry = pGeometry;
                            }
                            InsertOneFeatIntoCursor(ref pFeat, pDesGeometry, ref pDesFeatCursor, ref pDesFeatBuffer, pSpatialReference, ref pDicField);
                        }
                    }
                    lID = pEnumIDs.Next();

                    //每一千个要素就把缓冲区内的要素类写入数据库
                    if (iFeatCount >= 1000)
                    {
                        pDesFeatCursor.Flush();
                        System.Windows.Forms.Application.DoEvents();
                        iFeatCount = 0;
                    }

                    if ((vProgressBar != null))
                    {
                        vProgressBar.Value = vProgressBar.Value + 1;
                    }
                }

                if (iFeatCount % 1000 != 0)
                {
                    pDesFeatCursor.Flush();
                }

            }
            if (vProgressBar != null)
            {
                vProgressBar.Value = 0;
                vProgressBar.Visible = false;
            }
            pDesFeatCursor = null;
            pDesFeatBuffer = null;
            return functionReturnValue;
        }
        //<CSCM>
        //********************************************************************************
        //** 函 数 名: InsertOneFeatIntoCursor
        //** 版    权: CopyRight (C)  
        //** 创 建 人: 杨旭斌
        //** 功能描述:   '新建一个要素,配合要素游标进行,因为三个插入要素的函数均用到此函数,所以就独立出来,
        //               如果后来要把要素打散的话,会现容易修改一点
        //               pFeatBuffer: 新建的要素  pGeometry新插入要素的图形   pFeat:插入的参照要素, pSpatialReference如果进行空间参考变换的话,新的空间参考
        //               pFieldDic 提取时新旧字段的对照关系
        //** 创建日期:
        //** 修 改 人:
        //** 修改日期:
        //** 修改时间: 20080825
        //** 参数列表: pOriFeat (IFeature)
        //             pGeometry (IGeometry) 
        //             pDesFeatCursor (IFeatureCursor)
        //             pDesFeatBuffer (IFeatureBuffer)
        //** 版    本:1.0
        //*********************************************************************************
        //</CSCM>
        public static bool InsertOneFeatIntoCursor(ref IFeature pOriFeat, IGeometry pGeometry, ref IFeatureCursor pDesFeatCursor, ref IFeatureBuffer pDesFeatBuffer, ISpatialReference pSpatialReference, ref Dictionary<string, string> pDicField)
        {
            bool functionReturnValue = false;
            //pSpatialReference = null;
            //pDicField = null;

            if (pGeometry == null | pOriFeat == null | pDesFeatCursor == null | pDesFeatBuffer == null)
                return functionReturnValue;
            if (pGeometry.IsEmpty)
                return functionReturnValue;
            // ERROR: Not supported in C#: OnErrorStatement


            //如果需要进行投影变换的话,进行投影变换
            if (pSpatialReference != null)
            {
                pGeometry.Project(pSpatialReference);
                //印骅 20081201 投影不成功则退出函数
                if (pGeometry.IsEmpty)
                {
                    return functionReturnValue;
                }
            }

            //赋属性
            SetFieldsValue(ref pOriFeat, ref pDesFeatBuffer, ref pDicField);

            //判断Z值
            ValidateZValue(ref pGeometry);
            //赋图形
            pDesFeatBuffer.Shape = pGeometry;

            //插入要素
            pDesFeatCursor.InsertFeature(pDesFeatBuffer);

            functionReturnValue = true;
            return functionReturnValue;
        ERR:
            functionReturnValue = false;
            return functionReturnValue;
        }


        //如果是要素具有Z值，则判断该要素的Z值是否有效，
        //无效时对其进行剪切后赋值给新的要素时会出错
        public static bool ValidateZValue(ref IGeometry pDesGeometry)
        {
            IZAware pZAware = default(IZAware);
            IPoint pPnt = default(IPoint);
            IPointCollection pPntCol = default(IPointCollection);
            int i = 0;
            double dZ = 0;
            int lValidPntNum = 0;
            pZAware = (IZAware)pDesGeometry;
            if (pZAware.ZAware == true)
            {
                if (pZAware.ZSimple == false)
                {
                    pPntCol = (IPointCollection)pDesGeometry;
                    //取所有有效点的高程平均值
                    for (i = 0; i <= pPntCol.PointCount - 1; i++)
                    {
                        pPnt = pPntCol.get_Point(i);
                        pZAware = (IZAware)pPnt;
                        if (pZAware.ZSimple == true)
                        {
                            dZ = dZ + pPnt.Z;
                            lValidPntNum = lValidPntNum + 1;
                        }
                    }
                    dZ = dZ / lValidPntNum;
                    for (i = 0; i <= pPntCol.PointCount - 1; i++)
                    {
                        pPnt = pPntCol.get_Point(i);
                        pZAware = (IZAware)pPnt;
                        if (pZAware.ZSimple == false)
                        {
                            pPnt.Z = dZ;
                            pPntCol.UpdatePoint(i, pPnt);
                        }
                    }
                }
            }
            return true;
        }


        private static void SetFieldsValue(ref IFeature pFeat, ref IFeatureBuffer pDesFeatBuffer, ref Dictionary<string, string> pDicField)
        {
            //pDicField = null;

            int lIndex = 0;
            //要素缓冲区中的字段索引
            int lDesFieldIndex = 0;
            string sFieldName = null;
            //要素缓冲区中的字段的名称
            IField pField = default(IField);

            //根据原字段和目标字段集之间的关系,可以反回来循环
            if (pDicField != null)
            {
                for (lIndex = 0; lIndex <= pFeat.Fields.FieldCount - 1; lIndex++)
                {
                    pField = pFeat.Fields.get_Field(lIndex);
                    if (pField.Editable == true & pField.Type != esriFieldType.esriFieldTypeGeometry)
                    {
                        sFieldName = pField.Name.Trim();
                        if (pDicField.ContainsKey(sFieldName))
                        {
                            sFieldName = pDicField[sFieldName];
                            lDesFieldIndex = pDesFeatBuffer.Fields.FindField(sFieldName);
                            if (lDesFieldIndex > -1)
                            {

                                if (string.IsNullOrEmpty(Convert.ToString( pFeat.get_Value(lIndex))) == false)
                                {
                                    pDesFeatBuffer.set_Value(lDesFieldIndex, pFeat.get_Value(lIndex));
                                }
                                else
                                {
                                    //字段不可以为null,则赋值为" "
                                    if (pField.IsNullable)
                                    {
                                        pDesFeatBuffer.set_Value(lDesFieldIndex, System.DBNull.Value);
                                    }
                                    else
                                    {
                                        if (pField.Type == esriFieldType.esriFieldTypeString)
                                        {
                                            pDesFeatBuffer.set_Value(lDesFieldIndex, " ");
                                        }
                                        else if (pField.Type == esriFieldType.esriFieldTypeDouble | pField.Type == esriFieldType.esriFieldTypeInteger | pField.Type == esriFieldType.esriFieldTypeSingle)
                                        {
                                            pDesFeatBuffer.set_Value(lDesFieldIndex, 0);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                for (lIndex = 0; lIndex <= pDesFeatBuffer.Fields.FieldCount - 1; lIndex++)
                {
                    if (pDesFeatBuffer.Fields.get_Field(lIndex).Editable)
                    {
                        sFieldName = pDesFeatBuffer.Fields.get_Field(lIndex).Name.Trim();
                        //给特征缓冲区中的特定字段赋值
                        lDesFieldIndex = pFeat.Fields.FindField(sFieldName);
                        if (lDesFieldIndex > -1)
                        {
                            if (string.IsNullOrEmpty(pFeat.get_Value(lDesFieldIndex).ToString()) == false)
                            {
                                pDesFeatBuffer.set_Value(lIndex, pFeat.get_Value(lDesFieldIndex));
                            }
                            else
                            {
                                //字段不可以为null,则赋值为" "
                                if (pDesFeatBuffer.Fields.get_Field(lIndex).IsNullable)
                                {
                                    pDesFeatBuffer.set_Value(lIndex, System.DBNull.Value);
                                }
                                else
                                {
                                    if (pDesFeatBuffer.Fields.get_Field(lIndex).Type == esriFieldType.esriFieldTypeString)
                                    {
                                        pDesFeatBuffer.set_Value(lIndex, " ");
                                    }
                                    else if (pDesFeatBuffer.Fields.get_Field(lIndex).Type == esriFieldType.esriFieldTypeDouble | pDesFeatBuffer.Fields.get_Field(lIndex).Type == esriFieldType.esriFieldTypeInteger | pDesFeatBuffer.Fields.get_Field(lIndex).Type == esriFieldType.esriFieldTypeSingle)
                                    {
                                        pDesFeatBuffer.set_Value(lIndex, 0);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        //格式转换后，检验字段的有效性并修复字段 YH 15/4/09
        public static bool FieldsCheckAfterTransaction(IWorkspace pInputWorkspace, IWorkspace pOutPutWorkspace, IFields pInputFields, ref IFields pOoutPutFields)
        {
            bool functionReturnValue = false;
            IFieldChecker pFieldChecker = default(IFieldChecker);
            IEnumFieldError pEnumFieldError = null;
            pFieldChecker = new FieldChecker();
            pFieldChecker.InputWorkspace = pInputWorkspace;
            pFieldChecker.ValidateWorkspace = pOutPutWorkspace;
            pFieldChecker.Validate(pInputFields, out pEnumFieldError, out pOoutPutFields);

            //处理字段中不符合语义规则的情况
            if ((pEnumFieldError != null))
            {
                pEnumFieldError.Reset();
                IFieldError pFieldError = default(IFieldError);
                pFieldError = pEnumFieldError.Next();
                //g_clsErrorHandle.DisplayInformation("导出字段中存在无效字段: " + pOoutPutFields.Field(pFieldError.FieldIndex).Name + "导出终止!!", false);
                MessageBoxEx.Show("导出字段中存在无效字段: " + pOoutPutFields.get_Field(pFieldError.FieldIndex).Name + "导出终止!!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
                return functionReturnValue;
            }
            return true;
            return functionReturnValue;
        }

        //判断是否需要创建要素类
        public static bool CheckFeatCls(bool bIsCut, IFeatureLayer pFeatLayer, ref int lFeatCount, IGeometry pDomainGeometry)
        {
            //pDomainGeometry = null;

            IFeatureSelection pFeatSelection = default(IFeatureSelection);
            if (bIsCut)
            {
                pFeatSelection = ClsSelectAndQuery.GetFeatSelection(ref pFeatLayer, "", esriSelectionResultEnum.esriSelectionResultNew, esriSpatialRelEnum.esriSpatialRelIntersects, ref pDomainGeometry);
                lFeatCount = pFeatSelection.SelectionSet.Count;
            }
            else
            {
                long lFeatCount2 = (long)lFeatCount;
                ClsSelectAndQuery.GetFeatCursor(pFeatLayer.FeatureClass, "", esriSpatialRelEnum.esriSpatialRelIntersects, ref pDomainGeometry, true, ref lFeatCount2);
                lFeatCount = (int)lFeatCount2;
            }
            return true;
        }

    }
}
