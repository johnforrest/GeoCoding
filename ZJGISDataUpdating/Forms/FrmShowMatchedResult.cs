using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using DevComponents.DotNetBar.Controls;
using ESRI.ArcGIS.Controls;
using System.Collections.ObjectModel;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;
using SelectQuery;
using ZJGISCommon;
using ZJGISEntiTable.Froms;
using ZJGISDataUpdating.Forms;
using ZJGISDataUpdating.Class;


namespace ZJGISDataUpdating
{
    public partial class FrmShowMatchedResult : DevComponents.DotNetBar.Office2007Form
    {

        // 20171006 定义下拉列表框
        private ComboBox cmb_Temp = new ComboBox();
        StrRowAndCol[] pRowAndCol = new StrRowAndCol[10000];
        int count = 0;

        ClsTableWrapper m_ClsTabWrapper;
        ITable m_Table;
        private string m_TableName;

        public string TableName
        {
            get { return m_TableName; }
            set { m_TableName = value; }
        }

        Dictionary<int, DataGridViewRow> m_InRowDic;
        Dictionary<int, DataGridViewRow> m_OutRowDic;

        IMapControl4 m_MapControlFrom;
        IMapControl4 m_MapControlTo;
        IMapControl4 m_MapControlOverlap;
        DevComponents.DotNetBar.TabControl m_TabControl;

        IFeatureClass m_TUFeatCls = null;
        IFeatureClass m_TEFeatCls = null;

        DataGridViewX dgvFrom = new DataGridViewX();
        DataGridViewX dgvTo = new DataGridViewX();

        bool m_bAll = false;
        bool m_bMatched = false;
        bool m_bAttribute = false;
        bool m_bShape = false;
        bool m_bAttriAndShape = false;
        bool m_bNewFeat = false;
        bool m_bOneToMore = false;

        bool m_bMoreToOne = false;
        bool m_bOneToOne = false;
        bool m_bMoreToMore = false;
        bool m_bNew = false;

        FrmTwoAttri two = new FrmTwoAttri();

