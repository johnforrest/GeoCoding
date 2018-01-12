using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.DataSourcesGDB;
using System.Xml;
using ZJGISOpenData.Forms;
using ESRI.ArcGIS.DataSourcesFile;
using System.Resources;
using System.Reflection;
using ESRI.ArcGIS.Geometry;
using System.Collections.ObjectModel;
using ZJGISCommon;
using DevComponents.DotNetBar;
using System.IO;
using DevComponents.DotNetBar.Controls;
using ZJGISDataUpdating.Class;

namespace ZJGISDataUpdating
{
    class UpdataDataAdd
    {

        //   Form previousForm;
        //    Dictionary<int, DataGridViewRow> m_InRowDic=new Dictionary<int,DataGridViewRow> ();//前一窗体的表格内容.原图层
        private static Dictionary<int, DataGridViewRow> m_OutRowDic;//传出内容，工作层
        public Dictionary<int, DataGridViewRow> OutRowDic
        {
            get { return m_OutRowDic; }
        }
        private static TextBoxX textBoxX1;
        public TextBoxX TextBoxX1
        {
            set { textBoxX1 = value; }
        }
        IWorkspace m_SDEWorkspace;

        private static DataGridViewX dataGridViewX1;
        public DataGridViewX DGVUpdata
        {
            set { dataGridViewX1 = value; }
        }



