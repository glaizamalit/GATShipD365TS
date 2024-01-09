namespace GATShipD365TS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Dock")]
    public partial class Dock
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        public int PORTCALL_ID { get; set; }

        public int QUAY_ID { get; set; }

        public int? CARGO_ID { get; set; }

        public int? SORT_ORDER { get; set; }

        public int? TYPE { get; set; }

        [StringLength(50)]
        public string TYPE_OTHER { get; set; }

        public DateTime? DATE_FROM { get; set; }

        public DateTime? TIME_FROM { get; set; }

        public DateTime? DATE_TO { get; set; }

        public DateTime? TIME_TO { get; set; }

        [StringLength(250)]
        public string COMMENTS { get; set; }

        [StringLength(50)]
        public string HISTORY { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? HOURS_TO_STAY { get; set; }

        public short? ACTIVE_QUAY { get; set; }

        public DateTime? ETB { get; set; }

        public DateTime? ATA { get; set; }

        public DateTime? ETD { get; set; }

        public DateTime? ATD { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? ARR_DRAFT { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? DEP_DRAFT { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? SHIFTING_TIME { get; set; }

        public short? CALCULATE_TIMES { get; set; }

        public DateTime? CALC_ETA { get; set; }

        public DateTime? CALC_ETD { get; set; }

        public DateTime? PILOT_IN_PBT { get; set; }

        public short? PILOT_IN_NEEDED { get; set; }

        public string PILOT_IN_REMARKS { get; set; }

        [StringLength(50)]
        public string PILOT_IN_PO { get; set; }

        public int? PILOT_IN_DESCRIPTION_ID { get; set; }

        public int? VESSEL_ORIENTATION { get; set; }

        [StringLength(250)]
        public string POSITION_REMARK { get; set; }

        public DateTime? PILOT_OUT_PBT { get; set; }

        public short? PILOT_OUT_NEEDED { get; set; }

        public string PILOT_OUT_REMARKS { get; set; }

        [StringLength(50)]
        public string PILOT_OUT_PO { get; set; }

        public int? PILOT_OUT_DESCRIPTION_ID { get; set; }

        [StringLength(50)]
        public string DAN_OUT_REST_ID { get; set; }

        [StringLength(50)]
        public string DAN_IN_REST_ID { get; set; }

        public DateTime? ITC { get; set; }

        public DateTime? ETC { get; set; }

        public short? PILOT_IN_DAN_FIXED_TIME { get; set; }

        public short? PILOT_OUT_DAN_FIXED_TIME { get; set; }

        public short? PILOT_IN_DAN_ANCHORING { get; set; }

        public short? PILOT_OUT_DAN_COMPLETED { get; set; }

        public short? PILOT_IN_DAN_COMPLETED { get; set; }

        public DateTime? ITC_DELAY { get; set; }

        public DateTime? ETC_DELAY { get; set; }

        [StringLength(50)]
        public string PORTBASE_ID { get; set; }

        public int? BOLLARD_FROM { get; set; }

        public int? BOLLARD_TO { get; set; }

        public int? TUG_COMPANY { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? TUG_QTY { get; set; }

        public int? TUG_DEP_COMPANY { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? TUG_DEP_QTY { get; set; }

        [StringLength(100)]
        public string TUG_REMARKS { get; set; }

        [StringLength(100)]
        public string TUG_DEP_REMARKS { get; set; }
    }
}
