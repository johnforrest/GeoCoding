using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geometry;
using DevComponents.DotNetBar;

namespace SelectQuery
{
    public partial class FrmMeasure : DevComponents.DotNetBar.Office2007Form
    {
        IMapControl3 m_MapControl;
        //IProjectedCoordinateSystem m_pProjectedCoordinateSystem;
        //bool m_blnAtuoPrj;
        ButtonItem[] m_DistanceButtonArray;
        ButtonItem[] m_AreaButtonArray;
        ClsMeasureLength m_pMeasureLength;

        public IMapControl3 MainMap
        {
            get
            {
                return m_MapControl;
            }
            set
            {
                m_MapControl = value;
            }
        }

        public FrmMeasure()
        {
            InitializeComponent();
        }

        private void MeasureLength_Click(object sender, EventArgs e)
        {
            if (this.MeasureLength.Checked)
                this.MeasureLength.Checked = false;
            else
                this.MeasureLength.Checked = true;
            this.MeasurePolygon.Checked = false;
            this.MeasureFeature.Checked = false;

            if (this.MeasureLength.Checked)
            {
                m_pMeasureLength = new ClsMeasureLength();
                m_pMeasureLength.OnCreate(this.MainMap);
                ClsDeclare.g_txtMeasure = this.txtMeasure;
                this.MainMap.CurrentTool = m_pMeasureLength;
            }
            else
                this.MainMap.CurrentTool = null;

        }

        private void MeasurePolygon_Click(object sender, EventArgs e)
        {
            if (this.MeasurePolygon.Checked)
                this.MeasurePolygon.Checked = false;
            else
                this.MeasurePolygon.Checked = true;
            this.MeasureLength.Checked = false;
            this.MeasureFeature.Checked = false;

            if (this.MeasurePolygon.Checked)
            {
                ClsMeasurePolygon pMeasurePolygon = new ClsMeasurePolygon();
                if (m_pMeasureLength != null)
                {
                    if (m_pMeasureLength.DrawLineElement != null)
                    {
                        m_pMeasureLength.DrawLineElement.DeleteAllElements();
                        m_pMeasureLength.ActiveView.PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGeography, null, null);

                    }
                }
                pMeasurePolygon.OnCreate(this.MainMap);
                ClsDeclare.g_txtMeasure = this.txtMeasure;
                this.MainMap.CurrentTool = pMeasurePolygon;
            }
            else
                this.MainMap.CurrentTool = null;


        }

        private void MeasureFeature_Click(object sender, EventArgs e)
        {
            if (this.MeasureFeature.Checked)
                this.MeasureFeature.Checked = false;
            else
                this.MeasureFeature.Checked = true;
            this.MeasureLength.Checked = false;
            this.MeasurePolygon.Checked = false;

            if (MeasureFeature.Checked)
            {
                ClsMeasureFeature pMeasureFeature = new ClsMeasureFeature();
                pMeasureFeature.OnCreate(this.MainMap);
                ClsDeclare.g_txtMeasure = this.txtMeasure;
                this.MainMap.CurrentTool = pMeasureFeature;
            }
            else
                this.MainMap.CurrentTool = null;
        }

        private void Snap_Click(object sender, EventArgs e)
        {
            if (ClsDeclare.g_blnSnap)
                ClsDeclare.g_blnSnap = false;
            else
                ClsDeclare.g_blnSnap = true;
        }

        private void frmMeasure_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (m_pMeasureLength != null)
                {
                    if (m_pMeasureLength.DrawLineElement != null)
                    {
                        m_pMeasureLength.DrawLineElement.DeleteAllElements();
                        m_pMeasureLength.ActiveView.PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGeography,null,null);
                    }
                }
                ClsDeclare.g_Sys.MapControl.CurrentTool = null;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace.ToString());
                throw;
            }
        }

        private void frmMeasure_Load(object sender, EventArgs e)
        {
            ClsDeclare.g_txtMeasure = this.txtMeasure;
            ClsDeclare.g_txtMeasure.Text = "如果量算距离，请点击长度量算按钮，然后画线" + "\r" + "\n" + "如果量算面积，请点击面积量算按钮，然后画多边形" + "\r" + "\n" + "如果量算要素，请点击要素量算按钮，然后选择要素";
            ClsDeclare.g_blnSnap = false;

            InitialButtonItems();
            ClsDeclare.g_UnitConverter = new ClsConvertUnit(ClsMeasureSnap.GetMapUnits(ClsDeclare.g_pMap));
            SetButtonCheck(ClsMeasureSnap.GetMapUnits(ClsDeclare.g_pMap));

        }
        private void InitialButtonItems()
        {
            m_DistanceButtonArray = new DevComponents.DotNetBar.ButtonItem[]{ this.miDKilometer, this.miDMeter, this.miDDecimeter, this.miDCentimeter, this.miDMillimeter, this.miDecimalDegree };
            m_AreaButtonArray = new DevComponents.DotNetBar.ButtonItem[]{ this.miAKilometer, this.miAMeter, this.miADecimeter, this.miACentimeter, this.miAMillimeter };


        }
        private void SetButtonCheck(string vMapUnitName)
        {
            int i;
            DevComponents.DotNetBar.ButtonItem pButtonItem;

            for (i = 0; i < m_DistanceButtonArray.Length;i++ )
            {
                pButtonItem = m_DistanceButtonArray[i];
                if (pButtonItem.Text == vMapUnitName)
                    pButtonItem.Checked = true;
                else
                    pButtonItem.Checked = false;
            }
            for (i = 0; i < m_AreaButtonArray.Length;i++ )
            {
                pButtonItem = m_AreaButtonArray[i];
                if (pButtonItem.Text == vMapUnitName)
                    pButtonItem.Checked = true;
                else
                    pButtonItem.Checked = false;

            }
        
        
        }

        private void miDKilometer_Click(object sender, EventArgs e)
        {
            ButtonItem pButtonItem = sender as ButtonItem;
            SetButtonCheck(pButtonItem.Text);
            ClsDeclare.g_UnitConverter.SetCurrentMapUnit(pButtonItem.Text);
        }

        private void miDMeter_Click(object sender, EventArgs e)
        {
            miDKilometer_Click(sender, e);
        }

        private void miACentimeter_Click(object sender, EventArgs e)
        {
            miDKilometer_Click(sender, e);
        }

        private void miADecimeter_Click(object sender, EventArgs e)
        {
            miDKilometer_Click(sender, e);
        }

        private void miAKilometer_Click(object sender, EventArgs e)
        {
            miDKilometer_Click(sender, e);
        }

        private void miAMeter_Click(object sender, EventArgs e)
        {
            miDKilometer_Click(sender, e);
        }

        private void miAMillimeter_Click(object sender, EventArgs e)
        {
            miDKilometer_Click(sender, e);
        }

        private void miArea_Click(object sender, EventArgs e)
        {
            //miDKilometer_Click(sender, e);
        }

        private void miClear_Click(object sender, EventArgs e)
        {
            this.txtMeasure.Clear();
        }

        private void miDCentimeter_Click(object sender, EventArgs e)
        {
            miDKilometer_Click(sender, e);
        }

        private void miDDecimeter_Click(object sender, EventArgs e)
        {
            miDKilometer_Click(sender, e);
        }

        private void miDecimalDegree_Click(object sender, EventArgs e)
        {
            miDKilometer_Click(sender, e);
        }

        private void miDistance_Click(object sender, EventArgs e)
        {
            //miDKilometer_Click(sender, e);
        }

        private void miDMillimeter_Click(object sender, EventArgs e)
        {
            miDKilometer_Click(sender, e);
        }
         
    }
}
