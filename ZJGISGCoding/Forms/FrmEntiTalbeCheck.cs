using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using ZJGISCommon;
using ZJGISOpenData.Forms;

namespace ZJGISGCoding.Forms
{
    public partial class FrmEntiTalbeCheck : DevComponents.DotNetBar.Office2007Form
    {
        ITable entiTable;

        public FrmEntiTalbeCheck()
        {
            InitializeComponent();
        }

        private void FrmEntiTalbeCheck_Load(object sender, EventArgs e)
        {
            this.labelX2.Visible = false;
            this.progressBarX1.Visible = false;
        }
        private void buttonXPath_Click(object sender, EventArgs e)
        {
            FrmOpenData frmOpenData = new FrmOpenData();
            frmOpenData.IsShowTable = true;
            if (frmOpenData.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }
            Collection<object> tableCol = new Collection<object>();
            tableCol = frmOpenData.TableCollection;
            IDataset dataset = null;
            //if (tableCol.Count == 1)
            if (tableCol.Count > 1)
            {
                dataset = tableCol[0] as IDataset;
                entiTable = tableCol[0] as ITable;
            }
            else
            {
                MessageBox.Show("请加载数据源", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (dataset == null)
            {
                MessageBox.Show("请加载匹配结果表！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string tempResultTablePath = frmOpenData.PathName + @"\" + dataset.Name;
            if (string.IsNullOrEmpty(tempResultTablePath))
            {
                return;
            }
            textBoxX1.Text = tempResultTablePath;

        }

        private void buttonXCheck_Click(object sender, EventArgs e)
        {
           

            //进度条
            ClsBarSync pBarX1 = new ClsBarSync(progressBarX1);
            pBarX1.SetStep(1);
            pBarX1.SetMax(entiTable.RowCount(null));


            bool hasENTIID = false;
            if (entiTable == null)
            {
                return;
            }

            for (int i = 0; i < entiTable.Fields.FieldCount; i++)
            {
                if (entiTable.Fields.get_Field(i).Name == "ENTIID")
                {
                    hasENTIID = true;
                }
            }
            if (!hasENTIID)
            {
                MessageBox.Show("不存在实体编码字段！");
            }

            DataTable dtentble = ToDataTable(entiTable);
            DataTable dtresult = new DataTable();
            dtresult = dtentble.Clone();
            //dt.PrimaryKey = new System.Data.DataColumn[] { dt.Columns["OBJECTID"] };
            //string entiid = null;
            //DataView dv = dt.DefaultView;
            //string entiid = "ENTIID";
            //dv.RowFilter = "select * "+"from " + entiTable + "group by " + entiid+ "WHERE " +"Count(" + entiid + ")>1" ;
            //int i = dv.Count;
            //string[] strarr = GetNamesFromDataTable(dt);
            var result = from t in dtentble.AsEnumerable()
                         group t by t.Field<string>("ENTIID") into g
                         where g.Count() > 1
                         select g;

            foreach (var item in result)
            {
                if (item == null)
                {
                    MessageBox.Show("实体表不存在重复的地理编码！");
                }
                else
                {
                    this.labelX2.Visible = true;
                    this.progressBarX1.Visible = true;

                    int test = entiTable.RowCount(null);

                    for (int i = 0; i < dtentble.Rows.Count; i++)
                    {
                        string TES = dtentble.Rows[i]["ENTIID"].ToString();
                        if (dtentble.Rows[i]["ENTIID"].ToString() == item.Key)
                        {
                            //DataRow rowtemp = dtentble.Rows[i];

                            dtresult.Rows.Add(dtentble.Rows[i].ItemArray);
                        }
                        pBarX1.PerformOneStep();
                    }

                }
            }

            if (dtresult.Rows.Count > 1)
            {
                FrmResultDGV frmResult = new ZJGISGCoding.Forms.FrmResultDGV();
                BindingSource bs = new BindingSource();
                bs.DataSource = dtresult;
                frmResult.dataGridView1.DataSource = bs;
                frmResult.ShowDialog();
            }
            else
            {
                MessageBox.Show("实体表不存在重复的地理编码！");
            }


        }

        /// <summary>
        /// 将ITable转换为DataTable
        /// </summary>
        /// <param name="mTable"></param>
        /// <returns></returns>
        public DataTable ToDataTable(ITable mTable)
        {
            try
            {
                DataTable pTable = new DataTable();
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
                        StrRow[i] = pRrow.get_Value(i).ToString();
                    }
                    pRow.ItemArray = StrRow;
                    pTable.Rows.Add(pRow);
                    pRrow = pCursor.NextRow();
                }
                return pTable;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 获取重复的行
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public string[] GetNamesFromDataTable(DataTable dataTable)
        {
            DataView dv = dataTable.DefaultView;
            dataTable = dv.ToTable(true, "ENTIID");
            string[] names = new string[dataTable.Rows.Count];
            for (int i = 0; i < names.Length; i++)
            {
                names[i] = dataTable.Rows[i][0].ToString();
            }
            return names;
        }

    }
}
