using GATShipD365TS.App_Code;
using GATShipD365TS.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace GATShipD365TS.Helper
{
    public class EntityHelper
    {
        WIS_Sync contextWis = new WIS_Sync();
        GSWallem contextgswallem = new GSWallem();
        A3Helper a3Helper = new A3Helper();
        public string A3LocationCode(int? id, string fileNumber, string typeOfId, ref List<string> validationMsg)
        {
            LogManager.Log.Info("We are currently in location code of..." + Config.LocationCode);
            LogManager.Log.Info("Validate the Location Code...");
            string a3LocCode = null;
            var getLocCodeByDaId = "";
            try
            {
                getLocCodeByDaId = fileNumber;

                if (getLocCodeByDaId == null)
                {
                    a3LocCode = null;
                    throw new Exception("0001:: Location Code not found.");
                }
                else
                {
                    a3LocCode = getLocCodeByDaId.Substring(2, 3);

                    LogManager.Log.Info("Compare a3LocationCode from NominationFileNumber to Config File....");
                    if (a3LocCode == Config.LocationCode)
                    {
                        LogManager.Log.Info("Location Code [" + a3LocCode + "] is correct....");
                    }
                    else
                    {
                        throw new Exception("0070:: Location Code using NominationFileNumber and from Config File are not the same.");
                    }
                }

                return a3LocCode;
            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting a3LocCode: " + e.Message + ":::" + e.InnerException);
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                return a3LocCode;
            }
        }

        public string SalesTaxCode(int a3_id, string a3LocationCode, decimal? salesTaxAmount, decimal? amount, ref List<string> validationMsg)
        {
            LogManager.Log.Info("Validate the Sales Tax Code...");
            string salesTaxCode = "";
            try
            {
                if (IsSalesTax(a3LocationCode, ref validationMsg))
                {
                    var vatCode = (from v in contextgswallem.Expenses
                                   join vc in contextgswallem.Vatcodes on v.VAT_ID equals vc.VAT_ID into g
                                   from l in g.DefaultIfEmpty()
                                   where v.ID == a3_id
                                   select new { l.VAT_CODE, l.VAT_CODE2, l.VAT_DESCRIPTION }).ToList();

                    if (vatCode == null || vatCode.Count() == 0)
                    {
                        salesTaxCode = null;
                        throw new Exception("0002:: Sales Tax Code cannot be determined.");
                    }
                    else
                    {
                        if (salesTaxAmount != 0)
                        {
                            salesTaxCode = vatCode.FirstOrDefault().VAT_CODE;
                            LogManager.Log.Info("VAT_CODE is..." + salesTaxCode);
                            salesTaxCode = string.IsNullOrEmpty(salesTaxCode) ? "GST" : salesTaxCode;
                            LogManager.Log.Info("New VAT_CODE is..." + salesTaxCode);
                        }
                        else
                        {
                            salesTaxCode = vatCode.FirstOrDefault().VAT_CODE2;
                            LogManager.Log.Info("VAT_CODE2 is..." + salesTaxCode);
                            salesTaxCode = string.IsNullOrEmpty(salesTaxCode) ? "GST_EX" : salesTaxCode;
                            LogManager.Log.Info("New VAT_CODE2 is..." + salesTaxCode);
                        }
                        if (salesTaxCode != null)
                        {
                            salesTaxCode = salesTaxCode.Length > 20 ? salesTaxCode.Substring(0, 20) : salesTaxCode;
                        }
                        else
                        {
                            salesTaxCode = "";
                        }


                    }

                }
                else
                {
                    salesTaxCode = "";
                }

                return salesTaxCode;
            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting SalesTaxCode: " + e.Message + ":::" + e.InnerException);
                validationMsg = a3Helper.AppendError("Exception occur in getting SalesTaxCodeForDN: " + e.Message + "<br>");
                return salesTaxCode;
            }
        }

        public string SalesTaxCodeForDN(int a3_id, string a3LocationCode, decimal? salesTaxAmount, decimal? amount, ref List<string> validationMsg)
        {
            LogManager.Log.Info("Validate the Sales Tax Code For DN...");
            string salesTaxCodeForDN = "";
            try
            {
                if (IsSalesTax(a3LocationCode, ref validationMsg))
                {
                    var vatCode = (from v in contextgswallem.Expenses
                                   join vc in contextgswallem.Vatcodes on v.VAT_ID equals vc.VAT_ID into g
                                   from l in g.DefaultIfEmpty()
                                   where v.ID == a3_id
                                   select new { l.VAT_CODE, l.VAT_CODE2, l.VAT_DESCRIPTION }).ToList();

                    if (vatCode == null || vatCode.Count() == 0)
                    {
                        salesTaxCodeForDN = null;
                        throw new Exception("0002:: Sales Tax Code For DN cannot be determined.");
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(vatCode.FirstOrDefault().VAT_CODE))
                        {
                            salesTaxCodeForDN = "GST_EX";
                        }
                        else
                        {
                            salesTaxCodeForDN = vatCode.FirstOrDefault().VAT_CODE2;
                            salesTaxCodeForDN = string.IsNullOrEmpty(salesTaxCodeForDN) ? "GST_EX" : salesTaxCodeForDN;
                        }
                        if (salesTaxCodeForDN != null)
                        {
                            salesTaxCodeForDN = salesTaxCodeForDN.Length > 20 ? salesTaxCodeForDN.Substring(0, 20) : salesTaxCodeForDN;
                        }
                        else
                        {
                            salesTaxCodeForDN = "";
                        }

                    }

                }
                else
                {
                    salesTaxCodeForDN = "";
                }

                return salesTaxCodeForDN;
            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting SalesTaxCodeForDN: " + e.Message + ":::" + e.InnerException);
                validationMsg = a3Helper.AppendError("Exception occur in getting SalesTaxCodeForDN: " + e.Message + "<br>");
                return salesTaxCodeForDN;
            }
        }

        public string SalesTaxGroupCode(string a3LocationCode, decimal? amount, ref List<string> validationMsg)
        {
            LogManager.Log.Info("Validate the Sales Tax Code...");
            string salesTaxGroupCode = "";
            try
            {
                if (IsSalesTax(a3LocationCode, ref validationMsg))
                {
                    var a3SMCMapping = (from v in contextWis.a3SMCMappings
                                        where v.a3LocCode.Trim() == a3LocationCode
                                        select v.SalesTaxCode).ToList();

                    if (a3SMCMapping == null || a3SMCMapping.Count() == 0)
                    {
                        salesTaxGroupCode = null;
                        throw new Exception("0002:: Sales Tax Code cannot be determined.");
                    }
                    else
                    {
                        salesTaxGroupCode = a3SMCMapping.FirstOrDefault();
                        salesTaxGroupCode = salesTaxGroupCode.Length > 20 ? salesTaxGroupCode.Substring(0, 20) : salesTaxGroupCode;
                    }
                }
                else
                {
                    salesTaxGroupCode = "";
                }

                return salesTaxGroupCode;
            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting SalesTaxCode: " + e.Message + ":::" + e.InnerException);
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                return salesTaxGroupCode;
            }
        }

        public bool IsSalesTax(string a3LocationCode, ref List<string> validationMsg)
        {
            LogManager.Log.Info("Validate the IsSalesTax...");
            bool isSalesTax = false;
            try
            {
                var a3SMCMapping = (from v in contextWis.a3SMCMappings
                                    where v.a3LocCode.Trim() == a3LocationCode
                                    select v.isSalesTax).ToList();

                if (a3SMCMapping == null || a3SMCMapping.Count() == 0)
                {
                    isSalesTax = false;
                    throw new Exception("0004:: Sales Tax cannot be determined.");
                }
                else
                {
                    isSalesTax = a3SMCMapping.FirstOrDefault();
                }
                return isSalesTax;
            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting isSalesTax: " + e.Message + ":::" + e.InnerException);
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                return isSalesTax;
            }
        }

        public bool IsGenJrnl(string a3LocationCode, ref List<string> validationMsg)
        {
            LogManager.Log.Info("Validate the IsGenJrnl...");
            bool isGenJrnl = false;
            try
            {
                var a3SMCMapping = (from v in contextWis.a3SMCMappings
                                    where v.a3LocCode.Trim() == a3LocationCode
                                    select v.isGenJrnl).ToList();

                if (a3SMCMapping == null || a3SMCMapping.Count() == 0)
                {
                    isGenJrnl = false;
                    throw new Exception("0005:: Generate Journal cannot be determined.");
                }
                else
                {
                    isGenJrnl = a3SMCMapping.FirstOrDefault();
                }

                return isGenJrnl;
            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting isGenJrnl: " + e.Message + ":::" + e.InnerException);
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                return isGenJrnl;
            }
        }
        public string SMCCode(string locCode, string typeOfId, ref List<string> validationMsg)
        {
            LogManager.Log.Info("Validate the SMC Code...");
            string retsmcCode = null;
            try
            {
                var smcCode = (from v in contextWis.a3SMCMappings
                               where v.a3LocCode.Trim() == locCode
                               select v.SMCCode).ToList();

                if (smcCode == null || smcCode.Count() == 0)
                {
                    retsmcCode = null;
                    throw new Exception("0006:: SMC Code does not exist in a3SMCMappings.");
                }
                else
                {
                    retsmcCode = smcCode.SingleOrDefault().ToString().Trim();
                }
                return retsmcCode;
            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting SMCCode: " + e.Message + ":::" + e.InnerException);
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                return retsmcCode;
            }
        }

        public string SMCVendorCode(int? id, string typeOfId, string locCode, ref List<string> validationMsg)
        {
            LogManager.Log.Info("Validate the SMC Vendor Code...");
            string retsmcVendorCode = null;
            try
            {
                var smcVendorCode = (from v in contextWis.a3SMCMappings
                                     where v.a3LocCode.Trim() == locCode
                                     select v.VendorCode).ToList();

                if (smcVendorCode == null || smcVendorCode.Count() == 0)
                {
                    retsmcVendorCode = null;
                    throw new Exception("0007:: SMC Vendor Code does not exist in a3SMCMappings.");
                }
                else
                {
                    retsmcVendorCode = smcVendorCode.SingleOrDefault().ToString().Trim();
                }
                return retsmcVendorCode;
            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting SMCVendorCode: " + e.Message + ":::" + e.InnerException);
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                return retsmcVendorCode;
            }
        }
        public string RemoveSpecialCharacters(string str)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                if (str != null)
                {
                    foreach (char c in str)
                    {
                        if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == ' ')
                        {
                            sb.Append(c);
                        }
                    }
                    return sb.ToString().ToUpper();
                }
                else
                {
                    return str;
                }

            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in remove special characters: " + e.Message + ":::" + e.InnerException);
                throw;
            }

        }
        public string ChargeCodeForFund(string comment, string businessType, int? id, string typeOfId, ref List<string> validationMsg)
        {
            LogManager.Log.Info("Validate the Charge Code For Fund...");
            string retChargeCode = null;
            try
            {
                var typeOfReceipt = TypeOfReceiptForFund(comment, ref validationMsg);

                if (typeOfReceipt != null)
                {
                    if (typeOfReceipt.ToUpper().Trim() == "ADV" || typeOfReceipt.ToUpper().Trim() == "ADVANCE")
                    {

                        var chargeCode = (from v in contextWis.Registries
                                          where v.RegKey == "a3ChargeCodeForFund_" + businessType
                                          select v.RegValue).ToList();
                        if (chargeCode == null || chargeCode.Count() == 0)
                        {
                            retChargeCode = null;
                            throw new Exception("0011:: Charge Code For Fund " + "[" + businessType + "]" + " cannot be found in Registry.");
                        }
                        else
                        {
                            retChargeCode = chargeCode.SingleOrDefault().ToString().Trim();
                        }
                    }
                    else if (typeOfReceipt.ToUpper().Trim() == "BERTH")
                    {
                        retChargeCode = "4209";
                    }
                    else
                    {
                        retChargeCode = "0000";
                    }
                    return retChargeCode;
                }
                else
                {
                    retChargeCode = null;
                    throw new Exception("0012:: Charge Code For Fund is null.");

                }
            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting Charge Code For Fund: " + e.Message + ":::" + e.InnerException);
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                return retChargeCode;
            }


        }

        public string getSeparatorForComment(ref List<string> validationMsg)
        {
            LogManager.Log.Info("Validate the Separator...");
            string sepString = null;
            try
            {
                var separator = (from v in contextWis.Registries
                                 where v.RegKey.Trim() == "a3SeparatorCharacter"
                                 select v.RegValue).ToList();
                if (separator == null || separator.Count() < 0)
                {
                    sepString = null;
                    throw new Exception("0014:: Separator is null.");
                }
                else
                {
                    sepString = separator.FirstOrDefault().ToString();
                }
                return sepString;
            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting Separator: " + e.Message + ":::" + e.InnerException);
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                return sepString;
            }
        }

        public string getTypeOfReceiptRegistry(ref List<string> validationMsg)
        {
            LogManager.Log.Info("Validate the Separator...");
            string sepString = null;
            try
            {
                var separator = (from v in contextWis.Registries
                                 where v.RegKey.Trim() == "a3TypeOfReceiptForFund"
                                 select v.RegValue).ToList();
                if (separator == null || separator.Count() < 0)
                {
                    sepString = null;
                    throw new Exception("0015:: Type of receipt is null.");
                }
                else
                {
                    sepString = separator.FirstOrDefault().ToString();
                }
                return sepString;
            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting Type of Receipt in Registry: " + e.Message + ":::" + e.InnerException);
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                return sepString;
            }
        }
        public string FirstSectionOfFundComment(string comment, ref List<string> validationMsg)
        {
            LogManager.Log.Info("Validate the Currency...");
            string retCurrency = null;
            try
            {
                var separator = getSeparatorForComment(ref validationMsg);
                string[] SplitComment = comment != null ? comment.Split(separator.ToCharArray()) : null;
                if (SplitComment == null || SplitComment.Count() < 0)
                {
                    retCurrency = "";
                    throw new Exception("0016:: Currency for Fund is empty.");
                }
                else
                {
                    retCurrency = SplitComment[0].ToUpper().Trim();
                    if (retCurrency == "")
                    {
                        retCurrency = "";
                        throw new Exception("0016:: Currency for Fund is empty.");
                    }
                }
                return retCurrency;
            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting Currency For Fund: " + e.Message + ":::" + e.InnerException);
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                return retCurrency;
            }
        }
        public string FirstSectionOfInvoiceComment(string comment, ref List<string> validationMsg)
        {
            LogManager.Log.Info("Validate the Currency...");
            string retCurrency = null;
            try
            {
                var separator = getSeparatorForComment(ref validationMsg);
                string[] SplitComment = comment != null ? comment.Split(separator.ToCharArray()) : null;
                if (SplitComment == null || SplitComment.Count() < 0)
                {
                    retCurrency = "";
                    throw new Exception("0017:: Currency for Invoice is empty.");
                }
                else
                {
                    retCurrency = SplitComment[0].ToUpper().Trim();
                    if (retCurrency == "")
                    {
                        throw new Exception("0017:: Currency for Invoice is empty.");
                    }
                }
                return retCurrency;
            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting Currency For Invoice: " + e.Message + ":::" + e.InnerException);
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                return retCurrency;
            }
        }
        public string CurrencyForFund(string comment, ref List<string> validationMsg)
        {
            LogManager.Log.Info("Validate the Currency for Fund...");
            var currency = "";
            try
            {
                if (comment == null || comment.Length == 0)
                {
                    currency = "";
                    throw new Exception("0016:: Currency for Fund is empty.");
                }
                else
                {
                    currency = FirstSectionOfFundComment(comment, ref validationMsg).Substring(0, 3);
                    return currency;
                }
            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting Currency For Fund: " + e.Message + ":::" + e.InnerException);
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                return currency;
            }

        }
        public string CurrencyForInvoice(string comment, ref List<string> validationMsg)
        {
            LogManager.Log.Info("Validate the Currency for Invoice...");
            var currency = "";
            try
            {
                if (comment == null || comment.Length == 0)
                {
                    currency = "";
                    throw new Exception("0017:: Currency for Invoice is empty.");
                }
                else
                {
                    currency = FirstSectionOfInvoiceComment(comment, ref validationMsg).Substring(0, 3);
                    return currency;
                }
            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting Currency For Invoice: " + e.Message + ":::" + e.InnerException);
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                return currency;
            }
        }

        public string MediatedInvoiceComment(string remark, ref List<string> validationMsg)
        {
            LogManager.Log.Info("Validate the Invoice Comment/Remark...");
            string mediatedInvoiceComment = "";
            try
            {

            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting Invoice Remark: " + e.Message + ":::" + e.InnerException);
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                return mediatedInvoiceComment;
            }

            return mediatedInvoiceComment;
        }
        public string CommentForFund(string comment, ref List<string> validationMsg)
        {
            LogManager.Log.Info("Validate the Comment...");
            string retComment = null;
            try
            {
                var separator = getSeparatorForComment(ref validationMsg);
                string[] SplitComment = comment != null ? comment.Split(separator.ToCharArray()) : null;
                if (SplitComment == null || SplitComment.Count() < 7)
                {
                    retComment = "";
                    LogManager.Log.Info("Comment For Fund is empty.");

                }
                else
                {
                    retComment = SplitComment[6].ToUpper().Trim();
                }
                return retComment;

            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting Comment For Fund: " + e.Message + ":::" + e.InnerException);
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                return retComment;
            }

        }
        public string AccountTypeForFCFin(int? payeeCompanyID, string principalNumber, string comment, string locCode, DateTime? postingDate, ref List<string> validationMsg)
        {
            LogManager.Log.Info("Validate the Account Type For FCFin...");
            var accountType = "";

            try
            {
                locCode = locCode != null || locCode != "" ? locCode.ToUpper() : locCode;
                var principalNo = principalNumber;
                var bankRef = BankReferenceNumberForFund(comment, locCode, postingDate, ref validationMsg);
                bankRef = bankRef.Length > 10 ? bankRef.TrimStart().Substring(0, 10) : bankRef;
                bankRef = bankRef == null ? bankRef : bankRef.ToUpper();
                var currency = CurrencyForFund(comment, ref validationMsg);

                switch (locCode)
                {
                    case "HKG":
                        if (principalNo == null || bankRef == null || currency == null)
                        {
                            accountType = null;
                            throw new Exception("0021:: Account Type for FCFin is null.");
                        }
                        else
                        {

                            if (bankRef == "PETTY CASH" && currency != "HKD")
                            {
                                accountType = "Ledger";
                            }
                            else
                            {
                                accountType = "Bank";
                            }


                        }
                        break;
                    case "SGP":
                        accountType = "Bank";
                        break;
                    case "THA":
                        accountType = "Bank";
                        break;
                    case "VNM":
                        accountType = "Bank";
                        break;
                    case "VTN":
                        accountType = "Bank";
                        break;
                    case "MMR":
                        accountType = "Ledger";
                        break;

                    default:
                        accountType = "Bank";
                        break;

                }
                return accountType;



            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting Account Type For FCFin: " + e.Message + ":::" + e.InnerException);
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                return accountType;
            }

        }
        public string DIM6(string locCode, int id, string entity, ref List<string> validationMsg)
        {
            string dimension6 = "";

            try
            {
                locCode = locCode != null || locCode != "" ? locCode.ToUpper() : locCode;
                switch (locCode)
                {
                    case "HKG":
                        dimension6 = "HKHKG";
                        break;
                    case "SGP":
                        dimension6 = "";
                        break;
                    case "THA":
                        dimension6 = "";
                        break;
                    case "VNM":
                        dimension6 = "";
                        break;
                    case "VTN":
                        dimension6 = "";
                        break;
                    case "MMR":
                        dimension6 = "";
                        break;
                    case "JPN":
                        LogManager.Log.Info("Entity: " + entity);
                        var dim = (from e in contextgswallem.Expenses
                                   join d in contextgswallem.Docks on e.PORTCALL_ID equals d.PORTCALL_ID
                                   join q in contextgswallem.Quays on d.QUAY_ID equals q.ID
                                   join h in contextgswallem.Harbours on q.HARBOUR_ID equals h.ID
                                   where e.ID == id
                                   orderby d.ATD, d.ATA, d.ETB, d.SORT_ORDER descending
                                   select new { h.LOCODE, harbourName = h.NAME, quarryName = q.NAME, d.ETB, d.ATD, d.ETD, d.ATA }).ToList();
                        LogManager.Log.Info("Dim list count for " + entity + ": " + dim.Count());


                        if (entity.ToUpper() == "DA")
                        {
                            dim = (from e in contextgswallem.Expenses
                                   join d in contextgswallem.Docks on e.PORTCALL_ID equals d.PORTCALL_ID
                                   join q in contextgswallem.Quays on d.QUAY_ID equals q.ID
                                   join h in contextgswallem.Harbours on q.HARBOUR_ID equals h.ID
                                   join dc in contextgswallem.debitcreditnotes on e.INVOICE_ID equals dc.DCN_ID
                                   where dc.DCN_ID == id
                                   orderby d.ATD, d.ATA, d.ETB, d.SORT_ORDER descending
                                   select new { h.LOCODE, harbourName = h.NAME, quarryName = q.NAME, d.ETB, d.ATD, d.ETD, d.ATA }).ToList();
                            LogManager.Log.Info("Dim list count for " + entity + ": " + dim.Count());
                        }

                        if (dim.Count() > 0 && dim != null)
                        {
                            var harbourLocCode = dim.FirstOrDefault().LOCODE;
                            if (harbourLocCode != null && harbourLocCode != "")
                            {
                                dimension6 = harbourLocCode;
                            }
                            else
                            {
                                throw new Exception("0099:: Cannot locate Dim6 for Japan.");
                            }
                        }
                        else
                        {
                            throw new Exception("0099:: Cannot locate Dim6 for Japan.");
                        }
                        break;

                    default:
                        dimension6 = "";
                        break;
                }

                return dimension6;
            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting DIM6: " + e.Message + ":::" + e.InnerException);
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                return "";
            }

        }

        public string AccountForFCFin(int? payeeCompanyID, string usdBankAccountNoJPN, string jpyBankAccountNoJPN, string principalNumber, string comment, string locCode, DateTime? postingDate, ref List<string> validationMsg)
        {
            LogManager.Log.Info("Validate the Account For FCFin...");
            var account = "";
            //  var principalNo = PrincipalNumber(payeeCompanyID, "NOMID", ref validationMsg);
            var currency = CurrencyForFund(comment, ref validationMsg);
            var bankRef = BankReferenceNumberForFund(comment, locCode, postingDate, ref validationMsg);
            bankRef = bankRef.Length > 10 ? bankRef.TrimStart().Substring(0, 10) : bankRef;
            bankRef = bankRef == null ? bankRef : bankRef.ToUpper();

            try
            {
                locCode = locCode != null || locCode != "" ? locCode.ToUpper() : locCode;
                var firstSec = FirstSectionOfFundComment(comment, ref validationMsg).ToUpper().Trim();

                var accountCode = (from a in contextWis.Registries
                                   where a.RegKey.Contains("AccountForFCFin")
                                   select a).ToList();

                switch (locCode)
                {
                    case "HKG":
                        if (bankRef == null || currency == null)
                        {
                            account = null;
                            throw new Exception("0022:: Account for Fund Create Final cannot be found.");
                        }
                        else
                        {
                            if (bankRef == "PETTY CASH")
                            {
                                account = accountCode.Where(a => a.RegKey == "AccountForFCFin1").FirstOrDefault().RegValue;                                
                            }
                            else
                            {
                                if (currency != "HKD")
                                {
                                    account = accountCode.Where(a => a.RegKey == "AccountForFCFin2").FirstOrDefault().RegValue;
                                }
                                else
                                {
                                    account = accountCode.Where(a => a.RegKey == "AccountForFCFin3").FirstOrDefault().RegValue;
                                }
                            }
                        }

                        break;
                    case "SGP":
                        if (firstSec == "USD-CITI")
                        {
                            account = accountCode.Where(a => a.RegKey == "AccountForFCFin2").FirstOrDefault().RegValue;
                        }

                        else if (firstSec == "SGD-CITI")
                        {
                            account = accountCode.Where(a => a.RegKey == "AccountForFCFin4").FirstOrDefault().RegValue;
                        }
                        else if (firstSec == "USD-DBS")
                        {
                            account = accountCode.Where(a => a.RegKey == "AccountForFCFin5").FirstOrDefault().RegValue;
                        }
                        else if (firstSec == "SGD-DBS")
                        {
                            account = accountCode.Where(a => a.RegKey == "AccountForFCFin6").FirstOrDefault().RegValue;
                        }
                        else
                        {
                            throw new Exception("0022:: Account for Fund Create Final cannot be found.");
                        }
                        break;
                    case "THA":
                        account = accountCode.Where(a => a.RegKey == "AccountForFCFin7").FirstOrDefault().RegValue;
                        break;
                    case "VNM":
                        account = accountCode.Where(a => a.RegKey == "AccountForFCFin8").FirstOrDefault().RegValue;
                        break;
                    case "MMR":
                        account = accountCode.Where(a => a.RegKey == "AccountForFCFin9").FirstOrDefault().RegValue;
                        break;
                    case "JPN":
                        if (firstSec == "USD")
                        {
                            account = usdBankAccountNoJPN;
                        }

                        else if (firstSec == "JPY")
                        {
                            account = jpyBankAccountNoJPN;
                        }
                        else
                        {
                            throw new Exception("0022:: Account for Fund Create Final cannot be found.");
                        }
                        break;
                    case "VTN":
                        if (firstSec == "USD")
                        {
                            account = accountCode.Where(a => a.RegKey == "AccountForFCFin10").FirstOrDefault().RegValue;
                        }

                        else if (firstSec == "VND")
                        {
                            account = accountCode.Where(a => a.RegKey == "AccountForFCFin11").FirstOrDefault().RegValue;
                        }
                        else
                        {
                            throw new Exception("0022:: Account for Fund Create Final cannot be found.");
                        }
                        break;
                    default:
                        LogManager.Log.Info("No Location Code or Unknown Location Code indicated in config file. Location Code:[" + locCode + "]");
                        throw new Exception("0001:: Location Code not found.");
                }

                return account;

            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting Account For FCFin: " + e.Message + ":::" + e.InnerException);
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                return account;
            }

        }

        public string ClearingAccountForFCAdv(string locCode, ref List<string> validationMsg)
        {
            var clearingAccount = "";

            try
            {
                var accountcode = (from a in contextWis.Registries
                                   where a.RegKey.Contains("ClearingAccountCode")
                                   select a).ToList();

                locCode = locCode != null || locCode != "" ? locCode.ToUpper() : locCode;
                switch (locCode)
                {
                    case "HKG":
                        clearingAccount = accountcode.Where(a => a.RegKey == "ClearingAccountCode1").FirstOrDefault().RegValue;
                        break;
                    case "SGP":
                        clearingAccount = accountcode.Where(a => a.RegKey == "ClearingAccountCode1").FirstOrDefault().RegValue;
                        break;
                    case "THA":
                        clearingAccount = accountcode.Where(a => a.RegKey == "ClearingAccountCode1").FirstOrDefault().RegValue;
                        break;
                    case "VNM":
                        clearingAccount = accountcode.Where(a => a.RegKey == "ClearingAccountCode2").FirstOrDefault().RegValue;
                        break;
                    case "MMR":
                        clearingAccount = accountcode.Where(a => a.RegKey == "ClearingAccountCode1").FirstOrDefault().RegValue;
                        break;
                    case "JPN":
                        clearingAccount = accountcode.Where(a => a.RegKey == "ClearingAccountCode3").FirstOrDefault().RegValue;
                        break;
                    case "VTN":
                        clearingAccount = accountcode.Where(a => a.RegKey == "ClearingAccountCode2").FirstOrDefault().RegValue;
                        break;
                    default:
                        LogManager.Log.Info("No Location Code or Unknown Location Code indicated in config file. Location Code:[" + locCode + "]");
                        throw new Exception("0001:: Location Code not found.");
                }
                return clearingAccount;
            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting Clearing Account For FCAdv: " + e.Message + ":::" + e.InnerException);
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                return "";
            }

        }
        public string AccountForFCAddAdv(int? payeeCompanyID, string comment, ref List<string> validationMsg)
        {
            LogManager.Log.Info("Validate the Account For FCAddAdv...");
            var account = "";
            try
            {
                if (CurrencyForFund(comment, ref validationMsg) == "HKD")
                {
                    account = "2005167";
                }
                else if (CurrencyForFund(comment, ref validationMsg) == "USD")
                {
                    account = "2005168";
                }
                else
                {
                    account = null;
                    throw new Exception("0023:: Currency should be HKD or USD.");
                }

                return account;
            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting Account For FCAddAdv: " + e.Message + ":::" + e.InnerException);
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                return account;
            }

        }
        public string TypeOfReceiptForFund(string comment, ref List<string> validationMsg)
        {
            LogManager.Log.Info("Validate the Type of Receipt...");
            string retReceipt = null;
            try
            {
                var Receipt = getTypeOfReceiptRegistry(ref validationMsg);
                string[] ListOfReceipt = Receipt.Split(new[] { ',' }, StringSplitOptions.None);
                var separator = getSeparatorForComment(ref validationMsg);
                string[] SplitComment = comment != null ? comment.Split(new[] { separator }, StringSplitOptions.None) : null;
                if (SplitComment == null || SplitComment.Count() < 6)
                {
                    retReceipt = "";
                    LogManager.Log.Info("Type of Receipt is empty.");
                }
                else
                {

                    retReceipt = SplitComment[5];
                    LogManager.Log.Info("Type of Receipt is " + "[" + retReceipt + "]");
                }
                int cntTypeReceipt = ListOfReceipt.Where(x => x.ToUpper().Trim() == retReceipt.ToUpper().Trim()).Count();
                if (cntTypeReceipt == 0)
                {
                    retReceipt = null;
                    throw new Exception("0024:: Type of Receipt is not valid.");
                }
                return retReceipt;

            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting Type Of Receipt For Fund: " + e.Message + ":::" + e.InnerException);
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                return retReceipt;
            }

        }
        public decimal ExchangeRateForFund(string comment, ref List<string> validationMsg)
        {
            LogManager.Log.Info("Validate the Exchange Rate For Fund...");
            decimal retRate = 0;
            try
            {
                var separator = getSeparatorForComment(ref validationMsg);
                string[] SplitComment = comment != null ? comment.Split(separator.ToCharArray()) : null;
                if (SplitComment == null || SplitComment.Count() < 4)
                {
                    retRate = 0;
                    throw new Exception("0025:: Exchange Rate For Fund is empty.");
                }
                else
                {
                    string amount = SplitComment[3];
                    if (amount.Trim() == "")
                    {
                        throw new Exception("0025:: Exchange Rate For Fund is empty.");
                    }
                    else if (amount.Trim() == "0")
                    {
                        throw new Exception("0026:: Exchange Rate for Fund is zero (0).");
                    }
                    else if (decimal.Parse(amount.Trim()) < 0)
                    {
                        throw new Exception("0027:: Exchange Rate for Fund is negative.");
                    }
                    else
                    {
                        retRate = Convert.ToDecimal(amount);
                    }
                }
                LogManager.Log.Info("ExchangeRateForFund: " + retRate);
                return retRate;
            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting Type Of Exchange Rate For Fund: " + e.Message + ":::" + e.InnerException);
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                return retRate;
            }

        }
        public decimal? AmountForFund(string comment, ref List<string> validationMsg)
        {
            LogManager.Log.Info("Validate the Amount For Fund...");
            decimal? retAmount = 0;
            try
            {
                var separator = getSeparatorForComment(ref validationMsg);
                string[] SplitComment = comment != null ? comment.Split(separator.ToCharArray()) : null;
                if (SplitComment == null || SplitComment.Count() < 2)
                {
                    retAmount = 0;
                    throw new Exception("0028:: Amount For Fund is empty.");
                }
                else
                {
                    string amount = SplitComment[1];
                    if (amount.Trim() == "")
                    {
                        throw new Exception("0028:: Amount For Fund is empty.");
                    }
                    else
                    {
                        retAmount = Convert.ToDecimal(amount);
                        retAmount = Convert.ToDecimal(string.Format("{0:F2}", Decimal.Round(retAmount.Value, 2)));
                    }

                }
                return retAmount;
            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting Type Of Amount For Fund: " + e.Message + "<br>");
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                retAmount = null;
                return retAmount;
            }

        }

        public decimal? AmountForInvoice(string comment, ref List<string> validationMsg)
        {
            LogManager.Log.Info("Validate the Amount For Invoice...");
            decimal? retAmount = 0;
            try
            {
                var separator = getSeparatorForComment(ref validationMsg);
                string[] SplitComment = comment != null ? comment.Split(separator.ToCharArray()) : null;
                if (SplitComment == null || SplitComment.Count() < 2)
                {
                    retAmount = 0;
                    throw new Exception("0029:: Amount For Invoice is empty.");
                }
                else
                {
                    string amount = SplitComment[1];
                    if (amount.Trim() == "")
                    {
                        throw new Exception("0029:: Amount For Invoice is empty.");
                    }
                    else
                    {
                        retAmount = Convert.ToDecimal(amount);
                        retAmount = Convert.ToDecimal(string.Format("{0:F2}", Decimal.Round(retAmount.Value, 2)));
                    }

                }
                return retAmount;
            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting Type Of Amount For Invoice: " + e.Message + "<br>");
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                retAmount = null;
                return retAmount;
            }

        }


        public decimal? AmountWithTaxForInvoice(string comment, ref List<string> validationMsg)
        {
            LogManager.Log.Info("Validate the Amount with Tax For Invoice...");
            decimal? retAmount = 0;
            try
            {
                var separator = getSeparatorForComment(ref validationMsg);
                string[] SplitComment = comment != null ? comment.Split(separator.ToCharArray()) : null;
                if (SplitComment == null || SplitComment.Count() < 3)
                {
                    retAmount = 0;
                    throw new Exception("0030:: Amount with Tax For Invoice is empty.");
                }
                else
                {
                    string amount = SplitComment[2];
                    if (amount.Trim() == "")
                    {
                        throw new Exception("0030:: Amount with Tax For Invoice is empty.");
                    }
                    else
                    {
                        retAmount = Convert.ToDecimal(amount);
                        retAmount = Convert.ToDecimal(string.Format("{0:F2}", Decimal.Round(retAmount.Value, 2)));
                    }

                }
                return retAmount;
            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting Type Of Amount with Tax For Invoice: " + e.Message + "<br>");
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                retAmount = null;
                return retAmount;
            }

        }


        public decimal? ExchangeRateForInvoice(string comment, ref List<string> validationMsg)
        {
            LogManager.Log.Info("Validate the Exchange Rate for Invoice ...");
            decimal? retAmount = 0;
            try
            {
                var separator = getSeparatorForComment(ref validationMsg);
                string[] SplitComment = comment != null ? comment.Split(separator.ToCharArray()) : null;
                if (SplitComment == null || SplitComment.Count() < 4)
                {
                    retAmount = 0;
                    throw new Exception("0031:: Exchange Rate for Invoice is empty.");
                }
                else
                {
                    string amount = SplitComment[3];
                    if (amount.Trim() == "")
                    {
                        throw new Exception("0031:: Exchange Rate for Invoice is blank.");
                    }
                    else if (amount.Trim() == "0")
                    {
                        throw new Exception("0032:: Exchange Rate for Invoice is zero (0).");
                    }
                    else if (decimal.Parse(amount.Trim()) < 0)
                    {
                        throw new Exception("0033:: Exchange Rate for Invoice is negative.");
                    }
                    else
                    {
                        retAmount = Convert.ToDecimal(amount);
                    }

                }
                LogManager.Log.Info("ExchangeRateForInvoice: " + retAmount);
                return retAmount;
            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting Exchange Rate for Invoice : " + e.Message + "<br>");
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                retAmount = null;
                return retAmount;
            }

        }

        public string CommentForInvoice(string comment, ref List<string> validationMsg)
        {
            LogManager.Log.Info("Validate the Comment For Invoice...");
            string retValue = "";
            try
            {
                var separator = getSeparatorForComment(ref validationMsg);
                string[] SplitComment = comment != null ? comment.Split(separator.ToCharArray()) : null;
                if (SplitComment == null || SplitComment.Count() < 5)
                {
                    LogManager.Log.Info("Comment For Invoice is empty.");
                }
                else
                {
                    retValue = SplitComment[4];
                }
                return retValue;
            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting Comment For Invoice: " + e.Message + "<br>");
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                retValue = null;
                return retValue;
            }

        }


        public string Account(string CurrencyForFund, ref List<string> validationMsg)
        {
            LogManager.Log.Info("Validate the Account...");
            string code = null;
            try
            {
                if (CurrencyForFund.ToUpper().Trim() == "HKD")
                {
                    code = "2005167";
                }
                else if (CurrencyForFund.ToUpper().Trim() == "USD")
                {
                    code = "2005168";
                }
                else
                {
                    code = null;
                    throw new Exception("0023:: Currency should be HKD or USD.");
                }
                return code;
            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting Account: " + e.Message + "<br>");
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                return code;
            }

        }
        public string BankReferenceNumberForFund(string comment, string locCode, DateTime? postingDate, ref List<string> validationMsg)
        {
            LogManager.Log.Info("Validate the Bank Reference Number...");
            string retBankRef = null;
            try
            {
                var separator = getSeparatorForComment(ref validationMsg);
                string[] SplitComment = comment != null ? comment.Split(separator.ToCharArray()) : null;
                if (SplitComment == null || SplitComment.Count() < 5)
                {
                    retBankRef = "";
                    throw new Exception("0038:: Bank Reference Number For Fund is empty.");
                }
                else
                {
                    switch (locCode)
                    {
                        case "HKG":
                            retBankRef = SplitComment[4].Trim().Length < 20 ? SplitComment[4].Trim() : SplitComment[4].Trim().Substring(0, 20);
                            break;
                        case "SGP":
                            var firstComment = SplitComment[0].Trim();
                            retBankRef = SplitComment[4].Trim().Length < 20 ? SplitComment[4].Trim() : SplitComment[4].Trim().Substring(0, 20);

                            var regValList = (from v in contextWis.Registries
                                              where v.RegKey == "GSSGPBankRef_" + firstComment
                                              select v.RegValue).ToList();
                            if (regValList.Count == 0)
                            {
                                throw new Exception("0100:: No Record found in Registry table.");
                            }
                            else
                            {
                                string regVal = regValList.FirstOrDefault();
                                string[] regValArr = regVal.Split(",".ToCharArray());
                                string bankRef1 = "";
                                string bankRef2 = "";

                                if (retBankRef.Length > 2)
                                {
                                    bankRef1 = retBankRef.Substring(0, 3).ToUpper();
                                    bankRef2 = retBankRef.Substring(0, 2).ToUpper();
                                }
                                else if (retBankRef.Length == 2)
                                {
                                    bankRef2 = retBankRef.Substring(0, 2).ToUpper();
                                }
                                else
                                {
                                    throw new Exception("0100:: Bank Reference Number is incomplete.");
                                }




                                var codeList = (from r in regValArr
                                                where r == bankRef1 || r == bankRef2
                                                select r
                                           ).ToList();

                                if (codeList.Count() > 0)
                                {

                                    string code = "";

                                    if (codeList.Count > 0)
                                    {
                                        code = codeList.FirstOrDefault();
                                    }


                                    int x = 0;
                                    string year = "";
                                    string month = "";
                                    DateTime dt = postingDate.Value;
                                    string postDateYear = "";
                                    string postDateMonth = "";
                                    string[] slashTest = retBankRef.Split("/".ToCharArray());
                                    switch (code)
                                    {
                                        case "RUC":
                                        case "RUD":
                                        case "RC":
                                        case "RD":
                                            //further validation with "YY/MM/"
                                            //<Bank Reference Number> should be in format: XXXYY/MM/999 or XXYY/MM/999(E.g.RC22/11/001 or RUC22/11/123A)

                                            if (slashTest.Count() < 3 || retBankRef.Length < 8)
                                            {
                                                throw new Exception("0101:: Bank Reference Number is invalid" + " [" + retBankRef + "] " + ".");
                                            }


                                            x = code.Length;
                                            year = retBankRef.Substring(x, 2);
                                            month = retBankRef.Substring(x + 3, 2);
                                            dt = postingDate.Value;
                                            postDateYear = dt.Year.ToString().Substring(2, 2);
                                            postDateMonth = dt.Month.ToString();
                                            postDateMonth = postDateMonth.Length == 1 ? "0" + postDateMonth : postDateMonth;

                                            if ((year != postDateYear) || (month != postDateMonth))
                                            {
                                                throw new Exception("0101:: Bank Reference Number is invalid" + " [" + retBankRef + "] " + ".");
                                            }


                                            break;
                                        case "JV":
                                            //further validation with "YYYY/MM/"
                                            //<Bank Reference Number> should be in format: JVYYYY/MM/999(E.g.JV2022/11/123A)

                                            if (slashTest.Count() < 3 || retBankRef.Length < 10)
                                            {
                                                throw new Exception("0101:: Bank Reference Number is invalid" + " [" + retBankRef + "] " + ".");
                                            }

                                            x = code.Length;
                                            year = retBankRef.Substring(x, 4);
                                            month = retBankRef.Substring(x + 5, 2);
                                            dt = postingDate.Value;
                                            postDateYear = dt.Year.ToString().Substring(0, 4);
                                            postDateMonth = dt.Month.ToString();
                                            postDateMonth = postDateMonth.Length == 1 ? "0" + postDateMonth : postDateMonth;
                                            if ((year != postDateYear) || (month != postDateMonth))
                                            {
                                                throw new Exception("0101:: Bank Reference Number is invalid" + " [" + retBankRef + "] " + ".");
                                            }
                                            break;
                                        default:
                                            //further validation with "YYMM"
                                            //< Bank Reference Number> should be in format: XXXYYMM999 or XXYYMM999(E.g.PD2211001 or PUC2211123A)

                                            if (retBankRef.Length < 7)
                                            {
                                                throw new Exception("0101:: Bank Reference Number is invalid" + " [" + retBankRef + "] " + ".");
                                            }

                                            x = code.Length;
                                            year = retBankRef.Substring(x, 2);
                                            month = retBankRef.Substring(x + 2, 2);
                                            dt = postingDate.Value;
                                            postDateYear = dt.Year.ToString().Substring(2, 2);
                                            postDateMonth = dt.Month.ToString();

                                            if ((year != postDateYear) || (month != postDateMonth))
                                            {
                                                throw new Exception("0102:: Bank Reference Number is not equal to posting date." + " [" + retBankRef + "] " + ".");
                                            }

                                            break;
                                    }

                                }
                                else
                                {
                                    throw new Exception("0103:: Unknown Bank Reference Number.");
                                }



                            }
                            //if (firstComment == "USD-CITI" && retBankRef.Substring(0, 3) != "RUC")
                            //{
                            //    throw new Exception("0096:: Currency is in USD and bank reference is not in RUC/RUD.");
                            //}
                            //else if (firstComment == "SGD-CITI" && retBankRef.Substring(0, 2) != "RC")
                            //{
                            //    throw new Exception("0097:: Currency is in SGD and bank reference is not in RC/RD.");
                            //}
                            //else if (firstComment == "USD-DBS" && retBankRef.Substring(0, 3) != "RUD")
                            //{
                            //    throw new Exception("0096:: Currency is in USD and bank reference is not in RUC/RUD.");
                            //}
                            //else if (firstComment == "SGD-DBS" && retBankRef.Substring(0, 2) != "RD")
                            //{
                            //    throw new Exception("0097:: Currency is in SGD and bank reference is not in RC/RD.");
                            //}
                            //else
                            //{
                            //    retBankRef = SplitComment[4].Trim();
                            //}
                            break;
                        case "THA":
                            retBankRef = SplitComment[4].Trim();
                            break;
                        case "VNM":
                            retBankRef = SplitComment[4].Trim();
                            break;
                        case "VTN":
                            retBankRef = SplitComment[4].Trim();
                            break;
                        case "MMR":
                            retBankRef = SplitComment[4].Trim();
                            break;
                        case "JPN":
                            retBankRef = SplitComment[4].Trim();
                            break;

                        default:
                            retBankRef = SplitComment[4].Trim();
                            break;

                    }


                    if (retBankRef == "")
                    {
                        throw new Exception("0038:: Bank Reference Number For Fund is empty.");
                    }
                    else
                    {
                        retBankRef = retBankRef.Length > 20 ? retBankRef.Substring(0, 20) : retBankRef;
                    }
                }


                return retBankRef;
            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting Type Of Bank Reference Number For Fund: " + e.Message + "<br>");
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                return retBankRef;
            }


        }
        public string DeptForFund(int? id, string PIC, string businessType, string locCode, string typeOfId, string type, string chargeDept, string principalNumber, int? refNo, ref List<string> validationMsg)
        {
            LogManager.Log.Info("Validate the Dept For Fund...");
            string dept = null;
            try
            {
                locCode = locCode != null || locCode != "" ? locCode.ToUpper() : locCode;
                switch (locCode)
                {
                    case "HKG":
                        if ((principalNumber == "C072-01" || principalNumber == "H001-01") && HasDynamicFieldInExpTemplate("1200", refNo, ref validationMsg) && (chargeDept == null || chargeDept == ""))
                        {
                            throw new Exception("0091:: Principal Number is either C072-01 or H001-01, has dynamic field of 1200 and charge dept is null or blank.");
                        }
                        else
                        {
                            if (chargeDept != "" && chargeDept != null)
                            {
                                dept = chargeDept;
                            }
                            else
                            {
                                var pic = PIC;
                                if (pic == "" || pic == null)
                                {
                                    var busstype = businessType;
                                    var query = (from v in contextWis.Registries
                                                 where v.RegKey == "a3DeptCodeForFund_" + busstype
                                                 select v.RegValue).ToList();
                                    if (query == null || query.Count() == 0)
                                    {
                                        dept = null;
                                        throw new Exception("0039:: Dept For Fund is null.");
                                    }
                                    else
                                    {
                                        dept = query.SingleOrDefault().ToString().Trim();
                                    }
                                }
                                else
                                {
                                    dept = DeptByPIC(id, PIC, locCode, typeOfId, type, ref validationMsg);
                                    if (dept == null)
                                    {
                                        throw new Exception("0039:: Dept For Fund is null.");
                                    }
                                }
                            }
                        }
                        break;
                    case "SGP":
                        dept = "0000";
                        break;
                    case "THA":
                        dept = "0000";
                        break;
                    case "VNM":
                        dept = "0000";
                        break;
                    case "VTN":
                        dept = "0000";
                        break;
                    case "MMR":
                        dept = "0000";
                        break;
                    case "JPN":
                        dept = "0000";
                        break;

                    default:
                        dept = "0000";
                        break;
                }


                return dept;
            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting Type Of Bank Reference Number For Fund: " + e.Message + "<br>");
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                return dept;
            }


        }
        public string DeptByPIC(int? id, string PIC, string locCode, string typeOfId, string type, ref List<string> validationMsg)
        {
            LogManager.Log.Info("Validate the Dept By PIC...");
            string deptpic = null;

            try
            {
                locCode = locCode != null || locCode != "" ? locCode.ToUpper() : locCode;
                if (locCode != null || locCode != string.Empty)
                {
                    switch (locCode)
                    {

                        case "HKG":
                            //ErrorMessage when using CodeFirst: This server version is not supported. You must have Microsoft SQL Server 2005 or later                
                            var picode = PIC;

                            var staff = (from v in contextWis.Staffs
                                         where v.init_ == picode
                                         select new { v.loc, v.co, v.dept }).ToList();

                            if (staff.Count() == 0 || staff == null)
                            {
                                throw new Exception("0037:: Dept By PIC does not exist in Staff.");
                            }
                            else
                            {
                                var keyword = "a3DeptCodeFor" + type + "_" + staff.FirstOrDefault().loc + staff.FirstOrDefault().co + staff.FirstOrDefault().dept;
                                var pic = (from v in contextWis.Registries
                                           where v.RegKey == keyword
                                           select v.RegValue);
                                if (pic == null || pic.Count() == 0)
                                {
                                    deptpic = null;
                                    throw new Exception("0040:: Dept By PIC does not exist in Registry.");
                                }
                                else
                                {
                                    deptpic = pic.SingleOrDefault().ToString().Trim();
                                }

                            }
                            break;

                        case "SGP":
                            deptpic = "0000";
                            break;

                        case "THA":
                            deptpic = "0000";
                            break;

                        case "VNM":
                            deptpic = "0000";
                            break;

                        case "VTN":
                            deptpic = "0000";
                            break;

                        case "MMR":
                            deptpic = "0000";
                            break;
                        case "JPN":
                            deptpic = "0000";
                            break;

                        default:
                            deptpic = "0000";
                            break;
                    }


                    return deptpic;

                }
                else
                {
                    throw new Exception("0001:: Location code not found.");
                }

            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting Dept By PIC: " + e.Message + "<br>");
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                return deptpic;
            }

        }

        public string DebitBankForFCAdv(string locCode, string usdBankAccountNoJPN, string jpyBankAccountNoJPN, string comment, ref List<string> validationMsg)
        {
            var debit = "";
            try
            {
                var firstSection = FirstSectionOfFundComment(comment, ref validationMsg);
                locCode = locCode != null || locCode != "" ? locCode.ToUpper() : locCode;

                var accountcode = (from a in contextWis.Registries
                                   where a.RegKey.Contains("DrCrBank")
                                   select a).ToList();

                switch (locCode)
                {
                    case "HKG":
                        if (firstSection == "USD")
                        {
                            debit = accountcode.Where(a => a.RegKey == "DrCrBank1").FirstOrDefault().RegValue;
                        }
                        else if (firstSection == "HKD")
                        {
                            debit = accountcode.Where(a => a.RegKey == "DrCrBank2").FirstOrDefault().RegValue;
                        }
                        else
                        {
                            throw new Exception("0046:: Debit bank cannot be found.");
                        }
                        break;
                    case "SGP":
                        if (firstSection == "USD-CITI")
                        {
                            debit = accountcode.Where(a => a.RegKey == "DrCrBank1").FirstOrDefault().RegValue;
                        }
                        else if (firstSection == "SGD-CITI")
                        {
                            debit = accountcode.Where(a => a.RegKey == "DrCrBank3").FirstOrDefault().RegValue;
                        }
                        else if (firstSection == "USD-DBS")
                        {
                            debit = accountcode.Where(a => a.RegKey == "DrCrBank4").FirstOrDefault().RegValue;
                        }
                        else if (firstSection == "SGD-DBS")
                        {
                            debit = accountcode.Where(a => a.RegKey == "DrCrBank5").FirstOrDefault().RegValue;
                        }
                        else
                        {
                            throw new Exception("0046:: Debit bank cannot be found.");
                        }
                        break;
                    case "THA":
                        if (firstSection == "" || firstSection == null)
                        {
                            throw new Exception("0046:: Debit bank cannot be found.");
                        }
                        else
                        {
                            debit = accountcode.Where(a => a.RegKey == "DrCrBank6").FirstOrDefault().RegValue;
                        }
                        break;
                    case "VNM":
                        if (firstSection == "" || firstSection == null)
                        {
                            throw new Exception("0046:: Debit bank cannot be found.");
                        }
                        else
                        {
                            debit = accountcode.Where(a => a.RegKey == "DrCrBank7").FirstOrDefault().RegValue;
                        }
                        break;
                    case "MMR":
                        if (firstSection == "" || firstSection == null)
                        {
                            throw new Exception("0046:: Debit bank cannot be found.");
                        }
                        else
                        {
                            debit = accountcode.Where(a => a.RegKey == "DrCrBank8").FirstOrDefault().RegValue;
                        }
                        break;
                    case "JPN":
                        if (firstSection == "USD")
                        {
                            debit = usdBankAccountNoJPN;
                        }
                        else if (firstSection == "JPY")
                        {
                            debit = jpyBankAccountNoJPN;
                        }
                        else
                        {
                            throw new Exception("0046:: Debit bank cannot be found.");
                        }
                        break;
                    case "VTN":
                        if (firstSection == "USD")
                        {
                            debit = accountcode.Where(a => a.RegKey == "DrCrBank9").FirstOrDefault().RegValue;
                        }
                        else if (firstSection == "VND")
                        {
                            debit = accountcode.Where(a => a.RegKey == "DrCrBank10").FirstOrDefault().RegValue;
                        }
                        else
                        {
                            throw new Exception("0046:: Debit bank cannot be found.");
                        }
                        break;
                    default:
                        LogManager.Log.Info("No Location Code or Unknown Location Code indicated in config file. Location Code:[" + locCode + "]");
                        throw new Exception("0001:: Location Code not found.");
                }


                return debit;
            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting Debit Bank for FCAdv " + e.Message + ":::" + e.InnerException);
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                return debit;
            }

        }

        public string DebitAccountCodeForInvoice(string a3LocCode, string postingCode, string code1, string code2, ref List<string> validationMsg)
        {
            LogManager.Log.Info("Validate the Debit Account Code for Invoice...");
            string retdac = null;
            try
            {
                var accountCode = (from a in contextWis.Registries
                                   where a.RegKey.Contains("DebitAccountCode")
                                   select a).ToList();


                switch (a3LocCode)
                {
                    case "HKG":
                        if (postingCode != string.Empty && postingCode != null)
                        {
                            retdac = accountCode.Where(a => a.RegKey == "DebitAccountCode1").FirstOrDefault().RegValue;
                        }
                        else if ((code1 != string.Empty && code1 != null) && (code2 != string.Empty && code2 != null))
                        {
                            retdac = code1;
                        }
                        else if ((code1 != string.Empty && code1 != null) && (code2 == string.Empty || code2 == null))
                        {
                            retdac = code1;
                        }
                        else
                        {
                            LogManager.Log.Info("Debit Account Code For Invoice not found.");
                            throw new Exception("0047:: Debit Account Code For Invoice not found.");
                        }

                        break;

                    case "SGP":
                        if (postingCode != string.Empty && postingCode != null)
                        {
                            retdac = accountCode.Where(a => a.RegKey == "DebitAccountCode1").FirstOrDefault().RegValue;
                        }
                        else if ((code1 != string.Empty && code1 != null) && (code2 != string.Empty && code2 != null))
                        {
                            retdac = code1;
                        }
                        else if ((code1 != string.Empty && code1 != null) && (code2 == string.Empty || code2 == null))
                        {
                            retdac = code1;
                        }
                        else
                        {
                            LogManager.Log.Info("Debit Account Code For Invoice not found.");
                            throw new Exception("0047:: Debit Account Code For Invoice not found.");
                        }

                        break;
                    case "THA":
                        if (postingCode != string.Empty && postingCode != null)
                        {
                            retdac = accountCode.Where(a => a.RegKey == "DebitAccountCode1").FirstOrDefault().RegValue;
                        }
                        else if ((code1 != string.Empty && code1 != null) && (code2 != string.Empty && code2 != null))
                        {
                            retdac = code1;
                        }
                        else if ((code1 != string.Empty && code1 != null) && (code2 == string.Empty || code2 == null))
                        {
                            retdac = code1;
                        }
                        else
                        {
                            LogManager.Log.Info("Debit Account Code For Invoice not found.");
                            throw new Exception("0047:: Debit Account Code For Invoice not found.");
                        }

                        break;
                    case "VNM":
                        if (postingCode != string.Empty && postingCode != null)
                        {
                            retdac = "2000198";
                        }
                        else if ((code1 != string.Empty && code1 != null) && (code2 != string.Empty && code2 != null))
                        {
                            retdac = code1;
                        }
                        else if ((code1 != string.Empty && code1 != null) && (code2 == string.Empty || code2 == null))
                        {
                            retdac = code1;
                        }
                        else
                        {
                            LogManager.Log.Info("Debit Account Code For Invoice not found.");
                            throw new Exception("0047:: Debit Account Code For Invoice not found.");
                        }

                        break;

                    case "MMR":
                        if (postingCode != string.Empty && postingCode != null)
                        {
                            retdac = accountCode.Where(a => a.RegKey == "DebitAccountCode1").FirstOrDefault().RegValue;
                        }
                        else if ((code1 != string.Empty && code1 != null) && (code2 != string.Empty && code2 != null))
                        {
                            retdac = code1;
                        }
                        else if ((code1 != string.Empty && code1 != null) && (code2 == string.Empty || code2 == null))
                        {
                            retdac = code1;
                        }
                        else
                        {
                            LogManager.Log.Info("Debit Account Code For Invoice not found.");
                            throw new Exception("0047:: Debit Account Code For Invoice not found.");
                        }

                        break;

                    case "JPN":
                        if (postingCode != string.Empty && postingCode != null)
                        {
                            retdac = "2000192";
                        }
                        else if ((code1 != string.Empty && code1 != null) && (code2 != string.Empty && code2 != null))
                        {
                            retdac = code1;
                        }
                        else if ((code1 != string.Empty && code1 != null) && (code2 == string.Empty || code2 == null))
                        {
                            retdac = code1;
                        }
                        else
                        {
                            LogManager.Log.Info("Debit Account Code For Invoice not found.");
                            throw new Exception("0047:: Debit Account Code For Invoice not found.");
                        }

                        break;

                    case "VTN":
                        if (postingCode != string.Empty && postingCode != null)
                        {
                            retdac = "2000198";
                        }
                        else if ((code1 != string.Empty && code1 != null) && (code2 != string.Empty && code2 != null))
                        {
                            retdac = code1;
                        }
                        else if ((code1 != string.Empty && code1 != null) && (code2 == string.Empty || code2 == null))
                        {
                            retdac = code1;
                        }
                        else
                        {
                            LogManager.Log.Info("Debit Account Code For Invoice not found.");
                            throw new Exception("0047:: Debit Account Code For Invoice not found.");
                        }

                        break;

                    default:
                        break;
                }
                return retdac;
            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting Debit Account Code For Invoice: " + e.Message + ":::" + e.InnerException);
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                return retdac;
            }

        }
        public string CreditAccountTypeForInvoice(string a3LocCode, string postingCode, string code1, string code2, ref List<string> validationMsg)
        {
            var creditAccountType = "";
            try
            {
                var postingCodeVal = (from p in contextWis.Registries
                                      where p.RegKey == "PostingCode"
                                      select p).ToList();

                if (postingCode != string.Empty && postingCode != null)
                {
                    if (postingCode == postingCodeVal.Where(p => p.RegKey == "PostingCode1").FirstOrDefault().RegValue)
                    {
                        creditAccountType = "Bank";
                    }
                    else
                    {
                        creditAccountType = "Ledger";
                    }
                }
                else if ((code1 != string.Empty && code1 != null) && (code2 != string.Empty && code2 != null))
                {
                    creditAccountType = "Bank";
                }
                else if ((code1 != string.Empty && code1 != null) && (code2 == string.Empty || code2 == null))
                {
                    creditAccountType = "Vendor";
                }
                else
                {
                    LogManager.Log.Info("Credit Account Type For Invoice not found.");
                    throw new Exception("0048:: Credit Account Type For Invoice not found.");
                }

                return creditAccountType;
            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting Credit Account Type For Invoice: " + e.Message + ":::" + e.InnerException);
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                return creditAccountType;
            }

        }

        public string CreditAccountCodeForInvoice(string a3LocCode, string postingCode, string vendorCode, string code1, string code2, ref List<string> validationMsg)
        {
            LogManager.Log.Info("Validate the Credit Account Code for Invoice...");
            string retdac = null;
            try
            {

                if (postingCode != string.Empty && postingCode != null)
                {
                    retdac = postingCode;
                }
                else if ((code1 != string.Empty && code1 != null) && (code2 != string.Empty && code2 != null))
                {
                    retdac = code2;
                }
                else if ((code1 != string.Empty && code1 != null) && (code2 == string.Empty || code2 == null))
                {
                    retdac = vendorCode;
                }
                else
                {
                    LogManager.Log.Info("Credit Account Code For Invoice not found.");
                    throw new Exception("0049:: Credit Account Code For Invoice not found.");
                }

                return retdac;
            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting Credit Account Code For Invoice: " + e.Message + ":::" + e.InnerException);
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                return retdac;
            }

        }
        public string DebitAccountCodeForDebitNote(string a3LocCode, string postingCode, string code1, string code2, ref List<string> validationMsg)
        {
            LogManager.Log.Info("Validate the Debit Account Code...");
            string retdac = null;
            try
            {
                var accountCode = (from a in contextWis.Registries
                                   where a.RegKey.Contains("DebitAccountCode")
                                   select a).ToList();

                switch (a3LocCode)
                {
                    case "HKG":
                        if (postingCode != string.Empty && postingCode != null)
                        {
                            retdac = accountCode.Where(a => a.RegKey == "DebitAccountCode1").FirstOrDefault().RegValue;
                        }
                        else if ((code1 != string.Empty && code1 != null) && (code2 != string.Empty && code2 != null))
                        {
                            retdac = code1;
                        }
                        else
                        {
                            LogManager.Log.Info("Debit Account Code For Debit Note not found.");
                            throw new Exception("0050:: Debit Account Code For Debit Note not found.");
                        }
                        break;

                    case "SGP":
                        if (postingCode != string.Empty && postingCode != null)
                        {
                            retdac = accountCode.Where(a => a.RegKey == "DebitAccountCode1").FirstOrDefault().RegValue;
                        }
                        else if ((code1 != string.Empty && code1 != null) && (code2 != string.Empty && code2 != null))
                        {
                            retdac = code1;
                        }
                        else
                        {
                            LogManager.Log.Info("Debit Account Code For Debit Note not found.");
                            throw new Exception("0050:: Debit Account Code For Debit Note not found.");
                        }
                        break;
                    case "THA":
                        if (postingCode != string.Empty && postingCode != null)
                        {
                            retdac = accountCode.Where(a => a.RegKey == "DebitAccountCode1").FirstOrDefault().RegValue;
                        }
                        else if ((code1 != string.Empty && code1 != null) && (code2 != string.Empty && code2 != null))
                        {
                            retdac = code1;
                        }
                        else
                        {
                            LogManager.Log.Info("Debit Account Code For Debit Note not found.");
                            throw new Exception("0050:: Debit Account Code For Debit Note not found.");
                        }
                        break;
                    case "VNM":
                        if (postingCode != string.Empty && postingCode != null)
                        {
                            retdac = "2000198";
                        }
                        else if ((code1 != string.Empty && code1 != null) && (code2 != string.Empty && code2 != null))
                        {
                            retdac = code1;
                        }
                        else
                        {
                            LogManager.Log.Info("Debit Account Code For Debit Note not found.");
                            throw new Exception("0050:: Debit Account Code For Debit Note not found.");
                        }
                        break;
                    case "MMR":
                        if (postingCode != string.Empty && postingCode != null)
                        {
                            retdac = accountCode.Where(a => a.RegKey == "DebitAccountCode1").FirstOrDefault().RegValue;
                        }
                        else if ((code1 != string.Empty && code1 != null) && (code2 != string.Empty && code2 != null))
                        {
                            retdac = code1;
                        }
                        else
                        {
                            LogManager.Log.Info("Debit Account Code For Debit Note not found.");
                            throw new Exception("0050:: Debit Account Code For Debit Note not found.");
                        }
                        break;
                    case "JPN":
                        if (postingCode != string.Empty && postingCode != null)
                        {
                            retdac = "2000192";
                        }
                        else if ((code1 != string.Empty && code1 != null) && (code2 != string.Empty && code2 != null))
                        {
                            retdac = code1;
                        }
                        else
                        {
                            LogManager.Log.Info("Debit Account Code For Debit Note not found.");
                            throw new Exception("0050:: Debit Account Code For Debit Note not found.");
                        }
                        break;
                    case "VTN":
                        if (postingCode != string.Empty && postingCode != null)
                        {
                            retdac = "2000198";
                        }
                        else if ((code1 != string.Empty && code1 != null) && (code2 != string.Empty && code2 != null))
                        {
                            retdac = code1;
                        }
                        else
                        {
                            LogManager.Log.Info("Debit Account Code For Debit Note not found.");
                            throw new Exception("0050:: Debit Account Code For Debit Note not found.");
                        }
                        break;

                    default:
                        break;
                }
                return retdac;
            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting Debit Account Code For Debit: " + e.Message + ":::" + e.InnerException);
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                return retdac;
            }

        }
        public string CreditAccountCodeForDebitNote(string a3LocCode, string postingCode, string code1, string code2, ref List<string> validationMsg)
        {
            LogManager.Log.Info("Validate the Credit Account Code...");
            string retcac = null;
            try
            {
                if (postingCode != string.Empty && postingCode != null)
                {
                    retcac = postingCode;
                }
                else if ((code1 != string.Empty && code1 != null) && (code2 != string.Empty && code2 != null))
                {
                    retcac = code2;
                }
                else
                {
                    LogManager.Log.Info("Credit Account Code For Debit Note not found.");
                    throw new Exception("0051:: Credit Account Code For Debit Note not found.");
                }
                return retcac;
            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting Credit Account Code For Debit Note: " + e.Message + ":::" + e.InnerException);
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                return retcac;
            }

        }
        public string InvoiceNumber(string reference, ref List<string> validationMsg)
        {
            LogManager.Log.Info("Validate the Invoice File Number...");
            string retInvoiceNumber = null;
            try
            {

                if (reference == null || reference == "")
                {
                    retInvoiceNumber = null;
                    throw new Exception("0053:: Missing Invoice File Number.");
                }
                else
                {
                    retInvoiceNumber = reference;
                }
                return retInvoiceNumber;
            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting Invoice File Number: " + e.Message + ":::" + e.InnerException);
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                return retInvoiceNumber;
            }

        }

        public string InvoiceNumberWithSuffix(int? daId, string vendorCode, string typeOfId, int? companyId, string reference, string invoiceNo, ref bool tmpIsSuffixed, ref List<string> validationMsg)
        {
            LogManager.Log.Info("Validate the Invoice Number With Suffix...");
            string retInvoiceNumberWithSuffix = null;
            //string tmpKey = invoiceNo;
            try
            {
                //var suffixNo = InvoiceSuffixNo(companyId, invoiceNo, vendorCode, ref validationMsg);

                //if (suffixNo == null)
                //{
                //    retInvoiceNumberWithSuffix = tmpKey;
                //    tmpIsSuffixed = false;
                //}
                //else
                //{
                //    retInvoiceNumberWithSuffix = tmpKey + "(" + (suffixNo + 1) + ")";
                //    tmpIsSuffixed = true;
                //}

                retInvoiceNumberWithSuffix = AddSuffix(invoiceNo, vendorCode, ref tmpIsSuffixed, ref validationMsg);

                return retInvoiceNumberWithSuffix;
            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting Type Of Invoice Number: " + e.Message + ":::" + e.InnerException);
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                return retInvoiceNumberWithSuffix;
            }

        }
        public decimal? AmountForDMCom(int? id, decimal? totalAmount, string NomTypeName, ref List<string> validationMsg)
        {
            LogManager.Log.Info("Validate the Amount For DMCom...");
            decimal? retAmount = null;

            try
            {
                retAmount = Convert.ToDecimal(string.Format("{0:F2}", Decimal.Round((totalAmount).Value, 2)));
                return retAmount;
            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting Amount for DMCom: " + e.Message + ":::" + e.InnerException);
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                return retAmount;
            }

        }
        public decimal? BankChargeForFund(string comment, ref List<string> validationMsg)
        {
            LogManager.Log.Info("Validate the Bank Charge For Fund...");
            decimal? bankcharge = null;
            try
            {
                var separator = getSeparatorForComment(ref validationMsg);
                string[] SplitComment = comment != null ? comment.Split(separator.ToCharArray()) : null;
                if (SplitComment == null || SplitComment.Count() < 3)
                {
                    bankcharge = null;
                    throw new Exception("0056:: Bank Charge For Fund is empty.");
                }
                else
                {
                    string amount = SplitComment[2];
                    if (amount.Trim() == "")
                    {
                        throw new Exception("0056:: Bank Charge For Fund is empty.");
                    }
                    else
                    {
                        bankcharge = Convert.ToDecimal(amount);
                        bankcharge = Convert.ToDecimal(string.Format("{0:F2}", Decimal.Round(bankcharge.Value, 2)));
                    }

                }
                return bankcharge;
            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting Comment For Invoice: " + e.Message + "<br>");
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                bankcharge = null;
                return bankcharge;
            }
        }

        public string DeptForInvoice(int? id, string PIC, string bussinessType, string locCode, string typeOfId, string type, string chargeDept, int? expTemplateId, string principalNumber, string vendorCode, string SMCVendorCode, ref List<string> validationMsg)
        {
            LogManager.Log.Info("Validate the Dept For Invoice...");
            var retDept = "";
            try
            {
                var expTemplateIds = (from e in contextWis.Registries
                                      where (e.RegKey == "GSExpTemplateIDForICInvCost" ||
                                           e.RegKey == "GSExpTemplateIDForFPE_CNCO" ||
                                           e.RegKey == "GSExpTemplateIDForTramp_C" ||
                                           e.RegKey == "GSExpTemplateIDForDNtMisc_AR" ||
                                           e.RegKey == "GSExpTemplateIDForDNt_AR" ||
                                           e.RegKey == "GSExpTemplateIDForHoegh_FRT") &&
                                           (chargeDept == null || chargeDept == "") &&
                                           e.RegValue == expTemplateId.ToString()
                                      select e.RegValue).ToList();


                if (expTemplateIds.Count > 0)
                {
                    throw new Exception("0092:: ExpTemplateId not found in the Registry and Charge Dept is null or blank.");
                }
                else if (vendorCode != SMCVendorCode && (principalNumber == "C072-01" || principalNumber == "H001-01") && (chargeDept == null || chargeDept == ""))
                {
                    throw new Exception("0093:: Vendor Code is not equal to SMC Vendor Code and Principal Number is either C072-01 or H001-01 and Charge Dept is null or blank.");
                }
                else
                {
                    if (chargeDept != null && chargeDept != "")
                    {
                        retDept = chargeDept;
                    }
                    else
                    {
                        if (PIC == "")
                        {
                            var businesstype = bussinessType;
                            var query = (from v in contextWis.Registries
                                         where v.RegKey == "a3DeptCodeForInvoice_" + businesstype
                                         select v.RegValue).ToList();
                            if (query == null || query.Count() == 0)
                            {
                                retDept = null;
                                throw new Exception("0058:: Dept For Invoice does not exist in Registry.");
                            }
                            else
                            {
                                retDept = query.SingleOrDefault().ToString().Trim();
                            }
                        }
                        else
                        {
                            retDept = DeptByPIC(id, PIC, locCode, typeOfId, type, ref validationMsg);
                        }
                    }
                }
                return retDept;
            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting Dept For Invoice: " + e.Message + ":::" + e.InnerException);
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                return retDept;
            }

        }
        public decimal? ActualSalesTaxAmount(string locCode, decimal? amount, decimal? amountWithTaxForInvoice, bool isDebitNote, string comment, ref List<string> validationMsg)
        {
            LogManager.Log.Info("Validate the Actual Sales Tax Amount...");
            decimal? actualsales = null;
            try
            {
                var issales = IsSalesTax(locCode, ref validationMsg);
                var exchange = ExchangeRateForInvoice(comment, ref validationMsg);
                bool isGenDN = IsGenDNPerSer(locCode, ref validationMsg);

                if (issales)
                {
                    actualsales = (amountWithTaxForInvoice - amount);
                    actualsales = actualsales != null ? Convert.ToDecimal(string.Format("{0:F2}", Decimal.Round(actualsales.Value, 2))) : (decimal?)null;
                }
                else
                {
                    actualsales = null;
                }
                return actualsales;
            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting Actual Sales Tax Amount: " + e.Message + ":::" + e.InnerException);
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                return actualsales;
            }
        }
        public int? SequenceNumber(ref List<string> validationMsg)
        {
            LogManager.Log.Info("Validate the Sequence Number...");
            int? sequenceNo = 0;
            try
            {
                var eventJournal = a3Helper.EventJournal();

                if (eventJournal.Count() > 0)
                {
                    var sqno = (from seq in eventJournal
                                where seq.JrnlDate == DateTime.Now.Date
                                group seq by seq.JrnlDate into sq
                                select new { seqNo = sq.Max(x => x.SeqNo) }).ToList();

                    if (sqno.Count() == 0)
                    {
                        sequenceNo = 1;
                    }
                    else
                    {
                        sequenceNo = sqno.FirstOrDefault().seqNo + 1;
                    }
                }
                return sequenceNo;
            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting Sequence Number: " + e.Message + ":::" + e.InnerException);
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                return sequenceNo;
            }
        }

        public string FileBatchId(string actionEntity, int? sequenceNo, string smcCode, ref List<string> validationMsg)
        {
            LogManager.Log.Info("Validate the File Batch ID...");
            string batchId = "";

            try
            {
                //e.g.A3_20190718_0001_FCAdv
                batchId = Config.SystemCode + "_" + DateTime.Now.Date.ToString("yyyyMMdd") + "_" + (sequenceNo).ToString().PadLeft(4, '0') + "_" + actionEntity + "_" + smcCode;
                LogManager.Log.Info("BatchId: " + batchId);
                return batchId;
            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting Dept For Invoice: " + e.Message + ":::" + e.InnerException);
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                return batchId;
            }
        }
        public bool ShowVoyageCodeHour(int? id, string locCode, string typeOfId, ref List<string> validationMsg)
        {
            LogManager.Log.Info("Validate the Show Voyage Code hour...");
            bool show = false;
            try
            {
                var query = (from v in contextWis.a3SMCMappings
                             where v.a3LocCode == locCode
                             select v.isShowVoyageCodeHour).ToList();
                if (query == null || query.Count() == 0)
                {
                    show = false;
                }
                else
                {
                    show = query.FirstOrDefault();
                }

                return show;
            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting ShowVoyageCodeHour: " + e.Message + ":::" + e.InnerException);
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                return show;
            }

        }

        public string NominationType(string nominationType, ref List<string> validationMsg)
        {
            LogManager.Log.Info("Validate the Nomination Type...");
            string retDeparture = null;
            try
            {
                var shortName = (from v in contextWis.a3NominationTypes
                                 where v.NomTypeName == nominationType
                                 select v.NomTypeShortName).ToList();
                if (shortName == null || shortName.Count() == 0)
                {
                    retDeparture = null;
                    throw new Exception("0067:: Nomination Type is null.");
                }
                else
                {
                    retDeparture = shortName.SingleOrDefault().ToString();
                }
                return retDeparture;
            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting Nomination Type: " + e.Message + ":::" + e.InnerException);
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                return retDeparture;
            }
        }

        public string NomTypeCode(string NomTypeName, ref List<string> validationMsg)
        {
            LogManager.Log.Info("Validate the Nomination Type Code...");
            string retNomTypeCode = null;
            try
            {
                var TypeCode = (from v in contextWis.a3NominationTypes
                                where v.NomTypeName == NomTypeName
                                select v.NomTypeCode).ToList();
                if (TypeCode == null || TypeCode.Count() == 0)
                {
                    retNomTypeCode = null;
                    throw new Exception("0068:: Nomination Type Code does not exist in a3NominationTypes.");
                }
                else
                {
                    retNomTypeCode = TypeCode.SingleOrDefault().ToString();
                }
                return retNomTypeCode;
            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting Nomination Type Code: " + e.Message + ":::" + e.InnerException);
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                return retNomTypeCode;
            }
        }

        public int? InvoiceSuffixNo(int? companyId, string invoiceNo, string vendorCode, ref List<string> validationMsg)
        {
            LogManager.Log.Info("Validate the Suffix Number...");
            int? suffixNum = null;
            try
            {
                string tmpKey = invoiceNo;
                if (tmpKey != null)
                {
                    tmpKey = (tmpKey.Length > 47 ? tmpKey.Substring(0, 47) : tmpKey);
                }

                var suffixNo = (from a in contextWis.a3InvoiceSuffixes
                                where a.VendorCode == vendorCode && a.InvoiceNo == tmpKey
                                select a.SuffixNo).ToList();

                if (suffixNo == null || suffixNo.Count() == 0)
                {
                    suffixNum = null;
                }
                else
                {
                    suffixNum = int.Parse(suffixNo.SingleOrDefault().ToString());
                }

                LogManager.Log.Info("InvoiceSuffixNo value: " + suffixNum);
                return suffixNum;
            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting Suffix Number: " + e.Message + ":::" + e.InnerException);
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                return suffixNum;
            }
        }
        public string InvoiceWithSuffixForDAComplete(int? id, string appointmentVesselController, string vesselName, string nominationFileNumber, string principalNum, string locCode, string typeOfId, ref string tmpKey, string voyageCode, ref bool tmpIsSuffixedDa, ref List<string> validationMsg)
        {
            LogManager.Log.Info("Validate the Suffix Number for DA Complete...");
            string Invoicesuffix = null;
            try
            {
                var dummyVesselName = (from v in contextWis.Registries
                                       where v.RegKey == "a3DummyVesselName"
                                       select v.RegValue).FirstOrDefault().ToString();

                var hoeghCompanyName = (from h in contextWis.Registries
                                        where h.RegKey == "a3HoeghCompanyName"
                                        select h.RegValue).FirstOrDefault().ToString();

                if (appointmentVesselController.ToUpper() == hoeghCompanyName.ToUpper() && principalNum.ToUpper() == "T054-01" && voyageCode != "0000")
                {
                    tmpKey = nominationFileNumber;
                }

                if (vesselName == dummyVesselName || principalNum == "M064-01")
                {
                    tmpKey = nominationFileNumber;
                }

                var temp = tmpKey;
                var suffix = (from v in contextWis.a3DAComInvoiceSuffixes
                              where v.a3LocCode == locCode && v.DAComInvoice == temp
                              select v.SuffixNo).ToList();
                if (suffix == null || suffix.Count() == 0)
                {
                    Invoicesuffix = tmpKey;
                    tmpIsSuffixedDa = false;
                }
                else
                {
                    var suffixNumber = int.Parse(suffix.FirstOrDefault().ToString());
                    Invoicesuffix = tmpKey + "(" + (suffixNumber + 1) + ")";
                    tmpIsSuffixedDa = true;
                }
                return Invoicesuffix;
            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting Suffix Number for DA Complete: " + e.Message + ":::" + e.InnerException);
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                return Invoicesuffix;
            }
        }
        public int? SuffixNo(int? id, string locCode, string typeOfId, string tmpKey, ref List<string> validationMsg)
        {
            LogManager.Log.Info("Validate the Suffix Number for DA Complete...");
            int? sufnum = null;
            try
            {
                var suffix = (from v in contextWis.a3DAComInvoiceSuffixes
                              where v.a3LocCode == locCode && v.DAComInvoice == tmpKey
                              select v.SuffixNo).ToList();
                if (suffix == null || suffix.Count() == 0)
                {
                    sufnum = null;
                }
                else
                {
                    sufnum = int.Parse(suffix.SingleOrDefault().ToString()) + 1;
                }

                LogManager.Log.Info("SuffixNo is " + sufnum);
                return sufnum;
            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting Suffix Number for DA Complete: " + e.Message + ":::" + e.InnerException);
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                return sufnum;
            }
        }
        public string CreditAccountForDMCom(string locCode, ref List<string> validationMsg)
        {
            string crAccount = "";

            try
            {
                locCode = locCode != null || locCode != "" ? locCode.ToUpper() : locCode;

                var accountcode = (from a in contextWis.Registries
                                   where a.RegKey.Contains("CreditAccountCodeDMCOM")
                                   select a).ToList();

                switch (locCode)
                {

                    case "HKG":
                        crAccount = accountcode.Where(a => a.RegKey == "CreditAccountCodeDMCOM1").FirstOrDefault().RegValue;
                        break;
                    case "SGP":
                        crAccount = accountcode.Where(a => a.RegKey == "CreditAccountCodeDMCOM1").FirstOrDefault().RegValue;
                        break;
                    case "THA":
                        crAccount = accountcode.Where(a => a.RegKey == "CreditAccountCodeDMCOM1").FirstOrDefault().RegValue;
                        break;
                    case "VNM":
                        crAccount = accountcode.Where(a => a.RegKey == "CreditAccountCodeDMCOM2").FirstOrDefault().RegValue;
                        break;
                    case "MMR":
                        crAccount = accountcode.Where(a => a.RegKey == "CreditAccountCodeDMCOM1").FirstOrDefault().RegValue;
                        break;
                    case "JPN":
                        crAccount = accountcode.Where(a => a.RegKey == "CreditAccountCodeDMCOM3").FirstOrDefault().RegValue;
                        break;
                    case "VTN":
                        crAccount = accountcode.Where(a => a.RegKey == "CreditAccountCodeDMCOM2").FirstOrDefault().RegValue;
                        break;

                    default:
                        LogManager.Log.Info("No Location Code or Unknown Location Code indicated in config file. Location Code:[" + locCode + "]");
                        throw new Exception("0001:: Location Code not found.");

                }

                return crAccount;


            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting Credit Amount For DMCom: " + e.Message + ":::" + e.InnerException);
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                return "";
            }


        }

        public string DebitBankChargeforFCFin(string locCode, ref List<string> validationMsg)
        {
            string drBankCharge = "";

            try
            {
                locCode = locCode != null || locCode != "" ? locCode.ToUpper() : locCode;
                var accountCode = (from p in contextWis.Registries
                                      where p.RegKey.Contains("DebitBankCharge")
                                      select p).ToList();

                switch (locCode)
                {
                    case "HKG":
                        drBankCharge = accountCode.Where(a => a.RegKey == "DebitBankCharge1").FirstOrDefault().RegValue;
                        break;
                    case "SGP":
                        drBankCharge = accountCode.Where(a => a.RegKey == "DebitBankCharge1").FirstOrDefault().RegValue;
                        break;
                    case "THA":
                        drBankCharge = accountCode.Where(a => a.RegKey == "DebitBankCharge1").FirstOrDefault().RegValue;
                        break;
                    case "VNM":
                        drBankCharge = accountCode.Where(a => a.RegKey == "DebitBankCharge1").FirstOrDefault().RegValue;
                        break;
                    case "MMR":
                        drBankCharge = accountCode.Where(a => a.RegKey == "DebitBankCharge1").FirstOrDefault().RegValue;
                        break;
                    case "VTN":
                        drBankCharge = accountCode.Where(a => a.RegKey == "DebitBankCharge1").FirstOrDefault().RegValue;
                        break;
                    default:
                        drBankCharge = accountCode.Where(a => a.RegKey == "DebitBankCharge1").FirstOrDefault().RegValue;
                        break;
                }

                return drBankCharge;
            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting Credit Amount For DMCom: " + e.Message + ":::" + e.InnerException);
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                return "";
            }


        }

        public string DebitBankChargeforFCAdv(string locCode, ref List<string> validationMsg)
        {
            string drBankCharge = "";

            try
            {
                locCode = locCode != null || locCode != "" ? locCode.ToUpper() : locCode;
                var accountCode = (from p in contextWis.Registries
                                   where p.RegKey.Contains("DebitBankCharge")
                                   select p).ToList();

                switch (locCode)
                {
                    case "HKG":
                        drBankCharge = accountCode.Where(a => a.RegKey == "DebitBankCharge1").FirstOrDefault().RegValue;
                        break;
                    case "SGP":
                        drBankCharge = accountCode.Where(a => a.RegKey == "DebitBankCharge1").FirstOrDefault().RegValue;
                        break;
                    case "THA":
                        drBankCharge = accountCode.Where(a => a.RegKey == "DebitBankCharge1").FirstOrDefault().RegValue;
                        break;
                    case "VNM":
                        drBankCharge = accountCode.Where(a => a.RegKey == "DebitBankCharge1").FirstOrDefault().RegValue;
                        break;
                    case "MMR":
                        drBankCharge = accountCode.Where(a => a.RegKey == "DebitBankCharge1").FirstOrDefault().RegValue;
                        break;
                    case "VTN":
                        drBankCharge = accountCode.Where(a => a.RegKey == "DebitBankCharge1").FirstOrDefault().RegValue;
                        break;
                    default:
                        drBankCharge = accountCode.Where(a => a.RegKey == "DebitBankCharge1").FirstOrDefault().RegValue;
                        break;
                }

                return drBankCharge;
            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting Credit Amount For DMCom: " + e.Message + ":::" + e.InnerException);
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                return "";
            }


        }
        public string Dim2forInvoice(string locCode, ref List<string> validationMsg)
        {
            string dim2 = "";

            try
            {
                locCode = locCode != null || locCode != "" ? locCode.ToUpper() : locCode;

                switch (locCode)
                {
                    case "HKG":
                        dim2 = "0000";
                        break;
                    case "SGP":
                        dim2 = "";
                        break;
                    case "THA":
                        dim2 = "";
                        break;
                    case "VNM":
                        dim2 = "";
                        break;
                    case "MMR":
                        dim2 = "";
                        break;
                    case "VTN":
                        dim2 = "";
                        break;
                    default:
                        dim2 = "";
                        break;

                }
                return dim2;
            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting Dimension 2 for Invoice: " + e.Message + ":::" + e.InnerException);
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                return "";
            }


        }
        public string Dim2forFund(string locCode, ref List<string> validationMsg)
        {
            string dim2 = "";

            try
            {
                locCode = locCode != null || locCode != "" ? locCode.ToUpper() : locCode;

                switch (locCode)
                {
                    case "HKG":
                        dim2 = "0000";
                        break;
                    case "SGP":
                        dim2 = "";
                        break;
                    case "THA":
                        dim2 = "";
                        break;
                    case "VNM":
                        dim2 = "";
                        break;
                    case "MMR":
                        dim2 = "";
                        break;
                    case "VTN":
                        dim2 = "";
                        break;
                    default:
                        dim2 = "";
                        break;

                }
                return dim2;

            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting Dimension 2 for Invoice: " + e.Message + ":::" + e.InnerException);
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                return "";
            }


        }
        public string UserField1forFund(int? id, string businessType, string comment, string typeOfId, string locCode, ref List<string> validationMsg)
        {
            string userField1 = "";
            try
            {
                locCode = locCode != null || locCode != "" ? locCode.ToUpper() : locCode;

                switch (locCode)
                {
                    case "HKG":
                        userField1 = "";
                        break;
                    case "SGP":
                        userField1 = ChargeCodeForFund(comment, businessType, id, typeOfId, ref validationMsg);
                        break;
                    case "THA":
                        userField1 = ChargeCodeForFund(comment, businessType, id, typeOfId, ref validationMsg);
                        break;
                    case "VNM":
                        userField1 = ChargeCodeForFund(comment, businessType, id, typeOfId, ref validationMsg);
                        break;
                    case "VTN":
                        userField1 = ChargeCodeForFund(comment, businessType, id, typeOfId, ref validationMsg);
                        break;
                    case "MMR":
                        userField1 = ChargeCodeForFund(comment, businessType, id, typeOfId, ref validationMsg);
                        break;

                    default:
                        userField1 = ChargeCodeForFund(comment, businessType, id, typeOfId, ref validationMsg);
                        break;

                }

                if (userField1 != null && userField1 != "")
                {
                    userField1 = userField1.Length > 50 ? userField1.Substring(0, 50) : userField1;
                }

                return userField1;

            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting Credit Amount For DMCom: " + e.Message + ":::" + e.InnerException);
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                return "";
            }


        }

        public string Dim5forFund(int? id, string businessType, string comment, string typeOfId, string locCode, ref List<string> validationMsg)
        {
            string dim5 = "";
            try
            {
                locCode = locCode != null || locCode != "" ? locCode.ToUpper() : locCode;

                switch (locCode)
                {
                    case "HKG":
                        dim5 = ChargeCodeForFund(comment, businessType, id, typeOfId, ref validationMsg);
                        break;
                    case "SGP":
                        dim5 = "";
                        break;
                    case "THA":
                        dim5 = "";
                        break;
                    case "VNM":
                        dim5 = "";
                        break;
                    case "VTN":
                        dim5 = "";
                        break;
                    case "MMR":
                        dim5 = "";
                        break;

                    default:
                        dim5 = "";
                        break;

                }

                return dim5;


            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting Credit Amount For DMCom: " + e.Message + ":::" + e.InnerException);
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                return "";
            }


        }

        public string DMComLineDescription(daData result, string CompletedAt, ref List<string> validationMsg)
        {
            string LineDesc = "";
            try
            {
                var Arrival = result.arrivalDate;
                var Departure = result.departureDate;
                var NomFileNum = result.fileNumber;
                var Vessel = result.vesselName;
                var RegValue = (from v in contextWis.Registries
                                where v.RegKey == "a3DummyVesselName"
                                select v.RegValue).FirstOrDefault().ToString();
                string NomTypeSName = NominationType(result.nominationType, ref validationMsg);
                var regValueHoegh = (from v in contextWis.Registries
                                     where v.RegKey == "a3HoeghCompanyName"
                                     select v.RegValue).FirstOrDefault().ToString();
                var vesselController = result.portCallOperator;
                if (vesselController.ToUpper() == regValueHoegh.ToUpper() && result.principalNumber.ToUpper() == "H001-01")
                {
                    if (Departure != null)
                    {
                        var stringDDate = Departure;
                        DateTime newDDate = Convert.ToDateTime(stringDDate);

                        var stringADate = Arrival != null ? Arrival.ToString() : null;
                        DateTime newADate = Convert.ToDateTime(stringADate);

                        LineDesc = "SOA " + CompletedAt + " " + Vessel + " " + newADate.ToString("dd MMM") + " - " + newDDate.ToString("dd MMM yyyy") + " " + NomTypeSName;
                    }
                    else
                    {
                        var stringADate = Arrival != null ? Arrival.ToString() : null;
                        DateTime newADate = Convert.ToDateTime(stringADate);

                        LineDesc = "SOA " + CompletedAt + " " + Vessel + " " + newADate.ToString("dd MMM yyyy") + " " + NomTypeSName;
                    }
                }
                else if (vesselController.ToUpper() == regValueHoegh.ToUpper() && result.principalNumber.ToUpper() == "T054-01" && result.voyageCode != "0000")
                {
                    if (Departure != null)
                    {
                        var stringDDate = Departure.ToString();
                        DateTime newDDate = Convert.ToDateTime(stringDDate);

                        var stringADate = Arrival != null ? Arrival.ToString() : null;
                        DateTime newADate = Convert.ToDateTime(stringADate);

                        LineDesc = "SOA " + CompletedAt + " " + Vessel + " " + newADate.ToString("dd MMM") + " - " + newDDate.ToString("dd MMM yyyy") + " " + NomFileNum;
                    }
                    else
                    {
                        var stringADate = Arrival != null ? Arrival.ToString() : null;
                        DateTime newADate = Convert.ToDateTime(stringADate);

                        LineDesc = "SOA " + CompletedAt + " " + Vessel + " " + newADate.ToString("dd MMM yyyy") + " " + NomFileNum;
                    }
                }
                else if (vesselController.ToUpper() == regValueHoegh.ToUpper() && result.principalNumber.ToUpper() == "M064-01")
                {
                    if (Departure != null)
                    {
                        var stringDDate = Departure.ToString();
                        DateTime newDDate = Convert.ToDateTime(stringDDate);

                        var stringADate = Arrival != null ? Arrival.ToString() : null;
                        DateTime newADate = Convert.ToDateTime(stringADate);

                        LineDesc = "SOA " + CompletedAt + " " + Vessel + " " + newADate.ToString("dd MMM") + " - " + newDDate.ToString("dd MMM yyyy") + " " + NomFileNum;
                    }
                    else
                    {
                        var stringADate = Arrival != null ? Arrival.ToString() : null;
                        DateTime newADate = Convert.ToDateTime(stringADate);

                        LineDesc = "SOA " + CompletedAt + " " + Vessel + " " + newADate.ToString("dd MMM yyyy") + " " + NomFileNum;
                    }
                }
                else if (Vessel.ToUpper() == RegValue.ToUpper() && result.principalNumber.ToUpper() == "H001-01")
                {
                    LineDesc = "SOA " + CompletedAt + " " + NomFileNum + " General A/C";
                }
                else if (Vessel.ToUpper() == RegValue.ToUpper())
                {
                    LineDesc = "SOA " + CompletedAt + " " + NomFileNum;
                }
                else if (Vessel.ToUpper() != RegValue.ToUpper())
                {
                    if (Departure != null)
                    {
                        var stringDDate = Departure.ToString();
                        DateTime newDDate = Convert.ToDateTime(stringDDate);

                        var stringADate = Arrival != null ? Arrival.ToString() : null;
                        DateTime newADate = Convert.ToDateTime(stringADate);

                        LineDesc = "SOA " + CompletedAt + " " + Vessel + " " + newADate.ToString("dd MMM") + " - " + newDDate.ToString("dd MMM yyyy") + " " + NomTypeSName;
                    }
                    else
                    {
                        var stringADate = Arrival != null ? Arrival.ToString() : null;
                        DateTime newADate = Convert.ToDateTime(stringADate);

                        LineDesc = "SOA " + CompletedAt + " " + Vessel + " " + newADate.ToString("dd MMM yyyy") + " " + NomTypeSName;
                    }
                }
                else
                {
                    throw new Exception("0072:: Invalid DMCom Line Description.");
                }
                return LineDesc;
            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting DMCom Line Description: " + e.Message + ":::" + e.InnerException);
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                return LineDesc;
            }

        }
        public bool IsGenDNPerSer(string locCode, ref List<string> validationMsg)
        {
            LogManager.Log.Info("Validate the IsGenDNPerSer...");
            try
            {
                var isGenDN = (from v in contextWis.a3SMCMappings
                               where v.a3LocCode == locCode
                               select v.isGenDNPerSer).ToList();

                if (isGenDN == null || isGenDN.Count() == 0)
                {
                    return false;
                    throw new Exception("0074:: Record for IsGenDNPerSer is not found.");
                }
                else
                {
                    return isGenDN.FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting IsGenDNPerSer" + e.Message + ":::" + e.InnerException);
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                return false;
            }
        }

        public string CreditAccountTypeForDebitNote(string a3LocCode, string postingCode, string code1, string code2, ref List<string> validationMsg)
        {
            LogManager.Log.Info("Validate the Credit Account Type for debit note...");
            string retdac = null;
            try
            {
                var postingCodeVal = (from p in contextWis.Registries
                                      where p.RegKey.Contains("PostingCode")
                                      select p).ToList();

                switch (a3LocCode)
                {
                    case "HKG":
                        if (postingCode != string.Empty && postingCode != null)
                        {
                            if (postingCode == postingCodeVal.Where(p => p.RegKey == "PostingCode2").FirstOrDefault().RegValue)
                            {
                                retdac = "Bank";
                            }
                            else
                            {
                                retdac = "Ledger";
                            }
                        }
                        else if ((code1 != string.Empty && code1 != null) && (code2 != string.Empty && code2 != null))
                        {
                            if (postingCode == postingCodeVal.Where(p => p.RegKey == "PostingCode2").FirstOrDefault().RegValue)
                            {
                                retdac = "Bank";
                            }
                            else
                            {
                                retdac = "Ledger";
                            }
                        }
                        else
                        {
                            LogManager.Log.Info("Credit Account Type For Debit Note not found.");
                            throw new Exception("0077:: Credit Account Type For Debit Note not found.");
                        }

                        break;

                    case "SGP":
                        if (postingCode != string.Empty && postingCode != null)
                        {
                            if (postingCode == postingCodeVal.Where(p => p.RegKey == "PostingCode2").FirstOrDefault().RegValue ||
                                postingCode == postingCodeVal.Where(p => p.RegKey == "PostingCode3").FirstOrDefault().RegValue ||
                                postingCode == postingCodeVal.Where(p => p.RegKey == "PostingCode4").FirstOrDefault().RegValue ||
                                postingCode == postingCodeVal.Where(p => p.RegKey == "PostingCode5").FirstOrDefault().RegValue)
                            {
                                retdac = "Bank";
                            }
                            else
                            {
                                retdac = "Ledger";
                            }
                        }
                        else if ((code1 != string.Empty && code1 != null) && (code2 != string.Empty && code2 != null))
                        {
                            if (postingCode == postingCodeVal.Where(p => p.RegKey == "PostingCode2").FirstOrDefault().RegValue ||
                                postingCode == postingCodeVal.Where(p => p.RegKey == "PostingCode3").FirstOrDefault().RegValue ||
                                postingCode == postingCodeVal.Where(p => p.RegKey == "PostingCode4").FirstOrDefault().RegValue ||
                                postingCode == postingCodeVal.Where(p => p.RegKey == "PostingCode5").FirstOrDefault().RegValue)
                            {
                                retdac = "Bank";
                            }
                            else
                            {
                                retdac = "Ledger";
                            }
                        }
                        else
                        {
                            LogManager.Log.Info("Credit Account Type For Debit Note not found.");
                            throw new Exception("0077:: Credit Account Type For Debit Note not found.");
                        }

                        break;
                    case "THA":
                        if (postingCode != string.Empty && postingCode != null)
                        {
                            if (postingCode == "2004281")
                            {
                                retdac = "Bank";
                            }
                            else
                            {
                                retdac = "Ledger";
                            }
                        }
                        else if ((code1 != string.Empty && code1 != null) && (code2 != string.Empty && code2 != null))
                        {
                            if (postingCode == "2004281")
                            {
                                retdac = "Bank";
                            }
                            else
                            {
                                retdac = "Ledger";
                            }
                        }
                        else
                        {
                            LogManager.Log.Info("Credit Account Type For Debit Note not found.");
                            throw new Exception("0077:: Credit Account Type For Debit Note not found.");
                        }

                        break;
                    case "VNM":
                        if (postingCode != string.Empty && postingCode != null)
                        {
                            if (postingCode == "2004164")
                            {
                                retdac = "Bank";
                            }
                            else
                            {
                                retdac = "Ledger";
                            }
                        }
                        else if ((code1 != string.Empty && code1 != null) && (code2 != string.Empty && code2 != null))
                        {
                            if (postingCode == "2004164")
                            {
                                retdac = "Bank";
                            }
                            else
                            {
                                retdac = "Ledger";
                            }
                        }
                        else
                        {
                            LogManager.Log.Info("Credit Account Type For Debit Note not found.");
                            throw new Exception("0077:: Credit Account Type For Debit Note not found.");
                        }

                        break;
                    case "MMR":
                        if (postingCode != string.Empty && postingCode != null)
                        {
                            if (postingCode == "2004281")
                            {
                                retdac = "Bank";
                            }
                            else
                            {
                                retdac = "Ledger";
                            }
                        }
                        else if ((code1 != string.Empty && code1 != null) && (code2 != string.Empty && code2 != null))
                        {
                            if (postingCode == "2004281")
                            {
                                retdac = "Bank";
                            }
                            else
                            {
                                retdac = "Ledger";
                            }
                        }
                        else
                        {
                            LogManager.Log.Info("Credit Account Type For Debit Note not found.");
                            throw new Exception("0077:: Credit Account Type For Debit Note not found.");
                        }

                        break;
                    case "JPN":
                        if (postingCode != string.Empty && postingCode != null)
                        {
                            if (postingCode == "2004070" || postingCode == "2004071")
                            {
                                retdac = "Bank";
                            }
                            else
                            {
                                retdac = "Ledger";
                            }
                        }
                        else if ((code1 != string.Empty && code1 != null) && (code2 != string.Empty && code2 != null))
                        {
                            if (postingCode == "2004070" || postingCode == "2004071")
                            {
                                retdac = "Bank";
                            }
                            else
                            {
                                retdac = "Ledger";
                            }
                        }
                        else
                        {
                            LogManager.Log.Info("Credit Account Type For Debit Note not found.");
                            throw new Exception("0077:: Credit Account Type For Debit Note not found.");
                        }

                        break;
                    case "VTN":
                        if (postingCode != string.Empty && postingCode != null)
                        {
                            if (postingCode == "2004345" || postingCode == "2004346")
                            {
                                retdac = "Bank";
                            }
                            else
                            {
                                retdac = "Ledger";
                            }
                        }
                        else if ((code1 != string.Empty && code1 != null) && (code2 != string.Empty && code2 != null))
                        {
                            if (postingCode == "2004345" || postingCode == "2004346")
                            {
                                retdac = "Bank";
                            }
                            else
                            {
                                retdac = "Ledger";
                            }
                        }
                        else
                        {
                            LogManager.Log.Info("Credit Account Type For Debit Note not found.");
                            throw new Exception("0077:: Credit Account Type For Debit Note not found.");
                        }

                        break;
                    default:
                        break;
                }
                return retdac;
            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting Credit Account Type For debit note: " + e.Message + ":::" + e.InnerException);
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                return retdac;
            }

        }

        public string VendorCode(string vendorCode, ref List<string> validationMsg)
        {
            LogManager.Log.Info("Validate the Vendor Code...");
            try
            {
                if (vendorCode == null || vendorCode == "")
                {
                    throw new Exception("0036:: Missing Vendor Code.");
                }

                return vendorCode;
            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting Vendor Code: " + e.Message + ":::" + e.InnerException);
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                return vendorCode;
            }

        }

        public DateTime? DateReceived(DateTime? dateReceived, ref List<string> validationMsg)
        {
            LogManager.Log.Info("Validate the Date Received...");
            try
            {
                if (dateReceived == null)
                {
                    throw new Exception("0003:: Date Received is null.");
                }

                return dateReceived;
            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting Date Received: " + e.Message + ":::" + e.InnerException);
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                return dateReceived;
            }

        }

        public DateTime? DateIssued(DateTime? dateIssued, ref List<string> validationMsg)
        {
            LogManager.Log.Info("Validate the Date Issued...");
            try
            {
                if (dateIssued == null)
                {
                    throw new Exception("0044:: Date Issued is null.");
                }

                return dateIssued;
            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting Date Issued: " + e.Message + ":::" + e.InnerException);
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                return dateIssued;
            }

        }


        public string CreditMiscAccountForDNtMiscAR(string businessType, ref List<string> validationMsg)
        {
            LogManager.Log.Info("Validate the Credit Misc Account For Debit Note Misc AR...");
            string creditMisc = "";
            try
            {

                if (businessType.ToUpper() == "AUTOLINER")
                {
                    creditMisc = "6210301";
                }
                else if (businessType.ToUpper() == "CRUISE")
                {
                    creditMisc = "6230901";
                }
                else if (businessType.ToUpper() == "LINER")
                {
                    creditMisc = "6200701";
                }
                else if (businessType.ToUpper() == "TRAMP")
                {
                    creditMisc = "6220901";
                }
                else if (businessType == string.Empty || businessType == null)
                {
                    throw new Exception("0013:: Missing Business Type.");
                }
                else
                {
                    creditMisc = "6270001";
                }

                return creditMisc;
            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting Credit Misc Account For Debit Note Misc AR: " + e.Message + ":::" + e.InnerException);
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                return creditMisc;
            }

        }


        public string Dim3AsReferral(string principalNumber, string nominationFileNumber, string referral, ref List<string> validationMsg)
        {
            LogManager.Log.Info("Validate the Dim3 as Referral...");
            string dim3AsReferral = "";
            try
            {
                if (referral != null && referral != "")
                {
                    dim3AsReferral = referral;
                    return dim3AsReferral;
                }
                else
                {
                    var accountNo = (from h in contextgswallem.Hotlists
                                     join p in contextgswallem.PortCalls on h.PORTCALL_ID equals p.ID
                                     join ct in contextgswallem.Client_Types on h.CLIENT_ROLE_ID equals ct.ID
                                     join c in contextgswallem.Clients on h.CLIENT_ID equals c.ID
                                     where p.PORTCALL_NUMBER == nominationFileNumber && ct.NAME == "Principal"
                                     select new { accountNo = c.ACCOUNT_NO }).OrderByDescending(x => x.accountNo).ToList();

                    if (principalNumber == "Z9999" || principalNumber == "C084-01" || principalNumber == "T054-01" || principalNumber == "M064-01")
                    {
                        throw new Exception("0094:: Principal number is either Z9999, C084-01, T054-01 or M064-01.");
                    }
                    else
                    {
                        dim3AsReferral = principalNumber;
                        return dim3AsReferral;
                    }
                }
            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting Dim3 as Referral: " + e.Message + ":::" + e.InnerException);
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                return dim3AsReferral;
            }

        }

        public string VoyageCode(int id, int? portCallId, string voyageCode, string vesselName, DateTime? etaDate, string harbourLocCode, ref List<string> validationMsg)
        {
            LogManager.Log.Info("Validate the Voyage Code...");
            try
            {
                var portCallNo = (from e in contextgswallem.Expenses
                                  join pc in contextgswallem.PortCalls on e.PORTCALL_ID equals pc.ID
                                  where e.ID == id
                                  select pc.PORTCALL_NUMBER).FirstOrDefault();

                var portCall = (from p in contextWis.VoyageCodeControls
                                where p.PortCall_ID == portCallId
                                select p).ToList();

                if (portCall.Count > 0)
                {
                    voyageCode = portCall.FirstOrDefault().FinanceVoyageCode;
                }
                else
                {
                    vesselName = vesselName.Length > 5 ? vesselName.Substring(0, 5) : vesselName.Trim();
                    voyageCode = vesselName + etaDate.Value.ToString("yyMMddhh") + harbourLocCode;

                    VoyageCodeControl voyageCodeControl = new VoyageCodeControl();
                    voyageCodeControl.PortCall_ID = portCallId ?? 0;
                    voyageCodeControl.PortCall_Number = portCallNo;
                    voyageCodeControl.FinanceVoyageCode = voyageCode;
                    contextWis.VoyageCodeControls.Add(voyageCodeControl);
                }

                return voyageCode;
            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting Voyage Code: " + e.Message + ":::" + e.InnerException);
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                return voyageCode;
            }

        }

        public string Dim3ForFund(string principalNumber, string category, ref List<string> validationMsg)
        {
            LogManager.Log.Info("Validate the Dim3 for Fund...");
            string dim3 = "";
            try
            {
                if ((category == "FINAL" || category == "ADVANCE") && principalNumber == "Z9999")
                {
                    dim3 = "0000";
                    return dim3;
                }
                else
                {
                    dim3 = principalNumber;
                    return dim3;
                }
            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting Dim3 for Fund: " + e.Message + ":::" + e.InnerException);
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                return dim3;
            }
        }

        public string Dim3ForDMCom(string principalNumber, ref List<string> validationMsg)
        {
            LogManager.Log.Info("Validate the Dim3 for DMCom...");
            string dim3 = "";
            try
            {
                if (principalNumber == "Z9999")
                {
                    dim3 = "0000";
                    return dim3;
                }
                else
                {
                    dim3 = principalNumber;
                    return dim3;
                }
            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting Dim3 for DMCom: " + e.Message + ":::" + e.InnerException);
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                return dim3;
            }

        }

        public bool HasDynamicFieldInExpTemplate(string integrationCode, int? refNumber, ref List<string> validationMsg)
        {
            try
            {
                var rowCount = (from e in contextgswallem.Expenses
                                join expTDy in contextgswallem.expense_template_dyna on e.EXPENSE_TEMPLATE_ID equals expTDy.ETD_ELT_ID
                                join df in contextgswallem.dyna_fields on expTDy.ETD_FIELD_ID equals df.ID
                                where df.INTEGRATION_REFERENCE == integrationCode && e.REF_NUMBER == refNumber
                                select expTDy.ETD_ELT_ID).ToList();

                if (rowCount.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting HasDynamicFieldInExpTemplate: " + e.Message + ":::" + e.InnerException);
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                return false;
            }
        }

        public string DebitAccountTypeForFCAdv(string a3LocCode, ref List<string> validationMsg)
        {
            LogManager.Log.Info("Validate the Debit Account Type for FCAdv...");
            string retdac = null;
            try
            {
                switch (a3LocCode)
                {
                    case "HKG":
                        retdac = "Bank";
                        break;

                    case "SGP":
                        retdac = "Bank";
                        break;
                    case "THA":
                        retdac = "Bank";
                        break;
                    case "VNM":
                        retdac = "Bank";
                        break;
                    case "VTN":
                        retdac = "Bank";
                        break;
                    case "MMR":
                        retdac = "Ledger";
                        break;
                    default:
                        retdac = "Bank";
                        break;
                }
                return retdac;
            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting Debit Account Type for FCAdv: " + e.Message + ":::" + e.InnerException);
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                return retdac;
            }

        }

        public void ValidateTansAndDocumentDate(string entity, DateTime? transDate, DateTime? documentDate, ref List<string> validationMsg)
        {
            try
            {
                LogManager.Log.Info("Validate transDate (" + transDate + ") and documentDate (" + documentDate + ")");

                DateTime? dateReceived = documentDate;
                if (entity == "FUND")
                {
                    dateReceived = DateReceived(documentDate, ref validationMsg);
                }

                if (dateReceived != null)
                {
                    if (transDate.Value.Date < dateReceived.Value.Date)
                    {
                        throw new Exception("0095:: Transaction Date is earlier that the DocumentDate.");
                    }
                }
            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in validating trans and document date: " + e.Message + ":::" + e.InnerException);
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
            }

        }

        public DateTime? PostingDate(DateTime? postingDate, string locCode, ref List<string> validationMsg)
        {
            DateTime? postingDateLog = postingDate == null ? DateTime.Parse("1900-01-01") : postingDate;
            if (postingDateLog == DateTime.Parse("1900-01-01"))
            {
                LogManager.Log.Info("Posting Date is null.");
            }
            LogManager.Log.Info("Validate the Posting Date..." + postingDateLog.ToString());
            DateTime? retdac = postingDate;
            try
            {
                var postingDateRange = (from u in contextWis.Registries
                                        where u.RegKey == "PostingDateRange_" + locCode
                                        select u).FirstOrDefault();
                if (postingDateRange.RegValue == "")
                {
                    return retdac;
                }
                else
                {
                    DateTime? pd = DateTime.Parse(retdac.Value.Date.ToString("dd-MMM-yyyy"));
                    DateTime from = DateTime.Parse(postingDateRange.RegValue.Split(new string[] { "to" }, StringSplitOptions.RemoveEmptyEntries)[0]);
                    DateTime to = DateTime.Parse(postingDateRange.RegValue.Split(new string[] { "to" }, StringSplitOptions.RemoveEmptyEntries)[1]);

                    if (pd >= from && pd <= to)
                    {
                        return retdac;
                    }
                    else
                    {
                        throw new Exception("0098:: Posting Date must be within the posting date range in the Registry..");
                    }
                }
            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting Posting Date: " + e.Message + ":::" + e.InnerException);
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                return retdac;
            }

        }

        public string CreditLogisticAccountForDNtMiscAR(string businessType, ref List<string> validationMsg)
        {
            LogManager.Log.Info("Validate the Credit Logistic Account For Debit Note Misc AR...");
            string crediLogistic = "";
            try
            {
                var accountCode = (from a in contextWis.Registries
                                   where a.RegKey.Contains("CrLogisticAcctDNtMisc_AR")
                                   select a).ToList();

                if (businessType.ToUpper() == "AUTOLINER")
                {
                    crediLogistic = accountCode.Where(a => a.RegKey == "CrLogisticAcctDNtMisc_AR1").FirstOrDefault().RegValue;
                }
                else if (businessType.ToUpper() == "CRUISE")
                {
                    crediLogistic = accountCode.Where(a => a.RegKey == "CrLogisticAcctDNtMisc_AR2").FirstOrDefault().RegValue;
                }
                else if (businessType.ToUpper() == "LINER")
                {
                    crediLogistic = accountCode.Where(a => a.RegKey == "CrLogisticAcctDNtMisc_AR3").FirstOrDefault().RegValue;
                }
                else if (businessType.ToUpper() == "TRAMP")
                {
                    crediLogistic = accountCode.Where(a => a.RegKey == "CrLogisticAcctDNtMisc_AR4").FirstOrDefault().RegValue;
                }
                else if (businessType == string.Empty || businessType == null)
                {
                    throw new Exception("0013:: Missing Business Type.");
                }
                else
                {
                    crediLogistic = accountCode.Where(a => a.RegKey == "CrLogisticAcctDNtMisc_AR5").FirstOrDefault().RegValue;
                }

                return crediLogistic;
            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting Credit Logistic Account For Debit Note Misc AR: " + e.Message + ":::" + e.InnerException);
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                return crediLogistic;
            }

        }

        public string CreditDocAccountForDNtMiscAR(string businessType, ref List<string> validationMsg)
        {
            LogManager.Log.Info("Validate the Credit Doc Account For Debit Note Misc AR...");
            string creditDoc = "";
            try
            {
                var accountCode = (from a in contextWis.Registries
                                   where a.RegKey.Contains("CrDocAcctDNtMisc_AR")
                                   select a).ToList();

                if (businessType.ToUpper() == "AUTOLINER")
                {
                    creditDoc = accountCode.Where(a => a.RegKey == "CrDocAcctDNtMisc_AR1").FirstOrDefault().RegValue;
                }
                else if (businessType.ToUpper() == "CRUISE")
                {
                    creditDoc = accountCode.Where(a => a.RegKey == "CrDocAcctDNtMisc_AR2").FirstOrDefault().RegValue;
                }
                else if (businessType.ToUpper() == "LINER")
                {
                    creditDoc = accountCode.Where(a => a.RegKey == "CrDocAcctDNtMisc_AR3").FirstOrDefault().RegValue;
                }
                else if (businessType.ToUpper() == "TRAMP")
                {
                    creditDoc = accountCode.Where(a => a.RegKey == "CrDocAcctDNtMisc_AR4").FirstOrDefault().RegValue;
                }
                else if (businessType == string.Empty || businessType == null)
                {
                    throw new Exception("0013:: Missing Business Type.");
                }
                else
                {
                    creditDoc = accountCode.Where(a => a.RegKey == "CrDocAcctDNtMisc_AR5").FirstOrDefault().RegValue;
                }

                return creditDoc;
            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting Credit Doc Account For Debit Note Misc AR: " + e.Message + ":::" + e.InnerException);
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                return creditDoc;
            }

        }

        public string CreditLandTransportationAccountForDNtMiscAR(string businessType, ref List<string> validationMsg)
        {
            LogManager.Log.Info("Validate the Credit Land Transportation Account For Debit Note Misc AR...");
            string creditLandTranspo = "";
            try
            {
                var accountCode = (from a in contextWis.Registries
                                   where a.RegKey.Contains("CrLandTransAcctDNtMisc_AR")
                                   select a).ToList();

                if (businessType.ToUpper() == "AUTOLINER")
                {
                    creditLandTranspo = accountCode.Where(a => a.RegKey == "CrLandTransAcctDNtMisc_AR1").FirstOrDefault().RegValue;
                }
                else if (businessType.ToUpper() == "CRUISE")
                {
                    creditLandTranspo = accountCode.Where(a => a.RegKey == "CrLandTransAcctDNtMisc_AR2").FirstOrDefault().RegValue;
                }
                else if (businessType.ToUpper() == "LINER")
                {
                    creditLandTranspo = accountCode.Where(a => a.RegKey == "CrLandTransAcctDNtMisc_AR3").FirstOrDefault().RegValue;
                }
                else if (businessType.ToUpper() == "TRAMP")
                {
                    creditLandTranspo = accountCode.Where(a => a.RegKey == "CrLandTransAcctDNtMisc_AR4").FirstOrDefault().RegValue;
                }
                else if (businessType == string.Empty || businessType == null)
                {
                    throw new Exception("0013:: Missing Business Type.");
                }
                else
                {
                    creditLandTranspo = accountCode.Where(a => a.RegKey == "CrLandTransAcctDNtMisc_AR5").FirstOrDefault().RegValue;
                }

                return creditLandTranspo;
            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting Credit Land Transportation Account For Debit Note Misc AR: " + e.Message + ":::" + e.InnerException);
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                return creditLandTranspo;
            }

        }

        public string CreditSeaTransportationAccountForDNtMiscAR(string businessType, ref List<string> validationMsg)
        {
            LogManager.Log.Info("Validate the Credit Sea Transportation Account For Debit Note Misc AR...");
            string creditSeaTranspo = "";
            try
            {
                var accountCode = (from a in contextWis.Registries
                                   where a.RegKey.Contains("CrSeaTransAcctDNtMisc_AR")
                                   select a).ToList();

                if (businessType.ToUpper() == "AUTOLINER")
                {
                    creditSeaTranspo = accountCode.Where(a => a.RegKey == "CrSeaTransAcctDNtMisc_AR1").FirstOrDefault().RegValue;
                }
                else if (businessType.ToUpper() == "CRUISE")
                {
                    creditSeaTranspo = accountCode.Where(a => a.RegKey == "CrSeaTransAcctDNtMisc_AR2").FirstOrDefault().RegValue;
                }
                else if (businessType.ToUpper() == "LINER")
                {
                    creditSeaTranspo = accountCode.Where(a => a.RegKey == "CrSeaTransAcctDNtMisc_AR3").FirstOrDefault().RegValue;
                }
                else if (businessType.ToUpper() == "TRAMP")
                {
                    creditSeaTranspo = accountCode.Where(a => a.RegKey == "CrSeaTransAcctDNtMisc_AR4").FirstOrDefault().RegValue;
                }
                else if (businessType == string.Empty || businessType == null)
                {
                    throw new Exception("0013:: Missing Business Type.");
                }
                else
                {
                    creditSeaTranspo = accountCode.Where(a => a.RegKey == "CrSeaTransAcctDNtMisc_AR5").FirstOrDefault().RegValue;
                }

                return creditSeaTranspo;
            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting Credit Sea Transportation Account For Debit Note Misc AR: " + e.Message + ":::" + e.InnerException);
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                return creditSeaTranspo;
            }

        }

        public string AddSuffix(string invoiceNo, string vendorCode, ref bool tmpIsSuffixed, ref List<string> validationMsg)
        {
            LogManager.Log.Info("Validate the Suffix...");
            string retInvoiceNumberWithSuffix = null;
            string tmpKey = invoiceNo;
            if (tmpKey != null)
            {
                tmpKey = (tmpKey.Length > 47 ? tmpKey.Substring(0, 47) : tmpKey);
            }

            try
            {
                var suffixNo = (from i in contextWis.a3InvoiceSuffixes
                                where i.VendorCode == vendorCode && i.InvoiceNo == tmpKey
                                select i.SuffixNo).ToList();

                LogManager.Log.Info("suffixNo count: " + suffixNo.Count());

                if (suffixNo == null || suffixNo.Count() == 0)
                {
                    retInvoiceNumberWithSuffix = tmpKey;
                    tmpIsSuffixed = false;
                }
                else
                {
                    retInvoiceNumberWithSuffix = tmpKey + "(" + (suffixNo.FirstOrDefault() + 1) + ")";
                    tmpIsSuffixed = true;
                }

                LogManager.Log.Info("AddSuffix value: " + retInvoiceNumberWithSuffix);
                return retInvoiceNumberWithSuffix;
            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting Suffix: " + e.Message + ":::" + e.InnerException);
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                return retInvoiceNumberWithSuffix;
            }

        }

        public string InvoiceForFC(string a3LocCode, string voyageCode, string typeOfReceipt, string bankReference, ref List<string> validationMsg)
        {
            LogManager.Log.Info("Validate the Invoice For FCBkChg ...");
            string retdac = null;
            try
            {
                switch (a3LocCode)
                {
                    case "HKG":
                        retdac = (voyageCode + " " + typeOfReceipt).Length > 50 ? (voyageCode + " " + typeOfReceipt).Substring(0, 50) : voyageCode + " " + typeOfReceipt;
                        break;

                    case "SGP":
                        retdac = (bankReference).Length > 50 ? (bankReference).Substring(0, 50) : bankReference;
                        break;

                    default:
                        retdac = (voyageCode + " " + typeOfReceipt).Length > 50 ? (voyageCode + " " + typeOfReceipt).Substring(0, 50) : voyageCode + " " + typeOfReceipt;
                        break;
                }
                return retdac;
            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting Invoice For FCBkChg: " + e.Message + ":::" + e.InnerException);
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                return retdac;
            }

        }


        public string HarbourLocCode(int id, string locCode, string entity, ref List<string> validationMsg)
        {
            string harbourLocCode = "";
            try
            {
                locCode = locCode != null || locCode != "" ? locCode.ToUpper() : locCode;
                if (locCode != "JPN")
                {
                    LogManager.Log.Info("Entity: " + entity);
                    var dim = (from e in contextgswallem.Setups
                               join d in contextgswallem.PortCalls on e.ID equals d.SETUP_ID
                               join q in contextgswallem.Expenses on d.ID equals q.PORTCALL_ID
                               where e.ID == id
                               select e.EXT_CODE_1).ToList();
                    LogManager.Log.Info("Dim list count for " + entity + ": " + dim.Count());

                    if (entity == "DA")
                    {
                        dim = (from e in contextgswallem.Setups
                               join d in contextgswallem.PortCalls on e.ID equals d.SETUP_ID
                               join q in contextgswallem.Expenses on d.ID equals q.PORTCALL_ID
                               join da in contextgswallem.debitcreditnotes on q.INVOICE_ID equals da.DCN_ID
                               where da.DCN_ID == id
                               select e.EXT_CODE_1).ToList();
                        LogManager.Log.Info("Dim list count for " + entity + ": " + dim.Count());
                    }


                    if (dim.Count() > 0 && dim != null)
                    {
                        harbourLocCode = dim.FirstOrDefault();
                    }
                    else
                    {
                        harbourLocCode = "";
                    }
                }
                else
                {
                    LogManager.Log.Info("Entity: " + entity);
                    var dim = (from e in contextgswallem.Expenses
                               join d in contextgswallem.Docks on e.PORTCALL_ID equals d.PORTCALL_ID
                               join q in contextgswallem.Quays on d.QUAY_ID equals q.ID
                               join h in contextgswallem.Harbours on q.HARBOUR_ID equals h.ID
                               where e.ID == id
                               orderby d.ATD, d.ATA, d.ETB, d.SORT_ORDER descending
                               select new { h.LOCODE, harbourName = h.NAME, quarryName = q.NAME, d.ETB, d.ATD, d.ETD, d.ATA }).ToList();
                    LogManager.Log.Info("Dim list count for " + entity + ": " + dim.Count());


                    if (entity.ToUpper() == "DA")
                    {
                        dim = (from e in contextgswallem.Expenses
                               join d in contextgswallem.Docks on e.PORTCALL_ID equals d.PORTCALL_ID
                               join q in contextgswallem.Quays on d.QUAY_ID equals q.ID
                               join h in contextgswallem.Harbours on q.HARBOUR_ID equals h.ID
                               join dc in contextgswallem.debitcreditnotes on e.INVOICE_ID equals dc.DCN_ID
                               where dc.DCN_ID == id
                               orderby d.ATD, d.ATA, d.ETB, d.SORT_ORDER descending
                               select new { h.LOCODE, harbourName = h.NAME, quarryName = q.NAME, d.ETB, d.ATD, d.ETD, d.ATA }).ToList();
                        LogManager.Log.Info("Dim list count for " + entity + ": " + dim.Count());
                    }

                    if (dim.Count() > 0 && dim != null)
                    {
                        harbourLocCode = dim.FirstOrDefault().LOCODE;
                    }
                    else
                    {
                        harbourLocCode = "";
                    }
                }

                if (harbourLocCode == "")
                {
                    switch (locCode)
                    {
                        case "HKG":
                            harbourLocCode = "HKHKG";
                            break;
                        case "SGP":
                            harbourLocCode = "SGSIN";
                            break;
                        case "THA":
                            harbourLocCode = "";
                            break;
                        case "VNM":
                            harbourLocCode = "";
                            break;
                        case "VTN":
                            harbourLocCode = "";
                            break;
                        case "MMR":
                            harbourLocCode = "";
                            break;
                        case "JPN":
                            harbourLocCode = "";
                            break;

                        default:
                            harbourLocCode = "";
                            break;
                    }
                }

                return harbourLocCode;
            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in getting Harbour Location Code: " + e.Message + ":::" + e.InnerException);
                validationMsg = a3Helper.AppendError(e.Message + "<br>");
                return "";
            }
        }

        public string CreditAccountForFCExGL()
        {
            var accountCode = (from a in contextWis.Registries
                               where a.RegKey == "CrAccountFCExGL"
                               select a.RegValue).FirstOrDefault();

            return accountCode;            
        }

        public string DebitAccountForInvoiceDNHoegh()
        {
            var accountCode = (from a in contextWis.Registries
                               where a.RegKey == "DrAccountInvoiceDNHoegh"
                               select a.RegValue).FirstOrDefault();

            return accountCode;
        }

        public string CreditAccountForInvoiceDNJP()
        {
            var accountCode = (from a in contextWis.Registries
                               where a.RegKey == "CrAccountInvoiceDNJP"
                               select a.RegValue).FirstOrDefault();

            return accountCode;
        }

        public string CreditAccountBankChargeInvoiceDN()
        {
            var accountCode = (from a in contextWis.Registries
                               where a.RegKey == "CrAccountBankChargeInvoiceDN"
                               select a.RegValue).FirstOrDefault();

            return accountCode;
        }

        public string CreditAccountPacoIncomeInvoiceDN()
        {
            var accountCode = (from a in contextWis.Registries
                               where a.RegKey == "CrAccountPacoIncomeInvoiceDN"
                               select a.RegValue).FirstOrDefault();

            return accountCode;
        }

        public string CreditAccountCommIncomeInvoiceDN()
        {
            var accountCode = (from a in contextWis.Registries
                               where a.RegKey == "CrAccountCommIncomeInvoiceDN"
                               select a.RegValue).FirstOrDefault();

            return accountCode;
        }

        public string DebitAccountInvoiceDNTH()
        {
            var accountCode = (from a in contextWis.Registries
                               where a.RegKey == "DrAccountInvoiceDNTH"
                               select a.RegValue).FirstOrDefault();

            return accountCode;
        }

        public string CreditAccountInvoiceDN()
        {
            var accountCode = (from a in contextWis.Registries
                               where a.RegKey == "CrAccountInvoiceDN"
                               select a.RegValue).FirstOrDefault();

            return accountCode;
        }

        public string DebitClearingInvoiceDN_AR()
        {
            var accountCode = (from a in contextWis.Registries
                               where a.RegKey == "ClearingAccountCode1"
                               select a.RegValue).FirstOrDefault();

            return accountCode;
        }

        public string CreditBankChargeInvoiceDN_AR()
        {
            var accountCode = (from a in contextWis.Registries
                               where a.RegKey == "ClearingAccountCode1"
                               select a.RegValue).FirstOrDefault();

            return accountCode;
        }

        public string CreditAccountInvoiceFPE_CNCO()
        {
            var accountCode = (from a in contextWis.Registries
                               where a.RegKey == "CrDocAcctDNtMisc_AR3"
                               select a.RegValue).FirstOrDefault();

            return accountCode;
        }

        public string CreditAccountInvoiceFPE_CNCO2()
        {
            var accountCode = (from a in contextWis.Registries
                               where a.RegKey == "CrAccountInvoiceCNCO"
                               select a.RegValue).FirstOrDefault();

            return accountCode;
        }

        public string CreditAccountInvoiceFPE_CNCO3()
        {
            var accountCode = (from a in contextWis.Registries
                               where a.RegKey == "DrAccountInvoiceDNHoegh"
                               select a.RegValue).FirstOrDefault();

            return accountCode;
        }

        public string CreditAccountInvoiceTramp_C()
        {
            var accountCode = (from a in contextWis.Registries
                               where a.RegKey == "CrDocAcctDNtMisc_AR1"
                               select a.RegValue).FirstOrDefault();

            return accountCode;
        }

        public string CreditAccountTrampInvoiceTramp_C()
        {
            var accountCode = (from a in contextWis.Registries
                               where a.RegKey == "CrAccountTrampInvoiceTramp_C"
                               select a.RegValue).FirstOrDefault();

            return accountCode;
        }

    }
}
