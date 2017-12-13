using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;

namespace ZJGISCommon.Classes
{
    public class LayerConfig
    {
        public string LayerName;
        //待填充实体表的7个字段
        public string NameField;
        public string StartVersion;
        public string EndVersion;
        public string GUID;
        public string SourceName;
        public string SourceType;
        public string EntityID;

        public string ShapeType;
    }

    #region 亮哥写死的代码
    //public static class ClsConfig
    //{
    //    public static Dictionary<string, LayerConfig> LayerConfigs = new Dictionary<string, LayerConfig>();
    //    public static Dictionary<string, string> SourceDatabase = new Dictionary<string, string>();
    //    static ClsConfig()
    //    {
    //        LayerConfigs.Add("PN_DM_G2", new LayerConfig()
    //        {
    //            LayerName = "PN_DM_G2",
    //            NameField = "兴趣点名称",
    //            StartVersion = "FROMVERSION",
    //            EndVersion = "TOVERSION",
    //            GUID = "ELEMID",
    //            SourceName = "地名地址库",
    //            SourceType = "A",
    //            EntityID = "ENTIID",
    //            ShapeType = "点",
    //        });
    //        LayerConfigs.Add("RES_CMU_PT_G2", new LayerConfig()
    //        {
    //            LayerName = "RES_CMU_PT_G2",
    //            NameField = "FNAME",
    //            StartVersion = "FROM_VERSION",
    //            //StartVersion = "FROMVERSION",
    //            EndVersion = "TO_VERSION",
    //            //EndVersion = "TOVERSION",
    //            GUID = "ELEMID",
    //            SourceName = "省库",
    //            SourceType = "B",
    //            EntityID = "ENTIID",
    //            ShapeType = "点",
    //        });
    //        LayerConfigs.Add("RES_PT_I_G2", new LayerConfig()
    //        {
    //            LayerName = "RES_PT_I_G2",
    //            NameField = "NAME",
    //            StartVersion = "FROMVERSION",
    //            EndVersion = "TOVERSION",
    //            GUID = "ELEMID",
    //            SourceName = "2000",
    //            SourceType = "C",
    //            EntityID = "ENTIID",
    //            ShapeType = "点",
    //        });
    //        LayerConfigs.Add("TRA_LN_I_G2", new LayerConfig()
    //        {
    //            LayerName = "TRA_LN_I_G2",
    //            NameField = "NAME",
    //            StartVersion = "FROMVERSION",
    //            EndVersion = "TOVERSION",
    //            GUID = "ELEMID",
    //            SourceName = "2000",
    //            SourceType = "C",
    //            EntityID = "ENTIID",
    //            ShapeType = "线",
    //        });
    //        LayerConfigs.Add("TRA_NET_LN_G2", new LayerConfig()
    //        {
    //            LayerName = "TRA_NET_LN_G2",
    //            NameField = "FNAME",
    //            StartVersion = "FROM_VERSION",
    //            //StartVersion = "FROMVERSION",
    //            EndVersion = "TO_VERSION",
    //            //EndVersion = "TOVERSION",
    //            GUID = "ELEMID",
    //            SourceName = "省库",
    //            SourceType = "B",
    //            EntityID = "ENTIID",
    //            ShapeType = "线",
    //        });
    //        LayerConfigs.Add("TRA_LN_I_G2_update", new LayerConfig()
    //        {
    //            LayerName = "TRA_LN_I_G2_update",
    //            NameField = "NAME",
    //            StartVersion = "FROMVERSION",
    //            EndVersion = "TOVERSION",
    //            GUID = "ELEMID",
    //            SourceName = "2000",
    //            SourceType = "C",
    //            EntityID = "ENTIID",
    //            ShapeType = "线",
    //        });
    //        LayerConfigs.Add("RES_PT_I_G2_update", new LayerConfig()
    //        {
    //            LayerName = "RES_PT_I_G2_update",
    //            NameField = "NAME",
    //            StartVersion = "FROMVERSION",
    //            EndVersion = "TOVERSION",
    //            GUID = "ELEMID",
    //            SourceName = "2000",
    //            SourceType = "C",
    //            EntityID = "ENTIID",
    //            ShapeType = "点",
    //        });

    //        LayerConfigs.Add("POI", new LayerConfig()
    //        {
    //            LayerName = "POI",
    //            NameField = "NAME",
    //            StartVersion = "FROMVERSION",
    //            EndVersion = "TOVERSION",
    //            GUID = "ELEMID",
    //            SourceName = "地名地址库",
    //            SourceType = "A",
    //            EntityID = "ENTIID",
    //            ShapeType = "点",
    //        });
    //        LayerConfigs.Add("RES_PT_I", new LayerConfig()
    //        {
    //            LayerName = "RES_PT_I",
    //            NameField = "NAME",
    //            StartVersion = "FROMVERSION",
    //            EndVersion = "TOVERSION",
    //            GUID = "ELEMID",
    //            SourceName = "2000",
    //            SourceType = "C",
    //            EntityID = "ENTIID",
    //            ShapeType = "点",
    //        });

    //        SourceDatabase.Add("A", "地名地址库");
    //        SourceDatabase.Add("B", "省库");
    //        SourceDatabase.Add("C", "2000");
    //    }

    //}
    #endregion
    /// <summary>
    /// 20170624自己修改的代码
    /// </summary>
    public static class ClsConfig
    {
        public static Dictionary<string, LayerConfig> LayerConfigs = new Dictionary<string, LayerConfig>();
        public static Dictionary<string, string> SourceDatabase = new Dictionary<string, string>();
        //20170624
        public static List<LayerConfig> pLayerList = new List<LayerConfig>();
        public static string path = null;

        static ClsConfig()
        {
            //string path = new DirectoryInfo("../").FullName + @"Res\xml\Layer.xml";
            path = new DirectoryInfo("../").FullName + @"Res\xml\Layer.xml";
            XElement xe = XElement.Load(path);
            IEnumerable<XElement> elements = from ele in xe.Elements("layer")
                                             select ele;
            pLayerList = ClsXmlLinq.ReadXmlByElements(elements);
            if (pLayerList != null)
            {
                foreach (LayerConfig plc in pLayerList)
                {
                    if (!LayerConfigs.ContainsKey(plc.LayerName))
                    {
                    	LayerConfigs.Add(plc.LayerName, plc);
                    }
                }
            }
           

            //SourceDatabase.Add("A", "地名地址库");
            //SourceDatabase.Add("B", "省库");
            //SourceDatabase.Add("C", "2000");

            SourceDatabase.Add("P", "地名地址库");
            SourceDatabase.Add("K", "500");
            SourceDatabase.Add("I", "2000");
            SourceDatabase.Add("G", "10000");
        }

    }

}
