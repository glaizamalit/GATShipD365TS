namespace GATShipD365TS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Expense")]
    public partial class Expense
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        public int? PORTCALL_ID { get; set; }

        public int? CLIENT_ID { get; set; }

        public int? EXPENSE_GROUP_ID { get; set; }

        public int? AGENCY_RULE_ID { get; set; }

        public int? USER_RULE_1_ID { get; set; }

        public int? USER_RULE_2_ID { get; set; }

        public int? EXPENSE_TYPE { get; set; }

        public DateTime? EXPENSE_DATE { get; set; }

        public int? SORT_ORDER { get; set; }

        [StringLength(20)]
        public string INITIALS { get; set; }

        [StringLength(100)]
        public string EXPENSE_TEXT { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? QUANTITY { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? PRICE { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? AMOUNT { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? HANDLING { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? VAT { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? TOTAL_SUM { get; set; }

        [StringLength(100)]
        public string VOUCHER_REF { get; set; }

        [StringLength(20)]
        public string ACCOUNT_CODE { get; set; }

        public string MISC { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? AGENCY_FEE { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? REBATE_PERCENT { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? REBATE_TOTAL { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? POSTAGE_PETTIES { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? TELEX_FAX { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? POSTAGE_FOR_VESSEL { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? HAULING_EXPENSES_KM { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? HAULING_EXPENSES_TOTAL { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? BANK_CHARGES { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? WATER_CLERK_OVERTIME { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? USER_RULE_MULTIPLY_1 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? USER_RULE_VALUE_1 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? USER_RULE_MULTIPLY_2 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? USER_RULE_VALUE_2 { get; set; }

        [StringLength(26)]
        public string USER_DEFINED_CAPTION_1 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? USER_DEFINED_VALUE_1 { get; set; }

        [StringLength(26)]
        public string USER_DEFINED_CAPTION_2 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? USER_DEFINED_VALUE_2 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? TOTAL_FEES { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? AMOUNT_ADVANCE { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? AMOUNT_RECEIVED { get; set; }

        public int? RECEIVED { get; set; }

        public DateTime? RECEIVED_DATE { get; set; }

        [StringLength(50)]
        public string PAID_HISTORY { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? AMOUNT_FOREIGN_CURRENCY { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? AMOUNT_LOCAL_CURRENCY { get; set; }

        [StringLength(150)]
        public string AMOUNT_AS_TEXT { get; set; }

        public int? RECEIVED_ACCOUNT { get; set; }

        [StringLength(50)]
        public string HISTORY { get; set; }

        public int? HARD_CURRENCY_ID { get; set; }

        public int? CTM_TYPE { get; set; }

        public short? BUDGET { get; set; }

        [StringLength(20)]
        public string BATCH_NO { get; set; }

        [StringLength(50)]
        public string CODE1 { get; set; }

        [StringLength(50)]
        public string CODE2 { get; set; }

        public int? REF_NUMBER { get; set; }

        public short? NOT_ON_DA { get; set; }

        public DateTime? EXPENSE_TIME { get; set; }

        public DateTime? PRINTED_DATE { get; set; }

        [StringLength(30)]
        public string PRINTED_USER { get; set; }

        public int? EXPENSE_TEMPLATE_ID { get; set; }

        public int? CURRENCY_ID { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? ORIGINAL_BUDGET { get; set; }

        [StringLength(1000)]
        public string LINK_DOCUMENT { get; set; }

        public int? INVOICE_ID { get; set; }

        public int? INVOICE_MERGE { get; set; }

        public int? INVOICE_INCLUDE { get; set; }

        public DateTime? BUDGET_EXPORT_DATE { get; set; }

        public DateTime? INVOICE_EXPORT_DATE { get; set; }

        public int? SUPPLIER_ID { get; set; }

        public int? EXPENSE_STATUS { get; set; }

        public int? VOUCHER_TYPE { get; set; }

        [StringLength(100)]
        public string COUNTER_ACCOUNT { get; set; }

        public int? ACCOUNT_TYPE { get; set; }

        public short? PAYLOCK { get; set; }

        public int? VOUCHER_NUMBER { get; set; }

        public int? VAT_ID { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? ORIGINAL_ACTUAL_VALUE { get; set; }

        public DateTime? VOUCHER_PRINTED { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? WORK_HOURS { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? WORK_PRICE { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? WORK_AMOUNT { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? EXCHANGE_RATE { get; set; }

        public int? PREVIOUS_CLIENT_ID { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? PROFIT_PCT { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? PROFIT { get; set; }

        public int? APPROVED { get; set; }

        public string APPROVAL_COMMENT { get; set; }

        [StringLength(3)]
        public string INVOICE_CUR { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? ROE_INV_CUR { get; set; }

        public int? INCOMING_INVOICE_ID { get; set; }

        public int? SETTLEMENT_INVOICE_ID { get; set; }

        public string VAT_DESCRIPTION { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? INVOICE_AMOUNT { get; set; }

        public int? EXP_SET_ID { get; set; }

        public int? CARGO_ID { get; set; }

        [StringLength(30)]
        public string PO_NO { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? ROE_CTM { get; set; }

        public int? DEBIT_EXP_ID { get; set; }

        public virtual incoming_invoice incoming_invoice { get; set; }

        public virtual PortCall PortCall { get; set; }
    }
}
