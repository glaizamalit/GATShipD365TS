namespace GATShipD365TS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class a3NominationType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int NomTypeID { get; set; }

        [Required]
        [StringLength(50)]
        public string NomTypeName { get; set; }

        [Required]
        [StringLength(50)]
        public string NomTypeShortName { get; set; }

        [Required]
        [StringLength(10)]
        public string NomTypeCode { get; set; }
    }
}
