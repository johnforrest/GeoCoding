using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ZJGISEntityCode.Classes;
using ZJGISCommon.Classes;

namespace ZJGISGCoding.Forms
{
    public partial class FrmCheckNull : DevComponents.DotNetBar.Office2007Form
    {
        public FrmCheckNull()
        {
            InitializeComponent();
        }

        private IMap pMapControl;
        public FrmCheckNull(IMap pMapControl)
        {
            InitializeComponent();
            this.pMapControl = pMapControl;
        }

        private void frm_CheckNull_Load(object sender, EventArgs e)
        {

            if (pMapControl.LayerCount > 0)
            {
                for (int i = 0; i < pMapControl.LayerCount; i++)
                {
                    ILayer pLayer = pMapControl.get_Layer(i);
                    if (pLayer is IGroupLayer)
                    {
                        ICompositeLayer pCompositeLayer = pLayer as ICompositeLayer;
                        for (int j = 0; j < pCompositeLayer.Count; j++)
                        {
                            if (pCompositeLayer.get_Layer(j) is IFeatureLayer)
                            {
                                cbxLayerName.Items.Add(pCompositeLayer.get_Layer(j).Name);
                            }
                        }
                    }
                    else
                    {
                        if (pLayer is IFeatureLayer)
                        {
                            cbxLayerName.Items.Add(pMapControl.get_Layer(i).Name);
                        }
                    }
                }
                cbxLayerName.SelectedIndex = 0;
            }
            else
            {
                return;
            }
        }
        /// <summary>
        /// 编码空值检查
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Hide();
            IFeatureLayer pFeatureLayer = ClsDeclare.GetLayerByName(pMapControl, cbxLayerName.Text) as IFeatureLayer;

            CheckLayers(pFeatureLayer);


            this.Dispose();
            this.Close();
        }

