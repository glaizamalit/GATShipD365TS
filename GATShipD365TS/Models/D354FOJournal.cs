using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GATShipD365TS.Models
{
    public class D365FOJournal
    {
        public D365FOHeader _gatshipData { get; set; }
    }

    public class D365FOHeader
    {
        public string CompanyCode { get; set; }
        public string TransType { get; set; }
        public string Description { get; set; }
        public string FileBatchID { get; set; }
        public List<D365FODetail> Lines { get; set; }
    }

    public class D365FODetail
    {
        public string FileBatchID { get; set; }
        public string TransDate { get; set; }
        public string AccountType { get; set; }
        public string Account { get; set; }      
        public string RevenueType { get; set; }
        public string Currency { get; set; }
        public decimal? Amount { get; set; }
        public string LineDescription { get; set; }   
        public decimal? ExchangeRate { get; set; }
        public string SalesTaxGroup { get; set; }
        public string ItemSalesTaxGroup { get; set; }
        public decimal? SalesTaxAmount { get; set; }
        public string Invoice { get; set; }
        public string Document { get; set; }
        public string DocumentDate { get; set; }
        public string DueDate { get; set; }
        public string Payment { get; set; }
        public string BankTransType { get; set; }
        public string CreatedByUserId { get; set; }
        public string UserField1 { get; set; }
        public string UserField2 { get; set; }
        public string UserField3 { get; set; }
        public string Remark { get; set; }
        public string VesselCustomerCode { get; set; }
        public string VoyageCode { get; set; }

    }
}
