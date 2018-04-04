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
        //private static IFeatureWorkspace m_FeatureWorkspace;

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
                    string pSourceName = sourceFeature.get_Value(sourceFeature.Fields.FindField(ClsConfig.LayerConfigs[(sourceFcls  as IDataset).Name].NameField)).ToString();
                    string pTargetName = string.Empty;
                    //面匹配分类码
                    //string pSourceFCODE = sourceFeature.get_Value(sourceFeature.Fields.FindField("FCODE")).ToString().Trim();
                    string pTargetFCODE = string.Empty;

                    //不同形状的数据
                    if (sourceFcls.ShapeType != targetFcls.ShapeType)
                    {
                        if (sourceFeature.Shape.GeometryType == esriGeometryType.esriGeometryPoint)
                        {

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
                                    string test3 = ClsConfig.LayerConfigs[(targetFcls  as IDataset).Name].NameField;
                                    pTargetName = targetFeature.get_Value(targetFeature.Fields.FindField(ClsConfig.LayerConfigs[(targetFcls  as IDataset).Name].NameField)).ToString();
                                    //test
                                    string test1 = targetFeature.get_Value(targetFeature.Fields.FindField(ClsConfig.LayerConfigs[(targetFcls  as IDataset).Name].EntityID)).ToString();
                                    string test2 = sourceFeature.get_Value(sourceFeature.Fields.FindField(ClsConfig.LayerConfigs[(sourceFcls  as IDataset).Name].EntityID)).ToString();
                                    if (ClsStatic.StringSameOrNot(pSourceName, pTargetName) > 1)
                                    {
                                        targetFeature.set_Value(targetFeature.Fields.FindField(ClsConfig.LayerConfigs[(targetFcls  as IDataset).Name].EntityID), sourceFeature.get_Value(sourceFeature.Fields.FindField(ClsConfig.LayerConfigs[(sourceFcls  as IDataset).Name].EntityID)));
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
                                    //    if (ClsStatic.StringSameOrNot(pSourceName, pTargetName) > 1)
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
                                    pTargetName = targetFeature.get_Value(targetFeature.Fields.FindField(ClsConfig.LayerConfigs[(targetFcls  as IDataset).Name].NameField)).ToString();
                                    //targetFeature.Shape = sourceFeature.Shape;
                                    if (pSourceName == pTargetName)
                                    {
                                        targetFeature.set_Value(targetFeature.Fields.FindField(ClsConfig.LayerConfigs[(targetFcls  as IDataset).Name].EntityID), sourceFeature.get_Value(sourceFeature.Fields.FindField(ClsConfig.LayerConfigs[(sourceFcls  as IDataset).Name].EntityID)));
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
                                        pTargetName = targetFeature.get_Value(targetFeature.Fields.FindField(ClsConfig.LayerConfigs[(targetFcls  as IDataset).Name].NameField)).ToString();
                                        //targetFeature.Shape = sourceFeature.Shape;
                                        if (pSourceName == pTargetName)
                                        {
                                            if (targetFeature.get_Value(targetFeature.Fields.FindField(ClsConfig.LayerConfigs[(targetFcls  as IDataset).Name].EntityID)).ToString().Trim().Length == 0)
                                            {
                                                targetFeature.set_Value(targetFeature.Fields.FindField(ClsConfig.LayerConfigs[(targetFcls  as IDataset).Name].EntityID), sourceFeature.get_Value(sourceFeature.Fields.FindField(ClsConfig.LayerConfigs[(sourceFcls  as IDataset).Name].EntityID)));
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
                                    targetFeature.set_Value(targetFeature.Fields.FindField(ClsConfig.LayerConfigs[(targetFcls  as IDataset).Name].EntityID), sourceFeature.get_Value(sourceFeature.Fields.FindField(ClsConfig.LayerConfigs[(sourceFcls  as IDataset).Name].EntityID)));
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
                                    //    pTargetName = targetFeature.get_Value(targetFeature.Fields.FindField(ClsConfig.LayerConfigs[(targetFcls  as IDataset).Name].NameField)).ToString();
                                    //    //targetFeature.Shape = sourceFeature.Shape;
                                    //    if (pSourceName == pTargetName)
                                    //    {
                                    //        if (targetFeature.get_Value(targetFeature.Fields.FindField(ClsConfig.LayerConfigs[(targetFcls  as IDataset).Name].EntityID)).ToString().Trim().Length == 0)
                                    //        {
                                    //            targetFeature.set_Value(targetFeature.Fields.FindField(ClsConfig.LayerConfigs[(targetFcls  as IDataset).Name].EntityID), sourceFeature.get_Value(sourceFeature.Fields.FindField(ClsConfig.LayerConfigs[(sourceFcls  as IDataset).Name].EntityID)));
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
                                    //string test3 = ClsConfig.LayerConfigs[(targetFcls  as IDataset).Name].NameField;
                                    string test3 = ClsConfig.LayerConfigs[(targetFcls  as IDataset).Name].NameField;
                                    pTargetName = targetFeature.get_Value(targetFeature.Fields.FindField(ClsConfig.LayerConfigs[(targetFcls  as IDataset).Name].NameField)).ToString();
                                    //test
                                    string test1 = targetFeature.get_Value(targetFeature.Fields.FindField(ClsConfig.LayerConfigs[(targetFcls  as IDataset).Name].EntityID)).ToString();
                                    string test2 = sourceFeature.get_Value(sourceFeature.Fields.FindField(ClsConfig.LayerConfigs[(sourceFcls  as IDataset).Name].EntityID)).ToString();
                                    //if (ClsStatic.StringSameOrNot(pSourceName, pTargetName) > 1)
                                    //{
                                    targetFeature.set_Value(targetFeature.Fields.FindField(ClsConfig.LayerConfigs[(targetFcls  as IDataset).Name].EntityID), sourceFeature.get_Value(sourceFeature.Fields.FindField(ClsConfig.LayerConfigs[(sourceFcls  as IDataset).Name].EntityID)));
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
                                    //    if (ClsStatic.StringSameOrNot(pSourceName, pTargetName) > 1)
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
                                    pTargetName = targetFeature.get_Value(targetFeature.Fields.FindField(ClsConfig.LayerConfigs[(targetFcls  as IDataset).Name].NameField)).ToString();
                                    //targetFeature.Shape = sourceFeature.Shape;
                                    //if (pSourceName == pTargetName)
                                    //{
                                    targetFeature.set_Value(targetFeature.Fields.FindField(ClsConfig.LayerConfigs[(targetFcls  as IDataset).Name].EntityID), sourceFeature.get_Value(sourceFeature.Fields.FindField(ClsConfig.LayerConfigs[(sourceFcls  as IDataset).Name].EntityID)));
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
                                        pTargetName = targetFeature.get_Value(targetFeature.Fields.FindField(ClsConfig.LayerConfigs[(targetFcls  as IDataset).Name].NameField)).ToString();
                                        //targetFeature.Shape = sourceFeature.Shape;
                                        if (pSourceName == pTargetName)
                                        {
                                            if (targetFeature.get_Value(targetFeature.Fields.FindField(ClsConfig.LayerConfigs[(targetFcls  as IDataset).Name].EntityID)).ToString().Trim().Length == 0)
                                            {
                                                targetFeature.set_Value(targetFeature.Fields.FindField(ClsConfig.LayerConfigs[(targetFcls  as IDataset).Name].EntityID), sourceFeature.get_Value(sourceFeature.Fields.FindField(ClsConfig.LayerConfigs[(sourceFcls  as IDataset).Name].EntityID)));
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
                                    targetFeature.set_Value(targetFeature.Fields.FindField(ClsConfig.LayerConfigs[(targetFcls  as IDataset).Name].EntityID), sourceFeature.get_Value(sourceFeature.Fields.FindField(ClsConfig.LayerConfigs[(sourceFcls  as IDataset).Name].EntityID)));
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
                                    //    pTargetName = targetFeature.get_Value(targetFeature.Fields.FindField(ClsConfig.LayerConfigs[(targetFcls  as IDataset).Name].NameField)).ToString();
                                    //    //targetFeature.Shape = sourceFeature.Shape;
                                    //    if (pSourceName == pTargetName)
                                    //    {
                                    //        if (targetFeature.get_Value(targetFeature.Fields.FindField(ClsConfig.LayerConfigs[(targetFcls  as IDataset).Name].EntityID)).ToString().Trim().Length == 0)
                                    //        {
                                    //            targetFeature.set_Value(targetFeature.Fields.FindField(ClsConfig.LayerConfigs[(targetFcls  as IDataset).Name].EntityID), sourceFeature.get_Value(sourceFeature.Fields.FindField(ClsConfig.LayerConfigs[(sourceFcls  as IDataset).Name].EntityID)));
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



    }
}
