using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
    public partial class FrmMatchPointDif : DevComponents.DotNetBar.Office2007Form
    {
        Form previousForm;
        Dictionary<int, DataGridViewRow> m_InRowDic;
        Dictionary<int, DataGridViewRow> m_OutRowDic;

        public FrmMatchPointDif()
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
                    dgvRow.Cells[3].Value = table + "_PtTable";


                }
            }

            //设置页面初始化
            IFeatureClass pTUFeatCls = this.dataGridViewX1[1, 0].Tag as IFeatureClass;
            IWorkspaceFactory2 pWorkspaceFactory = new FileGDBWorkspaceFactoryClass() as IWorkspaceFactory2;
            string path = ClsDeclare.g_WorkspacePath;
            //path需要GDB
            IWorkspace2 pWorkspace = pWorkspaceFactory.OpenFromFile(path, 0) as IWorkspace2;
            IFeatureWorkspace featureWorkspace = pWorkspace as IFeatureWorkspace;
            IFeatureClass pTEFeatCls = featureWorkspace.OpenFeatureClass(this.dataGridViewX1[2, 0].Value.ToString());

            Initload(pTUFeatCls, pTEFeatCls);
        }
        /// <summary>
        /// 初始化匹配界面设置
        /// </summary>
        /// <param name="sourceFeatCls"></param>
        /// <param name="targetFeatCls"></param>
        private void Initload(IFeatureClass sourceFeatCls, IFeatureClass targetFeatCls)
        {
            //初始化设置 labelXAttriShow按钮（请选择一个属性字段进行属性匹配）为false
            //this.labelXAttriShow.Visible = false;

            //if (this.comboBoxExMatchType.Items.Count > 0)
            //{
            //    //this.comboBoxExMatchType.Text = this.comboBoxExMatchType.Items[0].ToString();
            //    this.comboBoxExMatchType.Text = this.comboBoxExMatchType.Items[1].ToString();
            //}

            if (this.comboBoxExBuffer.Items.Count > 0)
            {
                this.comboBoxExBuffer.Text = this.comboBoxExBuffer.Items[1].ToString();
            }

            //20170911

            this.tabControl1.Tabs[0].Visible = true;
            //this.tabControl1.Tabs[0].Visible = false;
            this.tabControl1.Tabs[1].Visible = false;
            this.tabControl1.Tabs[2].Visible = true;
            //this.tabControl1.Tabs[2].Visible = false;

            //权重界面设置

            this.radioButtonBuffer.Checked = true;

            this.labelTotalNum.Enabled = true;
            this.WeightsliderTotal.Enabled = true;

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
            //dgvcbColumn.HeaderText = "更新图层字段";
            dgvcbColumn.HeaderText = "待匹配图层字段";
            dgvcbColumn.Width = dataGridViewX2.Width / 3;
            dataGridViewX2.Columns.Add(dgvcbColumn);
            LoadFields(sourceFeatCls, targetFeatCls);
        }
        /// <summary>
        /// 填充字段到DataGridView2中
        /// </summary>
        /// <param name="sourceFeatCls">源图层</param>
        /// <param name="targetFeatCls">待匹配图层</param>
        private void LoadFields(IFeatureClass sourceFeatCls, IFeatureClass targetFeatCls)
        {
            IFields2 sourceFields = sourceFeatCls.Fields as IFields2;
            IFields2 targetFields = targetFeatCls.Fields as IFields2;
            DataGridViewRow dgvRow;

            dataGridViewX2.Rows.Clear();
            bool bsource = false;
            bool btarget = false;
            for (int j = 0; j < sourceFields.FieldCount; j++)
            {
                if (sourceFields.get_Field(j).Name.Trim().Length > 0)
                {
                    bsource = true;
                }
            }
            for (int j = 0; j < targetFields.FieldCount; j++)
            {
                if (targetFields.get_Field(j).Name.Trim().Length > 0)
                {
                    btarget = true;
                }
            }

            for (int i = 0; i < sourceFields.FieldCount; i++)
            {
                if (sourceFields.get_Field(i).Type == esriFieldType.esriFieldTypeGeometry || sourceFields.get_Field(i).Editable == false)
                    continue;

                dgvRow = new DataGridViewRow();
                dgvRow = dataGridViewX2.Rows[dataGridViewX2.Rows.Add()];
                ////源图层和待匹配图层中都存在别名时，则用别名
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
                dgvRow.Cells[1].Value = sourceFields.get_Field(i).Name;

                DataGridViewCheckBoxCell dgvCheckBoxCell = new DataGridViewCheckBoxCell();
                dgvCheckBoxCell = dgvRow.Cells[0] as DataGridViewCheckBoxCell;

                DataGridViewComboBoxCell dgvComCell = new DataGridViewComboBoxCell();
                dgvComCell = dgvRow.Cells[2] as DataGridViewComboBoxCell;

                for (int j = 0; j < targetFields.FieldCount; j++)
                {
                    if (targetFields.get_Field(j).Type == esriFieldType.esriFieldTypeGeometry || targetFields.get_Field(j).Editable == false)
                        continue;
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
        /// <summary>
        /// 创建表
        /// </summary>
        /// <param name="workspace">保存路径</param>
        /// <param name="tableName">表名称</param>
        /// <param name="fields">字段集</param>
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
        /// 生成匹配字段
        /// </summary>
        /// <param name="workspace">保存路径</param>
        /// <returns></returns>
        public IFields CreateMatchPointTFields(IWorkspace2 workspace, string tempFieldsName)
        {
            IObjectClassDescription objectClassDescription = new ObjectClassDescriptionClass();
            IFields fields = new FieldsClass();
            IFieldsEdit fieldsEdit = fields as IFieldsEdit;
            fields = objectClassDescription.RequiredFields;

            //字段0
            IField field = new FieldClass();
            IFieldEdit fieldEdit = field as IFieldEdit;
            fieldEdit.Name_2 = "OBJECTID";
            fieldEdit.Type_2 = esriFieldType.esriFieldTypeOID;
            fieldEdit.IsNullable_2 = false;
            fieldsEdit.AddField(field);

            //字段1
            IField field1 = new FieldClass();
            IFieldEdit fieldEdit1 = field1 as IFieldEdit;
            fieldEdit1.Name_2 = "源OID";
            fieldEdit1.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit1.IsNullable_2 = true;
            fieldEdit1.AliasName_2 = "源OID";
            fieldEdit1.Editable_2 = true;
            fieldsEdit.AddField(field1);

            //字段2
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

            // todo ：分号3
            string[] strarry = tempFieldsName.Split(';');
            #region 多个字段
            for (int i = 0; i < strarry.Length; i++)
            {
                if (strarry[i].Length > 0)
                {
                    if (strarry[i].Contains(":"))
                    {
                        string[] arr = ClsStatic.SplitStrColon(strarry[i]);
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
            #endregion

            ////字段4
            //IField field4 = new FieldClass();
            //IFieldEdit fieldEdit4 = field4 as IFieldEdit;
            ////fieldEdit4.Name_2 = "源编码";
            ////fieldEdit4.Name_2 = "源分类码";
            //fieldEdit4.Name_2 = "源图层名称";
            //fieldEdit4.Type_2 = esriFieldType.esriFieldTypeString;
            //fieldEdit4.IsNullable_2 = true;
            ////fieldEdit4.AliasName_2 = "源编码";
            ////fieldEdit4.AliasName_2 = "源分类码";
            //fieldEdit4.AliasName_2 = "源图层名称";
            //fieldEdit4.Editable_2 = true;
            //fieldsEdit.AddField(field4);

            ////字段5
            //IField field5 = new FieldClass();
            //IFieldEdit fieldEdit5 = field5 as IFieldEdit;
            ////fieldEdit5.Name_2 = "更新编码";
            ////fieldEdit5.Name_2 = "待匹配编码";
            ////fieldEdit5.Name_2 = "待匹配分类码";
            //fieldEdit5.Name_2 = "待匹配图层名称";
            //fieldEdit5.Type_2 = esriFieldType.esriFieldTypeString;
            //fieldEdit5.IsNullable_2 = true;
            ////fieldEdit5.AliasName_2 = "更新编码";
            ////fieldEdit5.AliasName_2 = "待匹配编码";
            ////fieldEdit5.AliasName_2 = "待匹配分类码";
            //fieldEdit5.AliasName_2 = "待匹配图层名称";
            //fieldEdit5.Editable_2 = true;
            //fieldsEdit.AddField(field5);

            //字段3
            IField field3 = new FieldClass();
            IFieldEdit fieldEdit3 = field3 as IFieldEdit;
            fieldEdit3.Name_2 = "变化标记";
            fieldEdit3.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit3.IsNullable_2 = true;
            fieldEdit3.AliasName_2 = "变化标记";
            fieldEdit3.Editable_2 = true;
            fieldsEdit.AddField(field3);

            //20170911
            //字段6
            IField field6 = new FieldClass();
            IFieldEdit fieldEdit6 = field6 as IFieldEdit;
            fieldEdit6.Name_2 = "位置相似度";
            fieldEdit6.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit6.IsNullable_2 = true;
            fieldEdit6.AliasName_2 = "位置相似度";
            fieldEdit6.Editable_2 = true;
            fieldsEdit.AddField(field6);

            fields = fieldsEdit as IFields;
            return fields;

        }
        #region 点实体匹配
        //TODO ：点实体的开始匹配按钮
        /// <summary>
        /// 开始匹配（点）
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

            //创建并填充数据到表MatchedPointFCSetting、并进行属性匹配
            loadFeatSetting(gdbPath, targetFeatureName, pSourceFeatCls2);

            ITable table = null;
            IFields fileds = null;


            string tempFieldsName = "";
            // todo ：分号1
            //由dataGridViewX2来设置匹配属性字段——cell[12]
            for (int i = 0; i < dataGridViewX2.RowCount; i++)
            {
                DataGridViewCheckBoxCell dgvCheckBoxCell = dataGridViewX2[0, i] as DataGridViewCheckBoxCell;
                if (Convert.ToBoolean(dgvCheckBoxCell.Value) == true)
                {
                    //源图层名称和待匹配图层名称是否一致
                    //一致
                    if (this.dataGridViewX2[1, i].Value.ToString().Trim() == this.dataGridViewX2[2, i].Value.ToString().Trim())
                    {
                        tempFieldsName = tempFieldsName + dataGridViewX2[1, i].Value.ToString() + ";";
                    }
                    else   //不一致
                    {
                        tempFieldsName = tempFieldsName + dataGridViewX2[1, i].Value.ToString() + ":" + dataGridViewX2[2, i].Value.ToString() + ";";
                    }
                }

            }


            for (int i = 0; i < this.dataGridViewX1.Rows.Count; i++)
            {
                IFeatureClass pSourceFeatCls = this.dataGridViewX1.Rows[i].Cells[1].Tag as IFeatureClass;

                if (pSourceFeatCls.ShapeType == esriGeometryType.esriGeometryPoint)
                {
                    //创建结果表TRA_PT_I_PtTable字段——6个字段
                    fileds = CreateMatchPointTFields(pWorkspace, tempFieldsName);
                }
                //创建空表
                //如果表存在，就删除表
                if (pWorkspace.get_NameExists(esriDatasetType.esriDTTable, this.dataGridViewX1.Rows[i].Cells[3].Value.ToString()))
                {
                    //table = featureWorkspace.OpenTable(this.dataGridViewX1.Rows[i].Cells[3].Value.ToString());
                    ClsDeleteTables.DeleteFeatureClass(gdbPath, this.dataGridViewX1.Rows[i].Cells[3].Value.ToString());
                    //IWorkspaceEdit workspaceEdit = pWorkspace as IWorkspaceEdit;
                    //workspaceEdit.StartEditing(true);
                    //workspaceEdit.StartEditOperation();

                    //ICursor cursor = table.Search(null, false);
                    //IRow r = cursor.NextRow();
                    //while (r != null)
                    //{
                    //    r.Delete();
                    //    r = cursor.NextRow();
                    //}
                    //workspaceEdit.StopEditOperation();
                    //workspaceEdit.StopEditing(true);
                }
                table = CreateTable(pWorkspace, this.dataGridViewX1.Rows[i].Cells[3].Value.ToString(), fileds);

                this.dataGridViewX1[3, i].Tag = table;
                //待匹配图层的FeatureClass——pTEFeatCls
                IFeatureClass pTargetFeatCls = featureWorkspace.OpenFeatureClass(this.dataGridViewX1.Rows[i].Cells[2].Value.ToString());
                ITable tableSetting = null;
        
                double[] weight = new double[4];
                double buffer = 0;
                double manhattan = 0;
                string fields = "";

                IDataset dataset = pTargetFeatCls as IDataset;
                string matchedFCName = dataset.Name;

                //如果为点匹配
                if (pTargetFeatCls.ShapeType == esriGeometryType.esriGeometryPoint)
                {
                    tableSetting = featureWorkspace.OpenTable(ClsConstant.pointSettingTable);
                    ICursor cursor = tableSetting.Search(null, false);
                    IRow row = cursor.NextRow();
                    while (row != null)
                    {
                        //如果表MatchedPointFCSetting中的字段（待匹配图层名）等于待匹配的图层名
                        if (row.get_Value(row.Fields.FindField("MatchedFCName")).ToString() == matchedFCName)
                        {
                            // 拓扑匹配
                            if (this.tabControl1.Tabs[1].Visible == true)
                            {
                                //把表MatchedPointFCSetting中的（拓扑缓冲区半径）字段值 付给buffer变量
                                string temp = row.get_Value(row.Fields.FindField("Buffer")).ToString();
                                if (temp.Contains("米"))
                                {
                                    temp = temp.Substring(0, temp.LastIndexOf("米")).Trim();
                                }
                                buffer = Convert.ToDouble(temp);

                                //表MatchedPointFCSetting中的（匹配属性）字段值
                                fields = row.get_Value(row.Fields.FindField("MatchedFields")).ToString();

                                break;
                            }
                            //几何匹配
                            else if (this.tabControl1.Tabs[0].Visible == true)
                            {
                          
                                //表MatchedPointFCSetting中的（综合相似度阈值）字段值
                                weight[0] = Convert.ToDouble(row.get_Value(row.Fields.FindField("TotalNum")));
                                manhattan = Convert.ToDouble(row.get_Value(row.Fields.FindField("ManhattanDis")));
                                //把表MatchedPointFCSetting中的（搜索半径）字段值 付给buffer变量
                                string temp = row.get_Value(row.Fields.FindField("SearchedRadius")).ToString();
                                if (temp.Contains("米"))
                                {
                                    temp = temp.Substring(0, temp.LastIndexOf("米")).Trim();
                                }
                                buffer = Convert.ToDouble(temp);

                                //表MatchedPointFCSetting中的（匹配属性）字段值
                                fields = row.get_Value(row.Fields.FindField("MatchedFields")).ToString();

                            }
                        }
                        row = cursor.NextRow();
                    }
                }

                ClsPointMatch clsPointMatch = new ClsPointMatch();

                //几何匹配
                if (this.tabControl1.Tabs[0].Visible == true)
                {
                    clsPointMatch.SearchChangedFeatures(pSourceFeatCls, pTargetFeatCls, table, weight, buffer, manhattan,fields, progressBarMain, progressBarSub, labelXStatus, chkBoxPointIndicator);
                    sw.Stop();
                    //Console.WriteLine("Stopwatch 时间精度：{0}ms", sw.ElapsedMilliseconds);
                    MessageBoxEx.Show("几何匹配已完成！总需要时间" + sw.ElapsedMilliseconds + "ms", "提示");
                }
                //拓扑匹配
                else if (this.tabControl1.Tabs[1].Visible == true)
                {
                    clsPointMatch.SearchChangedFeaturesByTop(pSourceFeatCls, pTargetFeatCls, table, buffer, fields, progressBarMain, progressBarSub, labelXStatus);
                    sw.Stop();
                    MessageBoxEx.Show("拓扑匹配已完成！总需要时间" + sw.ElapsedMilliseconds + "ms", "提示");
                }

            }

            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.Close();


        }
        #endregion
        /// <summary>
        /// 上一步
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
        /// 创建表MatchedPointFCSetting字段
        /// </summary>
        /// <param name="workspace">保存路径</param>
        /// <returns></returns>
        private IFields CreateMatchedPointFCSettingFields()
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
            fieldEdit1.AliasName_2 = "待匹配图层名";
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
            fieldEdit3.Type_2 = esriFieldType.esriFieldTypeDouble;
            fieldEdit3.AliasName_2 = "位置相似度";
            fieldEdit3.IsNullable_2 = true;
            fieldsEdit.AddField(field3);

            IField field10 = new FieldClass();
            IFieldEdit fieldEdit10 = field10 as IFieldEdit;
            fieldEdit10.Name_2 = "TotalNum";
            fieldEdit10.Type_2 = esriFieldType.esriFieldTypeDouble;
            fieldEdit10.AliasName_2 = "综合相似度阈值";
            fieldEdit10.IsNullable_2 = true;
            fieldsEdit.AddField(field10);



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

            IField field4 = new FieldClass();
            IFieldEdit fieldEdit4 = field4 as IFieldEdit;
            fieldEdit4.Name_2 = "SearchedRadius";
            fieldEdit4.Type_2 = esriFieldType.esriFieldTypeDouble;
            fieldEdit4.AliasName_2 = "搜索半径";
            fieldEdit4.IsNullable_2 = true;
            fieldsEdit.AddField(field4);

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

            IField field20 = new FieldClass();
            IFieldEdit fieldEdit20 = field20 as IFieldEdit;
            fieldEdit20.Name_2 = "ManhattanDis";
            fieldEdit20.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit20.AliasName_2 = "曼哈顿距离";
            fieldEdit20.IsNullable_2 = true;
            fieldsEdit.AddField(field20);

            fields = fieldsEdit as IFieldsEdit;
            return fields;

        }
        #region 滑块滑动事件
        /// <summary>
        /// 缓冲区半径
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Weightslider_ValueChanged(object sender, EventArgs e)
        {
            //滑块的最大值设为1000，除以10之后就是100，这样设置可以更加明显看出滑块的移动
            this.labelBuffer.Text = Convert.ToDouble((Buffer.Value) / 10.0).ToString();
        }

        /// <summary>
        /// 综合相似度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Weightslider4_ValueChanged(object sender, EventArgs e)
        {
            this.labelTotalNum.Text = Convert.ToDouble((WeightsliderTotal.Value) / 20.0).ToString();
        }
        /// <summary>
        /// 曼哈顿距离
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WeightsliderManhattan_ValueChanged(object sender, EventArgs e)
        {
            this.labelManhattan.Text = Convert.ToDouble(WeightsliderTotal.Value).ToString();
        }
        #endregion
        /// <summary>
        /// 加载匹配参数
        /// </summary>
        /// <param name="gdbPath">表的存放路径</param>
        /// <param name="targetFeatureName">待匹配图层名称</param>
        /// <param name="sourceFeatCls">源图层</param>
        private void loadFeatSetting(string gdbPath, string targetFeatureName, IFeatureClass sourceFeatCls)
        {
            if ((targetFeatureName != "") && (ClsDeclare.g_WorkspacePath != ""))
            {
                //属性匹配
                //20170612注释掉
                //判断选择字段是否对应
                for (int i = 0; i < dataGridViewX2.RowCount; i++)
                {
                    DataGridViewCheckBoxCell dgvCheckBoxCell1 = dataGridViewX2[0, i] as DataGridViewCheckBoxCell;
                    //判断是否进行属性匹配，若进行，必须选取对应字段
                    if (Convert.ToBoolean(dgvCheckBoxCell1.Value) == true)
                    {
                        if (this.dataGridViewX2[2, i].Value == null)
                        {
                            MessageBoxEx.Show("没有选择属性匹配字段，请选择一个属性匹配字段！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                }


                //创建表MatchedPointFCSetting
                IWorkspaceFactory pWorkspaceFactory = new FileGDBWorkspaceFactory();
                IWorkspace workspace = pWorkspaceFactory.OpenFromFile(gdbPath, 0) as IWorkspace;
                IWorkspace2 workspace2 = pWorkspaceFactory.OpenFromFile(gdbPath, 0) as IWorkspace2;
                IFeatureWorkspace featureWorkspace = workspace2 as IFeatureWorkspace;
                ITable table = null;
                IFields fields = null;

                if (sourceFeatCls.ShapeType == esriGeometryType.esriGeometryPoint)
                {
                    //表MatchedPointFCSetting是否存在，存在就打开表
                    if (workspace2.get_NameExists(esriDatasetType.esriDTTable, ClsConstant.pointSettingTable))
                    {

                        ClsDeleteTables.DeleteTable(workspace, ClsConstant.pointSettingTable);
                        //table = featureWorkspace.OpenTable(ClsConstant.pointSettingTable);
                    }
                    //表MatchedPointFCSetting是否存在，不存在就创建表
                    //创建13个字段
                    fields = CreateMatchedPointFCSettingFields();
                    UID uid = new UIDClass();
                    uid.Value = "esriGeoDatabase.Object";
                    IFieldChecker fieldChecker = new FieldCheckerClass();
                    IEnumFieldError enumFieldError = null;
                    IFields validatedFields = null;
                    fieldChecker.ValidateWorkspace = (IWorkspace)workspace;
                    fieldChecker.Validate(fields, out enumFieldError, out validatedFields);

                    table = featureWorkspace.CreateTable(ClsConstant.pointSettingTable, validatedFields, uid, null, "");

                    //table = CreatTable(workspace2, table, featureWorkspace);
                }
                else
                {
                    MessageBoxEx.Show("同尺度匹配不允许不同的几何类型！");
                    return;
                }

                //填充表MatchedPointFCSetting
                IWorkspaceEdit pWorkspaceEdit = featureWorkspace as IWorkspaceEdit;
                pWorkspaceEdit.StartEditing(true);
                pWorkspaceEdit.StartEditOperation();
                Dictionary<string, int> pDic = new Dictionary<string, int>();
                string tempFieldsName = "";
                DataGridViewCheckBoxCell dgvCheckBoxCell = new DataGridViewCheckBoxCell();

                //遍历并判断表MatchedPointFCSetting的MatchedFCName（待匹配图层名）字段是否有值
                //有值就把值写入pDic字段中
                int index = table.FindField("MatchedFCName");
                ICursor pCursor = table.Search(null, false);
                IRow pRow = pCursor.NextRow();
                while (pRow != null)
                {
                    if (pRow.get_Value(index).ToString() != "")
                    {
                        pDic.Add(pRow.get_Value(index).ToString(), pRow.OID);
                    }
                    pRow = pCursor.NextRow();
                }

                string tempBuffer = "";

                //几何+属性匹配
                if (this.tabControl1.Tabs[0].Visible)
                {
                    //字典中不包含待匹配图层名称m_MatchedFCName
                    //说明MatchedPointFCSetting表中没有任何记录
                    if (!pDic.ContainsKey(targetFeatureName))
                    {

                        IRow tempRow = table.CreateRow();

                        IDataset dataset = sourceFeatCls as IDataset;
                        if (ClsDeclare.g_SourceFeatClsPathDic.ContainsKey(dataset.Name))
                        {
                            //设置源图层名称——cell[1]
                            tempRow.set_Value(tempRow.Fields.FindField("SourceFCName"), dataset.Name);
                            //设置源图层路径——cell[2]
                            tempRow.set_Value(tempRow.Fields.FindField("SourcePath"), ClsDeclare.g_SourceFeatClsPathDic[dataset.Name]);
                            //设置待匹配图层路径——cell[4]
                            tempRow.set_Value(tempRow.Fields.FindField("WorkspacePath"), ClsDeclare.g_WorkspacePath);
                        }

                        //由传入的待匹配图层名设置（待匹配图层名）m_MatchedFCName字段——cell[3]列
                        tempRow.set_Value(index, targetFeatureName);
                        //由MatchedPointFCSetting的行数设置参数ID_cell[5]
                        tempRow.set_Value(tempRow.Fields.FindField("FCSettingID"), table.RowCount(null) - 1);
                        //由labelBuffer设置位置相似度——cell[6]
                        tempRow.set_Value(tempRow.Fields.FindField("Center"), Convert.ToDouble(labelBuffer.Text));
                        tempRow.set_Value(tempRow.Fields.FindField("ManhattanDis"), Convert.ToDouble(labelManhattan.Text));

                        //由labelTotalNum设置综合相似度阈值——cell[7]
                        //根据源图层的坐标系统，把平面坐标的距离转化为源图层下的度数
                        if (labelTotalNum.Text != "")
                        {
                            //读取的label的值
                            string temStr = labelTotalNum.Text;
                            if (temStr.Contains("米"))
                            {
                                temStr = temStr.Substring(0, temStr.LastIndexOf("米"));
                            }
                            IDataset dataset1 = sourceFeatCls as IDataset;
                            IGeoDataset geoDataset = dataset1 as IGeoDataset;
                            double a = ClsConvertUnit.GetBufferValueByUnit(geoDataset.SpatialReference, Convert.ToDouble(temStr));
                            string sa = a.ToString("#0.00000");
                            //综合相似度阈值
                            tempRow.set_Value(tempRow.Fields.FindField("TotalNum"), sa);
                        }

                        //由labelBuffer设置搜索半径——cell[9]
                        //根据源图层的坐标系统，把平面坐标的距离转化为源图层下的度数
                        if (this.comboBoxExBuffer.Text != "")
                        {
                            //读取的是拓扑匹配中comboBox的值
                            tempBuffer = this.comboBoxExBuffer.Text;
                            if (tempBuffer.Contains("米"))
                            {
                                tempBuffer = tempBuffer.Substring(0, tempBuffer.LastIndexOf("米"));
                            }
                            IDataset dataset1 = sourceFeatCls as IDataset;
                            IGeoDataset geoDataset = dataset1 as IGeoDataset;

                            double b = ClsConvertUnit.GetBufferValueByUnit(geoDataset.SpatialReference, Convert.ToDouble(tempBuffer));
                            string sb = b.ToString("#0.000000");
                            tempRow.set_Value(tempRow.Fields.FindField("buffer"), sb);
                        }
                        else
                        {
                            MessageBoxEx.Show("请选择缓冲区半径！");
                            return;
                        }



                        //由labelBuffer设置搜索半径——cell[10]
                        //根据源图层的坐标系统，把平面坐标的距离转化为源图层下的度数
                        if (this.labelBuffer.Text != "")
                        {
                            //读取的是label的值
                            tempBuffer = this.labelBuffer.Text;
                            if (tempBuffer.Contains("米"))
                            {
                                tempBuffer = tempBuffer.Substring(0, tempBuffer.LastIndexOf("米"));
                            }
                            IDataset dataset1 = sourceFeatCls as IDataset;
                            IGeoDataset geoDataset = dataset1 as IGeoDataset;
                            double b = ClsConvertUnit.GetBufferValueByUnit(geoDataset.SpatialReference, Convert.ToDouble(tempBuffer));
                            string sb = b.ToString("#0.00000");
                            tempRow.set_Value(tempRow.Fields.FindField("SearchedRadius"), sb);
                        }
                        else
                        {
                            MessageBoxEx.Show("请选择缓冲区半径！");
                            return;
                        }

                        //设置属性匹配字段为1——cell[11]
                        tempRow.set_Value(tempRow.Fields.FindField("Attribute"), 1);

                        // todo ：分号2
                        //由dataGridViewX2来设置匹配属性字段——cell[12]
                        for (int i = 0; i < dataGridViewX2.RowCount; i++)
                        {
                            dgvCheckBoxCell = dataGridViewX2[0, i] as DataGridViewCheckBoxCell;
                            if (Convert.ToBoolean(dgvCheckBoxCell.Value) == true)
                            {
                                if (this.dataGridViewX2[1, i].Value.ToString() == this.dataGridViewX2[2, i].Value.ToString())
                                {
                                    tempFieldsName = tempFieldsName + dataGridViewX2[1, i].Value.ToString() + ";";
                                }
                                else
                                {
                                    tempFieldsName = tempFieldsName + dataGridViewX2[1, i].Value.ToString() + ":" + dataGridViewX2[2, i].Value.ToString();
                                }
                            }

                        }
                        if (tempFieldsName == "")
                        {
                            //tRow.set_Value(tRow.Fields.FindField("Attribute"), 0);
                            //MessageBoxEx.Show("没有选择属性匹配对应字段！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            //tRow.set_Value(tRow.Fields.FindField("Attribute"), 1);
                            tempRow.set_Value(tempRow.Fields.FindField("MatchedFields"), tempFieldsName.Trim());
                        }

                        tempRow.Store();
                    }
                    //字典中包含待匹配图层名称m_MatchedFCName这个键
                    //说明MatchedPointFCSetting含有数据
                    else
                    {
                        IRow tRow = table.GetRow(pDic[targetFeatureName]);
                        IDataset dataset = sourceFeatCls as IDataset;
                        if (ClsDeclare.g_SourceFeatClsPathDic.ContainsKey(dataset.Name))
                        {
                            //设置源图层名称——cell[1]
                            tRow.set_Value(tRow.Fields.FindField("SourceFCName"), dataset.Name);
                            //设置源图层路径——cell[2]
                            tRow.set_Value(tRow.Fields.FindField("SourcePath"), ClsDeclare.g_SourceFeatClsPathDic[dataset.Name]);
                            //设置待匹配图层路径——cell[4]
                            tRow.set_Value(tRow.Fields.FindField("WorkspacePath"), ClsDeclare.g_WorkspacePath);
                        }

                        //由传入的待匹配图层名设置（待匹配图层名）m_MatchedFCName字段——cell[3]列
                        tRow.set_Value(index, targetFeatureName);
                        //由MatchedPointFCSetting的行数设置参数ID_cell[5]
                        tRow.set_Value(tRow.Fields.FindField("FCSettingID"), table.RowCount(null) - 1);
                        //由labelBuffer设置位置相似度——cell[6]
                        tRow.set_Value(tRow.Fields.FindField("Center"), Convert.ToDouble(labelBuffer.Text));

                        //由labelTotalNum设置综合相似度阈值——cell[7]
                        //根据源图层的坐标系统，把平面坐标的距离转化为源图层下的度数
                        tRow.set_Value(tRow.Fields.FindField("TotalNum"), Convert.ToDouble(labelTotalNum.Text));
                        tRow.set_Value(tRow.Fields.FindField("ManhattanDis"), Convert.ToDouble(labelManhattan.Text));

                        //由labelBuffer设置搜索半径——cell[9]
                        //根据源图层的坐标系统，把平面坐标的距离转化为源图层下的度数
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
                            tRow.set_Value(tRow.Fields.FindField("buffer"), sb);
                        }
                        else
                        {
                            MessageBoxEx.Show("请选择缓冲区半径！");
                            return;
                        }

                        //由labelBuffer设置搜索半径——cell[10]
                        //根据源图层的坐标系统，把平面坐标的距离转化为源图层下的度数
                        if (this.labelBuffer.Text != "")
                        {
                            tempBuffer = this.labelBuffer.Text;
                            if (tempBuffer.Contains("米"))
                            {
                                tempBuffer = tempBuffer.Substring(0, tempBuffer.LastIndexOf("米"));
                            }
                            IDataset dataset1 = sourceFeatCls as IDataset;
                            IGeoDataset geoDataset = dataset1 as IGeoDataset;

                            double b = ClsConvertUnit.GetBufferValueByUnit(geoDataset.SpatialReference, Convert.ToDouble(tempBuffer));
                            string sb = b.ToString("#0.000000");
                            tRow.set_Value(tRow.Fields.FindField("SearchedRadius"), sb);
                        }
                        else
                        {
                            MessageBoxEx.Show("请选择缓冲区半径！");
                            return;
                        }
                        //设置属性匹配字段为1——cell[11]
                        tRow.set_Value(tRow.Fields.FindField("Attribute"), 1);
                        //由dataGridViewX2来设置匹配属性字段——cell[12]
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

                        if (tempFieldsName == "")
                        {
                            //tRow.set_Value(tRow.Fields.FindField("Attribute"), 0);
                            //MessageBoxEx.Show("没有选择属性匹配对应字段！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            //tRow.set_Value(tRow.Fields.FindField("Attribute"), 1);
                            tRow.set_Value(tRow.Fields.FindField("MatchedFields"), tempFieldsName.Trim());
                        }

                        tRow.Store();
                    }
                }
                pWorkspaceEdit.StopEditOperation();
                pWorkspaceEdit.StopEditing(true);
            }


        }

  

        #region checkbox事件
        private void checkBoxXGeoAttr_Click(object sender, EventArgs e)
        {
            this.tabControl1.Tabs[0].Visible = true;
            this.tabControl1.Tabs[1].Visible = false;
            //this.tabControl1.Tabs[1].Visible = true;
            this.tabControl1.Tabs[2].Visible = true;
        }

        private void checkBoxXTopo_Click(object sender, EventArgs e)
        {
            this.tabControl1.Tabs[0].Visible = false;
            //this.tabControl1.Tabs[1].Visible = false;
            this.tabControl1.Tabs[1].Visible = true;
            this.tabControl1.Tabs[2].Visible = false;
        }

        private void checkBoxXGeoAttr_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxXGeoAttr.Checked)
            {
                checkBoxXTopo.Checked = false;
                checkBoxXGeoAttr.Checked = true;
            }
        }

        private void checkBoxXTopo_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxXTopo.Checked)
            {
                checkBoxXGeoAttr.Checked = false;
                checkBoxXTopo.Checked = true;
            }
        }

        #endregion
    }
}
