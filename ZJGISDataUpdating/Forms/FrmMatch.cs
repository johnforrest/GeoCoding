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
//using System.Text.RegularExpressions;
namespace ZJGISDataUpdating
{
    public partial class FrmMatch : DevComponents.DotNetBar.Office2007Form
    {
        Form previousForm;

        Dictionary<int, DataGridViewRow> m_InRowDic;
        Dictionary<int, DataGridViewRow> m_OutRowDic;
     
        public FrmMatch()
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
            dgvCheckBoxColumn.Width = Convert.ToInt32(width*0.1); 
            this.dataGridViewX1.Columns.Add(dgvCheckBoxColumn);

            this.dataGridViewX1.Columns.Add("SourceFileName", "源图层名称");
            this.dataGridViewX1.Columns[1].Width = Convert.ToInt32(width * 0.25);
            this.dataGridViewX1.Columns[1].ReadOnly = true;

            this.dataGridViewX1.Columns.Add("workspaceFileName", "工作层名称");
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
                    dgvRow.Cells[3].Value = table +"_PyTable";


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
            if (this.comboBoxExMatchType.Items.Count > 0)
            {
                this.comboBoxExMatchType.Text = this.comboBoxExMatchType.Items[0].ToString();
            }
            this.tabControl1.Tabs[0].Visible = true;
            this.tabControl1.Tabs[1].Visible = false;
            this.tabControl1.Tabs[2].Visible = true;
            //权重界面设置
            this.radioButtonShape.Checked = true;
            this.radioButtonCenter.Checked = false;
            this.radioButtonArea.Checked = false;

            
            this.radioButtonArea.Enabled = false;

            this.labelAreaNum.Enabled = false;
            this.Weightslider2.Enabled = false;
            this.labelCenterNum.Enabled = false;
            this.Weightslider3.Enabled = false;
            this.labelTotalNum.Enabled = true;
            this.Weightslider4.Enabled = true;

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

        private void LoadFields(IFeatureClass sourceFeatCls, IFeatureClass targetFeatCls)
        {
            IFields2 sourceFields = sourceFeatCls.Fields as IFields2;
            IFields2 targetFields = targetFeatCls.Fields as IFields2;
            DataGridViewRow dgvRow;

            dataGridViewX2.Rows.Clear();

            for (int i = 0; i < sourceFields.FieldCount; i++)
            {
                if (sourceFields.get_Field(i).Type == esriFieldType.esriFieldTypeGeometry || sourceFields.get_Field(i).Editable == false)
                    continue;

                dgvRow = new DataGridViewRow();
                dgvRow = dataGridViewX2.Rows[dataGridViewX2.Rows.Add()];
                dgvRow.Cells[1].Value = sourceFields.get_Field(i).AliasName;

                DataGridViewCheckBoxCell dgvCheckBoxCell = new DataGridViewCheckBoxCell();
                dgvCheckBoxCell = dgvRow.Cells[0] as DataGridViewCheckBoxCell;

                DataGridViewComboBoxCell dgvComCell = new DataGridViewComboBoxCell();
                dgvComCell = dgvRow.Cells[2] as DataGridViewComboBoxCell;

                for (int j = 0; j < targetFields.FieldCount; j++)
                {
                    if (targetFields.get_Field(j).Type == esriFieldType.esriFieldTypeGeometry || targetFields.get_Field(j).Editable == false)
                        continue;
                    dgvComCell.Items.Add(targetFields.get_Field(j).AliasName);

                    if (targetFields.get_Field(j).AliasName == sourceFields.get_Field(i).AliasName)
                    {
                        dgvComCell.Value = targetFields.get_Field(j).AliasName;
                       
                        dgvCheckBoxCell.Value = false;
                    }
                }
            }

        }
        //创建表
        public ITable CreateTable(IWorkspace2 workspace,string tableName,IFields fields)
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
        //设置匹配多边形字段
        public IFields CreateMatchPolygonTFields(IWorkspace2 workspace)
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
                fieldEdit5.Name_2 = "更新编码";
                fieldEdit5.Type_2 = esriFieldType.esriFieldTypeString;
                fieldEdit5.IsNullable_2 = true;
                fieldEdit5.AliasName_2 = "更新编码";
                fieldEdit5.Editable_2 = true;
                fieldsEdit.AddField(field5);


