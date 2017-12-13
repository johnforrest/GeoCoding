using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar.Controls;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geoprocessor;
using ZJGISOpenData;
using ZJGISOpenData.Forms;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geoprocessing;

namespace ZJGISDataMatch.Forms
{
    public partial class FrmIntersect : DevComponents.DotNetBar.Office2007Form
    {
        IMapControl2 pMapControl;
        Dictionary<string, string> pdic = new Dictionary<string, string>();
        Dictionary<string, ILayer> pdicLayer = new Dictionary<string, ILayer>();


        public FrmIntersect()
        {
            InitializeComponent();
        }

        public FrmIntersect(IMapControl2 mapcontrol)
        {
            InitializeComponent();
            this.pMapControl = mapcontrol;
        }
        private void FrmIntersect_Load(object sender, EventArgs e)
        {
            InitialComboxEx(comboBoxEx1);
            InitialComboxEx(comboBoxEx2);

        }
        private void InitialComboxEx(ComboBoxEx cobox)
        {
            IMap pMap = pMapControl.Map;
            IEnumLayer pEnumLayer = pMap.get_Layers(null, true);
            pEnumLayer.Reset();
            ILayer pLayer = pEnumLayer.Next();
            while (pLayer != null)
            {
                cobox.Items.Add(pLayer.Name);
                //获取layer的完整路径
                IDataLayer2 pDatalayer = pLayer as IDataLayer2;
                IDatasetName pDatasetName = pDatalayer.DataSourceName as IDatasetName;
                IWorkspaceName pWPName = pDatasetName.WorkspaceName;

                if (!pdic.ContainsKey(pLayer.Name))
                {
                    pdic.Add(pLayer.Name, pWPName.PathName + "\\" + pLayer.Name + ".shp");
                }
                if (!pdicLayer.ContainsKey(pLayer.Name))
                {
                	pdicLayer.Add(pLayer.Name, pLayer);
                }
                pLayer = pEnumLayer.Next();

            }

        }

     

        private void buttonX4SaveAs_Click(object sender, EventArgs e)
        {
            //set the output layer
            SaveFileDialog saveDlg = new SaveFileDialog();
            saveDlg.CheckPathExists = true;
            saveDlg.Filter = "Shapefile (*.shp)|*.shp|File Geodatabase (*.gdb)|*.gdb";
            saveDlg.OverwritePrompt = true;
            saveDlg.Title = "Output Layer";
            saveDlg.RestoreDirectory = true;
            saveDlg.FileName = (string)comboBoxEx2.SelectedItem + "_Intersect.shp";

            DialogResult dr = saveDlg.ShowDialog();
            if (dr == DialogResult.OK)
                textBoxX3.Text = saveDlg.FileName;
        }

        private void btnIntersect_Click(object sender, EventArgs e)
        {
            IGpValueTableObject valTbl = new GpValueTableObjectClass();
            valTbl.SetColumns(2);
            object row = "";
            object rank = 1;

            //row = inputFeatClass; 
            row = pdicLayer[this.comboBoxEx1.SelectedItem.ToString()];
            valTbl.SetRow(0, ref row);
            valTbl.SetValue(0, 1, ref rank);

            //row = clipFeatClass;
            row = pdicLayer[this.comboBoxEx2.SelectedItem.ToString()];
            valTbl.SetRow(1, ref row);
            rank = 2;
            valTbl.SetValue(1, 1, ref rank);



            //构造Geoprocessor  
            Geoprocessor gp = new Geoprocessor();
            gp.OverwriteOutput = true;
            //设置参数  
            ESRI.ArcGIS.AnalysisTools.Intersect intersect = new ESRI.ArcGIS.AnalysisTools.Intersect();
            //intersect.in_features = @"F:\foshan\Data\wuqutu_b.shp;F:\foshan\Data\world30.shp";
            //intersect.in_features = this.comboBoxEx1.SelectedItem + ";" + this.comboBoxEx2.SelectedItem;
            //intersect.in_features = pdic[this.comboBoxEx1.SelectedItem.ToString()] + ";" + pdic[this.comboBoxEx2.SelectedItem.ToString()];
            intersect.in_features = valTbl;
            //intersect.out_feature_class = @"E:\intersect.shp";
            intersect.out_feature_class = this.textBoxX3.Text;
            intersect.join_attributes = "ALL";
            intersect.output_type = "INPUT";

            //执行Intersect工具  
            //RunTool(gp, intersect, null);

            try
            {
                gp.Execute(intersect, null);
                //ReturnMessages(geoprocessor);
                MessageBox.Show("Intersect分析完成!");

            }
            catch (Exception err)
            {
                //Console.WriteLine(err.Message);
                MessageBox.Show(err.Message);
                //ReturnMessages(geoprocessor);
            }


        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //private string GetPathByName(string name)
        //{
        //    IMap pMap = pMapControl.Map;
        //    IEnumLayer pEnumLayer = pMap.get_Layers(null, true);
        //    pEnumLayer.Reset();
        //    ILayer pLayer = pEnumLayer.Next();
        //    while (pLayer != null)
        //    {
        //        //cobox.Items.Add(pLayer.Name);
        //        // Console.WriteLine(pLayer.Name);             
        //        pLayer = pEnumLayer.Next();
        //    }
        //}

        private void RunTool(Geoprocessor geoprocessor, IGPProcess process, ITrackCancel TC)
        {
            // Set the overwrite output option to true  
            geoprocessor.OverwriteOutput = true;

            try
            {
                geoprocessor.Execute(process, null);
                ReturnMessages(geoprocessor);

            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                ReturnMessages(geoprocessor);
            }
        }

        // Function for returning the tool messages.  
        private void ReturnMessages(Geoprocessor gp)
        {
            string ms = "";
            if (gp.MessageCount > 0)
            {
                for (int Count = 0; Count <= gp.MessageCount - 1; Count++)
                {
                    ms += gp.GetMessage(Count);
                }
            }

        }

        private void comboBoxEx1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBoxEx2_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }



    }
}
