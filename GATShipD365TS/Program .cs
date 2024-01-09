using GATShipD365TS.App_Code;
using GATShipD365TS.GATShip.Entities;
using GATShipD365TS.Helper;
using GATShipD365TS.Models;
using log4net.Repository.Hierarchy;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GATShipD365TS
{
    class Program
    {
        static void Main(string[] args)
        {
            string jsonData = "";

            int eventId = eventId = Config.id;
            string entity = "";
            string action = "";
            int a3_id = 0;
            int? nomDaId = 0;
            string NomFileNumber = "";
            string InvoiceNumber = "";
            string category = "";
            string typeOfID = "";
            DateTime? received_at = DateTime.Now;
            WIS_Sync context = new WIS_Sync();
            GSWallem contextgswallem = new GSWallem();
            EntityHelper entityHelper = new EntityHelper();
            A3Helper a3Helper = new A3Helper();
            Registry registry = new Registry();
            List<string> validationMsg = new List<string>();
            // string payload = "";
            string errorMsg = "";
            string supplierName = "";
            IEnumerable<string> uniqueErrors = null;
            string initials = "";

            Config.InitConfig();
            TS_App.Start();

            LogManager.Log.Info("Start...validationMsg..." + validationMsg.Count());

            foreach (var item in validationMsg)
            {
                LogManager.Log.Info("Looping validationMsg..." + item);
            }

            try
            {


                LogManager.Log.Info("Processing Start...");
                LogManager.Log.Info("Processing Location Code..." + Config.LocationCode);

                string systemMode = A3Helper.GetSystemMode();

                if (systemMode.Trim().ToUpper() == "RUNNING")
                {
                    A3Helper.UpdateWMEStatus("Locked", " ");

                    int lastRecordId = 0;
                    int lastRecordId2 = 0;
                    int nofQ = int.Parse(Config.numberOfRecordsInEachQuery);
                    int nofMOld = int.Parse(Config.numberOfMinutesOldOfEvent);
                    var eventJournal = (from c in context.a3EventJournals
                                        where c.a3LocCode == Config.LocationCode
                                        select c.id);
                    int countEventJournal = eventJournal.Count();
                    LogManager.Log.Info("There are " + countEventJournal + " of record(s) in a3EventJournals for Location Code: " + Config.LocationCode);
                    if (countEventJournal == 0)
                    {
                        lastRecordId = 0;
                        eventJournal = (from c in context.a3EventJournals
                                        select c.id);

                        lastRecordId2 = eventJournal.Max();

                    }
                    else
                    {
                        lastRecordId = eventJournal.Max();
                        lastRecordId2 = eventJournal.Max();
                    }
                    LogManager.Log.Info("Last record is: " + lastRecordId);
                    LogManager.Log.Info("Fetching the records in a3EventStage...");

#if !DEBUG
                            //var a3Events = context.a3EventStage
                            //   .Where(p => p.a3LocCode == Config.LocationCode && (p.id > lastRecordId && p.id <= (lastRecordId2 + nofQ)) && DbFunctions.DiffMinutes(p.received_at, DateTime.Now) > nofMOld)
                            //   .OrderBy(p => p.id)
                            //   .Select(p => p)
                            //   .ToList();

                            var a3Events = context.a3EventStage
                              .Where(p => p.a3LocCode == Config.LocationCode && (p.id > lastRecordId) && DbFunctions.DiffMinutes(p.received_at, DateTime.Now) > nofMOld)
                              .OrderBy(p => p.id)
                              .Select(p => p)
                              .ToList();
#endif
#if DEBUG
                    var a3Events = context.a3EventStage.Where(p => p.id == Config.id && p.a3LocCode == Config.LocationCode);
                    //var a3Events = context.a3EventStage
                    //   .Where(p => p.a3LocCode == Config.LocationCode && (p.id > lastRecordId && p.id <= (lastRecordId2 + nofQ)) && DbFunctions.DiffMinutes(p.received_at, DateTime.Now) > nofMOld)
                    //   .OrderBy(p => p.id)
                    //   .Select(p => p)
                    //   .ToList();
#endif

                    int recordCnt = 0;
                    recordCnt = a3Events.Count();
                    LogManager.Log.Info("There are " + recordCnt + " of record(s) to be processed from a3EventStage for " + Config.LocationCode);
                    foreach (var item in a3Events)
                    {
                        initials = "";
                        eventId = item.id;
                        entity = item.entity.Replace("a3_table_", "");
                        action = item.action;
                        a3_id = item.a3_id;
                        jsonData = Regex.Replace(item.data, @"[\u0000-\u0008\u000A-\u001F\u0100-\uFFFF]", "");       //replaces special characters so that it will be properly posted to AX with the correct length                  
                        received_at = item.received_at;
                        LogManager.Log.Info("------------------------------------------------------------------------");
                        LogManager.Log.Info("Processing Event Id:" + eventId + ", Entity:" + entity + ", Action:" + action);
                        LogManager.Log.Info("Processing GetPayload: " + jsonData);
                        //bool getPayload = GetPayload(ref eventId, ref entity, ref action, ref received_at, ref jsonData, ref errorMessage);

                        FundsPayload fundsResult = new FundsPayload();
                        InvoicePayload invoiceResult = new InvoicePayload();
                        DAPayload daResult = new DAPayload();
                        entity = entity.ToUpper();


                        var events = Config.ListOfEvents.Split(',');
                        string foundEntityInList = events.FirstOrDefault(x => x == entity);
                        LogManager.Log.Info("Inside For Each...validationMsg..." + validationMsg.Count());
                        if (foundEntityInList != null)
                        {
                            StringBuilder msgSb = new StringBuilder();
                            string dtlWarn = "";

                            if (entity == "FUND")
                            {
                                fundsResult = JsonConvert.DeserializeObject<FundsPayload>(jsonData);
                                nomDaId = fundsResult.data.nominationId;
                                category = fundsResult.data.category;
                                typeOfID = "NOMID";
                                NomFileNumber = fundsResult.data.fileNumber;
                                initials = fundsResult.data.initials;

                                var nomination = fundsResult.data.nominationId;

                                if (nomination == null || nomination == 0)
                                {
                                    msgSb = new StringBuilder();

                                    dtlWarn = "Event Id: " + eventId + "<br>Entity: " + entity + "<br>Action: " + fundsResult.action + "<br>Category: " + category + "<br><br>";
                                    errorMsg = "NominationId is null or 0." + " <br> ";

                                    msgSb.Append(Config.headerWarn);
                                    msgSb.Append(dtlWarn);
                                    msgSb.Append("0088:: " + errorMsg);
                                    msgSb.Append(Config.footerWarn);

                                    LogManager.Log.Warn(msgSb.ToString());
                                    A3Helper.SendMail(Config.fromWarn, Config.toWarn + "," + initials, Config.ccWarn, Config.bccWarn, Config.subjectWarn, msgSb.ToString());
                                    A3Helper.InsertToA3EventJournal(eventId, action, entity, a3_id, jsonData, received_at, 0, "", "", "", "", true, true, "No nomination record found in A3TableNomination.");
                                    continue;
                                }
                            }
                            else if (entity == "DA")
                            {
                                daResult = JsonConvert.DeserializeObject<DAPayload>(jsonData);
                                nomDaId = daResult.data.nominationId;
                                typeOfID = "NOMID";
                                NomFileNumber = daResult.data.fileNumber;

                                if (daResult.data.completeAt == null && daResult.action.ToUpper() != "MODIFY")
                                {
                                    validationMsg = a3Helper.AppendError("0083:: This DA is a REVERSE action having Complete At date is null." + "<br>");
                                    uniqueErrors = validationMsg.Distinct<string>();

                                    errorMsg = a3Helper.FormatErrorMsg(eventId, NomFileNumber, received_at, supplierName, daResult, jsonData, false);

                                    throw new Exception(errorMsg);
                                }

                                //if (daResult.data.arrivalDate < DateTime.Parse("2020-01-01") || daResult.data.arrivalDate == null)
                                //{
                                //    validationMsg = a3Helper.AppendError("0063:: The Arrival Date is null or earlier than January 01, 2020." + "<br>");
                                //    LogManager.Log.Info("Exception occur in getting completeAt: 0063:: The Arrival Date is null or earlier than January 01, 2020.");
                                //    //uniqueErrors = validationMsg.Distinct<string>();

                                //    //errorMsg = a3Helper.FormatErrorMsg(eventId, NomFileNumber, received_at, supplierName, daResult, jsonData, false);

                                //    //throw new Exception(errorMsg);
                                //}

                                //if (daResult.data.departureDate < DateTime.Parse("2020-01-01") || daResult.data.arrivalDate == null)
                                //{
                                //    validationMsg = a3Helper.AppendError("0064:: The Departure Date is null or earlier than January 01, 2020." + "<br>");
                                //    LogManager.Log.Info("0064:: The Departure Date is null or earlier than January 01, 2020.");
                                //    //uniqueErrors = validationMsg.Distinct<string>();

                                //    //errorMsg = a3Helper.FormatErrorMsg(eventId, NomFileNumber, received_at, supplierName, daResult, jsonData, false);

                                //    //throw new Exception(errorMsg);
                                //}

                                var nomination = daResult.data.nominationId;

                                if (nomination == null || nomination == 0)
                                {
                                    msgSb = new StringBuilder();

                                    dtlWarn = "Event Id: " + eventId + "<br>Entity: " + entity + "<br>Action: " + daResult.action + "<br><br>";
                                    errorMsg = "No nomination record found in A3TableNomination." + " <br> ";

                                    msgSb.Append(Config.headerWarn);
                                    msgSb.Append(dtlWarn);
                                    msgSb.Append("0088:: " + errorMsg);
                                    msgSb.Append(Config.footerWarn);

                                    LogManager.Log.Warn(msgSb.ToString());
                                    A3Helper.SendMail(Config.fromWarn, Config.toWarn + "," + initials, Config.ccWarn, Config.bccWarn, Config.subjectWarn, msgSb.ToString());
                                    A3Helper.InsertToA3EventJournal(eventId, action, entity, a3_id, jsonData, received_at, 0, "", "", "", "", true, true, "No nomination record found in A3TableNomination.");
                                    continue;
                                }

                            }
                            else
                            {
                                invoiceResult = JsonConvert.DeserializeObject<InvoicePayload>(jsonData);
                                nomDaId = invoiceResult.data.nominationId;

                                if (nomDaId == null || nomDaId == 0)
                                {
                                    msgSb = new StringBuilder();

                                    dtlWarn = "Event Id: " + eventId + "<br>Entity: " + entity + "<br>Action: " + fundsResult.action + "<br><br>";
                                    errorMsg = "NominationId is null or 0." + " <br> ";

                                    msgSb.Append(Config.headerWarn);
                                    msgSb.Append(dtlWarn);
                                    msgSb.Append("0088:: " + errorMsg);
                                    msgSb.Append(Config.footerWarn);

                                    LogManager.Log.Warn(msgSb.ToString());
                                    A3Helper.SendMail(Config.fromWarn, Config.toWarn + "," + initials, Config.ccWarn, Config.bccWarn, Config.subjectWarn, msgSb.ToString());
                                    A3Helper.InsertToA3EventJournal(eventId, action, entity, a3_id, jsonData, received_at, 0, "", "", "", "", true, true, "No nomination record found in A3TableNomination.");
                                    continue;
                                }

                                typeOfID = "DAID";
                                entity = "DAINVOICE";
                                NomFileNumber = invoiceResult.data.fileNumber;
                                initials = invoiceResult.data.initials;

                                if (action == "CREATE")
                                {
                                    supplierName = invoiceResult.data.vendorName;

                                    if (invoiceResult.data.cancelledAt != null)
                                    {
                                        validationMsg = a3Helper.AppendError("0084:: The INVOICE is a CREATE action having Cancelled At date is NOT null." + "<br>");
                                        uniqueErrors = validationMsg.Distinct<string>();

                                        errorMsg = a3Helper.FormatErrorMsg(eventId, NomFileNumber, received_at, supplierName, invoiceResult, jsonData, false);

                                        throw new Exception(errorMsg);
                                    }
                                }
                                else
                                {
                                    if (invoiceResult.data.cancelledAt == null)
                                    {
                                        validationMsg = a3Helper.AppendError("0085:: This INVOICE is a MODIFY action having Cancelled At date is null." + "<br>");
                                        uniqueErrors = validationMsg.Distinct<string>();

                                        errorMsg = a3Helper.FormatErrorMsg(eventId, NomFileNumber, received_at, supplierName, invoiceResult, jsonData, false);

                                        throw new Exception(errorMsg);
                                    }
                                }

                                string reference = invoiceResult.data.reference;
                                InvoiceNumber = entityHelper.InvoiceNumber(reference, ref validationMsg);
                                LogManager.Log.Info("InvoiceNumber...validationMsg..." + validationMsg.Count());
                            }

                            LogManager.Log.Info("Type of ID: " + typeOfID == "NOMID" ? "Nomination ID" : "DA ID");

                            bool retVal = false;
                            GATShip.Entities.FundsAddAdv fundAddAdv = new GATShip.Entities.FundsAddAdv();
                            GATShip.Entities.FundsFinal fundFin = new GATShip.Entities.FundsFinal();
                            GATShip.Entities.InvoiceInvoice invoiceInv = new GATShip.Entities.InvoiceInvoice();
                            GATShip.Entities.InvoiceCost invoiceInvCost = new GATShip.Entities.InvoiceCost();
                            GATShip.Entities.InvoiceVNCost invoiceInvVNCost = new GATShip.Entities.InvoiceVNCost();
                            GATShip.Entities.InvoiceVTCost invoiceInvVTCost = new GATShip.Entities.InvoiceVTCost();
                            GATShip.Entities.InvoiceTH invoiceInvTH = new GATShip.Entities.InvoiceTH();
                            GATShip.Entities.InvoiceJP invoiceInvJP = new GATShip.Entities.InvoiceJP();
                            GATShip.Entities.InvoiceFPE_CNCO invoiceICFPE = new GATShip.Entities.InvoiceFPE_CNCO();
                            GATShip.Entities.InvoiceTramp_C invoiceICTramp = new GATShip.Entities.InvoiceTramp_C();
                            GATShip.Entities.DA da = new GATShip.Entities.DA();
                            GATShip.Entities.InvoiceDebitNote invoiceDNt = new GATShip.Entities.InvoiceDebitNote();
                            GATShip.Entities.InvoiceDebitNoteMisc invoiceDNtMisc = new GATShip.Entities.InvoiceDebitNoteMisc();
                            GATShip.Entities.InvoiceDebitNote_AR invoiceDNt_AR = new GATShip.Entities.InvoiceDebitNote_AR();
                            GATShip.Entities.InvoiceDebitNoteHoegh_FRT invoiceDNtHoegh_FRT = new GATShip.Entities.InvoiceDebitNoteHoegh_FRT();
                            GATShip.Entities.InvoiceDebitNoteJP invoiceDNJP = new GATShip.Entities.InvoiceDebitNoteJP();
                            GATShip.Entities.InvoiceDebitNoteTH invoiceDNTH = new GATShip.Entities.InvoiceDebitNoteTH();
                            GATShip.Entities.InvoiceDebitNoteVN invoiceDNVN = new GATShip.Entities.InvoiceDebitNoteVN();
                            GATShip.Entities.InvoiceDebitNoteVT invoiceDNVT = new GATShip.Entities.InvoiceDebitNoteVT();


                            var locCode = entityHelper.A3LocationCode(nomDaId, NomFileNumber, typeOfID, ref validationMsg);
                            LogManager.Log.Info("A3LocationCode...validationMsg..." + validationMsg.Count());
                            bool isGenerateJournal = entityHelper.IsGenJrnl(locCode, ref validationMsg);

                            if (isGenerateJournal && locCode != "" && locCode != null)
                            {
                                switch (entity)
                                {
                                    case "FUND":
                                        action = fundsResult.action.ToUpper();
                                        entityHelper.ValidateTansAndDocumentDate(entity, fundsResult.data.postingDate, fundsResult.data.dateReceived, ref validationMsg);

                                        if (category == null)
                                        {
                                            category = "ADDITIONAL";
                                        }
                                        else
                                        {
                                            category = category.ToUpper();
                                            if (category == "ADDITIONAL" || category == "ADVANCE" || category == "FINAL" || category == "")
                                            {

                                                category = category == "" ? "ADDITIONAL" : category;
                                            }
                                            else
                                            {
                                                validationMsg = a3Helper.AppendError("0086:: Fund's category not found. The category is " + category + " <br>");
                                                uniqueErrors = validationMsg.Distinct<string>();

                                                var vendorName = fundsResult.data.vendorName;
                                                errorMsg = a3Helper.FormatErrorMsg(eventId, NomFileNumber, received_at, vendorName, fundsResult, jsonData, false);

                                                throw new Exception(errorMsg);
                                            }
                                        }

                                        switch (category)
                                        {
                                            case "ADVANCE":
                                                if (action == "CREATE")
                                                {
                                                    retVal = fundAddAdv.Create(eventId, a3_id, fundsResult, jsonData, received_at, "FCAdv", ref validationMsg);
                                                }
                                                else
                                                {
                                                    retVal = fundAddAdv.Modify(eventId, a3_id, fundsResult, jsonData, received_at, "FMAdv", ref validationMsg);
                                                }

                                                break;

                                            case "ADDITIONAL":
                                                if (action == "CREATE")
                                                {
                                                    retVal = fundAddAdv.Create(eventId, a3_id, fundsResult, jsonData, received_at, "FCAdd", ref validationMsg);
                                                }
                                                else
                                                {
                                                    retVal = fundAddAdv.Modify(eventId, a3_id, fundsResult, jsonData, received_at, "FMAdd", ref validationMsg);
                                                }
                                                break;

                                            case "FINAL":

                                                if (action == "CREATE")
                                                {
                                                    retVal = fundFin.Create(eventId, a3_id, fundsResult, jsonData, received_at, "FCFin", ref validationMsg);
                                                }
                                                else
                                                {
                                                    retVal = fundFin.Modify(eventId, a3_id, fundsResult, jsonData, received_at, "FMFin", ref validationMsg);
                                                }
                                                break;

                                            default:
                                                break;
                                        }
                                        break;
                                    case "DAINVOICE":
                                        action = invoiceResult.action.ToUpper();
                                        entityHelper.ValidateTansAndDocumentDate(entity, invoiceResult.data.postingDate, invoiceResult.data.issuedAt, ref validationMsg);
                                        LogManager.Log.Info("ValidateTansAndDocumentDate...validationMsg..." + validationMsg.Count());
                                        string vendorCode = entityHelper.VendorCode(invoiceResult.data.vendorCode, ref validationMsg);
                                        LogManager.Log.Info("VendorCode...validationMsg..." + validationMsg.Count());
                                        string smcVendorCode = entityHelper.SMCVendorCode(nomDaId, entity, locCode, ref validationMsg);
                                        LogManager.Log.Info("SMCVendorCode...validationMsg..." + validationMsg.Count());

                                        if (action == "CREATE")
                                        {
                                            string kindofinvoice = vendorCode != smcVendorCode ? "Invoice" : "Debit Note";
                                            if (vendorCode != smcVendorCode && invoiceResult.data.cancelledAt == null)
                                            {
                                                var ICInvCost = (from e in context.Registries
                                                                 where e.RegKey == "GSExpTemplateIDForICInvCost"
                                                                 select e.RegValue);

                                                var ICInvVNCost = (from e in context.Registries
                                                                   where e.RegKey == "GSExpGroupIDForInvVNCost"
                                                                   select e.RegValue);

                                                var ICInvVTCost = (from e in context.Registries
                                                                   where e.RegKey == "GSExpGroupIDForInvVTCost"
                                                                   select e.RegValue);

                                                if (locCode == "THA")
                                                {
                                                    retVal = invoiceInvTH.Create(eventId, a3_id, invoiceResult, jsonData, received_at, "ICInvTH", vendorCode, smcVendorCode, ref validationMsg);
                                                }
                                                else if (locCode == "JPN")
                                                {
                                                    retVal = invoiceInvJP.Create(eventId, a3_id, invoiceResult, jsonData, received_at, "ICInvJP", vendorCode, smcVendorCode, ref validationMsg);
                                                }
                                                else
                                                {
                                                    if (invoiceResult.data.expTemplateId == int.Parse(ICInvCost.FirstOrDefault()))
                                                    {
                                                        retVal = invoiceInvCost.Create(eventId, a3_id, invoiceResult, jsonData, received_at, "ICInvCost", vendorCode, smcVendorCode, ref validationMsg);
                                                    }
                                                    else if (invoiceResult.data.expenseGroupId == int.Parse(ICInvVNCost.FirstOrDefault()) && locCode == "VNM")
                                                    {
                                                        retVal = invoiceInvVNCost.Create(eventId, a3_id, invoiceResult, jsonData, received_at, "ICInvVNCost", vendorCode, smcVendorCode, ref validationMsg);
                                                    }
                                                    else if (invoiceResult.data.expenseGroupId == int.Parse(ICInvVTCost.FirstOrDefault()) && locCode == "VTN")
                                                    {
                                                        retVal = invoiceInvVTCost.Create(eventId, a3_id, invoiceResult, jsonData, received_at, "ICInvVTCost", vendorCode, smcVendorCode, ref validationMsg);
                                                    }
                                                    else
                                                    {
                                                        retVal = invoiceInv.Create(eventId, a3_id, invoiceResult, jsonData, received_at, "ICInv", vendorCode, smcVendorCode, ref validationMsg);
                                                    }
                                                }
                                            }
                                            else if (vendorCode == smcVendorCode && invoiceResult.data.cancelledAt == null)
                                            {
                                                var ICDNtMisc_AR = (from e in context.Registries
                                                                    where e.RegKey == "GSExpTemplateIDForDNtMisc_AR"
                                                                    select e.RegValue);

                                                var ICFPE_CNCO = (from e in context.Registries
                                                                  where e.RegKey == "GSExpTemplateIDForFPE_CNCO"
                                                                  select e.RegValue);

                                                var ICTramp_C = (from e in context.Registries
                                                                 where e.RegKey == "GSExpTemplateIDForTramp_C"
                                                                 select e.RegValue);

                                                var ICDNtHoegh_FRT = (from e in context.Registries
                                                                      where e.RegKey == "GSExpTemplateIDForHoegh_FRT"
                                                                      select e.RegValue);

                                                var ICDNt_AR = (from e in context.Registries
                                                                where e.RegKey == "GSExpTemplateIDForDNt_AR"
                                                                select e.RegValue);


                                                if (locCode == "JPN")
                                                {
                                                    retVal = invoiceDNJP.Create(eventId, a3_id, invoiceResult, jsonData, received_at, "ICDNtJP", vendorCode, smcVendorCode, ref validationMsg);
                                                }
                                                else if (locCode == "THA")
                                                {
                                                    retVal = invoiceDNTH.Create(eventId, a3_id, invoiceResult, jsonData, received_at, "ICDNtTH", vendorCode, smcVendorCode, ref validationMsg);
                                                }
                                                else if (locCode == "VNM")
                                                {
                                                    retVal = invoiceDNVN.Create(eventId, a3_id, invoiceResult, jsonData, received_at, "ICDNtVN", vendorCode, smcVendorCode, ref validationMsg);
                                                }
                                                else if (locCode == "VTN")
                                                {
                                                    retVal = invoiceDNVT.Create(eventId, a3_id, invoiceResult, jsonData, received_at, "ICDNtVT", vendorCode, smcVendorCode, ref validationMsg);
                                                }
                                                else
                                                {
                                                    if (invoiceResult.data.expTemplateId == int.Parse(ICFPE_CNCO.FirstOrDefault()))
                                                    {
                                                        retVal = invoiceICFPE.Create(eventId, a3_id, invoiceResult, jsonData, received_at, "ICFPE_CNCO", vendorCode, smcVendorCode, ref validationMsg);
                                                    }
                                                    else if (invoiceResult.data.expTemplateId == int.Parse(ICTramp_C.FirstOrDefault()))
                                                    {
                                                        retVal = invoiceICTramp.Create(eventId, a3_id, invoiceResult, jsonData, received_at, "ICTramp_C", vendorCode, smcVendorCode, ref validationMsg);
                                                    }
                                                    else if (invoiceResult.data.expTemplateId == int.Parse(ICDNtMisc_AR.FirstOrDefault()))
                                                    {
                                                        retVal = invoiceDNtMisc.Create(eventId, a3_id, invoiceResult, jsonData, received_at, "ICDNtMisc_AR", vendorCode, smcVendorCode, ref validationMsg);
                                                    }
                                                    else if (invoiceResult.data.expTemplateId == int.Parse(ICDNt_AR.FirstOrDefault()))
                                                    {
                                                        retVal = invoiceDNt_AR.Create(eventId, a3_id, invoiceResult, jsonData, received_at, "ICDNt_AR", vendorCode, smcVendorCode, ref validationMsg);
                                                    }
                                                    else if (invoiceResult.data.expTemplateId == int.Parse(ICDNtHoegh_FRT.FirstOrDefault()))
                                                    {
                                                        retVal = invoiceDNtHoegh_FRT.Create(eventId, a3_id, invoiceResult, jsonData, received_at, "ICDNtHoegh_FRT", vendorCode, smcVendorCode, ref validationMsg);
                                                    }
                                                    else
                                                    {
                                                        retVal = invoiceDNt.Create(eventId, a3_id, invoiceResult, jsonData, received_at, "ICDNt", vendorCode, smcVendorCode, ref validationMsg);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                validationMsg = a3Helper.AppendError("0084:: The INVOICE is " + kindofinvoice + " and Cancelled At date is NOT null." + " <br>");
                                                uniqueErrors = validationMsg.Distinct<string>();

                                                supplierName = invoiceResult.data.vendorName;
                                                errorMsg = a3Helper.FormatErrorMsg(eventId, NomFileNumber, received_at, supplierName, invoiceResult, jsonData, false);

                                                throw new Exception(errorMsg);
                                            }
                                        }
                                        else
                                        {
                                            if (invoiceResult.data.cancelledAt != null)
                                            {
                                                var invoiceType = "";

                                                string kindofinvoice = vendorCode != smcVendorCode ? "Invoice" : "Debit Note";
                                                if (vendorCode != smcVendorCode)
                                                {
                                                    var ICInvCost = (from e in context.Registries
                                                                     where e.RegKey == "GSExpTemplateIDForICInvCost"
                                                                     select e.RegValue);

                                                    var ICInvVNCost = (from e in context.Registries
                                                                       where e.RegKey == "GSExpGroupIDForInvVNCost"
                                                                       select e.RegValue);

                                                    var ICInvVTCost = (from e in context.Registries
                                                                       where e.RegKey == "GSExpGroupIDForInvVTCost"
                                                                       select e.RegValue);

                                                    if (locCode == "THA")
                                                    {
                                                        invoiceType = "IMInvTH";
                                                    }
                                                    else
                                                    {
                                                        if (invoiceResult.data.expTemplateId == int.Parse(ICInvCost.FirstOrDefault()))
                                                        {
                                                            invoiceType = "IMInvCost";
                                                        }
                                                        else if (invoiceResult.data.expenseGroupId == int.Parse(ICInvVNCost.FirstOrDefault()) && locCode == "VNM")
                                                        {
                                                            invoiceType = "IMInvVNCost";
                                                        }
                                                        else if (invoiceResult.data.expenseGroupId == int.Parse(ICInvVTCost.FirstOrDefault()) && locCode == "VTN")
                                                        {
                                                            invoiceType = "IMInvVTCost";
                                                        }
                                                        else
                                                        {
                                                            invoiceType = "IMInv";
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    var ICDNtMisc_AR = (from e in context.Registries
                                                                        where e.RegKey == "GSExpTemplateIDForDNtMisc_AR"
                                                                        select e.RegValue);

                                                    var ICFPE_CNCO = (from e in context.Registries
                                                                      where e.RegKey == "GSExpTemplateIDForFPE_CNCO"
                                                                      select e.RegValue);

                                                    var ICTramp_C = (from e in context.Registries
                                                                     where e.RegKey == "GSExpTemplateIDForTramp_C"
                                                                     select e.RegValue);

                                                    var ICDNtHoegh_FRT = (from e in context.Registries
                                                                          where e.RegKey == "GSExpTemplateIDForHoegh_FRT"
                                                                          select e.RegValue);

                                                    if (locCode == "THA")
                                                    {
                                                        invoiceType = "IMDNtTH";

                                                    }
                                                    else if (locCode == "VNM")
                                                    {
                                                        invoiceType = "IMDNtVN";
                                                    }
                                                    else if (locCode == "VTN")
                                                    {
                                                        invoiceType = "IMDNtVT";
                                                    }
                                                    else
                                                    {
                                                        if (invoiceResult.data.expTemplateId == int.Parse(ICFPE_CNCO.FirstOrDefault()))
                                                        {
                                                            invoiceType = "IMFPE_CNCO";
                                                        }
                                                        else if (invoiceResult.data.expTemplateId == int.Parse(ICTramp_C.FirstOrDefault()))
                                                        {
                                                            invoiceType = "IMTramp_C";
                                                        }
                                                        else if (invoiceResult.data.expTemplateId == int.Parse(ICDNtMisc_AR.FirstOrDefault()))
                                                        {
                                                            invoiceType = "IMDNtMisc_AR";
                                                        }
                                                        else if (invoiceResult.data.expTemplateId == int.Parse(ICDNtHoegh_FRT.FirstOrDefault()))
                                                        {
                                                            invoiceType = "IMDNtHoegh_FRT";
                                                        }
                                                        else
                                                        {
                                                            invoiceType = "IMDNt";
                                                        }
                                                    }
                                                }

                                                retVal = invoiceInv.Modify(eventId, a3_id, invoiceResult, jsonData, received_at, vendorCode, smcVendorCode, invoiceType, ref validationMsg);
                                            }
                                            else
                                            {
                                                validationMsg = a3Helper.AppendError("0085:: This INVOICE is a MODIFY action having Cancelled At date is null." + " <br>");
                                                uniqueErrors = validationMsg.Distinct<string>();

                                                supplierName = invoiceResult.data.vendorName;
                                                errorMsg = a3Helper.FormatErrorMsg(eventId, NomFileNumber, received_at, supplierName, invoiceResult, jsonData, false);

                                                throw new Exception(errorMsg);
                                            }
                                        }
                                        break;
                                    case "DA":
                                        action = daResult.action.Trim().ToUpper();

                                        if (action == "MODIFY")
                                        {
                                            retVal = da.Modify(eventId, a3_id, daResult, jsonData, received_at, "DMCom", ref validationMsg);
                                        }
                                        else
                                        {
                                            retVal = da.Reverse(eventId, a3_id, daResult, jsonData, received_at, "DMCom", ref validationMsg);
                                        }

                                        break;
                                    default:
                                        break;
                                }

                                if (Config.bypassValidation == false && !retVal)
                                {
                                    errorMsg = "";
                                    uniqueErrors = validationMsg.Distinct<string>();

                                    if (uniqueErrors.Contains("The server was not found or was not accessible.") || uniqueErrors.Contains("The underlying provider failed on Open.") || uniqueErrors.Contains("Timeout expired.") || uniqueErrors.Contains("initializing the database") || uniqueErrors.Contains("Could not connect to"))
                                    {
                                        errorMsg = "The server was not found or was not accessible.";
                                        throw new Exception(errorMsg);
                                    }

                                    if (entity == "FUND")
                                    {
                                        var vendorName = fundsResult.data.vendorName;
                                        errorMsg = a3Helper.FormatErrorMsg(eventId, NomFileNumber, received_at, vendorName, fundsResult, jsonData, false);
                                    }
                                    else
                                    {
                                        if (entity == "DAINVOICE")
                                        {
                                            supplierName = invoiceResult.data.vendorName;
                                            errorMsg = a3Helper.FormatErrorMsg(eventId, NomFileNumber, received_at, supplierName, invoiceResult, jsonData, false);
                                        }
                                        else
                                        {
                                            errorMsg = a3Helper.FormatErrorMsg(eventId, NomFileNumber, received_at, "", daResult, jsonData, false);
                                        }
                                    }

                                    throw new Exception(errorMsg);
                                }
                            }
                            else if (!isGenerateJournal && locCode != "" && locCode != null)
                            {
                                LogManager.Log.Info("IsGenJrnl is false...");
                                A3Helper.InsertToA3EventJournal(eventId, action, entity, a3_id, jsonData, received_at, 0, "", "", "", "", false, true, "IsGenJrnl is false.");
                            }
                            else
                            {
                                if (Config.bypassValidation == false)
                                {
                                    LogManager.Log.Info("Error encountered..");
                                    errorMsg = "";
                                    uniqueErrors = validationMsg.Distinct<string>();

                                    if (entity == "FUND")
                                    {
                                        var vendorName = fundsResult.data.vendorName;
                                        errorMsg = a3Helper.FormatErrorMsg(eventId, NomFileNumber, received_at, vendorName, fundsResult, jsonData, false);
                                    }
                                    else
                                    {
                                        if (entity == "DAINVOICE")
                                        {
                                            supplierName = invoiceResult.data.vendorName;
                                            errorMsg = a3Helper.FormatErrorMsg(eventId, NomFileNumber, received_at, supplierName, invoiceResult, jsonData, false);
                                        }
                                        else
                                        {
                                            errorMsg = a3Helper.FormatErrorMsg(eventId, NomFileNumber, received_at, "", daResult, jsonData, false);
                                        }
                                    }

                                    throw new Exception(errorMsg);
                                }
                            }
                        }
                        else
                        {
                            LogManager.Log.Info("Event ID: " + eventId + "(" + item.entity.Replace("a3_table_", "") + "): A3 Event Entity is not relevant.");
                            A3Helper.InsertToA3EventJournal(eventId, action, item.entity.Replace("a3_table_", ""), a3_id, jsonData, received_at, 0, "", "", "", "", false, true, "A3 Event Entity is not relevant.");
                        }
                    }

                    A3Helper.UpdateWME("a3WMERetryCount" + "_" + Config.LocationCode, "0");
                    LogManager.Log.Info("Processing End...");
                    A3Helper.UpdateWMEStatus("Running", " ");
                }
                else
                {
                    A3Helper.StopSystem(systemMode);
                }
            }
            catch (Exception ex)
            {
                List<string> emailRecipients = new List<string>();
                StringBuilder errorMessages = new StringBuilder();
                int numOfRetry = 0;
                bool retry = false;
                bool error = false;
                errorMsg = ex.Message + " " + ex.InnerException;
                LogManager.Log.Info("ErrorMsg: " + errorMsg);
                if (errorMsg.Contains("The server was not found or was not accessible.") || errorMsg.Contains("The underlying provider failed on Open.") || errorMsg.Contains("Timeout expired.") || errorMsg.Contains("initializing the database") || errorMsg.Contains("Could not connect to"))
                {
                    LogManager.Log.Info("Add error to the list.");
                    uniqueErrors.ToList().Add("9999:: The server was not found or was not accessible.");
                }

                if (uniqueErrors != null)
                {
                    LogManager.Log.Info("Looping unique errors...");
                    foreach (var item in uniqueErrors)
                    {
                        LogManager.Log.Info("Item: " + item + " ...");
                        uniqueErrors = validationMsg.Distinct<string>();
                        var errorCode = item.Split(':')[0].ToString();
                        LogManager.Log.Info("Error code: " + errorCode + "...");

                        LogManager.Log.Info("Checking error registry table...");
                        var errorCodeForRetry = (from e in context.ErrorRegistries
                                                 where e.ErrCode == errorCode
                                                 select e).FirstOrDefault();

                        if (errorCodeForRetry != null && errorCodeForRetry.Recipients != string.Empty) { emailRecipients.Add(errorCodeForRetry.Recipients); }

                        if (!string.IsNullOrEmpty(errorCode))
                        {
                            if (errorCode.Trim().Length > 4)
                            {
                                LogManager.Log.Info("Error Code is greater than 4 characters..." + "[" + errorCode + "]");
                                errorMessages = errorMessages.Append(item + "<br>");
                                error = true;
                            }
                            else
                            {
                                if (errorCodeForRetry == null || errorCodeForRetry.NofRetry == 0)
                                {
                                    LogManager.Log.Info("No. of retry: " + errorCodeForRetry.NofRetry);
                                    error = true;
                                }
                                else
                                {
                                    if (numOfRetry == 0) numOfRetry = errorCodeForRetry.NofRetry;
                                    if (numOfRetry > errorCodeForRetry.NofRetry && errorCodeForRetry.NofRetry != 0)
                                    {
                                        numOfRetry = errorCodeForRetry.NofRetry;
                                    }
                                    LogManager.Log.Info("numOfRetry: " + numOfRetry);
                                    retry = true;
                                }
                                var errCode = errorCodeForRetry.ErrCode;
                                var errDesc = errorCodeForRetry.ErrMessage;
                                var nextActionUser = errorCodeForRetry.NextActionUser == string.Empty ? "" : "User Action: " + errorCodeForRetry.NextActionUser;
                                var nextActionHelpdesk = errorCodeForRetry.NextActionHelpdesk == string.Empty ? "<br>" : "<br>Helpdesk: " + errorCodeForRetry.NextActionHelpdesk + "<br><br>";
                                errorMessages = errorMessages.Append(errCode + ":: " + errDesc + "<br>" + nextActionUser + nextActionHelpdesk);
                            }
                        }
                    }
                }
                else
                {
                    error = true;
                }

                Hierarchy hier = log4net.LogManager.GetRepository() as Hierarchy;
                var smtpappender = (ExtendedSmtpAppender)hier.GetAppenders().Where(appender => appender.Name.Equals("SmtpAppender", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                var emailTo = smtpappender.To + "," + (initials == "" ? "" : initials + "@wallem.com,") + string.Join(",", emailRecipients.Distinct<string>());
                smtpappender.To = emailTo.TrimEnd(',');
                smtpappender.ActivateOptions();

                if (error)
                {
                    LogManager.Log.Info("Processing End...");
                    A3Helper.UpdateWMEStatus("Error", ex.Message);
                    LogManager.Log.Error(ex.Message.Replace("{error}", errorMessages.ToString()) + " " + ex.InnerException);
                }
                else
                {
                    string retryCountField = "a3WMERetryCount" + "_" + Config.LocationCode;
                    if (retry)
                    {
                        var a3WMERetryCount = (from e in context.Registries
                                               where e.RegKey == retryCountField
                                               select e.RegValue);

                        int WMERetryCount = a3WMERetryCount == null ? 0 : int.Parse(a3WMERetryCount.FirstOrDefault());
                        LogManager.Log.Info("WMERetryCount: " + WMERetryCount);

                        if (WMERetryCount < numOfRetry)
                        {
                            WMERetryCount = WMERetryCount + 1;
                            if (WMERetryCount == 1 && !ex.Message.Contains("The server was not found or was not accessible.") && !uniqueErrors.Contains("The server was not found or was not accessible."))
                            {
                                ex.Message.Replace("<font color='red'>Please modify fund to above errors</font><br><br>", "");
                                ex.Message.Replace("<font color='red'>Please reverse invoice and reupload</font><br><br>", "");
                                LogManager.Log.Warn(ex.Message + " " + ex.InnerException + ":::" + jsonData);
                                if (entity.ToUpper() != "DA" && initials != "")
                                {
                                    initials = "," + initials + "@wallem.com";
                                }
                                A3Helper.SendMail(Config.fromWarn, Config.toWarn + initials, Config.ccWarn, Config.bccWarn, Config.subjectWarn, ex.Message.Replace("{error}", errorMessages.ToString()));
                            }

                            A3Helper.UpdateWME(retryCountField, WMERetryCount.ToString());
                            LogManager.Log.Info("Processing End...");
                            A3Helper.UpdateWMEStatus("Running", " ");
                            TS_App.End();
                        }
                        else
                        {
                            A3Helper.UpdateWME(retryCountField, "0");
                            LogManager.Log.Info("Processing End...");
                            A3Helper.UpdateWMEStatus("Error", ex.Message);
                            LogManager.Log.Error(ex.Message.Replace("{error}", errorMessages.ToString()) + " " + ex.InnerException);
                        }
                    }
                }
            }
        }

        public static bool GetPayload(ref int eventId, ref string entity, ref string action, ref DateTime? received_at, ref string jsonData, ref string errorMessage)
        {
            try
            {
                int refId = eventId;

                LogManager.Log.Info("GetPayload Start...");
                WIS_Sync ctx = new WIS_Sync();

                var events = (from e in ctx.a3EventStage
                              where e.id == refId
                              select new { e.data, e.entity, e.action, e.received_at }
                              ).First();

                jsonData = events.data;
                entity = events.entity;
                action = events.action;
                received_at = events.received_at;
                errorMessage = "";
                return true;
            }
            catch (Exception ex)
            {
                LogManager.Log.Info("GetPayload Error: " + ex.Message);
                errorMessage = ex.Message;
                throw;
            }

        }


    }
}