                IField field6 = new FieldClass();
                IFieldEdit fieldEdit6 = field6 as IFieldEdit;
                fieldEdit6.Name_2 = "位置相似度";
                fieldEdit6.Type_2 = esriFieldType.esriFieldTypeString;
                fieldEdit6.IsNullable_2 = true;
                fieldEdit6.AliasName_2 = "位置相似度";
                fieldEdit6.Editable_2 = true;
                fieldsEdit.AddField(field6);

                IField field7 = new FieldClass();
                IFieldEdit fieldEdit7 = field7 as IFieldEdit;
                fieldEdit7.Name_2 = "面积相似度";
                fieldEdit7.Type_2 = esriFieldType.esriFieldTypeString;
                fieldEdit7.IsNullable_2 = true;
                fieldEdit7.AliasName_2 = "面积相似度";
                fieldEdit7.Editable_2 = true;
                fieldsEdit.AddField(field7);

                IField field8 = new FieldClass();
                IFieldEdit fieldEdit8 = field8 as IFieldEdit;
                fieldEdit8.Name_2 = "形状相似度";
                fieldEdit8.Type_2 = esriFieldType.esriFieldTypeString;
                fieldEdit8.IsNullable_2 = true;
                fieldEdit8.AliasName_2 = "形状相似度";
                fieldEdit8.Editable_2 = true;
                fieldsEdit.AddField(field8);

