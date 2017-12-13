using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using DevComponents.DotNetBar;

namespace ZJGISEntiTable.Froms
{
    public partial class FrmItemAttr : Office2007Form
    {
        public FrmItemAttr()
        {
            InitializeComponent();
            this.dataChild.RowHeadersVisible = false;
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(SystemInformation.WorkingArea.Width - this.Width, SystemInformation.WorkingArea.Height - this.Height);
        }

        public void LoadData(IRow item)
        {
            this.dataChild.Columns.Clear();
            this.dataChild.Columns.Add("index", "字段");
            this.dataChild.Columns.Add("value", "值");
            this.dataChild.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            //this.dataChild.Columns[0].DefaultCellStyle.BackColor = Color.LightSteelBlue;

            this.dataChild.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            //this.dataChild.Columns[1].DefaultCellStyle.BackColor =Color.LightYellow ;
            this.dataChild.Rows.Clear();
            //this.dataChild.DefaultCellStyle.BackColor = Color.LightYellow;

            IFields fields = item.Fields;
            for (int i = 0; i < fields.FieldCount; i++)
            {
                IField field = fields.get_Field(i);
                int index = this.dataChild.Rows.Add();
                //this.dataChild.Rows[index].DefaultCellStyle.BackColor = Color.LightSteelBlue;
                this.dataChild.Rows[index].Cells[0].Value = field.Name;
                this.dataChild.Rows[index].Cells[1].Value = item.get_Value(i);
            }
        }
    }
}
