using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using DevComponents.DotNetBar.Controls;
using System.Data.OracleClient;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto; //添加方式

namespace ZJGISDataUpdating.Class
{
    public sealed class ClsDeclare
    {
       
        public static OracleConnection g_Connection;
        //匹配结果文件最终保存的路径
        public static string g_WorkspacePath;
        //源数据比例尺
        public static string strFrom;
        //待匹配数据比例尺
        public static string strTo;
        //同尺度匹配
        public static bool g_SameScaleMatch;
        //多尺度匹配
        public static bool g_DifScaleMatch;

        public static IWorkspace g_SdeWorkspace;
        public static bool g_SourceToBase;
        public static bool g_BaseToFrame;
        public static bool g_FrameToFrame;

        public static Dictionary<string, string> g_SourceFeatClsPathDic;
        public static Dictionary<string, string> g_WorkFeatClsPathDic;

        


    }
}
