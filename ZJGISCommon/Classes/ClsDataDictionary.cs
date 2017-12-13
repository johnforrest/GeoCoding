using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZJGISCommon
{
    class ClsDataDictionary
    {
        #region "数据字典对照表"
        public static Dictionary<string, string> ZJDataRef = new Dictionary<string, string>
        {

           {"1:10000界桩点","SDE.PIL_PT_10K"},
           {"1:10000境界线","SDE.BOU_LN_10K"},
           {"1:10000政区面","SDE.REG_PY_10K"},
           {"1:10000居民地面","SDE.RES_PY_10K"},
           {"1:10000房屋面","SDE.HOU_PY_10K"},
           {"1:10000街区面","SDE.NEI_PY_10K"},
           {"1:10000道路线","SDE.ROD_LN_10K"},
           {"1:10000道路网线","SDE.ROD_NET_LN_10K"},
           {"1:10000交通铁路线","SDE.TRA_LRR_LN_10K"},
           {"1:10000交通路网线","SDE.TRA_NET_LN_10K"},
           {"1:10000河流线","SDE.HYD_LN_10K"},
           {"1:10000水网线","SDE.HYD_NET_LN_10K"},
           {"1:10000湖泊面","SDE.LAK_PY_10K"},
           {"1:10000水库面","SDE.RER_PY_10K"},
           {"1:10000植被面","SDE.VEG_PY_10K"},
           {"1:10000等高线","SDE.CON_LN_10K"}
          
        };
        #endregion
    }
}
