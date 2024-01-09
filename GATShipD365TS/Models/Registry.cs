namespace GATShipD365TS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Registry")]
    public partial class Registry
    {
        [Key]
        [StringLength(50)]
        public string RegKey { get; set; }

        [Required]
        [StringLength(500)]
        public string RegValue { get; set; }

        [Required]
        [StringLength(500)]
        public string RegDesc { get; set; }
    }
}
