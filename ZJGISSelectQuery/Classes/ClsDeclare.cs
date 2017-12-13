using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Carto;
using System.Windows.Forms;
using ESRI.ArcGIS.Geometry;
using ZJGISCommon;

namespace SelectQuery
{
    public static class ClsDeclare
    {
        public static ClsSysSet g_Sys;
        public static IMap g_pMap;
        public static String g_SysSelectStyle;
        public static bool g_blnSnap;
        public static RichTextBox g_txtMeasure;
        public static string g_strUnit;
        public static IProjectedCoordinateSystem g_pProjectedCoordinateSystem;
        public static string strQueryDictionary;
        //public static ErrorHandle.ClsErrorHandle g_ErrorHandler = new ErrorHandle.ClsErrorHandle();
        public static ClsConvertUnit g_UnitConverter;
        public static string g_MapNoField = "code";
        public static string g_strTFMapPath = Application.StartupPath + @"\..\res\cachedb\map";
    }
}
