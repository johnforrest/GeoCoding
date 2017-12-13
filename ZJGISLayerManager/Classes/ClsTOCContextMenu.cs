using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using System.Windows.Forms;
using ESRI.ArcGIS.SystemUI;

namespace ZJGISLayerManager.Classes
{
    public sealed  class ClsTOCContextMenu
    {
        #region "私有字段"
        public AxTOCControl TOCControlLayer;
        public AxTOCControl TOCLayer
        {
            get
            {
                return TOCControlLayer;
            }
            set
            {
                //if (TOCControlLayer != null)
                //{
                //    TOCControlLayer.OnMouseUp += new MouseEventHandler(TOCControlLayer_OnMouseUp);
                //}
                TOCControlLayer = value;
                //if (TOCControlLayer != null)
                //{
                //    TOCControlLayer.OnMouseUp +=new ITOCControlEvents_Ax_OnMouseUpEventHandler(TOCControlLayer_OnMouseUp);
                //}
            }
            //定义后，可以监听FrmDataTree中的TOCLayerl的事件
        }

        //由frmMap->FrmDataTree中传进
        private IMapControl4 m_pMapControl;
        //地图右键菜单
        public IToolbarMenu m_pMapMenu;
        //图层右键菜单
         public IToolbarMenu m_pLayerMenu;
        //符号化
        private ClsSymbol m_Symbol;
        ////移除图层
        //ClsRemoveLayer m_RemoveLayer;
        //移除所有图层
        private ClsRemoveAllLayer m_RemoveAllLayer;
        //删除图层Item
        private ClsRemoveLayer m_RemoveLayer;
        //查看属性表
        private ClsAttributeTable m_LayerAttribute;
        //设置标注
        private ClsSetLabel m_SetLabel;
        //缩放至图层
        private ClsZoomToLayer m_ZoomToLayer;
        //图层可选
        ClsLayerSelectable m_LayerSelectable1;
        ClsLayerSelectable m_LayerSelectable2;
        //显示标注
        private ClsShowLabelFeature m_ShowLabelFeature;
        //显示图层属性
        private ClsLyrExtent m_LayerExtent;
        //窗口的引用
        private Form m_FrmMap;
        private DevComponents.AdvTree.AdvTree m_advTreeTOC = new DevComponents.AdvTree.AdvTree();
        #endregion
        private string m_NoValidLayers = "";

        #region "构造函数"
        /// <summary>
        /// 在PluginMapLayer中传进值，由接口IApplicationMapLayer的属性传进
        /// </summary>
        /// <param name="pTocControl">FrmDataTree中的TOCLayer控件</param>
        /// <param name="pMapControl">frmMap->FrmDataTree中的MapControl控件</param>
        /// <param name="pFrmMap">工程规划窗口</param>
        /// <remarks></remarks>
        public ClsTOCContextMenu(AxTOCControl pTocControl, IMapControl4 pMapControl, Form pFrmMap)
        {
            //传入控件
            TOCControlLayer = pTocControl;
            m_pMapControl = pMapControl;
            m_FrmMap = pFrmMap;

            //添加自定义命令到地图快捷菜单(TOC根节点)
            m_pMapMenu = new ToolbarMenuClass();
            m_pMapMenu.AddItem(new ClsLayerVisibleTrue(), -1, -1, false, esriCommandStyles.esriCommandStyleIconAndText);
            //图层可见
            m_pMapMenu.AddItem(new ClsLayerVisibleFalse(), -1, -1, false, esriCommandStyles.esriCommandStyleIconAndText);
            //图层不可见
            m_pMapMenu.SetHook(m_pMapControl);

            //添加自定义命令到地图快捷菜单(TOC子节点)
            m_pLayerMenu = new ToolbarMenuClass();
            //m_LayerAttribute = new ClsAttributeTable();
            //m_RemoveLayer = new ClsRemoveLayer();
            //m_ZoomToLayer = new ClsZoomToLayer();
            //m_SetLabel = new ClsSetLabel();
            //m_ShowLabelFeature = new ClsShowLabelFeature();
            //m_LayerExtent = new ClsLyrExtent();
            //m_Symbol = new ClsSymbol(m_pMapControl);
            m_RemoveLayer = new ClsRemoveLayer();
            m_RemoveAllLayer = new ClsRemoveAllLayer(TOCControlLayer, m_pMapControl);
            m_LayerExtent = new ClsLyrExtent(m_FrmMap);
            m_ZoomToLayer = new ClsZoomToLayer();
            m_LayerSelectable1 = new ClsLayerSelectable();
            m_LayerSelectable2 = new ClsLayerSelectable();
            m_LayerAttribute = new ClsAttributeTable();
            m_SetLabel = new ClsSetLabel(m_pMapControl);
            m_ShowLabelFeature = new ClsShowLabelFeature();

            //m_pLayerMenu.AddItem(m_LayerAttribute);
            ////图层属性表
            //m_pLayerMenu.AddItem(m_RemoveLayer);
            ////移除图层
            //m_pLayerMenu.AddItem(m_ZoomToLayer);
            ////缩放至图层
            //m_pLayerMenu.AddItem(m_SetLabel);
            ////设置标注
            //m_pLayerMenu.AddItem(m_ShowLabelFeature);
            ////显示标注
            //m_pLayerMenu.AddItem(m_LayerExtent);
            ////图层属性
            //m_pLayerMenu.AddItem(m_Symbol, -1, -1, false, esriCommandStyles.esriCommandStyleIconAndText);
            m_pLayerMenu.AddItem(m_RemoveLayer, -1, -1, false, esriCommandStyles.esriCommandStyleIconAndText);
            m_pLayerMenu.AddItem(m_ZoomToLayer, -1, -1, false, esriCommandStyles.esriCommandStyleIconAndText);
            m_pLayerMenu.AddItem(m_LayerAttribute, -1, -1, false, esriCommandStyles.esriCommandStyleIconAndText);

            m_LayerSelectable1.TocControl = TOCControlLayer.Object as ITOCControl2;
            m_LayerSelectable2.TocControl = TOCControlLayer.Object as ITOCControl2;
            m_pLayerMenu.AddItem(m_LayerSelectable1, 1, -1, true, esriCommandStyles.esriCommandStyleIconAndText); //'图层可选
            m_pLayerMenu.AddItem(m_LayerSelectable2, 2, -1, false, esriCommandStyles.esriCommandStyleIconAndText); //'/图层不可选
            ////
            m_pLayerMenu.AddItem(m_SetLabel, -1, -1, true, esriCommandStyles.esriCommandStyleIconAndText);           // '设置标注
            m_pLayerMenu.AddItem(m_ShowLabelFeature, -1, -1, false, esriCommandStyles.esriCommandStyleIconAndText);              //'显示标注
            m_pLayerMenu.AddItem(m_LayerExtent, -1, -1, true, esriCommandStyles.esriCommandStyleIconAndText);         //'图层范围；

            m_pLayerMenu.SetHook(m_pMapControl);

            if (TOCControlLayer != null)
            {
                TOCControlLayer.OnMouseUp += new ITOCControlEvents_Ax_OnMouseUpEventHandler(TOCControlLayer_OnMouseUp);
            }
        }
        #endregion

