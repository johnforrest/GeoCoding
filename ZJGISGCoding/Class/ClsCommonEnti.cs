using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar.Controls;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using DevComponents.DotNetBar;
using ESRI.ArcGIS.Geometry;
using ZJGISCommon.Classes;

namespace ZJGISGCoding.Class
{
    public class ClsCommonEnti
    {
        ClsCommon pClsCom = new ClsCommon();
        /// <summary>
        /// 生成格网码
        /// </summary>
        /// <param name="pMapControl"></param>
        /// <param name="cbxLayerName"></param>
        public void CreatGridCode(IMap pMapControl, ComboBoxEx cbxLayerName)
        {
            string strField = "GridCode";
            IFeatureLayer pFeatureLayer = (IFeatureLayer)pClsCom.GetLayerByName(pMapControl, cbxLayerName.Text);

            IDataset cDataset = pFeatureLayer.FeatureClass as IDataset;
            IGeoDataset cGeoDataset = cDataset as IGeoDataset;
            ISpatialReference cSpatialReference = cGeoDataset.SpatialReference;
            if (cSpatialReference is IProjectedCoordinateSystem)
            {
                MessageBox.Show("该图层为投影坐标，请转换为相应的地理坐标,再开始地理编码！");
            }

            if (pFeatureLayer != null)
            {
                //检查格网字段是否存在，不存在就添加格网字段GridCode
                pClsCom.CheckGridCode(pFeatureLayer, strField);

                if (pFeatureLayer.FeatureClass.Fields.FindField(ClsConfig.LayerConfigs[(pFeatureLayer.FeatureClass as IDataset).Name].NameField) != -1)
                {
                    IDataset pDataset = pFeatureLayer.FeatureClass as IDataset;
                    IWorkspaceEdit pWorkspaceEdit = null;
                    if (pDataset != null)
                    {
                        pWorkspaceEdit = pDataset.Workspace as IWorkspaceEdit;
                        if (pWorkspaceEdit != null || pWorkspaceEdit.IsBeingEdited() == false)
                        {
                            pWorkspaceEdit.StartEditing(true);
                            pWorkspaceEdit.StartEditOperation();
                        }
                        IFeatureCursor pFeatureCursor = pFeatureLayer.Search(null, false);

                        int i = pFeatureLayer.FeatureClass.FeatureCount(null);
                        int j = 0;

                        IFeature pFeature = pFeatureCursor.NextFeature();

                        while (pFeature != null)
                        {
                            //对于名称为空的字段进行筛选
                            //string pFeaEnti = pFeature.get_Value(pFeature.Fields.FindField(ClsConfig.LayerConfigs[(pFeatureLayer.FeatureClass  as IDataset).Name].NameField)).ToString();
                            LayerConfig lyconfig = ClsConfig.LayerConfigs[(pFeatureLayer.FeatureClass as IDataset).Name.ToString().Trim()];
                            string test = ClsConfig.LayerConfigs[(pFeatureLayer.FeatureClass as IDataset).Name].NameField;
                            IFeatureClass pFeatureClass = pFeatureLayer.FeatureClass;
                            string pFeatureName = pFeature.get_Value(pFeature.Fields.FindField(ClsConfig.LayerConfigs[(pFeatureLayer.FeatureClass as IDataset).Name].NameField)).ToString();

                            if (pFeatureName.Trim().Length > 0)
                            {
                                //获取格网信息
                                string GridCode = pClsCom.GetCodeString(pFeature);
                                if (GridCode != "")
                                {
                                    pFeature.set_Value(pFeature.Fields.FindField(strField), GridCode);
                                    pFeature.Store();

                                    j++;
                                }
                                else
                                {
                                    MessageBoxEx.Show("格网码生成失败，请转换成地理坐标！");
                                    return;
                                }
                            }
                            pFeature = pFeatureCursor.NextFeature();
                        }
                        pWorkspaceEdit.StopEditing(true);
                        pWorkspaceEdit.StopEditOperation();
                        MessageBoxEx.Show("格网生成成功！");
                    }
                }
                else
                {
                    MessageBox.Show("此图层不存在名称字段！");
                }
            }
            else
            {
                MessageBoxEx.Show("没有选中任何图层！");
            }
        }


