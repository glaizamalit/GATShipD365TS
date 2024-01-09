namespace GATShipD365TS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class a3_creditor_invoice
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        [Key]
        [Column(Order = 1)]
        public DateTime document_date { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int document_number { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(20)]
        public string document_type { get; set; }

        [Key]
        [Column(Order = 4)]
        public decimal amount { get; set; }

        [Key]
        [Column(Order = 5)]
        public decimal amount_with_tax { get; set; }

        [Key]
        [Column(Order = 6)]
        [StringLength(10)]
        public string currency { get; set; }

        [Key]
        [Column(Order = 7)]
        public decimal exchange_rate { get; set; }

        [Key]
        [Column(Order = 8)]
        public DateTime cancelled_at { get; set; }

        [Key]
        [Column(Order = 9)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int a3_creditor_id { get; set; }

        [Key]
        [Column(Order = 10)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int a3_file_id { get; set; }
    }
}
