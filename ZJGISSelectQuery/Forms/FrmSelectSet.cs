using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geodatabase;
using Microsoft.VisualBasic;

namespace SelectQuery
{
    public partial class FrmSelectSet :DevComponents.DotNetBar.Office2007Form
    {
        ISelectionEnvironment m_pSelectEnv;
        int[] m_ChangeSelect;
        long m_MaxSelectCount;
        IMapControl2 m_MapControl;

        public IMapControl3 MainMap
        {
            get
            {
                return (IMapControl3)m_MapControl;
            }
            set
            {
                m_MapControl = value;
            }
        }
        public long MaxSelectCount
        {
            get
            {
                return m_MaxSelectCount;
            }
            set
            {
                m_MaxSelectCount = value;
            }
        }

        public FrmSelectSet()
        {
            InitializeComponent();
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            int i;
            for (i = 0; i < this.dgvGridList.RowCount; i++)
            {
                this.dgvGridList.Rows[i].Cells[0].Value = true;
            }
        }

        private void btnInvertSelect_Click(object sender, EventArgs e)
        {
            this.dgvGridList.CurrentCell=this.dgvGridList.Rows[0].Cells[1];


            int i;
            for (i = 0; i < this.dgvGridList.RowCount; i++)
            {
                if (this.dgvGridList.Rows[i].Cells["Is_Checked"].Value!=null)
                    this.dgvGridList.Rows[i].Cells["Is_Checked"].Value = false;
                else if (this.dgvGridList.Rows[i].Cells["Is_Checked"].Value==null)
                    this.dgvGridList.Rows[i].Cells["Is_Checked"].Value = true;
            }
        }

        private void frmSelectSet_Load(object sender, EventArgs e)
        {
            dgvGridList.ClearSelection();
            dgvGridList.Enabled = true;

            System.Drawing.Color pColor;
            IRgbColor pRGBColor;
            m_pSelectEnv = new SelectionEnvironmentClass();
            pRGBColor = m_pSelectEnv.DefaultColor as IRgbColor;
            pColor = Color.FromArgb(pRGBColor.Red, pRGBColor.Green, pRGBColor.Blue);
            this.cColorPicker.SelectedColor = pColor;

            this.txtTolerance.Text = m_pSelectEnv.SearchTolerance.ToString();
            this.txtSelectCount.Text = ClsDeclare.g_Sys.MaxSelectCount.ToString();

            PopGrid();
            this.dgvGridList.AllowUserToAddRows = false;
            this.dgvGridList.ReadOnly = false;


        }
        private void PopGrid()
        {
            int i;
            IFeatureLayer pFeatureLayer;
            IFeatureSelection pFeatSel;
            int pRow;
            Collection pLyrCol;
            Collection LyrCol=null;
            pLyrCol = ClsSelectQuery.FunGetFeaLyrCol(ClsDeclare.g_pMap,null,LyrCol);
            m_ChangeSelect = new int[pLyrCol.Count];

            for (i = 1; i <= pLyrCol.Count; i++)
            {
                ILayer pLayer;
                pLayer = pLyrCol[i] as ILayer;

                if (pLayer is IFeatureLayer)
                {
                    pFeatureLayer = pLayer as IFeatureLayer;
                    pFeatSel = pFeatureLayer as IFeatureSelection;
                    if (pFeatSel.SelectionSet != null)
                    {
                        dgvGridList.Rows.Add();
                        pRow = dgvGridList.RowCount;
                        if (pFeatureLayer.Selectable)
                        {
                            dgvGridList.Rows[pRow - 2].Cells[0].Value = true;
                            m_ChangeSelect[i-1] = 1;


                        }
                        else
                        {
                            dgvGridList.Rows[pRow - 2].Cells[0].Value = false;
                            m_ChangeSelect[i] = 0;
                        }
                        dgvGridList.Rows[pRow - 2].Cells[1].Value = pFeatureLayer.Name;
                    }
                }
            
            }
        
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            int i;
            IFeatureLayer pFeatureLayer;
            bool bClearSelect;
            IActiveView pActiveView;

            m_pSelectEnv = new SelectionEnvironmentClass();
            bClearSelect = false;
            for (i = 0; i < dgvGridList.RowCount; i++)
            {
                pFeatureLayer = ClsSelectQuery.FunFindFeatureLayerByName(dgvGridList.Rows[i].Cells[1].Value.ToString(),ClsDeclare.g_pMap);
                if (pFeatureLayer is IFeatureLayer)
                {
                    if (this.dgvGridList.Rows[i].Cells[0].Value != null)
                        pFeatureLayer.Selectable = true;
                    else
                        pFeatureLayer.Selectable = false;
                    if (pFeatureLayer.Selectable == false && m_ChangeSelect[i] == 1)
                        bClearSelect = true;

                
                }
            
            }
            IRgbColor pRGBColor = new RgbColorClass();
            pRGBColor.Red = cColorPicker.SelectedColor.R;
            pRGBColor.Green = cColorPicker.SelectedColor.G;
            pRGBColor.Blue = cColorPicker.SelectedColor.B;
            m_pSelectEnv.DefaultColor = pRGBColor;

            if (txtTolerance != null)
                m_pSelectEnv.SearchTolerance = Convert.ToInt32(txtTolerance.Text);

            if (this.txtSelectCount != null)
            {
                if(this.txtSelectCount.Text == "")
                    m_MaxSelectCount=0;
                else
                    m_MaxSelectCount=Convert.ToInt64(this.txtSelectCount.Text);
                ClsDeclare.g_Sys.MaxSelectCount = m_MaxSelectCount;
            }
            if (bClearSelect == true)
            {
                pActiveView = ClsDeclare.g_pMap as IActiveView;
                pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection,null,null);
                ClsDeclare.g_pMap.ClearSelection();
                pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection,null,null);
            }
            this.Close();
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtTolerance_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(8) || e.KeyChar == '.')
            {
                if (e.KeyChar == '.' && (Strings.InStr(txtTolerance.Text, ".", CompareMethod.Text))!=0)
                    e.Handled = true;
                else
                    e.Handled = false;
            }
            else
                e.Handled = true;
        }
    }
}
