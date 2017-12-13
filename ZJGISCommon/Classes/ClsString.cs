using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using ESRI.ArcGIS.esriSystem;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace ZJGISCommon
{
    public class ClsString
    {
        /// <summary>
        /// 将字符串信息初始成数据库查询信息
        /// </summary>
        /// <param name="DBcn">oracle连接</param>
        /// <param name="TableName">表名</param>
        /// <param name="condition">条件查询</param>
        /// <param name="BinaryRec">欲查询信息</param>
        /// <returns>字符串</returns>
        public string ReadBinaryToStringDB(OracleConnection DBcn, string TableName,
            string condition, string BinaryRec)
        {
            OracleCommand DBCommand = null;
            OracleDataAdapter DBAdapter = null;

            string MySQL = " Select " + BinaryRec + " From " + TableName
                + " Where " + condition;

            try
            {
                DBCommand = new OracleCommand(MySQL, DBcn);
                DBAdapter = new OracleDataAdapter(DBCommand);

                return DBAdapter.ToString();
            }
            catch (Exception)
            {
                return null;
            }
        }


        /// <summary>
        /// 自动初始化属性集
        /// </summary>
        /// <param name="pPropset">属性集</param>
        /// <param name="strPropset">初始字符串</param>
        public void InitPropset(IPropertySet pPropset, string strPropset)
        {
            //先以分号分割
            string[] sArray = null;
            sArray = strPropset.Split(new char[] { ';' });

            //再用逗号分割并初始化属性集
            string[] sArray2 = null;
            for (int i = 0; i < sArray.Length; i++)
            {
                sArray2 = sArray[i].Split(new char[] { ',' });
                pPropset.SetProperty(sArray2[0], sArray2[1]);
            }
        }


        /// <summary>
        /// 获得最后一个点后的字串（一般用来或的文件后缀名）
        /// </summary>
        /// <param name="pName">待处理串</param>
        /// <returns>处理后的子串</returns>
        public static string SplitName(string pName)
        {
            int i = 0;
            string[] strSplit = null;

            if (pName == null)
            {
                return null;
            }

            //用点分割
            strSplit = pName.Split(new char[] { '.' });

            //获得最后一个子串
            i = strSplit.Length - 1;
            return strSplit[i];
        }


        /// <summary>
        /// 将一个字符串中一某个分隔符分割后的某个字符串替换为新字符串
        /// </summary>
        /// <param name="str">待操作字符串</param>
        /// <param name="identifiers">分隔符</param>
        /// <param name="num">替换位置</param>
        /// <param name="replaceStr">用来替换的字符串</param>
        /// <returns>完成操作后的字符串</returns>
        public string ReplaceStrByNumber(string str, string identifiers,
            int num, string replaceStr)
        {
            StringBuilder strTemp = new StringBuilder();

            if (num <= 0)
            {
                return null;
            }

            if (str == null)
            {
                return null;
            }

            //分割
            string[] Splits = null;
            Splits = str.Split(new string[] { identifiers }, str.Length,
                StringSplitOptions.RemoveEmptyEntries);

            if (num > Splits.Length)
            {
                return null;
            }

            //插入新字符串
            Splits[num - 1] = replaceStr;

            //重组字符串
            for (int i = 0; i < Splits.Length; i++)
            {
                if (i == 0)
                {
                    strTemp.Append(Splits[i]);
                }
                else
                {
                    strTemp.Append(identifiers + Splits[i]);
                }
            }

            return strTemp.ToString();
        }


        /// <summary>
        /// 见字符串分割成多个子串
        /// </summary>
        /// <param name="strInterval">分隔符</param>
        /// <param name="strArray">分割后的字符串数组</param>
        /// <param name="str">待分割串</param>
        public void GetStrArray(string strInterval, ref string[] strArray, string str)
        {
            strArray = str.Split(new string[] { strInterval }, StringSplitOptions.None);
        }


        /// <summary>
        /// 把NULL值转成空字符串，如果不是NULL则不转
        /// </summary>
        /// <param name="vValue">待转换的变量</param>
        /// <returns>转换的值</returns>
        public string ConvertWRTNull(object vValue)
        {
            if (!Convert.IsDBNull(vValue))//如果不是空
            {
                return vValue.ToString();
            }
            else
            {
                return string.Empty;
            }
        }


        /// <summary>
        /// 作简单的字符解密
        /// </summary>
        /// <param name="MyStr">欲解密字符</param>
        /// <returns>解密后字符</returns>
        public static string UnEncryptStr(string MyStr)
        {
            StringBuilder tmpStr = new StringBuilder();

            //每个字符的编码加五后对应的字符作为解密后字符
            for (int i = 0; i < MyStr.Length; i++)
            {

                System.Diagnostics.Debug.WriteLine(MyStr.Substring(i, 1));
                tmpStr.Append((char)((int)MyStr[i] + 5));

            }

            return tmpStr.ToString();
        }


        /// <summary>
        /// 作简单的字符串加密
        /// </summary>
        /// <param name="MyStr">欲加密字符串</param>
        /// <returns>加密后字符串</returns>
        public static string EncryptStr(string MyStr)
        {
            StringBuilder temStr = new StringBuilder();

            //每个字符减五后对应的字符作为加密后字符
            for (int i = 0; i < MyStr.Length; i++)
            {
                temStr.Append((char)((int)MyStr[i] - 5));
            }

            return temStr.ToString();
        }

        /// <summary>
        /// DES加密算法（暂未使用）
        /// </summary>
        /// <param name="str">欲加密字符串</param>
        /// <param name="Key">TripeDes算法随机密钥</param>
        /// <param name="IV">TripeDes的初始化向量</param>
        /// <returns></returns>
        public static string EncryptText(string str)
        {
            byte[] Key = Encoding.UTF8.GetBytes(GetMD5("whu110", "sss").Substring(2,16));
            byte[] IV = Encoding.UTF8.GetBytes(GetMD5("kanshenmekan", "sss").Substring(15, 16));
            MemoryStream memoryStream = new MemoryStream();

            CryptoStream cryptoStream = new CryptoStream(memoryStream,
                new TripleDESCryptoServiceProvider().CreateEncryptor(Key, IV),
                CryptoStreamMode.Write);
            byte[] toEncrypt = Encoding.UTF8.GetBytes(str);
            byte[] encryptedBytes = null;

            try
            {
                cryptoStream.Write(toEncrypt, 0, toEncrypt.Length);
                cryptoStream.FlushFinalBlock();

                encryptedBytes = memoryStream.ToArray();

            }
            catch (CryptographicException err)
            {
                MessageBox.Show("加密出错" + err.Message);
            }
            finally
            {
                cryptoStream.Close();
                memoryStream.Close();
            }
            return Convert.ToBase64String(encryptedBytes);
        }


        /// <summary>
        /// DES解密算法（暂未使用）
        /// </summary>
        /// <param name="dataBytes"></param>
        /// <param name="Key">TripeDes算法随机密钥</param>
        /// <param name="IV">TripeDes的初始化向量</param>
        /// <returns>解密后字符</returns>
        public static string DecryptText(string dataString)
        {
            byte[] dataBytes =Convert.FromBase64String(dataString);
            byte[] Key = Encoding.UTF8.GetBytes(GetMD5("whu110", "sss").Substring(2, 16));
            byte[] IV = Encoding.UTF8.GetBytes(GetMD5("kanshenmekan", "sss").Substring(15, 16));
            MemoryStream memoryStream = new MemoryStream(dataBytes);

            CryptoStream cryptoStream = new CryptoStream(memoryStream,
                new TripleDESCryptoServiceProvider().CreateDecryptor(Key, IV),
                CryptoStreamMode.Read);
            string decryptedString = null;
            try
            {
                byte[] decryptBytes = new byte[dataBytes.Length];
                cryptoStream.Read(decryptBytes, 0, decryptBytes.Length);
                decryptedString = Encoding.UTF8.GetString(decryptBytes);

            }
            catch (CryptographicException err)
            {
                MessageBox.Show("解密出错" + err.Message);
            }
            finally
            {
                cryptoStream.Close();
                memoryStream.Close();
            }

            return decryptedString.Trim('\0');
        }

        /// <param name="sDataIn">需要加密的字符串</param>
        /// <param name="move">偏移量</param>
        /// <returns>sDataIn加密后的字符串</returns>
        public static string GetMD5(string sDataIn, string move)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bytValue, bytHash;
            bytValue = System.Text.Encoding.UTF8.GetBytes(move + sDataIn);
            bytHash = md5.ComputeHash(bytValue);
            md5.Clear();
            string sTemp = "";
            for (int i = 0; i < bytHash.Length; i++)
            {
                sTemp += bytHash[i].ToString("x").PadLeft(2, '0');
            }
            return sTemp;
        }
    }
}
