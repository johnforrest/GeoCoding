/*-------------------------------------------------------------------------
 * 版权所有：武汉大学
 * 创建人：johnforrest
 * 联系方式：571716604@qq.com
 * 创建时间： 5/3/2018 10:50:43 AM
 * 类名称：ClsGeoHashCode
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
using System.Threading.Tasks;
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
    public class ClsGeoHashCode
    {
        ClsCommon pClsCom = new ClsCommon();
        FrmProgressBar progressbar = null;

        public int Precision { get; set; }

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
                pClsCom.CheckGridField(pFeatureLayer, strField);
                //存在名称字段
                if (pFeatureLayer.FeatureClass.Fields.FindField(ClsConfig.LayerConfigs[(pFeatureLayer.FeatureClass as IDataset).Name].NameField) != -1)
                {
                    progressbar = new FrmProgressBar(pFeatureLayer.FeatureClass.FeatureCount(null));
                    progressbar.Show();

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

                            //存在格网字段
                            if (pFeatureName.Trim().Length > 0)
                            {
                                //获取格网信息
                                //string GridCode = pClsCom.GetCodeString(pFeature);
                                string GridCode = pClsCom.GetGeoHashCode(pFeature,Precision);
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
                    MessageBox.Show("此图层不存在名称字段！");
                }
            }
            else
            {
                MessageBoxEx.Show("没有选中任何图层！");
            }
        }

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

            string gridField = "GridCode";
            string roadField = "ROADCODE";
            IFeatureLayer pFeatureLayer = (IFeatureLayer)pClsCom.GetLayerByName(pMapControl, cbxLayerName.Text);
            IFeatureClass pFeatureClass = null;

            IDataset cDataset = pFeatureLayer.FeatureClass as IDataset;
            IGeoDataset cGeoDataset = cDataset as IGeoDataset;
            ISpatialReference cSpatialReference = cGeoDataset.SpatialReference;
            if (cSpatialReference is IProjectedCoordinateSystem)
            {
                MessageBox.Show("该图层为投影坐标，请转换为相应的地理坐标,再开始地理编码！");
            }

            if (pFeatureLayer != null)
            {
                progressbar = new FrmProgressBar(pFeatureLayer.FeatureClass.FeatureCount(null) * 2);
                progressbar.Show();

                pFeatureClass = pFeatureLayer.FeatureClass;

                //存在路网字段
                if (pFeatureClass.Fields.FindField(roadField) != -1)
                {
                    //没有GridCode字段就创建GridCode字段
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
                        int j = 0, k = 0, m = 0;

                        IFeature pFeature = pFeatureCursor.NextFeature();
                        //TODO :路线编码字段
                        while (pFeature != null)
                        {
                            //名称不为空，路线编码为空
                            if (pFeature.get_Value(pFeature.Fields.FindField(ClsConfig.LayerConfigs[(pFeatureLayer.FeatureClass as IDataset).Name].NameField)).ToString().Trim().Length > 0)
                            {
                                pNameNotNull.Add(pFeature, pFeature.get_Value(pFeature.Fields.FindField(ClsConfig.LayerConfigs[(pFeatureLayer.FeatureClass as IDataset).Name].NameField)).ToString());
                                pOID.Add(pFeature.OID);
                            }
                            //路线编码不为空且一个道路字母除外（例如Q等）,名称为空
                            if (pFeature.get_Value(pFeature.Fields.FindField(roadField)).ToString().Trim().Length > 1 &&
                                pFeature.get_Value(pFeature.Fields.FindField(ClsConfig.LayerConfigs[(pFeatureLayer.FeatureClass as IDataset).Name].NameField)).ToString().Trim().Length == 0)
                            {
                                pRoadNotNull.Add(pFeature, pFeature.get_Value(pFeature.Fields.FindField(roadField)).ToString());
                                pOID.Add(pFeature.OID);
                            }
                            progressbar.GoOneStep();

                            pFeature = pFeatureCursor.NextFeature();

                        }
                        //progressbar.CloseForm();
                        pWorkspaceEdit.StopEditing(true);
                        pWorkspaceEdit.StopEditOperation();
                    }
                    CreatGridCodeRoad2(pFeatureLayer, pNameNotNull, pRoadNotNull, pOID, progressbar);
                }
                else
                {
                    MessageBox.Show("此图层不存在ROADCODE路网字段，请重新检查图层字段！");
                }
            }
            else
            {
                MessageBox.Show("没有选中任何图层！");
            }
        }

        /// <summary>
        /// 生成格网码(道路)
        /// </summary>
        /// <param name="pMapControl"></param>
        /// <param name="cbxLayerName"></param>
        public void CreatGridCodeRoad2(IFeatureLayer pFeatureLayer, Dictionary<IFeature, string> pNameNotNull, Dictionary<IFeature, string> pRoadNotNull, List<object> pOID, FrmProgressBar pgBar)
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
                                    //string pGridCode3 = pClsCom.GetCodeString(pFeature);
                                    string pGridCode3 = pClsCom.GetGeoHashCode(pFeature,Precision);
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
                                    //string pGridCode2 = pClsCom.GetCodeString(pFeature);
                                    string pGridCode2 = pClsCom.GetGeoHashCode(pFeature,Precision);
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
                        pgBar.GoOneStep();
                        pFeature = pFeatureCursor.NextFeature();
                    }
                    pgBar.CloseForm();
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
            string GridCode = pClsCom.GetGeoHashCode(pFeature2,Precision);

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
                            //string GridCode = pClsCom.GetCodeString(pFeature);
                            string GridCode = pClsCom.GetGeoHashCode(pFeature,Precision);
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

    }
}
