namespace GATShipD365TS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Harbour")]
    public partial class Harbour
    {        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        [StringLength(10)]
        public string NATIONAL_CODES_ID { get; set; }

        [StringLength(200)]
        public string NAME { get; set; }

        [StringLength(100)]
        public string CUSTOMS_PLACE { get; set; }

        public int? OWNED { get; set; }

        public string COMMENTS { get; set; }

        [Required]
        [StringLength(50)]
        public string HISTORY { get; set; }

        [StringLength(50)]
        public string HARBOUR_NO { get; set; }

        [StringLength(50)]
        public string CODE1 { get; set; }

        [StringLength(20)]
        public string LOCODE { get; set; }

        [StringLength(30)]
        public string ISPS_NUMBER { get; set; }

        [StringLength(50)]
        public string CUSTOMS_CODE { get; set; }

        [StringLength(30)]
        public string CSCID { get; set; }

        [StringLength(10)]
        public string LATITUDE_DEG { get; set; }

        [StringLength(10)]
        public string LATITUDE_MIN { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? POS_TO_PORT_HOURS { get; set; }

        [StringLength(10)]
        public string LONGITUDE_DEG { get; set; }

        [StringLength(10)]
        public string LONGITUDE_MIN { get; set; }

        public int? OWNED_BY_AGENCY_ID { get; set; }

        public int? PILOT_IN_DESCRIPTION_ID { get; set; }

        public int? PILOT_OUT_DESCRIPTION_ID { get; set; }

        public int? PILOT_SHIFTING_DESCRIPTION_ID { get; set; }

        public int? INACTIVE { get; set; }       
    }
}
