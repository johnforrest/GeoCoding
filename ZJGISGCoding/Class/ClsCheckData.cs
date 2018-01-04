using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevComponents.DotNetBar.Controls;
using DevComponents.DotNetBar;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using Microsoft.VisualBasic;
using ZJGISGCoding.Class;
using ZJGISCommon.Classes;
using System.Data;
namespace ZJGISGCoding
{
    public static class ClsCheckData
    {

        /// <summary>
        /// 添加图层到COMBOX
        /// </summary>
        /// <param name="pMapControl"></param>
        /// <param name="cbxLayerName"></param>
        public static void AddDataToCom(IMap pMapControl, ComboBoxEx cbxLayerName)
        {
            for (int i = 0; i < pMapControl.LayerCount; i++)
            {
                ILayer pLayer = pMapControl.get_Layer(i);
                if (pLayer is IGroupLayer)
                {
                    ICompositeLayer pCompositeLayer = pLayer as ICompositeLayer;
                    for (int j = 0; j < pCompositeLayer.Count; j++)
                    {
                        if (pCompositeLayer.get_Layer(j) is IFeatureLayer)
                        {
                            cbxLayerName.Items.Add(pCompositeLayer.get_Layer(j).Name);
                        }
                    }
                }
                else
                {
                    if (pLayer is IFeatureLayer)
                    {

                        cbxLayerName.Items.Add(pMapControl.get_Layer(i).Name);

                    }
                }
            }
            if (cbxLayerName.Items.Count != 0)
            {
                cbxLayerName.SelectedIndex = 0;
            }

        }
        /// <summary>
        /// 添加图层到COMBOX
        /// </summary>
        /// <param name="pMapControl"></param>
        /// <param name="cbxLayerName"></param>
        public static void AddDataToCom(IMap pMapControl, ComboBoxItem cbxLayerName)
        {
            for (int i = 0; i < pMapControl.LayerCount; i++)
            {
                ILayer pLayer = pMapControl.get_Layer(i);
                if (pLayer is IGroupLayer)
                {
                    ICompositeLayer pCompositeLayer = pLayer as ICompositeLayer;
                    for (int j = 0; j < pCompositeLayer.Count; j++)
                    {
                        if (pCompositeLayer.get_Layer(j) is IFeatureLayer)
                        {
                            cbxLayerName.Items.Add(pCompositeLayer.get_Layer(j).Name);
                        }
                    }
                }
                else
                {
                    if (pLayer is IFeatureLayer)
                    {

                        cbxLayerName.Items.Add(pMapControl.get_Layer(i).Name);

                    }
                }
            }
            if (cbxLayerName.Items.Count != 0)
            {
                cbxLayerName.SelectedIndex = 0;
            }

        }
        /// <summary>
        /// 空值检查
        /// </summary>
        /// <param name="pMapControl"></param>
        /// <param name="cbxLayerName"></param>
        /// <param name="dgv"></param>
        public static void CheckCode(IMap pMapControl, ComboBoxEx cbxLayerName, DataGridViewX dgv)
        {
            int j = 0;

            DataGridViewX dataView = dgv;
            dataView.Rows.Clear();
            dataView.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            dataView.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.White;
            dataView.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font(dataView.Font, System.Drawing.FontStyle.Bold);
            dataView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            dataView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Sunken;
            dataView.GridColor = System.Drawing.Color.Black;
            dataView.RowHeadersVisible = false;
            dataView.ColumnCount = 3;
            dataView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dataView.MultiSelect = false;
            dataView.AllowUserToAddRows = false;
            dataView.ReadOnly = true;
            dataView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataView.Columns[0].Name = "LayerName";
            dataView.Columns[0].HeaderText = "图层名称";

            dataView.Columns[1].Name = "YesOrNo";
            dataView.Columns[1].HeaderText = "是否有FCODE";

            dataView.Columns[2].Name = "LayerGroup";
            dataView.Columns[2].HeaderText = "所在图层组";

            string strField = "FCODE";
            ILayer pLayer = GetLayerByName(pMapControl, cbxLayerName.Text);
            if (pLayer is IGroupLayer)
            {
                string GroupLayer = pLayer.Name;
                ICompositeLayer pCompositeLayer = pLayer as ICompositeLayer;
                for (int k = 0; k < pCompositeLayer.Count; k++)
                {
                    if (pCompositeLayer.get_Layer(k) is IFeatureLayer)
                    {
                        j = dataView.Rows.Add();
                        dataView["LayerGroup", j].Value = GroupLayer;
                        dataView["LayerName", j].Value = pMapControl.get_Layer(k).Name;
                        IFeatureLayer pFeatureLayer = pCompositeLayer.get_Layer(k) as IFeatureLayer;
                        IClass pTable = pFeatureLayer.FeatureClass as IClass;
                        if (pTable.Fields.FindField(strField) == -1)
                        {
                            dataView["YesOrNo", j].Value = "否";
                            dataView.Columns[0].Width = dataView.Width / 3;
                            dataView.Columns[1].Width = dataView.Width / 3;
                            dataView.Columns[2].AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
                        }
                        else
                        {
                            dataView["YesOrNo", j].Value = "是";
                            NullCheck(pMapControl, dataView, pLayer);
                        }

                    }
                }

            }
            else
            {
                if (pLayer is IFeatureLayer)
                {
                    j = dataView.Rows.Add();
                    dataView["LayerGroup", j].Value = "NULL";
                    dataView["LayerName", j].Value = cbxLayerName.Text;
                    IFeatureLayer pFeatureLayer = pLayer as IFeatureLayer;
                    IClass pTable = pFeatureLayer.FeatureClass as IClass;
                    if (pTable.Fields.FindField(strField) == -1)
                    {
                        dataView["YesOrNo", j].Value = "否";
                        dataView.Columns[0].Width = dataView.Width / 3;
                        dataView.Columns[1].Width = dataView.Width / 3;
                        dataView.Columns[2].AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
                    }
                    else
                    {
                        dataView["YesOrNo", j].Value = "是";
                        NullCheck(pMapControl, dataView, pLayer);

                    }

                }
            }
        }




