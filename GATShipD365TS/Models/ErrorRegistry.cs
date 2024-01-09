namespace GATShipD365TS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ErrorRegistry")]
    public partial class ErrorRegistry
    {
        [Key]
        [StringLength(10)]
        public string ErrCode { get; set; }

        [Required]
        [StringLength(1000)]
        public string ErrMessage { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string NextActionUser { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string NextActionHelpdesk { get; set; }

        public int NofRetry { get; set; }

        [Required]
        [StringLength(500)]
        public string Recipients { get; set; }

        [Required]
        [StringLength(500)]
        public string CorrectiveAction { get; set; }
    }
}
