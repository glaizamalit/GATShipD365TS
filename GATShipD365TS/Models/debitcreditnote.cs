namespace GATShipD365TS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("debitcreditnote")]
    public partial class debitcreditnote
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DCN_ID { get; set; }

        public int DCN_DEBITOR_ID { get; set; }

        public DateTime? DCN_DUE_DATE { get; set; }

        [Required]
        [StringLength(30)]
        public string DCN_NUMBER { get; set; }

        public DateTime? DCN_EXPORTED { get; set; }

        [StringLength(50)]
        public string DCN_HISTORY { get; set; }

        public int DCN_TYPE { get; set; }

        [StringLength(30)]
        public string DCN_DEBIT_REF { get; set; }

        public DateTime? DCN_CREATED { get; set; }

        public int DCN_AGENCY_ID { get; set; }

        [StringLength(250)]
        public string DCN_FILE_NAME { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? DCN_TOTAL_AMOUNT { get; set; }

        [StringLength(100)]
        public string DCN_KID { get; set; }

        [StringLength(250)]
        public string DCN_FILE_NAME2 { get; set; }

        [StringLength(3000)]
        public string DCN_PRE_ADDRESS { get; set; }

        [StringLength(5000)]
        public string DCN_ADDRESS { get; set; }

        [StringLength(500)]
        public string DCN_DEBTOR_NAME { get; set; }

        [StringLength(50)]
        public string DCN_CURRENCY_CODE { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? DCN_EXCHANGE_RATE { get; set; }

        public int? DCN_DOCUMENT_ID { get; set; }

        public DateTime? DCN_SETTLEMENT_RECEIVED { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? DCN_SETTLEMENT_AMOUNT_PAID { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? DCN_PORTCALL_ROE { get; set; }

        public short? DCN_INCLUDE_VAT { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? DCN_VAT_AMOUNT { get; set; }

        public short? DCN_MERGE_ON_ACCOUNT_NO { get; set; }
    }
}
