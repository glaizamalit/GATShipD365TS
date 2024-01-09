using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GATShipD365TS.Models
{
    public class DAPayload
    {
        [Required]
        public string action { get; set; }
        [Required]
        public string tenant { get; set; }
        [Required]
        public string entity { get; set; }
        [Required]
        public daData data { get; set; }       
    }
    public class daData
    {
        [Required]
        public int? id { get; set; }
        public int? DCN_ID { get; set; }
        public DateTime? completeAt { get; set; }
        [Required]
        public DateTime? arrivalDate { get; set; }
        public DateTime? departureDate { get; set; }
        public decimal? totalAmount { get; set; }
        public int? portCallId { get; set; }
        public int? nominationId { get; set; }
        public string nominationType { get; set; }
        public string voyageCode { get; set; }
        public string businessType { get; set; }
        public string principalCurrency { get; set; }
        public decimal? principalRate { get; set; }
        public string portCallOperator { get; set; }
        public string fileNumber { get; set; }
        public string PIC { get; set; }
        public string principalNumber { get; set; }
        public string principalName { get; set; }
        public string vesselName { get; set; }
        public DateTime? createdDate { get; set; }
        public string dcnNumber { get; set; }
        public int? invoiceId { get; set; }
        public DateTime? eta_date { get; set; }

    }
}
