using System.Windows.Forms;
namespace ZJGISLayerManager
{
    partial class ImageCombo : ComboBox
    {
        ///// <summary>
        ///// Required designer variable.
        ///// </summary>
        //private System.ComponentModel.IContainer components = null;

        ///// <summary> 
        ///// Clean up any resources being used.
        ///// </summary>
        ///// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing && (components != null))
        //    {
        //        components.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        //#region Component Designer generated code

        ///// <summary>
        ///// Required method for Designer support - do not modify
        ///// the contents of this method with the code editor.
        ///// </summary>
        //private void InitializeComponent()
        //{
        //    components = new System.ComponentModel.Container();
        //}

        //#endregion

        [System.Diagnostics.DebuggerNonUserCode()]
        public void New(System.ComponentModel.IContainer Container)
        {

            //Windows.Forms 类撰写设计器支持所必需的
            Container.Add(this);

        }

        [System.Diagnostics.DebuggerNonUserCode()]
        public void New()
        {

            //组件设计器需要此调用。
            InitializeComponent();
            DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
        }


        //Component 重写 Dispose，以清理组件列表。
        [System.Diagnostics.DebuggerNonUserCode()]
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        //组件设计器所必需的

        private System.ComponentModel.IContainer components;
        //注意: 以下过程是组件设计器所必需的
        //可使用组件设计器修改它。
        //不要使用代码编辑器修改它。
        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ResumeLayout(false);

        }

    }
}
