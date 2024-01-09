namespace GATShipD365TS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class dyna_values
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        public int FIELD_ID { get; set; }

        public int? FOREIGN_KEY_INT { get; set; }

        [StringLength(30)]
        public string FOREIGN_KEY { get; set; }

        [StringLength(4000)]
        public string VALUE_TEXT { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? VALUE_FLOAT { get; set; }

        public DateTime? VALUE_DATETIME { get; set; }

        public virtual dyna_fields dyna_fields { get; set; }
    }
}
