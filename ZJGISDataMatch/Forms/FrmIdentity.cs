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
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geoprocessing;
using ESRI.ArcGIS.Geoprocessor;

namespace ZJGISDataMatch.Forms
{
    public partial class FrmIdentity : DevComponents.DotNetBar.Office2007Form
    {
        IMapControl2 pMapControl;
        Dictionary<string, string> pdic = new Dictionary<string, string>();
        Dictionary<string, ILayer> pdicLayer = new Dictionary<string, ILayer>();

        public FrmIdentity()
        {
            InitializeComponent();
        }
        public FrmIdentity(IMapControl2 mapcontrol)
        {
            InitializeComponent();
            this.pMapControl = mapcontrol;
        }
        private void FrmIdentity_Load(object sender, EventArgs e)
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
        private void btnSavePath_Click(object sender, EventArgs e)
        {
            //set the output layer
            SaveFileDialog saveDlg = new SaveFileDialog();
            saveDlg.CheckPathExists = true;
            saveDlg.Filter = "Shapefile (*.shp)|*.shp|File Geodatabase (*.gdb)|*.gdb";
            saveDlg.OverwritePrompt = true;
            saveDlg.Title = "Output Layer";
            saveDlg.RestoreDirectory = true;
            saveDlg.FileName = (string)comboBoxEx2.SelectedItem + "_Indentity.shp";

            DialogResult dr = saveDlg.ShowDialog();
            if (dr == DialogResult.OK)
                textBoxX1.Text = saveDlg.FileName;
        }

        private void btnIndentity_Click(object sender, EventArgs e)
        {

            //构造Geoprocessor  
            Geoprocessor gp = new Geoprocessor();
            gp.OverwriteOutput = true;
            //设置参数  
          

            ILayer pInputFeaturelayer = GetLayerByName(pMapControl.Map, this.comboBoxEx1.SelectedItem.ToString());
            ILayer pIndentityFeaturelayer = GetLayerByName(pMapControl.Map, this.comboBoxEx2.SelectedItem.ToString());
            IFeatureClass pInputFeatureClass = (pInputFeaturelayer as IFeatureLayer).FeatureClass;
            IFeatureClass pIndentityFeatureClass = (pIndentityFeaturelayer as IFeatureLayer).FeatureClass;
            //ESRI.ArcGIS.AnalysisTools.Identity indentity = new ESRI.ArcGIS.AnalysisTools.Identity(pdic[this.comboBoxEx1.SelectedItem.ToString()], pdic[this.comboBoxEx2.SelectedItem.ToString()], this.textBoxX1.Text);
            //ESRI.ArcGIS.AnalysisTools.Identity indentity = new ESRI.ArcGIS.AnalysisTools.Identity((IFeatureLayer)pdicLayer[this.comboBoxEx1.SelectedItem.ToString()], (IFeatureLayer)pdicLayer[this.comboBoxEx2.SelectedItem.ToString()], this.textBoxX1.Text);
            //ESRI.ArcGIS.AnalysisTools.Identity indentity = new ESRI.ArcGIS.AnalysisTools.Identity(pInputFeatureClass, pIndentityFeatureClass, this.textBoxX1.Text);

            ESRI.ArcGIS.AnalysisTools.Identity indentity = new ESRI.ArcGIS.AnalysisTools.Identity();
            //设置参数  方法一：直接用完整路径加文件名和扩展名
            //indentity.in_features = pdic[this.comboBoxEx1.SelectedItem.ToString()];
            //indentity.identity_features = pdic[this.comboBoxEx2.SelectedItem.ToString()];
            //indentity.out_feature_class = this.textBoxX1.Text;
            //设置参数  方法二：直接用featureclass名称也行
            indentity.in_features = pInputFeatureClass;
            indentity.identity_features = pIndentityFeatureClass;
            indentity.out_feature_class = this.textBoxX1.Text;


            indentity.join_attributes = "ALL";

            //执行Intersect工具  

            object sev = null;
            try
            {
                // Execute the tool.
                gp.Execute(indentity, null);
                MessageBox.Show("标识叠加成功！");

            }
            catch (Exception ex)
            {
                // Print geoprocessing execution error messages.
                //     Return all of the message descriptions of the last tool executed.
                //显示全部信息：关键中的关键
                MessageBox.Show(gp.GetMessages(ref sev));
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 根据图层名称获取ILayer图层
        /// </summary>
        /// <param name="pMapControl"></param>
        /// <param name="LayerName"></param>
        /// <returns></returns>
        public ILayer GetLayerByName(IMap pMapControl, string LayerName)
        {
            ILayer pLayer = null;
            try
            {
                for (int i = 0; i < pMapControl.LayerCount; i++)
                {
                    if (pMapControl.get_Layer(i) is IGroupLayer)
                    {
                        ICompositeLayer pCompositeLayer = pMapControl.get_Layer(i) as ICompositeLayer;
                        for (int j = 0; j < pCompositeLayer.Count; j++)
                        {
                            if (pCompositeLayer.get_Layer(j).Name == LayerName)
                            {
                                pLayer = pCompositeLayer.get_Layer(j);
                                break;
                            }
                        }
                    }
                    else
                    {
                        if (pMapControl.get_Layer(i) is IFeatureLayer && pMapControl.get_Layer(i).Name == LayerName)
                        {
                            pLayer = pMapControl.get_Layer(i);
                            break;
                        }
                    }
                }
                return pLayer;
            }
            catch
            {
                MessageBox.Show("当前图层为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }
        }

    }
}
