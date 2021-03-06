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

namespace ZJGISGCoding.Class
{
    public class ClsPOIEnti
    {
        ClsCommon pClsCom = new ClsCommon();
        FrmProgressBar progressbar;
        /// <summary>
        /// 生成格网码(道路)
        /// </summary>
        /// <param name="pMapControl"></param>
        /// <param name="cbxLayerName"></param>
        public void RestPOIGrid(IMap pMapControl, ComboBoxEx cbxLayerName)
        {
            Dictionary<IFeature, string> pBothNotNull = new Dictionary<IFeature, string>();
            Dictionary<IFeature, string> pRoadNotNull = new Dictionary<IFeature, string>();
            Dictionary<IFeature, string> pNameNotNull = new Dictionary<IFeature, string>();
            //Dictionary<IFeature, string> pGridCode = new Dictionary<IFeature, string>();
            List<object> pOID = new List<object>();

            string gridField = "GridCode";
            IFeatureLayer pFeatureLayer = (IFeatureLayer)pClsCom.GetLayerByName(pMapControl, cbxLayerName.Text);

           

            if (pFeatureLayer != null)
            {
                progressbar = new FrmProgressBar(pFeatureLayer.FeatureClass.FeatureCount(null));
                progressbar.Show();

                IDataset cDataset = pFeatureLayer.FeatureClass as IDataset;
                IGeoDataset cGeoDataset = cDataset as IGeoDataset;
                ISpatialReference cSpatialReference = cGeoDataset.SpatialReference;
                if (cSpatialReference is IProjectedCoordinateSystem)
                {
                    MessageBox.Show("该图层为投影坐标，请转换为相应的地理坐标,再开始地理编码！");
                }
                //检查是否存在格网字段，不存在则补充此字段
                pClsCom.CheckGridField(pFeatureLayer, gridField);

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
                        IFeature pFeature = pFeatureCursor.NextFeature();
                        while (pFeature != null)
                        {
                            string pFeatureName = pFeature.get_Value(pFeature.Fields.FindField(ClsConfig.LayerConfigs[(pFeatureLayer.FeatureClass as IDataset).Name].NameField)).ToString();
                            string pFeaEnti = pFeature.get_Value(pFeature.Fields.FindField(ClsConfig.LayerConfigs[(pFeatureLayer.FeatureClass as IDataset).Name].EntityID)).ToString();//匹配名称
                            //名称不为空，地理编码为空
                            if (pFeatureName.Trim().Length > 0 && pFeaEnti.Trim().Length == 0)
                            {
                                //获取格网信息
                                string GridCode = pClsCom.GetCodeString(pFeature);
                                if (GridCode != "")
                                {
                                    pFeature.set_Value(pFeature.Fields.FindField(gridField), GridCode);
                                    pFeature.Store();
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
                    }
                    MessageBox.Show("POI格网补充成功！");

                }
                else
                {
                    MessageBox.Show("此图层不存在名称字段，请重新配置图层！");
                }
            }
            else
            {
                MessageBoxEx.Show("没有选中任何图层,请选择图层！");
            }

        }

        /// <summary>
        /// 对剩余POI点进行编码
        /// </summary>
        /// <param name="pMapControl"></param>
        /// <param name="cbxLayerName"></param>
        public void RestPOICode(IMap pMapControl, ComboBoxEx cbxLayerName)
        {
            IFeatureLayer pFeatureLayer = (IFeatureLayer)pClsCom.GetLayerByName(pMapControl, cbxLayerName.Text);
            if (pFeatureLayer!=null)
            {
                progressbar = new FrmProgressBar(pFeatureLayer.FeatureClass.FeatureCount(null)*2);
                progressbar.Show();

                IFields pFields = pFeatureLayer.FeatureClass.Fields;

                Dictionary<IFeature, string> pDicGridCode = new Dictionary<IFeature, string>();
                Dictionary<IFeature, string> pEntiCode = new Dictionary<IFeature, string>();
                //遍历Feature
                IDataset pDataset = pFeatureLayer.FeatureClass as IDataset;
                IFeatureClass pFeatureClass = pFeatureLayer.FeatureClass;
                bool hasFOCDE = false;
                string fCODE = string.Empty;
                for (int i = 0; i < pFeatureClass.Fields.FieldCount; i++)
                {
                    if (pFeatureClass.Fields.get_Field(i).Name == "FCODE")
                    {
                        hasFOCDE = true;
                    }
                }

                if (hasFOCDE)
                {
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
                        IFeature pFeature = pFeatureCursor.NextFeature();
                        while (pFeature != null)
                        {
                            //格网不为空，记录格网
                            if (pFeature.get_Value(pFeature.Fields.FindField("GridCode")).ToString().Length > 0)
                            {
                                pDicGridCode.Add(pFeature, pFeature.get_Value(pFeature.Fields.FindField("GridCode")).ToString());
                            }
                            //地理编码不为空，截取格网并记录下来
                            if (pFeature.get_Value(pFeature.Fields.FindField("ENTIID")).ToString().Trim().Length > 0)
                            {
                                string pgdCode = pFeature.get_Value(pFeature.Fields.FindField("ENTIID")).ToString().Substring(0, 11);
                                if (!pDicGridCode.ContainsKey(pFeature))
                                {
                                    pDicGridCode.Add(pFeature, pgdCode);
                                }

                                if (!pEntiCode.ContainsKey(pFeature))
                                {
                                    pEntiCode.Add(pFeature, pFeature.get_Value(pFeature.Fields.FindField("ENTIID")).ToString());
                                }

                            }
                            progressbar.GoOneStep();
                            pFeature = pFeatureCursor.NextFeature();
                        }
                        pWorkspaceEdit.StopEditing(true);
                        pWorkspaceEdit.StopEditOperation();
                        RestPOICode2(pFeatureLayer, pEntiCode, pDicGridCode, "GridCode", "ENTIID",progressbar);
                    }
                    else
                    {
                        MessageBox.Show("该图层不存在分类码字段！分类码字段名称应该FCODE");
                        return;
                    }
                    MessageBox.Show("剩余POI编码成功！");
                }
                else
                {
                    MessageBox.Show("没有分类码FCODE字段，请检查图层字段！");
                } 
            }
            else
            {
                MessageBox.Show("没有选中任何图层，请选择图层！");
            }
        }