        public void FrmSetMatchEx_Load()
        {

            double width = dataGridViewX1.Width;

            DataGridViewCheckBoxColumn dgvCheckBoxColumn = new DataGridViewCheckBoxColumn();
            dgvCheckBoxColumn.HeaderText = "状态";
            dgvCheckBoxColumn.Width = Convert.ToInt32(width * 0.1); ;
            dataGridViewX1.Columns.Add(dgvCheckBoxColumn);


            dataGridViewX1.Columns.Add("SourceFileName", "工作层名称");
            dataGridViewX1.Columns[1].ReadOnly = true;
            dataGridViewX1.Columns[1].Width = Convert.ToInt32(width * 0.2);

            DataGridViewImageColumn dgvImageColumn = new DataGridViewImageColumn();
            dgvImageColumn.HeaderText = "几何类型";
            dgvImageColumn.ReadOnly = true;
            dgvImageColumn.Width = Convert.ToInt32(width * 0.2);
            dataGridViewX1.Columns.Add(dgvImageColumn);

            dataGridViewX1.Columns.Add("Path", "路径");
            dataGridViewX1.Columns[3].ReadOnly = true;
            dataGridViewX1.Columns[3].Width = Convert.ToInt32(width * 0.5);

        }
        public void buttonXSetDb_Click()
        {
            ZJGISCommon.FrmSDE frmSDE = new ZJGISCommon.FrmSDE();
            frmSDE.ShowDialog();
            m_SDEWorkspace = frmSDE.SDEWorkspace as IWorkspace;
            if (m_SDEWorkspace == null)
            {
                MessageBoxEx.Show("SDE连接失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            frmSDE.Close();
        }

        /// <summary>
        /// updateDataAdd的下一步
        /// </summary>
        public void buttonXNext_Click()
        {
            int count = 0;
            if (dataGridViewX1.Rows.Count > 0)
            {
                m_OutRowDic = new Dictionary<int, DataGridViewRow>();

                for (int i = 0; i < dataGridViewX1.Rows.Count; i++)
                {
                    if (dataGridViewX1.Rows[i].Visible == true && Convert.ToBoolean(dataGridViewX1[0, i].Value))
                    {
                        //记录待更新图层工作路径
                        ClsDeclare.g_WorkFeatClsPathDic = new Dictionary<string, string>();
                        ClsDeclare.g_WorkFeatClsPathDic.Add(dataGridViewX1[1, i].Value.ToString(), dataGridViewX1[3, i].Value.ToString());

                        ClsDeclare.g_WorkspacePath = dataGridViewX1[3, i].Value.ToString();

                        m_OutRowDic.Add(count, dataGridViewX1.Rows[i]);
                        count++;
                    }
                }
                if (count == 0)
                {
                    MessageBoxEx.Show("请加载更新数据！");
                    return;
                }
            }
            else
            {
                MessageBoxEx.Show("请加载更新数据！");
                return;
            }
        }

        #region 连接SDE
        //private void buttonXConnectDb_Click(object sender, EventArgs e)
        //{
        //    //m_PropertySet = new PropertySetClass();
        //    //m_PropertySet.SetProperty("Server", "192.168.202.196");
        //    //m_PropertySet.SetProperty("Instance", "5151");
        //    //m_PropertySet.SetProperty("Database", "orcl");
        //    //m_PropertySet.SetProperty("user", "sde");
        //    //m_PropertySet.SetProperty("password", "lgq");
        //    //m_PropertySet.SetProperty("version", "SDE.DEFAULT");

        //    IWorkspaceFactory workspaceFactory = new SdeWorkspaceFactoryClass();
        //    IWorkspace workspace = null;
        //    try
        //    {
        //        workspace = ClsDBInfo.SdeWorkspace;
        //    }
        //    catch (Exception)
        //    {

        //    }
        //    if (workspace != null)
        //    {
        //        m_SDEWorkspace = workspace;
        //        MessageBoxEx.Show("SDE连接成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

        //        //if (ClsDeclare.g_SourceToBase || ClsDeclare.g_BaseToFrame && !ClsDeclare.g_FrameToFrame)
        //        //{
        //            this.buttonXSameSetMatchData.Enabled = true;
        //        //}
        //        //else if (!ClsDeclare.g_SourceToBase && !ClsDeclare.g_BaseToFrame && ClsDeclare.g_FrameToFrame)
        //        //{
        //        //    this.buttonXDifSetMatchData.Enabled = true;
        //        //}
        //    }
        //    else
        //    {
        //        this.buttonXSetDb.Enabled = true;
        //        MessageBoxEx.Show("SDE连接失败，请重新配置！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //    }

        //}
        #endregion

        private IFeatureClass SearchMatchedFeatCls(IWorkspace workspace, string subFeatureName, XmlDocument xmlDoc)
        {
            try
            {
                IFeatureWorkspace featWorkspace = workspace as IFeatureWorkspace;
                IFeatureClass featureClass = null;
                string fileName = "";

                XmlNode xmlNode = xmlDoc.SelectSingleNode("/Root/Scale1W");
                XmlNodeList nodeList = xmlNode.ChildNodes;

                foreach (XmlNode node in nodeList)
                {
                    string text = node.Attributes["TEXT"].Value.ToString();
                    if (text == subFeatureName)
                    {
                        fileName = node.Attributes["NAME"].Value.ToString();
                        break;
                    }
                }
                if (fileName == "")
                {
                    MessageBox.Show("数据库中没有找到对应待更新图层名，请从新配置源图层文件！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }

                XmlNode root = xmlDoc.DocumentElement;
                string user = root.Attributes["USER"].Value.ToString();
                string sdefileName = user + "." + fileName;

                XmlNode xmlDataset = xmlDoc.SelectSingleNode("/Root/Scale1W/FeatureDataset[@TEXT='1万框架库']");
                string datasetName = xmlDataset.Attributes["NAME"].Value.ToString();
                IFeatureDataset featureDataset = featWorkspace.OpenFeatureDataset(datasetName);
                IEnumDataset enumDataset = featureDataset.Subsets;
                enumDataset.Reset();
                IDataset dataset = enumDataset.Next();
                while (dataset != null)
                {
                    if (dataset.Type == esriDatasetType.esriDTFeatureClass)
                    {
                        if (dataset.Name == sdefileName)
                        {
                            featureClass = dataset as IFeatureClass;
                            return featureClass;
                        }
                    }
                    dataset = enumDataset.Next();
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private IFeatureClass SearchMatchedFeatClsForBase(IWorkspace workspace, string subFeatureName, XmlDocument xmlDoc)
        {
            try
            {
                IFeatureWorkspace featWorkspace = workspace as IFeatureWorkspace;
                IFeatureClass featureClass = null;
                string fileName = "";

                XmlNode xmlNode = xmlDoc.SelectSingleNode("/Root/BASEDATA");
                XmlNodeList nodeList = xmlNode.ChildNodes;

                foreach (XmlNode node in nodeList)
                {
                    string text = node.Attributes["TEXT"].Value.ToString();
                    if (text == subFeatureName)
                    {
                        fileName = node.Attributes["NAME"].Value.ToString();
                        break;
                    }
                }
                if (fileName == "")
                {
                    MessageBox.Show("数据库中没有找到对应待更新图层名，请从新配置源图层文件！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }

                XmlNode root = xmlDoc.DocumentElement;
                string user = root.Attributes["USER"].Value.ToString();
                string sdefileName = user + "." + fileName;

                XmlNode xmlDataset = xmlDoc.SelectSingleNode("/Root/BASEDATA/FeatureDataset[@TEXT='基础库']");
                string datasetName = xmlDataset.Attributes["NAME"].Value.ToString();
                IFeatureDataset featureDataset = featWorkspace.OpenFeatureDataset(datasetName);
                IEnumDataset enumDataset = featureDataset.Subsets;
                enumDataset.Reset();
                IDataset dataset = enumDataset.Next();
                while (dataset != null)
                {
                    if (dataset.Type == esriDatasetType.esriDTFeatureClass)
                    {
                        if (dataset.Name == sdefileName)
                        {
                            featureClass = dataset as IFeatureClass;
                            return featureClass;
                        }
                    }
                    dataset = enumDataset.Next();
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private IFeatureClass SearchMatchedFeatCls(IWorkspace workspace, string subFeatureName, string scale, XmlDocument xmlDoc)
        {
            try
            {
                IFeatureWorkspace featWorkspace = workspace as IFeatureWorkspace;
                IFeatureClass featureClass = null;
                string fileName = "";
                XmlNode xmlNode;
                XmlNodeList nodeList = null;

                XmlNode xmlDataset;
                string datasetName = "";

                if (scale == "1万")
                {
                    xmlNode = xmlDoc.SelectSingleNode("/Root/Scale1W");
                    nodeList = xmlNode.ChildNodes;

                    xmlDataset = xmlDoc.SelectSingleNode("/Root/Scale1W/FeatureDataset[@TEXT='1万框架库']");
                    datasetName = xmlDataset.Attributes["NAME"].Value.ToString();
                }
                else if (scale == "5万")
                {
                    xmlNode = xmlDoc.SelectSingleNode("/Root/Scale5W");
                    nodeList = xmlNode.ChildNodes;

                    xmlDataset = xmlDoc.SelectSingleNode("/Root/Scale5W/FeatureDataset[@TEXT='5万框架库']");
                    datasetName = xmlDataset.Attributes["NAME"].Value.ToString();
                }
                foreach (XmlNode node in nodeList)
                {
                    string text = node.Attributes["TEXT"].Value.ToString();
                    if (text == subFeatureName)
                    {
                        fileName = node.Attributes["NAME"].Value.ToString();
                        break;
                    }
                }
                if (fileName == "")
                {
                    MessageBox.Show("数据库中没有找到对应待更新图层名，请从新配置源图层文件！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }

                XmlNode root = xmlDoc.DocumentElement;
                string user = root.Attributes["USER"].Value.ToString();
                string sdefileName = user + "." + fileName;

                IFeatureDataset featureDataset = featWorkspace.OpenFeatureDataset(datasetName);
                IEnumDataset enumDataset = featureDataset.Subsets;
                enumDataset.Reset();
                IDataset dataset = enumDataset.Next();
                while (dataset != null)
                {
                    if (dataset.Type == esriDatasetType.esriDTFeatureClass)
                    {
                        if (dataset.Name == sdefileName)
                        {
                            featureClass = dataset as IFeatureClass;
                            return featureClass;
                        }
                    }
                    dataset = enumDataset.Next();
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 加载待更新的数据
        /// </summary>
        public void btnAddData_Click()
        {
            //string path = ClsDeclare.g_WorkspacePath;
            //if (path == null || path == "")
            //{
            //    MessageBox.Show("请先加在源数据！");
            //    return;
            //}
            FrmOpenData frmOpenData = new FrmOpenData();

            //frmOpenData.IsUpdateDate = true;
            if (frmOpenData.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }
            string pathName = frmOpenData.PathName;
            Collection<object> featClsCol = new Collection<object>();
            featClsCol = frmOpenData.FeatClsCollection;
            ResourceManager rm = new ResourceManager("ZJGISDataUpdating.Properties.Resources", Assembly.GetExecutingAssembly());
            if (featClsCol.Count > 0)
            {
                for (int i = 0; i < featClsCol.Count; i++)
                {
                    IFeatureClass pNewFeatCls = featClsCol[i] as IFeatureClass;
                    string fileName = pNewFeatCls.AliasName;
                    DataGridViewRow dgvRow = new DataGridViewRow();
                    dgvRow = dataGridViewX1.Rows[dataGridViewX1.Rows.Add()];

                    if (pNewFeatCls.ShapeType == esriGeometryType.esriGeometryPolygon)
                    {
                        dgvRow.Cells[2].Value = (Bitmap)rm.GetObject("polygon");
                        dgvRow.Cells[2].Tag = "面";
                    }
                    else if (pNewFeatCls.ShapeType == esriGeometryType.esriGeometryPolyline || pNewFeatCls.ShapeType == esriGeometryType.esriGeometryLine)
                    {
                        dgvRow.Cells[2].Value = (Bitmap)rm.GetObject("line");
                        dgvRow.Cells[2].Tag = "线";
                    }
                    else if (pNewFeatCls.ShapeType == esriGeometryType.esriGeometryMultipoint || pNewFeatCls.ShapeType == esriGeometryType.esriGeometryPoint)
                    {
                        dgvRow.Cells[2].Value = (Bitmap)rm.GetObject("point");
                        dgvRow.Cells[2].Tag = "点";
                    }
                    else
                    {
                        MessageBox.Show("请加载正确格式的数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        dataGridViewX1.Rows.Remove(dgvRow);
                        return;
                    }

                    DataGridViewCheckBoxCell dgvCheckBoxCell = new DataGridViewCheckBoxCell();
                    dgvCheckBoxCell = dgvRow.Cells[0] as DataGridViewCheckBoxCell;
                    dgvCheckBoxCell.Value = true;

                    dgvRow.Cells[1].Value = fileName;
                    dgvRow.Cells[1].Tag = pNewFeatCls;
                    dgvRow.Cells[3].Value = pathName;

                    ////生成工作层
                    //IWorkspaceFactory2 pWorkspaceFactory = new FileGDBWorkspaceFactory() as IWorkspaceFactory2;

                    //IWorkspace2 workspace;
                    //string temp = path.Substring(path.LastIndexOf(".") + 1);
                    //if (temp.ToUpper() == "GDB")
                    //{
                    //    workspace = pWorkspaceFactory.OpenFromFile(path, 0) as IWorkspace2;
                    //}
                    //else
                    //{
                    //    if (Directory.Exists(path + @"\temp.gdb"))
                    //    {
                    //        workspace = pWorkspaceFactory.OpenFromFile(path + @"\temp.gdb", 0) as IWorkspace2;
                    //        ClsDeclare.g_WorkspacePath = path + @"\temp.gdb";
                    //    }
                    //    else
                    //    {
                    //        IWorkspaceName workspaceName = pWorkspaceFactory.Create(path, "temp", null, 0);
                    //        IName name = workspaceName as IName;
                    //        workspace = name.Open() as IWorkspace2;
                    //        ClsDeclare.g_WorkspacePath = path + @"\temp.gdb";

                    //    }
                    //}

                    //
                    if (dataGridViewX1.Rows.Count > 0)
                    {
                        for (int j = 0; j < dataGridViewX1.Rows.Count; j++)
                        {
                            if (dataGridViewX1.Rows[j].Visible == true)
                            {
                                string path = dataGridViewX1[3, j].Value.ToString();
                                if (path.Contains("."))
                                {
                                    string type = path.Substring(path.LastIndexOf(".") + 1, 3);
                                    if (type.ToUpper() == "GDB")
                                    {
                                        textBoxX1.Text = path.Substring(0, path.LastIndexOf(".")) + ".gdb";
                                    }
                                }
                                else
                                {
                                    textBoxX1.Text = dataGridViewX1[3, j].Value.ToString();
                                }

                            }

                        }
                    }

                    ClsDeclare.g_WorkspacePath = textBoxX1.Text;
                }
            }
        }
        /// <summary>
        /// 删除待更新数据
        /// </summary>
        public void buttonXDelData_Click()
        {
            if (dataGridViewX1.SelectedRows.Count > 0)
            {
                for (int i = 0; i < dataGridViewX1.SelectedRows.Count; i++)
                {
                    dataGridViewX1.Rows.Remove(dataGridViewX1.SelectedRows[i]);
                }

            }
        }
        /// <summary>
        /// 获得最终的路径
        /// </summary>
        public void buttonXBrowserPath_Click()
        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            if (folderDlg.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            if (folderDlg.SelectedPath != null)
            {
                textBoxX1.Text = folderDlg.SelectedPath;
            }
        }
    }
}
