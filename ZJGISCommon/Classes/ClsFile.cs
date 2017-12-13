using System;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace ZJGISCommon
{
    public class ClsFile
    {
        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        /// <param name="FName">文件名</param>
        /// <returns>存在与否</returns>
        public bool FileExist(string FName)
        {
            
            if (File.Exists(FName))
            {
                //文件存在
                return true;
            }
            else
            {
                //文件不存在
                return false;
            }
        }


        /// <summary>
        /// 取得字符串中分隔符的个数
        /// </summary>
        /// <param name="sFiles">字符串</param>
        /// <param name="vSearchChar">分隔符</param>
        /// <returns>分隔符个数</returns>
        public int CountDelimiters(string sFiles, object vSearchChar)
        {
            //试分割
            string [] temp=sFiles.Split(vSearchChar.ToString().ToCharArray());

            //返回分割数
            return temp.Length-1;
        }


        /// <summary>
        /// 获得一个用间隔符分开的字符串中的某个序号的字符串
        /// </summary>
        /// <param name="str">字符</param>
        /// <param name="Identifiers">分隔符</param>
        /// <param name="num">序号</param>
        /// <returns>num序号的字符串</returns>
        public string GetStrByNumber(string str, string Identifiers, int num)
        {
            if (num<=0||str==null)
            {
                return null;
            }

            //分割
            string[] Splits;
            Splits = str.Split(Identifiers.ToCharArray());

            if (num>Splits.Length)
            {
                //大于数组长度
                return null;
            }
            else
            {
                //返回字符串
                return Splits[num - 1];
            }
        }


        /// <summary>
        /// 获得ini文件中对应的key的值
        /// </summary>
        /// <param name="Tmp_File"></param>
        /// <param name="Tmp_Key">键值</param>
        /// <param name="FileName">文件名</param>
        /// <returns></returns>
        public string GetKey(string Tmp_File, string Tmp_Key, string FileName)
        {
            StringBuilder ret=new StringBuilder(" ");

            if (FileExist(Tmp_File)==false)
            {
                //文件不存在
                return null;
            }

            //得到字符串
            GetPrivateProfileString("DbConnect Information", Tmp_Key, " ", ret, 30, FileName);

            //返回字符串
            return ret.ToString().Substring(0,ret.ToString().IndexOf(Convert.ToChar(0)));
        }


        //声明库api
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath); 
    }
}
