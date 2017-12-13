using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
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
    public partial class FrmUpdateResult : Form
    {
        private Dictionary<string, ClsUpdateInfo> _processedUpdateInfos;
        private Dictionary<string, string> dic = new Dictionary<string, string>();
        private ITable _entityTable = null;
        private ITable _versionTable = null;



        public FrmUpdateResult()
        {
            InitializeComponent();

            this.updateState.Nodes.Add("新增");
            this.updateState.Nodes.Add("修改");
            this.updateState.Nodes.Add("删除");
            this.updateState.ExpandAll();

            dic.Add("新增", "New");
            dic.Add("修改", "Update");
            dic.Add("删除", "Delete");
        }


        /// <summary>
        /// 加载填充数据
        /// </summary>
        /// <param name="processedUpdateInfos"></param>
        /// <param name="entityTable"></param>
        /// <param name="versionTable"></param>
        public void LoadData(Dictionary<string, ClsUpdateInfo> processedUpdateInfos, ITable entityTable, ITable versionTable)
        {
            this._processedUpdateInfos = processedUpdateInfos;
            this._entityTable = entityTable;
            this._versionTable = versionTable;
            this.ClearData();
            //初始化显示为新增状态
            this.FillGridView("新增");
        }
        /// <summary>
        /// 填充DataGridView
        /// </summary>
        /// <param name="type"></param>
        private void FillGridView(string type)
        {
            for (int i = 0; i < this._processedUpdateInfos.Count; i++)
            {
                string key = this._processedUpdateInfos.Keys.ElementAt(i);
                if (this._processedUpdateInfos[key].UpdateState == dic[type])
                {
                    switch (type)
                    {
                        case "新增":
                            this.FillGridViewAdd(this._processedUpdateInfos[key]);
                            break;
                        case "删除":
                            this.FillGridViewDelete(this._processedUpdateInfos[key]);
                            break;
                        case "修改":
                            this.FillGridViewUpdate(this._processedUpdateInfos[key]);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        /// <summary>
        /// 新增实体数据——填充DataGridView
        /// </summary>
        /// <param name="updateInfo"></param>
        private void FillGridViewAdd(ClsUpdateInfo updateInfo)
        {
            int index = this.updateContent.Rows.Add();

            string entiField = ClsConfig.LayerConfigs[updateInfo.UpdatedLyr.Name].EntityID;
            string nameField = ClsConfig.LayerConfigs[updateInfo.UpdatedLyr.Name].NameField;
            string fromField = ClsConfig.LayerConfigs[updateInfo.UpdatedLyr.Name].StartVersion;
            string sourceName = ClsConfig.LayerConfigs[updateInfo.UpdatedLyr.Name].SourceName;
            string sourceType = ClsConfig.LayerConfigs[updateInfo.UpdatedLyr.Name].SourceType;
            IFeature feat = updateInfo.Feature;

            string entityID = feat.get_Value(feat.Fields.FindField(entiField)).ToString();
            this.updateContent.Rows[index].Cells[0].Value = entityID;

            string name = feat.get_Value(feat.Fields.FindField(nameField)).ToString();
            this.updateContent.Rows[index].Cells[1].Value = name;

            string fromTimeID = feat.get_Value(feat.Fields.FindField(fromField)).ToString();
            DateTime fromTime = this.GetDateTime(fromTimeID, sourceName, this._versionTable);
            this.updateContent.Rows[index].Cells[2].Value = fromTime.ToShortDateString();

            this.updateContent.Rows[index].Cells[3].Value = "";
            this.updateContent.Rows[index].Cells[4].Value = fromTime.ToShortDateString();
            this.updateContent.Rows[index].Cells[5].Value = sourceName;
            this.updateContent.Rows[index].Cells[6].Value = sourceType;

            int keysCount = ClsConfig.SourceDatabase.Keys.Count;
            string updateDateBase = "";
            for (int i = 0; i < keysCount; i++)
            {
                string key = ClsConfig.SourceDatabase.Keys.ElementAt(i);
                if (key != sourceType)
                {
                    updateDateBase += key;
                }
            }
            this.updateContent.Rows[index].Cells[7].Value = updateDateBase;
            this.updateContent.Rows[index].Tag = updateInfo;


            ////刷新地理实体表
            UpdateNewEntiData(updateInfo);


            ////刷新地理实体表
            //IDataset dataset = this._entityTable as IDataset;
            //IWorkspace workspace = dataset.Workspace;
            //IWorkspaceEdit workspaceEdit = workspace as IWorkspaceEdit;
            //workspaceEdit.StartEditing(true);
            //workspaceEdit.StartEditOperation();

            //IRowBuffer rowBuffer = null;
            //ICursor rowCursor = null;
            //IRow row = null;

            //IQueryFilter queryFilter = new QueryFilterClass();
            //queryFilter.WhereClause = entiField + " = '" + feat.get_Value(feat.Fields.FindField(entiField)).ToString() + "'";
            ////把实体表的遍历放入到Feature的遍历过程中
            //rowCursor = this._entityTable.Search(queryFilter, false);
            //row = rowCursor.NextRow();
            //while (row == null)
            //{
            //    rowBuffer = this._entityTable.CreateRowBuffer();
            //    rowCursor = this._entityTable.Insert(true);
            //    rowBuffer = InsertEntiData(rowBuffer, feat, this._versionTable, pFeatureLayname);
            //    rowCursor.InsertRow(rowBuffer);
            //}
            //workspaceEdit.StopEditOperation();
            //workspaceEdit.StopEditing(true);

        }
        /// <summary>
        /// 同步更新地理实体表
        /// </summary>
        /// <param name="updateInfo"></param>
        public void UpdateNewEntiData(ClsUpdateInfo updateInfo)
        {
            string entiField = ClsConfig.LayerConfigs[updateInfo.UpdatedLyr.Name].EntityID;
            IFeature feat = updateInfo.Feature;

            //刷新地理实体表
            IDataset dataset = this._entityTable as IDataset;
            IWorkspace workspace = dataset.Workspace;
            IWorkspaceEdit workspaceEdit = workspace as IWorkspaceEdit;
            workspaceEdit.StartEditing(true);
            workspaceEdit.StartEditOperation();

            IRowBuffer rowBuffer = null;
            ICursor rowCursor = null;
            IRow row = null;

            IQueryFilter queryFilter = new QueryFilterClass();
            queryFilter.WhereClause = entiField + " = '" + feat.get_Value(feat.Fields.FindField(entiField)).ToString() + "'";
            //把实体表的遍历放入到Feature的遍历过程中
            rowCursor = this._entityTable.Search(queryFilter, false);
            //test
            int test = this._entityTable.RowCount(null);
            row = rowCursor.NextRow();
            if(row == null)
            {
                rowBuffer = this._entityTable.CreateRowBuffer();
                rowCursor = this._entityTable.Insert(true);
                rowBuffer = InsertEntiData(rowBuffer, feat, this._versionTable, updateInfo.UpdatedLyr.Name);
                rowCursor.InsertRow(rowBuffer);

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
        public IRowBuffer InsertEntiData(IRowBuffer rowBuffer, IFeature pFeature,ITable versionTable, string pFeatureLayname)
        {
            try
            {
                string nameField = ClsConfig.LayerConfigs[pFeatureLayname].NameField;
                string entityField = ClsConfig.LayerConfigs[pFeatureLayname].EntityID;
                string fromField = ClsConfig.LayerConfigs[pFeatureLayname].StartVersion;
                string sourceName = ClsConfig.LayerConfigs[pFeatureLayname].SourceName;
                string sourceType = ClsConfig.LayerConfigs[pFeatureLayname].SourceType;

                //如果地理实体有名称字段并且含有名称
                if (pFeature.get_Value(pFeature.Fields.FindField(nameField)).ToString().Length > 0)
                {
                    rowBuffer.set_Value(rowBuffer.Fields.FindField("FNAME"), pFeature.get_Value(pFeature.Fields.FindField(nameField)).ToString());
                }

                rowBuffer.set_Value(rowBuffer.Fields.FindField("ENTIID"), pFeature.get_Value(pFeature.Fields.FindField(entityField)).ToString());

                DateTime collectTime = GetDateTime(pFeature.get_Value(pFeature.Fields.FindField(fromField)).ToString(), sourceName, versionTable);

                rowBuffer.set_Value(rowBuffer.Fields.FindField("STARTDATE"), collectTime.ToShortDateString());
                rowBuffer.set_Value(rowBuffer.Fields.FindField("UPDATEDATE"), collectTime.ToShortDateString());
                rowBuffer.set_Value(rowBuffer.Fields.FindField("USOURCE"), sourceName);
                rowBuffer.set_Value(rowBuffer.Fields.FindField("ESOURCE"), sourceType);

            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return rowBuffer;

        }

        /// <summary>
        /// 删除实体数据——填充DataGridView
        /// </summary>
        /// <param name="updateInfo"></param>
        private void FillGridViewDelete(ClsUpdateInfo updateInfo)
        {
            int index = this.updateContent.Rows.Add();

            string entiField = ClsConfig.LayerConfigs[updateInfo.UpdatedLyr.Name].EntityID;
            string fromField = ClsConfig.LayerConfigs[updateInfo.UpdatedLyr.Name].StartVersion;
            string sourceName = ClsConfig.LayerConfigs[updateInfo.UpdatedLyr.Name].SourceName;
            string sourceType = ClsConfig.LayerConfigs[updateInfo.UpdatedLyr.Name].SourceType;
            IFeature feat = updateInfo.Feature;

            string entityID = feat.get_Value(feat.Fields.FindField(entiField)).ToString();
            this.updateContent.Rows[index].Cells[0].Value = entityID;

            IQueryFilter queryFilter = new QueryFilterClass();
            queryFilter.WhereClause = "ENTIID = '" + entityID + "'";
            ICursor cursor = this._entityTable.Search(queryFilter, false);
            IRow row = cursor.NextRow();

            string name = row.get_Value(row.Fields.FindField("FNAME")).ToString();
            this.updateContent.Rows[index].Cells[1].Value = name;

            string startTime = row.get_Value(row.Fields.FindField("STARTDATE")).ToString();
            this.updateContent.Rows[index].Cells[2].Value = startTime;

            string fromTimeID = feat.get_Value(feat.Fields.FindField(fromField)).ToString();
            DateTime fromTime = this.GetDateTime(fromTimeID, sourceName, this._versionTable);
            this.updateContent.Rows[index].Cells[3].Value = fromTime.ToShortDateString();
            this.updateContent.Rows[index].Cells[4].Value = fromTime.ToShortDateString();

            this.updateContent.Rows[index].Cells[5].Value = sourceName;

            string eSource = row.get_Value(row.Fields.FindField("ESOURCE")).ToString();
            this.updateContent.Rows[index].Cells[6].Value = eSource;

            int keysCount = ClsConfig.SourceDatabase.Keys.Count;
            string updateDateBase = "";
            for (int i = 0; i < keysCount; i++)
            {
                string key = ClsConfig.SourceDatabase.Keys.ElementAt(i);
                if (eSource.Contains(key))
                {
                    if (key != sourceType)
                    {
                        updateDateBase += key;
                    }
                }

            }
            this.updateContent.Rows[index].Cells[7].Value = updateDateBase;
            this.updateContent.Rows[index].Tag = updateInfo;
        }

        /// <summary>
        /// 更改实体数据——填充DataGridView
        /// </summary>
        /// <param name="updateInfo"></param>
        private void FillGridViewUpdate(ClsUpdateInfo updateInfo)
        {
            int index = this.updateContent.Rows.Add();

            string entiField = ClsConfig.LayerConfigs[updateInfo.UpdatedLyr.Name].EntityID;
            string fromField = ClsConfig.LayerConfigs[updateInfo.UpdatedLyr.Name].StartVersion;
            string sourceName = ClsConfig.LayerConfigs[updateInfo.UpdatedLyr.Name].SourceName;
            string sourceType = ClsConfig.LayerConfigs[updateInfo.UpdatedLyr.Name].SourceType;
            IFeature feat = updateInfo.Feature;

            string entityID = feat.get_Value(feat.Fields.FindField(entiField)).ToString();
            this.updateContent.Rows[index].Cells[0].Value = entityID;

            IQueryFilter queryFilter = new QueryFilterClass();
            queryFilter.WhereClause = "ENTIID = '" + entityID + "'";
            ICursor cursor = this._entityTable.Search(queryFilter, false);
            IRow row = cursor.NextRow();

            string name = row.get_Value(row.Fields.FindField("FNAME")).ToString();
            this.updateContent.Rows[index].Cells[1].Value = name;

            string startTime = row.get_Value(row.Fields.FindField("STARTDATE")).ToString();
            this.updateContent.Rows[index].Cells[2].Value = startTime;

            this.updateContent.Rows[index].Cells[3].Value = "";

            string fromTimeID = feat.get_Value(feat.Fields.FindField(fromField)).ToString();
            DateTime fromTime = this.GetDateTime(fromTimeID, sourceName, this._versionTable);

            string updateDate = row.get_Value(row.Fields.FindField("UPDATEDATE")).ToString();
            DateTime updateTime = DateTime.Parse(updateDate);

            if (updateTime > fromTime)
            {
                this.updateContent.Rows[index].Cells[4].Value = updateTime.ToShortDateString();
            }
            else
            {
                this.updateContent.Rows[index].Cells[4].Value = fromTime.ToShortDateString();
            }

            this.updateContent.Rows[index].Cells[5].Value = sourceName;

            string eSource = row.get_Value(row.Fields.FindField("ESOURCE")).ToString();
            this.updateContent.Rows[index].Cells[6].Value = eSource;

            int keysCount = ClsConfig.SourceDatabase.Keys.Count;
            string updateDateBase = "";
            for (int i = 0; i < keysCount; i++)
            {
                string key = ClsConfig.SourceDatabase.Keys.ElementAt(i);
                if (eSource.Contains(key))
                {
                    if (key != sourceType)
                    {
                        updateDateBase += key;
                    }
                }

            }
            this.updateContent.Rows[index].Cells[7].Value = updateDateBase;
            this.updateContent.Rows[index].Tag = updateInfo;
        }
        /// <summary>
        /// 清除DatagridView并建立列
        /// </summary>
        private void ClearData()
        {
            this.updateContent.Columns.Clear();
            this.updateContent.Rows.Clear();

            this.updateContent.Columns.Add("ENTIID", "地理实体标识码");
            this.updateContent.Columns.Add("FNAME", "地理实体名称");
            this.updateContent.Columns.Add("STARTDATE", "起始时间");
            this.updateContent.Columns.Add("ENDDATE", "消亡时间");
            this.updateContent.Columns.Add("UPDATEDATE", "更新时间");
            this.updateContent.Columns.Add("USOURCE", "更新数据源");
            this.updateContent.Columns.Add("ESOURCE", "实体所在数据库");
            this.updateContent.Columns.Add("TSOURCE", "同步更新数据库");

            this.updateContent.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            this.updateContent.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            this.updateContent.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
            this.updateContent.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
            this.updateContent.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
            this.updateContent.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
            this.updateContent.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
            this.updateContent.Columns[7].SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        private void updateState_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            this.ClearData();
            this.FillGridView(e.Node.Text);
        }

        public DateTime GetDateTime(string versionID, string type, ITable versionTable)
        {
            DateTime result = DateTime.Now;

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

        private void updateContent_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            ClsUpdateInfo updateInfo = this.updateContent.Rows[e.RowIndex].Tag as ClsUpdateInfo;
            IMapControl4 pMainMapControl = ClsControl.MapControlMain;
            pMainMapControl.Map.ClearSelection();//清除地图的选择
            IEnvelope env = new EnvelopeClass();
            pMainMapControl.Map.SelectFeature(updateInfo.UpdatedLyr, updateInfo.Feature);//将查询到的地物作为选择对象高亮显示在地图上 
            env.Union(updateInfo.Feature.Shape.Envelope);
            //env.Expand(2, 2, true);
            //pMainMapControl.Extent = env;

            IPoint pt = new PointClass();
            pt.X = (env.UpperLeft.X + env.LowerRight.X) / 2;
            pt.Y = (env.UpperLeft.Y + env.LowerRight.Y) / 2;
            pMainMapControl.CenterAt(pt);
            pMainMapControl.ActiveView.Refresh();//刷新地图，这样才能显示出地物 
        }
    }
}
