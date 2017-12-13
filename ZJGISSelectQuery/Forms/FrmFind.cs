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
using ESRI.ArcGIS.esriSystem;
using Microsoft.VisualBasic;
using ESRI.ArcGIS.Controls;
//using OWC10;

namespace SelectQuery 
{
    public partial class FrmFind : DevComponents.DotNetBar.Office2007Form
    {
        private IMap m_pMap;
        string m_sExtentFind;
        string m_sMatchFind;
        bool m_bStop;
        IEnumLayer m_pEnumLayer;
        //int m_iLayerCount;
        //string m_sFind;
        public  IMap FocusMap
        {
            set
            {
                m_pMap = value;
            }
        }
        public FrmFind()
        {
            InitializeComponent();
        }

        private void bar1_ItemClick(object sender, EventArgs e)
        {

        }

        private void panelEx1_Click(object sender, EventArgs e)
        {

        }

        private void labelX3_Click(object sender, EventArgs e)
        {

        }

        private void buttonItem2_Click(object sender, EventArgs e)
        {

        }

        private void frmFind_Load(object sender, EventArgs e)
        {
            txtStrFind.Text = "";
            cboExtentFind.Items.Clear();
            cboExtentFind.Items.Add("当前地图全部图层");
            cboExtentFind.Items.Add("当前地图可见图层");
            cboExtentFind.Items.Add("当前地图可选图层");
            cboExtentFind.Text = "当前地图全部图层";

            cboMatchFind.Items.Clear();
            cboMatchFind.Items.Add("模糊匹配");
            cboMatchFind.Items.Add("精确匹配");
            cboMatchFind.Items.Add("= 查询值");
            cboMatchFind.Items.Add(">= 查询值");
            cboMatchFind.Items.Add("> 查询值");
            cboMatchFind.Items.Add("<= 查询值");
            cboMatchFind.Items.Add("< 查询值");
            cboMatchFind.Items.Add("<> 查询值");
            cboMatchFind.Text = "模糊匹配";

            dgvResFind.ColumnHeadersDefaultCellStyle.BackColor=System.Drawing.Color.Navy;
            dgvResFind.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.White;
            dgvResFind.ColumnHeadersDefaultCellStyle.Font=new System.Drawing.Font(dgvResFind.Font,System.Drawing.FontStyle.Bold);
            dgvResFind.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            dgvResFind.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Sunken;
            dgvResFind.GridColor =  System.Drawing.Color.Black;
            dgvResFind.RowHeadersVisible = false;
            dgvResFind.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvResFind.MultiSelect = false;
            dgvResFind.AllowUserToAddRows = false;
            dgvResFind.ReadOnly = true;
            dgvResFind.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvResFind.ColumnCount = 3;
            dgvResFind.Columns[0].Name = "FieldValue";
            dgvResFind.Columns[0].HeaderText = "字段值";
            dgvResFind.Columns[0].Width = dgvResFind.Width / 3;
            dgvResFind.Columns[1].Name = "FieldName";
            dgvResFind.Columns[1].HeaderText = "字段名称";
            dgvResFind.Columns[1].Width = dgvResFind.Width / 3;
            dgvResFind.Columns[2].Name = "LayerName";
            dgvResFind.Columns[2].HeaderText = "所在的图层";
            dgvResFind.Columns[2].Width = dgvResFind.Width / 3;

            Progress.Visible = false;
            Progress.MarqueeAnimationSpeed = 10;
            


        }

        private void cboExtentFind_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_sExtentFind = cboExtentFind.Text;
        }