        /// <summary>
        /// 空值检查
        /// </summary>
        /// <param name="pMapControl"></param>
        /// <param name="dgv"></param>
        /// <param name="pLayer"></param>
        /// <returns></returns>
        public static bool NullCheck(IMap pMapControl, DataGridViewX dgv, ILayer pLayer)
        {
            DataGridViewX dataView = dgv;
            string strField = "FCODE";
            IFeatureLayer pFeatureLayer = pLayer as IFeatureLayer;
            if (pFeatureLayer == null)
            {

                return false;
            }
            IQueryFilter pQueryFilter = new QueryFilter();
            pQueryFilter.WhereClause = "\"" + strField + "\"" + " = ''";// or \"" + strField + "\" is NULL";
            IFeatureCursor pFeatureCursor = pFeatureLayer.Search(pQueryFilter, false);
            IFeature pFeature = pFeatureCursor.NextFeature();
            if (pFeature == null)
            {
                dataView.Columns[0].Width = dataView.Width / 3;
                dataView.Columns[1].Width = dataView.Width / 3;
                dataView.Columns[2].AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
                MessageBoxEx.Show("FCODE无空值！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            }
            int j = 0;
            dataView.ColumnCount = dataView.ColumnCount + 1;
            dataView.Columns[3].Name = "FID";
            dataView.Columns[3].HeaderText = "空值所在编号";

            dataView.Columns[0].Width = dataView.Width / 4;
            dataView.Columns[1].Width = dataView.Width / 4;
            dataView.Columns[2].Width = dataView.Width / 4;
            dataView.Columns[3].AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;


            while (pFeature != null)
            {


                dataView["FID", j].Value = pFeature.OID.ToString();
                dataView["LayerGroup", j].Value = "NULL";
                dataView["LayerName", j].Value = pLayer.Name;
                dataView["YesOrNo", j].Value = "是";
                pFeature = pFeatureCursor.NextFeature();
                if (pFeature != null)
                {
                    j = dataView.Rows.Add();
                }

            }
            MessageBoxEx.Show("FCODE有空值！");
            return false;

        }


        ///// <summary>
        ///// 检查图层编码
        ///// </summary>
        ///// <param name="pFeatureLayer">待检查的图层</param>
        //public static DataTable CheckLayers(IFeatureLayer pFeatureLayer)
        //{
        //    DataTable dt = null;
        //    if (pFeatureLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPoint)
        //    {
        //        ClsCommonEnti pcommonEnti = new ClsCommonEnti();
        //        //pcommonEnti.CreatGridCode(mapMain.Map, cbxCodeLayer);
        //    }
        //    else if (pFeatureLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolyline)
        //    {
        //        ClsRoadEnti pRoadEnti = new ClsRoadEnti();
        //        //pRoadEnti.CreatGridCodeRoad(mapMain.Map, cbxCodeLayer);
        //    }
        //    else if (pFeatureLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
        //    {
        //        ClsResEnti pResEnti = new ClsResEnti();
        //        //pResEnti.CreatGridCodeRES(mapMain.Map, cbxCodeLayer);
        //    }
            

        //}


        /// <summary>
        /// 道路编码函数
        /// </summary>
        /// <param name="pFeatureLayer"></param>
        /// <param name="pField"></param>
        /// <param name="pRoadCode"></param>
        /// <param name="pENEIID"></param>
        private static void CreatRoadCode(IFeatureLayer pFeatureLayer, IField pField, string pRoadCode, string pENEIID)
        {
            if (pField.AliasName.Contains(pRoadCode))
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

                    //test
                    int test = pFeatureLayer.FeatureClass.FeatureCount(null);
                    IFeature pFeature = pFeatureCursor.NextFeature();


                    while (pFeature != null)
                    {

                        //获取单条Feature的某个字段值
                        int test2 = pFeature.Fields.FindFieldByAliasName(pRoadCode);
                        string pRoad = pFeature.get_Value(pFeature.Fields.FindFieldByAliasName(pRoadCode)).ToString();


                        if (pRoad.Length > 1 && pRoad.Length < 6)
                        {
                            if (pRoad.Length == 5)
                            {
                                pFeature.set_Value(pFeature.Fields.FindField(pENEIID), pRoad + 330521);
                                pFeature.Store();
                            }
                            else
                            {
                                string _Count = null;
                                for (int k = 0; k < 5 - pRoad.Length; k++)
                                {
                                    _Count += "_";
                                }
                                pRoad = pRoad + _Count;
                                pFeature.set_Value(pFeature.Fields.FindField(pENEIID), pRoad + 330521);
                                pFeature.Store();
                            }
                        }
                        pFeature = pFeatureCursor.NextFeature();
                    }
                    pWorkspaceEdit.StopEditing(true);
                    pWorkspaceEdit.StopEditOperation();

                }
            }
        }