        /// <summary>
        /// 生成编码
        /// </summary>
        /// <param name="pMapControl"></param>
        /// <param name="cbxLayerName"></param>
        public void Code(IMap pMapControl, ComboBoxEx cbxLayerName)
        {
            //要编码的字段——ENTIID
            string strField = "ENTIID";
            Dictionary<IFeature, string> pGridFCode = new Dictionary<IFeature, string>();
            IFeatureLayer pFeatureLayer = (IFeatureLayer)pClsCom.GetLayerByName(pMapControl, cbxLayerName.Text);
            if (pFeatureLayer != null)
            {
                //检查地理实体编码字段是否存在，不存在就添加地理实体编码字段
                pClsCom.CheckGridCode(pFeatureLayer, strField);

                IDataset pDataset = pFeatureLayer.FeatureClass as IDataset;
                IWorkspaceEdit pWorkspaceEdit = null;
                if (pDataset != null)
                {
                    pWorkspaceEdit = pDataset.Workspace as IWorkspaceEdit;
                    if (pWorkspaceEdit != null || pWorkspaceEdit.IsBeingEdited() == false)
                    {
                        pWorkspaceEdit.StartEditing(true);
                        pWorkspaceEdit.StartEditOperation();
                    }
                    IFeatureCursor pFeatureCursor = pFeatureLayer.Search(null, false);

                    int i = pFeatureLayer.FeatureClass.FeatureCount(null);
                    int j = 0;

                    string pNum = string.Empty;
                    string pCode = string.Empty;
                    string pEntiidCode = string.Empty;
                    IFeature pFeature = pFeatureCursor.NextFeature();

                    while (pFeature != null)
                    {
                        string pGridCode = pFeature.get_Value(pFeature.Fields.FindField("GridCode")).ToString().Trim();
                        if (pGridCode.Length > 0)
                        {
                            //分类码
                            int index = pFeature.Fields.FindField("FCode");
                            if (index != -1)
                            {
                                string FCode = pFeature.get_Value(pFeature.Fields.FindField("FCode")).ToString();
                                if (FCode.Length > 0)
                                {
                                    //处理分类码
                                    if (ZJGISCommon.Classes.ClsFcode.pDicFcodeGlobal.ContainsKey(FCode))
                                    {
                                        FCode = ClsFcode.pDicFcodeGlobal[FCode];
                                        FCode = FCode.ToUpper();
                                    }

                                    if (FCode.Length > 0)
                                    {
                                        //格网代码和分类代码的组合
                                        pCode = pGridCode + FCode;
                                        pGridFCode.Add(pFeature, pCode);
                                    }
                                }
                            }
                        }
                        pFeature = pFeatureCursor.NextFeature();
                    }
                    pWorkspaceEdit.StopEditing(true);
                    pWorkspaceEdit.StopEditOperation();
                }
                else
                {
                    MessageBox.Show("所选的数据集为空！");
                }
                Code2(pFeatureLayer, pGridFCode);

            }
            else
            {
                MessageBoxEx.Show("没有选中任何图层！");
            }
        }


