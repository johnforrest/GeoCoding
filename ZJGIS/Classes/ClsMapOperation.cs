using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.SystemUI;
//using Codes = XCGIS.Codes;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.DataSourcesRaster;

namespace ZJGIS.Class
{
    class ClsMapOperation
    {
        public void ZoomIn(AxMapControl mapControl, IEnvelope pEnvelope)
        {
            if (pEnvelope == null || pEnvelope.IsEmpty || pEnvelope.Height == 0 || pEnvelope.Width == 0)
            {
                return;
            }
            mapControl.ActiveView.Extent = pEnvelope;
            mapControl.ActiveView.Refresh();
        }
        public void ZoomOut(AxMapControl mapControl, IEnvelope pEnvelope)
        {
            //如果拉框范围为空则退出
            if (pEnvelope == null || pEnvelope.IsEmpty || pEnvelope.Height == 0 || pEnvelope.Width == 0)
            {
                return;
            }
            //如果有拉框范围，则以拉框范围为中心，缩小倍数为：当前视图范围/拉框范围
            else
            {
                double dWidth = mapControl.ActiveView.Extent.Width * mapControl.ActiveView.Extent.Width / pEnvelope.Width;
                double dHeight = mapControl.ActiveView.Extent.Height * mapControl.ActiveView.Extent.Height / pEnvelope.Height;
                double dXmin = mapControl.ActiveView.Extent.XMin -
                               ((pEnvelope.XMin - mapControl.ActiveView.Extent.XMin) * mapControl.ActiveView.Extent.Width /
                                pEnvelope.Width);
                double dYmin = mapControl.ActiveView.Extent.YMin -
                               ((pEnvelope.YMin - mapControl.ActiveView.Extent.YMin) * mapControl.ActiveView.Extent.Height /
                                pEnvelope.Height);
                double dXmax = dXmin + dWidth;
                double dYmax = dYmin + dHeight;
                pEnvelope.PutCoords(dXmin, dYmin, dXmax, dYmax);
            }
            mapControl.ActiveView.Extent = pEnvelope;
            mapControl.ActiveView.Refresh();
        }
        public void Pan(AxMapControl mapControl)
        {
            mapControl.CurrentTool = null;
            ICommand pCommand = new ESRI.ArcGIS.Controls.ControlsMapPanToolClass();
            pCommand.OnCreate(mapControl.Object);
            mapControl.CurrentTool = pCommand as ITool;

        }
        public void SwipeView(AxMapControl mapControl)
        {
            //    ILayerEffectProperties pEffectLayer = new CommandsEnvironmentClass();
            //    for (int i= 0; i< mapControl.LayerCount; i++)
            //    {
            //        if(mapControl.get_Layer(i).Visible==true||i == mapControl.LayerCount-1)
            //        {
            //            pEffectLayer.SwipeLayer = mapControl.get_Layer(i);//设置卷帘图层
            //            break;
            //        }
            //    }
            mapControl.CurrentTool = null;
            mapControl.MousePointer = esriControlsMousePointer.esriPointerDefault;
            ICommand cmd = new ControlsMapSwipeToolClass();//调用卷帘工具
            cmd.OnCreate(mapControl.Object);
            mapControl.CurrentTool = cmd as ITool;
        }
        public void Flicker(AxMapControl mapcontrol)
        {
            mapcontrol.CurrentTool = null;
            mapcontrol.MousePointer = esriControlsMousePointer.esriPointerDefault;
            ICommand pcmd = new ControlsMapFlickerCommand();
            pcmd.OnCreate(mapcontrol.Object);
            mapcontrol.CurrentTool = pcmd as ITool;
            pcmd.OnClick();
        }
    }
}