        /// <summary>
        /// 对房屋进行编码
        /// </summary>
        /// <param name="pMapControl"></param>
        /// <param name="cbxLayerName"></param>
        public static void HouseCode(IMap pMapControl, ComboBoxEx cbxLayerName)
        {
            IFeatureLayer pFeatureLayer = (IFeatureLayer)GetLayerByName(pMapControl, cbxLayerName.Text);
            //遍历字段
            IField pField = null;
            IFields pFields = pFeatureLayer.FeatureClass.Fields;
            bool pRoadCode = false;
            bool pEntiID = false;

            //匹配正则表达式，字段字符串必须得全是字母，把ENTIID2，ENTIID3，ENTIID4等剔除掉
            System.Text.RegularExpressions.Regex pRex = new System.Text.RegularExpressions.Regex(@"^[A-Za-z]+$");

            for (int i = 0; i < pFields.FieldCount - 1; i++)
            {
                pField = pFields.get_Field(i);
                if (pField.AliasName.Contains("路线编码"))
                {
                    pRoadCode = true;
                }
                if (pField.AliasName.Contains("ENTIID") && pRex.IsMatch(pField.AliasName.ToString()))
                {
                    pEntiID = true;
                }

            }
            if (pRoadCode)
            {
                MessageBox.Show("请按照道路实体编码规则编码！");
            }
            else
            {
                if (pEntiID)
                {
                    //字段中含有ENTIID字段，直接进行guid编码即可
                    CreateHouseCode(pFeatureLayer, "ENTIID");
                }
                else
                {
                    //字段中没有ENTIID字段，先增加ENTIID再进行guid编码即可
                    IClass pTable = pFeatureLayer.FeatureClass as IClass;
                    IField pField2 = new FieldClass();
                    IFieldEdit pFieldEdit = pField2 as IFieldEdit;
                    pFieldEdit.Name_2 = "ENTIID";
                    pFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
                    pTable.AddField(pField2);
                    //对街道、房屋实体进行编码
                    CreateHouseCode(pFeatureLayer, "ENTIID");
                }
            }
            MessageBox.Show("街道房屋实体编码成功！");
        }
        /// <summary>
        /// 对街道、房屋实体进行编码
        /// </summary>
        /// <param name="pFeatureLayer">需要编码的图层</param>
        /// <param name="pENTIID">需要编码的字段名称</param>
        private static void CreateHouseCode(IFeatureLayer pFeatureLayer, string pENTIID)
        {
            #region 针对GUID编码来讲
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

                //test
                int test = pFeatureLayer.FeatureClass.FeatureCount(null);
                IFeature pFeature = pFeatureCursor.NextFeature();
                while (pFeature != null)
                {
                    //获取单条Feature的某个字段值
                    int test2 = pFeature.Fields.FindFieldByAliasName(pENTIID);

                    string pGUID = System.Guid.NewGuid().ToString();
                    pFeature.set_Value(pFeature.Fields.FindFieldByAliasName(pENTIID), pGUID);
                    pFeature.Store();
                    pFeature = pFeatureCursor.NextFeature();
                }
                pWorkspaceEdit.StopEditing(true);
                pWorkspaceEdit.StopEditOperation();

            }

