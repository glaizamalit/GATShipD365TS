namespace GATShipD365TS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class incoming_invoice
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public incoming_invoice()
        {
            Expenses = new HashSet<Expense>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IIN_ID { get; set; }

        public int? IIN_INTERNAL_NO { get; set; }

        [StringLength(30)]
        public string IIN_NUMBER { get; set; }

        public DateTime? IIN_DUE_DATE { get; set; }

        public int? IIN_SUPPLIER_ID { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? IIN_AMOUNT { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? IIN_VAT_AMOUNT { get; set; }

        [StringLength(5)]
        public string IIN_CURRENCY_CODE { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? IIN_ROE { get; set; }

        public int? IIN_DOCUMENT_ID { get; set; }

        public string IIN_COMMENT { get; set; }

        public int? IIN_AGENCY_ID { get; set; }

        [StringLength(50)]
        public string IIN_HISTORY { get; set; }

        public DateTime? IIN_EXPORTED { get; set; }

        public DateTime? IIN_CREATED_DATE { get; set; }

        [StringLength(50)]
        public string IIN_REFERENCE_NO { get; set; }

        public int? IIN_PORTCALL_ID { get; set; }

        public int? IIN_INACTIVE { get; set; }

        [StringLength(25)]
        public string IIN_INTERNAL_NUMBER { get; set; }

        public DateTime? IIN_ISSUED_DATE { get; set; }

        public int? IIN_VAT_CODE_ID { get; set; }

        [StringLength(30)]
        public string IIN_PO_NO { get; set; }

        public short? IIN_MULTI_ENABLED { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Expense> Expenses { get; set; }

        public virtual PortCall PortCall { get; set; }
    }
}
