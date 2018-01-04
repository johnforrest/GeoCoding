using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace ZJGISGCoding.Forms
{
    public partial class FrmResultDGV : DevComponents.DotNetBar.Office2007Form
    {
        public FrmResultDGV()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="pRow">一条数据IFeature或者IRow</param>
        public void LoadData(IRow pRow)
        {
            this.dataChild.Columns.Clear();
            for (int i = 0; i < pRow.Fields.FieldCount; i++)
            {
                this.dataChild.Columns.Add(pRow.Fields.get_Field(i).Name, pRow.Fields.get_Field(i).AliasName);
                this.dataChild.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            this.dataChild.Rows.Clear();


            //IFields fields = pRow.Fields;
            for (int i = 0; i < pRow.Fields.FieldCount; i++)
            {
                //IField field = pRow.Fields.get_Field(i);
                int index = this.dataChild.Rows.Add();
                //this.dataChild.Rows[index].DefaultCellStyle.BackColor = Color.LightSteelBlue;
                this.dataChild.Rows[index].Cells[i].Value = pRow.Fields.get_Field(i).DefaultValue;
                //this.dataChild.Rows[index].Cells[1].Value = pRow.get_Value(i);
            }
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="pRow">一条数据IFeature或者IRow</param>
        public void LoadData(List<IRow> list)
        {
            if (list.Count > 0)
            {
                this.dataChild.Columns.Clear();
                for (int i = 0; i < list[0].Fields.FieldCount; i++)
                {
                    this.dataChild.Columns.Add(list[0].Fields.get_Field(i).Name, list[0].Fields.get_Field(i).AliasName);
                    this.dataChild.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                this.dataChild.Rows.Clear();

                for (int j = 0; j < list.Count; j++)
                {
                    for (int i = 0; i < list[j].Fields.FieldCount; i++)
                    {
                        this.dataChild.Rows.Add();
                        if (i != 1)
                        {
                            //int index = this.dataChild.Rows.Add();
                            //this.dataChild.Rows[index].DefaultCellStyle.BackColor = Color.LightSteelBlue;
                            this.dataChild.Rows[j].Cells[i].Value = list[j].get_Value(i);
                            //this.dataChild.Rows[index].Cells[1].Value = pRow.get_Value(i);
                        }
                        else
                        {
                            //list[j].Table as IDataset
                            if ((list[j].Table as IFeatureClass).ShapeType == esriGeometryType.esriGeometryPolygon)
                            {
                                this.dataChild.Rows[j].Cells[i].Value = "面";
                            }
                            else if ((list[j].Table as IFeatureClass).ShapeType == esriGeometryType.esriGeometryPolyline
                                || (list[j].Table as IFeatureClass).ShapeType == esriGeometryType.esriGeometryLine)
                            {
                                this.dataChild.Rows[j].Cells[i].Value =  "线";
                            }
                            else if ((list[j].Table as IFeatureClass).ShapeType == esriGeometryType.esriGeometryMultipoint
                                || (list[j].Table as IFeatureClass).ShapeType == esriGeometryType.esriGeometryPoint)
                            {
                                this.dataChild.Rows[j].Cells[i].Value ="点";
                            }
                        }
                    }
                }
            }
        }


    }
}
