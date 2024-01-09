//
using GATShipD365TS.App_Code;
using GATShipD365TS.Helper;
using GATShipD365TS.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GATShipD365TS.GATShip.Entities
{
    public class DA
    {
        A3Helper a3Helper = new A3Helper();
        JsonResponse response;
        string fileLocation = "";
        bool success = false;
        EntityHelper DaComplete = new EntityHelper();
        bool tmpIsSuffixedDa;
        string entityType = "NOMID";
        const char doubleqoute = '"';
        const char singlequote = '\'';
        string action;
        string entity;
        int id = 0;
        string errorMsg = "";
        public bool Modify(int eventId, int a3_id, DAPayload result, string jsonData, DateTime? received_at, string actionEntity, ref List<string> validationMsg)
        {
            LogManager.Log.Info("Start processing Event Id: " + eventId);
            try
            {
                action = result.action.ToUpper();
                entity = result.entity;
                id = a3_id;
                int? nominationId = result.data.nominationId;
                StringBuilder msgSb = new StringBuilder();
                string warningMsg = "";
                string errorMsg = "";

                string nominationFileNumber = result.data.fileNumber;

                #region VALIDATIONS                           
                string locCode = DaComplete.A3LocationCode(nominationId, result.data.fileNumber, entityType, ref validationMsg);
                string smccode = DaComplete.SMCCode(locCode, entityType, ref validationMsg);
                string curr = result.data.principalCurrency;
                decimal? dmcomExchangeRate = result.data.principalRate;
                string NominationType = result.data.nominationType;
                decimal? totalAmount = result.data.totalAmount;
                decimal? AmountForDMCom = DaComplete.AmountForDMCom(nominationId, totalAmount, NominationType, ref validationMsg);
                int? sequenceNumber = DaComplete.SequenceNumber(ref validationMsg);
                string fileBatchId = DaComplete.FileBatchId(actionEntity, sequenceNumber, smccode, ref validationMsg);
                string vesselName = result.data.vesselName;
                DateTime? arrivalDate = result.data.arrivalDate;
                DateTime? DepartureDate = result.data.departureDate;
                string PrincipalNumber = result.data.principalNumber;
                string principalName = result.data.principalName;
                string PIC = result.data.PIC;                
                string businessType = result.data.businessType;
                string DeptforInvoice = DaComplete.DeptForInvoice(nominationId, PIC, businessType, locCode, entityType, "Invoice", "", null, PrincipalNumber, "", "", ref validationMsg);
                string harbourLocCode = DaComplete.HarbourLocCode(id, locCode, entity, ref validationMsg);
                string voyageCode = DaComplete.VoyageCode(id, result.data.portCallId, result.data.voyageCode, vesselName, result.data.eta_date, harbourLocCode, ref validationMsg);
                string tmpKey = voyageCode + DaComplete.NomTypeCode(NominationType, ref validationMsg);
                string appointmentVesselController = result.data.portCallOperator;
                string suffix = DaComplete.InvoiceWithSuffixForDAComplete(nominationId, appointmentVesselController, vesselName, nominationFileNumber, PrincipalNumber, locCode, entityType, ref tmpKey, voyageCode, ref tmpIsSuffixedDa, ref validationMsg);
                int? SuffixNo = DaComplete.SuffixNo(nominationId, locCode, entityType, tmpKey, ref validationMsg);
                string CompletedAt = result.data.completeAt != null ? result.data.completeAt.Value.ToString("yyyy-MM-dd") : null;
                string creditAccount = DaComplete.CreditAccountForDMCom(locCode, ref validationMsg);
                string dimension2 = DaComplete.Dim2forFund(locCode, ref validationMsg);
                string dimension6 = DaComplete.DIM6(locCode, id, entity, ref validationMsg);
                decimal? negate = AmountForDMCom == 0 ? AmountForDMCom : AmountForDMCom * -1;

                string description = DaComplete.DMComLineDescription(result.data, CompletedAt, ref validationMsg);
                string dim3ForDMCom = DaComplete.Dim3ForDMCom(PrincipalNumber, ref validationMsg);
                LogManager.Log.Info("Action Result... " + result.action);
                if (result.data.completeAt == null && result.action.ToUpper() == "MODIFY")
                {
                    validationMsg.Add("0083:: This DA is a MODIFY action having Complete At date is null. <br>");
                    LogManager.Log.Info("Exception occur in getting completeAt: 0083:: This DA is a MODIFY action having Complete At date is null.");
                }

                if (result.data.arrivalDate < DateTime.Parse("2020-01-01") || result.data.arrivalDate == null)
                {
                    validationMsg.Add("0063:: The Arrival Date is null or earlier than January 01, 2020. <br>");
                    LogManager.Log.Info("Exception occur in getting completeAt: 0063:: The Arrival Date is null or earlier than January 01, 2020.");
                }

                if (result.data.departureDate < DateTime.Parse("2020-01-01") || result.data.arrivalDate == null)
                {
                   validationMsg.Add("0064:: The Departure Date is null or earlier than January 01, 2020. <br>");
                    LogManager.Log.Info("0064:: The Departure Date is null or earlier than January 01, 2020.");
                }


                //validate errors                
                if (a3Helper.ValidateErrors(eventId, a3_id, result, nominationFileNumber, jsonData, received_at, "", ref validationMsg))
                {
                    return true;
                }

                if (Config.bypassValidation == false && validationMsg.Count() > 0)
                {
                    return false;
                }

                if (locCode == "HKG")
                {
                   
                    if (AmountForDMCom == 0 && NominationType.ToUpper() == "LUMPSUM BUNKERING")
                    {
                        A3Helper.InsertToA3EventJournal(eventId, action, entity, id, jsonData, received_at, 0, "", "", "", "", true, true, "Amount is equal to (=) zero(0).");
                        return true;
                    }
                    else if (AmountForDMCom == 0 && NominationType.ToUpper() != "LUMPSUM BUNKERING")
                    {
                        //errorMsg = "Amount is equal to(=) zero(0).";
                        //LogManager.Log.Info(errorMsg);
                        //validationMsg = a3Helper.AppendError("0043:: " + errorMsg + "<br>");
                        //return false;                       
                        warningMsg = a3Helper.FormatWarningMsg(eventId, nominationFileNumber, received_at, "", result, jsonData);
                        warningMsg = warningMsg.Replace("{warning}", "Amount is equal to(=) zero(0)." + " <br> ");

                        LogManager.Log.Warn(warningMsg);
                        A3Helper.SendMail(Config.fromWarn, Config.toWarn, Config.ccWarn, Config.bccWarn, Config.subjectWarn, warningMsg + "<br>" + Config.footerWarn);
                        A3Helper.InsertToA3EventJournal(eventId, action, entity, id, jsonData, received_at, 0, "", "", "", "", true, true, "Amount is equal to (=) zero(0).");
                        return true;
                    }
                }
                else
                {
                    if(AmountForDMCom == 0)
                    {
                        A3Helper.InsertToA3EventJournal(eventId, action, entity, id, jsonData, received_at, 0, "", "", "", "", true, true, "Amount is equal to (=) zero(0).");
                        return true;
                    }
                }


                #endregion
                #region SA
                LogManager.Log.Info("Create journal for Bank...");
                D365FOJournal journal = new D365FOJournal();
                D365FOHeader header = new D365FOHeader();
                header.CompanyCode = smccode;
                header.TransType = "DMCom";
                // header.JournalName = "SA";
                header.Description = (actionEntity + " " + description).Length > 60 ? (actionEntity + " " + description).Substring(0, 60) : (actionEntity + " " + description);
                header.FileBatchID = fileBatchId;

                List<D365FODetail> journalDetail = new List<D365FODetail>();

                for (var i = 1; i <= 2; i++)
                {
                    D365FODetail detail = new D365FODetail();
                    detail.FileBatchID = fileBatchId;
                    detail.TransDate = result.data.completeAt.Value.ToString("yyyy-MM-dd");
                    detail.AccountType = i == 1 ? "Customer" : "Ledger";
                    detail.Account = i == 1 ? PrincipalNumber : creditAccount;                   
                    detail.VesselCustomerCode = dim3ForDMCom;
                    detail.VoyageCode = voyageCode;                    
                    detail.RevenueType = businessType;
                    detail.Currency = curr;
                    detail.Amount = i == 1 ? AmountForDMCom : negate;
                    detail.LineDescription = description.Length > 60 ? description.Substring(0, 60) : description;
                    //  detail.ExchangeRateType = "";
                    detail.ExchangeRate = result.data.principalRate;
                    detail.SalesTaxGroup = "";
                    detail.ItemSalesTaxGroup = "";
                    detail.SalesTaxAmount = null;
                    detail.Invoice = suffix.Length > 48 ? suffix.Substring(0, 48) : suffix.ToString();
                    detail.Document = nominationFileNumber;
                    detail.DocumentDate = result.data.completeAt.Value.ToString("yyyy-MM-dd");
                    detail.DueDate = null;
                    detail.Payment = "";
                    detail.BankTransType = "";
                    detail.CreatedByUserId = "WME";
                    detail.UserField1 = PIC;
                    detail.UserField2 = "";
                    detail.UserField3 = "";
                    detail.Remark = "";
                    journalDetail.Add(detail);
                }

                header.Lines = journalDetail;
                journal._gatshipData = header;

                #endregion

                bool rev = false; ;
                string folder = Config.ProcessedFolder + Config.A3Folders.DAComplete.ToString();
                string filePath = folder + "\\" + DateTime.Now.ToString("yyyyMMdd");
                string fileName = journal._gatshipData.FileBatchID + ".json";
                fileLocation = filePath + "\\" + fileName;

                LogManager.Log.Info("Creating JSON to DA Complete processed folder...");
                Config.ValidatePath(fileLocation);

                LogManager.Log.Info("Posting to D365...");

                string jsonRet = "{\"Message\":{\"Status\":\"\",\"Msg\":\"\",\"Submitted\":\"\"}}";
                D365FO data = new D365FO();
                jsonRet = data.PostData(journal).Result;
              
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

                    A3Helper.InsertToA3EventJournal(eventId, action, entity, id, jsonData, received_at, sequenceNumber, filePath, fileName, "", "", true, rev, entityKey);

                    A3Helper.UpdateDynaValues(result.data.DCN_ID, suffix);

                    if (SuffixNo == null)
                    {
                        A3Helper.InsertToA3DAComInvoiceSuffix(eventId, tmpKey, 0, locCode, result);
                    }
                    else
                    {
                        A3Helper.UpdateA3DAComInvoiceSuffix(eventId, tmpKey, SuffixNo, locCode, result);
                    }

                    if (tmpIsSuffixedDa && Config.isNotifySuffix)
                    {
                        warningMsg = a3Helper.FormatWarningMsg(eventId, nominationFileNumber, received_at, "", result, jsonData);
                        warningMsg = warningMsg.Replace("{warning}", "Suffixed Invoice Number is " + suffix.ToString() + "." + "<br>");

                        LogManager.Log.Warn(warningMsg);
                        A3Helper.SendMail(Config.fromWarn, Config.toWarn, Config.ccWarn, Config.bccWarn, Config.subjectWarn, warningMsg + "<br>" + Config.footerWarn);
                    }
                }
                else
                {
                    LogManager.Log.Error(response.Status + ": " + response.ReturnMsg);

                }

                return success;
            }
            catch (Exception ex)
            {               
                LogManager.Log.Info("Processing Error:" + ex.Message);
                LogManager.Log.Error("Processing Error:" + ex.Message);
                LogManager.Log.Info("Processing End...");
                throw;
            }
        }


        public bool Reverse(int eventId, int a3_id, DAPayload result, string jsonData, DateTime? received_at, string actionEntity, ref List<string> validationMsg)
        {
            string warningMsg = "";
            string nominationFileNumber = result.data.fileNumber;
            StringBuilder msgSb = new StringBuilder();

            if (result.data.totalAmount == 0)
            {
                warningMsg = a3Helper.FormatWarningMsg(eventId, nominationFileNumber, received_at, "", result, jsonData);
                warningMsg = warningMsg.Replace("{warning}", "Total Amount is equal to(=) zero(0)." + " <br> ");

                LogManager.Log.Warn(warningMsg);
                A3Helper.SendMail(Config.fromWarn, Config.toWarn, Config.ccWarn, Config.bccWarn, Config.subjectWarn, warningMsg + "<br>" + Config.footerWarn);
                A3Helper.InsertToA3EventJournal(eventId, action, entity, id, jsonData, received_at, 0, "", "", "", "", true, true, "Total Amount is equal to (=) zero(0).");
                return true;
            }
            else
            {
                 //int daId = result.data.daId;
                int? daId = result.data.id;
                action = result.action.ToUpper();
                entity = result.entity;
                id = a3_id;

                warningMsg = "";
             
                var eventJournal = a3Helper.EventJournal();
                var previousJournal = (from seq in eventJournal
                                       where seq.a3_id == result.data.id && seq.id < eventId && seq.entity.ToUpper() == entity.ToUpper()
                                       select seq).OrderByDescending(x => x.id);

                if (previousJournal.Count() == 0)
                {
                    errorMsg = "0078:: Previous journal for event Id " + eventId + " is not found in the Event Journal table";
                    warningMsg = a3Helper.FormatWarningMsg(eventId, nominationFileNumber, received_at, "", result, jsonData);
                    warningMsg = warningMsg.Replace("{warning}", errorMsg + " <br> ");

                    LogManager.Log.Warn(warningMsg);
                    A3Helper.SendMail(Config.fromWarn, Config.toWarn, Config.ccWarn, Config.bccWarn, Config.subjectWarn, warningMsg + "<br>" + Config.footerWarn);

                    A3Helper.InsertToA3EventJournal(eventId, action, entity, id, jsonData, received_at, 0, "", "", "", "", true, true, errorMsg);
                    return true;
                }
                else
                {
                    if (previousJournal.FirstOrDefault().isReversed == true)
                    {
                        LogManager.Log.Info("Event ID: " + eventId + "(" + result.entity + "): A3 Event Entity was already reversed.");
                        A3Helper.InsertToA3EventJournal(eventId, action, entity, id, jsonData, received_at, 0, "", "", "", "", true, true, "A3 Event Entity was already reversed.");
                        return true;
                    }
                    else
                    {
                        var fileLoc = previousJournal.FirstOrDefault().JrnlFilePath + "\\" + previousJournal.FirstOrDefault().JrnlFileName;
                        if (File.Exists(fileLoc))
                        {
                            D365FOJournal journal = a3Helper.Deserialize(fileLoc);
                            D365FOHeader Header = journal._gatshipData;

                            List<D365FODetail> detail = Header.Lines;
                            Header.FileBatchID = Header.FileBatchID + "_R";
                            Header.Description = Header.Description.Length > 56 ? Header.Description.Substring(0, 56) + "_Rev" : Header.Description + "_Rev";

                            var counter = 0;
                            foreach (var item in detail)
                            {
                                counter++;
                                item.FileBatchID = Header.FileBatchID;
                                item.LineDescription = item.LineDescription.Length > 56 ? item.LineDescription.Substring(0, 56) + "_Rev" : item.LineDescription + "_Rev";
                                //item.Amount = item.Amount * -1;
                                if (counter == 1) //debit
                                {
                                    item.Amount = result.data.totalAmount;
                                }
                                else //credit
                                {
                                    item.Amount = result.data.totalAmount * -1;
                                }
                                //item.SalesTaxAmount = item.SalesTaxAmount != null ? (item.SalesTaxAmount * -1) : null;
                                item.Invoice = item.Invoice.Length > 46 ? item.Invoice.Substring(0, 46) + "_Rev" : item.Invoice + "_Rev";
                                item.UserField2 = detail[0].Amount < 0 ? "SpecialReversal" : string.Empty;
                                item.TransDate = result.data.completeAt.Value.ToString("yyyy-MM-dd");
                            }

                            string folder = Config.ProcessedFolder + Config.A3Folders.DAComplete.ToString();
                            string filePath = folder + "\\" + DateTime.Now.ToString("yyyyMMdd");
                            string fileName = Header.FileBatchID + ".json";
                            string fileLocation = filePath + "\\" + fileName;
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
                                A3Helper.UpdateA3EventJournal(eventId, result, filePath, fileName);
                                A3Helper.InsertToA3EventJournal(eventId, action, entity, id, jsonData, received_at, 0, "", "", "", "", true, false, entityKey);
                            }
                            else
                            {
                                validationMsg = a3Helper.AppendError(response.ReturnMsg);
                                return false;
                            }

                            return true;
                        }
                        else
                        {
                            LogManager.Log.Info("Cannot locate file " + previousJournal.FirstOrDefault().JrnlFileName + " in " + previousJournal.FirstOrDefault().JrnlFilePath + " for Event Id " + eventId);
                            validationMsg = a3Helper.AppendError("0079:: Cannot locate file " + previousJournal.FirstOrDefault().JrnlFileName + " in " + previousJournal.FirstOrDefault().JrnlFilePath + " for Event Id " + eventId);
                            return false;
                        }
                    }
                }
            }
        }

    }
}
