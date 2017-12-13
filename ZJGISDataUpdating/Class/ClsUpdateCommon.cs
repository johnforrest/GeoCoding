using System;
using System.Collections.Generic;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.esriSystem;
using System.Windows.Forms;
using ESRI.ArcGIS.CatalogUI;
using ESRI.ArcGIS.Catalog;
using DevComponents.DotNetBar.Controls;
using ESRI.ArcGIS.Geometry;
using ZJGISCommon;
using ESRI.ArcGIS.Carto;
using ZJGISDataUpdating.Class;
using ZJGISCommon.Classes;

namespace ZJGISDataUpdating
{
    class ClsUpdateCommon
    {
        //定义poi前两个组的码段
        public static string pPoiOneTwo = "1330521000000";
        //private static IFeatureWorkspace m_FeatureWorkspace;

        /// <summary>
        /// 打开源图层
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public static IFeatureClass OpenSourceLayer(out string path)
        {
            IGxDialog dlg = new GxDialog();

            IGxObjectFilter pGxFilter = new GxFilterFeatureClassesClass();
            dlg.ObjectFilter = pGxFilter;
            dlg.Title = "添加源图层";
            dlg.ButtonCaption = "添加";
            dlg.AllowMultiSelect = false;

            IEnumGxObject pEnumGxObject;

            dlg.DoModalOpen(0, out pEnumGxObject);
            if (pEnumGxObject != null)
            {
                pEnumGxObject.Reset();
                IGxObject gxObj;

                while ((gxObj = pEnumGxObject.Next()) != null)
                {
                    if (gxObj is IGxDataset)
                    {
                        IGxDataset gxDataset = gxObj as IGxDataset;
                        IDataset pDataset = gxDataset.Dataset;
                        if (pDataset.Type == esriDatasetType.esriDTFeatureClass)
                        {
                            IFeatureClass pIFeatureClass = pDataset as IFeatureClass;
                            path = gxObj.FullName;
                            return pIFeatureClass;
                        }
                    }
                }
            }
            path = string.Empty;
            return null;
        }
        /// <summary>
        /// 打开指定路径的匹配结果表
        /// </summary>
        /// <returns>返回的表内容</returns>
        public static ITable OpenRelateTable(out string path)
        {
            IGxDialog dlg = new GxDialog();

            IGxObjectFilter pGxFilter = new GxFilterTablesClass();
            dlg.ObjectFilter = pGxFilter;
            dlg.Title = "添加关系表";
            dlg.ButtonCaption = "添加";
            dlg.AllowMultiSelect = false;

            IEnumGxObject pEnumGxObject;

            dlg.DoModalOpen(0, out pEnumGxObject);
            if (pEnumGxObject != null)
            {
                pEnumGxObject.Reset();
                IGxObject gxObj;

                while ((gxObj = pEnumGxObject.Next()) != null)
                {
                    if (gxObj is IGxDataset)
                    {
                        IGxDataset gxDataset = gxObj as IGxDataset;
                        IDataset pDataset = gxDataset.Dataset;
                        if (pDataset.Type == esriDatasetType.esriDTTable)
                        {
                            ITable pTable = pDataset as ITable;
                            if (pTable.FindField("源OID") != -1 && pTable.FindField("待更新OID") != -1
                                && pTable.FindField("变化标记") != -1)
                            {
                                path = gxObj.FullName;
                                return pTable;
                            }
                            else
                            {
                                MessageBox.Show("您打开的表格式不正确，请打开匹配关系表!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }
            }
            path = string.Empty;
            return null;
        }

        /// <summary>
        /// 数据编码（自动赋值编码）
        /// </summary>
        /// <param name="source">源要素类</param>
        /// <param name="target">目标要素类</param>
        /// <param name="relation">匹配结果表</param>
        /// <param name="updateRela">欲更新的属性表字段</param>
        /// <param name="pProgressBarX"></param>
        /// <param name="targetFeatureWorkspace">待匹配图层路径</param>
        /// <returns></returns>
        //public static bool UpdateData(IFeatureClass sourceFcls, IFeatureClass targetFcls, ITable resultTable, Dictionary<string, string> updateRela
        //    , ProgressBarX pProgressBarX, IFeatureWorkspace targetFeatureWorkspace)
        public static bool UpdateData(IFeatureClass sourceFcls, IFeatureClass targetFcls, ITable resultTable,
            ProgressBarX pProgressBarX, IFeatureWorkspace targetFeatureWorkspace)
        {
            ClsBarSync progressBar = new ClsBarSync(pProgressBarX);
            progressBar.SetStep(1);
            progressBar.SetMax(resultTable.RowCount(null));

            //检查参数
            if (sourceFcls == null || targetFcls == null || resultTable == null)
            {
                return false;
            }

            #region 原注释
            //当欲更新字段在目标要素类中不存在时，新建字段
            //foreach (string item in fields)
            //{
            //    if (target.FindField(item) == -1)
            //    {
            //        IField addedField = source.Fields.get_Field(source.FindField(item));
            //        target.AddField(addedField);
            //    }
            //}
            #endregion
            //20170914 注释掉

            ////当欲更新字段在目标要素类中不存在时，新建字段
            //foreach (KeyValuePair<string, string> item in updateRela)
            //{
            //    if (targetFcls.FindField(item.Value) == -1)
            //    {
            //        IField addedField = new FieldClass();
            //        IFieldEdit pFieldEdit = addedField as IFieldEdit;
            //        pFieldEdit.Name_2 = item.Value;
            //        pFieldEdit.AliasName_2 = item.Value;
            //        pFieldEdit.Editable_2 = true;
            //        pFieldEdit.Required_2 = false;
            //        pFieldEdit.IsNullable_2 = true;
            //        targetFcls.AddField(addedField);
            //    }
            //}
            IWorkspaceEdit2 pWorkspaceEdit = targetFeatureWorkspace as IWorkspaceEdit2;
            pWorkspaceEdit.StartEditing(false);
            pWorkspaceEdit.StartEditOperation();

            #region 20170515注释掉
            //ClsUpdateCommon.EnableAchive(target.AliasName, targetFeatureWorkspace);
            //IHistoricalWorkspace pHistoricalWorkspace = targetFeatureWorkspace as IHistoricalWorkspace;
            //DateTime dTime=DateTime.Now;
            //pHistoricalWorkspace.AddHistoricalMarker(dTime.ToString(), dTime);//严格应该为数据库时间
            #endregion

            #region 原来注释
            //foreach (KeyValuePair<string, string> item in updateRela)
            //{
            //    if (target.FindField(item.Value) == -1)
            //    {
            //        IField addedField = new FieldClass();
            //        IFieldEdit pFieldEdit = addedField as IFieldEdit;
            //        pFieldEdit.Name_2 = item.Value;
            //        pFieldEdit.AliasName_2 = item.Value;
            //        pFieldEdit.Editable_2 = true;
            //        pFieldEdit.Required_2 = false;
            //        pFieldEdit.IsNullable_2 = true;
            //        target.AddField(addedField);
            //    }
            //}
            #endregion

            //源图层要素的oid——1
            int pSrcOIDIndex = resultTable.FindField("源OID");
            //待匹配图层要素的oid——2
            int pTarOIDIndex = resultTable.FindField("待匹配OID");
            //变化标记的oid——3
            int pChangeTagIndex = resultTable.FindField("变化标记");

            ICursor rowCursor = resultTable.Search(null, false);
            IRow tempRow = null;
            tempRow = rowCursor.NextRow();
            while (tempRow != null)
            {
                //当没有匹配的项时或者未发生变化时不做任何更改
                if (tempRow.get_Value(pTarOIDIndex) == null || tempRow.get_Value(pChangeTagIndex).ToString() == "0")
                {
                    progressBar.PerformOneStep();
                    continue;
                }
                //获取源图层要素
                IFeature sourceFeature = sourceFcls.GetFeature(Convert.ToInt32(tempRow.get_Value(pSrcOIDIndex)));
                //变化标记字段值
                string pChangeContent = tempRow.get_Value(pChangeTagIndex).ToString();
                //获取源图层要素
                string[] fromids = tempRow.get_Value(pTarOIDIndex).ToString().Split(';');


                if (!(fromids.Length == 1 && fromids[0].Trim().Length == 0))
                {
                    //查找匹配与待匹配的要素
                    IFeature targetFeature = null;
                    //点和线匹配名称
                    string pSourceName = sourceFeature.get_Value(sourceFeature.Fields.FindField(ClsConfig.LayerConfigs[sourceFcls.AliasName].NameField)).ToString();
                    string pTargetName = string.Empty;
                    //面匹配分类码
                    //string pSourceFCODE = sourceFeature.get_Value(sourceFeature.Fields.FindField("FCODE")).ToString().Trim();
                    string pTargetFCODE = string.Empty;

                    //不同形状的数据
                    if (sourceFcls.ShapeType != targetFcls.ShapeType)
                    {
                        if (sourceFeature.Shape.GeometryType == esriGeometryType.esriGeometryPoint)
                        {

                            ClsCommon pClsCom = new ClsCommon();
                            //根据不同的匹配类型进行相关操作
                            switch (pChangeContent)
                            {
                                case "图形变化"://图形变化
                                //case "一对一":
                                case ClsConstant.One2One:
                                    //targetFeature.Shape = sourceFeature.Shape;
                                    //plistString.Add(sourceFeature.get_Value(sourceFeature.Fields.FindField("ENTIID")).ToString());
                                    //test
                                    targetFeature = targetFcls.GetFeature(Convert.ToInt32(tempRow.get_Value(pTarOIDIndex)));
                                    string test3 = ClsConfig.LayerConfigs[targetFcls.AliasName].NameField;
                                    pTargetName = targetFeature.get_Value(targetFeature.Fields.FindField(ClsConfig.LayerConfigs[targetFcls.AliasName].NameField)).ToString();
                                    //test
                                    string test1 = targetFeature.get_Value(targetFeature.Fields.FindField(ClsConfig.LayerConfigs[targetFcls.AliasName].EntityID)).ToString();
                                    string test2 = sourceFeature.get_Value(sourceFeature.Fields.FindField(ClsConfig.LayerConfigs[sourceFcls.AliasName].EntityID)).ToString();
                                    if (pClsCom.StringSameOrNot(pSourceName, pTargetName) > 1)
                                    {
                                        targetFeature.set_Value(targetFeature.Fields.FindField(ClsConfig.LayerConfigs[targetFcls.AliasName].EntityID), sourceFeature.get_Value(sourceFeature.Fields.FindField(ClsConfig.LayerConfigs[sourceFcls.AliasName].EntityID)));
                                        targetFeature.Store();
                                    }
                                    //sourceFeature.set_Value(sourceFeature.Fields.FindField("ENTIID"), targetFeature.get_Value(targetFeature.Fields.FindField("ENTIID")));
                                    break;
                                //case "新增要素"://新添加要素
                                case ClsConstant.One2Zero://新添加要素
                                    //targetFeature = target.CreateFeature();
                                    //targetFeature.Shape = sourceFeature.ShapeCopy;
                                    //UpdateAttribute(sourceFeature, targetFeature, updateRela);
                                    break;
                                //case "一对多":
                                case ClsConstant.One2More:
                                    //for (int i = 0; i < fromids.Length; i++)
                                    //{
                                    //    string test = fromids[i];
                                    //    int test3 = Convert.ToInt32(fromids[i]);

                                    //    targetFeature = target.GetFeature(Convert.ToInt32(fromids[i]));
                                    //    pTargetName = targetFeature.get_Value(targetFeature.Fields.FindFieldByAliasName("名称")).ToString();
                                    //    //targetFeature.Shape = sourceFeature.Shape;
                                    //    if (pClsCom.StringSameOrNot(pSourceName, pTargetName) > 1)
                                    //    {
                                    //        if (targetFeature.get_Value(targetFeature.Fields.FindField("ENTIID")).ToString().Trim().Length == 0)
                                    //        {
                                    //            targetFeature.set_Value(targetFeature.Fields.FindField("ENTIID"), sourceFeature.get_Value(sourceFeature.Fields.FindField("ENTIID")));
                                    //            targetFeature.Store();
                                    //        }
                                    //    }
                                    //}
                                    //MessageBox.Show("出现一对多情况，请校对匹配结果表后执行匹配", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    //return false;
                                    break;
                                case "属性变化"://属性变化
                                    //UpdateAttribute(sourceFeature, targetFeature, updateRela);
                                    break;
                                case "属性图形变化"://都变化
                                    //targetFeature.Shape = sourceFeature.Shape;
                                    //UpdateAttribute(sourceFeature, targetFeature, updateRela);
                                    break;
                                //case "多对一"://多尺度更新
                                case ClsConstant.More2One://多尺度更新
                                    //targetFeature.Shape = finalGeometry;
                                    break;
                                default:
                                    break;
                            }
                        }
                        else if (sourceFeature.Shape.GeometryType == esriGeometryType.esriGeometryPolyline)
                        {
                            //根据不同的匹配类型进行相关操作
                            switch (pChangeContent)
                            {
                                case "图形变化"://图形变化
                                //case "一对一":
                                case ClsConstant.One2One:
                                    targetFeature = targetFcls.GetFeature(Convert.ToInt32(tempRow.get_Value(pTarOIDIndex)));
                                    pTargetName = targetFeature.get_Value(targetFeature.Fields.FindField(ClsConfig.LayerConfigs[targetFcls.AliasName].NameField)).ToString();
                                    //targetFeature.Shape = sourceFeature.Shape;
                                    if (pSourceName == pTargetName)
                                    {
                                        targetFeature.set_Value(targetFeature.Fields.FindField(ClsConfig.LayerConfigs[targetFcls.AliasName].EntityID), sourceFeature.get_Value(sourceFeature.Fields.FindField(ClsConfig.LayerConfigs[sourceFcls.AliasName].EntityID)));
                                        targetFeature.Store();
                                    }
                                    //sourceFeature.set_Value(sourceFeature.Fields.FindField("ENTIID"), targetFeature.get_Value(targetFeature.Fields.FindField("ENTIID")));
                                    break;
                                //case "新增要素"://新添加要素
                                case ClsConstant.One2Zero://新添加要素
                                    //targetFeature = target.CreateFeature();
                                    //targetFeature.Shape = sourceFeature.ShapeCopy;
                                    //UpdateAttribute(sourceFeature, targetFeature, updateRela);
                                    break;
                                //case "一对多":
                                case ClsConstant.One2More:
                                    //MessageBox.Show("出现一对多情况，请校对匹配结果表后执行匹配", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    //return false;
                                    for (int i = 0; i < fromids.Length; i++)
                                    {
                                        string test = fromids[i];
                                        int test2 = Convert.ToInt32(fromids[i]);

                                        targetFeature = targetFcls.GetFeature(Convert.ToInt32(fromids[i]));
                                        pTargetName = targetFeature.get_Value(targetFeature.Fields.FindField(ClsConfig.LayerConfigs[targetFcls.AliasName].NameField)).ToString();
                                        //targetFeature.Shape = sourceFeature.Shape;
                                        if (pSourceName == pTargetName)
                                        {
                                            if (targetFeature.get_Value(targetFeature.Fields.FindField(ClsConfig.LayerConfigs[targetFcls.AliasName].EntityID)).ToString().Trim().Length == 0)
                                            {
                                                targetFeature.set_Value(targetFeature.Fields.FindField(ClsConfig.LayerConfigs[targetFcls.AliasName].EntityID), sourceFeature.get_Value(sourceFeature.Fields.FindField(ClsConfig.LayerConfigs[sourceFcls.AliasName].EntityID)));
                                                targetFeature.Store();
                                            }
                                        }
                                    }

                                    break;
                                case "属性变化"://属性变化
                                    //UpdateAttribute(sourceFeature, targetFeature, updateRela);
                                    break;
                                case "属性图形变化"://都变化
                                    //targetFeature.Shape = sourceFeature.Shape;
                                    //UpdateAttribute(sourceFeature, targetFeature, updateRela);
                                    break;
                                //case "多对一"://多尺度更新
                                case ClsConstant.More2One://多尺度更新
                                    //targetFeature.Shape = finalGeometry;
                                    break;
                                default:
                                    break;
                            }
                        }
                        else if (sourceFeature.Shape.GeometryType == esriGeometryType.esriGeometryPolygon)
                        {
                            //根据不同的匹配类型进行相关操作
                            switch (pChangeContent)
                            {
                                case "图形变化"://图形变化
                                //case "一对一":
                                case ClsConstant.One2One:
                                    targetFeature = targetFcls.GetFeature(Convert.ToInt32(tempRow.get_Value(pTarOIDIndex)));
                                    pTargetFCODE = targetFeature.get_Value(targetFeature.Fields.FindField("FCODE")).ToString().Trim();
                                    //targetFeature.Shape = sourceFeature.Shape;
                                    //if (pSourceFCODE == pTargetFCODE)
                                    //{
                                    targetFeature.set_Value(targetFeature.Fields.FindField(ClsConfig.LayerConfigs[targetFcls.AliasName].EntityID), sourceFeature.get_Value(sourceFeature.Fields.FindField(ClsConfig.LayerConfigs[sourceFcls.AliasName].EntityID)));
                                    targetFeature.Store();
                                    //}
                                    //sourceFeature.set_Value(sourceFeature.Fields.FindField("ENTIID"), targetFeature.get_Value(targetFeature.Fields.FindField("ENTIID")));
                                    break;
                                //case "新增要素"://新添加要素
                                case ClsConstant.One2Zero://新添加要素
                                    //targetFeature = target.CreateFeature();
                                    //targetFeature.Shape = sourceFeature.ShapeCopy;
                                    //UpdateAttribute(sourceFeature, targetFeature, updateRela);
                                    break;
                                //case "一对多":
                                case ClsConstant.One2More:
                                    //MessageBox.Show("出现一对多情况，请校对匹配结果表后执行匹配", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    //return false;

                                    //for (int i = 0; i < fromids.Length; i++)
                                    //{
                                    //    string test = fromids[i];
                                    //    int test2 = Convert.ToInt32(fromids[i]);
                                    //    targetFeature = targetFcls.GetFeature(Convert.ToInt32(fromids[i]));
                                    //    pTargetName = targetFeature.get_Value(targetFeature.Fields.FindField(ClsConfig.LayerConfigs[targetFcls.AliasName].NameField)).ToString();
                                    //    //targetFeature.Shape = sourceFeature.Shape;
                                    //    if (pSourceName == pTargetName)
                                    //    {
                                    //        if (targetFeature.get_Value(targetFeature.Fields.FindField(ClsConfig.LayerConfigs[targetFcls.AliasName].EntityID)).ToString().Trim().Length == 0)
                                    //        {
                                    //            targetFeature.set_Value(targetFeature.Fields.FindField(ClsConfig.LayerConfigs[targetFcls.AliasName].EntityID), sourceFeature.get_Value(sourceFeature.Fields.FindField(ClsConfig.LayerConfigs[sourceFcls.AliasName].EntityID)));
                                    //            targetFeature.Store();
                                    //        }
                                    //    }
                                    //}

                                    break;
                                case "属性变化"://属性变化
                                    //UpdateAttribute(sourceFeature, targetFeature, updateRela);
                                    break;
                                case "属性图形变化"://都变化
                                    //targetFeature.Shape = sourceFeature.Shape;
                                    //UpdateAttribute(sourceFeature, targetFeature, updateRela);
                                    break;
                                //case "多对一"://多尺度更新
                                case ClsConstant.More2More://多尺度更新
                                    //targetFeature.Shape = finalGeometry;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    //相同形状的数据
                    else
                    {
                        if (sourceFeature.Shape.GeometryType == esriGeometryType.esriGeometryPoint)
                        {

                            ClsCommon pClsCom = new ClsCommon();
                            //根据不同的匹配类型进行相关操作
                            switch (pChangeContent)
                            {
                                case "图形变化"://图形变化
                                //case "一对一":
                                case ClsConstant.One2One:
                                    //targetFeature.Shape = sourceFeature.Shape;
                                    //plistString.Add(sourceFeature.get_Value(sourceFeature.Fields.FindField("ENTIID")).ToString());
                                    //test
                                    targetFeature = targetFcls.GetFeature(Convert.ToInt32(tempRow.get_Value(pTarOIDIndex)));
                                    string test3 = ClsConfig.LayerConfigs[targetFcls.AliasName].NameField;
                                    pTargetName = targetFeature.get_Value(targetFeature.Fields.FindField(ClsConfig.LayerConfigs[targetFcls.AliasName].NameField)).ToString();
                                    //test
                                    string test1 = targetFeature.get_Value(targetFeature.Fields.FindField(ClsConfig.LayerConfigs[targetFcls.AliasName].EntityID)).ToString();
                                    string test2 = sourceFeature.get_Value(sourceFeature.Fields.FindField(ClsConfig.LayerConfigs[sourceFcls.AliasName].EntityID)).ToString();
                                    //if (pClsCom.StringSameOrNot(pSourceName, pTargetName) > 1)
                                    //{
                                    targetFeature.set_Value(targetFeature.Fields.FindField(ClsConfig.LayerConfigs[targetFcls.AliasName].EntityID), sourceFeature.get_Value(sourceFeature.Fields.FindField(ClsConfig.LayerConfigs[sourceFcls.AliasName].EntityID)));
                                    targetFeature.Store();
                                    //}
                                    //sourceFeature.set_Value(sourceFeature.Fields.FindField("ENTIID"), targetFeature.get_Value(targetFeature.Fields.FindField("ENTIID")));
                                    break;
                                //case "新增要素"://新添加要素
                                case ClsConstant.One2Zero://新添加要素
                                    //targetFeature = target.CreateFeature();
                                    //targetFeature.Shape = sourceFeature.ShapeCopy;
                                    //UpdateAttribute(sourceFeature, targetFeature, updateRela);
                                    break;
                                //case "一对多":
                                case ClsConstant.One2More:
                                    //for (int i = 0; i < fromids.Length; i++)
                                    //{
                                    //    string test = fromids[i];
                                    //    int test3 = Convert.ToInt32(fromids[i]);

                                    //    targetFeature = target.GetFeature(Convert.ToInt32(fromids[i]));
                                    //    pTargetName = targetFeature.get_Value(targetFeature.Fields.FindFieldByAliasName("名称")).ToString();
                                    //    //targetFeature.Shape = sourceFeature.Shape;
                                    //    if (pClsCom.StringSameOrNot(pSourceName, pTargetName) > 1)
                                    //    {
                                    //        if (targetFeature.get_Value(targetFeature.Fields.FindField("ENTIID")).ToString().Trim().Length == 0)
                                    //        {
                                    //            targetFeature.set_Value(targetFeature.Fields.FindField("ENTIID"), sourceFeature.get_Value(sourceFeature.Fields.FindField("ENTIID")));
                                    //            targetFeature.Store();
                                    //        }
                                    //    }
                                    //}
                                    //MessageBox.Show("出现一对多情况，请校对匹配结果表后执行匹配", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    //return false;
                                    break;
                                case "属性变化"://属性变化
                                    //UpdateAttribute(sourceFeature, targetFeature, updateRela);
                                    break;
                                case "属性图形变化"://都变化
                                    //targetFeature.Shape = sourceFeature.Shape;
                                    //UpdateAttribute(sourceFeature, targetFeature, updateRela);
                                    break;
                                //case "多对一"://多尺度更新
                                case ClsConstant.More2One://多尺度更新
                                    //targetFeature.Shape = finalGeometry;
                                    break;
                                default:
                                    break;
                            }
                        }
                        else if (sourceFeature.Shape.GeometryType == esriGeometryType.esriGeometryPolyline)
                        {
                            //根据不同的匹配类型进行相关操作
                            switch (pChangeContent)
                            {
                                case "图形变化"://图形变化
                                //case "一对一":
                                case ClsConstant.One2One:
                                    targetFeature = targetFcls.GetFeature(Convert.ToInt32(tempRow.get_Value(pTarOIDIndex)));
                                    pTargetName = targetFeature.get_Value(targetFeature.Fields.FindField(ClsConfig.LayerConfigs[targetFcls.AliasName].NameField)).ToString();
                                    //targetFeature.Shape = sourceFeature.Shape;
                                    //if (pSourceName == pTargetName)
                                    //{
                                    targetFeature.set_Value(targetFeature.Fields.FindField(ClsConfig.LayerConfigs[targetFcls.AliasName].EntityID), sourceFeature.get_Value(sourceFeature.Fields.FindField(ClsConfig.LayerConfigs[sourceFcls.AliasName].EntityID)));
                                    targetFeature.Store();
                                    //}
                                    //sourceFeature.set_Value(sourceFeature.Fields.FindField("ENTIID"), targetFeature.get_Value(targetFeature.Fields.FindField("ENTIID")));
                                    break;
                                //case "新增要素"://新添加要素
                                case ClsConstant.One2Zero://新添加要素
                                    //targetFeature = target.CreateFeature();
                                    //targetFeature.Shape = sourceFeature.ShapeCopy;
                                    //UpdateAttribute(sourceFeature, targetFeature, updateRela);
                                    break;
                                //case "一对多":
                                case ClsConstant.One2More:
                                    //MessageBox.Show("出现一对多情况，请校对匹配结果表后执行匹配", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    //return false;
                                    for (int i = 0; i < fromids.Length; i++)
                                    {
                                        string test = fromids[i];
                                        int test2 = Convert.ToInt32(fromids[i]);

                                        targetFeature = targetFcls.GetFeature(Convert.ToInt32(fromids[i]));
                                        pTargetName = targetFeature.get_Value(targetFeature.Fields.FindField(ClsConfig.LayerConfigs[targetFcls.AliasName].NameField)).ToString();
                                        //targetFeature.Shape = sourceFeature.Shape;
                                        if (pSourceName == pTargetName)
                                        {
                                            if (targetFeature.get_Value(targetFeature.Fields.FindField(ClsConfig.LayerConfigs[targetFcls.AliasName].EntityID)).ToString().Trim().Length == 0)
                                            {
                                                targetFeature.set_Value(targetFeature.Fields.FindField(ClsConfig.LayerConfigs[targetFcls.AliasName].EntityID), sourceFeature.get_Value(sourceFeature.Fields.FindField(ClsConfig.LayerConfigs[sourceFcls.AliasName].EntityID)));
                                                targetFeature.Store();
                                            }
                                        }
                                    }

                                    break;
                                case "属性变化"://属性变化
                                    //UpdateAttribute(sourceFeature, targetFeature, updateRela);
                                    break;
                                case "属性图形变化"://都变化
                                    //targetFeature.Shape = sourceFeature.Shape;
                                    //UpdateAttribute(sourceFeature, targetFeature, updateRela);
                                    break;
                                //case "多对一"://多尺度更新
                                case ClsConstant.More2One://多尺度更新
                                    //targetFeature.Shape = finalGeometry;
                                    break;
                                default:
                                    break;
                            }
                        }
                        else if (sourceFeature.Shape.GeometryType == esriGeometryType.esriGeometryPolygon)
                        {
                            //根据不同的匹配类型进行相关操作
                            switch (pChangeContent)
                            {
                                case "图形变化"://图形变化
                                //case "一对一":
                                case ClsConstant.One2One:
                                    targetFeature = targetFcls.GetFeature(Convert.ToInt32(tempRow.get_Value(pTarOIDIndex)));
                                    pTargetFCODE = targetFeature.get_Value(targetFeature.Fields.FindField("FCODE")).ToString().Trim();
                                    //targetFeature.Shape = sourceFeature.Shape;
                                    //if (pSourceFCODE == pTargetFCODE)
                                    //{
                                    targetFeature.set_Value(targetFeature.Fields.FindField(ClsConfig.LayerConfigs[targetFcls.AliasName].EntityID), sourceFeature.get_Value(sourceFeature.Fields.FindField(ClsConfig.LayerConfigs[sourceFcls.AliasName].EntityID)));
                                    targetFeature.Store();
                                    //}
                                    //sourceFeature.set_Value(sourceFeature.Fields.FindField("ENTIID"), targetFeature.get_Value(targetFeature.Fields.FindField("ENTIID")));
                                    break;
                                //case "新增要素"://新添加要素
                                case ClsConstant.One2Zero://新添加要素
                                    //targetFeature = target.CreateFeature();
                                    //targetFeature.Shape = sourceFeature.ShapeCopy;
                                    //UpdateAttribute(sourceFeature, targetFeature, updateRela);
                                    break;
                                //case "一对多":
                                case ClsConstant.One2More:
                                    //MessageBox.Show("出现一对多情况，请校对匹配结果表后执行匹配", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    //return false;

                                    //for (int i = 0; i < fromids.Length; i++)
                                    //{
                                    //    string test = fromids[i];
                                    //    int test2 = Convert.ToInt32(fromids[i]);
                                    //    targetFeature = targetFcls.GetFeature(Convert.ToInt32(fromids[i]));
                                    //    pTargetName = targetFeature.get_Value(targetFeature.Fields.FindField(ClsConfig.LayerConfigs[targetFcls.AliasName].NameField)).ToString();
                                    //    //targetFeature.Shape = sourceFeature.Shape;
                                    //    if (pSourceName == pTargetName)
                                    //    {
                                    //        if (targetFeature.get_Value(targetFeature.Fields.FindField(ClsConfig.LayerConfigs[targetFcls.AliasName].EntityID)).ToString().Trim().Length == 0)
                                    //        {
                                    //            targetFeature.set_Value(targetFeature.Fields.FindField(ClsConfig.LayerConfigs[targetFcls.AliasName].EntityID), sourceFeature.get_Value(sourceFeature.Fields.FindField(ClsConfig.LayerConfigs[sourceFcls.AliasName].EntityID)));
                                    //            targetFeature.Store();
                                    //        }
                                    //    }
                                    //}

                                    break;
                                case "属性变化"://属性变化
                                    //UpdateAttribute(sourceFeature, targetFeature, updateRela);
                                    break;
                                case "属性图形变化"://都变化
                                    //targetFeature.Shape = sourceFeature.Shape;
                                    //UpdateAttribute(sourceFeature, targetFeature, updateRela);
                                    break;
                                //case "多对一"://多尺度更新
                                case ClsConstant.More2More://多尺度更新
                                    //targetFeature.Shape = finalGeometry;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    //targetFeature.Store();

                }

                progressBar.PerformOneStep();
                tempRow = rowCursor.NextRow();
            }

            //对待匹配图层中剩余的点（）进行编码
            //CreatePOICodeRest(target, "ENTIID", plistString);

            pWorkspaceEdit.StopEditOperation();
            pWorkspaceEdit.StopEditing(true);
            return true;
        }



        /// <summary>
        /// 对村落、POI实体进行编码
        /// </summary>
        /// <param name="pFeatureLayer">需要编码的图层</param>
        /// <param name="pENTIID">需要编码的字段名称</param>
        //private static void CreatePOICodeRest(IFeatureLayer pFeatureLayer, string pENTIID, List<string> plistStr)
        private static void CreatePOICodeRest(IFeatureClass pFeatureClass, string pENTIID, List<string> plistStr)
        {
            #region 针对GUID编码来讲
            int i = 0;
            //遍历Feature
            //IDataset pDataset = pFeatureLayer.FeatureClass as IDataset;
            IDataset pDataset = pFeatureClass as IDataset;
            IWorkspaceEdit pWorkspaceEdit = null;
            if (pDataset != null)
            {
                pWorkspaceEdit = pDataset.Workspace as IWorkspaceEdit;
                if (pWorkspaceEdit != null || pWorkspaceEdit.IsBeingEdited() == false)
                {
                    pWorkspaceEdit.StartEditing(true);
                    pWorkspaceEdit.StartEditOperation();
                }
                //IFeatureCursor pFeatureCursor = pFeatureLayer.Search(null, false);
                IFeatureCursor pFeatureCursor = pFeatureClass.Search(null, false);

                //test
                //int test = pFeatureLayer.FeatureClass.FeatureCount(null);
                int test = pFeatureClass.FeatureCount(null);
                IFeature pFeature = pFeatureCursor.NextFeature();
                while (pFeature != null)
                {
                    string pResult = null;
                    bool pflag = false;

                    //获取单条Feature的某个字段值
                    //int test2 = pFeature.Fields.FindFieldByAliasName(pENTIID);
                    while (i < 10000000)
                    {
                        i++;
                        break;
                    }
                    for (int k = 0; k < 7 - i.ToString().Length; k++)
                    {
                        pResult += "0";
                    }
                    //string pEntiid = pPoiOneTwo + ReturnNumberCharacter(i);
                    string pEntiid = pPoiOneTwo + pResult + i.ToString();

                    foreach (string s in plistStr)
                    {
                        if (s == pEntiid)
                        {
                            pflag = true;
                        }
                    }

                    if (pflag)
                    {
                        continue;
                    }
                    else
                    {
                        pFeature.set_Value(pFeature.Fields.FindField(pENTIID), pEntiid);
                        pFeature.Store();
                    }

                    pFeature = pFeatureCursor.NextFeature();

                }
                pWorkspaceEdit.StopEditing(true);
                pWorkspaceEdit.StopEditOperation();

            }

            #endregion
        }

        private static void UpdateAttribute(IFeature sourceFeature, IFeature targetFeature, Dictionary<string, string> updateRela)
        {
            foreach (KeyValuePair<string, string> item in updateRela)
            {
                int fieldIndex = targetFeature.Fields.FindField(item.Value);
                if (fieldIndex != -1)
                {
                    targetFeature.set_Value(fieldIndex, sourceFeature.get_Value(sourceFeature.Fields.FindField(item.Key)));
                }
            }
        }
        /// <summary>
        /// 获取字段列表
        /// </summary>
        /// <param name="queryFeatureClass">FeatureClass</param>
        /// <returns></returns>
        public static List<string> GetAttribute(IFeatureClass queryFeatureClass)
        {
            List<string> listFieldsName = new List<string>();
            IFields pFields = queryFeatureClass.Fields;
            for (int i = 0; i < pFields.FieldCount; i++)
            {
                string fieldName = pFields.get_Field(i).Name;
                bool editable = pFields.get_Field(i).Editable;
                if (!fieldName.ToUpper().Contains("OBJECTID") && editable)
                {
                    listFieldsName.Add(fieldName);
                }
            }
            return listFieldsName;
        }

        public static bool EnableAchive(string name, IFeatureWorkspace featureWorkspace)
        {
            IVersionedObject3 pVersionedObject = null;
            IArchivableObject pArchivableObject = null;
            bool bRegistered = false;
            bool bMovingEditsToBase = false;
            IFeatureWorkspace pFeatureWorkspace = featureWorkspace;

            try
            {
                pVersionedObject = pFeatureWorkspace.OpenFeatureClass(name) as IVersionedObject3;
                pVersionedObject.GetVersionRegistrationInfo(out bRegistered, out  bMovingEditsToBase);

                //如果数据没有注册为版本，则进行注册
                if (!bRegistered)
                {
                    pVersionedObject.RegisterAsVersioned3(false);
                }
                else
                {
                    if (bMovingEditsToBase)
                    {
                        pVersionedObject.UnRegisterAsVersioned3(false);
                        pVersionedObject.RegisterAsVersioned3(false);
                    }
                }
                //获取归档对象、判断数据是否归档
                pArchivableObject = pVersionedObject as IArchivableObject;
                if (!pArchivableObject.IsArchiving)
                {
                    pArchivableObject.EnableArchiving(null, null, false);
                }
                return true;
            }
            catch (Exception)
            {

            }
            return false;
        }
    }
}