            #endregion
        }

        //定义poi前两个组的码段
        public static string pPoiOneTwo = "1330521000000";
        /// <summary>
        /// 对村落POI进行编码
        /// </summary>
        /// <param name="pMapControl"></param>
        /// <param name="cbxLayerName"></param>
        public static void POICode(IMap pMapControl, ComboBoxEx cbxLayerName)
        {
            IFeatureLayer pFeatureLayer = (IFeatureLayer)GetLayerByName(pMapControl, cbxLayerName.Text);
            //遍历字段
            IField pField = null;
            IFields pFields = pFeatureLayer.FeatureClass.Fields;
            //判断是否存在ENTIID字段
            //匹配正则表达式，字段字符串必须得全是字母，把ENTIID2，ENTIID3，ENTIID4等剔除掉
            System.Text.RegularExpressions.Regex pRex = new System.Text.RegularExpressions.Regex(@"^[A-Za-z]+$");
            bool pEntiID = false;
            for (int i = 0; i < pFields.FieldCount - 1; i++)
            {
                pField = pFields.get_Field(i);
                if (pField.AliasName.Contains("ENTIID") && pRex.IsMatch(pField.AliasName.ToString()))
                {
                    pEntiID = true;
                }

            }
            if (pEntiID)
            {
                //字段中含有ENTIID字段，直接进行guid编码即可
                CreatePOICode(pFeatureLayer, "ENTIID");
            }
            else
            {
                //字段中没有ENTIID字段，先增加ENTIID再进行guid编码即可
                IClass pTable = pFeatureLayer.FeatureClass as IClass;
                IField pField2 = new FieldClass();
                IFieldEdit pFieldEdit = pField2 as IFieldEdit;
                pFieldEdit.Name_2 = "ENTIID";
                pFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
                pTable.AddField(pField2);
                //对街道、房屋实体进行编码
                CreatePOICode(pFeatureLayer, "ENTIID");
            }
            MessageBox.Show("POI编码成功！");
        }
        /// <summary>
        /// 对村落、POI实体进行编码
        /// </summary>
        /// <param name="pFeatureLayer">需要编码的图层</param>
        /// <param name="pENTIID">需要编码的字段名称</param>
        private static void CreatePOICode(IFeatureLayer pFeatureLayer, string pENTIID)
        {
            #region 针对GUID编码来讲
            int i = 0;

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

                //test
                int test = pFeatureLayer.FeatureClass.FeatureCount(null);
                IFeature pFeature = pFeatureCursor.NextFeature();
                while (pFeature != null)
                {
                    string pResult = null;

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
                    pFeature.set_Value(pFeature.Fields.FindField(pENTIID), pEntiid);
                    pFeature.Store();
                    pFeature = pFeatureCursor.NextFeature();
                }
                pWorkspaceEdit.StopEditing(true);
                pWorkspaceEdit.StopEditOperation();

            }

