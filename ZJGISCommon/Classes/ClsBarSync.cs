using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevComponents.DotNetBar.Controls;

namespace ZJGISCommon
{
    public class ClsBarSync
    {
        private ProgressBarX barProgress;

        public ClsBarSync(ProgressBarX progress)
        {
            barProgress = progress;
        }

        private delegate void SetBar(int value);
        /// <summary>
        /// 设置单步移动
        /// </summary>
        /// <param name="value"></param>
        public void SetStep(int value)
        {
            if (barProgress.InvokeRequired)
            {
                SetBar pSetBar = SetStep;
                barProgress.Invoke(pSetBar, value);
            }
            else
            {
                barProgress.Step = value;
            }
        }
        /// <summary>
        /// 设置最小值
        /// </summary>
        /// <param name="value"></param>
        public void SetMin(int value)
        {
            if (barProgress.InvokeRequired)
            {
                SetBar pSetBar = SetMin;
                barProgress.Invoke(pSetBar, value);
            }
            else
            {
                barProgress.Minimum = value;
            }
        }

        public void SetPosition(int value)
        {
            if (barProgress.InvokeRequired)
            {
                SetBar pSetBar = SetPosition;
                barProgress.Invoke(pSetBar, value);
            }
            else
            {
                barProgress.Value = value;
            }
        }
        /// <summary>
        /// 设置最大值
        /// </summary>
        /// <param name="value"></param>
        public void SetMax(int value)
        {
            if (barProgress.InvokeRequired)
            {
                SetBar pSetBar = SetMax;
                barProgress.Invoke(pSetBar, value);
            }
            else
            {
                barProgress.Maximum = value;
            }
        }

        private delegate void DoWithOutArgs();
        /// <summary>
        /// 向下移动一步
        /// </summary>
        public void PerformOneStep()
        {
            if (barProgress.InvokeRequired)
            {
                DoWithOutArgs pStartStep = PerformOneStep;
                barProgress.Invoke(pStartStep);
            }
            else
            {
                barProgress.PerformStep();
            }
        }
    }
}
