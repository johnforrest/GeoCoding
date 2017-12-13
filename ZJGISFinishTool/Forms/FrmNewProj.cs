using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using ZJGISOpenData.Forms;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System.IO;
using ESRI.ArcGIS.Carto;
using System.Collections.ObjectModel;
using DevComponents.DotNetBar;
namespace ZJGISFinishTool
{
    public partial class FrmNewProj : DevComponents.DotNetBar.Office2007Form
    {
        FrmOpenData pFrmOpenData;
        //不将数据添加到map中
        private bool m_bAddData = false;
        //是否投影转换
        private bool m_bHasSpatRef = false;
        
        private IFeatureClass m_pFeatureCls;
        //要素类记录数
        private int m_iNum = 1;
        //转换不成功的记录数
        private int m_iTNum = 1;

        //路径集合
        private static Collection<object> m_PathCollection = null;
        //要素类集合
        private Collection<object> m_FeatClsCollection = null;

        //OutPut函数的返回值
        private long m_lResult;
        //是否全部都转换成功
        private bool m_bAllTransferDone = true;

        //
        private IMap m_pMap;

        public FrmNewProj(IMap pMap)
        {
            InitializeComponent();

            m_pMap = pMap;
        }

        private void cmbFileStyle_TextChanged(object sender, EventArgs e)
        {
            string sFileStyle = null;
            string sFileRoute = null;
            string sCurrentFileStyle = null;
            string[] sArray = null;
            sFileRoute = txtSaveFileRoute.Text;
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
                        //sArray = sFileRoute.Split("\\");
                        sArray = sFileRoute.Split(new char[] { '\\' });
                        sFileRoute = sFileRoute.Substring(0, sFileRoute.Length - sArray[Information.UBound(sArray, 1)].Length);
                    }
                    else
                    {
                        sFileRoute = sFileRoute.Replace("." + sCurrentFileStyle.ToLower(), "." + sFileStyle.ToLower());
                    }
                }
            }
            txtSaveFileRoute.Text = sFileRoute;
        }

        private void btBrowser_Click(object sender, EventArgs e)
        {
            OpenFileDialog OFileDlg = default(OpenFileDialog);
            OFileDlg = new OpenFileDialog();
            
            OFileDlg.Title = "加载prj文件";
            OFileDlg.InitialDirectory = "c:\\";
            OFileDlg.RestoreDirectory = true;
            OFileDlg.Filter = "PRJ(*.prj)|*.prj";
            if (OFileDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtPreDefinedPrj.Text = OFileDlg.FileName;
                m_bHasSpatRef = true;
            }
            else
            {
                txtPreDefinedPrj.Text = "";
                m_bHasSpatRef = false;
            }
        }

        private void btSaveBrowser_Click(object sender, EventArgs e)
        {
            string sFileType = null;
            string sFileFilter = null;
            string sRoute = "";
            sFileType = this.cmbFileStyle.Text;
            if (sFileType == "PDB")
            {
                sFileFilter = "PDB ( *.mdb )|*.mdb";
                sRoute = GetFileRouteByDialog(this.SaveFile, sFileFilter);
            }
            else if (sFileType == "FDB")
            {
                sFileFilter = "FDB ( *.gdb )|*.gdb";
                sRoute = GetFileRouteByDialog(this.SaveFile, sFileFilter);
            }
            else if (sFileType == "SHP")
            {
                sRoute = GetFolderByDialog(this.FolderBrowser);
            }
            this.txtSaveFileRoute.Text = sRoute;

        }

        private void btStart_Click(object sender, EventArgs e)
        {
            this.Refresh();
            Application.DoEvents();
            Deal();
            this.Refresh();
        }

        private void FrmNewProj_Load(object sender, EventArgs e)
        {
            //初始化文件类型下拉框
            {
                this.cmbFileStyle.Items.Add("FDB");
                this.cmbFileStyle.Items.Add("SHP");
                this.cmbFileStyle.Items.Add("PDB");

                this.cmbFileStyle.SelectedIndex = 0;
                this.cmbFileStyle.DropDownStyle = ComboBoxStyle.DropDownList;

                this.txtSaveFileRoute.Enabled = false;
                this.txtPreDefinedPrj.Enabled = false;

                ProgressBar.Visible = false;
            }
        }

        private void btOpenBrowser_Click(object sender, EventArgs e)
        {
           
            pFrmOpenData = new FrmOpenData((IBasicMap)m_pMap);
            pFrmOpenData.BlnAddData = m_bAddData;
            //不将数据载入mapcontrol

            pFrmOpenData.ShowDialog();

            bool bHasIt = false;
            ////是否已经加载过

            IGeoDataset pGeodataset = default(IGeoDataset);

            if (pFrmOpenData.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                ////批量载入要素类路径
                if (m_PathCollection == null)
                {
                    m_PathCollection = (Collection<object>)pFrmOpenData.PathCln;
                    m_FeatClsCollection = (Collection<object>)pFrmOpenData.FeatClsCollection;

                    //如果没有选中任何项，退出函数
                    if (m_PathCollection == null || m_PathCollection.Count == 0)
                    {
                        return;
                    }

                    // 20081213 
                    if (m_FeatClsCollection.Count == 0)
                    {
                        return;
                    }

                    //修改为在datagridView中显示要素类
                    this.dgvData.Rows.Add(m_PathCollection.Count);

                    for (m_iNum = 0; m_iNum <= m_PathCollection.Count - 1; m_iNum++)
                    {
                        pGeodataset = (IGeoDataset)m_FeatClsCollection[m_iNum];

                        this.dgvData.Rows[m_iNum].Tag = m_FeatClsCollection[m_iNum];

                        this.dgvData.Rows[m_iNum].Cells["colNum"].Value = m_iNum;
                        this.dgvData.Rows[m_iNum].Cells["colFeatClsPath"].Value = m_PathCollection[m_iNum];
                        this.dgvData.Rows[m_iNum].Cells["colCoordinateSys"].Value = pGeodataset.SpatialReference.Name;
                    }
                }
                else
                {
                    //// 重复添加数据
                    Collection<object> NextPathCollection = null;
                    Collection<object> NextFeatClsCollection = null;
                   
                    int j = 0;
                    int k = 0;

                    NextPathCollection = (Collection<object>)pFrmOpenData.PathCln;
                    NextFeatClsCollection = (Collection<object>)pFrmOpenData.FeatClsCollection;

                    // 处理选中要素集后确定出错的问题
                    if (NextFeatClsCollection.Count == 0)
                    {
                        return;
                    }

                    //印骅 如果没有选中任何项，退出函数
                    if (NextPathCollection == null || NextPathCollection.Count == 0)
                    {
                        return;
                    }
                    //for (int i = 0; i <= NextPathCollection.Count-1; i++)
                    //{
                    //    m_PathCollection.Add(NextPathCollection[i]);
                    //}
                    for (k = 0; k <= NextPathCollection.Count - 1; k++)
                    {

                        for (j = 0; j <= m_PathCollection.Count - 1; j++)
                        {
                            bHasIt = false;
                            //重复项 
                            if (NextPathCollection[k] == m_PathCollection[j])
                            {
                                bHasIt = true;
                                break; // TODO: might not be correct. Was : Exit For
                            }
                        }
                        //添加非重复项
                        if (!bHasIt)
                        {
                            this.dgvData.Rows.Add();

                            pGeodataset = (IGeoDataset)NextFeatClsCollection[k];

                            this.dgvData.Rows[m_iNum ].Cells["colNum"].Value = m_iNum;
                            this.dgvData.Rows[m_iNum ].Cells["colFeatClsPath"].Value = NextPathCollection[k];
                            this.dgvData.Rows[m_iNum ].Cells["colCoordinateSys"].Value = pGeodataset.SpatialReference.Name;

                            m_PathCollection.Add(NextPathCollection[k]);
                            m_FeatClsCollection.Add(NextFeatClsCollection[k]);

                            m_iNum += 1;
                        }
                    }
                }
            }
        }

        private void btDelete_Click(object sender, EventArgs e)
        {
            int iIndex = 0;
            //删除项的索引号
            if (this.dgvData.SelectedRows.Count != 0)
            {
                foreach (DataGridViewRow row in this.dgvData.SelectedRows)
                {
                    if (!row.IsNewRow)
                    {
                        iIndex = 0;
                        iIndex = row.Index;
                        //lvwItem.Index获得从0开始的索引
                        dgvData.Rows.Remove(row);
                        //根据索引，删除集合中的项
                        iIndex += 1;
                        //获得lvwItem对应于Collection中的索引
                        m_PathCollection.Remove(iIndex);
                        m_FeatClsCollection.Remove(iIndex);

                        m_iNum -= 1;
                    }
                }

                if (this.dgvData.RowCount != 0)
                {
                    int iNum = 1;
                    foreach (DataGridViewRow row in this.dgvData.Rows)
                    {
                        row.Cells["colNum"].Value = iNum;
                        iNum += 1;
                    }
                }
            }
        }

        private void btQuit_Click(object sender, EventArgs e)
        {
            this.Close();
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

        //获得提取的工作区间
        private IWorkspace GetWorkspace()
        {
            IWorkspace functionReturnValue = default(IWorkspace);
            string sFileRoute = null;
            string sFileStyle = null;
            IWorkspace pWorkspace = default(IWorkspace);
            sFileRoute = this.txtSaveFileRoute.Text;
            sFileStyle = this.cmbFileStyle.Text;
            if (string.IsNullOrEmpty(sFileRoute.Trim()))
            {
                //g_clsErrorHandle.DisplayInformation("请设置提取路径！", false);
                MessageBoxEx.Show("请设置提取路径！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                functionReturnValue = null;
                return functionReturnValue;
            }
            pWorkspace = null;
            if (sFileStyle == "SHP")
            {
                pWorkspace = ClsCreatAndGetDataset.CreatOrGetSHPWorkspace(sFileRoute);
            }
            else if (sFileStyle == "FDB")
            {
                pWorkspace = ClsCreatAndGetDataset.GetPDBOrGDBWorkspace(sFileRoute, true);
            }
            else if (sFileStyle == "PDB")
            {
                pWorkspace = ClsCreatAndGetDataset.GetPDBOrGDBWorkspace(sFileRoute, false);
            }
            functionReturnValue = pWorkspace;
            return functionReturnValue;
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
            else
            {
                return "";
            }
        }

        private void Deal()
        {
            IWorkspace pWorkspace = default(IWorkspace);
            string sDesFeatClsName = null;
            IFeatureClass pDesFeatCls = default(IFeatureClass);
            string sInfo = null;
            ISpatialReference pSpatRef = null;

            int i = 0;

            string sTxtPath = "";
            bool bExist = false;
            //判断Log文件夹是否存在

            //印骅 20081201 是否选择了要素类
            if (dgvData.Rows.Count == 0)
            {
                //g_clsErrorHandle.DisplayInformation("没有添加任何数据，请添加数据！", false);
                MessageBoxEx.Show("没有添加任何数据，请添加数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //是否进行投影转换,只有选择路径和投影参数以后才可转换
            if (m_bHasSpatRef == false)
            {
                if (string.IsNullOrEmpty(txtSaveFileRoute.Text.Trim()))
                {
                    
                    MessageBoxEx.Show("请设置提取路径！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    //g_clsErrorHandle.DisplayInformation("没有设置投影参数！请选择*.prj文件！", false);
                    MessageBoxEx.Show("没有设置投影参数！请选择*.prj文件！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            pWorkspace = GetWorkspace();

            if (pWorkspace == null)
                return;

            this.ProgressBar.Visible = true;

            // 创建PrjLog.txt到bin\Log文件夹
            sTxtPath = System.Windows.Forms.Application.StartupPath.Replace("\\bin", "");
            bExist = Directory.Exists(sTxtPath + "\\Log");
            if (!bExist)
            {
                Directory.CreateDirectory(sTxtPath + "\\Log");
            }
            FileStream fsStream = new FileStream(sTxtPath + "\\Log" + "\\PrjLog.txt", FileMode.Append, FileAccess.Write);
            StreamWriter swWriter = new StreamWriter(fsStream);
            swWriter.WriteLine("下列要素类不能投影到指定坐标系，转换失败，要素类创建错误！");
            swWriter.WriteLine("记录数" + Strings.Chr(9) + "要素类路径");

            ////批量投影转换并提取
            for (i = 0; i <= m_PathCollection.Count - 1; i++)
            {
                //// 获得要素类
                m_pFeatureCls = (IFeatureClass)m_FeatClsCollection[i];
                sDesFeatClsName = m_pFeatureCls.AliasName;
                sInfo = "当前操作层：" + m_pFeatureCls.AliasName;
                this.lblInfo.Text = sInfo;
                Application.DoEvents();
                if (!string.IsNullOrEmpty(sDesFeatClsName.Trim()))
                {
                    if (m_pFeatureCls != null)
                    {
                        this.lblInfo.Text = sInfo + "正在获得目标要素类，请稍候....";
                        lblInfo.Refresh();
                        Application.DoEvents();
                        ////读prj，获得空间参考
                        if (m_bHasSpatRef == true)
                        {
                            pSpatRef = ClsPublicFunction.LoadPRJ(txtPreDefinedPrj.Text);
                            if (sDesFeatClsName.Contains("."))
                            {
                                int dotIndex = 0;
                                dotIndex = sDesFeatClsName.IndexOf(".");
                                sDesFeatClsName = sDesFeatClsName.Substring(dotIndex + 1, sDesFeatClsName.Length - dotIndex - 1);
                            }
                            IFeatureWorkspace pFeatureWorkspace = (IFeatureWorkspace)pWorkspace;
                            IFields pFields = m_pFeatureCls.Fields;
                            pDesFeatCls = ClsCreatAndGetDataset.CreatOrOpenFeatClsByName(ref pFeatureWorkspace, sDesFeatClsName, ref pFields, pSpatRef, null);
                            pWorkspace = (IWorkspace)pFeatureWorkspace;
                            //m_pFeatureCls.Fields = pFields;
                        }
                        else
                        {
                            if (sDesFeatClsName.Contains("."))
                            {
                                int dotIndex = 0;
                                dotIndex = sDesFeatClsName.IndexOf(".");
                                sDesFeatClsName = sDesFeatClsName.Substring(dotIndex + 1, sDesFeatClsName.Length - dotIndex - 1);
                            }
                            IFeatureWorkspace pFeatureWorkspace = (IFeatureWorkspace)pWorkspace;
                            IFields pFields = m_pFeatureCls.Fields;
                            pDesFeatCls = ClsCreatAndGetDataset.CreatOrOpenFeatClsByName(ref pFeatureWorkspace, sDesFeatClsName, ref pFields, null, null);
                            pWorkspace = (IWorkspace)pFeatureWorkspace;
                            //m_pFeatureCls.Fields = pFields;
                        }
                        if (pDesFeatCls != null)
                        {
                            //如果不剪切的，则使用该方法，需要剪切，则使用选择集，分位于内部要素和位于外部要素或直接使用pfeatcls搜索二遍
                            //如果数据量大的话，搜索二遍的方法是不可行的
                            m_lResult = 0;
                            if (m_bHasSpatRef == true)
                            {
                                m_lResult = ClsCreateFeat.OutPutFeat(ref pDesFeatCls, ref m_pFeatureCls, "", null, false, ref pSpatRef, null, ref this.ProgressBar, ref this.lblInfo, sInfo);
                            }
                            else
                            {
                                ISpatialReference pSpatialReference = null;
                                ClsCreateFeat.OutPutFeat(ref pDesFeatCls, ref m_pFeatureCls, "", null, false, ref pSpatialReference, null, ref this.ProgressBar, ref this.lblInfo, sInfo);
                            }

                            // 投影转换失败则将日志写入PrjLog.txt
                            if (m_lResult == -1)
                            {
                                m_bAllTransferDone = false;

                                swWriter.WriteLine(Convert.ToInt32(m_iTNum).ToString() + Strings.Chr(9).ToString() + Strings.Chr(9).ToString() + m_PathCollection[i].ToString());
                                m_iTNum += 1;
                            }

                            ////  更新featureclass的范围
                            ISchemaLock schemaLock = default(ISchemaLock);
                            schemaLock = (ISchemaLock)pDesFeatCls;

                            IFeatureClassManage featureClassManage = default(IFeatureClassManage);
                            featureClassManage = (IFeatureClassManage)pDesFeatCls;
                            featureClassManage.UpdateExtent();
                            schemaLock.ChangeSchemaLock(esriSchemaLock.esriSharedSchemaLock);
                        }
                    }
                }
            }
            lblInfo.Text = "投影转换完成";
            m_iTNum = 1;
            //要素类记录数归1

            this.Refresh();
            Application.DoEvents();

            this.ProgressBar.Visible = false;
           
            MessageBoxEx.Show("投影转换完成！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            txtSaveFileRoute.Text = "";


            //写入转换完成时间
            DateTime Now = DateTime.Now;
            swWriter.Write("转换完成时间：" + Strings.Chr(9));
            swWriter.Write(Strings.Chr(9) + Now.ToString() + Constants.vbCrLf);
            swWriter.WriteLine();
            swWriter.Close();
            fsStream.Close();
            // 有转换不成功时，弹出提示信息
            if (m_bAllTransferDone == false)
            {
                
                MessageBoxEx.Show("有要素类投影转换失败！详情请查看\"Log\\PrjLog.txt\"！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            m_bAllTransferDone = true;
        }

    }
}
