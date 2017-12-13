using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace ZJGISDataExtract.Classes
{
   public class ExtendArray
    {
       private  int intFirstStr;
       private int intSecond;
       private string[] FirstArray;
       private string[] SecondArray;
       public ExtendArray(int i, int j)
       {
           this.intFirstStr = i;
           this.intSecond = j;
       }
       private void Intilt()
       {
            FirstArray = new string[intFirstStr];
            SecondArray = new string[intSecond];
       }
       public void SetValue(string[] arr1, string[] arr2)
       {
           if (arr1.Length != intFirstStr)
           {
               MessageBoxEx.Show("传入数组长度错误！", "提示");
           }
           if (arr2.Length != intSecond)
           {
               MessageBoxEx.Show("传入数组长度错误！", "提示");
           }
           else
           {
               FirstArray = arr1;
               SecondArray = arr2;
           }
       }

    }
}
