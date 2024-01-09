using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GATShipD365TS.App_Code;
using GATShipD365TS.Helper;
using GATShipD365TS.Models;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.IO;

namespace GATShipD365TS.GATShip.Entities
{
    public class FundsFinal
    {
        A3Helper a3Helper = new A3Helper();
        EntityHelper funds = new EntityHelper();
        JsonResponse response;
        bool success = false;
        string entityType = "NOMID";
        const char doubleqoute = '"';
        const char singlequote = '\'';
        string action = "";
        string entity = "";
        int id = 0;
        string warningMsg = "";
        string errorMsg = "";
        public bool Create(int eventId, int a3_id, FundsPayload result, string jsonData, DateTime? received_at, string actionEntity, ref List<string> validationMsg)
        {
            LogManager.Log.Info("Start processing Event Id: " + eventId);
            try
            {
                bool tmpIsSuffixed = false;
                string fileLocation = "";
                string comment = result.data.comment;
                int? nominationId = result.data.nominationId;
                action = result.action.ToUpper();
                entity = result.entity;
                id = a3_id;
                string category = result.data.category;
                category = category == null || category == "" ? "ADDITIONAL" : category.ToUpper();
                StringBuilder msgSb = new StringBuilder();
                string initials = result.data.initials + "@wallem.com";

                LogManager.Log.Info("Validating fields...");


                #region VALIDATIONS      
                string nominationFileNumber = result.data.fileNumber;

                string typeOfReceiptForFund = funds.TypeOfReceiptForFund(comment, ref validationMsg);
                typeOfReceiptForFund = typeOfReceiptForFund != null ? typeOfReceiptForFund.ToUpper() : typeOfReceiptForFund;

                switch (typeOfReceiptForFund)
                {
                    case "EXGL_AR":
                        FundsExGL_AR fundsExGL_AR = new FundsExGL_AR();
                        return fundsExGL_AR.Create(eventId, a3_id, result, jsonData, received_at, "FCExGL_AR", category, ref validationMsg);

                    case "EXGL_C":
                        FundsExGL_C fundsExGL_C = new FundsExGL_C();
                        return fundsExGL_C.Create(eventId, a3_id, result, jsonData, received_at, "FCExGL_C", category, ref validationMsg);

                    case "BKCHG":
                        FundsBkChg fundsBkChg = new FundsBkChg();
                        return fundsBkChg.Create(eventId, a3_id, result, jsonData, received_at, "FCBkChg", category, ref validationMsg);

                    default:
                        break;
                }

                string locCode = funds.A3LocationCode(nominationId, result.data.fileNumber, entityType, ref validationMsg);
                int? sequenceNumber = funds.SequenceNumber(ref validationMsg);
                int? payeeCompanyId = result.data.payeeCompanyId;
                string principalNumber = result.data.principalNumber;
                decimal? amountForFund = funds.AmountForFund(comment, ref validationMsg);
                string harbourLocCode = funds.HarbourLocCode(id, locCode, entity, ref validationMsg);
                string voyageCode = funds.VoyageCode(id, result.data.portCallId, result.data.voyageCode, result.data.vesselName, result.data.eta_date, harbourLocCode, ref validationMsg);
                string smcCode = funds.SMCCode(locCode, entityType, ref validationMsg);
                string fileBatchId = funds.FileBatchId(actionEntity, sequenceNumber, smcCode, ref validationMsg);
                int? refNo = result.data.refNumber;
                string businessType = result.data.businessType;
                string deptForFund = funds.DeptForFund(nominationId, result.data.PIC, result.data.businessType, locCode, entityType, "Fund", result.data.chargeDept, principalNumber, refNo, ref validationMsg);
                string currencyForFund = funds.CurrencyForFund(comment, ref validationMsg);
                string principalName = result.data.principalName;
                decimal exchangeRateForFund = funds.ExchangeRateForFund(comment, ref validationMsg);
                string accountTypeForFCFin = funds.AccountTypeForFCFin(payeeCompanyId, principalNumber, comment, locCode, result.data.postingDate, ref validationMsg);
                string accountForFCFin = funds.AccountForFCFin(payeeCompanyId, result.data.usdBankAccountNoJPN, result.data.jpyBankAccountNoJPN, principalNumber, comment, locCode, result.data.postingDate, ref validationMsg);
                string bankReferenceNumberForFund = funds.BankReferenceNumberForFund(comment, locCode, result.data.postingDate, ref validationMsg);
                string dim6 = funds.DIM6(locCode, id, entity, ref validationMsg);
                string dim2 = funds.Dim2forFund(locCode, ref validationMsg);
                string dim5 = funds.Dim5forFund(nominationId, result.data.businessType, comment, entityType, locCode, ref validationMsg);
                string userfield1 = funds.UserField1forFund(nominationId, result.data.businessType, comment, entityType, locCode, ref validationMsg);
                string debitBankChargeForFCFin = funds.DebitBankChargeforFCFin(locCode, ref validationMsg);
                decimal? bankChargeForFund = funds.BankChargeForFund(comment, ref validationMsg);
                DateTime? dateReceived = funds.DateReceived(result.data.dateReceived, ref validationMsg);
                string dim3ForFund = funds.Dim3ForFund(principalNumber, category, ref validationMsg);
                DateTime? postingDate = funds.PostingDate(result.data.postingDate, locCode, ref validationMsg);
                string reference = result.data.misc;
                string invoiceNo = funds.InvoiceNumber(reference, ref validationMsg);
                string invoiceNumberWithSuffix = funds.AddSuffix(reference, principalNumber, ref tmpIsSuffixed, ref validationMsg);
                int? suffixNo = funds.InvoiceSuffixNo(0, reference, principalNumber, ref validationMsg);


                if (result.data.misc == null)
                {
                    LogManager.Log.Info("Misc is null in Funds Final.");
                    validationMsg = a3Helper.AppendError("0090:: Misc is null in Funds Final.");
                }

                //validate errors                
                if (a3Helper.ValidateErrors(eventId, a3_id, result, nominationFileNumber, jsonData, received_at, result.data.vendorName, ref validationMsg))
                {
                    return true;
                }

                if (Config.bypassValidation == false && validationMsg.Count() > 0)
                {
                    return false;
                }

                if ((amountForFund != null && amountForFund == 0) || amountForFund - bankChargeForFund == 0)
                {
                    errorMsg = "";
                    if (amountForFund == 0)
                    {
                        errorMsg = "0043:: Amount is equal to(=) zero(0).";
                    }
                    else
                    {
                        errorMsg = "0019:: Amount for fund minus Bank Charge for Fund is equal to(=) zero(0).";
                    }

                    warningMsg = a3Helper.FormatWarningMsg(eventId, nominationFileNumber, received_at, result.data.vendorName, result, jsonData);
                    warningMsg = warningMsg.Replace("{warning}", errorMsg + " <br> ");

                    LogManager.Log.Warn(warningMsg);
                    A3Helper.SendMail(Config.fromWarn, Config.toWarn + "," + initials, Config.ccWarn, Config.bccWarn, Config.subjectWarn, warningMsg + "<br>" + Config.footerWarn);

                    A3Helper.InsertToA3EventJournal(eventId, action, entity, id, jsonData, received_at, 0, "", "", "", "", true, true, errorMsg);
                    return true;
                }

                #endregion

                LogManager.Log.Info("Create journal for Bank...");
                int maxCount = 3;
                if (bankChargeForFund == 0)
                {
                    maxCount = 2;
                }

                D365FOJournal journal = new D365FOJournal();
                D365FOHeader header = new D365FOHeader();
                header.CompanyCode = smcCode;
                header.TransType = "FCFin";
                if (action == "CREATE")
                {
                    header.Description = (actionEntity + " " + voyageCode + " " + principalNumber).Length > 60 ? (actionEntity + " " + voyageCode + " " + principalNumber).Substring(0, 60) : (actionEntity + " " + voyageCode + " " + principalNumber);
                    header.FileBatchID = fileBatchId;
                }
                else
                {
                    header.Description = (actionEntity + " " + voyageCode + " " + principalNumber).Length > 56 ? (actionEntity + " " + voyageCode + " " + principalNumber).Substring(0, 56) + "_Rev" : (actionEntity + " " + voyageCode + " " + principalNumber) + "_Rev";
                    header.FileBatchID = fileBatchId + "_R";
                }

                List<D365FODetail> journalDetail = new List<D365FODetail>();

                for (var i = 1; i <= maxCount; i++)
                {
                    D365FODetail detail = new D365FODetail();
                    detail.FileBatchID = header.FileBatchID;
                    detail.TransDate = result.data.postingDate.Value.ToString("yyyy-MM-dd");                   
                    detail.VesselCustomerCode = dim3ForFund;
                    detail.VoyageCode = voyageCode;                   
                    detail.RevenueType = businessType;
                    detail.Currency = currencyForFund;
                    if (action == "CREATE")
                    {
                        detail.LineDescription = (typeOfReceiptForFund + " " + principalName + " received on " + result.data.dateReceived.Value.ToString("dd/MM/yyyy")).Length > 60 ? (typeOfReceiptForFund + " " + principalName + " received on " + result.data.dateReceived.Value.ToString("dd/MM/yyyy")).Substring(0, 60) : (typeOfReceiptForFund + " " + principalName + " received on " + result.data.dateReceived.Value.ToString("dd/MM/yyyy"));
                    }
                    else
                    {
                        detail.LineDescription = (typeOfReceiptForFund + " " + principalName + " received on " + result.data.dateReceived.Value.ToString("dd/MM/yyyy")).Length > 56 ? (typeOfReceiptForFund + " " + principalName + " received on " + result.data.dateReceived.Value.ToString("dd/MM/yyyy")).Substring(0, 56) + "_Rev" : (typeOfReceiptForFund + " " + principalName + " received on " + result.data.dateReceived.Value.ToString("dd/MM/yyyy")) + "_Rev";
                    }
                    detail.ExchangeRate = exchangeRateForFund;
                    detail.SalesTaxGroup = string.Empty;
                    detail.ItemSalesTaxGroup = string.Empty;
                    detail.SalesTaxAmount = null;                   
                    detail.Document = nominationFileNumber;
                    detail.DocumentDate = result.data.dateReceived.Value.ToString("yyyy-MM-dd");
                    detail.DueDate = null;
                    detail.BankTransType = string.Empty;
                    detail.CreatedByUserId = Config.SystemCreatedBy;
                    detail.Payment = bankReferenceNumberForFund;
                    detail.UserField1 = userfield1;
                    detail.UserField2 = string.Empty;
                    detail.UserField3 = string.Empty;
                    detail.Remark = string.Empty;
                    detail.Invoice = (invoiceNumberWithSuffix.Length > 50 ? invoiceNumberWithSuffix.Substring(0, 50) : invoiceNumberWithSuffix);

                    if (i == 1) //debit ->  net fund received
                    {
                        detail.AccountType = accountTypeForFCFin;
                        detail.Account = accountForFCFin;
                        detail.Amount = amountForFund - bankChargeForFund;                      

                    }
                    else if (i == 2 && maxCount == 3) //debit -> bank charge
                    {
                        detail.AccountType = "Ledger";
                        detail.Account = debitBankChargeForFCFin;
                        detail.Amount = bankChargeForFund;
                    }
                    else //credit
                    {
                        detail.AccountType = result.data.clientCategory;
                        detail.Account = principalNumber;
                        detail.Amount = amountForFund * -1;
                    }

                    journalDetail.Add(detail);

                }

                header.Lines = journalDetail;
                journal._gatshipData = header;

                LogManager.Log.Info("Creating JSON to Funds Final processed folder...");
                string folder = Config.ProcessedFolder + Config.A3Folders.FundsFinal.ToString();
                string filePath = folder + "\\" + DateTime.Now.ToString("yyyyMMdd");
                string fileName = journal._gatshipData.FileBatchID + ".json";
                fileLocation = filePath + "\\" + fileName;
                Config.ValidatePath(fileLocation);

                LogManager.Log.Info("Posting to D354...");

                string jsonRet = "{\"Message\":{\"Status\":\"\",\"Msg\":\"\",\"Submitted\":\"\"}}";
                D365FO ax = new D365FO();
                jsonRet = ax.PostData(journal).Result;

                //reading response from D365
                response = JsonConvert.DeserializeObject<JsonResponse>(jsonRet);
                success = response.Status == 1 ? true : false;
                A3Helper.Serialize(journal, fileLocation);

                if (success)
                {
                    string entityKey = "";
                    entityKey = response.ReturnMsg;

                    LogManager.Log.Info("Status is [Success]");
                    LogManager.Log.Info("Entity key is: " + "[" + entityKey + "]");

                    A3Helper.InsertToA3EventJournal(eventId, action, entity, id, jsonData, received_at, sequenceNumber, filePath, fileName, "", "", true, false, response.ReturnMsg);

                    invoiceNo = invoiceNo.Length > 47 ? invoiceNo.Substring(0, 47) : invoiceNo;

                    if (suffixNo == null)
                    {
                        A3Helper.InsertToA3InvoiceSuffix(eventId, principalNumber, invoiceNo, 0, result);
                    }
                    else
                    {
                        A3Helper.UpdateA3InvoiceSuffix(eventId, principalNumber, invoiceNo, suffixNo, result);
                    }

                    if (tmpIsSuffixed && Config.isNotifySuffix)
                    {
                        warningMsg = a3Helper.FormatWarningMsg(eventId, nominationFileNumber, received_at, result.data.vendorName, result, jsonData);
                        warningMsg = warningMsg.Replace("{warning}", "Suffixed Invoice Number is " + invoiceNumberWithSuffix + "." + "<br>");

                        LogManager.Log.Warn(warningMsg);
                        A3Helper.SendMail(Config.fromWarn, Config.toWarn + "," + initials, Config.ccWarn, Config.bccWarn, Config.subjectWarn, warningMsg + "<br>" + Config.footerWarn);
                    }

                    //link again invoice id to expense table
                    A3Helper.LinkInvoiceExpense(id);
                    return true;
                }
                else
                {
                    validationMsg = a3Helper.AppendError(response.ReturnMsg);
                    return false;
                }
            }
            catch (Exception ex)
            {
                validationMsg = a3Helper.AppendError(ex.Message + ex.InnerException);
                return false;
            }
        }

