using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZJGISLayerManager;

namespace ZJGISLayerManager
{
    public partial class FrmRendererAddValue : DevComponents.DotNetBar.Office2007Form
    {
        private string[] m_Values;
        private string[] m_ReturnValues;

        public FrmRendererAddValue(string[] Values)
        {
            InitializeComponent();

            m_Values = Values;
        }
        public string[] ReturnValues
        {
            get
            {
                return m_ReturnValues;
            }
            set
            {
                m_ReturnValues = value;
            }
        }

        private void FrmRendererAddValue_Load(object sender, EventArgs e)
        {
            if (m_Values == null)
            {
                return;
            }
            this.lstValues.Items.AddRange(m_Values);
        }

        //添加值
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (this.lstValues.SelectedItems.Count == 0)
            {
                //ModDeclare.g_ErrorHandler.DisplayInformation("请至少选择一个值!", false, "确定",null);
                MessageBox.Show("请至少选择一个值!", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                return;
            }
            int i;
            for (i = 0; i <= this.lstValues.SelectedItems.Count - 1; i++)             //将所有选中的值作为数组传出
            {
                //m_ReturnValues = new string[i];
                System.Array.Resize(ref m_ReturnValues, i + 1);
                m_ReturnValues[i] = this.lstValues.SelectedItems[i].ToString();
            }
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

    }
}
