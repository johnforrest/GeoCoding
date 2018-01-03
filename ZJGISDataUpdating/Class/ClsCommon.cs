using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ZJGISDataUpdating.Class
{
    public class ClsCommon
    {



        /// <summary>
        /// 两字符串中有几个相同字符
        /// </summary>
        /// <param name="fir">第一个字符串</param>
        /// <param name="sec">第二个字符串</param>
        /// <returns></returns>
        public int StringSameOrNot(string fir, string sec)
        {
            int same = 0;
            //去掉指定字符
            fir = RemoveString(fir);
            sec = RemoveString(sec);
            if (fir == sec)   //第一种判断方式 
            {
                same = 2;
            }
            else
            {
                int flag = sec.Length;
                StringBuilder sbfir = new StringBuilder();
                StringBuilder sbsec = new StringBuilder();
                sbfir.Append(fir);
                sbsec.Append(sec);

                int strlength = 0;
                if (sbfir.Length == sbsec.Length)
                {
                    strlength = sbfir.Length;
                }
                else if ((sbfir.Length < sbsec.Length))
                {
                    strlength = sbfir.Length;
                }
                else
                {
                    strlength = sbsec.Length;
                }

                try
                {

                    if (strlength < 4)
                    {
                        for (int i = 0; i < strlength; i++)
                        {
                            if (sbfir.ToString().Substring(i, 1) == sbsec.ToString().Substring(i, 1))
                            {
                                same++;
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            if (sbfir.ToString().Substring(i, 1) == sbsec.ToString().Substring(i, 1))
                            {
                                same++;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            return same;
        }
        /// <summary>
        /// 两字符串中有几个相同字符
        /// </summary>
        /// <param name="fir">第一个字符串</param>
        /// <param name="sec">第二个字符串</param>
        /// <returns></returns>
        public int StringSameOrNot2(string fir, string sec)
        {
            int same = 0;
            //去掉指定字符
            fir = RemoveString(fir);
            sec = RemoveString(sec);


            if (fir == sec)   //第一种判断方式 
            {
                same = 3;
            }
            else
            {
                int flag = sec.Length;
                StringBuilder sbfir = new StringBuilder();
                StringBuilder sbsec = new StringBuilder();
                sbfir.Append(fir);
                sbsec.Append(sec);

                int strlength = 0;
                if (sbfir.Length == sbsec.Length)
                {
                    strlength = sbfir.Length;
                }
                else if ((sbfir.Length < sbsec.Length))
                {
                    strlength = sbfir.Length;
                }
                else
                {
                    strlength = sbsec.Length;
                }

                try
                {

                    if (strlength < 4)
                    {
                        for (int i = 0; i < strlength; i++)
                        {
                            if (sbfir.ToString().Substring(i, 1) == sbsec.ToString().Substring(i, 1))
                            {
                                same++;
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < 4; i++)
                        //for (int i = 0; i < 3; i++)
                        {
                            if (sbfir.ToString().Substring(i, 1) == sbsec.ToString().Substring(i, 1))
                            {
                                same++;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            return same;
        }

        /// <summary>
        /// 去掉字符串指定字符
        /// </summary>
        /// <param name="fir"></param>
        public string RemoveString(string fir)
        {
            //判断字符串是否包含指定的字符
            if (fir.IndexOf("德清县") > -1)
            {
                fir = fir.Replace("德清县", "");
            }
            else if (fir.IndexOf("德清") > -1)
            {
                fir = fir.Replace("德清", "");
            }

            if (fir.IndexOf("浙江省") > -1)
            {
                fir = fir.Replace("浙江省", "");
            }
            else if (fir.IndexOf("浙江") > -1)
            {
                fir = fir.Replace("浙江", "");
            }

            if (fir.IndexOf("浙江德清") > -1)
            {
                fir = fir.Replace("浙江德清", "");
            }
            return fir;
        }



    }
}
