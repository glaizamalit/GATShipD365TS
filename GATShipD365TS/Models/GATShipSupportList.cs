namespace GATShipD365TS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GATShipSupportList")]
    public partial class GATShipSupportList
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(6)]
        public string Initials { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ForeignKeyId { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int InvoiceId { get; set; }
     
              
        [StringLength(20)]
        public string Status { get; set; }

        [Key]
        [Column(Order = 4)]
        public DateTime CreatedDt { get; set; }

        public DateTime? UpdatedDt { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(5)]
        public string LocCode { get; set; }

        [StringLength(200)]
        public string Remarks { get; set; }
    }
}
