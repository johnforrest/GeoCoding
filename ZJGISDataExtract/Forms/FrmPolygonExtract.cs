using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using System.IO;
using SelectQuery;
using ESRI.ArcGIS.Controls;
using DevComponents.DotNetBar;
namespace ZJGISDataExtract
{
    public partial class FrmPolygonExtract : DevComponents.DotNetBar.Office2007Form
    {
        //模块级变量定义区
        private IMap m_pMap;
        private IGeometry m_pGeometry;
        private bool bIsCut;
        private IElement m_pElement;
        List<IFeatureLayer> cheFeatureLayer = new List<IFeatureLayer>();
        private PolygonExtract m_pExtractByPolygon = new PolygonExtract();
        private IMapControl4 m_pMapControl;
        public static string[] strArryExpress; 


        public IMapControl4 MapControl
        {
            set { m_pMapControl = value; }
        }

        //传入当前地图
        public IMap Map
        {
            set
            {
                m_pMap = value;
            }
        }

        //传入多边形范围
        public IGeometry Geometry
        {
            set
            {
                m_pGeometry = value;
            }
        }

        //传入多边形element
        public IElement Element
        {
            set
            {
                m_pElement = value;
            }
        }


        public FrmPolygonExtract()
        {
            InitializeComponent();
        }

