using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZJGISDataUpdating.Class
{
    public struct StrRowAndCol
    {
        //字段  
        private int row;
        private int column;
        private string _value;

        //行属性  
        public int Row
        {
            get
            {
                return row;
            }
            set
            {
                row = value;
            }
        }
        //列属性  
        public int Column
        {
            get
            {
                return column;
            }
            set
            {
                column = value;
            }
        }
        //值属性  
        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }  
    }
}
