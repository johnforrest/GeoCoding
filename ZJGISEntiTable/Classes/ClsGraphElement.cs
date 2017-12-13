using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;

namespace ZJGISEntiTable.Classes
{
    public class ClsGraphElement
    {
        private DateTime _versionTime;

        public DateTime VersionTime
        {
            get { return _versionTime; }
            set { _versionTime = value; }
        }

        private DateTime _endVersionTime;

        public DateTime EndVersionTime
        {
            get { return _endVersionTime; }
            set { _endVersionTime = value; }
        }

        private string _version;

        public string Version
        {
            get { return _version; }
            set { _version = value; }
        }
        

        private string _source;

        public string Source
        {
            get { return _source; }
            set { _source = value; }
        }

        private IFeature _feature;

        public IFeature Feature
        {
            get { return _feature; }
            set { _feature = value; }
        }

        private string _layerName;

        public string LayerName
        {
            get { return _layerName; }
            set { _layerName = value; }
        }
    }

    public class ClsGraphElements
    {
        private List<ClsGraphElement> _graphElements = new List<ClsGraphElement>();

        public List<ClsGraphElement> GraphElementsLst
        {
            get { return _graphElements; }
            set { _graphElements = value; }
        }

        public List<ClsGraphElement> GraphElementSort
        {
            get
            {
                List<ClsGraphElement> graphElementsSort = new List<ClsGraphElement>();
                for (int i = 0; i < this._graphElements.Count; i++)
                {
                    graphElementsSort.Add(this._graphElements[i]);
                }

                for (int i = 0; i < graphElementsSort.Count - 1; i++)
                {
                    for (int j = i + 1; j < graphElementsSort.Count; j++)
                    {
                        if (graphElementsSort[i].VersionTime > graphElementsSort[j].VersionTime)
                        {
                            ClsGraphElement temp = graphElementsSort[i];
                            graphElementsSort[i] = graphElementsSort[j];
                            graphElementsSort[j] = temp;
                        }
                    }
                }

                return graphElementsSort;
            }
        }

        public Dictionary<DateTime, List<ClsGraphElement>> EntityElements
        {
            get
            {
                Dictionary<DateTime, List<ClsGraphElement>> result = new Dictionary<DateTime, List<ClsGraphElement>>();
                for (int i = 0; i < GraphElementSort.Count; i++)
                {
                    DateTime versionTime = GraphElementSort[i].VersionTime;
                    for (int j = 0; j < GraphElementSort.Count; j++)
                    {
                        DateTime endTime = GraphElementSort[j].EndVersionTime;
                        if (!result.ContainsKey(versionTime))
                        {
                            result.Add(versionTime, new List<ClsGraphElement>());
                        }
                        if (endTime > versionTime)
                        {
                            result[versionTime].Add(GraphElementSort[i]);
                        }
                    }
                }
                return result;
            }
        }

        public void Clear()
        {
            this._graphElements.Clear();
        }

        public void Add(ClsGraphElement ge)
        {
            this._graphElements.Add(ge);
        }
    }
}