        private void FrmPolygonExtract_Load(object sender, EventArgs e)
        {
            //列表dataGridViewX1
            int num=0;
            for (int i = 0; i < m_pMap.LayerCount; i++)
            {
                if (m_pMap.get_Layer(i).Visible)
                {
                    if (m_pMap.get_Layer(i) is IGroupLayer)
                    {
                       ICompositeLayer pCompositeLayer = (ICompositeLayer)m_pMap.get_Layer(i);
                        for (int j = 0; j < pCompositeLayer.Count; j++)
                        {
                            if (pCompositeLayer.get_Layer(j).Visible)
                            {
                                //注记层，不提取
                                if (pCompositeLayer.get_Layer(j) is IAnnotationLayer | pCompositeLayer.get_Layer(j) is GdbRasterCatalogLayer | pCompositeLayer.get_Layer(j) is IRasterLayer)
                                {
                                    continue;
                                }
                                IFeatureLayer pFeatLayer = pCompositeLayer.get_Layer(j) as IFeatureLayer;
                                num++;
                                dgvChoose.Rows.Add(num, pFeatLayer.Name);
                                cheFeatureLayer.Add(pFeatLayer);
                            }
                           
                        }
                    }
                    else
                    {
                        if (m_pMap.get_Layer(i) is IAnnotationLayer | m_pMap.get_Layer(i) is GdbRasterCatalogLayer | m_pMap.get_Layer(i) is IRasterLayer)
                        {
                            continue;
                        }
                        IFeatureLayer pFeatLayer = m_pMap.get_Layer(i) as IFeatureLayer;
                        num++;
                        dgvChoose.Rows.Add(num, pFeatLayer.Name);
                        cheFeatureLayer.Add(pFeatLayer);
                    }
                }
            }
            if (num!=0)
                strArryExpress = new string[num];


            //初始化文件类型下拉框
            {
                this.cmbFileStyle.Items.Add("File GeoDatabase");
                this.cmbFileStyle.Items.Add("SHP File");
                this.cmbFileStyle.Items.Add("Personal GeoDatabase");

                this.cmbFileStyle.SelectedIndex = 0;
                this.cmbFileStyle.DropDownStyle = ComboBoxStyle.DropDownList;
                //lblInfo.Text = "进度信息："
                this.txtFileRoute.Enabled = false;

                if (m_pGeometry != null)
                {
                    ChkClip.Visible = true;
                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtFileRoute.Text))
            {
                lblInfo.Visible = true;
            }

            this.Refresh();
            Application.DoEvents();
            Deal();
            this.Refresh();

            txtFileRoute.Text = "";
            if (m_pElement != null)
            {
                IGraphicsContainer pGraphicsContainer = (IGraphicsContainer)m_pMap;
                IActiveView pActiveView = (IActiveView)m_pMap;
                pGraphicsContainer.DeleteElement(m_pElement);
                pActiveView.Refresh();
                m_pElement = null;
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //删除element元素
            {
                if (m_pElement != null)
                {
                    //删除多边形
                    IGraphicsContainer pGraphicsContainer = default(IGraphicsContainer);
                    IActiveView pActiveView = default(IActiveView);
                    pGraphicsContainer = (IGraphicsContainer)m_pMap;
                    pActiveView = (IActiveView)m_pMap;
                    pGraphicsContainer.DeleteElement(m_pElement);
                    pActiveView.Refresh();

                    m_pElement = null;
                }

                this.Close();
            }
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

      
            if (this.ChkClip.Checked)
            {
                bIsCut = true;
                ////是否剪切
            }
            else
            {
                bIsCut = false;
            }

            ////印骅 20081121  获得多边形范围
            pDomainGeometry = m_pGeometry;

            if (m_pMap.LayerCount == 0)
            {
                //g_clsErrorHandle.DisplayInformation("没有加载图层或没有图层处于选中状态！请选中需要提取的图层！", false);
                MessageBoxEx.Show("没有加载图层或没有图层处于选中状态！请选中需要提取的图层！", "提示");
                return;
            }

            //WHFUtilities.clsLog ploger = default(WHFUtilities.clsLog);
            // 陈昉  2009-2-23  修改 修改原因 写日志
            int iCount = 0;
            IDataset pTempDataset = default(IDataset);
            //ploger = new WHFUtilities.clsLog();
            //ploger.DBConnectionString = "Data Source=" + g_Sys.DbName + ";User ID=" + g_Sys.DbUser + " ;Password=" + g_Sys.DbPass;
            int intChooseLayer = 0;
            for (int i = 0; i < cheFeatureLayer.Count; i++)
            {
                if (Convert.ToBoolean(((DataGridViewCheckBoxCell)dgvChoose.Rows[i].Cells["Column3"]).Value))
                {
                    if (pWorkspace == null)
                    {
                        pWorkspace = GetWorkspace();
                    }
                    
                        intChooseLayer++;
                        pFeatLayer = cheFeatureLayer[i];//地图上的图层
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

                                    iCount = (int)ClsCreateFeat.OutPutFeat(ref pDesFeatCls, ref pFeatureCls, strArryExpress[i], pDomainGeometry, bIsCut, ref pSpatialReference, pDicField, ref this.lblInfo, sInfo);
                                    IFeatureLayer showFeatureLayer = new FeatureLayer();
                                    showFeatureLayer.FeatureClass = pDesFeatCls;
                                    ILayer showLayer = showFeatureLayer as ILayer;
                                    showLayer.Name = pDesFeatCls.AliasName + "_Extract";
                                    m_pMap.AddLayer(showLayer);
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
            this.Refresh();
            Application.DoEvents();         
            if (intChooseLayer == 0)
            {
                MessageBoxEx.Show("没有选择任何要素图层！", "提示");
                return;
            }
            if (ErrorCount > 0)
            {
                this.lblInfo.Text = "操作中有错误发生！";
                //g_clsErrorHandle.DisplayInformation("操作中有错误发生！", false);
                MessageBoxEx.Show("操作中有错误发生！", "提示");
            }
            else
            {
                //Member member = new Member();
                //member.AddLayerToMap(pWorkspace, m_pMap);
                ESRI.ArcGIS.Carto.IActiveView activeView = m_pMap as ESRI.ArcGIS.Carto.IActiveView;
                activeView.Refresh();
                this.lblInfo.Text = "操作完成！";
                //g_clsErrorHandle.DisplayInformation("操作完成！", false);
                MessageBoxEx.Show("操作完成！", "提示");
            }
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
                        sFileRoute = System.IO.Directory.GetParent(sFileRoute).FullName;

                    }
                    else
                    {
                        sFileRoute = sFileRoute.Replace("." + sCurrentFileStyle.ToLower(), "." + sFileStyle.ToLower());
                    }
                }
            }
            txtFileRoute.Text = sFileRoute;
        }


        private void dataGridViewX1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dgvChoose.CurrentCell.ColumnIndex==3 )
            {
                DataGridViewCheckBoxCell che= dgvChoose.CurrentRow.Cells[2] as DataGridViewCheckBoxCell;
                bool ische = Convert.ToBoolean(che.Value);
                if (ische)
                {
                    FrmQueryByAttribute1 pFrmQuery = new FrmQueryByAttribute1();
                    ClsDeclare.g_pMap = m_pMap;
                    pFrmQuery.StrFromName = "FrmPolygonExtract";
                    pFrmQuery.LayerName = dgvChoose.CurrentRow.Cells[1].Value.ToString();
                    pFrmQuery.LayerNum = Convert.ToInt32(dgvChoose.CurrentRow.Cells[0].Value);
                    pFrmQuery.ShowDialog();
                }
            }
        }
    }
}
