using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevComponents.DotNetBar;
using System.Windows.Forms;
using ZJGISOpenData.Forms;
using System.Collections.ObjectModel;
using System.Resources;
using System.Reflection;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesFile;
using DevComponents.DotNetBar.Controls;
using System.Drawing;
using ZJGISCommon;
using ESRI.ArcGIS.esriSystem;
using ZJGISDataUpdating.Class;

namespace ZJGISDataUpdating
{
    class SourceDataAdd
    {
        private static DataGridViewX dataGridViewX1;
        public DataGridViewX DGVSource
        {
            set { dataGridViewX1 = value; }
        }

        private static Dictionary<int, DataGridViewRow> m_InRowDic;//源数据

        public Dictionary<int, DataGridViewRow> InRowDic
        {
            get { return m_InRowDic; }
        }

        /// <summary>
        /// 加载源数据按钮，更新数据按钮
        /// </summary>
        public void buttonXAddData_Click()
        {
            FrmOpenData frmOpenData = new FrmOpenData();
            if (frmOpenData.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }
            string pathName = frmOpenData.PathName;

            Collection<object> featClsCol = new Collection<object>();
            //获取选中的FeatureClass集合
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

                    //设置选中的实体的类型图标
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


                }
            }
            //if (dataGridViewX1.Rows.Count > 0)
            //{
            //    for (int i = 0; i < dataGridViewX1.Rows.Count; i++)
            //    {
            //        if (dataGridViewX1.Rows[i].Visible == true)
            //        {
            //            string path = dataGridViewX1[3, i].Value.ToString();
            //            if (path.Contains("."))
            //            {
            //                string type = path.Substring(path.LastIndexOf(".") + 1, 3);
            //                if (type.ToUpper() == "GDB")
            //                {
            //                   textBoxX1.Text = path.Substring(0, path.LastIndexOf(".")) + ".gdb";
            //                }
            //            }
            //            else
            //            {
            //                textBoxX1.Text = dataGridViewX1[3, i].Value.ToString();
            //            }
            //            break;
            //        }

            //    }
            //}

            //ClsDeclare.g_WorkspacePath = textBoxX1.Text;
        }
        public void FrmConfigMatchEnv_Load()
        {

        }

        /// <summary>
        /// 删除添加的数据
        /// </summary>
        public void buttonXDelData_Click()
        {
            if (dataGridViewX1.SelectedRows.Count > 0)
            {
                for (int i = 0; i < dataGridViewX1.SelectedRows.Count; i++)
                {
                    dataGridViewX1.Rows.Remove(dataGridViewX1.SelectedRows[i]);
                }
                //int count = 0;
                //for (int j = 0; j < dataGridViewX1.Rows.Count; j++)
                //{
                //    if (dataGridViewX1.Rows[j].Visible == true)
                //    {
                //        textBoxX1.Text = dataGridViewX1[3, j].Value.ToString();
                //        count++;
                //        break;
                //    }
                //}
                //if (count == 0)
                //{
                //    textBoxX1.Text = "";
                //}
            }
        }

        /// <summary>
        /// sourceDataAdd的下一步操作
        /// </summary>
        public void buttonXNext_Click()
        {
            if (dataGridViewX1.RowCount > 0)
            {
                m_InRowDic = new Dictionary<int, DataGridViewRow>();
                int count = 0;

                for (int i = 0; i < dataGridViewX1.RowCount; i++)
                {
                    if (dataGridViewX1.Rows[i].Visible == true && Convert.ToBoolean(dataGridViewX1[0, i].Value))
                    {
                        //记录源图层工作路径，只记录一次
                        ClsDeclare.g_SourceFeatClsPathDic = new Dictionary<string, string>();
                        ClsDeclare.g_SourceFeatClsPathDic.Add(dataGridViewX1[1, i].Value.ToString(), dataGridViewX1[3, i].Value.ToString());

                        m_InRowDic.Add(count, dataGridViewX1.Rows[i]);
                        count++;
                    }
                }
                if (count == 0)
                {
                    MessageBoxEx.Show("请加载源数据！");
                    return;
                }
                //pathWorkspace = textBoxX1.Text;
            }
            else
            {
                MessageBoxEx.Show("请加载源数据！");
                return;
            }
        }

        public void AddAllFeatureClass()
        {
            string txtPath = System.Windows.Forms.Application.StartupPath + "\\..\\Res\\path\\ExtractInfo.txt";
            SaveInfoToTxt saveInfoTxt = new SaveInfoToTxt();
            string WorkSpacePath = "";
            saveInfoTxt.ReadTxt(txtPath, out WorkSpacePath);
            WorkSpacePath = WorkSpacePath.Trim();
            IWorkspaceFactory workspaceFactory = new ESRI.ArcGIS.DataSourcesGDB.FileGDBWorkspaceFactoryClass();
            IWorkspace workSpace = workspaceFactory.OpenFromFile(WorkSpacePath, 0);
            IDatasetName datasetName;
            IEnumDatasetName enumDatasetNameFeature = workSpace.get_DatasetNames(esriDatasetType.esriDTFeatureClass);
            datasetName = null;
            while ((datasetName = enumDatasetNameFeature.Next()) != null)
            {
                if (datasetName.Type == esriDatasetType.esriDTFeatureClass)
                {
                    ResourceManager rm = new ResourceManager("ZJGISDataUpdating.Properties.Resources", Assembly.GetExecutingAssembly());
                    IName name = datasetName as IName;
                    IFeatureClass featureClass = name.Open() as IFeatureClass;

                    string fileName = featureClass.AliasName;

                    DataGridViewRow dgvRow = new DataGridViewRow();
                    dgvRow = dataGridViewX1.Rows[dataGridViewX1.Rows.Add()];

                    if (featureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                    {
                        dgvRow.Cells[2].Value = (Bitmap)rm.GetObject("polygon");
                        dgvRow.Cells[2].Tag = "面";
                    }
                    else if (featureClass.ShapeType == esriGeometryType.esriGeometryPolyline || featureClass.ShapeType == esriGeometryType.esriGeometryLine)
                    {
                        dgvRow.Cells[2].Value = (Bitmap)rm.GetObject("line");
                        dgvRow.Cells[2].Tag = "线";
                    }
                    else if (featureClass.ShapeType == esriGeometryType.esriGeometryMultipoint || featureClass.ShapeType == esriGeometryType.esriGeometryPoint)
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
                    dgvRow.Cells[1].Tag = featureClass;
                    dgvRow.Cells[3].Value = WorkSpacePath;

                }
            }
        }
    }
}
