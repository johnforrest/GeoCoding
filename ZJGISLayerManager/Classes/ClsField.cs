//<CSCC>
//-------------------------------------------------------------------------------------
//    部件名：ClsField
//    工程名：LayerManager
//    版权: CopyRight (c) 2010
//    创建人：
//    描述：对字段名称和别名的操作
//    创建日期：
//    修改人：
//    修改说明：
//    修改日期：
//-------------------------------------------------------------------------------------
//</CSCC>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZJGISLayerManager
{
    class ClsField
    {
        private string m_Name;
        private string m_AliasName;

        public string Name
        {
            get
            {
                return m_Name;
            }
            set
            {
                m_Name = value;
            }
        }

        public string AliasName
        {
            get
            {
                return m_AliasName;
            }
            set
            {
                m_AliasName = value;
            }
        }

    }
}
