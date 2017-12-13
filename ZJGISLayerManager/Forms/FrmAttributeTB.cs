//<CSCC>
//-------------------------------------------------------------------------------------
//    部件名：FrmAttributeTable
//    工程名：LayerManager
//    版权: CopyRight (c) 2010
//    创建人：
//    描述：图层属性显示窗口
//    创建日期：
//    修改人：MXF
//    修改说明：覆盖一个已存在的文件时报错
//    修改日期：11-22-2008
//-------------------------------------------------------------------------------------
//</CSCC>


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using System.IO;
using Microsoft.VisualBasic;
using ZJGISLayerManager;
//using DevComponents.DotNetBar.Controls;

namespace ZJGISLayerManager
{
    public partial class FrmAttributeTB : DevComponents.DotNetBar.Office2007Form
    {
        private bool m_bStopLoad;           //加载属性数据时，是否终止该过程
        private ILayer m_pSelLayer;         //传递进来的图层
        private IMap m_pMap;                //

        public string finallyGcode = null;

        ITableSort pTs;//处理排序
        public DataTable dt2;
        int row_index = 0;
        int col_index = 0;



        public static Dictionary<int, string> pDic = new Dictionary<int, string> { };
        public static Dictionary<int, string> pDicSub = new Dictionary<int, string> { };
        bool up = true;
        //获取TOC中选定的图层
        public ILayer SelLayer
        {
            set
            {
                m_pSelLayer = value;
            }
        }

        public IMap SetMap
        {
            set
            {
                m_pMap = value;
            }
        }

        public FrmAttributeTB()
        {
            InitializeComponent();
        }

        public void AddDataToTable(ILayer pLayer)
        {
            //如果选择的是矢量图层
            if (pLayer is IFeatureLayer)
            {
                IFeatureLayer pFeatureLayer;
                IFeatureClass pFeatClass;
                pFeatureLayer = (IFeatureLayer)m_pSelLayer;
                pFeatClass = pFeatureLayer.FeatureClass;
                //向DataGridView里加载图层的属性表数据
                //AddFeatDataToTable(pFeatClass);
                AddFeatDataToTableByDatable(pFeatClass);
            }
            //如果选择的图层是RasterLayer
            else if (pLayer is IRasterLayer)
            {
                IRasterLayer pRasterLayer;
                IAttributeTable pAttributeTable;
                ITable pTable;

                pRasterLayer = (IRasterLayer)m_pSelLayer;
                pAttributeTable = (IAttributeTable)pRasterLayer;
                pTable = pAttributeTable.AttributeTable;

                //如果属性表为空 退出
                if (pTable == null)
                    return;
                //向DataGridView里加载图层的属性表数据
                AddRasterDataToTable(pTable);
            }


            if (this.dgvTable.RowCount > 0)
            {
                this.biExportExcel.Enabled = true;
            }
            else
            {
                this.biExportExcel.Enabled = false;
            }

            if (this.lstFields.Items.Count > 0)
            {
                lstFields.Enabled = true;
                biDisplaySel.Enabled = true;
                biDisplayAll.Enabled = true;
            }
            else
            {
                lstFields.Enabled = false;
                biDisplaySel.Enabled = false;
                biDisplayAll.Enabled = false;
            }
        }
        /// <summary>
        /// 添加要素类的所有属性数据到表格中
        /// </summary>
        /// <param name="pFeatClass">要素类</param>
        private void AddFeatDataToTableByDatable(IFeatureClass pFeatClass)
        {

            IFields pFields = pFeatClass.Fields;
            IField pField;
            int i;

            //添加字段名称到左边的字段列表以及右边的表格列中
            for (i = 0; i <= pFields.FieldCount - 1; i++)
            {
                pField = pFields.get_Field(i);
                lstFields.Items.Add(pField.AliasName);
                lstFields.Items[i].Checked = true;          //初始化所有字段选中

                //this.dgvTable.Columns.Add(pField.AliasName, pField.AliasName);
            }

            if (pFeatClass != null)
            {
                DataTable dt = ToDataTable(pFeatClass);
                BindingSource bs = new BindingSource();
                bs.DataSource = dt;
                this.dgvTable.DataSource = bs;
                this.dgvTable.Columns.RemoveAt(0);

            }

        }

