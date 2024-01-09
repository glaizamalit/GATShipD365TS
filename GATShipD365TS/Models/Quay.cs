namespace GATShipD365TS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Quay")]
    public partial class Quay
    {      
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        public int HARBOUR_ID { get; set; }

        [StringLength(50)]
        public string NAME { get; set; }

        [StringLength(50)]
        public string PLACE { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? LENGTH { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? DRAFT { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? CRANE_RANGE { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? AIR_DRAFT { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? LOAD_CAPACITY { get; set; }

        public short? USEDEFAULT { get; set; }

        [StringLength(250)]
        public string COMMENTS { get; set; }

        [Required]
        [StringLength(50)]
        public string HISTORY { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? BEAM { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? DWT { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? USER_VALUE_1 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? USER_VALUE_2 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? USER_VALUE_3 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? USER_VALUE_4 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? USER_VALUE_5 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? USER_VALUE_6 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? USER_VALUE_7 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? USER_VALUE_8 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? USER_VALUE_9 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? USER_VALUE_10 { get; set; }

        [StringLength(30)]
        public string QUAY_NO { get; set; }

        [StringLength(50)]
        public string CODE { get; set; }

        [StringLength(50)]
        public string CUSTOMS_CODE { get; set; }

        [StringLength(30)]
        public string GSCID { get; set; }

        [StringLength(250)]
        public string TERMINAL_NAME { get; set; }

        public DateTime? PASSAGE_TIME { get; set; }

        public string QUAY_RESTRICTIONS { get; set; }       
    }
}
