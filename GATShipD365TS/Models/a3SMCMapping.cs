namespace GATShipD365TS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class a3SMCMapping
    {
        [Key]
        [StringLength(3)]
        public string a3LocCode { get; set; }

        [Required]
        [StringLength(30)]
        public string a3LocDesc { get; set; }

        [Required]
        [StringLength(10)]
        public string SMCCode { get; set; }

        [Required]
        [StringLength(50)]
        public string CpyName { get; set; }

        [Required]
        [StringLength(20)]
        public string VendorCode { get; set; }

        public bool isGenJrnl { get; set; }

        public bool isSalesTax { get; set; }

        [Required]
        [StringLength(10)]
        public string SalesTaxCode { get; set; }

        public bool isShowVoyageCodeHour { get; set; }

        public bool isGenInvPerSer { get; set; }
        public bool isGenDNPerSer { get; set; }
    }
}