        /// <summary>
        /// 将ITable转换为DataTable
        /// </summary>
        /// <param name="mTable"></param>
        /// <returns></returns>
        public DataTable ToDataTable(IFeatureClass pFeatClass)
        {
            IField pField;
            ITable mTable=(ITable)pFeatClass;
            //字段取值
            object objFiedlValue;
            //属性表中的记录总数
            int intRowCount;
            //记录表总数目
            int j = 0;

            //this.dgvTable.Rows.Clear();

            intRowCount = pFeatClass.FeatureCount(null);
            this.lblCount.Text = this.lblCount.Text + intRowCount;
            this.ProgressBar.Visible = true;
            this.ProgressBar.Value = 0;
            this.ProgressBar.Maximum = intRowCount;

            this.dgvTable.RowCount = intRowCount;
            try
            {
                DataTable pTable = new DataTable();
                pTable.Columns.Clear();
                pTable.Rows.Clear();
                for (int i = 0; i < mTable.Fields.FieldCount; i++)
                {
                    pTable.Columns.Add(mTable.Fields.get_Field(i).Name);
                }
                ICursor pCursor = mTable.Search(null, false);
                IRow pRrow = pCursor.NextRow();
                while (pRrow != null)
                {
                    DataRow pRow = pTable.NewRow();
                    string[] StrRow = new string[pRrow.Fields.FieldCount];

                    for (int i = 0; i < pRrow.Fields.FieldCount; i++)
                    {
                        pField = pRrow.Fields.get_Field(i);
                        if (pField.Type == esriFieldType.esriFieldTypeGeometry)
                        {
                            objFiedlValue = GeomTypeAsString(pFeatClass.ShapeType); //获取几何形状的类型
                            StrRow[i] = (string)objFiedlValue;
                        }
                        else if (pField.Type == esriFieldType.esriFieldTypeBlob)
                        {
                            objFiedlValue = "Blob";
                            StrRow[i] = (string)objFiedlValue;
                        }
                        else if (pField.Type == esriFieldType.esriFieldTypeRaster)
                        {
                            objFiedlValue = "Raster";
                            StrRow[i] = (string)objFiedlValue;
                        }
                        else
                        {
                            StrRow[i] = pRrow.get_Value(i).ToString();
                            //objFiedlValue = pRrow.get_Value(i);          //字段值
                        }

                        //dgvTable[nFldIndex, i].Value = objFiedlValue;               //按顺序添加到列表视图中

                        //StrRow[i] = pRrow.get_Value(i).ToString();

                    }
                    pRow.ItemArray = StrRow;
                    pTable.Rows.Add(pRow);
                    pRrow = pCursor.NextRow();
                    j++;
                    this.ProgressBar.Value = j;

                }

                this.biExportExcel.Enabled = true;
                this.biHide.Enabled = true;
                lstFields.Enabled = true;
                biDisplaySel.Enabled = true;
                biDisplayAll.Enabled = true;
                this.ProgressBar.Visible = false;
                m_bStopLoad = false;

                return pTable;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 添加要素类的所有属性数据到表格中
        /// </summary>
        /// <param name="pFeatClass">要素类</param>
        private void AddFeatDataToTable(IFeatureClass pFeatClass)
        {
            if (pFeatClass == null)
            {
                return;
            }
            //属性表中的记录总数
            int intRowCount;
            int i;
            int nFldIndex;
            IFeatureCursor pFeatCursor;
            IFeature pFeature;
            //字段取值
            object objFiedlValue;
            IFields pFields;
            IField pField;

            try
            {
                this.Cursor = Cursors.WaitCursor;
                pFields = pFeatClass.Fields;
                //添加字段名称到左边的字段列表以及右边的表格列中
                for (i = 0; i <= pFields.FieldCount - 1; i++)
                {
                    pField = pFields.get_Field(i);
                    lstFields.Items.Add(pField.AliasName);
                    //初始化所有字段选中
                    lstFields.Items[i].Checked = true;

                    //添加字段名称到右边的DataGridViews
                    this.dgvTable.Columns.Add(pField.AliasName, pField.AliasName);
                }

                this.lstFields.Columns[0].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                intRowCount = pFeatClass.FeatureCount(null);
                this.lblCount.Text = this.lblCount.Text + intRowCount;

                if (intRowCount < 1)
                {
                    this.Cursor = Cursors.Default;
                    return;
                }

                this.ProgressBar.Visible = true;
                this.ProgressBar.Value = 0;
                this.ProgressBar.Maximum = intRowCount;

                this.dgvTable.RowCount = intRowCount;

                pFeatCursor = pFeatClass.Search(null, false);
                pFeature = pFeatCursor.NextFeature();

                i = 0;
                //遍历每一个要素
                while (pFeature != null)
                {
                    if (m_bStopLoad == true)       //停止加载
                    {
                        break;
                    }
                    //加载当前记录的所有字段取值到表格指定行的各列中
                    for (nFldIndex = 0; nFldIndex <= pFields.FieldCount - 1; nFldIndex++)
                    {
                        pField = pFields.get_Field(nFldIndex);
                        if (pField.Type == esriFieldType.esriFieldTypeGeometry)
                        {
                            objFiedlValue = GeomTypeAsString(pFeatClass.ShapeType); //获取几何形状的类型
                        }
                        else if (pField.Type == esriFieldType.esriFieldTypeBlob)
                        {
                            objFiedlValue = "Blob";
                        }
                        else if (pField.Type == esriFieldType.esriFieldTypeRaster)
                        {
                            objFiedlValue = "Raster";
                        }
                        else
                        {
                            objFiedlValue = pFeature.get_Value(nFldIndex);          //字段值
                        }

                        dgvTable[nFldIndex, i].Value = objFiedlValue;               //按顺序添加到列表视图中
                    }
                    dgvTable.Rows[i].Tag = pFeature;

                    pFeature = pFeatCursor.NextFeature();
                    i += 1;

                    this.ProgressBar.Value = i;
                }

                this.biExportExcel.Enabled = true;
                this.biHide.Enabled = true;
                lstFields.Enabled = true;
                biDisplaySel.Enabled = true;
                biDisplayAll.Enabled = true;
                this.ProgressBar.Visible = false;
                m_bStopLoad = false;

                this.Cursor = Cursors.Default;

            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                //ModDeclare.g_ErrorHandler.HandleError(true,"AddFeatDataToTable" ,0,null,ex.StackTrace);
                MessageBox.Show("AddFeatDataToTable", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            }
        }

        //向DataGridView里加载栅格图层的属性表数据
        private void AddRasterDataToTable(ITable pTable)
        {
            if (pTable == null)
            {
                return;
            }

            int intRowCount;            //属性表中的记录总数
            int i, j;
            int nFldIndex;
            object objFieldValue;       //字段取值
            IFields pFields;
            IField pField;
            IRow pRow;

            try
            {
                this.Cursor = Cursors.WaitCursor;

                pFields = pTable.Fields;

                //添加字段名称到左边的字段列表以及右边的表格列中
                for (i = 0; i <= pFields.FieldCount - 1; i++)
                {
                    pField = pFields.get_Field(i);
                    lstFields.Items.Add(pField.AliasName);
                    lstFields.Items[i].Checked = true;          //初始化所有字段选中

                    this.dgvTable.Columns.Add(pField.AliasName, pField.AliasName);
                }
                this.lstFields.Columns[0].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);

                intRowCount = pTable.RowCount(null);
                this.lblCount.Text = this.lblCount.Text + intRowCount;
                if (intRowCount < 1)
                {
                    this.Cursor = Cursors.Default;
                    return;
                }
                this.ProgressBar.Visible = true;
                this.ProgressBar.Value = 0;
                this.ProgressBar.Maximum = intRowCount;

                this.dgvTable.RowCount = intRowCount;

                //逐行添加
                for (j = 0; j <= intRowCount - 1; j++)
                {
                    //停止加载
                    if (m_bStopLoad == true)
                    {
                        break;
                    }

                    //设置进度条的取值
                    this.ProgressBar.Value = j;
                    pRow = pTable.GetRow(j);

                    //加载当前记录的所有字段取值到表格指定行的各列中
                    for (nFldIndex = 0; nFldIndex <= pFields.FieldCount - 1; nFldIndex++)
                    {
                        pField = pFields.get_Field(nFldIndex);
                        if (pField.Type == esriFieldType.esriFieldTypeBlob)
                        {
                            objFieldValue = "Blob";
                        }
                        else if (pField.Type == esriFieldType.esriFieldTypeRaster)
                        {
                            objFieldValue = "Raster";
                        }
                        else
                        {
                            objFieldValue = pRow.get_Value(nFldIndex);      //字段值
                        }
                        dgvTable[nFldIndex, j].Value = objFieldValue;       //按顺序添加到列表视图中
                    }
                    this.ProgressBar.Value = j;
                }

                this.ProgressBar.Visible = false;
                m_bStopLoad = false;

                this.Cursor = Cursors.Default;

            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                //ModDeclare.g_ErrorHandler.HandleError(true, "AddFeatDataToTable", 0, null, ex.StackTrace);
                MessageBox.Show("AddFeatDataToTable", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            }

        }

        /// <summary>
        /// 获取几何对象的类型，返回中文类型字符串
        /// </summary>
        /// <param name="nGeomType"></param>
        /// <returns></returns>
        public string GeomTypeAsString(esriGeometryType nGeomType)
        {
            switch ((int)nGeomType)
            {
                case 0:
                    return "空";
                case 1:
                    return "点";
                case 2:
                    return "多点";
                case 3:
                    return "线";
                case 4:
                    return "面";
                case 5:
                    return "矩形";
                case 6:
                    return "路径";
                case 7:
                case 10:
                case 12:
                    return "任意";
                case 8:
                    return "未定义";
                case 9:
                    return "三维多片";
                case 11:
                    return "环";
                case 13:
                    return "简单线";
                case 14:
                    return "圆弧";
                case 15:
                    return "贝塞尔曲线";
                case 16:
                    return "椭圆弧线";
                case 17:
                    return "Bag";
                case 18:
                    return "三角条带";
                case 19:
                    return "三角扇";
                case 20:
                    return "射线";
                case 21:
                    return "球体";
                default:
                    return "未知";
            }
        }

        //按Esc键，停止加载
        private void FrmAttributeTB_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(27))
            {
                m_bStopLoad = true;
            }
        }

        private void FrmAttributeTB_Load(object sender, EventArgs e)
        {
            if (m_pSelLayer == null)
            {
                return;
            }

            //设置窗体名称
            this.Text = m_pSelLayer.Name + "要素属性表";

            //判断是否按了Esc键
            m_bStopLoad = false;

            this.ProgressBar.Visible = false;           //未加载数据之前，进度条隐藏

            //设置列表视图的属性
            dgvTable.AllowUserToResizeColumns = true;
            dgvTable.AllowUserToResizeRows = false;

            //清空列表视图
            this.dgvTable.Columns.Clear();
            this.dgvTable.Rows.Clear();
            this.lstFields.Items.Clear();
            this.lstFields.CheckBoxes = true;           //显示复选框
        }

        ////保存操作
        //private void biSave_Click(object sender, EventArgs e)
        //{
        //    MessageBox.Show("保存属性表成功！");
        //    this.barFields.Visible = !this.barFields.Visible;
        //    //if (this.barFields.Visible == true)
        //    //{
        //    //    this.biHide.Text = "隐藏列表框";
        //    //}
        //    //else
        //    //{
        //    //    this.biHide.Text = "显示列表框";
        //    //}
        //}

        #region  button click event
        /// <summary>
        /// 表格中显示所有字段的信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void biDisplayAll_Click(object sender, EventArgs e)
        {
            if (lstFields.Items.Count < 1)
            {
                return;
            }

            int i;
            //所有字段均选中
            for (i = 0; i <= lstFields.Items.Count - 1; i++)
            {
                lstFields.Items[i].Checked = true;
                dgvTable.Columns[i].Visible = true;
            }

            lstFields.Invalidate();
            dgvTable.Refresh();
        }
        /// <summary>
        /// 隐藏所有字段（不显示所有的字段）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void biUnDisplayAll_Click(object sender, EventArgs e)
        {
            if (this.lstFields.Items.Count < 1)
            {
                return;
            }
            int i;
            for (i = 0; i <= lstFields.Items.Count - 1; i++)
            {
                lstFields.Items[i].Checked = false;
                dgvTable.Columns[i].Visible = false;

            }

            lstFields.Invalidate();
            dgvTable.Refresh();
        }
        /// <summary>
        /// 右边列表视图中只显示用户选择的字段信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void biDisplaySel_Click(object sender, EventArgs e)
        {
            if (lstFields.Items.Count < 1)
            {
                return;
            }
            if (lstFields.CheckedItems.Count == 0)
            {
                //ModDeclare.g_ErrorHandler.DisplayInformation("请选择要显示的字段！", false,null,null);
                MessageBox.Show("请选择要显示的字段！", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                return;
            }

            int i;
            for (i = 0; i <= lstFields.Items.Count - 1; i++)
            {
                if (lstFields.Items[i].Checked == false)
                {
                    dgvTable.Columns[i].Visible = false;
                }
                else
                {
                    dgvTable.Columns[i].Visible = true;
                }
            }
            dgvTable.Refresh();
        }

        /// <summary>
        /// 显示或隐藏字段列表框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void biHide_Click(object sender, EventArgs e)
        {
            this.barFields.Visible = !this.barFields.Visible;
            if (this.barFields.Visible == true)
            {
                this.biHide.Text = "隐藏列表框";
            }
            else
            {
                this.biHide.Text = "显示列表框";
            }
        }

        /// <summary>
        /// 保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void biSave_Click(object sender, EventArgs e)
        {
            #region 方法一
            //////ILayer pLayer = (ILayer)m_mapControl.CustomProperty;
            //IFeatureLayer pFLayer = m_pSelLayer as IFeatureLayer;
            //IFeatureClass pFeatureClass = pFLayer.FeatureClass;
            //ITable pTable;
            ////pTable = pFeatureClass.CreateFeature().Table;
            //////很重要的一种获取shp表格的一种方式
            //pTable = pFLayer as ITable;

            //int t = dgvTable.CurrentRow.Index;
            //IRow pRow;
            //pRow = pTable.GetRow(dgvTable.CurrentRow.Index);


            //int test = pTable.Fields.FindField("GCode");
            //pRow.set_Value(pTable.Fields.FindField("GCode"), finallyGcode);
            //try
            //{
            //    pRow.Store();
            //}
            //catch (System.Exception ex)
            //{
            //    return;  
            //}

            #endregion


            #region 方法二

            bool bHasEdits = false;
            bool bSave = false;
            int pIndex = 0;


            IFeatureLayer pFeatureLayer = m_pSelLayer as IFeatureLayer;
            IDataset pDataset = pFeatureLayer.FeatureClass as IDataset;
            //if (pDataset == null) return;

            IWorkspaceEdit pWorkspaceEdit = null;
            if (pDataset != null)
            {
                pWorkspaceEdit = pDataset.Workspace as IWorkspaceEdit;

                IWorkspaceEdit test1 = pWorkspaceEdit;
                bool test2 = pWorkspaceEdit.IsBeingEdited();

                if (pWorkspaceEdit != null || pWorkspaceEdit.IsBeingEdited() == false)
                {
                    pWorkspaceEdit.StartEditing(true);
                    pWorkspaceEdit.StartEditOperation();
                }
                IFeatureCursor pFeatureCursor = pFeatureLayer.Search(null, false);
                int i = pFeatureLayer.FeatureClass.FeatureCount(null);
                IFeature pFeature = pFeatureCursor.NextFeature();
                while (pFeature != null)
                {


                    //test
                    int index = pFeature.Fields.FindField("GCode");
                    int t = dgvTable.CurrentRow.Index;
                    int test = pFeature.Table.Fields.FindField("GCode");


                    int test3 = (int)dgvTable.CurrentRow.Cells[0].Value;
                    //int test4=dgvTable


                    try
                    {
                        pIndex = Convert.ToInt32(dgvTable.CurrentRow.Cells[0].Value);
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show("值转换失败！");
                    }

                    //if (pFeature.OID == (dgvTable.CurrentRow.Index))
                    //if (pFeature.OID == (dgvTable.CurrentRow.Index + 1))
                    if (pFeature.OID == pIndex)
                    {
                        IRow pRow;
                        //pRow = pFeature.Table.GetRow(dgvTable.CurrentRow.Index + 1);
                        //pRow = pFeature.Table.GetRow(dgvTable.CurrentRow.Index);
                        pRow = pFeature.Table.GetRow(pIndex);
                        //int test = pFeature.Table.Fields.FindField("GCode");
                        pRow.set_Value(pFeature.Table.Fields.FindField("GCode"), finallyGcode);
                        pRow.Store();
                        pFeature.Store();

                        #region 注释掉
                        //if (pWorkspaceEdit.IsBeingEdited())
                        //{
                        //    pWorkspaceEdit.HasEdits(ref bHasEdits);
                        //    if (bHasEdits)
                        //    {
                        //        DialogResult result;
                        //        result = MessageBox.Show("是否保存已做的修改?", "提示", MessageBoxButtons.YesNo);
                        //        if (result == DialogResult.Yes)
                        //        {
                        //            bSave = true;
                        //        }
                        //    }
                        //}
                        #endregion

                        //pFeature.Delete();
                        break;
                    }


                    pFeature = pFeatureCursor.NextFeature();
                }

                pWorkspaceEdit.StopEditing(true);
                pWorkspaceEdit.StopEditOperation();
            }
            #endregion


            pDic.Clear();
            pDicSub.Clear();

            MessageBox.Show("保存成功！");

        }

        #endregion
        //右键快捷菜单
        private void lstFields_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.barSelectType.Popup(MousePosition);
            }
        }

