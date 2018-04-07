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
using System.Diagnostics;
//using System.Text.RegularExpressions;
namespace ZJGISDataUpdating
{
    public partial class FrmMatchLineDif : DevComponents.DotNetBar.Office2007Form
    {
        Form previousForm;
        Dictionary<int, DataGridViewRow> m_InRowDic;
        Dictionary<int, DataGridViewRow> m_OutRowDic;
        //Dictionary<int, Dictionary<string, IFeatureClass>> m_FeatClsDic;



        public FrmMatchLineDif()
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

        private void FrmMatch_Load(object sender, EventArgs e)
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
            this.dataGridViewX1.Columns[2].Width = Convert.ToInt32(width * 0.4);
            this.dataGridViewX1.Columns[2].ReadOnly = true;

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
                    //工作层表
                    string table = m_OutRowDic[i].Cells[1].Value.ToString();
                    //由于工作层有_TE,结果表应该是待更新层_Table
                    // dgvRow.Cells[3].Value = table.Substring(0, table.LastIndexOf("_")) + "_Table";
                    dgvRow.Cells[3].Value = table + "_LnTable";


                }
            }

            //设置页面初始化
            IFeatureClass sourceFeatCls = this.dataGridViewX1[1, 0].Tag as IFeatureClass;
            IWorkspaceFactory2 pWorkspaceFactory = new FileGDBWorkspaceFactoryClass() as IWorkspaceFactory2;
            string path = ClsDeclare.g_WorkspacePath;
            //path需要GDB
            IWorkspace2 pWorkspace = pWorkspaceFactory.OpenFromFile(path, 0) as IWorkspace2;
            IFeatureWorkspace featureWorkspace = pWorkspace as IFeatureWorkspace;
            IFeatureClass targetFeatCls = featureWorkspace.OpenFeatureClass(this.dataGridViewX1[2, 0].Value.ToString());

            Initload(sourceFeatCls, targetFeatCls);
        }
        /// <summary>
        /// 初始化匹配界面设置
        /// </summary>
        /// <param name="sourceFeatCls"></param>
        /// <param name="targetFeatCls"></param>
        private void Initload(IFeatureClass sourceFeatCls, IFeatureClass targetFeatCls)
        {
            if (this.comboBoxExBuffer.Items.Count > 0)
            {
                this.comboBoxExBuffer.Text = this.comboBoxExBuffer.Items[3].ToString();
            }

            //tab显示设置
            //this.tabControl1.Tabs[0].Visible = true;
            //this.tabControl1.Tabs[1].Visible = false;
            ////this.tabControl1.Tabs[1].Visible = true;
            //this.tabControl1.Tabs[2].Visible = true;
            this.tabItemGeo.Visible = true;
            this.tabItemGAGeo.Visible = false;
            this.tabItemGAttr.Visible = false;
            this.tabItemTopo.Visible = false;

            //权重界面设置
            this.radioButtonShape.Checked = true;
            this.radioButtonNode.Checked = false;
            this.radioButtonBuffer.Checked = false;

            this.rBtnShapeGeo.Checked = true;
            this.rBtnNodeGeo.Checked = false;
            this.rBtnbufferGeo.Checked = false;

            //进度条设置
            this.WeightsliderNode.Enabled = false;
            this.WeightsliderBuffer.Enabled = false;
            this.WeightsliderTotal.Enabled = true;

            this.sliderNodeGeo.Enabled = false;
            this.sliderBufferGeo.Enabled = false;
            this.sliderTotalGeo.Enabled = true;

            //label设置
            this.labelNode.Enabled = false;
            this.labelTotal.Enabled = true;

            this.labelNodeGeo.Enabled = false;
            this.labelTotalGeo.Enabled = true;

            DataGridViewCheckBoxColumn dgvCheckColumn = new DataGridViewCheckBoxColumn();
            dgvCheckColumn.Name = "CheckBoxColumn";
            dgvCheckColumn.HeaderText = "";
            dgvCheckColumn.ReadOnly = false;
            dgvCheckColumn.Width = dataGridViewX2.Width / 8;
            dataGridViewX2.Columns.Add(dgvCheckColumn);

            dataGridViewX2.Columns.Add("SourceLayer", "源图层字段");
            dataGridViewX2.Columns[1].Width = dataGridViewX2.Width / 3;
            dataGridViewX2.Columns[1].ReadOnly = true;

            DataGridViewComboBoxColumn dgvcbColumn = new DataGridViewComboBoxColumn();
            dgvcbColumn.Name = "TargetLayer";
            dgvcbColumn.HeaderText = "更新图层字段";
            dgvcbColumn.Width = dataGridViewX2.Width / 3;
            dataGridViewX2.Columns.Add(dgvcbColumn);
            LoadFields(sourceFeatCls, targetFeatCls);
        }
        //TODO :初始化datagridview中的值，全用名称不用别名
        /// <summary>
        /// 填充DataGridView2，用的name字段
        /// </summary>
        /// <param name="sourceFeatCls"></param>
        /// <param name="targetFeatCls"></param>
        private void LoadFields(IFeatureClass sourceFeatCls, IFeatureClass targetFeatCls)
        {
            IFields2 sourceFields = sourceFeatCls.Fields as IFields2;
            IFields2 targetFields = targetFeatCls.Fields as IFields2;
            DataGridViewRow dgvRow;

            dataGridViewX2.Rows.Clear();

            //bool bsource = false;
            //bool btarget = false;

            //for (int j = 0; j < sourceFields.FieldCount; j++)
            //{
            //    if (sourceFields.get_Field(j).AliasName.Trim().Length > 0)
            //    {
            //        bsource = true;
            //    }
            //}
            //for (int j = 0; j < targetFields.FieldCount; j++)
            //{
            //    if (targetFields.get_Field(j).AliasName.Trim().Length > 0)
            //    {
            //        btarget = true;
            //    }
            //}

            for (int i = 0; i < sourceFields.FieldCount; i++)
            {
                if (sourceFields.get_Field(i).Type == esriFieldType.esriFieldTypeGeometry || sourceFields.get_Field(i).Editable == false)
                    continue;

                dgvRow = new DataGridViewRow();
                dgvRow = dataGridViewX2.Rows[dataGridViewX2.Rows.Add()];
                //如果源图层和待匹配图层都存在别名，则用别名
                //if (bsource && btarget)
                //{
                //    dgvRow.Cells[1].Value = sourceFields.get_Field(i).AliasName;

                //    DataGridViewCheckBoxCell dgvCheckBoxCell = new DataGridViewCheckBoxCell();
                //    dgvCheckBoxCell = dgvRow.Cells[0] as DataGridViewCheckBoxCell;

                //    DataGridViewComboBoxCell dgvComCell = new DataGridViewComboBoxCell();
                //    dgvComCell = dgvRow.Cells[2] as DataGridViewComboBoxCell;

                //    for (int j = 0; j < targetFields.FieldCount; j++)
                //    {
                //        if (targetFields.get_Field(j).Type == esriFieldType.esriFieldTypeGeometry || targetFields.get_Field(j).Editable == false)
                //            continue;
                //        dgvComCell.Items.Add(targetFields.get_Field(j).AliasName);

                //        if (targetFields.get_Field(j).AliasName == sourceFields.get_Field(i).AliasName)
                //        {
                //            dgvComCell.Value = targetFields.get_Field(j).AliasName;
                //            dgvCheckBoxCell.Value = false;
                //        }
                //    }
                //}
                //else
                //{
                //dgvRow.Cells[1].Value = sourceFields.get_Field(i).AliasName;
                dgvRow.Cells[1].Value = sourceFields.get_Field(i).Name;

                DataGridViewCheckBoxCell dgvCheckBoxCell = new DataGridViewCheckBoxCell();
                dgvCheckBoxCell = dgvRow.Cells[0] as DataGridViewCheckBoxCell;

                DataGridViewComboBoxCell dgvComCell = new DataGridViewComboBoxCell();
                dgvComCell = dgvRow.Cells[2] as DataGridViewComboBoxCell;

                for (int j = 0; j < targetFields.FieldCount; j++)
                {
                    if (targetFields.get_Field(j).Type == esriFieldType.esriFieldTypeGeometry || targetFields.get_Field(j).Editable == false)
                        continue;
                    //dgvComCell.Items.Add(targetFields.get_Field(j).AliasName);

                    //if (targetFields.get_Field(j).AliasName == sourceFields.get_Field(i).AliasName)
                    //{
                    //    dgvComCell.Value = targetFields.get_Field(j).AliasName;
                    //    dgvCheckBoxCell.Value = false;
                    //}
                    dgvComCell.Items.Add(targetFields.get_Field(j).Name);

                    if (targetFields.get_Field(j).Name == sourceFields.get_Field(i).Name)
                    {
                        dgvComCell.Value = targetFields.get_Field(j).Name;
                        dgvCheckBoxCell.Value = false;
                    }
                }
                //}
            }
        }

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
        /// 设置线匹配字段
        /// </summary>
        /// <param name="workspace"></param>
        /// <returns></returns>
        private IFields CreateMatchedPolylineFCSettingFields(IWorkspace2 workspace)
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
            fieldEdit1.Name_2 = "SourceFCName";
            fieldEdit1.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit1.IsNullable_2 = false;
            fieldEdit1.AliasName_2 = "源图层名称";
            fieldsEdit.AddField(field1);



            IField field2 = new FieldClass();
            IFieldEdit fieldEdit2 = field2 as IFieldEdit;
            fieldEdit2.Name_2 = "SourcePath";
            fieldEdit2.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit2.IsNullable_2 = false;
            fieldEdit2.AliasName_2 = "源图层路径";
            fieldsEdit.AddField(field2);



            IField field3 = new FieldClass();
            IFieldEdit fieldEdit3 = field3 as IFieldEdit;
            fieldEdit3.Name_2 = "MatchedFCName";
            fieldEdit3.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit3.IsNullable_2 = false;
            //fieldEdit3.AliasName_2 = "更新图层名";
            fieldEdit3.AliasName_2 = "待匹配层名";
            fieldsEdit.AddField(field3);

            IField field4 = new FieldClass();
            IFieldEdit fieldEdit4 = field4 as IFieldEdit;
            fieldEdit4.Name_2 = "WorkspacePath";
            fieldEdit4.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit4.IsNullable_2 = false;
            fieldEdit4.AliasName_2 = "工作区路径";
            fieldsEdit.AddField(field4);


            IField field5 = new FieldClass();
            IFieldEdit fieldEdit5 = field5 as IFieldEdit;
            fieldEdit5.Name_2 = "FCSettingID";
            fieldEdit5.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit5.IsNullable_2 = false;
            fieldEdit5.AliasName_2 = "参数ID";
            fieldsEdit.AddField(field5);

            IField field6 = new FieldClass();
            IFieldEdit fieldEdit6 = field6 as IFieldEdit;
            fieldEdit6.Name_2 = "MatchedPoints";
            fieldEdit6.Type_2 = esriFieldType.esriFieldTypeDouble;
            fieldEdit6.AliasName_2 = "节点匹配";
            fieldEdit6.IsNullable_2 = true;
            fieldsEdit.AddField(field6);



            IField field8 = new FieldClass();
            IFieldEdit fieldEdit8 = field8 as IFieldEdit;
            fieldEdit8.Name_2 = "MatchedPointsBuffer";
            fieldEdit8.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit8.AliasName_2 = "节点缓冲区半径";
            fieldEdit8.IsNullable_2 = true;
            fieldsEdit.AddField(field8);

            IField field9 = new FieldClass();
            IFieldEdit fieldEdit9 = field9 as IFieldEdit;
            fieldEdit9.Name_2 = "SP";
            fieldEdit9.Type_2 = esriFieldType.esriFieldTypeDouble;
            fieldEdit9.AliasName_2 = "形状匹配";
            fieldEdit9.IsNullable_2 = true;
            fieldsEdit.AddField(field9);



            IField field11 = new FieldClass();
            IFieldEdit fieldEdit11 = field11 as IFieldEdit;
            fieldEdit11.Name_2 = "Top";
            fieldEdit11.Type_2 = esriFieldType.esriFieldTypeInteger;
            fieldEdit11.AliasName_2 = "拓扑匹配";
            fieldEdit11.IsNullable_2 = true;
            fieldsEdit.AddField(field11);


            IField field13 = new FieldClass();
            IFieldEdit fieldEdit13 = field13 as IFieldEdit;
            fieldEdit13.Name_2 = "Buffer";
            fieldEdit13.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit13.AliasName_2 = "拓扑缓冲区半径";
            fieldEdit13.IsNullable_2 = true;
            fieldsEdit.AddField(field13);

            IField field12 = new FieldClass();
            IFieldEdit fieldEdit12 = field12 as IFieldEdit;
            fieldEdit12.Name_2 = "Attribute";
            fieldEdit12.Type_2 = esriFieldType.esriFieldTypeInteger;
            fieldEdit12.AliasName_2 = "属性匹配";
            fieldEdit12.IsNullable_2 = true;
            fieldsEdit.AddField(field12);

            IField field14 = new FieldClass();
            IFieldEdit fieldEdit14 = field14 as IFieldEdit;
            fieldEdit14.Name_2 = "MatchedFields";
            fieldEdit14.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit14.AliasName_2 = "匹配属性字段";
            fieldEdit14.IsNullable_2 = true;
            fieldsEdit.AddField(field14);

            IField field15 = new FieldClass();
            IFieldEdit fieldEdit15 = field15 as IFieldEdit;
            fieldEdit15.Name_2 = "SearchedRadius";
            fieldEdit15.Type_2 = esriFieldType.esriFieldTypeDouble;
            fieldEdit15.AliasName_2 = "搜索半径";
            fieldEdit15.IsNullable_2 = true;
            fieldsEdit.AddField(field15);

            IField field10 = new FieldClass();
            IFieldEdit fieldEdit10 = field10 as IFieldEdit;
            fieldEdit10.Name_2 = "TotalNum";
            fieldEdit10.Type_2 = esriFieldType.esriFieldTypeDouble;
            fieldEdit10.AliasName_2 = "综合相似度阈值";
            fieldEdit10.IsNullable_2 = true;
            fieldsEdit.AddField(field10);

            IField fieldInclude = new FieldClass();
            IFieldEdit fieldEditInclude = fieldInclude as IFieldEdit;
            fieldEditInclude.Name_2 = "IncludeAngle";
            fieldEditInclude.Type_2 = esriFieldType.esriFieldTypeDouble;
            fieldEditInclude.AliasName_2 = "夹角";
            fieldEditInclude.IsNullable_2 = true;
            fieldsEdit.AddField(fieldInclude);

            IField fieldHausdorff = new FieldClass();
            IFieldEdit fieldEditHausdorff = fieldHausdorff as IFieldEdit;
            fieldEditHausdorff.Name_2 = "Hausdorff";
            fieldEditHausdorff.Type_2 = esriFieldType.esriFieldTypeDouble;
            fieldEditHausdorff.AliasName_2 = "豪斯多夫距离";
            fieldEditHausdorff.IsNullable_2 = true;
            fieldsEdit.AddField(fieldHausdorff);

            fields = fieldsEdit as IFields;
            return fields;
        }
        /// <summary>
        /// 匹配结果表字段
        /// </summary>
        /// <param name="workspace">工作区路径</param>
        /// <returns>返回的字段集合</returns>
        private IFields CreateMatchPolylineTFields(IWorkspace2 workspace, string tempFieldsName)
        {
            IObjectClassDescription objectClassDescription = new ObjectClassDescriptionClass();
            IFields fields = new FieldsClass();
            IFieldsEdit fieldsEdit = fields as IFieldsEdit;
            fields = objectClassDescription.RequiredFields;

            //同比例尺下
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
                //fieldEdit2.Name_2 = "待更新OID";
                fieldEdit2.Name_2 = "待匹配OID";
                fieldEdit2.Type_2 = esriFieldType.esriFieldTypeString;
                fieldEdit2.IsNullable_2 = true;
                //fieldEdit2.AliasName_2 = "待更新OID";
                fieldEdit2.AliasName_2 = "待匹配OID";
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

                IField field4 = new FieldClass();
                IFieldEdit fieldEdit4 = field4 as IFieldEdit;
                fieldEdit4.Name_2 = "源编码";
                fieldEdit4.Type_2 = esriFieldType.esriFieldTypeString;
                fieldEdit4.IsNullable_2 = true;
                fieldEdit4.AliasName_2 = "源编码";
                fieldEdit4.Editable_2 = true;
                fieldsEdit.AddField(field4);



                IField field5 = new FieldClass();
                IFieldEdit fieldEdit5 = field5 as IFieldEdit;
                //fieldEdit5.Name_2 = "更新编码";
                fieldEdit5.Name_2 = "匹配编码";
                fieldEdit5.Type_2 = esriFieldType.esriFieldTypeString;
                fieldEdit5.IsNullable_2 = true;
                //fieldEdit5.AliasName_2 = "更新编码";
                fieldEdit5.AliasName_2 = "匹配编码";
                fieldEdit5.Editable_2 = true;
                fieldsEdit.AddField(field5);


                IField field6 = new FieldClass();
                IFieldEdit fieldEdit6 = field6 as IFieldEdit;
                fieldEdit6.Name_2 = "节点相似度";
                fieldEdit6.Type_2 = esriFieldType.esriFieldTypeString;
                fieldEdit6.IsNullable_2 = true;
                fieldEdit6.AliasName_2 = "节点相似度";
                fieldEdit6.Editable_2 = true;
                fieldsEdit.AddField(field6);

                IField field7 = new FieldClass();
                IFieldEdit fieldEdit7 = field7 as IFieldEdit;
                fieldEdit7.Name_2 = "形状相似度";
                fieldEdit7.Type_2 = esriFieldType.esriFieldTypeString;
                fieldEdit7.IsNullable_2 = true;
                fieldEdit7.AliasName_2 = "形状相似度";
                fieldEdit7.Editable_2 = true;
                fieldsEdit.AddField(field7);

                IField field8 = new FieldClass();
                IFieldEdit fieldEdit8 = field8 as IFieldEdit;
                fieldEdit8.Name_2 = "综合相似度";
                fieldEdit8.Type_2 = esriFieldType.esriFieldTypeString;
                fieldEdit8.IsNullable_2 = true;
                fieldEdit8.AliasName_2 = "综合相似度";
                fieldEdit8.Editable_2 = true;
                fieldsEdit.AddField(field8);
            }
            //TODO :不同比例尺下线状实体的字段
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

                string[] strarry = tempFieldsName.Split(';');
                for (int i = 0; i < strarry.Length; i++)
                {
                    if (strarry[i].Length > 0)
                    {
                        if (strarry[i].Contains(':'))
                        {
                            string[] arr = strarry[i].Split(':');
                            //字段4
                            IField field4 = new FieldClass();
                            IFieldEdit fieldEdit4 = field4 as IFieldEdit;
                            //fieldEdit4.Name_2 = "源编码";
                            //fieldEdit4.Name_2 = "源分类码";
                            //fieldEdit4.Name_2 = "源图层名称";
                            fieldEdit4.Name_2 = "源要素" + arr[0].Trim();
                            fieldEdit4.Type_2 = esriFieldType.esriFieldTypeString;
                            fieldEdit4.IsNullable_2 = true;
                            //fieldEdit4.AliasName_2 = "源编码";
                            //fieldEdit4.AliasName_2 = "源分类码";
                            //fieldEdit4.AliasName_2 = "源图层名称";
                            fieldEdit4.AliasName_2 = "源要素" + arr[0].Trim();
                            fieldEdit4.Editable_2 = true;
                            fieldsEdit.AddField(field4);

                            //字段5
                            IField field5 = new FieldClass();
                            IFieldEdit fieldEdit5 = field5 as IFieldEdit;
                            //fieldEdit5.Name_2 = "更新编码";
                            //fieldEdit5.Name_2 = "待匹配编码";
                            //fieldEdit5.Name_2 = "待匹配分类码";
                            //fieldEdit5.Name_2 = "待匹配图层名称";
                            fieldEdit5.Name_2 = "待匹配要素" + arr[1].Trim();
                            fieldEdit5.Type_2 = esriFieldType.esriFieldTypeString;
                            fieldEdit5.IsNullable_2 = true;
                            //fieldEdit5.AliasName_2 = "更新编码";
                            //fieldEdit5.AliasName_2 = "待匹配编码";
                            //fieldEdit5.AliasName_2 = "待匹配分类码";
                            //fieldEdit5.AliasName_2 = "待匹配图层名称";
                            fieldEdit5.AliasName_2 = "待匹配要素" + arr[1].Trim(); ;
                            fieldEdit5.Editable_2 = true;
                            fieldsEdit.AddField(field5);
                        }
                        else
                        {
                            //字段4
                            IField field4 = new FieldClass();
                            IFieldEdit fieldEdit4 = field4 as IFieldEdit;
                            //fieldEdit4.Name_2 = "源编码";
                            //fieldEdit4.Name_2 = "源分类码";
                            //fieldEdit4.Name_2 = "源图层名称";
                            fieldEdit4.Name_2 = "源要素" + strarry[i].Trim();
                            fieldEdit4.Type_2 = esriFieldType.esriFieldTypeString;
                            fieldEdit4.IsNullable_2 = true;
                            //fieldEdit4.AliasName_2 = "源编码";
                            //fieldEdit4.AliasName_2 = "源分类码";
                            //fieldEdit4.AliasName_2 = "源图层名称";
                            fieldEdit4.AliasName_2 = "源要素" + strarry[i].Trim();
                            fieldEdit4.Editable_2 = true;
                            fieldsEdit.AddField(field4);

                            //字段5
                            IField field5 = new FieldClass();
                            IFieldEdit fieldEdit5 = field5 as IFieldEdit;
                            //fieldEdit5.Name_2 = "更新编码";
                            //fieldEdit5.Name_2 = "待匹配编码";
                            //fieldEdit5.Name_2 = "待匹配分类码";
                            //fieldEdit5.Name_2 = "待匹配图层名称";
                            fieldEdit5.Name_2 = "待匹配要素" + strarry[i].Trim();
                            fieldEdit5.Type_2 = esriFieldType.esriFieldTypeString;
                            fieldEdit5.IsNullable_2 = true;
                            //fieldEdit5.AliasName_2 = "更新编码";
                            //fieldEdit5.AliasName_2 = "待匹配编码";
                            //fieldEdit5.AliasName_2 = "待匹配分类码";
                            //fieldEdit5.AliasName_2 = "待匹配图层名称";
                            fieldEdit5.AliasName_2 = "待匹配要素" + strarry[i].Trim(); ;
                            fieldEdit5.Editable_2 = true;
                            fieldsEdit.AddField(field5);
                        }
                    }
                }

                //IField field4 = new FieldClass();
                //IFieldEdit fieldEdit4 = field4 as IFieldEdit;
                ////fieldEdit4.Name_2 = "源编码";
                //fieldEdit4.Name_2 = "源图层名称";
                //fieldEdit4.Type_2 = esriFieldType.esriFieldTypeString;
                //fieldEdit4.IsNullable_2 = true;
                ////fieldEdit4.AliasName_2 = "源编码";
                //fieldEdit4.AliasName_2 = "源图层名称";
                //fieldEdit4.Editable_2 = true;
                //fieldsEdit.AddField(field4);



                //IField field5 = new FieldClass();
                //IFieldEdit fieldEdit5 = field5 as IFieldEdit;
                ////fieldEdit5.Name_2 = "更新编码";
                //fieldEdit5.Name_2 = "待匹配图层名称";
                //fieldEdit5.Type_2 = esriFieldType.esriFieldTypeString;
                //fieldEdit5.IsNullable_2 = true;
                ////fieldEdit5.AliasName_2 = "更新编码";
                //fieldEdit5.AliasName_2 = "待匹配图层名称";
                //fieldEdit5.Editable_2 = true;
                //fieldsEdit.AddField(field5);



                IField field3 = new FieldClass();
                IFieldEdit fieldEdit3 = field3 as IFieldEdit;
                fieldEdit3.Name_2 = "变化标记";
                fieldEdit3.Type_2 = esriFieldType.esriFieldTypeString;
                fieldEdit3.IsNullable_2 = true;
                fieldEdit3.AliasName_2 = "变化标记";
                fieldEdit3.Editable_2 = true;
                fieldsEdit.AddField(field3);

                IField field6 = new FieldClass();
                IFieldEdit fieldEdit6 = field6 as IFieldEdit;
                fieldEdit6.Name_2 = "节点相似度";
                fieldEdit6.Type_2 = esriFieldType.esriFieldTypeString;
                fieldEdit6.IsNullable_2 = true;
                fieldEdit6.AliasName_2 = "节点相似度";
                fieldEdit6.Editable_2 = true;
                fieldsEdit.AddField(field6);

                IField field7 = new FieldClass();
                IFieldEdit fieldEdit7 = field7 as IFieldEdit;
                fieldEdit7.Name_2 = "形状相似度";
                fieldEdit7.Type_2 = esriFieldType.esriFieldTypeString;
                fieldEdit7.IsNullable_2 = true;
                fieldEdit7.AliasName_2 = "形状相似度";
                fieldEdit7.Editable_2 = true;
                fieldsEdit.AddField(field7);

                IField field8 = new FieldClass();
                IFieldEdit fieldEdit8 = field8 as IFieldEdit;
                fieldEdit8.Name_2 = "综合相似度";
                fieldEdit8.Type_2 = esriFieldType.esriFieldTypeString;
                fieldEdit8.IsNullable_2 = true;
                fieldEdit8.AliasName_2 = "综合相似度";
                fieldEdit8.Editable_2 = true;
                fieldsEdit.AddField(field8);
            }

            #region comments
            //IField field4 = new FieldClass();
            //IFieldEdit fieldEdit4 = field4 as IFieldEdit;
            //fieldEdit4.Name_2 = "pointSimilar";
            //fieldEdit4.Type_2 = esriFieldType.esriFieldTypeString;
            //fieldEdit4.IsNullable_2 = true;
            //fieldEdit4.AliasName_2 = "匹配点相似度";
            //fieldEdit4.Editable_2 = true;
            //fieldsEdit.AddField(field4);

            //IField field5 = new FieldClass();
            //IFieldEdit fieldEdit5 = field5 as IFieldEdit;
            //fieldEdit5.Name_2 = "shapeSimilar";
            //fieldEdit5.Type_2 = esriFieldType.esriFieldTypeString;
            //fieldEdit5.IsNullable_2 = true;
            //fieldEdit5.AliasName_2 = "形状相似度";
            //fieldEdit5.Editable_2 = true;
            //fieldsEdit.AddField(field5);

            //IField field6 = new FieldClass();
            //IFieldEdit fieldEdit6 = field6 as IFieldEdit;
            //fieldEdit6.Name_2 = "Similarity";
            //fieldEdit6.Type_2 = esriFieldType.esriFieldTypeString;
            //fieldEdit6.IsNullable_2 = true;
            //fieldEdit6.AliasName_2 = "Similarity";
            //fieldEdit6.Editable_2 = true;
            //fieldsEdit.AddField(field6);
            #endregion

            fields = fieldsEdit as IFields;

            return fields;


        }
        #region 线实体的开始匹配
        //TODO :线实体的开始匹配
        /// <summary>
        /// 开始（线）匹配
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonXStartMatch_Click(object sender, EventArgs e)
        {
            //StopWatch类计时
            Stopwatch sw = new Stopwatch();
            sw.Start();

            double Total = Convert.ToDouble(labelNode.Text) + Convert.ToDouble(labelShape.Text);
            if (Total != 1)
            {
                MessageBoxEx.Show("综合权重之和不为1 ！");
                return;
            }

            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            IWorkspaceFactory2 pWorkspaceFactory = new FileGDBWorkspaceFactoryClass() as IWorkspaceFactory2;
            string gdbPath = ClsDeclare.g_WorkspacePath;
            IWorkspace2 pWorkspace = pWorkspaceFactory.OpenFromFile(gdbPath, 0) as IWorkspace2;
            IFeatureWorkspace featureWorkspace = pWorkspace as IFeatureWorkspace;

            IFeatureClass sourceFeatureClass = this.dataGridViewX1[1, 0].Tag as IFeatureClass;
            string targetFeatureName = this.dataGridViewX1[2, 0].Value.ToString();

            //创建设置表MatchedPolylineFCSetting
            loadFeatSetting(gdbPath, targetFeatureName, sourceFeatureClass);

            ITable table = null;
            IFields fileds = null;
            string tempFieldsName = "";

            //TODO ：设置选择的字段
            //由dataGridViewX2来设置匹配属性字段——cell[12]
            for (int i = 0; i < dataGridViewX2.RowCount; i++)
            {
                DataGridViewCheckBoxCell dgvCheckBoxCell = dataGridViewX2[0, i] as DataGridViewCheckBoxCell;
                if (Convert.ToBoolean(dgvCheckBoxCell.Value) == true)
                {
                    if (this.dataGridViewX2[1, i].Value.ToString().ToUpper() == this.dataGridViewX2[2, i].Value.ToString().ToUpper())
                    {
                        tempFieldsName = tempFieldsName + dataGridViewX2[1, i].Value.ToString() + ";";
                    }
                    else
                    {
                        tempFieldsName = tempFieldsName + dataGridViewX2[1, i].Value.ToString() + ":" + dataGridViewX2[2, i].Value.ToString() + ";";
                    }
                }

            }
            for (int i = 0; i < this.dataGridViewX1.Rows.Count; i++)
            {
                IFeatureClass pSrcFcls = this.dataGridViewX1.Rows[i].Cells[1].Tag as IFeatureClass;

                if (pSrcFcls.ShapeType == esriGeometryType.esriGeometryPolyline ||
                    pSrcFcls.ShapeType == esriGeometryType.esriGeometryLine)
                {
                    //创建表所需要的所有字段值
                    fileds = CreateMatchPolylineTFields(pWorkspace, tempFieldsName);
                }

                //表如果存在，创建空表
                if (pWorkspace.get_NameExists(esriDatasetType.esriDTTable, this.dataGridViewX1.Rows[i].Cells[3].Value.ToString()))
                {
                    table = featureWorkspace.OpenTable(this.dataGridViewX1.Rows[i].Cells[3].Value.ToString());
                    //ClsDeleteTables.DeleteFeatureClass(gdbPath, this.dataGridViewX1.Rows[i].Cells[3].Value.ToString());
                    //table = CreateTable(pWorkspace, this.dataGridViewX1.Rows[i].Cells[3].Value.ToString(), fileds);

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
                //表不存在，就创建空表
                else
                {
                    table = CreateTable(pWorkspace, this.dataGridViewX1.Rows[i].Cells[3].Value.ToString(), fileds);
                }

                this.dataGridViewX1[3, i].Tag = table;
                IFeatureClass pTarFcls = featureWorkspace.OpenFeatureClass(this.dataGridViewX1.Rows[i].Cells[2].Value.ToString());

                ITable tableSetting = null;
                int matchedMode = -1;
                int matchedMay = -1;
                double[] weight = new double[4];
                double includeAngel = 0.0;
                double hausdorff = 0.0;
                double buffer = 0;
                string fields = "";

                IDataset dataset = pTarFcls as IDataset;
                string matchedFCName = dataset.Name;


                if (pTarFcls.ShapeType == esriGeometryType.esriGeometryPolyline)
                {
                    tableSetting = featureWorkspace.OpenTable(ClsConstant.lineSettingTable);
                    ICursor cursor = tableSetting.Search(null, false);
                    IRow row = cursor.NextRow();
                    while (row != null)
                    {
                        if (row.get_Value(row.Fields.FindField("MatchedFCName")).ToString() == matchedFCName)
                        {
                            //几何+属性匹配
                            if (this.tabItemGAGeo.Visible == true)
                            {
                                fields = row.get_Value(row.Fields.FindField("MatchedFields")).ToString();
                                matchedMay = 0;
                                matchedMode = 10;
                                weight[0] = Convert.ToDouble(row.get_Value(row.Fields.FindField("SP")));
                                string temp = row.get_Value(row.Fields.FindField("MatchedPointsBuffer")).ToString();
                                if (temp.Contains("米"))
                                {
                                    temp = temp.Substring(0, temp.LastIndexOf("米")).Trim();
                                }
                                buffer = Convert.ToDouble(temp);

                                weight[1] = Convert.ToDouble(row.get_Value(row.Fields.FindField("MatchedPoints")));
                                weight[2] = Convert.ToDouble(row.get_Value(row.Fields.FindField("TotalNum")));

                            }
                            //几何匹配
                            else if (this.tabItemGeo.Visible == true)
                            {
                                fields = row.get_Value(row.Fields.FindField("MatchedFields")).ToString();
                                matchedMay = 0;
                                matchedMode = 10;
                                weight[0] = Convert.ToDouble(row.get_Value(row.Fields.FindField("SP")));
                                string temp = row.get_Value(row.Fields.FindField("MatchedPointsBuffer")).ToString();
                                if (temp.Contains("米"))
                                {
                                    temp = temp.Substring(0, temp.LastIndexOf("米")).Trim();
                                }
                                buffer = Convert.ToDouble(temp);

                                weight[1] = Convert.ToDouble(row.get_Value(row.Fields.FindField("MatchedPoints")));
                                weight[2] = Convert.ToDouble(row.get_Value(row.Fields.FindField("TotalNum")));

                                includeAngel = Convert.ToDouble(row.get_Value(row.Fields.FindField("IncludeAngle")));
                                hausdorff = Convert.ToDouble(row.get_Value(row.Fields.FindField("Hausdorff")));

                            }
                            //拓扑匹配
                            //if (row.get_Value(row.Fields.FindField("Top")).ToString() == "1")
                            else if (this.tabItemTopo.Visible == true)
                            {
                                matchedMay = 1;
                                matchedMode = -1;
                                //weight = 0;
                                string temp = row.get_Value(row.Fields.FindField("Buffer")).ToString();
                                if (temp.Contains("米"))
                                {
                                    temp = temp.Substring(0, temp.LastIndexOf("米")).Trim();
                                }
                                buffer = Convert.ToDouble(temp);
                                fields = row.get_Value(row.Fields.FindField("MatchedFields")).ToString();

                                break;
                            }


                        }
                        row = cursor.NextRow();
                    }
                }

                ClsLineMatch clsLineMatch = new ClsLineMatch();
                //几何+属性匹配
                if (this.tabItemGAGeo.Visible == true)
                {
                    clsLineMatch.SearchChangedFeaturesGeoAttr(pSrcFcls, pTarFcls, table, weight, buffer, fields, progressBarMain, progressBarSub, labelXStatus, chkBoxLineIndicator);
                    sw.Stop();
                    #region 反向匹配
                    //if (ckbEachMatch.Checked)
                    //{
                    //    //读取匹配改变的要素的ID
                    //    ICursor cursor2 = table.Search(null, false);
                    //    IRow row = cursor2.NextRow();
                    //    List<int> UpdataDataID = new List<int>();
                    //    while (row != null)
                    //    {
                    //        string strResult = row.get_Value(3).ToString();
                    //        if (strResult != "未变化" && strResult != "新增要素")
                    //        {
                    //            string str = row.get_Value(2).ToString();//获得更新要素ID
                    //            if (str != null && str != "")
                    //            {
                    //                if (str.Contains(";"))
                    //                {
                    //                    string[] strArray = str.Split(';');
                    //                    foreach (string strID in strArray)
                    //                        UpdataDataID.Add(Convert.ToInt32(strID));
                    //                }
                    //                else
                    //                    UpdataDataID.Add(Convert.ToInt32(str));
                    //            }
                    //        }
                    //        row = cursor2.NextRow();
                    //    }
                    //    if (UpdataDataID.Count == 0)
                    //    {
                    //        return;
                    //    }
                    //    //添加一行用于反向隔开
                    //    IWorkspaceEdit workspaceEdit = pWorkspace as IWorkspaceEdit;
                    //    workspaceEdit.StartEditing(true);
                    //    workspaceEdit.StartEditOperation();

                    //    IRowBuffer invalidRowBuffer = table.CreateRowBuffer();
                    //    ICursor invalidRowCursor = table.Insert(true);
                    //    //invalidRowBuffer.set_Value(1, "11111");
                    //    invalidRowCursor.InsertRow(invalidRowBuffer);

                    //    workspaceEdit.StopEditOperation();
                    //    workspaceEdit.StopEditing(true);

                    //    clsCoreUpdateFun.ReverseSearchChangedFeatures(pTEFeatCls, UpdataDataID, pTUFeatCls, table, matchedMode, weight, buffer, fields, progressBarMain, progressBarSub, labelXStatus);
                    //} 
                    #endregion
                    MessageBoxEx.Show("几何属性匹配已完成！总需要时间" + sw.ElapsedMilliseconds + "ms", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                //几何匹配
                else if (this.tabItemGeo.Visible == true)
                {
                    clsLineMatch.SearchChangedFeaturesGeo(pSrcFcls, pTarFcls, table, weight, buffer, fields, progressBarMain, progressBarSub, 
                        labelXStatus, chkBoxLineIndicator,includeAngel,hausdorff);
                    sw.Stop();
                    MessageBoxEx.Show("几何匹配已完成！总需要时间" + sw.ElapsedMilliseconds + "ms", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                //拓扑匹配
                else if (this.tabItemTopo.Visible == true)
                {
                    clsLineMatch.SearchChangedFeaturesByTop(pSrcFcls, pTarFcls, table, buffer, fields, progressBarMain, progressBarSub, labelXStatus);
                    sw.Stop();
                    MessageBox.Show("拓扑匹配已完成！总需要时间" + sw.ElapsedMilliseconds + "ms", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.Close();
        }

        #endregion
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
        /// 创建设置表MatchedPolylineFCSetting
        /// </summary>
        /// <param name="gdbPath"></param>
        /// <param name="targetFeatureName"></param>
        /// <param name="sourceFeatCls"></param>
        private void loadFeatSetting(string gdbPath, string targetFeatureName, IFeatureClass sourceFeatCls)
        {
            if ((targetFeatureName != "") && (gdbPath != ""))
            {
                //判断选择字段是否对应
                for (int i = 0; i < dataGridViewX2.RowCount; i++)
                {
                    DataGridViewCheckBoxCell dgvCheckBoxCell1 = dataGridViewX2[0, i] as DataGridViewCheckBoxCell;
                    if (Convert.ToBoolean(dgvCheckBoxCell1.Value) == true)
                    {
                        if (this.dataGridViewX2[2, i].Value == null)
                        {
                            MessageBox.Show("没有选择属性匹配字段，只进行几何匹配！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }

                IWorkspaceFactory pWorkspaceFactory = new FileGDBWorkspaceFactory();
                IWorkspace2 workspace = pWorkspaceFactory.OpenFromFile(gdbPath, 0) as IWorkspace2;
                IFeatureWorkspace featureWorkspace = workspace as IFeatureWorkspace;
                ITable table = null;
                IFields fields = null;

                table = TableIsExist(sourceFeatCls, gdbPath, table, featureWorkspace);

                IWorkspaceEdit pWorkspaceEdit = featureWorkspace as IWorkspaceEdit;
                pWorkspaceEdit.StartEditing(true);
                pWorkspaceEdit.StartEditOperation();

                //Collection<string> pCol = new Collection<string>();
                Dictionary<string, int> pDic = new Dictionary<string, int>();

                string tempFieldsName = "";
                string tempBuffer = "";
                DataGridViewCheckBoxCell dgvCheckBoxCell = new DataGridViewCheckBoxCell();

                ICursor pCursor = table.Search(null, false);
                IRow pRow = pCursor.NextRow();
                while (pRow != null)
                {
                    //表中某行的 待匹配图层名不为空
                    if (pRow.get_Value(table.FindField("MatchedFCName")).ToString() != "")
                    {
                        pDic.Add(pRow.get_Value(table.FindField("MatchedFCName")).ToString(), pRow.OID);
                    }
                    pRow = pCursor.NextRow();
                }
                //几何+属性匹配
                if (this.tabControl1.Controls[1].Visible)
                {
                    //表中某行的 待匹配图层名为空，也就是没有相应的记录
                    if (!pDic.ContainsKey(targetFeatureName))
                    {
                        //创建匹配的新记录
                        IRow tempRow = table.CreateRow();

                        if (ClsDeclare.g_SourceFeatClsPathDic.ContainsKey((sourceFeatCls as IDataset).Name))
                        {
                            tempRow.set_Value(tempRow.Fields.FindField("SourceFCName"), (sourceFeatCls as IDataset).Name);
                            tempRow.set_Value(tempRow.Fields.FindField("SourcePath"), ClsDeclare.g_SourceFeatClsPathDic[(sourceFeatCls as IDataset).Name]);
                            tempRow.set_Value(tempRow.Fields.FindField("WorkspacePath"), ClsDeclare.g_WorkspacePath);
                        }

                        tempRow.set_Value(table.FindField("MatchedFCName"), targetFeatureName);
                        tempRow.set_Value(tempRow.Fields.FindField("FCSettingID"), table.RowCount(null) - 1);
                        //权重
                        tempRow.set_Value(tempRow.Fields.FindField("MatchedPoints"), Convert.ToDouble(labelNode.Text));

                        tempRow.set_Value(tempRow.Fields.FindField("SearchedRadius"), Convert.ToDouble(labelBufferRadius.Text));

                        tempRow.set_Value(tempRow.Fields.FindField("SP"), Convert.ToDouble(labelShape.Text));
                        tempRow.set_Value(tempRow.Fields.FindField("TotalNum"), Convert.ToDouble(labelTotal.Text));

                        if (this.labelBufferRadius.Text != "")
                        {
                            tempBuffer = this.labelBufferRadius.Text;
                            if (tempBuffer.Contains("米"))
                            {
                                tempBuffer = tempBuffer.Substring(0, tempBuffer.LastIndexOf("米"));
                            }
                            IDataset dataset1 = sourceFeatCls as IDataset;
                            IGeoDataset geoDataset = dataset1 as IGeoDataset;

                            double b = ClsConvertUnit.GetBufferValueByUnit(geoDataset.SpatialReference, Convert.ToDouble(tempBuffer));
                            string sb = b.ToString("#0.000000");
                            tempRow.set_Value(tempRow.Fields.FindField("MatchedPointsBuffer"), sb);
                        }
                        else
                        {
                            MessageBoxEx.Show("请选择缓冲区半径！");
                            return;
                        }

                        //TODO :读取选择的字段

                        //修改属性
                        //tempRow.set_Value(tempRow.Fields.FindField("Attribute"), 1);

                        for (int i = 0; i < dataGridViewX2.RowCount; i++)
                        {
                            dgvCheckBoxCell = dataGridViewX2[0, i] as DataGridViewCheckBoxCell;
                            if (Convert.ToBoolean(dgvCheckBoxCell.Value) == true)
                            {
                                if (this.dataGridViewX2[1, i].Value.ToString().ToUpper() == this.dataGridViewX2[2, i].Value.ToString().ToUpper())
                                {
                                    tempFieldsName = tempFieldsName + dataGridViewX2[1, i].Value.ToString() + ";";
                                }
                                else
                                {
                                    tempFieldsName = tempFieldsName + dataGridViewX2[1, i].Value.ToString() + ":" + dataGridViewX2[2, i].Value.ToString() + ";";
                                }
                            }

                        }

                        if (tempFieldsName != "")
                        {
                            tempRow.set_Value(tempRow.Fields.FindField("Attribute"), 1);
                            tempRow.set_Value(tempRow.Fields.FindField("MatchedFields"), tempFieldsName.Trim());
                        }
                        else
                        {
                            tempRow.set_Value(tempRow.Fields.FindField("Attribute"), 0);
                            MessageBox.Show("没有选择属性匹配对应字段！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        tempRow.Store();
                    }
                    //表中某行的 待匹配图层名不为空，也就是含有之前的匹配记录                    
                    else
                    {
                        //通过待匹配图层名获取该记录
                        IRow tRow = table.GetRow(pDic[targetFeatureName]);

                        if (ClsDeclare.g_SourceFeatClsPathDic.ContainsKey((sourceFeatCls as IDataset).Name))
                        {
                            tRow.set_Value(tRow.Fields.FindField("SourceFCName"), (sourceFeatCls as IDataset).Name);
                            tRow.set_Value(tRow.Fields.FindField("SourcePath"), ClsDeclare.g_SourceFeatClsPathDic[(sourceFeatCls as IDataset).Name]);
                            tRow.set_Value(tRow.Fields.FindField("WorkspacePath"), ClsDeclare.g_WorkspacePath);
                        }

                        tRow.set_Value(table.FindField("MatchedFCName"), targetFeatureName);
                        tRow.set_Value(tRow.Fields.FindField("FCSettingID"), table.RowCount(null) - 1);
                        //权重
                        tRow.set_Value(tRow.Fields.FindField("MatchedPoints"), Convert.ToDouble(labelNode.Text));

                        tRow.set_Value(tRow.Fields.FindField("SearchedRadius"), Convert.ToDouble(labelBufferRadius.Text));

                        tRow.set_Value(tRow.Fields.FindField("SP"), Convert.ToDouble(labelShape.Text));
                        tRow.set_Value(tRow.Fields.FindField("TotalNum"), Convert.ToDouble(labelTotal.Text));

                        if (this.labelBufferRadius.Text != "")
                        {
                            tempBuffer = this.labelBufferRadius.Text;
                            if (tempBuffer.Contains("米"))
                            {
                                tempBuffer = tempBuffer.Substring(0, tempBuffer.LastIndexOf("米"));
                            }
                            IDataset dataset1 = sourceFeatCls as IDataset;
                            IGeoDataset geoDataset = dataset1 as IGeoDataset;

                            double b = ClsConvertUnit.GetBufferValueByUnit(geoDataset.SpatialReference, Convert.ToDouble(tempBuffer));
                            string sb = b.ToString("#0.000000");
                            tRow.set_Value(tRow.Fields.FindField("MatchedPointsBuffer"), sb);
                        }
                        else
                        {
                            MessageBoxEx.Show("请选择缓冲区半径！");
                            return;
                        }

                        //修改属性
                        //tRow.set_Value(tRow.Fields.FindField("Attribute"), 1);
                        for (int i = 0; i < dataGridViewX1.RowCount; i++)
                        {
                            dgvCheckBoxCell = dataGridViewX2[0, i] as DataGridViewCheckBoxCell;
                            if (Convert.ToBoolean(dgvCheckBoxCell.Value) == true)
                            {
                                if (this.dataGridViewX2[1, i].Value.ToString().ToUpper() == this.dataGridViewX2[2, i].Value.ToString().ToUpper())
                                {
                                    tempFieldsName = tempFieldsName + dataGridViewX2[1, i].Value.ToString() + ";";
                                }
                                else
                                {
                                    tempFieldsName = tempFieldsName + dataGridViewX2[1, i].Value.ToString() + "(" + dataGridViewX2[2, i].Value.ToString() + ")";
                                }
                            }

                        }

                        if (tempFieldsName == "")
                        {
                            //tRow.set_Value(tRow.Fields.FindField("Attribute"), 0);
                            tRow.set_Value(tRow.Fields.FindField("Attribute"), 0);
                            MessageBoxEx.Show("没有选择属性匹配对应字段！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            //tRow.set_Value(tRow.Fields.FindField("Attribute"), 1);
                            tRow.set_Value(tRow.Fields.FindField("Attribute"), 1);
                            tRow.set_Value(tRow.Fields.FindField("MatchedFields"), tempFieldsName.Trim());
                        }

                        tRow.Store();
                    }
                }
                //几何匹配
                else if (this.tabControl1.Controls[0].Visible)
                {
                    //表中某行的 待匹配图层名为空，也就是没有相应的记录
                    if (!pDic.ContainsKey(targetFeatureName))
                    {
                        //创建匹配的新记录
                        IRow tempRow = table.CreateRow();

                        if (ClsDeclare.g_SourceFeatClsPathDic.ContainsKey((sourceFeatCls as IDataset).Name))
                        {
                            tempRow.set_Value(tempRow.Fields.FindField("SourceFCName"), (sourceFeatCls as IDataset).Name);
                            tempRow.set_Value(tempRow.Fields.FindField("SourcePath"), ClsDeclare.g_SourceFeatClsPathDic[(sourceFeatCls as IDataset).Name]);
                            tempRow.set_Value(tempRow.Fields.FindField("WorkspacePath"), ClsDeclare.g_WorkspacePath);
                        }

                        tempRow.set_Value(table.FindField("MatchedFCName"), targetFeatureName);
                        tempRow.set_Value(tempRow.Fields.FindField("FCSettingID"), table.RowCount(null) - 1);
                        //权重
                        tempRow.set_Value(tempRow.Fields.FindField("MatchedPoints"), Convert.ToDouble(labelNodeGeo.Text));

                        tempRow.set_Value(tempRow.Fields.FindField("SearchedRadius"), Convert.ToDouble(labelBufferGeo.Text));

                        tempRow.set_Value(tempRow.Fields.FindField("SP"), Convert.ToDouble(labelShapeGeo.Text));
                        tempRow.set_Value(tempRow.Fields.FindField("TotalNum"), Convert.ToDouble(labelTotalGeo.Text));

                        if (this.labelBufferGeo.Text != "")
                        {
                            tempBuffer = this.labelBufferGeo.Text;
                            if (tempBuffer.Contains("米"))
                            {
                                tempBuffer = tempBuffer.Substring(0, tempBuffer.LastIndexOf("米"));
                            }
                            IDataset dataset1 = sourceFeatCls as IDataset;
                            IGeoDataset geoDataset = dataset1 as IGeoDataset;

                            double b = ClsConvertUnit.GetBufferValueByUnit(geoDataset.SpatialReference, Convert.ToDouble(tempBuffer));
                            string sb = b.ToString("#0.000000");
                            tempRow.set_Value(tempRow.Fields.FindField("MatchedPointsBuffer"), sb);
                        }
                        else
                        {
                            MessageBoxEx.Show("请选择缓冲区半径！");
                            return;
                        }

                        //TODO :读取选择的字段

                        //修改属性
                        //tempRow.set_Value(tempRow.Fields.FindField("Attribute"), 1);

                        for (int i = 0; i < dataGridViewX2.RowCount; i++)
                        {
                            dgvCheckBoxCell = dataGridViewX2[0, i] as DataGridViewCheckBoxCell;
                            if (Convert.ToBoolean(dgvCheckBoxCell.Value) == true)
                            {
                                if (this.dataGridViewX2[1, i].Value.ToString().ToUpper() == this.dataGridViewX2[2, i].Value.ToString().ToUpper())
                                {
                                    tempFieldsName = tempFieldsName + dataGridViewX2[1, i].Value.ToString() + ";";
                                }
                                else
                                {
                                    tempFieldsName = tempFieldsName + dataGridViewX2[1, i].Value.ToString() + ":" + dataGridViewX2[2, i].Value.ToString() + ";";
                                }
                            }

                        }

                        if (tempFieldsName != "")
                        {
                            tempRow.set_Value(tempRow.Fields.FindField("Attribute"), 1);
                            tempRow.set_Value(tempRow.Fields.FindField("MatchedFields"), tempFieldsName.Trim());
                        }
                        else
                        {
                            tempRow.set_Value(tempRow.Fields.FindField("Attribute"), 0);
                            //MessageBox.Show("没有选择属性匹配对应字段！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                        tempRow.set_Value(table.FindField("IncludeAngle"), Convert.ToDouble(labelIncludeGeo.Text));
                        tempRow.set_Value(table.FindField("Hausdorff"), Convert.ToDouble(labelHausdorffGeo.Text));


                        tempRow.Store();
                    }
                    //表中某行的 待匹配图层名不为空，也就是含有之前的匹配记录                    
                    else
                    {
                        //通过待匹配图层名获取该记录
                        IRow tRow = table.GetRow(pDic[targetFeatureName]);

                        if (ClsDeclare.g_SourceFeatClsPathDic.ContainsKey((sourceFeatCls as IDataset).Name))
                        {
                            tRow.set_Value(tRow.Fields.FindField("SourceFCName"), (sourceFeatCls as IDataset).Name);
                            tRow.set_Value(tRow.Fields.FindField("SourcePath"), ClsDeclare.g_SourceFeatClsPathDic[(sourceFeatCls as IDataset).Name]);
                            tRow.set_Value(tRow.Fields.FindField("WorkspacePath"), ClsDeclare.g_WorkspacePath);
                        }

                        tRow.set_Value(table.FindField("MatchedFCName"), targetFeatureName);
                        tRow.set_Value(tRow.Fields.FindField("FCSettingID"), table.RowCount(null) - 1);
                        //权重
                        tRow.set_Value(tRow.Fields.FindField("MatchedPoints"), Convert.ToDouble(labelNodeGeo.Text));

                        tRow.set_Value(tRow.Fields.FindField("SearchedRadius"), Convert.ToDouble(labelBufferGeo.Text));

                        tRow.set_Value(tRow.Fields.FindField("SP"), Convert.ToDouble(labelShapeGeo.Text));
                        tRow.set_Value(tRow.Fields.FindField("TotalNum"), Convert.ToDouble(labelTotalGeo.Text));

                        if (this.labelBufferGeo.Text != "")
                        {
                            tempBuffer = this.labelBufferGeo.Text;
                            if (tempBuffer.Contains("米"))
                            {
                                tempBuffer = tempBuffer.Substring(0, tempBuffer.LastIndexOf("米"));
                            }
                            IDataset dataset1 = sourceFeatCls as IDataset;
                            IGeoDataset geoDataset = dataset1 as IGeoDataset;

                            double b = ClsConvertUnit.GetBufferValueByUnit(geoDataset.SpatialReference, Convert.ToDouble(tempBuffer));
                            string sb = b.ToString("#0.000000");
                            tRow.set_Value(tRow.Fields.FindField("MatchedPointsBuffer"), sb);
                        }
                        else
                        {
                            MessageBoxEx.Show("请选择缓冲区半径！");
                            return;
                        }

                        //修改属性
                        //tRow.set_Value(tRow.Fields.FindField("Attribute"), 1);
                        for (int i = 0; i < dataGridViewX1.RowCount; i++)
                        {
                            dgvCheckBoxCell = dataGridViewX2[0, i] as DataGridViewCheckBoxCell;
                            if (Convert.ToBoolean(dgvCheckBoxCell.Value) == true)
                            {
                                if (this.dataGridViewX2[1, i].Value.ToString().ToUpper() == this.dataGridViewX2[2, i].Value.ToString().ToUpper())
                                {
                                    tempFieldsName = tempFieldsName + dataGridViewX2[1, i].Value.ToString() + ";";
                                }
                                else
                                {
                                    tempFieldsName = tempFieldsName + dataGridViewX2[1, i].Value.ToString() + "(" + dataGridViewX2[2, i].Value.ToString() + ")";
                                }
                            }

                        }

                        if (tempFieldsName == "")
                        {
                            tRow.set_Value(tRow.Fields.FindField("Attribute"), 0);
                            MessageBoxEx.Show("没有选择属性匹配对应字段！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            tRow.set_Value(tRow.Fields.FindField("Attribute"), 1);
                            tRow.set_Value(tRow.Fields.FindField("MatchedFields"), tempFieldsName.Trim());
                        }

                        tRow.set_Value(tRow.Fields.FindField("IncludeAngle"), Convert.ToDouble(labelIncludeGeo.Text));
                        tRow.set_Value(tRow.Fields.FindField("Hausdorff"), Convert.ToDouble(labelHausdorffGeo.Text));

                        tRow.Store();
                    }
                }
                //拓扑匹配
                else if (this.tabControl1.Controls[3].Visible)
                {
                    if (!pDic.ContainsKey(targetFeatureName))
                    {
                        IRow tempRow = table.CreateRow();

                        tempRow.set_Value(table.FindField("MatchedFCName"), targetFeatureName);
                        tempRow.set_Value(tempRow.Fields.FindField("FCSettingID"), table.RowCount(null) - 1);
                        tempRow.set_Value(tempRow.Fields.FindField("Top"), 1);

                        if (this.comboBoxExBuffer.Text != "")
                        {
                            tempBuffer = this.comboBoxExBuffer.Text;
                            if (tempBuffer.Contains("米"))
                            {
                                tempBuffer = tempBuffer.Substring(0, tempBuffer.LastIndexOf("米"));
                            }
                            IDataset dataset1 = sourceFeatCls as IDataset;
                            IGeoDataset geoDataset = dataset1 as IGeoDataset;

                            double b = ClsConvertUnit.GetBufferValueByUnit(geoDataset.SpatialReference, Convert.ToDouble(tempBuffer));
                            string sb = b.ToString("#0.000000");
                            tempRow.set_Value(tempRow.Fields.FindField("Buffer"), sb);
                        }
                        else
                        {
                            MessageBox.Show("请选择缓冲区半径！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;

                        }
                        //修改属性！！！！！！！！！！！！！！！！！！！！！
                        tempRow.set_Value(tempRow.Fields.FindField("Attribute"), 1);
                        for (int i = 0; i < dataGridViewX2.RowCount; i++)
                        {
                            dgvCheckBoxCell = dataGridViewX2[0, i] as DataGridViewCheckBoxCell;
                            if (Convert.ToBoolean(dgvCheckBoxCell.Value) == true)
                            {
                                if (this.dataGridViewX2[1, i].Value.ToString().ToUpper() == this.dataGridViewX2[2, i].Value.ToString().ToUpper())
                                {
                                    tempFieldsName = tempFieldsName + dataGridViewX2[1, i].Value.ToString() + ";";
                                }
                                else
                                {
                                    tempFieldsName = tempFieldsName + dataGridViewX2[1, i].Value.ToString() + "(" + dataGridViewX2[2, i].Value.ToString() + ")";
                                }
                            }

                        }

                        if (tempFieldsName == "")
                        {
                            //tRow.set_Value(tRow.Fields.FindField("Attribute"), 0);
                            //MessageBox.Show("没有选择属性匹配对应字段！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            //tRow.set_Value(tRow.Fields.FindField("Attribute"), 1);
                            tempRow.set_Value(tempRow.Fields.FindField("MatchedFields"), tempFieldsName.Trim());
                        }

                        IDataset dataset = sourceFeatCls as IDataset;
                        if (ClsDeclare.g_SourceFeatClsPathDic.ContainsKey(dataset.Name))
                        {

                            tempRow.set_Value(tempRow.Fields.FindField("SourceFCName"), dataset.Name);
                            tempRow.set_Value(tempRow.Fields.FindField("SourcePath"), ClsDeclare.g_SourceFeatClsPathDic[dataset.Name]);
                            tempRow.set_Value(tempRow.Fields.FindField("WorkspacePath"), ClsDeclare.g_WorkspacePath);
                        }
                        tempRow.Store();
                    }
                    else
                    {
                        IRow tRow = table.GetRow(pDic[targetFeatureName]);
                        tRow.set_Value(tRow.Fields.FindField("Top"), 1);
                        if (this.comboBoxExBuffer.Text != "")
                        {
                            tempBuffer = this.comboBoxExBuffer.Text;
                            if (tempBuffer.Contains("米"))
                            {
                                tempBuffer = tempBuffer.Substring(0, tempBuffer.LastIndexOf("米"));
                            }
                            IDataset dataset1 = sourceFeatCls as IDataset;
                            IGeoDataset geoDataset = dataset1 as IGeoDataset;

                            double b = ClsConvertUnit.GetBufferValueByUnit(geoDataset.SpatialReference, Convert.ToDouble(tempBuffer));
                            string sb = b.ToString("#0.000000");
                            tRow.set_Value(tRow.Fields.FindField("Buffer"), sb);
                        }
                        else
                        {
                            MessageBox.Show("请选择缓冲区半径！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        //修改属性！！！！！！！！！！！！
                        tRow.set_Value(tRow.Fields.FindField("Attribute"), 1);
                        for (int i = 0; i < dataGridViewX2.RowCount; i++)
                        {
                            dgvCheckBoxCell = dataGridViewX2[0, i] as DataGridViewCheckBoxCell;
                            if (Convert.ToBoolean(dgvCheckBoxCell.Value) == true)
                            {
                                if (this.dataGridViewX2[1, i].Value.ToString().ToUpper() == this.dataGridViewX2[2, i].Value.ToString().ToUpper())
                                {
                                    tempFieldsName = tempFieldsName + dataGridViewX2[1, i].Value.ToString() + ";";
                                }
                                else
                                {
                                    tempFieldsName = tempFieldsName + dataGridViewX2[1, i].Value.ToString() + "(" + dataGridViewX2[2, i].Value.ToString() + ")";
                                }
                            }

                        }

                        if (tempFieldsName == "")
                        {
                            //tRow.set_Value(tRow.Fields.FindField("Attribute"), 0);
                            //MessageBox.Show("没有选择属性匹配对应字段！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            //tRow.set_Value(tRow.Fields.FindField("Attribute"), 1);
                            tRow.set_Value(tRow.Fields.FindField("MatchedFields"), tempFieldsName.Trim());
                        }
                        IDataset dataset = sourceFeatCls as IDataset;
                        if (ClsDeclare.g_SourceFeatClsPathDic.ContainsKey(dataset.Name))
                        {

                            tRow.set_Value(tRow.Fields.FindField("SourceFCName"), dataset.Name);
                            tRow.set_Value(tRow.Fields.FindField("SourcePath"), ClsDeclare.g_SourceFeatClsPathDic[dataset.Name]);
                            tRow.set_Value(tRow.Fields.FindField("WorkspacePath"), ClsDeclare.g_WorkspacePath);
                        }
                        tRow.Store();
                    }
                }
                pWorkspaceEdit.StopEditOperation();
                pWorkspaceEdit.StopEditing(true);
            }

        }
        /// <summary>
        /// 判断表是否为空，并进行相应的操作
        /// </summary>
        /// <param name="sourceFeatCls"></param>
        /// <param name="workspace"></param>
        /// <param name="table"></param>
        /// <param name="featureWorkspace"></param>
        /// <returns></returns>
        private ITable TableIsExist(IFeatureClass sourceFeatCls, string gdbPath, ITable table,
            IFeatureWorkspace featureWorkspace)
        {
            IWorkspaceFactory pWorkspaceFactory = new FileGDBWorkspaceFactory();
            IWorkspace2 workspace = pWorkspaceFactory.OpenFromFile(gdbPath, 0) as IWorkspace2;

            IFields fields;
            if (sourceFeatCls.ShapeType == esriGeometryType.esriGeometryPolyline ||
                sourceFeatCls.ShapeType == esriGeometryType.esriGeometryLine)
            {
                //如果表存在，那么打开表
                if (workspace.get_NameExists(esriDatasetType.esriDTTable, ClsConstant.lineSettingTable))
                {
                    table = featureWorkspace.OpenTable(ClsConstant.lineSettingTable);
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
                //如果不存在表就创建表
                else
                {
                    fields = CreateMatchedPolylineFCSettingFields(workspace);
                    UID uid = new UIDClass();
                    uid.Value = "esriGeoDatabase.Object";
                    IFieldChecker fieldChecker = new FieldCheckerClass();
                    IEnumFieldError enumFieldError = null;
                    IFields validatedFields = null;
                    fieldChecker.ValidateWorkspace = (IWorkspace)workspace;
                    fieldChecker.Validate(fields, out enumFieldError, out validatedFields);

                    table = featureWorkspace.CreateTable(ClsConstant.lineSettingTable, validatedFields, uid, null, "");
                }
            }
            return table;
        }

        #region radio按钮1
        private void radioButtonShape_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButtonShape.Checked)
            {
                this.labelShape.Enabled = true;
                this.WeightsliderShape.Enabled = true;
            }
            else
            {
                this.labelShape.Enabled = false;
                this.WeightsliderShape.Enabled = false;
            }
        }

        private void radioButtonCenter_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButtonNode.Checked)
            {
                this.WeightsliderNode.Enabled = true;
                this.labelNode.Enabled = true;
            }
            else
            {
                this.WeightsliderNode.Enabled = false;
                this.labelNode.Enabled = false;
            }
        }

        private void radioButtonArea_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButtonBuffer.Checked == true)
            {
                this.WeightsliderBuffer.Enabled = true;
                labelBufferRadius.Enabled = true;
            }
            else
            {
                this.WeightsliderBuffer.Enabled = false;
                labelBufferRadius.Enabled = false;
            }

        }

        #endregion
        #region 进度条
        /// <summary>
        /// 形状相似度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Weightslider_ValueChanged(object sender, EventArgs e)
        {
            this.labelShape.Text = Convert.ToDouble((WeightsliderShape.Value / 100.00)).ToString();
            WeightsliderNode.Value = (100 - WeightsliderShape.Value);
            //Weightslider2.Maximum = 100 - Weightslider1.Value;
            this.labelNode.Text = Convert.ToDouble((WeightsliderNode.Value / 100.00)).ToString();

        }
        /// <summary>
        /// 节点相似度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Weightslider2_ValueChanged(object sender, EventArgs e)
        {
            this.labelNode.Text = Convert.ToDouble((WeightsliderNode.Value / 100.00)).ToString();
        }
        /// <summary>
        /// 缓冲区半径
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Weightslider3_ValueChanged(object sender, EventArgs e)
        {
            this.labelBufferRadius.Text = Convert.ToDouble((WeightsliderBuffer.Value)).ToString();
        }
        /// <summary>
        /// 综合相似度阈值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Weightslider4_ValueChanged(object sender, EventArgs e)
        {
            this.labelTotal.Text = Convert.ToDouble((WeightsliderTotal.Value / 100.00)).ToString();
        }

        #endregion
        #region radio按钮2
        /// <summary>
        /// 形状
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rBtnShapeGeo_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rBtnShapeGeo.Checked)
            {
                this.sliderShapeGeo.Enabled = true;
                this.labelShapeGeo.Enabled = true;
            }
            else
            {
                this.sliderShapeGeo.Enabled = false;
                this.labelShapeGeo.Enabled = false;
            }
        }
        /// <summary>
        /// 节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rBtnNodeGeo_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rBtnNodeGeo.Checked)
            {
                this.sliderNodeGeo.Enabled = true;
                this.labelNodeGeo.Enabled = true;
            }
            else
            {
                this.sliderNodeGeo.Enabled = false;
                this.labelNodeGeo.Enabled = false;
            }
        }
        /// <summary>
        /// 缓冲区
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rBtnbufferGeo_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rBtnbufferGeo.Checked)
            {
                this.sliderBufferGeo.Enabled = true;
                this.labelBufferGeo.Enabled = true;
            }
            else
            {
                this.sliderBufferGeo.Enabled = false;
                this.labelBufferGeo.Enabled = false;
            }
        }
        /// <summary>
        /// 夹角
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rBtnIncludeAngleGeo_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rBtnIncludeAngleGeo.Checked)
            {
                this.sliderIncludeAngleGeo.Enabled = true;
                this.labelIncludeGeo.Enabled = true;
            }
            else
            {
                this.sliderIncludeAngleGeo.Enabled = false;
                this.labelIncludeGeo.Enabled = false;
            }
        }
        /// <summary>
        /// Hausdorff距离
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rBtnHausdorffGeo_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rBtnHausdorffGeo.Checked)
            {
                this.sliderHausdorffGeo.Enabled = true;
                this.labelHausdorffGeo.Enabled = true;
            }
            else
            {
                this.sliderHausdorffGeo.Enabled = false;
                this.labelHausdorffGeo.Enabled = false;
            }
        }
        #endregion

        #region 进度条2
        /// <summary>
        /// 形状相似度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sliderShapeGeo_ValueChanged(object sender, EventArgs e)
        {
            this.labelShapeGeo.Text = Convert.ToDouble((sliderShapeGeo.Value / 100.00)).ToString();
            sliderShapeGeo.Value = (100 - sliderShapeGeo.Value);
            //Weightslider2.Maximum = 100 - Weightslider1.Value;
            this.labelNodeGeo.Text = Convert.ToDouble((sliderNodeGeo.Value / 100.00)).ToString();
        }
        /// <summary>
        /// 节点相似度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sliderNodeGeo_ValueChanged(object sender, EventArgs e)
        {
            this.labelNodeGeo.Text = Convert.ToDouble((sliderNodeGeo.Value / 100.00)).ToString();
        }
        /// <summary>
        /// 缓冲区半径
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sliderBufferGeo_ValueChanged(object sender, EventArgs e)
        {
            this.labelBufferGeo.Text = Convert.ToDouble((sliderBufferGeo.Value)).ToString();
        }
        /// <summary>
        /// 综合相似度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sliderTotalGeo_ValueChanged(object sender, EventArgs e)
        {
            this.labelTotalGeo.Text = Convert.ToDouble((sliderTotalGeo.Value / 100.00)).ToString();
        }
        /// <summary>
        /// 夹角进度条
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sliderIncludeAngleGeo_ValueChanged(object sender, EventArgs e)
        {
            this.labelIncludeGeo.Text = Convert.ToDouble(sliderIncludeAngleGeo.Value).ToString();
        }
        /// <summary>
        /// Hausdorff距离进度条
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sliderHausdorffGeo_ValueChanged(object sender, EventArgs e)
        {
            this.labelHausdorffGeo.Text = Convert.ToDouble(sliderHausdorffGeo.Value).ToString();
        }
        #endregion
        #region checkbox事件
        /// <summary>
        /// 纯粹几何匹配
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxXGeo_Click(object sender, EventArgs e)
        {
            this.tabItemGeo.Visible = true;
            this.tabItemTopo.Visible = false;
            this.tabItemGAGeo.Visible = false;
            this.tabItemGAttr.Visible = false;
        }
        /// <summary>
        /// 几何属性匹配
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxXGeoAttr_Click(object sender, EventArgs e)
        {
            //this.tabControl1.Tabs[0].Visible = true;
            //this.tabControl1.Tabs[2].Visible = true;
            //this.tabControl1.Tabs[1].Visible = false;
            ////this.tabControl1.Tabs[1].Visible = true;

            this.tabItemGeo.Visible = false;
            this.tabItemTopo.Visible = false;
            this.tabItemGAGeo.Visible = true;
            this.tabItemGAttr.Visible = true;
        }
        /// <summary>
        /// 拓扑匹配
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxXTopo_Click(object sender, EventArgs e)
        {
            //this.tabControl1.Tabs[0].Visible = false;
            ////this.tabControl1.Tabs[1].Visible = false;
            //this.tabControl1.Tabs[1].Visible = true;
            //this.tabControl1.Tabs[2].Visible = false;
            this.tabItemGeo.Visible = false;
            this.tabItemTopo.Visible = true;
            this.tabItemGAGeo.Visible = false;
            this.tabItemGAttr.Visible = false;
        }

        private void checkBoxXGeo_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxXGeo.Checked)
            {
                checkBoxXGeo.Checked = true;
                checkBoxXGeoAttr.Checked = false;
                checkBoxXTopo.Checked = false;
            }
        }

        private void checkBoxXGeoAttr_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxXGeoAttr.Checked)
            {
                checkBoxXGeo.Checked = false;
                checkBoxXGeoAttr.Checked = true;
                checkBoxXTopo.Checked = false;
            }
        }

        private void checkBoxXTopo_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxXTopo.Checked)
            {
                checkBoxXGeo.Checked = false;
                checkBoxXGeoAttr.Checked = false;
                checkBoxXTopo.Checked = true;
            }
        }
        #endregion

   

    

    }
}
