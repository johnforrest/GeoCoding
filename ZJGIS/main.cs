using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ZJGISCommon;
using ESRI.ArcGIS.esriSystem;
namespace ZJGIS
{

    static class main
    {

        //private IAoInitialize m_AoInitialize = new AoInitializeClass();

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.EngineOrDesktop);

            ////20170310
            ////实例化数据连接信息 写入连接SDE的设置信息
            //ClsConnectInfo connectInfo = new ClsConnectInfo();
            //connectInfo.GetInfoFromXml();
            //ClsDBInfo dbInfo = new ClsDBInfo(connectInfo);

            Application.Run(new frmMain());
        }
    }
}
