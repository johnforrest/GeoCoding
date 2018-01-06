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

        /// <summary>
        /// 将FeatClass属性表高效率转换成DataTable  
        ///gisrsman.cnblogs.com
        /// </summary>
        /// <param name="featCls">输入的要素类</param>
        /// <param name="pQueryFilter">查询器，无则为Null</param>
        /// <returns></returns>
        public static DataTable FeatClass2DataTable(IFeatureClass featCls, IQueryFilter pQueryFilter)
        {
            DataTable pAttDT = null;
            string pFieldName;
            string pFieldValue;
            DataRow pDataRow;

            if (featCls != null)
            {
                //根据IFeatureClass字段结构初始化一个表结构
                pAttDT = InitTableByFeaCls(featCls);

                ITable pFeatTable = featCls as ITable;
                int pFieldCout = pFeatTable.Fields.FieldCount;
                ICursor pCursor = pFeatTable.Search(pQueryFilter, false);
                IRow pRow = pCursor.NextRow();

                while (pRow != null)
                {
                    pDataRow = pAttDT.NewRow();
                    for (int j = 0; j < pFieldCout; j++)
                    {
                        pFieldValue = pRow.get_Value(j).ToString();
                        pFieldName = pFeatTable.Fields.get_Field(j).Name;
                        pDataRow[pFieldName] = pFieldValue;
                    }
                    pAttDT.Rows.Add(pDataRow);
                    pRow = pCursor.NextRow();
                }
            }
            return pAttDT;
        }

        private static DataTable InitTableByFeaCls(IFeatureClass featCls)
        {
            throw new NotImplementedException();
        }

        public static DataTable FeatureClassToDataTable(IFeatureClass pListFeaCls)
        {
            DataTable pDataTable = new DataTable();
            try
            {
                if (pListFeaCls != null)
                {
                    ITable pFeaTable = pListFeaCls as ITable;
                    ICursor pCursor = pFeaTable.Search(null, false);
                    IRow pRow = pCursor.NextRow();
                    while (pRow != null)
                    {
                        object[] ob = new object[pRow.Fields.FieldCount];
                        for (int i = 0; i < pRow.Fields.FieldCount; i++)
                        {
                            ob[i] = pRow.get_Value(i);
                        }
                        pDataTable.Rows.Add(ob);
                        pRow = pCursor.NextRow();
                    }
                }
                return pDataTable;
            }
            catch
            {
                return null;
            }
        }

    }
}
