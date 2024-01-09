
using GATShipD365TS.App_Code;
using GATShipD365TS.Helper;
using GATShipD365TS.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GATShipD365TS.GATShip.Entities
{
    public class InvoiceDebitNote_AR
    {
        A3Helper a3Helper = new A3Helper();
        EntityHelper invoice = new EntityHelper();
        string entityType = "DAID";
        const char doubleqoute = '"';
        const char singlequote = '\'';
        string action = "";
        string entity = "";
        int id = 0;
        string warningMsg = "";
        public bool Create(int eventId, int a3_id, InvoicePayload result, string jsonData, DateTime? received_at, string actionEntity, string vendorCode, string smcVendorCode, ref List<string> validationMsg)
        {
            LogManager.Log.Info("Start processing Event Id: " + eventId);
            try
            {

                WIS_Sync contextWis = new WIS_Sync();
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
                bool isGenDNPerSer = invoice.IsGenDNPerSer(locCode, ref validationMsg);
                
                string reference = result.data.reference;
                string invoiceNo = invoice.InvoiceNumber(reference, ref validationMsg);
                int? suffixNo = invoice.InvoiceSuffixNo(companyId, invoiceNo, vendorCode, ref validationMsg);
                string invoiceNumberWithSuffix = invoice.InvoiceNumberWithSuffix(daId, vendorCode, entityType, companyId, reference, invoiceNo, ref tmpIsSuffixed, ref validationMsg);
                string principalName = result.data.principalName;
                string dim2ForInvoice = invoice.Dim2forInvoice(locCode, ref validationMsg);
                string dim6 = invoice.DIM6(locCode, id, entity, ref validationMsg);

                string remarks = result.data.remarks;
                LogManager.Log.Info("Remarks Value: " + remarks);

                decimal? exchangeRate = invoice.ExchangeRateForInvoice(remarks, ref validationMsg);
                string currency = invoice.CurrencyForInvoice(remarks, ref validationMsg);
                decimal? amountWithTaxForInvoice = invoice.AmountWithTaxForInvoice(remarks, ref validationMsg);

                decimal? amount = result.data.amount;
                string postingCode = result.data.postingCode;
                decimal? actualSalesTaxAmount = invoice.ActualSalesTaxAmount(locCode, amount, amountWithTaxForInvoice, true, remarks, ref validationMsg);

                string salesTaxCode = "";
                string salesTaxGroupCode = invoice.SalesTaxGroupCode(locCode, amount, ref validationMsg);
                //string descForDebitNote = invoice.DescriptionForDebitNote(locCode, businessType, postingCode, daId, "daId", ref validationMsg);
                string code1 = result.data.code1;
                string code2 = result.data.code2;                                            
                DateTime? dateIssued = invoice.DateIssued(result.data.issuedAt, ref validationMsg);
                string creditMiscAccountForDNtMiscAR = invoice.CreditMiscAccountForDNtMiscAR(businessType, ref validationMsg);
                string dim3 = invoice.Dim3AsReferral(principalNumber, nominationFileNumber, result.data.referral, ref validationMsg);
                DateTime? postingDate = invoice.PostingDate(result.data.postingDate, locCode, ref validationMsg);
                string creditLandTransportationAccountForDNtMiscAR = invoice.CreditLandTransportationAccountForDNtMiscAR(businessType, ref validationMsg);
                string creditSeaTransportationAccountForDNtMiscAR = invoice.CreditSeaTransportationAccountForDNtMiscAR(businessType, ref validationMsg);
                string creditDocAccountForDNtMiscAR = invoice.CreditDocAccountForDNtMiscAR(businessType, ref validationMsg);
                string creditLogisticAccountForDNtMiscAR = invoice.CreditLogisticAccountForDNtMiscAR(businessType, ref validationMsg);
               //string debitAccountCodeForDebitNote = invoice.DebitAccountCodeForDebitNote(locCode, postingCode, code1, code2, ref validationMsg);              
               //string debitAccountTypeForDebitNote = "Ledger";


                if (amountWithTaxForInvoice != (result.data.mis + result.data.lt + result.data.seaTransport + result.data.bcr + result.data.pacoIncome + result.data.doc + result.data.logistic))
                {
                    validationMsg.Add("0087:: Credit amount is not equal to debit amount." + "<br>");
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
                #endregion

                LogManager.Log.Info("Create journal for Bank...");

                D365FOJournal journal = new D365FOJournal();
                D365FOHeader header = new D365FOHeader();

                msgSb = new StringBuilder();
                StringBuilder sbErrorMsg = new StringBuilder();
                header.CompanyCode = smcCode;
                header.TransType = "ICDNt_AR";

                if (action == "CREATE")
                {
                    header.Description = (actionEntity + " " + voyageCode + " " + nominationFileNumber + " " + invoiceNo).Length > 60 ? (actionEntity + " " + voyageCode + " " + nominationFileNumber + " " + invoiceNo).Substring(0, 60) : (actionEntity + " " + voyageCode + " " + nominationFileNumber + " " + invoiceNo);
                    header.FileBatchID = fileBatchId;
                }
                else
                {
                    header.Description = (actionEntity + " " + voyageCode + " " + nominationFileNumber + " " + invoiceNo).Length > 56 ? (actionEntity + " " + voyageCode + " " + nominationFileNumber + " " + invoiceNo).Substring(0, 56) + "_Rev" : (actionEntity + " " + voyageCode + " " + nominationFileNumber + " " + invoiceNo) + "_Rev";
                    header.FileBatchID = fileBatchId + "_R";
                }

                List<D365FODetail> journalDetail = new List<D365FODetail>();


                if (action == "CREATE")
                {
                    if (amountWithTaxForInvoice < 0)
                    {
                        warningMsg = a3Helper.FormatWarningMsg(eventId, nominationFileNumber, received_at, result.data.vendorName, result, jsonData);
                        warningMsg = warningMsg.Replace("{warning}", "Amount is negative." + "<br>");

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

                //Generating Debit and Credit lines
                for (var i = 1; i <= 8; i++)
                {
                    D365FODetail detail = new D365FODetail();
                    detail.FileBatchID = header.FileBatchID;
                    detail.TransDate = result.data.postingDate.Value.ToString("yyyy-MM-dd");                  
                    detail.VesselCustomerCode = dim3;
                    detail.VoyageCode = voyageCode;                 
                    detail.RevenueType = businessType;
                    detail.Currency = currency;
                    detail.ExchangeRate = exchangeRate;
                    detail.SalesTaxGroup = salesTaxGroupCode;
                    if (action == "CREATE")
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
                    detail.UserField2 = string.Empty;
                    detail.UserField3 = amountWithTaxForInvoice < 0 ? "SpecialReversal" : string.Empty;
                    detail.Remark = string.Empty;
                    detail.SalesTaxAmount = actualSalesTaxAmount != null ? Math.Abs(actualSalesTaxAmount.Value) : actualSalesTaxAmount;

                    salesTaxCode = invoice.SalesTaxCode(a3_id, locCode, detail.SalesTaxAmount, amount, ref validationMsg);

                    if (i == 1) //debit
                    {
                        detail.AccountType = "Ledger";
                        detail.Account = invoice.DebitClearingInvoiceDN_AR();
                        detail.Amount = amountWithTaxForInvoice;
                        if(action == "CREATE")
                        {
                            detail.LineDescription = (voyageCode + " " + result.data.description + " " + invoiceNo + " " + result.data.expText).Length > 60 ? (voyageCode + " " + result.data.description + " " + invoiceNo + " " + result.data.expText).Substring(0, 60) : (voyageCode + " " + result.data.description + " " + invoiceNo + " " + result.data.expText);
                        }
                        else
                        {
                            detail.LineDescription = (voyageCode + " " + result.data.description + " " + invoiceNo + " " + result.data.expText).Length > 56 ? (voyageCode + " " + result.data.description + " " + invoiceNo + " " + result.data.expText).Substring(0, 56) + "_Rev" : (voyageCode + " " + result.data.description + " " + invoiceNo + " " + result.data.expText) + "_Rev";
                        }
                       
                        detail.ItemSalesTaxGroup = salesTaxCode == "" ? "" : salesTaxGroupCode + "_EX";
                    }
                    else if (i == 2)
                    {
                        detail.AccountType = "Ledger";
                        detail.Account = creditMiscAccountForDNtMiscAR;
                        detail.LineDescription = (voyageCode + " " + result.data.description + " " + invoiceNo + " Misc").Length > 60 ? (voyageCode + " " + result.data.description + " " + invoiceNo + " Misc").Substring(0, 60) : (voyageCode + " " + result.data.description + " " + invoiceNo + " Misc");
                        detail.Amount = result.data.mis * -1;                        
                        detail.ItemSalesTaxGroup = salesTaxCode;
                    }
                    else if (i == 3)
                    {
                        detail.AccountType = "Ledger";
                        detail.Account = creditLandTransportationAccountForDNtMiscAR;
                        if (action == "CREATE")
                        {
                            detail.LineDescription = (voyageCode + " " + result.data.description + " " + invoiceNo + " Land transportation").Length > 60 ? (voyageCode + " " + result.data.description + " " + invoiceNo + " Land transportation").Substring(0, 60) : (voyageCode + " " + result.data.description + " " + invoiceNo + " Land transportation");
                        }
                        else
                        {
                            detail.LineDescription = (voyageCode + " " + result.data.description + " " + invoiceNo + " Land transportation").Length > 56 ? (voyageCode + " " + result.data.description + " " + invoiceNo + " Land transportation").Substring(0, 56) + "_Rev" : (voyageCode + " " + result.data.description + " " + invoiceNo + " Land transportation") + "_Rev";
                        }                       
                        detail.Amount = result.data.lt * -1;
                        detail.ItemSalesTaxGroup = salesTaxCode;
                    }
                    else if (i == 4)
                    {
                        detail.AccountType = "Ledger";
                        detail.Account = creditSeaTransportationAccountForDNtMiscAR;
                        if (action == "CREATE")
                        {
                            detail.LineDescription = (voyageCode + " " + result.data.description + " " + invoiceNo + " Sea Transportation").Length > 60 ? (voyageCode + " " + result.data.description + " " + invoiceNo + " Sea Transportation").Substring(0, 60) : (voyageCode + " " + result.data.description + " " + invoiceNo + " Sea Transportation");
                        }
                        else
                        {
                            detail.LineDescription = (voyageCode + " " + result.data.description + " " + invoiceNo + " Sea Transportation").Length > 56 ? (voyageCode + " " + result.data.description + " " + invoiceNo + " Sea Transportation").Substring(0, 56) + "_Rev" : (voyageCode + " " + result.data.description + " " + invoiceNo + " Sea Transportation") + "_Rev";
                        }                        
                        detail.Amount = result.data.seaTransport * -1;
                        detail.ItemSalesTaxGroup = salesTaxCode;
                    }
                    else if (i == 5)
                    {
                        detail.AccountType = "Ledger";
                        detail.Account = invoice.CreditAccountBankChargeInvoiceDN();
                        if (action == "CREATE")
                        {
                            detail.LineDescription = (voyageCode + " " + result.data.description + " " + invoiceNo + " Bank charges recovery").Length > 60 ? (voyageCode + " " + result.data.description + " " + invoiceNo + " Bank charges recovery").Substring(0, 60) : (voyageCode + " " + result.data.description + " " + invoiceNo + " Bank charges recovery");
                        }
                        else
                        {
                            detail.LineDescription = (voyageCode + " " + result.data.description + " " + invoiceNo + " Bank charges recovery").Length > 56 ? (voyageCode + " " + result.data.description + " " + invoiceNo + " Bank charges recovery").Substring(0, 56) + "_Rev" : (voyageCode + " " + result.data.description + " " + invoiceNo + " Bank charges recovery") + "_Rev";
                        }                       
                        detail.Amount = result.data.bcr * -1;
                        detail.ItemSalesTaxGroup = salesTaxCode;
                    }
                    else if (i == 6) //paco
                    {
                        detail.AccountType = "Ledger";
                        detail.Account = invoice.CreditAccountPacoIncomeInvoiceDN();
                        if (action == "CREATE")
                        {
                            detail.LineDescription = (voyageCode + " " + result.data.description + " " + invoiceNo + " Paco income").Length > 60 ? (voyageCode + " " + result.data.description + " " + invoiceNo + " Paco income").Substring(0, 60) : (voyageCode + " " + result.data.description + " " + invoiceNo + " Paco income");
                        }
                        else
                        {
                            detail.LineDescription = (voyageCode + " " + result.data.description + " " + invoiceNo + " Paco income").Length > 56 ? (voyageCode + " " + result.data.description + " " + invoiceNo + " Paco income").Substring(0, 56) + "_Rev" : (voyageCode + " " + result.data.description + " " + invoiceNo + " Paco income") + "_Rev";
                        }                        
                        detail.Amount = result.data.pacoIncome * -1;
                        detail.ItemSalesTaxGroup = salesTaxCode;
                    }
                    else if (i == 7) //doc
                    {
                        detail.AccountType = "Ledger";
                        detail.Account = creditDocAccountForDNtMiscAR;                      
                        if (action == "CREATE")
                        {
                            detail.LineDescription = (voyageCode + " " + result.data.description + " " + invoiceNo + " DOC").Length > 60 ? (voyageCode + " " + result.data.description + " " + invoiceNo + " DOC").Substring(0, 60) : (voyageCode + " " + result.data.description + " " + invoiceNo + " DOC");
                        }
                        else
                        {
                            detail.LineDescription = (voyageCode + " " + result.data.description + " " + invoiceNo + " DOC").Length > 56 ? (voyageCode + " " + result.data.description + " " + invoiceNo + " DOC").Substring(0, 56) + "_Rev" : (voyageCode + " " + result.data.description + " " + invoiceNo + " DOC") + "_Rev";
                        }
                        detail.Amount = result.data.doc * -1;
                        detail.ItemSalesTaxGroup = salesTaxCode;
                    }
                      else //logistic
                    {
                        detail.AccountType = "Ledger";
                        detail.Account = creditLogisticAccountForDNtMiscAR;                      
                        if (action == "CREATE")
                        {
                            detail.LineDescription = (voyageCode + " " + result.data.description + " " + invoiceNo + " Logistic").Length > 60 ? (voyageCode + " " + result.data.description + " " + invoiceNo + " Logistic").Substring(0, 60) : (voyageCode + " " + result.data.description + " " + invoiceNo + " Logistic");
                        }
                        else
                        {
                            detail.LineDescription = (voyageCode + " " + result.data.description + " " + invoiceNo + " Logistic").Length > 56 ? (voyageCode + " " + result.data.description + " " + invoiceNo + " Logistic").Substring(0, 56) + "_Rev" : (voyageCode + " " + result.data.description + " " + invoiceNo + " Logistic") + "_Rev";
                        }
                        detail.Amount = result.data.logistic * -1;
                        detail.ItemSalesTaxGroup = salesTaxCode;
                    }

                    if (detail.Amount != 0)
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
