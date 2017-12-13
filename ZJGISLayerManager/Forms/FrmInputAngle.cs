using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ZJGISLayerManager.Forms
{
    public partial class FrmInputAngle : DevComponents.DotNetBar.Office2007Form
    {
        private string m_strInput;
        //Label标题
        private string m_strLblCaption;
        //是否限制只输入数字
        private bool m_bOnlyNumber;

        public string strInput
        {
            get
            {
                return m_strInput;
            }
        }

        public FrmInputAngle()
        {
            InitializeComponent();
        }

        private void FrmInputAngle_Load(object sender, EventArgs e)
        {
            //Label默认值
            if (string.IsNullOrEmpty(m_strLblCaption))
            {
                m_strLblCaption = "请输入信息：";
            }

            lblTitle.Text = m_strLblCaption;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            m_strInput = "";
            this.DialogResult = DialogResult.Cancel;
        }

        private void txtInputText_TextChanged(object sender, EventArgs e)
        {
            DevComponents.DotNetBar.Controls.TextBoxX vTxtbox = sender as DevComponents.DotNetBar.Controls.TextBoxX;
            if (m_bOnlyNumber == true)
            {
                vTxtbox.Text = Convert.ToDouble(vTxtbox.Text.Replace("&", ""))
                    + (vTxtbox.Text.Substring(vTxtbox.Text.Length - 1, 1) == "."
                    && vTxtbox.Text.IndexOf(".") == vTxtbox.Text.Length - 1 ? "." : "");
                vTxtbox.SelectionStart = vTxtbox.MaxLength;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            m_strInput = this.txtInputText.Text;
            this.DialogResult = DialogResult.OK;
        }
    }
}
