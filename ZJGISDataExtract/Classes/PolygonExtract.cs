using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using System.Windows.Forms;
using ESRI.ArcGIS.Geometry;
using System.Collections;
using System.Resources;
using System.Reflection;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using DevComponents.DotNetBar;

namespace ZJGISDataExtract
{
    public sealed class PolygonExtract : BaseTool
    {
      private IHookHelper _hookHelper;
      private IPolygon _polygon;
      private static IElement _element;
      private FrmPolygonExtract _frmPolygonExtract;

      public PolygonExtract()
      {
      }

      public override void OnCreate(object hook)
      {
          try
          {
              _hookHelper = new HookHelper();
              _hookHelper.Hook = hook;
              if (_hookHelper.ActiveView == null)
              {
                  _hookHelper = null;
              }
          }
          catch
          {
              _hookHelper = null;
          }
          if (_hookHelper == null)
              base.m_enabled = false;
          else
              base.m_enabled = true;
      }
      public override void OnClick()
      {
          if (_hookHelper != null)
          {
              if (_hookHelper.FocusMap.LayerCount == 0)
              {
                  MessageBoxEx.Show("没有加载图层或图层没有选中状态，请选中需要提取的图层！", "提示");
                  IMapControl4 mapControl = _hookHelper.Hook as IMapControl4;
                  mapControl.CurrentTool = null;
                  return;
              }
              if (_frmPolygonExtract == null)
              {
                  _frmPolygonExtract = new FrmPolygonExtract();
              }
              else
                  return;
          }
      }

        /// <summary>
        /// 获得自定义多边形
        /// </summary>
        /// <param name="Button"></param>
        /// <param name="Shift"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
      public override void OnMouseDown(int Button, int Shift, int X, int Y)
      {
          IMapControl4 mapControl = null;
          if (_hookHelper != null)
          {
              mapControl = (IMapControl4)_hookHelper.Hook;
              _polygon = (IPolygon)mapControl.TrackPolygon();
          }
          if (_polygon != null)
          {
              if (_polygon.IsEmpty == false)
              {
                  IMap map = _hookHelper.FocusMap;
                  IGraphicsContainer graphicsContainer = (IGraphicsContainer)map;
                  IActiveView activeView = (IActiveView)map;
                  if (_element != null)
                  {
                      graphicsContainer.DeleteElement(_element);
                      activeView.Refresh();
                  }

                  IRgbColor color = new RgbColor();
                  color.Red = 255;
                  color.Green = 255;
                  color.Blue = 0;

                  IMarkerLineSymbol markerLineSymbol = new MarkerLineSymbol();
                  markerLineSymbol.Color = color;
                  markerLineSymbol.Width = 2;

                  ISimpleFillSymbol simpleFillSym = new SimpleFillSymbol();
                  simpleFillSym.Style = esriSimpleFillStyle.esriSFSHollow;
                  simpleFillSym.Outline = markerLineSymbol;

                  IFillShapeElement polygonElement = new PolygonElementClass();
                  polygonElement.Symbol = simpleFillSym;

                  _element = (IElement)polygonElement;
                  _element.Geometry = _polygon;

                  graphicsContainer.AddElement(_element, 0);
                  activeView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);

                  //传值
                  if (_frmPolygonExtract.IsDisposed)
                      _frmPolygonExtract = new FrmPolygonExtract();
                  _frmPolygonExtract.Map =_hookHelper.FocusMap;
                  _frmPolygonExtract.MapControl = (IMapControl4)_hookHelper.Hook;
                  _frmPolygonExtract.Geometry = _polygon;
                  _frmPolygonExtract.Element = _element;
                  _frmPolygonExtract.ShowDialog();
              }
              else
              {
                  MessageBoxEx.Show("画出的几何图象是空的！", "提示");
              }
          }
          else
          {
              return;
          }
        mapControl.CurrentTool = null;
          _element = null;
      }



    }
}
