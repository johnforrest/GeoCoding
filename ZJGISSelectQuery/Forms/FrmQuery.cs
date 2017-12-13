using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
//using CADViewLib;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using System.Data.OracleClient;
using ESRI.ArcGIS.Geometry;
using Microsoft.VisualBasic;
using ZJGISCommon;

namespace SelectQuery 
{
    public partial class FrmQuery :DevComponents.DotNetBar.Office2007Form

    {
        Collection m_pQueryResultCln;
        ClsMapLayer m_pQueryResult;
        //Collection m_FeatClsCln;
        Collection m_FeaturesInTree;
        Dictionary<object,string> m_dictAdded = new Dictionary<object,string>();

        public Collection QueryResultCln 
        {
            get
            {
                return m_pQueryResultCln;
            }
            set
            {
                m_pQueryResultCln = value;
            }
        }
        public ClsMapLayer QueryResult 
        {
            set
            {
                m_pQueryResult = value;
            }
        }
        public void ReLoadQueryResult()
        {
            this.dgvAttributes.Rows.Clear();
            this.tvwFeatures.Nodes.Clear();
            PopulateQueryTree(m_pQueryResultCln, tvwFeatures);
        }

        public FrmQuery()
        {
            InitializeComponent();
            dgvAttributes.Columns.Clear();
            dgvAttributes.Columns.Add("Field","字段");
            dgvAttributes.Columns[0].Width=dgvAttributes.Width/3;
            dgvAttributes.Columns[0].ReadOnly=true;
            dgvAttributes.Columns[0].SortMode=DataGridViewColumnSortMode.NotSortable;
            dgvAttributes.Columns.Add("Value", "取值");
            dgvAttributes.Columns[1].Width=dgvAttributes.Width/3;
            dgvAttributes.Columns[1].ReadOnly=true;
            dgvAttributes.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvAttributes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvAttributes.RowHeadersVisible = false;
        }

        private void frmQuery_Load(object sender, EventArgs e)
        {
            if (m_pQueryResultCln != null)
            {
                this.tvwFeatures.Nodes.Clear();
                PopulateQueryTree(m_pQueryResultCln, tvwFeatures);
            }
            else if (m_pQueryResult != null)
                PopulateFeatColTree(m_pQueryResult);
        }

