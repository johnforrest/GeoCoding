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
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Geodatabase;
using DevComponents.DotNetBar;


namespace ZJGISDataExtract
{
    public partial class FrmLayerExtract : DevComponents.DotNetBar.Office2007Form
    {
        private IMap _map;
        private List<IFeatureLayer> choseLayer = new List<IFeatureLayer>(); 
        public FrmLayerExtract(IHookHelper hookHelper)
        {
            InitializeComponent();
            this._map = hookHelper.FocusMap;         
        }
        
        private void FrmLayerExtract_Load(object sender, EventArgs e)
        {
            int num = 0;
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
                                IFeatureLayer pFeatLayer = pCompositeLayer.get_Layer(j) as IFeatureLayer;
                                num++;
                                dgvChoose.Rows.Add(num, pFeatLayer.Name);
                                choseLayer.Add(pFeatLayer);
                            }

                        }
                    }
                    else
                    {
                        if (_map.get_Layer(i) is IAnnotationLayer | _map.get_Layer(i) is GdbRasterCatalogLayer | _map.get_Layer(i) is IRasterLayer)
                        {
                            continue;
                        }
                        IFeatureLayer pFeatLayer = _map.get_Layer(i) as IFeatureLayer;
                        num++;
                        dgvChoose.Rows.Add(num, pFeatLayer.Name);
                        choseLayer.Add(pFeatLayer);
                    }
                }
            }

            //初始化文件类型下拉框
            this.cmbFileStyle.Items.Add("File GeoDatabase");
            this.cmbFileStyle.Items.Add("SHP File");
            this.cmbFileStyle.Items.Add("Personal GeoDatabase");

            this.cmbFileStyle.SelectedIndex = 0;
            this.cmbFileStyle.DropDownStyle = ComboBoxStyle.DropDownList;
            this.txtFileRoute.Enabled = false;



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
        }


        //获得提取的工作区间
        private IWorkspace GetWorkspace()
        {
            IWorkspace functionReturnValue = default(IWorkspace);
            string sFileRoute = null;
            string sFileStyle = null;
            IWorkspace pWorkspace = default(IWorkspace);
            sFileRoute = this.txtFileRoute.Text;//定义路径
            sFileStyle = this.cmbFileStyle.Text;//定义类型
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
            //grouplayer中图层可见性
            string sDesFeatClsName = null;
            IFeatureClass pDesFeatCls = default(IFeatureClass);
            string sInfo = null;
            IFeatureLayer pFeatLayer = default(IFeatureLayer);
            IFeatureClass pFeatureCls = default(IFeatureClass);
            int ErrorCount = 0;
            ErrorCount = 0;
                 
            if (_map.LayerCount == 0)
            {
                //g_clsErrorHandle.DisplayInformation("没有加载图层或没有图层处于选中状态！请选中需要提取的图层！", false);
                MessageBoxEx.Show("没有加载图层或没有图层处于选中状态！请选中需要提取的图层！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //WHFUtilities.clsLog ploger = default(WHFUtilities.clsLog);
            //// 陈昉  2009-2-23  修改 修改原因 写日志
            IDataset pTempDataset = default(IDataset);
            int iCount = 0;
            //ploger = new WHFUtilities.clsLog();
            //ploger.DBConnectionString = "Data Source=" + g_Sys.DbName + ";User ID=" + g_Sys.DbUser + " ;Password=" + g_Sys.DbPass;
            int intChooseLayer = 0;
            for(int i=0;i <choseLayer.Count; i ++)
            {
                DataGridViewCheckBoxCell che = dgvChoose.Rows[i].Cells[2] as DataGridViewCheckBoxCell;
                if (Convert.ToBoolean(che.Value))
                {
                    if (pWorkspace == null)
                    {
                        pWorkspace = GetWorkspace();
                    }
                
                        intChooseLayer++;
                        pFeatLayer = choseLayer[i];
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
                                IFeatureWorkspace pFeatureWorkspace = (IFeatureWorkspace)pWorkspace;
                                IFields pFields = pFeatureCls.Fields;
                                pDesFeatCls = ClsCreatAndGetDataset.CreatOrOpenFeatClsByName(ref pFeatureWorkspace, sDesFeatClsName, ref pFields, null, null);
                                pWorkspace = (IWorkspace)pFeatureWorkspace;
                                //pFeatureCls.Fields = pFields;

                                if (pDesFeatCls != null)
                                {
                                    //如果不剪切的，则使用该方法，需要剪切，则使用选择集，分位于内部要素和位于外部要素或直接使用pfeatcls搜索二遍
                                    //如果数据量大的话，搜索二遍的方法是不可行的
                                    ISpatialReference pSpatialRefer = null;

                                    iCount = (int)ClsCreateFeat.OutPutFeat(ref pDesFeatCls, ref pFeatureCls, "", null, false, ref pSpatialRefer, null, ref this.lblInfo, sInfo);
                                    IFeatureLayer showFeatureLayer = new FeatureLayer();
                                    showFeatureLayer.FeatureClass = pDesFeatCls;
                                    ILayer showLayer = showFeatureLayer as ILayer;
                                    showLayer.Name = pDesFeatCls.AliasName + "_Extract";
                                    _map.AddLayer(showLayer);
                                    if (iCount > 0)
                                    {
                                        pTempDataset = (IDataset)pFeatureCls;
                                        //// 陈昉  2009-3-1  修改 修改原因 写日志                                        
                                        //ploger.log(System.Environment.MachineName, g_Sys.User.UserID, System.DateTime.Now.ToString(), "数据管理子系统", "WHFDataExtract", "快速提取" + pTempDataset.Name + "中" + iCount + "个要素到" + pDesFeatCls.AliasName, "目标 ：" + System.IO.Path.GetFileName(pWorkspace.PathName));
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
            if (intChooseLayer == 0)
            {
                MessageBoxEx.Show("没有选择提取要素，请选择！", "提示");
                return;
            }
            this.Refresh();
            Application.DoEvents();
            if (ErrorCount > 0)
            {
                this.lblInfo.Text = "操作中有错误发生！";
                //g_clsErrorHandle.DisplayInformation("操作中有错误发生！", false);
                MessageBoxEx.Show("操作中有错误发生！", "提示");
                return;
            }
            else
            {
                //Member member = new Member();
                //member.AddLayerToMap(pWorkspace, _map);
                this.lblInfo.Text = "操作完成！";
                //g_clsErrorHandle.DisplayInformation("操作完成！", false);
                MessageBoxEx.Show("操作完成！", "提示");
            }
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

        ////初始化可见图层列表
        //private void SetLayerByMap(IMap pMap, ref Collection pFeatClsCol)
        //{
        //    int i = 0;
        //    ILayer pLayer = default(ILayer);
        //    IFeatureLayer pFeatLayer = default(IFeatureLayer);
        //    IFeatureClass pFeatCls = default(IFeatureClass);
        //    if (pMap == null)
        //        return;

        //    for (i = 0; i <= pMap.LayerCount - 1; i++)
        //    {
        //        pLayer = pMap.get_Layer(i);
        //        if (pLayer.Visible == true && pLayer is IFeatureLayer && (!(pLayer is IGdbRasterCatalogLayer)))
        //        {
        //            pFeatLayer = (IFeatureLayer)pLayer;
        //            pFeatCls = pFeatLayer.FeatureClass;
        //            if (pFeatCls != null)
        //            {
        //                if (pFeatClsCol != null)
        //                {
        //                    pFeatClsCol.Add(pFeatLayer.FeatureClass, null, null, null);
        //                }
        //                else
        //                {
        //                    pFeatClsCol = new Collection();
        //                    pFeatClsCol.Add(pFeatLayer.FeatureClass, null, null, null);
        //                }
        //            }
        //        }
        //    }
        //}

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmbFileStyle_TextChanged(object sender, EventArgs e)
        {
            string sFileStyle;
            string sFileRoute;
            string sCurrentFileStyle;
            string[] sArray;

            sFileRoute = txtFileRoute.Text;
            if (sFileRoute.Trim() == "")
            {
                return;
            }
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

            if (sFileRoute.Trim() != "")
            {
                if (sFileRoute.Length < 5)
                {
                    sCurrentFileStyle = "SHP";
                }
                else
                {
                    sCurrentFileStyle = sFileRoute.Substring(sFileRoute.Length - 3, 3).ToUpper();
                    if (sCurrentFileStyle != "MDB" && sCurrentFileStyle != "GDB")
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
                        //sFileRoute = sFileRoute.Substring(0, sFileRoute.Length - sArray[Information.UBound(sArray, 0)].Length);
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
                sFileFilter = "FDB ( *.gdb )|*.gdb";
                sRoute = GetFileRouteByDialog(this.SaveFile, sFileFilter);
            }
            else if (sFileType == "SHP File")
            {
                sRoute = GetFolderByDialog(this.FolderBrowser);
            }
            this.txtFileRoute.Text = sRoute;
        }
    }
}
