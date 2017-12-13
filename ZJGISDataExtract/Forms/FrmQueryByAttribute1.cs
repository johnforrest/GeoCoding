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
using DevComponents.DotNetBar;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Controls;
using ZJGISSelectQuery;
using ZJGISCommon;
using SelectQuery;
namespace ZJGISDataExtract 
{
    public partial class FrmQueryByAttribute1 :DevComponents.DotNetBar.Office2007Form
    {
        IFeatureLayer m_pCurrentLayer;
        IFeatureClass m_pCurrentFeatCls;
        bool m_blnSelect;
        Collection m_FeatClsCln;
        Collection m_pQueryResultCln;
        ClsMapLayer m_pQueryResult;
      
        public Map m_MapEvent;
        Dictionary<string, object> m_dict = new Dictionary<string, object>();

        string m_PreFocus="";
        string m_AfterFocus="";
        int m_Focus=0;
        public Collection FeatClsCln 
        {
            set
            {
                m_FeatClsCln = value;
            }
        }
        public bool blnSelect
        {
            set
            {
                m_blnSelect = value;
            }
        }

        private static  string layerName;
        public string LayerName
        {
            set {  layerName=value; }
            get { return layerName; }
        }
        private static int layerNum;
        public int LayerNum
        {
            set { layerNum = value; }
            get { return layerNum; }
        }
        private string strExpress;
        public string StrExpress
        {
            set { strExpress = value; }
            get { return strExpress; }
        }
        private string strFromName;
        public string StrFromName
        {
            set { strFromName = value; }
            get { return strFromName; }
        }
         

        public FrmQueryByAttribute1()
        {
            InitializeComponent();

        }

        private void frmQueryByAttribute1_Load(object sender, EventArgs e)
        {
            m_MapEvent = ClsDeclare.g_pMap as Map;
            m_MapEvent.ItemAdded += new IActiveViewEvents_ItemAddedEventHandler(m_MapEvent_ItemAdded);
            m_MapEvent.ItemDeleted += new IActiveViewEvents_ItemDeletedEventHandler(m_MapEvent_ItemDeleted);
            
            if (ClsDeclare.g_pMap.LayerCount == 0)
                return;
            //InitCtlComboSelectMethod();
            //InitCtlComboLayer();
            if (txtLayerName.Text == "")
            {
                txtLayerName.Text = layerName;
                ListField();
            }

            m_pQueryResultCln = new Collection();
            this.txtExpress.Text = "";
        }
        //private void InitCtlComboSelectMethod()
        //{
        //    cboSelectMethod.Items.Add("创建一个新的选择结果");
        //    cboSelectMethod.Items.Add("添加到当前选择集中");
        //    cboSelectMethod.Items.Add("从当前选择结果中移除");
        //    cboSelectMethod.Items.Add("从当前选择结果中选择");
        //    cboSelectMethod.Text = cboSelectMethod.Items[0].ToString();

        
        //}
        //private void InitCtlComboLayer()
        //{
        //    try
        //    {
        //        int i;
        //        ILayer pLayer;
        //        IFeatureLayer pFeatlayer;
        //        IFeatureClass pFeatCls;
        //        Collection pLyrCol;
        //        Collection LyrCol=null;
        //        if (m_FeatClsCln == null)
        //        {
        //            pLyrCol = ClsSelectQuery.FunGetFeaLyrCol(ClsDeclare.g_pMap, null,LyrCol);
        //            for (i = 1; i <= pLyrCol.Count; i++)
        //            {
        //                pLayer = pLyrCol[i] as ILayer;
        //                if (pLayer is IFeatureLayer)
        //                {
        //                    IFeatureLayer pFeatureLayer;
        //                    pFeatureLayer = pLayer as IFeatureLayer;
        //                    if (pFeatureLayer.Selectable)
        //                        cboLayerList.Items.Add(pLayer.Name);
        //                }
        //            }

        //        }
        //        else
        //        {
        //            for (i = 1; i <= m_FeatClsCln.Count; i++)
        //            {
        //                pLayer = m_FeatClsCln[i] as ILayer;
        //                if (pLayer is IFeatureLayer)
        //                {
        //                    pFeatlayer = pLayer as IFeatureLayer;
        //                    pFeatCls = pFeatlayer.FeatureClass;
        //                    cboLayerList.Items.Add(pFeatCls.AliasName);
        //                }

        //            }

        //        }

        //        if(cboLayerList.Items.Count>0)
        //            cboLayerList.Text=cboLayerList.Items[0].ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBoxEx.Show("错误", ex.StackTrace, MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        throw;
        //    }
        
