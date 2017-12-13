using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;

namespace ZJGISDataMatch.Classes
{
    public static class ClsCheck
    {
        public static bool CheckNullField(ILayer pLayer,string pField)
        {
            int i = 0,j=0;
            IFeatureLayer pFeatureLayer = null;
            if (pLayer is IFeatureLayer)
            {
                pFeatureLayer = pLayer as IFeatureLayer;
                IDataset pDataset = pFeatureLayer.FeatureClass as IDataset;
                IWorkspaceEdit pWorkspaceEdit = null;
                if (pDataset != null)
                {
                    pWorkspaceEdit = pDataset.Workspace as IWorkspaceEdit;
                    if (pWorkspaceEdit != null || pWorkspaceEdit.IsBeingEdited() == false)
                    {
                        pWorkspaceEdit.StartEditing(true);
                        pWorkspaceEdit.StartEditOperation();
                    }

                    IFeatureCursor pFeatureCursor = pFeatureLayer.Search(new QueryFilterClass(), false);
                    IFeature pFeature = pFeatureCursor.NextFeature();

                    
                   i = pFeatureLayer.FeatureClass.FeatureCount(null);

                    while (pFeature != null)
                    {
                        //test
                        int index = pFeature.Fields.FindField(pField);
                        //test
                        int test = pFeature.get_Value(index).ToString().Trim().Length;
                        if (pFeature.get_Value(index).ToString().Trim().Length==0)
                        {
                            j++;
                        }

                        pFeature = pFeatureCursor.NextFeature();
                    }

                    pWorkspaceEdit.StopEditing(true);
                    pWorkspaceEdit.StopEditOperation();
                }
            }


            if (j > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
           

        }
    }
}
