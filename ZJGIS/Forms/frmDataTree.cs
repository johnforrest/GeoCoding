using System.Collections.Generic;
using System.Drawing;
using System;
using System.Collections;
using System.Diagnostics;
using System.Data;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Rendering;
using System.Xml;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using ZJGISLayerManager;
using ZJGISLayerManager.Classes;
using ZJGISCommon;


namespace ZJGIS
{
	public partial class frmDataTree
	{


        public ILayer pLayer = null;  //作为最终希望获取的那个目标图层

        //toc右键菜单
        private ClsTOCContextMenu m_tocContextMenu;
      

		private IMapControl4 m_MapMain;
		private object m_pBuddyControl;
        private Form FrmMain;

        public Form FrmActive
        {
            get
            {
                return FrmMain;
            }
            set
            {
                FrmMain = value;
            }
        }
		public IMapControl4 MapMain
		{
			get
			{
				return m_MapMain;
			}
			set
			{
				m_MapMain = value;
			}
		}
		
		public object BuddyControl
		{
            get
            {
                return m_pBuddyControl;
            }

			set
			{
				m_pBuddyControl = value;
			}
		}
		
		public frmDataTree()
		{
			InitializeComponent();
		
			this.SetTopLevel(false);
		}
		
		~frmDataTree()
		{
			
		}
		
		private void frmDataTree_Disposed(object sender, System.EventArgs e)
		{
			
		}
		
		private void frmDataTree_Load(object sender, System.EventArgs e)
		{
			
			this.TOCLayer.SetBuddyControl(m_pBuddyControl); //设置Toc的Buddy
            try
            {
                //初始化右键toc类
                if (m_tocContextMenu == null)
                {
                    AxTOCControl pAxTOCControl = TOCLayer as AxTOCControl;
                    m_tocContextMenu = new ClsTOCContextMenu(pAxTOCControl, MapMain, FrmMain);
                   
                }
             
            }
            catch (Exception )
            {
                
            }
				
		}
		
		
		private void TOCLayer_OnDoubleClick(object sender, ITOCControlEvents_OnDoubleClickEvent e)
		{
			if (e.button != 1)
			{
				return;
			}
            esriTOCControlItem itemType = esriTOCControlItem.esriTOCControlItemNone;
            IBasicMap pBasicMap = null;
            ILayer pLayer = null;
            object unk = null;
            object data = null;
            TOCLayer.HitTest(e.x, e.y, ref itemType, ref pBasicMap, ref pLayer, ref unk, ref data);
            if (e.button == 1)
            {
                if (itemType == esriTOCControlItem.esriTOCControlItemLegendClass)
                {
                    ILegendClass pLegendClass = ((ILegendGroup)unk).get_Class((int)data);

                    FrmSymbolSelect SymbolSelectorFrm = new FrmSymbolSelect(pLegendClass, pLayer);
                    if (SymbolSelectorFrm.ShowDialog() == DialogResult.OK)
                    {
                        m_MapMain.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
                        pLegendClass.Symbol = SymbolSelectorFrm.pSymbol;
                        m_MapMain.ActiveView.Refresh();
                        TOCLayer.Refresh();
                    }
                }
            }
		}

        private void TOCLayer_OnMouseDown(object sender, ITOCControlEvents_OnMouseDownEvent e)
        {
            if (TOCLayer != null)
            {
                esriTOCControlItem pItem = esriTOCControlItem.esriTOCControlItemNone;
                IBasicMap pBasicMap = null;
                //ILayer pLayer = null;
                object pOther = null;
                object pIndex = null;

                this.TOCLayer.MousePointer = esriControlsMousePointer.esriPointerArrow;

                TOCLayer.HitTest(e.x, e.y, ref pItem, ref pBasicMap, ref pLayer, ref pOther, ref pIndex);

                ////实现调整图层顺序功能
                //if ((e.button == 1))
                //{
                //    return;
                //}
                //else
                //{
                //    if (pItem == esriTOCControlItem.esriTOCControlItemNone)
                //    {
                //        return;
                //    }
                //    //确定所选中的信息
                //    this.TOCLayer.GetSelectedItem(ref pItem, ref pBasicMap, ref pLayer, ref pOther, ref pIndex);
                //    //弹出相应的快捷菜单 根节点 子节点
                //    if ((pItem == esriTOCControlItem.esriTOCControlItemMap))
                //    {
                //        m_pMapMenu.PopupMenu(e.x, e.y, TOCLayer.hWnd);

                //    }
                //    else if ((pItem == esriTOCControlItem.esriTOCControlItemLayer))
                //    {
                //        if (pLayer is IGroupLayer)
                //        {
                //            return;
                //        }
                //        if (pLayer is ICompositeLayer)
                //        {
                //            return;
                //        }

                //        //m_Symbol.Layer = pLayer;
                //        //移除图层
                //        //m_RemoveLayer.Layer = pLayer;
                //        //m_RemoveLayer.Enabled2 = true;
                //        //m_LayerAttribute.Enabled2 = true;
                //        //一些图层是业务功能图层，不能删除
                //        //if (pLayer.Name.Contains(g_DY) | pLayer.Name.Contains(g_BDZ) | pLayer.Name.Contains(g_GT) | pLayer.Name.Contains(g_XL))
                //        //{
                //        //    m_RemoveLayer.Enabled2 = false;
                //        //}

                //        //m_LayerAttribute.Layer = pLayer;
                //        ////查看属性表

                //        //InitDataEditEnvironment(pLayer);
                //        ////基础下图层属性表不看查看

                //        //m_LayerAttribute.FrmMap = m_FrmMap;
                //        //m_LayerSelectable1.Layer = pLayer;
                //        //m_LayerSelectable2.Layer = pLayer;
                //        //m_ZoomToLayer.Layer = pLayer;
                //        ////缩放至图层
                //        //m_SetLabel.Layer = pLayer;
                //        ////设置标注
                //        //m_ShowLabelFeature.Layer = pLayer;
                //        ////显示标注
                //        //m_LayerExtent.Layer = pLayer;
                //        ////显示图层属性

                //        ////区域定位图层不操作
                //        //if (pLayer.Name == g_ZoneLocation)
                //        //{
                //        //    m_RemoveLayer.Enabled2 = false;
                //        //    m_SetLabel.Enabled2 = false;
                //        //    m_ShowLabelFeature.Enabled2 = false;
                //        //}
                //        m_pLayerMenu.PopupMenu(e.x, e.y, TOCLayer.hWnd);
                //    }
                //}



            }
        }
	}
}
