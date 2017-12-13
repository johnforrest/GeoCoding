using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace ZJGISGCoding
{
    class ClsLog
    {
        public static void WriteFile(Exception ex,string FID)
        {
            String sFileName;
            String sFilePath = Path.Combine(Application.StartupPath, "Log");
            if (Directory.Exists(sFilePath) == false)
                Directory.CreateDirectory(sFilePath);
            else
            {
                DirectoryInfo dInfo = new DirectoryInfo(sFilePath);
                if (dInfo.GetFiles().Length > 100)
                    foreach (FileInfo fInfo in dInfo.GetFiles())
                        fInfo.Delete();
            }
            //用当前日期（年月日）作为文件名
            sFileName = DateTime.Now.ToShortDateString().Replace("/", "-") + ".log";  //文件名不能包括:
            sFilePath = Path.Combine(sFilePath, sFileName);

            StreamWriter streamWriter;

            if (File.Exists(sFilePath))
                streamWriter = File.AppendText(sFilePath);
            else
                streamWriter = File.CreateText(sFilePath);

            streamWriter.WriteLine();
            streamWriter.WriteLine(DateTime.Now.ToString());
            streamWriter.WriteLine(ex.ToString());
            streamWriter.WriteLine(ex.Message);
            streamWriter.WriteLine(ex.InnerException);
            streamWriter.WriteLine(FID + "编码错误");
            streamWriter.Close();
        }
    }
}
