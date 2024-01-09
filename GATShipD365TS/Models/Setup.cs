namespace GATShipD365TS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Setup")]
    public partial class Setup
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        public int? DEFAULT_LAYOUT_ID { get; set; }

        public int? AGENCY_RULE_ID { get; set; }

        public int? USER_RULE_1_ID { get; set; }

        public int? USER_RULE_2_ID { get; set; }

        [StringLength(80)]
        public string AGENT { get; set; }

        [StringLength(80)]
        public string OFFICE { get; set; }

        [StringLength(80)]
        public string ADDRESS { get; set; }

        [StringLength(80)]
        public string POSTAL_ADDRESS { get; set; }

        [StringLength(80)]
        public string PLACE { get; set; }

        [StringLength(80)]
        public string CUSTOM_AREA { get; set; }

        [StringLength(30)]
        public string PHONE { get; set; }

        [StringLength(30)]
        public string TELEFAX { get; set; }

        [StringLength(30)]
        public string TELEX { get; set; }

        [StringLength(30)]
        public string MOBILE { get; set; }

        [StringLength(80)]
        public string EMAIL { get; set; }

        [StringLength(40)]
        public string AGENT_CARGO_NUMBER { get; set; }

        [StringLength(4000)]
        public string BANK_ACCOUNT { get; set; }

        [StringLength(40)]
        public string ORG_NUMBER { get; set; }

        [StringLength(140)]
        public string EDITOR_LOC { get; set; }

        [StringLength(130)]
        public string TEMPLATE_LOC { get; set; }

        [StringLength(130)]
        public string DOCUMENT_LOC { get; set; }

        [StringLength(130)]
        public string IMAGE_LOC { get; set; }

        [StringLength(130)]
        public string HTML_LOC { get; set; }

        [StringLength(26)]
        public string USER_DEFINED_CAPTION_1 { get; set; }

        [StringLength(26)]
        public string USER_DEFINED_CAPTION_2 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? CABFARE { get; set; }

        public int? ADVANCE_SORT_ORDER { get; set; }

        public int? AGENCY_SORT_ORDER { get; set; }

        public int? CTM_SORT_ORDER { get; set; }

        [StringLength(50)]
        public string HISTORY { get; set; }

        public int? TAB_PROFORMA { get; set; }

        public int? TAB_PRIOR_NOTICE { get; set; }

        public int? TAB_GENERAL_DECL { get; set; }

        public int? TAB_GOODS { get; set; }

        public int? TAB_BL { get; set; }

        public int? TAB_MANIFEST { get; set; }

        public int? TAB_CHECKLIST { get; set; }

        public int? TAB_ROUTE { get; set; }

        public int? TAB_HISTORY { get; set; }

        public int? AGENCY_FEE_PRINT { get; set; }

        public int? ROUND_PROFORMA { get; set; }

        [StringLength(40)]
        public string PORTCALL_FORMAT { get; set; }

        public int? SERIES_START { get; set; }

        public int? RESTART_SERIES { get; set; }

        public DateTime? LAST_DATE { get; set; }

        public int? LAST_IN_SERIES { get; set; }

        [StringLength(50)]
        public string GROUP_NAME { get; set; }

        public int? ICON_INDEX { get; set; }

        public int? PORTCALL_LIST { get; set; }

        public int SETUP_EXTRA_ID { get; set; }

        public short? TAB_EMAIL { get; set; }

        public short? TAB_LOGINFO { get; set; }

        [StringLength(50)]
        public string EXT_CODE_1 { get; set; }

        public int? SETUP3_ID { get; set; }

        public int? INACTIVE { get; set; }

        [StringLength(50)]
        public string EXT_CODE_2 { get; set; }
    }
}