                IField field9 = new FieldClass();
                IFieldEdit fieldEdit9 = field9 as IFieldEdit;
                fieldEdit9.Name_2 = "综合相似度";
                fieldEdit9.Type_2 = esriFieldType.esriFieldTypeString;
                fieldEdit9.IsNullable_2 = true;
                fieldEdit9.AliasName_2 = "综合相似度";
                fieldEdit9.Editable_2 = true;
                fieldsEdit.AddField(field9);
            fields = fieldsEdit as IFields;
            return fields;
        } 
        private void buttonXStartMatch_Click(object sender, EventArgs e)
        {
            double Total = Convert.ToDouble(labelCenterNum.Text)+Convert.ToDouble(labelAreaNum.Text)+Convert.ToDouble(labelSpNum.Text);
            if(Total !=1)
            {
                MessageBoxEx.Show("综合权重之和不为1 ！");
                return;
            }
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            IWorkspaceFactory2 pWorkspaceFactory = new FileGDBWorkspaceFactoryClass() as IWorkspaceFactory2;
            string path = ClsDeclare.g_WorkspacePath;
            IWorkspace2 pWorkspace = pWorkspaceFactory.OpenFromFile(path, 0) as IWorkspace2;
            IFeatureWorkspace featureWorkspace = pWorkspace as IFeatureWorkspace;

            IFeatureClass pTUFeatCls2 = this.dataGridViewX1[1, 0].Tag as IFeatureClass;
            string fileName = this.dataGridViewX1[2, 0].Value.ToString();
            //创建设置表
            loadFeatSetting(path,fileName,pTUFeatCls2);
            ITable table = null;
            IFields fileds = null;
                for (int i = 0; i < this.dataGridViewX1.Rows.Count; i++)
                {
                        IFeatureClass pTUFeatCls = this.dataGridViewX1.Rows[i].Cells[1].Tag as IFeatureClass;
                  
                        if (pTUFeatCls.ShapeType == esriGeometryType.esriGeometryPolygon)
                        {
                            fileds = CreateMatchPolygonTFields(pWorkspace);//结果表字段
                        }
                       
                      
                        //创建空表
                        if (pWorkspace.get_NameExists(esriDatasetType.esriDTTable, this.dataGridViewX1.Rows[i].Cells[3].Value.ToString()))
                        {
                            table = featureWorkspace.OpenTable(this.dataGridViewX1.Rows[i].Cells[3].Value.ToString());
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
                        else
                        {
                            table = CreateTable(pWorkspace, this.dataGridViewX1.Rows[i].Cells[3].Value.ToString(), fileds);
                        }
                        this.dataGridViewX1[3, i].Tag = table;
                        IFeatureClass pTEFeatCls = featureWorkspace.OpenFeatureClass(this.dataGridViewX1.Rows[i].Cells[2].Value.ToString());

                        ITable tableSetting = null;
                        int matchedMode = -1;
                        int matchedMay = -1;
                        double[] weight = new double[4];
                        double buffer = 0;
                        string fields = "";

                        IDataset dataset = pTEFeatCls as IDataset;
                        string matchedFCName = dataset.Name;
                        //临时层有_TE
                       // matchedFCName = matchedFCName.Substring(0, matchedFCName.LastIndexOf("_"));
                        //设置匹配选择方式，matchedMay为0 代表几何匹配  为1代表拓扑匹配  为2代表属性匹配 
                        //设置匹配方法配置，如果matchedMay为0,  matchedMode=1 正切匹配，matchedMode=2 重心匹配 matchedMode=3 面积匹配 
                        if (pTEFeatCls.ShapeType == esriGeometryType.esriGeometryPolygon)//如果为面匹配
                        {
                            tableSetting = featureWorkspace.OpenTable("MatchedPolygonFCSetting");
                            ICursor cursor = tableSetting.Search(null, false);
                            IRow row = cursor.NextRow();
                            while (row != null)
                            {
                                if (row.get_Value(row.Fields.FindField("MatchedFCName")).ToString() == matchedFCName)
                                {
                                    if (row.get_Value(row.Fields.FindField("Top")).ToString() == "1")//拓扑匹配
                                    {
                                        matchedMay = 1;
                                        matchedMode = -1;
                                        string temp = row.get_Value(row.Fields.FindField("Buffer")).ToString();
                                        if (temp.Contains("米"))
                                        {
                                            temp = temp.Substring(0, temp.LastIndexOf("米")).Trim();
                                        }
                                        buffer = Convert.ToDouble(temp);
                                        fields = row.get_Value(row.Fields.FindField("MatchedFields")).ToString();
                                        break;
                                    }
                                    else
                                    {
                                        fields = row.get_Value(row.Fields.FindField("MatchedFields")).ToString();

                                        double sp = Convert.ToDouble(row.get_Value(row.Fields.FindField("SP")));
                                        double center = Convert.ToDouble(row.get_Value(row.Fields.FindField("Center")));
                                        double area = Convert.ToDouble(row.get_Value(row.Fields.FindField("Area")));
                                        double totalNum = Convert.ToDouble(row.get_Value(row.Fields.FindField("TotalNum")));
                                       
                                        matchedMay = 0;
                                        matchedMode = 123;
                                        weight[0] = sp;
                                        weight[1] = center;
                                        weight[2] = area;
                                        weight[3] = totalNum;
                                        buffer = 0;
                                       
                                    }
                                }
                                row = cursor.NextRow();
                            }
                        }
                        else if (pTEFeatCls.ShapeType == esriGeometryType.esriGeometryPolyline)
                        {
                            tableSetting = featureWorkspace.OpenTable(ClsConstant.lineSettingTable);
                            ICursor cursor = tableSetting.Search(null, false);
                            IRow row = cursor.NextRow();
                            while (row != null)
                            {
                                if (row.get_Value(row.Fields.FindField("MatchedFCName")).ToString() == matchedFCName)
                                {
                                    if (row.get_Value(row.Fields.FindField("Top")).ToString() == "1")
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
                                    else
                                    {
                                        fields = row.get_Value(row.Fields.FindField("MatchedFields")).ToString();

                                        string sp = row.get_Value(row.Fields.FindField("SP")).ToString();
                                        string point = row.get_Value(row.Fields.FindField("MatchedPoints")).ToString();

                                        if (sp == "1" && point == "0")
                                        {
                                            matchedMay = 0;
                                            matchedMode = 1;
                                            weight[0] = Convert.ToDouble(row.get_Value(row.Fields.FindField("SPNum")));
                                            string temp = row.get_Value(row.Fields.FindField("MatchedPointsBuffer")).ToString();
                                            if (temp.Contains("米"))
                                            {
                                                temp = temp.Substring(0, temp.LastIndexOf("米")).Trim();
                                            }
                                            buffer = Convert.ToDouble(temp);
                                            //fields = "";

                                            break;
                                        }
                                        else if (sp == "0" && point == "1")
                                        {
                                            matchedMay = 0;
                                            matchedMode = 2;
                                            weight[1] = Convert.ToDouble(row.get_Value(row.Fields.FindField("MatchedPointsNum")));
                                            string temp = row.get_Value(row.Fields.FindField("MatchedPointsBuffer")).ToString();
                                            if (temp.Contains("米"))
                                            {
                                                temp = temp.Substring(0, temp.LastIndexOf("米")).Trim();
                                            }
                                            buffer = Convert.ToDouble(temp);
                                            //fields = "";
                                            break;
                                        }
                                    }
                                }
                                row = cursor.NextRow();
                            }
                        }

                        ClsCoreUpdateFun clsCoreUpdateFun = new ClsCoreUpdateFun();
                        if (matchedMay == 0)//几何匹配
                        {
                            clsCoreUpdateFun.SearchChangedFeatures(pTUFeatCls, pTEFeatCls, table, matchedMode, weight, buffer, fields, progressBarMain, progressBarSub, labelXStatus);
                                //反向匹配
                         if (ckbEachMatch.Checked)
                        {                                                                  
                            //读取匹配改变的要素的ID
                            ICursor cursor2 = table.Search(null, false);
                            IRow row = cursor2.NextRow();
                            List<int> UpdataDataID = new List<int>();
                            while (row != null)
                            {
                                string strResult = row.get_Value(3).ToString();
                                if (strResult != "未变化" && strResult != "新增要素")
                                {
                                    string str = row.get_Value(2).ToString();//获得更新要素ID
                                    if (str != null && str != "")
                                    {
                                        if (str.Contains(";"))
                                        {
                                            string[] strArray = str.Split(';');
                                            foreach (string strID in strArray)
                                                UpdataDataID.Add(Convert.ToInt32(strID));
                                        }
                                        else
                                            UpdataDataID.Add(Convert.ToInt32(str));
                                    }
                                }
                                row = cursor2.NextRow();
                            }
                            if (UpdataDataID.Count == 0)
                            {
                                return;
                            }
                            //添加一行用于反向隔开
                            IWorkspaceEdit workspaceEdit = pWorkspace as IWorkspaceEdit;
                            workspaceEdit.StartEditing(true);
                            workspaceEdit.StartEditOperation();

                            IRowBuffer invalidRowBuffer = table.CreateRowBuffer();
                            ICursor invalidRowCursor = table.Insert(true);
                            invalidRowCursor.InsertRow(invalidRowBuffer);

                            workspaceEdit.StopEditOperation();
                            workspaceEdit.StopEditing(true);

                            clsCoreUpdateFun.ReverseSearchChangedFeatures(pTEFeatCls, UpdataDataID, pTUFeatCls, table, matchedMode, weight, buffer, fields, progressBarMain, progressBarSub, labelXStatus);
                        }
                         MessageBoxEx.Show("匹配已完成！", "提示");
                }
                 
                        else if (matchedMay == 1)
                        {
                            clsCoreUpdateFun.SearchChangedFeaturesByTop(pTUFeatCls, pTEFeatCls, table, buffer, fields, progressBarMain, progressBarSub, labelXStatus);
                        }


                }
   
          



            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.Close();
            //加载匹配结果 
            //this.Visible = false;
           // LoadMathResult(path,table);
           
          
        }
       
        private void buttonXPrevious_Click(object sender, EventArgs e)
        {
            if(previousForm!=null)
            {
                if (!previousForm.Visible)
                {
                    previousForm.Visible = true;
                }
                previousForm.Activate();
                this.Close();
            }
        }
        //设置匹配参数
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
            fieldEdit1.AliasName_2 = "更新图层名";
            fieldsEdit.AddField(field1);

            IField field18 = new FieldClass();
            IFieldEdit fieldEdit18 = field18 as IFieldEdit;
            fieldEdit18.Name_2 = "WorkspacePath";
            fieldEdit18.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit18.IsNullable_2 = false;
            fieldEdit18.AliasName_2 = "工作区路径";
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
            fieldEdit3.AliasName_2 = "重心点匹配";
            fieldEdit3.IsNullable_2 = true;
            fieldsEdit.AddField(field3);

          
            //IField field5 = new FieldClass();
            //IFieldEdit fieldEdit5 = field5 as IFieldEdit;
            //fieldEdit5.Name_2 = "CenterWT";
            //fieldEdit5.Type_2 = esriFieldType.esriFieldTypeDouble;
            //fieldEdit5.AliasName_2 = "重心点权重";
            //fieldEdit5.IsNullable_2 = true;
            //fieldsEdit.AddField(field5);

            IField field6 = new FieldClass();
            IFieldEdit fieldEdit6 = field6 as IFieldEdit;
            fieldEdit6.Name_2 = "Area";
            fieldEdit6.Type_2 = esriFieldType.esriFieldTypeDouble;
            fieldEdit6.AliasName_2 = "面积匹配";
            fieldEdit6.IsNullable_2 = true;
            fieldsEdit.AddField(field6);

            //IField field8 = new FieldClass();
            //IFieldEdit fieldEdit8 = field8 as IFieldEdit;
            //fieldEdit8.Name_2 = "AreaWT";
            //fieldEdit8.Type_2 = esriFieldType.esriFieldTypeDouble;
            //fieldEdit8.AliasName_2 = "面积权重";
            //fieldEdit8.IsNullable_2 = true;
            //fieldsEdit.AddField(field8);


            IField field9 = new FieldClass();
            IFieldEdit fieldEdit9 = field9 as IFieldEdit;
            fieldEdit9.Name_2 = "SP";
            fieldEdit9.Type_2 = esriFieldType.esriFieldTypeDouble;
            fieldEdit9.AliasName_2 = "形状匹配";
            fieldEdit9.IsNullable_2 = true;
            fieldsEdit.AddField(field9);

            IField field10 = new FieldClass();
            IFieldEdit fieldEdit10 = field10 as IFieldEdit;
            fieldEdit10.Name_2 = "TotalNum";
            fieldEdit10.Type_2 = esriFieldType.esriFieldTypeDouble;
            fieldEdit10.AliasName_2 = "综合相似度阈值";
            fieldEdit10.IsNullable_2 = true;
            fieldsEdit.AddField(field10);

            //IField field11 = new FieldClass();
            //IFieldEdit fieldEdit11 = field11 as IFieldEdit;
            //fieldEdit11.Name_2 = "SPWT";
            //fieldEdit11.Type_2 = esriFieldType.esriFieldTypeDouble;
            //fieldEdit11.AliasName_2 = "形状权重";
            //fieldEdit11.IsNullable_2 = true;
            //fieldsEdit.AddField(field11);

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

            fields = fieldsEdit as IFieldsEdit;
            return fields;

        }

