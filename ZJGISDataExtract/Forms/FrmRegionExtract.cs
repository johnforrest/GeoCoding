using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using SelectQuery;
using DevComponents.DotNetBar;

namespace ZJGISDataExtract
{
    public partial class FrmRegionExtract : DevComponents.DotNetBar.Office2007Form
    {
        private IMap _map;
        public static string[] strArryExpress;
        private int RegionID = -1;
        //private bool[] bRegionLayer;
        private string[] strRegionLayer;
        IFeatureLayer _featureLayerRegion = null;
        //List<IFeatureLayer> chkFeatureLayer = new List<IFeatureLayer>();
        public FrmRegionExtract(IHookHelper hookHelper)
        {
            InitializeComponent();
            this._map = hookHelper.FocusMap;     
        }

        private void FrmRegionExtract_Load(object sender, EventArgs e)//图层列表出来
        {
            DataGridViewRow dgvRow = new DataGridViewRow();     
            int LayerNum=0;
            for (int i = 0; i < _map.LayerCount; i++)
            {
                if (_map.get_Layer(i).Visible)
                {
                    if (_map.get_Layer(i) is IGroupLayer)
                    {
                        ICompositeLayer pCompositeLayer = (ICompositeLayer)_map.get_Layer(i);
                        for (int j = 0; j < pCompositeLayer.Count; j++)
                        {
                            if (pCompositeLayer.get_Layer(j).Visible)
                            {
                                //注记层，不提取
                                if (pCompositeLayer.get_Layer(j) is IAnnotationLayer | pCompositeLayer.get_Layer(j) is GdbRasterCatalogLayer | pCompositeLayer.get_Layer(j) is IRasterLayer)
                                {
                                    continue;
                                }
                                LayerNum++;
                                IFeatureLayer pFeatLayer = pCompositeLayer.get_Layer(j) as IFeatureLayer;

                                dgvRow = dgvLayerChoose.Rows[dgvLayerChoose.Rows.Add()];
                                dgvRow.Cells[0].Value = LayerNum;
                                dgvRow.Cells[1].Value = pFeatLayer.Name;
                                dgvRow.Cells[1].Tag = pFeatLayer;
                                DataGridViewCheckBoxCell dgvCheckBoxCell = new DataGridViewCheckBoxCell();
                                dgvCheckBoxCell = dgvRow.Cells[2] as DataGridViewCheckBoxCell;
                                dgvCheckBoxCell.Value = true;

                                //chkFeatureLayer.Add(pFeatLayer);
                                if (pFeatLayer.Name == "省级行政区")
                                    _featureLayerRegion = pFeatLayer;
                            }

                        }
                    }
                    else
                    {
                        if (_map.get_Layer(i) is IAnnotationLayer | _map.get_Layer(i) is GdbRasterCatalogLayer | _map.get_Layer(i) is IRasterLayer)
                        {
                            continue;
                        }
                        LayerNum++;
                        IFeatureLayer pFeatLayer = _map.get_Layer(i) as IFeatureLayer;

                        dgvRow = dgvLayerChoose.Rows[dgvLayerChoose.Rows.Add()];
                        dgvRow.Cells[0].Value = LayerNum;
                        dgvRow.Cells[1].Value = pFeatLayer.Name;
                        dgvRow.Cells[1].Tag = pFeatLayer;
                        DataGridViewCheckBoxCell dgvCheckBoxCell = new DataGridViewCheckBoxCell();
                        dgvCheckBoxCell = dgvRow.Cells[2] as DataGridViewCheckBoxCell;
                        dgvCheckBoxCell.Value = true;

                        
                        if (pFeatLayer.Name == "省级行政区")
                            _featureLayerRegion = pFeatLayer;
                    }
                }
            }
            strArryExpress = new string[LayerNum];

            //初始化文件类型下拉框
            this.cmbFileStyle.Items.Add("File GeoDatabase");
            this.cmbFileStyle.Items.Add("SHP File");
            this.cmbFileStyle.Items.Add("Personal GeoDatabase");
            this.cmbFileStyle.SelectedIndex = 0;
            this.cmbFileStyle.DropDownStyle = ComboBoxStyle.DropDownList;
            this.txtFileRoute.Enabled = false;

            //行政区选择
            int RegionNum = 0;
            if (_featureLayerRegion != null)
            {
                IFeatureCursor featureCursor = _featureLayerRegion.FeatureClass.Search(null, true);
                IFeature feature = featureCursor.NextFeature();
                while (feature != null)
                {
                    RegionNum++;
                    IFields fields = feature.Fields;
                    int fieldsNum = fields.FindField("NAME");
                    string RegionName = feature.get_Value(fieldsNum).ToString();
                    dgvRegionChoose.Rows.Add(RegionNum, RegionName);
                    feature = featureCursor.NextFeature();
                }
            }
            //bRegionLayer = new bool[ LayerNum];
            strRegionLayer = new string[ LayerNum];
            //dgvLayerChoose.ReadOnly = true;
        }

