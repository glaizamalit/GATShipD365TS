
using GATShipD365TS.App_Code;
using GATShipD365TS.Helper;
using GATShipD365TS.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GATShipD365TS.GATShip.Entities
{
    public class InvoiceTH
    {
        A3Helper a3Helper = new A3Helper();
        EntityHelper invoice = new EntityHelper();
        JsonResponse response;
        bool success = false;
        string entityType = "DAID";
        const char doubleqoute = '"';
        const char singlequote = '\'';
        string action = "";
        string entity = "";
        int id = 0;
        string errorMsg = "";
        string warningMsg = "";

        public bool Create(int eventId, int a3_id, InvoicePayload result, string jsonData, DateTime? received_at, string actionEntity, string vendorCode, string smcVendorCode, ref List<string> validationMsg)
        {
            LogManager.Log.Info("Start processing Event Id: " + eventId);
            try
            {
                GSWallem contextgswallem = new GSWallem();

                bool tmpIsSuffixed = false;

                JsonResponse response;
                bool success = false;
                string fileLocation = "";
                //int daId = result.data.daId;
                int? daId = result.data.id;
                action = result.action.ToUpper();
                entity = result.entity;
                id = a3_id;
                string initials = result.data.initials + "@wallem.com";


                StringBuilder msgSb = new StringBuilder();

                LogManager.Log.Info("Validating fields...");

                #region VALIDATIONS      
                string nominationFileNumber = result.data.fileNumber;
                int? nominationId = result.data.nominationId;

                string locCode = invoice.A3LocationCode(nominationId, result.data.fileNumber, entityType, ref validationMsg);
                string smcCode = invoice.SMCCode(locCode, entityType, ref validationMsg);
                int? companyId = result.data.companyId;
                //string vendorCode = invoice.VendorCode(result.data.vendorCode, ref validationMsg);
                string vendorName = result.data.vendorName;
                int? sequenceNumber = invoice.SequenceNumber(ref validationMsg);
                string fileBatchId = invoice.FileBatchId(actionEntity, sequenceNumber, smcCode, ref validationMsg);
                string PIC = result.data.PIC;
                string businessType = result.data.businessType;
                string principalNumber = result.data.principalNumber;
                string vesselName = result.data.vesselName;
                string deptForInvoice = invoice.DeptForInvoice(daId, PIC, businessType, locCode, entityType, "Invoice", result.data.chargeDept, result.data.expTemplateId, principalNumber, vendorCode, smcVendorCode, ref validationMsg);
                // string arrivalDate = invoice.DateForVoyageCode(daId, locCode, entityType, ref validationMsg);
                string harbourLocCode = invoice.HarbourLocCode(id, locCode, entity, ref validationMsg);
                string voyageCode = invoice.VoyageCode(id, result.data.portCallId, result.data.voyageCode, vesselName, result.data.eta_date, harbourLocCode, ref validationMsg);

                string remarks = result.data.remarks;
                LogManager.Log.Info("Remarks Value: " + remarks);

                string currency = invoice.CurrencyForInvoice(remarks, ref validationMsg);
                
                string reference = result.data.reference;
                string invoiceNo = invoice.InvoiceNumber(reference, ref validationMsg);
                int? suffixNo = invoice.InvoiceSuffixNo(companyId, invoiceNo, vendorCode, ref validationMsg);
                string invoiceNumberWithSuffix = invoice.InvoiceNumberWithSuffix(daId, vendorCode, entityType, companyId, reference, invoiceNo, ref tmpIsSuffixed, ref validationMsg);
                string principalName = result.data.principalName;
                string dim2ForInvoice = invoice.Dim2forInvoice(locCode, ref validationMsg);
                string dim6 = invoice.DIM6(locCode, id, entity, ref validationMsg);
                decimal? amount = result.data.amount;
                decimal? amountWithTaxForInvoice = invoice.AmountWithTaxForInvoice(remarks, ref validationMsg);
                //  decimal? actualSalesTaxAmount = invoice.ActualSalesTaxAmount(locCode, result.data.amount, amountWithTaxForInvoice, false, remarks, ref validationMsg);
                decimal? amountForInvoice = invoice.AmountForInvoice(remarks, ref validationMsg);
                decimal? exchangeRate = invoice.ExchangeRateForInvoice(remarks, ref validationMsg);
                string postingCode = result.data.postingCode;
                //  string salesTaxGroupCode = invoice.SalesTaxGroupCode(locCode, amount, ref validationMsg);
                string code1 = result.data.code1;
                string code2 = result.data.code2;
                string debitAccountCodeForInvoice = invoice.DebitAccountCodeForInvoice(locCode, postingCode, code1, code2, ref validationMsg);
                string creditAccountCodeForInvoice = invoice.CreditAccountCodeForInvoice(locCode, postingCode, vendorCode, code1, code2, ref validationMsg);
                string creditAccountTypeInvoice = invoice.CreditAccountTypeForInvoice(locCode, postingCode, code1, code2, ref validationMsg);
                string debitAccountTypeInvoice = "Ledger";
                DateTime? dateIssued = invoice.DateIssued(result.data.issuedAt, ref validationMsg);
                string dim3 = invoice.Dim3AsReferral(principalNumber, nominationFileNumber, result.data.referral, ref validationMsg);
                DateTime? postingDate = invoice.PostingDate(result.data.postingDate, locCode, ref validationMsg);

                //validate errors                
                if (a3Helper.ValidateErrors(eventId, a3_id, result, nominationFileNumber, jsonData, received_at, result.data.vendorName, ref validationMsg))
                {
                    return true;
                }

                if (Config.bypassValidation == false && validationMsg.Count() > 0)
                {
                    return false;
                }
                #endregion

                LogManager.Log.Info("Create journal for Bank...");

                D365FOJournal journal = new D365FOJournal();
                D365FOHeader header = new D365FOHeader();

                msgSb = new StringBuilder();
                StringBuilder sbErrorMsg = new StringBuilder();
                header.CompanyCode = smcCode;
                header.TransType = "ICInvTH";
                var lineDesc = "";

                if(action == "CREATE")
                {
                    header.Description = (actionEntity + " " + voyageCode + " " + nominationFileNumber + " " + invoiceNumberWithSuffix + " " + vendorName).Length > 60 ? (actionEntity + " " + voyageCode + " " + nominationFileNumber + " " + invoiceNumberWithSuffix + " " + vendorName).Substring(0, 60) : (actionEntity + " " + voyageCode + " " + nominationFileNumber + " " + invoiceNumberWithSuffix + " " + vendorName);
                    header.FileBatchID = fileBatchId;
                    lineDesc = (result.data.description + " " + invoiceNumberWithSuffix + " " + vendorName).Length > 60 ? (result.data.description + " " + invoiceNumberWithSuffix + " " + vendorName).Substring(0, 60) : (result.data.description + " " + invoiceNumberWithSuffix + " " + vendorName);
                }
                else
                {
                    header.Description = (actionEntity + " " + voyageCode + " " + nominationFileNumber + " " + invoiceNumberWithSuffix + " " + vendorName).Length > 56 ? (actionEntity + " " + voyageCode + " " + nominationFileNumber + " " + invoiceNumberWithSuffix + " " + vendorName).Substring(0, 56) + "_Rev" : (actionEntity + " " + voyageCode + " " + nominationFileNumber + " " + invoiceNumberWithSuffix + " " + vendorName) + "_Rev";
                    header.FileBatchID = fileBatchId +"_R";
                    lineDesc = (result.data.description + " " + invoiceNumberWithSuffix + " " + vendorName).Length > 56 ? (result.data.description + " " + invoiceNumberWithSuffix + " " + vendorName).Substring(0, 56) + "_Rev" : (result.data.description + " " + invoiceNumberWithSuffix + " " + vendorName) + "_Rev";
                }
              
                List<D365FODetail> journalDetail = new List<D365FODetail>();

                if (action == "CREATE")
                {
                    if (amountWithTaxForInvoice < 0)
                    {
                        warningMsg = a3Helper.FormatWarningMsg(eventId, nominationFileNumber, received_at, result.data.vendorName, result, jsonData);
                        warningMsg = warningMsg.Replace("{warning}", "Amount is negative. <br>");

                        LogManager.Log.Warn(warningMsg);
                        A3Helper.SendMail(Config.fromWarn, Config.toWarn + "," + initials, Config.ccWarn, Config.bccWarn, Config.subjectWarn, warningMsg + "<br>" + Config.footerWarn);
                    }
                }

                if (amountWithTaxForInvoice == 0)
                {
                    warningMsg = a3Helper.FormatWarningMsg(eventId, nominationFileNumber, received_at, result.data.vendorName, result, jsonData);
                    warningMsg = warningMsg.Replace("{warning}", "Amount is equal to(=) zero(0)." + " <br> ");

                    LogManager.Log.Warn(warningMsg);
                    A3Helper.SendMail(Config.fromWarn, Config.toWarn + "," + initials, Config.ccWarn, Config.bccWarn, Config.subjectWarn, warningMsg + "<br>" + Config.footerWarn);
                    A3Helper.InsertToA3EventJournal(eventId, action, entity, id, jsonData, received_at, 0, "", "", "", "", true, true, "Amount is equal to (=) zero(0).");
                    return true;
                }

                
                for (var i = 1; i <= 3; i++)
                {
                    D365FODetail detail = new D365FODetail();
                    detail.FileBatchID = header.FileBatchID;
                    detail.TransDate = result.data.postingDate.Value.ToString("yyyy-MM-dd");                  
                    detail.VesselCustomerCode = dim3;
                    detail.VoyageCode = voyageCode;                   
                    detail.RevenueType = businessType;
                    detail.Currency = currency;
                    detail.ExchangeRate = exchangeRate;
                    detail.SalesTaxGroup = string.Empty;
                    if(action == "CREATE")
                    {
                        detail.Invoice = invoiceNumberWithSuffix.Length > 50 ? invoiceNumberWithSuffix.Substring(0, 50) : invoiceNumberWithSuffix;
                    }
                    else
                    {
                        detail.Invoice = invoiceNumberWithSuffix.Length > 46 ? invoiceNumberWithSuffix.Substring(0, 46) + "_Rev" : invoiceNumberWithSuffix + "_Rev";
                    }
                    string docNum = nominationFileNumber + " " + result.data.misc;
                    detail.Document = (docNum.Length > 50 ? docNum.Substring(0, 50) : docNum);
                    detail.DocumentDate = result.data.issuedAt.Value.ToString("yyyy-MM-dd");
                    detail.DueDate = null;
                    detail.Payment = string.Empty;
                    detail.BankTransType = string.Empty;
                    detail.CreatedByUserId = Config.SystemCreatedBy;
                    detail.UserField1 = PIC;
                    detail.UserField2 = amountWithTaxForInvoice < 0 ? "SpecialReversal" : string.Empty;
                    detail.UserField3 = string.Empty;
                    detail.Remark = string.Empty;
                    detail.SalesTaxAmount = null;
                    detail.LineDescription = lineDesc;

                    // salesTaxCode = invoice.SalesTaxCode(locCode, actualSalesTaxAmount, amount, ref validationMsg);
                    detail.ItemSalesTaxGroup = string.Empty;

                    if (i == 1) //debit
                    {
                        detail.AccountType = debitAccountTypeInvoice;
                        detail.Account = debitAccountCodeForInvoice;
                        detail.Amount = amountForInvoice + result.data.vat;
                    }
                    else if (i == 2) //debit
                    {
                        detail.AccountType = debitAccountTypeInvoice;
                        detail.Account = invoice.DebitAccountInvoiceDNTH();
                        detail.Amount = result.data.vendorGST;
                    }
                    else //credit
                    {
                        detail.AccountType = creditAccountTypeInvoice;
                        detail.Account = creditAccountCodeForInvoice;
                        detail.Amount = (amountForInvoice + result.data.vat + result.data.vendorGST) * -1;
                    }

                    if (i == 2)
                    {
                        if (result.data.vendorGST != 0)
                        {
                            journalDetail.Add(detail);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        journalDetail.Add(detail);
                    }                  
                }

                header.Lines = journalDetail;
                journal._gatshipData = header;

                if (journalDetail.Count() > 0)
                {
                    //Send email warning for those invalid amount
                    if (sbErrorMsg.ToString() != "")
                    {
                        warningMsg = a3Helper.FormatWarningMsg(eventId, nominationFileNumber, received_at, result.data.vendorName, result, jsonData);
                        warningMsg = warningMsg.Replace("{warning}", sbErrorMsg.ToString() + "<br>");

                        LogManager.Log.Warn(warningMsg);
                        A3Helper.SendMail(Config.fromWarn, Config.toWarn + "," + initials, Config.ccWarn, Config.bccWarn, Config.subjectWarn, warningMsg + "<br>" + Config.footerWarn);
                    }


                    LogManager.Log.Info("Creating JSON to Invoice processed folder...");
                    string folder = Config.ProcessedFolder + Config.A3Folders.Invoice.ToString();
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
                        LogManager.Log.Info("Status is [Success]");
                        string entityKey = "";
                        entityKey = response.ReturnMsg;
                        LogManager.Log.Info("Entity key is: " + "[" + entityKey + "]");
                        A3Helper.InsertToA3EventJournal(eventId, action, entity, id, jsonData, received_at, sequenceNumber, filePath, fileName, "", "", true, false, entityKey);

                        invoiceNo = invoiceNo.Length > 47 ? invoiceNo.Substring(0, 47) : invoiceNo;

                        if (suffixNo == null)
                        {
                            A3Helper.InsertToA3InvoiceSuffix(eventId, vendorCode, invoiceNo, 0, result);
                        }
                        else
                        {
                            A3Helper.UpdateA3InvoiceSuffix(eventId, vendorCode, invoiceNo, suffixNo, result);
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
                else
                {
                    string msg = "There are no journals generated for Event ID: " + eventId;
                    LogManager.Log.Warn(msg);
                    sbErrorMsg.Append(msg);

                    warningMsg = a3Helper.FormatWarningMsg(eventId, nominationFileNumber, received_at, result.data.vendorName, result, jsonData);
                    warningMsg = warningMsg.Replace("{warning}", sbErrorMsg.ToString() + "<br>");

                    LogManager.Log.Warn(warningMsg);
                    A3Helper.SendMail(Config.fromWarn, Config.toWarn + "," + initials, Config.ccWarn, Config.bccWarn, Config.subjectWarn, warningMsg + "<br>" + Config.footerWarn);
                    A3Helper.InsertToA3EventJournal(eventId, action, entity, id, jsonData, received_at, 0, "", "", "", "", true, true, msg);
                    return true;
                }

            }
            catch (Exception ex)
            {
                validationMsg = a3Helper.AppendError(ex.Message + ex.InnerException);
                return false;
            }
        }
    }
}