        public FrmShowMatchedResult()
        {
            InitializeComponent();

            m_InRowDic = new Dictionary<int, DataGridViewRow>();
            m_OutRowDic = new Dictionary<int, DataGridViewRow>();

            this.bindingNavigator1.BindingSource = this.bindingSource1;
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
        public ITable Table
        {
            get
            {
                return m_Table;
            }
            set
            {
                m_Table = value;
            }
        }
        public DataGridViewX DGVX
        {
            get
            {
                return this.dataGridViewX1;
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
        public DevComponents.DotNetBar.TabControl tabControl
        {
            get
            {
                return m_TabControl;
            }
            set
            {
                m_TabControl = value;
            }
        }
        public IMapControl4 MapControlFrom
        {
            get
            {
                return m_MapControlFrom;
            }
            set
            {
                m_MapControlFrom = value;
            }
        }
        public IMapControl4 MapControlTo
        {
            get
            {
                return m_MapControlTo;
            }
            set
            {
                m_MapControlTo = value;
            }
        }
        public IMapControl4 MapControlOverlap
        {
            get
            {
                return m_MapControlOverlap;
            }
            set
            {
                m_MapControlOverlap = value;
            }
        }
        public DataGridViewX DGVFrom
        {
            set
            {
                dgvFrom = value;
            }
        }
        public DataGridViewX DGVTo
        {
            set
            {
                dgvTo = value;
            }
        }

        private void FrmShowMatchedResult_Load(object sender, EventArgs e)
        {

            //加载地图到MapControl

            //if (m_InRowDic.Count > 0 && m_OutRowDic.Count > 0)
            //{
            //    m_MapControlFrom.ClearLayers();
            //    m_MapControlOverlap.ClearLayers();
            //    m_MapControlTo.ClearLayers();
            //    for (int i = 0; i < m_InRowDic.Count; i++)
            //    {
            //        string name = m_InRowDic[i].Cells[1].Value.ToString();
            //        string path = m_InRowDic[i].Cells[3].Value.ToString();

            //        m_MapControlFrom.AddShapeFile(path, name);
            //        m_MapControlOverlap.AddShapeFile(path, name);
            //    }
            //    for (int i = 0; i < m_OutRowDic.Count; i++)
            //    {
            //        string name = m_OutRowDic[i].Cells[1].Value.ToString();
            //        string path = m_OutRowDic[i].Cells[3].Value.ToString();
            //        m_MapControlTo.AddShapeFile(path, name);
            //        m_MapControlOverlap.AddShapeFile(path, name);
            //    }
            //}
            AddITableToDGV(m_Table, null);

            //启动标题自动排序功能
            for (int i = 0; i < dataGridViewX1.ColumnCount; i++)
            {
                this.dataGridViewX1.Columns[i].SortMode = DataGridViewColumnSortMode.Automatic;
            }

            if (m_TabControl.Tabs[0].Visible)
                m_TabControl.Tabs[0].Visible = false;
            if (!m_TabControl.Tabs[1].Visible)
                m_TabControl.Tabs[1].Visible = true;
            if (!m_TabControl.Tabs[2].Visible)
                m_TabControl.Tabs[2].Visible = true;
        }


        /// <summary>
        /// 把查询结果表添加到datagridview中
        /// </summary>
        /// <param name="table">查询结果表</param>
        /// <param name="queryFilter">查询条件</param>
        public void AddITableToDGV(ITable table, IQueryFilter queryFilter)
        {
            this.dataGridViewX1.Rows.Clear();

            if (queryFilter == null)
            {
                m_ClsTabWrapper = new ClsTableWrapper(table);

                this.bindingSource1.DataSource = m_ClsTabWrapper;
                this.dataGridViewX1.DataSource = this.bindingSource1;

                m_bAll = true;

                //设置BackColor
                if (this.dataGridViewX1.Columns["综合相似度"] != null)
                {
                    IRgbColor pRgbColor = new RgbColorClass();
                    pRgbColor.Red = 194;
                    pRgbColor.Blue = 217;
                    pRgbColor.Green = 247;
                    //pSimpleFillSymbol1.Color = pRgbColor;
                    dataGridViewX1.Columns["综合相似度"].DefaultCellStyle.BackColor = ColorTranslator.FromOle(pRgbColor.RGB);
                }

                //为空行设置背景颜色为红色
                int NullRow = 0;
                for (NullRow = 0; NullRow < dataGridViewX1.RowCount; NullRow++)
                {
                    string fromoids = this.dataGridViewX1.Rows[NullRow].Cells[1].Value.ToString();
                    string tooids = this.dataGridViewX1.Rows[NullRow].Cells[2].Value.ToString();
                    if (fromoids == "" && tooids == "")
                    {
                        this.dataGridViewX1.Rows[NullRow].DefaultCellStyle.BackColor = Color.Red;
                        break;
                    }

                }


            }
            else
            {
                if (table.RowCount(queryFilter) > 0)
                {
                    m_ClsTabWrapper = new ClsTableWrapper(table.Fields);
                    ICursor pCursor = table.Search(queryFilter, false);
                    IRow pRow = pCursor.NextRow();
                    while (pRow != null)
                    {
                        m_ClsTabWrapper.Add(pRow);
                        pRow = pCursor.NextRow();
                    }
                    this.bindingSource1.DataSource = m_ClsTabWrapper;
                    this.dataGridViewX1.DataSource = this.bindingSource1;
                    this.dataGridViewX1.Columns[0].Width = 55;
                }
            }
        }
        /// <summary>
        /// 全部
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAll_Click(object sender, EventArgs e)
        {
            if (m_Table == null)
                return;
            if (!m_bAll)
            {
                this.dataGridViewX1.Rows.Clear();
                AddITableToDGV(m_Table, null);

                m_bAll = true;
                m_bMatched = false;
                m_bAttriAndShape = false;
                m_bAttribute = false;
                m_bShape = false;
                m_bNewFeat = false;
                m_bOneToMore = false;

                m_bMoreToOne = false;
                m_bOneToOne = false;
                m_bMoreToMore = false;
                m_bNew = false;
            }
        }
        /// <summary>
        /// 未变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMatched_Click(object sender, EventArgs e)
        {
            if (m_Table == null)
                return;
            if (!m_bMatched)
            {
                this.dataGridViewX1.Rows.Clear();
                IQueryFilter queryFilter = new QueryFilterClass();
                queryFilter.WhereClause = "变化标记='未变化'";
                AddITableToDGV(m_Table, queryFilter);

                m_bAll = false;
                m_bMatched = true;
                m_bAttriAndShape = false;
                m_bAttribute = false;
                m_bShape = false;
                m_bNewFeat = false;
                m_bOneToMore = false;

                m_bMoreToOne = false;
                m_bOneToOne = false;
                m_bMoreToMore = false;
                m_bNew = false;
            }
        }
        /// <summary>
        /// 属性变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAttribute_Click(object sender, EventArgs e)
        {
            if (m_Table == null)
                return;
            if (!m_bAttribute)
            {
                this.dataGridViewX1.Rows.Clear();
                IQueryFilter queryFilter = new QueryFilterClass();
                queryFilter.WhereClause = "变化标记='属性变化'";
                AddITableToDGV(m_Table, queryFilter);

                m_bAll = false;
                m_bMatched = false;
                m_bAttriAndShape = false;
                m_bAttribute = true;
                m_bShape = false;
                m_bNewFeat = false;
                m_bOneToMore = false;

                m_bMoreToOne = false;
                m_bOneToOne = false;
                m_bMoreToMore = false;
                m_bNew = false;
            }
        }
        /// <summary>
        /// 图形变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnShape_Click(object sender, EventArgs e)
        {
            if (m_Table == null)
                return;
            if (!m_bShape)
            {
                this.dataGridViewX1.Rows.Clear();
                IQueryFilter queryFilter = new QueryFilterClass();
                queryFilter.WhereClause = "变化标记='图形变化'";
                AddITableToDGV(m_Table, queryFilter);

                m_bAll = false;
                m_bMatched = false;
                m_bAttriAndShape = false;
                m_bAttribute = false;
                m_bShape = true;
                m_bNewFeat = false;
                m_bOneToMore = false;

                m_bMoreToOne = false;
                m_bOneToOne = false;
                m_bMoreToMore = false;
                m_bNew = false;
            }
        }
        /// <summary>
        /// 属性图形变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnShapeAttribute_Click(object sender, EventArgs e)
        {
            if (m_Table == null)
                return;
            if (!m_bAttriAndShape)
            {
                this.dataGridViewX1.Rows.Clear();
                IQueryFilter queryFilter = new QueryFilterClass();
                queryFilter.WhereClause = "变化标记='属性图形变化'";
                AddITableToDGV(m_Table, queryFilter);

                m_bAll = false;
                m_bMatched = false;
                m_bAttriAndShape = true;
                m_bAttribute = false;
                m_bShape = false;
                m_bNewFeat = false;
                m_bOneToMore = false;

                m_bMoreToOne = false;
                m_bOneToOne = false;
                m_bMoreToMore = false;
                m_bNew = false;
            }
        }
        /// <summary>
        /// 一对多关系
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOneToMore_Click(object sender, EventArgs e)
        {
            if (m_Table == null)
                return;
            if (!m_bOneToMore)
            {
                this.dataGridViewX1.Rows.Clear();
                IQueryFilter queryFilter = new QueryFilterClass();
                //queryFilter.WhereClause = "变化标记='一对多'";
                queryFilter.WhereClause = "变化标记="+"'"+ClsConstant.One2More+"'";
                AddITableToDGV(m_Table, queryFilter);

                m_bAll = false;
                m_bMatched = false;
                m_bAttriAndShape = true;
                m_bAttribute = false;
                m_bShape = false;
                m_bNewFeat = false;
                m_bAttriAndShape = false;

                m_bMoreToOne = false;
                m_bOneToOne = false;
                m_bMoreToMore = false;
                m_bNew = false;
            }
        }
        /// <summary>
        /// 跨尺度（多对一）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMoreToOne_Click(object sender, EventArgs e)
        {
            if (m_Table == null)
                return;
            if (!m_bMoreToOne)
            {
                this.dataGridViewX1.Rows.Clear();
                IQueryFilter queryFilter = new QueryFilterClass();
                //queryFilter.WhereClause = "变化标记='多对一'";
                queryFilter.WhereClause = "变化标记=" + "'" + ClsConstant.More2One + "'";

                AddITableToDGV(m_Table, queryFilter);

                m_bMoreToMore = false;
                m_bOneToOne = false;
                m_bNew = false;

                m_bAll = false;
                m_bMatched = false;
                m_bAttribute = false;
                m_bShape = false;
                m_bAttriAndShape = false;
                m_bNewFeat = false;
                m_bOneToMore = false;
            }
        }
        /// <summary>
        /// 跨尺度（一对一）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOneToOne_Click(object sender, EventArgs e)
        {
            if (m_Table == null)
                return;
            if (!m_bOneToOne)
            {
                this.dataGridViewX1.Rows.Clear();
                IQueryFilter queryFilter = new QueryFilterClass();
                //queryFilter.WhereClause = "变化标记='一对一'";
                queryFilter.WhereClause = "变化标记=" + "'" + ClsConstant.One2One + "'";

                AddITableToDGV(m_Table, queryFilter);

                m_bMoreToMore = false;
                m_bMoreToOne = false;
                m_bNew = false;

                m_bAll = false;
                m_bMatched = false;
                m_bAttribute = false;
                m_bShape = false;
                m_bAttriAndShape = false;
                m_bNewFeat = false;
                m_bOneToMore = false;
            }
        }
        /// <summary>
        /// 跨尺度（新增要素）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNew_Click(object sender, EventArgs e)
        {
            if (m_Table == null)
                return;
            if (!m_bNew)
            {
                this.dataGridViewX1.Rows.Clear();
                IQueryFilter queryFilter = new QueryFilterClass();
                //queryFilter.WhereClause = "变化标记='新增要素'";
                queryFilter.WhereClause = "变化标记=" + "'" + ClsConstant.One2Zero + "'";
                AddITableToDGV(m_Table, queryFilter);

                m_bOneToOne = false;
                m_bMoreToOne = false;
                m_bMoreToMore = false;

                m_bAll = false;
                m_bMatched = false;
                m_bAttribute = false;
                m_bShape = false;
                m_bAttriAndShape = false;
                m_bNewFeat = false;
                m_bOneToMore = false;
            }
        }

        /// <summary>
        /// 跨尺度（多对多）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMoreToMore_Click(object sender, EventArgs e)
        {
            if (m_Table == null)
                return;
            if (!m_bMoreToMore)
            {
                this.dataGridViewX1.Rows.Clear();
                IQueryFilter queryFilter = new QueryFilterClass();
                //queryFilter.WhereClause = "变化标记='多对多'";
                queryFilter.WhereClause = "变化标记=" + "'" + ClsConstant.More2More + "'";

                AddITableToDGV(m_Table, queryFilter);

                m_bOneToOne = false;
                m_bMoreToOne = false;
                m_bNew = false;

                m_bAll = false;
                m_bMatched = false;
                m_bAttribute = false;
                m_bShape = false;
                m_bAttriAndShape = false;
                m_bNewFeat = false;
                m_bOneToMore = false;
            }
        }

        private void dataGridViewX1_MouseUp(object sender, MouseEventArgs e)
        {
            //如果选择某行，则所选要素合并显示
            if (this.dataGridViewX1.SelectedRows.Count > 0)
            {
                Collection<int> fromIDCol = new Collection<int>();
                Collection<int> toIDCol = new Collection<int>();
                int i = 0;
                ILayer fromLayr = null;
                ILayer toLayer = null;

                IGeometry fromGeo = null;
                IGeometry toGeo = null;
                IGeometry tempGeo = null;

                //m_MapControlFrom.ActiveView.FocusMap.ClearSelection();
                //m_MapControlTo.ActiveView.FocusMap.ClearSelection();
                //m_MapControlOverlap.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, null);

                for (i = 0; i < this.dataGridViewX1.SelectedRows.Count; i++)
                {
                    DataGridViewRow dgvRow = new DataGridViewRow();
                    dgvRow = this.dataGridViewX1.SelectedRows[i];
                    string oids = dgvRow.Cells["待匹配OID"].Value.ToString();
                    string fromOIDs = dgvRow.Cells["源OID"].Value.ToString();

                    if (oids != "" && fromOIDs != "")
                    {

                        if (fromOIDs.Contains(";"))
                        {
                            string[] array = fromOIDs.Split(';');
                            for (int m = 0; m < array.Length; m++)
                            {
                                fromIDCol.Add(Convert.ToInt32(array[m]));
                            }
                        }
                        else
                        {
                            fromIDCol.Add(Convert.ToInt32(dgvRow.Cells["源OID"].Value));
                        }

                        if (dgvRow.Cells["待匹配OID"].Value.ToString() != "")
                        {
                            //string oids = dgvRow.Cells["待匹配OID"].Value.ToString();
                            if (oids.Contains(";"))
                            {
                                string[] array = oids.Split(';');
                                for (int n = 0; n < array.Length; n++)
                                {
                                    toIDCol.Add(Convert.ToInt32(array[n]));
                                }

                            }
                            else
                            {
                                toIDCol.Add(Convert.ToInt32(dgvRow.Cells["待匹配OID"].Value));
                            }
                        }
                    }

                }
                if (m_MapControlFrom.ActiveView.FocusMap.LayerCount > 0)
                {
                    fromLayr = m_MapControlFrom.ActiveView.FocusMap.get_Layer(0);
                }

                if (m_MapControlTo.ActiveView.FocusMap.LayerCount > 0)
                {
                    toLayer = m_MapControlTo.ActiveView.FocusMap.get_Layer(0);
                }
                IFeatureSelection fromSelection = fromLayr as IFeatureSelection;
                IFeatureSelection toSelection = toLayer as IFeatureSelection;
                fromSelection.Clear();
                toSelection.Clear();
                m_MapControlFrom.ActiveView.Refresh();
                m_MapControlTo.ActiveView.Refresh();
                //m_MapControlFrom.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, fromLayr, null);
                //m_MapControlTo.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, toLayer, null);

                if (fromIDCol.Count != 0)
                {


                    ITopologicalOperator fromTop = m_TUFeatCls.GetFeature(fromIDCol[0]).Shape as ITopologicalOperator;

                    for (i = 0; i < fromIDCol.Count; i++)
                    {
                        IFeature feature = m_TUFeatCls.GetFeature(fromIDCol[i]);
                        m_MapControlFrom.ActiveView.FocusMap.SelectFeature(fromLayr, feature);

                        tempGeo = feature.Shape as IGeometry;
                        fromGeo = fromTop.Union(tempGeo);
                        fromTop = fromGeo as ITopologicalOperator;

                    }

                    if (toIDCol.Count > 0)
                    {

                        //IQueryFilter queryFilter;
                        if (toIDCol.Count == 1)
                        {
                            IFeature feature = m_TEFeatCls.GetFeature(toIDCol[0]);
                            //queryFilter = new QueryFilterClass();
                            //queryFilter.WhereClause = "OBJECTID=" + toIDCol[0].ToString();
                            //IFeatureCursor featureCursor = m_TEFeatCls.Search(queryFilter, false);
                            //IFeature feature = featureCursor.NextFeature();
                            m_MapControlTo.ActiveView.FocusMap.SelectFeature(toLayer, feature);
                            toGeo = feature.Shape as IGeometry;
                        }
                        else
                        {
                            //queryFilter = new QueryFilterClass();
                            //queryFilter.WhereClause = "OBJECTID=" + toIDCol[0].ToString();
                            //IFeatureCursor featureCursor = m_TEFeatCls.Search(queryFilter, false);
                            //IFeature feature = featureCursor.NextFeature();
                            IFeature feature = m_TEFeatCls.GetFeature(toIDCol[0]);

                            m_MapControlTo.ActiveView.FocusMap.SelectFeature(toLayer, feature);
                            ITopologicalOperator toTop = feature.Shape as ITopologicalOperator;

                            for (i = 1; i < toIDCol.Count; i++)
                            {
                                //queryFilter = new QueryFilterClass();
                                //queryFilter.WhereClause = "OBJECTID=" + toIDCol[i].ToString();
                                //IFeatureCursor featureCursor2 = m_TEFeatCls.Search(queryFilter, false);
                                //feature = featureCursor2.NextFeature();
                                feature = m_TEFeatCls.GetFeature(toIDCol[i]);
                                m_MapControlTo.ActiveView.FocusMap.SelectFeature(toLayer, feature);

                                tempGeo = feature.Shape as IGeometry;
                                toGeo = toTop.Union(tempGeo);
                                toTop = toGeo as ITopologicalOperator;
                            }
                        }
                    }

                    IEnvelope fromEnv = new EnvelopeClass();
                    fromEnv = fromGeo.Envelope;

                    fromEnv.Expand(2, 2, true);
                    m_MapControlFrom.ActiveView.Extent = fromEnv;

                    if (toGeo != null)
                    {
                        IEnvelope toEnv = new EnvelopeClass();
                        toEnv = toGeo.Envelope;
                        toEnv.Expand(2, 2, true);
                        m_MapControlTo.ActiveView.Extent = toEnv;
                    }
                    else
                    {
                        fromEnv.Expand(2, 2, true);
                        m_MapControlTo.ActiveView.Extent = fromEnv;

                    }
                    m_MapControlOverlap.ActiveView.Extent = fromEnv;

                    m_MapControlFrom.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, fromLayr, null);
                    m_MapControlTo.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, toLayer, null);
                    //m_MapControlOverlap.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, null);
                    m_MapControlOverlap.ActiveView.Refresh();
                }
            }

        }
        /// <summary>
        /// datagridview标题单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewX1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int test = this.dataGridViewX1.Columns.Count;
            string mtableName = m_TableName;

            //多边形同尺度匹配表
            #region 20170913 comment
            //if (this.dataGridViewX1.SelectedRows.Count == 1 && this.dataGridViewX1.Columns.Count == 10 ||
            //    this.dataGridViewX1.SelectedRows.Count == 1 && this.dataGridViewX1.Columns.Count == 7)
            //{
            //    dgvFrom.Rows.Clear();
            //    dgvTo.Rows.Clear();

            //    int i = 0;
            //    int j = 0;

            //    string fromoids = this.dataGridViewX1.SelectedRows[0].Cells[1].Value.ToString();
            //    string tooids = this.dataGridViewX1.SelectedRows[0].Cells[2].Value.ToString();

            //    string[] fromarray = fromoids.Split(';');
            //    string[] toarray = tooids.Split(';');

            //    IFeatureLayer fromFeatLyr = m_MapControlFrom.get_Layer(0) as IFeatureLayer;
            //    IFeatureClass fromFeatCls = fromFeatLyr.FeatureClass;



            //    if (fromarray[0] != "")
            //    {
            //        IFeature fromFeature = fromFeatCls.GetFeature(Convert.ToInt32(fromarray[0]));
            //        if (dgvFrom.Columns.Count == 0)
            //        {
            //            int fromColumn = fromFeature.Fields.FieldCount - 1;
            //            int fromCount = 0;
            //            for (i = 0; i < fromFeature.Fields.FieldCount; i++)
            //            {
            //                if (fromFeature.Fields.get_Field(i).Type != esriFieldType.esriFieldTypeGeometry)
            //                {
            //                    dgvFrom.Columns.Add(fromFeature.Fields.get_Field(i).Name, fromFeature.Fields.get_Field(i).AliasName);
            //                    dgvFrom.Columns[fromCount].ReadOnly = true;
            //                    dgvFrom.Columns[fromCount].Width = dgvFrom.Width / fromColumn;
            //                    fromCount++;
            //                }
            //            }
            //        }

            //        for (int m = 0; m < fromarray.Length; m++)
            //        {
            //            DataGridViewRow dgvFromRow = new DataGridViewRow();
            //            dgvFromRow = dgvFrom.Rows[dgvFrom.Rows.Add()];
            //            fromFeature = fromFeatCls.GetFeature(Convert.ToInt32(fromarray[m]));

            //            for (i = 0; i < dgvFrom.Columns.Count; i++)
            //            {
            //                for (j = 0; j < fromFeature.Fields.FieldCount; j++)
            //                {
            //                    if (dgvFrom.Columns[i].Name == fromFeature.Fields.get_Field(j).Name)
            //                    {
            //                        dgvFromRow.Cells[i].Value = fromFeature.get_Value(j);
            //                        break;
            //                    }
            //                }
            //            }
            //        }
            //    }
            //    IFeatureLayer toFeatLyr = m_MapControlTo.get_Layer(0) as IFeatureLayer;
            //    IFeatureClass toFeatCls = toFeatLyr.FeatureClass;
            //    //IQueryFilter queryFilter = new QueryFilterClass();


            //    if (toarray[0] != "")
            //    {
            //        //queryFilter.WhereClause = "OBJECTID=" + toarray[0];
            //        //featureCursor = toFeatCls.Search(queryFilter,false);
            //        //IFeature toFeature = featureCursor.NextFeature();
            //        IFeature toFeature = toFeatCls.GetFeature(Convert.ToInt32(toarray[0]));
            //        if (dgvTo.Columns.Count == 0)
            //        {
            //            int toColumn = toFeature.Fields.FieldCount - 1;
            //            int toCount = 0;
            //            for (i = 0; i < toFeature.Fields.FieldCount; i++)
            //            {
            //                if (toFeature.Fields.get_Field(i).Type != esriFieldType.esriFieldTypeGeometry)
            //                {
            //                    dgvTo.Columns.Add(toFeature.Fields.get_Field(i).Name, toFeature.Fields.get_Field(i).AliasName);
            //                    dgvTo.Columns[toCount].ReadOnly = true;
            //                    dgvTo.Columns[toCount].Width = dgvTo.Width / toColumn;
            //                    toCount++;
            //                }
            //            }
            //        }

            //        for (int n = 0; n < toarray.Length; n++)
            //        {
            //            DataGridViewRow dgvToRow = new DataGridViewRow();
            //            dgvToRow = dgvTo.Rows[dgvTo.Rows.Add()];

            //            //queryFilter.WhereClause = "OBJECTID=" + toarray[n];
            //            //featureCursor = toFeatCls.Search(queryFilter, false);
            //            toFeature = toFeatCls.GetFeature(Convert.ToInt32(toarray[n]));

            //            for (i = 0; i < dgvTo.Columns.Count; i++)
            //            {
            //                for (j = 0; j < toFeature.Fields.FieldCount; j++)
            //                {
            //                    if (dgvTo.Columns[i].Name == toFeature.Fields.get_Field(j).Name)
            //                    {
            //                        dgvToRow.Cells[i].Value = toFeature.get_Value(j);
            //                        break;
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
            ////不同尺度多边形匹配
            //else
            #endregion
            //if (this.dataGridViewX1.SelectedRows.Count == 1 && this.dataGridViewX1.Columns.Count == 6)
            if (m_TableName.Contains("_DifPyTable") || m_TableName.Contains("_py") || m_TableName.Contains("_Py") || m_TableName.Contains("_pY"))
            {

                //dgvFrom.Rows.Clear();
                //dgvTo.Rows.Clear();

                IFeatureLayer fromFeatLyr = m_MapControlFrom.get_Layer(0) as IFeatureLayer;
                IFeatureClass fromFeatCls = fromFeatLyr.FeatureClass;

                IFeatureLayer toFeatLyr = m_MapControlTo.get_Layer(0) as IFeatureLayer;
                IFeatureClass toFeatCls = toFeatLyr.FeatureClass;

                IRow rowfrom = null;
                IRow rowto = null;

                int i = 0;
                int j = 0;

                //if (this.dataGridViewX1.SelectedRows[0].Cells["变化标记"].Value.ToString() == "新增要素")
                    if (this.dataGridViewX1.SelectedRows[0].Cells["变化标记"].Value.ToString() == ClsConstant.One2Zero)
                {
                    int fromOID = Convert.ToInt32(this.dataGridViewX1.SelectedRows[0].Cells["源OID"].Value.ToString().Trim());
                    IFeature fromFeature = fromFeatCls.GetFeature(fromOID);
                    rowfrom = fromFeature as IRow;

                    two.LoadData(rowfrom, null);
                    two.Show();

                    #region 20171003注释掉
                    ////创建from列
                    //if (dgvFrom.Columns.Count == 0)
                    //{
                    //    int fromColumn = fromFeature.Fields.FieldCount - 1;
                    //    int fromCount = 0;
                    //    for (i = 0; i < fromFeature.Fields.FieldCount; i++)
                    //    {
                    //        if (fromFeature.Fields.get_Field(i).Type != esriFieldType.esriFieldTypeGeometry)
                    //        {
                    //            //dgvFrom.Columns.Add(fromFeature.Fields.get_Field(i).Name, fromFeature.Fields.get_Field(i).AliasName);
                    //            dgvFrom.Columns.Add(fromFeature.Fields.get_Field(i).Name, fromFeature.Fields.get_Field(i).Name);
                    //            dgvFrom.Columns[fromCount].ReadOnly = true;
                    //            dgvFrom.Columns[fromCount].Width = dgvFrom.Width / fromColumn;
                    //            fromCount++;
                    //        }
                    //    }
                    //}
                    ////填充DataGridView
                    //DataGridViewRow dgvFromRow = new DataGridViewRow();
                    //dgvFromRow = dgvFrom.Rows[dgvFrom.Rows.Add()];
                    //for (i = 0; i < dgvFrom.Columns.Count; i++)
                    //{
                    //    for (j = 0; j < fromFeature.Fields.FieldCount; j++)
                    //    {
                    //        if (dgvFrom.Columns[i].Name == fromFeature.Fields.get_Field(j).Name)
                    //        {
                    //            dgvFromRow.Cells[i].Value = fromFeature.get_Value(j);
                    //            break;
                    //        }
                    //    }
                    //}
                    #endregion

                }
                //else if (this.dataGridViewX1.SelectedRows[0].Cells["变化标记"].Value.ToString() == "一对一")
                else if (this.dataGridViewX1.SelectedRows[0].Cells["变化标记"].Value.ToString() == ClsConstant.One2One)
                {
                    int fromOID = Convert.ToInt32(this.dataGridViewX1.SelectedRows[0].Cells["源OID"].Value.ToString().Trim());
                    IFeature fromFeature = fromFeatCls.GetFeature(fromOID);
                    rowfrom = fromFeature as IRow;

                    #region 20171003注释掉
                    ////创建from列
                    //if (dgvFrom.Columns.Count == 0)
                    //{
                    //    int fromColumn = fromFeature.Fields.FieldCount - 1;
                    //    int fromCount = 0;
                    //    for (i = 0; i < fromFeature.Fields.FieldCount; i++)
                    //    {
                    //        if (fromFeature.Fields.get_Field(i).Type != esriFieldType.esriFieldTypeGeometry)
                    //        {
                    //            //dgvFrom.Columns.Add(fromFeature.Fields.get_Field(i).Name, fromFeature.Fields.get_Field(i).AliasName);
                    //            dgvFrom.Columns.Add(fromFeature.Fields.get_Field(i).Name, fromFeature.Fields.get_Field(i).Name);
                    //            dgvFrom.Columns[fromCount].ReadOnly = true;
                    //            dgvFrom.Columns[fromCount].Width = dgvFrom.Width / fromColumn;
                    //            fromCount++;
                    //        }
                    //    }
                    //}

                    //DataGridViewRow dgvFromRow = new DataGridViewRow();
                    //dgvFromRow = dgvFrom.Rows[dgvFrom.Rows.Add()];
                    //for (i = 0; i < dgvFrom.Columns.Count; i++)
                    //{
                    //    for (j = 0; j < fromFeature.Fields.FieldCount; j++)
                    //    {
                    //        if (dgvFrom.Columns[i].Name == fromFeature.Fields.get_Field(j).Name)
                    //        {
                    //            dgvFromRow.Cells[i].Value = fromFeature.get_Value(j);
                    //            break;
                    //        }
                    //    }
                    //}
                    #endregion

                    //待匹配图层
                    string toOID = this.dataGridViewX1.SelectedRows[0].Cells["待匹配OID"].Value.ToString().Trim();
                    //IQueryFilter queryFilter = new QueryFilterClass();
                    //IFeatureCursor featureCursor = null;
                    //queryFilter.WhereClause = "FIXOID=" + toOID;
                    //featureCursor = toFeatCls.Search(queryFilter, false);
                    IFeature toFeature = toFeatCls.GetFeature(Convert.ToInt32(toOID));//得到得更新图层选中要素
                    rowto = toFeature as IRow;


                    two.LoadData(rowfrom, rowto);
                    two.Show();

                    #region 20171003注释掉
                    //if (dgvTo.Columns.Count == 0)
                    //{
                    //    int toColumn = toFeature.Fields.FieldCount - 1;
                    //    int toCount = 0;
                    //    for (i = 0; i < toFeature.Fields.FieldCount; i++)
                    //    {
                    //        if (toFeature.Fields.get_Field(i).Type != esriFieldType.esriFieldTypeGeometry)
                    //        {
                    //            //dgvTo.Columns.Add(toFeature.Fields.get_Field(i).Name, toFeature.Fields.get_Field(i).AliasName);
                    //            dgvTo.Columns.Add(toFeature.Fields.get_Field(i).Name, toFeature.Fields.get_Field(i).Name);
                    //            dgvTo.Columns[toCount].ReadOnly = true;
                    //            dgvTo.Columns[toCount].Width = dgvTo.Width / toColumn;
                    //            toCount++;
                    //        }
                    //    }
                    //}

                    //DataGridViewRow dgvToRow = new DataGridViewRow();
                    //dgvToRow = dgvTo.Rows[dgvTo.Rows.Add()];
                    //for (i = 0; i < dgvTo.Columns.Count; i++)
                    //{
                    //    for (j = 0; j < toFeature.Fields.FieldCount; j++)
                    //    {
                    //        if (dgvTo.Columns[i].Name == toFeature.Fields.get_Field(j).Name)
                    //        {
                    //            dgvToRow.Cells[i].Value = toFeature.get_Value(j);
                    //            break;
                    //        }
                    //    }
                    //}
                    #endregion

                }
                //else if (this.dataGridViewX1.SelectedRows[0].Cells["变化标记"].Value.ToString() == "多对一")
                else if (this.dataGridViewX1.SelectedRows[0].Cells["变化标记"].Value.ToString() == ClsConstant.More2One)
                {
                    //待匹配OID字段值
                    string toOID = this.dataGridViewX1.SelectedRows[0].Cells["待匹配OID"].Value.ToString().Trim();


                    //源图层字段
                    List<IRow> list = new List<IRow>();

                    int Increase = e.RowIndex;
                    int Decrease = e.RowIndex - 1;

                    if (Decrease > -1)
                    {
                        //从当前行的前一行往前推导
                        while (this.dataGridViewX1.Rows[Decrease].Cells["待匹配OID"].Value.ToString() == toOID)
                        {
                            //此行的前一行也要选中
                            this.dataGridViewX1.Rows[Decrease].Selected = true;

                            int fromOID = Convert.ToInt32(this.dataGridViewX1.Rows[Decrease].Cells["源OID"].Value.ToString().Trim());
                            IFeature fromFeature = fromFeatCls.GetFeature(fromOID);
                            rowfrom = fromFeature as IRow;
                            list.Add(rowfrom);
                            rowfrom = null;
                            #region 20171003注释掉
                            ////创建from列
                            //if (dgvFrom.Columns.Count == 0)
                            //{
                            //    int fromColumn = fromFeature.Fields.FieldCount - 1;
                            //    int fromCount = 0;
                            //    for (i = 0; i < fromFeature.Fields.FieldCount; i++)
                            //    {
                            //        if (fromFeature.Fields.get_Field(i).Type != esriFieldType.esriFieldTypeGeometry)
                            //        {
                            //            //dgvFrom.Columns.Add(fromFeature.Fields.get_Field(i).Name, fromFeature.Fields.get_Field(i).AliasName);
                            //            dgvFrom.Columns.Add(fromFeature.Fields.get_Field(i).Name, fromFeature.Fields.get_Field(i).Name);
                            //            dgvFrom.Columns[fromCount].ReadOnly = true;
                            //            dgvFrom.Columns[fromCount].Width = dgvFrom.Width / fromColumn;
                            //            fromCount++;
                            //        }
                            //    }
                            //}
                            ////填充字段
                            //DataGridViewRow dgvFromRow = new DataGridViewRow();
                            //dgvFromRow = dgvFrom.Rows[dgvFrom.Rows.Add()];
                            //for (i = 0; i < dgvFrom.Columns.Count; i++)
                            //{
                            //    for (j = 0; j < fromFeature.Fields.FieldCount; j++)
                            //    {
                            //        if (dgvFrom.Columns[i].Name == fromFeature.Fields.get_Field(j).Name)
                            //        {
                            //            dgvFromRow.Cells[i].Value = fromFeature.get_Value(j);
                            //            break;
                            //        }
                            //    }
                            //}
                            #endregion
                            //继续比较前面的一行
                            Decrease--;
                            if (Decrease <= -1)
                            {
                                break;
                            }
                        }
                    }
                    //从当前行往后推导
                    while (this.dataGridViewX1.Rows[Increase].Cells["待匹配OID"].Value.ToString() == toOID)
                    {
                        this.dataGridViewX1.Rows[Increase].Selected = true;

                        int fromOID = Convert.ToInt32(this.dataGridViewX1.Rows[Increase].Cells["源OID"].Value.ToString().Trim());
                        IFeature fromFeature = fromFeatCls.GetFeature(fromOID);

                        rowfrom = fromFeature as IRow;
                        list.Add(rowfrom);
                        rowfrom = null;
                        #region 20171003注释掉
                        ////创建from列
                        //if (dgvFrom.Columns.Count == 0)
                        //{
                        //    int fromColumn = fromFeature.Fields.FieldCount - 1;
                        //    int fromCount = 0;
                        //    for (i = 0; i < fromFeature.Fields.FieldCount; i++)
                        //    {
                        //        if (fromFeature.Fields.get_Field(i).Type != esriFieldType.esriFieldTypeGeometry)
                        //        {
                        //            //dgvFrom.Columns.Add(fromFeature.Fields.get_Field(i).Name, fromFeature.Fields.get_Field(i).AliasName);
                        //            dgvFrom.Columns.Add(fromFeature.Fields.get_Field(i).Name, fromFeature.Fields.get_Field(i).Name);
                        //            dgvFrom.Columns[fromCount].ReadOnly = true;
                        //            dgvFrom.Columns[fromCount].Width = dgvFrom.Width / fromColumn;
                        //            fromCount++;
                        //        }
                        //    }
                        //}
                        ////填充此行
                        //DataGridViewRow dgvFromRow = new DataGridViewRow();
                        //dgvFromRow = dgvFrom.Rows[dgvFrom.Rows.Add()];
                        //for (i = 0; i < dgvFrom.Columns.Count; i++)
                        //{
                        //    for (j = 0; j < fromFeature.Fields.FieldCount; j++)
                        //    {
                        //        if (dgvFrom.Columns[i].Name == fromFeature.Fields.get_Field(j).Name)
                        //        {
                        //            dgvFromRow.Cells[i].Value = fromFeature.get_Value(j);
                        //            break;
                        //        }
                        //    }
                        //}
                        #endregion
                        //往后推导
                        Increase++;
                        if (Increase >= this.dataGridViewX1.Rows.Count)
                        {
                            break;
                        }
                    }


                    //待匹配字段

                    //IQueryFilter queryFilter = new QueryFilterClass();
                    //IFeatureCursor featureCursor = null;
                    //queryFilter.WhereClause = "FIXOID=" + toOID;
                    //featureCursor = toFeatCls.Search(queryFilter, false);
                    //IFeature toFeature = featureCursor.NextFeature();//得到得更新图层选中要素
                    IFeature toFeature = toFeatCls.GetFeature(Convert.ToInt32(toOID));
                    rowto = toFeature as IRow;

                    two.LoadData(list, rowto);
                    two.Show();

                    #region 20171003注释掉
                    ////创建待匹配字段
                    //if (dgvTo.Columns.Count == 0)
                    //{
                    //    int toColumn = toFeature.Fields.FieldCount - 1;
                    //    int toCount = 0;
                    //    for (i = 0; i < toFeature.Fields.FieldCount; i++)
                    //    {
                    //        if (toFeature.Fields.get_Field(i).Type != esriFieldType.esriFieldTypeGeometry)
                    //        {
                    //            //dgvTo.Columns.Add(toFeature.Fields.get_Field(i).Name, toFeature.Fields.get_Field(i).AliasName);
                    //            dgvTo.Columns.Add(toFeature.Fields.get_Field(i).Name, toFeature.Fields.get_Field(i).Name);
                    //            dgvTo.Columns[toCount].ReadOnly = true;
                    //            dgvTo.Columns[toCount].Width = dgvTo.Width / toColumn;
                    //            toCount++;
                    //        }
                    //    }
                    //}
                    ////待匹配字段填充
                    //DataGridViewRow dgvToRow = new DataGridViewRow();
                    //dgvToRow = dgvTo.Rows[dgvTo.Rows.Add()];
                    //for (i = 0; i < dgvTo.Columns.Count; i++)
                    //{
                    //    for (j = 0; j < toFeature.Fields.FieldCount; j++)
                    //    {
                    //        if (dgvTo.Columns[i].Name == toFeature.Fields.get_Field(j).Name)
                    //        {
                    //            dgvToRow.Cells[i].Value = toFeature.get_Value(j);
                    //            break;
                    //        }
                    //    }
                    //}
                    #endregion
                }
            }
            //不同尺度线匹配
            //else if (this.dataGridViewX1.SelectedRows.Count == 1 && this.dataGridViewX1.Columns.Count == 9)
            else if (m_TableName.Contains("_DifLnTable") || m_TableName.Contains("_ln") || m_TableName.Contains("_Ln") || m_TableName.Contains("_lN"))
            {
                //dgvFrom.Rows.Clear();
                //dgvTo.Rows.Clear();

                int i = 0;
                int j = 0;

                string fromoids = this.dataGridViewX1.SelectedRows[0].Cells[1].Value.ToString();
                string tooids = this.dataGridViewX1.SelectedRows[0].Cells[2].Value.ToString();

                string[] fromarray = fromoids.Split(';');
                string[] toarray = tooids.Split(';');


                IRow rowfrom = null;
                IRow rowto = null;

                //源图层
                IFeatureLayer fromFeatLyr = m_MapControlFrom.get_Layer(0) as IFeatureLayer;
                IFeatureClass fromFeatCls = fromFeatLyr.FeatureClass;

                if (fromarray[0] != "")
                {
                    IFeature fromFeature = fromFeatCls.GetFeature(Convert.ToInt32(fromarray[0]));

                    rowfrom = fromFeature as IRow;

                    #region 20171003注释掉
                    ////填充字段
                    //if (dgvFrom.Columns.Count == 0)
                    //{
                    //    int fromColumn = fromFeature.Fields.FieldCount - 1;
                    //    int fromCount = 0;
                    //    for (i = 0; i < fromFeature.Fields.FieldCount; i++)
                    //    {
                    //        if (fromFeature.Fields.get_Field(i).Type != esriFieldType.esriFieldTypeGeometry)
                    //        {
                    //            //dgvFrom.Columns.Add(fromFeature.Fields.get_Field(i).Name, fromFeature.Fields.get_Field(i).AliasName);
                    //            dgvFrom.Columns.Add(fromFeature.Fields.get_Field(i).Name, fromFeature.Fields.get_Field(i).Name);
                    //            dgvFrom.Columns[fromCount].ReadOnly = true;
                    //            dgvFrom.Columns[fromCount].Width = dgvFrom.Width / fromColumn;
                    //            fromCount++;
                    //        }
                    //    }
                    //}
                    ////填充字段值
                    //for (int m = 0; m < fromarray.Length; m++)
                    //{
                    //    DataGridViewRow dgvFromRow = new DataGridViewRow();
                    //    dgvFromRow = dgvFrom.Rows[dgvFrom.Rows.Add()];
                    //    fromFeature = fromFeatCls.GetFeature(Convert.ToInt32(fromarray[m]));

                    //    for (i = 0; i < dgvFrom.Columns.Count; i++)
                    //    {
                    //        for (j = 0; j < fromFeature.Fields.FieldCount; j++)
                    //        {
                    //            if (dgvFrom.Columns[i].Name == fromFeature.Fields.get_Field(j).Name)
                    //            {
                    //                dgvFromRow.Cells[i].Value = fromFeature.get_Value(j);
                    //                break;
                    //            }
                    //        }
                    //    }
                    //}
                    #endregion
                }

                //待匹配图层
                IFeatureLayer toFeatLyr = m_MapControlTo.get_Layer(0) as IFeatureLayer;
                IFeatureClass toFeatCls = toFeatLyr.FeatureClass;
                //IQueryFilter queryFilter = new QueryFilterClass();
                if (toarray[0] != "")
                {
                    //填充字段
                    //queryFilter.WhereClause = "OBJECTID=" + toarray[0];
                    //featureCursor = toFeatCls.Search(queryFilter,false);
                    //IFeature toFeature = featureCursor.NextFeature();
                    IFeature toFeature = toFeatCls.GetFeature(Convert.ToInt32(toarray[0]));
                    rowto = toFeature as IRow;

                    #region 20171003注释掉
                    //if (dgvTo.Columns.Count == 0)
                    //{
                    //    int toColumn = toFeature.Fields.FieldCount - 1;
                    //    int toCount = 0;
                    //    for (i = 0; i < toFeature.Fields.FieldCount; i++)
                    //    {
                    //        if (toFeature.Fields.get_Field(i).Type != esriFieldType.esriFieldTypeGeometry)
                    //        {
                    //            //dgvTo.Columns.Add(toFeature.Fields.get_Field(i).Name, toFeature.Fields.get_Field(i).AliasName);
                    //            dgvTo.Columns.Add(toFeature.Fields.get_Field(i).Name, toFeature.Fields.get_Field(i).Name);
                    //            dgvTo.Columns[toCount].ReadOnly = true;
                    //            dgvTo.Columns[toCount].Width = dgvTo.Width / toColumn;
                    //            toCount++;
                    //        }
                    //    }
                    //}
                    ////填充字段值
                    //for (int n = 0; n < toarray.Length; n++)
                    //{
                    //    DataGridViewRow dgvToRow = new DataGridViewRow();
                    //    dgvToRow = dgvTo.Rows[dgvTo.Rows.Add()];

                    //    //queryFilter.WhereClause = "OBJECTID=" + toarray[n];
                    //    //featureCursor = toFeatCls.Search(queryFilter, false);
                    //    toFeature = toFeatCls.GetFeature(Convert.ToInt32(toarray[n]));

                    //    for (i = 0; i < dgvTo.Columns.Count; i++)
                    //    {
                    //        for (j = 0; j < toFeature.Fields.FieldCount; j++)
                    //        {
                    //            if (dgvTo.Columns[i].Name == toFeature.Fields.get_Field(j).Name)
                    //            {
                    //                dgvToRow.Cells[i].Value = toFeature.get_Value(j);
                    //                break;
                    //            }
                    //        }
                    //    }
                    //}
                    #endregion


                }
                two.LoadData(rowfrom, rowto);
                two.Show();
            }
            //不同尺度点匹配
            else if (m_TableName.Contains("_PtTable") || m_TableName.Contains("_pt") || m_TableName.Contains("_Pt") || m_TableName.Contains("_pT"))
            {
                //dgvFrom.Rows.Clear();
                //dgvTo.Rows.Clear();

                int i = 0;
                int j = 0;

                string fromoids = this.dataGridViewX1.SelectedRows[0].Cells[1].Value.ToString();
                string tooids = this.dataGridViewX1.SelectedRows[0].Cells[2].Value.ToString();

                string[] fromarray = fromoids.Split(';');
                string[] toarray = tooids.Split(';');

                IRow rowfrom = null;
                IRow rowto = null;
                //源图层
                IFeatureLayer fromFeatLyr = m_MapControlFrom.get_Layer(0) as IFeatureLayer;
                IFeatureClass fromFeatCls = fromFeatLyr.FeatureClass;

                if (fromarray[0] != "")
                {
                    IFeature fromFeature = fromFeatCls.GetFeature(Convert.ToInt32(fromarray[0]));

                    rowfrom = fromFeature as IRow;
                    //IRow row = fromFeature as IRow;

                    //FrmItemAttr fatrri = new FrmItemAttr();
                    //fatrri.LoadData(row);
                    //fatrri.Show();

                    #region 20171003注释掉
                    ////填充字段
                    //if (dgvFrom.Columns.Count == 0)
                    //{
                    //    int fromColumn = fromFeature.Fields.FieldCount - 1;
                    //    int fromCount = 0;
                    //    for (i = 0; i < fromFeature.Fields.FieldCount; i++)
                    //    {
                    //        if (fromFeature.Fields.get_Field(i).Type != esriFieldType.esriFieldTypeGeometry)
                    //        {
                    //            //dgvFrom.Columns.Add(fromFeature.Fields.get_Field(i).Name, fromFeature.Fields.get_Field(i).AliasName);
                    //            dgvFrom.Columns.Add(fromFeature.Fields.get_Field(i).Name, fromFeature.Fields.get_Field(i).Name);

                    //            dgvFrom.Columns[fromCount].ReadOnly = true;
                    //            dgvFrom.Columns[fromCount].Width = dgvFrom.Width / fromColumn;
                    //            fromCount++;
                    //        }
                    //    }
                    //}
                    ////填充字段值
                    //for (int m = 0; m < fromarray.Length; m++)
                    //{
                    //    DataGridViewRow dgvFromRow = new DataGridViewRow();
                    //    dgvFromRow = dgvFrom.Rows[dgvFrom.Rows.Add()];
                    //    fromFeature = fromFeatCls.GetFeature(Convert.ToInt32(fromarray[m]));

                    //    for (i = 0; i < dgvFrom.Columns.Count; i++)
                    //    {
                    //        for (j = 0; j < fromFeature.Fields.FieldCount; j++)
                    //        {
                    //            if (dgvFrom.Columns[i].Name == fromFeature.Fields.get_Field(j).Name)
                    //            {
                    //                dgvFromRow.Cells[i].Value = fromFeature.get_Value(j);
                    //                break;
                    //            }
                    //        }
                    //    }
                    //}
                    #endregion


                }


                //待匹配图层
                IFeatureLayer toFeatLyr = m_MapControlTo.get_Layer(0) as IFeatureLayer;
                IFeatureClass toFeatCls = toFeatLyr.FeatureClass;
                //IQueryFilter queryFilter = new QueryFilterClass();
                if (toarray[0] != "")
                {
                    //填充字段
                    //queryFilter.WhereClause = "OBJECTID=" + toarray[0];
                    //featureCursor = toFeatCls.Search(queryFilter,false);
                    //IFeature toFeature = featureCursor.NextFeature();
                    IFeature toFeature = toFeatCls.GetFeature(Convert.ToInt32(toarray[0]));

                    rowto = toFeature as IRow;
                    //IRow row = toFeature as IRow;

                    //FrmItemAttr fatrri = new FrmItemAttr();
                    //fatrri.LoadData(row);
                    //fatrri.Show();

                    #region  20171003注释掉
                    //if (dgvTo.Columns.Count == 0)
                    //{
                    //    int toColumn = toFeature.Fields.FieldCount - 1;
                    //    int toCount = 0;
                    //    for (i = 0; i < toFeature.Fields.FieldCount; i++)
                    //    {
                    //        if (toFeature.Fields.get_Field(i).Type != esriFieldType.esriFieldTypeGeometry)
                    //        {
                    //            //dgvTo.Columns.Add(toFeature.Fields.get_Field(i).Name, toFeature.Fields.get_Field(i).AliasName);
                    //            dgvTo.Columns.Add(toFeature.Fields.get_Field(i).Name, toFeature.Fields.get_Field(i).Name);
                    //            dgvTo.Columns[toCount].ReadOnly = true;
                    //            dgvTo.Columns[toCount].Width = dgvTo.Width / toColumn;
                    //            toCount++;
                    //        }
                    //    }
                    //}
                    ////填充字段值
                    //for (int n = 0; n < toarray.Length; n++)
                    //{
                    //    DataGridViewRow dgvToRow = new DataGridViewRow();
                    //    dgvToRow = dgvTo.Rows[dgvTo.Rows.Add()];

                    //    //queryFilter.WhereClause = "OBJECTID=" + toarray[n];
                    //    //featureCursor = toFeatCls.Search(queryFilter, false);
                    //    toFeature = toFeatCls.GetFeature(Convert.ToInt32(toarray[n]));

                    //    for (i = 0; i < dgvTo.Columns.Count; i++)
                    //    {
                    //        for (j = 0; j < toFeature.Fields.FieldCount; j++)
                    //        {
                    //            if (dgvTo.Columns[i].Name == toFeature.Fields.get_Field(j).Name)
                    //            {
                    //                dgvToRow.Cells[i].Value = toFeature.get_Value(j);
                    //                break;
                    //            }
                    //        }
                    //    }
                    //}
                    #endregion

                }

                two.LoadData(rowfrom, rowto);
                two.Show();
            }

        }
        private void buttonItemDelRow_Click(object sender, EventArgs e)
        {
            if (dataGridViewX1.SelectedRows.Count > 0)
            {
                IDataset pTempDataset = m_Table as IDataset;
                IWorkspaceEdit pWorkspaceEdit = pTempDataset.Workspace as IWorkspaceEdit;
                pWorkspaceEdit.StartEditing(true);
                pWorkspaceEdit.StartEditOperation();
                for (int i = 0; i < dataGridViewX1.SelectedRows.Count; i++)
                {
                    int index = dataGridViewX1.SelectedRows[i].Index;
                    int oid = Convert.ToInt32(dataGridViewX1.Rows[index].Cells[0].Value);
                    m_Table.GetRow(oid).Delete();

                }
                pWorkspaceEdit.StopEditOperation();
                pWorkspaceEdit.StopEditing(true);

                AddITableToDGV(m_Table, null);
            }
            else
            {
                MessageBox.Show("请选择要删除的行！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void dataGridViewX1_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            //if (e.CellValue1.ToString().Length == 0 && e.CellValue2.ToString().Length == 0)
            //{
            //    e.SortResult = 0;
            //    // e.Handled = true;就是把默认的给屏蔽掉了
            //    e.Handled = true;
            //}
            //else if (e.CellValue1.ToString().Length == 0)
            //{
            //    e.SortResult = -1;
            //    e.Handled = true;

            //}
            //else if (e.CellValue2.ToString().Length == 0)
            //{
            //    e.SortResult = 1;
            //    e.Handled = true;

            //}
            //else
            //{
            //    //e.Handled = false;则采用datagridview默认的排序方式下面的这行代码和e.Handled=false等效。
            //    //e.SortResult = string.Compare(e.CellValue1.ToString(), e.CellValue2.ToString());
            //    e.Handled = false;
            //}
        }

        //private void dataGridViewX1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        //{
        //    DataTable d = dvtodt(this.dataGridViewX1);
        //    string test = this.dataGridViewX1.Columns[e.ColumnIndex].HeaderText;
        //    d.DefaultView.Sort = this.dataGridViewX1.Columns[e.ColumnIndex].HeaderText+ " ASC";//aa是列标题的text，ASC代表升序（desc降序）
        //    this.dataGridViewX1.Columns.Clear();
        //    this.dataGridViewX1.DataSource = d;
        //}

        //将DataGridView 内容读进datatable
        public DataTable dvtodt(DataGridView dv)
        {
            DataTable dt = new DataTable();
            DataColumn dc;
            for (int i = 0; i < dv.Columns.Count; i++)
            {
                dc = new DataColumn();
                dc.ColumnName = dv.Columns[i].HeaderText.ToString();
                dt.Columns.Add(dc);
            }
            for (int j = 0; j < dv.Rows.Count - 1; j++)
            {
                DataRow dr = dt.NewRow();
                for (int x = 0; x < dv.Columns.Count; x++)
                {
                    dr[x] = dv.Rows[j].Cells[x].Value;
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        private void dataGridViewX1_CurrentCellChanged(object sender, EventArgs e)
        {
            if (this.dataGridViewX1.CurrentCell != null)
            {
                DataGridViewColumn column = dataGridViewX1.CurrentCell.OwningColumn;
                //如果是要显示下拉列表的列的话，即选择的是变化标记的列的话
                if (column.Name.Equals("变化标记"))
                {
                    int columnIndex = dataGridViewX1.CurrentCell.ColumnIndex;
                    int rowIndex = dataGridViewX1.CurrentCell.RowIndex;
                    Rectangle rect = dataGridViewX1.GetCellDisplayRectangle(columnIndex, rowIndex, false);
                    comboBox1.Left = rect.Left;
                    comboBox1.Top = rect.Top;
                    comboBox1.Width = rect.Width;
                    comboBox1.Height = rect.Height;

                    //将单元格的内容显示为下拉列表的当前项
                    string consultingRoom = dataGridViewX1.Rows[rowIndex].Cells[columnIndex].Value.ToString();
                    int index = comboBox1.Items.IndexOf(consultingRoom);

                    comboBox1.SelectedIndex = index;
                    comboBox1.Visible = true;
                }
                else
                {
                    comboBox1.Visible = false;
                }
            }

        }

        /// <summary>
        /// 重绘下拉列表的每一项，如果不重绘的话，看不到下拉列表的每一项的内容，
        /// 但是神奇的是可以选择（因为下拉列表此时是有内容的，只不过是没有显示出来）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            e.Graphics.DrawString(comboBox1.Items[e.Index].ToString(), e.Font, Brushes.Black, e.Bounds, StringFormat.GenericDefault);
        }
        /// <summary>
        /// 在下拉列表选择项变化的时候，更改DataGridView相应的单元格的内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dataGridViewX1.CurrentCell != null)
                dataGridViewX1.CurrentCell.Value = comboBox1.Items[comboBox1.SelectedIndex];

        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonXSave_Click(object sender, EventArgs e)
        {
            IWorkspaceEdit m_WorkspaceEdit;
            m_WorkspaceEdit = ((IDataset)m_Table).Workspace as IWorkspaceEdit;
            if (!m_WorkspaceEdit.IsBeingEdited())
            {
                m_WorkspaceEdit.StartEditing(false);
            }
            m_WorkspaceEdit.StartEditOperation();


            #region 方法一
            ////ICursor pCur = m_Table.Search(null, false);
            //ICursor pCur = m_Table.Update(null, false);
            //IRow row = pCur.NextRow();
            //while (pCur != null && pCur.NextRow() != null)
            //{
            //    int pIndex = row.Fields.FindField("变化标记");

            //    if (row != null)
            //    {
            //        row.set_Value(pIndex, row.get_Value(pIndex));
            //        row.Store();
            //    }
            //    row = pCur.NextRow();
            //}
            #endregion

            #region 方法二
  
            int index = 0;
            int souIndex = 0;
            int rowCount = dataGridViewX1.Rows.Count;//得到总行数    
            int cellCount = dataGridViewX1.Rows[1].Cells.Count;//得到总列数    
            for (int i=0; i < dataGridViewX1.Columns.Count; i++)
            {
                if (dataGridViewX1.Columns[i].Name == "源OID")
                {
                    souIndex = i;

                }
                if (dataGridViewX1.Columns[i].Name == "变化标记")
                {
                    index = i;
                }
            }
            IRow pRow = null;
            ICursor pCursor = null;
            int pIndex = 0;
            int pOID = 0;
            for (int i = 0; i < rowCount; i++)//得到总行数并在之内循环    
            {
                string strDGV = dataGridViewX1.Rows[i].Cells[index].Value.ToString();

                pCursor = m_Table.Update(null, false);
                pRow = pCursor.NextRow();
                pIndex = pRow.Fields.FindField("变化标记");
                pOID = pRow.Fields.FindField("源OID");
                while (pRow != null)
                {
                    if (dataGridViewX1.Rows[i].Cells[souIndex].Value.ToString() == pRow.get_Value(pOID).ToString())
                    {
                        if (pRow.get_Value(pIndex).ToString().Trim() != strDGV)
                        {
                            //int tstin = pRow.OID;
                            pRow.set_Value(pIndex, strDGV);
                            pCursor.UpdateRow(pRow);
                        }
                    }
                    pRow = pCursor.NextRow();
                }

            }
            #endregion
            m_WorkspaceEdit.StopEditOperation();
            m_WorkspaceEdit.StopEditing(true);
            MessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK);
        }
        /// <summary>
        ///保存DataTable表为BF ,tempPath文件完整路径
        /// </summary>
        /// <param name="mTable"></param>
        /// <returns></returns>
        public static bool SaveTable(DataTable mTable, string tempPath)
        {
            try
            {
                #region 新建表字段
                IField pField = null;
                IFields fields = new FieldsClass();
                IFieldsEdit fieldsEdit = (IFieldsEdit)fields;
                fieldsEdit.FieldCount_2 = 3;
                pField = new FieldClass();
                IFieldEdit fieldEdit = (IFieldEdit)pField;
                fieldEdit.Name_2 = "FromField";
                fieldEdit.AliasName_2 = "开始字段值";
                fieldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
                fieldEdit.Editable_2 = true;
                //添加开始字段
                fieldsEdit.set_Field(0, pField);
                IField pField1 = new FieldClass();
                IFieldEdit fieldEdit1 = (IFieldEdit)pField1;
                fieldEdit1.Name_2 = "ToField";
                fieldEdit1.AliasName_2 = "结束字段值";
                fieldEdit1.Type_2 = esriFieldType.esriFieldTypeDouble;
                fieldEdit1.Editable_2 = true;
                //添加结束字段
                fieldsEdit.set_Field(1, pField1);
                IField pField2 = new FieldClass();
                IFieldEdit fieldEdit2 = (IFieldEdit)pField2;
                fieldEdit2.Name_2 = "outField";
                fieldEdit2.AliasName_2 = "分类字段值";
                fieldEdit2.Type_2 = esriFieldType.esriFieldTypeDouble;
                fieldEdit2.Editable_2 = true;
                //添加重分类字段
                fieldsEdit.set_Field(2, pField2);
                #endregion

                string path = System.IO.Path.GetDirectoryName(tempPath);
                string fileName = System.IO.Path.GetFileName(tempPath);

                IWorkspaceFactory class2 = new ESRI.ArcGIS.DataSourcesFile.ShapefileWorkspaceFactoryClass();
                ESRI.ArcGIS.Geodatabase.IWorkspace pWorkspace = class2.OpenFromFile(path, 0);
                IFeatureWorkspace pFWS = pWorkspace as IFeatureWorkspace;
                //删除已有的
                if (System.IO.File.Exists(tempPath))
                {
                    System.IO.File.Delete(tempPath);
                }
                fileName = fileName.Split('.')[0];

                //创建空表
                ESRI.ArcGIS.Geodatabase.ITable pTable;
                pTable = pFWS.CreateTable(fileName, fieldsEdit, null, null, "");

                //获取表中记录数
                int count = mTable.Rows.Count;
                //转换为ITable中的数据
                for (int k = 0; k < count; k++)
                {
                    //ITable 的记录
                    IRow row = pTable.CreateRow();

                    DataRow pRrow = mTable.Rows[k];
                    //列元素
                    int rowNum = pRrow.ItemArray.Length;
                    // 添加记录
                    for (int n = 1; n < rowNum + 1; n++)
                    {
                        row.set_Value(n, pRrow.ItemArray.GetValue(n - 1));
                        row.Store();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        /// <summary>
        /// 将ITable转换为DataTable
        /// </summary>
        /// <param name="mTable"></param>
        /// <returns></returns>
        public static DataTable ToDataTable(ITable mTable)
        {
            try
            {
                DataTable pTable = new DataTable();
                for (int i = 0; i < mTable.Fields.FieldCount; i++)
                {
                    pTable.Columns.Add(mTable.Fields.get_Field(i).Name);
                }
                ICursor pCursor = mTable.Search(null, false);
                IRow pRrow = pCursor.NextRow();
                while (pRrow != null)
                {
                    DataRow pRow = pTable.NewRow();
                    string[] StrRow = new string[pRrow.Fields.FieldCount];
                    for (int i = 0; i < pRrow.Fields.FieldCount; i++)
                    {
                        StrRow[i] = pRrow.get_Value(i).ToString();
                    }
                    pRow.ItemArray = StrRow;
                    pTable.Rows.Add(pRow);
                    pRrow = pCursor.NextRow();
                }
                return pTable;
            }
            catch (Exception ex)
            {
                return null;
            }
        }



    }
}
