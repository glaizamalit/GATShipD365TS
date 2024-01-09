namespace GATShipD365TS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GSRecExported")]
    public partial class GSRecExported
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string GSTableName { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int GSRecID { get; set; }
    }
}
