namespace GATShipD365TS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class service_set
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SSE_ID { get; set; }

        [Required]
        [StringLength(100)]
        public string SSE_NAME { get; set; }

        [StringLength(50)]
        public string SSE_HISTORY { get; set; }
    }
}
