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
    public partial class FrmLayerConfig : DevComponents.DotNetBar.Office2007Form
    {
        ClsXmlOperation xmlop;
        public ClsXmlOperation Xmlop
        {
            get { return xmlop; }
            set { xmlop = value; }
        }

        public FrmLayerConfig()
        {
            InitializeComponent();
        }
        public FrmLayerConfig(ClsXmlOperation xmlop): this()
        {
            this.xmlop = xmlop;
            LoadData();
            //LoadAcount();
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        private void LoadData()
        {
            ListViewItem myItem = new ListViewItem();

            listView1.Columns.Clear();
            listView1.Items.Clear();
            listView1.Columns.Add("图层名", 100, System.Windows.Forms.HorizontalAlignment.Left);
            listView1.Columns.Add("名称字段", 100, System.Windows.Forms.HorizontalAlignment.Left);
            listView1.Columns.Add("起始版本", 100, System.Windows.Forms.HorizontalAlignment.Left);
            listView1.Columns.Add("终止版本", 100, System.Windows.Forms.HorizontalAlignment.Left);
            listView1.Columns.Add("要素编码", 100, System.Windows.Forms.HorizontalAlignment.Left);
            listView1.Columns.Add("图层来源", 100, System.Windows.Forms.HorizontalAlignment.Left);
            listView1.Columns.Add("图层类型", 100, System.Windows.Forms.HorizontalAlignment.Left);
            listView1.Columns.Add("地理实体编码", 100, System.Windows.Forms.HorizontalAlignment.Left);
            listView1.Columns.Add("图层几何形状", 100, System.Windows.Forms.HorizontalAlignment.Left);
            //listView1.Columns.Add("Indext", 80, System.Windows.Forms.HorizontalAlignment.Left);

            //XmlNodeList nodelist = xmlop.GetTopElement("RecordList").ChildNodes;
            //XmlNodeList nodelist = xmlop.GetTopElement("LayerConfig").ChildNodes;
            XmlNodeList nodelist = xmlop.GetChildrenNodes("LayerConfig");
            if (nodelist.Count > 0)
            {
                //DropDownList1.Items.Clear();
                foreach (XmlElement nl in nodelist)//读元素值
                {
                    string[] array = new string[9];
                    array = Xmlop.GetInnerXMLValue(nl);
                    //DropDownList1.Items.Add(el.Attributes["key"].InnerXml);
                    //this.TextBox2.Text=el.Attributes["key"].InnerText;
                    myItem = listView1.Items.Add(array[0]);
                    for (int i = 1; i < array.Length; i++)
                    {
                        myItem.SubItems.Add(array[i]);
                    }
                    myItem.UseItemStyleForSubItems = true;
                    //MessageBox.Show(myItem.SubItems.Count.ToString());
                    //if (Convert.ToDouble(myItem.SubItems[3].Text) < 0)
                    //if (myItem.SubItems[1].Text == "收入/支出")
                    //    myItem.BackColor = (Convert.ToDouble(myItem.SubItems[3].Text) < 0) ? Color.LightGoldenrodYellow : Color.LightSeaGreen;
                    //else if (myItem.SubItems[1].Text == "借款/还款")
                    //    myItem.BackColor = Color.LimeGreen;
                    //同样在这里可以修改元素值,在后面save。
                    //  el.Attributes["value"].Value=this.TextBox2.Text;
                }
            }
        }
        /// <summary>
        /// 加载账户
        /// </summary>
        private void LoadAcount()
        {
            ListViewItem myItem = new ListViewItem();
            //listView2.Columns.Clear();
            //listView2.Items.Clear();
            //listView2.Columns.Add("账户类型", 120, System.Windows.Forms.HorizontalAlignment.Left);
            //listView2.Columns.Add("实时余额", 80, System.Windows.Forms.HorizontalAlignment.Right);

            XmlNodeList nodelista = xmlop.GetTopElement("Account").ChildNodes;
            XmlNodeList nodelistr = xmlop.GetTopElement("RecordList").ChildNodes;
            if (nodelista.Count > 0)
            {
                //DropDownList1.Items.Clear();

                foreach (XmlElement ela in nodelista)//读元素值
                {
                    String aname = ela.Attributes["AName"].Value;
                    Double anum = Convert.ToDouble(ela.Attributes["AStart"].Value);

                    myItem.SubItems.Add(ela.Attributes["AStart"].Value);
                    if (nodelistr.Count > 0)
                    {
                        foreach (XmlElement elr in nodelistr)//读元素值
                        {
                            if (elr.Attributes["From"].Value == aname)
                            {
                                anum += Convert.ToDouble(elr.Attributes["Amount"].Value);
                            }
                            if ((elr.Attributes["Type"].Value == "取存/转账") && (elr.Attributes["Project"].Value == aname))
                            {
                                anum -= Convert.ToDouble(elr.Attributes["Amount"].Value);
                            }

                        }

                    }
                    //myItem = listView2.Items.Add(aname);
                    myItem.SubItems.Add(string.Format("{0:N2}", (Convert.ToDouble(anum))));
                    myItem.UseItemStyleForSubItems = true;
                    //MessageBox.Show(myItem.SubItems.Count.ToString());
                    if (anum < 0)
                        myItem.ForeColor = Color.Red;
                }
            }
        }

        private void AddROpt()
        {
            FrmAddLayerConfig frm2 = new FrmAddLayerConfig(xmlop, (listView1.Items.Count + 1).ToString());
            frm2.Text = "添加记录";
            frm2.Savebt.Text = "添加";
            if (frm2.ShowDialog() == DialogResult.OK)
            {
                LoadData();
                //LoadAcount();
            }
        }
        private void EditROpt()
        {
            if (this.listView1.SelectedItems.Count != 1)
                MessageBox.Show("请选取一个账户操作");
            else
            {
                int k = this.listView1.SelectedIndices[0];
                Color clr = listView1.Items[k].BackColor;
                listView1.Items[k].BackColor = Color.LightGray;

                FrmAddLayerConfig frm2 = new FrmAddLayerConfig(xmlop, this.listView1.Items[k].SubItems[5].Text);
                frm2.Text = "修改记录";
                frm2.Savebt.Text = "修改";

                frm2.layerName.Text = this.listView1.Items[k].SubItems[0].Text;
                frm2.nameField.Text = this.listView1.Items[k].SubItems[1].Text;
                frm2.startVersion.Text = this.listView1.Items[k].SubItems[2].Text; ;
                frm2.endVersion.Text = this.listView1.Items[k].SubItems[3].Text;
                frm2.guid.Text = this.listView1.Items[k].SubItems[4].Text;
                frm2.sourceName.Text = this.listView1.Items[k].SubItems[5].Text;
                frm2.sourceType.Text = this.listView1.Items[k].SubItems[6].Text;
                frm2.entiID.Text = this.listView1.Items[k].SubItems[7].Text;
                frm2.shapeType.Text = this.listView1.Items[k].SubItems[8].Text;

                if (frm2.ShowDialog() == DialogResult.OK)
                {
                    LoadData();
                    //LoadAcount();
                    //listView2.Select();
                    this.listView1.Items[k].Selected = true;
                }
                else
                    listView1.Items[k].BackColor = clr;
            }
        }
        /// <summary>
        /// 删除节点
        /// </summary>
        private void DelROpt()
        {
            ListView.SelectedIndexCollection indexes = this.listView1.SelectedIndices;
            int k = indexes.Count;
            if (k == 0)
                MessageBox.Show("请选取记录进行删除操作");
            else
            {
                listView1.HideSelection = false;
                if (MessageBox.Show("你确认要删除选择的记录吗？", "删除警告", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    == System.Windows.Forms.DialogResult.Yes)
                {

                    //XmlElement element = xmlop.GetTopElement("RecordList");
                    XmlNode element = xmlop.GetTopNode("LayerConfig");
                    XmlNodeList nodelist = element.ChildNodes; 

                    ArrayList eldellist = new ArrayList();
                    foreach (int delindex in indexes)
                    {
                        eldellist.Add(nodelist[delindex]);
                    }
                    foreach (XmlElement el in eldellist)
                    {
                        element.RemoveChild(el);
                    }
                    //xmlop.Doc.Save("Data.xml");
                    xmlop.Doc.Save(xmlop.Xmlfile);
                    LoadData();
                    //LoadAcount();
                }
                listView1.HideSelection = true;
            }
        }

  

        /// <summary>
        /// 添加记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Addbt_Click(object sender, EventArgs e)
        {
            AddROpt();
        }
        /// <summary>
        /// 编辑记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Editbt_Click(object sender, EventArgs e)
        {
            EditROpt();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Delbt_Click(object sender, EventArgs e)
        {
            DelROpt();
        }
        
        private void MouseROpt(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                EditROpt();
            }
            else
                AddROpt();
        }

        private void KeyROpt(object sender, KeyPressEventArgs e)
        {
            //MessageBox.Show("1");
            if ((int)e.KeyChar == 32)
            {
                if (listView1.SelectedItems.Count == 1)
                    EditROpt();
                else
                    AddROpt();
            }
            else if ((int)e.KeyChar == 27 && listView1.SelectedItems.Count > 0)
                DelROpt();
        }
    }
}

