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

namespace ZJGISOpenData.Forms
{
    public partial class FrmOpenData : DevComponents.DotNetBar.Office2007Form
    {
        private ElementStyle _RightAlignFileSizeStyle = null;
        private string strFix = null;
        private DirectoryInfo currentDirectory = null;

        private string pFinallyPath = null;
        private string pFullpath = null;

        private IBasicMap targetMap = null;

        private Collection<object> m_ListCollection = new Collection<object>();
        private Collection<object> m_DatasetCol = new Collection<object>();
        private IWorkspace m_pMdbWS;
        private IWorkspace m_pSDEWorkspace;

        //private string m_LastPathName;
        private string m_sFilePath = null;
        private string m_sFullPath = null;
        private string m_sMdbPath = null;

        private Collection<object> m_pSelectedCln = null;
        private bool m_blnAddData = true;
        private bool blnEditMapSpatial = false;

        private string m_sFeatDatasetName = null;
        private string m_sFeatClsName = null;

        private Collection<object> m_FileCollection = new Collection<object>();
        private Collection<object> m_FeatClsCollection = new Collection<object>();
        private Collection<object> m_TableCollection = new Collection<object>();

        private List<object> preList = new List<object>();

        private string currentPath = ""; //��ǰ·��


        //�Ƿ���ʾ��
        private bool isShowTable = false;
        private bool isUpdateDate = false;
        public IWorkspace Workspace
        {
            get
            {
                return m_pMdbWS;
            }
        }


