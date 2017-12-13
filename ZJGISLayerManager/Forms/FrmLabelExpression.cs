//<CSCC>
//-------------------------------------------------------------------------------------
//    部件名：FrmLabelExpression
//    工程名：LayerManager
//    版权: CopyRight (c) 2010
//    创建人：
//    描述：设置显示注记的表达式
//    创建日期：
//    修改人：MXF
//    修改说明：小错误修改
//    修改日期：11-08-2008
//-------------------------------------------------------------------------------------
//</CSCC>


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using Microsoft.VisualBasic;
//using ErrorHandle;
//using ZJGISDialog;

namespace ZJGISLayerManager
{
    public partial class FrmLabelExpression :DevComponents.DotNetBar.Office2007Form
    {
        private IGeoFeatureLayer m_pGeoFeatLayer;
        private ILabelEngineLayerProperties m_pLabelEngineLayerProperties;

        //private ClsErrorHandle m_pDisplayInformation = new ClsErrorHandle();
        
        public FrmLabelExpression()
        {
            InitializeComponent();
        }

        public IGeoFeatureLayer GeoFeatureLayer
        {
            set
            {
                m_pGeoFeatLayer = value;
            }
        }

        public ILabelEngineLayerProperties LabelEngineLayerProperties
        {
            get
            {
                return m_pLabelEngineLayerProperties;
            }
            set
            {
                m_pLabelEngineLayerProperties = value;
            }
        }

        private void FrmLabelExpression_Load(object sender, EventArgs e)
        {
            IFields pFields;
            IField pField;
            int i;
            string sValue;

            //初始化字段列表
            if (m_pGeoFeatLayer!=null)
            {
                pFields = m_pGeoFeatLayer.FeatureClass.Fields;

                lstFieldName.Columns.Add("字段");
                lstFieldName.Columns[0].Width = this.lstFieldName.Width;

                for (i = 0; i <= pFields.FieldCount-1; i++)
                {
                    pField = pFields.get_Field(i);
                    if ((pField.Type!=esriFieldType.esriFieldTypeGeometry)&&(pField.Type!=esriFieldType.esriFieldTypeBlob))
                    {
                        lstFieldName.Items.Add(pField.Name + "[" + pField.AliasName + "]");
                    }
                }
            }

            //初始化表达式
            txtExpression.Multiline = true;
            if (m_pLabelEngineLayerProperties!=null)
            {
                sValue = m_pLabelEngineLayerProperties.Expression;
                txtExpression.Text = sValue;
            }

        }

        //附加
        private void btnAppendix_Click(object sender, EventArgs e)
        {
            Appendix();
        }

        //清除
        private void btnClear_Click(object sender, EventArgs e)
        {
            txtExpression.Text = "";
            btnClear.Enabled = false;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtExpression.Text.Trim()=="")
            {
                //m_pDisplayInformation.DisplayInformation("表达式为空,请填写!", false, null, " 退 出 ");
                MessageBox.Show("表达式为空,请填写!", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                return;
            }

            if (verifyExpression(txtExpression.Text.Trim())==true)
            {
                m_pLabelEngineLayerProperties.Expression = txtExpression.Text;
                this.Close();
            }
            else
            {
                //if (m_pDisplayInformation.DisplayInformation("也许这是一个复杂表达式,无法正确的检查,要继续吗?", true, " 是 ", " 否 "))
                    if (MessageBox.Show("也许这是一个复杂表达式,无法正确的检查,要继续吗?", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)==DialogResult.OK)
                {
                    m_pLabelEngineLayerProperties.Expression = txtExpression.Text;
                    this.Close();
                }
            }
        }

        //**************************************************************************************
       
        //** 功能描述 : 粗略的检查注记的表达式是否正确(只检查简单表达式,不检查复杂表达式)
       