        private void expandableSplitter1_ExpandedChanged(object sender, DevComponents.DotNetBar.ExpandedChangeEventArgs e)
        {

        }
        /// <summary>
        /// 把pQueryResultCln内容填充到vTree中
        /// </summary>
        /// <param name="pQueryResultCln"></param>
        /// <param name="vTree"></param>
        public void PopulateQueryTree(Collection pQueryResultCln,TreeView vTree)
        {
            m_FeaturesInTree = new Collection();
            tvwFeatures.Nodes.Clear();
            if (pQueryResultCln == null)
                return;
            ESRI.ArcGIS.Carto.ILayer pTempLyr;
            TreeNode RootNode=new TreeNode();
            RootNode.Text = "查询结果";
            RootNode.Tag = "查询结果";

            this.tvwFeatures.Nodes.Add(RootNode);
            //TreeNode pNewNode = new TreeNode();
            //pNewNode.Text = "当前链接信息";
            //pNewNode.Tag = "当前链接信息";
            //this.tvwFeatures.Nodes.Add(pNewNode);
            IFeatureClass pFeatCls;
            IFeatureCursor pFeatCur;
            ISelectionSet pSelectionSet;
            ClsMapLayer pQueryResult;
            for (int i = 0; i < pQueryResultCln.Count; i++)
            {
                pQueryResult = pQueryResultCln[i + 1] as ClsMapLayer;
                pFeatCls = pQueryResult.FeatCls;
                pTempLyr =ClsSelectQuery.FunFindLayer(pFeatCls, ClsDeclare.g_pMap) as ESRI.ArcGIS.Carto.ILayer;

                pFeatCur = pQueryResult.FeatCur;
                pSelectionSet = pQueryResult.FeatSelectionSet;
                AddQueryResult(RootNode, pQueryResult.FeatLayer, pSelectionSet);
            }
            lblFeatCount.Text = "查找到 " + m_FeaturesInTree.Count + " 个要素";
            this.tvwFeatures.ExpandAll();
            if (this.tvwFeatures.Nodes[0].Nodes.Count > 0)
            {
                if (this.tvwFeatures.Nodes[0].Nodes[0].Nodes.Count > 0)
                {
                    this.tvwFeatures.Focus();
                    this.tvwFeatures.SelectedNode = this.tvwFeatures.Nodes[0].Nodes[0].Nodes[0];
                    IFeature pFeature;
                    pFeature = tvwFeatures.SelectedNode.Tag as IFeature;
                    tvwFeatures.Tag = tvwFeatures.SelectedNode.Parent.Text;
                    dgvAttributes.Tag = pFeature;
                    PopulateGridFromFeature(pFeature);
                }
            }

        }

      
        public void AddQueryResult(TreeNode pNode,IFeatureLayer pFeatLyr,ISelectionSet pSelectionSet)
        {
            try
            {
                if (pSelectionSet == null)
                    return;
                if (pSelectionSet.Count == 0)
                    return;
                if (pFeatLyr == null)
                    return;
                TreeNode curNode = null;
                IFeatureClass pFeatCls = null;
                IDataset pDataSet = null;
                int i = 0;

                pFeatCls = pFeatLyr.FeatureClass;
                pDataSet = (IDataset)pFeatCls;
                curNode = pNode.Nodes.Add(pFeatLyr.Name);

                curNode.Tag = "地物层";
                curNode.Expand();

                IFeature pFeat=null;
                TreeNode xNode=null;
                ICursor pCursor=null;
                IFeatureCursor pFeatCur;
                pSelectionSet.Search(null,false,out pCursor);

                pFeatCur = (IFeatureCursor)pCursor;
                pFeat = pFeatCur.NextFeature();
                while(pFeat!=null)
                {
                    i = i + 1;
                    xNode = curNode.Nodes.Add(GetKey(pFeat, i), GetString(pFeat));
                    xNode.Tag = pFeat;
                    m_FeaturesInTree.Add(pFeat, GetKey(pFeat, i),null,null);
                    pFeat = pFeatCur.NextFeature();
                }
                tvwFeatures.ExpandAll();

            }
            catch (Exception)
            {
                //ClsDeclare.g_ErrorHandler.DisplayInformation("加载树图出错，请检查。错误信息："+ex.Message, false,null,null);
                throw;
            }
        }
        public string GetKey(IFeature pFeature,int iFCount)
        {
            string strPrefix;
            if (pFeature.Class.ObjectClassID < 0)
                strPrefix = pFeature.Class.AliasName;
            else
                strPrefix = Convert.ToString(pFeature.Class.ObjectClassID);
            if (pFeature.HasOID)
                return strPrefix + "_" + Convert.ToString(pFeature.OID);
            else
                return strPrefix + "_" + Convert.ToString(iFCount);
            
             
        }
        public string GetString(IFeature pFeature)
        {
            for (int i = 0; i < pFeature.Fields.FieldCount; i++)
            {
                if (pFeature.Fields.get_Field(i).Type == esriFieldType.esriFieldTypeString)
                {
                    if (!(pFeature.get_Value(i) is DBNull) && pFeature.get_Value(i).ToString().Trim() != null)
                     
                        return pFeature.get_Value(i).ToString();
                     
                }
            }
            return Convert.ToString(pFeature.OID);

        }
        public void PopulateGridFromFeature(IFeature pFeature)
        {
            this.dgvAttributes.Rows.Clear();
            if (pFeature == null)
                return;
            string sFieldName;
            object sFieldValue;
            DataGridViewRow dgvRow;
            for (int i = 0; i < pFeature.Fields.FieldCount; i++)
            {
                if (pFeature.Fields.get_Field(i).Type != esriFieldType.esriFieldTypeGeometry)
                {
                    sFieldName = pFeature.Fields.get_Field(i).AliasName;
                    if (sFieldName == "Raster")
                        sFieldValue = "Raster";
                    else
                        sFieldValue = ConvertWRTNull(pFeature.get_Value(i));
                    //this.dgvAttributes.Rows.Add(i);
                    dgvRow = this.dgvAttributes.Rows[dgvAttributes.Rows.Add()];
                    dgvRow.Cells[0].Value = sFieldName;
                    dgvRow.Cells[1].Value = sFieldValue;
                }

            }
        }
        public object ConvertWRTNull(object vValue)
        {
            if (!(vValue is DBNull))
                return vValue;
            else
                return "";

        }
        private void GetFeatInfo(IFeature pFeature,string sLayerName)
        {
            string sLinkType="";
            string sPath="";
            OracleCommand pOrclCommand;
            OracleConnection pConn=new OracleConnection();
            OracleDataReader pReader;
            try
            {
                pConn = ClsDeclare.g_Sys.SysCn;
                if (pConn.State == ConnectionState.Closed)
                    pConn.Open();
                pOrclCommand = new OracleCommand();
                pOrclCommand.Connection = pConn;
                pOrclCommand.CommandText = "SELECT * FROM WH_T_LINKTABLE WHERE LAYERNAME ='" + sLayerName + "' AND FEATUREID = '" + pFeature.OID + "' ";
                pReader = pOrclCommand.ExecuteReader();
                if (pReader.Read() == true)
                {
                    sLinkType = Convert.ToString(pReader[2]);
                    sPath = Convert.ToString(pReader[6]);
                }
                else if (sLinkType == "doc" || sLinkType == "xls")
                    System.Diagnostics.Process.Start(sPath);
                else if (sLinkType == "httpadr")
                {
                    object webobj;
                    webobj =Microsoft.VisualBasic.Interaction.CreateObject("InternetExplorer.Application",null);
                    //webobj.Visible = true;
                    //webobj.Navigate2(sPath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误", ex.StackTrace, MessageBoxButtons.OK, MessageBoxIcon.Error);
                //ClsDeclare.g_ErrorHandler.HandleError(true, null, 0, null, ex.StackTrace);
                throw;
            }
        }

        public void PopulateFeatColTree(ClsMapLayer pQueryResult)
        {
            if (pQueryResult == null)
                return;
            if (pQueryResult.FeatLayer == null || pQueryResult.FeatCln == null)
                return;
            if (pQueryResult.FeatCln.Count < 1)
                return;
            try
            {
                m_dictAdded = new Dictionary<object,string>();
                tvwFeatures.Nodes.Clear();
                TreeNode RootNode = new TreeNode();
                RootNode.Text = "查询结果";
                RootNode.Tag = "查询结果";
                this.tvwFeatures.Nodes.Add(RootNode);
                TreeNode pNewNode = new TreeNode();
                pNewNode.Text = "当前链接信息";
                pNewNode.Tag = "当前链接信息";
                this.tvwFeatures.Nodes.Add(pNewNode);
                TreeNode curNode = null;
                curNode = RootNode.Nodes.Add(pQueryResult.FeatLayer.Name);
                curNode.Tag = "地物层";
                curNode.Expand();

                Collection pFeatCol;
                IFeature pFeature;
                TreeNode xNode=null;
                pFeatCol = pQueryResult.FeatCln;

                for (int i = 1; i <= pFeatCol.Count;i++ )
                {
                    pFeature = pFeatCol[i] as IFeature;
                    if (pFeature == null)
                        continue;
                    if (!(m_dictAdded.ContainsValue(Convert.ToString(pFeature.OID))))
                    {

                        m_dictAdded.Add(pFeature, Convert.ToString(pFeature.OID));
                        xNode = curNode.Nodes.Add(GetKey(pFeature, i), GetString(pFeature));
                        xNode.Tag = pFeature;
                    }
                    
                }
                lblFeatCount.Text = "查找到 " + m_dictAdded.Count + " 个要素";
                this.tvwFeatures.ExpandAll();
                if (this.tvwFeatures.Nodes[0].Nodes.Count > 0)
                {
                    if (this.tvwFeatures.Nodes[0].Nodes[0].Nodes.Count > 0)
                    {
                        this.tvwFeatures.SelectedNode = this.tvwFeatures.Nodes[0].Nodes[0].Nodes[0];
                        pFeature = tvwFeatures.SelectedNode.Tag as IFeature;
                        PopulateGridFromFeature(pFeature);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("错误", ex.StackTrace, MessageBoxButtons.OK, MessageBoxIcon.Error);
                //ClsDeclare.g_ErrorHandler.HandleError(true, null, 0, null, ex.StackTrace);
                throw;
            }
        }

        private void expandableSplitter1_ExpandedChanged_1(object sender, DevComponents.DotNetBar.ExpandedChangeEventArgs e)
        {
            tvwFeatures.Visible =!(tvwFeatures.Visible);
        }

        private void dataGridViewX1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void tvwFeatures_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            //if (e.Node.Text == "当前链接信息")
            //{
            //    string sLayerName = Convert.ToString(tvwFeatures.Tag);
            //    IFeature pFeature = this.dgvAttributes.Tag as IFeature;
            //    if (pFeature != null && sLayerName != "")
            //        GetFeatInfo(pFeature, sLayerName);
            //    return;
            //}
            //else if (AxCADViewX1.Visible == true)
            //{
            //    AxCADViewX1.Visible = false;
            //    dgvAttributes.Visible = true;
            //    AxCADViewX1.BringToFront();
            //}
            IGeometry pGeometry;
            IEnvelope pEnvelope;
            IActiveView pActiveView;
            if (e.Node.Level == 0 || e.Node.Level == 1)
                this.dgvAttributes.Rows.Clear();
            else
            {
                IFeature pFeat;
                pFeat = e.Node.Tag as IFeature;
                if (pFeat == null)
                    return;
                tvwFeatures.Tag = e.Node.Parent.Text;
                this.dgvAttributes.Tag = pFeat;
                dgvAttributes.Refresh();
                Application.DoEvents();
                PopulateGridFromFeature(pFeat);

                pActiveView = ClsDeclare.g_pMap as IActiveView;
                pGeometry = pFeat.Shape;

                if (pGeometry.GeometryType == esriGeometryType.esriGeometryPoint)
                {
                    IPoint pPoint;
                    pPoint = pGeometry as IPoint;
                    ClsDeclare.g_Sys.MapControl.CenterAt(pPoint);
                }
                else
                {
                    IPoint pPnt;
                    pEnvelope = pActiveView.Extent;
                    pPnt = new PointClass();
                    pPnt.SpatialReference = ClsDeclare.g_pMap.SpatialReference;
                    pPnt.PutCoords((pGeometry.Envelope.XMax + pGeometry.Envelope.XMin) / 2, (pGeometry.Envelope.YMax + pGeometry.Envelope.YMin) / 2);

                    pEnvelope.CenterAt(pPnt);
                    pActiveView.Extent = pEnvelope;
                }
                pActiveView.Refresh();
                Application.DoEvents();
                ClsDeclare.g_Sys.MapControl.FlashShape(pFeat.Shape, 3, 100, null);
            }
            
        }


    
    }
}
