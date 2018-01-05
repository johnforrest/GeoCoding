/*-------------------------------------------------------------------------
 * 版权所有：武汉大学
 * 创建人：johnforrest
 * 联系方式：571716604@qq.com
 * 创建时间： 1/5/2018 2:18:47 PM
 * 类名称：ClsITableDataTable
 * 本类主要用途描述：
 * 修改人：
 * 修改时间：
 * 修改备注：
 * @version 1.0
 *  -------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ESRI.ArcGIS.Geodatabase;
using System.Windows.Forms;

namespace ZJGISCommon.Classes
{
    public static class ClsITableDataTable
    {
        /// <summary>
        /// 将ITable转换为DataTable
        /// </summary>
        /// <param name="mTable"></param>
        /// <returns></returns>
        public static DataTable ToDataTable(ITable mTable)
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
        /// 将DataGridView 内容读进datatable
        /// </summary>
        /// <param name="dv"></param>
        /// <returns></returns>
        public static DataTable DataGridViewToDataTable(DataGridView dv)
        {
            DataTable dt = new DataTable();
            DataColumn dc;
            for (int i = 0; i < dv.Columns.Count; i++)
            {
                dc = new DataColumn();
                dc.ColumnName = dv.Columns[i].HeaderText.ToString();
                dt.Columns.Add(dc);
            }
            for (int j = 0; j < dv.Rows.Count - 1; j++)
            {
                DataRow dr = dt.NewRow();
                for (int x = 0; x < dv.Columns.Count; x++)
                {
                    dr[x] = dv.Rows[j].Cells[x].Value;
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

    }
}
