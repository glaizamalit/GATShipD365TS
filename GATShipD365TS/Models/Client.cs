namespace GATShipD365TS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Client")]
    public partial class Client
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Client()
        {
            Client1 = new HashSet<Client>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        [StringLength(10)]
        public string NATIONAL_CODES_ID { get; set; }

        [StringLength(200)]
        public string NAME { get; set; }

        [StringLength(50)]
        public string ALPHANAME { get; set; }

        [StringLength(70)]
        public string STREET_ADDRESS { get; set; }

        [StringLength(70)]
        public string PBOX_ADDRESS { get; set; }

        [StringLength(70)]
        public string POSTAL_ADDRESS { get; set; }

        [StringLength(200)]
        public string PHONE { get; set; }

        [StringLength(200)]
        public string FAX { get; set; }

        [StringLength(200)]
        public string TELEX { get; set; }

        [StringLength(1000)]
        public string EMAIL { get; set; }

        [StringLength(100)]
        public string WEB { get; set; }

        public int? ADVANCE { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? REBATE_PERCENT { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? REBATE_FIXED { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? FIXED_FEE { get; set; }

        [StringLength(50)]
        public string ACCOUNT_NO { get; set; }

        [StringLength(250)]
        public string INSTRUCTIONS { get; set; }

        [StringLength(250)]
        public string COMMENTS { get; set; }

        [StringLength(50)]
        public string HISTORY { get; set; }

        public string ADDRESS { get; set; }

        public short? INACTIVE { get; set; }

        public string NEW_COMMENTS { get; set; }

        public string NEW_INSTRUCTIONS { get; set; }

        [StringLength(50)]
        public string BANK_ACCOUNT { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? HANDLING_PERCENT { get; set; }

        [StringLength(30)]
        public string VAT_NO { get; set; }

        public DateTime? LAST_CHANGED { get; set; }

        public DateTime? DATE_ADDED { get; set; }

        [StringLength(4000)]
        public string BANK_DETAILS { get; set; }

        public DateTime? EXPORTED { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? CREDIT_OUTSTANDING { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? CREDIT_LIMIT { get; set; }

        public int? PROTECT_ADDRESS_FROM_OVERWRITE { get; set; }

        public int? CREDIT_TERM { get; set; }

        public int? CLIENT_CURRENCY { get; set; }

        [StringLength(257)]
        public string INVOICE_TEMPLATE { get; set; }

        [StringLength(257)]
        public string CREDIT_NOTE_TEMPLATE { get; set; }

        [StringLength(100)]
        public string USERNAME { get; set; }

        [StringLength(100)]
        public string PASSWORD { get; set; }

        public short? WEBMODULEACCESS { get; set; }

        public int? WEB_SOF { get; set; }

        public int? WEB_DOCUMENTS { get; set; }

        public int? WEB_EXPENSES { get; set; }

        public int? CATEGORY { get; set; }

        [StringLength(50)]
        public string EXTERNAL_REF { get; set; }

        [StringLength(250)]
        public string NAME2 { get; set; }

        public short? INCLUDE_NAME2_ON_INVOICE { get; set; }

        [StringLength(1000)]
        public string NEW_STREET_ADDRESS { get; set; }

        [StringLength(100)]
        public string ZIP { get; set; }

        [StringLength(200)]
        public string CITY { get; set; }

        [StringLength(30)]
        public string EORI_NO { get; set; }

        [StringLength(50)]
        public string COC_CODE { get; set; }

        [StringLength(50)]
        public string SCAC_CODE { get; set; }

        [StringLength(30)]
        public string GSCID { get; set; }

        public string ADR_VISIT_STREET { get; set; }

        [StringLength(20)]
        public string ADR_VISIT_ZIP { get; set; }

        [StringLength(200)]
        public string ADR_VISIT_CITY { get; set; }

        [StringLength(10)]
        public string ADR_VISIT_COUNTRY_CODE { get; set; }

        public string ADR_INVOICE_STREET { get; set; }

        [StringLength(10)]
        public string ADR_INVOICE_ZIP { get; set; }

        [StringLength(200)]
        public string ADR_INVOICE_CITY { get; set; }

        [StringLength(10)]
        public string ADR_INVOICE_COUNTRY_CODE { get; set; }

        [StringLength(200)]
        public string OFFICE_DESCRIPTION { get; set; }

        public int? HEAD_OFFICE_ID { get; set; }

        public short? ADR_MULTI { get; set; }

        public short? SPECIAL_AGREEMENTS { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? TENDER_LIMIT { get; set; }

        public short? SHOW_WARNING { get; set; }

        public string WARNING { get; set; }

        [StringLength(30)]
        public string BANK_SWIFT { get; set; }

        [StringLength(50)]
        public string BANK_IBAN { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? HANDLING2_START { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? HANDLING2_PCT { get; set; }

        [StringLength(500)]
        public string DOC_FOLDER { get; set; }

        public int? AGENCY_FILTER_ID { get; set; }

        public int? WEB_ACCESS_GROUP { get; set; }

        public int? WEB_ACCESS_GRP_ROLE { get; set; }

        public short? INV_DUEDAYS_NEXT_MONTH { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Client> Client1 { get; set; }

        public virtual Client Client2 { get; set; }
    }
}
