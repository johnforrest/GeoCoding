using System;
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

namespace ZJGISGCoding.Class
{
    public class ClsRoadEnti
    {
        ClsCommon pClsCom = new ClsCommon();

        /// <summary>
        /// 生成格网码(道路)
        /// </summary>
        /// <param name="pMapControl"></param>
        /// <param name="cbxLayerName"></param>
        public void CreatGridCodeRoad(IMap pMapControl, ComboBoxEx cbxLayerName)
        {

            Dictionary<IFeature, string> pBothNotNull = new Dictionary<IFeature, string>();
            Dictionary<IFeature, string> pRoadNotNull = new Dictionary<IFeature, string>();
            Dictionary<IFeature, string> pNameNotNull = new Dictionary<IFeature, string>();
            //Dictionary<IFeature, string> pGridCode = new Dictionary<IFeature, string>();
            List<object> pOID = new List<object>();

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
                    //int i = pFeatureLayer.FeatureClass.FeatureCount(null);
                    int j = 0, k = 0, m = 0;

                    IFeature pFeature = pFeatureCursor.NextFeature();

                    while (pFeature != null)
                    {
                        int test = pFeature.Fields.FindField(ClsConfig.LayerConfigs[(pFeatureLayer.FeatureClass as IDataset).Name].NameField);
                        int test2 = pFeature.Fields.FindField("ROADCODE");
                        #region 注释掉
                        ////路线编码和名称都不为空
                        //if (pFeature.get_Value(pFeature.Fields.FindFieldByAliasName("名称")).ToString().Trim().Length > 0 && pFeature.get_Value(pFeature.Fields.FindField("ROADCODE")).ToString().Trim().Length > 0)
                        //{
                        //    pBothNotNull.Add(pFeature, pFeature.get_Value(pFeature.Fields.FindFieldByAliasName("名称")).ToString());
                        //    pOID.Add(pFeature.OID);
                        //}
                        ////路线编码不为空且一个道路字母除外（例如Q等）,名称为空
                        //if (pFeature.get_Value(pFeature.Fields.FindField("ROADCODE")).ToString().Trim().Length > 1 && pFeature.get_Value(pFeature.Fields.FindFieldByAliasName("名称")).ToString().Trim().Length == 0)
                        //{
                        //    pRoadNotNull.Add(pFeature, pFeature.get_Value(pFeature.Fields.FindField("ROADCODE")).ToString());
                        //    pOID.Add(pFeature.OID);

                        //}
                        ////名称不为空，路线编码为空
                        //if (pFeature.get_Value(pFeature.Fields.FindFieldByAliasName("名称")).ToString().Trim().Length > 0 && pFeature.get_Value(pFeature.Fields.FindField("ROADCODE")).ToString().Trim().Length == 0)
                        //{
                        //    pNameNotNull.Add(pFeature, pFeature.get_Value(pFeature.Fields.FindFieldByAliasName("名称")).ToString());
                        //    pOID.Add(pFeature.OID);
                        //}
                        #endregion
                        //名称不为空，路线编码为空
                        if (pFeature.get_Value(pFeature.Fields.FindField(ClsConfig.LayerConfigs[(pFeatureLayer.FeatureClass as IDataset).Name].NameField)).ToString().Trim().Length > 0)
                        {
                            pNameNotNull.Add(pFeature, pFeature.get_Value(pFeature.Fields.FindField(ClsConfig.LayerConfigs[(pFeatureLayer.FeatureClass as IDataset).Name].NameField)).ToString());
                            pOID.Add(pFeature.OID);
                        }
                        //路线编码不为空且一个道路字母除外（例如Q等）,名称为空
                        if (pFeature.get_Value(pFeature.Fields.FindField("ROADCODE")).ToString().Trim().Length > 1 && pFeature.get_Value(pFeature.Fields.FindField(ClsConfig.LayerConfigs[(pFeatureLayer.FeatureClass as IDataset).Name].NameField)).ToString().Trim().Length == 0)
                        {
                            pRoadNotNull.Add(pFeature, pFeature.get_Value(pFeature.Fields.FindField("ROADCODE")).ToString());
                            pOID.Add(pFeature.OID);
                        }
                        pFeature = pFeatureCursor.NextFeature();

                    }

                    #region  遍历所有Feature，查找属于同一个地理实体的图元
                    //遍历所有Feature，查找属于同一个地理实体的图元
                    //while (pFeature != null)
                    //{
                    //    //路线编码和名称都不为空
                    //    if (pFeature.get_Value(pFeature.Fields.FindField("NAME")).ToString().Trim().Length > 0 && pFeature.get_Value(pFeature.Fields.FindField("ROADCODE")).ToString().Trim().Length > 0)
                    //    {
                    //        if (j == 0)
                    //        {
                    //            //获取格网信息
                    //            string GridCode = GetCodeString(pFeature);
                    //            if (GridCode != "")
                    //            {
                    //                pFeature.set_Value(pFeature.Fields.FindField(strField), GridCode);
                    //                pFeature.Store();
                    //                j++;
                    //                //pFeature = pFeatureCursor.NextFeature();
                    //                //pBothNotNull.Add(pFeature,pFeature.get_Value(pFeature.Fields.FindField("NAME")).ToString());
                    //            }
                    //            else
                    //            {
                    //                MessageBoxEx.Show("格网码生成失败，请转换成地理坐标！");
                    //                return;
                    //            }
                    //        }
                    //        else
                    //        {
                    //            foreach (string s in pBothNotNull.Values)
                    //            {
                    //                if (s == pFeature.get_Value(pFeature.Fields.FindField("NAME")))
                    //                {
                    //                    IFeature pfirstKeyFeature = pBothNotNull.FirstOrDefault(q => q.Value == s).Key;  //get first key
                    //                    if (pfirstKeyFeature.get_Value(pfirstKeyFeature.Fields.FindField("ROADCODE")).ToString() == pFeature.get_Value(pFeature.Fields.FindField("ROADCODE")).ToString())
                    //                    {
                    //                        pFeature.set_Value(pFeature.Fields.FindField(strField), pfirstKeyFeature.get_Value(pfirstKeyFeature.Fields.FindField("GridCode")));
                    //                        pFeature.Store();
                    //                        m++;
                    //                        //pFeature = pFeatureCursor.NextFeature();
                    //                        //pBothNotNull.Add(pFeature, pFeature.get_Value(pFeature.Fields.FindField("NAME")).ToString());
                    //                    } 
                    //                    else
                    //                    {
                    //                        string GridCode = GetCodeString(pFeature);
                    //                        if (GridCode != "")
                    //                        {
                    //                            pFeature.set_Value(pFeature.Fields.FindField(strField), GridCode);
                    //                            pFeature.Store();
                    //                            j++;
                    //                            //pFeature = pFeatureCursor.NextFeature();
                    //                            //pBothNotNull.Add(pFeature, pFeature.get_Value(pFeature.Fields.FindField("NAME")).ToString());
                    //                        }
                    //                        else
                    //                        {
                    //                            MessageBoxEx.Show("格网码生成失败，请转换成地理坐标！");
                    //                            return;
                    //                        }
                    //                    }

                    //                }
                    //                else
                    //                {
                    //                    string GridCode = GetCodeString(pFeature);
                    //                    if (GridCode != "")
                    //                    {
                    //                        pFeature.set_Value(pFeature.Fields.FindField(strField), GridCode);
                    //                        pFeature.Store();
                    //                        j++;
                    //                        //pFeature = pFeatureCursor.NextFeature();
                    //                        //pBothNotNull.Add(pFeature, pFeature.get_Value(pFeature.Fields.FindField("NAME")).ToString());
                    //                    }
                    //                    else
                    //                    {
                    //                        MessageBoxEx.Show("格网码生成失败，请转换成地理坐标！");
                    //                        return;
                    //                    }
                    //                }
                    //            }
                    //        }
                    //    }


                    //    //路线编码不为空,名称为空
                    //    if (pFeature.get_Value(pFeature.Fields.FindField("ROADCODE")).ToString().Trim().Length > 0 && pFeature.get_Value(pFeature.Fields.FindField("NAME")).ToString().Trim().Length == 0)
                    //    {
                    //        if (k == 0)
                    //        {
                    //            //获取格网信息
                    //            string GridCode = GetCodeString(pFeature);
                    //            if (GridCode != "")
                    //            {
                    //                pFeature.set_Value(pFeature.Fields.FindField(strField), GridCode);
                    //                pFeature.Store();
                    //                k++;
                    //                //pFeature = pFeatureCursor.NextFeature();
                    //                //pNameNull.Add(pFeature, pFeature.get_Value(pFeature.Fields.FindField("ROADCODE")).ToString());

                    //            }
                    //            else
                    //            {
                    //                MessageBoxEx.Show("格网码生成失败，请转换成地理坐标！");
                    //                return;
                    //            }
                    //        }
                    //        else
                    //        {
                    //            foreach (string s in pNameNull.Values)
                    //            {
                    //                //test
                    //                string test = pFeature.get_Value(pFeature.Fields.FindField("ROADCODE")).ToString();

                    //                if (s == pFeature.get_Value(pFeature.Fields.FindField("ROADCODE")).ToString())
                    //                {
                    //                    IFeature pfirstKeyFeature = pEntiIDNull.FirstOrDefault(q => q.Value == s).Key;  //get first key
                    //                    pFeature.set_Value(pFeature.Fields.FindField(strField), pfirstKeyFeature.get_Value(pfirstKeyFeature.Fields.FindField("ROADCODE")));
                    //                    pFeature.Store();
                    //                    k++;
                    //                    //pFeature = pFeatureCursor.NextFeature();
                    //                    //pNameNull.Add(pFeature, pFeature.get_Value(pFeature.Fields.FindField("ROADCODE")).ToString());
                    //                }
                    //                else
                    //                {
                    //                    //获取格网信息
                    //                    string GridCode = GetCodeString(pFeature);
                    //                    if (GridCode != "")
                    //                    {
                    //                        pFeature.set_Value(pFeature.Fields.FindField(strField), GridCode);
                    //                        pFeature.Store();
                    //                        k++;
                    //                        //pFeature = pFeatureCursor.NextFeature();
                    //                        //pNameNull.Add(pFeature, pFeature.get_Value(pFeature.Fields.FindField("ROADCODE")).ToString());

                    //                    }
                    //                    else
                    //                    {
                    //                        MessageBoxEx.Show("格网码生成失败，请转换成地理坐标！");
                    //                        return;
                    //                    }
                    //                }
                    //            }

                    //        }

                    //    }


                    //    //名称不为空，路线编码为空
                    //    if (pFeature.get_Value(pFeature.Fields.FindField("NAME")).ToString().Trim().Length > 0 && pFeature.get_Value(pFeature.Fields.FindField("ROADCODE")).ToString().Trim().Length == 0)
                    //    {
                    //        if (m == 0)
                    //        {
                    //            //获取格网信息
                    //            string GridCode = GetCodeString(pFeature);
                    //            if (GridCode != "")
                    //            {
                    //                pFeature.set_Value(pFeature.Fields.FindField(strField), GridCode);
                    //                pFeature.Store();
                    //                m++;
                    //                //pFeature = pFeatureCursor.NextFeature();
                    //                //pEntiIDNull.Add(pFeature, pFeature.get_Value(pFeature.Fields.FindField("NAME")).ToString());
                    //            }
                    //            else
                    //            {
                    //                MessageBoxEx.Show("格网码生成失败，请转换成地理坐标！");
                    //                return;
                    //            }
                    //        }
                    //        else
                    //        {
                    //            foreach (string s in pEntiIDNull.Values)
                    //            {
                    //                //test
                    //                string test = pFeature.get_Value(pFeature.Fields.FindField("NAME")).ToString();

                    //                if (s == pFeature.get_Value(pFeature.Fields.FindField("NAME")).ToString())
                    //                {
                    //                    IFeature pfirstKeyFeature = pEntiIDNull.FirstOrDefault(q => q.Value == s).Key;  //get first key

                    //                    pFeature.set_Value(pFeature.Fields.FindField(strField), pfirstKeyFeature.get_Value(pfirstKeyFeature.Fields.FindField("GridCode")));
                    //                    pFeature.Store();
                    //                    m++;
                    //                    //pFeature = pFeatureCursor.NextFeature();
                    //                    //pEntiIDNull.Add(pFeature, pFeature.get_Value(pFeature.Fields.FindField("NAME")).ToString());
                    //                }
                    //                else
                    //                {
                    //                    //获取格网信息
                    //                    string GridCode = GetCodeString(pFeature);
                    //                    if (GridCode != "")
                    //                    {
                    //                        pFeature.set_Value(pFeature.Fields.FindField(strField), GridCode);
                    //                        pFeature.Store();
                    //                        m++;
                    //                        //pFeature = pFeatureCursor.NextFeature();
                    //                        //pEntiIDNull.Add(pFeature, pFeature.get_Value(pFeature.Fields.FindField("NAME")).ToString());

                    //                    }
                    //                    else
                    //                    {
                    //                        MessageBoxEx.Show("格网码生成失败，请转换成地理坐标！");
                    //                        return;
                    //                    }
                    //                }
                    //            }
                    //        }
                    //    }
                    //    pFeature = pFeatureCursor.NextFeature();
                    //}
                    #endregion

                    pWorkspaceEdit.StopEditing(true);
                    pWorkspaceEdit.StopEditOperation();

                }
            }
            else
            {
                MessageBoxEx.Show("没有选中任何图层！");
            }
            CreatGridCodeRoad2(pFeatureLayer, pNameNotNull, pRoadNotNull, pOID);
        }


        ///// <summary>
        ///// 生成格网码(道路)
        ///// </summary>
        ///// <param name="pMapControl"></param>
        ///// <param name="cbxLayerName"></param>
        //public void CreatGridCodeRoad2(IFeatureLayer pFeatureLayer, Dictionary<IFeature, string> pBothNotNull,
        //    Dictionary<IFeature, string> pNameNotNull, Dictionary<IFeature, string> pRoadNotNull, List<object> pOID)
        //{
        //    string strField = "GridCode";
        //    if (pFeatureLayer != null)
        //    {
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
        //            //int i = pFeatureLayer.FeatureClass.FeatureCount(null);

        //            IFeature pFeature = pFeatureCursor.NextFeature();

        //            //遍历所有Feature，查找属于同一个地理实体的图元
        //            while (pFeature != null)
        //            {
        //                if (pOID.Contains(pFeature.OID))
        //                {
        //                    //路线编码和名称都不为空
        //                    if (pFeature.get_Value(pFeature.Fields.FindFieldByAliasName("名称")).ToString().Trim().Length > 0 && pFeature.get_Value(pFeature.Fields.FindField("ROADCODE")).ToString().Trim().Length > 0)
        //                    {
        //                        string pFeatureName1 = pFeature.get_Value(pFeature.Fields.FindFieldByAliasName("名称")).ToString();
        //                        string pRoadCode1 = pFeature.get_Value(pFeature.Fields.FindField("ROADCODE")).ToString();
        //                        int flag = 0;

        //                        //获取格网信息
        //                        foreach (string s in pBothNotNull.Values)
        //                        {
        //                            if (s == pFeatureName1)
        //                            {
        //                                flag++;
        //                            }
        //                        }

        //                        if (flag == 1)
        //                        {
        //                            string pGridCode1 = pClsCom.GetCodeString(pFeature);
        //                            if (pGridCode1 != "")
        //                            {
        //                                pFeature.set_Value(pFeature.Fields.FindField(strField), pGridCode1);
        //                                pFeature.Store();
        //                            }
        //                            else
        //                            {
        //                                MessageBoxEx.Show("格网码生成失败，请转换成地理坐标！");
        //                                return;
        //                            }
        //                        }
        //                        else
        //                        {
        //                            List<IFeature> keyList = (from q in pBothNotNull
        //                                                      where q.Value == pFeatureName1
        //                                                      select q.Key).ToList<IFeature>(); //get all keys

        //                            if (pFeature.OID == keyList[0].OID)
        //                            {
        //                                CreatGridCodeComm(pFeature, pFeature, strField);

        //                                #region 注释2
        //                                ////获取格网信息
        //                                //string GridCode = GetCodeString(pFeature);
        //                                //if (GridCode != "")
        //                                //{
        //                                //    pFeature.set_Value(pFeature.Fields.FindField(strField), GridCode);
        //                                //    pFeature.Store();
        //                                //}
        //                                //else
        //                                //{
        //                                //    MessageBoxEx.Show("格网码生成失败，请转换成地理坐标！");
        //                                //    return;
        //                                //}
        //                                #endregion
        //                            }

        //                            if (pFeature.OID != keyList[0].OID)
        //                            {
        //                                CreatGridCodeComm(pFeature, keyList[0], strField);

        //                                #region 注释3
        //                                ////获取格网信息
        //                                //string GridCode = GetCodeString(keyList[0]);
        //                                //if (GridCode != "")
        //                                //{
        //                                //    pFeature.set_Value(pFeature.Fields.FindField(strField), GridCode);
        //                                //    pFeature.Store();
        //                                //}
        //                                //else
        //                                //{
        //                                //    MessageBoxEx.Show("格网码生成失败，请转换成地理坐标！");
        //                                //    return;
        //                                //}
        //                                #endregion

        //                            }
        //                        }

        //                    }


        //                    //路线编码不为空,名称为空
        //                    if (pFeature.get_Value(pFeature.Fields.FindField("ROADCODE")).ToString().Trim().Length > 1 && pFeature.get_Value(pFeature.Fields.FindFieldByAliasName("名称")).ToString().Trim().Length == 0)
        //                    {
        //                        string pRoadCode2 = pFeature.get_Value(pFeature.Fields.FindField("ROADCODE")).ToString();

        //                        int flag1 = 0;

        //                        foreach (string s in pRoadNotNull.Values)
        //                        {
        //                            if (pRoadCode2 == s)
        //                            {
        //                                flag1++;
        //                            }
        //                        }
        //                        //名字只出现一次的情况
        //                        if (flag1 == 1)
        //                        {
        //                            //获取格网信息
        //                            string pGridCode2 = pClsCom.GetCodeString(pFeature);
        //                            if (pGridCode2 != "")
        //                            {
        //                                pFeature.set_Value(pFeature.Fields.FindField(strField), pGridCode2);
        //                                pFeature.Store();
        //                            }
        //                            else
        //                            {
        //                                MessageBoxEx.Show("格网码生成失败，请转换成地理坐标！");
        //                                return;
        //                            }
        //                        }
        //                        //名字出现多次的情况
        //                        else
        //                        {
        //                            List<IFeature> keyList = (from q in pRoadNotNull
        //                                                      where q.Value == pRoadCode2
        //                                                      select q.Key).ToList<IFeature>(); //get all keys
        //                            if (pFeature.OID == keyList[0].OID)
        //                            {
        //                                CreatGridCodeComm(pFeature, pFeature, strField);

        //                                #region 注释5
        //                                //////test
        //                                ////获取格网信息
        //                                //string GridCode = GetCodeString(pFeature);
        //                                //if (GridCode != "")
        //                                //{
        //                                //    pFeature.set_Value(pFeature.Fields.FindField(strField), GridCode);
        //                                //    pFeature.Store();
        //                                //}
        //                                //else
        //                                //{
        //                                //    MessageBoxEx.Show("格网码生成失败，请转换成地理坐标！");
        //                                //    return;
        //                                //}
        //                                #endregion
        //                            }
        //                            if (pFeature.OID != keyList[0].OID)
        //                            {
        //                                CreatGridCodeComm(pFeature, keyList[0], strField);

        //                                #region 注释6
        //                                ////获取格网信息
        //                                //string GridCode = GetCodeString(keyList[0]);
        //                                //if (GridCode != "")
        //                                //{
        //                                //    pFeature.set_Value(pFeature.Fields.FindField(strField), GridCode);
        //                                //    pFeature.Store();
        //                                //}
        //                                //else
        //                                //{
        //                                //    MessageBoxEx.Show("格网码生成失败，请转换成地理坐标！");
        //                                //    return;
        //                                //}
        //                                #endregion
        //                            }
        //                        }

        //                    }


        //                    //名称不为空，路线编码为空
        //                    if (pFeature.get_Value(pFeature.Fields.FindFieldByAliasName("名称")).ToString().Trim().Length > 0 && pFeature.get_Value(pFeature.Fields.FindField("ROADCODE")).ToString().Trim().Length == 0)
        //                    {
        //                        string pFeatureName2 = pFeature.get_Value(pFeature.Fields.FindFieldByAliasName("名称")).ToString();
        //                        int flag2 = 0;
        //                        foreach (string s in pNameNotNull.Values)
        //                        {
        //                            if (pFeatureName2 == s)
        //                            {
        //                                flag2++;
        //                            }
        //                        }
        //                        //名字没有相同的情况
        //                        if (flag2 == 1)
        //                        {
        //                            //获取格网信息
        //                            string pGridCode3 = pClsCom.GetCodeString(pFeature);
        //                            if (pGridCode3 != "")
        //                            {
        //                                pFeature.set_Value(pFeature.Fields.FindField(strField), pGridCode3);
        //                                pFeature.Store();
        //                            }
        //                            else
        //                            {
        //                                MessageBoxEx.Show("格网码生成失败，请转换成地理坐标！");
        //                                return;
        //                            }
        //                        }
        //                        //名字有相同的情况
        //                        else
        //                        {
        //                            List<IFeature> keyList = (from q in pNameNotNull
        //                                                      where q.Value == pFeatureName2
        //                                                      select q.Key).ToList<IFeature>(); //get all keys
        //                            //如果改要素就是第一条要素，则正常进行格网编码
        //                            if (pFeature.OID == keyList[0].OID)
        //                            {
        //                                CreatGridCodeComm(pFeature, pFeature, strField);

        //                                #region 注释8
        //                                ////获取格网信息
        //                                //string GridCode = GetCodeString(pFeature);
        //                                //if (GridCode != "")
        //                                //{
        //                                //    pFeature.set_Value(pFeature.Fields.FindField(strField), GridCode);
        //                                //    pFeature.Store();
        //                                //}
        //                                //else
        //                                //{
        //                                //    MessageBoxEx.Show("格网码生成失败，请转换成地理坐标！");
        //                                //    return;
        //                                //}
        //                                #endregion
        //                            }
        //                            //如果改要素不是第一条要素，则按照第一条要素进行格网编码
        //                            if (pFeature.OID != keyList[0].OID)
        //                            {

        //                                CreatGridCodeComm(pFeature, keyList[0], strField);
        //                                #region 注释9
        //                                ////获取格网信息
        //                                //string GridCode = GetCodeString(keyList[0]);
        //                                //if (GridCode != "")
        //                                //{
        //                                //    pFeature.set_Value(pFeature.Fields.FindField(strField), GridCode);
        //                                //    pFeature.Store();
        //                                //}
        //                                //else
        //                                //{
        //                                //    MessageBoxEx.Show("格网码生成失败，请转换成地理坐标！");
        //                                //    return;
        //                                //}
        //                                #endregion

        //                            }
        //                        }
        //                    }
        //                }
        //                pFeature = pFeatureCursor.NextFeature();
        //            }

        //            pWorkspaceEdit.StopEditing(true);
        //            pWorkspaceEdit.StopEditOperation();
        //            MessageBoxEx.Show("格网生成成功！");

        //        }
        //    }
        //    else
        //    {
        //        MessageBoxEx.Show("没有选中任何图层！");
        //    }
        //}

        /// <summary>
        /// 生成格网码(道路)
        /// </summary>
        /// <param name="pMapControl"></param>
        /// <param name="cbxLayerName"></param>
        public void CreatGridCodeRoad2(IFeatureLayer pFeatureLayer,Dictionary<IFeature, string> pNameNotNull, Dictionary<IFeature, string> pRoadNotNull, List<object> pOID)
        {
            string strField = "GridCode";
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
                    //int i = pFeatureLayer.FeatureClass.FeatureCount(null);

                    IFeature pFeature = pFeatureCursor.NextFeature();

                    //遍历所有Feature，查找属于同一个地理实体的图元
                    while (pFeature != null)
                    {
                        if (pOID.Contains(pFeature.OID))
                        {

                            //名称不为空
                            if (pFeature.get_Value(pFeature.Fields.FindField(ClsConfig.LayerConfigs[(pFeatureLayer.FeatureClass as IDataset).Name].NameField)).ToString().Trim().Length > 0)
                            {
                                string pFeatureName2 = pFeature.get_Value(pFeature.Fields.FindField(ClsConfig.LayerConfigs[(pFeatureLayer.FeatureClass as IDataset).Name].NameField)).ToString();
                                int flag2 = 0;
                                foreach (string s in pNameNotNull.Values)
                                {
                                    if (pFeatureName2 == s)
                                    {
                                        flag2++;
                                    }
                                }
                                //名字没有相同的情况
                                if (flag2 == 1)
                                {
                                    //获取格网信息
                                    string pGridCode3 = pClsCom.GetCodeString(pFeature);
                                    if (pGridCode3 != "")
                                    {
                                        pFeature.set_Value(pFeature.Fields.FindField(strField), pGridCode3);
                                        pFeature.Store();
                                    }
                                    else
                                    {
                                        MessageBoxEx.Show("格网码生成失败，请转换成地理坐标！");
                                        return;
                                    }
                                }
                                //名字有相同的情况
                                else
                                {
                                    List<IFeature> keyList = (from q in pNameNotNull
                                                              where q.Value == pFeatureName2
                                                              select q.Key).ToList<IFeature>(); //get all keys
                                    //如果改要素就是第一条要素，则正常进行格网编码
                                    if (pFeature.OID == keyList[0].OID)
                                    {
                                        CreatGridCodeComm(pFeature, pFeature, strField);

                                        #region 注释8
                                        ////获取格网信息
                                        //string GridCode = GetCodeString(pFeature);
                                        //if (GridCode != "")
                                        //{
                                        //    pFeature.set_Value(pFeature.Fields.FindField(strField), GridCode);
                                        //    pFeature.Store();
                                        //}
                                        //else
                                        //{
                                        //    MessageBoxEx.Show("格网码生成失败，请转换成地理坐标！");
                                        //    return;
                                        //}
                                        #endregion
                                    }
                                    //如果改要素不是第一条要素，则按照第一条要素进行格网编码
                                    if (pFeature.OID != keyList[0].OID)
                                    {

                                        CreatGridCodeComm(pFeature, keyList[0], strField);
                                        #region 注释9
                                        ////获取格网信息
                                        //string GridCode = GetCodeString(keyList[0]);
                                        //if (GridCode != "")
                                        //{
                                        //    pFeature.set_Value(pFeature.Fields.FindField(strField), GridCode);
                                        //    pFeature.Store();
                                        //}
                                        //else
                                        //{
                                        //    MessageBoxEx.Show("格网码生成失败，请转换成地理坐标！");
                                        //    return;
                                        //}
                                        #endregion

                                    }
                                }
                            }

                            //路线编码不为空,名称为空
                            if (pFeature.get_Value(pFeature.Fields.FindField("ROADCODE")).ToString().Trim().Length > 1 && pFeature.get_Value(pFeature.Fields.FindField(ClsConfig.LayerConfigs[(pFeatureLayer.FeatureClass as IDataset).Name].NameField)).ToString().Trim().Length == 0)
                            {
                                string pRoadCode2 = pFeature.get_Value(pFeature.Fields.FindField("ROADCODE")).ToString();

                                int flag1 = 0;

                                foreach (string s in pRoadNotNull.Values)
                                {
                                    if (pRoadCode2 == s)
                                    {
                                        flag1++;
                                    }
                                }
                                //名字只出现一次的情况
                                if (flag1 == 1)
                                {
                                    //获取格网信息
                                    string pGridCode2 = pClsCom.GetCodeString(pFeature);
                                    if (pGridCode2 != "")
                                    {
                                        pFeature.set_Value(pFeature.Fields.FindField(strField), pGridCode2);
                                        pFeature.Store();
                                    }
                                    else
                                    {
                                        MessageBoxEx.Show("格网码生成失败，请转换成地理坐标！");
                                        return;
                                    }
                                }
                                //名字出现多次的情况
                                else
                                {
                                    List<IFeature> keyList = (from q in pRoadNotNull
                                                              where q.Value == pRoadCode2
                                                              select q.Key).ToList<IFeature>(); //get all keys
                                    if (pFeature.OID == keyList[0].OID)
                                    {
                                        CreatGridCodeComm(pFeature, pFeature, strField);

                                        #region 注释5
                                        //////test
                                        ////获取格网信息
                                        //string GridCode = GetCodeString(pFeature);
                                        //if (GridCode != "")
                                        //{
                                        //    pFeature.set_Value(pFeature.Fields.FindField(strField), GridCode);
                                        //    pFeature.Store();
                                        //}
                                        //else
                                        //{
                                        //    MessageBoxEx.Show("格网码生成失败，请转换成地理坐标！");
                                        //    return;
                                        //}
                                        #endregion
                                    }
                                    if (pFeature.OID != keyList[0].OID)
                                    {
                                        CreatGridCodeComm(pFeature, keyList[0], strField);

                                        #region 注释6
                                        ////获取格网信息
                                        //string GridCode = GetCodeString(keyList[0]);
                                        //if (GridCode != "")
                                        //{
                                        //    pFeature.set_Value(pFeature.Fields.FindField(strField), GridCode);
                                        //    pFeature.Store();
                                        //}
                                        //else
                                        //{
                                        //    MessageBoxEx.Show("格网码生成失败，请转换成地理坐标！");
                                        //    return;
                                        //}
                                        #endregion
                                    }
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
        public void CreatGridCodeComm(IFeature pFeature, IFeature pFeature2, string pGridCode)
        {
            //获取格网信息
            string GridCode = pClsCom.GetCodeString(pFeature2);

            if (GridCode != "")
            {
                pFeature.set_Value(pFeature.Fields.FindField(pGridCode), GridCode);
                pFeature.Store();
            }
            else
            {
                MessageBoxEx.Show("格网码生成失败，请转换成地理坐标！");
                return;
            }
        }


        /// <summary>
        /// 对道路进行编码
        /// </summary>
        /// <param name="pMapControl"></param>
        /// <param name="cbxLayerName"></param>
        public void RoadCode(IMap pMapControl, ComboBoxEx cbxLayerName)
        {
            IFeatureLayer pFeatureLayer = (IFeatureLayer)pClsCom.GetLayerByName(pMapControl, cbxLayerName.Text);
            IFields pFields = pFeatureLayer.FeatureClass.Fields;
            Dictionary<IFeature, string> pDicGridCode = new Dictionary<IFeature, string>();
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
                while (pFeature != null)
                {
                    if (pFeature.get_Value(pFeature.Fields.FindField("GridCode")).ToString().Length > 0)
                    {
                        pDicGridCode.Add(pFeature, pFeature.get_Value(pFeature.Fields.FindField("GridCode")).ToString());
                    }
                    pFeature = pFeatureCursor.NextFeature();
                }
                pWorkspaceEdit.StopEditing(true);
                pWorkspaceEdit.StopEditOperation();

            }

            CreatRoadCode2(pFeatureLayer, pDicGridCode, "FCODE", "GridCode", "ENTIID");

            #region 20170609注释掉
            //for (int i = 0; i < pFields.FieldCount - 1; i++)
            //{
            //    pField = pFields.get_Field(i);

            //    #region 只针对路线编码来讲
            //    //if (pField.AliasName.Contains("路线编码"))
            //    //{
            //    //    //遍历Feature
            //    //    IDataset pDataset = pFeatureLayer.FeatureClass as IDataset;
            //    //    IWorkspaceEdit pWorkspaceEdit = null;
            //    //    if (pDataset != null)
            //    //    {
            //    //        pWorkspaceEdit = pDataset.Workspace as IWorkspaceEdit;
            //    //        if (pWorkspaceEdit != null || pWorkspaceEdit.IsBeingEdited() == false)
            //    //        {
            //    //            pWorkspaceEdit.StartEditing(true);
            //    //            pWorkspaceEdit.StartEditOperation();
            //    //        }
            //    //        IFeatureCursor pFeatureCursor = pFeatureLayer.Search(null, false);

            //    //        //test
            //    //        int test = pFeatureLayer.FeatureClass.FeatureCount(null);
            //    //        IFeature pFeature = pFeatureCursor.NextFeature();


            //    //        while (pFeature != null)
            //    //        {

            //    //            //获取单条Feature的某个字段值
            //    //            int test2 = pFeature.Fields.FindFieldByAliasName("路线编码");
            //    //            string pRoad = pFeature.get_Value(pFeature.Fields.FindFieldByAliasName("路线编码")).ToString();


            //    //            if (pRoad.Length > 1 && pRoad.Length < 6)
            //    //            {
            //    //                if (pRoad.Length == 5)
            //    //                {
            //    //                    pFeature.set_Value(pFeature.Fields.FindField("ENTIID"), pRoad + 330521);
            //    //                    pFeature.Store();
            //    //                }
            //    //                else
            //    //                {
            //    //                    string _Count = null;
            //    //                    for (int k = 0; k < 5 - pRoad.Length; k++)
            //    //                    {
            //    //                        _Count += "_";
            //    //                    }
            //    //                    pRoad = pRoad + _Count;
            //    //                    pFeature.set_Value(pFeature.Fields.FindField("ENTIID"), pRoad + 330521);
            //    //                    pFeature.Store();
            //    //                }

            //    //            }


            //    //            pFeature = pFeatureCursor.NextFeature();
            //    //        }
            //    //        pWorkspaceEdit.StopEditing(true);
            //    //        pWorkspaceEdit.StopEditOperation();

            //    //    }
            //    //}
            //    #endregion

            //    if (pField.AliasName.Contains("路线编码"))
            //    {
            //        IFields pFieldsInner1 = pFeatureLayer.FeatureClass.Fields;
            //        for (int j = 0; j < pFieldsInner1.FieldCount - 1; j++)
            //        {
            //            IField pFieldInner1 = pFieldsInner1.get_Field(j);

            //            if (pFieldInner1.Name.Contains("ENTIID"))
            //            {
            //                //CreatRoadCode(pFeatureLayer, pField, "路线编码", "ENTIID");
            //                CreatRoadCode2(pFeatureLayer, "FCODE","GridCode","ENTIID");
            //            }

            //        }


            //    }

            //    #region 地理实体编码234
            //    //if (pField.AliasName.Contains("路线编码2"))
            //    //{
            //    //    IFields pFieldsInner1 = pFeatureLayer.FeatureClass.Fields;
            //    //    for (int j = 0; j < pFieldsInner1.FieldCount - 1; j++)
            //    //    {
            //    //        IField pFieldInner1 = pFieldsInner1.get_Field(j);

            //    //        if (pFieldInner1.Name.Contains("ENTIID2"))
            //    //        {
            //    //            //CreatRoadCode(pFeatureLayer, pField, "路线编码2", "ENTIID2");
            //    //            CreatRoadCode2(pFeatureLayer, "FCODE", "GridCode", "ENTIID2");

            //    //        }
            //    //    }
            //    //}
            //    //if (pField.AliasName.Contains("路线编码3"))
            //    //{
            //    //    IFields pFieldsInner1 = pFeatureLayer.FeatureClass.Fields;
            //    //    for (int j = 0; j < pFieldsInner1.FieldCount - 1; j++)
            //    //    {
            //    //        IField pFieldInner1 = pFieldsInner1.get_Field(j);

            //    //        if (pFieldInner1.Name.Contains("ENTIID2"))
            //    //        {
            //    //            //CreatRoadCode(pFeatureLayer, pField, "路线编码3", "ENTIID3");
            //    //            CreatRoadCode2(pFeatureLayer, "FCODE", "GridCode", "ENTIID2");

            //    //        }
            //    //    }
            //    //}
            //    //if (pField.AliasName.Contains("路线编码4"))
            //    //{
            //    //    IFields pFieldsInner1 = pFeatureLayer.FeatureClass.Fields;
            //    //    for (int j = 0; j < pFieldsInner1.FieldCount - 1; j++)
            //    //    {
            //    //        IField pFieldInner1 = pFieldsInner1.get_Field(j);

            //    //        if (pFieldInner1.Name.Contains("ENTIID4"))
            //    //        {
            //    //            //CreatRoadCode(pFeatureLayer, pField, "路线编码4", "ENTIID4");
            //    //            CreatRoadCode2(pFeatureLayer, "FCODE", "GridCode", "ENTIID4");

            //    //        }
            //    //    }
            //    //}
            //    #endregion
            //}
            #endregion

            MessageBox.Show("道路编码成功！");
        }

        /// <summary>
        /// 道路编码函数
        /// </summary>
        /// <param name="pFeatureLayer"></param>
        /// <param name="pField"></param>
        /// <param name="pRoadCode"></param>
        /// <param name="pENEIID"></param>
        private void CreatRoadCode2(IFeatureLayer pFeatureLayer, Dictionary<IFeature, string> pDGCode, string pFCODE, string pGridCode, string pENEIID)
        {
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

                    if (pFeature.get_Value(pFeature.Fields.FindField(pGridCode)).ToString().Length > 0)
                    {

                        foreach (string s in pDGCode.Values)
                        {
                            if (gridCode1 == s)
                            {
                                flag++;
                            }
                        }

                        //格网码唯一的情况：那么地理编码也一定唯一
                        if (flag == 1)
                        {
                            //分类码
                            int pFCodeIndex = pFeature.Fields.FindField(pFCODE);
                            if (pFCodeIndex != -1)
                            {
                                string pFCode = pFeature.get_Value(pFCodeIndex).ToString();
                                //处理分类码
                                //ClsReturnFCode pReturnFcode = new ClsReturnFCode();
                                //pFCode = pReturnFcode.ReturnFeatureClass(pFCode);
                                pFCode = ZJGISCommon.Classes.ClsFcode.pDicFcodeGlobal[pFCode];
                                if (pFCode != null)
                                {
                                    pNum = "A01";
                                    pEntiidCode = gridCode1 + pFCode + pNum;
                                    pFeature.set_Value(pFeature.Fields.FindField(pENEIID), pEntiidCode);
                                    pFeature.Store();
                                }
                            }
                        }
                        //格网码有多个的情况：那么地理编码也一定唯一
                        else
                        {
                            List<IFeature> keyListTotal = (from q in pDGCode
                                                           where q.Value == gridCode1
                                                           select q.Key).ToList<IFeature>(); //get all keys


                            //test
                            object testobj = pFeature.OID;
                            //顺序码
                            string pFeatureName = pFeature.get_Value(pFeature.Fields.FindField(ClsConfig.LayerConfigs[(pFeatureLayer.FeatureClass as IDataset).Name].NameField)).ToString().Trim();
                            for (int k = 0; k < keyListTotal.Count; k++)
                            {
                                string pkeyListTotalNamek = keyListTotal[k].get_Value(keyListTotal[k].Fields.FindField(ClsConfig.LayerConfigs[(pFeatureLayer.FeatureClass as IDataset).Name].NameField)).ToString().Trim();
                                if (pFeatureName == pkeyListTotalNamek)
                                {
                                    pNum = "A" + string.Format("{0:00}", k + 1);
                                }

                            }

                            //分类码
                            int pFcodeindex = pFeature.Fields.FindField(pFCODE);
                            if (pFcodeindex != -1)
                            {
                                string pFCode = pFeature.get_Value(pFcodeindex).ToString();

                                //处理分类码
                                //ClsReturnFCode pReturnFcode = new ClsReturnFCode();
                                //pFCode = pReturnFcode.ReturnFeatureClass(pFCode);
                                pFCode = ZJGISCommon.Classes.ClsFcode.pDicFcodeGlobal[pFCode];
                                if (pFCode != null)
                                {
                                    pEntiidCode = gridCode1 + pFCode + pNum;
                                    pFeature.set_Value(pFeature.Fields.FindField(pENEIID), pEntiidCode);
                                    pFeature.Store();
                                }
                            }


                            #region  20170614注释掉
                            //if (pFeature.OID == keyListTotal[0].OID)
                            //{
                            //    //分类码
                            //    int pFcodeindex = pFeature.Fields.FindField(pFCODE);
                            //    if (pFcodeindex != -1)
                            //    {
                            //        string pFCode = pFeature.get_Value(pFcodeindex).ToString();

                            //        //处理分类码
                            //        ClsReturnFCode pReturnFcode = new ClsReturnFCode();
                            //        pFCode = pReturnFcode.ReturnFeatureClass(pFCode);
                            //        if (pFCode != null)
                            //        {
                            //            string gridCode = pFeature.get_Value(pFeature.Fields.FindField(pGridCode)).ToString();

                            //            if (gridCode.Trim().Length > 0)
                            //            {
                            //                pNum = "A01";
                            //                pEntiidCode = gridCode + pFCode + pNum;

                            //                pFeature.set_Value(pFeature.Fields.FindField(pENEIID), pEntiidCode);
                            //                pFeature.Store();
                            //            }
                            //        }
                            //    }



                            //}

                            //if (pFeature.OID != keyListTotal[0].OID)
                            //{
                            //    string pFeatureName = pFeature.get_Value(pFeature.Fields.FindField("NAME")).ToString().Trim();
                            //    string pkeyListTotalName0 = keyListTotal[0].get_Value(keyListTotal[0].Fields.FindField("NAME")).ToString().Trim();
                            //    //前提：格网码相同存在多条要素，如果当前要素和和第一条要素的名称相同，那么第一条要素可以任意编码
                            //    for (int k = 1; k < keyListTotal.Count; k++)
                            //    {
                            //        string pkeyListTotalNamek = keyListTotal[k].get_Value(keyListTotal[k].Fields.FindField("NAME")).ToString().Trim();
                            //        if (pFeatureName == pkeyListTotalNamek)
                            //        {
                            //            pNum = "A" + string.Format("{0:00}", k + 1);

                            //        }

                            //    }


                            //    if (pFeatureName == pkeyListTotalName0)
                            //    {
                            //        //分类码
                            //        int pFcodeindex = keyListTotal[0].Fields.FindField(pFCODE);
                            //        if (pFcodeindex != -1)
                            //        {
                            //            string pFCode = keyListTotal[0].get_Value(pFcodeindex).ToString();

                            //            //处理分类码
                            //            ClsReturnFCode pReturnFcode = new ClsReturnFCode();
                            //            pFCode = pReturnFcode.ReturnFeatureClass(pFCode);
                            //            if (pFCode != null)
                            //            {
                            //                string gridCode = keyListTotal[0].get_Value(keyListTotal[0].Fields.FindField(pGridCode)).ToString();

                            //                if (gridCode.Trim().Length > 0)
                            //                {
                            //                    pNum = "A01";
                            //                    pEntiidCode = gridCode + pFCode + pNum;

                            //                    //if (!pDicAfter.Keys.Contains(j))
                            //                    //{
                            //                    //    pDicAfter.Add(j, pEntiidCode);
                            //                    //}

                            //                    //为当前的Feature设置 keyListTotal[0]的地理编码
                            //                    pFeature.set_Value(pFeature.Fields.FindField(pENEIID), pEntiidCode);
                            //                    pFeature.Store();
                            //                }
                            //            }
                            //        }


                            //    }
                            //    //前提：格网码相同存在多条要素，如果当前要素和和第一条要素的名称不同，那么就是格网码相同，但是不是同一个地理实体的情况
                            //    //下一步比较其分类码是否相同
                            //    else
                            //    {
                            //        string pFeatureFcode = pFeature.get_Value(pFeature.Fields.FindField(pFCODE)).ToString();
                            //        string pkeyListTotalFcode0 = keyListTotal[0].get_Value(keyListTotal[0].Fields.FindField(pFCODE)).ToString();
                            //        //处理分类码
                            //        ClsReturnFCode pReturnFcode = new ClsReturnFCode();
                            //        pFeatureFcode = pReturnFcode.ReturnFeatureClass(pFeatureFcode);
                            //        pkeyListTotalFcode0 = pReturnFcode.ReturnFeatureClass(pkeyListTotalFcode0);
                            //        //如果分类码相同，那么前两个码段相同靠顺序码区分
                            //        if (pFeatureFcode == pkeyListTotalFcode0)
                            //        {
                            //            string gridCode = keyListTotal[0].get_Value(keyListTotal[0].Fields.FindField(pGridCode)).ToString();

                            //            for (int k = 1; k < keyListTotal.Count; k++)
                            //            {
                            //                pNum = "A" + string.Format("{0:00}", k + 1);
                            //            }

                            //            pEntiidCode = gridCode + pFeatureFcode + pNum;

                            //            pFeature.set_Value(pFeature.Fields.FindField(pENEIID), pEntiidCode);
                            //            pFeature.Store();

                            //        }
                            //        //如果分类码不同，那么前两个码段就可以区分出来是否是同一个地理实体
                            //        else
                            //        {
                            //            string gridCode = keyListTotal[0].get_Value(keyListTotal[0].Fields.FindField(pGridCode)).ToString();
                            //            pNum = "A01";
                            //            pEntiidCode = gridCode + pFeatureFcode + pNum;

                            //            //为当前的Feature设置 keyListTotal[0]的地理编码
                            //            pFeature.set_Value(pFeature.Fields.FindField(pENEIID), pEntiidCode);
                            //            pFeature.Store();
                            //        }

                            //    }

                            //}
                            #endregion

                        }
                    }

                    pFeature = pFeatureCursor.NextFeature();
                }
                pWorkspaceEdit.StopEditing(true);
                pWorkspaceEdit.StopEditOperation();
            }
        }


        /// <summary>
        /// 生成格网码(道路)
        /// </summary>
        /// <param name="pMapControl"></param>
        /// <param name="cbxLayerName"></param>
        public void CreatRestRoadGrid(IMap pMapControl, ComboBoxEx cbxLayerName)
        {
            Dictionary<IFeature, string> pBothNotNull = new Dictionary<IFeature, string>();
            Dictionary<IFeature, string> pRoadNotNull = new Dictionary<IFeature, string>();
            Dictionary<IFeature, string> pNameNotNull = new Dictionary<IFeature, string>();
            //Dictionary<IFeature, string> pGridCode = new Dictionary<IFeature, string>();
            List<object> pOID = new List<object>();

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

                    #region 注释掉
                    ////设置查询条件，过滤名称为空的要素
                    //IQueryFilter queryFilter = new QueryFilterClass();
                    ////Setting the SubFields and WhereClause:
                    ////if you are updating the geometry you must include the shapefield in the subfields.
                    ////queryFilter.SubFields = "STATE_NAME,POPULATION";

                    ////注意：此处的FNAME字段只针对于存在改字段的图层，否则此字段需要修改
                    ////queryFilter.SubFields = "FNAME,ENTIID";
                    //queryFilter.SubFields = "FNAME";

                    //if (pDataset.Workspace.Type == esriWorkspaceType.esriFileSystemWorkspace)
                    //{
                    //    queryFilter.WhereClause = "\"" + "FNAME" + "\"" + " !=" + "null"; //shpfile
                    //}
                    //else
                    //{
                    //    queryFilter.WhereClause = "[" + "FNAME" + "]" + " !=" + "null"; //gdb
                    //}



                    ////queryFilter.WhereClause = "STATE_NAME ！= null";
                    ////queryFilter.WhereClause = "FNAME!=null";
                    ////Using a query filter to search a feature class:
                    ////IFeatureCursor featureCursor = featureClass.Search(queryFilter, false);
                    //IFeatureCursor pFeatureCursor = pFeatureLayer.Search(queryFilter, false);
                    ////IFeatureCursor pFeatureCursor = pFeatureLayer.Search(null, false);
                    #endregion

                    IFeatureCursor pFeatureCursor = pFeatureLayer.Search(null, false);

                    //int i = pFeatureLayer.FeatureClass.FeatureCount(null);
                    //int j = 0, k = 0, m = 0;

                    IFeature pFeature = pFeatureCursor.NextFeature();

                    while (pFeature != null)
                    {
                        //名称不为空，地理编码为空
                        if (pFeature.get_Value(pFeature.Fields.FindField(ClsConfig.LayerConfigs[(pFeatureLayer.FeatureClass as IDataset).Name].NameField)).ToString().Trim().Length > 0 &&
                            pFeature.get_Value(pFeature.Fields.FindField(ClsConfig.LayerConfigs[(pFeatureLayer.FeatureClass as IDataset).Name].EntityID)).ToString().Trim().Length == 0)
                        {
                            pNameNotNull.Add(pFeature, pFeature.get_Value(pFeature.Fields.FindField(ClsConfig.LayerConfigs[(pFeatureLayer.FeatureClass as IDataset).Name].NameField)).ToString());
                            pOID.Add(pFeature.OID);
                        }

                        pFeature = pFeatureCursor.NextFeature();
                    }

                    #region  遍历所有Feature，查找属于同一个地理实体的图元
                    //遍历所有Feature，查找属于同一个地理实体的图元
                    //while (pFeature != null)
                    //{
                    //    //路线编码和名称都不为空
                    //    if (pFeature.get_Value(pFeature.Fields.FindField("NAME")).ToString().Trim().Length > 0 && pFeature.get_Value(pFeature.Fields.FindField("ROADCODE")).ToString().Trim().Length > 0)
                    //    {
                    //        if (j == 0)
                    //        {
                    //            //获取格网信息
                    //            string GridCode = GetCodeString(pFeature);
                    //            if (GridCode != "")
                    //            {
                    //                pFeature.set_Value(pFeature.Fields.FindField(strField), GridCode);
                    //                pFeature.Store();
                    //                j++;
                    //                //pFeature = pFeatureCursor.NextFeature();
                    //                //pBothNotNull.Add(pFeature,pFeature.get_Value(pFeature.Fields.FindField("NAME")).ToString());
                    //            }
                    //            else
                    //            {
                    //                MessageBoxEx.Show("格网码生成失败，请转换成地理坐标！");
                    //                return;
                    //            }
                    //        }
                    //        else
                    //        {
                    //            foreach (string s in pBothNotNull.Values)
                    //            {
                    //                if (s == pFeature.get_Value(pFeature.Fields.FindField("NAME")))
                    //                {
                    //                    IFeature pfirstKeyFeature = pBothNotNull.FirstOrDefault(q => q.Value == s).Key;  //get first key
                    //                    if (pfirstKeyFeature.get_Value(pfirstKeyFeature.Fields.FindField("ROADCODE")).ToString() == pFeature.get_Value(pFeature.Fields.FindField("ROADCODE")).ToString())
                    //                    {
                    //                        pFeature.set_Value(pFeature.Fields.FindField(strField), pfirstKeyFeature.get_Value(pfirstKeyFeature.Fields.FindField("GridCode")));
                    //                        pFeature.Store();
                    //                        m++;
                    //                        //pFeature = pFeatureCursor.NextFeature();
                    //                        //pBothNotNull.Add(pFeature, pFeature.get_Value(pFeature.Fields.FindField("NAME")).ToString());
                    //                    } 
                    //                    else
                    //                    {
                    //                        string GridCode = GetCodeString(pFeature);
                    //                        if (GridCode != "")
                    //                        {
                    //                            pFeature.set_Value(pFeature.Fields.FindField(strField), GridCode);
                    //                            pFeature.Store();
                    //                            j++;
                    //                            //pFeature = pFeatureCursor.NextFeature();
                    //                            //pBothNotNull.Add(pFeature, pFeature.get_Value(pFeature.Fields.FindField("NAME")).ToString());
                    //                        }
                    //                        else
                    //                        {
                    //                            MessageBoxEx.Show("格网码生成失败，请转换成地理坐标！");
                    //                            return;
                    //                        }
                    //                    }

                    //                }
                    //                else
                    //                {
                    //                    string GridCode = GetCodeString(pFeature);
                    //                    if (GridCode != "")
                    //                    {
                    //                        pFeature.set_Value(pFeature.Fields.FindField(strField), GridCode);
                    //                        pFeature.Store();
                    //                        j++;
                    //                        //pFeature = pFeatureCursor.NextFeature();
                    //                        //pBothNotNull.Add(pFeature, pFeature.get_Value(pFeature.Fields.FindField("NAME")).ToString());
                    //                    }
                    //                    else
                    //                    {
                    //                        MessageBoxEx.Show("格网码生成失败，请转换成地理坐标！");
                    //                        return;
                    //                    }
                    //                }
                    //            }
                    //        }
                    //    }


                    //    //路线编码不为空,名称为空
                    //    if (pFeature.get_Value(pFeature.Fields.FindField("ROADCODE")).ToString().Trim().Length > 0 && pFeature.get_Value(pFeature.Fields.FindField("NAME")).ToString().Trim().Length == 0)
                    //    {
                    //        if (k == 0)
                    //        {
                    //            //获取格网信息
                    //            string GridCode = GetCodeString(pFeature);
                    //            if (GridCode != "")
                    //            {
                    //                pFeature.set_Value(pFeature.Fields.FindField(strField), GridCode);
                    //                pFeature.Store();
                    //                k++;
                    //                //pFeature = pFeatureCursor.NextFeature();
                    //                //pNameNull.Add(pFeature, pFeature.get_Value(pFeature.Fields.FindField("ROADCODE")).ToString());

                    //            }
                    //            else
                    //            {
                    //                MessageBoxEx.Show("格网码生成失败，请转换成地理坐标！");
                    //                return;
                    //            }
                    //        }
                    //        else
                    //        {
                    //            foreach (string s in pNameNull.Values)
                    //            {
                    //                //test
                    //                string test = pFeature.get_Value(pFeature.Fields.FindField("ROADCODE")).ToString();

                    //                if (s == pFeature.get_Value(pFeature.Fields.FindField("ROADCODE")).ToString())
                    //                {
                    //                    IFeature pfirstKeyFeature = pEntiIDNull.FirstOrDefault(q => q.Value == s).Key;  //get first key
                    //                    pFeature.set_Value(pFeature.Fields.FindField(strField), pfirstKeyFeature.get_Value(pfirstKeyFeature.Fields.FindField("ROADCODE")));
                    //                    pFeature.Store();
                    //                    k++;
                    //                    //pFeature = pFeatureCursor.NextFeature();
                    //                    //pNameNull.Add(pFeature, pFeature.get_Value(pFeature.Fields.FindField("ROADCODE")).ToString());
                    //                }
                    //                else
                    //                {
                    //                    //获取格网信息
                    //                    string GridCode = GetCodeString(pFeature);
                    //                    if (GridCode != "")
                    //                    {
                    //                        pFeature.set_Value(pFeature.Fields.FindField(strField), GridCode);
                    //                        pFeature.Store();
                    //                        k++;
                    //                        //pFeature = pFeatureCursor.NextFeature();
                    //                        //pNameNull.Add(pFeature, pFeature.get_Value(pFeature.Fields.FindField("ROADCODE")).ToString());

                    //                    }
                    //                    else
                    //                    {
                    //                        MessageBoxEx.Show("格网码生成失败，请转换成地理坐标！");
                    //                        return;
                    //                    }
                    //                }
                    //            }

                    //        }

                    //    }


                    //    //名称不为空，路线编码为空
                    //    if (pFeature.get_Value(pFeature.Fields.FindField("NAME")).ToString().Trim().Length > 0 && pFeature.get_Value(pFeature.Fields.FindField("ROADCODE")).ToString().Trim().Length == 0)
                    //    {
                    //        if (m == 0)
                    //        {
                    //            //获取格网信息
                    //            string GridCode = GetCodeString(pFeature);
                    //            if (GridCode != "")
                    //            {
                    //                pFeature.set_Value(pFeature.Fields.FindField(strField), GridCode);
                    //                pFeature.Store();
                    //                m++;
                    //                //pFeature = pFeatureCursor.NextFeature();
                    //                //pEntiIDNull.Add(pFeature, pFeature.get_Value(pFeature.Fields.FindField("NAME")).ToString());
                    //            }
                    //            else
                    //            {
                    //                MessageBoxEx.Show("格网码生成失败，请转换成地理坐标！");
                    //                return;
                    //            }
                    //        }
                    //        else
                    //        {
                    //            foreach (string s in pEntiIDNull.Values)
                    //            {
                    //                //test
                    //                string test = pFeature.get_Value(pFeature.Fields.FindField("NAME")).ToString();

                    //                if (s == pFeature.get_Value(pFeature.Fields.FindField("NAME")).ToString())
                    //                {
                    //                    IFeature pfirstKeyFeature = pEntiIDNull.FirstOrDefault(q => q.Value == s).Key;  //get first key

                    //                    pFeature.set_Value(pFeature.Fields.FindField(strField), pfirstKeyFeature.get_Value(pfirstKeyFeature.Fields.FindField("GridCode")));
                    //                    pFeature.Store();
                    //                    m++;
                    //                    //pFeature = pFeatureCursor.NextFeature();
                    //                    //pEntiIDNull.Add(pFeature, pFeature.get_Value(pFeature.Fields.FindField("NAME")).ToString());
                    //                }
                    //                else
                    //                {
                    //                    //获取格网信息
                    //                    string GridCode = GetCodeString(pFeature);
                    //                    if (GridCode != "")
                    //                    {
                    //                        pFeature.set_Value(pFeature.Fields.FindField(strField), GridCode);
                    //                        pFeature.Store();
                    //                        m++;
                    //                        //pFeature = pFeatureCursor.NextFeature();
                    //                        //pEntiIDNull.Add(pFeature, pFeature.get_Value(pFeature.Fields.FindField("NAME")).ToString());

                    //                    }
                    //                    else
                    //                    {
                    //                        MessageBoxEx.Show("格网码生成失败，请转换成地理坐标！");
                    //                        return;
                    //                    }
                    //                }
                    //            }
                    //        }
                    //    }
                    //    pFeature = pFeatureCursor.NextFeature();
                    //}
                    #endregion

                    pWorkspaceEdit.StopEditing(true);
                    pWorkspaceEdit.StopEditOperation();

                }
            }
            else
            {
                MessageBoxEx.Show("没有选中任何图层！");
            }
            CreatRestRoadGrid2(pFeatureLayer, pNameNotNull, pOID);
        }



        /// <summary>
        /// 补全格网
        /// </summary>
        /// <param name="pMapControl"></param>
        /// <param name="cbxLayerName"></param>
        public void CreatRestRoadGrid2(IFeatureLayer pFeatureLayer, Dictionary<IFeature, string> pNameNotNull, List<object> pOID)
        {
            string strField = "GridCode";

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

                    #region 注释掉
                    ////设置查询条件，过滤名称为空的要素
                    //IQueryFilter queryFilter = new QueryFilterClass();
                    ////Setting the SubFields and WhereClause:
                    ////if you are updating the geometry you must include the shapefield in the subfields.
                    ////queryFilter.SubFields = "STATE_NAME,POPULATION";
                    //queryFilter.SubFields = "FNAME,ENTIID";
                    ////queryFilter.WhereClause = "STATE_NAME ！= null";
                    //queryFilter.WhereClause = "FNAME!=null";
                    ////Using a query filter to search a feature class:
                    ////IFeatureCursor featureCursor = featureClass.Search(queryFilter, false);
                    //IFeatureCursor pFeatureCursor = pFeatureLayer.Search(queryFilter, false);
                    #endregion
                    IFeatureCursor pFeatureCursor = pFeatureLayer.Search(null, false);

                    int i = pFeatureLayer.FeatureClass.FeatureCount(null);
                    int j = 0;

                    IFeature pFeature = pFeatureCursor.NextFeature();

                    while (pFeature != null)
                    {
                        //对于名称为空的字段进行筛选
                        //test
                        string test = pFeature.get_Value(pFeature.Fields.FindField(ClsConfig.LayerConfigs[(pFeatureLayer.FeatureClass as IDataset).Name].NameField)).ToString();

                        //名称不为空，路线编码为空
                        if (pOID.Contains(pFeature.OID))
                        {
                            string pFeatureName2 = pFeature.get_Value(pFeature.Fields.FindField(ClsConfig.LayerConfigs[(pFeatureLayer.FeatureClass as IDataset).Name].NameField)).ToString();
                            int flag2 = 0;
                            foreach (string s in pNameNotNull.Values)
                            {
                                if (pFeatureName2 == s)
                                {
                                    flag2++;
                                }
                            }
                            //名字没有相同的情况
                            if (flag2 == 1)
                            {
                                //获取格网信息
                                string pGridCode3 = pClsCom.GetCodeString(pFeature);
                                if (pGridCode3 != "")
                                {
                                    pFeature.set_Value(pFeature.Fields.FindField(strField), pGridCode3);
                                    pFeature.Store();
                                }
                                else
                                {
                                    MessageBoxEx.Show("格网码生成失败，请转换成地理坐标！");
                                    return;
                                }
                            }
                            //名字有相同的情况
                            else
                            {
                                List<IFeature> keyList = (from q in pNameNotNull
                                                          where q.Value == pFeatureName2
                                                          select q.Key).ToList<IFeature>(); //get all keys
                                //如果该要素就是第一条要素，则正常进行格网编码
                                if (pFeature.OID == keyList[0].OID)
                                {
                                    CreatGridCodeComm(pFeature, pFeature, strField);

                                    #region 注释8
                                    ////获取格网信息
                                    //string GridCode = GetCodeString(pFeature);
                                    //if (GridCode != "")
                                    //{
                                    //    pFeature.set_Value(pFeature.Fields.FindField(strField), GridCode);
                                    //    pFeature.Store();
                                    //}
                                    //else
                                    //{
                                    //    MessageBoxEx.Show("格网码生成失败，请转换成地理坐标！");
                                    //    return;
                                    //}
                                    #endregion
                                }
                                //如果改要素不是第一条要素，则按照第一条要素进行格网编码
                                if (pFeature.OID != keyList[0].OID)
                                {

                                    CreatGridCodeComm(pFeature, keyList[0], strField);
                                    #region 注释9
                                    ////获取格网信息
                                    //string GridCode = GetCodeString(keyList[0]);
                                    //if (GridCode != "")
                                    //{
                                    //    pFeature.set_Value(pFeature.Fields.FindField(strField), GridCode);
                                    //    pFeature.Store();
                                    //}
                                    //else
                                    //{
                                    //    MessageBoxEx.Show("格网码生成失败，请转换成地理坐标！");
                                    //    return;
                                    //}
                                    #endregion

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
        /// 对道路进行编码
        /// </summary>
        /// <param name="pMapControl"></param>
        /// <param name="cbxLayerName"></param>
        public void CreatRestRoadCode(IMap pMapControl, ComboBoxEx cbxLayerName)
        {
            IFeatureLayer pFeatureLayer = (IFeatureLayer)pClsCom.GetLayerByName(pMapControl, cbxLayerName.Text);
            IFields pFields = pFeatureLayer.FeatureClass.Fields;

            Dictionary<IFeature, string> pDicGridCode = new Dictionary<IFeature, string>();
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
                while (pFeature != null)
                {
                    if (pFeature.get_Value(pFeature.Fields.FindField("GridCode")).ToString().Length > 0)
                    {
                        pDicGridCode.Add(pFeature, pFeature.get_Value(pFeature.Fields.FindField("GridCode")).ToString());
                    }
                    pFeature = pFeatureCursor.NextFeature();
                }
                pWorkspaceEdit.StopEditing(true);
                pWorkspaceEdit.StopEditOperation();

            }

            CreatRestRoadCode2(pFeatureLayer, pDicGridCode, "FCODE", "GridCode", "ENTIID");

            #region 20170609注释掉
            //for (int i = 0; i < pFields.FieldCount - 1; i++)
            //{
            //    pField = pFields.get_Field(i);

            //    #region 只针对路线编码来讲
            //    //if (pField.AliasName.Contains("路线编码"))
            //    //{
            //    //    //遍历Feature
            //    //    IDataset pDataset = pFeatureLayer.FeatureClass as IDataset;
            //    //    IWorkspaceEdit pWorkspaceEdit = null;
            //    //    if (pDataset != null)
            //    //    {
            //    //        pWorkspaceEdit = pDataset.Workspace as IWorkspaceEdit;
            //    //        if (pWorkspaceEdit != null || pWorkspaceEdit.IsBeingEdited() == false)
            //    //        {
            //    //            pWorkspaceEdit.StartEditing(true);
            //    //            pWorkspaceEdit.StartEditOperation();
            //    //        }
            //    //        IFeatureCursor pFeatureCursor = pFeatureLayer.Search(null, false);

            //    //        //test
            //    //        int test = pFeatureLayer.FeatureClass.FeatureCount(null);
            //    //        IFeature pFeature = pFeatureCursor.NextFeature();


            //    //        while (pFeature != null)
            //    //        {

            //    //            //获取单条Feature的某个字段值
            //    //            int test2 = pFeature.Fields.FindFieldByAliasName("路线编码");
            //    //            string pRoad = pFeature.get_Value(pFeature.Fields.FindFieldByAliasName("路线编码")).ToString();


            //    //            if (pRoad.Length > 1 && pRoad.Length < 6)
            //    //            {
            //    //                if (pRoad.Length == 5)
            //    //                {
            //    //                    pFeature.set_Value(pFeature.Fields.FindField("ENTIID"), pRoad + 330521);
            //    //                    pFeature.Store();
            //    //                }
            //    //                else
            //    //                {
            //    //                    string _Count = null;
            //    //                    for (int k = 0; k < 5 - pRoad.Length; k++)
            //    //                    {
            //    //                        _Count += "_";
            //    //                    }
            //    //                    pRoad = pRoad + _Count;
            //    //                    pFeature.set_Value(pFeature.Fields.FindField("ENTIID"), pRoad + 330521);
            //    //                    pFeature.Store();
            //    //                }

            //    //            }


            //    //            pFeature = pFeatureCursor.NextFeature();
            //    //        }
            //    //        pWorkspaceEdit.StopEditing(true);
            //    //        pWorkspaceEdit.StopEditOperation();

            //    //    }
            //    //}
            //    #endregion

            //    if (pField.AliasName.Contains("路线编码"))
            //    {
            //        IFields pFieldsInner1 = pFeatureLayer.FeatureClass.Fields;
            //        for (int j = 0; j < pFieldsInner1.FieldCount - 1; j++)
            //        {
            //            IField pFieldInner1 = pFieldsInner1.get_Field(j);

            //            if (pFieldInner1.Name.Contains("ENTIID"))
            //            {
            //                //CreatRoadCode(pFeatureLayer, pField, "路线编码", "ENTIID");
            //                CreatRoadCode2(pFeatureLayer, "FCODE","GridCode","ENTIID");
            //            }

            //        }


            //    }

            //    #region 地理实体编码234
            //    //if (pField.AliasName.Contains("路线编码2"))
            //    //{
            //    //    IFields pFieldsInner1 = pFeatureLayer.FeatureClass.Fields;
            //    //    for (int j = 0; j < pFieldsInner1.FieldCount - 1; j++)
            //    //    {
            //    //        IField pFieldInner1 = pFieldsInner1.get_Field(j);

            //    //        if (pFieldInner1.Name.Contains("ENTIID2"))
            //    //        {
            //    //            //CreatRoadCode(pFeatureLayer, pField, "路线编码2", "ENTIID2");
            //    //            CreatRoadCode2(pFeatureLayer, "FCODE", "GridCode", "ENTIID2");

            //    //        }
            //    //    }
            //    //}
            //    //if (pField.AliasName.Contains("路线编码3"))
            //    //{
            //    //    IFields pFieldsInner1 = pFeatureLayer.FeatureClass.Fields;
            //    //    for (int j = 0; j < pFieldsInner1.FieldCount - 1; j++)
            //    //    {
            //    //        IField pFieldInner1 = pFieldsInner1.get_Field(j);

            //    //        if (pFieldInner1.Name.Contains("ENTIID2"))
            //    //        {
            //    //            //CreatRoadCode(pFeatureLayer, pField, "路线编码3", "ENTIID3");
            //    //            CreatRoadCode2(pFeatureLayer, "FCODE", "GridCode", "ENTIID2");

            //    //        }
            //    //    }
            //    //}
            //    //if (pField.AliasName.Contains("路线编码4"))
            //    //{
            //    //    IFields pFieldsInner1 = pFeatureLayer.FeatureClass.Fields;
            //    //    for (int j = 0; j < pFieldsInner1.FieldCount - 1; j++)
            //    //    {
            //    //        IField pFieldInner1 = pFieldsInner1.get_Field(j);

            //    //        if (pFieldInner1.Name.Contains("ENTIID4"))
            //    //        {
            //    //            //CreatRoadCode(pFeatureLayer, pField, "路线编码4", "ENTIID4");
            //    //            CreatRoadCode2(pFeatureLayer, "FCODE", "GridCode", "ENTIID4");

            //    //        }
            //    //    }
            //    //}
            //    #endregion
            //}
            #endregion

            MessageBox.Show("道路编码成功！");
        }


        /// <summary>
        /// 道路编码函数
        /// </summary>
        /// <param name="pFeatureLayer"></param>
        /// <param name="pField"></param>
        /// <param name="pRoadCode"></param>
        /// <param name="pENEIID"></param>
        private void CreatRestRoadCode2(IFeatureLayer pFeatureLayer, Dictionary<IFeature, string> pDGCode, string pFCODE, string pGridCode, string pENEIID)
        {
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

                    if (pFeature.get_Value(pFeature.Fields.FindField(pGridCode)).ToString().Length > 0)
                    {

                        foreach (string s in pDGCode.Values)
                        {
                            if (gridCode1 == s)
                            {
                                flag++;
                            }
                        }

                        //格网码唯一的情况：那么地理编码也一定唯一
                        if (flag == 1)
                        {
                            //分类码
                            int pFCodeIndex = pFeature.Fields.FindField(pFCODE);
                            if (pFCodeIndex != -1)
                            {
                                string pFCode = pFeature.get_Value(pFCodeIndex).ToString();
                                //处理分类码
                                //ClsReturnFCode pReturnFcode = new ClsReturnFCode();
                                //pFCode = pReturnFcode.ReturnFeatureClass(pFCode);
                                pFCode = ZJGISCommon.Classes.ClsFcode.pDicFcodeGlobal[pFCode];
                                if (pFCode != null)
                                {
                                    pNum = "A01";
                                    pEntiidCode = gridCode1 + pFCode + pNum;
                                    pFeature.set_Value(pFeature.Fields.FindField(pENEIID), pEntiidCode);
                                    pFeature.Store();
                                }
                            }
                        }
                        //格网码有多个的情况
                        else
                        {
                            List<IFeature> keyListTotal = (from q in pDGCode
                                                           where q.Value == gridCode1
                                                           select q.Key).ToList<IFeature>(); //get all keys


                            //test
                            object testobj = pFeature.OID;
                            //顺序码
                            string pFeatureName = pFeature.get_Value(pFeature.Fields.FindField(ClsConfig.LayerConfigs[(pFeatureLayer.FeatureClass as IDataset).Name].NameField)).ToString().Trim();
                            for (int k = 0; k < keyListTotal.Count; k++)
                            {
                                string pkeyListTotalNamek = keyListTotal[k].get_Value(keyListTotal[k].Fields.FindField(ClsConfig.LayerConfigs[(pFeatureLayer.FeatureClass as IDataset).Name].NameField)).ToString().Trim();
                                if (pFeatureName == pkeyListTotalNamek)
                                {
                                    pNum = "A" + string.Format("{0:00}", k + 1);
                                }

                            }

                            //分类码
                            int pFcodeindex = pFeature.Fields.FindField(pFCODE);
                            if (pFcodeindex != -1)
                            {
                                string pFCode = pFeature.get_Value(pFcodeindex).ToString();

                                //处理分类码
                                //ClsReturnFCode pReturnFcode = new ClsReturnFCode();
                                //pFCode = pReturnFcode.ReturnFeatureClass(pFCode);
                                pFCode = ZJGISCommon.Classes.ClsFcode.pDicFcodeGlobal[pFCode];
                                if (pFCode != null)
                                {
                                    pEntiidCode = gridCode1 + pFCode + pNum;
                                    pFeature.set_Value(pFeature.Fields.FindField(pENEIID), pEntiidCode);
                                    pFeature.Store();
                                }
                            }


                            #region  20170614注释掉
                            //if (pFeature.OID == keyListTotal[0].OID)
                            //{
                            //    //分类码
                            //    int pFcodeindex = pFeature.Fields.FindField(pFCODE);
                            //    if (pFcodeindex != -1)
                            //    {
                            //        string pFCode = pFeature.get_Value(pFcodeindex).ToString();

                            //        //处理分类码
                            //        ClsReturnFCode pReturnFcode = new ClsReturnFCode();
                            //        pFCode = pReturnFcode.ReturnFeatureClass(pFCode);
                            //        if (pFCode != null)
                            //        {
                            //            string gridCode = pFeature.get_Value(pFeature.Fields.FindField(pGridCode)).ToString();

                            //            if (gridCode.Trim().Length > 0)
                            //            {
                            //                pNum = "A01";
                            //                pEntiidCode = gridCode + pFCode + pNum;

                            //                pFeature.set_Value(pFeature.Fields.FindField(pENEIID), pEntiidCode);
                            //                pFeature.Store();
                            //            }
                            //        }
                            //    }



                            //}

                            //if (pFeature.OID != keyListTotal[0].OID)
                            //{
                            //    string pFeatureName = pFeature.get_Value(pFeature.Fields.FindField("NAME")).ToString().Trim();
                            //    string pkeyListTotalName0 = keyListTotal[0].get_Value(keyListTotal[0].Fields.FindField("NAME")).ToString().Trim();
                            //    //前提：格网码相同存在多条要素，如果当前要素和和第一条要素的名称相同，那么第一条要素可以任意编码
                            //    for (int k = 1; k < keyListTotal.Count; k++)
                            //    {
                            //        string pkeyListTotalNamek = keyListTotal[k].get_Value(keyListTotal[k].Fields.FindField("NAME")).ToString().Trim();
                            //        if (pFeatureName == pkeyListTotalNamek)
                            //        {
                            //            pNum = "A" + string.Format("{0:00}", k + 1);

                            //        }

                            //    }


                            //    if (pFeatureName == pkeyListTotalName0)
                            //    {
                            //        //分类码
                            //        int pFcodeindex = keyListTotal[0].Fields.FindField(pFCODE);
                            //        if (pFcodeindex != -1)
                            //        {
                            //            string pFCode = keyListTotal[0].get_Value(pFcodeindex).ToString();

                            //            //处理分类码
                            //            ClsReturnFCode pReturnFcode = new ClsReturnFCode();
                            //            pFCode = pReturnFcode.ReturnFeatureClass(pFCode);
                            //            if (pFCode != null)
                            //            {
                            //                string gridCode = keyListTotal[0].get_Value(keyListTotal[0].Fields.FindField(pGridCode)).ToString();

                            //                if (gridCode.Trim().Length > 0)
                            //                {
                            //                    pNum = "A01";
                            //                    pEntiidCode = gridCode + pFCode + pNum;

                            //                    //if (!pDicAfter.Keys.Contains(j))
                            //                    //{
                            //                    //    pDicAfter.Add(j, pEntiidCode);
                            //                    //}

                            //                    //为当前的Feature设置 keyListTotal[0]的地理编码
                            //                    pFeature.set_Value(pFeature.Fields.FindField(pENEIID), pEntiidCode);
                            //                    pFeature.Store();
                            //                }
                            //            }
                            //        }


                            //    }
                            //    //前提：格网码相同存在多条要素，如果当前要素和和第一条要素的名称不同，那么就是格网码相同，但是不是同一个地理实体的情况
                            //    //下一步比较其分类码是否相同
                            //    else
                            //    {
                            //        string pFeatureFcode = pFeature.get_Value(pFeature.Fields.FindField(pFCODE)).ToString();
                            //        string pkeyListTotalFcode0 = keyListTotal[0].get_Value(keyListTotal[0].Fields.FindField(pFCODE)).ToString();
                            //        //处理分类码
                            //        ClsReturnFCode pReturnFcode = new ClsReturnFCode();
                            //        pFeatureFcode = pReturnFcode.ReturnFeatureClass(pFeatureFcode);
                            //        pkeyListTotalFcode0 = pReturnFcode.ReturnFeatureClass(pkeyListTotalFcode0);
                            //        //如果分类码相同，那么前两个码段相同靠顺序码区分
                            //        if (pFeatureFcode == pkeyListTotalFcode0)
                            //        {
                            //            string gridCode = keyListTotal[0].get_Value(keyListTotal[0].Fields.FindField(pGridCode)).ToString();

                            //            for (int k = 1; k < keyListTotal.Count; k++)
                            //            {
                            //                pNum = "A" + string.Format("{0:00}", k + 1);
                            //            }

                            //            pEntiidCode = gridCode + pFeatureFcode + pNum;

                            //            pFeature.set_Value(pFeature.Fields.FindField(pENEIID), pEntiidCode);
                            //            pFeature.Store();

                            //        }
                            //        //如果分类码不同，那么前两个码段就可以区分出来是否是同一个地理实体
                            //        else
                            //        {
                            //            string gridCode = keyListTotal[0].get_Value(keyListTotal[0].Fields.FindField(pGridCode)).ToString();
                            //            pNum = "A01";
                            //            pEntiidCode = gridCode + pFeatureFcode + pNum;

                            //            //为当前的Feature设置 keyListTotal[0]的地理编码
                            //            pFeature.set_Value(pFeature.Fields.FindField(pENEIID), pEntiidCode);
                            //            pFeature.Store();
                            //        }

                            //    }

                            //}
                            #endregion

                        }
                    }

                    pFeature = pFeatureCursor.NextFeature();
                }
                pWorkspaceEdit.StopEditing(true);
                pWorkspaceEdit.StopEditOperation();
            }
        }




    }
}
