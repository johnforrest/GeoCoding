using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZJGISEntiTable.Classes
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
    public static class ClsConfig
    {
        public static Dictionary<string, LayerConfig> LayerConfigs = new Dictionary<string, LayerConfig>();
        public static Dictionary<string, string> SourceDatabase = new Dictionary<string, string>();

        static ClsConfig()
        {
            LayerConfigs.Add("PN_DM_G2", new LayerConfig()
            {
                LayerName = "PN_DM_G2",
                NameField = "兴趣点名称",
                StartVersion = "FROMVERSION",
                EndVersion = "TOVERSION",
                GUID = "ELEMID",
                SourceName = "地名地址库",
                SourceType = "A",
                EntityID = "ENTIID",
                ShapeType = "点",
            });
            LayerConfigs.Add("RES_CMU_PT_G2", new LayerConfig()
            {
                LayerName = "RES_CMU_PT_G2",
                NameField = "FNAME",
                StartVersion = "FROM_VERSION",
                //StartVersion = "FROMVERSION",
                EndVersion = "TO_VERSION",
                //EndVersion = "TOVERSION",
                GUID = "ELEMID",
                SourceName = "省库",
                SourceType = "B",
                EntityID = "ENTIID",
                ShapeType = "点",
            });
            LayerConfigs.Add("RES_PT_I_G2", new LayerConfig()
            {
                LayerName = "RES_PT_I_G2",
                NameField = "NAME",
                StartVersion = "FROMVERSION",
                EndVersion = "TOVERSION",
                GUID = "ELEMID",
                SourceName = "2000",
                SourceType = "C",
                EntityID = "ENTIID",
                ShapeType = "点",
            });
            LayerConfigs.Add("TRA_LN_I_G2", new LayerConfig()
            {
                LayerName = "TRA_LN_I_G2",
                NameField = "NAME",
                StartVersion = "FROMVERSION",
                EndVersion = "TOVERSION",
                GUID = "ELEMID",
                SourceName = "2000",
                SourceType = "C",
                EntityID = "ENTIID",
                ShapeType = "线",
            });
            LayerConfigs.Add("TRA_NET_LN_G2", new LayerConfig()
            {
                LayerName = "TRA_NET_LN_G2",
                NameField = "FNAME",
                StartVersion = "FROM_VERSION",
                //StartVersion = "FROMVERSION",
                EndVersion = "TO_VERSION",
                //EndVersion = "TOVERSION",
                GUID = "ELEMID",
                SourceName = "省库",
                SourceType = "B",
                EntityID = "ENTIID",
                ShapeType = "线",
            });
            LayerConfigs.Add("TRA_LN_I_G2_update", new LayerConfig()
            {
                LayerName = "TRA_LN_I_G2_update",
                NameField = "NAME",
                StartVersion = "FROMVERSION",
                EndVersion = "TOVERSION",
                GUID = "ELEMID",
                SourceName = "2000",
                SourceType = "C",
                EntityID = "ENTIID",
                ShapeType = "线",
            });
            LayerConfigs.Add("RES_PT_I_G2_update", new LayerConfig()
            {
                LayerName = "RES_PT_I_G2_update",
                NameField = "NAME",
                StartVersion = "FROMVERSION",
                EndVersion = "TOVERSION",
                GUID = "ELEMID",
                SourceName = "2000",
                SourceType = "C",
                EntityID = "ENTIID",
                ShapeType = "点",
            });
            
            SourceDatabase.Add("P", "地名地址库");
            SourceDatabase.Add("G", "10000");
            SourceDatabase.Add("K", "2000");
            SourceDatabase.Add("I", "2000");
        }

    }
}