        /// <summary>
        /// 生成编码
        /// </summary>
        /// <param name="pMapControl"></param>
        /// <param name="cbxLayerName"></param>
        public void Code2(IFeatureLayer pFeatureLayer, Dictionary<IFeature, string> pGridFCode)
        {
            //要编码的字段——ENTIID
            string strField = "ENTIID";

            if (pFeatureLayer != null)
            {
                //检查地理实体编码字段是否存在，不存在就添加地理实体编码字段
                pClsCom.CheckGridCode(pFeatureLayer, strField);

                IDataset pDataset = pFeatureLayer.FeatureClass as IDataset;
                IWorkspaceEdit pWorkspaceEdit = null;
                if (pDataset != null)
                {
                    pWorkspaceEdit = pDataset.Workspace as IWorkspaceEdit;
                    if (pWorkspaceEdit != null || pWorkspaceEdit.IsBeingEdited() == false)
                    {
                        pWorkspaceEdit.StartEditing(true);
                        pWorkspaceEdit.StartEditOperation();
                    }
                    IFeatureCursor pFeatureCursor = pFeatureLayer.Search(null, false);

                    int i = pFeatureLayer.FeatureClass.FeatureCount(null);
                    int j = 0;

                    string pNum = string.Empty;
                    string pCode = string.Empty;
                    string pEntiidCode = string.Empty;
                    IFeature pFeature = pFeatureCursor.NextFeature();
                    int NullNum = 0;

                    while (pFeature != null)
                    {
                        string pGridCode = pFeature.get_Value(pFeature.Fields.FindField("GridCode")).ToString().Trim();
                        if (pGridCode.Length > 0)
                        //if (pGridFCode.ContainsKey(pFeature))
                        {
                            //分类码
                            int index = pFeature.Fields.FindField("FCode");
                            if (index != -1)
                            {
                                string FCode = pFeature.get_Value(pFeature.Fields.FindField("FCode")).ToString();
                                if (FCode == " ")
                                {
                                    NullNum++;
                                }
                                else
                                {
                                    //处理分类码
                                    if (ClsFcode.pDicFcodeGlobal.ContainsKey(FCode))
                                    {
                                        FCode = ClsFcode.pDicFcodeGlobal[FCode];
                                        FCode = FCode.ToUpper();

                                    }
                                    // 确定分类码长度为3
                                    if (FCode.Length == 3)
                                    {
                                        //格网代码和分类代码的组合
                                        pCode = pGridCode + FCode;

                                        List<IFeature> pFeaturekeyList = (from q in pGridFCode
                                                                          where q.Value == pCode
                                                                          select q.Key).ToList<IFeature>(); //get all keys

                                        if (pFeaturekeyList.Count >= 1 && pFeaturekeyList.Count <= 99)
                                        {
                                            for (int k = 0; k < pFeaturekeyList.Count; k++)
                                            {
                                                if (pFeature.OID == pFeaturekeyList[k].OID)
                                                {
                                                    pNum = "A" + string.Format("{0:00}", k + 1);
                                                }
                                            }

                                        }
                                        else if (pFeaturekeyList.Count > 99 && pFeaturekeyList.Count <= 198)
                                        {
                                            for (int k = 0; k < pFeaturekeyList.Count; k++)
                                            {
                                                if (pFeature.OID == pFeaturekeyList[k].OID)
                                                {
                                                    pNum = "B" + string.Format("{0:00}", k - 99);
                                                }
                                            }
                                        }

                                        //20170314
                                        pEntiidCode = pCode + pNum;
                                        pFeature.set_Value(pFeature.Fields.FindField(strField), pEntiidCode);
                                        pFeature.Store();
                                        j++;
                                    }
                                }
                            }
                            else
                            {
                                MessageBoxEx.Show("图层没有分类码字段！");
                                return;
                            }

                        }
                        pFeature = pFeatureCursor.NextFeature();
                    }
                    pWorkspaceEdit.StopEditing(true);
                    pWorkspaceEdit.StopEditOperation();
                    if (NullNum != 0)
                    {
                        MessageBoxEx.Show("编码生成完成！,其中FCode字段为空的要素个数为" + Convert.ToString(NullNum) + "个");
                    }
                    else
                    {
                        MessageBoxEx.Show("编码生成完成！");
                    }
                }
                else
                {
                    MessageBox.Show("所选的数据集为空！");
                }
            }
            else
            {
                MessageBoxEx.Show("没有选中任何图层！");
            }
        }


