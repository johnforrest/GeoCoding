#region
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Geometry;
using DevComponents.DotNetBar;
using ZJGISCommon;
using ESRI.ArcGIS.Carto;
using ZJGISDataUpdating.Class;
#endregion
//using System.Text.RegularExpressions;
namespace ZJGISDataUpdating
{
    public partial class FrmMatchPolygonDif : DevComponents.DotNetBar.Office2007Form
    {
        Form previousForm;

        Dictionary<int, DataGridViewRow> m_InRowDic;
        Dictionary<int, DataGridViewRow> m_OutRowDic;
        //Dictionary<int, Dictionary<string, IFeatureClass>> m_FeatClsDic;

        public FrmMatchPolygonDif()
        {
            InitializeComponent();
            m_InRowDic = new Dictionary<int, DataGridViewRow>();
            m_OutRowDic = new Dictionary<int, DataGridViewRow>();
        }

        public Dictionary<int, DataGridViewRow> InRowDic
        {
            set
            {
                m_InRowDic = value;
            }
        }
        public Dictionary<int, DataGridViewRow> OutRowDic
        {
            set
            {
                m_OutRowDic = value;
            }
        }

        public Form PreviousForm
        {
            set
            {
                previousForm = value;
            }
        }

        private void FrmMatchDif_Load(object sender, EventArgs e)
        {
            double width = this.dataGridViewX1.Width;
            DataGridViewCheckBoxColumn dgvCheckBoxColumn = new DataGridViewCheckBoxColumn();
            dgvCheckBoxColumn.HeaderText = "状态";
            dgvCheckBoxColumn.Width = Convert.ToInt32(width * 0.1);
            this.dataGridViewX1.Columns.Add(dgvCheckBoxColumn);

            this.dataGridViewX1.Columns.Add("SourceFileName", "源图层名称");
            this.dataGridViewX1.Columns[1].Width = Convert.ToInt32(width * 0.25);
            this.dataGridViewX1.Columns[1].ReadOnly = true;

            this.dataGridViewX1.Columns.Add("workspaceFileName", "待匹配图层名称");
            this.dataGridViewX1.Columns[2].Width = Convert.ToInt32(width * 0.25);
            this.dataGridViewX1.Columns[2].ReadOnly = true;

            this.dataGridViewX1.Columns.Add("MatchResultTable", "匹配结果表");
            this.dataGridViewX1.Columns[3].Width = Convert.ToInt32(width * 0.4);
            this.dataGridViewX1.Columns[3].ReadOnly = true;
            //20170504 书写错误？？
            //this.dataGridViewX1.Columns.Add("MatchResultTable", "匹配结果表");
            //this.dataGridViewX1.Columns[2].Width = Convert.ToInt32(width * 0.4);
            //this.dataGridViewX1.Columns[2].ReadOnly = true;

            //当源图层和待更新图层都最少含有一行数据时
            if (m_InRowDic.Count > 0 && m_OutRowDic.Count > 0)
            {

                for (int i = 0; i < m_InRowDic.Count; i++)
                {
                    DataGridViewRow dgvRow = new DataGridViewRow();
                    dgvRow = this.dataGridViewX1.Rows[this.dataGridViewX1.Rows.Add()];

                    dgvRow.Cells[1].Value = m_InRowDic[i].Cells[1].Value;
                    dgvRow.Cells[1].Tag = m_InRowDic[i].Cells[1].Tag;

                    dgvRow.Cells[2].Value = m_OutRowDic[i].Cells[1].Value;
                    dgvRow.Cells[2].Tag = m_OutRowDic[i].Cells[1].Tag;

                    DataGridViewCheckBoxCell dgvCheckBoxCell = new DataGridViewCheckBoxCell();
                    dgvCheckBoxCell = dgvRow.Cells[0] as DataGridViewCheckBoxCell;
                    dgvCheckBoxCell.Value = true;

                    //把最后新表的名字放到了dgvRow的第四个cell里面了
                    //工作层表
                    string table = m_OutRowDic[i].Cells[1].Value.ToString();
                    //由于工作层有_TE,结果表应该是待更新层_Table
                    // dgvRow.Cells[3].Value = table.Substring(0, table.LastIndexOf("_")) + "_Table";
                    dgvRow.Cells[3].Value = table + "_DifPyTable";


                }
            }


        }
        #region 面实体匹配
        //TODO: 面实体的开始匹配
        /// <summary>
        /// 开始匹配
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonXStartMatch_Click(object sender, EventArgs e)
        {
            //StopWatch类计时
            Stopwatch sw = new Stopwatch();
            sw.Start();

            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            IWorkspaceFactory2 pWorkspaceFactory = new FileGDBWorkspaceFactoryClass() as IWorkspaceFactory2;
            string gdbPath = ClsDeclare.g_WorkspacePath;
            IWorkspace2 pWorkspace = pWorkspaceFactory.OpenFromFile(gdbPath, 0) as IWorkspace2;
            IFeatureWorkspace featureWorkspace = pWorkspace as IFeatureWorkspace;

            IFeatureClass pSourceFeatCls2 = this.dataGridViewX1[1, 0].Tag as IFeatureClass;
            string targetFeatureName = this.dataGridViewX1[2, 0].Value.ToString();
            //创建填充表MatchedPolygonFCSettingDif
            loadFeatSetting(gdbPath, targetFeatureName, pSourceFeatCls2);

            for (int j = 0; j < this.dataGridViewX1.RowCount; j++)
            {
                IFeatureClass srcFeatCls = this.dataGridViewX1.Rows[j].Cells[1].Tag as IFeatureClass;
                IFields fileds = null;
                if (srcFeatCls.ShapeType == esriGeometryType.esriGeometryPolygon)
                {
                    //CreateMatchPolygonTFields函数返回了表xx的所有字段信息
                    fileds = CreateMatchPolygonTFields(pWorkspace);
                }
                //结果表xxx_DifPyTable存在，删除表再重现创建
                ITable table = null;
                if (pWorkspace.get_NameExists(esriDatasetType.esriDTTable, this.dataGridViewX1.Rows[j].Cells[3].Value.ToString()))
                {
                    //ClsDeleteTables.DeleteFeatureClass(gdbPath, this.dataGridViewX1.Rows[j].Cells[3].Value.ToString());
                    //table = CreateTable(pWorkspace, this.dataGridViewX1.Rows[j].Cells[3].Value.ToString(), fileds);
                    table = featureWorkspace.OpenTable(this.dataGridViewX1.Rows[j].Cells[3].Value.ToString());

                    IWorkspaceEdit workspaceEdit = pWorkspace as IWorkspaceEdit;
                    workspaceEdit.StartEditing(true);
                    workspaceEdit.StartEditOperation();
                    ICursor cursor = table.Search(null, false);
                    IRow r = cursor.NextRow();
                    while (r != null)
                    {
                        r.Delete();
                        r = cursor.NextRow();
                    }
                    workspaceEdit.StopEditOperation();
                    workspaceEdit.StopEditing(true);
                }
                //结果表xxx_DifPyTable不存在，创建表
                else
                {
                    table = CreateTable(pWorkspace, this.dataGridViewX1.Rows[j].Cells[3].Value.ToString(), fileds);
                }
                //把生成匹配结果表xxx_DifPyTable(空表)存储到cell[3]的Tag属性中
                this.dataGridViewX1[3, j].Tag = table;
                //根据工作图层的名称（待更新图层的名称）来打开待更新图层数据（feature）
                IFeatureClass tarFeatCls = featureWorkspace.OpenFeatureClass(this.dataGridViewX1.Rows[j].Cells[2].Value.ToString());
                IDataset dataset = tarFeatCls as IDataset;
                string matchedFcName = dataset.Name;

                ITable tableSetting = null;

                double buffer = 0;
                double minArea = 0.0;
                if (srcFeatCls.ShapeType == esriGeometryType.esriGeometryPolygon)
                {
                    tableSetting = featureWorkspace.OpenTable(ClsConstant.polygonSettingTable);
                    if (tableSetting != null)
                    {
                        ICursor cursor = tableSetting.Search(null, false);
                        IRow row = cursor.NextRow();
                        while (row != null)
                        {
                            if (row.get_Value(row.Fields.FindField("MatchedFCName")).ToString() == matchedFcName)
                            {
                                //面积重叠比匹配
                                if (this.tabItemAreaOverlap.Visible == true)
                                {
                                    string temp = row.get_Value(row.Fields.FindField("Buffer")).ToString();
                                    if (temp.Contains("米"))
                                    {
                                        temp = temp.Substring(0, temp.LastIndexOf("米")).Trim();
                                    }
                                    buffer = Convert.ToDouble(temp);
                                    minArea = Convert.ToDouble(row.get_Value(row.Fields.FindField("AreaNum")));
                                }
                                ////几何匹配
                                //else if (this.tabItemGeo.Visible == true)
                                //{
                                //    fields = row.get_Value(row.Fields.FindField("MatchedFields")).ToString();
                                //    matchedMay = 0;
                                //    matchedMode = 10;
                                //    weight[0] = Convert.ToDouble(row.get_Value(row.Fields.FindField("SP")));
                                //    string temp = row.get_Value(row.Fields.FindField("MatchedPointsBuffer")).ToString();
                                //    if (temp.Contains("米"))
                                //    {
                                //        temp = temp.Substring(0, temp.LastIndexOf("米")).Trim();
                                //    }
                                //    buffer = Convert.ToDouble(temp);

                                //    weight[1] = Convert.ToDouble(row.get_Value(row.Fields.FindField("MatchedPoints")));
                                //    weight[2] = Convert.ToDouble(row.get_Value(row.Fields.FindField("TotalNum")));

                                //    includeAngel = Convert.ToDouble(row.get_Value(row.Fields.FindField("IncludeAngle")));
                                //    hausdorff = Convert.ToDouble(row.get_Value(row.Fields.FindField("Hausdorff")));

                                //}
                                ////拓扑匹配
                                ////if (row.get_Value(row.Fields.FindField("Top")).ToString() == "1")
                                //else if (this.tabItemTopo.Visible == true)
                                //{
                                //    matchedMay = 1;
                                //    matchedMode = -1;
                                //    //weight = 0;
                                //    string temp = row.get_Value(row.Fields.FindField("Buffer")).ToString();
                                //    if (temp.Contains("米"))
                                //    {
                                //        temp = temp.Substring(0, temp.LastIndexOf("米")).Trim();
                                //    }
                                //    buffer = Convert.ToDouble(temp);
                                //    fields = row.get_Value(row.Fields.FindField("MatchedFields")).ToString();

                                //    break;
                                //}
                            }
                            row = cursor.NextRow();
                        }
                    }
                    else
                    {
                        MessageBox.Show("tableSetting为空！");
                    }
                }

                //ClsCoreUpdateFun clsCoreUpdateFun = new ClsCoreUpdateFun();
                ClsPolygonMatch clsPolygonMatch = new ClsPolygonMatch();
                //面积重叠比匹配
                if (this.tabItemAreaOverlap.Visible)
                {
                    if (radioButtonAreaAgo.Checked)
                    {
                        //填充结果表xxx_DifPyTable
                        clsPolygonMatch.DifScaleSearchChangedPolygonFeatures(srcFeatCls, tarFeatCls, table, buffer, minArea, progressBarMain, progressBarSub, labelXStatus);
                        sw.Stop();
                        MessageBoxEx.Show("面积重叠匹配已完成！总需要时间" + sw.ElapsedMilliseconds + "ms", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (radioButtonAreaNow.Checked)
                    {
                        //填充结果表xxx_DifPyTable
                        clsPolygonMatch.SearchChangedPolygonFeaturesArea(srcFeatCls, tarFeatCls, table, buffer, minArea, progressBarMain, progressBarSub, labelXStatus);
                        sw.Stop();
                        MessageBoxEx.Show("面积重叠匹配已完成！总需要时间" + sw.ElapsedMilliseconds + "ms", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (radioButtonArea2.Checked)
                    {
                        //填充结果表xxx_DifPyTable
                        clsPolygonMatch.SearchChangedPolygonFeaturesArea2(srcFeatCls, tarFeatCls, table, buffer, minArea, progressBarMain, progressBarSub, labelXStatus);
                        sw.Stop();
                        MessageBoxEx.Show("面积重叠匹配已完成！总需要时间" + sw.ElapsedMilliseconds + "ms", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (radioButtonAreaDirec.Checked)
                    {
                        //填充结果表xxx_DifPyTable
                        clsPolygonMatch.SearchChangedPolygonFeaturesAreaDirec(srcFeatCls, tarFeatCls, table, buffer, minArea, progressBarMain, progressBarSub, labelXStatus);
                        sw.Stop();
                        MessageBoxEx.Show("面积重叠匹配已完成！总需要时间" + sw.ElapsedMilliseconds + "ms", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                   
                }
                else
                {

                }
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.Close();

        }
        #endregion
        /// <summary>
        /// 返回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonXPrevious_Click(object sender, EventArgs e)
        {
            if (previousForm != null)
            {
                if (!previousForm.Visible)
                {
                    previousForm.Visible = true;
                }
                previousForm.Activate();
                this.Close();
            }
        }
        /// <summary>
        /// 创建表
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="tableName"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public ITable CreateTable(IWorkspace2 workspace, string tableName, IFields fields)
        {
            UID uid = new UIDClass();
            if (workspace == null)
                return null;
            IFeatureWorkspace featureWorkspace = (IFeatureWorkspace)workspace;
            ITable table;

            if (workspace.get_NameExists(esriDatasetType.esriDTTable, tableName))
            {
                table = featureWorkspace.OpenTable(tableName);
                return table;
            }
            uid.Value = "esriGeoDatabase.Object";

            if (fields == null)
            {
                return null;
            }
            IFieldChecker fieldChecker = new FieldCheckerClass();
            IEnumFieldError enumFieldError = null;
            IFields validatedFields = null;
            fieldChecker.ValidateWorkspace = (IWorkspace)workspace;
            fieldChecker.Validate(fields, out enumFieldError, out validatedFields);

            table = featureWorkspace.CreateTable(tableName, validatedFields, uid, null, "");

            return table;
        }
        /// <summary>
        /// 设置匹配多边形字段
        /// </summary>
        /// <param name="workspace">根据工作图层路径打开的工具空间</param>
        /// <returns></returns>
        public IFields CreateMatchPolygonTFields(IWorkspace2 workspace)
        {
            IObjectClassDescription objectClassDescription = new ObjectClassDescriptionClass();
            IFields fields = new FieldsClass();
            IFieldsEdit fieldsEdit = fields as IFieldsEdit;
            fields = objectClassDescription.RequiredFields;

            //test
            //false
            bool test = ClsDeclare.g_SameScaleMatch && !ClsDeclare.g_DifScaleMatch;
            //true
            bool test2 = !ClsDeclare.g_SameScaleMatch && ClsDeclare.g_DifScaleMatch;

            //同一比例尺的匹配
            if (ClsDeclare.g_SameScaleMatch && !ClsDeclare.g_DifScaleMatch)
            {
                IField field = new FieldClass();
                IFieldEdit fieldEdit = field as IFieldEdit;
                fieldEdit.Name_2 = "OBJECTID";
                fieldEdit.Type_2 = esriFieldType.esriFieldTypeOID;
                fieldEdit.IsNullable_2 = false;
                fieldsEdit.AddField(field);

                IField field1 = new FieldClass();
                IFieldEdit fieldEdit1 = field1 as IFieldEdit;
                fieldEdit1.Name_2 = "源OID";
                fieldEdit1.Type_2 = esriFieldType.esriFieldTypeString;
                fieldEdit1.IsNullable_2 = true;
                fieldEdit1.AliasName_2 = "源OID";
                fieldEdit1.Editable_2 = true;
                fieldsEdit.AddField(field1);

                IField field2 = new FieldClass();
                IFieldEdit fieldEdit2 = field2 as IFieldEdit;
                fieldEdit2.Name_2 = "待更新OID";
                fieldEdit2.Type_2 = esriFieldType.esriFieldTypeString;
                fieldEdit2.IsNullable_2 = true;
                fieldEdit2.AliasName_2 = "待更新OID";
                fieldEdit2.Editable_2 = true;
                fieldsEdit.AddField(field2);

                IField field3 = new FieldClass();
                IFieldEdit fieldEdit3 = field3 as IFieldEdit;
                fieldEdit3.Name_2 = "变化标记";
                fieldEdit3.Type_2 = esriFieldType.esriFieldTypeString;
                fieldEdit3.IsNullable_2 = true;
                fieldEdit3.AliasName_2 = "变化标记";
                fieldEdit3.Editable_2 = true;
                fieldsEdit.AddField(field3);

                //    IField field4 = new FieldClass();
                //    IFieldEdit fieldEdit4 = field4 as IFieldEdit;
                //    fieldEdit4.Name_2 = "源编码";
                //    fieldEdit4.Type_2 = esriFieldType.esriFieldTypeString;
                //    fieldEdit4.IsNullable_2 = true;
                //    fieldEdit4.AliasName_2 = "源编码";
                //    fieldEdit4.Editable_2 = true;
                //    fieldsEdit.AddField(field4);



                //    IField field5 = new FieldClass();
                //    IFieldEdit fieldEdit5 = field5 as IFieldEdit;
                //    fieldEdit5.Name_2 = "更新编码";
                //    fieldEdit5.Type_2 = esriFieldType.esriFieldTypeString;
                //    fieldEdit5.IsNullable_2 = true;
                //    fieldEdit5.AliasName_2 = "更新编码";
                //    fieldEdit5.Editable_2 = true;
                //    fieldsEdit.AddField(field5);


                IField field4 = new FieldClass();
                IFieldEdit fieldEdit4 = field4 as IFieldEdit;
                fieldEdit4.Name_2 = "位置相似度";
                fieldEdit4.Type_2 = esriFieldType.esriFieldTypeString;
                fieldEdit4.IsNullable_2 = true;
                fieldEdit4.AliasName_2 = "位置相似度";
                fieldEdit4.Editable_2 = true;
                fieldsEdit.AddField(field4);

                IField field5 = new FieldClass();
                IFieldEdit fieldEdit5 = field5 as IFieldEdit;
                fieldEdit5.Name_2 = "面积相似度";
                fieldEdit5.Type_2 = esriFieldType.esriFieldTypeString;
                fieldEdit5.IsNullable_2 = true;
                fieldEdit5.AliasName_2 = "面积相似度";
                fieldEdit5.Editable_2 = true;
                fieldsEdit.AddField(field5);

                IField field6 = new FieldClass();
                IFieldEdit fieldEdit6 = field6 as IFieldEdit;
                fieldEdit6.Name_2 = "形状相似度";
                fieldEdit6.Type_2 = esriFieldType.esriFieldTypeString;
                fieldEdit6.IsNullable_2 = true;
                fieldEdit6.AliasName_2 = "形状相似度";
                fieldEdit6.Editable_2 = true;
                fieldsEdit.AddField(field6);

                IField field7 = new FieldClass();
                IFieldEdit fieldEdit7 = field7 as IFieldEdit;
                fieldEdit7.Name_2 = "综合相似度";
                fieldEdit7.Type_2 = esriFieldType.esriFieldTypeString;
                fieldEdit7.IsNullable_2 = true;
                fieldEdit7.AliasName_2 = "综合相似度";
                fieldEdit7.Editable_2 = true;
                fieldsEdit.AddField(field7);
            }
            //不同比例尺的匹配
            else if (!ClsDeclare.g_SameScaleMatch && ClsDeclare.g_DifScaleMatch)
            {
                IField field = new FieldClass();
                IFieldEdit fieldEdit = field as IFieldEdit;
                fieldEdit.Name_2 = "OBJECTID";
                fieldEdit.Type_2 = esriFieldType.esriFieldTypeOID;
                fieldEdit.IsNullable_2 = false;
                fieldsEdit.AddField(field);

                IField field1 = new FieldClass();
                IFieldEdit fieldEdit1 = field1 as IFieldEdit;
                fieldEdit1.Name_2 = "源OID";
                fieldEdit1.Type_2 = esriFieldType.esriFieldTypeString;
                fieldEdit1.IsNullable_2 = true;
                fieldEdit1.AliasName_2 = "源OID";
                fieldEdit1.Editable_2 = true;
                fieldsEdit.AddField(field1);

                IField field2 = new FieldClass();
                IFieldEdit fieldEdit2 = field2 as IFieldEdit;
                //fieldEdit2.Name_2 = "待更新OID";
                fieldEdit2.Name_2 = "待匹配OID";
                fieldEdit2.Type_2 = esriFieldType.esriFieldTypeString;
                fieldEdit2.IsNullable_2 = true;
                //fieldEdit2.AliasName_2 = "待更新OID";
                fieldEdit2.AliasName_2 = "待匹配OID";
                fieldEdit2.Editable_2 = true;
                fieldsEdit.AddField(field2);

                IField field4 = new FieldClass();
                IFieldEdit fieldEdit4 = field4 as IFieldEdit;
                //fieldEdit4.Name_2 = "源编码";
                fieldEdit4.Name_2 = "源要素分类码";
                fieldEdit4.Type_2 = esriFieldType.esriFieldTypeString;
                fieldEdit4.IsNullable_2 = true;
                //fieldEdit4.AliasName_2 = "源编码";
                fieldEdit4.AliasName_2 = "源要素分类码";
                fieldEdit4.Editable_2 = true;
                fieldsEdit.AddField(field4);

                IField field5 = new FieldClass();
                IFieldEdit fieldEdit5 = field5 as IFieldEdit;
                //fieldEdit5.Name_2 = "更新编码";
                fieldEdit5.Name_2 = "待匹配要素分类码";
                fieldEdit5.Type_2 = esriFieldType.esriFieldTypeString;
                fieldEdit5.IsNullable_2 = true;
                //fieldEdit5.AliasName_2 = "更新编码";
                fieldEdit5.AliasName_2 = "待匹配要素分类码";
                fieldEdit5.Editable_2 = true;
                fieldsEdit.AddField(field5);

                IField field3 = new FieldClass();
                IFieldEdit fieldEdit3 = field3 as IFieldEdit;
                fieldEdit3.Name_2 = "变化标记";
                fieldEdit3.Type_2 = esriFieldType.esriFieldTypeString;
                fieldEdit3.IsNullable_2 = true;
                fieldEdit3.AliasName_2 = "变化标记";

                fieldEdit3.Editable_2 = true;
                fieldsEdit.AddField(field3);
            }
            fields = fieldsEdit as IFields;

            return fields;
        }
        /// <summary>
        /// 创建反向匹配多边形字段
        /// </summary>
        /// <param name="workspace"></param>
        /// <returns></returns>
        public IFields CreateReverseMatchPolygonTFields(IWorkspace2 workspace)
        {
            IObjectClassDescription objectClassDescription = new ObjectClassDescriptionClass();
            IFields fields = new FieldsClass();
            IFieldsEdit fieldsEdit = fields as IFieldsEdit;
            fields = objectClassDescription.RequiredFields;

            IField field = new FieldClass();
            IFieldEdit fieldEdit = field as IFieldEdit;
            fieldEdit.Name_2 = "OBJECTID";
            fieldEdit.Type_2 = esriFieldType.esriFieldTypeOID;
            fieldEdit.IsNullable_2 = false;
            fieldsEdit.AddField(field);

            IField field1 = new FieldClass();
            IFieldEdit fieldEdit1 = field1 as IFieldEdit;
            fieldEdit1.Name_2 = "待更新OID";
            fieldEdit1.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit1.IsNullable_2 = true;
            fieldEdit1.AliasName_2 = "待更新OID";
            fieldEdit1.Editable_2 = true;
            fieldsEdit.AddField(field1);

            IField field2 = new FieldClass();
            IFieldEdit fieldEdit2 = field2 as IFieldEdit;
            fieldEdit2.Name_2 = "源OID";
            fieldEdit2.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit2.IsNullable_2 = true;
            fieldEdit2.AliasName_2 = "源OID";
            fieldEdit2.Editable_2 = true;
            fieldsEdit.AddField(field2);

            IField field3 = new FieldClass();
            IFieldEdit fieldEdit3 = field3 as IFieldEdit;
            fieldEdit3.Name_2 = "变化标记";
            fieldEdit3.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit3.IsNullable_2 = true;
            fieldEdit3.AliasName_2 = "变化标记";
            fieldEdit3.Editable_2 = true;
            fieldsEdit.AddField(field3);

            //    IField field4 = new FieldClass();
            //    IFieldEdit fieldEdit4 = field4 as IFieldEdit;
            //    fieldEdit4.Name_2 = "源编码";
            //    fieldEdit4.Type_2 = esriFieldType.esriFieldTypeString;
            //    fieldEdit4.IsNullable_2 = true;
            //    fieldEdit4.AliasName_2 = "源编码";
            //    fieldEdit4.Editable_2 = true;
            //    fieldsEdit.AddField(field4);

            //    IField field5 = new FieldClass();
            //    IFieldEdit fieldEdit5 = field5 as IFieldEdit;
            //    fieldEdit5.Name_2 = "更新编码";
            //    fieldEdit5.Type_2 = esriFieldType.esriFieldTypeString;
            //    fieldEdit5.IsNullable_2 = true;
            //    fieldEdit5.AliasName_2 = "更新编码";
            //    fieldEdit5.Editable_2 = true;
            //    fieldsEdit.AddField(field5);

            IField field4 = new FieldClass();
            IFieldEdit fieldEdit4 = field4 as IFieldEdit;
            fieldEdit4.Name_2 = "位置相似度";
            fieldEdit4.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit4.IsNullable_2 = true;
            fieldEdit4.AliasName_2 = "位置相似度";
            fieldEdit4.Editable_2 = true;
            fieldsEdit.AddField(field4);

            IField field5 = new FieldClass();
            IFieldEdit fieldEdit5 = field5 as IFieldEdit;
            fieldEdit5.Name_2 = "面积相似度";
            fieldEdit5.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit5.IsNullable_2 = true;
            fieldEdit5.AliasName_2 = "面积相似度";
            fieldEdit5.Editable_2 = true;
            fieldsEdit.AddField(field5);

            IField field6 = new FieldClass();
            IFieldEdit fieldEdit6 = field6 as IFieldEdit;
            fieldEdit6.Name_2 = "形状相似度";
            fieldEdit6.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit6.IsNullable_2 = true;
            fieldEdit6.AliasName_2 = "形状相似度";
            fieldEdit6.Editable_2 = true;
            fieldsEdit.AddField(field6);

            IField field7 = new FieldClass();
            IFieldEdit fieldEdit7 = field7 as IFieldEdit;
            fieldEdit7.Name_2 = "综合相似度";
            fieldEdit7.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit7.IsNullable_2 = true;
            fieldEdit7.AliasName_2 = "综合相似度";
            fieldEdit7.Editable_2 = true;
            fieldsEdit.AddField(field7);
            fields = fieldsEdit as IFields;
            return fields;
        }

        /// <summary>
        /// 设置匹配参数
        /// </summary>
        /// <param name="gdbPath">待更新数据的存放路径</param>
        /// <param name="targetFeatureName">工作图层的名称（待更新图层的名称）</param>
        /// <param name="sourceFeatCls">源数据</param>
        private void loadFeatSetting(string gdbPath, string targetFeatureName, IFeatureClass sourceFeatCls)
        {
            if ((targetFeatureName != "") && (gdbPath != ""))
            {
                IWorkspaceFactory pWorkspaceFactory = new FileGDBWorkspaceFactory();
                IWorkspace2 workspace = pWorkspaceFactory.OpenFromFile(gdbPath, 0) as IWorkspace2;
                IFeatureWorkspace featureWorkspace = workspace as IFeatureWorkspace;
                ITable table = null;
                IFields fields = null;

                if (sourceFeatCls.ShapeType == esriGeometryType.esriGeometryPolygon)
                {
                    table = TableExist(workspace, table, featureWorkspace);

                    IWorkspaceEdit pWorkspaceEdit = featureWorkspace as IWorkspaceEdit;
                    pWorkspaceEdit.StartEditing(true);
                    pWorkspaceEdit.StartEditOperation();

                    Dictionary<string, int> pDic = new Dictionary<string, int>();
                    //查看是否MatchedFCName字段是否存在
                    int indexTargetFeatCls = table.FindField("MatchedFCName");
                    ICursor pCursor = table.Search(null, false);
                    IRow pRow = pCursor.NextRow();
                    while (pRow != null)
                    {
                        if (pRow.get_Value(indexTargetFeatCls).ToString() != "")
                        {
                            pDic.Add(pRow.get_Value(indexTargetFeatCls).ToString(), pRow.OID);
                        }
                        pRow = pCursor.NextRow();
                    }

                    //设置面积阈值
                    double minArea = 0;
                    if (this.textBoxminArea.Text != "")
                    {
                        string text = this.textBoxminArea.Text;
                        if (text.Contains("平方米"))
                        {
                            text = text.Substring(0, text.LastIndexOf("平方米")).Trim();
                        }
                        else
                        {
                            text = text.Trim();
                        }
                        minArea = Convert.ToDouble(text);
                    }
                    //面积重叠比匹配
                    if (this.tabItemAreaOverlap.Visible)
                    {
                        //表MatchedPolygonFCSettingDif不存在
                        if (!pDic.ContainsKey(targetFeatureName))
                        {
                            IRow tempRow = table.CreateRow();
                            tempRow.set_Value(indexTargetFeatCls, targetFeatureName);
                            tempRow.set_Value(tempRow.Fields.FindField("FCSettingID"), table.RowCount(null) - 1);
                            tempRow.set_Value(tempRow.Fields.FindField("Voronoi"), 0);
                            tempRow.set_Value(tempRow.Fields.FindField("MinArea"), minArea);

                            IDataset dataset = sourceFeatCls as IDataset;
                            if (ClsDeclare.g_SourceFeatClsPathDic.ContainsKey(dataset.Name))
                            {
                                tempRow.set_Value(tempRow.Fields.FindField("SourceFCName"), dataset.Name);
                                tempRow.set_Value(tempRow.Fields.FindField("SourcePath"), ClsDeclare.g_SourceFeatClsPathDic[dataset.Name]);
                                tempRow.set_Value(tempRow.Fields.FindField("WorkspacePath"), ClsDeclare.g_WorkspacePath);
                            }
                            tempRow.set_Value(tempRow.Fields.FindField("Buffer"), Convert.ToDouble(this.labelBuffer.Text));
                            tempRow.set_Value(tempRow.Fields.FindField("AreaNum"), Convert.ToDouble(this.labelArea.Text));



                            tempRow.Store();
                        }
                        //表MatchedPolygonFCSettingDif存在
                        else
                        {
                            IRow tRow = table.GetRow(pDic[targetFeatureName]);

                            tRow.set_Value(tRow.Fields.FindField("Voronoi"), 0);
                            tRow.set_Value(tRow.Fields.FindField("MinArea"), minArea);

                            IDataset dataset = sourceFeatCls as IDataset;
                            if (ClsDeclare.g_SourceFeatClsPathDic.ContainsKey(dataset.Name))
                            {

                                tRow.set_Value(tRow.Fields.FindField("SourceFCName"), dataset.Name);
                                tRow.set_Value(tRow.Fields.FindField("SourcePath"), ClsDeclare.g_SourceFeatClsPathDic[dataset.Name]);
                                tRow.set_Value(tRow.Fields.FindField("WorkspacePath"), ClsDeclare.g_WorkspacePath);
                            }
                            tRow.set_Value(tRow.Fields.FindField("Buffer"), Convert.ToDouble(this.labelBuffer.Text));
                            tRow.set_Value(tRow.Fields.FindField("AreaNum"), Convert.ToDouble(this.labelArea.Text));

                            tRow.Store();
                        }
                    }

                    pWorkspaceEdit.StopEditOperation();
                    pWorkspaceEdit.StopEditing(true);
                }
            }
        }
        /// <summary>
        /// 对表MatchedPolygonFCSettingDif进行判断处理
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="table"></param>
        /// <param name="featureWorkspace"></param>
        /// <returns></returns>
        private ITable TableExist(IWorkspace2 workspace, ITable table, IFeatureWorkspace featureWorkspace)
        {
            IFields fields;
            //匹配参数表是否存在，存在则打开表并删除相关记录
            if (workspace.get_NameExists(esriDatasetType.esriDTTable, ClsConstant.polygonSettingTable))
            {
                table = featureWorkspace.OpenTable(ClsConstant.polygonSettingTable);
                IWorkspaceEdit workspaceEdit = workspace as IWorkspaceEdit;
                workspaceEdit.StartEditing(true);
                workspaceEdit.StartEditOperation();

                ICursor cursor = table.Search(null, false);
                IRow r = cursor.NextRow();
                while (r != null)
                {
                    r.Delete();
                    r = cursor.NextRow();
                }
                workspaceEdit.StopEditOperation();
                workspaceEdit.StopEditing(true);

                //ClsDeleteTables.DeleteFeatureClass(gdbPath, ClsConstant.lineSettingTable);
                //fields = CreateMatchedPolylineFCSettingFields(workspace);
                //UID uid = new UIDClass();
                //uid.Value = "esriGeoDatabase.Object";
                //IFieldChecker fieldChecker = new FieldCheckerClass();
                //IEnumFieldError enumFieldError = null;
                //IFields validatedFields = null;
                //fieldChecker.ValidateWorkspace = (IWorkspace)workspace;
                //fieldChecker.Validate(fields, out enumFieldError, out validatedFields);

                //table = featureWorkspace.CreateTable(ClsConstant.lineSettingTable, validatedFields, uid, null, "");
            }
            //匹配参数表是否存在，不存在则创建表
            else
            {
                //返回MatchedPolygonFCSettingDif的所有字段集
                fields = CreateMatchedPolygonFCSettingFields(workspace);
                UID uid = new UIDClass();
                uid.Value = "esriGeoDatabase.Object";
                IFieldChecker fieldChecker = new FieldCheckerClass();
                IEnumFieldError enumFieldError = null;
                IFields validatedFields = null;
                fieldChecker.ValidateWorkspace = (IWorkspace)workspace;
                fieldChecker.Validate(fields, out enumFieldError, out validatedFields);

                //创建表MatchedPolygonFCSettingDif
                table = featureWorkspace.CreateTable(ClsConstant.polygonSettingTable, validatedFields, uid, null, "");
            }
            return table;
        }

        /// <summary>
        /// 设置最后生成的表MatchedPolygonFCSettingDif的字段
        /// </summary>
        /// <param name="workspace"></param>
        /// <returns></returns>
        private IFields CreateMatchedPolygonFCSettingFields(IWorkspace2 workspace)
        {
            IObjectClassDescription objectClassDescription = new ObjectClassDescriptionClass();
            IFields fields = new FieldsClass();
            IFieldsEdit fieldsEdit = fields as IFieldsEdit;
            fields = objectClassDescription.RequiredFields;

            IField field = new FieldClass();
            IFieldEdit fieldEdit = field as IFieldEdit;
            fieldEdit.Name_2 = "OBJECTID";
            fieldEdit.Type_2 = esriFieldType.esriFieldTypeOID;
            fieldEdit.IsNullable_2 = false;
            fieldsEdit.AddField(field);

            IField field16 = new FieldClass();
            IFieldEdit fieldEdit16 = field16 as IFieldEdit;
            fieldEdit16.Name_2 = "SourceFCName";
            fieldEdit16.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit16.IsNullable_2 = false;
            fieldEdit16.AliasName_2 = "源图层名称";
            fieldsEdit.AddField(field16);

            IField field17 = new FieldClass();
            IFieldEdit fieldEdit17 = field17 as IFieldEdit;
            fieldEdit17.Name_2 = "SourcePath";
            fieldEdit17.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit17.IsNullable_2 = false;
            fieldEdit17.AliasName_2 = "源图层路径";
            fieldsEdit.AddField(field17);


            IField field1 = new FieldClass();
            IFieldEdit fieldEdit1 = field1 as IFieldEdit;
            fieldEdit1.Name_2 = "MatchedFCName";
            fieldEdit1.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit1.IsNullable_2 = false;
            //fieldEdit1.AliasName_2 = "更新图层名";
            fieldEdit1.AliasName_2 = "待匹配图层名称";
            fieldsEdit.AddField(field1);

            IField field18 = new FieldClass();
            IFieldEdit fieldEdit18 = field18 as IFieldEdit;
            fieldEdit18.Name_2 = "WorkspacePath";
            fieldEdit18.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit18.IsNullable_2 = false;
            //fieldEdit18.AliasName_2 = "工作区路径";
            fieldEdit18.AliasName_2 = "待匹配图层路径";
            fieldsEdit.AddField(field18);

            IField field2 = new FieldClass();
            IFieldEdit fieldEdit2 = field2 as IFieldEdit;
            fieldEdit2.Name_2 = "FCSettingID";
            fieldEdit2.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit2.IsNullable_2 = false;
            fieldEdit2.AliasName_2 = "参数ID";
            fieldsEdit.AddField(field2);

            IField field3 = new FieldClass();
            IFieldEdit fieldEdit3 = field3 as IFieldEdit;
            fieldEdit3.Name_2 = "Center";
            fieldEdit3.Type_2 = esriFieldType.esriFieldTypeInteger;
            fieldEdit3.AliasName_2 = "重心点匹配";
            fieldEdit3.IsNullable_2 = true;
            fieldsEdit.AddField(field3);

            IField field4 = new FieldClass();
            IFieldEdit fieldEdit4 = field4 as IFieldEdit;
            fieldEdit4.Name_2 = "CenterNum";
            fieldEdit4.Type_2 = esriFieldType.esriFieldTypeDouble;
            fieldEdit4.AliasName_2 = "重心点阈值";
            fieldEdit4.IsNullable_2 = true;
            fieldsEdit.AddField(field4);

            IField field25 = new FieldClass();
            IFieldEdit fieldEdit25 = field25 as IFieldEdit;
            fieldEdit25.Name_2 = "CenterWT";
            fieldEdit25.Type_2 = esriFieldType.esriFieldTypeDouble;
            fieldEdit25.AliasName_2 = "重心点权重";
            fieldEdit25.IsNullable_2 = true;
            fieldsEdit.AddField(field25);

            IField field6 = new FieldClass();
            IFieldEdit fieldEdit6 = field6 as IFieldEdit;
            fieldEdit6.Name_2 = "Area";
            fieldEdit6.Type_2 = esriFieldType.esriFieldTypeInteger;
            fieldEdit6.AliasName_2 = "面积匹配";
            fieldEdit6.IsNullable_2 = true;
            fieldsEdit.AddField(field6);

            IField field7 = new FieldClass();
            IFieldEdit fieldEdit7 = field7 as IFieldEdit;
            fieldEdit7.Name_2 = "AreaNum";
            fieldEdit7.Type_2 = esriFieldType.esriFieldTypeDouble;
            fieldEdit7.AliasName_2 = "面积阈值";
            fieldEdit7.IsNullable_2 = true;
            fieldsEdit.AddField(field7);

            IField field8 = new FieldClass();
            IFieldEdit fieldEdit8 = field8 as IFieldEdit;
            fieldEdit8.Name_2 = "AreaWT";
            fieldEdit8.Type_2 = esriFieldType.esriFieldTypeDouble;
            fieldEdit8.AliasName_2 = "面积权重";
            fieldEdit8.IsNullable_2 = true;
            fieldsEdit.AddField(field8);


            IField field9 = new FieldClass();
            IFieldEdit fieldEdit9 = field9 as IFieldEdit;
            fieldEdit9.Name_2 = "SP";
            fieldEdit9.Type_2 = esriFieldType.esriFieldTypeInteger;
            fieldEdit9.AliasName_2 = "形状匹配";
            fieldEdit9.IsNullable_2 = true;
            fieldsEdit.AddField(field9);

            IField field10 = new FieldClass();
            IFieldEdit fieldEdit10 = field10 as IFieldEdit;
            fieldEdit10.Name_2 = "SPNum";
            fieldEdit10.Type_2 = esriFieldType.esriFieldTypeDouble;
            fieldEdit10.AliasName_2 = "形状阈值";
            fieldEdit10.IsNullable_2 = true;
            fieldsEdit.AddField(field10);

            IField field11 = new FieldClass();
            IFieldEdit fieldEdit11 = field11 as IFieldEdit;
            fieldEdit11.Name_2 = "SPWT";
            fieldEdit11.Type_2 = esriFieldType.esriFieldTypeDouble;
            fieldEdit11.AliasName_2 = "形状权重";
            fieldEdit11.IsNullable_2 = true;
            fieldsEdit.AddField(field11);

            IField field12 = new FieldClass();
            IFieldEdit fieldEdit12 = field12 as IFieldEdit;
            fieldEdit12.Name_2 = "Top";
            fieldEdit12.Type_2 = esriFieldType.esriFieldTypeInteger;
            fieldEdit12.AliasName_2 = "拓扑匹配";
            fieldEdit12.IsNullable_2 = true;
            fieldsEdit.AddField(field12);

            IField field13 = new FieldClass();
            IFieldEdit fieldEdit13 = field13 as IFieldEdit;
            fieldEdit13.Name_2 = "Buffer";
            fieldEdit13.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit13.AliasName_2 = "拓扑缓冲区半径";
            fieldEdit13.IsNullable_2 = true;
            fieldsEdit.AddField(field13);

            IField field14 = new FieldClass();
            IFieldEdit fieldEdit14 = field14 as IFieldEdit;
            fieldEdit14.Name_2 = "Attribute";
            fieldEdit14.Type_2 = esriFieldType.esriFieldTypeInteger;
            fieldEdit14.AliasName_2 = "属性匹配";
            fieldEdit14.IsNullable_2 = true;
            fieldsEdit.AddField(field14);

            IField field15 = new FieldClass();
            IFieldEdit fieldEdit15 = field15 as IFieldEdit;
            fieldEdit15.Name_2 = "MatchedFields";
            fieldEdit15.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit15.AliasName_2 = "匹配属性字段";
            fieldEdit15.IsNullable_2 = true;
            fieldsEdit.AddField(field15);

            IField field19 = new FieldClass();
            IFieldEdit fieldEdit19 = field19 as IFieldEdit;
            fieldEdit19.Name_2 = "Voronoi";
            fieldEdit19.Type_2 = esriFieldType.esriFieldTypeInteger;
            fieldEdit19.AliasName_2 = "泰森多边形";
            fieldEdit19.IsNullable_2 = true;
            fieldsEdit.AddField(field19);

            IField field5 = new FieldClass();
            IFieldEdit fieldEdit5 = field5 as IFieldEdit;
            fieldEdit5.Name_2 = "MinArea";
            fieldEdit5.Type_2 = esriFieldType.esriFieldTypeDouble;
            fieldEdit5.AliasName_2 = "最小面积值";
            fieldEdit5.IsNullable_2 = true;
            fieldsEdit.AddField(field5);

            fields = fieldsEdit as IFieldsEdit;
            return fields;
        }
        #region slider滑动事件
        /// <summary>
        /// 缓冲区半径
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sliderBuffer_ValueChanged(object sender, EventArgs e)
        {
            this.labelBuffer.Text = (Convert.ToDouble(this.sliderBuffer.Value)).ToString();
        }
        /// <summary>
        /// 面积值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sliderArea_ValueChanged(object sender, EventArgs e)
        {
            this.labelArea.Text = (Convert.ToDouble((this.sliderArea.Value / 100.00))).ToString();
        }
        #endregion


    }
}
