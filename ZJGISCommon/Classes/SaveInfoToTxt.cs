using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DevComponents.DotNetBar;

namespace ZJGISCommon
{
    /// <summary>
    /// txt写入和读取
    /// </summary>
 public  class SaveInfoToTxt
    {
     /// <summary>
     /// 写入内容到txt
     /// </summary>
     /// <param name="path"></param>
     /// <param name="content"></param>
     public void InsertToTxt(string path, string content)
     {
         //string txtName = "SaveExtractInfo.txt";
         //string txtPath = System.Windows.Forms.Application.StartupPath + "\\..\\Res\\path\\" + txtName;
         if (content != null || content != "")
         {
             if (File.Exists(path))
             {
                 StreamWriter sw = new StreamWriter(path);
                 sw.WriteLine(path);
                 sw.Close();
             }
             else
             {
                 FileStream fileStream = new FileStream(path, FileMode.CreateNew);
                 fileStream.Close();
                 StreamWriter sw = new StreamWriter(path);
                 sw.WriteLine(content);
                 sw.Close();
             }
         }
     }
     /// <summary>
     /// 读取txt中所有要素
     /// </summary>
     /// <param name="path"></param>
     /// <param name="content"></param>
     public void ReadTxt(string path, out string content)
     {        
             if (File.Exists(path))
             {
                 StreamReader sr = new StreamReader(path);
                 content = sr.ReadToEnd();
             }
             else
             {
                 MessageBoxEx.Show("路径有错");
                content=null ;
             }
     }
    }
}
