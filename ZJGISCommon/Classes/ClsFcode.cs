using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.esriSystem;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace ZJGISCommon.Classes
{
    public static class ClsFcode
    {
        //数字分类码是键，大类是值
        public static Dictionary<string, string> pDicFcodeGlobal = new Dictionary<string, string>();

        static ClsFcode()
        {
            try
            {
                string path = new DirectoryInfo("../").FullName + @"Res\xls\地理实体分类.xls";
                string filename = System.IO.Path.GetFileName(path);

                //打开myxls.xls文件
                //using (System.IO.FileStream fs = File.OpenRead(@"c:/myxls.xls"))
                using (System.IO.FileStream fs = new System.IO.FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    //把xls文件中的数据写入wk中
                    HSSFWorkbook wk = new HSSFWorkbook(fs);
                    //NumberOfSheets是myxls.xls中总共的表数
                    for (int i = 0; i < wk.NumberOfSheets; i++)
                    {
                        //读取当前表数据
                        ISheet sheet = wk.GetSheetAt(i);
                        //j=1把第一行的标题过滤掉
                        for (int j = 1; j <= sheet.LastRowNum; j++)
                        {
                            //读取当前行数据
                            IRow row = sheet.GetRow(j);
                            if (row != null)
                            {
                                ICell cell1 = row.GetCell(0);  //当前表格
                                ICell cell2 = row.GetCell(2);  //当前表格
                                if (cell1 != null && cell2 != null)
                                {
                                    //大类
                                    string cell1value = cell1.ToString().Trim();
                                    //数字分类码
                                    string cell2value = cell2.ToString().Trim();
                                    if (cell1value.Length > 0 && cell2value.Trim().Length > 0 && !pDicFcodeGlobal.ContainsKey(cell2value))
                                    {
                                        pDicFcodeGlobal.Add(cell2value, cell1value);
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                               
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
            }
        }




    }
}
