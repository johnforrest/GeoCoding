using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.esriSystem;
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
    public partial class FrmEntitySearch : DevComponents.DotNetBar.Office2007Form
    {
        private ITable _versionTable = null;
        private ITable _entityTable = null;
        //实体编码，实体来源
        private Dictionary<string, string> _entityDictionary = new Dictionary<string, string>();
        private ClsGraphElements _graphElements = new ClsGraphElements();
        private IMapControl4 _mapControl = null;
        private List<IFeatureLayer> _pFeatlayerList = null;
        private FrmItemAttr _frm = new FrmItemAttr();
        public FrmEntitySearch()
        {
            InitializeComponent();
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
                this.FillEntityDictionary();
                this.FillAutoCompleteBox();
            }
        }
        /// <summary>
        /// 打开版本记录表路径
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
        /// <param name="name">实体表名称</param>
        /// <param name="path">实体表路径</param>
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
        /// <summary>
        /// 打开版本记录表
        /// </summary>
        /// <param name="pathName"></param>
        /// <returns></returns>
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
        /// 填充实体字典
        /// </summary>
        private void FillEntityDictionary()
        {
            this._entityDictionary.Clear();
            ICursor cursor = this._entityTable.Search(null, false);
            IRow row = cursor.NextRow();
            while (row != null)
            {
                string entityID = row.get_Value(row.Fields.FindField("ENTIID")).ToString();
                string source = row.get_Value(row.Fields.FindField("ESOURCE")).ToString();
                this._entityDictionary.Add(entityID, source);
                row = cursor.NextRow();
            }
        }
        /// <summary>
        /// 填充TextBox
        /// </summary>
        private void FillAutoCompleteBox()
        {
            AutoCompleteStringCollection ac = new AutoCompleteStringCollection();

            foreach (var item in this._entityDictionary)
            {
                ac.Add(item.Key);
            }

            this.EntityCode.AutoCompleteCustomSource = ac;//设置该文本框的自动完成数据源
        }
        /// <summary>
        /// 获取图层元素
        /// </summary>
        /// <param name="pFeature">要素</param>
        /// <param name="lyrName">图层名</param>
        /// <returns></returns>
        private ClsGraphElement GetGraphElement(IFeature pFeature, string lyrName)
        {
            string fromField = ClsConfig.LayerConfigs[lyrName].StartVersion;
            string endField = ClsConfig.LayerConfigs[lyrName].EndVersion;
            string sourceName = ClsConfig.LayerConfigs[lyrName].SourceName;
            string sourceType = ClsConfig.LayerConfigs[lyrName].SourceType;
            //声明一个类ClsGraphElement的实例ge
            ClsGraphElement ge = new ClsGraphElement();
            string fromVersion = pFeature.get_Value(pFeature.Fields.FindField(fromField)).ToString();
            DateTime collectTime = this.GetDateTime(fromVersion, sourceName, this._versionTable);
            string endVersion = pFeature.get_Value(pFeature.Fields.FindField(endField)).ToString();
            DateTime endTime = this.GetDateTime(endVersion, sourceName, this._versionTable);
            //对类的各个字段进行赋值
            ge.VersionTime = collectTime;
            ge.EndVersionTime = endTime;
            ge.Feature = pFeature;
            ge.Source = sourceType;
            ge.LayerName = lyrName;
            ge.Version = fromVersion;
            //返回类
            return ge;
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
        /// 确定（输入完成之后）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EntityCodeBtn_Click(object sender, EventArgs e)
        {
            string selectEntityID = this.EntityCode.Text;
            if (selectEntityID == "")
            {
                MessageBox.Show("查询的地理实体不存在！");
                return;
            }
            string source = this._entityDictionary[selectEntityID];

            IMap pMap = this._mapControl.Map;
            if (pMap == null) { return; }

            _pFeatlayerList = new List<IFeatureLayer>();
            ClsCommon pClsCommon = new ClsCommon();
            _pFeatlayerList = pClsCommon.GetFeatureLayers(pMap);

            this._graphElements.Clear();

            foreach (IFeatureLayer pFeatureLay in _pFeatlayerList)
            {
                string sourceType = ClsConfig.LayerConfigs[pFeatureLay.Name].SourceType;
                if (source.Contains(sourceType))
                {
                    this.GetGraphElementsInLyr(selectEntityID, _pFeatlayerList, pFeatureLay.Name);
                }
            }
            this.InitialTrackBar();
        }
        /// <summary>
        /// 初始化TrackBar
        /// </summary>
        private void InitialTrackBar()
        {
            var versionCount = this._graphElements.EntityElements.Keys.Count;
            if (versionCount > 0)
            {
                this.VersionTimeSld.Minimum = 0;
                this.VersionTimeSld.Maximum = versionCount - 1;
                this.VersionTimeSld.Value = 0;

                double deltaWidth = (400 - 95) * 1.0 / (versionCount - 1);
                for (int i = 0; i < versionCount; i++)
                {
                    Label lbl = new Label();//声明一个label
                    lbl.Location = new System.Drawing.Point((int)(deltaWidth * i + 95), 260);//设置位置
                    //lbl.Size = new Size(100, 20);//设置大小
                    DateTime key = this._graphElements.EntityElements.Keys.ElementAt(i);
                    List<ClsGraphElement> plist = this._graphElements.EntityElements[key];

                    //lbl.Text = key.ToShortDateString() + plist[0].LayerName;
                    lbl.Text = key.ToShortDateString() + plist[0].Source;
                    this.Controls.Add(lbl);//在当前窗体上添加这个label控件

                    //Label labelsource = new Label();//声明一个label
                    //labelsource.Location = new System.Drawing.Point((int)(deltaWidth * i + 95), 240);//设置位置
                    ////lbl.Size = new Size(100, 20);//设置大小
                    ////DateTime key = this._graphElements.EntityElements.Keys.ElementAt(i);
                    //List<ClsGraphElement> plist=this._graphElements.EntityElements[key];
                    ////labelsource.Text = key.ToShortDateString();
                    //labelsource.Text = plist[0].LayerName;
                    //this.Controls.Add(labelsource);//在当前窗体上添加这个label控件
                }
                this.VersionTimeSld_ValueChanged(null, null);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityID">实体编码</param>
        /// <param name="pFeatlayerList">MapControl中的图层集</param>
        /// <param name="lyrName">图层名称</param>
        private void GetGraphElementsInLyr(string entityID, List<IFeatureLayer> pFeatlayerList, string lyrName)
        {
            string entityField = ClsConfig.LayerConfigs[lyrName].EntityID;

            IFeatureLayer featLyr = this.GetFeatureLayerByName(pFeatlayerList, lyrName);
            IFeatureCursor pCursor = null;
            IFeature pFeature = null;
            //查询一个字段（ENTIID）
            IQueryFilter queryFilter = new QueryFilterClass();
            queryFilter.WhereClause = entityField + " = '" + entityID + "'";
            pCursor = featLyr.FeatureClass.Search(queryFilter, false);
            if (pCursor != null)
            {
                pFeature = pCursor.NextFeature();
            }
            while (pFeature != null)
            {
                ClsGraphElement ge = this.GetGraphElement(pFeature, lyrName);
                this._graphElements.Add(ge);
                pFeature = pCursor.NextFeature();
            }
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
        /// trackBarValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VersionTimeSld_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime key = this._graphElements.EntityElements.Keys.ElementAt(this.VersionTimeSld.Value);
                List<ClsGraphElement> graphElements = this._graphElements.EntityElements[key];

                this._mapControl.Map.ClearSelection();//清除地图的选择

                IEnvelope env = new EnvelopeClass();
                for (int i = 0; i < graphElements.Count; i++)
                {
                    ClsGraphElement ge = graphElements[i];
                    this._mapControl.Map.SelectFeature(this.GetFeatureLayerByName(this._pFeatlayerList, ge.LayerName), ge.Feature);//将查询到的地物作为选择对象高亮显示在地图上 
                    env.Union(ge.Feature.Shape.Envelope);
                }

                //env.Expand(2, 2, true);
                //this._mapControl.Extent = env;

                IPoint pt = new PointClass();
                pt.X = (env.UpperLeft.X + env.LowerRight.X) / 2;
                pt.Y = (env.UpperLeft.Y + env.LowerRight.Y) / 2;
                this._mapControl.CenterAt(pt);
                this._mapControl.ActiveView.Refresh();//刷新地图，这样才能显示出地物 
                //显示查询的信息
                this.ShowEntityData(this.EntityCode.Text);
            }
            catch (System.Exception ex)
            {
                //MessageBox.Show(ex.Message);
                MessageBox.Show("请先点击确定！");
            }
        }
        /// <summary>
        /// 显示该条地理实体数据
        /// </summary>
        /// <param name="entityID">地理实体编码</param>
        private void ShowEntityData(string entityID)
        {
            IQueryFilter queryFilter = new QueryFilterClass();
            queryFilter.WhereClause = "ENTIID = '" + entityID + "'";
            ICursor cursor = this._entityTable.Search(queryFilter, false);
            IRow row = cursor.NextRow();

            if (this._frm.IsDisposed)
            {
                this._frm = new FrmItemAttr();
            }
            this._frm.LoadData(row);
            this._frm.Show();
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmEntitySearch_Load(object sender, EventArgs e)
        {
            IMapControl4 pMainMapControl = ClsControl.MapControlMain;
            this._mapControl = pMainMapControl;
        }
    }
}