        private void cboMatchFind_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_sMatchFind = cboMatchFind.Text;
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            IFeatureLayer pFeatureLayer;
            IFeatureClass pFeatureClass;
            IFeature pFeature;
            IDataset pDataset;
            IFields pFields;
            IField pField;
            IFeatureCursor pFeatureCursor;
            IQueryFilter pQueryFilter;
            ISQLSyntax pSQLSyntax;
            string sFind;
            System.Text.StringBuilder sbWhereClause;
            //string sWhereClause;
            string sFieldName;
            string sWildcardManyMatch;
            string strWC;
            Dictionary<int,string> dicFields;
            UID pUID;
            object objFeatureValue;
            int iFeatureCount=0, iResCount=0,j=0;
            bool bAddRow=false;
            int iResLimitCount;
            try
            {
                sFind = txtStrFind.Text.Trim();
                if (m_sMatchFind == "= 查询值" || m_sMatchFind == ">= 查询值" || m_sMatchFind == "> 查询值" || m_sMatchFind == "<= 查询值" || m_sMatchFind == "< 查询值")
                {
                    try
                    {
                        Convert.ToDouble(sFind);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("输入不合法：匹配方式为"+ m_sMatchFind + "时，必须输入数字型数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //ClsDeclare.g_ErrorHandler.DisplayInformation("输入不合法：匹配方式为" + m_sMatchFind + "时，必须输入数字型数据！",false,"确定",null);
                        return;
                    }
                }
                if (sFind.IndexOf("*") >= 0 || sFind.IndexOf("?") >= 0 || sFind.IndexOf("_") >= 0 || sFind.IndexOf("%") >= 0)
                {
                    MessageBox.Show("查询值中含有通配符，模糊查询暂不支持通配符查询！" + "\r" + "\n" + "请重新输入", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //ClsDeclare.g_ErrorHandler.DisplayInformation("查询值中含有通配符，模糊查询暂不支持通配符查询！" + "\r" + "\n" + "请重新输入",false,null,null);
                    return;
                }
                pUID = new UIDClass();
                pUID.Value = "{E156D7E5-22AF-11D3-9F99-00C04F6BC78E}";
                m_pEnumLayer = m_pMap.get_Layers(pUID, true);
                if (m_pEnumLayer == null)
                    return;

                Progress.Visible = true;
                Progress.ProgressType = DevComponents.DotNetBar.eProgressItemType.Marquee;
                lblPrompt.Text = "正在查询，请稍候...";
                dgvResFind.Rows.Clear();

                dicFields = new Dictionary<int,string>();
                pQueryFilter = new QueryFilterClass();
                iResLimitCount = 10000;
                m_bStop = false;
                m_pEnumLayer.Reset();
                pFeatureLayer = m_pEnumLayer.Next() as IFeatureLayer;
                while (pFeatureLayer != null)
                {
                    if (m_bStop)
                    {
                        Progress.Visible = false;
                        //MessageBox.Show("查询被中止...", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //ClsDeclare.g_ErrorHandler.DisplayInformation("查询被中止...", false,null,null);
                        return;
                    }
                    switch (m_sExtentFind)
                    {
                        case "当前地图全部图层":
                        case "当前地图可见图层":
                            if (pFeatureLayer.Visible == false)
                            {
                                pFeatureLayer = m_pEnumLayer.Next() as IFeatureLayer;
                                continue;
                                
                            }
                            break;
                        case "当前地图可选图层":
                            if (pFeatureLayer.Selectable == false)
                            {
                                pFeatureLayer = m_pEnumLayer.Next() as IFeatureLayer;
                                continue;
                            }
                            break;
                    }
                    pFeatureClass = pFeatureLayer.FeatureClass;
                    pDataset = (IDataset)pFeatureClass;
                    pSQLSyntax = (ISQLSyntax)pDataset.Workspace;
                    sWildcardManyMatch = pSQLSyntax.GetSpecialCharacter(esriSQLSpecialCharacters.esriSQL_WildcardManyMatch);
                    //获得搜索条件sWhereClause
                    sbWhereClause = new System.Text.StringBuilder();
                    pFields = pFeatureClass.Fields;
                    for (int i = 0; i < pFields.FieldCount; i++)
                    {
                        pField = pFields.get_Field(i);
                        if (pField.Editable == true && pField.Type != esriFieldType.esriFieldTypeGeometry)
                        {
                            switch (m_sMatchFind)
                            {

                                case "= 查询值":
                                case ">= 查询值":
                                case "> 查询值":
                                case "<= 查询值":
                                case "< 查询值":
                                    {
                                        if (pField.Type != esriFieldType.esriFieldTypeDouble && pField.Type != esriFieldType.esriFieldTypeInteger && pField.Type != esriFieldType.esriFieldTypeSingle && pField.Type != esriFieldType.esriFieldTypeSmallInteger)
                                            continue;
                                        if (!dicFields.ContainsKey(i))
                                            dicFields.Add(i, pField.Name);
                                        sbWhereClause = sbWhereClause.Append(" OR " + pField.Name + " " + m_sMatchFind.Substring(0, 2).Trim() + " " + sFind);
                                        break;
                                    }
                                case "模糊匹配":
                                    {
                                        if (pField.Type == esriFieldType.esriFieldTypeDouble || pField.Type == esriFieldType.esriFieldTypeInteger || pField.Type == esriFieldType.esriFieldTypeSingle || pField.Type == esriFieldType.esriFieldTypeSmallInteger)
                                            continue;
                                        if (!dicFields.ContainsKey(i))
                                            dicFields.Add(i, pField.Name);
                                        sbWhereClause = sbWhereClause.Append(" OR " + pField.Name + " LIKE " + "'" + sWildcardManyMatch + sFind + sWildcardManyMatch + "'");
                                        break;
                                    }
                                case "精确匹配":
                                    {
                                        if (pField.Type == esriFieldType.esriFieldTypeDouble || pField.Type == esriFieldType.esriFieldTypeInteger || pField.Type == esriFieldType.esriFieldTypeSingle || pField.Type == esriFieldType.esriFieldTypeSmallInteger)
                                            continue;
                                        if (!dicFields.ContainsKey(i))
                                            dicFields.Add(i, pField.Name);
                                        sbWhereClause = sbWhereClause.Append(" OR " + pField.Name + " LIKE " + "'" + sFind + "'");
                                        break;
                                    }
                            }
                        }

                    }
                    if (sbWhereClause.ToString() == "")
                    {
                        pFeatureLayer = m_pEnumLayer.Next() as IFeatureLayer;
                        continue;
                    }
                    else   
                    {
                        int sl;
                        sl=sbWhereClause.ToString().Length-4;
                        strWC = sbWhereClause.ToString().Substring(4, sl);
                    }
                    pQueryFilter.WhereClause = strWC;
                    pFeatureCursor = pFeatureLayer.Search(pQueryFilter, false);

                    sFieldName = "";
                    if (pFeatureCursor == null)
                        return;
                    pFeature = pFeatureCursor.NextFeature();

                    while (pFeature != null)
                    {
                        iFeatureCount += 1;
                        for (int i = 0; i < pFields.FieldCount; i++)
                        {
                            if (m_bStop)
                            {
                                Progress.Visible = false;
                                //MessageBox.Show("查询被中止...", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                //ClsDeclare.g_ErrorHandler.DisplayInformation("查询被中止...", false,null,null);
                                return;
                            }
                            if (dicFields.TryGetValue(i,out sFieldName))
                            {
                                objFeatureValue = pFeature.get_Value(i);
                                if (objFeatureValue is DBNull)
                                    continue;
                                switch (m_sMatchFind)
                                {
                                    case "= 查询值": 
                                        {
                                            if (Convert.ToDouble(objFeatureValue) == (Convert.ToDouble(sFind)))
                                                bAddRow = true;
                                         
                                        }
                                        break;
                                    case ">= 查询值":
                                        {
                                            if (Convert.ToDouble(objFeatureValue) >= (Convert.ToDouble(sFind)))
                                                bAddRow = true;
                                          
                                        }
                                        break;
                                    case "<= 查询值":
                                        {
                                            if (Convert.ToDouble(objFeatureValue) <= (Convert.ToDouble(sFind)))
                                                bAddRow = true;
                                           

                                        }
                                        break;
                                    case "> 查询值":
                                        {
                                            if (Convert.ToDouble(objFeatureValue) > (Convert.ToDouble(sFind)))
                                                bAddRow = true;
                                            

                                        }
                                        break;
                                    case "< 查询值":
                                        {
                                            if (Convert.ToDouble(objFeatureValue) > (Convert.ToDouble(sFind)))
                                                bAddRow = true;
                                            
                                        }
                                        break;
                                    case "模糊匹配":
                                        {
                                            if (pFields.get_Field(i).Type == esriFieldType.esriFieldTypeDate)
                                                objFeatureValue = Strings.Format(objFeatureValue,"{0:yyyy-M-d}");
                                            if (objFeatureValue.ToString().Contains(sFind))
                                                bAddRow = true;
                                            
                                        }
                                        break;
                                    case "精确匹配":
                                        {

                                            if (pFields.get_Field(i).Type == esriFieldType.esriFieldTypeDate)
                                                objFeatureValue = Strings.Format(objFeatureValue,"{0:yyyy-M-d}");
                                            if (objFeatureValue.ToString() == sFind)
                                                bAddRow = true;
                                            


                                        }
                                        break;
                                }
                                if (bAddRow)
                                {
                                    iResCount += 1;
                                    j = dgvResFind.Rows.Add();
                                    dgvResFind.Rows[j].Tag = pFeature.OID + "_" + pFeatureLayer.Name;
                                    dgvResFind["FieldValue", j].Value = objFeatureValue;
                                    dgvResFind["FieldName", j].Value = sFieldName;
                                    dgvResFind["LayerName", j].Value = pFeatureLayer.Name;
                                    lblPrompt.Text = "已找到" + iResCount + "个记录，对应" + iFeatureCount + "个要素";
                                    bAddRow = false;


                                }
                                if (iResCount % 30 == 0)
                                    Application.DoEvents();
                                if (iResCount >= iResLimitCount)
                                {
                                    Progress.Visible = false;
                                    MessageBox.Show("查询结果超出最大限制数量，将中止查询！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    //ClsDeclare.g_ErrorHandler.DisplayInformation("查询结果超出最大限制数量，将中止查询！", false,null,null);
                                    return;
                                }
                            }
                        }
                        pFeature = pFeatureCursor.NextFeature();
                    }
                    dicFields.Clear();
                    pFeatureLayer = m_pEnumLayer.Next() as IFeatureLayer;

                }
                Progress.Visible = false;
                //m_bStop = true;
            }
            catch (Exception)
            {
                Progress.Visible = false;

                //ClsDeclare.g_ErrorHandler.HandleError(true, null, 0, null, ex.StackTrace);
                return;
            }
            finally
            {
                Progress.Visible = false;
                lblPrompt.Text = "共找到" + iResCount + "个记录，对应" + iFeatureCount + "个要素";
            }
            
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            m_bStop = true;
            MessageBox.Show("查询被中止...", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //lblPrompt.Text = "正在中止查询...";
        }

        private void dgvResFind_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int iOID, iRowIndex, iPosition;
            string sRowTag, sLayerName;
            iRowIndex = dgvResFind.HitTest(e.X,e.Y).RowIndex;
            sRowTag = dgvResFind.Rows[iRowIndex].Tag.ToString();
            iPosition = Strings.InStr(sRowTag, "_", CompareMethod.Text);
            iOID =Convert.ToInt32(sRowTag.Substring(0, iPosition - 1));
            sLayerName = sRowTag.Substring(iPosition);

            IFeature pFeature;
            IFeatureLayer pFeatureLayer;
            IFeatureClass pFeatureClass;
            pFeature = null;
            m_pEnumLayer.Reset();
            pFeatureLayer = (IFeatureLayer)m_pEnumLayer.Next();
            while (pFeatureLayer != null)
            {
                if (pFeatureLayer.Name == sLayerName)
                {
                    pFeatureClass = pFeatureLayer.FeatureClass;
                    pFeature = pFeatureClass.GetFeature(iOID);
                    break;
                }
                pFeatureLayer = m_pEnumLayer.Next() as IFeatureLayer;
            }
            if (pFeature == null)
                return;
            IHookActions pHookActions;
            IHookHelper pHookHelper;
            pHookHelper = new HookHelperClass();
            pHookHelper.Hook = ClsDeclare.g_Sys.MapControl.Object;
            pHookActions = (IHookActions)pHookHelper;

            if(pHookActions.get_ActionSupported(pFeature.ShapeCopy, esriHookActions.esriHookActionsPan))
                pHookActions.DoAction(pFeature.ShapeCopy, esriHookActions.esriHookActionsPan);

             Application.DoEvents();
             if (pHookActions.get_ActionSupported(pFeature.ShapeCopy, esriHookActions.esriHookActionsFlash))
                 pHookActions.DoAction(pFeature.ShapeCopy, esriHookActions.esriHookActionsFlash);


        }

        private void txtStrFind_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnFind_Click(null,null);
        }


        internal static void Show(object p)
        {
            throw new NotImplementedException();
        }
    }
}