        #region right click button
        /// <summary>
        /// 所有字段均选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void biSelectAll_Click(object sender, EventArgs e)
        {
            if (this.lstFields.Items.Count < 1)
            {
                return;
            }
            int i;
            for (i = 0; i <= lstFields.Items.Count - 1; i++)
            {
                lstFields.Items[i].Checked = true;
            }
        }

        /// <summary>
        /// 所有字段均取消选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void biUnSelectAll_Click(object sender, EventArgs e)
        {
            if (this.lstFields.Items.Count < 1)
            {
                return;
            }
            int i;
            for (i = 0; i <= lstFields.Items.Count - 1; i++)
            {
                lstFields.Items[i].Checked = false;
            }
        }

        #endregion
        /// <summary>
        /// 右键菜单实现地理编码功能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRightClickGeocoding_Click(object sender, EventArgs e)
        {


            #region  自己代码
            dgvTable.BeginEdit(true);

            int j = 0;
            string Num = string.Empty;

            if (dgvTable.CurrentRow == null)
            {
                return;
            }

            DataGridViewRow dgvRow = dgvTable.CurrentRow;
            string gridcodeValue = dgvRow.Cells["GridCode"].Value.ToString();//你自己要获取的数据
            string fcodeValue = dgvRow.Cells["Fcode"].Value.ToString();
            string gcodeValue = dgvRow.Cells["GCode"].Value.ToString();
            string Code = gridcodeValue + fcodeValue;


            if (gridcodeValue == null)
            {
                MessageBox.Show("GridCode字段为空！");
                return;
            }
            if (fcodeValue == null)
            {
                MessageBox.Show("FCODE字段为空！");
                return;
            }
            if (gcodeValue == null)
            {
                MessageBox.Show("不存在GCode字段！");
                return;
            }


            //把属性表里面的值都遍历到字典中
            for (j = 0; j < dgvTable.RowCount; j++)
            {
                //dgvTable.Rows[j + 1].Cells["Fcode"].ToString() //写法错误，少一个value
                string sfcode = dgvTable.Rows[j].Cells["GCode"].Value.ToString();

                //pDic存储fcode字段值                
                pDic.Add(j, sfcode);
                if (sfcode.Length < 3)
                {
                    continue;

                }
                else
                {
                    //pDicSub存储GridCode和Fcode的组合字段
                    pDicSub.Add(j, sfcode.Substring(0, sfcode.Length - 3));
                }
            }

            if (pDicSub.ContainsValue(Code) == true)  //pDicSub字典中出现GridCode和Fcode的组合字段
            {

                List<int> keyList = (from q in pDicSub
                                     where q.Value == Code
                                     select q.Key).ToList<int>(); //get all keys
                keyList.Sort();
                int t = keyList.Max();
                Num = string.Format("{0:000}", Convert.ToInt32(pDic[keyList.Max()].Substring(pDic[keyList.Max()].Length - 3)) + 1);
            }
            else  //pDicSub字典中没有出现GridCode和Fcode的组合字段
            {
                Num = "001";
            }

            gcodeValue = Code + Num;
            dgvTable.CurrentRow.Cells["GCode"].Value = gcodeValue;
            finallyGcode = gcodeValue;


            dgvTable.EndEdit();
            MessageBox.Show("编码成功！请保存");

            #endregion

            #region 方法二
            //IFeatureLayer pFeatureLayer = m_pSelLayer as IFeatureLayer;
            //IDataset pDataset = pFeatureLayer.FeatureClass as IDataset;
            //IWorkspaceEdit pWorkspaceEdit = null;
            //if (pDataset != null)
            //{
            //    pWorkspaceEdit = pDataset.Workspace as IWorkspaceEdit;
            //    if (pWorkspaceEdit != null || pWorkspaceEdit.IsBeingEdited() == false)
            //    {
            //        pWorkspaceEdit.StartEditing(true);
            //        pWorkspaceEdit.StartEditOperation();
            //    }
            //    IFeatureCursor pFeatureCursor = pFeatureLayer.Search(null, false);
            //    int i = pFeatureLayer.FeatureClass.FeatureCount(null);


            //    int j = 0;
            //    string Num = string.Empty;
            //    string gcodeValue = string.Empty;

            //    DataGridViewRow dgvRow = dgvTable.CurrentRow;
            //    string gridcodeValue = dgvRow.Cells["GridCode"].Value.ToString();//你自己要获取的数据
            //    string fcodeValue = dgvRow.Cells["Fcode"].Value.ToString();
            //    //string gcodeValue = dgvRow.Cells["GCode"].Value.ToString();
            //    string Code = gridcodeValue + fcodeValue;

            //    //把属性表里面的值都遍历到字典中
            //    for (j = 0; j < dgvTable.RowCount; j++)
            //    {
            //        //dgvTable.Rows[j + 1].Cells["Fcode"].ToString() //写法错误，少一个value
            //        string sfcode = dgvTable.Rows[j].Cells["GCode"].Value.ToString();

            //        //pDic存储fcode字段值                
            //        pDic.Add(j, sfcode);
            //        if (sfcode.Length < 3)
            //        {
            //            continue;

            //        }
            //        else
            //        {
            //            //pDicSub存储GridCode和Fcode的组合字段
            //            pDicSub.Add(j, sfcode.Substring(0, sfcode.Length - 3));
            //        }
            //    }

            //    IFeature pFeature = pFeatureCursor.NextFeature();



            //    //查找字典中是否有当前行的值
            //    while (pFeature != null)
            //    {

            //        int index = pFeature.Fields.FindField("FCode");

            //        //pDic.Add(j, sfcode);
            //        //pDicSub.Add(j, sfcode.Substring(0, sfcode.Length - 3));

            //        if (pDicSub.ContainsValue(Code) == true)  //pDicSub字典中出现GridCode和Fcode的组合字段
            //        {

            //            List<int> keyList = (from q in pDicSub
            //                                 where q.Value == Code
            //                                 select q.Key).ToList<int>(); //get all keys
            //            keyList.Sort();
            //            int t = keyList.Max();
            //            Num = string.Format("{0:000}", Convert.ToInt32(pDic[keyList.Max()].Substring(pDic[keyList.Max()].Length - 3)) + 1);
            //        }
            //        else  //pDicSub字典中没有出现GridCode和Fcode的组合字段
            //        {
            //            Num = "001";
            //        }

            //        gcodeValue = Code + Num;
            //        //dgvTable.CurrentRow.Cells["GCode"].Value = gcodeValue;

            //        //pFeature.set_Value(pFeature.Fields.FindField("GCode"), gcodeValue);
            //        pFeature.set_Value(dgvTable.CurrentRow.Index, gcodeValue);

            //        pFeature.Store();
            //        j++;
            //        pFeature = pFeatureCursor.NextFeature();



            //        pWorkspaceEdit.StopEditing(true);
            //        pWorkspaceEdit.StopEditOperation();

            //    }

            //}

            //MessageBox.Show("编码生成完成！");
            //pDic.Clear();
            //pDicSub.Clear();
            #endregion

        }