        public string FileType
        {
            get
            {
                return strFix;
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
        public bool IsUpdateDate
        {
            get
            {
                return isUpdateDate;
            }
            set
            {
                isUpdateDate = value;
            }
        }
        public FrmOpenData(IBasicMap targMap)
        {
            InitializeComponent();

            //���û��˰�ťͼ��
            buttonXBack.Image = Properties.Resources.Up.ToBitmap();


            //��ʼͼ���б��ļ�
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
        /// ���캯��
        /// </summary>
        public FrmOpenData()
        {
            InitializeComponent();

            //���û��˰�ťͼ��
            //buttonXBack.Image = Properties.Resources.Up.ToBitmap();

            //��ʼͼ���б��ļ�
            imageListFiles.Images.Add("Folder", ClsGetSysIcon.GetDirectoryIcon());
            imageListFiles.Images.Add("Dataset", Properties.Resources.Dataset);
            imageListFiles.Images.Add("Pointlayer", Properties.Resources.Pointlayer);
            imageListFiles.Images.Add("Linelayer", Properties.Resources.Linelayer);
            imageListFiles.Images.Add("Polylayer", Properties.Resources.Polylayer);
            imageListFiles.Images.Add("Database", Properties.Resources.Database);
            imageListFiles.Images.Add("table", Properties.Resources.table);

            //targetMap = targMap;
        }
        private void FrmOpenData_Load(object sender, EventArgs e)
        {
            if (!isShowTable)
            {
                comboBoxExFileType.Items.Add("File GeoDataBase (.gdb)");

                comboBoxExFileType.Items.Add("Shapefile  (.shp)");
                comboBoxExFileType.Items.Add("dbf�ļ�(.dbf)");

                //Personal DataBase(.mdb)
                comboBoxExFileType.Items.Add("SDE���ݿ�");
            }
            else
            {
                comboBoxExFileType.Items.Add("File GeoDataBase (.gdb)");
                comboBoxExFileType.Items.Add("dbf�ļ�(.dbf)");
                comboBoxExFileType.Items.Add("Shapefile  (.shp)");
            }

            comboBoxExFileType.SelectedIndex = 0;

            //20170615ע�͵�
            ////�������ʼֵ
            //if (!isUpdateDate)
            //{
            //    comboBoxExFileType.SelectedIndex = 0;
            //}
            //else
            //{
            //comboBoxExFileType.Items.RemoveAt(0);
            //    comboBoxExFileType.SelectedIndex = 0;

            //}
            //��ȡ����
            DriveInfo[] drives = DriveInfo.GetDrives();

            advTreeFiles.BeginUpdate();
            listViewExFiles.BeginUpdate();
            foreach (DriveInfo driveInfo in drives)
            {
                //��ʼ���������ڵ�
                if (driveInfo.DriveType != DriveType.Fixed) continue;
                Node node = new Node();
                node.Tag = driveInfo;
                node.Text = driveInfo.Name.Replace(@"\", "") + "���ش���";
                node.Image = Properties.Resources.Harddrive;
                advTreeFiles.Nodes.Add(node);
                node.ExpandVisibility = eNodeExpandVisibility.Visible;

                //��ʼ�������ļ��Ӵ��е���ʾ
                imageListFiles.Images.Add(node.Text, ClsGetSysIcon.GetIcon(driveInfo.Name, true));
                ListViewItem lvi = new ListViewItem(node.Text);
                lvi.ImageKey = node.Text;
                lvi.Tag = driveInfo;

                listViewExFiles.Items.Add(lvi);
            }

            //��ʼ������ڵ�
            Node nodeDesk = new Node();
            nodeDesk.Tag = new DirectoryInfo(ClsGetSysPath.GetPath(PathType.DeskTopPath));
            nodeDesk.Text = "����";
            nodeDesk.Image = Properties.Resources.Home;
            nodeDesk.ExpandVisibility = eNodeExpandVisibility.Visible;
            advTreeFiles.Nodes.Add(nodeDesk);

            //��ʼ���ҵ��ĵ��ڵ�
            Node nodeDoc = new Node();
            nodeDoc.Tag = new DirectoryInfo(ClsGetSysPath.GetPath(PathType.PersonalPath));
            nodeDoc.Text = "�ҵ��ĵ�";
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
        /// �����ڵ�չ��֮ǰ���������ӽڵ���ʾ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void advTreeFiles_BeforeExpand(object sender, AdvTreeNodeCancelEventArgs e)
        {
            Node parent = e.Node;
            if (parent.Nodes.Count > 0) return;

            if (parent.Tag is DriveInfo)
            {
                advTreeFiles.BeginUpdate();
                DriveInfo driveInfo = (DriveInfo)parent.Tag;
                LoadDirectoriesToTree(parent, driveInfo.RootDirectory);
                if (!parent.HasChildNodes)
                {
                    e.Cancel = true;
                    return;
                }
                parent.ExpandVisibility = eNodeExpandVisibility.Auto;
                advTreeFiles.EndUpdate(true);
            }
            else if (parent.Tag is DirectoryInfo)
            {
                LoadDirectoriesToTree(parent, (DirectoryInfo)parent.Tag);
                if (!parent.HasChildNodes)
                {
                    e.Cancel = true;
                }
            }
        }

        /// <summary>
        /// ѡ�����������˵�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxExFileType_SelectedIndexChanged(object sender, EventArgs e)
        {
            preList.Clear();
            strFix = GetStringFix(comboBoxExFileType.SelectedItem.ToString());
            if (strFix != null)//////////////////////////////////////////////////////////////////�������sde
            {
                advTreeFiles.Enabled = true;
                if (imageListFiles.Images.ContainsKey("File"))
                {
                    imageListFiles.Images.RemoveByKey("File");
                }

                imageListFiles.Images.Add("File", ClsGetSysIcon.GetIcon(strFix, true));//////��ȡ��Ӧ���ļ�ͼ��
                if (currentDirectory != null)
                {
                    LoadFolderFileToList(currentDirectory);
                }
            }
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

                        //          cmbSDEList.Text = "Connect to SDE " + pFrmSDE.m_SDEPropertSet.GetProperty("Server");
                    }
                }
            }
            m_ListCollection.Clear();
            m_FileCollection.Clear();
            m_FeatClsCollection.Clear();
            m_TableCollection.Clear();
        }

        private void buttonXCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// ��Ŀ¼����Ϣ�����Ӵ�
        /// </summary>
        /// <param name="directoryInfo">��һ��Ŀ¼��Ϣ</param>
        private void LoadFolderFileToList(DirectoryInfo directoryInfo)
        {
            DirectoryInfo[] directories = directoryInfo.GetDirectories();
            listViewExFiles.BeginUpdate();
            listViewExFiles.Items.Clear();

            foreach (DirectoryInfo dir in directories)
            {
                if ((dir.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden) continue;//�����ļ�����ʾ
                ListViewItem lvi = new ListViewItem(dir.Name);
                lvi.Tag = dir;

                if (dir.Name.Contains(".") && dir.Name.Substring(dir.Name.LastIndexOf(".")).ToLower() == ".gdb")//�����gdb�ļ����ж��Ƿ���ʾ
                {
                    if (strFix == ".gdb")/////////////////////////////////////////////��������.gdb������ʾ
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
                    if (fileName.Substring(index).ToLower() == strFix)
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

        private void listViewExFiles_DoubleClick(object sender, EventArgs e)
        {
            DirectoryInfo directoryInfo = null;
            object tempObject = listViewExFiles.SelectedItems[0].Tag;
            string temItemName = listViewExFiles.SelectedItems[0].Text;
            if (tempObject is DriveInfo)/////////////////////////////////////////////˫������Ϊ���̸�Ŀ¼
            {
                directoryInfo = ((DriveInfo)tempObject).RootDirectory;
                currentDirectory = directoryInfo;
                LoadFolderFileToList(directoryInfo);
                preList.Clear();
            }
            else
            {
                if ((tempObject is DirectoryInfo))/////////////////////////////////////˫������Ϊ�ļ���
                {

                    directoryInfo = (DirectoryInfo)tempObject;
                    currentDirectory = directoryInfo;
                    DirectoryInfo[] dirs = directoryInfo.GetDirectories();
                    if(dirs.Length==0&&temItemName.Length > 4 && temItemName.Substring(temItemName.Length - 4) == ".gdb")
                    {
                        IWorkspaceFactory2 pFact = new FileGDBWorkspaceFactory() as IWorkspaceFactory2;
                        m_pMdbWS = pFact.OpenFromFile(directoryInfo.FullName, 0);
                        if (!isShowTable)
                        {
                            LoadDatasetToList(m_pMdbWS);
                        }
                        else
                        {
                            LoadTablesToList(m_pMdbWS);
                        }
                    }
                    else
                    {
                        LoadFolderFileToList(directoryInfo);
                    }
                }
                else
                {
                    if (tempObject is IDatasetName)//////////////////////////////////////��������ݼ������Ӽ����ص���ͼ��
                    {
                        IDatasetName datasetName = tempObject as IDatasetName;
                        string p = datasetName.WorkspaceName.WorkspaceFactoryProgID;
                        if (datasetName.Type == esriDatasetType.esriDTFeatureDataset)
                        {
                            LoadDatasetLayerToList(datasetName);
                        }
                        else
                        {
                            if (tempObject is IFeatureClassName)///////////////////////////�����Ҫ������뵽Ŀ���ͼ
                            {
                                IName name = datasetName as IName;
                                IFeatureClass featureClass = name.Open() as IFeatureClass;

                                //����ѡ��FeatureClass��������
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
                            else if (tempObject is ITableName)
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
                            if (strFix == ".shp")/////////////////////////////���˫������.shp�ļ�������ص�Ŀ���ͼ
                            {
                                //string path = currentDirectory.FullName;
                                string path = null;
                                if (currentDirectory != null)
                                {
                                    path = currentDirectory.FullName;

                                }
                                else
                                {
                                    path = pFinallyPath;
                                }
                                workspaceFactory = new ShapefileWorkspaceFactory() as IWorkspaceFactory;
                                featWorkspace = workspaceFactory.OpenFromFile(path, 0) as IFeatureWorkspace;
                                IFeatureClass featureClass = featWorkspace.OpenFeatureClass(temItemName);
                                IFeatureLayer featureLayer = new FeatureLayerClass();
                                featureLayer.Name = featureClass.AliasName;
                                featureLayer.FeatureClass = featureClass;

                                //����ѡ��FeatureClass��������
                                m_FeatClsCollection.Add(featureClass);
                                if (targetMap != null)
                                {
                                    ClsMapLayer.AddLyrToBasicMap(targetMap, (ILayer)featureLayer);
                                }
                                this.DialogResult = DialogResult.OK;
                            }
                            if (strFix == ".mdb")//////////////////////////////���˫����Ϊmdb�ļ������Ӽ�����
                            {
                                m_sMdbPath = currentDirectory.FullName + "\\" + temItemName;
                                m_sFilePath = m_sMdbPath;
                                IWorkspaceFactory2 pFact = new AccessWorkspaceFactory() as IWorkspaceFactory2;
                                if (pFact.IsWorkspace(m_sMdbPath))
                                {
                                    //���MDB·��
                                    m_pMdbWS = pFact.OpenFromFile(m_sMdbPath, 0);
                                    LoadDatasetToList(m_pMdbWS);
                                }
                            }

                            if (strFix == ".gdb")/////////////////////////////���˫����Ϊgdb�ļ������Ӽ�����
                            {
                                //m_sMdbPath = currentDirectory.FullName + "\\" + temItemName;
                                if (currentDirectory != null)
                                {
                                    m_sMdbPath = currentDirectory.FullName + "\\" + temItemName;
                                }
                                else
                                {
                                    m_sMdbPath = pFinallyPath + "\\" + temItemName;
                                    //m_sMdbPath = pFinallyPath;
                                }
                                m_sFilePath = m_sMdbPath;
                                IWorkspaceFactory2 pFact = new FileGDBWorkspaceFactoryClass();

                                m_pMdbWS = pFact.OpenFromFile(m_sMdbPath, 0);
                                LoadDatasetToList(m_pMdbWS);

                            }
                        }
                        catch (System.Exception ex)
                        {
                            MessageBox.Show("ѡ����ļ����Ͳ���ȷ��");
                        }
                    }
                }
            }
        }

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
                if (strFix == null)
                {
                    MessageBox.Show("�Ѿ�������һ����!", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (currentDirectory == null)
                {
                    MessageBox.Show("�Ѿ�������һ����!", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    if (strFix == ".mdb" && preList.Count == 1)////////////////////////////////����ǰ�����.mdb�ļ���Ŀ¼
                    {
                        textBoxXPath.Text = currentDirectory.FullName;
                        LoadFolderFileToList(currentDirectory);
                        preList.Clear();
                        return;
                    }
                    if (currentDirectory.Parent == null)////////////////////////��һ��Ϊ�գ����ҵĵ���
                    {
                        currentDirectory = currentDirectory.Parent;
                        textBoxXPath.Text = "";

                        listViewExFiles.Items.Clear();
                        DriveInfo[] drives = DriveInfo.GetDrives();
                        foreach (DriveInfo driveInfo in drives)
                        {
                            if (driveInfo.DriveType != DriveType.Fixed) continue;
                            string label = driveInfo.Name.Replace(@"\", "") + "���ش���";
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
                //if (comboBoxExFileType.Text == "SDE���ݿ�")
                //{
                //    listViewExFiles.Items.Clear();


                //    if ((m_pSDEWorkspace != null))
                //    {
                //        ////���Ӳ���������ʧ����ListSDELyr�ؼ�����ʾSDEͼ���б�
                //        LoadDatasetToList(m_pSDEWorkspace);
                //    }
                //    else
                //    {
                //        comboBoxExFileType_SelectedIndexChanged(null, null);
                //    }
                //}
            }
        }
        /// <summary>
        ///��Ŀ¼��������ͼ
        /// </summary>
        /// <param name="parent">���ڵ�</param>
        /// <param name="directoryInfo">���ڵ�Ŀ¼��Ϣ</param>
        private void LoadDirectoriesToTree(Node parent, DirectoryInfo directoryInfo)
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
        /// ��ȡ���������ļ����Ͷ�Ӧ�ĺ�׺
        /// </summary>
        /// <param name="fullName">�ļ�����ȫ��</param>
        /// <returns>��ȡ�ĺ�׺��</returns>
        private string GetStringFix(string fullName)
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

        private void LoadDatasetToList(IWorkspace datasetWorkSpace)
        {
            listViewExFiles.Items.Clear();

            IDatasetName datasetName;
            IFeatureClassName featureclassName;
            ListViewItem listItemFile;
            int shpType;

            IEnumDatasetName enumDatasetName =
                datasetWorkSpace.get_DatasetNames(esriDatasetType.esriDTFeatureDataset);
            while ((datasetName = enumDatasetName.Next()) != null)
            {
                listItemFile = listViewExFiles.Items.Add(datasetName.Name, "Dataset");
                listItemFile.Tag = datasetName;
            }

            IEnumDatasetName enumDatasetNameFeature =
                datasetWorkSpace.get_DatasetNames(esriDatasetType.esriDTFeatureClass);
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

            IEnumDatasetName enumDatasetNameRaster =
                datasetWorkSpace.get_DatasetNames(esriDatasetType.esriDTRasterDataset);
            datasetName = null;

            while ((datasetName = enumDatasetNameRaster.Next()) != null)
            {
                listItemFile = listViewExFiles.Items.Add(datasetName.Name, "Rasterlayer");
                listItemFile.Tag = datasetName;
            }

            enumDatasetNameRaster =
                datasetWorkSpace.get_DatasetNames(esriDatasetType.esriDTRasterCatalog);
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
        /// �򿪰�ť
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonXOpen_Click(object sender, EventArgs e)
        {
            ////yjw begin 2016.8.12
            ////������������ʵ��ѡ�д򿪹��ܡ�

            DirectoryInfo directoryInfo = null;
            //�����listviewExFiles��ѡ���ˣ������textBoxXPath�е�����
            if (listViewExFiles.SelectedItems.Count > 0)
            {
                for (int i = 0; i < listViewExFiles.SelectedItems.Count; i++)
                {
                    object tempObject = listViewExFiles.SelectedItems[i].Tag;
                    string temItemName = listViewExFiles.SelectedItems[i].Text;

                    if (tempObject is DriveInfo)///////////////////////////////////////////ѡ�е���Ϊ���̸�Ŀ¼
                    {
                        directoryInfo = ((DriveInfo)tempObject).RootDirectory;
                        currentDirectory = directoryInfo;
                        LoadFolderFileToList(directoryInfo);
                        preList.Clear();
                    }
                    else if ((tempObject is DirectoryInfo))////////////////////////////////ѡ�е���Ϊ�ļ���
                    {
                        directoryInfo = (DirectoryInfo)tempObject;
                        currentDirectory = directoryInfo;
                        //���ļ��в��������ļ����Ҹ��ļ��еĺ�׺Ϊ.gdb
                        DirectoryInfo[] dirs = directoryInfo.GetDirectories();
                        if (dirs.Length == 0 && temItemName.Length > 4 && temItemName.Substring(temItemName.Length - 4) == ".gdb")
                        {
                            IWorkspaceFactory2 pFact = new FileGDBWorkspaceFactory() as IWorkspaceFactory2;
                            m_pMdbWS = pFact.OpenFromFile(directoryInfo.FullName, 0);
                            if (!isShowTable)
                            {
                                LoadDatasetToList(m_pMdbWS);
                            }
                            else
                            {
                                LoadTablesToList(m_pMdbWS);
                            }
                        }
                        else
                        {
                            LoadFolderFileToList(directoryInfo);
                        }
                    }
                    else if (tempObject is IDatasetName)////////////////////////////��������ݼ������Ӽ����ص���ͼ��
                    {
                        IDatasetName datasetName = tempObject as IDatasetName;
                        if (datasetName.Type == esriDatasetType.esriDTFeatureDataset)
                        {
                            LoadDatasetLayerToList(datasetName);
                        }
                        else
                        {
                            if (tempObject is IFeatureClassName)///////////////////////////�����Ҫ������뵽Ŀ���ͼ
                            {
                                IName name = datasetName as IName;
                                IFeatureClass featureClass = name.Open() as IFeatureClass;

                                //����ѡ��FeatureClass��������
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
                            else if (tempObject is ITableName)
                            {
                                IName name = datasetName as IName;
                                ITable table = name.Open() as ITable;
                                m_TableCollection.Add(table);
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
            //yjw end


            if (listViewExFiles.SelectedItems.Count == 0)
            {
                return;
            }
            else
            {
                if (strFix == ".shp")
                {
                    if (targetMap != null)
                    {
                        //ClsMapLayer.AddSelectedLayer(targetMap, strFix, m_ListCollection, m_DatasetCol, currentDirectory.FullName, m_blnAddData);
                        if (currentDirectory != null)
                        {
                            ClsMapLayer.AddSelectedLayer(targetMap, strFix, m_ListCollection, m_DatasetCol, currentDirectory.FullName, m_blnAddData);
                        }
                        else
                        {
                            ClsMapLayer.AddSelectedLayer(targetMap, strFix, m_ListCollection, m_DatasetCol, pFinallyPath, m_blnAddData);
                        }
                    }
                    ////20081201 ӡ�� ���Ҫ���༯��
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
                if (strFix == ".mdb")
                {
                    //ӡ�� 20081204 û��ѡ���κ���
                    if (listViewExFiles.SelectedItems.Count == 0)
                    {
                        m_FileCollection.Clear();
                        return;
                    }
                    //Strings.Right(lvwDriver.SelectedItems(0).Name, 4) = ".mdb"

                    for (int i = 0; i < listViewExFiles.SelectedItems.Count;i++ )
                    {
                        string pStr = listViewExFiles.SelectedItems[i].Text;
                        if (pStr.Length >= 4 && pStr.Substring(pStr.Length - 4) == ".mdb")
                        {
                            listViewExFiles_DoubleClick(null, null);
                        }
                        else
                        {
                            ////���ռ���featureclass ��featuredataset ����
                            if (targetMap != null)
                            {
                                ClsMapLayer.AddSelectedLayer(targetMap, "IDatasetName", m_ListCollection, m_DatasetCol, "", m_blnAddData);
                            }
                            //// ���MDB��Ҫ���༯��
                            IDatasetName pDSN = null;
                            IName pName = null;
                            IDataset pDataset = null;
                            IFeatureClass pFeatCls = null;

                            //If m_ListCollection.Item(i) = esriDatasetType.esriDTFeatureDataset Then Exit Sub 'ѡ��Ҫ�ؼ������������
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
                if (strFix == ".gdb")
                {
                    for (int i = 0; i < listViewExFiles.SelectedItems.Count;i++ )
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
                                listViewExFiles_DoubleClick(null, null);
                            }
                        }
                        else
                        {
                            ////���ռ���featureclass ��featuredataset ����
                            if (listViewExFiles.SelectedItems.Count == 0)
                            {
                                return;
                            }
                            else
                            {
                                object tempObj = listViewExFiles.SelectedItems[i].Tag;
                                if (tempObj is IFeatureClassName)
                                {
                                    IDatasetName datasetName = tempObj as IDatasetName;
                                    IName name = datasetName as IName;

                                    IFeatureClass featureClass = name.Open() as IFeatureClass;
                                    //20170515ע�͵�
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
                                    //20170314 gdb���ݼ������Σ�ע�͵����д���
                                    //ClsMapLayer.AddSelectedLayer(targetMap, "IDatasetName", m_ListCollection, m_DatasetCol, null, m_blnAddData);
                                }
                                this.DialogResult = DialogResult.OK;
                            }
                        }
                    }
                }
                if (strFix == null)
                {
                    if (targetMap != null)
                    {
                        ClsMapLayer.AddSelectedLayer(targetMap, "IDatasetName", m_ListCollection, m_DatasetCol, null, m_blnAddData);
                    }
                    this.DialogResult = DialogResult.OK;
                }
            }
        }




        private void listViewExFiles_MouseUp(object sender, MouseEventArgs e)
        {

            if (this.DialogResult == DialogResult.OK)
            {
                return;
            }
            if (listViewExFiles.SelectedItems.Count == 0)
            {
                return;
            }

            object tempObject = listViewExFiles.SelectedItems[0].Tag;
            if (tempObject is DirectoryInfo) //////////////////////////////////////////////////////////////////////////ѡ��Ϊ �ļ���
            {
                textBoxXPath.Text = ((DirectoryInfo)tempObject).FullName;
                //return;
            }
            if (tempObject is FileInfo)//////////////////////////////////////////////////////////////////////////ѡ��Ϊ�ļ�
            {
                textBoxXPath.Text = ((FileInfo)tempObject).FullName;
                //return;
            }
            if (tempObject is DriveInfo)//////////////////////////////////////////////////////////////////////////ѡ��Ϊ����
            {
                textBoxXPath.Text = ((DriveInfo)tempObject).Name;
                //return;
            }


            ListViewItem pListViewItem = null;
            //int relVal = 0;

            m_ListCollection.Clear();
            m_FileCollection.Clear();
            m_FeatClsCollection.Clear();
            m_TableCollection.Clear();

            string temItem = listViewExFiles.SelectedItems[0].Text;
            if (strFix == ".mdb")
            {
                if (temItem.Length >= 4 && temItem.Substring(temItem.Length - 4).ToLower() == ".mdb")
                {
                    return;
                }

                if (textBoxXPath.Text.ToLower().Contains(".mdb"))
                {
                    for (int i = 0; i < listViewExFiles.SelectedItems.Count; i++)
                    {
                        pListViewItem = listViewExFiles.SelectedItems[i];
                        m_ListCollection.Add(pListViewItem.Tag);
                        ////ӡ�� 20081201 ���MDB��Ҫ���������·��
                        //�򿪵���Ҫ�ؼ�
                        if (this.listViewExFiles.SelectedItems[i].Tag is IFeatureDatasetName)
                        {
                            IDatasetName pDSN = default(IDatasetName);
                            pDSN = this.listViewExFiles.SelectedItems[0].Tag as IDatasetName;
                            m_sFeatDatasetName = pDSN.Name;
                            ////���Ҫ�ؼ�����(m_sFeatDatasetName)
                            //�򿪵���Ҫ����
                        }
                        else
                        {
                            m_sFeatClsName = pListViewItem.Text;
                            //���Ҫ��������(m_sFeatClsName)
                        }
                        if (m_sFeatDatasetName == null)
                        {
                            m_sFullPath = m_sMdbPath + "\\" + m_sFeatClsName;
                        }
                        else
                        {
                            m_sFullPath = m_sMdbPath + "\\" + m_sFeatDatasetName + "\\" + m_sFeatClsName;
                        }
                        m_FileCollection.Add(m_sFullPath);
                    }
                }
            }
            else
            {
                if (strFix == ".gdb")
                {
                    if ((temItem.ToLower().Contains(".gdb") && temItem.Substring(temItem.Length - 4).ToLower() == ".gdb")
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
                    if (strFix == null)
                    {
                        if (listViewExFiles.SelectedItems.Count == 0)
                        {
                            return;
                        }
                        ListViewItem plistviewitem = null;
                        m_ListCollection.Clear();
                        for (int i = 0; i < listViewExFiles.SelectedItems.Count; i++)
                        {
                            plistviewitem = listViewExFiles.SelectedItems[i];
                            m_ListCollection.Add(plistviewitem.Tag);
                        }
                    }
                    else
                    {
                        for (int i = 0; i <= listViewExFiles.SelectedItems.Count - 1; i++)
                        {
                            pListViewItem = listViewExFiles.SelectedItems[i];

                            if (pListViewItem.Text.ToLower().Contains(strFix) &&
                                pListViewItem.Text.Substring(pListViewItem.Text.Length - 4).ToLower() == strFix)
                            {
                                //txtName.Text = txtName.Text + pListViewItem.Text + ";";
                                m_ListCollection.Add(pListViewItem.Text);
                                ////ӡ�� 20081129 ���shapefile�����ļ�·������
                                if (currentDirectory != null)
                                {
                                    m_sFullPath = currentDirectory.FullName + "\\" + pListViewItem.Text;

                                }
                                else
                                {
                                    m_sFullPath = pFinallyPath + "\\" + pListViewItem.Text;
                                }
                                //m_sFullPath = currentDirectory.FullName + "\\" + pListViewItem.Text;
                                m_sFilePath = m_sFullPath;
                                m_FileCollection.Add(m_sFullPath);
                            }
                        }
                    }
                }
            }




        }

        private void advTreeFiles_NodeClick(object sender, TreeNodeMouseEventArgs e)
        {
            DirectoryInfo directoryInfo = null;
            if (e.Node.Tag is DriveInfo)///////////////////////////////////�ڵ�Ϊ����
            {
                directoryInfo = ((DriveInfo)e.Node.Tag).RootDirectory;
                textBoxXPath.Text = directoryInfo.Name;
            }
            else
            {
                if (e.Node.Tag is DirectoryInfo)///////////////////////////�ڵ�ΪĿ¼
                {
                    directoryInfo = (DirectoryInfo)e.Node.Tag;
                    textBoxXPath.Text = directoryInfo.FullName;
                }
                else
                {
                    return;
                }
            }
            currentDirectory = directoryInfo;////////////////////////////��ǰĿ¼
            //m_LastPathName = directoryInfo.FullName;
            LoadFolderFileToList(directoryInfo);
        }


        /// <summary>
        /// �����textbox�еĻس��¼�
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
        /// �����б�(�г���ǰĿ¼�µ�Ŀ¼���ļ�)
        /// </summary>
        /// <param name="newPath"></param>
        private void ListUpdate(string newPath)
        {
            try
            {
                DirectoryInfo currentDir = new DirectoryInfo(newPath);
                //��ȡĿ¼
                DirectoryInfo[] dirs = currentDir.GetDirectories();
                //��ȡ�ļ�
                FileInfo[] files = currentDir.GetFiles();
                //ɾ��ImageList�еĳ���ͼ��
                foreach (ListViewItem item in listViewExFiles.Items)
                {
                    if (item.Text.EndsWith(".exe"))  //�ǳ���
                    {
                        imageList2.Images.RemoveByKey(item.Text);
                        imageList3.Images.RemoveByKey(item.Text);
                    }
                }
                //���listview����Ķ���
                listViewExFiles.Items.Clear();
                //�г��ļ���
                foreach (DirectoryInfo dir in dirs)
                {
                    ListViewItem dirItem = listViewExFiles.Items.Add(dir.Name, 2);
                    dirItem.Name = dir.FullName;
                    dirItem.SubItems.Add("");
                    dirItem.SubItems.Add("�ļ���");
                    dirItem.SubItems.Add(dir.LastWriteTimeUtc.ToString());
                }
                //�г��ļ�
                foreach (FileInfo file in files)
                {
                    ListViewItem fileItem = listViewExFiles.Items.Add(file.Name);
                    if (file.Extension == ".exe" || file.Extension == "")   //�����ļ�������չ��
                    {
                        System.Drawing.Icon fileIcon = ClsGetSysIcon.GetIconByFileName(file.FullName);
                        imageList2.Images.Add(file.Name, fileIcon);
                        imageList3.Images.Add(file.Name, fileIcon);
                        fileItem.ImageKey = file.Name;
                    }
                    else    //�����ļ�
                    {
                        if (!imageList2.Images.ContainsKey(file.Extension))  //ImageList�в����ڴ���ͼ��
                        {
                            System.Drawing.Icon fileIcon = ClsGetSysIcon.GetIconByFileName(file.FullName);
                            imageList2.Images.Add(file.Extension, fileIcon);
                            imageList3.Images.Add(file.Extension, fileIcon);
                        }
                        fileItem.ImageKey = file.Extension;
                    }
                    fileItem.Name = file.FullName;
                    fileItem.SubItems.Add(file.Length.ToString() + "�ֽ�");
                    fileItem.SubItems.Add(file.Extension);
                    fileItem.SubItems.Add(file.LastWriteTimeUtc.ToString());
                }
                currentPath = newPath;
                textBoxXPath.Text = currentPath;   //���µ�ַ��
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




    }
}