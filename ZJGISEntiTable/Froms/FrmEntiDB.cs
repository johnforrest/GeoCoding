using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using ZJGISCommon;
using ZJGISCommon.Classes;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using ZJGISEntiTable.Classes;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;
using DevComponents.DotNetBar;
using System.Collections.ObjectModel;

namespace ZJGISEntiTable.Froms
{
    public partial class FrmEntiDB : DevComponents.DotNetBar.Office2007Form
    {
        public FrmEntiDB()
        {
            InitializeComponent();
        }

        private void FrmEntiDB_Load(object sender, EventArgs e)
        {
            //隐藏进度条
            this.labelX1.Visible = false;
            this.progressBarXEntiDB.Visible = false;
        }
        /// <summary>
        /// 浏览实体表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenPath_Click(object sender, EventArgs e)
        {
            //FolderBrowserDialog fBD = new FolderBrowserDialog();
            //fBD.Description = "选择GDB路径";//控件上显示的说明文本
            //fBD.RootFolder = Environment.SpecialFolder.Desktop;//设置开始浏览的根文件夹
            //fBD.ShowNewFolderButton = true;//是否显示“新建文件夹”按钮
            //if (fBD.ShowDialog() == DialogResult.OK)
            //{
            //    txbGdbPath.Text = fBD.SelectedPath;
            //}

            string tempResultTablePath = null;
            ZJGISOpenData.Forms.FrmOpenData frmOpenData = new ZJGISOpenData.Forms.FrmOpenData();
            frmOpenData.IsShowTable = true;
            if (frmOpenData.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }
            Collection<object> tableCol = new Collection<object>();
            tableCol = frmOpenData.TableCollection;
            IDataset dataset = null;
            if (tableCol.Count > 1)
            {
                dataset = tableCol[0] as IDataset;
            }
            else
            {
                MessageBoxEx.Show("请加载数据源", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (dataset == null)
            {
                MessageBoxEx.Show("请加载匹配结果表！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            tempResultTablePath = frmOpenData.PathName + @"\" + dataset.Name;
            if (string.IsNullOrEmpty(tempResultTablePath))
            {
                return;
            }
            this.txbGdbPath.Text = tempResultTablePath;
            //this._entityTable = this.OpenEntityTable(dataset.Name, frmOpenData.PathName);


        }
        /// <summary>
        /// 数据库版本记录表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenVersionPath_Click(object sender, EventArgs e)
        {
            //OpenFileDialog oFD = new OpenFileDialog();
            //oFD.Title = "数据版本记录表路径";//对话框标题
            //oFD.Filter = "dbf文件(*.dbf)|*.dbf|所有文件(*.*)|*.*";//设置文件名筛选器
            //oFD.Multiselect = true;//是否可以多选文件
            //if (oFD.ShowDialog() == DialogResult.OK)
            //{
            //    txbVersionPath.Text = oFD.FileName;
            //}

            string tempResultTablePath = null;
            ZJGISOpenData.Forms.FrmOpenData frmOpenData = new ZJGISOpenData.Forms.FrmOpenData();
            frmOpenData.IsShowTable = true;
            if (frmOpenData.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }
            Collection<object> tableCol = new Collection<object>();
            tableCol = frmOpenData.TableCollection;
            IDataset dataset = null;
            if (tableCol.Count > 1)
            {
                dataset = tableCol[0] as IDataset;
            }
            else
            {
                MessageBoxEx.Show("请加载数据源", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (dataset == null)
            {
                MessageBoxEx.Show("请加载匹配结果表！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            tempResultTablePath = frmOpenData.PathName + @"\" + dataset.Name;
            if (string.IsNullOrEmpty(tempResultTablePath))
            {
                return;
            }
            this.txbVersionPath.Text = tempResultTablePath;
            //this._entityTable = this.OpenEntityTable(dataset.Name, frmOpenData.PathName);
        }

        /// <summary>
        /// 创建实体表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCreateEntiTable_Click(object sender, EventArgs e)
        {
            //显示进度条
            this.labelX1.Visible = true;
            this.progressBarXEntiDB.Visible = true;

            IWorkspaceFactory2 pWorkspaceFactory = new FileGDBWorkspaceFactoryClass() as IWorkspaceFactory2;
            //20180131
            string path = System.IO.Path.GetDirectoryName(this.txbGdbPath.Text);
            //string entitableName = this.txbEntiName.Text.ToString();
            string entitableName = System.IO.Path.GetFileName(this.txbGdbPath.Text);

            IWorkspace2 pWorkspace = pWorkspaceFactory.OpenFromFile(path, 0) as IWorkspace2;
            IFeatureWorkspace featureWorkspace = pWorkspace as IFeatureWorkspace;


            IMapControl4 pMainMapControl = ClsControl.MapControlMain;
            IMap pMap = pMainMapControl.Map;
            List<IFeatureLayer> pFeatlayerList = new List<IFeatureLayer>();

            //把dbf表转换成为ITable
            string dbfPath = System.IO.Path.GetDirectoryName(this.txbVersionPath.Text);
            string dbfName = System.IO.Path.GetFileName(this.txbVersionPath.Text);
            ITable versionTable = OpenDBFTable(dbfPath, dbfName);

            int pBarNumberTotal = 0;

            if (pMap != null)
            {
                ClsCommon pClsCommon = new ClsCommon();
                pFeatlayerList = pClsCommon.GetFeatureLayers(pMap);

                ITable entitable = null;

                //如果表存在，就删除数据
                if (pWorkspace.get_NameExists(esriDatasetType.esriDTTable, entitableName))
                {
                    entitable = featureWorkspace.OpenTable(entitableName);
                    //IWorkspaceEdit workspaceEdit = pWorkspace as IWorkspaceEdit;
                    //workspaceEdit.StartEditing(true);
                    //workspaceEdit.StartEditOperation();

                    //ICursor cursor = entitable.Search(null, false);
                    //IRow r = cursor.NextRow();
                    //while (r != null)
                    //{
                    //    r.Delete();
                    //    r = cursor.NextRow();
                    //}
                    //workspaceEdit.StopEditOperation();
                    //workspaceEdit.StopEditing(true);
                }
                //如果表不存在，就创建字段创建表
                else
                {
                    IFields fileds = null;
                    fileds = CreateEntiTableFields(pWorkspace);
                    entitable = CreateTable(pWorkspace, entitableName, fileds);
                }

                //进度条
                ClsBarSync pBarEntiDB = new ClsBarSync(progressBarXEntiDB);
                pBarEntiDB.SetStep(1);

                foreach (IFeatureLayer pFeatureLay in pFeatlayerList)
                {
                    IFeatureClass pFeatureCls = pFeatureLay.FeatureClass;
                    int pCount = pFeatureCls.FeatureCount(null);
                    pBarNumberTotal += pCount;
                    //pBarEntiDB.PerformOneStep();
                }
                pBarEntiDB.SetMax(pBarNumberTotal);
                foreach (IFeatureLayer pFeatureLay in pFeatlayerList)
                {
                    IFeatureClass pFeatureCls = pFeatureLay.FeatureClass;
                    string pFeatureLayname = pFeatureLay.Name;
                    FillEntiTable(pFeatureCls, entitable, versionTable, pFeatureLayname, pBarEntiDB);
                    //pBarEntiDB.PerformOneStep();
                }
            }

            //DialogResult dr = MessageBox.Show("内容？", "对话框标题", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            DialogResult dr = MessageBox.Show("实体表创建成功!", "实体表提示信息", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                //点确定的代码
                this.Close();
            }
            else
            {
                //点取消的代码   
            }
            //MessageBox.Show("实体表创建成功！");
        }


        /// <summary>
        /// 生成实体表字段
        /// </summary>
        /// <param name="workspace">保存路径</param>
        /// <returns></returns>
        public IFields CreateEntiTableFields(IWorkspace2 workspace)
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

            IField field0 = new FieldClass();
            IFieldEdit fieldEdit0 = field0 as IFieldEdit;
            fieldEdit0.Name_2 = "ENTIID";
            fieldEdit0.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit0.IsNullable_2 = false;
            fieldEdit0.AliasName_2 = "地理实体标识码";
            fieldEdit0.Editable_2 = true;
            fieldsEdit.AddField(field0);

            IField field1 = new FieldClass();
            IFieldEdit fieldEdit1 = field1 as IFieldEdit;
            fieldEdit1.Name_2 = "FNAME";
            fieldEdit1.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit1.IsNullable_2 = true;
            fieldEdit1.AliasName_2 = "实体中文描述";
            fieldEdit1.Editable_2 = true;
            fieldsEdit.AddField(field1);

            IField field2 = new FieldClass();
            IFieldEdit fieldEdit2 = field2 as IFieldEdit;
            fieldEdit2.Name_2 = "STARTDATE";
            fieldEdit2.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit2.IsNullable_2 = true;
            fieldEdit2.AliasName_2 = "起始时间";
            fieldEdit2.Editable_2 = true;
            fieldsEdit.AddField(field2);

            IField field3 = new FieldClass();
            IFieldEdit fieldEdit3 = field3 as IFieldEdit;
            fieldEdit3.Name_2 = "ENDDATE";
            fieldEdit3.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit3.IsNullable_2 = true;
            fieldEdit3.AliasName_2 = "消亡时间";
            fieldEdit3.Editable_2 = true;
            fieldsEdit.AddField(field3);

            IField field4 = new FieldClass();
            IFieldEdit fieldEdit4 = field4 as IFieldEdit;
            fieldEdit4.Name_2 = "UPDATEDATE";
            fieldEdit4.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit4.IsNullable_2 = true;
            fieldEdit4.AliasName_2 = "更新时间";
            fieldEdit4.Editable_2 = true;
            fieldsEdit.AddField(field4);



            IField field5 = new FieldClass();
            IFieldEdit fieldEdit5 = field5 as IFieldEdit;
            fieldEdit5.Name_2 = "USOURCE";
            fieldEdit5.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit5.IsNullable_2 = true;
            fieldEdit5.AliasName_2 = "更新数据源";
            fieldEdit5.Editable_2 = true;
            fieldsEdit.AddField(field5);


            IField field6 = new FieldClass();
            IFieldEdit fieldEdit6 = field6 as IFieldEdit;
            fieldEdit6.Name_2 = "ESOURCE";
            fieldEdit6.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit6.IsNullable_2 = true;
            fieldEdit6.AliasName_2 = "实体所在数据源";
            fieldEdit6.Editable_2 = true;
            fieldsEdit.AddField(field6);


            fields = fieldsEdit as IFields;
            return fields;
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
        /// 遍历填充实体表
        /// </summary>
        /// <param name="pFeatureCls">图层要素类</param>
        /// <param name="resultTable">实体表</param>
        /// <param name="versionTable">实体与版本对照表</param>
        /// <param name="name">featlayer名称</param>
        public void FillEntiTable(IFeatureClass pFeatureCls, ITable entiTable, ITable versionTable, string pFeatureLayname, ClsBarSync pBarEntiDB)
        {

            string nameField = "";
            if (ClsConfig.LayerConfigs[pFeatureLayname].NameField.ToString()!="null"&&ClsConfig.LayerConfigs[pFeatureLayname].NameField.ToString().Length>0)
            {
                nameField=ClsConfig.LayerConfigs[pFeatureLayname].NameField;
            }
            string entityField = ClsConfig.LayerConfigs[pFeatureLayname].EntityID;


            IFeatureCursor pCursor = null;
            IFeature pFeature = null;
            int iFeatCount = 0;
            bool bFlagEnti = false;
            bool bFlagName = false;

            ////实体表进入编辑状态
            IDataset dataset = entiTable as IDataset;
            IWorkspace workspace = dataset.Workspace;
            IWorkspaceEdit workspaceEdit = workspace as IWorkspaceEdit;
            workspaceEdit.StartEditing(true);
            workspaceEdit.StartEditOperation();

            pCursor = pFeatureCls.Search(null, false);
            if (pCursor != null)
            {
                pFeature = pCursor.NextFeature();
            }

            IRowBuffer rowBuffer = null;
            IRow row = null;
            ICursor rowCursor = null;

            //ClsBarSync pBarEntiDB = new ClsBarSync(progressBarXEntiDB);
            //pBarEntiDB.SetStep(1);
            //pBarEntiDB.SetMax(pFeatureCls.FeatureCount(null));

            while (pFeature != null)
            {
                IField pField = null;
                IFields pFields = pFeatureCls.Fields;
                for (int i = 0; i < pFields.FieldCount - 1; i++)
                {
                    pField = pFields.get_Field(i);
                    if (pField.Name.Trim() == entityField)
                    {
                        bFlagEnti = true;
                    }
                    if (pField.Name.Trim() == nameField)
                    {
                        bFlagName = true;
                    }
                }
                //存在ENTIID字段并且含有地理编码
                if (bFlagEnti && pFeature.get_Value(pFeature.Fields.FindField(entityField)).ToString().Length > 0)
                {
                    //只查询ENTIID这一个字段(查询一个字段)
                    IQueryFilter queryFilter = new QueryFilterClass();
                    queryFilter.WhereClause = entityField + " = '" + pFeature.get_Value(pFeature.Fields.FindField(entityField)).ToString() + "'";
                    //把实体表的遍历放入到Feature的遍历过程中
                    rowCursor = entiTable.Search(queryFilter, false);
                    row = rowCursor.NextRow();
                    //如果实体表的某一行为空，插入数据
                    if (row == null)
                    {
                        rowBuffer = entiTable.CreateRowBuffer();
                        rowCursor = entiTable.Insert(true);
                        rowBuffer = InsertEntiData(rowBuffer, pFeature, bFlagName, versionTable, pFeatureLayname);
                        rowCursor.InsertRow(rowBuffer);
                    }
                    //如果实体表的某一行含有数据，则更新数据
                    else
                    {
                        row = UpdateEntiData(row, pFeature, bFlagName, versionTable, pFeatureLayname);
                        row.Store();
                    }
                    iFeatCount++;
                }

                //每1千个要素就把缓冲区内的要素类写入目标层
                if (iFeatCount >= 500)
                {
                    rowCursor.Flush();
                    iFeatCount = 0;
                }
                pBarEntiDB.PerformOneStep();

                pFeature = pCursor.NextFeature();
            }

            if (rowCursor != null)
            {
                rowCursor.Flush();
            }
            workspaceEdit.StopEditOperation();
            workspaceEdit.StopEditing(true);


        }
        /// <summary>
        /// 向实体表中插入数据
        /// </summary>
        /// <param name="rowBuffer">实体表的一行</param>
        /// <param name="pFeature">待插入ENTIID的要素</param>
        /// <param name="bFlagName">待插入要素的描素信息（名称）</param>
        /// <param name="versionTable">版本与历史对照表</param>
        /// <param name="name">要素所在featlayer的名称</param>
        /// <returns></returns>
        public IRowBuffer InsertEntiData(IRowBuffer rowBuffer, IFeature pFeature, bool bFlagName, ITable versionTable, string pFeatureLayname)
        {
            //try
            //{
            string nameField = ClsConfig.LayerConfigs[pFeatureLayname].NameField;
            string entityField = ClsConfig.LayerConfigs[pFeatureLayname].EntityID;
            string fromField = ClsConfig.LayerConfigs[pFeatureLayname].StartVersion;
            string sourceName = ClsConfig.LayerConfigs[pFeatureLayname].SourceName;
            string sourceType = ClsConfig.LayerConfigs[pFeatureLayname].SourceType;

            //如果地理实体有名称字段，并且含有名称
            if (bFlagName && pFeature.get_Value(pFeature.Fields.FindField(nameField)).ToString().Length > 0)
            {
                rowBuffer.set_Value(rowBuffer.Fields.FindField("FNAME"), pFeature.get_Value(pFeature.Fields.FindField(nameField)).ToString());
            }

            rowBuffer.set_Value(rowBuffer.Fields.FindField("ENTIID"), pFeature.get_Value(pFeature.Fields.FindField(entityField)).ToString());

            //含有其实版本
            if (pFeature.get_Value(pFeature.Fields.FindField(fromField)).ToString().Trim().Length > 0)
            {
                DateTime collectTime = GetDateTime(pFeature.get_Value(pFeature.Fields.FindField(fromField)).ToString(), sourceName, versionTable);

                rowBuffer.set_Value(rowBuffer.Fields.FindField("STARTDATE"), collectTime.ToShortDateString());
                rowBuffer.set_Value(rowBuffer.Fields.FindField("UPDATEDATE"), collectTime.ToShortDateString());
            }
            rowBuffer.set_Value(rowBuffer.Fields.FindField("USOURCE"), sourceName);
            rowBuffer.set_Value(rowBuffer.Fields.FindField("ESOURCE"), sourceType);

            //}
            //catch (System.Exception ex)
            //{
            //    //MessageBox.Show(ex.Message);
            //    MessageBox.Show(ex.StackTrace);
            //}
            return rowBuffer;

        }
        /// <summary>
        /// 更新实体表
        /// </summary>
        /// <param name="rowBuffer">实体表的一行</param>
        /// <param name="pFeature">待插入ENTIID的要素</param>
        /// <param name="bFlagName">待插入要素的描素信息（名称）</param>
        /// <param name="versionTable">版本与历史对照表</param>
        /// <param name="name">要素所在featlayer的名称</param>
        /// <returns></returns>
        public IRow UpdateEntiData(IRow row, IFeature pFeature, bool bFlagName, ITable versionTable, string pFeatureLayname)
        {
            string nameField = ClsConfig.LayerConfigs[pFeatureLayname].NameField;
            string entityField = ClsConfig.LayerConfigs[pFeatureLayname].EntityID;
            string fromField = ClsConfig.LayerConfigs[pFeatureLayname].StartVersion;
            string sourceName = ClsConfig.LayerConfigs[pFeatureLayname].SourceName;
            string sourceType = ClsConfig.LayerConfigs[pFeatureLayname].SourceType;

            if (pFeature.get_Value(pFeature.Fields.FindField(fromField)).ToString().Trim().Length > 0)
            {
                DateTime collectTime = GetDateTime(pFeature.get_Value(pFeature.Fields.FindField(fromField)).ToString(), sourceName, versionTable);
                //DateTime startDateTime = DateTime.Parse(row.get_Value(row.Fields.FindField("STARTDATE")).ToString());
                DateTime startDateTime;
                DateTime updateDateTime;

                if (DateTime.TryParse(row.get_Value(row.Fields.FindField("STARTDATE")).ToString(), out startDateTime))
                {
                    if (collectTime < startDateTime)
                    {
                        row.set_Value(row.Fields.FindField("STARTDATE"), collectTime.ToShortDateString());
                    }
                }
                //if (collectTime < DateTime.Parse(row.get_Value(row.Fields.FindField("STARTDATE")).ToString()))
                //if (collectTime < startDateTime&&startDateTime!=null)
                //{
                //    row.set_Value(row.Fields.FindField("STARTDATE"), collectTime.ToShortDateString());
                //}

                if (DateTime.TryParse(row.get_Value(row.Fields.FindField("UPDATEDATE")).ToString(), out updateDateTime))
                {
                    //if (collectTime > DateTime.Parse(row.get_Value(row.Fields.FindField("UPDATEDATE")).ToString()))
                    if (collectTime > updateDateTime)
                    {
                        row.set_Value(row.Fields.FindField("UPDATEDATE"), collectTime.ToShortDateString());

                        //如果地理实体有名称字段，并且含有名称
                        if (bFlagName && pFeature.get_Value(pFeature.Fields.FindField(nameField)).ToString().Length > 0)
                        {
                            row.set_Value(row.Fields.FindField("FNAME"), pFeature.get_Value(pFeature.Fields.FindField(nameField)).ToString());
                        }

                        row.set_Value(row.Fields.FindField("USOURCE"), sourceName);
                    }
                }
            }

            string eSource = row.get_Value(row.Fields.FindField("ESOURCE")).ToString();
            if (!eSource.Contains(sourceType))
            {
                eSource += sourceType;
                row.set_Value(row.Fields.FindField("ESOURCE"), eSource);
            }

            return row;
        }

        /// <summary>
        /// 获取起始版本号对应的时间
        /// </summary>
        /// <param name="versionID">要素中起始版本号（数字）</param>
        /// <param name="type">时间与版本对照表中的所记录的类型</param>
        /// <param name="versionTable">实体表</param>
        /// <returns></returns>
        public DateTime GetDateTime(string versionID, string type, ITable versionTable)
        {
            DateTime result = new DateTime();
            //通过设置查询两个字段为过滤条件
            IQueryFilter queryFilter = new QueryFilterClass();
            queryFilter.WhereClause = "VERSION_ID = " + versionID + " and USOURSE = '" + type + "'";
            ICursor rowCursor = versionTable.Search(queryFilter, false);
            IRow row = rowCursor.NextRow();
            if (row != null)
            {
                result = (DateTime)row.get_Value(row.Fields.FindField("VERSION_TI"));
            }
            return result;
        }

        /// <summary>
        /// 打开dbf表
        /// </summary>
        /// <param name="pathName"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public ITable OpenDBFTable(string pathName, string tableName)
        {
            // Create the workspace name object.
            IWorkspaceName workspaceName = new WorkspaceNameClass();
            workspaceName.PathName = pathName;
            if (pathName.Contains(".gdb"))
            {
                workspaceName.WorkspaceFactoryProgID = "esriDataSourcesGDB.FileGDBWorkspaceFactory";
            }
            else
            {
                workspaceName.WorkspaceFactoryProgID = "esriDataSourcesFile.shapefileworkspacefactory";
            }
            // Create the table name object.
            IDatasetName dataSetName = new TableNameClass();
            dataSetName.Name = tableName;
            dataSetName.WorkspaceName = workspaceName;
            // Open the table.
            IName name = (IName)dataSetName;
            ITable table = (ITable)name.Open();
            return table;
        }

    }
}