        #region "鼠标右键弹起事件"

        /// <summary>
        /// 鼠标右键弹起事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks></remarks>
        private void TOCControlLayer_OnMouseUp(object sender, ITOCControlEvents_OnMouseUpEvent e)
        {
            if (TOCControlLayer != null)
            {
                esriTOCControlItem pItem = esriTOCControlItem.esriTOCControlItemNone;
                IBasicMap pBasicMap = null;
                ILayer pLayer = null;
                object pOther = null;
                object pIndex = null;
                this.TOCControlLayer.MousePointer = esriControlsMousePointer.esriPointerArrow;
                //TOCLayer.HitTest(e.x, e.y, ref pItem, ref pMap, ref pLayer, ref pOther, ref pIndex);

                TOCControlLayer.HitTest(e.x, e.y, ref pItem, ref pBasicMap, ref pLayer, ref pOther, ref pIndex);
                //实现调整图层顺序功能
                if ((e.button == 1))
                {
                    return;
                }
                else
                {
                    if (pItem == esriTOCControlItem.esriTOCControlItemNone)
                    {
                        return;
                    }
                    //确定所选中的信息
                    this.TOCControlLayer.GetSelectedItem(ref pItem, ref pBasicMap, ref pLayer, ref pOther, ref pIndex);
                    //弹出相应的快捷菜单 根节点 子节点
                    if ((pItem == esriTOCControlItem.esriTOCControlItemMap))
                    {
                        m_pMapMenu.PopupMenu(e.x, e.y, TOCControlLayer.hWnd);

                    }
                    else if ((pItem == esriTOCControlItem.esriTOCControlItemLayer))
                    {
                        if (pLayer is IGroupLayer)
                        {
                            return;
                        }
                        if (pLayer is ICompositeLayer)
                        {
                            return;
                        }

                        //m_Symbol.Layer = pLayer;
                        //移除图层
                        m_RemoveLayer.Layer = pLayer;
                        m_RemoveLayer.Enabled2 = true;
                        m_LayerAttribute.Enabled2 = true;
                        //一些图层是业务功能图层，不能删除
                        //if (pLayer.Name.Contains(g_DY) | pLayer.Name.Contains(g_BDZ) | pLayer.Name.Contains(g_GT) | pLayer.Name.Contains(g_XL))
                        //{
                        //    m_RemoveLayer.Enabled2 = false;
                        //}

                        m_LayerAttribute.Layer = pLayer;
                        //查看属性表

                        InitDataEditEnvironment(pLayer);
                        //基础下图层属性表不看查看

                        m_LayerAttribute.FrmMap = m_FrmMap;
                        m_LayerSelectable1.Layer = pLayer;
                        m_LayerSelectable2.Layer = pLayer;
                        m_ZoomToLayer.Layer = pLayer;
                        //缩放至图层
                        m_SetLabel.Layer = pLayer;
                        //设置标注
                        m_ShowLabelFeature.Layer = pLayer;
                        //显示标注
                        m_LayerExtent.Layer = pLayer;
                        //显示图层属性

                        ////区域定位图层不操作
                        //if (pLayer.Name == g_ZoneLocation)
                        //{
                        //    m_RemoveLayer.Enabled2 = false;
                        //    m_SetLabel.Enabled2 = false;
                        //    m_ShowLabelFeature.Enabled2 = false;
                        //}
                        m_pLayerMenu.PopupMenu(e.x, e.y, TOCControlLayer.hWnd);
                    }
                }
            }

        }
        #endregion