            #endregion
        }
        /// <summary>
        /// 返回一个小于9 999 999的数字和字母组合
        /// </summary>
        /// <returns></returns>
        private static string ReturnNumberCharacter(int i)
        {
            //int i = 0;
            string pResult = null;
            while (i < 10000000)
            {
                i++;
                break;
            }
            for (int k = 0; k < 7 - i.ToString().Length; k++)
            {
                pResult += "0";
            }
            return pResult + i.ToString();
        }




        /// <summary>
        /// 生成格网信息
        /// </summary>
        /// <param name="pFeature">Featureclass</param>
        /// <returns></returns>
        public static string GetCodeString(IFeature pFeature)
        {
            double centroidX = 0;
            double centroidY = 0;
            IPoint pPoint = null;
            IPoint pPointEverrage = new PointClass();
            IPoint pPointNearest = new PointClass();

            IPoint pPointEverrage1 = new PointClass();
            IPoint pPointNearest1 = new PointClass();
            //求要素的质量中心
            switch (pFeature.Shape.GeometryType)
            {
                case esriGeometryType.esriGeometryPoint:
                    pPoint = pFeature.ShapeCopy as IPoint;
                    centroidX = pPoint.X;
                    centroidY = pPoint.Y;
                    break;
                case esriGeometryType.esriGeometryPolyline:
                    IPolyline pPolyline = pFeature.ShapeCopy as IPolyline;
                    IPointCollection pPointCollection = pPolyline as IPointCollection;

                    //20170607 先求出点集的中心点，然后再算出中心点到polyline的最近的点，之后用改点来替代整个polyline，保证了该点在改地理实体上
                    IProximityOperator proOperator = pPolyline as IProximityOperator;

                    double centerX = 0;
                    double centerY = 0;
                    for (int i = 0; i < pPointCollection.PointCount; i++)
                    {
                        pPoint = pPointCollection.get_Point(i);
                        centerX += pPoint.X;
                        centerY += pPoint.Y;
                    }
                    centroidX = centerX / pPointCollection.PointCount;
                    centroidY = centerY / pPointCollection.PointCount;

                    //20170607
                    pPointEverrage.X = centroidX;
                    pPointEverrage.Y = centroidY;
                    if (pPointEverrage != null)
                    {
                        pPointNearest = proOperator.ReturnNearestPoint(pPointEverrage, esriSegmentExtension.esriNoExtension);
                    }
                    centroidX = pPointNearest.X;
                    centroidY = pPointNearest.Y;
                    break;

                case esriGeometryType.esriGeometryPolygon:
                    //多边形获取的是中心节点的坐标
                    IPolygon pPolygon = pFeature.ShapeCopy as IPolygon;
                    IPointCollection pPolygonCollection = pPolygon as IPointCollection;

                    //20170607 先求出点集的中心点，然后再算出中心点到polyline的最近的点，之后用改点来替代整个polyline，保证了该点在改地理实体上
                    IProximityOperator pOperator = pPolygon as IProximityOperator;

                    double PolygoncenterX = 0;
                    double PolygoncenterY = 0;
                    for (int i = 0; i < pPolygonCollection.PointCount; i++)
                    {
                        pPoint = pPolygonCollection.get_Point(i);
                        PolygoncenterX += pPoint.X;
                        PolygoncenterY += pPoint.Y;
                    }
                    centroidX = PolygoncenterX / pPolygonCollection.PointCount;
                    centroidY = PolygoncenterY / pPolygonCollection.PointCount;

                    //20170607
                    pPointEverrage1.X = centroidX;
                    pPointEverrage1.Y = centroidY;
                    if (pPointEverrage != null)
                    {
                        pPointNearest1 = pOperator.ReturnNearestPoint(pPointEverrage1, esriSegmentExtension.esriNoExtension);
                    }
                    centroidX = pPointNearest1.X;
                    centroidY = pPointNearest1.Y;

                    break;
            }


            try
            {
                //加入第一级格网和第二级格网的信息
                string strCode = loadFirstGrid(centroidX, centroidY);

                return strCode;
            }
            catch (Exception ex)
            {
                ClsLog.WriteFile(ex, pFeature.OID.ToString());
                return "";
            }
        }
        /// <summary>
        /// 根据图层名称获取ILayer图层
        /// </summary>
        /// <param name="pMapControl"></param>
        /// <param name="LayerName"></param>
        /// <returns></returns>
        public static ILayer GetLayerByName(IMap pMapControl, string LayerName)
        {
            ILayer pLayer = null;
            try
            {
                for (int i = 0; i < pMapControl.LayerCount; i++)
                {
                    if (pMapControl.get_Layer(i) is IGroupLayer)
                    {
                        ICompositeLayer pCompositeLayer = pMapControl.get_Layer(i) as ICompositeLayer;
                        for (int j = 0; j < pCompositeLayer.Count; j++)
                        {
                            if (pCompositeLayer.get_Layer(j).Name == LayerName)
                            {
                                pLayer = pCompositeLayer.get_Layer(j);
                                break;
                            }
                        }
                    }
                    else
                    {
                        if (pMapControl.get_Layer(i) is IFeatureLayer && pMapControl.get_Layer(i).Name == LayerName)
                        {
                            pLayer = pMapControl.get_Layer(i);
                            break;
                        }
                    }
                }
                return pLayer;
            }
            catch
            {
                MessageBox.Show("当前图层为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }
        }
        /// <summary>
        /// 获取第一级格网
        /// </summary>
        /// <param name="x_centroid"></param>
        /// <param name="y_centroid"></param>
        /// <returns></returns>
        private static string loadFirstGrid(double x_centroid, double y_centroid)
        {
            string FirstGridCode = string.Empty;
            if (x_centroid > 120 && y_centroid > 28)
            {
                FirstGridCode = "H51";
                FirstGridCode += loadSecondGrid(6, 4, x_centroid, y_centroid, 120, 28, 126, 32, 1);
            }
            else if (x_centroid < 120 && y_centroid > 28)
            {
                FirstGridCode = "H50";
                FirstGridCode += loadSecondGrid(6, 4, x_centroid, y_centroid, 114, 28, 120, 32, 2);
            }
            else if (x_centroid < 120 && y_centroid < 28)
            {
                FirstGridCode = "G50";
                FirstGridCode += loadSecondGrid(8, 5, x_centroid, y_centroid, 114, 24, 120, 28, 3);
            }
            else if (x_centroid > 120 && y_centroid < 28)
            {
                FirstGridCode = "G51";
                FirstGridCode += loadSecondGrid(8, 5, x_centroid, y_centroid, 120, 24, 126, 28, 4);
            }
            return FirstGridCode;

        }
        /// <summary>
        /// 获取第二级格网
        /// </summary>
        /// <param name="Xaverage"></param>
        /// <param name="Yaverage"></param>
        /// <param name="x_centroid"></param>
        /// <param name="y_centroid"></param>
        /// <param name="StartX"></param>
        /// <param name="StartY"></param>
        /// <param name="EndX"></param>
        /// <param name="EndY"></param>
        /// <param name="Quadrant"></param>
        /// <returns></returns>
        private static string loadSecondGrid(int Xaverage, int Yaverage, double x_centroid, double y_centroid, double StartX, double StartY, double EndX, double EndY, int Quadrant)
        {
            //二级格网划分
            string SecondGridCode = string.Empty;
            double LengthX = (EndX - StartX) / Xaverage;//经度6或8等分
            double LengthY = (EndY - StartY) / Yaverage;//纬度4等分

            double OriginalX = StartX;
            double OriginalY = StartY;

            double DistanceX = x_centroid - OriginalX;
            double DistanceY = y_centroid - OriginalY;
            if (DistanceY < 0)
            {
                DistanceY = 0;
            }
            if (DistanceX < 0)
            {
                DistanceX = 0;
            }
            int intXCode = Convert.ToInt16(Math.Floor(DistanceX / LengthX));
            int intYCode = Convert.ToInt16(Math.Floor(DistanceY / LengthY));
            //字典
            string tempString = intXCode.ToString() + intYCode.ToString();
            string strCode = string.Empty;
            if (Quadrant == 1)
            {
                strCode = ClsDictionary.SecondGridD1[Convert.ToInt32(tempString)];
            }
            else if (Quadrant == 2)
            {
                strCode = ClsDictionary.SecondGridD2[Convert.ToInt32(tempString)];
            }
            else if (Quadrant == 3)
            {
                strCode = ClsDictionary.SecondGridD3[Convert.ToInt32(tempString)];
            }
            else if (Quadrant == 4)
            {
                strCode = ClsDictionary.SecondGridD4[Convert.ToInt32(tempString)];
            }
            SecondGridCode += strCode;

            //5级格网划分
            int GridLevel = 3;//只划分到第五级，也就是3
            Xaverage = 10;//10等分
            Yaverage = 10;//10等分

            string[] strW = new string[3];
            string[] strN = new string[3];
            //循环三次，循环一次，按10等分再分一次网格。
            for (int i = 1; i <= GridLevel; i++)
            {

                OriginalX += intXCode * LengthX;
                OriginalY += intYCode * LengthY;

                LengthX = LengthX / Xaverage;
                LengthY = LengthY / Yaverage;

                DistanceX = x_centroid - OriginalX;
                DistanceY = y_centroid - OriginalY;
                if (DistanceY < 0)
                {

                    DistanceY = 0;
                }
                if (DistanceX < 0)
                {
                    DistanceX = 0;
                }
                intXCode = Convert.ToInt16(Math.Floor(DistanceX / LengthX));
                intYCode = Convert.ToInt16(Math.Floor(DistanceY / LengthY));
                strW[i - 1] = intXCode.ToString();
                strN[i - 1] = intYCode.ToString();
                //SecondGridCode +=  intYCode.ToString() + intXCode.ToString() ;

            }
            //原来的排序规则
            //SecondGridCode += strN[0] + strN[1] + strN[2] + strW[0] + strW[1] + strW[2];

            SecondGridCode += strW[0] + strN[0] + strW[1] + strN[1] + strW[2] + strN[2];
            return SecondGridCode;
        }

        private static void FillFrm(IFeature pFeature, DataGridViewX pDataGridView, int k)
        {
            if (k == 0)
            {
                pDataGridView.Rows.Clear();

            }


            pDataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Sunken;
            pDataGridView.ColumnCount = pFeature.Fields.FieldCount;

            for (int m = 0; m < pFeature.Fields.FieldCount; m++)
            {

                pDataGridView.Columns[m].HeaderText = pFeature.Fields.get_Field(m).AliasName;
            }


            if (pFeature != null)
            {

                pDataGridView.Rows.Add();
                for (int j = 0; j < pFeature.Fields.FieldCount; j++)
                {
                    pDataGridView[j, k].Value = pFeature.get_Value(j).ToString();
                }
            }
        }



    }
}