        //**************************************************************************************
        private bool verifyExpression(string sExpression)
        {
            int i;                              //左括号的位置
            int j;                              //右括号的位置
            int k;                              //][中间字符串 " 的位置
            int m;                              //][中间字符串&的位置
            string sValue = "";                 //字段名称
            string[] sValueArray;
            string sChar;                       //左右括号中间的字符
            string sTempExpression;             //用于记录表达式的中间变量

            Dictionary<string, int> DicAllFieldName = new Dictionary<string, int>();

            //添加所有的字段名
            for (i = 0; i <= lstFieldName.Items.Count-1; i++)
            {
                sValue = this.lstFieldName.Items[i].Text;
                sValueArray = sValue.Split(new char[] { '[' });
                sValue = Strings.UCase(sValueArray[0]);

                if (DicAllFieldName.ContainsKey(Strings.UCase(sValue))==false)
                {
                    DicAllFieldName.Add(Strings.UCase(sValue), DicAllFieldName.Count);
                }
            }

            if ((sExpression+"").Trim()!="")
            {
                //检查表达式的第一个字符是否正确,:第一个字符应该是字母或[,否则就是错误的
                sValue = Strings.Left(sExpression.Trim(), 1);

                if (sValue!="[")
                {
                    //如果是字母或数字
                    if (((Strings.Asc(sValue)<123)&&(Strings.Asc(sValue)>96))||((Strings.Asc(sValue)<91&&(Strings.Asc(sValue)>64))||(Strings.Asc(sValue)<58&&Strings.Asc(sValue)>47)))
                    {

                    }
                    else
                    {
                        //m_pDisplayInformation.DisplayInformation("表达式的开始处有误,请检查!", false, null, "退出");
                        MessageBox.Show("表达式的开始处有误,请检查!", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                        return false;
                    }
                }

                //检查左右括号是否丢失
                if (Strings.InStr(sExpression,"[",CompareMethod.Text)==0)
                {
                    //m_pDisplayInformation.DisplayInformation("缺失左括号,请检查", false,"" , "退出");
                    MessageBox.Show("缺失左括号,请检查!", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    return false;
                }

                sTempExpression = sExpression.Trim();
                while (Strings.InStr(sTempExpression,"[",CompareMethod.Text)>0)
                {
                    i = Strings.InStr(sTempExpression, "[", CompareMethod.Text);        //存在左括号
                    j = Strings.InStr(sTempExpression, "]", CompareMethod.Text);        //存在右括号

                    if (i > 0)        //存在左括号
                    {

                        if (j > 0)      //存在右括号  
                        {
                            //获取位于[]内的字符
                            if (i<j)
                            {
                                sChar = Strings.Mid(sTempExpression, i + 1, j - i - 1);

                                //检查[]中间是否包函有& + " 等符号,如果包涵则视为丢失右括号
                                if (Strings.InStr(sChar,"&",CompareMethod.Text)>0||Strings.InStr(sChar,"+",CompareMethod.Text)>0||Strings.InStr(sChar,"\"",CompareMethod.Text)>0)
                                {
                                    //m_pDisplayInformation.DisplayInformation("缺丢右括号,请检查", false, null,"退出");
                                    MessageBox.Show("缺丢右括号,请检查!", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                                    return false;
                                }
                                else if ((sChar+"").Trim()=="")
                                {
                                    //m_pDisplayInformation.DisplayInformation("有一对括号内的字段名称为空,请检查!", false, null, "退出");
                                    MessageBox.Show("有一对括号内的字段名称为空,请检查!", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                                    return false;
                                }
                                else if (DicAllFieldName.ContainsKey(Strings.UCase(sChar))==false)
                                {
                                    //m_pDisplayInformation.DisplayInformation("有一对括号内的字段名称错误", false, null, "退出");
                                    MessageBox.Show("有一对括号内的字段名称错误,请检查!", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                                    return false;
                                }
                            }
                            
                        }
                        else        //不存在右括号
                        {
                            //m_pDisplayInformation.DisplayInformation("缺失右括号,请检查", false, null, "退出");
                            MessageBox.Show("缺失右括号,请检查!", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                            return false;
                        }

                        //如果能通过检查,则截取除该字段后的其它字符串
                        sTempExpression = Strings.Mid(sTempExpression, j + 1, Strings.Len(sTempExpression) - j);
                    }
                    else            //不存在左括号
                    {
                        //m_pDisplayInformation.DisplayInformation("左括号丢失,请检查", false, null, "退出");
                        MessageBox.Show("左括号丢失,请检查!", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                        return false;
                    }
                }

                //再检查剩余的字符串里边是否只包含了']括号
                if (Strings.InStr(sTempExpression,"[",CompareMethod.Text)==0)
                {
                    if (Strings.InStr(sTempExpression,"]",CompareMethod.Text)>0)
                    {
                        //m_pDisplayInformation.DisplayInformation("缺失左括号,请检查", false, null, "退出");
                        MessageBox.Show("缺失左括号,请检查!", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                        return false;
                    }
                }

                //再检查字符串中的连接符是否正确
                sTempExpression = sExpression;
                j = Strings.InStr(sTempExpression, "]", CompareMethod.Text);

                if (j!=Strings.Len(sTempExpression))
                {
                    while (Strings.InStr(sTempExpression,"[",CompareMethod.Text)>0)
                    {
                        j = Strings.InStr(sTempExpression, "]", CompareMethod.Text);
                        sTempExpression = Strings.Mid(sTempExpression, j + 1, Strings.Len(sTempExpression) - j);

                        i = Strings.InStr(sTempExpression, "[", CompareMethod.Text);

                        if (i==0)
                        {
                            return true;
                        }
                        sChar = Strings.Mid(sTempExpression, 1, i - 1);

                        if ((sChar+"").Trim()!="")
                        {
                            //不存在连接符,& , + ,and ,or ,比较符(>,<,=")则视为错误的表达式
                            if ((Strings.InStr(sChar, "&", CompareMethod.Text) == 0) && (Strings.InStr(sChar, "+", CompareMethod.Text) == 0) && (Strings.InStr(sChar, "And", CompareMethod.Text) == 0) && (Strings.InStr(sChar, "Or", CompareMethod.Text) == 0) && (Strings.InStr(sChar, "=", CompareMethod.Text) == 0) && (Strings.InStr(sChar, ">", CompareMethod.Text) == 0) && (Strings.InStr(sChar, "<", CompareMethod.Text) == 0))
                            {
                                //m_pDisplayInformation.DisplayInformation("二个字段间没有连接符&或+", false, null, "退出");
                                MessageBox.Show("二个字段间没有连接符&或+", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                                return false;
                            }
                            else if ((Strings.InStr(sChar, "&", CompareMethod.Text) == 0)&&(Strings.InStr(sChar, "+", CompareMethod.Text) == 0))
                            {
                                //如果不只是存在 & 符号的话,即还存在其它的字符,应该只有一对""和空格而已
                                if (Strings.Len(sChar)>1)
                                {
                                    k = Strings.InStr(sChar, "\"", CompareMethod.Text);

                                    if (k == 0)       //][中间的字符串是否存在二个&&,且&&中间没有任何"",如果不存在"字符
                                    {
                                        if (Strings.InStr(sChar, "&", CompareMethod.Text)!=0)
                                        {
                                            m = Strings.InStr(sChar, "&", CompareMethod.Text);
                                        }
                                        else
                                        {
                                            m = Strings.InStr(sChar, "+", CompareMethod.Text);
                                        }

                                        sChar = Strings.Mid(sChar, m + 1, Strings.Len(sChar) - m);

                                        if ((Strings.InStr(sChar, "&", CompareMethod.Text)>0)||(Strings.InStr(sChar, "+", CompareMethod.Text)>0))
                                        {
                                            //m_pDisplayInformation.DisplayInformation("二个字段中间有多余的连接符", false, null, "退出");
                                            MessageBox.Show("二个字段中间有多余的连接符", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                                            return false;
                                        }
                                    }
                                    else            //应该检查"符号是否成对,且只有二个,
                                                    //还应该检查""中间是否含有其它的字母,暂不写
                                    {
                                        sChar = Strings.Mid(sChar, k + 1, Strings.Len(sChar) - k);

                                        if (Strings.InStr(sChar, "\"", CompareMethod.Text)<0)
                                        {
                                            //m_pDisplayInformation.DisplayInformation("缺失\",请检查", false, null, "退出");
                                            MessageBox.Show("缺失\",请检查", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                                            return false;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            //m_pDisplayInformation.DisplayInformation("二个字段间没有连接符&或+", false, null, "退出");
                            MessageBox.Show("二个字段间没有连接符&或+", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                            return false;
                        }

                    }
                }
                return true;

            }
            return true;
        }

        //把某个字段附加到表达式的后边
        private void Appendix()
        {
            string sValue;
            string[] sValueArray;
            int i;

            for (i = 0; i <= lstFieldName.SelectedItems.Count-1; i++)
            {
                sValue = lstFieldName.SelectedItems[i].Text;
                sValueArray = sValue.Split(new char[] { '[' });
                sValue = sValueArray[0];

                if (txtExpression.Text.Trim()=="")
                {
                    txtExpression.Text = "[" + sValue + "]";
                }
                else
                {
                    //txtExpression.Text = txtExpression.Text + " " + "& " + "\" \"" + "&" + "[" + sValue + "]";
                    txtExpression.Text = txtExpression.Text + " " + "& " + "\" \"" + " &" + " [" + sValue + "]";
                }

            }
            btnClear.Enabled = true;
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //验证表达式
        private void btnVerify_Click(object sender, EventArgs e)
        {
            if (verifyExpression(txtExpression.Text.Trim())==true)
            {
                //m_pDisplayInformation.DisplayInformation("表达式是正确的!", false, null, "退出");
                MessageBox.Show("表达式是正确的!", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            }
        }

    }
}
