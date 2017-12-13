using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Collections;

namespace ZJGISXMLOperation.Forms
{
    public partial class FrmAddLayerConfig : DevComponents.DotNetBar.Office2007Form
    {
        ClsXmlOperation xmlop;
        String editaname = "";
        public ClsXmlOperation Xmlop
        {
            get { return xmlop; }
            set { xmlop = value; }
        }
        Object[] obt;
        public FrmAddLayerConfig()
        {
            InitializeComponent();
        }
        public FrmAddLayerConfig(ClsXmlOperation xmlop)
        {
            InitializeComponent();

            this.xmlop = xmlop;
            obt = xmlop.GetAttriList("Account", "AName");
            InitializeData();
        }
        public FrmAddLayerConfig(ClsXmlOperation xmlop, String editaname)
        {
            InitializeComponent();
            InitializeData();
            this.xmlop = xmlop;
            this.editaname = editaname;
        }
        private void InitializeData()
        {
            sourceName.Items.Add(10000);
            sourceName.Items.Add(2000);
            sourceName.Items.Add(500);
            sourceName.Items.Add("地名地址库");
            this.sourceName.SelectedIndex = 0;

            sourceType.Items.Add("I");
            sourceType.Items.Add("K");
            sourceType.Items.Add("G");
            sourceType.Items.Add("p");
            this.sourceType.SelectedIndex = 0;

            shapeType.Items.Add("点");
            shapeType.Items.Add("线");
            shapeType.Items.Add("面");
            this.shapeType.SelectedIndex = 0;
        }

        private void Savebt_Click(object sender, EventArgs e)
        {
            String inedxvalue = (layerName.Text).Replace("-", "");
            String[] attrinamelist = new string[] { "layerName", "nameField", "startVersion", "endVersion", 
                "guid", "sourceName", "sourceType", "entiID", "shapeType" };
            String[] attrivaluelist = new string[] { layerName.Text, nameField.Text, startVersion.Text, endVersion.Text,
                guid.Text, sourceName.Text,sourceType.Text,entiID.Text,shapeType.Text};


            if (Savebt.Text == "添加")
                xmlop.InsetAfterEl("LayerConfig", layerName.Text, attrinamelist, attrivaluelist);
            //xmlop.InsetAfterEl("RecordList", "r", "Date", layerName.Text, attrinamelist, attrivaluelist);
            else if (Savebt.Text == "修改")
                xmlop.AttriValueEdit("LayerConfig", layerName.Text, attrinamelist, attrivaluelist);
            #region 以前注释
            ////装入指定的XML文档
            //XmlDocument doc = new XmlDocument();
            //doc.Load("Data.xml");
            //XmlNodeList topM = doc.DocumentElement.ChildNodes;
            //foreach (XmlElement element in topM)
            //{
            //    if (element.Name.ToLower() == "recordlist")
            //    {
            //        //XmlElement el = doc.CreateElement("r"+((element.ChildNodes.Count==0)?"0":(Convert.ToInt32(element.LastChild.Name.Substring(1)) + 1).ToString()));
            //        XmlElement el = doc.CreateElement("r");
            //        el.SetAttribute("Date", Datetb.Text);
            //        el.SetAttribute("Type", Typecbx.Text);
            //        el.SetAttribute("From", Fromcbx.Text);
            //        el.SetAttribute("Amount", string.Format("{0:N2}", (Convert.ToDouble((Amounttb.Text == "") ? "0" : Amounttb.Text))));

            //        el.SetAttribute("Project", Projectcbx.Text);
            //        el.SetAttribute("Index", ((element.ChildNodes.Count + 1).ToString()));
            //        //element2.InnerText = "？？？";
            //        element.AppendChild(el);
            //    }
            //}
            //doc.Save("Data.xml");
            #endregion
        }

        private void Typecbx_TextChanged(object sender, EventArgs e)
        {
            //sourceName.Items.Clear();
            //if (Typecbx.Text == "取存/转账")
            //{
            //    sourceName.Items.AddRange(obt);
            //    sourceName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            //}
            //else
            //    sourceName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            //endVersion.Select();
        }

        private void Cancelbt_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}
