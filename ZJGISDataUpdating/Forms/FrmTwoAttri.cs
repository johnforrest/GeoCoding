using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;

namespace ZJGISDataUpdating.Forms
{
    public partial class FrmTwoAttri : Form
    {
        public FrmTwoAttri()
        {
            InitializeComponent();
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView2.RowHeadersVisible = false;
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(SystemInformation.WorkingArea.Width - this.Width, SystemInformation.WorkingArea.Height - this.Height);
        }

        public void LoadData(IRow item1, IRow item2)
        {
            if (item1 != null)
            {
                this.dataGridView1.Columns.Clear();
                this.dataGridView1.Columns.Add("index", "字段");
                this.dataGridView1.Columns.Add("value", "值");
                this.dataGridView1.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                //this.dataChild.Columns[0].DefaultCellStyle.BackColor = Color.LightSteelBlue;

                this.dataGridView1.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                //this.dataChild.Columns[1].DefaultCellStyle.BackColor =Color.LightYellow ;
                this.dataGridView1.Rows.Clear();
                //this.dataChild.DefaultCellStyle.BackColor = Color.LightYellow;

                IFields fields = item1.Fields;
                for (int i = 0; i < fields.FieldCount; i++)
                {
                    IField field = fields.get_Field(i);
                    int index = this.dataGridView1.Rows.Add();
                    //this.dataChild.Rows[index].DefaultCellStyle.BackColor = Color.LightSteelBlue;
                    this.dataGridView1.Rows[index].Cells[0].Value = field.Name;
                    this.dataGridView1.Rows[index].Cells[1].Value = item1.get_Value(i);
                }
            }
            if (item2 != null)
            {
                this.dataGridView2.Columns.Clear();
                this.dataGridView2.Columns.Add("index", "字段");
                this.dataGridView2.Columns.Add("value", "值");
                this.dataGridView2.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                //this.dataChild.Columns[0].DefaultCellStyle.BackColor = Color.LightSteelBlue;

                this.dataGridView2.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                //this.dataChild.Columns[1].DefaultCellStyle.BackColor =Color.LightYellow ;
                this.dataGridView2.Rows.Clear();
                //this.dataChild.DefaultCellStyle.BackColor = Color.LightYellow;

                IFields fields2 = item2.Fields;
                for (int i = 0; i < fields2.FieldCount; i++)
                {
                    IField field = fields2.get_Field(i);
                    int index = this.dataGridView2.Rows.Add();
                    //this.dataChild.Rows[index].DefaultCellStyle.BackColor = Color.LightSteelBlue;
                    this.dataGridView2.Rows[index].Cells[0].Value = field.Name;
                    this.dataGridView2.Rows[index].Cells[1].Value = item2.get_Value(i);
                }
            }

        }

        public void LoadData(List<IRow> list, IRow item2)
        {
            if (list.Count > 0)
            {
                //创建列
                this.dataGridView1.Columns.Clear();
                this.dataGridView1.Columns.Add("index", "字段");
                this.dataGridView1.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;

                for (int i = 0; i < list.Count; i++)
                {
                    //this.dataGridView1.Columns.Add("value", "值" + i + 1);
                    this.dataGridView1.Columns.Add("value", list[i].get_Value(0).ToString());
                    //this.dataChild.Columns[0].DefaultCellStyle.BackColor = Color.LightSteelBlue;
                    this.dataGridView1.Columns[i + 1].SortMode = DataGridViewColumnSortMode.NotSortable;
                    //this.dataChild.Columns[1].DefaultCellStyle.BackColor =Color.LightYellow ;
                    //this.dataGridView1.Rows.Clear();
                    //this.dataChild.DefaultCellStyle.BackColor = Color.LightYellow;
                }
                //创建行
                IFields fields2 = list[0].Fields;
                for (int j = 0; j < fields2.FieldCount; j++)
                {
                    IField field = fields2.get_Field(j);
                    int index = this.dataGridView1.Rows.Add();
                    //this.dataChild.Rows[index].DefaultCellStyle.BackColor = Color.LightSteelBlue;
                    this.dataGridView1.Rows[index].Cells[0].Value = field.Name;
                    //this.dataGridView1.Rows[index].Cells[i + 1].Value = list[i].get_Value(j);
                }
                //填充行和列
                for (int i = 0; i < list.Count; i++)
                {
                    ////this.dataGridView1.Columns.Add("value", "值" + i + 1);
                    //this.dataGridView1.Columns.Add("value", list[i].get_Value(0).ToString());
                    ////this.dataChild.Columns[0].DefaultCellStyle.BackColor = Color.LightSteelBlue;
                    //this.dataGridView1.Columns[i + 1].SortMode = DataGridViewColumnSortMode.NotSortable;
                    ////this.dataChild.Columns[1].DefaultCellStyle.BackColor =Color.LightYellow ;
                    //this.dataGridView1.Rows.Clear();
                    //this.dataChild.DefaultCellStyle.BackColor = Color.LightYellow;

                    IFields fields = list[i].Fields;
                    for (int j = 0; j < fields.FieldCount; j++)
                    {
                        IField field = fields.get_Field(j);
                        //int index = this.dataGridView1.Rows.Add();
                        //this.dataChild.Rows[index].DefaultCellStyle.BackColor = Color.LightSteelBlue;
                        this.dataGridView1.Rows[j].Cells[0].Value = field.Name;
                        this.dataGridView1.Rows[j].Cells[i + 1].Value = list[i].get_Value(j);
                    }
                }
            }
            if (item2 != null)
            {
                this.dataGridView2.Columns.Clear();
                this.dataGridView2.Columns.Add("index", "字段");
                this.dataGridView2.Columns.Add("value", "值");
                this.dataGridView2.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                //this.dataChild.Columns[0].DefaultCellStyle.BackColor = Color.LightSteelBlue;

                this.dataGridView2.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                //this.dataChild.Columns[1].DefaultCellStyle.BackColor =Color.LightYellow ;
                this.dataGridView2.Rows.Clear();
                //this.dataChild.DefaultCellStyle.BackColor = Color.LightYellow;

                IFields fields2 = item2.Fields;
                for (int i = 0; i < fields2.FieldCount; i++)
                {
                    IField field = fields2.get_Field(i);
                    int index = this.dataGridView2.Rows.Add();
                    //this.dataChild.Rows[index].DefaultCellStyle.BackColor = Color.LightSteelBlue;
                    this.dataGridView2.Rows[index].Cells[0].Value = field.Name;
                    this.dataGridView2.Rows[index].Cells[1].Value = item2.get_Value(i);
                }
            }

        }

    }
}
