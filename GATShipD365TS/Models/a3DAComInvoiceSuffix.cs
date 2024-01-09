namespace GATShipD365TS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class a3DAComInvoiceSuffix
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(3)]
        public string a3LocCode { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(30)]
        public string DAComInvoice { get; set; }

        public int? SuffixNo { get; set; }

        public DateTime CreatedDT { get; set; }

        public DateTime UpdatedDT { get; set; }
    }
}
