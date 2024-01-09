namespace GATShipD365TS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ExpenseView")]
    public partial class ExpenseView
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int EXP_ID { get; set; }

        public int? PORTCALL_ID { get; set; }

        public int? EXP_TYPE { get; set; }

        public DateTime? EXP_DATE { get; set; }

        [StringLength(100)]
        public string EXP_GRP { get; set; }

        [StringLength(100)]
        public string EXP_TEXT { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? EXP_QUANTITY { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? EXP_PRICE { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? EXP_AMOUNT { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? EXP_HANDLING { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? EXP_VAT { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? EXP_LOCAL_VAT { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? VAT_PERCENTAGE { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? EXP_TOTAL_SUM { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? EXP_LOCAL_SUM { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? EXP_TOTAL_PROFIT { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short EXP_BUDGET { get; set; }

        [StringLength(200)]
        public string CLIENT_NAME { get; set; }

        [StringLength(50)]
        public string CLIENT_ACCOUNT { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? EXP_ORIG_BUDGET { get; set; }

        [StringLength(1000)]
        public string EXP_LINK_DOCUMENT { get; set; }

        public int? EXP_INVOICE_ID { get; set; }

        [StringLength(20)]
        public string EXP_ACCOUNT_CODE { get; set; }

        public int? EXPENSE_STATUS { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(32)]
        public string EXPENSE_STATUS_TEXT { get; set; }

        [StringLength(200)]
        public string SUPPNAME { get; set; }

        [StringLength(50)]
        public string SUPP_ACCOUNT { get; set; }

        [StringLength(30)]
        public string InvoiceNo { get; set; }

        [StringLength(100)]
        public string VOUCHER_REF { get; set; }

        [StringLength(20)]
        public string INITIALS { get; set; }

        public int? EG_SORT_ORDER { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int EXP_GROUP_ID { get; set; }

        public DateTime? PRINTED_DATE { get; set; }

        [StringLength(20)]
        public string BATCH_NO { get; set; }

        public int? REF_NUMBER { get; set; }

        public int? CLIENT_ID { get; set; }

        public int? EXP_TEMPLATE_ID { get; set; }

        public int? EXP_SUPPLIER_ID { get; set; }

        public int? EXP_CURRENCY_ID { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? EXP_WORK_HOURS { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? EXP_WORK_PRICE { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? EXP_WORK_AMOUNT { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? EXP_HANDLING_PERCENT { get; set; }

        [StringLength(50)]
        public string LOCAL_CURRENCY_CODE { get; set; }

        [StringLength(5)]
        public string INVOICE_CURRENCY { get; set; }

        [StringLength(50)]
        public string EXP_CURRENCY_NAME { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? EXP_EXCHANGE_RATE { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? INV_EXCHANGE_RATE { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? EXP_ORIGINAL_ACTUAL_VALUE { get; set; }

        [StringLength(2000)]
        public string EXP_MISC { get; set; }

        public int? INCOMING_INVOICE_ID { get; set; }
    }
}
