using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Catalog;
using ESRI.ArcGIS.CatalogUI;
using ESRI.ArcGIS.Geodatabase;

namespace ZJGISDataUpdating.Class
{
    public static class ClsStatic
    {
        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <param name="str">以str来分割字符串</param>
        /// <returns>返回分割后的字符串数组</returns>
        public static string[] SplitStrColon(string str)
        {
            if (str==null)
            {
                return null;
            }
            String[] arr = new string[2];
            if (str.Contains(":"))
            {
                arr = str.Split(':');
            }

            return arr;
        }


        /// <summary>
        /// 判断码是否相同
        /// </summary>
        /// <param name="pSrcFeature">源图层</param>
        /// <param name="pTarFeature">目标图层</param>
        /// <returns></returns>
        public static bool CodeIsMatch(IFeature pSrcFeature, IFeature pTarFeature,string fieldName)
        {
            //int SCodeIndex = pSrcFeature.Fields.FindField("GCode");
            //int TCodeIndex = pTarFeature.Fields.FindField("GCode");
            int SCodeIndex = pSrcFeature.Fields.FindField(fieldName);
            int TCodeIndex = pTarFeature.Fields.FindField(fieldName);
            if (TCodeIndex != -1 && SCodeIndex != -1)
            {
                string SstrFirSecCode = String.Empty;
                string TstrFirSecCode = String.Empty;

                string temStr2 = pSrcFeature.get_Value(SCodeIndex).ToString();
                string temStr1 = pTarFeature.get_Value(TCodeIndex).ToString();

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
        /// 两字符串中有几个相同字符
        /// </summary>
        /// <param name="fir">第一个字符串</param>
        /// <param name="sec">第二个字符串</param>
        /// <returns></returns>
        public static int StringSameOrNot(string fir, string sec)
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

        /// <summary>
        /// 判断是否是全中文字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsCharacterString(string str)
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
        public static int StringSameOrNot2(string fir, string sec)
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return same;
        }
        /// <summary>
        /// 去掉"德清县等"指定字符
        /// </summary>
        /// <param name="fir"></param>
        public static string RemoveString(string fir)
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

        /// <summary>
        /// 处理进程条
        /// </summary>
        /// <param name="lngCount"></param>
        /// <param name="prgMain"></param>
        /// <param name="prgSub"></param>
        public static void AboutProcessBar(int lngCount, ProgressBar prgMain, ProgressBar prgSub)
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
        #region 没有用到
        //定义poi前两个组的码段
        public static string pPoiOneTwo = "1330521000000";
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
        #endregion
    }
}
