namespace GATShipD365TS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Client_Type
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        [StringLength(25)]
        public string NAME { get; set; }

        [StringLength(50)]
        public string HISTORY { get; set; }

        public short? DEFAULT_HOTLIST { get; set; }

        public int? SORT_ORDER { get; set; }

        public short? DEFAULT_RESTRICT { get; set; }

        public int? ROLE_OR_GROUP { get; set; }

        public int? WEB_ACCESS_GROUP_ID { get; set; }

        public short? DEFAULT_VESSEL_HOTLIST { get; set; }

        public short? RESTRICT_TO_CATEGORY { get; set; }

        public short? MANDATORY_HOTLIST { get; set; }

        public short? MANDATORY_VESSEL { get; set; }
    }
}