        #region "处理基础地理信息和树图映射，其下节点不可查看属性表"

        /// <summary>
        /// 初始化基础地理信息编辑环境
        /// </summary>
        /// <remarks></remarks>

        private void InitDataEditEnvironment(ILayer pLayer)
        {
            //初始化基础地理信息m_advTreeTOC结构
            this.m_advTreeTOC.Nodes.Clear();
            //this.m_advTreeTOC.Nodes.Add(this.AddLayersToNode(pLayer, g_BaseName, m_pMapControl.Map));
        }

        private DevComponents.AdvTree.Node AddLayersToNode(ILayer pLayer, string groupLayerName, IMap pMap)
        {
            m_NoValidLayers = "";
            //新建数据节点
            DevComponents.AdvTree.Node LayerNode = new DevComponents.AdvTree.Node();
            for (int i = 0; i <= pMap.LayerCount - 1; i += 1)
            {
                //进入专题数据的GroupLayer
                if (pMap.get_Layer(i) is IGroupLayer & (pMap.get_Layer(i).Name == groupLayerName))
                {
                    AddLayerRecursive(pLayer, pMap.get_Layer(i), LayerNode);
                    //已找到专题数据图层，退出for
                    break; // TODO: might not be correct. Was : Exit For
                }
            }
            return LayerNode;
        }

        private void AddLayerRecursive(ILayer pLayer1, ILayer pLayer, DevComponents.AdvTree.Node pLayerNode)
        {
            ICompositeLayer pCompositeLayer = null;
            if (pLayer is ICompositeLayer)
            {
                pCompositeLayer = pLayer as ICompositeLayer;
            }
            if (pCompositeLayer == null)
            {
                //pLayer不是组合图层，检查

                if (CheckLayerValid(pLayer).Length > 0)
                {
                }
                else
                {
                    //图层有效，节点添加的是FeatureLayer
                    pLayerNode.Text = pLayer.Name;
                    pLayerNode.Tag = (IFeatureLayer)pLayer;

                    if (pLayer1.Name == pLayer.Name)
                    {
                        m_LayerAttribute.Enabled2 = false;
                        return;
                    }

                }
            }
            else
            {
                //pLayer是组合图层，节点添加的是ICompositeLayer
                pLayerNode.Text = pLayer.Name;
                pLayerNode.Tag = pCompositeLayer;
                for (int i = 0; i <= pCompositeLayer.Count - 1; i += 1)
                {
                    ILayer subLayer = pCompositeLayer.get_Layer(i);
                    //每个图层建一个节点
                    DevComponents.AdvTree.Node subLayerNode = new DevComponents.AdvTree.Node();
                    //将此节点添加到pLayerNode下面
                    pLayerNode.Nodes.Add(subLayerNode);
                    if ((subLayer is ICompositeLayer))
                    {
                        //递归，分析subLayerNode和旗下的节点
                        AddLayerRecursive(pLayer1, subLayer, subLayerNode);
                    }
                    else if ((subLayer is IFeatureLayer))
                    {
                        //subLayer不是组合图层，检查
                        if (CheckLayerValid(subLayer).Length > 0)
                        {
                            if (pLayer1.Name == subLayer.Name)
                            {
                                m_LayerAttribute.Enabled2 = false;
                                return;
                            }
                        }
                        else
                        {
                            //图层有效，节点添加的是FeatureLayer
                            if (pLayer1.Name == subLayer.Name)
                            {
                                m_LayerAttribute.Enabled2 = false;
                                return;
                            }
                            subLayerNode.Text = subLayer.Name;

                            subLayerNode.Tag = (IFeatureLayer)subLayer;
                        }
                    }
                    else
                    {
                        if (pLayer1.Name == subLayer.Name)
                        {
                            m_LayerAttribute.Enabled2 = false;
                            return;
                        }
                    }
                }
            }

        }

        private string CheckLayerValid(ILayer pLayer)
        {
            if ((pLayer.Valid == false))
            {
                //判断该图层是否有效
                return pLayer.Name;
            }
            if (!(pLayer is IFeatureLayer))
            {
                //该图层不是FeatureLayer
                return pLayer.Name;
            }
            if ((((IFeatureLayer)pLayer).FeatureClass == null))
            {
                //该图层没有FeatureClass
                return pLayer.Name;
            }
            return "";
        }

        #endregion

    }
}
