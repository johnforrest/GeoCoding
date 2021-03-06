﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ZJGISCommon.Classes;
using ZJGISCommon.Forms;
using System.Data;

namespace ZJGISGCoding.Class
{
    public class ClsResEnti
    {
        ClsCommon pClsCom = new ClsCommon();
        FrmProgressBar progressbar = null;

        /// <summary>
        /// 生成格网码
        /// </summary>
        /// <param name="pMapControl"></param>
        /// <param name="cbxLayerName"></param>
        public void CreatGridCodeRES(IMap pMapControl, ComboBoxEx cbxLayerName)
        {

            string gridField = "GridCode";
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
                progressbar = new FrmProgressBar(pFeatureLayer.FeatureClass.FeatureCount(null));
                progressbar.Show();

                pClsCom.CheckGridField(pFeatureLayer, gridField);

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
                    //int i = pFeatureLayer.FeatureClass.FeatureCount(null);
                    int j = 0;

                    IFeature pFeature = pFeatureCursor.NextFeature();

                    while (pFeature != null)
                    {
                        if (pFeature.get_Value(pFeature.Fields.FindField("FCODE")).ToString() == "3103011500" ||
                            pFeature.get_Value(pFeature.Fields.FindField("FCODE")).ToString() == "3103012500" ||
                            pFeature.get_Value(pFeature.Fields.FindField("FCODE")).ToString() == "3103013500" ||
                            pFeature.get_Value(pFeature.Fields.FindField("FCODE")).ToString() == "3103020500" ||
                            pFeature.get_Value(pFeature.Fields.FindField("FCODE")).ToString() == "3106000500" ||
                            pFeature.get_Value(pFeature.Fields.FindField("FCODE")).ToString() == "3107000500" ||
                            pFeature.get_Value(pFeature.Fields.FindField("FCODE")).ToString() == "3108000500")
                        {

                            //获取格网信息
                            string GridCode = pClsCom.GetCodeString(pFeature);
                            if (GridCode != "")
                            {
                                pFeature.set_Value(pFeature.Fields.FindField(gridField), GridCode);
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
                        progressbar.GoOneStep();
                        pFeature = pFeatureCursor.NextFeature();
                    }
                    progressbar.CloseForm();
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
        /// 生成编码
        /// </summary>
        /// <param name="pMapControl"></param>
        /// <param name="cbxLayerName"></param>
        public void RESCode(IMap pMapControl, ComboBoxEx cbxLayerName)
        {
            Dictionary<int, string> pDic = new Dictionary<int, string>();
            Dictionary<int, string> pDicAfter = new Dictionary<int, string>();

            //要编码的字段——ENTIID
            string entiField = "ENTIID";

            IFeatureLayer pFeatureLayer = (IFeatureLayer)pClsCom.GetLayerByName(pMapControl, cbxLayerName.Text);
            if (pFeatureLayer != null)
            {
                progressbar = new FrmProgressBar(pFeatureLayer.FeatureClass.FeatureCount(null));
                progressbar.Show();

                pClsCom.CheckGridField(pFeatureLayer, entiField);

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
                        if (pFeature.get_Value(pFeature.Fields.FindField("GridCode")).ToString().Length > 0)
                        {
                            //分类码
                            int index = pFeature.Fields.FindField("FCode");
                            if (index != -1)
                            {
                                string FCode = pFeature.get_Value(pFeature.Fields.FindField("FCode")).ToString();
                                if (FCode == " ")
                                {
                                    //FillFrm(pFeature,pNullFrm.GetDataGridView,NullNum);
                                    NullNum++;
                                }
                                else
                                {
                                    //test
                                    //object testoid = pFeature.OID;
                                    //处理分类码
                                    //ClsReturnFCode pReturnFcode = new ClsReturnFCode();
                                    //FCode = pReturnFcode.ReturnFeatureClass(FCode);
                                    if (ZJGISCommon.Classes.ClsFcode.pDicFcodeGlobal.ContainsKey(FCode))
                                    {
                                        FCode = ZJGISCommon.Classes.ClsFcode.pDicFcodeGlobal[FCode];
                                        if (FCode != null)
                                        {
                                            //处理格网吗
                                            string GridCode = pFeature.get_Value(pFeature.Fields.FindField("GridCode")).ToString();
                                            //如果格网分类码不为空——即名称字段不为空且Feature不为空。
                                            //（Feature和名称字段内容不为空，那么格网码就不为空，进而地理编码就不为空）
                                            if (GridCode.Trim().Length > 0)
                                            {
                                                //格网代码和分类代码的组合
                                                pCode = GridCode + FCode;

                                                if (j == 0)
                                                {
                                                    //pNum = "001";
                                                    pNum = "A01";
                                                    pDic.Add(j, pCode);
                                                }
                                                else
                                                {
                                                    if (pDic.ContainsValue(pCode) == true)  //字典中出现GridCode和Fcode的组合字段
                                                    {

                                                        List<int> keyList = (from q in pDic
                                                                             where q.Value == pCode
                                                                             select q.Key).ToList<int>(); //get all keys

                                                        keyList.Sort();
                                                        int t = keyList.Max();
                                                        object test = Convert.ToInt32(pDicAfter[keyList.Max()].Substring(pDicAfter[keyList.Max()].Length - 2));
                                                        string test2 = string.Format("{0:00}", Convert.ToInt32(pDicAfter[keyList.Max()].Substring(pDicAfter[keyList.Max()].Length - 2)) + 1);

                                                        string pCharacter = pDicAfter[keyList.Max()].Substring(pDicAfter[keyList.Max()].Length - 3, 1);
                                                        //后三位都是数字的情况下
                                                        //pNum = string.Format("{0:000}", Convert.ToInt32(pDicAfter[keyList.Max()].Substring(pDicAfter[keyList.Max()].Length - 3)) + 1);
                                                        //后三位是1位字母加2位数字的情况下
                                                        if (pCharacter == "A" && Convert.ToInt32(pDicAfter[keyList.Max()].Substring(pDicAfter[keyList.Max()].Length - 2)) < 99)
                                                        {
                                                            pNum = "A" + string.Format("{0:00}", Convert.ToInt32(pDicAfter[keyList.Max()].Substring(pDicAfter[keyList.Max()].Length - 2)) + 1);
                                                        }
                                                        else
                                                        {
                                                            if (Convert.ToInt32(pDicAfter[keyList.Max()].Substring(pDicAfter[keyList.Max()].Length - 2)) == 99)
                                                            {
                                                                pNum = "B01";
                                                            }
                                                            else
                                                            {
                                                                pNum = "B" + string.Format("{0:00}", Convert.ToInt32(pDicAfter[keyList.Max()].Substring(pDicAfter[keyList.Max()].Length - 2)) + 1);
                                                            }
                                                        }
                                                    }
                                                    else  //字典中没有出现GridCode和Fcode的组合字段
                                                    {
                                                        //pNum = "001";
                                                        pNum = "A01";
                                                    }

                                                    pDic.Add(j, pCode);//20170310

                                                }
                                                //20170314
                                                pEntiidCode = pCode + pNum;
                                                if (!pDicAfter.Keys.Contains(j))
                                                {
                                                    pDicAfter.Add(j, pEntiidCode);
                                                }
                                                pFeature.set_Value(pFeature.Fields.FindField(entiField), pEntiidCode);
                                                pFeature.Store();
                                                j++;
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("分类码" + FCode + "不存在对应的大类，请添加分类码和大类的映射关系！");
                                        }

                                    }

                                }
                            }
                            else
                            {
                                MessageBoxEx.Show("图层没有分类码字段！");
                                return;
                            }

                        }

                        progressbar.GoOneStep();
                        pFeature = pFeatureCursor.NextFeature();
                    }
                    progressbar.CloseForm();
                    pWorkspaceEdit.StopEditing(true);
                    pWorkspaceEdit.StopEditOperation();
                    if (NullNum != 0)
                    {
                        MessageBoxEx.Show("编码生成完成！,其中FCode字段为空的要素个数为" + Convert.ToString(NullNum) + "个");
                    }
                    else
                    {
                        MessageBoxEx.Show("编码生成完成！");
                        pDic.Clear();
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
        /// 生成格网码
        /// </summary>
        /// <param name="pMapControl"></param>
        /// <param name="cbxLayerName"></param>
        public void RestRESGridCode(IMap pMapControl, ComboBoxEx cbxLayerName)
        {

            string gridField = "GridCode";
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
                progressbar = new FrmProgressBar(pFeatureLayer.FeatureClass.FeatureCount(null));
                progressbar.Show();
                //没有GridCode字段，先增加GridCode字段
                pClsCom.CheckGridField(pFeatureLayer, gridField);

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
                    //int i = pFeatureLayer.FeatureClass.FeatureCount(null);
                    int j = 0;

                    IFeature pFeature = pFeatureCursor.NextFeature();

                    while (pFeature != null)
                    {
                        bool entiidIsNull = pFeature.get_Value(pFeature.Fields.FindField(ClsConfig.LayerConfigs[(pFeatureLayer.FeatureClass as IDataset).Name].EntityID)).ToString().Trim().Length == 0;
                        //地理编码为空、且分类码为固定的几类
                        if (entiidIsNull)
                        {
                            if (pFeature.get_Value(pFeature.Fields.FindField("FCODE")).ToString() == "3103011500" ||
                                pFeature.get_Value(pFeature.Fields.FindField("FCODE")).ToString() == "3103012500" ||
                                pFeature.get_Value(pFeature.Fields.FindField("FCODE")).ToString() == "3103013500" ||
                                pFeature.get_Value(pFeature.Fields.FindField("FCODE")).ToString() == "3103020500" ||
                                pFeature.get_Value(pFeature.Fields.FindField("FCODE")).ToString() == "3106000500" ||
                                pFeature.get_Value(pFeature.Fields.FindField("FCODE")).ToString() == "3107000500" ||
                                pFeature.get_Value(pFeature.Fields.FindField("FCODE")).ToString() == "3108000500")
                            {

                                //获取格网信息
                                string GridCode = pClsCom.GetCodeString(pFeature);
                                if (GridCode != "")
                                {
                                    pFeature.set_Value(pFeature.Fields.FindField(gridField), GridCode);
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
                        progressbar.GoOneStep();
                        pFeature = pFeatureCursor.NextFeature();
                    }
                    progressbar.CloseForm();
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
        /// 补全房屋编码
        /// </summary>
        /// <param name="pMapControl"></param>
        /// <param name="cbxLayerName"></param>
        public void RestRESCode(IMap pMapControl, ComboBoxEx cbxLayerName)
        {
            Dictionary<IFeature, string> pDicGridCode = new Dictionary<IFeature, string>();
            Dictionary<IFeature, string> pDicGridFCode = new Dictionary<IFeature, string>();
            Dictionary<IFeature, string> pDicEntiid = new Dictionary<IFeature, string>();

            string entiField = "ENTIID";

            IFeatureLayer pFeatureLayer = (IFeatureLayer)pClsCom.GetLayerByName(pMapControl, cbxLayerName.Text);

            if (pFeatureLayer != null)
            {
                progressbar = new FrmProgressBar(pFeatureLayer.FeatureClass.FeatureCount(null)*2);
                progressbar.Show();
                //判断是否存在ENTIID字段，如果不存在，新增ENTIID字段
                pClsCom.CheckGridField(pFeatureLayer, entiField);

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
                        if (pFeature.get_Value(pFeature.Fields.FindField("GridCode")).ToString().Trim().Length > 0
                            && pFeature.get_Value(pFeature.Fields.FindField("ENTIID")).ToString().Trim().Length == 0)
                        {
                            pDicGridCode.Add(pFeature, pFeature.get_Value(pFeature.Fields.FindField("GridCode")).ToString());
                        }
                        //没有格网码,有实体编码
                        if (pFeature.get_Value(pFeature.Fields.FindField("GridCode")).ToString().Trim().Length == 0
                            && pFeature.get_Value(pFeature.Fields.FindField("ENTIID")).ToString().Trim().Length > 0)
                        {
                            pDicEntiid.Add(pFeature, pFeature.get_Value(pFeature.Fields.FindField("ENTIID")).ToString());
                            //test
                            string test = pFeature.get_Value(pFeature.Fields.FindField("ENTIID")).ToString().Substring(0, 11);
                            string test1 = pFeature.get_Value(pFeature.Fields.FindField("ENTIID")).ToString().Substring(0, pFeature.get_Value(pFeature.Fields.FindField("ENTIID")).ToString().Length - 2);

                            pDicGridCode.Add(pFeature, pFeature.get_Value(pFeature.Fields.FindField("ENTIID")).ToString().Substring(0, 11));
                            pDicGridFCode.Add(pFeature, pFeature.get_Value(pFeature.Fields.FindField("ENTIID")).ToString().Substring(0, pFeature.get_Value(pFeature.Fields.FindField("ENTIID")).ToString().Length - 2));
                        }
                        progressbar.GoOneStep();
                        pFeature = pFeatureCursor.NextFeature();
                    }
                    pWorkspaceEdit.StopEditing(true);
                    pWorkspaceEdit.StopEditOperation();
                    RestRESCode2(pFeatureLayer, pDicEntiid, pDicGridFCode, pDicGridCode,progressbar);

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

        public void RestRESCode2(IFeatureLayer pFeatureLayer, Dictionary<IFeature, string> pDicEntiid, Dictionary<IFeature, string> pDicGridFCode, Dictionary<IFeature, string> pDicGridCode,FrmProgressBar pgBar)
        {
            string entiField = "ENTIID";

            if (pFeatureLayer != null)
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
                            int pFcodeindex = pFeature.Fields.FindField("FCODE");
                            //存在分类码字段
                            if (pFcodeindex != -1)
                            {
                                string pFcode = pFeature.get_Value(pFcodeindex).ToString();
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
                                    if (ZJGISCommon.Classes.ClsFcode.pDicFcodeGlobal.ContainsKey(pFcode))
                                    {
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
                                                pNum = "B" + string.Format("{0:00}", j - 100);
                                            }
                                            //组合地理实体编码的第一、第二、第三个码段，形成最终的地理实体编码
                                            pEntiidCode = pGrCode + pFcode + pNum;

                                            pDicEntiid.Add(pFeature, pEntiidCode);
                                            pDicGridFCode.Add(pFeature, pEntiidCode.Substring(0, pEntiidCode.Length - 2));

                                            pFeature.set_Value(pFeature.Fields.FindField(entiField), pEntiidCode);
                                            pFeature.Store();
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("分类码" + pFcode + "不存在对应的大类，请添加分类码和大类的映射关系！");
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("图层没有分类码字段FCODE！");
                                return;
                            }
                        }
                        pgBar.GoOneStep();
                        pFeature = pFeatureCursor.NextFeature();
                    }
                    pgBar.CloseForm();
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

        /// <summary>
        /// 检查普通实体编码（有名称的实体才检查是否有编码）
        /// </summary>
        /// <param name="pMapControl"></param>
        /// <param name="cbxLayerName"></param>
        //public DataTable CheckCommonEnti(IMap pMapControl, ComboBoxEx cbxLayerName)
        public List<IRow> CheckRESEnti(IFeatureLayer pFeatureLayer)
        {

            //ITable pTable = new ITable();
            List<IRow> list = new List<IRow>();
            IDataset cDataset = pFeatureLayer.FeatureClass as IDataset;
            IGeoDataset cGeoDataset = cDataset as IGeoDataset;
            ISpatialReference cSpatialReference = cGeoDataset.SpatialReference;
            if (cSpatialReference is IProjectedCoordinateSystem)
            {
                MessageBox.Show("该图层为投影坐标，请转换为相应的地理坐标,再开始地理编码！");
            }

            if (pFeatureLayer != null)
            {
                progressbar = new FrmProgressBar(pFeatureLayer.FeatureClass.FeatureCount(null));
                progressbar.Show();

                //检查格网字段是否存在，不存在就添加格网字段GridCode
                //pClsCom.CheckGridCode(pFeatureLayer, gridField);
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
                        if ((pFeature.get_Value(pFeature.Fields.FindField("FCODE")).ToString() == "3103011500" ||
                            pFeature.get_Value(pFeature.Fields.FindField("FCODE")).ToString() == "3103012500" ||
                            pFeature.get_Value(pFeature.Fields.FindField("FCODE")).ToString() == "3103013500" ||
                            pFeature.get_Value(pFeature.Fields.FindField("FCODE")).ToString() == "3103020500" ||
                            pFeature.get_Value(pFeature.Fields.FindField("FCODE")).ToString() == "3106000500" ||
                            pFeature.get_Value(pFeature.Fields.FindField("FCODE")).ToString() == "3107000500" ||
                            pFeature.get_Value(pFeature.Fields.FindField("FCODE")).ToString() == "3108000500") &&
                            pFeature.get_Value(pFeature.Fields.FindField(ClsConfig.LayerConfigs[(pFeatureLayer as IDataset).Name].EntityID)).ToString().Length == 0)
                        {
                            list.Add((pFeature as IRow));
                        }
                        progressbar.GoOneStep();

                        pFeature = pFeatureCursor.NextFeature();
                    }
                    progressbar.CloseForm();
                    pWorkspaceEdit.StopEditing(true);
                    pWorkspaceEdit.StopEditOperation();
                }
            }
            else
            {
                MessageBoxEx.Show("没有选中任何图层！");
            }
            return list;
        }

        #region 20180106
        //public DataTable CheckRESEnti(IFeatureLayer pFeatureLayer)
        //{

        //    List<string> list = new List<string>();
        //    list.Add("3103011500");
        //    list.Add("3103012500");
        //    list.Add("3103013500");
        //    list.Add("3103020500");
        //    list.Add("3106000500");
        //    list.Add("3107000500");
        //    list.Add("3108000500");

        //    FrmProgressBar progressbar = null;

        //    //ITable pTable = new ITable();
        //    //List<IRow> list = new List<IRow>();
        //    DataTable dt = new DataTable();

        //    IDataset cDataset = pFeatureLayer.FeatureClass as IDataset;
        //    IGeoDataset cGeoDataset = cDataset as IGeoDataset;
        //    ISpatialReference cSpatialReference = cGeoDataset.SpatialReference;
        //    if (cSpatialReference is IProjectedCoordinateSystem)
        //    {
        //        MessageBox.Show("该图层为投影坐标，请转换为相应的地理坐标,再开始地理编码！");
        //    }

        //    if (pFeatureLayer != null)
        //    {
        //        progressbar = new FrmProgressBar(pFeatureLayer.FeatureClass.FeatureCount(null));
        //        progressbar.Show();

        //        dt = ZJGISCommon.Classes.ClsITableDataTable.FeatureClassToDataTable(pFeatureLayer.FeatureClass);


        //        DataRow[] drArr = dt.Select("WZMC='" + MaterialName + "' and   CZ='" + MaterialTexture + "   and   GG='" + MaterialSpecs + "'");

        //        //检查格网字段是否存在，不存在就添加格网字段GridCode
        //        //pClsCom.CheckGridCode(pFeatureLayer, gridField);
        //        IDataset pDataset = pFeatureLayer.FeatureClass as IDataset;
        //        IWorkspaceEdit pWorkspaceEdit = null;
        //        if (pDataset != null)
        //        {
        //            pWorkspaceEdit = pDataset.Workspace as IWorkspaceEdit;
        //            if (pWorkspaceEdit != null || pWorkspaceEdit.IsBeingEdited() == false)
        //            {
        //                pWorkspaceEdit.StartEditing(true);
        //                pWorkspaceEdit.StartEditOperation();
        //            }
        //            IFeatureCursor pFeatureCursor = pFeatureLayer.Search(null, false);

        //            int i = pFeatureLayer.FeatureClass.FeatureCount(null);
        //            int j = 0;

        //            IFeature pFeature = pFeatureCursor.NextFeature();
        //            while (pFeature != null)
        //            {
        //                if (pFeature.get_Value(pFeature.Fields.FindField("FCODE")).ToString() == "3103011500" ||
        //                    pFeature.get_Value(pFeature.Fields.FindField("FCODE")).ToString() == "3103012500" ||
        //                    pFeature.get_Value(pFeature.Fields.FindField("FCODE")).ToString() == "3103013500" ||
        //                    pFeature.get_Value(pFeature.Fields.FindField("FCODE")).ToString() == "3103020500" ||
        //                    pFeature.get_Value(pFeature.Fields.FindField("FCODE")).ToString() == "3106000500" ||
        //                    pFeature.get_Value(pFeature.Fields.FindField("FCODE")).ToString() == "3107000500" ||
        //                    pFeature.get_Value(pFeature.Fields.FindField("FCODE")).ToString() == "3108000500")
        //                {
        //                    list.Add((pFeature as IRow));
        //                }
        //                progressbar.GoOneStep();

        //                pFeature = pFeatureCursor.NextFeature();
        //            }
        //            progressbar.CloseForm();
        //            pWorkspaceEdit.StopEditing(true);
        //            pWorkspaceEdit.StopEditOperation();
        //        }
        //    }
        //    else
        //    {
        //        MessageBoxEx.Show("没有选中任何图层！");
        //    }
        //    return list;
        //}
        #endregion

    }
}