        public bool Modify(int eventId, int a3_id, FundsPayload result, string jsonData, DateTime? received_at, string actionEntity, ref List<string> validationMsg)
        {
            int? nominationId = result.data.nominationId;
            bool retVal = false;
            action = result.action.ToUpper();
            entity = result.entity;
            id = a3_id;
            string initials = result.data.initials + "@wallem.com";

            string nominationFileNumber = result.data.fileNumber;
            int? sequenceNumber = funds.SequenceNumber(ref validationMsg);

            string typeOfReceipt = funds.TypeOfReceiptForFund(result.data.comment, ref validationMsg);
            typeOfReceipt = typeOfReceipt != null ? typeOfReceipt.ToUpper() : typeOfReceipt;

            switch (typeOfReceipt)
            {
                case "EXGL_AR":
                    FundsExGL_AR fundsExGL_AR = new FundsExGL_AR();
                    return fundsExGL_AR.Modify(eventId, a3_id, result, jsonData, received_at, "FMExGL_AR", result.data.category, ref validationMsg);

                case "EXGL_C":
                    FundsExGL_C fundsExGL_C = new FundsExGL_C();
                    return fundsExGL_C.Modify(eventId, a3_id, result, jsonData, received_at, "FMExGL_C", result.data.category, ref validationMsg);

                case "BKCHG":
                    FundsBkChg fundsBkChg = new FundsBkChg();
                    return fundsBkChg.Modify(eventId, a3_id, result, jsonData, received_at, "FMBkChg", result.data.category, ref validationMsg);

                default:
                    break;
            }

            var eventJournal = a3Helper.EventJournal();
            var previousJournal = (from seq in eventJournal
                                   where seq.a3_id == result.data.id && seq.id < eventId && seq.entity.ToUpper() == entity.ToUpper()
                                   select seq).OrderByDescending(x => x.id);

            return Create(eventId, a3_id, result, jsonData, received_at, "FMFin", ref validationMsg);
        }
    }
}