        /// <summary>
        /// 检查图层编码
        /// </summary>
        /// <param name="pFeatureLayer">待检查的图层</param>
        private void CheckLayers(IFeatureLayer pFeatureLayer)
        {
            string strField = "ENTIID";
            string strFCODE = "FCODE";
            //匹配图层名称
            string strName = ClsConfig.LayerConfigs[pFeatureLayer.Name].NameField.ToString();

            if (pFeatureLayer == null)
            {
                this.Show();
                return;
            }
            IFeatureClass pFeatureClass = pFeatureLayer.FeatureClass;
            if (pFeatureClass.Fields.FindField(strField) == -1)
            {
                MessageBox.Show("图层不包含地理实体编码字段!");
                this.Show();
                return;
            }
            if (pFeatureClass.Fields.FindField(strFCODE) == -1)
            {
                MessageBox.Show("图层不包含分类码字段!");
                this.Show();
                return;
            }
            bool test = string.IsNullOrEmpty(strName);
            bool test1 = strName=="null";
            int test2 = String.Compare(strName, "null");
            bool test3 = strName.Equals("null");
            //图层包含名称字段
            if (!strName.Equals("null") && strName.Trim().Length > 0)
            {
                IQueryFilter pQueryFilter = new QueryFilter();
                //pQueryFilter.WhereClause = "\"" + strField + "\"" + " = ''";
                pQueryFilter.WhereClause = "\"" + strName + "\" is not NULL";
                IFeatureCursor pFeatureCursor = pFeatureLayer.Search(pQueryFilter, false);
                IFeature pFeature = pFeatureCursor.NextFeature();
                //if (pFeature == null)
                //{
                //    MessageBox.Show("编码成功，无空值！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    return;
                //}
                int j = 0;
                FrmDataCheck frm = new FrmDataCheck();

                DataGridView dataView = frm.GetDataGridView;
                dataView.Rows.Clear();
                dataView.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataView.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.White;
                dataView.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font(dataView.Font, System.Drawing.FontStyle.Bold);
                dataView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
                dataView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Sunken;
                dataView.GridColor = System.Drawing.Color.Black;
                dataView.RowHeadersVisible = false;
                dataView.ColumnCount = 2;
                dataView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
                dataView.MultiSelect = false;
                dataView.AllowUserToAddRows = false;
                dataView.ReadOnly = true;
                dataView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                dataView.Columns[0].Name = "OID";
                dataView.Columns[0].HeaderText = "OID";
                dataView.Columns[0].Width = dataView.Width / 3;
                dataView.Columns[1].Name = "Name";
                dataView.Columns[1].HeaderText = "名字";
                dataView.Columns[1].AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;

                //string strName = pFeature.get_Value(indexofName).ToString();

                while (pFeature != null)
                {
                    string pEntiID = pFeature.get_Value(pFeatureClass.FindField(strField)).ToString();
                    //IQueryFilter pQueryFilter1 = new QueryFilter();
                    ////pQueryFilter.WhereClause = "\"" + strField + "\"" + " = ''";
                    //pQueryFilter1.WhereClause = "\"" + strField + "\" is NULL";
                    //IFeatureCursor pFeatureCursor1 = pFeatureLayer.Search(pQueryFilter1, false);
                    //IFeature pFeature1 = pFeatureCursor1.NextFeature();
                    ////if (pTable.Fields.FindField(strField) == -1)
                    if (pEntiID.Trim().Length == 0)
                    {
                        j = dataView.Rows.Add();
                        dataView["FID", j].Value = pFeature.OID.ToString();
                        //dataView["Name", j].Value = cbxLayerName.Text;
                        //string strName = "Name";
                        dataView["NAME", j].Value = pFeature.get_Value(pFeatureClass.FindField(strName)).ToString();
                        //pFeature = pFeatureCursor.NextFeature();
                    }
                    pFeature = pFeatureCursor.NextFeature();
                    //else
                    //{
                    // MessageBox.Show("编码成功，无空值！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // return;
                    //}
                }
                if (dataView.RowCount == 0)
                {
                    MessageBox.Show("编码检查完成，无空值！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                frm.Show();
            }
            else
            {
                string[] strarr = { "3103011500", "3103012500", "3103013500", "3103020500", "3106000500", "3107000500", "3108000500" };
                IQueryFilter pQueryFilter = new QueryFilter();
                //pQueryFilter.WhereClause = "\"" + strField + "\"" + " = ''";
                //pQueryFilter.WhereClause = "\"" + strFCODE + "\" is not NULL";
                //pQueryFilter.WhereClause = strarr.Contains("\"" + strFCODE + "\"");
                //IFeatureCursor pFeatureCursor = pFeatureLayer.Search(pQueryFilter, false);
                IFeatureCursor pFeatureCursor = pFeatureLayer.Search(null, false);
                IFeature pFeature = pFeatureCursor.NextFeature();
         
                int j = 0;
                FrmDataCheck frm = new FrmDataCheck();

                DataGridView dataView = frm.GetDataGridView;
                dataView.Rows.Clear();
                dataView.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dataView.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.White;
                dataView.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font(dataView.Font, System.Drawing.FontStyle.Bold);
                dataView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
                dataView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Sunken;
                dataView.GridColor = System.Drawing.Color.Black;
                dataView.RowHeadersVisible = false;
                dataView.ColumnCount = 2;
                dataView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
                dataView.MultiSelect = false;
                dataView.AllowUserToAddRows = false;
                dataView.ReadOnly = true;
                dataView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                dataView.Columns[0].Name = "OID";
                dataView.Columns[0].HeaderText = "OID";
                dataView.Columns[0].Width = dataView.Width / 3;
                dataView.Columns[1].Name = "FCODE";
                dataView.Columns[1].HeaderText = "分类码";
                dataView.Columns[1].AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;

                //string strName = pFeature.get_Value(indexofName).ToString();

                while (pFeature != null)
                {
                    string pFCode = pFeature.get_Value(pFeatureClass.FindField(strFCODE)).ToString();
                    string pEntiID = pFeature.get_Value(pFeatureClass.FindField(strField)).ToString();

                    //IQueryFilter pQueryFilter1 = new QueryFilter();
                    ////pQueryFilter.WhereClause = "\"" + strField + "\"" + " = ''";
                    //pQueryFilter1.WhereClause = "\"" + strField + "\" is NULL";
                    //IFeatureCursor pFeatureCursor1 = pFeatureLayer.Search(pQueryFilter1, false);
                    //IFeature pFeature1 = pFeatureCursor1.NextFeature();
                    ////if (pTable.Fields.FindField(strField) == -1)
                    if (strarr.Contains(pFCode)&&pEntiID.Trim().Length == 0)
                    {
                        j = dataView.Rows.Add();
                        dataView["FID", j].Value = pFeature.OID.ToString();
                        //dataView["Name", j].Value = cbxLayerName.Text;
                        //string strName = "Name";
                        dataView["FCODE", j].Value = pFeature.get_Value(pFeatureClass.FindField(strFCODE)).ToString();
                        //pFeature = pFeatureCursor.NextFeature();
                    }
                    pFeature = pFeatureCursor.NextFeature();
                    //else
                    //{
                    // MessageBox.Show("编码成功，无空值！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // return;
                    //}
                }
                if (dataView.RowCount == 0)
                {
                    MessageBox.Show("编码检查完成，无空值！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                frm.Show();
            }

        }
    }
}