        private void RestPOICode2(IFeatureLayer pFeatureLayer, Dictionary<IFeature, string> pEntiCode, Dictionary<IFeature, string> pDicGridCode, string pGridCode, string pENEIID,FrmProgressBar pgBar)
        {
            string pFCODE = string.Empty;
            //遍历Feature
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
                IFeature pFeature = pFeatureCursor.NextFeature();
                string pNum = string.Empty;
                string pEntiidCode = string.Empty;

                while (pFeature != null)
                {
                    int flag = 0;
                    string gridCode1 = pFeature.get_Value(pFeature.Fields.FindField(pGridCode)).ToString();
                    pFCODE = pFeature.get_Value(pFeature.Fields.FindField("FCODE")).ToString();
                    if (pFCODE.Trim().Length > 0)
                    {
                        //ClsReturnFCode pReturnFcode = new ClsReturnFCode();
                        //pFCODE = pReturnFcode.ReturnFeatureClass(pFCODE);
                        pFCODE = ZJGISCommon.Classes.ClsFcode.pDicFcodeGlobal[pFCODE];
                        //格网码不为空，只对有格网码的的要素进行编码
                        if (pFeature.get_Value(pFeature.Fields.FindField(pGridCode)).ToString().Length > 0 && pFCODE.Length > 0)
                        {
                            foreach (string s in pDicGridCode.Values)
                            {
                                if (gridCode1 == s)
                                {
                                    flag++;
                                }
                            }
                            //格网码唯一的情况：那么地理编码也一定唯一
                            if (flag == 1)
                            {
                                pNum = "A01";
                                pEntiidCode = gridCode1 + pFCODE + pNum;
                                pFeature.set_Value(pFeature.Fields.FindField(pENEIID), pEntiidCode);
                                pFeature.Store();
                            }
                            //格网码有多个的情况
                            else
                            {
                                List<IFeature> keyListTotal = (from q in pDicGridCode
                                                               where q.Value == gridCode1
                                                               select q.Key).ToList<IFeature>(); //get all keys
                                //test
                                object testobj = pFeature.OID;

                                int flag2 = 0;
                                //顺序码
                                for (int k = 0; k < keyListTotal.Count; k++)
                                {
                                    string enticode = keyListTotal[k].get_Value(keyListTotal[k].Fields.FindField("ENTIID")).ToString();
                                    if (enticode.Trim().Length > 0)
                                    {
                                        flag2++;
                                        pNum = enticode.Substring(enticode.Length - 2);
                                        //pNum = "A" + string.Format("{0:00}", (Convert.ToInt32(pNum) + 1));
                                    }
                                }
                                //顺序码
                                for (int k = 0; k < keyListTotal.Count; k++)
                                {
                                    if (pFeature.OID == keyListTotal[k].OID && flag2 > 0)
                                    {
                                        pNum = "A" + string.Format("{0:00}", k + flag2 + 1);
                                    }

                                    if (pFeature.OID == keyListTotal[k].OID && flag2 == 0)
                                    {
                                        pNum = "A" + string.Format("{0:00}", k + 1);
                                    }

                                }

                                pEntiidCode = gridCode1 + pFCODE + pNum;
                                pFeature.set_Value(pFeature.Fields.FindField(pENEIID), pEntiidCode);
                                pFeature.Store();
                            }
                        }
                    }
                    pgBar.GoOneStep();
                    pFeature = pFeatureCursor.NextFeature();
                }
                pgBar.CloseForm();
                pWorkspaceEdit.StopEditing(true);
                pWorkspaceEdit.StopEditOperation();
            }
        }


    }
}
