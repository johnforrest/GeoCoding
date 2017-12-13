using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

using ESRI.ArcGIS.Geodatabase;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using System.Collections.ObjectModel;
using ZJGISCommon;
using ESRI.ArcGIS.DataSourcesGDB;

namespace ZJGISDataUpdating
{
    public partial class FrmUpdate : DevComponents.DotNetBar.Office2007Form
    {
        ITable relationTable;
        IFeatureClass sourceFeatureclass;
        List<string> sourceFields;
        List<string> targetFields;
        string targetFeatureClassName;
        IFeatureWorkspace targeFeatureWorkspace;
        IFeatureClass targetFeatureClass;
        //Dictionary<string, string> attributeRelation;

        public FrmUpdate()
        {
            InitializeComponent();
        }

        private void FrmUpdate_Load(object sender, EventArgs e)
        {
            this.progressBarXUpdate.Visible = false;
            this.labelX3.Visible = false;
        }

        /// <summary>
        /// 浏览源图层
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonXBrowseLayer_Click(object sender, EventArgs e)
        {
            string sourceFeatureclassPath = null;

            ZJGISOpenData.Forms.FrmOpenData frmOpenData = new ZJGISOpenData.Forms.FrmOpenData();

            if (frmOpenData.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            //sourceFeatureclass = ClsUpdateCommon.OpenSourceLayer(out sourceFeatureclassPath);
            Collection<object> featClsCol = new Collection<object>();
            featClsCol = frmOpenData.FeatClsCollection;
            if (featClsCol.Count == 1)
            {
                sourceFeatureclass = featClsCol[0] as IFeatureClass;
            }
            else
            {
                MessageBoxEx.Show("请加载数据源", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            sourceFeatureclassPath = frmOpenData.PathName + @"\" + sourceFeatureclass.AliasName;
            if (string.IsNullOrEmpty(sourceFeatureclassPath))
            {
                return;
            }
            IDataset pDataset = sourceFeatureclass as IDataset;
            textBoxXLayerPath.Text = sourceFeatureclassPath;

            wizardPage1.NextButtonEnabled = eWizardButtonState.True;
            sourceFields = ClsUpdateCommon.GetAttribute(sourceFeatureclass);
        }
        /// <summary>
        ///  浏览待匹配图层
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonXBrowserTarlayer_Click(object sender, EventArgs e)
        {
            string targetFeatureclassPath = null;

            ZJGISOpenData.Forms.FrmOpenData frmOpenData = new ZJGISOpenData.Forms.FrmOpenData();

            if (frmOpenData.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            //sourceFeatureclass = ClsUpdateCommon.OpenSourceLayer(out sourceFeatureclassPath);
            Collection<object> featClsCol = new Collection<object>();
            featClsCol = frmOpenData.FeatClsCollection;
            if (featClsCol.Count == 1)
            {
                targetFeatureClass = featClsCol[0] as IFeatureClass;
            }
            else
            {
                MessageBoxEx.Show("请加载数据源", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            targetFeatureClassName = targetFeatureClass.AliasName;
            targetFeatureclassPath = frmOpenData.PathName + @"\" + targetFeatureClass.AliasName;
            if (string.IsNullOrEmpty(targetFeatureclassPath))
            {
                return;
            }
            IDataset pDataset = targetFeatureClass as IDataset;
            textBoxXTargetLayer.Text = targetFeatureclassPath;


            //20170515注释掉
            //targeFeatureWorkspace = ClsDBInfo.SdeWorkspace as IFeatureWorkspace;
            FileGDBWorkspaceFactoryClass fac = new FileGDBWorkspaceFactoryClass();
            targeFeatureWorkspace = (IFeatureWorkspace)fac.OpenFromFile(frmOpenData.PathName, 0);
            //targetFeatureClass = targeFeatureWorkspace.OpenFeatureClass(targetFeatureClassName);
            //targetFields = ClsUpdateCommon.GetAttribute(targetFeatureClass);

            wizardPage1.NextButtonEnabled = eWizardButtonState.True;
            targetFields = ClsUpdateCommon.GetAttribute(targetFeatureClass);
        }
        //TODO :打开实体表dbf
        /// <summary>
        /// 浏览匹配结果表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonXBrowseTable_Click(object sender, EventArgs e)
        {
            string tempResultTablePath = null;
            //relationTable = ClsUpdateCommon.OpenRelateTable(out tempResultTablePath);
            ZJGISOpenData.Forms.FrmOpenData frmOpenData = new ZJGISOpenData.Forms.FrmOpenData();
            frmOpenData.IsShowTable = true;
            if (frmOpenData.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            //sourceFeatureclass = ClsUpdateCommon.OpenSourceLayer(out sourceFeatureclassPath);
            Collection<object> tableCol = new Collection<object>();

            tableCol = frmOpenData.TableCollection;
            IDataset dataset = null;
            //if (tableCol.Count == 1)
            if (tableCol.Count > 1)
            {
                dataset = tableCol[0] as IDataset;
                relationTable = tableCol[0] as ITable;
            }
            else
            {
                MessageBoxEx.Show("请加载数据源", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (dataset == null)
            {
                MessageBoxEx.Show("请加载匹配结果表！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            tempResultTablePath = frmOpenData.PathName + @"\" + dataset.Name;
            if (string.IsNullOrEmpty(tempResultTablePath))
            {
                return;
            }
            textBoxXTablePath.Text = tempResultTablePath;

            //获取目标图层 20171002注释掉
            //targetFeatureClassName = dataset.Name.Substring(0, dataset.Name.LastIndexOf('_'));
            //wizardPage2.NextButtonEnabled = eWizardButtonState.True;

            ////20170515注释掉
            ////targeFeatureWorkspace = ClsDBInfo.SdeWorkspace as IFeatureWorkspace;
            //FileGDBWorkspaceFactoryClass fac = new FileGDBWorkspaceFactoryClass();
            //targeFeatureWorkspace = (IFeatureWorkspace)fac.OpenFromFile(frmOpenData.PathName, 0);

            //targetFeatureClass = targeFeatureWorkspace.OpenFeatureClass(targetFeatureClassName);
            //targetFields = ClsUpdateCommon.GetAttribute(targetFeatureClass);
        }


        /// <summary>
        /// 匹配赋值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wizardUpdate_FinishButtonClick(object sender, CancelEventArgs e)
        {
            this.progressBarXUpdate.Visible = true;
            this.labelX3.Visible = true;
            //if (ClsUpdateCommon.UpdateData(sourceFeatureclass, stargetFeatureClass, relationTable, attributeRelation, progressBarXUpdate, targeFeatureWorkspace)
            //    == false)
            if (ClsUpdateCommon.UpdateData(sourceFeatureclass, targetFeatureClass, relationTable, progressBarXUpdate, targeFeatureWorkspace)
            == false)
            {
                MessageBoxEx.Show("数据匹配赋值失败！");
                return;
            }
            DialogResult dr = MessageBox.Show("编码赋值完成", "结果", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                //点确定的代码
                this.Close();
            }
            else
            {//点取消的代码  
                this.Close();
            }

        }

        private void wizardUpdate_CancelButtonClick(object sender, CancelEventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wizardPage1_CancelButtonClick(object sender, CancelEventArgs e)
        {
            this.Close();
        }


        #region 以下所有功能没有用到
        ///*一下所有功能没有用到*/

        ///// <summary>
        ///// 下一步1 填充dataGridViewXRela
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void wizardPage2_NextButtonClick(object sender, CancelEventArgs e)
        //{
        //    //dataGridViewXRela.Rows.Clear();
        //    //((DataGridViewComboBoxExColumn)dataGridViewXRela.Columns[2]).DataSource = targetFields;

        //    //foreach (string item in sourceFields)
        //    //{
        //    //    DataGridViewRow addedRow = dataGridViewXRela.Rows[dataGridViewXRela.Rows.Add()];
        //    //    addedRow.Cells[0].Value = "N";
        //    //    addedRow.Cells[1].Value = item;
        //    //}
        //}

        ///// <summary>
        ///// 下一步2 
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void wizardPage3_NextButtonClick(object sender, CancelEventArgs e)
        //{
        //    //attributeRelation = new Dictionary<string, string>();
        //    //foreach (DataGridViewRow item in dataGridViewXRela.Rows)
        //    //{
        //    //    //此行没有被选中
        //    //    if (item.Cells[0].Value == null)
        //    //    {
        //    //        break;
        //    //    }
        //    //    //此行被选中
        //    //    if (item.Cells[0].Value.ToString() == "Y")
        //    //    {
        //    //        if (item.Cells[2].Value == null)
        //    //        {
        //    //            MessageBoxEx.Show("请输入对应的目标字段", ",提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //    //            e.Cancel = true;
        //    //            return;
        //    //        }
        //    //        else
        //    //        {
        //    //            if (attributeRelation.ContainsValue(item.Cells[2].Value.ToString()))
        //    //            {
        //    //                MessageBoxEx.Show("不能用多个字段匹配同一字段，请检查后执行", ",提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //    //                e.Cancel = true;
        //    //                return;
        //    //            }
        //    //            attributeRelation.Add(item.Cells[1].Value.ToString(), item.Cells[2].Value.ToString());
        //    //        }
        //    //    }
        //    //}
        //}
        ///// <summary>
        ///// 下一步3 匹配
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void wizardPage4_NextButtonClick(object sender, CancelEventArgs e)
        //{
        //    //if (ClsUpdateCommon.UpdateData(sourceFeatureclass, stargetFeatureClass, relationTable, attributeRelation, progressBarXUpdate, targeFeatureWorkspace)
        //    //    == false)
        //    //{
        //    //    MessageBoxEx.Show("数据更新失败！");
        //    //    return;
        //    //}
        //}
        ///// <summary>
        ///// 下一步4 关闭
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void wizardPage5_FinishButtonClick(object sender, CancelEventArgs e)
        //{
        //    this.Close();
        //}
        #endregion

    }
}
