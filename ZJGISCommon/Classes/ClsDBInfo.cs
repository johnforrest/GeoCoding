using ESRI.ArcGIS.Geodatabase;
using System.Data.OracleClient;


namespace ZJGISCommon
{

    public class ClsDBInfo
    {
        private static IWorkspace sdeWorkspace;
        private static OracleConnection oracleConn;
        private static ClsConnectInfo connInfo;
        public ClsDBInfo(ClsConnectInfo info)
        {
            connInfo = info;
        }

        public static OracleConnection OracleConn
        {
            get
            {
                if (oracleConn==null)
                {
                    oracleConn = new OracleConnection(connInfo.GetAdoConn());
                }
                return oracleConn;
            }
        }
        public static IWorkspace SdeWorkspace
        {
            get
            {
                if (sdeWorkspace==null)
                {
                    sdeWorkspace = connInfo.GetWorkspace();
                }
                return sdeWorkspace;
            }
            set
            {
                sdeWorkspace = value;
            }
        }

    }
}