        /// <summary>
        /// 导出属性表到Excel文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void biExportExcel_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlgSave = new SaveFileDialog();
            String strFileName;
            String strConn;
            String strColumnName;
            String strField = "";
            String strField2 = "";
            String strValue = "";
            int i, j;
            OleDbConnection dbConn = new OleDbConnection();
            OleDbCommand dbCommand = new OleDbCommand();
            String strTableName;

            string ProviderName = "Microsoft.Jet.OLEDB.4.0;";
            string ExtendedString = "'Excel 8.0;";
            string Hdr = "Yes;";
            string IMEX = "0';";

            try
            {
                strTableName = this.Text;

                dlgSave.Title = "导出属性表到Excel文件";
                dlgSave.Filter = "Excel 文件(*.xls)|*.xls";
                dlgSave.FileName = strTableName;

                if (dlgSave.ShowDialog() == DialogResult.Cancel)
                {
                    return;
                }
                strFileName = dlgSave.FileName;
                if (strFileName == "")
                {
                    return;
                }
                if (File.Exists(strFileName))
                {
                    File.Delete(strFileName);
                }
                this.Cursor = Cursors.WaitCursor;

                strConn = "Data Source=" + @strFileName + ";" +
                        "Provider=" + ProviderName +
                        "Extended Properties=" + ExtendedString +
                        "HDR=" + Hdr +
                        "IMEX=" + IMEX;
                for (j = 0; j <= dgvTable.Columns.Count - 1; j++)
                {
                    if (dgvTable.Columns[j].Visible == false)
                    {
                        continue;
                    }
                    strColumnName = dgvTable.Columns[j].Name;

                    if (strColumnName.Contains("."))
                    {
                        //strColumnName = strColumnName.Substring(0, strColumnName.IndexOf(".")) + "_" + strColumnName.Substring(strColumnName.IndexOf(".") + 1);
                        strColumnName = strColumnName.Replace(".", "_");
                    }
                    if (strColumnName.Contains("-"))
                    {
                        strColumnName = strColumnName.Replace("-", "_");
                    }
                    strField = strField + strColumnName + " char(255),";
                    strField2 = strField2 + strColumnName + ",";
                }
                strField = Strings.Left(strField, strField.Length - 1);
                strField2 = Strings.Left(strField2, strField2.Length - 1);

                dbConn.ConnectionString = strConn;
                if (strTableName.Contains("("))
                {
                    strTableName = strTableName.Substring(0, strTableName.IndexOf("(")) + "_" + strTableName.Substring(strTableName.IndexOf("(") + 1);
                }
                if (strTableName.Contains(")"))
                {
                    strTableName = strTableName.Substring(0, strTableName.IndexOf(")")) + "_" + strTableName.Substring(strTableName.IndexOf(")") + 1);
                }

                dbConn.Open();
                dbCommand.Connection = dbConn;
                dbCommand.CommandText = "CREATE TABLE " + strTableName + "(" + strField + ")";
                dbCommand.ExecuteNonQuery();

                this.ProgressBar.Visible = true;
                this.barStatus.RecalcLayout();
                this.ProgressBar.Value = 0;
                this.ProgressBar.Maximum = this.dgvTable.RowCount;

                for (i = 0; i <= dgvTable.Rows.Count - 1; i++)
                {
                    for (j = 0; j <= dgvTable.Columns.Count - 1; j++)
                    {
                        if (dgvTable.Columns[j].Visible == false)
                        {
                            continue;
                        }
                        strValue = strValue + "'" + dgvTable[j, i].Value.ToString() + "'" + ",";
                    }
                    strValue = Strings.Left(strValue, strValue.Length - 1);

                    dbCommand.CommandText = "INSERT INTO " + strTableName + "(" + strField2 + ")" + "values (" + strValue + ")";
                    dbCommand.ExecuteNonQuery();

                    strValue = "";
                    this.ProgressBar.Value = i;
                }

                this.ProgressBar.Visible = false;
                dbConn.Close();
                this.Cursor = Cursors.Default;
                //ModDeclare.g_ErrorHandler.DisplayInformation("", false,null,null);
                MessageBox.Show("数据导出完毕！", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                //ModDeclare.g_ErrorHandler.DisplayInformation("导出数据到Excel文件出错，原因：" + ex.Message, false,null,null);
                MessageBox.Show("导出数据到Excel文件出错，原因：" + ex.Message, "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            }
            finally
            {
                ProgressBar.Visible = false;
                if (dbConn.State == ConnectionState.Open)
                {
                    dbConn.Close();
                }
                dbConn = null;
                dbCommand = null;
                dlgSave = null;
            }
        }

        /// <summary>
        /// 选中整行时，三维场景中选中对应的要素
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvTable_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if ((m_pMap == null) || (m_pSelLayer == null))
            {
                return;
            }
            IFeature pFeature;
            IActiveView pActView;
            pFeature = (IFeature)dgvTable.Rows[e.RowIndex].Tag;

            if (pFeature == null)
            {
                return;
            }
            m_pMap.ClearSelection();
            m_pMap.SelectFeature(m_pSelLayer, pFeature);
            pActView = (IActiveView)m_pMap;
            pActView.Refresh();

            this.labelXSelectItems.Text = "选中记录数：" + dgvTable.SelectedRows.Count;
            this.labelXSelectItems.Visible = true;
        }

        private void dgvTable_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //MessageBox.Show("保存成功！");

        }



