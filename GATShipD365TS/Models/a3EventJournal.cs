namespace GATShipD365TS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class a3EventJournal
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }

        [Required]
        [StringLength(20)]
        public string action { get; set; }

        [Required]
        [StringLength(20)]
        public string entity { get; set; }

        public int a3_id { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string data { get; set; }

        public DateTime? received_at { get; set; }

        public bool isRelevant { get; set; }

        public DateTime? JrnlDate { get; set; }

        public int? SeqNo { get; set; }

        [Required(AllowEmptyStrings = true)]
        [StringLength(100)]
        public string JrnlFilePath { get; set; }

        [Required(AllowEmptyStrings = true)]
        [StringLength(100)]
        public string JrnlFileName { get; set; }

        public bool isReversed { get; set; }

        [Required(AllowEmptyStrings = true)]
        [StringLength(100)]
        public string RevJrnlFilePath { get; set; }

        [Required(AllowEmptyStrings = true)]
        [StringLength(100)]
        public string RevJrnlFileName { get; set; }

        [Required(AllowEmptyStrings = true)]
        [StringLength(200)]
        public string SystemMessage { get; set; }

        public DateTime CreatedDT { get; set; }

        public DateTime UpdatedDT { get; set; }

        [StringLength(3)]
        public string a3LocCode { get; set; }
    }
}