        //}
        private void ListField()
        {
            try
            {
                ILayer pLayer;
                IFeatureLayer pFeatlayer;
                IFeatureClass pFeatCls;
                int i;

                if (m_FeatClsCln == null)
                {
                    m_pCurrentLayer = ClsSelectQuery.FunFindFeatureLayerByName(layerName, ClsDeclare.g_pMap);
                    m_pCurrentFeatCls = m_pCurrentLayer.FeatureClass;

                }
                else
                {
                    for (i = 1; i <= m_FeatClsCln.Count;i++ )
                    {
                        pLayer = m_FeatClsCln[i] as ILayer;
                        if (pLayer is IFeatureLayer)
                        {
                            pFeatlayer = pLayer as IFeatureLayer;
                            pFeatCls = pFeatlayer.FeatureClass;
                            if (pFeatCls.AliasName == layerName)
                            {
                                m_pCurrentFeatCls = pFeatCls;
                                break;
                            }
                        }

                    }
                }
                lstField.Items.Clear();
                for (i = 0; i < m_pCurrentFeatCls.Fields.FieldCount; i++)
                {
                    if (Convert.ToUInt32(m_pCurrentFeatCls.Fields.get_Field(i).Type) < 5 && m_pCurrentFeatCls.Fields.get_Field(i).Editable == true)
                        lstField.Items.Add(m_pCurrentFeatCls.Fields.get_Field(i).AliasName + "【" + m_pCurrentFeatCls.Fields.get_Field(i).Name + "】");

                }
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show("错误", ex.StackTrace, MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        
        }

        //private void cboLayerList_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (cboLayerList.Text != "")
        //        ListField();
        //}

        private void lstField_DoubleClick(object sender, EventArgs e)
        {
            string strAll;

            try
            {
                strAll = this.txtExpress.Text;
                m_PreFocus = strAll.Substring(0,this.txtExpress.SelectionStart);
                m_AfterFocus = strAll.Substring(this.txtExpress.SelectionStart);

                m_PreFocus = m_PreFocus + SubstrField(this.lstField.Text);

                m_Focus = m_PreFocus.Length;
                SetRichTextBox();


            }
            catch (Exception ex)
            {
                MessageBoxEx.Show("错误", ex.StackTrace, MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }

        }
        private string SubstrField(string strFieldText)
        {
            int startPos;
            startPos = strFieldText.IndexOf("【");
            if (startPos < 0)
                return "";
            else
                return strFieldText.Substring(startPos+1,strFieldText.Length-startPos-2);
        }
        private void SetRichTextBox()
        {
            string strText;
            try
            {
                strText = m_PreFocus + m_AfterFocus;
                this.txtExpress.Text = strText;
                this.txtExpress.SelectionStart = m_Focus;
                this.txtExpress.Focus();
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show("错误", ex.StackTrace, MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        
        
        }

        private void lstValue_DoubleClick(object sender, EventArgs e)
        {
            string strAll;

            strAll = this.txtExpress.Text;
            m_PreFocus = strAll.Substring(0,this.txtExpress.SelectionStart);
            m_AfterFocus = strAll.Substring(this.txtExpress.SelectionStart);

            m_PreFocus = m_PreFocus + this.lstValue.Text;
            m_Focus = m_PreFocus.Length;
            SetRichTextBox();
        }

        private void lstField_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.lstValue.Items.Clear();

        }
        private void ShowExpression(string sOperator)
        {

            try
            {
                string sFieldName = lstField.GetItemText(lstField.SelectedItem);
                string sFieldValue = lstValue.GetItemText(lstValue.SelectedItem);
                string strExpression;
                strExpression = sFieldName + " " + sOperator + " " + sFieldValue;

                txtExpress.Text = strExpression;

            }
            catch (Exception ex)
            {
                MessageBoxEx.Show("错误", ex.StackTrace, MessageBoxButtons.OK, MessageBoxIcon.Error);
                //ClsDeclare.g_ErrorHandler.HandleError(true,null,0,null,ex.StackTrace);
                throw;
            }
        }

        private void btnOperate1_Click(object sender, EventArgs e)
        {
            ButtonX btnOperator;
            btnOperator = sender as ButtonX;
            String strAll;

            strAll = this.txtExpress.Text;
            m_PreFocus = strAll.Substring(0,this.txtExpress.SelectionStart);
            m_AfterFocus = strAll.Substring(this.txtExpress.SelectionStart);
            m_Focus = this.txtExpress.SelectionStart;

            if (btnOperator.Text != "()" && btnOperator.Text != "_" && btnOperator.Text != "%")
            {
                m_PreFocus = m_PreFocus + " " + btnOperator.Text + " ";
                m_Focus = m_PreFocus.Length;
            }
            else if (btnOperator.Text == "()")
            {
                m_PreFocus = m_PreFocus + " (";
                m_AfterFocus = ") " + m_AfterFocus;
                m_Focus = m_Focus + 2;
            }
            else if (btnOperator.Text == "_" || btnOperator.Text == "%")
            {
                m_PreFocus = m_PreFocus + btnOperator.Text;
                m_Focus = m_PreFocus.Length;
            }
            SetRichTextBox();
        }
        private void Verify()
        {
            IQueryFilter pQueryFilter;
            IFeatureClass pFeatCls;
            IFeatureCursor pFeatCursor;
            IFeature pFeat;
            IFeatureLayer pFeatLayer;


            if (this.txtExpress.Text == "")
            {
                //ClsDeclare.g_ErrorHandler.DisplayInformation("请输入表达式",false,"确定",null);
                return;
            }
            pFeatLayer = ClsSelectQuery.FunFindFeatureLayerByName(layerName,ClsDeclare.g_pMap);
            if (pFeatLayer == null)
                return;
            try
            {
                pFeatCls = pFeatLayer.FeatureClass;
                pQueryFilter = new QueryFilterClass();
                pQueryFilter.SubFields = "*";
                pQueryFilter.WhereClause = this.txtExpress.Text.Trim();
                pFeatCls.Search(pQueryFilter, false);
                pFeatCursor = pFeatCls.Search(pQueryFilter, false);
                pFeat = pFeatCursor.NextFeature();

                if (pFeat == null)
                {
                    //ClsDeclare.g_ErrorHandler.DisplayInformation("此表达式搜索不到要素,请检查表达式！", false, null, null);
                }
            }
            catch (System.Exception ex)
            {
                MessageBoxEx.Show("错误", ex.StackTrace, MessageBoxButtons.OK, MessageBoxIcon.Error);
                //ClsDeclare.g_ErrorHandler.HandleError(true, null, 0, null, ex.StackTrace);
                throw;
            }                                
            finally
            {
                pQueryFilter = null;
                pFeatCls = null;
                pFeatCursor = null;
                pFeat = null;
            }
        
        }

        private void btnApply_ClientSizeChanged(object sender, EventArgs e)
        {

        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            try
            {
                Verify();
                string strQuery;
                strQuery = this.txtExpress.Text;
                if (strQuery == "")
                    return;
                ILayer pLayer=null ;
                if (layerName!=""||layerName !=null)
                    pLayer = ClsSelectQuery.FunFindFeatureLayerByName(layerName, ClsDeclare.g_pMap);                
                if (pLayer == null)
                    return;
                IFeatureLayer pFeatLayer;
                pFeatLayer = pLayer as IFeatureLayer;

                if (pFeatLayer.Visible == false)
                { 
                }
                    //ClsDeclare.g_ErrorHandler.DisplayInformation("请选择可见图层",false,"确定",null);
                IFeatureSelection pFeatureSelection;
                pFeatureSelection = pFeatLayer as IFeatureSelection;

                IQueryFilter pQueryFilter = new QueryFilterClass();
                pQueryFilter.WhereClause = strQuery;

                esriSelectionResultEnum SelType;
                SelType = esriSelectionResultEnum.esriSelectionResultNew;

                //switch (this.cboSelectMethod.Text)
                //{
                //    case "创建一个新的选择结果":
                //        { 
                //        if(ClsDeclare.g_pMap.SelectionCount>0)
                //            ClsDeclare.g_pMap.ClearSelection();
                //        SelType = esriSelectionResultEnum.esriSelectionResultNew;
                                
                //        }
                //        break;
                //    case "添加到当前选择集中":
                //        SelType = esriSelectionResultEnum.esriSelectionResultAdd;
                //        break;
                //    case "从当前选择结果中移除":
                //        SelType = esriSelectionResultEnum.esriSelectionResultSubtract;
                //        break;
                //    case "从当前选择结果中选择":
                //        SelType = esriSelectionResultEnum.esriSelectionResultAnd;
                //        break;
                //}
                if (pFeatLayer.Selectable)
                    pFeatureSelection.SelectFeatures(pQueryFilter,SelType,false);

                IActiveView pActiveView;
                pActiveView = ClsDeclare.g_pMap as IActiveView;
                pActiveView.Refresh();

                IFeatureClass pFeatCls;
                IGeometryCollection pGeometryCol;
                IGeometryBag pGeometryBag;
                IEnumIDs pEnumIDs;
                IFeature pFeature;
                int iOBJID;

                pGeometryCol = new GeometryBagClass();
                pGeometryBag = pGeometryCol as IGeometryBag;
                pFeatCls = pFeatLayer.FeatureClass;
                pEnumIDs = pFeatureSelection.SelectionSet.IDs;
                iOBJID = pEnumIDs.Next();
                object Missing=Type.Missing;
                object Missing1=Type.Missing;
                while (iOBJID != -1)
                {
                    pFeature = pFeatCls.GetFeature(iOBJID);
                    pGeometryCol.AddGeometry(pFeature.Shape,ref Missing,ref Missing1);
                    iOBJID = pEnumIDs.Next();
                }

                IHookActions pHookActions;
                IHookHelper pHookHelper;
                pHookHelper = new HookHelperClass();
                pHookHelper.Hook = ClsDeclare.g_Sys.MapControl.Object;
                pHookActions = pHookHelper as IHookActions;
                if (pHookActions.get_ActionSupported(pGeometryBag.Envelope, esriHookActions.esriHookActionsPan))
                    pHookActions.DoAction(pGeometryBag.Envelope, esriHookActions.esriHookActionsPan);
                Application.DoEvents();
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show("错误", ex.StackTrace, MessageBoxButtons.OK, MessageBoxIcon.Error);
                //ClsDeclare.g_ErrorHandler.HandleError(true,null,0,null,ex.StackTrace);
                throw;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtExpress.Text = "";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)//传入参数
        {
            if (strFromName == "FrmPolygonExtract")
                 FrmPolygonExtract.strArryExpress[layerNum - 1] = this.txtExpress.Text.Trim();
           else  if (strFromName == "FrmRegionExtract")
                FrmRegionExtract.strArryExpress[layerNum - 1] = this.txtExpress.Text.Trim();
             this.Close();
        }
        private void m_MapEvent_ItemAdded(object Item)
        {
          //  InitCtlComboLayer();
        
        }
        private void m_MapEvent_ItemDeleted(object Item)
        {
          //  InitCtlComboLayer();
        }

        private void btnVerify_Click(object sender, EventArgs e)
        {
            IQueryFilter pQueryFilter;
            IFeatureClass pFeatCls;
            IFeatureCursor pFeatCursor;
            IFeature pFeat;
            IFeatureLayer pFeatLayer;

            if (this.txtExpress.Text == "")
            {
                //ClsDeclare.g_ErrorHandler.DisplayInformation("请输入表达式", false, "确定",null);
                return;

            }
            pFeatLayer = ClsSelectQuery.FunFindFeatureLayerByName(layerName,ClsDeclare.g_pMap);
            if (pFeatLayer == null)
                return;
            try
            {
                pFeatCls = pFeatLayer.FeatureClass;
                pQueryFilter = new QueryFilterClass();
                pQueryFilter.SubFields = "*";
                pQueryFilter.WhereClause = this.txtExpress.Text.Trim();
                pFeatCls.Search(pQueryFilter, false);
                pFeatCursor = pFeatCls.Search(pQueryFilter, false);
                pFeat = pFeatCursor.NextFeature();

                //if (pFeat != null)
                    //ClsDeclare.g_ErrorHandler.DisplayInformation("表达式正确！", false, null, null);
                //else
                    //ClsDeclare.g_ErrorHandler.DisplayInformation("此表达式搜索不到要素,请检查表达式！", false, null, null);


            }
            catch (Exception)
            {
                //ClsDeclare.g_ErrorHandler.DisplayInformation("查询表达式不合法！", false, null, null);
                MessageBoxEx.Show("查询表达式不合法！请修改");
            }
            finally
            { 
                pQueryFilter =null;
                pFeatCls =null;
                pFeatCursor = null;
                pFeat = null;
            }

        }

        private void btnValue_Click(object sender, EventArgs e)
        {

            try
            {
                m_dict = new Dictionary<string, object>();
                lstValue.Items.Clear();
                int sSelIndex;
                sSelIndex = lstField.SelectedIndex;

                IFeatureLayer pFeatureLayer;
                IFeatureClass pFeatureClass=null;
                int iLayerCount;
                int i, j;
                ILayer pLayer;
                ILayer pGLayer;

                iLayerCount = ClsDeclare.g_pMap.LayerCount;
                for (i = 0; i < iLayerCount;i++ )
                {
                    pLayer = ClsDeclare.g_pMap.get_Layer(i);
                    if (ClsDeclare.g_pMap.get_Layer(i).Name == layerName)
                    {
                        pFeatureLayer = ClsDeclare.g_pMap.get_Layer(i) as IFeatureLayer;
                        pFeatureClass = pFeatureLayer.FeatureClass;
                        break;
                    }
                    if (pLayer is IGroupLayer)
                    {
                        ICompositeLayer pCompositeLayer;
                        pCompositeLayer = pLayer as ICompositeLayer;
                        for (j = 0; j < pCompositeLayer.Count; j++)
                        {
                            pGLayer = pCompositeLayer.get_Layer(j);
                            if (pGLayer.Name == layerName)
                            {
                                pFeatureLayer = pCompositeLayer.get_Layer(i) as IFeatureLayer;
                                pFeatureClass = pFeatureLayer.FeatureClass;
                                break;
                            }
                        }
                    }

                }
                string strTemp;
                strTemp = SubstrField(this.lstField.Text);
                if (strTemp == "")
                {
                    //ClsDeclare.g_ErrorHandler.DisplayInformation("您还没有选择字段！",false,"确定",null);
                    return;
                }

                sSelIndex = pFeatureClass.FindField(SubstrField(this.lstField.Text));
                IFeatureCursor pfCursor;
                pfCursor = pFeatureClass.Search(null,true);
                IFeature pFeature;
                pFeature = pfCursor.NextFeature();

                while(pFeature!=null)
                {
                    if (sSelIndex == 0)
                        lstValue.Items.Add(pFeature.get_Value(0));
                    else
                    {
                        if (DBNull.Value == pFeature.get_Value(sSelIndex))
                        {
                            pFeature = pfCursor.NextFeature();
                            continue;
                        }
                        else if (pFeature.get_Value(sSelIndex) is System.String)
                        {
                            if (m_dict.ContainsKey("'" + pFeature.get_Value(sSelIndex) + "'")==false)
                            {
                                m_dict.Add("'" + pFeature.get_Value(sSelIndex) + "'", pFeature);
                                lstValue.Items.Add("'" + pFeature.get_Value(sSelIndex) + "'");

                            }

                        }
                        else
                        {
                            if (m_dict.ContainsKey(Convert.ToString(pFeature.get_Value(sSelIndex))))
                            {
                                m_dict.Add(pFeature.get_Value(sSelIndex).ToString(),pFeature);
                                lstValue.Items.Add(pFeature.get_Value(sSelIndex));
                            }
                        }
              
                    
                    }
                    pFeature = pfCursor.NextFeature();
                }
                //if (this.lstValue.Items.Count == 0)
                    //ClsDeclare.g_ErrorHandler.DisplayInformation("该字段没有值！",false,null,null);
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show("错误", ex.StackTrace, MessageBoxButtons.OK, MessageBoxIcon.Error);
                //ClsDeclare.g_ErrorHandler.HandleError(true,null,0,null,ex.StackTrace);
                throw;
            }

        }

        private void btnOperate2_Click(object sender, EventArgs e)
        {
            ButtonX btnOperator;
            btnOperator = sender as ButtonX;
            String strAll;

            strAll = this.txtExpress.Text;
            m_PreFocus = strAll.Substring(0, this.txtExpress.SelectionStart);
            m_AfterFocus = strAll.Substring(this.txtExpress.SelectionStart);
            m_Focus = this.txtExpress.SelectionStart;

            if (btnOperator.Text != "()" && btnOperator.Text != "_" && btnOperator.Text != "%")
            {
                m_PreFocus = m_PreFocus + " " + btnOperator.Text + " ";
                m_Focus = m_PreFocus.Length;
            }
            else if (btnOperator.Text == "()")
            {
                m_PreFocus = m_PreFocus + " (";
                m_AfterFocus = ") " + m_AfterFocus;
                m_Focus = m_Focus + 2;
            }
            else if (btnOperator.Text == "_" || btnOperator.Text == "%")
            {
                m_PreFocus = m_PreFocus + btnOperator.Text;
                m_Focus = m_PreFocus.Length;
            }
            SetRichTextBox();
        }

        private void btnOperate3_Click(object sender, EventArgs e)
        {
            ButtonX btnOperator;
            btnOperator = sender as ButtonX;
            String strAll;

            strAll = this.txtExpress.Text;
            m_PreFocus = strAll.Substring(0, this.txtExpress.SelectionStart);
            m_AfterFocus = strAll.Substring(this.txtExpress.SelectionStart);
            m_Focus = this.txtExpress.SelectionStart;

            if (btnOperator.Text != "()" && btnOperator.Text != "_" && btnOperator.Text != "%")
            {
                m_PreFocus = m_PreFocus + " " + btnOperator.Text + " ";
                m_Focus = m_PreFocus.Length;
            }
            else if (btnOperator.Text == "()")
            {
                m_PreFocus = m_PreFocus + " (";
                m_AfterFocus = ") " + m_AfterFocus;
                m_Focus = m_Focus + 2;
            }
            else if (btnOperator.Text == "_" || btnOperator.Text == "%")
            {
                m_PreFocus = m_PreFocus + btnOperator.Text;
                m_Focus = m_PreFocus.Length;
            }
            SetRichTextBox();
        }

        private void btnOperate4_Click(object sender, EventArgs e)
        {
            ButtonX btnOperator;
            btnOperator = sender as ButtonX;
            String strAll;

            strAll = this.txtExpress.Text;
            m_PreFocus = strAll.Substring(0, this.txtExpress.SelectionStart);
            m_AfterFocus = strAll.Substring(this.txtExpress.SelectionStart);
            m_Focus = this.txtExpress.SelectionStart;

            if (btnOperator.Text != "()" && btnOperator.Text != "_" && btnOperator.Text != "%")
            {
                m_PreFocus = m_PreFocus + " " + btnOperator.Text + " ";
                m_Focus = m_PreFocus.Length;
            }
            else if (btnOperator.Text == "()")
            {
                m_PreFocus = m_PreFocus + " (";
                m_AfterFocus = ") " + m_AfterFocus;
                m_Focus = m_Focus + 2;
            }
            else if (btnOperator.Text == "_" || btnOperator.Text == "%")
            {
                m_PreFocus = m_PreFocus + btnOperator.Text;
                m_Focus = m_PreFocus.Length;
            }
            SetRichTextBox();
        }

        private void btnOperate5_Click(object sender, EventArgs e)
        {
            ButtonX btnOperator;
            btnOperator = sender as ButtonX;
            String strAll;

            strAll = this.txtExpress.Text;
            m_PreFocus = strAll.Substring(0, this.txtExpress.SelectionStart);
            m_AfterFocus = strAll.Substring(this.txtExpress.SelectionStart);
            m_Focus = this.txtExpress.SelectionStart;

            if (btnOperator.Text != "()" && btnOperator.Text != "_" && btnOperator.Text != "%")
            {
                m_PreFocus = m_PreFocus + " " + btnOperator.Text + " ";
                m_Focus = m_PreFocus.Length;
            }
            else if (btnOperator.Text == "()")
            {
                m_PreFocus = m_PreFocus + " (";
                m_AfterFocus = ") " + m_AfterFocus;
                m_Focus = m_Focus + 2;
            }
            else if (btnOperator.Text == "_" || btnOperator.Text == "%")
            {
                m_PreFocus = m_PreFocus + btnOperator.Text;
                m_Focus = m_PreFocus.Length;
            }
            SetRichTextBox();
        }

        private void btnOperate6_Click(object sender, EventArgs e)
        {
            ButtonX btnOperator;
            btnOperator = sender as ButtonX;
            String strAll;

            strAll = this.txtExpress.Text;
            m_PreFocus = strAll.Substring(0, this.txtExpress.SelectionStart);
            m_AfterFocus = strAll.Substring(this.txtExpress.SelectionStart);
            m_Focus = this.txtExpress.SelectionStart;

            if (btnOperator.Text != "()" && btnOperator.Text != "_" && btnOperator.Text != "%")
            {
                m_PreFocus = m_PreFocus + " " + btnOperator.Text + " ";
                m_Focus = m_PreFocus.Length;
            }
            else if (btnOperator.Text == "()")
            {
                m_PreFocus = m_PreFocus + " (";
                m_AfterFocus = ") " + m_AfterFocus;
                m_Focus = m_Focus + 2;
            }
            else if (btnOperator.Text == "_" || btnOperator.Text == "%")
            {
                m_PreFocus = m_PreFocus + btnOperator.Text;
                m_Focus = m_PreFocus.Length;
            }
            SetRichTextBox();
        }

        private void btnOperate7_Click(object sender, EventArgs e)
        {
            ButtonX btnOperator;
            btnOperator = sender as ButtonX;
            String strAll;

            strAll = this.txtExpress.Text;
            m_PreFocus = strAll.Substring(0, this.txtExpress.SelectionStart);
            m_AfterFocus = strAll.Substring(this.txtExpress.SelectionStart);
            m_Focus = this.txtExpress.SelectionStart;

            if (btnOperator.Text != "()" && btnOperator.Text != "_" && btnOperator.Text != "%")
            {
                m_PreFocus = m_PreFocus + " " + btnOperator.Text + " ";
                m_Focus = m_PreFocus.Length;
            }
            else if (btnOperator.Text == "()")
            {
                m_PreFocus = m_PreFocus + " (";
                m_AfterFocus = ") " + m_AfterFocus;
                m_Focus = m_Focus + 2;
            }
            else if (btnOperator.Text == "_" || btnOperator.Text == "%")
            {
                m_PreFocus = m_PreFocus + btnOperator.Text;
                m_Focus = m_PreFocus.Length;
            }
            SetRichTextBox();
        }

        private void btnOperate8_Click(object sender, EventArgs e)
        {
            ButtonX btnOperator;
            btnOperator = sender as ButtonX;
            String strAll;

            strAll = this.txtExpress.Text;
            m_PreFocus = strAll.Substring(0, this.txtExpress.SelectionStart);
            m_AfterFocus = strAll.Substring(this.txtExpress.SelectionStart);
            m_Focus = this.txtExpress.SelectionStart;

            if (btnOperator.Text != "()" && btnOperator.Text != "_" && btnOperator.Text != "%")
            {
                m_PreFocus = m_PreFocus + " " + btnOperator.Text + " ";
                m_Focus = m_PreFocus.Length;
            }
            else if (btnOperator.Text == "()")
            {
                m_PreFocus = m_PreFocus + " (";
                m_AfterFocus = ") " + m_AfterFocus;
                m_Focus = m_Focus + 2;
            }
            else if (btnOperator.Text == "_" || btnOperator.Text == "%")
            {
                m_PreFocus = m_PreFocus + btnOperator.Text;
                m_Focus = m_PreFocus.Length;
            }
            SetRichTextBox();
        }

        private void btnOperate9_Click(object sender, EventArgs e)
        {
            ButtonX btnOperator;
            btnOperator = sender as ButtonX;
            String strAll;

            strAll = this.txtExpress.Text;
            m_PreFocus = strAll.Substring(0, this.txtExpress.SelectionStart);
            m_AfterFocus = strAll.Substring(this.txtExpress.SelectionStart);
            m_Focus = this.txtExpress.SelectionStart;

            if (btnOperator.Text != "()" && btnOperator.Text != "_" && btnOperator.Text != "%")
            {
                m_PreFocus = m_PreFocus + " " + btnOperator.Text + " ";
                m_Focus = m_PreFocus.Length;
            }
            else if (btnOperator.Text == "()")
            {
                m_PreFocus = m_PreFocus + " (";
                m_AfterFocus = ") " + m_AfterFocus;
                m_Focus = m_Focus + 2;
            }
            else if (btnOperator.Text == "_" || btnOperator.Text == "%")
            {
                m_PreFocus = m_PreFocus + btnOperator.Text;
                m_Focus = m_PreFocus.Length;
            }
            SetRichTextBox();
        }

        private void btnOperate10_Click(object sender, EventArgs e)
        {
            ButtonX btnOperator;
            btnOperator = sender as ButtonX;
            String strAll;

            strAll = this.txtExpress.Text;
            m_PreFocus = strAll.Substring(0, this.txtExpress.SelectionStart);
            m_AfterFocus = strAll.Substring(this.txtExpress.SelectionStart);
            m_Focus = this.txtExpress.SelectionStart;

            if (btnOperator.Text != "()" && btnOperator.Text != "_" && btnOperator.Text != "%")
            {
                m_PreFocus = m_PreFocus + " " + btnOperator.Text + " ";
                m_Focus = m_PreFocus.Length;
            }
            else if (btnOperator.Text == "()")
            {
                m_PreFocus = m_PreFocus + " (";
                m_AfterFocus = ") " + m_AfterFocus;
                m_Focus = m_Focus + 2;
            }
            else if (btnOperator.Text == "_" || btnOperator.Text == "%")
            {
                m_PreFocus = m_PreFocus + btnOperator.Text;
                m_Focus = m_PreFocus.Length;
            }
            SetRichTextBox();
        }

        private void btnOperate11_Click(object sender, EventArgs e)
        {
            ButtonX btnOperator;
            btnOperator = sender as ButtonX;
            String strAll;

            strAll = this.txtExpress.Text;
            m_PreFocus = strAll.Substring(0, this.txtExpress.SelectionStart);
            m_AfterFocus = strAll.Substring(this.txtExpress.SelectionStart);
            m_Focus = this.txtExpress.SelectionStart;

            if (btnOperator.Text != "()" && btnOperator.Text != "_" && btnOperator.Text != "%")
            {
                m_PreFocus = m_PreFocus + " " + btnOperator.Text + " ";
                m_Focus = m_PreFocus.Length;
            }
            else if (btnOperator.Text == "()")
            {
                m_PreFocus = m_PreFocus + " (";
                m_AfterFocus = ") " + m_AfterFocus;
                m_Focus = m_Focus + 2;
            }
            else if (btnOperator.Text == "_" || btnOperator.Text == "%")
            {
                m_PreFocus = m_PreFocus + btnOperator.Text;
                m_Focus = m_PreFocus.Length;
            }
            SetRichTextBox();
        }

        private void btnOperate12_Click(object sender, EventArgs e)
        {
            ButtonX btnOperator;
            btnOperator = sender as ButtonX;
            String strAll;

            strAll = this.txtExpress.Text;
            m_PreFocus = strAll.Substring(0, this.txtExpress.SelectionStart);
            m_AfterFocus = strAll.Substring(this.txtExpress.SelectionStart);
            m_Focus = this.txtExpress.SelectionStart;

            if (btnOperator.Text != "()" && btnOperator.Text != "_" && btnOperator.Text != "%")
            {
                m_PreFocus = m_PreFocus + " " + btnOperator.Text + " ";
                m_Focus = m_PreFocus.Length;
            }
            else if (btnOperator.Text == "()")
            {
                m_PreFocus = m_PreFocus + " (";
                m_AfterFocus = ") " + m_AfterFocus;
                m_Focus = m_Focus + 2;
            }
            else if (btnOperator.Text == "_" || btnOperator.Text == "%")
            {
                m_PreFocus = m_PreFocus + btnOperator.Text;
                m_Focus = m_PreFocus.Length;
            }
            SetRichTextBox();
        }

        private void btnOperate13_Click(object sender, EventArgs e)
        {
            ButtonX btnOperator;
            btnOperator = sender as ButtonX;
            String strAll;

            strAll = this.txtExpress.Text;
            m_PreFocus = strAll.Substring(0, this.txtExpress.SelectionStart);
            m_AfterFocus = strAll.Substring(this.txtExpress.SelectionStart);
            m_Focus = this.txtExpress.SelectionStart;

            if (btnOperator.Text != "()" && btnOperator.Text != "_" && btnOperator.Text != "%")
            {
                m_PreFocus = m_PreFocus + " " + btnOperator.Text + " ";
                m_Focus = m_PreFocus.Length;
            }
            else if (btnOperator.Text == "()")
            {
                m_PreFocus = m_PreFocus + " (";
                m_AfterFocus = ") " + m_AfterFocus;
                m_Focus = m_Focus + 2;
            }
            else if (btnOperator.Text == "_" || btnOperator.Text == "%")
            {
                m_PreFocus = m_PreFocus + btnOperator.Text;
                m_Focus = m_PreFocus.Length;
            }
            SetRichTextBox();
        }

        private void btnOperate14_Click(object sender, EventArgs e)
        {
            ButtonX btnOperator;
            btnOperator = sender as ButtonX;
            String strAll;

            strAll = this.txtExpress.Text;
            m_PreFocus = strAll.Substring(0, this.txtExpress.SelectionStart);
            m_AfterFocus = strAll.Substring(this.txtExpress.SelectionStart);
            m_Focus = this.txtExpress.SelectionStart;

            if (btnOperator.Text != "()" && btnOperator.Text != "_" && btnOperator.Text != "%")
            {
                m_PreFocus = m_PreFocus + " " + btnOperator.Text + " ";
                m_Focus = m_PreFocus.Length;
            }
            else if (btnOperator.Text == "()")
            {
                m_PreFocus = m_PreFocus + " (";
                m_AfterFocus = ") " + m_AfterFocus;
                m_Focus = m_Focus + 2;
            }
            else if (btnOperator.Text == "_" || btnOperator.Text == "%")
            {
                m_PreFocus = m_PreFocus + btnOperator.Text;
                m_Focus = m_PreFocus.Length;
            }
            SetRichTextBox();
        }
    }
}
