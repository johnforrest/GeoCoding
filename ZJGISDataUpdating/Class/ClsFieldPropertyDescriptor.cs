using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace ZJGISDataUpdating
{
    class ClsFieldPropertyDescriptor : PropertyDescriptor
    {
        int m_FieldIndex;
        Type m_NetType;
        Type m_ActualType;

        esriFieldType m_esriFieldType;
        bool m_IsEditable=true;

        IWorkspaceEdit m_WorkspaceEdit;
        ICodedValueDomain m_CodedValueDomain;

        bool m_UseCVDomain;
        TypeConverter m_ActualValueConverter;
        TypeConverter m_CVDomainValueDesConverter;

        public ClsFieldPropertyDescriptor(ITable table, string fieldName, int fieldIndex)
            : base(fieldName, null)
        {
            m_FieldIndex = fieldIndex;
            IField pField = table.Fields.get_Field(fieldIndex);

            m_esriFieldType = pField.Type;
            m_IsEditable = pField.Editable && (m_esriFieldType != esriFieldType.esriFieldTypeBlob) && (m_esriFieldType != esriFieldType.esriFieldTypeGeometry);

            m_NetType = m_ActualType = EsriFieldTypeToSystemType(pField);
            m_WorkspaceEdit = ((IDataset)table).Workspace as IWorkspaceEdit;

        }

        public ClsFieldPropertyDescriptor(IFields fields, string fieldName, int fieldIndex):base(fieldName,null)
        {
            m_FieldIndex = fieldIndex;
            IField pField = fields.get_Field(fieldIndex);
            m_esriFieldType = pField.Type;

            m_IsEditable = pField.Editable && (m_esriFieldType != esriFieldType.esriFieldTypeBlob) && (m_esriFieldType != esriFieldType.esriFieldTypeGeometry);
            m_NetType = m_ActualType = EsriFieldTypeToSystemType(pField);
            m_WorkspaceEdit = null;
        }
        public bool HasCVDomain
        {
            get
            {
                return m_CodedValueDomain != null;
            }

        }
        public bool UseCVDomain
        {
            set
            {
                m_UseCVDomain = value;
                if (value)
                {
                    m_NetType = typeof(string);
                }
                else
                {
                    m_NetType = m_ActualType;
                }
            }
        }
        public override TypeConverter Converter
        {
            get
            {
                TypeConverter retVal = null;
                if (m_CodedValueDomain != null)
                {
                    if (m_UseCVDomain)
                    {
                        if (m_CVDomainValueDesConverter == null)
                            m_CVDomainValueDesConverter = TypeDescriptor.GetConverter(typeof(string));
                        retVal = m_CVDomainValueDesConverter;
                    }
                    else
                    {
                        if (m_ActualValueConverter == null)
                        {
                            m_ActualValueConverter = TypeDescriptor.GetConverter(m_ActualType);
                        }
                        retVal = m_ActualValueConverter;
                    }
                }
                else
                {
                    retVal = base.Converter;
                }
                return retVal;
            }
        }

        public override bool CanResetValue(object component)
        {
            return false;
        }
        public override Type ComponentType
        {
            get 
            { 
                return typeof(IRow);
            }
        }
        public override bool IsReadOnly
        {
            get { return !m_IsEditable; }
        }
        public override Type PropertyType
        {
            get { return m_NetType; }
        }
        public override void SetValue(object component, object value)
        {
            IRow pRow = component as IRow;
            if (m_CodedValueDomain != null)
            {
                if (!m_UseCVDomain)
                {
                    if (!((IDomain)m_CodedValueDomain).MemberOf(value))
                    {
                        System.Windows.Forms.MessageBox.Show(string.Format(
                  "Value {0} is not valid for coded value domain {1}", value.ToString(), ((IDomain)m_CodedValueDomain).Name));
                        return;
                    }
                }
                else
                {
                    bool foundMatch = false;
                    for (int i = 0; i < m_CodedValueDomain.CodeCount; i++)
                    {
                        if (value.ToString() == m_CodedValueDomain.get_Name(i))
                        {
                            foundMatch = true;
                            value = i;
                            break;
                        }
                    }

                    if (!foundMatch)
                    {
                        System.Windows.Forms.MessageBox.Show(string.Format(
                  "Value {0} is not valid for coded value domain {1}", value.ToString(), ((IDomain)m_CodedValueDomain).Name));
                        return;
                    }
                }
            }

            pRow.set_Value(m_FieldIndex,value);
            bool weStartedEditing = false;
            if (m_WorkspaceEdit != null)
            {
                if (!m_WorkspaceEdit.IsBeingEdited())
                {
                    m_WorkspaceEdit.StartEditing(false);
                    weStartedEditing = true;
                }
                m_WorkspaceEdit.StartEditOperation();
                pRow.Store();
                m_WorkspaceEdit.StopEditOperation();

                if (weStartedEditing)
                    m_WorkspaceEdit.StopEditing(true);

            }
        }

        public override object GetValue(object component)
        {
            object retVal = null;
            IRow pRow = component as IRow;
            try
            {
                object value = pRow.get_Value(m_FieldIndex);

                if ((m_CodedValueDomain != null) && m_UseCVDomain)
                {
                    value = m_CodedValueDomain.get_Name(Convert.ToInt32(value));
                }
                switch (m_esriFieldType)
                {
                    case esriFieldType.esriFieldTypeBlob:
                        retVal = "Blob";
                        break;

                    case esriFieldType.esriFieldTypeGeometry:
                        retVal = GetGeometryTypeAsString(value);
                        break;

                    case esriFieldType.esriFieldTypeRaster:
                        retVal = "Raster";
                        break;

                    default:
                        retVal = value;
                        break;
                }
            }
            catch (Exception)
            {
                
                throw;
            }

            return retVal;
        }
        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }
        private Type EsriFieldTypeToSystemType(IField field)
        {
            esriFieldType esriType = field.Type;
            m_CodedValueDomain = field.Domain as ICodedValueDomain;
            if ((m_CodedValueDomain != null) && m_UseCVDomain == true)
            {
                return typeof(string);
            }

            try
            {
                switch (esriType)
                {
                    case esriFieldType.esriFieldTypeBlob:
                        //beyond scope of sample to deal with blob fields
                        return typeof(string);
                    case esriFieldType.esriFieldTypeDate:
                        return typeof(DateTime);
                    case esriFieldType.esriFieldTypeDouble:
                        return typeof(double);
                    case esriFieldType.esriFieldTypeGeometry:
                        return typeof(string);
                    case esriFieldType.esriFieldTypeGlobalID:
                        return typeof(string);
                    case esriFieldType.esriFieldTypeGUID:
                        return typeof(Guid);
                    case esriFieldType.esriFieldTypeInteger:
                        return typeof(Int32);
                    case esriFieldType.esriFieldTypeOID:
                        return typeof(Int32);
                    case esriFieldType.esriFieldTypeRaster:
                        //beyond scope of sample to correctly display rasters
                        return typeof(string);
                    case esriFieldType.esriFieldTypeSingle:
                        return typeof(Single);
                    case esriFieldType.esriFieldTypeSmallInteger:
                        return typeof(Int16);
                    case esriFieldType.esriFieldTypeString:
                        return typeof(string);
                    default:
                        return typeof(string);
                
                }

            }
            catch (Exception ex)
            {

                System.Diagnostics.Debug.WriteLine(ex.Message);
                return typeof(string);
            }
        }
        private string GetGeometryTypeAsString(object value)
        {
            string retVal = "";
            IGeometry geometry = value as IGeometry;
            if (geometry != null)
            {
                retVal = geometry.GeometryType.ToString();
            }
            return retVal;
        
        }
        public override void ResetValue(object component)
        {

        }
    }
}
