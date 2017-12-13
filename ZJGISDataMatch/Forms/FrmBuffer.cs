using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geoprocessing;
using ESRI.ArcGIS.Geoprocessor;
using ZJGISGCoding;
using ZJGISDataUpdating;
using ESRI.ArcGIS.Geodatabase;
using DevComponents.DotNetBar.Controls;

namespace ZJGISDataMatch.Forms
{
    public partial class FrmBuffer : DevComponents.DotNetBar.Office2007Form
    {
        //in order to scroll the messages textbox to the bottom we must import this Win32 call
        [DllImport("user32.dll")]
        private static extern int PostMessage(IntPtr wnd,
                                              uint Msg,
                                              IntPtr wParam,
                                              IntPtr lParam);

        private IHookHelper m_hookHelper = null;
        private const uint WM_VSCROLL = 0x0115;
        private const uint SB_BOTTOM = 7;

        //private IMapControl2 pMapControl;

        //public IMapControl2 PMapControl
        //{
        //    get { return pMapControl; }
        //    set { pMapControl = Value; }
        //}

        public FrmBuffer()
        {
            InitializeComponent();
        }


        private void FrmBuffer_Load(object sender, EventArgs e)
        {
            ClsCheckData.AddDataToCom(m_hookHelper.FocusMap as IMap, comboBoxEx1);
            InitialComboxEx(comboBoxEx2);
            //默认选中第一项
            comboBoxEx2.SelectedItem = comboBoxEx2.Items[0];
        }

        private void InitialComboxEx(ComboBoxEx pComboBoxEx)
        {
            pComboBoxEx.Items.Add("Meters");
            pComboBoxEx.Items.Add("Kilometers");
            pComboBoxEx.Items.Add("Decimal degrees");
            
            
        }

        public FrmBuffer(IHookHelper hookHelper)
        {
            InitializeComponent();
            m_hookHelper = hookHelper;

        }


        private void buttonX1Buffer_Click(object sender, EventArgs e)
        {
            //make sure that all parameters are okay
            double bufferDistance;
            //转换distance为double类型
            double.TryParse(textBoxX1.Text, out bufferDistance);
            if (0.0 == bufferDistance)
            {
                MessageBox.Show("缓冲区距离无效!");
                return;
            }
            //判断输出路径是否合法
            try
            {
                if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(textBoxX2.Text)) || ".shp" != System.IO.Path.GetExtension(textBoxX2.Text) )
                {
                    return;
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("路径无效!");

            }
            //判断图层个数
            if (m_hookHelper.FocusMap.LayerCount == 0)
                return;

            //get the layer from the map
            IFeatureLayer pInputlayer = GetFeatureLayer((string)comboBoxEx1.SelectedItem);
            if (null == pInputlayer)
            {
                textBoxX3.Text += "找不到图层 " + (string)comboBoxEx1.SelectedItem + "!\r\n";
                return;
            }

            //scroll the textbox to the bottom
            ScrollToBottom();

            textBoxX3.Text += "\r\n缓冲区分析开始，请稍候..\r\n";
            textBoxX3.Update();



            //get an instance of the geoprocessor
            Geoprocessor gp = new Geoprocessor();
            gp.OverwriteOutput = true;


            //IDataset dataset1 = pInputlayer as IDataset;
            //IGeoDataset geoDataset = dataset1 as IGeoDataset;

            //ClsConvertUnit clsConvertUnit = new ClsConvertUnit();
            //double pTrandist = clsConvertUnit.GetBufferValueByUnit(geoDataset.SpatialReference, bufferDistance);

            object test = Convert.ToString(bufferDistance) + " " + (string)comboBoxEx2.SelectedItem;

            //create a new instance of a buffer tool
            ESRI.ArcGIS.AnalysisTools.Buffer buffer = new ESRI.ArcGIS.AnalysisTools.Buffer(pInputlayer, textBoxX2.Text, Convert.ToString(bufferDistance) + " " + (string)comboBoxEx2.SelectedItem);
            ////ESRI.ArcGIS.AnalysisTools.Buffer buffer = new ESRI.ArcGIS.AnalysisTools.Buffer(pInputlayer, textBoxX2.Text, Convert.ToString(pTrandist) + " " + (string)comboBoxEx2.SelectedItem);

            //buffer.dissolve_option = "ALL";//这个要设成ALL,否则相交部分不会融合
            buffer.dissolve_option = "NONE";//这个要设成ALL,否则相交部分不会融合
            //buffer.line_side = "FULL";//默认是"FULL",最好不要改否则出错
            //buffer.line_end_type = "ROUND";//默认是"ROUND",最好不要改否则出错

            //execute the buffer tool (very easy :-))
            IGeoProcessorResult results = null;

            try
            {
                results = (IGeoProcessorResult)gp.Execute(buffer, null);
            }
            catch (Exception ex)
            {
                textBoxX3.Text += "缓冲区分析失败: " + pInputlayer.Name + "\r\n";
            }
            if (results.Status != esriJobStatus.esriJobSucceeded)
            {
                textBoxX3.Text += "缓冲区分析失败: " + pInputlayer.Name + "\r\n";
            }

            //scroll the textbox to the bottom
            ScrollToBottom();

            textBoxX3.Text += "\r\n分析完成.\r\n";
            textBoxX3.Text += "-----------------------------------------------------------------------------------------\r\n";
            //scroll the textbox to the bottom
            ScrollToBottom();
        }

        private void buttonX2Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private string ReturnMessages(Geoprocessor gp)
        {
            StringBuilder sb = new StringBuilder();
            if (gp.MessageCount > 0)
            {
                for (int Count = 0; Count <= gp.MessageCount - 1; Count++)
                {
                    System.Diagnostics.Trace.WriteLine(gp.GetMessage(Count));
                    sb.AppendFormat("{0}\n", gp.GetMessage(Count));
                }
            }
            return sb.ToString();
        }

        private IFeatureLayer GetFeatureLayer(string layerName)
        {
            //get the layers from the maps
            IEnumLayer layers = GetLayers();
            layers.Reset();

            ILayer layer = null;
            while ((layer = layers.Next()) != null)
            {
                if (layer.Name == layerName)
                    return layer as IFeatureLayer;
            }

            return null;
        }

        private IEnumLayer GetLayers()
        {
            UID uid = new UIDClass();
            uid.Value = "{40A9E885-5533-11d0-98BE-00805F7CED21}";
            IEnumLayer layers = m_hookHelper.FocusMap.get_Layers(uid, true);

            return layers;
        }

        private void ScrollToBottom()
        {
            PostMessage((IntPtr)textBoxX3.Handle, WM_VSCROLL, (IntPtr)SB_BOTTOM, (IntPtr)IntPtr.Zero);
        }

        private void buttonOpenData_Click(object sender, EventArgs e)
        {
            //set the output layer
            SaveFileDialog saveDlg = new SaveFileDialog();
            saveDlg.CheckPathExists = true;
            saveDlg.Filter = "Shapefile (*.shp)|*.shp|File Geodatabase (*.gdb)|*.gdb";
            saveDlg.OverwritePrompt = true;
            saveDlg.Title = "Output Layer";
            saveDlg.RestoreDirectory = true;
            saveDlg.FileName = (string)comboBoxEx1.Text + "_Buffer.shp";

            DialogResult dr = saveDlg.ShowDialog();
            if (dr == DialogResult.OK)
                textBoxX2.Text = saveDlg.FileName;
        }

    }
}
