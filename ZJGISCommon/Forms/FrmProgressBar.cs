using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ZJGISCommon.Forms
{
    public partial class FrmProgressBar : Form
    {
        private int maxCount;
        ClsBarSync progressBar;

        public FrmProgressBar()
        {
            InitializeComponent();
            progressBar = new ClsBarSync(progressBarX1);
        }
        public FrmProgressBar(int max)
        {
            InitializeComponent();
            maxCount = max;
            progressBar = new ClsBarSync(progressBarX1);
        }

        private void FrmProgressBar_Load(object sender, EventArgs e)
        {
            progressBar.SetStep(1);
            progressBar.SetMax(maxCount);

            //获取主屏幕的宽高
            System.Drawing.Rectangle rect = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
            //宽（像素）
            int w = rect.Width; 
            //高（像素）
            int h = rect.Height; 
            //因为button只需要向左右偏移，所以在Y轴上就没有改变，继续保留了原来的大小  
            this.Location = new Point(w / 2, h / 2);
        }

        /// <summary>
        /// 走一步
        /// </summary>
        public void GoOneStep()
        {
            progressBar.PerformOneStep();
        }
        /// <summary>
        /// 关闭窗体
        /// </summary>
        public void CloseForm()
        {
            this.Close();
        }
    }
}
