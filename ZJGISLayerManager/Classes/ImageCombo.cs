using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace ZJGISLayerManager
{
    public partial class ImageCombo : ComboBox
    {
        //private ImageList m_ImageList;

        //public ImageCombo()
        //{
        //    InitializeComponent();
        //}

        //public ImageCombo(IContainer container)
        //{
        //    container.Add(this);

        //    InitializeComponent();
        //}

        //public ImageList ImageList
        //{
        //    get
        //    {
        //        return m_ImageList;
        //    }
        //    set
        //    {
        //        m_ImageList = value;
        //    }
        //}

        //protected override void OnDrawItem(DrawItemEventArgs e)
        //{
        //    ClsImageComboItem pCurItem;
        //    Rectangle rect;

        //    pCurItem = (ClsImageComboItem)this.Items[e.Index];
        //    rect = e.Bounds;

        //    if ((int)e.State!=0&&(int)DrawItemState.Selected!=0 )
        //    {
        //        e.Graphics.FillRectangle(SystemBrushes.Highlight, rect);
        //    }
        //    else
        //    {
        //        e.Graphics.FillRectangle(SystemBrushes.Window, rect);
        //    }

        //    if (e.Index!=-1)
        //    {
        //        e.Graphics.DrawImage(m_ImageList.Images[pCurItem.ImageIndex], rect.Left, rect.Top, m_ImageList.Images[pCurItem.ImageIndex].Width, m_ImageList.Images[pCurItem.ImageIndex].Height);
        //    }
        //    else
        //    {
        //        e.Graphics.DrawImage(m_ImageList.Images[pCurItem.ImageIndex], rect.Left, rect.Top, m_ImageList.Images[pCurItem.ImageIndex].Width, m_ImageList.Images[pCurItem.ImageIndex].Height);
        //    }

        //    base.OnDrawItem(e);
        //}

        private ImageList m_ImageList;
        public ImageList ImageList
        {
            get { return m_ImageList; }
            set { m_ImageList = value; }
        }

        protected override void OnDrawItem(System.Windows.Forms.DrawItemEventArgs e)
        {
            ClsImageComboItem pCurItem = default(ClsImageComboItem);
            Rectangle rect = default(Rectangle);

            pCurItem = this.Items(e.Index);
            rect = e.Bounds;

            if ((int)e.State!=0 && (int)DrawItemState.Selected!=0)
            {
                e.Graphics.FillRectangle(SystemBrushes.Highlight, rect);
            }
            else
            {
                e.Graphics.FillRectangle(SystemBrushes.Window, rect);
            }

            if (e.Index != -1)
            {
                e.Graphics.DrawImage(m_ImageList.Images[pCurItem.ImageIndex], rect.Left, rect.Top, m_ImageList.Images[pCurItem.ImageIndex].Width, m_ImageList.Images[pCurItem.ImageIndex].Height);
            }
            else
            {
                e.Graphics.DrawImage(m_ImageList.Images[pCurItem.ImageIndex], rect.Left, rect.Top, m_ImageList.Images[pCurItem.ImageIndex].Width, m_ImageList.Images[pCurItem.ImageIndex].Height);
            }

            base.OnDrawItem(e);
        }

        private ClsImageComboItem Items(int p)
        {
            throw new NotImplementedException();
        }

    }
}
