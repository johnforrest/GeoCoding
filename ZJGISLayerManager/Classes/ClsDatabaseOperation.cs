//<CSCC>
//-------------------------------------------------------------------------------------
//    部件名：ClsDatabaseOperation
//    工程名：LayerManager
//    版权: CopyRight (c) 2010
//    创建人：
//    描述：对数据库的操作
//    创建日期：
//    修改人：
//    修改说明：
//    修改日期：
//-------------------------------------------------------------------------------------
//</CSCC>


using System.Data.OracleClient;
using System.Windows.Forms;
using System.IO;
using System.Data;
using System;
using Microsoft.VisualBasic;
using ZJGISLayerManager;

namespace ZJGISLayerManager
{
    class ClsDatabaseOperation
    {
        /// <summary>
        /// 根据数据库连接、表名和查询表达式查找，返回DataTable
        /// </summary>
        /// <param name="vDataConn">数据库连接</param>
        /// <param name="vstrSql">查询表达式</param>
        /// <returns></returns>
        public DataTable GetDataTable(OracleConnection vDataConn, string vstrSql)
        {
            OracleCommand pDBCommand = null;
            OracleDataAdapter pDataAdapter = null;
            DataTable pDataTable = null;

            try
            {
                pDBCommand = new OracleCommand(vstrSql, vDataConn);
                pDataAdapter = new OracleDataAdapter(pDBCommand);
                pDataTable = new DataTable();
                pDataAdapter.Fill(pDataTable);
                return pDataTable;
            }
            catch (Exception ex)
            {
                //ModDeclare.g_ErrorHandler.DisplayInformation("ClsDatabaseOperation的GetDataTable()出错" + ex.Message, false,null,null);
                MessageBox.Show("ClsDatabaseOperation的GetDataTable()出错" + ex.Message, "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                return null;
            }
        }
        /// <summary>
        /// 更新数据库
        /// </summary>
        /// <param name="vConnection">数据库连接</param>
        /// <param name="vTableName">表名</param>
        /// <param name="vUpdateDataSource">用于更新的DataTable</param>
        public void ApplyOperation(OracleConnection vConnection, string vTableName, DataTable vUpdateDataSource)
        {
            string strSql = "Select * from " + vTableName;
            OracleCommand pDBCommand = null;
            OracleDataAdapter pDataAdapter = null;
            OracleCommandBuilder pCommandBuilder = null;
            DataTable pDataTable;

            try
            {
                pDBCommand = new OracleCommand(strSql, vConnection);
                pDataAdapter = new OracleDataAdapter(pDBCommand);
                pCommandBuilder = new OracleCommandBuilder(pDataAdapter);
                pDataAdapter.Update(vUpdateDataSource);
            }
            catch (Exception ex)
            {
                //ModDeclare.g_ErrorHandler.DisplayInformation("ClsDatabaseOperation中ApplyOperation出错:" + ex.Message, false,null,null);
                MessageBox.Show("ClsDatabaseOperation中ApplyOperation出错:" + ex.Message, "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            }
            finally
            {
                pDBCommand = null;
                pDataAdapter = null;
                pCommandBuilder = null;
                pDataTable = null;
            }
        }
        /// <summary>
        /// 初始化DataGridView的列
        /// </summary>
        /// <param name="vDataGridView"></param>
        /// <param name="vStrings"></param>
        /// <returns></returns>
        public bool InitialDataGridView(DataGridView vDataGridView, string[] vStrings)
        {
            int i;
            try
            {
                for (i = 0; i <= vStrings.Length-1; i++)
                {
                    vDataGridView.Columns[i].HeaderText = vStrings[i];
                }
                return true;
            }
            catch (Exception ex)
            {
                //ModDeclare.g_ErrorHandler.DisplayInformation("ClsDatabaseOperation的InitialDataGridView出错：" + ex.Message, false,null,null);
                MessageBox.Show("ClsDatabaseOperation中ApplyOperation出错:" + ex.Message, "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                return false;
            }
        }
        /// <summary>
        /// 获得数据库中Blob字段(默认Unicode)，转成strings传出
        /// </summary>
        /// <param name="vDataTable">数据表</param>
        /// <param name="vRowIndex">行索引</param>
        /// <param name="vField">字段名</param>
        /// <returns>字符串</returns>
        public string GetBlobFromDatabase(DataTable vDataTable, int vRowIndex, string vField)
        {
            DataRow pDataRow;
            Byte[] pBytes;

            try
            {
                pDataRow = vDataTable.Rows[vRowIndex];
                pBytes = (byte[])pDataRow[vField];
                return (string)ConvertBytesToString(pBytes);
            }
            catch (Exception ex)
            {
                //ModDeclare.g_ErrorHandler.DisplayInformation("ClsDatabaseOperation的GetBlobFromDatabase出错：" + ex.Message, false,null,null);
                MessageBox.Show("ClsDatabaseOperation的GetBlobFromDatabase出错：" + ex.Message, "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                return "";
            }
        }
        /// <summary>
        /// 将Stream转成Unicode字节数组
        /// </summary>
        /// <param name="vStream"></param>
        /// <returns></returns>
        public Byte[] ConvertFileStreamToBytes(Stream vStream)
        {
            BinaryReader pBinaryReader;
            Char[] pChars;

            try
            {
                pBinaryReader = new BinaryReader(vStream);
                pChars = pBinaryReader.ReadChars((int)vStream.Length);
                return System.Text.Encoding.Unicode.GetBytes(pChars);
            }
            catch (Exception ex)
            {
                //ModDeclare.g_ErrorHandler.DisplayInformation("ClsDatabaseOperation的ConvertFileStreamToBytes出错：" + ex.Message, false,null,null);
                MessageBox.Show("ClsDatabaseOperation的ConvertFileStreamToBytes出错：" + ex.Message, "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                return null;
            }
            finally
            {
                vStream.Close();
            }
        }
        /// <summary>
        /// 将Stream转为字节数组
        /// </summary>
        /// <param name="vStream"></param>
        /// <returns></returns>
        public Byte[] ConvertStreamToBytes(Stream vStream)
        {
            BinaryReader pBinaryReader;

            try
            {
                pBinaryReader = new BinaryReader(vStream);
                return pBinaryReader.ReadBytes((int)vStream.Length);
            }
            catch (Exception ex)
            {
                //ModDeclare.g_ErrorHandler.DisplayInformation("ClsDatabaseOperation的ConvertStreamToBytes出错：" + ex.Message, false,null,null);
                MessageBox.Show("ClsDatabaseOperation的ConvertStreamToBytes出错：" + ex.Message, "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                return null;
            }
            finally
            {
                vStream.Close();
            }
        }
        /// <summary>
        /// 将字符串转为Unicode字节数组
        /// </summary>
        /// <param name="vString"></param>
        /// <returns></returns>
        public Byte[] ConverStringToBytes(string vString)
        {
            try
            {
                return System.Text.Encoding.Unicode.GetBytes(vString);
            }
            catch (Exception ex)
            {
                //ModDeclare.g_ErrorHandler.DisplayInformation("ClsDatabaseOperation的ConvertFileStreamToBytes出错：" + ex.Message, false, null, null);
                MessageBox.Show("ClsDatabaseOperation的ConvertFileStreamToBytes出错：" + ex.Message, "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                return null;
            }
        }
        /// <summary>
        /// 将Unicode字节数组转为字符串
        /// </summary>
        /// <param name="vBytes"></param>
        /// <returns></returns>
        public string ConvertBytesToString(byte[] vBytes)
        {
            try
            {
                return System.Text.Encoding.Unicode.GetString(vBytes);
            }
            catch (Exception ex)
            {
                //ModDeclare.g_ErrorHandler.DisplayInformation("ClsDatabaseOperation的ConvertBytesToString出错：" + ex.Message, false,null,null);
                MessageBox.Show("ClsDatabaseOperation的ConvertBytesToString出错：" + ex.Message, "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                return "";
            }
        }
        /// <summary>
        /// 判断传入参数是否为null，是则返回“”，否则返回其值
        /// </summary>
        /// <param name="vObj"></param>
        /// <returns></returns>
        public string ObjectNullToString(object vObj)
        {
            try
            {
                if (vObj == System.DBNull.Value)
                {
                    return "";
                }
                else
                {
                    return (string)vObj;
                }
            }
            catch (Exception ex)
            {
                //ModDeclare.g_ErrorHandler.DisplayInformation("ClsDatabaseOperation的DbNullToString出错：" + ex.Message, false,null,null);
                MessageBox.Show("ClsDatabaseOperation的DbNullToString出错：" + ex.Message, "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                return "";
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="vObj"></param>
        /// <returns></returns>
        public Byte[] ObjectNullToBytes(object vObj)
        {
            Byte[] pBytes=null;

            try
            {
                if (vObj == System.DBNull.Value)
                {
                    pBytes = new Byte[1];
                    pBytes[0] = 0;
                }
                else
                {
                    pBytes = (Byte[])vObj;
                }
                return pBytes;
            }
            catch (Exception ex)
            {
                //ModDeclare.g_ErrorHandler.DisplayInformation("ClsDatabaseOperation的ObjectNullToBytes出错：" + ex.Message, false, null, null);
                MessageBox.Show("ClsDatabaseOperation的ObjectNullToBytes出错：" + ex.Message, "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                return null;
            }
        }
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="vDBConn"></param>
        /// <param name="vSql"></param>
        public void ExecuteSQL(OracleConnection vDBConn, string vSql)
        {
            OracleCommand pDBCommand = null;

            try
            {
                if (vDBConn.State == ConnectionState.Closed)
                {
                    vDBConn.Open();
                }
                pDBCommand = new OracleCommand(vSql, vDBConn);
                pDBCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //ModDeclare.g_ErrorHandler.DisplayInformation("ClsDatabaseOperation的ExecuteSQL出错：" + ex.Message, false,null,null);
                MessageBox.Show("ClsDatabaseOperation的ExecuteSQL出错：" + ex.Message, "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            }
            finally
            {
                pDBCommand = null;
            }
        }
        /// <summary>
        /// 搜索一列中的最大值
        /// </summary>
        /// <param name="vDBConn"></param>
        /// <param name="vstrTableName"></param>
        /// <param name="vField"></param>
        /// <returns></returns>
        public long SearchForMaximumValueOfColumn(OracleConnection vDBConn, string vstrTableName, string vField)
        {
            DataTable pDataTable;

            try
            {
                pDataTable = GetDataTable(vDBConn, "select max(" + vField + ") from" + vstrTableName);
                return Convert.ToInt64(pDataTable.Rows[0][0].ToString());
            }
            catch (Exception ex)
            {
                //ModDeclare.g_ErrorHandler.DisplayInformation("ClsDatabaseOperation的SearchForMaximumValueOfColumn出错：" + ex.Message, false, null, null);
                MessageBox.Show("ClsDatabaseOperation的SearchForMaximumValueOfColumn出错：" + ex.Message, "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                return 0;
            }
            finally
            {
                pDataTable = null;
            }
        }

    }
}
