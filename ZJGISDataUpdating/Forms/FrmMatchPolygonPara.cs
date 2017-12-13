using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;
using Microsoft.VisualBasic;
using System.Collections.ObjectModel;
using DevComponents.DotNetBar.Controls;
using DevComponents.AdvTree;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
using DevComponents.DotNetBar;
namespace ZJGISDataUpdating
{
    public partial class FrmMatchPolygonPara : DevComponents.DotNetBar.Office2007Form
    {
        DataGridViewCell m_dgvCell;

        public FrmMatchPolygonPara()
        {
            InitializeComponent();
        }
        IFeatureClass m_TUFeatCls = null;
        IFeatureClass m_TEFeatCls = null;
        string m_MatchedFCName = "";
        string m_WorkspacePath = "";


        public DataGridViewCell DGVCell
        {
            set
            {
                m_dgvCell = value;
            }
        }
        public string WorkspacePath
        {
            set
            {
                m_WorkspacePath = value;
            }
        }

        public string MatchedFCName
        {
            set
            {
                m_MatchedFCName = value;
            }
        }

        public IFeatureClass TUFeatCls
        {
            get
            {
                return m_TUFeatCls;
            }
            set
            {
                m_TUFeatCls = value;
            }
        }
        public IFeatureClass TEFeatCls
        {
            get
            {
                return m_TEFeatCls;
            }
            set
            {
                m_TEFeatCls = value;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void FrmMatchParameters_Load(object sender, EventArgs e)
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

           // this.radioButtonCenter.Enabled = false;
            this.radioButtonArea.Enabled = false;

            this.labelAreaNum.Enabled = false;
            this.Weightslider2.Enabled = false;
            this.labelCenterNum.Enabled = false;
            this.Weightslider3.Enabled = false;
            this.labelTotalNum.Enabled = true;
            this.Weightslider4.Enabled = true;


            //this.txtAreaNum.Enabled = false;
            //this.txtCenterNum.Enabled = false;
            //this.labelCenterNum.Enabled = false;
            //this.labelTotalNum.Enabled = false;

            DataGridViewCheckBoxColumn dgvCheckColumn = new DataGridViewCheckBoxColumn();
            dgvCheckColumn.Name = "CheckBoxColumn";
            dgvCheckColumn.HeaderText = "";
            dgvCheckColumn.ReadOnly = false;
            dgvCheckColumn.Width = dataGridViewX1.Width / 8;
            dataGridViewX1.Columns.Add(dgvCheckColumn);

            dataGridViewX1.Columns.Add("SourceLayer", "源图层字段");
            dataGridViewX1.Columns[1].Width = dataGridViewX1.Width / 3;
            dataGridViewX1.Columns[1].ReadOnly = true;

            DataGridViewComboBoxColumn dgvcbColumn = new DataGridViewComboBoxColumn();
            dgvcbColumn.Name = "TargetLayer";
            dgvcbColumn.HeaderText = "更新图层字段";
            dgvcbColumn.Width = dataGridViewX1.Width / 3;
            dataGridViewX1.Columns.Add(dgvcbColumn);

            LoadFields(m_TUFeatCls,m_TEFeatCls);
        }

        private void LoadFields(IFeatureClass sourceFeatCls, IFeatureClass targetFeatCls)
        {
            IFields2 sourceFields = sourceFeatCls.Fields as IFields2;
            IFields2 targetFields = targetFeatCls.Fields as IFields2;
            DataGridViewRow dgvRow;

            dataGridViewX1.Rows.Clear();

            for (int i = 0; i < sourceFields.FieldCount; i++)
            {
                if (sourceFields.get_Field(i).Type == esriFieldType.esriFieldTypeGeometry || sourceFields.get_Field(i).Editable==false)
                    continue;

                dgvRow = new DataGridViewRow();
                dgvRow = dataGridViewX1.Rows[dataGridViewX1.Rows.Add()];
                dgvRow.Cells[1].Value = sourceFields.get_Field(i).AliasName;

                DataGridViewCheckBoxCell dgvCheckBoxCell = new DataGridViewCheckBoxCell();
                dgvCheckBoxCell = dgvRow.Cells[0] as DataGridViewCheckBoxCell;

                DataGridViewComboBoxCell dgvComCell = new DataGridViewComboBoxCell();
                dgvComCell = dgvRow.Cells[2] as DataGridViewComboBoxCell;

                for (int j = 0; j < targetFields.FieldCount; j++)
                {
                    if (targetFields.get_Field(j).Type == esriFieldType.esriFieldTypeGeometry || targetFields.get_Field(j).Editable==false)
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
        private void btnOK_Click(object sender, EventArgs e)
        {
            if ((m_MatchedFCName != "") && (m_WorkspacePath != ""))
            {
                //判断选择字段是否对应
                for (int i = 0; i < dataGridViewX1.RowCount; i++)
                {
                    DataGridViewCheckBoxCell dgvCheckBoxCell1 = dataGridViewX1[0, i] as DataGridViewCheckBoxCell;
                    if (Convert.ToBoolean(dgvCheckBoxCell1.Value) == true)//判断是否进行属性匹配，若进行，必须选取对应字段
                    {
                        if (this.dataGridViewX1[2, i].Value==null)
                        {
                            MessageBoxEx.Show("所选字段没有完全对应，请重新检查！","提示",MessageBoxButtons.OK,MessageBoxIcon.Error);
                            return;
                        }
                    }

                }

                IWorkspaceFactory pWorkspaceFactory = new FileGDBWorkspaceFactory();
                IWorkspace2 workspace = pWorkspaceFactory.OpenFromFile(m_WorkspacePath,0) as IWorkspace2;
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

                IWorkspaceEdit pWorkspaceEdit = featureWorkspace as IWorkspaceEdit;
                pWorkspaceEdit.StartEditing(true);
                pWorkspaceEdit.StartEditOperation();

                //Collection<string> pCol = new Collection<string>();
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

                        if (this.radioButtonCenter.Checked == true)
                        {
                            tempRow.set_Value(tempRow.Fields.FindField("Center"), 1);//权重为一
                            tempRow.set_Value(tempRow.Fields.FindField("CenterNum"), this.txtCenterNum.Text);
                            //tempRow.set_Value(tempRow.Fields.FindField("CenterWT"), this.txtCenterWT.Text);
                        }
                        else
                        {
                            tempRow.set_Value(tempRow.Fields.FindField("Center"), 0);
                        }

                        if (this.radioButtonArea.Checked == true)
                        {
                            tempRow.set_Value(tempRow.Fields.FindField("Area"), 1);
                            tempRow.set_Value(tempRow.Fields.FindField("AreaNum"), this.txtAreaNum.Text);
                            //tempRow.set_Value(tempRow.Fields.FindField("AreaWT"), this.txtAreaWT.Text);
                        }
                        else
                        {
                            tempRow.set_Value(tempRow.Fields.FindField("Area"), 0);
                        }

                        if (this.radioButtonShape.Checked == true)
                        {
                            tempRow.set_Value(tempRow.Fields.FindField("SP"), 1);
                            tempRow.set_Value(tempRow.Fields.FindField("SPNum"), this.txtSPNum.Text);
                            //tempRow.set_Value(tempRow.Fields.FindField("SPWT"), this.txtSPWT.Text);
                        }
                        else
                        {
                            tempRow.set_Value(tempRow.Fields.FindField("SP"), 0);
                        }
                        //修改属性！！！！！！！！！！！！！！！！！！！！！
                        tempRow.set_Value(tempRow.Fields.FindField("Attribute"), 1);
                        for (int i = 0; i < dataGridViewX1.RowCount; i++)
                        {
                            dgvCheckBoxCell = dataGridViewX1[0, i] as DataGridViewCheckBoxCell;
                            if (Convert.ToBoolean(dgvCheckBoxCell.Value) == true)
                            {
                                if (this.dataGridViewX1[1, i].Value.ToString().ToUpper() == this.dataGridViewX1[2, i].Value.ToString().ToUpper())
                                {
                                    tempFieldsName = tempFieldsName + dataGridViewX1[1, i].Value.ToString() + ";";
                                }
                                else
                                {
                                    tempFieldsName = tempFieldsName + dataGridViewX1[1, i].Value.ToString() + "(" + dataGridViewX1[2, i].Value.ToString() + ")";
                                }
                            }

                        }

                        if (tempFieldsName == "")
                        {
                            //tRow.set_Value(tRow.Fields.FindField("Attribute"), 0);
                            MessageBoxEx.Show("没有选择属性匹配对应字段！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            //tRow.set_Value(tRow.Fields.FindField("Attribute"), 1);
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
                        if (this.radioButtonCenter.Checked == true)
                        {
                            tRow.set_Value(tRow.Fields.FindField("Center"), 1);
                            tRow.set_Value(tRow.Fields.FindField("CenterNum"), this.txtCenterNum.Text);
                            //tRow.set_Value(tRow.Fields.FindField("CenterWT"), this.txtCenterWT.Text);
                        }
                        else
                        {
                            tRow.set_Value(tRow.Fields.FindField("Center"), 0);
                            tRow.set_Value(tRow.Fields.FindField("CenterNum"),0);
                        }

                        if (this.radioButtonArea.Checked == true)
                        {
                            tRow.set_Value(tRow.Fields.FindField("Area"), 1);
                            tRow.set_Value(tRow.Fields.FindField("AreaNum"), this.txtAreaNum.Text);
                            //tRow.set_Value(tRow.Fields.FindField("AreaWT"), this.txtAreaWT.Text);
                        }
                        else
                        {
                            tRow.set_Value(tRow.Fields.FindField("Area"), 0);
                            tRow.set_Value(tRow.Fields.FindField("AreaNum"),0);
                        }

                        if (this.radioButtonShape.Checked == true)
                        {
                            tRow.set_Value(tRow.Fields.FindField("SP"), 1);
                            tRow.set_Value(tRow.Fields.FindField("SPNum"), this.txtSPNum.Text);
                            //tRow.set_Value(tRow.Fields.FindField("SPWT"), this.txtSPWT.Text);
                        }
                        else
                        {
                            tRow.set_Value(tRow.Fields.FindField("SP"), 0);
                            tRow.set_Value(tRow.Fields.FindField("SPNum"),0);
                        }
                        //修改属性！！！！！！！！！！！！
                        tRow.set_Value(tRow.Fields.FindField("Attribute"), 1);
                        for (int i = 0; i < dataGridViewX1.RowCount; i++)
                        {
                            dgvCheckBoxCell = dataGridViewX1[0, i] as DataGridViewCheckBoxCell;
                            if (Convert.ToBoolean(dgvCheckBoxCell.Value) == true)
                            {
                                if (this.dataGridViewX1[1, i].Value.ToString().ToUpper() == this.dataGridViewX1[2, i].Value.ToString().ToUpper())
                                {
                                    tempFieldsName = tempFieldsName + dataGridViewX1[1, i].Value.ToString() + ";";
                                }
                                else
                                {
                                    tempFieldsName = tempFieldsName + dataGridViewX1[1, i].Value.ToString() + "(" + dataGridViewX1[2, i].Value.ToString() + ")";
                                }
                            }

                        }

                        if (tempFieldsName == "")
                        {
                            //tRow.set_Value(tRow.Fields.FindField("Attribute"), 0);
                            MessageBoxEx.Show("没有选择属性匹配对应字段！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            //tRow.set_Value(tRow.Fields.FindField("Attribute"), 1);
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
                        tempRow.set_Value(tempRow.Fields.FindField("Top"),1);

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
                        for (int i = 0; i < dataGridViewX1.RowCount; i++)
                        {
                            dgvCheckBoxCell = dataGridViewX1[0, i] as DataGridViewCheckBoxCell;
                            if (Convert.ToBoolean(dgvCheckBoxCell.Value) == true)
                            {
                                if (this.dataGridViewX1[1, i].Value.ToString().ToUpper() == this.dataGridViewX1[2, i].Value.ToString().ToUpper())
                                {
                                    tempFieldsName = tempFieldsName + dataGridViewX1[1, i].Value.ToString() + ";";
                                }
                                else
                                {
                                    tempFieldsName = tempFieldsName + dataGridViewX1[1, i].Value.ToString() + "(" + dataGridViewX1[2, i].Value.ToString() + ")";
                                }
                            }

                        }

                        if (tempFieldsName == "")
                        {
                            //tRow.set_Value(tRow.Fields.FindField("Attribute"), 0);
                            MessageBoxEx.Show("没有选择属性匹配对应字段！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            //tRow.set_Value(tRow.Fields.FindField("Attribute"), 1);
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
                        tRow.set_Value(tRow.Fields.FindField("Top"),1);
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
                        for (int i = 0; i < dataGridViewX1.RowCount; i++)
                        {
                            dgvCheckBoxCell = dataGridViewX1[0, i] as DataGridViewCheckBoxCell;
                            if (Convert.ToBoolean(dgvCheckBoxCell.Value) == true)
                            {
                                if (this.dataGridViewX1[1, i].Value.ToString().ToUpper() == this.dataGridViewX1[2, i].Value.ToString().ToUpper())
                                {
                                    tempFieldsName = tempFieldsName + dataGridViewX1[1, i].Value.ToString() + ";";
                                }
                                else
                                {
                                    tempFieldsName = tempFieldsName + dataGridViewX1[1, i].Value.ToString() + "(" + dataGridViewX1[2, i].Value.ToString() + ")";
                                }
                            }

                        }

                        if (tempFieldsName == "")
                        {
                            //tRow.set_Value(tRow.Fields.FindField("Attribute"), 0);
                            MessageBoxEx.Show("没有选择属性匹配对应字段！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            //tRow.set_Value(tRow.Fields.FindField("Attribute"), 1);
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
            m_dgvCell.Value= "已设置";
            this.Close();
            return;
        }
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
            fieldEdit3.Type_2 =esriFieldType.esriFieldTypeInteger;
            fieldEdit3.AliasName_2 = "重心点匹配";
            fieldEdit3.IsNullable_2 =true;
            fieldsEdit.AddField(field3);

            IField field4=new FieldClass();
            IFieldEdit fieldEdit4 = field4 as IFieldEdit;
            fieldEdit4.Name_2 = "CenterNum";
            fieldEdit4.Type_2 = esriFieldType.esriFieldTypeDouble;
            fieldEdit4.AliasName_2 = "重心点阈值";
            fieldEdit4.IsNullable_2 = true;
            fieldsEdit.AddField(field4);

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
                    //this.tabControl1.Tabs[2].Visible = false;
                }
                else if(this.comboBoxExMatchType.Text=="拓扑匹配")
                {
                    if (!this.tabControl1.Tabs[1].Visible)
                    {
                        this.tabControl1.Tabs[1].Visible = true;
                    }
                    this.tabControl1.Tabs[0].Visible = false;
                    //this.tabControl1.Tabs[2].Visible = false;
                }
                //else if (this.comboBoxExMatchType.Text == "属性匹配")
                //{
                //    if (!this.tabControl1.Tabs[2].Visible)
                //    {
                //        this.tabControl1.Tabs[2].Visible = true;
                //    }
                //    this.tabControl1.Tabs[0].Visible = false;
                //    this.tabControl1.Tabs[1].Visible = false;
                //}
            }
        }

        private void radioButtonShape_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButtonShape.Checked)
            {
                //this.txtSPNum.Enabled = true;
                this.labelSpNum.Enabled = true;

                this.Weightslider1.Enabled = true;
                //this.labelCenterNum.Enabled = true;
            }
            else
            {
                //this.txtSPNum.Enabled = false;
                this.labelAreaNum.Enabled = false;

                this.Weightslider1.Enabled = false;

                
               // this.labelCenterNum.Enabled = false;
            }
        }

        private void radioButtonCenter_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButtonCenter.Checked)
            {
               // this.txtCenterNum.Enabled = true;
                this.Weightslider2.Enabled = true;
                this.labelCenterNum.Enabled = true;
            }
            else
            {
                //this.txtCenterNum.Enabled = false;
                this.Weightslider2.Enabled = false;
                this.labelCenterNum.Enabled = false;
            }
        }

        private void radioButtonArea_CheckedChanged(object sender, EventArgs e)
        {
            //if (this.radioButtonArea.Checked)
            //{
            //    //this.txtAreaNum.Enabled = true;
            //    this.Weightslider3.Enabled = false;
            //    this.labelAreaNum.Enabled = true;
            //}
            //else
            //{
            //    //this.txtAreaNum.Enabled = false;
            //    this.Weightslider3.Enabled = false;
            //    this.labelAreaNum.Enabled = false;
            //}
        }

        private void Weightslider_ValueChanged(object sender, EventArgs e)
        {
            this.labelSpNum.Text = Convert.ToDouble((Weightslider1.Value / 100.00)).ToString();
            Weightslider2.Value = (100- Weightslider1.Value) / 2;
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

        private void Weightslider3_ValueChanged(object sender, EventArgs e)
        {
            //this.labelAreaNum.Text = Convert.ToDouble((100-Weightslider1.Value-Weightslider2.Value)/100.00).ToString();
        }

        private void Weightslider4_ValueChanged(object sender, EventArgs e)
        {
            this.labelTotalNum.Text = Convert.ToDouble((Weightslider4.Value / 100.00)).ToString();
        } 
    }
}
