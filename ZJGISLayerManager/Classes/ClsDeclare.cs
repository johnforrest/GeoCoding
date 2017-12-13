using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
//using ErrorHandle;
//using ZJGISDialog;

namespace ZJGISLayerManager
{
    static class ClsDeclare
    {
        //public static ClsSysSet g_Sys = new ClsSysSet();
        //public static ClsErrorHandle g_ErrorHandler = new ClsErrorHandle();
        public static string g_ColorFile = System.IO.Directory.GetParent(System.Windows.Forms.Application.StartupPath).FullName + @"\Res\txt\ColorRamps.txt";

        public static string g_StyleFolder = System.IO.Directory.GetParent(System.Windows.Forms.Application.StartupPath).FullName + @"\Res\style\";
                                     
    }
}
