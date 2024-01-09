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
    public class FundsBkChg
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
        string errorMsg = "";
        string warningMsg = "";

        public bool Create(int eventId, int a3_id, FundsPayload result, string jsonData, DateTime? received_at, string actionEntity, string category, ref List<string> validationMsg)
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
                //int? appointmentId = result.data.appointmentId;
                string fileNumber = result.data.fileNumber;
                string initials = result.data.initials + "@wallem.com";

                LogManager.Log.Info("Validating fields...");

                #region Validations

                string locCode = funds.A3LocationCode(nominationId, fileNumber, entityType, ref validationMsg);
                string smccode = funds.SMCCode(locCode, entityType, ref validationMsg);
                int? sequenceNumber = funds.SequenceNumber(ref validationMsg);
                string fileBatchId = funds.FileBatchId(actionEntity, sequenceNumber, smccode, ref validationMsg);
                string[] batchIdArr = fileBatchId.Split("_".ToCharArray());
                int sequenceBank = int.Parse(batchIdArr[2]) + 1;
                string fileBatchIdBank = batchIdArr[0] + "_" + batchIdArr[1] + "_" + sequenceBank.ToString().PadLeft(4, '0') + "_" + batchIdArr[3] + "_" + batchIdArr[4];
                string principalNumber = result.data.principalNumber;
                string principalName = result.data.principalName;
                string vesselName = result.data.vesselName;                
                //  string arrivalDate = funds.DateForVoyageCode(appointmentId, locCode, entityType, ref validationMsg);                
                string harbourLocCode = funds.HarbourLocCode(id, locCode, entity, ref validationMsg);
                string voyageCode = funds.VoyageCode(id, result.data.portCallId, result.data.voyageCode, vesselName, result.data.eta_date, harbourLocCode, ref validationMsg);
                int? refNo = result.data.refNumber;
                string deptForFund = funds.DeptForFund(nominationId, result.data.PIC, result.data.businessType, locCode, entityType, "Fund", result.data.chargeDept, principalNumber, refNo, ref validationMsg);
                string currency = funds.CurrencyForFund(comment, ref validationMsg);
                decimal? amountForFund = funds.AmountForFund(comment, ref validationMsg);
                string typeOfReceipt = funds.TypeOfReceiptForFund(comment, ref validationMsg);
                decimal exchangeRateForRund = funds.ExchangeRateForFund(comment, ref validationMsg);
                string nominationFileNumber = result.data.fileNumber;
                string bankReference = funds.BankReferenceNumberForFund(comment, locCode, result.data.postingDate, ref validationMsg);
                string creditBankForFCBkChg = funds.DebitBankForFCAdv(locCode, result.data.usdBankAccountNoJPN, result.data.jpyBankAccountNoJPN, comment, ref validationMsg);
                string dim6 = funds.DIM6(locCode, id, entity, ref validationMsg);
                string dim2 = funds.Dim2forFund(locCode, ref validationMsg);
                string dim5 = funds.Dim5forFund(nominationId, result.data.businessType, comment, entityType, locCode, ref validationMsg);
                string businessType = result.data.businessType;
                string userfield1 = funds.UserField1forFund(nominationId, result.data.businessType, comment, entityType, locCode, ref validationMsg);
                string debitBankChargeForFCBkChg = funds.DebitBankChargeforFCAdv(locCode, ref validationMsg);
                DateTime? dateReceived = funds.DateReceived(result.data.dateReceived, ref validationMsg);
                DateTime? postingDate = funds.PostingDate(result.data.postingDate, locCode, ref validationMsg);
                string invoiceNumberWithSuffix = funds.AddSuffix(voyageCode + " " + typeOfReceipt, principalNumber, ref tmpIsSuffixed, ref validationMsg);
                int? suffixNo = funds.InvoiceSuffixNo(0, voyageCode + " " + typeOfReceipt, principalNumber, ref validationMsg);
                string invoice = funds.InvoiceForFC(locCode, voyageCode, typeOfReceipt, bankReference, ref validationMsg);

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

                #region Customer
                LogManager.Log.Info("Create journal for Customer...");
                D365FOJournal journal = new D365FOJournal();
                D365FOHeader header = new D365FOHeader();
                header.CompanyCode = smccode;
                header.TransType = "FCBkChg";
                var desc = actionEntity + " " + voyageCode + " " + principalNumber;

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
                    // detail.DIM3 = principalNumber;
                    detail.VesselCustomerCode = principalNumber;
                    // detail.DIM4 = voyageCode;
                    detail.VoyageCode = voyageCode;
                    // detail.DIM5 = dim5;
                    // detail.DIM6 = dim6;
                    detail.RevenueType = businessType;
                    detail.Currency = currency;
                    if (action == "CREATE")
                    {
                        detail.LineDescription = (principalName + " Bank charge on " + result.data.dateReceived.Value.ToString("dd/MM/yyyy")).Length > 60 ? (principalName + " Bank charge on " + result.data.dateReceived.Value.ToString("dd/MM/yyyy")).Substring(0, 60) : (principalNumber + " Bank charge on " + result.data.dateReceived.Value.ToString("dd/MM/yyyy"));
                    }
                    else
                    {
                        detail.LineDescription = (principalName + " Bank charge on " + result.data.dateReceived.Value.ToString("dd/MM/yyyy")).Length > 56 ? (principalName + " Bank charge on " + result.data.dateReceived.Value.ToString("dd/MM/yyyy")).Substring(0, 56) + "_Rev" : (principalNumber + " Bank charge on " + result.data.dateReceived.Value.ToString("dd/MM/yyyy")) + "_Rev";
                    }
                    detail.ExchangeRate = exchangeRateForRund;
                    detail.SalesTaxGroup = "";
                    detail.ItemSalesTaxGroup = "";
                    detail.SalesTaxAmount = null;
                    detail.Invoice = invoice;
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
                        detail.AccountType = "Ledger";
                        detail.Account = debitBankChargeForFCBkChg;
                        detail.Amount = amountForFund;

                    }
                    else //credit
                    {
                        detail.AccountType = "Bank";
                        detail.Account = creditBankForFCBkChg;
                        detail.Amount = amountForFund * -1;

                    }

                    journalDetail.Add(detail);
                }

                header.Lines = journalDetail;
                journal._gatshipData = header;

                #endregion

                #region SaveJSONToFile

                LogManager.Log.Info("Creating JSON to Funds " + category + " processed folder...");
                string folder = Config.ProcessedFolder + Config.A3Folders.FundsBkChg.ToString();
                string filePath = folder + "\\" + DateTime.Now.ToString("yyyyMMdd");
                string fileName = journal._gatshipData.FileBatchID + ".json";
                fileLocation = filePath + "\\" + fileName;
                Config.ValidatePath(fileLocation);

                #endregion
                #region PostToAx
                LogManager.Log.Info("Posting to D365...");
                string jsonRet = "{\"Message\":{\"Status\":\"\",\"Msg\":\"\",\"Submitted\":\"\"}}";
                D365FO ax = new D365FO();
                jsonRet = ax.PostData(journal).Result;


                //reading response from D365
                response = JsonConvert.DeserializeObject<JsonResponse>(jsonRet);
                success = response.Status == 1 ? true : false;
                A3Helper.Serialize(journal, fileLocation);
                #endregion               

                if (success)
                {
                    string entityKey = "";
                    entityKey = response.ReturnMsg;

                    LogManager.Log.Info("Status is [Success] for posting second journal to AX.");

                    LogManager.Log.Info("Entity key is: " + "[" + entityKey + "]");

                    sequenceNumber = sequenceNumber + 1;
                    A3Helper.InsertToA3EventJournal(eventId, action, entity, id, jsonData, received_at, sequenceNumber, filePath, fileName, "", "", true, false, entityKey);

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

        public bool Modify(int eventId, int a3_id, FundsPayload result, string jsonData, DateTime? received_at, string actionEntity, string category, ref List<string> validationMsg)
        {
            int? nominationId = result.data.nominationId;
            bool retVal = false;
            action = result.action.ToUpper();
            entity = result.entity;
            id = a3_id;

            string nominationFileNumber = result.data.fileNumber;
            int? sequenceNumber = funds.SequenceNumber(ref validationMsg);
            string initials = result.data.initials + "@wallem.com";

            var eventJournal = a3Helper.EventJournal();
            var previousJournal = (from seq in eventJournal
                                   where seq.a3_id == result.data.id && seq.id < eventId && seq.entity.ToUpper() == entity.ToUpper()
                                   select seq).OrderByDescending(x => x.id);

            return Create(eventId, a3_id, result, jsonData, received_at, "FMBkChg", category, ref validationMsg);
        }

    }
}

