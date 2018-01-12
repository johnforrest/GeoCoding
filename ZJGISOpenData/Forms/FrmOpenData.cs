using System;
using System.IO;
using System.Windows.Forms;
using DevComponents.AdvTree;
using DevComponents.DotNetBar;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ZJGISOpenData.Classes;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.esriSystem;
using System.Collections.ObjectModel;
using ZJGISCommon;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ZJGISOpenData.Forms
{
    public partial class FrmOpenData : DevComponents.DotNetBar.Office2007Form
    {
        private ElementStyle _RightAlignFileSizeStyle = null;
        private string postFix = null;
        //1
        private DirectoryInfo currentDirectory = null;

        private string pFinallyPath = null;
        private string pFullpath = null;

        private IBasicMap targetMap = null;

        private Collection<object> m_ListCollection = new Collection<object>();
        private Collection<object> m_FileCollection = new Collection<object>();
        private Collection<object> m_FeatClsCollection = new Collection<object>();
        private Collection<object> m_TableCollection = new Collection<object>();

        private Collection<object> m_DatasetCol = new Collection<object>();
        //2
        private IWorkspace m_pMDBGDBWorkspace;
        private IWorkspace m_pSDEWorkspace;

        //private string m_LastPathName;
        private string m_sFilePath = null;
        private string m_sDirectoryFullPath = null;
        private string m_sMdbPath = null;
        //20180112
        private string opendatapath = new DirectoryInfo("../").FullName + "Res\\path\\opendatapath.txt";
        List<string> listOPData = new List<string>();

        private Collection<object> m_pSelectedCln = null;
        private bool m_blnAddData = true;
        private bool blnEditMapSpatial = false;

        //要素集名称
        private string m_sFeatureDatasetName = null;
        //要素类名称
        private string m_sFeatureClassName = null;


        //3
        private List<object> preList = new List<object>();

        private string currentPath = ""; //当前路径


        //是否显示表
        private bool isShowTable = false;
        public IWorkspace Workspace
        {
            get
            {
                return m_pMDBGDBWorkspace;
            }
        }


        public string FileType
        {
            get
            {
                return postFix;
            }
        }


        public IWorkspace SDEWorkspace
        {
            get
            {
                return m_pSDEWorkspace;
            }
        }


        public string PathName
        {
            get
            {
                //return m_LastPathName;
                return currentDirectory.FullName;
            }
        }


        public Collection<object> SelectedCln
        {
            get
            {
                return m_DatasetCol;
            }
        }

        public bool BlnAddData
        {
            set
            {
                m_blnAddData = value;
            }
        }


        public Collection<object> PathCln
        {
            get
            {
                return m_FileCollection;
            }
        }


        public Collection<object> FeatClsCollection
        {
            get
            {
                return m_FeatClsCollection;
            }
        }
        public Collection<object> TableCollection
        {
            get
            {
                return m_TableCollection;
            }
        }
        public string FilePath
        {
            get
            {
                return m_sFilePath;
            }
        }
        public bool IsShowTable
        {
            get
            {
                return isShowTable;
            }
            set
            {
                isShowTable = value;
            }
        }

        public FrmOpenData(IBasicMap targMap)
        {
            InitializeComponent();

            //设置回退按钮图标
            buttonXBack.Image = Properties.Resources.Up.ToBitmap();


            //初始图像列表文件
            imageListFiles.Images.Add("Folder", ClsGetSysIcon.GetDirectoryIcon());
            imageListFiles.Images.Add("Dataset", Properties.Resources.Dataset);
            imageListFiles.Images.Add("Pointlayer", Properties.Resources.Pointlayer);
            imageListFiles.Images.Add("Linelayer", Properties.Resources.Linelayer);
            imageListFiles.Images.Add("Polylayer", Properties.Resources.Polylayer);
            imageListFiles.Images.Add("Database", Properties.Resources.Database);
            imageListFiles.Images.Add("table", Properties.Resources.table);

            targetMap = targMap;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmOpenData()
        {
            InitializeComponent();

            //设置回退按钮图标
            buttonXBack.Image = Properties.Resources.Up.ToBitmap();

            //初始图像列表文件
            imageListFiles.Images.Add("Folder", ClsGetSysIcon.GetDirectoryIcon());
            imageListFiles.Images.Add("Dataset", Properties.Resources.Dataset);
            imageListFiles.Images.Add("Pointlayer", Properties.Resources.Pointlayer);
            imageListFiles.Images.Add("Linelayer", Properties.Resources.Linelayer);
            imageListFiles.Images.Add("Polylayer", Properties.Resources.Polylayer);
            imageListFiles.Images.Add("Database", Properties.Resources.Database);
            imageListFiles.Images.Add("table", Properties.Resources.table);

            //targetMap = targMap;
        }
        #region Form事件
        private void FrmOpenData_Load(object sender, EventArgs e)
        {
            InitialComboBox();
            //初始化comboBoxExFileType的选项，会调用对应的SelectedIndexChanged事件
            comboBoxExFileType.SelectedIndex = 0;

            //20180105 最初的
            //InitialBothView();

            //res文件夹的path文件夹下存在opendatapath.txt文件
            if (File.Exists(opendatapath))
            {
                ReadTxt();

                if (listOPData.Count > 0 && listOPData[listOPData.Count - 1].Length > 0)
                {
                    //加载最后一次加载的目录
                    string dir = listOPData[listOPData.Count - 1];
                    //ListUpdate(dir);
                    LoadPathToList(dir);
                }
                else
                {
                    InitialBothView();
                }
            }
            else
            {
                InitialBothView();
            }
        }
        /// <summary>
        /// 读取txt文件路径
        /// </summary>
        private void ReadTxt()
        {
            if (File.Exists(opendatapath))
            {
                System.IO.FileStream fs = new System.IO.FileStream(opendatapath, FileMode.Open, FileAccess.Read);
                StreamReader read = new StreamReader(fs, Encoding.Default);
                string strReadline;
                while ((strReadline = read.ReadLine()) != null && strReadline.Trim().Length > 0)
                {
                    // strReadline即为按照行读取的字符串
                    listOPData.Add(strReadline);
                }
                fs.Close();
                read.Close();
            }
        }

        private void InitialComboBox()
        {
            ////下拉框初始值
            if (!isShowTable)
            {
                comboBoxExFileType.Items.Add("File GeoDataBase (.gdb)");
                comboBoxExFileType.Items.Add("Shapefile  (.shp)");
                comboBoxExFileType.Items.Add("dbf文件(.dbf)");
                //Personal DataBase(.mdb)
                comboBoxExFileType.Items.Add("SDE数据库");
            }
            else
            {
                comboBoxExFileType.Items.Add("File GeoDataBase (.gdb)");
                comboBoxExFileType.Items.Add("dbf文件(.dbf)");
                comboBoxExFileType.Items.Add("Shapefile  (.shp)");
            }
        }

        /// <summary>
        /// 初始化advview和listview
        /// </summary>
        private void InitialBothView()
        {
            //获取磁盘
            DriveInfo[] drives = DriveInfo.GetDrives();

            advTreeFiles.BeginUpdate();
            listViewExFiles.BeginUpdate();
            foreach (DriveInfo driveInfo in drives)
            {
                //初始化磁盘树节点
                if (driveInfo.DriveType != DriveType.Fixed)
                    continue;
                Node node = new Node();
                node.Tag = driveInfo;
                node.Text = driveInfo.Name.Replace(@"\", "") + "本地磁盘";
                node.Image = Properties.Resources.Harddrive;
                advTreeFiles.Nodes.Add(node);
                node.ExpandVisibility = eNodeExpandVisibility.Visible;

                //初始磁盘在文件视窗中的显示
                imageListFiles.Images.Add(node.Text, ClsGetSysIcon.GetIcon(driveInfo.Name, true));
                ListViewItem lvi = new ListViewItem(node.Text);
                lvi.ImageKey = node.Text;
                lvi.Tag = driveInfo;

                listViewExFiles.Items.Add(lvi);
            }

            //初始化桌面节点
            Node nodeDesk = new Node();
            nodeDesk.Tag = new DirectoryInfo(ClsGetSysPath.GetPath(PathType.DeskTopPath));
            nodeDesk.Text = "桌面";
            nodeDesk.Image = Properties.Resources.Home;
            nodeDesk.ExpandVisibility = eNodeExpandVisibility.Visible;
            advTreeFiles.Nodes.Add(nodeDesk);

            //初始化我的文档节点
            Node nodeDoc = new Node();
            nodeDoc.Tag = new DirectoryInfo(ClsGetSysPath.GetPath(PathType.PersonalPath));
            nodeDoc.Text = "我的文档";
            nodeDoc.Image = Properties.Resources.MyDoc;
            nodeDoc.ExpandVisibility = eNodeExpandVisibility.Visible;
            advTreeFiles.Nodes.Add(nodeDoc);

            listViewExFiles.EndUpdate();
            advTreeFiles.EndUpdate();

            _RightAlignFileSizeStyle = new ElementStyle();
            _RightAlignFileSizeStyle.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Far;

            System.Diagnostics.Debug.WriteLine(comboBoxExFileType.Text.Trim());
        }
        /// <summary>
        /// 把固定路径下的文件夹读取到ListView中
        /// </summary>
        /// <param name="dirPath"></param>
        private void LoadPathToList(string dirPath)
        {
            string newPath = "";
            if (dirPath.Contains("."))
            {
                newPath = dirPath.Substring(0, dirPath.LastIndexOf("\\"));
            }
            else
            {
                newPath = dirPath;
            }
            if ( Directory.Exists(newPath)&&newPath.Length > 0)
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(newPath);
                LoadFolderFileToList(directoryInfo);
                //设置textBoxXPath目录
                this.textBoxXPath.Text = newPath;
                //设置当前DirectoryInfo
                currentDirectory = directoryInfo;
                //ListUpdate(newPath);
                InitialAdvTree();
            }
            else
            {
                InitialBothView();
            }
        }

        private void InitialAdvTree()
        {
            //获取磁盘
            DriveInfo[] drives = DriveInfo.GetDrives();
            advTreeFiles.BeginUpdate();
            listViewExFiles.BeginUpdate();
            foreach (DriveInfo driveInfo in drives)
            {
                //初始化磁盘树节点
                if (driveInfo.DriveType != DriveType.Fixed)
                    continue;
                Node node = new Node();
                node.Tag = driveInfo;
                node.Text = driveInfo.Name.Replace(@"\", "") + "本地磁盘";
                node.Image = Properties.Resources.Harddrive;
                advTreeFiles.Nodes.Add(node);
                node.ExpandVisibility = eNodeExpandVisibility.Visible;

                ////初始磁盘在文件视窗中的显示
                //imageListFiles.Images.Add(node.Text, ClsGetSysIcon.GetIcon(driveInfo.Name, true));
                //ListViewItem lvi = new ListViewItem(node.Text);
                //lvi.ImageKey = node.Text;
                //lvi.Tag = driveInfo;

                //listViewExFiles.Items.Add(lvi);
            }
            //初始化桌面节点
            Node nodeDesk = new Node();
            nodeDesk.Tag = new DirectoryInfo(ClsGetSysPath.GetPath(PathType.DeskTopPath));
            nodeDesk.Text = "桌面";
            nodeDesk.Image = Properties.Resources.Home;
            nodeDesk.ExpandVisibility = eNodeExpandVisibility.Visible;
            advTreeFiles.Nodes.Add(nodeDesk);

            //初始化我的文档节点
            Node nodeDoc = new Node();
            nodeDoc.Tag = new DirectoryInfo(ClsGetSysPath.GetPath(PathType.PersonalPath));
            nodeDoc.Text = "我的文档";
            nodeDoc.Image = Properties.Resources.MyDoc;
            nodeDoc.ExpandVisibility = eNodeExpandVisibility.Visible;
            advTreeFiles.Nodes.Add(nodeDoc);

            listViewExFiles.EndUpdate();
            advTreeFiles.EndUpdate();

            _RightAlignFileSizeStyle = new ElementStyle();
            _RightAlignFileSizeStyle.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Far;

            System.Diagnostics.Debug.WriteLine(comboBoxExFileType.Text.Trim());
        }

        private void InitialListView()
        {
            //获取磁盘
            DriveInfo[] drives = DriveInfo.GetDrives();

            advTreeFiles.BeginUpdate();
            listViewExFiles.BeginUpdate();
            foreach (DriveInfo driveInfo in drives)
            {
                //初始化磁盘树节点
                if (driveInfo.DriveType != DriveType.Fixed)
                    continue;
                Node node = new Node();
                node.Tag = driveInfo;
                node.Text = driveInfo.Name.Replace(@"\", "") + "本地磁盘";
                node.Image = Properties.Resources.Harddrive;
                advTreeFiles.Nodes.Add(node);
                node.ExpandVisibility = eNodeExpandVisibility.Visible;

                //初始磁盘在文件视窗中的显示
                imageListFiles.Images.Add(node.Text, ClsGetSysIcon.GetIcon(driveInfo.Name, true));
                ListViewItem lvi = new ListViewItem(node.Text);
                lvi.ImageKey = node.Text;
                lvi.Tag = driveInfo;
                listViewExFiles.Items.Add(lvi);
            }
            listViewExFiles.EndUpdate();
            advTreeFiles.EndUpdate();
        }

        private void FrmOpenData_FormClosed(object sender, FormClosedEventArgs e)
        {
            WriteTxt();
        }

        private void WriteTxt()
        {
            if (File.Exists(opendatapath))
            {
                if (currentDirectory != null)
                {
                    string path = currentDirectory.FullName;
                    if (!listOPData.Contains(path))
                    {
                        //只保存一行数据
                        System.IO.FileStream fs = new System.IO.FileStream(opendatapath, FileMode.Create);
                        //获得字节数组
                        byte[] data = System.Text.Encoding.Default.GetBytes(path);
                        //开始写入
                        fs.Write(data, 0, data.Length);
                        //清空缓冲区、关闭流
                        fs.Flush();
                        fs.Close();

                        //StreamWriter sw = new StreamWriter(opendatapath, true);
                        //sw.WriteLine();
                        //sw.Write(path);
                        //sw.Close();
                        ////fs.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("文件路径不存在，请检查文件路径！");
            }
        }

        #endregion
        /// <summary>
        /// 在指定目录下查找目录名，如果找到就打开。同时返回查找结果
        /// </summary>
        /// <param name="path">指定查找目录路径。</param>
        /// <param name="dirName">待查找的目录名。</param>
        /// <returns></returns>
        public bool FindAndOpenDir(string path, string dirName)
        {
            bool isFind = false;
            if (Directory.Exists(path))
            {//指定父目录存在时，才遍历子目录
                var subDirs = Directory.EnumerateDirectories(path);
                foreach (var subDir in subDirs)
                {//递归遍历子目录，直到找到指定目录。
                    DirectoryInfo dirInfo = new DirectoryInfo(subDir);
                    if (dirInfo.Name == dirName)
                    {
                        isFind = true;
                        Process.Start(subDir);//打开目录
                        break;
                    }
                    else
                    {//递归查找。当然现在就遍历不是很好，应该把当前目录下文件夹查找完再查找子目录，你可以自己修改。
                        isFind = FindAndOpenDir(subDir, dirName);
                    }
                }
            }
            if (!isFind)
            {//TODO:自定义未找到处理
            }
            return isFind;
        }
        private void ReturnParentFolder(string dirPath)
        {
            string newPath = "";
            if (dirPath.Contains("."))
            {
                newPath = dirPath.Substring(0, dirPath.LastIndexOf("\\"));
            }
            else
            {
                newPath = dirPath;
            }
            if (newPath.Length > 0)
            {
                ListUpdate(newPath);
            }
            else
            {
                InitialBothView();
            }

        }
        #region comboBox选择事件
        /// <summary>
        /// 选择类型下拉菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxExFileType_SelectedIndexChanged(object sender, EventArgs e)
        {
            preList.Clear();
            postFix = GetPostGetFix(comboBoxExFileType.SelectedItem.ToString());
            //不是sde
            if (postFix != null)
            {
                advTreeFiles.Enabled = true;
                if (imageListFiles.Images.ContainsKey("File"))
                {
                    imageListFiles.Images.RemoveByKey("File");
                }
                //获取对应的文件图标
                imageListFiles.Images.Add("File", ClsGetSysIcon.GetIcon(postFix, true));
                if (currentDirectory != null)
                {
                    LoadFolderFileToList(currentDirectory);
                }
            }
            //sde数据库
            else
            {
                advTreeFiles.Enabled = false;
                FrmSDE pFrmSDE = new FrmSDE();

                if (pFrmSDE.ShowDialog() == DialogResult.OK)
                {
                    if (pFrmSDE.m_pSDEWorkspace != null)
                    {
                        m_pSDEWorkspace = pFrmSDE.m_pSDEWorkspace;
                        LoadDatasetToList(pFrmSDE.m_pSDEWorkspace);
                        //cmbSDEList.Text = "Connect to SDE " + pFrmSDE.m_SDEPropertSet.GetProperty("Server");
                    }
                }
            }
            m_ListCollection.Clear();
            m_FileCollection.Clear();
            m_FeatClsCollection.Clear();
            m_TableCollection.Clear();
        }

        /// <summary>
        /// 获取文件后缀类型
        /// </summary>
        /// <param name="fullName">文件类型全名</param>
        /// <returns>获取的后缀名</returns>
        private string GetPostGetFix(string fullName)
        {
            if (fullName.Contains(".") && fullName.Contains("("))
            {
                return fullName.Substring(fullName.LastIndexOf("(")).Trim(new char[] { '(', ')' });
            }
            else
            {
                return null;
            }
        }

        #endregion
        #region advTree事件
        /// <summary>
        /// 在树节点展开之前，控制其子节点显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void advTreeFiles_BeforeExpand(object sender, AdvTreeNodeCancelEventArgs e)
        {
            Node parent = e.Node;
            if (parent.Nodes.Count > 0) return;
            //如果树节点是驱动
            if (parent.Tag is DriveInfo)
            {
                advTreeFiles.BeginUpdate();
                DriveInfo driveInfo = (DriveInfo)parent.Tag;
                LoadDirectoriesToadvTree(parent, driveInfo.RootDirectory);
                if (!parent.HasChildNodes)
                {
                    e.Cancel = true;
                    return;
                }
                parent.ExpandVisibility = eNodeExpandVisibility.Auto;
                advTreeFiles.EndUpdate(true);
            }
            //如果树节点是磁盘
            else if (parent.Tag is DirectoryInfo)
            {
                LoadDirectoriesToadvTree(parent, (DirectoryInfo)parent.Tag);
                if (!parent.HasChildNodes)
                {
                    e.Cancel = true;
                }
            }
        }
        /// <summary>
        /// 点击节点时触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void advTreeFiles_NodeClick(object sender, TreeNodeMouseEventArgs e)
        {
            DirectoryInfo directoryInfo = null;
            if (e.Node.Tag is DriveInfo)
            //节点为磁盘
            {
                directoryInfo = ((DriveInfo)e.Node.Tag).RootDirectory;
                textBoxXPath.Text = directoryInfo.Name;
            }
            else
            {
                //节点为目录
                if (e.Node.Tag is DirectoryInfo)
                {
                    directoryInfo = (DirectoryInfo)e.Node.Tag;
                    textBoxXPath.Text = directoryInfo.FullName;
                }
                else
                {
                    return;
                }
            }
            currentDirectory = directoryInfo;////////////////////////////当前目录
            //m_LastPathName = directoryInfo.FullName;
            LoadFolderFileToList(directoryInfo);
        }


        #endregion
        /// <summary>
        ///将目录加入advTree树
        /// </summary>
        /// <param name="parent">父节点</param>
        /// <param name="directoryInfo">父节点目录信息</param>
        private void LoadDirectoriesToadvTree(Node parent, DirectoryInfo directoryInfo)
        {
            DirectoryInfo[] directories = directoryInfo.GetDirectories();
            foreach (DirectoryInfo dir in directories)
            {
                if ((dir.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden) continue;
                Node node = new Node();
                node.Tag = dir;
                node.Text = dir.Name;
                node.Image = Properties.Resources.FolderClosed;
                node.ImageExpanded = Properties.Resources.FolderOpen;
                node.ExpandVisibility = eNodeExpandVisibility.Visible;
                parent.Nodes.Add(node);
            }
        }

        /// <summary>
        /// 将目录中信息载入listViewExFiles
        /// </summary>
        /// <param name="directoryInfo">上一级目录信息</param>
        private void LoadFolderFileToList(DirectoryInfo directoryInfo)
        {
            DirectoryInfo[] directories = directoryInfo.GetDirectories();
            listViewExFiles.BeginUpdate();
            listViewExFiles.Items.Clear();

            foreach (DirectoryInfo dir in directories)
            {
                if ((dir.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
                    continue;//隐藏文件不显示
                ListViewItem lvi = new ListViewItem(dir.Name);
                lvi.Tag = dir;

                //如果是gdb文件夹判断是否显示
                if (dir.Name.Contains(".") && dir.Name.Substring(dir.Name.LastIndexOf(".")).ToLower() == ".gdb")
                {
                    //过滤器是.gdb给予显示
                    if (postFix == ".gdb")
                    {
                        lvi.ImageKey = "Database";
                        listViewExFiles.Items.Add(lvi);
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    lvi.ImageKey = "Folder";
                    listViewExFiles.Items.Add(lvi);
                }
            }

            FileInfo[] files = directoryInfo.GetFiles();
            foreach (FileInfo file in files)
            {
                string fileName = file.Name;
                int index = fileName.LastIndexOf(".");
                if (index >= 0)
                {
                    if (fileName.Substring(index).ToLower() == postFix)
                    {
                        ListViewItem lvi = new ListViewItem(fileName);
                        lvi.Tag = file;
                        lvi.ImageKey = "File";
                        listViewExFiles.Items.Add(lvi);
                    }
                }
            }
            listViewExFiles.EndUpdate();
            preList.Clear();
        }

        #region listViewFiles事件
        private void listViewExFiles_DoubleClick(object sender, EventArgs e)
        {
            DirectoryInfo directoryInfo = null;
            object selectItemObject = listViewExFiles.SelectedItems[0].Tag;
            string selectItemText = listViewExFiles.SelectedItems[0].Text;
            if (selectItemObject == null && selectItemText.Length > 0 && !selectItemText.Contains("."))
            {
                string tempstr = currentPath + "\\" + selectItemText;
                LoadPathToList(tempstr);
            }
            else
            {
                //双击的项为磁盘根目录
                if (selectItemObject is DriveInfo)
                {
                    directoryInfo = ((DriveInfo)selectItemObject).RootDirectory;
                    currentDirectory = directoryInfo;
                    LoadFolderFileToList(directoryInfo);
                    preList.Clear();
                }
                //双击的项为文件夹
                else if ((selectItemObject is DirectoryInfo))
                {

                    directoryInfo = (DirectoryInfo)selectItemObject;
                    currentDirectory = directoryInfo;
                    DirectoryInfo[] dirs = directoryInfo.GetDirectories();
                    if (dirs.Length == 0 && selectItemText.Length > 4 && selectItemText.Substring(selectItemText.Length - 4) == ".gdb")
                    {
                        IWorkspaceFactory2 pFact = new FileGDBWorkspaceFactory() as IWorkspaceFactory2;
                        m_pMDBGDBWorkspace = pFact.OpenFromFile(directoryInfo.FullName, 0);
                        if (!isShowTable)
                        {
                            LoadDatasetToList(m_pMDBGDBWorkspace);
                        }
                        else
                        {
                            LoadTablesToList(m_pMDBGDBWorkspace);
                        }
                    }
                    else
                    {
                        LoadFolderFileToList(directoryInfo);
                    }
                }
                else
                {
                    //如果是数据集则将其子集加载到视图中
                    if (selectItemObject is IDatasetName)
                    {
                        IDatasetName datasetName = selectItemObject as IDatasetName;
                        string p = datasetName.WorkspaceName.WorkspaceFactoryProgID;
                        if (datasetName.Type == esriDatasetType.esriDTFeatureDataset)
                        {
                            LoadDatasetLayerToList(datasetName);
                        }
                        else
                        {
                            //如果是要素类加入到目标地图
                            if (selectItemObject is IFeatureClassName)
                            {
                                IName name = datasetName as IName;
                                IFeatureClass featureClass = name.Open() as IFeatureClass;

                                //加载选中FeatureClass到集合内
                                m_FeatClsCollection.Add(featureClass);

                                IFeatureLayer featureLayer = new FeatureLayerClass();
                                featureLayer.Name = featureClass.AliasName;
                                featureLayer.FeatureClass = featureClass;
                                if (targetMap != null)
                                {
                                    ClsMapLayer.AddLyrToBasicMap(targetMap, (ILayer)featureLayer);
                                }
                                this.DialogResult = DialogResult.OK;
                            }
                            else if (selectItemObject is ITableName)
                            {
                                IName name = datasetName as IName;
                                ITable table = name.Open() as ITable;
                                m_TableCollection.Add(table);
                            }
                        }
                    }
                    else
                    {
                        IWorkspaceFactory workspaceFactory;
                        IFeatureWorkspace featWorkspace;
                        try
                        {
                            //如果双击的是.shp文件则将其加载到目标地图
                            if (postFix == ".shp")
                            {
                                //string path = currentDirectory.FullName;
                                string path = null;
                                if (currentDirectory != null)
                                {
                                    path = currentDirectory.FullName;

                                }
                                else if (pFinallyPath != null)
                                {
                                    path = pFinallyPath;
                                }
                                else
                                {
                                    path = currentPath;
                                }
                                workspaceFactory = new ShapefileWorkspaceFactory() as IWorkspaceFactory;
                                featWorkspace = workspaceFactory.OpenFromFile(path, 0) as IFeatureWorkspace;
                                IFeatureClass featureClass = featWorkspace.OpenFeatureClass(selectItemText);
                                IFeatureLayer featureLayer = new FeatureLayerClass();
                                featureLayer.Name = featureClass.AliasName;
                                featureLayer.FeatureClass = featureClass;

                                //加载选中FeatureClass到集合内
                                m_FeatClsCollection.Add(featureClass);
                                if (targetMap != null)
                                {
                                    ClsMapLayer.AddLyrToBasicMap(targetMap, (ILayer)featureLayer);
                                }
                                this.DialogResult = DialogResult.OK;
                            }
                            //如果双击的为mdb文件将其子集加载
                            if (postFix == ".mdb")
                            {
                                m_sMdbPath = currentDirectory.FullName + "\\" + selectItemText;
                                m_sFilePath = m_sMdbPath;
                                IWorkspaceFactory2 pFact = new AccessWorkspaceFactory() as IWorkspaceFactory2;
                                if (pFact.IsWorkspace(m_sMdbPath))
                                {
                                    //获得MDB路径
                                    m_pMDBGDBWorkspace = pFact.OpenFromFile(m_sMdbPath, 0);
                                    LoadDatasetToList(m_pMDBGDBWorkspace);
                                }
                            }
                            //如果双击的为gdb文件将其子集加载
                            if (postFix == ".gdb")
                            {
                                //m_sMdbPath = currentDirectory.FullName + "\\" + temItemName;
                                if (currentDirectory != null)
                                {
                                    m_sMdbPath = currentDirectory.FullName + "\\" + selectItemText;
                                }
                                else
                                {
                                    if (pFinallyPath != null)
                                    {
                                        m_sMdbPath = pFinallyPath + "\\" + selectItemText;
                                    }
                                    else
                                    {
                                        m_sMdbPath = currentPath + "\\" + selectItemText;

                                    }
                                    //m_sMdbPath = pFinallyPath;
                                }
                                m_sFilePath = m_sMdbPath;
                                IWorkspaceFactory2 pFact = new FileGDBWorkspaceFactoryClass();

                                m_pMDBGDBWorkspace = pFact.OpenFromFile(m_sMdbPath, 0);
                                LoadDatasetToList(m_pMDBGDBWorkspace);

                            }
                        }
                        catch (System.Exception ex)
                        {
                            MessageBox.Show("选择的文件类型不正确！");
                        }
                    }
                }

            }
        }
        /// <summary>
        /// listViewExFiles单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewExFiles_MouseUp(object sender, MouseEventArgs e)
        {
            if (this.DialogResult == DialogResult.OK)
            {
                return;
            }
            //在listViewExFiles中单击空白地方
            if (listViewExFiles.SelectedItems.Count == 0)
            {
                return;
            }
            //在listViewExFiles中选中文件        
            else
            {
                object selectItemObject = listViewExFiles.SelectedItems[0].Tag;
                //选中为文件夹
                if (selectItemObject is DirectoryInfo)
                {
                    textBoxXPath.Text = ((DirectoryInfo)selectItemObject).FullName;
                    //return;
                }
                //选中为文件
                if (selectItemObject is FileInfo)
                {
                    textBoxXPath.Text = ((FileInfo)selectItemObject).FullName;
                    //return;
                }
                //选中为磁盘
                if (selectItemObject is DriveInfo)
                {
                    textBoxXPath.Text = ((DriveInfo)selectItemObject).Name;
                    //return;
                }

                ListViewItem pListViewItem = null;
                //int relVal = 0;

                m_ListCollection.Clear();
                m_FileCollection.Clear();
                m_FeatClsCollection.Clear();
                m_TableCollection.Clear();

                string selectItemText = listViewExFiles.SelectedItems[0].Text;

                if (postFix == ".mdb")
                {
                    if (selectItemText.Length >= 4 && selectItemText.Substring(selectItemText.Length - 4).ToLower() == ".mdb")
                    {
                        return;
                    }

                    if (textBoxXPath.Text.ToLower().Contains(".mdb"))
                    {
                        for (int i = 0; i < listViewExFiles.SelectedItems.Count; i++)
                        {
                            pListViewItem = listViewExFiles.SelectedItems[i];
                            m_ListCollection.Add(pListViewItem.Tag);
                            //打开的是要素集
                            if (this.listViewExFiles.SelectedItems[i].Tag is IFeatureDatasetName)
                            {
                                IDatasetName pDSN = default(IDatasetName);
                                pDSN = this.listViewExFiles.SelectedItems[0].Tag as IDatasetName;
                                //获得要素集名称(m_sFeatureDatasetName)
                                m_sFeatureDatasetName = pDSN.Name;
                            }
                            //打开的是要素类
                            else
                            {
                                //获得要素类名称(m_sFeatureClassName)
                                m_sFeatureClassName = pListViewItem.Text;
                            }

                            if (m_sFeatureDatasetName == null)
                            {
                                m_sDirectoryFullPath = m_sMdbPath + "\\" + m_sFeatureClassName;
                            }
                            else
                            {
                                m_sDirectoryFullPath = m_sMdbPath + "\\" + m_sFeatureDatasetName + "\\" + m_sFeatureClassName;
                            }

                            m_FileCollection.Add(m_sDirectoryFullPath);
                        }
                    }
                }
                else if (postFix == ".gdb")
                {
                    if ((selectItemText.ToLower().Contains(".gdb") && selectItemText.Substring(selectItemText.Length - 4).ToLower() == ".gdb")
                        || currentDirectory == null)
                    {
                        return;
                    }
                    if (currentDirectory.FullName.ToLower().Contains(".gdb"))
                    {
                        for (int i = 0; i < listViewExFiles.SelectedItems.Count; i++)
                        {
                            pListViewItem = listViewExFiles.SelectedItems[i];
                            //txtName.Text = txtName.Text + pListViewItem.Text + ";";
                            m_ListCollection.Add(pListViewItem.Tag);
                        }
                    }
                }
                else
                {
                    if (postFix == null)
                    {
                        if (listViewExFiles.SelectedItems.Count == 0)
                        {
                            return;
                        }
                        //ListViewItem plistviewitem = null;
                        m_ListCollection.Clear();
                        for (int i = 0; i < listViewExFiles.SelectedItems.Count; i++)
                        {
                            pListViewItem = listViewExFiles.SelectedItems[i];
                            m_ListCollection.Add(pListViewItem.Tag);
                        }
                    }
                    else
                    {
                        for (int i = 0; i <= listViewExFiles.SelectedItems.Count - 1; i++)
                        {
                            pListViewItem = listViewExFiles.SelectedItems[i];

                            if (pListViewItem.Text.ToLower().Contains(postFix) && pListViewItem.Text.Substring(pListViewItem.Text.Length - 4).ToLower() == postFix)
                            {
                                //txtName.Text = txtName.Text + pListViewItem.Text + ";";
                                m_ListCollection.Add(pListViewItem.Text);
                                //获得shapefile完整文件路径集合
                                if (currentDirectory != null)
                                {
                                    m_sDirectoryFullPath = currentDirectory.FullName + "\\" + pListViewItem.Text;
                                }
                                else
                                {
                                    m_sDirectoryFullPath = pFinallyPath + "\\" + pListViewItem.Text;
                                }
                                //m_sDirectoryFullPath = currentDirectory.FullName + "\\" + pListViewItem.Text;
                                m_sFilePath = m_sDirectoryFullPath;
                                m_FileCollection.Add(m_sDirectoryFullPath);
                            }
                        }
                    }
                }

            }

        }

        #endregion
        #region 按钮
        /// <summary>
        /// 打开按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonXOpen_Click(object sender, EventArgs e)
        {
            DirectoryInfo directoryInfo = null;
            //如果在listviewExFiles中选中了，否则打开textBoxXPath中的内容
            #region 20180112
            if (listViewExFiles.SelectedItems.Count > 0)
            {
                for (int i = 0; i < listViewExFiles.SelectedItems.Count; i++)
                {
                    object selectItemObject = listViewExFiles.SelectedItems[i].Tag;
                    string selectItemText = listViewExFiles.SelectedItems[i].Text;

                    if (selectItemObject == null && selectItemText.Length > 0 && !selectItemText.Contains("."))
                    {
                        string tempstr = currentPath + "\\" + selectItemText;
                        LoadPathToList(tempstr);
                    }
                    else
                    {
                        //选中的项为磁盘根目录
                        if (selectItemObject is DriveInfo)
                        {
                            directoryInfo = ((DriveInfo)selectItemObject).RootDirectory;
                            currentDirectory = directoryInfo;
                            LoadFolderFileToList(directoryInfo);
                            preList.Clear();
                        }
                        //选中的项为文件夹
                        else if ((selectItemObject is DirectoryInfo))
                        {
                            directoryInfo = (DirectoryInfo)selectItemObject;
                            currentDirectory = directoryInfo;
                            //该文件夹不包含子文件夹且该文件夹的后缀为.gdb
                            DirectoryInfo[] dirs = directoryInfo.GetDirectories();
                            if (dirs.Length == 0 && selectItemText.Length > 4 && selectItemText.Substring(selectItemText.Length - 4) == ".gdb")
                            {
                                IWorkspaceFactory2 pFact = new FileGDBWorkspaceFactory() as IWorkspaceFactory2;
                                m_pMDBGDBWorkspace = pFact.OpenFromFile(directoryInfo.FullName, 0);
                                if (!isShowTable)
                                {
                                    LoadDatasetToList(m_pMDBGDBWorkspace);
                                }
                                else
                                {
                                    LoadTablesToList(m_pMDBGDBWorkspace);
                                }
                            }
                            else
                            {
                                LoadFolderFileToList(directoryInfo);
                            }
                        }
                        //如果是数据集则将其子集加载到视图中
                        else if (selectItemObject is IDatasetName)
                        {
                            IDatasetName datasetName = selectItemObject as IDatasetName;
                            if (datasetName.Type == esriDatasetType.esriDTFeatureDataset)
                            {
                                LoadDatasetLayerToList(datasetName);
                            }
                            else
                            {
                                //如果是要素类加入到目标地图
                                if (selectItemObject is IFeatureClassName)
                                {
                                    IName name = datasetName as IName;
                                    IFeatureClass featureClass = name.Open() as IFeatureClass;

                                    //加载选中FeatureClass到集合内
                                    m_FeatClsCollection.Add(featureClass);
                                    IFeatureLayer featureLayer = new FeatureLayerClass();
                                    featureLayer.Name = featureClass.AliasName;
                                    featureLayer.FeatureClass = featureClass;
                                    if (targetMap != null)
                                    {
                                        ClsMapLayer.AddLyrToBasicMap(targetMap, (ILayer)featureLayer);
                                    }
                                    this.DialogResult = DialogResult.OK;
                                }
                                else if (selectItemObject is ITableName)
                                {
                                    IName name = datasetName as IName;
                                    ITable table = name.Open() as ITable;
                                    m_TableCollection.Add(table);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                string newPath = textBoxXPath.Text.Trim();
                if (newPath == "")
                    return;
                ListUpdate(newPath);
                pFinallyPath = newPath;
            }
            #endregion

            #region 20180112注释
            if (listViewExFiles.SelectedItems.Count > 0)
            {
                if (postFix == ".shp")
                {
                    if (targetMap != null)
                    {
                        //ClsMapLayer.AddSelectedLayer(targetMap, postFix, m_ListCollection, m_DatasetCol, currentDirectory.FullName, m_blnAddData);
                        if (currentDirectory != null)
                        {
                            ClsMapLayer.AddSelectedLayer(targetMap, postFix, m_ListCollection, m_DatasetCol, currentDirectory.FullName, m_blnAddData);
                        }
                        else
                        {
                            ClsMapLayer.AddSelectedLayer(targetMap, postFix, m_ListCollection, m_DatasetCol, pFinallyPath, m_blnAddData);
                        }
                    }
                    ////20081201 印骅 获得要素类集合
                    IWorkspaceFactory pWorkspaceFactory = null;
                    IFeatureWorkspace pFeatWorkspace = null;
                    IFeatureClass pFeatCls = null;
                    string sFileName = null;

                    pWorkspaceFactory = new ESRI.ArcGIS.DataSourcesFile.ShapefileWorkspaceFactory();
                    if (currentDirectory != null)
                    {
                        if (pWorkspaceFactory.IsWorkspace(currentDirectory.FullName))
                        {
                            pFeatWorkspace = pWorkspaceFactory.OpenFromFile(currentDirectory.FullName, 0) as IFeatureWorkspace;
                            for (int i = 0; i < m_ListCollection.Count; i++)
                            {
                                sFileName = m_ListCollection[i].ToString();
                                pFeatCls = pFeatWorkspace.OpenFeatureClass(sFileName);
                                m_FeatClsCollection.Add(pFeatCls);
                            }
                        }
                    }
                    else
                    {
                        if (pWorkspaceFactory.IsWorkspace(pFinallyPath))
                        {
                            pFeatWorkspace = pWorkspaceFactory.OpenFromFile(pFinallyPath, 0) as IFeatureWorkspace;
                            for (int i = 0; i < m_ListCollection.Count; i++)
                            {
                                sFileName = m_ListCollection[i].ToString();
                                pFeatCls = pFeatWorkspace.OpenFeatureClass(sFileName);
                                m_FeatClsCollection.Add(pFeatCls);
                            }
                        }
                    }

                    this.DialogResult = DialogResult.OK;
                }
                if (postFix == ".mdb")
                {
                    //印骅 20081204 没有选择任何项
                    if (listViewExFiles.SelectedItems.Count == 0)
                    {
                        m_FileCollection.Clear();
                        return;
                    }
                    //Strings.Right(lvwDriver.SelectedItems(0).Name, 4) = ".mdb"

                    for (int i = 0; i < listViewExFiles.SelectedItems.Count; i++)
                    {
                        string pStr = listViewExFiles.SelectedItems[i].Text;
                        if (pStr.Length >= 4 && pStr.Substring(pStr.Length - 4) == ".mdb")
                        {
                            listViewExFiles_DoubleClick(null, null);
                        }
                        else
                        {
                            ////按照加载featureclass 和featuredataset 来做
                            if (targetMap != null)
                            {
                                ClsMapLayer.AddSelectedLayer(targetMap, "IDatasetName", m_ListCollection, m_DatasetCol, "", m_blnAddData);
                            }
                            //// 获得MDB中要素类集合
                            IDatasetName pDSN = null;
                            IName pName = null;
                            IDataset pDataset = null;
                            IFeatureClass pFeatCls = null;

                            //If m_ListCollection.Item(i) = esriDatasetType.esriDTFeatureDataset Then Exit Sub '选择要素集，不添加数据
                            if (listViewExFiles.SelectedItems[i].Tag is IFeatureDatasetName)
                            {
                                this.DialogResult = DialogResult.OK;
                                return;
                            }
                            for (int j = 0; j < m_ListCollection.Count; j++)
                            {
                                pDSN = m_ListCollection[j] as IDatasetName;
                                pName = pDSN as IName;
                                pDataset = pName.Open() as IDataset;
                                pFeatCls = pDataset as IFeatureClass;
                                m_FeatClsCollection.Add(pFeatCls);
                            }
                            this.DialogResult = DialogResult.OK;
                        }
                    }
                }
                if (postFix == ".gdb")
                {
                    for (int i = 0; i < listViewExFiles.SelectedItems.Count; i++)
                    {
                        if (listViewExFiles.SelectedItems[i].Text.ToLower().Contains(".gdb")
                         && listViewExFiles.SelectedItems[i].Text.Substring(listViewExFiles.SelectedItems[i].Text.Length - 4) == ".gdb")
                        {
                            if (listViewExFiles.SelectedItems.Count == 0 || listViewExFiles.SelectedItems.Count > 1)
                            {
                                return;
                            }
                            else
                            {
                                //listViewExFiles_DoubleClick(null, null);
                                if (currentDirectory != null)
                                {
                                    m_sMdbPath = currentDirectory.FullName + "\\" + listViewExFiles.SelectedItems[i].Text;
                                }
                                else
                                {
                                    if (pFinallyPath != null)
                                    {
                                        m_sMdbPath = pFinallyPath + "\\" + listViewExFiles.SelectedItems[i].Text;
                                    }
                                    else
                                    {
                                        m_sMdbPath = currentPath + "\\" + listViewExFiles.SelectedItems[i].Text;

                                    }
                                    //m_sMdbPath = pFinallyPath;
                                }
                                m_sFilePath = m_sMdbPath;
                                IWorkspaceFactory2 pFact = new FileGDBWorkspaceFactoryClass();

                                m_pMDBGDBWorkspace = pFact.OpenFromFile(m_sMdbPath, 0);
                                LoadDatasetToList(m_pMDBGDBWorkspace);
                            }
                        }
                        else
                        {
                            ////按照加载featureclass 和featuredataset 来做
                            if (listViewExFiles.SelectedItems.Count >= 0)
                            {
                                object tempObj = listViewExFiles.SelectedItems[i].Tag;
                                if (tempObj is IFeatureClassName)
                                {
                                    IDatasetName datasetName = tempObj as IDatasetName;
                                    IName name = datasetName as IName;

                                    IFeatureClass featureClass = name.Open() as IFeatureClass;
                                    //20170515注释掉
                                    //m_FeatClsCollection.Add(featureClass);
                                }
                                if (tempObj is ITableName)
                                {
                                    IDatasetName datasetName = tempObj as IDatasetName;
                                    IName name = datasetName as IName;

                                    ITable table = name.Open() as ITable;
                                    m_TableCollection.Add(table);
                                }
                                if (targetMap != null)
                                {
                                    //20170314 gdb数据加载两次，注释掉此行代码
                                    //ClsMapLayer.AddSelectedLayer(targetMap, "IDatasetName", m_ListCollection, m_DatasetCol, null, m_blnAddData);
                                }
                                this.DialogResult = DialogResult.OK;
                            }
                            else
                            {
                                return;
                            }

                        }
                    }
                }
                if (postFix == null)
                {
                    if (targetMap != null)
                    {
                        ClsMapLayer.AddSelectedLayer(targetMap, "IDatasetName", m_ListCollection, m_DatasetCol, null, m_blnAddData);
                    }
                    this.DialogResult = DialogResult.OK;
                }
            }
            else
            {
                return;
            }
            #endregion

        }

        /// <summary>
        /// 后退按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonXBack_Click(object sender, EventArgs e)
        {
            object preObject = null;
            if (preList.Count >= 2)
            {
                preObject = preList[preList.Count - 2];
                preList.RemoveAt(preList.Count - 1);
                preList.RemoveAt(preList.Count - 1);
            }
            if (preObject is IWorkspace)
            {
                IWorkspace preLocation = preObject as IWorkspace;
                LoadDatasetToList(preLocation);
                return;
            }
            if (preObject is IDatasetName)
            {
                IDatasetName preLocation = preObject as IDatasetName;
                LoadDatasetLayerToList(preLocation);
                return;
            }
            if (preObject == null)
            {
                if (postFix == null)
                {
                    MessageBox.Show("已经是最上一层了!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (currentDirectory == null)
                {
                    MessageBox.Show("已经是最上一层了!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    //如果是包含有.mdb文件的目录
                    if (postFix == ".mdb" && preList.Count == 1)
                    {
                        textBoxXPath.Text = currentDirectory.FullName;
                        LoadFolderFileToList(currentDirectory);
                        preList.Clear();
                        return;
                    }
                    //上一层为空，即我的电脑
                    if (currentDirectory.Parent == null)
                    {
                        currentDirectory = currentDirectory.Parent;
                        textBoxXPath.Text = "";

                        listViewExFiles.Items.Clear();
                        DriveInfo[] drives = DriveInfo.GetDrives();
                        foreach (DriveInfo driveInfo in drives)
                        {
                            if (driveInfo.DriveType != DriveType.Fixed)
                                continue;
                            string label = driveInfo.Name.Replace(@"\", "") + "本地磁盘";
                            imageListFiles.Images.Add(label, ClsGetSysIcon.GetIcon(driveInfo.Name, true));
                            ListViewItem lvi = new ListViewItem(label);
                            lvi.ImageKey = label;
                            lvi.Tag = driveInfo;

                            listViewExFiles.Items.Add(lvi);
                        }
                    }
                    else
                    {
                        currentDirectory = currentDirectory.Parent;
                        textBoxXPath.Text = currentDirectory.FullName;
                        LoadFolderFileToList(currentDirectory);
                    }
                    preList.Clear();
                }
                //if (comboBoxExFileType.Text == "SDE数据库")
                //{
                //    listViewExFiles.Items.Clear();


                //    if ((m_pSDEWorkspace != null))
                //    {
                //        ////连接参数窗体消失后，在ListSDELyr控件中显示SDE图层列表
                //        LoadDatasetToList(m_pSDEWorkspace);
                //    }
                //    else
                //    {
                //        comboBoxExFileType_SelectedIndexChanged(null, null);
                //    }
                //}
            }
        }
        private void buttonXCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        #endregion
        private void LoadDatasetToList(IWorkspace datasetWorkSpace)
        {
            listViewExFiles.Items.Clear();

            IDatasetName datasetName;
            IFeatureClassName featureclassName;
            ListViewItem listItemFile;
            int shpType;

            IEnumDatasetName enumDatasetName = datasetWorkSpace.get_DatasetNames(esriDatasetType.esriDTFeatureDataset);
            while ((datasetName = enumDatasetName.Next()) != null)
            {
                listItemFile = listViewExFiles.Items.Add(datasetName.Name, "Dataset");
                listItemFile.Tag = datasetName;
            }

            IEnumDatasetName enumDatasetNameFeature = datasetWorkSpace.get_DatasetNames(esriDatasetType.esriDTFeatureClass);
            datasetName = null;

            while ((datasetName = enumDatasetNameFeature.Next()) != null)
            {
                if (datasetName.Type == esriDatasetType.esriDTFeatureClass)
                {
                    featureclassName = datasetName as IFeatureClassName;

                    if ((featureclassName.FeatureType == esriFeatureType.esriFTSimple)
                        || (featureclassName.FeatureType == esriFeatureType.esriFTSimpleJunction)
                        || (featureclassName.FeatureType == esriFeatureType.esriFTSimpleEdge)
                        || (featureclassName.FeatureType == esriFeatureType.esriFTComplexEdge)
                        || (featureclassName.FeatureType == esriFeatureType.esriFTComplexJunction))
                    {

                        shpType = (int)featureclassName.ShapeType;

                        if ((shpType == (int)esriGeometryType.esriGeometryPoint)
                            || (shpType == (int)esriGeometryType.esriGeometryMultipoint))
                        {
                            listItemFile = listViewExFiles.Items.Add(datasetName.Name, "Pointlayer");
                            listItemFile.Tag = datasetName;
                        }
                        else
                        {
                            if ((shpType == (int)esriGeometryType.esriGeometryRing)
                                || (shpType == (int)esriGeometryType.esriGeometryPolyline)
                                || (shpType == (int)esriGeometryType.esriGeometryCircularArc)
                                || (shpType == (int)esriGeometryType.esriGeometryEllipticArc))
                            {
                                listItemFile = listViewExFiles.Items.Add(datasetName.Name, "Linelayer");
                                listItemFile.Tag = featureclassName;
                            }
                            else
                            {
                                if ((shpType == (int)esriGeometryType.esriGeometryPolygon)
                                    || (shpType == (int)esriGeometryType.esriGeometryEnvelope))
                                {
                                    listItemFile = listViewExFiles.Items.Add(datasetName.Name, "Polylayer");
                                    listItemFile.Tag = featureclassName;
                                }
                            }
                        }
                    }
                    else
                    {
                        if ((featureclassName.FeatureType == esriFeatureType.esriFTAnnotation))
                        {
                            listItemFile = listViewExFiles.Items.Add(datasetName.Name, "Annolayer");
                            listItemFile.Tag = featureclassName;
                        }
                        else
                        {
                            if ((featureclassName.FeatureType == esriFeatureType.esriFTDimension))
                            {
                                listItemFile = listViewExFiles.Items.Add(datasetName.Name, "Dimensionlayer");
                                listItemFile.Tag = featureclassName;
                            }
                        }
                    }
                }
            }

            IEnumDatasetName enumDatasetNameRaster = datasetWorkSpace.get_DatasetNames(esriDatasetType.esriDTRasterDataset);
            datasetName = null;

            while ((datasetName = enumDatasetNameRaster.Next()) != null)
            {
                listItemFile = listViewExFiles.Items.Add(datasetName.Name, "Rasterlayer");
                listItemFile.Tag = datasetName;
            }

            enumDatasetNameRaster = datasetWorkSpace.get_DatasetNames(esriDatasetType.esriDTRasterCatalog);
            datasetName = null;

            while ((datasetName = enumDatasetNameRaster.Next()) != null)
            {
                listItemFile = listViewExFiles.Items.Add(datasetName.Name, "Rasterband");
                listItemFile.Tag = datasetName;
            }
            preList.Add(datasetWorkSpace);
        }

        private void LoadTablesToList(IWorkspace workspace)
        {
            listViewExFiles.Items.Clear();

            ITableName tableName;
            ListViewItem listItemFile;
            IDatasetName datasetName;

            IEnumDatasetName enumDatasetName = workspace.get_DatasetNames(esriDatasetType.esriDTTable);
            enumDatasetName.Reset();
            datasetName = enumDatasetName.Next();
            while (datasetName != null)
            {
                if (datasetName is ITableName)
                {
                    tableName = datasetName as ITableName;
                    listItemFile = listViewExFiles.Items.Add(datasetName.Name, "table");
                    listItemFile.Tag = datasetName;
                }
                datasetName = enumDatasetName.Next();
            }
            preList.Add(workspace);
        }
        public void LoadDatasetLayerToList(IDatasetName datasetName)
        {
            ListViewItem listItemDataset;
            IDatasetName subDatasetName;
            IFeatureClassName featureClassName;
            int shpType;

            IEnumDatasetName enumDatasetNameFeature = datasetName.SubsetNames;
            listViewExFiles.Items.Clear();

            while ((subDatasetName = enumDatasetNameFeature.Next()) != null)
            {
                if (subDatasetName.Type == esriDatasetType.esriDTFeatureClass)
                {
                    featureClassName = subDatasetName as IFeatureClassName;

                    if (featureClassName.FeatureType == esriFeatureType.esriFTSimple)
                    {
                        shpType = (int)featureClassName.ShapeType;

                        if ((shpType == (int)esriGeometryType.esriGeometryPoint)
                            || (shpType == (int)esriGeometryType.esriGeometryMultipoint))
                        {
                            listItemDataset = listViewExFiles.Items.Add(subDatasetName.Name, "Pointlayer");
                            listItemDataset.Tag = featureClassName;
                        }
                        else
                        {
                            if ((shpType == (int)esriGeometryType.esriGeometryRing)
                                || (shpType == (int)esriGeometryType.esriGeometryPolyline)
                                || (shpType == (int)esriGeometryType.esriGeometryCircularArc)
                                || (shpType == (int)esriGeometryType.esriGeometryEllipticArc))
                            {
                                listItemDataset = listViewExFiles.Items.Add(subDatasetName.Name, "Linelayer");
                                listItemDataset.Tag = featureClassName;
                            }
                            else
                            {
                                if ((shpType == (int)esriGeometryType.esriGeometryPolygon)
                                    || (shpType == (int)esriGeometryType.esriGeometryEnvelope))
                                {
                                    listItemDataset = listViewExFiles.Items.Add(subDatasetName.Name, "Polylayer");
                                    listItemDataset.Tag = featureClassName;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (featureClassName.FeatureType == esriFeatureType.esriFTAnnotation)
                        {
                            listItemDataset = listViewExFiles.Items.Add(subDatasetName.Name, "Annolayer");
                            listItemDataset.Tag = featureClassName;
                        }
                        if (featureClassName.FeatureType == esriFeatureType.esriFTDimension)
                        {
                            listItemDataset = listViewExFiles.Items.Add(datasetName.Name, "Dimensionlayer");
                            listItemDataset.Tag = featureClassName;
                        }
                        if ((featureClassName.FeatureType == esriFeatureType.esriFTSimpleJunction)
                            || (featureClassName.FeatureType == esriFeatureType.esriFTSimpleEdge)
                            || (featureClassName.FeatureType == esriFeatureType.esriFTComplexEdge)
                            || (featureClassName.FeatureType == esriFeatureType.esriFTComplexJunction))
                        {
                            shpType = (int)featureClassName.ShapeType;

                            if ((shpType == (int)esriGeometryType.esriGeometryPoint)
                                || (shpType == (int)esriGeometryType.esriGeometryMultipoint))
                            {
                                listItemDataset = listViewExFiles.Items.Add(subDatasetName.Name, "Pointlayer");
                                listItemDataset.Tag = featureClassName;
                            }
                            else
                            {
                                if ((shpType == (int)esriGeometryType.esriGeometryRing)
                                    || (shpType == (int)esriGeometryType.esriGeometryPolyline)
                                    || (shpType == (int)esriGeometryType.esriGeometryCircularArc)
                                    || (shpType == (int)esriGeometryType.esriGeometryEllipticArc))
                                {
                                    listItemDataset = listViewExFiles.Items.Add(subDatasetName.Name, "Linelayer");
                                    listItemDataset.Tag = featureClassName;
                                }
                                else
                                {
                                    if ((shpType == (int)esriGeometryType.esriGeometryPolygon)
                                        || (shpType == (int)esriGeometryType.esriGeometryEnvelope))
                                    {
                                        listItemDataset = listViewExFiles.Items.Add(subDatasetName.Name, "Polylayer");
                                        listItemDataset.Tag = featureClassName;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if ((subDatasetName.Type == esriDatasetType.esriDTGeometricNetwork))
                    {
                        listItemDataset = listViewExFiles.Items.Add(subDatasetName.Name, "NET");
                        listItemDataset.Tag = subDatasetName;
                    }
                    if (subDatasetName.Type == esriDatasetType.esriDTNetworkDataset)
                    {
                        listItemDataset = listViewExFiles.Items.Add(subDatasetName.Name, "NETWORK");
                        listItemDataset.Tag = subDatasetName;
                    }
                }

            }
            preList.Add(datasetName);
        }
        /// <summary>
        /// 鼠标在textbox中的回车事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxXPath_KeyPress(object sender, KeyPressEventArgs e)
        {
            //DirectoryInfo pDirectoryInof = null;
            //if (e.KeyChar == (char)Keys.Enter)

            if (e.KeyChar == 13)
            {
                string newPath = textBoxXPath.Text.Trim();
                if (newPath == "")
                    return;
                ListUpdate(newPath);
                pFinallyPath = newPath;
            }
        }

        /// <summary>
        /// 更新列表(列出当前目录下的目录和文件)
        /// </summary>
        /// <param name="newPath"></param>
        private void ListUpdate(string newPath)
        {
            try
            {
                DirectoryInfo currentDir = new DirectoryInfo(newPath);
                //获取目录
                DirectoryInfo[] dirs = currentDir.GetDirectories();
                //获取文件
                FileInfo[] files = currentDir.GetFiles();
                //删除ImageList中的程序图标
                foreach (ListViewItem item in listViewExFiles.Items)
                {
                    //程序
                    if (item.Text.EndsWith(".exe"))
                    {
                        imageList2.Images.RemoveByKey(item.Text);
                        imageList3.Images.RemoveByKey(item.Text);
                    }
                }
                //清除listview里面的东西
                listViewExFiles.Items.Clear();
                //列出文件夹
                foreach (DirectoryInfo dir in dirs)
                {
                    ListViewItem dirItem = listViewExFiles.Items.Add(dir.Name, 2);
                    dirItem.Name = dir.FullName;
                    dirItem.SubItems.Add("");
                    dirItem.SubItems.Add("文件夹");
                    dirItem.SubItems.Add(dir.LastWriteTimeUtc.ToString());
                }
                //列出文件
                foreach (FileInfo file in files)
                {
                    ListViewItem fileItem = listViewExFiles.Items.Add(file.Name);
                    //程序文件或无扩展名
                    if (file.Extension == ".exe" || file.Extension == "")
                    {
                        System.Drawing.Icon fileIcon = ClsGetSysIcon.GetIconByFileName(file.FullName);
                        imageList2.Images.Add(file.Name, fileIcon);
                        imageList3.Images.Add(file.Name, fileIcon);
                        fileItem.ImageKey = file.Name;
                    }
                    else    //其它文件
                    {
                        //ImageList中不存在此类图标
                        if (!imageList2.Images.ContainsKey(file.Extension))
                        {
                            System.Drawing.Icon fileIcon = ClsGetSysIcon.GetIconByFileName(file.FullName);
                            imageList2.Images.Add(file.Extension, fileIcon);
                            imageList3.Images.Add(file.Extension, fileIcon);
                        }
                        fileItem.ImageKey = file.Extension;
                    }
                    fileItem.Name = file.FullName;
                    fileItem.SubItems.Add(file.Length.ToString() + "字节");
                    fileItem.SubItems.Add(file.Extension);
                    fileItem.SubItems.Add(file.LastWriteTimeUtc.ToString());
                }
                currentPath = newPath;
                //更新地址栏
                textBoxXPath.Text = currentPath;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}