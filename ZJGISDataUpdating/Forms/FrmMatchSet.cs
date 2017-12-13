using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ZJGISCommon;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using System.Resources;
using System.Reflection;
using ZJGISDataUpdating.Class;

namespace ZJGISDataUpdating
{
    public partial class FrmMatchSet : DevComponents.DotNetBar.Office2007Form
    {
        public FrmMatchSet()
        {
            InitializeComponent();
        }
        //记住临时路径
        string temppath = string.Empty;

        SourceDataAdd sourceDataAdd;
        UpdataDataAdd targetDataAdd;

        private void FrmMatchSet_Load(object sender, EventArgs e)
        {
            sourceDataAdd = new SourceDataAdd();
            sourceDataAdd.DGVSource = dgvSource;

            targetDataAdd = new UpdataDataAdd();
            targetDataAdd.TextBoxX1 = txtWorkPath;
            targetDataAdd.DGVUpdata = dgvTarget;

            //源数据
            double width1 = dgvSource.Width;
            DataGridViewCheckBoxColumn dgvCheckBoxColumn1 = new DataGridViewCheckBoxColumn();
            dgvCheckBoxColumn1.HeaderText = "状态";
            dgvCheckBoxColumn1.Width = Convert.ToInt32(width1 * 0.1); ;
            dgvSource.Columns.Add(dgvCheckBoxColumn1);

            dgvSource.Columns.Add("SourceFileName", "源图层名称");
            dgvSource.Columns[1].ReadOnly = true;
            dgvSource.Columns[1].Width = Convert.ToInt32(width1 * 0.2);

            DataGridViewImageColumn dgvImageColumn = new DataGridViewImageColumn();
            dgvImageColumn.HeaderText = "几何类型";
            dgvImageColumn.ReadOnly = true;
            dgvImageColumn.Width = Convert.ToInt32(width1 * 0.2);
            dgvSource.Columns.Add(dgvImageColumn);

            dgvSource.Columns.Add("Path", "路径");
            dgvSource.Columns[3].ReadOnly = true;
            dgvSource.Columns[3].Width = Convert.ToInt32(width1 * 0.5);
            dgvSource.AllowUserToAddRows = false;


            //sourceDataAdd.FrmConfigMatchEnv_Load();
            targetDataAdd = new UpdataDataAdd();
            targetDataAdd.DGVUpdata = dgvTarget;

            double width2 = dgvTarget.Width;

            DataGridViewCheckBoxColumn dgvCheckBoxColumn2 = new DataGridViewCheckBoxColumn();
            dgvCheckBoxColumn2.HeaderText = "状态";
            dgvCheckBoxColumn2.Width = Convert.ToInt32(width2 * 0.1); ;
            dgvTarget.Columns.Add(dgvCheckBoxColumn2);


            //dgvUpdata.Columns.Add("SourceFileName", "工作层名称");
            dgvTarget.Columns.Add("SourceFileName", "待匹配图层名称");
            dgvTarget.Columns[1].ReadOnly = true;
            dgvTarget.Columns[1].Width = Convert.ToInt32(width2 * 0.2);

            DataGridViewImageColumn dgvImageColumn2 = new DataGridViewImageColumn();
            dgvImageColumn2.HeaderText = "几何类型";
            dgvImageColumn2.ReadOnly = true;
            dgvImageColumn2.Width = Convert.ToInt32(width2 * 0.2);
            dgvTarget.Columns.Add(dgvImageColumn2);

            dgvTarget.Columns.Add("Path", "路径");
            dgvTarget.Columns[3].ReadOnly = true;
            dgvTarget.Columns[3].Width = Convert.ToInt32(width2 * 0.5);
            // updataDataAdd.FrmSetMatchEx_Load();


            IMapControl4 mapControl = ClsControl.MapControlMain;
            ResourceManager rm = new ResourceManager("ZJGISDataUpdating.Properties.Resources", Assembly.GetExecutingAssembly());
            for (int i = 0; i < mapControl.LayerCount; i++)
            {
                IFeatureLayer featureLayer = mapControl.get_Layer(i) as IFeatureLayer;
                IFeatureClass featureClass = featureLayer.FeatureClass;
                IWorkspace workspace = (featureLayer.FeatureClass as IDataset).Workspace;

                if (workspace.Type != esriWorkspaceType.esriRemoteDatabaseWorkspace)
                {
                    //string fileName = featureClass.AliasName;
                    //string fileName = featureClass.;
                    string fileName = (featureClass as IDataset).Name;

                    DataGridViewRow dgvRowSource = new DataGridViewRow();
                    DataGridViewRow dgvRowTarget = new DataGridViewRow();
                    dgvRowSource = dgvSource.Rows[dgvSource.Rows.Add()];
                    dgvRowTarget = dgvTarget.Rows[dgvTarget.Rows.Add()];
                    if (featureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                    {
                        dgvRowSource.Cells[2].Value = (Bitmap)rm.GetObject("polygon");
                        dgvRowSource.Cells[2].Tag = "面";

                        dgvRowTarget.Cells[2].Value = (Bitmap)rm.GetObject("polygon");
                        dgvRowTarget.Cells[2].Tag = "面";
                    }
                    else if (featureClass.ShapeType == esriGeometryType.esriGeometryPolyline || featureClass.ShapeType == esriGeometryType.esriGeometryLine)
                    {
                        dgvRowSource.Cells[2].Value = (Bitmap)rm.GetObject("line");
                        dgvRowSource.Cells[2].Tag = "线";

                        dgvRowTarget.Cells[2].Value = (Bitmap)rm.GetObject("line");
                        dgvRowTarget.Cells[2].Tag = "线";
                    }
                    else if (featureClass.ShapeType == esriGeometryType.esriGeometryMultipoint || featureClass.ShapeType == esriGeometryType.esriGeometryPoint)
                    {
                        dgvRowSource.Cells[2].Value = (Bitmap)rm.GetObject("point");
                        dgvRowSource.Cells[2].Tag = "点";
                        dgvRowTarget.Cells[2].Value = (Bitmap)rm.GetObject("point");
                        dgvRowTarget.Cells[2].Tag = "点";
                    }
                    else
                    {
                        MessageBoxEx.Show("请加载正确格式的数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        dgvSource.Rows.Remove(dgvRowSource);
                        dgvTarget.Rows.Remove(dgvRowSource);
                        return;
                    }

                    DataGridViewCheckBoxCell dgvCheckBoxCell = new DataGridViewCheckBoxCell();
                    dgvCheckBoxCell = dgvRowSource.Cells[0] as DataGridViewCheckBoxCell;
                    dgvCheckBoxCell.Value = false;

                    dgvRowSource.Cells[1].Value = fileName;
                    dgvRowSource.Cells[1].Tag = featureClass;
                    dgvRowSource.Cells[3].Value = workspace.PathName;

                    DataGridViewCheckBoxCell dgvCheckBoxCellUpdata = new DataGridViewCheckBoxCell();
                    dgvCheckBoxCellUpdata = dgvRowSource.Cells[0] as DataGridViewCheckBoxCell;
                    dgvCheckBoxCellUpdata.Value = false;

                    dgvRowTarget.Cells[1].Value = fileName;
                    dgvRowTarget.Cells[1].Tag = featureClass;
                    dgvRowTarget.Cells[3].Value = workspace.PathName;
                }
            }


        }

        /// <summary>
        /// 加载源数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddSourceData_Click(object sender, EventArgs e)//加载源数据
        {
            sourceDataAdd.buttonXAddData_Click();
        }
        /// <summary>
        /// 删除源数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelSourceData_Click(object sender, EventArgs e)
        {
            sourceDataAdd.buttonXDelData_Click();
        }
        /// <summary>
        ///加载更新数据源
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddUpdataData_Click(object sender, EventArgs e)
        {
            targetDataAdd.btnAddData_Click();
        }
        /// <summary>
        /// 删除更新数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelUpdataData_Click(object sender, EventArgs e)
        {
            targetDataAdd.buttonXDelData_Click();
        }
        /// <summary>
        /// 浏览（匹配工作区）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBrowserPath_Click(object sender, EventArgs e)
        {
            targetDataAdd.buttonXBrowserPath_Click();
        }
        //TODO :数据集中的下一步操作
        /// <summary>
        /// 下一步
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNext_Click(object sender, EventArgs e)
        {
            try
            {
                sourceDataAdd.buttonXNext_Click();
                targetDataAdd.buttonXNext_Click();

                IFeatureClass pSourceFeatCls = sourceDataAdd.InRowDic[0].Cells[1].Tag as IFeatureClass;
                IFeatureClass pTarFeatCls = targetDataAdd.OutRowDic[0].Cells[1].Tag as IFeatureClass;
                //如果源图层和待匹配图层的类型不一样
                if (pSourceFeatCls.ShapeType != pTarFeatCls.ShapeType)
                {
                    FrmDifShape frmDifShape = new FrmDifShape();
                    frmDifShape.InRowDic = sourceDataAdd.InRowDic;
                    frmDifShape.OutRowDic = targetDataAdd.OutRowDic;
                    frmDifShape.PreviousForm = this;
                    this.Visible = false;
                    frmDifShape.ShowDialog();
                }
                //如果源图层和待匹配图层的类型一样
                else
                {
                    if (pSourceFeatCls.ShapeType == esriGeometryType.esriGeometryPolygon)
                    {
                        //不同尺度下的匹配更新
                        if (!ClsDeclare.g_SameScaleMatch && ClsDeclare.g_DifScaleMatch)
                        {
                            // 20170906
                            //Dictionary<string, string> strDic = new Dictionary<string, string>();
                            //strDic.Add("1:500", "0.5k");
                            //strDic.Add("1:2000", "2K");
                            //strDic.Add("1:10000", "10K");
                            //string temStrF = strDic[ClsDeclare.strFrom];
                            //string temStrT = strDic[ClsDeclare.strTo];
                            //test
                            string test = sourceDataAdd.InRowDic[0].Cells[1].Value.ToString();
                            string test2 = targetDataAdd.OutRowDic[0].Cells[1].Value.ToString();

                            //////20170504 注释掉
                            //if (!sourceDataAdd.InRowDic[0].Cells[1].Value.ToString().Contains(temStrF))
                            //{
                            //    MessageBoxEx.Show("请选择正确的尺度数据！");
                            //    return;
                            //}
                            //if (!updataDataAdd.OutRowDic[0].Cells[1].Value.ToString().Contains(temStrT))
                            //{
                            //    MessageBoxEx.Show("请选择正确的尺度数据！");
                            //    return;
                            //}

                            FrmMatchDif frmMatchDif = new FrmMatchDif();
                            frmMatchDif.InRowDic = sourceDataAdd.InRowDic;
                            frmMatchDif.OutRowDic = targetDataAdd.OutRowDic;
                            frmMatchDif.PreviousForm = this;
                            this.Visible = false;
                            frmMatchDif.ShowDialog();
                        }
                        //同尺度下的匹配更新（同比例尺下的匹配更新）
                        if (ClsDeclare.g_SameScaleMatch && !ClsDeclare.g_DifScaleMatch)
                        {
                            FrmMatch frmMatch = new FrmMatch();
                            frmMatch.InRowDic = sourceDataAdd.InRowDic;
                            frmMatch.OutRowDic = targetDataAdd.OutRowDic;
                            frmMatch.PreviousForm = this;
                            this.Visible = false;
                            frmMatch.ShowDialog();
                        }
                    }
                    else if (pSourceFeatCls.ShapeType == esriGeometryType.esriGeometryPolyline)
                    {
                        if (ClsDeclare.g_SameScaleMatch && !ClsDeclare.g_DifScaleMatch)
                        {

                            FrmMatchLine frmMathLine = new FrmMatchLine();
                            frmMathLine.InRowDic = sourceDataAdd.InRowDic;
                            frmMathLine.OutRowDic = targetDataAdd.OutRowDic;
                            frmMathLine.PreviousForm = this;
                            this.Visible = false;
                            frmMathLine.ShowDialog();
                        }
                        //不同比例尺下的
                        else if (!ClsDeclare.g_SameScaleMatch && ClsDeclare.g_DifScaleMatch)
                        {
                            FrmMatchLine frmMathLine = new FrmMatchLine();
                            frmMathLine.InRowDic = sourceDataAdd.InRowDic;
                            frmMathLine.OutRowDic = targetDataAdd.OutRowDic;
                            frmMathLine.PreviousForm = this;
                            this.Visible = false;
                            frmMathLine.ShowDialog();

                            //FrmMatchPolylineParaDifScale frmMatchPolylineParaDifScale = new FrmMatchPolylineParaDifScale();
                            //frmMatchPolylineParaDifScale.DGVCell = cell;
                            //frmMatchPolylineParaDifScale.WorkspacePath = ClsDeclare.g_WorkspacePath;
                            //frmMatchPolylineParaDifScale.TUFeatCls = pTUFeatCls;
                            //frmMatchPolylineParaDifScale.TEFeatCls = pTEFeatCls;
                            //frmMatchPolylineParaDifScale.MatchedFCName = fileName;
                            //frmMatchPolylineParaDifScale.ShowDialog();
                        }

                    }
                    else if (pSourceFeatCls.ShapeType == esriGeometryType.esriGeometryPoint)
                    {
                        //同比例尺下匹配
                        if (ClsDeclare.g_SameScaleMatch && !ClsDeclare.g_DifScaleMatch)
                        {
                            FrmMatchPoint frmMathLine = new FrmMatchPoint();
                            frmMathLine.InRowDic = sourceDataAdd.InRowDic;
                            frmMathLine.OutRowDic = targetDataAdd.OutRowDic;
                            frmMathLine.PreviousForm = this;
                            this.Visible = false;
                            frmMathLine.ShowDialog();
                        }
                        //不同比例尺下匹配
                        else if (!ClsDeclare.g_SameScaleMatch && ClsDeclare.g_DifScaleMatch)
                        {
                            FrmMatchPoint frmMatchPoint = new FrmMatchPoint();
                            frmMatchPoint.InRowDic = sourceDataAdd.InRowDic;
                            frmMatchPoint.OutRowDic = targetDataAdd.OutRowDic;
                            frmMatchPoint.PreviousForm = this;
                            this.Visible = false;
                            frmMatchPoint.ShowDialog();
                        }
                    }
                }
                #region 以前注释
                //int sourceLayerCount = 0;
                //int updataLayerCount = 0;
                //for (int i = 0; i < dgvSource.Rows.Count; i++)
                //{
                //    DataGridViewCheckBoxCell ckb = dgvSource.Rows[i].Cells[0] as DataGridViewCheckBoxCell;
                //    if (Convert.ToBoolean(ckb))
                //    {
                //        sourceLayerCount++;
                //    }
                //}
                //for (int i = 0; i < dgvUpdata.Rows.Count; i++)
                //{
                //    DataGridViewCheckBoxCell ckb = dgvUpdata.Rows[i].Cells[0] as DataGridViewCheckBoxCell;
                //    if (Convert.ToBoolean(ckb))
                //    {
                //        updataLayerCount++;
                //    }
                //}
                //if (sourceLayerCount != 1)
                //{
                //    MessageBoxEx.Show("源数据选择图层为0或两个以上，请重新勾选。", "提示");
                //    return;
                //}
                //if (updataLayerCount != 1)
                //{
                //    MessageBoxEx.Show("更新数据选择图层为0或两个以上，请重新勾选。", "提示");
                //    return;
                //}
                #endregion
            }
            catch (Exception ex)
            {
                this.Visible = true;
                MessageBox.Show(ex.StackTrace);
                MessageBoxEx.Show("请将更新数据放入GDB中再添加！");
            }
        }

        private void btnAddExtractData_Click(object sender, EventArgs e)
        {
            sourceDataAdd.AddAllFeatureClass();
        }

        private void dgvUpdata_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            //DataGridViewCheckBoxCell che = dgvUpdata.Rows[e.RowIndex].Cells[0] as DataGridViewCheckBoxCell;
            ////int ii = dgvUpdata.CurrentRow.Index;
            ////int iii = ;
            ////DataGridViewRow dgvRow = dgvUpdata[0, e.RowIndex];
            //if (Convert.ToBoolean(che.Value))
            //{
            //    txtWorkPath.Text = dgvUpdata.CurrentRow.Cells[3].Value.ToString();
            //}

        }
        int m_che = 0;
        private void dgvUpdata_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewCheckBoxCell che = dgvTarget.Rows[e.RowIndex].Cells[0] as DataGridViewCheckBoxCell;
            object obj = dgvTarget.Rows[e.RowIndex].Cells[0].Value;
            Application.DoEvents();
            if (!Convert.ToBoolean(che.Value))
            {
                txtWorkPath.Text = dgvTarget.CurrentRow.Cells[3].Value.ToString();

            }
        }

        private void txtWorkPath_TextChanged(object sender, EventArgs e)
        {

        }



    }
}
