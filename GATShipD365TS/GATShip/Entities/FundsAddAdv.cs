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
using Newtonsoft.Json.Linq;

namespace GATShipD365TS.GATShip.Entities
{
    public class FundsAddAdv
    {
        A3Helper a3Helper = new A3Helper();
        EntityHelper funds = new EntityHelper();
        //JsonResponse response;
        bool success = false;
        string entityType = "NOMID";
        const char doubleqoute = '"';
        const char singlequote = '\'';
        string action = "";
        string entity = "";
        int id = 0;
        string errorMsg = "";
        string warningMsg = "";
        public bool Create(int eventId, int a3_id, FundsPayload result, string jsonData, DateTime? received_at, string actionEntity, ref List<string> validationMsg)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sb2 = new StringBuilder();
            LogManager.Log.Info("Start processing Event Id: " + eventId);
            try
            {
                bool tmpIsSuffixed = false;
                int? nominationId = result.data.nominationId;
                int? payeeCompanyId = result.data.payeeCompanyId;
                string comment = result.data.comment;
                action = result.action.ToUpper();
                entity = result.entity;
                id = a3_id;
                string category = result.data.category;
                category = category == null || category == "" ? "ADDITIONAL" : category.ToUpper();
                StringBuilder msgSb = new StringBuilder();
                string initials = result.data.initials + "@wallem.com";

                LogManager.Log.Info("Validating fields...");

                #region Validations        
                string nominationFileNumber = result.data.fileNumber;

                string typeOfReceipt = funds.TypeOfReceiptForFund(comment, ref validationMsg);
                typeOfReceipt = typeOfReceipt != null ? typeOfReceipt.ToUpper() : typeOfReceipt;

                switch (typeOfReceipt)
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
                string smccode = funds.SMCCode(locCode, entityType, ref validationMsg);
                int? sequenceNumber = funds.SequenceNumber(ref validationMsg);
                string fileBatchId = funds.FileBatchId(actionEntity, sequenceNumber, smccode, ref validationMsg);
                string[] batchIdArr = fileBatchId.Split("_".ToCharArray());
                int sequenceBank = int.Parse(batchIdArr[2]) + 1;
                string fileBatchIdBank = batchIdArr[0] + "_" + batchIdArr[1] + "_" + sequenceBank.ToString().PadLeft(4, '0') + "_" + batchIdArr[3] + "_" + batchIdArr[4];
                string principalNumber = result.data.principalNumber;
                string principalName = result.data.principalName;
                string vesselName = result.data.vesselName;
                string harbourLocCode = funds.HarbourLocCode(id, locCode, entity, ref validationMsg);
                string voyageCode = funds.VoyageCode(id, result.data.portCallId, result.data.voyageCode, vesselName, result.data.eta_date, harbourLocCode, ref validationMsg);
                int? refNo = result.data.refNumber;
                string deptForFund = funds.DeptForFund(nominationId, result.data.PIC, result.data.businessType, locCode, entityType, "Fund", result.data.chargeDept, principalNumber, refNo, ref validationMsg);
                string currency = funds.CurrencyForFund(comment, ref validationMsg);
                decimal? amountForFund = funds.AmountForFund(comment, ref validationMsg);
                decimal exchangeRateForRund = funds.ExchangeRateForFund(comment, ref validationMsg);
                string bankReference = funds.BankReferenceNumberForFund(comment, locCode, result.data.postingDate, ref validationMsg);
                string clearingAccountForFCAdv = funds.ClearingAccountForFCAdv(locCode, ref validationMsg);
                string debitBankForFCAdv = funds.DebitBankForFCAdv(locCode, result.data.usdBankAccountNoJPN, result.data.jpyBankAccountNoJPN, comment, ref validationMsg);
                string dim6 = funds.DIM6(locCode, id, entity, ref validationMsg);
                string dim2 = funds.Dim2forFund(locCode, ref validationMsg);
                string dim5 = funds.Dim5forFund(nominationId, result.data.businessType, comment, entityType, locCode, ref validationMsg);
                string businessType = result.data.businessType;
                string userfield1 = funds.UserField1forFund(nominationId, result.data.businessType, comment, entityType, locCode, ref validationMsg);
                string debitBankChargeForFCAdv = funds.DebitBankChargeforFCAdv(locCode, ref validationMsg);
                decimal? bankChargeForFund = funds.BankChargeForFund(comment, ref validationMsg);
                DateTime? dateReceived = funds.DateReceived(result.data.dateReceived, ref validationMsg);
                string dim3ForFund = funds.Dim3ForFund(principalNumber, category, ref validationMsg);
                string debitAccountTypeForFCAdv = funds.DebitAccountTypeForFCAdv(locCode, ref validationMsg);
                DateTime? postingDate = funds.PostingDate(result.data.postingDate, locCode, ref validationMsg);
                string invoiceNumberWithSuffix = funds.AddSuffix(voyageCode + " " + typeOfReceipt, principalNumber, ref tmpIsSuffixed, ref validationMsg);
                int? suffixNo = funds.InvoiceSuffixNo(0, voyageCode + " " + typeOfReceipt, principalNumber, ref validationMsg);


                //validate errors                
                if (a3Helper.ValidateErrors(eventId, a3_id, result, nominationFileNumber, jsonData, received_at, result.data.vendorName, ref validationMsg))
                {
                    return true;
                }

                if (Config.bypassValidation == false && validationMsg.Count() > 0)
                {
                    return false;
                }

                if (action == "CREATE")
                {
                    if (amountForFund < 0)
                    {
                        warningMsg = a3Helper.FormatWarningMsg(eventId, nominationFileNumber, received_at, result.data.vendorName, result, jsonData);
                        warningMsg = warningMsg.Replace("{warning}", "Amount is negative." + "<br>");

                        LogManager.Log.Warn(warningMsg);
                        A3Helper.SendMail(Config.fromWarn, Config.toWarn + "," + initials, Config.ccWarn, Config.bccWarn, Config.subjectWarn, warningMsg + "<br>" + Config.footerWarn);
                    }
                }
                if (amountForFund == 0)
                {
                    msgSb = new StringBuilder();

                    warningMsg = a3Helper.FormatWarningMsg(eventId, nominationFileNumber, received_at, result.data.vendorName, result, jsonData);
                    warningMsg = warningMsg.Replace("{warning}", "Amount is equal to(=) zero(0)." + " <br> ");

                    LogManager.Log.Warn(warningMsg);
                    A3Helper.SendMail(Config.fromWarn, Config.toWarn + "," + initials, Config.ccWarn, Config.bccWarn, Config.subjectWarn, warningMsg + "<br>" + Config.footerWarn);
                    A3Helper.InsertToA3EventJournal(eventId, action, entity, id, jsonData, received_at, 0, "", "", "", "", true, true, "Amount is equal to (=) zero(0).");
                    return true;
                }


                #endregion

                JsonResponse response;
                string fileLocation = "";
                string fileLocation2 = "";

                LogManager.Log.Info("Creating journals for Customer and Bank...");

                #region Customer
                LogManager.Log.Info("Create journal for Customer...");
                D365FOJournal journal = new D365FOJournal();
                D365FOHeader header = new D365FOHeader();
                header.CompanyCode = smccode;
                header.TransType = "FCAdvR";
                var desc = actionEntity + " " + voyageCode + " " + principalNumber + " " + nominationFileNumber;

                if (action == "CREATE")
                {
                    header.Description = desc.Length > 60 ? desc.Substring(0, 60) : desc;
                    header.FileBatchID = fileBatchId;
                }
                else
                {
                    header.Description = desc.Length > 56 ? desc.Substring(0, 56) + "_Rev" : desc + "_Rev";
                    header.FileBatchID = fileBatchId + "_R";
                }

                List<D365FODetail> journalDetail = new List<D365FODetail>();

                for (var i = 1; i <= 2; i++)
                {
                    D365FODetail detail = new D365FODetail();
                    detail.FileBatchID = header.FileBatchID;
                    detail.TransDate = result.data.postingDate.Value.ToString("yyyy-MM-dd");
                    // detail.DIM1 = deptForFund;
                    // detail.DIM2 = dim2;
                    // detail.DIM3 = dim3ForFund;
                    detail.VesselCustomerCode = dim3ForFund;
                    // detail.DIM4 = voyageCode;
                    detail.VoyageCode = voyageCode;
                    // detail.DIM5 = dim5;
                    // detail.DIM6 = dim6;
                    detail.RevenueType = businessType;
                    detail.Currency = currency;
                    if (action == "CREATE")
                    {
                        detail.LineDescription = (vesselName + " " + principalNumber + " " + typeOfReceipt + " " + voyageCode).Length > 60 ? (vesselName + " " + principalNumber + " " + typeOfReceipt + " " + voyageCode).Substring(0, 60) : (vesselName + " " + principalNumber + " " + typeOfReceipt + " " + voyageCode);
                    }
                    else
                    {
                        detail.LineDescription = (vesselName + " " + principalNumber + " " + typeOfReceipt + " " + voyageCode).Length > 56 ? (vesselName + " " + principalNumber + " " + typeOfReceipt + " " + voyageCode).Substring(0, 56) + "_Rev" : (vesselName + " " + principalNumber + " " + typeOfReceipt + " " + voyageCode) + "_Rev";
                    }
                    detail.ExchangeRate = exchangeRateForRund;
                    detail.SalesTaxGroup = "";
                    detail.ItemSalesTaxGroup = "";
                    detail.SalesTaxAmount = null;
                    detail.Invoice = (invoiceNumberWithSuffix.Length > 50 ? invoiceNumberWithSuffix.Substring(0, 50) : invoiceNumberWithSuffix);
                    string docNum = nominationFileNumber + " " + result.data.misc;
                    detail.Document = (docNum.Length > 50 ? docNum.Substring(0, 50) : docNum);
                    detail.DocumentDate = result.data.dateReceived.Value.ToString("yyyy-MM-dd");
                    detail.DueDate = null;
                    detail.Payment = bankReference;
                    detail.BankTransType = "";
                    detail.CreatedByUserId = Config.SystemCreatedBy;
                    detail.UserField1 = userfield1;
                    detail.UserField2 = string.Empty;
                    detail.UserField3 = string.Empty;
                    detail.Remark = string.Empty;

                    if (i == 1) //debit
                    {
                        detail.AccountType = "Customer";
                        detail.Account = principalNumber;
                        detail.Amount = amountForFund;
                    }
                    else //credit
                    {
                        detail.AccountType = "Ledger";
                        detail.Account = clearingAccountForFCAdv;
                        detail.Amount = amountForFund * -1;

                    }

                    journalDetail.Add(detail);
                }

                header.Lines = journalDetail;
                journal._gatshipData = header;

                #endregion

                #region Bank
                LogManager.Log.Info("Create journal for Bank...");
                int maxCount = 3;
                if (bankChargeForFund == 0)
                {
                    maxCount = 2;
                }
                D365FOJournal journal2 = new D365FOJournal();
                D365FOHeader header2 = new D365FOHeader();
                header2.CompanyCode = smccode;
                header2.TransType = "FCAdvS";
                desc = actionEntity + " " + voyageCode + " " + principalNumber + " " + nominationFileNumber;

                if (action == "CREATE")
                {
                    header2.Description = desc.Length > 60 ? desc.Substring(0, 60) : desc;
                    header2.FileBatchID = fileBatchIdBank;
                }
                else
                {
                    header2.Description = desc.Length > 56 ? desc.Substring(0, 56) + "_Rev" : desc + "_Rev";
                    header2.FileBatchID = fileBatchIdBank + "_R";
                }


                List<D365FODetail> journalDetail2 = new List<D365FODetail>();

                for (var i = 1; i <= maxCount; i++)
                {
                    if ((i == 1 && (amountForFund - bankChargeForFund) != 0) || i != 1)
                    {
                        D365FODetail detail = new D365FODetail();
                        detail.FileBatchID = header2.FileBatchID;
                        detail.TransDate = result.data.postingDate.Value.ToString("yyyy-MM-dd");                      
                        detail.VesselCustomerCode = dim3ForFund;
                        detail.VoyageCode = voyageCode;                       
                        detail.RevenueType = businessType;
                        detail.Currency = currency;
                        if (action == "CREATE")
                        {
                            detail.LineDescription = (typeOfReceipt + " " + principalName + " received on " + result.data.dateReceived.Value.ToString("dd/MM/yyyy")).Length > 60 ? (typeOfReceipt + " " + principalName + " received on " + result.data.dateReceived.Value.ToString("dd/MM/yyyy")).Substring(0, 60) : (typeOfReceipt + " " + principalName + " received on " + result.data.dateReceived.Value.ToString("dd/MM/yyyy"));
                        }
                        else
                        {
                            detail.LineDescription = (typeOfReceipt + " " + principalName + " received on " + result.data.dateReceived.Value.ToString("dd/MM/yyyy")).Length > 56 ? (typeOfReceipt + " " + principalName + " received on " + result.data.dateReceived.Value.ToString("dd/MM/yyyy")).Substring(0, 56) + "_Rev" : (typeOfReceipt + " " + principalName + " received on " + result.data.dateReceived.Value.ToString("dd/MM/yyyy")) + "_Rev";
                        }

                        detail.ExchangeRate = exchangeRateForRund;
                        detail.SalesTaxGroup = "";
                        detail.ItemSalesTaxGroup = "";
                        detail.SalesTaxAmount = null;
                        detail.Invoice = (invoiceNumberWithSuffix.Length > 50 ? invoiceNumberWithSuffix.Substring(0, 50) : invoiceNumberWithSuffix);
                        string docNum = nominationFileNumber + " " + result.data.misc;
                        detail.Document = (docNum.Length > 50 ? docNum.Substring(0, 50) : docNum);
                        detail.DocumentDate = result.data.dateReceived.Value.ToString("yyyy-MM-dd");
                        detail.DueDate = null;
                        detail.Payment = bankReference;
                        detail.BankTransType = "";
                        detail.CreatedByUserId = Config.SystemCreatedBy;
                        detail.UserField1 = userfield1;
                        detail.UserField2 = string.Empty;
                        detail.UserField3 = string.Empty;
                        detail.Remark = string.Empty;

                        if (i == 1) //debit
                        {
                            detail.AccountType = debitAccountTypeForFCAdv;
                            detail.Account = debitBankForFCAdv;
                            detail.Amount = amountForFund - bankChargeForFund;

                        }
                        else if (i == 2 && maxCount == 3) //debit
                        {
                            detail.AccountType = "Ledger";
                            detail.Account = debitBankChargeForFCAdv;
                            detail.Amount = bankChargeForFund;
                        }
                        else //credit
                        {
                            detail.AccountType = "Customer";
                            detail.Account = principalNumber;
                            detail.Amount = amountForFund * -1;
                        }


                        journalDetail2.Add(detail);
                    }

                }

                header2.Lines = journalDetail2;
                journal2._gatshipData = header2;

                #endregion
                #region SaveJSONToFile

                LogManager.Log.Info("Creating JSON to Funds " + category + " processed folder...");
                string folder = actionEntity == "FCAdd" || actionEntity == "FMAdd" ? Config.A3Folders.FundsAdditional.ToString() : Config.A3Folders.FundsAdvance.ToString();
                string filePath = Config.ProcessedFolder + folder + "\\" + DateTime.Now.ToString("yyyyMMdd");
                string fileName = journal._gatshipData.FileBatchID + ".json";
                fileLocation = filePath + "\\" + fileName;
                Config.ValidatePath(fileLocation);

                string fileName2 = journal2._gatshipData.FileBatchID + ".json";
                fileLocation2 = filePath + "\\" + fileName2;
                Config.ValidatePath(fileLocation2);


                #endregion
                #region PostToAx
                LogManager.Log.Info("Posting to D354...");
                //string jsonRet = "{\"Message\":{\"Status\":\"\",\"Msg\":\"\",\"Submitted\":\"\"}}";
                string jsonRet;
                D365FO ax = new D365FO();               
                jsonRet = ax.PostData(journal).Result;
               
                // Parse the JSON response
                JObject responseObject = JObject.Parse(jsonRet);

                // Access the properties in the JSON object
                int status = int.Parse(responseObject["Status"].ToString());
                string returnMsg = responseObject["ReturnMsg"].ToString();

                //reading response from D365
                response = JsonConvert.DeserializeObject<JsonResponse>(jsonRet);
                success = response.Status == 1 ? true : false;
                A3Helper.Serialize(journal, fileLocation);

                #endregion

                if (success)
                {
                    LogManager.Log.Info("Status is [Success] for posting first journal to D365.");

                    LogManager.Log.Info("Posting to D365...");
                    jsonRet = "{\"Message\":{\"Status\":\"\",\"Msg\":\"\",\"Submitted\":\"\"}}";
                    ax = new D365FO();
                    jsonRet = ax.PostData(journal2).Result;

                    // Parse the JSON response
                    responseObject = JObject.Parse(jsonRet);

                    // Access the properties in the JSON object
                    status = int.Parse(responseObject["Status"].ToString());
                    returnMsg = responseObject["ReturnMsg"].ToString();

                    //reading response from D365
                    response = JsonConvert.DeserializeObject<JsonResponse>(jsonRet);
                    success = response.Status == 1 ? true : false;
                    A3Helper.Serialize(journal2, fileLocation2);


                    if (success)
                    {
                        string entityKey = "";
                        entityKey = response.ReturnMsg;

                        LogManager.Log.Info("Status is [Success] for posting second journal to AX.");

                        LogManager.Log.Info("Entity key is: " + "[" + entityKey + "]");

                        sequenceNumber = sequenceNumber + 1;
                        A3Helper.InsertToA3EventJournal(eventId, action, entity, id, jsonData, received_at, sequenceNumber, filePath, fileName + "," + fileName2, "", "", true, false, entityKey);

                        if (suffixNo == null)
                        {
                            A3Helper.InsertToA3InvoiceSuffix(eventId, principalNumber, voyageCode + " " + typeOfReceipt, 0, result);
                        }
                        else
                        {
                            A3Helper.UpdateA3InvoiceSuffix(eventId, principalNumber, voyageCode + " " + typeOfReceipt, suffixNo, result);
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
                        validationMsg = a3Helper.AppendError(returnMsg);

                        return false;
                    }
                }
                else
                {
                    validationMsg = a3Helper.AppendError(returnMsg);
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

            string category = result.data.category;
            category = category == null || category == "" ? "ADDITIONAL" : category.ToUpper();

            string nominationFileNumber = result.data.fileNumber;
            int? sequenceNumber = funds.SequenceNumber(ref validationMsg);

            string typeOfReceipt = funds.TypeOfReceiptForFund(result.data.comment, ref validationMsg);
            typeOfReceipt = typeOfReceipt != null ? typeOfReceipt.ToUpper() : typeOfReceipt;

            switch (typeOfReceipt)
            {
                case "EXGL_AR":
                    FundsExGL_AR fundsExGL_AR = new FundsExGL_AR();
                    return fundsExGL_AR.Modify(eventId, id, result, jsonData, received_at, "FMExGL_AR", category, ref validationMsg);

                case "EXGL_C":
                    FundsExGL_C fundsExGL_C = new FundsExGL_C();
                    return fundsExGL_C.Modify(eventId, id, result, jsonData, received_at, "FMExGL_C", category, ref validationMsg);

                case "BKCHG":
                    FundsBkChg fundsBkChg = new FundsBkChg();
                    return fundsBkChg.Modify(eventId, id, result, jsonData, received_at, "FMBkChg", category, ref validationMsg);

                default:
                    break;
            }

            return Create(eventId, a3_id, result, jsonData, received_at, actionEntity, ref validationMsg);
        }
    }
}
