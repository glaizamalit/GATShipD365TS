namespace GATShipD365TS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PortCall")]
    public partial class PortCall
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        public int? SETUP_ID { get; set; }

        public int VESSEL_ID { get; set; }

        [StringLength(25)]
        public string PORTCALL_NUMBER { get; set; }

        public int STATUS { get; set; }

        public int SUBSTITUTE { get; set; }

        public DateTime? ETA_DATE { get; set; }

        public DateTime? ETA_TIME { get; set; }

        public DateTime? ETB_DATE { get; set; }

        public DateTime? ETB_TIME { get; set; }

        public DateTime? ETD_DATE { get; set; }

        public DateTime? ETD_TIME { get; set; }

        public DateTime? ATA_DATE { get; set; }

        public DateTime? ATA_TIME { get; set; }

        public DateTime? ATD_DATE { get; set; }

        public DateTime? ATD_TIME { get; set; }

        public DateTime? CANCEL_TIME_DATE { get; set; }

        public DateTime? CANCEL_TIME_TIME { get; set; }

        [StringLength(100)]
        public string CANCEL_BY { get; set; }

        public DateTime? ANC_DATE_DATE { get; set; }

        public DateTime? ANC_DATE_TIME { get; set; }

        public DateTime? ANC_AWAY_DATE { get; set; }

        public DateTime? ANC_AWAY_TIME { get; set; }

        [StringLength(100)]
        public string ANC_PLACE { get; set; }

        public DateTime? LAYCAN_FROM_DATE { get; set; }

        public DateTime? LAYCAN_FROM_TIME { get; set; }

        public DateTime? LAYCAN_TO_DATE { get; set; }

        public DateTime? LAYCAN_TO_TIME { get; set; }

        public DateTime? LAYCAN_DATE_DATE { get; set; }

        public DateTime? LAYCAN_DATE_TIME { get; set; }

        public DateTime? LAYCAN_CANCEL_DATE { get; set; }

        public DateTime? LAYCAN_CANCEL_TIME { get; set; }

        [StringLength(50)]
        public string MASTER_NAME { get; set; }

        public int? FARLEDSBEVIS { get; set; }

        public int? VOY_TYPE { get; set; }

        [StringLength(20)]
        public string VOY { get; set; }

        public int? STEWEDORS { get; set; }

        public int? TRUCKS { get; set; }

        [StringLength(100)]
        public string CHARTERER { get; set; }

        [StringLength(50)]
        public string FIXED_BY { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? ARRIVAL_DRAFT { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? DEPARTURE_DRAFT { get; set; }

        [StringLength(50)]
        public string DROP_ANCHOR_PLACE { get; set; }

        public DateTime? DROP_ANCHOR_DATE { get; set; }

        public DateTime? DROP_ANCHOR_TIME { get; set; }

        public DateTime? ALL_FAST_DATE { get; set; }

        public DateTime? ALL_FAST_TIME { get; set; }

        public DateTime? EST_COMMENCE_DATE { get; set; }

        public DateTime? EST_COMMENCE_TIME { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? BUNKERS_ON_ARRIVAL_MDO { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? BUNKERS_ON_ARRIVAL_GO { get; set; }

        [StringLength(10)]
        public string BUNKERS_ON_ARRIVAL_USER_TEXT { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? BUNKERS_ON_ARRIVAL_USER_VALUE { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? DRAFT_ON_ARRIVAL_FWO { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? DRAFT_ON_DEPARTURE_FWO { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? DRAFT_ON_ARRIVAL_AFT { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? DRAFT_ON_DEPARTURE_AFT { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? BUNKERS_TAKEN_MDO { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? BUNKERS_TAKEN_GO { get; set; }

        [StringLength(10)]
        public string BUNKERS_TAKEN_USER_TEXT { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? BUNKERS_TAKEN_USER_VALUE { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? BUNKERS_ON_DEP_MDO { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? BUNKERS_ON_DEP_GO { get; set; }

        [StringLength(10)]
        public string BUNKERS_ON_DEP_USER_TEXT { get; set; }

        [StringLength(10)]
        public string BUNKERS_ON_DEP_USER_VALUE { get; set; }

        [StringLength(30)]
        public string AGENT_NEXT_PORT { get; set; }

        public int? PASSAGE_TIME_NEXT_PORT { get; set; }

        public string PROSPECT_MISC { get; set; }

        public string NOTES { get; set; }

        public string REMARKS { get; set; }

        [StringLength(50)]
        public string HISTORY { get; set; }

        [StringLength(100)]
        public string CARGO_NO { get; set; }

        public DateTime? ARRIVAL_EST_BERTH_DATE { get; set; }

        public DateTime? ARRIVAL_EST_BERTH_TIME { get; set; }

        public DateTime? ARRIVAL_EST_DEP_DATE { get; set; }

        public DateTime? ARRIVAL_EST_DEP_TIME { get; set; }

        public int? ARRIVAL_TUGS { get; set; }

        public int? ARRIVAL_MOORING { get; set; }

        public int? DEPARTURE_TUGS { get; set; }

        public int? DEPARTURE_MOORING { get; set; }

        [StringLength(60)]
        public string INBOUND_PILOT { get; set; }

        [StringLength(60)]
        public string OUTBOUND_PILOT { get; set; }

        public int? PASSING_HOURS_NEXT_PORT { get; set; }

        [StringLength(25)]
        public string MASTER_CERTIFICATE { get; set; }

        public short? INVOICE_STATUS { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? SLOP { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? SLUDGE { get; set; }

        [StringLength(50)]
        public string JNO { get; set; }

        [StringLength(50)]
        public string CARGONUMBER { get; set; }

        public string CARGO_DESC { get; set; }

        [StringLength(50)]
        public string USERDEF_1 { get; set; }

        [StringLength(50)]
        public string USERDEF_2 { get; set; }

        public short? CHECKLIST_ALL_WARNING { get; set; }

        [StringLength(20)]
        public string PERSON_IN_CHARGE { get; set; }

        public int? INTERNAL_VOY { get; set; }

        [StringLength(30)]
        public string LOCKED_BY { get; set; }

        public DateTime? LOCKED { get; set; }

        public int? SAFESEANET_VOYAGEID1 { get; set; }

        public int? SAFESEANET_VOYAGEID2 { get; set; }

        [StringLength(30)]
        public string GSCID { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] CHANGE_STAMP { get; set; }

        public short? BERTH_ATA_CONNECTED { get; set; }

        public short? BERTH_ATD_CONNECTED { get; set; }

        public short? BERTH_ETA_CONNECTED { get; set; }

        public short? BERTH_ETD_CONNECTED { get; set; }

        public short? BERTH_CALCULATION { get; set; }

        public string PROSPECTS_HINT { get; set; }

        [StringLength(5)]
        public string INV_CURRENCY { get; set; }

        [StringLength(1000)]
        public string DOC_FOLDER { get; set; }

        public DateTime? EXPORTED { get; set; }

        public string EXP_INSTRUCTIONS { get; set; }

        public int? TEAM_IN_CHARGE_ID { get; set; }

        public short? GDPR_DELETED { get; set; }
    }
}
