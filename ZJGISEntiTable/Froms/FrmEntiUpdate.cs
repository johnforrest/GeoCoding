using DevComponents.DotNetBar;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZJGISCommon;
using ZJGISCommon.Classes;
using ZJGISEntiTable.Classes;

namespace ZJGISEntiTable.Froms
{
    public partial class FrmEntiUpdate : Office2007Form
    {
        private IMapControl4 _mapControl = null;
        private List<IFeatureLayer> _pFeatlayerList = null;
        private ITable _versionTable = null;
        private ITable _entityTable = null;
        private FrmUpdateResult _frm = new FrmUpdateResult();
        public FrmEntiUpdate()
        {
            InitializeComponent();
        }
        /// <summary>
        /// load事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmEntiUpdate_Load(object sender, EventArgs e)
        {
            this.progressBarXEntiUpdate.Visible = false;
            this.labelX1.Visible = false;


            this._mapControl = ClsControl.MapControlMain;
            IMap pMap = this._mapControl.Map;
            if (pMap == null) { return; }


            this._pFeatlayerList = new List<IFeatureLayer>();
            ClsCommon pClsCommon = new ClsCommon();
            this._pFeatlayerList = pClsCommon.GetFeatureLayers(pMap);
            //初始化Combox
            foreach (IFeatureLayer pFeatureLay in this._pFeatlayerList)
            {
                this.toUpdateBox.Items.Add(pFeatureLay.Name);
                this.updatedBox.Items.Add(pFeatureLay.Name);
            }
        }
        /// <summary>
        /// 驱动更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void update_Click(object sender, EventArgs e)
        {
            this.progressBarXEntiUpdate.Visible = true;
            this.labelX1.Visible = true;


            IFeatureLayer toUpdateLyr = this.GetFeatureLayerByName(this._pFeatlayerList, this.toUpdateBox.SelectedItem.ToString());
            IFeatureLayer updatedLyr = this.GetFeatureLayerByName(this._pFeatlayerList, this.updatedBox.SelectedItem.ToString());

            List<ClsUpdateInfo> updateInfos = this.GetUpdateInfo(toUpdateLyr, updatedLyr);
            Dictionary<string, ClsUpdateInfo> processedUpdateInfos = this.ProcessUpdateInfo(updateInfos);
            if (this._frm.IsDisposed)
            {
                this._frm = new FrmUpdateResult();
            }
            this._frm.LoadData(processedUpdateInfos, this._entityTable, this._versionTable);

           

            this._frm.Show();


        }
        /// <summary>
        /// 打开实体表路径
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EntityTbPathBtn_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fBD = new FolderBrowserDialog();
            fBD.Description = "选择GDB路径";//控件上显示的说明文本
            fBD.RootFolder = Environment.SpecialFolder.Desktop;//设置开始浏览的根文件夹
            fBD.ShowNewFolderButton = true;//是否显示“新建文件夹”按钮
            if (fBD.ShowDialog() == DialogResult.OK)
            {
                this.EntityTbPath.Text = fBD.SelectedPath;
                this._entityTable = this.OpenEntityTable(this.EntityTbName.Text, fBD.SelectedPath);
            }
        }
        /// <summary>
        /// 打开版本记录表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VersionTbPathBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog oFD = new OpenFileDialog();
            oFD.Title = "数据版本记录表路径";//对话框标题
            oFD.Filter = "dbf文件(*.dbf)|*.dbf|所有文件(*.*)|*.*";//设置文件名筛选器
            oFD.Multiselect = true;//是否可以多选文件
            if (oFD.ShowDialog() == DialogResult.OK)
            {
                this.VersionTbPath.Text = oFD.FileName;
                this._versionTable = this.OpenVersionTable(oFD.FileName);
            }
        }
        /// <summary>
        /// 打开实体表
        /// </summary>
        /// <param name="name"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        private ITable OpenEntityTable(string name, string path)
        {
            ITable result = null;
            IWorkspaceFactory2 pWorkspaceFactory = new FileGDBWorkspaceFactoryClass() as IWorkspaceFactory2;
            IWorkspace2 pWorkspace = pWorkspaceFactory.OpenFromFile(path, 0) as IWorkspace2;
            IFeatureWorkspace featureWorkspace = pWorkspace as IFeatureWorkspace;
            if (pWorkspace.get_NameExists(esriDatasetType.esriDTTable, name))
            {
                result = featureWorkspace.OpenTable(name);
            }
            return result;
        }

        private ITable OpenVersionTable(string pathName)
        {
            ITable result = null;
            //把dbf表转换成为ITable
            string dbfPath = System.IO.Path.GetDirectoryName(pathName);
            string dbfName = System.IO.Path.GetFileName(pathName);

            result = this.OpenDBFTable(dbfPath, dbfName);
            return result;
        }

        /// <summary>
        /// 打开dbf表
        /// </summary>
        /// <param name="pathName"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        private ITable OpenDBFTable(string pathName, string tableName)
        {
            // Create the workspace name object.
            IWorkspaceName workspaceName = new WorkspaceNameClass();
            workspaceName.PathName = pathName;
            workspaceName.WorkspaceFactoryProgID = "esriDataSourcesFile.shapefileworkspacefactory";
            // Create the table name object.
            IDatasetName dataSetName = new TableNameClass();
            dataSetName.Name = tableName;
            dataSetName.WorkspaceName = workspaceName;
            // Open the table.
            IName name = (IName)dataSetName;
            ITable table = (ITable)name.Open();
            return table;
        }
        /// <summary>
        /// 通过名称获取要素图层
        /// </summary>
        /// <param name="pFeatlayerList"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private IFeatureLayer GetFeatureLayerByName(List<IFeatureLayer> pFeatlayerList, string name)
        {
            IFeatureLayer selectLyr = null;
            foreach (IFeatureLayer pFeatureLay in pFeatlayerList)
            {
                if (pFeatureLay.Name == name)
                {
                    selectLyr = pFeatureLay;
                    break;
                }
            }
            return selectLyr;
        }
        /// <summary>
        /// 遍历更新图层和待更新图层-获取更新信息-guid对比，未使用
        /// </summary>
        /// <param name="toUpdateLyr">原始图层</param>
        /// <param name="updatedLyr">待更新图层</param>
        /// <returns></returns>
        private List<ClsUpdateInfo> GetUpdateInfoDisposed(IFeatureLayer toUpdateLyr, IFeatureLayer updatedLyr)
        {
            string guidFieldTo = ClsConfig.LayerConfigs[(toUpdateLyr.FeatureClass as IDataset).Name].GUID;
            string fromFieldTo = ClsConfig.LayerConfigs[(toUpdateLyr.FeatureClass as IDataset).Name].StartVersion;
            string endFieldTo = ClsConfig.LayerConfigs[(toUpdateLyr.FeatureClass as IDataset).Name].EndVersion;

            string guidFieldEd = ClsConfig.LayerConfigs[(updatedLyr.FeatureClass as IDataset).Name].GUID;
            string fromFieldEd = ClsConfig.LayerConfigs[(updatedLyr.FeatureClass as IDataset).Name].StartVersion;
            string endFieldEd = ClsConfig.LayerConfigs[(updatedLyr.FeatureClass as IDataset).Name].EndVersion;

            List<ClsUpdateInfo> updateInfos = new List<ClsUpdateInfo>();

            IFeatureCursor pCursor = null;
            IFeature pFeature = null;
            pCursor = updatedLyr.FeatureClass.Search(null, false);
            if (pCursor != null)
            {
                pFeature = pCursor.NextFeature();
            }

            ClsBarSync pBarEntiUpdate = new ClsBarSync(progressBarXEntiUpdate);
            pBarEntiUpdate.SetMax(1);
            pBarEntiUpdate.SetMax(updatedLyr.FeatureClass.FeatureCount(null));

            //两个while循环遍历两个图层中的每一个要素
            while (pFeature != null)
            {
                string guid = pFeature.get_Value(pFeature.Fields.FindField(guidFieldEd)).ToString();
                string startVersion = pFeature.get_Value(pFeature.Fields.FindField(fromFieldEd)).ToString();
                string endVersion = pFeature.get_Value(pFeature.Fields.FindField(endFieldEd)).ToString();

                IFeatureCursor pCursorToUpdate = null;
                IFeature pFeatureToUpdate = null;
                //查询两个字段
                IQueryFilter queryFilter = new QueryFilterClass();
                queryFilter.WhereClause = guidFieldTo + " = '" + guid + "'";
                pCursorToUpdate = toUpdateLyr.FeatureClass.Search(queryFilter, false);
                if (pCursorToUpdate != null)
                {
                    pFeatureToUpdate = pCursorToUpdate.NextFeature();
                }
                if (pFeatureToUpdate == null)//没找到GUID相同的，说明是新增的
                {
                    ClsUpdateInfo ui = new ClsUpdateInfo();
                    ui.UpdateState = "New";
                    ui.GUID = guid;
                    ui.Feature = pFeature;
                    ui.ToUpdateLyr = toUpdateLyr;
                    ui.UpdatedLyr = updatedLyr;
                    updateInfos.Add(ui);
                }
                while (pFeatureToUpdate != null)
                {
                    string startVersionToUpdate = pFeatureToUpdate.get_Value(pFeatureToUpdate.Fields.FindField(fromFieldTo)).ToString();
                    string endVersionToUpdate = pFeatureToUpdate.get_Value(pFeatureToUpdate.Fields.FindField(endFieldTo)).ToString();

                    if (startVersionToUpdate == startVersion && endVersionToUpdate == endVersion)//版本号没变
                    {
                        break;
                    }
                    else
                    {
                        if (startVersionToUpdate == startVersion)//起始号相同，终止号不同，说明要素被删除了
                        {
                            ClsUpdateInfo ui = new ClsUpdateInfo();
                            ui.UpdateState = "Delete";
                            ui.GUID = guid;
                            ui.Feature = pFeature;
                            ui.ToUpdateLyr = toUpdateLyr;
                            ui.UpdatedLyr = updatedLyr;
                            updateInfos.Add(ui);
                            break;
                        }
                        else if (endVersionToUpdate == endVersion)//起始号不同，终止号相同，说明要素被更新了，因为更新的要素才可能出现guid相同，起始号不同
                        {
                            ClsUpdateInfo ui = new ClsUpdateInfo();
                            ui.UpdateState = "Update";
                            ui.GUID = guid;
                            ui.Feature = pFeature;
                            ui.ToUpdateLyr = toUpdateLyr;
                            ui.UpdatedLyr = updatedLyr;
                            updateInfos.Add(ui);
                            break;
                        }
                        else
                        {
                            pFeatureToUpdate = pCursorToUpdate.NextFeature();
                        }
                    }
                }
                pBarEntiUpdate.PerformOneStep();
                pFeature = pCursor.NextFeature();
            }

            return updateInfos;
        }
        private List<ClsUpdateInfo> GetUpdateInfo(IFeatureLayer toUpdateLyr, IFeatureLayer updatedLyr)
        {
            List<ClsUpdateInfo> updateInfos = new List<ClsUpdateInfo>();

            string usourceField = ClsConfig.LayerConfigs[(updatedLyr.FeatureClass as IDataset).Name].SourceName;
            string fromVersionField = ClsConfig.LayerConfigs[(updatedLyr.FeatureClass as IDataset).Name].StartVersion;
            string toVersionField = ClsConfig.LayerConfigs[(updatedLyr.FeatureClass as IDataset).Name].EndVersion;
            string guidField = ClsConfig.LayerConfigs[(updatedLyr.FeatureClass as IDataset).Name].GUID;
            //只查询一个字段
            //获取更新数据的当前版本号
            IQueryFilter queryFilter = new QueryFilterClass();
            queryFilter.WhereClause = "USOURSE = '" + usourceField + "'";
            ICursor rowCursor = this._versionTable.Search(queryFilter, false);
            IRow row = rowCursor.NextRow();
            int currentVersion = 0;
            while (row != null)
            {
                string version = row.get_Value(row.Fields.FindField("VERSION_ID")).ToString();
                int versionInt = Int32.Parse(version);
                if (versionInt > currentVersion)
                {
                    currentVersion = versionInt;
                }
                row = rowCursor.NextRow();
            }
            //查询两个字段
            //获取更新数据的版本号和当前版本号的关系
            IFeatureCursor pCursor = null;
            IFeature pFeature = null;
            queryFilter = new QueryFilterClass();
            queryFilter.WhereClause = fromVersionField + " = '" + currentVersion.ToString() + "' or " + toVersionField + " = '" + currentVersion.ToString() + "'";
            pCursor = updatedLyr.FeatureClass.Search(queryFilter, false);
            if (pCursor != null)
            {
                pFeature = pCursor.NextFeature();
            }

            ClsBarSync pBarEntiUpdate = new ClsBarSync(progressBarXEntiUpdate);
            pBarEntiUpdate.SetMax(updatedLyr.FeatureClass.FeatureCount(queryFilter));

            //两个while循环遍历两个图层中的每一个要素
            while (pFeature != null)
            {
                string startVersion = pFeature.get_Value(pFeature.Fields.FindField(fromVersionField)).ToString();
                string endVersion = pFeature.get_Value(pFeature.Fields.FindField(toVersionField)).ToString();
                string guid = pFeature.get_Value(pFeature.Fields.FindField(guidField)).ToString();
                if (startVersion == currentVersion.ToString() && endVersion == "99999")//起始号为当前版本，终止号为99999的话就是新增的要素
                {
                    ClsUpdateInfo ui = new ClsUpdateInfo();
                    ui.UpdateState = "New";
                    ui.GUID = guid;
                    ui.Feature = pFeature;
                    ui.ToUpdateLyr = toUpdateLyr;
                    ui.UpdatedLyr = updatedLyr;
                    updateInfos.Add(ui);
                }
                else if (startVersion != currentVersion.ToString()&&endVersion == currentVersion.ToString())//起始号不同，终止号为当前版本，则为删除
                {
                    ClsUpdateInfo ui = new ClsUpdateInfo();
                    ui.UpdateState = "Delete";
                    ui.GUID = guid;
                    ui.Feature = pFeature;
                    ui.ToUpdateLyr = toUpdateLyr;
                    ui.UpdatedLyr = updatedLyr;
                    updateInfos.Add(ui);
                }
                else 
                {
                   
                }

                pBarEntiUpdate.PerformOneStep();
                pFeature = pCursor.NextFeature();
            }

            return updateInfos;
        }
        /// <summary>
        /// 把遍历出的更新信息转变为以guid为key的字典形式
        /// </summary>
        /// <param name="updateInfos"></param>
        /// <returns></returns>
        private Dictionary<string, ClsUpdateInfo> ProcessUpdateInfo(List<ClsUpdateInfo> updateInfos)
        {
            Dictionary<string, List<ClsUpdateInfo>> processResult = new Dictionary<string, List<ClsUpdateInfo>>();
            for (int i = 0; i < updateInfos.Count; i++)
            {
                if (!processResult.ContainsKey(updateInfos[i].GUID))
                {
                    processResult.Add(updateInfos[i].GUID, new List<ClsUpdateInfo>());
                }
                processResult[updateInfos[i].GUID].Add(updateInfos[i]);
            }

            Dictionary<string, ClsUpdateInfo> result = new Dictionary<string, ClsUpdateInfo>();

            int keysCount = processResult.Keys.Count;
            for (int i = 0; i < keysCount; i++)
            {
                string key = processResult.Keys.ElementAt(i);
                if (processResult[key].Count == 1)
                {
                    result.Add(key, processResult[key][0]);
                }
                else
                {
                    for (int j = 0; j < processResult[key].Count; j++)
                    {
                        if (processResult[key][j].UpdateState == "New")
                        {
                            processResult[key][j].UpdateState = "Update";
                            result.Add(key, processResult[key][j]);
                        }
                    }
                }
            }

            return result;
        }
    }
}
