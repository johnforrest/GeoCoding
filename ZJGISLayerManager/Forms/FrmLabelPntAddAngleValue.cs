//<CSCC>
//-------------------------------------------------------------------------------------
//    部件名：FrmLabelPntAddAngleValue
//    工程名：LayerManager
//    版权: CopyRight (c) 2010
//    创建人：
//    描述：设置点状地物标注的旋转角度
//    创建日期：
//    修改人：
//    修改说明：
//    修改日期：
//-------------------------------------------------------------------------------------
//</CSCC>

//设置点层点标注旋转角度


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using ZJGISLayerManager;
using ZJGISLayerManager.Forms;

namespace ZJGISLayerManager
{
    public partial class FrmLabelPntAddAngleValue : DevComponents.DotNetBar.Office2007Form
    {
        private double[] m_dAngelValueArray;
        //private ClsErrorHandle m_pDisplayInformation = new ClsErrorHandle();

        public FrmLabelPntAddAngleValue()
        {
            InitializeComponent();
        }

        public double[] AngelValueArray
        {
            get
            {
                return m_dAngelValueArray;
            }
            set
            {
                m_dAngelValueArray = value;
            }
        }

        private void FrmLabelPntAddAngleValue_Load(object sender, EventArgs e)
        {
            int i;
            int iRowCount;

            for (i = 0; i <= m_dAngelValueArray.Length - 1; i++)
            {
                dgvList.Rows.Add();
                iRowCount = dgvList.RowCount;
                dgvList.Rows[iRowCount - 1].Cells["NO"].Value = iRowCount - 1;
                dgvList.Rows[iRowCount - 1].Cells["Angle"].Value = m_dAngelValueArray[i];
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            addRow();
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            UpRow();
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            DownRow();
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            delCurRow();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            //改变角度的数组
            if (CheckGridList() == true)
            {
                UpdateAngleArrayValue();
                this.Close();
            }
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void addRow()
        {
            int i;
            int iRowCount;
            string strAngle = "";
            FrmInputAngle pFrmInputAngle = new FrmInputAngle();

            try
            {
                iRowCount = dgvList.RowCount;

                if (pFrmInputAngle.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                {
                    return;
                }
                strAngle = pFrmInputAngle.strInput;

                if (strAngle == null)
                {
                    //ModDeclare.g_ErrorHandler.DisplayInformation("请输入新增角度", false, "确定", null);
                    MessageBox.Show("请输入新增角度", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    return;
                }

                for (i = 0; i <= iRowCount - 1; i++)
                {
                    if (strAngle == dgvList.Rows[i].Cells["Angle"].Value.ToString())
                    {
                        //if (m_pDisplayInformation.DisplayInformation("所取角度与已有角度重名,请重新输入角度", false,null,null) == true)
                        if (MessageBox.Show("请输入新增角度", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                        {
                            return;
                        }
                        else
                        {
                            return;
                        }
                    }
                }

                dgvList.Rows.Add();
                iRowCount = dgvList.RowCount;
                dgvList.Rows[iRowCount - 1].Cells["NO"].Value = iRowCount - 1;
                dgvList.Rows[iRowCount - 1].Cells["Angle"].Value = strAngle;

            }
            catch (Exception)
            {
                //ModDeclare.g_ErrorHandler.DisplayInformation("新增角度有误，请重新输入", false, "确定",null);
                MessageBox.Show("新增角度有误，请重新输入", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                return;
            }
        }

        private void UpRow()
        {
            int iCurRow;
            string sValue;
            iCurRow = (int)dgvList.CurrentRow.Cells[0].Value;

            if (iCurRow > 0)
            {
                sValue = dgvList.Rows[iCurRow - 1].Cells["Angle"].Value.ToString();
                dgvList.Rows[iCurRow - 1].Cells["Angle"].Value = dgvList.Rows[iCurRow].Cells["Angle"].Value;
                dgvList.Rows[iCurRow].Cells["Angle"].Value = sValue;
            }
        }

        private void DownRow()
        {
            int iCurRow;
            string sValue;
            iCurRow = (int)dgvList.CurrentRow.Cells[0].Value;

            if (iCurRow < dgvList.RowCount - 1)
            {
                sValue = dgvList.Rows[iCurRow].Cells["Angle"].Value.ToString();
                dgvList.Rows[iCurRow].Cells["Angle"].Value = dgvList.Rows[iCurRow + 1].Cells["Angle"].Value;
                dgvList.Rows[iCurRow + 1].Cells["Angle"].Value = sValue;
            }
        }

        private void delCurRow()
        {
            int iCurRow;
            int i;

            iCurRow = (int)dgvList.CurrentRow.Cells[0].Value;

            //if (ModDeclare.g_ErrorHandler.DisplayInformation("确定要删除？", true, "确定", "取消") == true)
            if (MessageBox.Show("确定要删除？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {
                dgvList.Rows.Remove(dgvList.CurrentRow);
                for (i = iCurRow; i <= dgvList.RowCount - 1; i++)
                {
                    dgvList.Rows[i].Cells["NO"].Value = (int)dgvList.Rows[i].Cells["NO"].Value - 1;
                }
            }
        }

        //**************************************************************************************
        //** 函数名称 :  UpdateAngleArrayValue
        //** 参    数 :
        //** 功能描述 :根据igridlist里边的值,获得新的角度数组
        //**
        //** 设 计 人 :  yangxubin
        //** 日    期 :  2005-12-5
        //** 修 改 人 :
        //** 版    本 :  1.0
        //**************************************************************************************
        //</CSCM>
        private void UpdateAngleArrayValue()
        {
            int i;
            int iRowCount;
            iRowCount = dgvList.RowCount;

            if (iRowCount == 0)
            {
                m_dAngelValueArray = new double[iRowCount];
            }
            else
            {
                m_dAngelValueArray = new double[iRowCount - 1];
            }

            if (iRowCount == 0)
            {
                m_dAngelValueArray[0] = 0;
            }
            else
            {
                for (i = 0; i <= iRowCount - 1; i++)
                {
                    m_dAngelValueArray[i] = (double)dgvList.Rows[i].Cells["Angle"].Value;
                }
            }

        }

        private bool CheckGridList()
        {
            int i;
            string sValue;

            for (i = 0; i <= dgvList.RowCount - 1; i++)
            {
                sValue = (dgvList.Rows[i].Cells["Angle"].Value + "").Trim();

                if (sValue == "")
                {
                    //m_pDisplayInformation.DisplayInformation("第" + i + "行的角度为空,请设置!", false, null, "退出");
                    MessageBox.Show("第" + i + "行的角度为空,请设置!", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    return false;
                }
                else
                {
                    if ((Conversion.Val(sValue) > 360) || (Conversion.Val(sValue) < 0))
                    {
                        //m_pDisplayInformation.DisplayInformation("第" + i + "行的角度值大于360度或小于0度,请重新设置", false, null, "退出");
                        MessageBox.Show("第" + i + "行的角度值大于360度或小于0度,请重新设置", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                        return false;
                    }
                }
            }
            return true;
        }

    }
}