        //private void dgvRegionChoose_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        //{
        //    //if (dgvRegionChoose.CurrentCell.ColumnIndex == 3)
        //    //{
        //    //    DataGridViewCheckBoxCell che = dgvRegionChoose.CurrentRow.Cells[2] as DataGridViewCheckBoxCell;
        //    //    bool ische = Convert.ToBoolean(che.Value);
        //    //    if (ische)
        //    //    {
        //    //        labRegion.Text = "提取" + dgvRegionChoose.CurrentRow.Cells[1].Value.ToString() + "行政区：";
        //    //        RegionID = dgvRegionChoose.CurrentRow.Index;
        //    //        dgvLayerChoose.ReadOnly = false;
        //    //    }
        //    //}
        //}

        private void dgvLayerChoose_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dgvLayerChoose.CurrentCell.ColumnIndex == 3)
            {
                DataGridViewCheckBoxCell che = dgvLayerChoose.CurrentRow.Cells[2] as DataGridViewCheckBoxCell;
                bool ische = Convert.ToBoolean(che.Value);
                if (ische)
                {
                    FrmQueryByAttribute1 pFrmQuery = new FrmQueryByAttribute1();
                    ClsDeclare.g_pMap = _map;
                    pFrmQuery.StrFromName = "FrmRegionExtract";
                    pFrmQuery.LayerName = dgvLayerChoose.CurrentRow.Cells[1].Value.ToString();
                    pFrmQuery.LayerNum = Convert.ToInt32(dgvLayerChoose.CurrentRow.Cells[0].Value);
                    pFrmQuery.ShowDialog();
                }
            }
        }

        //private void btnLayerOK_Click(object sender, EventArgs e)
        //{
        //    //if (RegionID < 0)
        //    //    return;
        //    //else
        //    //{
        //    //    int ii = bRegionLayer.Length / bRegionLayer.GetLength(0);
        //    //    for (int i = 0; i < ii; i++)
        //    //    {
        //    //        DataGridViewCheckBoxCell che = dgvLayerChoose.Rows[i].Cells[2] as DataGridViewCheckBoxCell;
        //    //        bool ische = Convert.ToBoolean(che.Value);
        //    //        bRegionLayer[i] = ische;
        //    //        strRegionLayer[ i] = strArryExpress[i];
        //    //        dgvLayerChoose.ReadOnly = true;
        //    //        labRegion.Text =  dgvRegionChoose.CurrentRow.Cells[1].Value.ToString() + "行政区提取条件已选定";
        //    //    }
        //    //}
        //}

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtFileRoute.Text))
            {
                lblInfo.Visible = true;
            }

            this.Refresh();
            Application.DoEvents();
            //int m = 0;
            //foreach (bool bol in bRegionLayer)
            //{
            //    if (bol == true)
            //    {
            //        m++;
            //    }
            //}
            //if (m == 0)
            //{
            //    MessageBoxEx.Show("没有选择图层条件，请选择！");
            //    return;
            //}
            //else
            //    m = 0;        
            Deal();
            this.Refresh();

            txtFileRoute.Text = "";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            string sFileType = null;
            string sFileFilter = null;
            string sRoute = "";
            sFileType = this.cmbFileStyle.Text;
            if (sFileType == "Personal GeoDatabase")
            {
                sFileFilter = "Personal GeoDatabase ( *.mdb )|*.mdb";
                sRoute = GetFileRouteByDialog(this.SaveFile, sFileFilter);
            }
            else if (sFileType == "File GeoDatabase")
            {
                sFileFilter = "File GeoDatabase ( *.gdb )|*.gdb";
                sRoute = GetFileRouteByDialog(this.SaveFile, sFileFilter);
            }
            else if (sFileType == "SHP File")
            {
                sRoute = GetFolderByDialog(this.FolderBrowser);
            }
            this.txtFileRoute.Text = sRoute;
        }

        private string GetFileRouteByDialog(SaveFileDialog vSaveFileDialog, string sFilter)
        {
            if (vSaveFileDialog == null)
                return "";
            var _with1 = vSaveFileDialog;
            _with1.FileName = "";
            _with1.Filter = sFilter;
            _with1.FilterIndex = 1;
            _with1.OverwritePrompt = true;
            _with1.Title = "文件保存对话框";
            if (_with1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                return vSaveFileDialog.FileName;
            }
            else
            {
                return "";
            }
        }

        private string GetFolderByDialog(FolderBrowserDialog vFolderBrowser)
        {
            if (vFolderBrowser == null)
                return "'";
            var _with2 = vFolderBrowser;
            if (_with2.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                return _with2.SelectedPath;
            }
            else if (_with2.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
            {
                if (string.IsNullOrEmpty(txtFileRoute.Text))
                {
                    return "";
                }
                else
                {
                    return _with2.SelectedPath;
                }
            }
            else
            {
                return "";
            }
        }
        //获得提取的工作区间
        private IWorkspace GetWorkspace()
        {
            IWorkspace functionReturnValue = default(IWorkspace);
            string sFileRoute = null;
            string sFileStyle = null;
            IWorkspace pWorkspace = default(IWorkspace);
            sFileRoute = this.txtFileRoute.Text;
            sFileStyle = this.cmbFileStyle.Text;
            if (string.IsNullOrEmpty(sFileRoute.Trim()))
            {
                //g_clsErrorHandle.DisplayInformation("请设置提取路径！", false);
                MessageBoxEx.Show("请设置提取路径！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                functionReturnValue = null;
                return functionReturnValue;
            }
            pWorkspace = null;
            if (sFileStyle == "SHP File")
            {
                pWorkspace = ClsCreatAndGetDataset.CreatOrGetSHPWorkspace(sFileRoute);
            }
            else if (sFileStyle == "File GeoDatabase")
            {
                pWorkspace = ClsCreatAndGetDataset.GetPDBOrGDBWorkspace(sFileRoute, true);
            }
            else if (sFileStyle == "Personal GeoDatabase")
            {
                pWorkspace = ClsCreatAndGetDataset.GetPDBOrGDBWorkspace(sFileRoute, false);
            }
            functionReturnValue = pWorkspace;
            return functionReturnValue;
        }

        private void cmbFileStyle_TextChanged(object sender, EventArgs e)
        {
             string sFileStyle = null;
            string sFileRoute = null;
            string sCurrentFileStyle = null;
           // string[] sArray = null;
            sFileRoute = txtFileRoute.Text;
            if (string.IsNullOrEmpty(sFileRoute.Trim()))
                return;
            sFileStyle = cmbFileStyle.Text;
            sFileStyle = sFileStyle.ToUpper();
            if (sFileStyle == "PDB")
            {
                sFileStyle = "MDB";
            }
            else if (sFileStyle == "FDB")
            {
                sFileStyle = "GDB";
            }

            if (!string.IsNullOrEmpty(sFileRoute.Trim()))
            {
                if (sFileRoute.Length < 5)
                {
                    sCurrentFileStyle = "SHP";
                }
                else
                {
                    sCurrentFileStyle = sFileRoute.Substring(sFileRoute.Length - 3, 3).ToUpper();
                    if (sCurrentFileStyle != "MDB" & sCurrentFileStyle != "GDB")
                    {
                        sCurrentFileStyle = "SHP";
                    }
                }
                if (sCurrentFileStyle == "SHP")
                {
                    sFileRoute = "";
                }
                else
                {
                    if (sFileStyle == "SHP")
                    {
                        //sArray = sFileRoute.Split(new char[] { '\\' });
                        //sFileRoute = sFileRoute.Substring(0, sFileRoute.Length - sArray[Information.UBound(sArray, 1)].Length);
                        sFileRoute = System.IO.Directory.GetDirectoryRoot(sFileRoute);

                    }
                    else
                    {
                        sFileRoute = sFileRoute.Replace("." + sCurrentFileStyle.ToLower(), "." + sFileStyle.ToLower());
                    }
                }
            }
            txtFileRoute.Text = sFileRoute;
        }

        //开始提取
        private void Deal()
        {
            IWorkspace pWorkspace = default(IWorkspace);
            string sDesFeatClsName = null;
            IFeatureClass pDesFeatCls = default(IFeatureClass);
            string sInfo = null;
            IFeatureLayer pFeatLayer = default(IFeatureLayer);
            IFeatureClass pFeatureCls = default(IFeatureClass);
            IGeometry pDomainGeometry = default(IGeometry);
            int ErrorCount = 0;
            int lFeatCount = 0;
            bool bIsCut = false;
                    
            if (this.ChkClip.Checked)
            {
                bIsCut = true;
                ////是否剪切
            }
            else
            {
                bIsCut = false;
            }
            int intChooseLayer = 0;

            ////印骅 20081121  获得多边形范围
            int iRegion = 0;
            if (_featureLayerRegion != null)
            {
                IFeatureCursor featureCursor = _featureLayerRegion.FeatureClass.Search(null, true);
                IFeature feature = featureCursor.NextFeature();
                while (feature != null)
                {
                    iRegion++;
                    if (Convert.ToBoolean(((DataGridViewCheckBoxCell)dgvRegionChoose.Rows[iRegion - 1].Cells["colChoose"]).Value))
                    {
                        intChooseLayer++; 
                        IPolygon polygon = feature.Shape as IPolygon;
                        pDomainGeometry = polygon;

                        if (_map.LayerCount == 0)
                        {
                            MessageBoxEx.Show("没有加载图层或没有图层处于选中状态！请选中需要提取的图层！", "提示");
                            return;
                        }

                        int iCount = 0;
                        IDataset pTempDataset = default(IDataset);
                        //ploger = new WHFUtilities.clsLog();
                        //ploger.DBConnectionString = "Data Source=" + g_Sys.DbName + ";User ID=" + g_Sys.DbUser + " ;Password=" + g_Sys.DbPass;

                        DataGridViewCheckBoxCell dgvCheckBoxCell = new DataGridViewCheckBoxCell();
                        DataGridViewRow dgvRow = new DataGridViewRow();
                        for (int i = 0; i < dgvLayerChoose.Rows.Count; i++)
                        {
                            dgvRow = dgvLayerChoose.Rows[i];
                            dgvCheckBoxCell = dgvRow.Cells[2] as DataGridViewCheckBoxCell;
                            bool bolcheck=Convert .ToBoolean( dgvCheckBoxCell.Value);
                            if (bolcheck)
                            {
                                if (pWorkspace == null)
                                {
                                    pWorkspace = GetWorkspace();     
                                }
                                    pFeatLayer = dgvLayerChoose.Rows[i].Cells[1].Tag as IFeatureLayer;
                                    pFeatureCls = pFeatLayer.FeatureClass;
                                    sDesFeatClsName = pFeatureCls.AliasName;
                                    sInfo = "当前操作层：" + pFeatLayer.Name;
                                    this.lblInfo.Text = sInfo;
                                    Application.DoEvents();
                                    if (!string.IsNullOrEmpty(sDesFeatClsName.Trim()))
                                    {
                                        if (pFeatureCls != null)
                                        {
                                            this.lblInfo.Text = sInfo + "正在获得目标要素类，请稍候....";
                                            lblInfo.Refresh();
                                            Application.DoEvents();
                                            if (sDesFeatClsName.Contains("."))
                                            {
                                                int dotIndex = 0;
                                                dotIndex = sDesFeatClsName.IndexOf(".");
                                                sDesFeatClsName = sDesFeatClsName.Substring(dotIndex + 1, sDesFeatClsName.Length - dotIndex - 1);
                                            }

                                            //判断是否需要创建要素类 yh 15/4/09
                                            ClsCreateFeat.CheckFeatCls(bIsCut, pFeatLayer, ref lFeatCount, pDomainGeometry);
                                            if (lFeatCount != 0)
                                            {
                                                IFeatureWorkspace pFeatureWorkspace = (IFeatureWorkspace)pWorkspace;
                                                IFields pFields = pFeatureCls.Fields;
                                                pDesFeatCls = ClsCreatAndGetDataset.CreatOrOpenFeatClsByName(ref pFeatureWorkspace, sDesFeatClsName, ref pFields, null, null);
                                                pWorkspace = (IWorkspace)pFeatureWorkspace;
                                                //pFeatureCls.Fields = pFields;
                                            }
                                            if (pDesFeatCls != null)
                                            {
                                                //如果不剪切的，则使用该方法，需要剪切，则使用选择集，分位于内部要素和位于外部要素或直接使用pfeatcls搜索二遍
                                                //如果数据量大的话，搜索二遍的方法是不可行的
                                                ISpatialReference pSpatialReference = null;
                                                Dictionary<string, string> pDicField = null;

                                                iCount = (int)ClsCreateFeat.OutPutFeat(ref pDesFeatCls, ref pFeatureCls, strRegionLayer[i], pDomainGeometry, bIsCut, ref pSpatialReference, pDicField, ref this.lblInfo, sInfo);
                                                IFeatureLayer showFeatureLayer = new FeatureLayer();
                                                showFeatureLayer.FeatureClass = pDesFeatCls;
                                                ILayer showLayer = showFeatureLayer as ILayer;
                                                showLayer.Name = pDesFeatCls.AliasName;
                                                _map.AddLayer(showLayer);
                                                // 陈昉  2009-3-1  修改 修改原因 写日志    
                                                if (iCount > 0)
                                                {
                                                    pTempDataset = (IDataset)pFeatureCls;
                                                    //ploger.log(System.Environment.MachineName, g_Sys.User.UserID, System.DateTime.Now.ToString(), 
                                                    //        "数据管理子系统", "WHFDataExtract", "快速提取" + pTempDataset.Name + "中" + iCount +
                                                    //        "个要素到" + pDesFeatCls.AliasName, "目标：" + System.IO.Path.GetFileName(pWorkspace.PathName));
                                                }
                                            }
                                            else
                                            {
                                                ErrorCount = ErrorCount + 1;
                                            }
                                        }
                                    }

                                }
                            }                                                               
                    }
                    feature = featureCursor.NextFeature();
                }
                this.Refresh();
                Application.DoEvents();
                if (ErrorCount > 0)
                {
                    this.lblInfo.Text = "操作中有错误发生！";
                    //g_clsErrorHandle.DisplayInformation("操作中有错误发生！", false);
                    MessageBoxEx.Show("操作中有错误发生！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }          
                if (intChooseLayer == 0)
                {
                    MessageBoxEx.Show("没有选择提取要素，请选择！", "提示");
                    return;
                }
                //Member member = new Member();
                //member.AddLayerToMap(pWorkspace, _map);
                ESRI.ArcGIS.Carto.IActiveView activeView = _map as ESRI.ArcGIS.Carto.IActiveView;
                activeView.Refresh();
                this.lblInfo.Text = "操作完成！";
                MessageBoxEx.Show("操作完成！", "提示");

            }


           
        }

    }
}
