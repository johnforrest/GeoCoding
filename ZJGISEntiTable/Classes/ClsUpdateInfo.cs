using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZJGISEntiTable.Classes
{
    public class ClsUpdateInfo
    {
        public string UpdateState;
        public string GUID;
        public IFeature Feature;
        public IFeatureLayer ToUpdateLyr;
        public IFeatureLayer UpdatedLyr;
    }
}
