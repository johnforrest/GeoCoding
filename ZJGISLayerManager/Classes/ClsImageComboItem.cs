//<CSCC>
//-------------------------------------------------------------------------------------
//    部件名：ClsImageComboItem
//    工程名：LayerManager
//    版权: CopyRight (c) 2010
//    创建人：
//    描述：
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
    class ClsImageComboItem
    {
        private string m_Text;
        private int m_ImageIndex;

        public string Text
        {
            get
            {
                return m_Text;
            }
            set
            {
                m_Text = value;
            }
        }

        public int ImageIndex
        {
            get
            {
                return m_ImageIndex;
            }
            set
            {
                m_ImageIndex = value;
            }
        }

    }
}
