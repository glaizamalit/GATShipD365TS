namespace GATShipD365TS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("vatcode")]
    public partial class vatcode
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int VAT_ID { get; set; }

        [Required]
        [StringLength(50)]
        public string VAT_CODE { get; set; }

        [StringLength(250)]
        public string VAT_DESCRIPTION { get; set; }

        [Column(TypeName = "numeric")]
        public decimal VAT_PERCENTAGE { get; set; }

        [StringLength(50)]
        public string VAT_HISTORY { get; set; }

        public int? VAT_RULE_ID { get; set; }

        [StringLength(50)]
        public string VAT_CODE2 { get; set; }

        [StringLength(50)]
        public string VAT_CODE3 { get; set; }
    }
}