        /// <summary>
        /// 补全格网
        /// </summary>
        /// <param name="pMapControl"></param>
        /// <param name="cbxLayerName"></param>
        public void CreatGridCodeRest(IMap pMapControl, ComboBoxEx cbxLayerName)
        {
            string strField = "GridCode";

            IFeatureLayer pFeatureLayer = (IFeatureLayer)pClsCom.GetLayerByName(pMapControl, cbxLayerName.Text);

            IDataset cDataset = pFeatureLayer.FeatureClass as IDataset;
            IGeoDataset cGeoDataset = cDataset as IGeoDataset;
            ISpatialReference cSpatialReference = cGeoDataset.SpatialReference;
            if (cSpatialReference is IProjectedCoordinateSystem)
            {
                MessageBox.Show("该图层为投影坐标，请转换为相应的地理坐标,再开始地理编码！");
            }

            if (pFeatureLayer != null)
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
                    MessageBoxEx.Show("添加字段有误,数据被占用！");
                    return;
                }

                IDataset pDataset = pFeatureLayer.FeatureClass as IDataset;
                IWorkspaceEdit pWorkspaceEdit = null;
                if (pDataset != null)
                {
                    pWorkspaceEdit = pDataset.Workspace as IWorkspaceEdit;
                    if (pWorkspaceEdit != null || pWorkspaceEdit.IsBeingEdited() == false)
                    {
                        pWorkspaceEdit.StartEditing(true);
                        pWorkspaceEdit.StartEditOperation();
                    }
                    IFeatureCursor pFeatureCursor = pFeatureLayer.Search(null, false);

                    int i = pFeatureLayer.FeatureClass.FeatureCount(null);
                    int j = 0;

                    IFeature pFeature = pFeatureCursor.NextFeature();

                    while (pFeature != null)
                    {
                        //对于名称为空的字段进行筛选
                        //test
                        string test = pFeature.get_Value(pFeature.Fields.FindFieldByAliasName("名称")).ToString();
                        //对于名称为空的字段进行筛选
                        if (pFeature.get_Value(pFeature.Fields.FindFieldByAliasName("名称")).ToString().Trim().Length > 0)
                        {
                            //存在ENTIID字段，且该Feature的ENTIID字段值不为空
                            if (pTable.Fields.FindField("ENTIID") != -1 && pFeature.get_Value(pFeature.Fields.FindField("ENTIID")).ToString().Length > 0)
                            {
                                pFeature = pFeatureCursor.NextFeature();
                                continue;
                            }
                            else
                            {
                                //获取格网信息
                                string GridCode = pClsCom.GetCodeString(pFeature);
                                if (GridCode != "")
                                {
                                    pFeature.set_Value(pFeature.Fields.FindField(strField), GridCode);
                                    pFeature.Store();
                                    j++;
                                    //pFeature = pFeatureCursor.NextFeature();
                                }
                                else
                                {
                                    MessageBoxEx.Show("格网码生成失败，请转换成地理坐标！");
                                    return;
                                }
                            }
                        }
                        pFeature = pFeatureCursor.NextFeature();
                    }

                    pWorkspaceEdit.StopEditing(true);
                    pWorkspaceEdit.StopEditOperation();
                    MessageBoxEx.Show("格网生成成功！");
                }
            }
            else
            {
                MessageBoxEx.Show("没有选中任何图层！");
            }
        }

        /// <summary>
        /// 补全编码
        /// </summary>
        /// <param name="pMapControl"></param>
        /// <param name="cbxLayerName"></param>
        public void CodeRest(IMap pMapControl, ComboBoxEx cbxLayerName)
        {
            Dictionary<IFeature, string> pDicGridCode = new Dictionary<IFeature, string>();
            Dictionary<IFeature, string> pDicGridFCode = new Dictionary<IFeature, string>();
            Dictionary<IFeature, string> pDicEntiid = new Dictionary<IFeature, string>();

            string strField = "ENTIID";

            IFeatureLayer pFeatureLayer = (IFeatureLayer)pClsCom.GetLayerByName(pMapControl, cbxLayerName.Text);

            if (pFeatureLayer != null)
            {
                //判断是否存在ENTIID字段，如果不存在，新增ENTIID字段
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
                    MessageBox.Show("添加字段有误");
                    return;
                }

                IDataset pDataset = pFeatureLayer.FeatureClass as IDataset;
                IWorkspaceEdit pWorkspaceEdit = null;
                if (pDataset != null)
                {
                    pWorkspaceEdit = pDataset.Workspace as IWorkspaceEdit;
                    if (pWorkspaceEdit != null || pWorkspaceEdit.IsBeingEdited() == false)
                    {
                        pWorkspaceEdit.StartEditing(true);
                        pWorkspaceEdit.StartEditOperation();
                    }
                    IFeatureCursor pFeatureCursor = pFeatureLayer.Search(null, false);
                    string pNum = string.Empty;
                    string pCode = string.Empty;
                    string pEntiidCode = string.Empty;
                    IFeature pFeature = pFeatureCursor.NextFeature();
                    while (pFeature != null)
                    {
                        //有格网码，没有实体编码
                        if (pFeature.get_Value(pFeature.Fields.FindField("GridCode")).ToString().Length > 0 && pFeature.get_Value(pFeature.Fields.FindField("ENTIID")).ToString().Length == 0)
                        {
                            pDicGridCode.Add(pFeature, pFeature.get_Value(pFeature.Fields.FindField("GridCode")).ToString());
                        }
                        //有实体编码，没有格网码
                        if (pFeature.get_Value(pFeature.Fields.FindField("GridCode")).ToString().Length == 0 && pFeature.get_Value(pFeature.Fields.FindField("ENTIID")).ToString().Length > 0)
                        {
                            pDicEntiid.Add(pFeature, pFeature.get_Value(pFeature.Fields.FindField("ENTIID")).ToString());
                            //test
                            //string test = pFeature.get_Value(pFeature.Fields.FindField("ENTIID")).ToString().Substring(0, 11);
                            //string test1 = pFeature.get_Value(pFeature.Fields.FindField("ENTIID")).ToString().Substring(0, pFeature.get_Value(pFeature.Fields.FindField("ENTIID")).ToString().Length - 2);
                            pDicGridCode.Add(pFeature, pFeature.get_Value(pFeature.Fields.FindField("ENTIID")).ToString().Substring(0, 11));
                            pDicGridFCode.Add(pFeature, pFeature.get_Value(pFeature.Fields.FindField("ENTIID")).ToString().Substring(0, pFeature.get_Value(pFeature.Fields.FindField("ENTIID")).ToString().Length - 2));
                        }
                        pFeature = pFeatureCursor.NextFeature();
                    }
                    pWorkspaceEdit.StopEditing(true);
                    pWorkspaceEdit.StopEditOperation();
                }
                else
                {
                    MessageBox.Show("所选的数据集为空！");
                }
            }
            else
            {
                MessageBox.Show("没有选中任何图层！");
            }
            CodeRest2(pFeatureLayer, pDicEntiid, pDicGridFCode, pDicGridCode);
        }

        public void CodeRest2(IFeatureLayer pFeatureLayer, Dictionary<IFeature, string> pDicEntiid, Dictionary<IFeature, string> pDicGridFCode, Dictionary<IFeature, string> pDicGridCode)
        {
            string strField = "ENTIID";

            if (pFeatureLayer != null)
            {
                //判断是否存在ENTIID字段，如果不存在，新增ENTIID字段
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
                    MessageBox.Show("添加字段有误");
                    return;
                }

                IDataset pDataset = pFeatureLayer.FeatureClass as IDataset;
                IWorkspaceEdit pWorkspaceEdit = null;
                if (pDataset != null)
                {
                    pWorkspaceEdit = pDataset.Workspace as IWorkspaceEdit;
                    if (pWorkspaceEdit != null || pWorkspaceEdit.IsBeingEdited() == false)
                    {
                        pWorkspaceEdit.StartEditing(true);
                        pWorkspaceEdit.StartEditOperation();
                    }
                    IFeatureCursor pFeatureCursor = pFeatureLayer.Search(null, false);
                    //test
                    //int i = pFeatureLayer.FeatureClass.FeatureCount(null);

                    int NullNum = 0;

                    string pNum = string.Empty;

                    string pCode = string.Empty;
                    string pEntiidCode = string.Empty;

                    IFeature pFeature = pFeatureCursor.NextFeature();

                    while (pFeature != null)
                    {
                        int j = 0;
                        //test
                        object testoid = pFeature.OID;
                        //格网码不为空
                        if (pFeature.get_Value(pFeature.Fields.FindField("GridCode")).ToString().Length > 0)
                        {
                            //获取格网码
                            string pGrCode = pFeature.get_Value(pFeature.Fields.FindField("GridCode")).ToString();

                            //分类码
                            int pFcodeindex = pFeature.Fields.FindField("FCode");
                            //存在分类码字段
                            if (pFcodeindex != -1)
                            {
                                string pFcode = pFeature.get_Value(pFeature.Fields.FindField("FCode")).ToString();
                                //分类码不为空
                                if (pFcode == " ")
                                {
                                    //FillFrm(pFeature,pNullFrm.GetDataGridView,NullNum);
                                    NullNum++;
                                }
                                else
                                {
                                    //如果分类码不为空——即名称字段不为空且Feature不为空。
                                    //（Feature和名称字段内容不为空，那么格网码就不为空，进而地理编码就不为空）
                                    //组合第一个码段（格网码）和第二个码段（分类码）

                                    //处理分类码
                                    //ClsReturnFCode pReturnFcode = new ClsReturnFCode();
                                    //pFcode = pReturnFcode.ReturnFeatureClass(pFcode);
                                    pFcode = ZJGISCommon.Classes.ClsFcode.pDicFcodeGlobal[pFcode];
                                    //分类码有对应的大类
                                    if (pFcode != null)
                                    {
                                        foreach (string s in pDicGridCode.Values)
                                        {
                                            if (s == pGrCode)
                                            {
                                                j++;
                                            }
                                        }

                                        if (j == 1)
                                        {
                                            pNum = "A01";
                                        }
                                        else if (j > 1 && j < 100)
                                        {
                                            List<IFeature> keyList = (from q in pDicGridCode
                                                                      where q.Value == pGrCode
                                                                      select q.Key).ToList<IFeature>(); //get all keys

                                            for (int k = 0; k < keyList.Count; k++)
                                            {
                                                if (pFeature.OID == keyList[k].OID)
                                                {
                                                    pNum = "A" + string.Format("{0:00}", k + 1);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            pNum = "B" + string.Format("{0:00}", j - 1);
                                        }
                                        //组合地理实体编码的第一、第二、第三个码段，形成最终的地理实体编码
                                        pEntiidCode = pGrCode + pFcode + pNum;

                                        pDicEntiid.Add(pFeature, pEntiidCode);
                                        pDicGridFCode.Add(pFeature, pEntiidCode.Substring(0, pEntiidCode.Length - 2));

                                        pFeature.set_Value(pFeature.Fields.FindField(strField), pEntiidCode);
                                        pFeature.Store();
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("图层没有分类码字段！");
                                return;
                            }
                        }
                        pFeature = pFeatureCursor.NextFeature();
                    }
                    pWorkspaceEdit.StopEditing(true);
                    pWorkspaceEdit.StopEditOperation();

                    if (NullNum != 0)
                    {
                        MessageBox.Show("编码生成完成！,其中FCode字段为空的要素个数为" + Convert.ToString(NullNum) + "个");
                    }
                    else
                    {
                        MessageBox.Show("编码生成完成！");
                    }
                }
                else
                {
                    MessageBox.Show("所选的数据集为空！");
                }
            }
            else
            {
                MessageBox.Show("没有选中任何图层！");
            }
        }







    }
}
