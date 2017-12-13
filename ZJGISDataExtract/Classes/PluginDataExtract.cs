using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PluginFramework;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Rendering;
using System.Windows.Forms;
using ZJGISDataExtract.Properties;

namespace ZJGISDataExtract
{
    class PluginDataExtract:IPlugin
    {
        private IApplication application = null;
        public IApplication Application
        {
            get { return application; }
            set { application = value; }
        }
        public void Load()
        {
            RibbonControl ribbonControl = application.RibbonControlMain;
            foreach (RibbonTabItem Tabltem in ribbonControl.Items)
            {
                if (Tabltem.Text == "数据提取")
                {
                    RibbonPanel panel = Tabltem.Panel;                
                        //新建一个RibbonBar，加载到数据提取中
                        RibbonBar barExtract = new RibbonBar();
                        barExtract.Text = "数据提取";
                        //添加按钮
                        ButtonItem btnExtractByPolygon = new ButtonItem();
                        btnExtractByPolygon.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
                        btnExtractByPolygon.Image = Resources.self;
                        btnExtractByPolygon.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top;

                        btnExtractByPolygon.Text = "自定义范围";
                        btnExtractByPolygon.Tooltip = "地图画自定义范围";
                        btnExtractByPolygon.Click += new EventHandler(btnExtractByPolygon_Click);

                        ButtonItem btnLayerExtract = new ButtonItem();
                        btnLayerExtract.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
                        btnLayerExtract.Image = Resources.图层;
                        btnLayerExtract.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top;
                        btnLayerExtract.Text = "图层提取";
                        btnLayerExtract.Tooltip = "提取单个或多个图层";
                        btnLayerExtract.Click+=new EventHandler(btnLayerExtract_Click);

                        ButtonItem btnAreaExtract = new ButtonItem();
                        btnAreaExtract.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
                        btnAreaExtract.Image = Resources.区域;
                        btnAreaExtract.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top;
                        btnAreaExtract.Text = "行政区域提取";
                        btnAreaExtract.Tooltip = "根据行政区范围提取";
                        btnAreaExtract.Click += new EventHandler(btnAreaExtract_Click);

                        ButtonItem btnMapExtract = new ButtonItem();
                        btnMapExtract.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
                        btnMapExtract.Image = Resources.图幅;
                        btnMapExtract.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top;
                        btnMapExtract.Text = "图幅提取";
                        btnMapExtract.Tooltip = "根据图幅范围提取";
                        btnMapExtract.Click += new EventHandler(btnMapExtract_Click);

                        barExtract.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] { btnExtractByPolygon, btnLayerExtract, btnAreaExtract, btnMapExtract });
                        panel.Controls.Add(barExtract);                   
                    break;
                }
            }
        }
    
        void btnExtractByPolygon_Click(object sender, EventArgs e)//自定范围提取
        {
            PolygonExtract polygonExtract = new PolygonExtract();
            polygonExtract.OnCreate(application.MapControMain.Object);
            application.MapControMain.CurrentTool = polygonExtract;

        }
        void btnLayerExtract_Click(object sender, EventArgs e)//图层提取
        {
            LayerExtract layerExtract = new LayerExtract();
            layerExtract.OnCreate(application.MapControMain.Object);
            application.MapControMain.CurrentTool = layerExtract;
        }
        void btnAreaExtract_Click(object sender, EventArgs e)//行政区提取
        {
            RegionExtract regionExtract = new RegionExtract();
            regionExtract.OnCreate(application.MapControMain.Object);
            application.MapControMain.CurrentTool = regionExtract;
        }
        void btnMapExtract_Click(object sender, EventArgs e)//提取到工作层
        {
            //throw new NotImplementedException();
        }
        public void UnLoad()
        {
             RibbonControl ribbonControl = application.RibbonControlMain;
             foreach (RibbonTabItem Tabltem in ribbonControl.Items)
             {
                 if (Tabltem.Text == "数据提取")
                 {
                     RibbonPanel panel = Tabltem.Panel;
                     RibbonBar ribbonBar;
                     foreach(Control panelControl in panel.Controls )
                     {
                         ribbonBar = panelControl as RibbonBar;
                         panel.Controls.Remove(ribbonBar);
                     }
                     break;
                 }
             }
        }
       
    }
}