        private void dgvTable_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {


        }

        /// <summary>
        /// 属性表的排序、当用户双击列标题时触发的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvTable_ColumnHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }


        private void dgvTable_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.CellValue1.ToString().Length == 0 && e.CellValue2.ToString().Length == 0)
            {
                e.SortResult = 0;
                // e.Handled = true;就是把默认的给屏蔽掉了
                e.Handled = true;
            }
            else if (e.CellValue1.ToString().Length == 0)
            {
                e.SortResult = -1;
                e.Handled = true;

            }
            else if (e.CellValue2.ToString().Length == 0)
            {
                e.SortResult = 1;
                e.Handled = true;

            }
            else
            {
                //e.Handled = false;则采用datagridview默认的排序方式下面的这行代码和e.Handled=false等效。
                //e.SortResult = string.Compare(e.CellValue1.ToString(), e.CellValue2.ToString());
                e.Handled = false;
            }
        }

        private void dgvTable_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //IFeatureLayer pFeaturelayer = m_pSelLayer as IFeatureLayer;
            //IFeatureClass pFeatureClass = pFeaturelayer.FeatureClass;

            //ITableSort pTableSort = new TableSortClass();
            //IFields pFields = pFeatureClass.Fields;
            //IField pField = pFields.get_Field(col_index);

            //pTableSort.Fields = pField.Name;

            //if (up)
            //{
            //    pTableSort.set_Ascending(pField.Name, true);
            //}
            //else
            //{
            //    pTableSort.set_Ascending(pField.Name, false);
            //}

            //pTableSort.set_CaseSensitive(pField.Name, true);
            //pTableSort.Table = pFeatureClass as ITable;
            //pTableSort.Sort(null);
            //ICursor pCursor = pTableSort.Rows;

            //pTs = pTableSort;


            //if (pCursor == null)
            //{
            //    MessageBox.Show("未排序！");
            //}
            //else
            //{
            //    MessageBox.Show("排序完成！");
            //}
            //up = false;

            //RefreshTable();
            ////dgvTable.Refresh();
        }

        /// <summary>
        /// 刷新属性表
        /// </summary>
        private void RefreshTable()
        {
            IFeatureLayer pFeaturelayer = m_pSelLayer as IFeatureLayer;
            IFeatureClass pFeatureClass = pFeaturelayer.FeatureClass;

            if (pFeatureClass == null) return;

            //新建一个datatable
            DataTable dataTable = new DataTable();
            //填充datacolumn
            DataColumn dataColumn = null;
            for (int i = 0; i < pFeatureClass.Fields.FieldCount; i++)
            {
                dataColumn = new DataColumn(pFeatureClass.Fields.get_Field(i).Name);
                dataTable.Columns.Add(dataColumn);
            }
            //记录原来的数据有多少行
            ICursor pCusor = pTs.Rows;
            IRow pRow = pCusor.NextRow();
            //填充dataRow
            DataRow dataRow = null;
            while (pRow != null)
            {
                dataRow = dataTable.NewRow();
                for (int j = 0; j < pFeatureClass.Fields.FieldCount; j++)
                {
                    if (j == pFeatureClass.FindField(pFeatureClass.ShapeFieldName))
                    {

                        dataRow[j] = pFeatureClass.ShapeType.ToString();
                    }
                    else
                    {
                        dataRow[j] = pRow.get_Value(j).ToString();
                    }
                }
                dataTable.Rows.Add(dataRow);
                pRow = pCusor.NextRow();
            }
            dgvTable.Refresh();
            //dgvTable.DataSource = dataTable;
            //dataGridView1.DataSource = dt;
            //dt2 = dt;
        }

        //private void dgvTable_MouseClick(object sender, MouseEventArgs e)
        //{
        //    if (e.Button == MouseButtons.Left)
        //    {
        //        DataGridView.HitTestInfo hit = this.dgvTable.HitTest(e.X, e.Y);
        //        if (hit.Type == DataGridViewHitTestType.ColumnHeader)
        //        {
        //            this.dgvTable.Columns[hit.ColumnIndex].SortMode = DataGridViewColumnSortMode.NotSortable;
        //            //this.dataGridView1.Columns[hit.ColumnIndex].Selected = true;
        //            col_index = hit.ColumnIndex;
        //        }

        //    }

        //    if (e.Button == MouseButtons.Left)
        //    {
        //        DataGridView.HitTestInfo hit = this.dgvTable.HitTest(e.X, e.Y);
        //        if (hit.Type == DataGridViewHitTestType.RowHeader)
        //        {
        //            row_index = hit.RowIndex;
        //        }

        //    }
        //}





    }
}
