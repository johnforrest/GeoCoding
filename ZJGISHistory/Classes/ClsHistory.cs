using System;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using System.Data.OracleClient;
using System.Data;
using System.Windows.Forms;
using ZJGISCommon;
using DevComponents.DotNetBar;

namespace ZJGISHistory
{
  public  static class ClsHistory
    {
        private static IWorkspace g_pWks;                                       //当前SDE工作区
        public static IWorkspace WorkSpace
        {
            get
            {
                return g_pWks;
            }
            set
            {
                g_pWks = value;
            }
        }
        private static IHistoricalWorkspace g_pHistoricalWorkspace;//SDE历史工作区
        public static IHistoricalWorkspace HistoricalWorkspace
        {
            get
            {
                return g_pHistoricalWorkspace;
            }
            set
            {
                g_pHistoricalWorkspace = value;
            }
        }
        private static IMap g_pMap;
        public static IMap Map
        {
            get
            {
                return g_pMap;
            }
            set
            {
                g_pMap = value;
            }
        }
        private static bool g_bHisLayer;                   //如果加载的本身就是历史数据，就不用判断该数据是否有历史了
        public static bool HistoryLayer
        {
            get
            {
                return g_bHisLayer;
            }
            set
            {
                g_bHisLayer = value;
            }
        }
        private static OracleConnection g_Conn=new OracleConnection();         //当前SDE对应的数据库连接
        public static OracleConnection Connection
        {
            get
            {
                return g_Conn;
            }
            set
            {
                g_Conn = value;
            }
        }

        public static void OpenConn()
        {
            if (string.IsNullOrEmpty(g_Conn.ConnectionString))
            {
                g_Conn.ConnectionString = "Data Source=" +"orcl" + ";password=" + "lgq" + ";User ID= " + "sde";
                g_Conn.Open();
            }

            if (g_Conn.State == ConnectionState.Closed)
            {
                g_Conn.Open();
            }
        }
        ////得到系统服务器的时间
        public static DateTime GetSystemTime()
        {

            OracleCommand vOraCommand = default(OracleCommand);
            OracleDataReader vOraReader = null;

            try
            {
                vOraCommand = new OracleCommand();
                vOraCommand.Connection = g_Conn;
                vOraCommand.CommandText = "Select sysdate AS curTime from dual ";

                if (g_Conn.State == ConnectionState.Closed)
                {
                    OpenConn();
                }

                vOraReader = vOraCommand.ExecuteReader();

                if (vOraReader.Read())
                {
                    return (DateTime)vOraReader["curTime"];
                }
                vOraReader.Close();
            }
            catch (Exception )
            {
                MessageBoxEx.Show("读取数据库时间失败!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (vOraReader != null)
                {
                    if (vOraReader.IsClosed == false)
                        vOraReader.Close();
                    vOraReader = null;
                }
                vOraCommand = null;
            }
            return DateTime.MaxValue;
        }

        public static void AddHisMarkerToCbo(ref ComboBox cbo)
        {
            OracleCommand vOraCommand = default(OracleCommand);
            OracleDataReader vOraReader = default(OracleDataReader);
            string sQueryString = null;

            try
            {
                cbo.Items.Clear();
                ////直接从数据库中读sde.gdb_historicalmarkers的表，便于排序和将hm_timestamp字段放在tag上
                ////读取SDE中历史标记的记录
                sQueryString = "select * from sde.gdb_historicalmarkers t order by t.hm_timestamp";
                vOraCommand = new OracleCommand(sQueryString, g_Conn);

                if (g_Conn.State == ConnectionState.Closed)
                {
                    g_Conn.Open();
                }
                vOraReader = vOraCommand.ExecuteReader();

                while (vOraReader.Read())
                {
                    cbo.Items.Add(vOraReader["hm_name"]);
                }

                //设置默认选择项
                cbo.Items.Add("请选择");
                cbo.Text = "请选择";

                vOraReader.Close();
                vOraReader = null;

            }
            catch (Exception )
            {
                MessageBoxEx.Show("读取数据库历史标记失败!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        ////获取指定表名的查询命令
        public static string GetTableNameRec(string strTableName, OracleConnection Syscn)
        {
            OracleDataReader vReader = default(OracleDataReader);
            OracleCommand vOraCommand = default(OracleCommand);
            string sQueryString = null;
            string sHTableName = "";

            sQueryString = "select table_name from sde.table_registry where table_name like '" + ClsString.SplitName(strTableName).ToUpper() + "_H%'order by table_name";
            vOraCommand = new OracleCommand(sQueryString, Syscn);
            vReader = vOraCommand.ExecuteReader();
            while (vReader.Read())
            {
                sHTableName = vReader["table_name"].ToString();
            }
            return sHTableName;
        }


    }
}
