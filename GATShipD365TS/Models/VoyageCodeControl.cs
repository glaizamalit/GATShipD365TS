namespace GATShipD365TS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VoyageCodeControl")]
    public partial class VoyageCodeControl
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PortCall_ID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(9)]
        public string PortCall_Number { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(18)]
        public string FinanceVoyageCode { get; set; }
    }
}
