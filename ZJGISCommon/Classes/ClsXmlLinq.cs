using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ZJGISCommon.Classes
{
    public class ClsXmlLinq
    {
        public static void LoadXml(string path, string namewithoutExtention)
        {
            //XElement xe = XElement.Load(@"..\..\Book.xml");
            XElement xe = XElement.Load(path);
            //IEnumerable<XElement> elements = from ele in xe.Elements("book")
            IEnumerable<XElement> elements = from ele in xe.Elements(namewithoutExtention)
                                             select ele;
            ReadXmlByElements(elements);
        }

        public static List<LayerConfig> ReadXmlByElements(IEnumerable<XElement> elements)
        {
            List<LayerConfig> LayerList = new List<LayerConfig>();
            foreach (var ele in elements)
            {
                LayerConfig layer = new LayerConfig();

                layer.LayerName = ele.Element("layerName").Value;
                //或者用下面这个也可以
                //layer.LayerName = ele.Attribute("name").Value;
                layer.NameField = ele.Element("nameField").Value;
                layer.StartVersion = ele.Element("startVersion").Value;
                layer.EndVersion = ele.Element("endVersion").Value;
                layer.GUID = ele.Element("guid").Value;
                layer.SourceName = ele.Element("sourceName").Value;
                layer.SourceType = ele.Element("sourceType").Value;
                layer.EntityID = ele.Element("entiID").Value;
                layer.ShapeType = ele.Element("shapeType").Value;

                LayerList.Add(layer);
            }
            return LayerList;
            //dgvBookInfo.DataSource = LayerList;
        }

        
    }
}
