namespace GATShipD365TS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class a3EventStage
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }

        [StringLength(255)]
        public string action { get; set; }

        [StringLength(255)]
        public string entity { get; set; }

        public int a3_id { get; set; }

        public string data { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? received_at { get; set; }

        [Required]
        [StringLength(3)]
        public string a3LocCode { get; set; }
    }
}
