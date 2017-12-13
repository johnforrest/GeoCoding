using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geoprocessor;

namespace ZJGISDataMatch.Classes
{
    class ClsIndentity
    {
        public string ReturnshpPath(string layername)
        {
            // string returnPath = null;

            SaveFileDialog saveDlg = new SaveFileDialog();
            saveDlg.CheckPathExists = true;
            saveDlg.Filter = "Shapefile (*.shp)|*.shp|File Geodatabase (*.gdb)|*.gdb";
            saveDlg.OverwritePrompt = true;
            saveDlg.Title = "Output Layer";
            saveDlg.RestoreDirectory = true;
            saveDlg.FileName = layername + "_Indentity.shp";

            // returnPath = saveDlg.FileName;
            return saveDlg.FileName;
            //DialogResult dr = saveDlg.ShowDialog();
            //if (dr == DialogResult.OK)
            //    textBoxX2.Text = saveDlg.FileName;
        }

        public void ReturnIndentityFeatureClass(IFeatureClass pInputFeatureClass, IFeatureClass pIndentityFeatureClass,string savePath)
        {
            //构造Geoprocessor  
            Geoprocessor gp = new Geoprocessor();
            gp.OverwriteOutput = true;
            //设置参数  
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
            indentity.out_feature_class = savePath;

            indentity.join_attributes = "ALL";

            //执行Intersect工具  
            object sev = null;
            try
            {
                gp.Execute(indentity, null);

            }
            catch (Exception ex)
            {
                MessageBox.Show(gp.GetMessages(ref sev));
            }
        }
    }
}
