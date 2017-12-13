using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZJGISDataUpdating.Class
{
    public class ClsConstant
    {
        //public const int compiletimeConstant = 1;
        public const string One2Zero = "未匹配";
        public const string One2One = "一对一";
        public const string One2More = "一对多";

        public const string More2One = "多对一";
        public const string More2More = "多对多";



        public const string pointSettingTable = "MatchedPointFCSetting";
        public const string lineSettingTable = "MatchedPolylineFCSetting";
        public const string polygonSettingTable = "MatchedPolygonFCSettingDif";
    }
}
