namespace GATShipD365TS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Vessel")]
    public partial class Vessel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Vessel()
        {
            PortCalls = new HashSet<PortCall>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        [StringLength(30)]
        public string VESSEL_TYPE_CODE { get; set; }

        [StringLength(10)]
        public string NATIONAL_CODES_ID { get; set; }

        public int? CLIENT_ID { get; set; }

        [StringLength(10)]
        public string VESSEL_PREFIX { get; set; }

        [StringLength(50)]
        public string NAME { get; set; }

        [StringLength(50)]
        public string LLOYDSNR { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? DWT { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? GT { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? NT { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? LOA { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? BEAM { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? SUMMERDRAFT { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? CBMTANK { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? AIR_DRAFT { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? SBT { get; set; }

        public short? DOUBLE_BOTTOM { get; set; }

        public short? DOUBLE_SKIN { get; set; }

        [StringLength(15)]
        public string CALLSIGN { get; set; }

        [StringLength(40)]
        public string MOBILE1 { get; set; }

        [StringLength(40)]
        public string MOBILE2 { get; set; }

        [StringLength(20)]
        public string TELEFAX { get; set; }

        [StringLength(40)]
        public string TELEX { get; set; }

        [StringLength(30)]
        public string FLAG { get; set; }

        [StringLength(50)]
        public string HOMEPORT { get; set; }

        public int? CONSTRUCTION_YEAR { get; set; }

        [StringLength(50)]
        public string REG_PLACE { get; set; }

        public DateTime? REG_DATE { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? KNOTS { get; set; }

        public short? BOWTHRUST { get; set; }

        public short? ANNUAL_FEE { get; set; }

        public short? SPANTS { get; set; }

        public string HOLD_HATCH_DESC { get; set; }

        [StringLength(20)]
        public string CLASS { get; set; }

        public string CRANES { get; set; }

        public string COMMENTS { get; set; }

        [StringLength(50)]
        public string HISTORY { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? LL { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? USER_VALUE { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? CGT { get; set; }

        public int? DOUBLE_SIDES { get; set; }

        public int? STERNTHRUST { get; set; }

        public int? REG_YEAR { get; set; }

        public int? OTHER_CLIENT_ID { get; set; }

        [StringLength(30)]
        public string TONNAGE_CERTIFICATE { get; set; }

        public int? CREW { get; set; }

        public int? MMSI { get; set; }

        [StringLength(50)]
        public string USER_TEXT_VALUE { get; set; }

        [StringLength(50)]
        public string ISPS_SERTIFICATE { get; set; }

        public int? VES_INACTIVE { get; set; }

        [StringLength(10)]
        public string ICE_CLASS { get; set; }

        [StringLength(30)]
        public string GSCID { get; set; }

        public short? SHOW_WARNING { get; set; }

        public string WARNING { get; set; }

        public int? PUBLIC_FLAG { get; set; }

        [StringLength(4000)]
        public string PUBLIC_ADV { get; set; }

        [StringLength(1000)]
        public string DOC_FOLDER { get; set; }

        public DateTime? TRADING_CERT { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? CSI { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? ESI { get; set; }

        public DateTime? EXPORTED { get; set; }

        public int? PASSENGERS { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PortCall> PortCalls { get; set; }
    }
}
