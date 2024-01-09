using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
namespace GATShipD365TS.Models
{
    public class FundsPayload
    {
        [Required]
        public string action { get; set; }
        [Required]
        public string tenant { get; set; }
        [Required]
        public string entity { get; set; }
        [Required]
        public data data { get; set; }       
    }
    public class data
    {
        [Required]
        public int? id { get; set; }
        [Required]
        public DateTime? dateReceived { get; set; }
        [Required]
        public int? payeeCompanyId { get; set; }
        [Required]
        [StringLength(50)]
        public string category { get; set; }
        [Required]
        public string comment { get; set; }
        [Required]
        public decimal? amount { get; set; }
        public int expId { get; set; }
        public int? portCallId { get; set; }
        [Required]
        public int? nominationId { get; set; }
        public string fileNumber { get; set; }
        public string voyageCode { get; set; }
        public string businessType { get; set; }
        public string principalNumber { get; set; }
        public string principalName { get; set; }
        public string PIC { get; set; }
        public string vesselName { get; set; }
        public DateTime? postingDate { get; set; }
        public int? creditedOrgId { get; set; }
        public string vendorName { get; set; }
        public int? refNumber { get; set; }
        public string initials { get; set; }
        public int? invoiceId { get; set; }
        public string misc { get; set; }
        public string chargeDept { get; set; }
        public string clientCategory { get; set; }
        public string usdBankAccountNoJPN { get; set; }
        public string jpyBankAccountNoJPN { get; set; }
        public DateTime? eta_date { get; set; }
    }
}
