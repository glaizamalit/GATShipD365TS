namespace GATShipD365TS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class dyna_fields
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public dyna_fields()
        {
            dyna_values = new HashSet<dyna_values>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        [StringLength(50)]
        public string CONTAINER { get; set; }

        public int FIELD_TYPE { get; set; }

        [StringLength(100)]
        public string CAPTION { get; set; }

        public int? SORT_ORDER { get; set; }

        [StringLength(30)]
        public string DOC_CODE { get; set; }

        public short? FIELD_LIMIT { get; set; }

        public int? MAX_CHARS { get; set; }

        public int? CHAR_CASE { get; set; }

        public int? DECIMALS { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? FIELD_LIMIT_FROM { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? FIELD_LIMIT_TO { get; set; }

        public int? FIELD_FORMAT { get; set; }

        public int FIELD_WIDTH { get; set; }

        public short? POS_MANUAL { get; set; }

        public int? POS_TOP { get; set; }

        public int? POS_LEFT { get; set; }

        public int? POS_LABEL_LEFT { get; set; }

        [StringLength(50)]
        public string HISTORY { get; set; }

        public int? COMBO_STYLE { get; set; }

        [StringLength(4000)]
        public string COMBO_ITEMS { get; set; }

        public int? RADIO_COLS { get; set; }

        [StringLength(50)]
        public string COMPONENT_NAME { get; set; }

        public short? BOLDFONT { get; set; }

        [StringLength(2000)]
        public string CAPTION_HINT { get; set; }

        public int? FIELD_HEIGHT { get; set; }

        [StringLength(250)]
        public string INTEGRATION_REFERENCE { get; set; }

        public short? NEVER_COPY { get; set; }

        public short? USE_IN_GRID { get; set; }

        public int? MANDATORY { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<dyna_values> dyna_values { get; set; }
    }
}
