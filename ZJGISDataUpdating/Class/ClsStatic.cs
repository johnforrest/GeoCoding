using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZJGISDataUpdating.Class
{
    public static class ClsStatic
    {
        public static string[] SplitStrColon(string str)
        {
            if (str==null)
            {
                return null;
            }
            String[] arr = new string[2];
            if (str.Contains(":"))
            {
                arr = str.Split(':');
            }

            return arr;
        }
    }
}
