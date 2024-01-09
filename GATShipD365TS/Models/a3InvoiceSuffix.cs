namespace GATShipD365TS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class a3InvoiceSuffix
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string VendorCode { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string InvoiceNo { get; set; }

        public int? SuffixNo { get; set; }

        public DateTime CreatedDT { get; set; }

        public DateTime UpdatedDT { get; set; }
    }
}
