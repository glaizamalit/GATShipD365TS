namespace GATShipD365TS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class expense_template_dyna
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ETD_ID { get; set; }

        public int? ETD_ELT_ID { get; set; }

        public int? ETD_FIELD_ID { get; set; }
    }
}
