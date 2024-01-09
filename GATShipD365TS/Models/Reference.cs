using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GATShipD365TS.GATShip.Entities
{

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/dynamics/2008/01/documents/Fault")]
    public partial class AifFault : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string customDetailXmlField;

        private FaultMessageList[] faultMessageListArrayField;

        private InfologMessage[] infologMessageListField;

        private string stackTraceField;

        private int xppExceptionTypeField;

        private bool xppExceptionTypeFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 0)]
        public string CustomDetailXml
        {
            get
            {
                return this.customDetailXmlField;
            }
            set
            {
                this.customDetailXmlField = value;
                this.RaisePropertyChanged("CustomDetailXml");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(IsNullable = true, Order = 1)]
        public FaultMessageList[] FaultMessageListArray
        {
            get
            {
                return this.faultMessageListArrayField;
            }
            set
            {
                this.faultMessageListArrayField = value;
                this.RaisePropertyChanged("FaultMessageListArray");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(IsNullable = true, Order = 2)]
        [System.Xml.Serialization.XmlArrayItemAttribute(Namespace = "http://schemas.datacontract.org/2004/07/Microsoft.Dynamics.AX.Framework.Services")]
        public InfologMessage[] InfologMessageList
        {
            get
            {
                return this.infologMessageListField;
            }
            set
            {
                this.infologMessageListField = value;
                this.RaisePropertyChanged("InfologMessageList");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 3)]
        public string StackTrace
        {
            get
            {
                return this.stackTraceField;
            }
            set
            {
                this.stackTraceField = value;
                this.RaisePropertyChanged("StackTrace");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public int XppExceptionType
        {
            get
            {
                return this.xppExceptionTypeField;
            }
            set
            {
                this.xppExceptionTypeField = value;
                this.RaisePropertyChanged("XppExceptionType");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool XppExceptionTypeSpecified
        {
            get
            {
                return this.xppExceptionTypeFieldSpecified;
            }
            set
            {
                this.xppExceptionTypeFieldSpecified = value;
                this.RaisePropertyChanged("XppExceptionTypeSpecified");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/dynamics/2008/01/documents/Fault")]
    public partial class FaultMessageList : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string documentField;

        private string documentOperationField;

        private FaultMessage[] faultMessageArrayField;

        private string fieldField;

        private string serviceField;

        private string serviceOperationField;

        private string serviceOperationParameterField;

        private string xPathField;

        private string xmlLineField;

        private string xmlPositionField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 0)]
        public string Document
        {
            get
            {
                return this.documentField;
            }
            set
            {
                this.documentField = value;
                this.RaisePropertyChanged("Document");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 1)]
        public string DocumentOperation
        {
            get
            {
                return this.documentOperationField;
            }
            set
            {
                this.documentOperationField = value;
                this.RaisePropertyChanged("DocumentOperation");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(IsNullable = true, Order = 2)]
        public FaultMessage[] FaultMessageArray
        {
            get
            {
                return this.faultMessageArrayField;
            }
            set
            {
                this.faultMessageArrayField = value;
                this.RaisePropertyChanged("FaultMessageArray");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 3)]
        public string Field
        {
            get
            {
                return this.fieldField;
            }
            set
            {
                this.fieldField = value;
                this.RaisePropertyChanged("Field");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 4)]
        public string Service
        {
            get
            {
                return this.serviceField;
            }
            set
            {
                this.serviceField = value;
                this.RaisePropertyChanged("Service");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 5)]
        public string ServiceOperation
        {
            get
            {
                return this.serviceOperationField;
            }
            set
            {
                this.serviceOperationField = value;
                this.RaisePropertyChanged("ServiceOperation");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 6)]
        public string ServiceOperationParameter
        {
            get
            {
                return this.serviceOperationParameterField;
            }
            set
            {
                this.serviceOperationParameterField = value;
                this.RaisePropertyChanged("ServiceOperationParameter");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 7)]
        public string XPath
        {
            get
            {
                return this.xPathField;
            }
            set
            {
                this.xPathField = value;
                this.RaisePropertyChanged("XPath");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 8)]
        public string XmlLine
        {
            get
            {
                return this.xmlLineField;
            }
            set
            {
                this.xmlLineField = value;
                this.RaisePropertyChanged("XmlLine");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 9)]
        public string XmlPosition
        {
            get
            {
                return this.xmlPositionField;
            }
            set
            {
                this.xmlPositionField = value;
                this.RaisePropertyChanged("XmlPosition");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/dynamics/2008/01/documents/Fault")]
    public partial class FaultMessage : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string codeField;

        private string messageField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 0)]
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
                this.RaisePropertyChanged("Code");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 1)]
        public string Message
        {
            get
            {
                return this.messageField;
            }
            set
            {
                this.messageField = value;
                this.RaisePropertyChanged("Message");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.datacontract.org/2004/07/Microsoft.Dynamics.AX.Framework.Services")]
    public partial class InfologMessage : object, System.ComponentModel.INotifyPropertyChanged
    {

        private InfologMessageType infologMessageTypeField;

        private bool infologMessageTypeFieldSpecified;

        private string messageField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public InfologMessageType InfologMessageType
        {
            get
            {
                return this.infologMessageTypeField;
            }
            set
            {
                this.infologMessageTypeField = value;
                this.RaisePropertyChanged("InfologMessageType");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool InfologMessageTypeSpecified
        {
            get
            {
                return this.infologMessageTypeFieldSpecified;
            }
            set
            {
                this.infologMessageTypeFieldSpecified = value;
                this.RaisePropertyChanged("InfologMessageTypeSpecified");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 1)]
        public string Message
        {
            get
            {
                return this.messageField;
            }
            set
            {
                this.messageField = value;
                this.RaisePropertyChanged("Message");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.datacontract.org/2004/07/Microsoft.Dynamics.AX.Framework.Services")]
    public enum InfologMessageType
    {

        /// <remarks/>
        Info,

        /// <remarks/>
        Warning,

        /// <remarks/>
        Error,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/dynamics/2006/02/documents/EntityKey")]
    public partial class KeyField : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string fieldField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string Field
        {
            get
            {
                return this.fieldField;
            }
            set
            {
                this.fieldField = value;
                this.RaisePropertyChanged("Field");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
                this.RaisePropertyChanged("Value");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/dynamics/2006/02/documents/EntityKey")]
    public partial class EntityKey : object, System.ComponentModel.INotifyPropertyChanged
    {

        private KeyField[] keyDataField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 0)]
        [System.Xml.Serialization.XmlArrayItemAttribute(IsNullable = false)]
        public KeyField[] KeyData
        {
            get
            {
                return this.keyDataField;
            }
            set
            {
                this.keyDataField = value;
                this.RaisePropertyChanged("KeyData");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/dynamics/2008/01/documents/THK_AIFA3GLJournal")]
    public partial class Entity_THK_AIFA3GLLine : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string accountField;

        private string accountTypeField;

        private System.Nullable<decimal> amountField;

        private bool amountFieldSpecified;

        private string bankTransTypeField;

        private string batchIDField;

        private string createdByUserIdField;

        private string currencyField;

        private System.Nullable<System.DateTime> dateField;

        private bool dateFieldSpecified;

        private string dim1;

        private string dim2;

        private string dim3;

        private string dim4;

        private string dim5;

        private string dim6;

        private System.Nullable<System.DateTime> documentDateField;

        private bool documentDateFieldSpecified;

        private string documentNumField;

        private System.Nullable<System.DateTime> dueField;

        private bool dueFieldSpecified;

        private System.Nullable<decimal> exchRateField;

        private bool exchRateFieldSpecified;

        private string invoiceField;

        private string lineDescriptionField;

        private string paymentField;

        private System.Nullable<decimal> salesTaxAmountField;

        private bool salesTaxAmountFieldSpecified;

        private string taxGroupField;

        private string taxItemGroupField;

        private string userField1;

        private string userField2;

        private string userField3;

        private string remark;

        private string revenueType;

        private string vesselCustomercode;

        private string voyageCode;

        private string classField;

        private Enum_AxdEntityAction actionField;

        private bool actionFieldSpecified;

        public Entity_THK_AIFA3GLLine()
        {
            this.classField = "entity";
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 0)]
        public string Account
        {
            get
            {
                return this.accountField;
            }
            set
            {
                this.accountField = value;
                this.RaisePropertyChanged("Account");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 1)]
        public string AccountType
        {
            get
            {
                return this.accountTypeField;
            }
            set
            {
                this.accountTypeField = value;
                this.RaisePropertyChanged("AccountType");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 2)]
        public System.Nullable<decimal> Amount
        {
            get
            {
                return this.amountField;
            }
            set
            {
                this.amountField = value;
                this.RaisePropertyChanged("Amount");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool AmountSpecified
        {
            get
            {
                return this.amountFieldSpecified;
            }
            set
            {
                this.amountFieldSpecified = value;
                this.RaisePropertyChanged("AmountSpecified");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 3)]
        public string BankTransType
        {
            get
            {
                return this.bankTransTypeField;
            }
            set
            {
                this.bankTransTypeField = value;
                this.RaisePropertyChanged("BankTransType");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 4)]
        public string FileBatchID
        {
            get
            {
                return this.batchIDField;
            }
            set
            {
                this.batchIDField = value;
                this.RaisePropertyChanged("FileBatchID");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 5)]
        public string CreatedByUserId
        {
            get
            {
                return this.createdByUserIdField;
            }
            set
            {
                this.createdByUserIdField = value;
                this.RaisePropertyChanged("CreatedByUserId");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 6)]
        public string Currency
        {
            get
            {
                return this.currencyField;
            }
            set
            {
                this.currencyField = value;
                this.RaisePropertyChanged("Currency");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date", IsNullable = true, Order = 7)]
        public System.Nullable<System.DateTime> TransDate
        {
            get
            {
                return this.dateField;
            }
            set
            {
                this.dateField = value;
                this.RaisePropertyChanged("TransDate");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DateSpecified
        {
            get
            {
                return this.dateFieldSpecified;
            }
            set
            {
                this.dateFieldSpecified = value;
                this.RaisePropertyChanged("DateSpecified");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 8)]
        public string DIM1
        {
            get
            {
                return this.dim1;
            }
            set
            {
                this.dim1 = value;
                this.RaisePropertyChanged("DIM1");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 9)]
        public string DIM2
        {
            get
            {
                return this.dim2;
            }
            set
            {
                this.dim2 = value;
                this.RaisePropertyChanged("DIM2");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 10)]
        public string DIM3
        {
            get
            {
                return this.dim3;
            }
            set
            {
                this.dim3 = value;
                this.RaisePropertyChanged("DIM3");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 11)]
        public string DIM4
        {
            get
            {
                return this.dim4;
            }
            set
            {
                this.dim4 = value;
                this.RaisePropertyChanged("DIM4");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 12)]
        public string DIM5
        {
            get
            {
                return this.dim5;
            }
            set
            {
                this.dim5 = value;
                this.RaisePropertyChanged("DIM5");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 13)]
        public string DIM6
        {
            get
            {
                return this.dim6;
            }
            set
            {
                this.dim6 = value;
                this.RaisePropertyChanged("DIM6");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date", IsNullable = true, Order = 14)]
        public System.Nullable<System.DateTime> DocumentDate
        {
            get
            {
                return this.documentDateField;
            }
            set
            {
                this.documentDateField = value;
                this.RaisePropertyChanged("DocumentDate");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DocumentDateSpecified
        {
            get
            {
                return this.documentDateFieldSpecified;
            }
            set
            {
                this.documentDateFieldSpecified = value;
                this.RaisePropertyChanged("DocumentDateSpecified");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 15)]
        public string Document
        {
            get
            {
                return this.documentNumField;
            }
            set
            {
                this.documentNumField = value;
                this.RaisePropertyChanged("Document");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date", IsNullable = true, Order = 16)]
        public System.Nullable<System.DateTime> DueDate
        {
            get
            {
                return this.dueField;
            }
            set
            {
                this.dueField = value;
                this.RaisePropertyChanged("DueDate");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DueSpecified
        {
            get
            {
                return this.dueFieldSpecified;
            }
            set
            {
                this.dueFieldSpecified = value;
                this.RaisePropertyChanged("DueSpecified");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 17)]
        public System.Nullable<decimal> ExchangeRate
        {
            get
            {
                return this.exchRateField;
            }
            set
            {
                this.exchRateField = value;
                this.RaisePropertyChanged("ExchangeRate");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ExchRateSpecified
        {
            get
            {
                return this.exchRateFieldSpecified;
            }
            set
            {
                this.exchRateFieldSpecified = value;
                this.RaisePropertyChanged("ExchRateSpecified");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 18)]
        public string Invoice
        {
            get
            {
                return this.invoiceField;
            }
            set
            {
                this.invoiceField = value;
                this.RaisePropertyChanged("Invoice");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 19)]
        public string LineDescription
        {
            get
            {
                return this.lineDescriptionField;
            }
            set
            {
                this.lineDescriptionField = value;
                this.RaisePropertyChanged("LineDescription");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 20)]
        public string Payment
        {
            get
            {
                return this.paymentField;
            }
            set
            {
                this.paymentField = value;
                this.RaisePropertyChanged("Payment");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 21)]
        public System.Nullable<decimal> SalesTaxAmount
        {
            get
            {
                return this.salesTaxAmountField;
            }
            set
            {
                this.salesTaxAmountField = value;
                this.RaisePropertyChanged("SalesTaxAmount");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool SalesTaxAmountSpecified
        {
            get
            {
                return this.salesTaxAmountFieldSpecified;
            }
            set
            {
                this.salesTaxAmountFieldSpecified = value;
                this.RaisePropertyChanged("SalesTaxAmountSpecified");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 22)]
        public string SalesTaxGroup
        {
            get
            {
                return this.taxGroupField;
            }
            set
            {
                this.taxGroupField = value;
                this.RaisePropertyChanged("SalesTaxGroup");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 23)]
        public string ItemSalesTaxGroup
        {
            get
            {
                return this.taxItemGroupField;
            }
            set
            {
                this.taxItemGroupField = value;
                this.RaisePropertyChanged("ItemSalesTaxGroup");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 24)]
        public string UserField1
        {
            get
            {
                return this.userField1;
            }
            set
            {
                this.userField1 = value;
                this.RaisePropertyChanged("UserField1");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 25)]
        public string UserField2
        {
            get
            {
                return this.userField2;
            }
            set
            {
                this.userField2 = value;
                this.RaisePropertyChanged("UserField2");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 26)]
        public string UserField3
        {
            get
            {
                return this.userField3;
            }
            set
            {
                this.userField3 = value;
                this.RaisePropertyChanged("UserField3");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 27)]
        public string Remark
        {
            get
            {
                return this.remark;
            }
            set
            {
                this.remark = value;
                this.RaisePropertyChanged("Remark");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 28)]
        public string RevenueType
        {
            get
            {
                return this.revenueType;
            }
            set
            {
                this.remark = value;
                this.RaisePropertyChanged("RevenueType");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 29)]
        public string VesselCustomerCode
        {
            get
            {
                return this.vesselCustomercode;
            }
            set
            {
                this.remark = value;
                this.RaisePropertyChanged("VesselCustomerCode");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 30)]
        public string VoyageCode
        {
            get
            {
                return this.voyageCode;
            }
            set
            {
                this.remark = value;
                this.RaisePropertyChanged("VoyageCode");
            }
        }


        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string @class
        {
            get
            {
                return this.classField;
            }
            set
            {
                this.classField = value;
                this.RaisePropertyChanged("class");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public Enum_AxdEntityAction action
        {
            get
            {
                return this.actionField;
            }
            set
            {
                this.actionField = value;
                this.RaisePropertyChanged("action");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool actionSpecified
        {
            get
            {
                return this.actionFieldSpecified;
            }
            set
            {
                this.actionFieldSpecified = value;
                this.RaisePropertyChanged("actionSpecified");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/dynamics/2008/01/sharedtypes")]
    public enum Enum_AxdEntityAction
    {

        /// <remarks/>
        create,

        /// <remarks/>
        update,

        /// <remarks/>
        replace,

        /// <remarks/>
        delete,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/dynamics/2008/01/documents/THK_AIFA3GLJournal")]
    public partial class Entity_THK_AIFA3GLHeader : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string _DocumentHashField;

        private string batchIDField;

        private string companyCodeField;
        private string transType;

        private string descriptionField;

        private Entity_THK_AIFA3GLLine[] tHK_AIFA3GLLineField;

        private string classField;

        private Enum_AxdEntityAction actionField;

        private bool actionFieldSpecified;

        public Entity_THK_AIFA3GLHeader()
        {
            this.classField = "entity";
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 0)]
        public string _DocumentHash
        {
            get
            {
                return this._DocumentHashField;
            }
            set
            {
                this._DocumentHashField = value;
                this.RaisePropertyChanged("_DocumentHash");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 1)]
        public string FileBatchID
        {
            get
            {
                return this.batchIDField;
            }
            set
            {
                this.batchIDField = value;
                this.RaisePropertyChanged("BatchID");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 2)]
        public string CompanyCode
        {
            get
            {
                return this.companyCodeField;
            }
            set
            {
                this.companyCodeField = value;
                this.RaisePropertyChanged("CompanyCode");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 3)]
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
                this.RaisePropertyChanged("Description");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("THK_AIFA3GLLine", Order = 4)]
        public Entity_THK_AIFA3GLLine[] THK_AIFA3GLLine
        {
            get
            {
                return this.tHK_AIFA3GLLineField;
            }
            set
            {
                this.tHK_AIFA3GLLineField = value;
                this.RaisePropertyChanged("THK_AIFA3GLLine");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 5)]
        public string TransType
        {
            get
            {
                return this.transType;
            }
            set
            {
                this.companyCodeField = value;
                this.RaisePropertyChanged("TransType");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string @class
        {
            get
            {
                return this.classField;
            }
            set
            {
                this.classField = value;
                this.RaisePropertyChanged("class");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public Enum_AxdEntityAction action
        {
            get
            {
                return this.actionField;
            }
            set
            {
                this.actionField = value;
                this.RaisePropertyChanged("action");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool actionSpecified
        {
            get
            {
                return this.actionFieldSpecified;
            }
            set
            {
                this.actionFieldSpecified = value;
                this.RaisePropertyChanged("actionSpecified");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/dynamics/2008/01/documents/THK_AIFA3GLJournal")]
    public partial class THK_AIFA3GLJournal : object, System.ComponentModel.INotifyPropertyChanged
    {

        private System.Nullable<Enum_boolean> clearNilFieldsOnUpdateField;

        private bool clearNilFieldsOnUpdateFieldSpecified;

        private System.Nullable<Enum_XMLDocPurpose> docPurposeField;

        private bool docPurposeFieldSpecified;

        private string senderIdField;

        private Entity_THK_AIFA3GLHeader[] tHK_AIFA3GLHeaderField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 0)]
        public System.Nullable<Enum_boolean> ClearNilFieldsOnUpdate
        {
            get
            {
                return this.clearNilFieldsOnUpdateField;
            }
            set
            {
                this.clearNilFieldsOnUpdateField = value;
                this.RaisePropertyChanged("ClearNilFieldsOnUpdate");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ClearNilFieldsOnUpdateSpecified
        {
            get
            {
                return this.clearNilFieldsOnUpdateFieldSpecified;
            }
            set
            {
                this.clearNilFieldsOnUpdateFieldSpecified = value;
                this.RaisePropertyChanged("ClearNilFieldsOnUpdateSpecified");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 1)]
        public System.Nullable<Enum_XMLDocPurpose> DocPurpose
        {
            get
            {
                return this.docPurposeField;
            }
            set
            {
                this.docPurposeField = value;
                this.RaisePropertyChanged("DocPurpose");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DocPurposeSpecified
        {
            get
            {
                return this.docPurposeFieldSpecified;
            }
            set
            {
                this.docPurposeFieldSpecified = value;
                this.RaisePropertyChanged("DocPurposeSpecified");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 2)]
        public string SenderId
        {
            get
            {
                return this.senderIdField;
            }
            set
            {
                this.senderIdField = value;
                this.RaisePropertyChanged("SenderId");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("THK_AIFA3GLHeader", Order = 3)]
        public Entity_THK_AIFA3GLHeader[] THK_AIFA3GLHeader
        {
            get
            {
                return this.tHK_AIFA3GLHeaderField;
            }
            set
            {
                this.tHK_AIFA3GLHeaderField = value;
                this.RaisePropertyChanged("THK_AIFA3GLHeader");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/dynamics/2008/01/sharedtypes")]
    public enum Enum_boolean
    {

        /// <remarks/>
        @false,

        /// <remarks/>
        @true,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/dynamics/2008/01/sharedtypes")]
    public enum Enum_XMLDocPurpose
    {

        /// <remarks/>
        Original,

        /// <remarks/>
        Duplicate,

        /// <remarks/>
        Proforma,
    }
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/dynamics/2010/01/datacontracts")]
    public partial class CallContext : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string companyField;

        private string languageField;

        private string logonAsUserField;

        private string messageIdField;

        private string partitionKeyField;

        private ArrayOfKeyValueOfstringstringKeyValueOfstringstring[] propertyBagField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 0)]
        public string Company
        {
            get
            {
                return this.companyField;
            }
            set
            {
                this.companyField = value;
                this.RaisePropertyChanged("Company");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 1)]
        public string Language
        {
            get
            {
                return this.languageField;
            }
            set
            {
                this.languageField = value;
                this.RaisePropertyChanged("Language");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 2)]
        public string LogonAsUser
        {
            get
            {
                return this.logonAsUserField;
            }
            set
            {
                this.logonAsUserField = value;
                this.RaisePropertyChanged("LogonAsUser");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 3)]
        public string MessageId
        {
            get
            {
                return this.messageIdField;
            }
            set
            {
                this.messageIdField = value;
                this.RaisePropertyChanged("MessageId");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 4)]
        public string PartitionKey
        {
            get
            {
                return this.partitionKeyField;
            }
            set
            {
                this.partitionKeyField = value;
                this.RaisePropertyChanged("PartitionKey");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(IsNullable = true, Order = 5)]
        [System.Xml.Serialization.XmlArrayItemAttribute("KeyValueOfstringstring", Namespace = "http://schemas.microsoft.com/2003/10/Serialization/Arrays", IsNullable = false)]
        public ArrayOfKeyValueOfstringstringKeyValueOfstringstring[] PropertyBag
        {
            get
            {
                return this.propertyBagField;
            }
            set
            {
                this.propertyBagField = value;
                this.RaisePropertyChanged("PropertyBag");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/2003/10/Serialization/Arrays")]
    public partial class ArrayOfKeyValueOfstringstringKeyValueOfstringstring : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string keyField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 0)]
        public string Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
                this.RaisePropertyChanged("Key");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 1)]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
                this.RaisePropertyChanged("Value");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }    
}
