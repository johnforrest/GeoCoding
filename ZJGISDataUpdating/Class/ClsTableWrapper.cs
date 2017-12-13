using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using ESRI.ArcGIS.Geodatabase;

namespace ZJGISDataUpdating
{
    class ClsTableWrapper : BindingList<IRow>, ITypedList
    {
        ITable m_WrappedTable;

        List<PropertyDescriptor> m_PropertiesList = new List<PropertyDescriptor>();
        IWorkspaceEdit m_WorkspaceEdit;

        public ClsTableWrapper(ITable table)
        {
            m_WrappedTable = table;
            GenerateFakeProperties();
            AddData();

            m_WorkspaceEdit = ((IDataset)m_WrappedTable).Workspace as IWorkspaceEdit;

            AllowNew = true;
            AllowRemove = true;
        
        }

        public ClsTableWrapper(IFields fields)
        {
            m_WorkspaceEdit = null;
            //m_WrappedTable = table;

            for (int i = 0; i < fields.FieldCount; i++)
            {
                ClsFieldPropertyDescriptor newPropertyDesc = new ClsFieldPropertyDescriptor(fields, fields.get_Field(i).Name, i);
                m_PropertiesList.Add(newPropertyDesc);
            }
            AllowNew = true;
            AllowRemove = true;
        }
        /// <summary>
        /// 生成伪属性字段
        /// </summary>
        private void GenerateFakeProperties()
        {
            for (int i = 0; i < m_WrappedTable.Fields.FieldCount; i++)
            {
                ClsFieldPropertyDescriptor newPropertyDesc = new ClsFieldPropertyDescriptor(m_WrappedTable,m_WrappedTable.Fields.get_Field(i).Name,i);
                m_PropertiesList.Add(newPropertyDesc);
            }
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        private void AddData()
        {
            ICursor pCursor = m_WrappedTable.Search(null,false);
            //test
            int test = m_WrappedTable.RowCount(null);
            IRow curRow = pCursor.NextRow();
            while (curRow != null)
            {
                Add(curRow);
                //test2
                int te = curRow.OID;
                curRow = pCursor.NextRow();
            }
        }

        public PropertyDescriptorCollection GetItemProperties(PropertyDescriptor[] listAccessors)
        {

            PropertyDescriptorCollection propCollection = null;
            if (listAccessors == null)
            {
                propCollection = new PropertyDescriptorCollection(m_PropertiesList.ToArray());

            }
            else
            {
                List<PropertyDescriptor> tempList = new List<PropertyDescriptor>();
                foreach (PropertyDescriptor curPropDesc in listAccessors)
                {
                    if (m_PropertiesList.Contains(curPropDesc))
                    {
                        tempList.Add(curPropDesc);
                    }

                }
                propCollection = new PropertyDescriptorCollection(tempList.ToArray());
            }
            return propCollection;
        }
        public string GetListName(PropertyDescriptor[] listAccessors)
        {
            return ((IDataset)m_WrappedTable).Name;
        }
        public bool UseCVDomain
        {
            set
            {
                foreach (ClsFieldPropertyDescriptor curPropDesc in m_PropertiesList)
                {
                    if (curPropDesc.HasCVDomain)
                    {
                        curPropDesc.UseCVDomain = value;
                    }
                }
            }
        }
        protected override void OnAddingNew(AddingNewEventArgs e)
        {
            if (AllowNew)
            {
                IRow newRow = m_WrappedTable.CreateRow();
                e.NewObject = newRow;

                for (int i = 0; i < newRow.Fields.FieldCount; i++)
                {
                    IField curField = newRow.Fields.get_Field(i);
                    if (curField.Editable && curField.IsNullable)
                    {
                        newRow.set_Value(i,curField.DefaultValue);
                    }
                }
                bool weStartedEditing = StartEditOp();
                newRow.Store();
                StopEditOp(weStartedEditing);

                base.OnAddingNew(e);
            }
        }
        protected override void RemoveItem(int index)
        {
            if (AllowRemove)
            {
                IRow itemToRemove = Items[index];
                int OID = itemToRemove.OID;

                bool weStartedEditing = StartEditOp();

                // Delete the row
                m_WrappedTable.GetRow(OID).Delete();
                //Items[index].Delete();

                StopEditOp(weStartedEditing);

                base.RemoveItem(index);
            }

        }
        public void RemoveRow(int index)
        {
            RemoveItem(index);
        
        }

        private bool StartEditOp()
        {
            bool retVal = false;
            if (!m_WorkspaceEdit.IsBeingEdited())
            {
                m_WorkspaceEdit.StartEditing(false);
                retVal = true;
            }
            m_WorkspaceEdit.StartEditOperation();
            return retVal;
        }

        private void StopEditOp(bool weStartedEditing)
        {
            m_WorkspaceEdit.StopEditOperation();
            if (weStartedEditing)
            {
                m_WorkspaceEdit.StopEditing(true);

            }
        }
    }
}