        private void comboBoxExMatchType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxExMatchType.Text != "")
            {
                if (this.comboBoxExMatchType.Text == "几何匹配")
                {
                    if (!this.tabControl1.Tabs[0].Visible)
                    {
                        this.tabControl1.Tabs[0].Visible = true;
                    }
                    this.tabControl1.Tabs[1].Visible = false;
                   
                }
                else if (this.comboBoxExMatchType.Text == "拓扑匹配")
                {
                    if (!this.tabControl1.Tabs[1].Visible)
                    {
                        this.tabControl1.Tabs[1].Visible = true;
                    }
                    this.tabControl1.Tabs[0].Visible = false;
                   
                }
               
            }
        }

        private void radioButtonShape_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButtonShape.Checked)
            {
               
                this.labelSpNum.Enabled = true;

                this.Weightslider1.Enabled = true;
              
            }
            else
            {
               
                this.labelAreaNum.Enabled = false;

                this.Weightslider1.Enabled = false;

            }
        }

        private void radioButtonCenter_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButtonCenter.Checked)
            {
               
                this.Weightslider2.Enabled = true;
                this.labelCenterNum.Enabled = true;
            }
            else
            {
               
                this.Weightslider2.Enabled = false;
                this.labelCenterNum.Enabled = false;
            }
        }

        private void Weightslider_ValueChanged(object sender, EventArgs e)
        {
            this.labelSpNum.Text = Convert.ToDouble((Weightslider1.Value / 100.00)).ToString();
            Weightslider2.Value = (100 - Weightslider1.Value) / 2;
            //Weightslider2.Maximum = 100 - Weightslider1.Value;
            this.labelCenterNum.Text = Convert.ToDouble((Weightslider2.Value / 100.00)).ToString();
            Weightslider3.Value = (100 - Weightslider1.Value) / 2;
            this.labelAreaNum.Text = Convert.ToDouble((Weightslider3.Value / 100.00)).ToString();
        }

        private void Weightslider2_ValueChanged(object sender, EventArgs e)
        {
          
            this.labelCenterNum.Text = Convert.ToDouble((Weightslider2.Value / 100.00)).ToString();
            this.Weightslider3.Value = 100 - Weightslider1.Value - Weightslider2.Value;
            this.labelAreaNum.Text = Convert.ToDouble(Weightslider3.Value / 100.00).ToString();
        }

        private void Weightslider4_ValueChanged(object sender, EventArgs e)
        {
            this.labelTotalNum.Text = Convert.ToDouble((Weightslider4.Value / 100.00)).ToString();
        }
        //加载匹配参数
        private void loadFeatSetting(string m_WorkspacePath, string m_MatchedFCName, IFeatureClass m_TUFeatCls)
        {
            if ((m_MatchedFCName != "") && (ClsDeclare.g_WorkspacePath != ""))
            {
                //判断选择字段是否对应
                for (int i = 0; i < dataGridViewX2.RowCount; i++)
                {
                    DataGridViewCheckBoxCell dgvCheckBoxCell1 = dataGridViewX2[0, i] as DataGridViewCheckBoxCell;
                    if (Convert.ToBoolean(dgvCheckBoxCell1.Value) == true)//判断是否进行属性匹配，若进行，必须选取对应字段
                    {
                        if (this.dataGridViewX2[2, i].Value == null)
                        {
                            MessageBoxEx.Show("所选字段没有完全对应，请重新检查！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                }

                IWorkspaceFactory pWorkspaceFactory = new FileGDBWorkspaceFactory();
                IWorkspace2 workspace = pWorkspaceFactory.OpenFromFile(m_WorkspacePath, 0) as IWorkspace2;
                IFeatureWorkspace featureWorkspace = workspace as IFeatureWorkspace;
                ITable table = null;
                IFields fields = null;

                if (m_TUFeatCls.ShapeType == esriGeometryType.esriGeometryPolygon)
                {
                    if (workspace.get_NameExists(esriDatasetType.esriDTTable, "MatchedPolygonFCSetting"))
                    {
                        table = featureWorkspace.OpenTable("MatchedPolygonFCSetting");
                    }
                    else
                    {
                        fields = CreateMatchedPolygonFCSettingFields(workspace);
                        UID uid = new UIDClass();
                        uid.Value = "esriGeoDatabase.Object";
                        IFieldChecker fieldChecker = new FieldCheckerClass();
                        IEnumFieldError enumFieldError = null;
                        IFields validatedFields = null;
                        fieldChecker.ValidateWorkspace = (IWorkspace)workspace;
                        fieldChecker.Validate(fields, out enumFieldError, out validatedFields);

                        table = featureWorkspace.CreateTable("MatchedPolygonFCSetting", validatedFields, uid, null, "");
                    }
                }
                else
                {
                    MessageBoxEx.Show("同尺度匹配不允许不同的几何类型！");
                    return;
                }
                IWorkspaceEdit pWorkspaceEdit = featureWorkspace as IWorkspaceEdit;
                pWorkspaceEdit.StartEditing(true);
                pWorkspaceEdit.StartEditOperation();

                Dictionary<string, int> pDic = new Dictionary<string, int>();

                string tempFieldsName = "";
                DataGridViewCheckBoxCell dgvCheckBoxCell = new DataGridViewCheckBoxCell();

                //查看是否MatchedFCName是否存在,当前为工作层
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

                if (this.tabControl1.Tabs[0].Visible)
                {
                    if (!pDic.ContainsKey(m_MatchedFCName))
                    {
                        IRow tempRow = table.CreateRow();
                        tempRow.set_Value(index, m_MatchedFCName);
                        tempRow.set_Value(tempRow.Fields.FindField("FCSettingID"), table.RowCount(null) - 1);

                       
                        tempRow.set_Value(tempRow.Fields.FindField("Center"), Convert.ToDouble(labelCenterNum.Text));//权重
                       
                        tempRow.set_Value(tempRow.Fields.FindField("Area"), Convert.ToDouble(labelAreaNum.Text));

                        tempRow.set_Value(tempRow.Fields.FindField("SP"), Convert.ToDouble(labelSpNum.Text));
                        tempRow.set_Value(tempRow.Fields.FindField("TotalNum"), Convert.ToDouble(labelTotalNum.Text));
                       
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
                           
                            //MessageBoxEx.Show("没有选择属性匹配对应字段！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            //return;
                        }
                        else
                        {
                           
                            tempRow.set_Value(tempRow.Fields.FindField("MatchedFields"), tempFieldsName.Trim());
                        }

                        IDataset dataset = m_TUFeatCls as IDataset;
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
                        IRow tRow = table.GetRow(pDic[m_MatchedFCName]);




                        tRow.set_Value(tRow.Fields.FindField("Center"), Convert.ToDouble(labelCenterNum.Text));//权重

                        tRow.set_Value(tRow.Fields.FindField("Area"), Convert.ToDouble(labelAreaNum.Text));

                        tRow.set_Value(tRow.Fields.FindField("SP"), Convert.ToDouble(labelSpNum.Text));
                        tRow.set_Value(tRow.Fields.FindField("TotalNum"), Convert.ToDouble(labelTotalNum.Text));
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
                           
                            MessageBoxEx.Show("没有选择属性匹配对应字段！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                           
                            tRow.set_Value(tRow.Fields.FindField("MatchedFields"), tempFieldsName.Trim());
                        }

                        IDataset dataset = m_TUFeatCls as IDataset;
                        if (ClsDeclare.g_SourceFeatClsPathDic.ContainsKey(dataset.Name))
                        {

                            tRow.set_Value(tRow.Fields.FindField("SourceFCName"), dataset.Name);
                            tRow.set_Value(tRow.Fields.FindField("SourcePath"), ClsDeclare.g_SourceFeatClsPathDic[dataset.Name]);
                            tRow.set_Value(tRow.Fields.FindField("WorkspacePath"), ClsDeclare.g_WorkspacePath);
                        }
                        tRow.Store();
                    }
                }
                else if (this.tabControl1.Tabs[1].Visible)
                {
                    if (!pDic.ContainsKey(m_MatchedFCName))
                    {
                        IRow tempRow = table.CreateRow();
                        tempRow.set_Value(index, m_MatchedFCName);
                        tempRow.set_Value(tempRow.Fields.FindField("FCSettingID"), table.RowCount(null) - 1);
                        tempRow.set_Value(tempRow.Fields.FindField("Top"), 1);

                        if (this.comboBoxExBuffer.Text != "")
                        {
                            tempRow.set_Value(tempRow.Fields.FindField("Buffer"), this.comboBoxExBuffer.Text);
                        }
                        else
                        {
                            MessageBox.Show("请选择缓冲区半径！");
                            return;
                        }
                        //修改属性！！！！！！！！！！！！
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
                            
                            MessageBoxEx.Show("没有选择属性匹配对应字段！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                           
                            tempRow.set_Value(tempRow.Fields.FindField("MatchedFields"), tempFieldsName.Trim());
                        }
                        IDataset dataset = m_TUFeatCls as IDataset;
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
                        IRow tRow = table.GetRow(pDic[m_MatchedFCName]);
                        tRow.set_Value(tRow.Fields.FindField("Top"), 1);
                        if (this.comboBoxExBuffer.Text != "")
                        {
                            tRow.set_Value(tRow.Fields.FindField("Buffer"), this.comboBoxExBuffer.Text);
                        }
                        else
                        {
                            MessageBoxEx.Show("请选择缓冲区半径！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        //修改属性！！！！！！！！！！！！
                        tRow.set_Value(tRow.Fields.FindField("Attribute"), 1);
                        for (int i = 0; i < dataGridViewX2.RowCount; i++)
                        {
                            dgvCheckBoxCell = dataGridViewX1[0, i] as DataGridViewCheckBoxCell;
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
                          
                            MessageBoxEx.Show("没有选择属性匹配对应字段！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                           
                            tRow.set_Value(tRow.Fields.FindField("MatchedFields"), tempFieldsName.Trim());
                        }

                        IDataset dataset = m_TUFeatCls as IDataset;
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
    }
}
