namespace GATShipD365TS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Hotlist")]
    public partial class Hotlist
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        public int CLIENT_ID { get; set; }

        public int PORTCALL_ID { get; set; }

        public int? STATUS { get; set; }

        [StringLength(50)]
        public string HISTORY { get; set; }

        public int? CLIENT_ROLE_ID { get; set; }

        [StringLength(150)]
        public string TEMPLATE_NAME { get; set; }
    }
}
