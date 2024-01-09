using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
namespace GATShipD365TS.Models
{
    public class InvoicePayload
    {
        [Required]
        public string action { get; set; }
        [Required]
        public string tenant { get; set; }
        [Required]
        public string entity { get; set; }
        [Required]
        public invoiceData data { get; set; }        

    }
    public class invoiceData
    {
        [Required]
        public int? id { get; set; }
        public DateTime? dateReceived { get; set; }
        [Required]
        public DateTime? issuedAt { get; set; }
        [Required]
        public DateTime? cancelledAt { get; set; }
        [Required]
        public int? companyId { get; set; }
        public string vendorCode { get; set; }
        public string vendorName { get; set; }
        [Required]
        public string reference { get; set; }
        public string description { get; set; }
        [Required]
        public string remarks { get; set; }
        [Required]
        public decimal? amount { get; set; }
        public int expId { get; set; }
        public int? portCallId { get; set; }
        public int? nominationId { get; set; }
        public string fileNumber { get; set; }
        public string voyageCode { get; set; }
        public string businessType { get; set; }
        public string principalNumber { get; set; }
        public string principalName { get; set; }
        public string PIC { get; set; }
        public string vesselName { get; set; }
        public DateTime? postingDate { get; set; }
        public string postingCode { get; set; }
        public int? creditedOrgId { get; set; }
        public string code1 { get; set; }
        public string code2 { get; set; }
        public int? refNumber { get; set; }
        public string initials { get; set; }
        public int? invoiceId { get; set; }
        public decimal? vat { get; set; }
        public decimal? vendorGST { get; set; }
        public decimal? sbf { get; set; }
        public decimal? bsf { get; set; }
        public decimal? umb { get; set; }
        public decimal? afc { get; set; }
        public decimal? afp { get; set; }
        public decimal? fpe { get; set; }
        public decimal? mis { get; set; }
        public decimal? lt { get; set; }
        public decimal? seaTransport { get; set; }
        public decimal? bcr { get; set; }
        public decimal? doc { get; set; }
        public decimal? frt { get; set; }
        public decimal? others { get; set; }
        public decimal? twc { get; set; }
        public decimal? psc { get; set; }
        public int? expTemplateId { get; set; }
        public string misc { get; set; }
        public string expText { get; set; }
        public string chargeDept { get; set; }
        public decimal? pacoIncome { get; set; }
        public string referral { get; set; }
        public string clientCategory { get; set; }
        public int? expenseGroupId { get; set; }
        public decimal? logistic { get; set; }
        public decimal? commission { get; set; }
        public DateTime? eta_date { get; set;}
    }
}