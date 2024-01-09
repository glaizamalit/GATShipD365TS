using GATShipD365TS.App_Code;
using GATShipD365TS.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;
using System.Net.Mail;
using System.Text.RegularExpressions;
//
using GATShipD365TS.Helper;
using System.Threading;
using log4net.Repository.Hierarchy;
using GATShipD365TS.GATShip.Entities;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace GATShipD365TS.Helper
{

    public class A3Helper
    {
        WIS_Sync context = new WIS_Sync();
        GSWallem contextgswallem = new GSWallem();
        List<string> errMsgs = new List<string>();

        public List<a3EventJournal> EventJournal()
        {
            try
            {
                var eventJournal = (from seq in context.a3EventJournals
                                    select seq).ToList();
                return eventJournal;
            }
            catch (Exception e)
            {
                LogManager.Log.Error(e.Message + " " + e.InnerException);
                throw;
            }

        }

        public static void SaveXML(string XMLReponse, string FileLocation)
        {
            LogManager.Log.Info("Save XML data to file...");
            try
            {
                var xmlDoc = new XmlDocument();

                xmlDoc.LoadXml(XMLReponse);
                xmlDoc.Save(RemFileNumber(FileLocation));
            }
            catch (Exception e)
            {
                LogManager.Log.Error(e.Message + " " + e.InnerException);
                throw;
            }

        }
       
        public static void Serialize(D365FOJournal details, string fileLocation)
        {                                  
            LogManager.Log.Info("Create JSON File " + details._gatshipData.FileBatchID);
            try
            {
                //serialize object to json
                JsonSerializer serializer = new JsonSerializer();
                string submittedJson = JsonConvert.SerializeObject(details,Newtonsoft.Json.Formatting.Indented);

                File.WriteAllText(fileLocation, submittedJson);
            }
            catch (Exception e)
            {
                LogManager.Log.Error(e.Message + " " + e.InnerException);
                throw;
            }

        }

        public D365FOJournal Deserialize(string fileLocation)
        {
            LogManager.Log.Info("Reading Json File located in " + fileLocation);
            try
            {
                D365FOJournal journal = null;
                var jsonContent = System.IO.File.ReadAllText(fileLocation);
                journal = JsonConvert.DeserializeObject<D365FOJournal>(jsonContent);               
                return journal;
            }
            catch (Exception e)
            {
                LogManager.Log.Error(e.Message + " " + e.InnerException);
                throw;
            }
        }
        public static string RemFileNumber(string FileLocation)
        {
            if (FileLocation.Contains("(") && FileLocation.Contains(")"))
            {
                int substringCount = 0;
                for (int i = FileLocation.IndexOf("("); i <= FileLocation.IndexOf(")"); i++)
                {
                    substringCount += 1;
                }
                FileLocation = FileLocation.Remove(FileLocation.IndexOf("("), substringCount);
            }

            return FileLocation;
        }

        public static void DeleteFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        public static void ValidatePath(string fileLocation)
        {
            int index = fileLocation.LastIndexOf("\\");
            var createPath = fileLocation.Remove(index);
            if (!string.IsNullOrEmpty(createPath))
            {
                if (!Directory.Exists(createPath))
                    Directory.CreateDirectory(createPath);
            }
        }

        public JsonResponse ReadingResponseFromD365(string jsonRet)
        {
            const char doubleqoute = '"';
            const char singlequote = '\'';
            const int NextIndex = 1;
            const int FirstIndex = 0;
            const string startstringToCheck = "Submitted\":\"";
            const string endstringToCheck = "\"}";
            JsonResponse response = new JsonResponse();

            LogManager.Log.Info("Reading response from D365");
            if (!jsonRet.Equals(""))
            {
                jsonRet = Regex.Unescape(jsonRet);
                jsonRet = jsonRet.Trim();

                while (jsonRet.StartsWith(doubleqoute.ToString()))
                {
                    jsonRet = jsonRet.Substring(NextIndex);
                }

                while (jsonRet.EndsWith(doubleqoute.ToString()))
                {
                    jsonRet = jsonRet.Substring(0, jsonRet.Length - NextIndex);
                }

                int submittedIndex = jsonRet.IndexOf(startstringToCheck) + startstringToCheck.Length;
                int submittedlen = jsonRet.Length - ((jsonRet.Length - jsonRet.IndexOf(endstringToCheck)) + submittedIndex);
                var tempSubmitted = jsonRet.Substring(submittedIndex, submittedlen);
                var newRetval = (jsonRet.Substring(FirstIndex, jsonRet.IndexOf(startstringToCheck)) + startstringToCheck) + tempSubmitted.Replace(doubleqoute, singlequote) + jsonRet.Substring(jsonRet.IndexOf(endstringToCheck));

                if (newRetval.Length == jsonRet.Length)
                {
                    jsonRet = newRetval;
                }

                response = JsonConvert.DeserializeObject<JsonResponse>(jsonRet);
            }
            else
            {
                LogManager.Log.Info("null response");
                var createResponse = new JsonResponse
                {
                    Status = 0,
                    Submitted = "No response generated",
                    ReturnMsg = "Failed processing D365FOJournal"
                };

                //response = new JsonResponse
                //{
                //    Message = createResponse
                //};
            }

            LogManager.Log.Info("Response received.");
            return response;
        }


        public static void InsertToA3EventJournal(int eventId, string action, string entity, int id, string jsonData, DateTime? received_at, int? sequenceNo, string filePath, string fileName, string filePathRev, string fileNameRev, bool isRelevant, bool isReversed, string systemMsg)
        {
            EntityHelper helper = new EntityHelper();
            LogManager.Log.Info("Insert record to A3EventJournal...");
            var wisSyncContext = new WIS_Sync();

            for (int i = 1; i <= 3; i++)
            {
                try
                {
                    var a3Journal = new a3EventJournal()
                    {
                        id = eventId,
                        action = action,
                        entity = entity,
                        a3_id = id,
                        data = jsonData,
                        received_at = received_at,
                        isRelevant = isRelevant,
                        JrnlDate = sequenceNo == 0 ? (DateTime?)null : DateTime.Now.Date,
                        SeqNo = sequenceNo,
                        JrnlFilePath = filePath,
                        JrnlFileName = fileName,
                        isReversed = isReversed,
                        RevJrnlFilePath = filePathRev,
                        RevJrnlFileName = fileNameRev,
                        SystemMessage = systemMsg,
                        CreatedDT = DateTime.Now,
                        UpdatedDT = DateTime.Now,
                        a3LocCode = Config.LocationCode

                    };
                    wisSyncContext.a3EventJournals.Add(a3Journal);
                    wisSyncContext.SaveChanges();

                    break;
                }
                catch (Exception e)
                {
                    string errorMsg = "Event Id: " + eventId + "<br>Entity: " + entity.ToUpper() + "<br>Action: " + action.ToUpper() + "<br><br>";
                    string errmsg = errorMsg + "<br>" + e.Message + "<br>" + e.InnerException + "<br>";
                    LogManager.Log.Info("Failed to insert record to a3EventJournal...");

                    if (errmsg.Contains("Could not connect") || errmsg.Contains("The server was not found or was not accessible.") || errmsg.Contains("The underlying provider failed on Open.") || errmsg.Contains("Timeout expired.") || errmsg.Contains("initializing the database") || errmsg.Contains("A transport-level error has occurred"))
                    {
                        LogManager.Log.Info("Retry: " + i);
                        if (i == 3)
                        {
                            LogManager.Log.Error(errmsg);
                            throw new Exception(errmsg);
                        }
                        Thread.Sleep(1000 * 60 * 1);
                    }
                    else
                    {
                        LogManager.Log.Error(errmsg);
                        throw new Exception(errmsg);
                    }
                }
            }
        }


        public static void UpdateA3EventJournal(int eventId, dynamic result, string revJournalFilePath, string revJournalFileName)
        {
            LogManager.Log.Info("Update record from A3EventJournal...");
            var wisSyncContext = new WIS_Sync();

            for (int i = 1; i <= 3; i++)
            {
                try
                {
                    int id = result.data.id;
                    string entity = result.entity;
                    var previousJournal = wisSyncContext.a3EventJournals.Where(m => m.a3_id == id && m.id < eventId && m.entity.ToUpper() == entity.ToUpper()).OrderByDescending(x => x.id);
                    a3EventJournal a3Journal = new a3EventJournal();
                    a3Journal = previousJournal.FirstOrDefault();
                    a3Journal.isReversed = true;
                    a3Journal.RevJrnlFilePath = revJournalFilePath;
                    a3Journal.RevJrnlFileName = revJournalFileName;
                    a3Journal.UpdatedDT = DateTime.Now;


                    wisSyncContext.a3EventJournals.Attach(a3Journal);
                    wisSyncContext.Entry(a3Journal).Property(u => u.isReversed).IsModified = true;
                    wisSyncContext.Entry(a3Journal).Property(u => u.RevJrnlFilePath).IsModified = true;
                    wisSyncContext.Entry(a3Journal).Property(u => u.RevJrnlFileName).IsModified = true;
                    wisSyncContext.Entry(a3Journal).Property(u => u.UpdatedDT).IsModified = true;
                    wisSyncContext.SaveChanges();

                    break;
                }
                catch (Exception e)
                {
                    string errorMsg = "Event Id: " + eventId + "<br>Entity: " + result.entity + "<br>Action: " + result.action + "<br><br>";
                    string errmsg = errorMsg + "<br>" + e.Message + "<br>" + e.InnerException + "<br>";
                    LogManager.Log.Info("Failed to updated record to A3EventJournal...");

                    if (errmsg.Contains("Could not connect") || errmsg.Contains("The server was not found or was not accessible.") || errmsg.Contains("The underlying provider failed on Open.") || errmsg.Contains("Timeout expired.") || errmsg.Contains("initializing the database") || errmsg.Contains("A transport-level error has occurred"))
                    {
                        LogManager.Log.Info("Retry: " + i);
                        if (i == 3)
                        {
                            LogManager.Log.Error(errmsg);
                            throw new Exception(errmsg);
                        }
                        Thread.Sleep(1000 * 60 * 1);
                    }
                    else
                    {
                        LogManager.Log.Error(errmsg);
                        throw new Exception(errmsg);
                    }
                }
            }
        }

        public List<string> AppendError(string msg)
        {
            errMsgs.Add(msg);
            return errMsgs;
        }

        public static void SendMail(string from, string to, string cc, string bcc, string subject, string body)
        {
            try
            {
                LogManager.Log.Info("Send warn email to recipients...");
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(from);
                string[] toArr = to.Split(",".ToCharArray());
                LogManager.Log.Info("To: " + to);
                string[] ccArr = cc.Split(",".ToCharArray());
                LogManager.Log.Info("Cc: " + cc);
                string[] bccArr = bcc.Split(",".ToCharArray());
                LogManager.Log.Info("Bcc: " + bcc);
                foreach (var item in toArr)
                {
                    if (item != "")
                    {
                        mail.To.Add(item);
                    }
                }
                LogManager.Log.Info("Added to..");
                foreach (var item in ccArr)
                {
                    if (item != "")
                    {
                        mail.CC.Add(item);
                    }
                }
                LogManager.Log.Info("Added cc..");
                foreach (var item in bccArr)
                {
                    if (item != "")
                    {
                        mail.Bcc.Add(item);
                    }

                }
                LogManager.Log.Info("Added bcc..");

                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient(Config.SMTPHost);
                smtp.Send(mail);
            }
            catch (Exception e)
            {
                LogManager.Log.Error(e.Message + " " + e.InnerException);
                throw;
            }

        }

        public static void InsertToA3DAComInvoiceSuffix(int eventId, string tmpKey, int? suffixNo, string locCode, dynamic result)
        {
            LogManager.Log.Info("Insert record to A3DAComInvoiceSuffix...");
            var wisSyncContext = new WIS_Sync();

            for (int i = 1; i < 3; i++)
            {
                try
                {
                    if (suffixNo == null)
                    {
                        suffixNo = 0;
                    }
                    var A3DAComInvoiceSuffix = new a3DAComInvoiceSuffix()
                    {
                        a3LocCode = locCode,
                        DAComInvoice = tmpKey,
                        SuffixNo = suffixNo,
                        CreatedDT = DateTime.Now,
                        UpdatedDT = DateTime.Now
                    };
                    wisSyncContext.a3DAComInvoiceSuffixes.Add(A3DAComInvoiceSuffix);
                    wisSyncContext.SaveChanges();

                    break;
                }
                catch (Exception e)
                {
                    string errorMsg = "Event Id: " + eventId + "<br>Entity: " + result.entity + "<br>Action: " + result.action + "<br><br>";
                    string errmsg = errorMsg + "<br>" + e.Message + "<br>" + e.InnerException + "<br>";
                    LogManager.Log.Info("Failed to insert record to A3DAComInvoiceSuffix...");

                    if (errmsg.Contains("Could not connect") || errmsg.Contains("The server was not found or was not accessible.") || errmsg.Contains("The underlying provider failed on Open.") || errmsg.Contains("Timeout expired.") || errmsg.Contains("initializing the database") || errmsg.Contains("A transport-level error has occurred"))
                    {
                        LogManager.Log.Info("Retry: " + i);
                        if (i == 3)
                        {
                            LogManager.Log.Error(errmsg);
                            throw new Exception(errmsg);
                        }
                        Thread.Sleep(1000 * 60 * 1);
                    }
                    else
                    {
                        LogManager.Log.Error(errmsg);
                        throw new Exception(errmsg);
                    }
                }
            }
        }
        public static void UpdateA3DAComInvoiceSuffix(int eventId, string tmpKey, int? suffixNo, string locCode, dynamic result)
        {
            LogManager.Log.Info("Update record from A3DAComInvoiceSuffix...");

            for (int i = 1; i < 3; i++)
            {
                try
                {
                    using (var wisSyncContext = new WIS_Sync())
                    {
                        var prevInvoiceSuffix = wisSyncContext.a3DAComInvoiceSuffixes.Where(m => m.a3LocCode == locCode && m.DAComInvoice == tmpKey);
                        a3DAComInvoiceSuffix a3DAComInvoice = new a3DAComInvoiceSuffix();
                        a3DAComInvoice = prevInvoiceSuffix.FirstOrDefault();
                        a3DAComInvoice.SuffixNo = suffixNo;
                        a3DAComInvoice.UpdatedDT = DateTime.Now;

                        wisSyncContext.a3DAComInvoiceSuffixes.Attach(a3DAComInvoice);
                        wisSyncContext.Entry(a3DAComInvoice).Property(u => u.SuffixNo).IsModified = true;
                        wisSyncContext.Entry(a3DAComInvoice).Property(u => u.UpdatedDT).IsModified = true;
                        wisSyncContext.SaveChanges();
                    }

                    break;
                }
                catch (Exception e)
                {
                    string errorMsg = "Event Id: " + eventId + "<br>Entity: " + result.entity + "<br>Action: " + result.action + "<br><br>";
                    string errmsg = errorMsg + "<br>" + e.Message + "<br>" + e.InnerException + "<br>";
                    LogManager.Log.Info("Failed to update record to A3DAComInvoiceSuffix...");

                    if (errmsg.Contains("Could not connect") || errmsg.Contains("The server was not found or was not accessible.") || errmsg.Contains("The underlying provider failed on Open.") || errmsg.Contains("Timeout expired.") || errmsg.Contains("initializing the database") || errmsg.Contains("A transport-level error has occurred"))
                    {
                        LogManager.Log.Info("Retry: " + i);
                        if (i == 3)
                        {
                            LogManager.Log.Error(errmsg);
                            throw new Exception(errmsg);
                        }
                        Thread.Sleep(1000 * 60 * 1);
                    }
                    else
                    {
                        LogManager.Log.Error(errmsg);
                        throw new Exception(errmsg);
                    }
                }

            }
        }


        public static void InsertToA3InvoiceSuffix(int eventId, string vendorCode, string invoiceNo, int? suffixNo, dynamic result)
        {
            LogManager.Log.Info("Insert record to A3InvoiceSuffix...");
            for (int i = 1; i < 3; i++)
            {
                try
                {
                    using (var wisSyncContext = new WIS_Sync())
                    {
                        var a3InvoiceSuffix = new a3InvoiceSuffix()
                        {
                            VendorCode = vendorCode,
                            InvoiceNo = invoiceNo.Length > 50 ? invoiceNo.Substring(0, 50) : invoiceNo,
                            SuffixNo = suffixNo,
                            CreatedDT = DateTime.Now,
                            UpdatedDT = DateTime.Now
                        };
                        wisSyncContext.a3InvoiceSuffixes.Add(a3InvoiceSuffix);
                        wisSyncContext.SaveChanges();
                    }

                    break;
                }
                catch (Exception e)
                {
                    string errorMsg = "Event Id: " + eventId + "<br>Entity: " + result.entity + "<br>Action: " + result.action + "<br><br>";
                    string errmsg = errorMsg + "<br>" + e.Message + "<br>" + e.InnerException + "<br>";
                    LogManager.Log.Info("Failed to insert record to A3InvoiceSuffix...");

                    if (errmsg.Contains("Could not connect") || errmsg.Contains("The server was not found or was not accessible.") || errmsg.Contains("The underlying provider failed on Open.") || errmsg.Contains("Timeout expired.") || errmsg.Contains("initializing the database") || errmsg.Contains("A transport-level error has occurred"))
                    {
                        LogManager.Log.Info("Retry: " + i);
                        if (i == 3)
                        {
                            LogManager.Log.Error(errmsg);
                            throw new Exception(errmsg);
                        }
                        Thread.Sleep(1000 * 60 * 1);
                    }
                    else
                    {
                        LogManager.Log.Error(errmsg);
                        throw new Exception(errmsg);
                    }
                }

            }
        }

        public static void UpdateA3InvoiceSuffix(int eventId, string vendorCode, string invoiceNo, int? suffixNo, dynamic result)
        {
            LogManager.Log.Info("Update record from A3InvoiceSuffix...");

            for (int i = 1; i < 3; i++)
            {
                try
                {
                    using (var wisSyncContext = new WIS_Sync())
                    {
                        var prevInvoiceSuffix = wisSyncContext.a3InvoiceSuffixes.Where(m => m.VendorCode == vendorCode && m.InvoiceNo == invoiceNo);
                        a3InvoiceSuffix a3InvoiceSuffix = new a3InvoiceSuffix();
                        a3InvoiceSuffix = prevInvoiceSuffix.FirstOrDefault();
                        a3InvoiceSuffix.SuffixNo = suffixNo + 1;
                        a3InvoiceSuffix.UpdatedDT = DateTime.Now;

                        wisSyncContext.a3InvoiceSuffixes.Attach(a3InvoiceSuffix);
                        wisSyncContext.Entry(a3InvoiceSuffix).Property(u => u.SuffixNo).IsModified = true;
                        wisSyncContext.Entry(a3InvoiceSuffix).Property(u => u.UpdatedDT).IsModified = true;
                        wisSyncContext.SaveChanges();
                    }

                    break;
                }
                catch (Exception e)
                {
                    string errorMsg = "Event Id: " + eventId + "<br>Entity: " + result.entity + "<br>Action: " + result.action + "<br><br>";
                    string errmsg = errorMsg + "<br>" + e.Message + "<br>" + e.InnerException + "<br>";
                    LogManager.Log.Info("Failed to update record to A3InvoiceSuffix...");

                    if (errmsg.Contains("Could not connect") || errmsg.Contains("The server was not found or was not accessible.") || errmsg.Contains("The underlying provider failed on Open.") || errmsg.Contains("Timeout expired.") || errmsg.Contains("initializing the database") || errmsg.Contains("A transport-level error has occurred"))
                    {
                        LogManager.Log.Info("Retry: " + i);
                        if (i == 3)
                        {
                            LogManager.Log.Error(errmsg);
                            throw new Exception(errmsg);
                        }
                        Thread.Sleep(1000 * 60 * 1);
                    }
                    else
                    {
                        LogManager.Log.Error(errmsg);
                        throw new Exception(errmsg);
                    }
                }
            }
        }

        public string FormatErrorMsg(int eventId, string nominationFileNum, DateTime? received_at, string companyName, dynamic result, string jsonData, bool isUnlinked)
        {
            LogManager.Log.Info("Formatting error message...");
            var errorMessage = "";
            try
            {
                StringBuilder body = new StringBuilder();

                if (result.entity.ToUpper() == "FUND")
                {
                    FundsPayload fundsResult = new FundsPayload();
                    fundsResult = result;
                    var category = fundsResult.data.category == null || fundsResult.data.category == string.Empty ? "ADDITIONAL" : fundsResult.data.category;

                    body.Append("This is to notify you that there are invalid data on <b>GATShip <font color='red'>and no journal was posted to AX.</font> </b><br>");
                    if (isUnlinked) { body.Append("<b><font color='red'>Invoice number is temporarily unlinked </font></b> for your data correcion.  It will be linked after posting.<br><br>"); }
                    body.Append("<br><b>Information Details :</b><br><br> ");
                    body.Append(nominationFileNum == "" ? "" : "Port Call Number: <b>" + nominationFileNum + "</b> <br>");
                    body.Append("Entity: <b>" + fundsResult.entity.ToUpper() + "</b> <br>");
                    body.Append("Action: <b>" + fundsResult.action + "</b> <br>");
                    body.Append("Fund Date: <b>" + (fundsResult.data.dateReceived.HasValue ? fundsResult.data.dateReceived.Value.ToString("dd-MMM-yyyy") : null) + "</b> <br>");
                    body.Append("Category: " + category + " <br>");
                    body.Append("Payee: " + companyName + "<br>");
                    body.Append("Amount: <b>" + fundsResult.data.amount + "</b> <br>");
                    body.Append("Ref Number: <b>" + fundsResult.data.refNumber + "</b> <br>");
                    body.Append("Initials: <b>" + fundsResult.data.initials + "</b> <br>");
                    body.Append("Fund Remarks: " + fundsResult.data.comment + "<br><br><br>");
                    body.Append("More info:-<br>");
                    body.Append("{error}<br><br>");
                    body.Append("<font color='red'>Please modify fund to above errors</font><br><br>");
                    body.Append("(Internal use only)<br>");
                    body.Append("Event Id : <b>" + eventId + "</b><br>");
                    body.Append("Received at : " + received_at + "<br>");
                    body.Append("Process Name : <b>" + "Process" + Config.SystemCode + "Events" + "</b><br>");
                    body.Append("Execution Date and Time : <b>" + DateTime.Now.ToString() + "</b><br><br>");
                    body.Append(jsonData);
                    errorMessage = body.ToString();
                }
                else
                {
                    if (result.entity.ToUpper() == "DAINVOICE")
                    {
                        InvoicePayload invoiceResult = result;

                        body.Append("This is to notify you that there are invalid data on <b>GATShip <font color='red'>and no journal was posted to AX.</font> </b><br>");
                        if (isUnlinked) { body.Append("<b><font color='red'>Invoice number is temporarily unlinked </font></b> for your data correcion.  It will be linked after posting.<br>"); }
                        body.Append("<br><b>Information Details :</b><br><br> ");
                        body.Append("Port Call Number: <b>" + nominationFileNum + "</b> <br>");
                        body.Append("Entity: <b>" + invoiceResult.entity.ToUpper() + "</b> <br>");
                        body.Append("Action: <b>" + invoiceResult.action + "</b> <br>");
                        body.Append("Invoice Issued Date: <b>" + (invoiceResult.data.issuedAt.HasValue ? invoiceResult.data.issuedAt.Value.ToString("dd-MMM-yyyy") : null) + "</b> <br>");
                        body.Append("Supplier: " + companyName + "<br>");
                        body.Append("Invoice Reference: " + invoiceResult.data.reference + "<br>");
                        body.Append("Amount: <b>" + invoiceResult.data.amount + "</b> <br>");
                        body.Append("Ref Number: <b>" + invoiceResult.data.refNumber + "</b> <br>");
                        body.Append("Initials: <b>" + invoiceResult.data.initials + "</b> <br>");
                        body.Append("Invoice Remarks: " + invoiceResult.data.remarks + "<br><br><br>");
                        body.Append("More info:-<br>");
                        body.Append("{error}<br><br>");
                        //body.Append("<font color='red'>Please reverse invoice and reupload</font><br><br>");
                        body.Append("(Internal use only)<br>");
                        body.Append("Event Id : <b>" + eventId + "</b><br>");
                        body.Append("Received at : " + received_at + "<br>");
                        body.Append("Process Name : <b>" + "Process" + Config.SystemCode + "Events" + "</b><br>");
                        body.Append("Execution Date and Time : <b>" + DateTime.Now.ToString() + "</b><br><br>");
                        body.Append(jsonData);

                    }
                    else
                    {
                        DAPayload daResult = result;

                        body.Append("This is to notify you that there are invalid data on <b>GATShip <font color='red'>and no journal was posted to AX.</font> </b><br><br>");
                        body.Append("<b>Information Details :</b><br><br> ");
                        body.Append("Port Call Number: <b>" + nominationFileNum + "</b> <br>");
                        body.Append("Entity: <b>" + daResult.entity.ToUpper() + "</b> <br>");
                        body.Append("FDA Invoice Number: <b>" + daResult.data.dcnNumber + "</b><br><br><br>");
                        //body.Append("Action: <b>" + daResult.action + "</b> <br><br><br>");
                        body.Append("More info:-<br>");
                        body.Append("{error}<br><br>");
                        body.Append("(Internal use only)<br>");
                        body.Append("Event Id : <b>" + eventId + "</b><br>");
                        body.Append("Received at : " + received_at + "<br>");
                        body.Append("Process Name : <b>" + "Process" + Config.SystemCode + "Events" + "</b><br>");
                        body.Append("Execution Date and Time : <b>" + DateTime.Now.ToString() + "</b><br><br>");
                        body.Append(jsonData);
                    }

                    errorMessage = body.ToString();
                }

                return errorMessage;
            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in formatting error message: " + e.Message + ":::" + e.InnerException);
                return errorMessage;
            }
        }

        public string FormatWarningMsg(int eventId, string nominationFileNum, DateTime? received_at, string companyName, dynamic result, string jsonData)
        {
            LogManager.Log.Info("Formatting error message...");
            var errorMessage = "";
            try
            {
                StringBuilder body = new StringBuilder();

                if (result.entity.ToUpper() == "FUND")
                {
                    FundsPayload fundsResult = new FundsPayload();
                    fundsResult = result;
                    var category = fundsResult.data.category == null || fundsResult.data.category == string.Empty ? "ADDITIONAL" : fundsResult.data.category;

                    body.Append(Config.headerWarn);
                    body.Append(nominationFileNum == "" ? "" : "Port Call Number: <b>" + nominationFileNum + "</b> <br>");
                    body.Append("Entity: <b>" + fundsResult.entity.ToUpper() + "</b> <br>");
                    body.Append("Action: <b>" + fundsResult.action + "</b> <br>");
                    body.Append("Fund Date: <b>" + (fundsResult.data.dateReceived.HasValue ? fundsResult.data.dateReceived.Value.ToString("dd-MMM-yyyy") : null) + "</b> <br>");
                    body.Append("Category: " + category + " <br>");
                    body.Append("Payee: " + companyName + "<br>");
                    body.Append("Amount: <b>" + fundsResult.data.amount + "</b> <br>");
                    body.Append("Ref Number: <b>" + fundsResult.data.refNumber + "</b> <br>");
                    body.Append("Initials: <b>" + fundsResult.data.initials + "</b> <br>");
                    body.Append("Fund Remarks: " + fundsResult.data.comment + "<br><br><br>");
                    body.Append("More info:-<br>");
                    body.Append("{warning}<br><br>");
                    body.Append("(Internal use only)<br>");
                    body.Append("Event Id : <b>" + eventId + "</b><br>");
                    body.Append("Received at : " + received_at + "<br>");
                    body.Append("Process Name : <b>" + "Process" + Config.SystemCode + "Events" + "</b><br>");
                    body.Append("Execution Date and Time : <b>" + DateTime.Now.ToString() + "</b><br><br>");
                    body.Append(jsonData);
                    errorMessage = body.ToString();
                }
                else
                {
                    if (result.entity.ToUpper() == "DAINVOICE")
                    {
                        InvoicePayload invoiceResult = result;

                        body.Append(Config.headerWarn);
                        body.Append(nominationFileNum == "" ? "" : "Port Call Number: <b>" + nominationFileNum + "</b> <br>");
                        body.Append("Entity: <b>" + invoiceResult.entity.ToUpper() + "</b> <br>");
                        body.Append("Action: <b>" + invoiceResult.action + "</b> <br>");
                        body.Append("Invoice Issued Date: <b>" + (invoiceResult.data.issuedAt.HasValue ? invoiceResult.data.issuedAt.Value.ToString("dd-MMM-yyyy") : null) + "</b> <br>");
                        body.Append("Supplier: " + companyName + "<br>");
                        body.Append("Invoice Reference: " + invoiceResult.data.reference + "<br>");
                        body.Append("Amount: <b>" + invoiceResult.data.amount + "</b> <br>");
                        body.Append("Ref Number: <b>" + invoiceResult.data.refNumber + "</b> <br>");
                        body.Append("Initials: <b>" + invoiceResult.data.initials + "</b> <br>");
                        body.Append("Invoice Remarks: " + invoiceResult.data.remarks + "<br><br><br>");
                        body.Append("More info:-<br>");
                        body.Append("{warning}<br><br>");
                        body.Append("(Internal use only)<br>");
                        body.Append("Event Id : <b>" + eventId + "</b><br>");
                        body.Append("Received at : " + received_at + "<br>");
                        body.Append("Process Name : <b>" + "Process" + Config.SystemCode + "Events" + "</b><br>");
                        body.Append("Execution Date and Time : <b>" + DateTime.Now.ToString() + "</b><br><br>");
                        body.Append(jsonData);

                    }
                    else
                    {
                        DAPayload daResult = result;

                        body.Append(Config.headerWarn);
                        body.Append(nominationFileNum == "" ? "" : "Port Call Number: <b>" + nominationFileNum + "</b> <br>");
                        body.Append("Entity: <b>" + daResult.entity.ToUpper() + "</b> <br>");
                        body.Append("FDA Invoice Number : <b>" + daResult.data.dcnNumber + "</b><br><br><br>");
                        //body.Append("Action: <b>" + daResult.action + "</b> <br><br><br>");
                        body.Append("More info:-<br>");
                        body.Append("{warning}<br><br>");
                        body.Append("(Internal use only)<br>");
                        body.Append("Event Id : <b>" + eventId + "</b><br>");
                        body.Append("Received at : " + received_at + "<br>");
                        body.Append("Process Name : <b>" + "Process" + Config.SystemCode + "Events" + "</b><br>");
                        body.Append("Execution Date and Time : <b>" + DateTime.Now.ToString() + "</b><br><br>");
                        body.Append(jsonData);
                    }

                    errorMessage = body.ToString();
                }

                return errorMessage;
            }
            catch (Exception e)
            {
                LogManager.Log.Info("Exception occur in formatting error message: " + e.Message + ":::" + e.InnerException);
                return errorMessage;
            }
        }

        public static void UpdateWMEStatus(string status, string errMsg)
        {
            LogManager.Log.Info("Update System Mode to " + status + "...");
            string keyStatus = "a3WMEStatus_" + Config.LocationCode;
            string keyError = "a3WMEStoppedDueToErrorDT_" + Config.LocationCode;
            string keySystemMsg = "a3WMELatestSystemMessage_" + Config.LocationCode;

            for (int i = 1; i < 3; i++)
            {
                try
                {
                    using (var wisSyncContext = new WIS_Sync())
                    {
                        var RegistryStatus = wisSyncContext.Registries.Where(m => m.RegKey == keyStatus);
                        Registry registries = new Registry();
                        registries = RegistryStatus.FirstOrDefault();
                        registries.RegValue = status;

                        wisSyncContext.Registries.Attach(registries);
                        wisSyncContext.Entry(registries).Property(u => u.RegValue).IsModified = true;
                        wisSyncContext.SaveChanges();
                    }

                    if (status.ToUpper() == "ERROR")
                    {
                        using (var wisSyncContext = new WIS_Sync())
                        {
                            var RegistryStatus = wisSyncContext.Registries.Where(m => m.RegKey == keyError);
                            Registry registries = new Registry();
                            registries = RegistryStatus.FirstOrDefault();
                            registries.RegValue = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                            wisSyncContext.Registries.Attach(registries);
                            wisSyncContext.Entry(registries).Property(u => u.RegValue).IsModified = true;
                            wisSyncContext.SaveChanges();
                        }
                        if (errMsg.Length > 500)
                        {
                            errMsg = "Validation/Internal error occured.";
                        }

                        LogManager.Log.Info(errMsg);

                        using (var wisSyncContext = new WIS_Sync())
                        {
                            var RegistryStatus = wisSyncContext.Registries.Where(m => m.RegKey == "a3WMELatestSystemMessage_" + Config.LocationCode);
                            Registry registries = new Registry();
                            registries = RegistryStatus.FirstOrDefault();
                            registries.RegValue = errMsg;

                            wisSyncContext.Registries.Attach(registries);
                            wisSyncContext.Entry(registries).Property(u => u.RegValue).IsModified = true;
                            wisSyncContext.SaveChanges();
                        }

                        LogManager.Log.Info("Updated error message in registry.");
                    }

                    break;
                }
                catch (Exception e)
                {
                    string errmsg = "Error Updating the WME Status: <br>" + e.Message + "<br>" + e.InnerException + "<br>";
                    LogManager.Log.Info("Failed to update record to Registry...");

                    if (errmsg.Contains("Could not connect") || errmsg.Contains("The server was not found or was not accessible.") || errmsg.Contains("The underlying provider failed on Open.") || errmsg.Contains("Timeout expired.") || errmsg.Contains("initializing the database") || errmsg.Contains("A transport-level error has occurred"))
                    {
                        LogManager.Log.Info("Retry: " + i);
                        if (i == 3)
                        {
                            LogManager.Log.Error(errmsg);
                            throw new Exception(errmsg);
                        }
                        Thread.Sleep(1000 * 60 * 1);
                    }
                    else
                    {
                        LogManager.Log.Error(errmsg);
                        throw new Exception(errmsg);
                    }
                }
            }

        }

        public static void UpdateWME(string regKey, string regValue)
        {
            LogManager.Log.Info("Update value for " + regKey);

            for (int i = 1; i < 3; i++)
            {
                try
                {
                    using (var wisSyncContext = new WIS_Sync())
                    {
                        var RegistryStatus = wisSyncContext.Registries.Where(m => m.RegKey == regKey);
                        Registry registries = new Registry();
                        registries = RegistryStatus.FirstOrDefault();
                        registries.RegValue = regValue;

                        wisSyncContext.Registries.Attach(registries);
                        wisSyncContext.Entry(registries).Property(u => u.RegValue).IsModified = true;
                        wisSyncContext.SaveChanges();
                    }

                    break;
                }
                catch (Exception e)
                {
                    string errmsg = "Error Updating the WME Status: < br>" + e.Message + "<br>" + e.InnerException + "<br>";
                    LogManager.Log.Info("Failed to update record to Registry...");

                    if (errmsg.Contains("Could not connect") || errmsg.Contains("The server was not found or was not accessible.") || errmsg.Contains("The underlying provider failed on Open.") || errmsg.Contains("Timeout expired.") || errmsg.Contains("initializing the database") || errmsg.Contains("A transport-level error has occurred"))
                    {
                        LogManager.Log.Info("Retry: " + i);
                        if (i == 3)
                        {
                            LogManager.Log.Error(errmsg);
                            throw new Exception(errmsg);
                        }
                        Thread.Sleep(1000 * 60 * 1);
                    }
                    else
                    {
                        LogManager.Log.Error(errmsg);
                        throw new Exception(errmsg);
                    }
                }

            }
        }

        public static string GetSystemMode()
        {
            string status = "";

            for (int i = 1; i < 3; i++)
            {
                try
                {
                    WIS_Sync context = new WIS_Sync();
                    LogManager.Log.Info("Check System mode...");
                    var a3_wme_integration_status = (from a in context.Registries
                                                     where a.RegKey == "a3WMEStatus_" + Config.LocationCode
                                                     select a).FirstOrDefault();
                    status = a3_wme_integration_status.RegValue;
                    LogManager.Log.Info("System mode is..." + status);
                    return status;
                }
                catch (Exception ex)
                {
                    LogManager.Log.Info("Failed to get system mode in Registry table for location " + Config.LocationCode);
                    LogManager.Log.Info(ex.Message);

                    string errmsg = ex.Message + "<br>" + ex.InnerException + "<br>";

                    if (errmsg.Contains("Could not connect") || errmsg.Contains("The server was not found or was not accessible.") || errmsg.Contains("The underlying provider failed on Open.") || errmsg.Contains("Timeout expired.") || errmsg.Contains("initializing the database") || errmsg.Contains("A transport-level error has occurred"))
                    {
                        LogManager.Log.Info("Retry: " + i);
                        if (i == 3)
                        {
                            LogManager.Log.Error(errmsg);
                            throw new Exception(errmsg);
                        }
                        Thread.Sleep(1000 * 60 * 1);
                    }
                    else
                    {
                        LogManager.Log.Info("Error:" + errmsg);
                        throw new Exception(errmsg);
                    }
                }
            }
            return status;
        }

        public static void StopSystem(string systemMode)
        {
            LogManager.Log.Info("Exiting execution due to System Mode is " + systemMode + ".");
            LogManager.Log.Info("Processing End...");
            TS_App.End();
            System.Environment.Exit(1);
        }

        public bool ValidateErrors(int eventId, int a3_id, dynamic result, string nominationFileNumber, string jsonData, DateTime? received_at, string companyName, ref List<string> validationMsg)
        {
            IEnumerable<string> uniqueErrors = null;
            uniqueErrors = validationMsg.Distinct<string>();
            string errorMsg = "";
            StringBuilder warningMessages = new StringBuilder();
            bool isUnlink = false;
            bool isFDAError = false;
            LogManager.Log.Info("Validate Errors...");
            try
            {


                foreach (var item in uniqueErrors.ToList())
                {
                    if (item.Contains("Could not connect") || item.Contains("The server was not found or was not accessible.") || item.Contains("The underlying provider failed on Open.") || item.Contains("Timeout expired.") || item.Contains("initializing the database") || item.Contains("A transport-level error has occurred"))
                    {
                        validationMsg.RemoveAll(i => i.Contains(item));
                        validationMsg.Add("9999:: The server was not found or was not accessible.");
                        return false;
                    }
                    if (item.Contains("Object reference not set to an instance of an object"))
                    {
                        validationMsg.RemoveAll(i => i.Contains(item));
                        validationMsg.Add("0500:: Object reference not set to an instance of an object.");
                        return false;
                    }

                    var errorCode = item.Split(':')[0].ToString();

                    LogManager.Log.Info("errorCode: " + errorCode);

                    var err = (from e in context.ErrorRegistries
                               where e.ErrCode == errorCode
                               select e).FirstOrDefault();

                    if (err.CorrectiveAction != null && err.CorrectiveAction != string.Empty)
                    {
                        var exp = (from e in contextgswallem.Expenses
                                   where e.ID == a3_id
                                   select e).ToList();

                        LogManager.Log.Info("Exp Count: " + exp.Count());
                        if (exp.Count() > 0)
                        {
                            LogManager.Log.Info("Invoice id in expense table: " + exp.FirstOrDefault().INVOICE_ID);
                            int? invId = exp.FirstOrDefault().INVOICE_ID;

                            if (invId != null)
                            {
                                //insert to new table and unlink
                                A3Helper.UnLinkInvoiceExpense(result.entity.ToUpper(), a3_id, invId.Value, result.data.initials, Config.LocationCode);
                                isUnlink = true;
                            }
                        }

                        var errCode = err.ErrCode;
                        var errDesc = err.ErrMessage;
                        var nextActionUser = err.NextActionUser == string.Empty ? "" : "User Action: " + err.NextActionUser;
                        var nextActionHelpdesk = err.NextActionHelpdesk == string.Empty ? "<br>" : "<br>Helpdesk: " + err.NextActionHelpdesk + "<br><br>";

                        warningMessages = warningMessages.Append(errCode + ":: " + errDesc + "<br>" + nextActionUser + nextActionHelpdesk);

                        //delete item in validationMsg
                        validationMsg.RemoveAll(i => i.Contains(item));
                    }
                }

                if (warningMessages.Length != 0)
                {
                    if (validationMsg.Count() == 0)
                    {
                        //update expense table
                        if (result.entity.ToUpper() != "DA")
                        {
                            UpdateGSWallem(a3_id, result.entity);
                            //skip error event                               
                            A3Helper.InsertToA3EventJournal(eventId, result.action.ToUpper(), result.entity, a3_id, jsonData, received_at, 0, "", "", "", "", true, true, "Invalid Event");

                            //remove record in GSRecExported table
                            string tableName = result.entity.ToUpper() != "DA" ? "Expense" : "debitcreditnote";
                            int recId = a3_id;
                            var gsrecexp = context.GSRecExporteds.Where(e => e.GSTableName == tableName && e.GSRecID == recId);

                            if (gsrecexp.Count() > 0)
                            {
                                context.GSRecExporteds.Remove(gsrecexp.FirstOrDefault());
                                context.SaveChanges();
                                LogManager.Log.Info("Successfully removed GSRecId " + recId + " in the GSRecExported table");
                            }
                        }
                        else
                        {
                            var dc = (from d in contextgswallem.debitcreditnotes
                                      where d.DCN_ID == a3_id
                                      select d).ToList();

                            LogManager.Log.Info("DCN Count: " + dc.Count());
                            if (dc.Count() > 0)
                            {
                                string userId = dc.FirstOrDefault().DCN_HISTORY.Split('-')[2];
                                LogManager.Log.Info("initials: " + userId);

                                var dcn = context.GATShipSupportLists.Where(m => m.ForeignKeyId == a3_id && m.Status == "ToDo");

                                if (dcn.Count() == 0)
                                {
                                    //insert to new table
                                    A3Helper.InsertToSupportList(result.entity.ToUpper(), eventId, a3_id, int.Parse(result.data.dcnNumber), userId, Config.LocationCode, nominationFileNumber, null, result.data.dcnNumber);
                                }
                                isUnlink = false;
                            }

                            //skip error event                               
                            A3Helper.InsertToA3EventJournal(eventId, result.action.ToUpper(), result.entity, a3_id, jsonData, received_at, 0, "", "", "", "", true, true, "Invalid Event");
                        }

                        errorMsg = FormatErrorMsg(eventId, nominationFileNumber, received_at, companyName, result, jsonData, isUnlink);
                        errorMsg = errorMsg.Replace("{error}", warningMessages.ToString());
                        var initials = "";

                        if (result.entity.ToUpper() != "DA")
                        {
                            initials = result.data.initials;
                            if (initials != "")
                            {
                                initials = "," + initials + "@wallem.com";
                            }
                        }
                        LogManager.Log.Info("initials: " + initials);

                        Hierarchy hier = log4net.LogManager.GetRepository() as Hierarchy;
                        //var smtpappender = (ExtendedSmtpAppender)hier.GetAppenders().Where(appender => appender.Name.Equals("SmtpAppender", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                        //LogManager.Log.Info("smtpappento.To: " + smtpappender.To);
                        //LogManager.Log.Info("initials: " + smtpappender.To);
                        //var emailTo = smtpappender.To.Contains(initials) ? smtpappender.To : smtpappender.To + (initials == "" ? "" : initials);
                        //LogManager.Log.Info("emailTo: " + smtpappender.To);
                        //smtpappender.To = emailTo.TrimEnd(',');
                        //LogManager.Log.Info("smtpappento.To: " + smtpappender.To);
                        //smtpappender.ActivateOptions();

                        //LogManager.Log.Warn(warningMessages);
                        //A3Helper.SendMail(Config.fromWarn, Config.toWarn + initials, Config.ccWarn, Config.bccWarn, Config.subjectWarn.Replace("Warning", "Error"), warningMsg + "<br>" + Config.footerWarn.Replace("This is only a warning message regarding the case. ", string.Empty));
                        LogManager.Log.Error(errorMsg);

                        return true;
                    }
                    else
                    {
                        validationMsg = uniqueErrors.ToList();
                    }
                }

                return false;
            }
            catch (Exception ex)
            {

                LogManager.Log.Error("Error in ValidateErrors..." + ex.Message + "::" + ex.InnerException);
                return false;
            }
        }

        public static void UpdateGSWallem(int? id, string entity)
        {
            LogManager.Log.Info("Udpdating Batch_No field with id " + id + " in gs_wallem.dbo.Expense ...");

            for (int i = 1; i < 3; i++)
            {
                try
                {
                    using (var gsWallemContext = new GSWallem())
                    {
                        var batchNo = gsWallemContext.Expenses.Where(m => m.ID == id).Select(m => m).ToList();
                        Expense expense = new Expense();
                        expense = batchNo.FirstOrDefault();
                        expense.BATCH_NO = null;
                        expense.BUDGET = 1;

                        gsWallemContext.Expenses.Attach(expense);
                        gsWallemContext.Entry(expense).Property(u => u.BATCH_NO).IsModified = true;
                        gsWallemContext.Entry(expense).Property(u => u.BUDGET).IsModified = true;
                        gsWallemContext.SaveChanges();
                        LogManager.Log.Info("Successfully updated Batch_No field with id " + id + " ...");
                    }

                    break;
                }
                catch (Exception ex)
                {
                    LogManager.Log.Info("Failed to update Batch_No field with id " + id + " in gs_wallem.dbo.Expense ...");
                    LogManager.Log.Info(ex.Message);

                    string errmsg = ex.Message + "<br>" + ex.InnerException + "<br>";

                    if (errmsg.Contains("Could not connect") || errmsg.Contains("The server was not found or was not accessible.") || errmsg.Contains("The underlying provider failed on Open.") || errmsg.Contains("Timeout expired.") || errmsg.Contains("initializing the database") || errmsg.Contains("A transport-level error has occurred"))
                    {
                        LogManager.Log.Info("Retry: " + i);
                        if (i == 3)
                        {
                            LogManager.Log.Error(errmsg);
                            throw new Exception(errmsg);
                        }
                        Thread.Sleep(1000 * 60 * 1);
                    }
                    else
                    {
                        LogManager.Log.Error(errmsg);
                        throw new Exception(errmsg);
                    }
                }
            }

        }


        public static void UpdateDynaValues(int? id, string invoicWithSuffixForDAComplete)
        {
            LogManager.Log.Info("Udpdating Value_text field with id " + id + " in gs_wallem.dbo.Dyna_Values ...");

            for (int i = 1; i < 3; i++)
            {
                try
                {
                    using (var gsWallemContext = new GSWallem())
                    {
                        var dynaFieldId = gsWallemContext.dyna_fields.Where(n => n.INTEGRATION_REFERENCE == "3050").Select(n => n.ID).FirstOrDefault();
                        var valueText = gsWallemContext.dyna_values.Where(m => m.FOREIGN_KEY_INT == id && m.FIELD_ID == dynaFieldId).Select(m => m).ToList();
                        dyna_values dyna_values = new dyna_values();
                        dyna_values = valueText.FirstOrDefault();
                        dyna_values.VALUE_TEXT = invoicWithSuffixForDAComplete;

                        gsWallemContext.dyna_values.Attach(dyna_values);
                        gsWallemContext.Entry(dyna_values).Property(u => u.VALUE_TEXT).IsModified = true;
                        gsWallemContext.SaveChanges();
                        LogManager.Log.Info("Successfully updated value_text field with id " + id + " ...");
                    }

                    break;
                }
                catch (Exception ex)
                {
                    LogManager.Log.Info("Failed to update value_text field with id " + id + " in gs_wallem.dbo.Dyna_Values ...");
                    LogManager.Log.Info(ex.Message);

                    string errmsg = ex.Message + "<br>" + ex.InnerException + "<br>";

                    if (errmsg.Contains("Could not connect") || errmsg.Contains("The server was not found or was not accessible.") || errmsg.Contains("The underlying provider failed on Open.") || errmsg.Contains("Timeout expired.") || errmsg.Contains("initializing the database") || errmsg.Contains("A transport-level error has occurred"))
                    {
                        LogManager.Log.Info("Retry: " + i);
                        if (i == 3)
                        {
                            LogManager.Log.Error(errmsg);
                            throw new Exception(errmsg);
                        }
                        Thread.Sleep(1000 * 60 * 1);
                    }
                    else
                    {
                        LogManager.Log.Info("Error:" + errmsg);
                        throw new Exception(errmsg);
                    }
                }
            }
        }

        public static void UnLinkInvoiceExpense(string entity, int expId, int invoiceId, string initials, string locCode)
        {
            LogManager.Log.Info("Unlink record in Expense table...");

            for (int i = 1; i < 3; i++)
            {
                try
                {
                    var wisSyncContext = new WIS_Sync();
                    var invoice = wisSyncContext.GATShipSupportLists.Where(m => m.ForeignKeyId == expId && m.Status == "ToDo");

                    if (invoice.Count() == 0)
                    {
                        //link invoice no
                        GSWallem contextGSWallem = new GSWallem();
                        var exp = (from e in contextGSWallem.Expenses
                                   where e.ID == expId
                                   select e).FirstOrDefault();

                        LogManager.Log.Info("Getting portcall number...");
                        var portCallNo = (from e in contextGSWallem.Expenses
                                          join pc in contextGSWallem.PortCalls on e.PORTCALL_ID equals pc.ID
                                          where e.ID == expId
                                          select pc.PORTCALL_NUMBER).FirstOrDefault();

                        LogManager.Log.Info("Getting invoice number...");
                        var invoiceNumber = (from e in contextGSWallem.Expenses
                                             join d in contextGSWallem.debitcreditnotes on e.INVOICE_ID equals d.DCN_ID
                                             where e.ID == expId
                                             select d.DCN_NUMBER).FirstOrDefault();

                        LogManager.Log.Info("Updating record in Expense table...");
                        Expense expense = new Expense();
                        expense = exp;
                        expense.INVOICE_ID = null;
                        contextGSWallem.Expenses.Attach(expense);
                        contextGSWallem.Entry(expense).Property(e => e.INVOICE_ID).IsModified = true;
                        contextGSWallem.SaveChanges();
                        LogManager.Log.Info("Successfully updated record in Expense table...");

                        //Insert record in GATShipSupportList
                        InsertToSupportList(entity, 0, expId, invoiceId, initials, locCode, portCallNo, exp.REF_NUMBER, invoiceNumber);
                    }


                    break;
                }
                catch (Exception ex)
                {
                    LogManager.Log.Info("Failed to update expense record with id " + expId + " in gs_wallem.dbo.Expense ...");
                    LogManager.Log.Info(ex.Message);

                    string errmsg = ex.Message + "<br>" + ex.InnerException + "<br>";

                    if (errmsg.Contains("Could not connect") || errmsg.Contains("The server was not found or was not accessible.") || errmsg.Contains("The underlying provider failed on Open.") || errmsg.Contains("Timeout expired.") || errmsg.Contains("initializing the database") || errmsg.Contains("A transport-level error has occurred"))
                    {
                        LogManager.Log.Info("Retry: " + i);
                        if (i == 3)
                        {
                            LogManager.Log.Error(errmsg);
                            throw new Exception(errmsg);
                        }
                        Thread.Sleep(1000 * 60 * 1);
                    }
                    else
                    {
                        LogManager.Log.Info("Error:" + errmsg);
                        throw new Exception(errmsg);
                    }
                }
            }
        }

        public static void InsertToSupportList(string entity, int eventId, int expId, int invoiceId, string initials, string locCode, string portCallNo, int? refNo, string invoiceNumber)
        {
            var wisSyncContext = new WIS_Sync();

            //Insert record in GATShipSupportList
            LogManager.Log.Info("Insert record in GATShipSupportList table...");
            for (int i = 1; i < 3; i++)
            {
                try
                {
                    GATShipSupportList inv = new GATShipSupportList();
                    inv.Initials = initials;
                    inv.ForeignKeyId = expId;
                    inv.Status = "ToDo";
                    inv.InvoiceId = invoiceId;
                    inv.LocCode = locCode;
                    inv.CreatedDt = DateTime.Now;
                    if (entity != "DA")
                    {
                        inv.Remarks = "WME unlinks invoice ID.  Pending to link Invoice ID back to expense record after posting. Portcall No.: " + portCallNo + "; Reference No: " + refNo + "; Invoice No.: " + invoiceNumber;
                    }
                    else
                    {
                        inv.Remarks = "Pending to repost FDA (by script according to error code on the alert sent). Portcall No.: " + portCallNo + "; Invoice No.: " + invoiceNumber + "; Event ID: " + eventId;
                    }

                    wisSyncContext.GATShipSupportLists.Add(inv);
                    wisSyncContext.SaveChanges();
                    LogManager.Log.Info("Successfully inserted record in GATShipSupportList table...");
                    break;
                }
                catch (Exception ex)
                {
                    LogManager.Log.Info("Failed to insert foreign key id " + expId + " in GATShipSupportList table...");
                    LogManager.Log.Info(ex.Message);

                    string errmsg = ex.Message + "<br>" + ex.InnerException + "<br>";

                    if (errmsg.Contains("Could not connect") || errmsg.Contains("The server was not found or was not accessible.") || errmsg.Contains("The underlying provider failed on Open.") || errmsg.Contains("Timeout expired.") || errmsg.Contains("initializing the database") || errmsg.Contains("A transport-level error has occurred"))
                    {
                        LogManager.Log.Info("Retry: " + i);
                        if (i == 3)
                        {
                            LogManager.Log.Error(errmsg);
                            throw new Exception(errmsg);
                        }
                        Thread.Sleep(1000 * 60 * 1);
                    }
                    else
                    {
                        LogManager.Log.Info("Error:" + errmsg);
                        throw new Exception(errmsg);
                    }
                }
            }
        }

        public static void LinkInvoiceExpense(int expId)
        {
            LogManager.Log.Info("Link record in Expense table...");

            for (int i = 1; i < 3; i++)
            {
                try
                {
                    var wisSyncContext = new WIS_Sync();
                    var invoice = wisSyncContext.GATShipSupportLists.Where(m => m.ForeignKeyId == expId && m.Status == "ToDo");

                    if (invoice.Count() > 0)
                    {
                        //link invoice no
                        GSWallem contextGSWallem = new GSWallem();
                        var exp = (from e in contextGSWallem.Expenses
                                   where e.ID == expId
                                   select e).FirstOrDefault();

                        LogManager.Log.Info("Getting portcall number...");
                        var portCallNo = (from e in contextGSWallem.Expenses
                                          join pc in contextGSWallem.PortCalls on e.PORTCALL_ID equals pc.ID
                                          where e.ID == expId
                                          select pc.PORTCALL_NUMBER).FirstOrDefault();

                        LogManager.Log.Info("Getting invoice number...");
                        var invNo = invoice.FirstOrDefault().InvoiceId;
                        var invoiceNumber = (from d in contextGSWallem.debitcreditnotes
                                             where d.DCN_ID == invNo
                                             select d.DCN_NUMBER).FirstOrDefault();

                        LogManager.Log.Info("Updating record in Expense table...");
                        Expense expense = new Expense();
                        expense = exp;
                        expense.INVOICE_ID = invoice.FirstOrDefault().InvoiceId;
                        contextGSWallem.Expenses.Attach(expense);
                        contextGSWallem.Entry(expense).Property(e => e.INVOICE_ID).IsModified = true;
                        contextGSWallem.SaveChanges();
                        LogManager.Log.Info("Successfully updated record in Expense table...");

                        //update record in GATShipSupportList
                        LogManager.Log.Info("Updating record in GATShipSupportList table...");
                        GATShipSupportList inv = new GATShipSupportList();
                        inv = invoice.FirstOrDefault();
                        inv.Status = "Done";
                        inv.UpdatedDt = DateTime.Now;
                        inv.Remarks = "WME linked Invoice ID back to expense record after posting. . Portcall No.: " + portCallNo + "; Reference No: " + exp.REF_NUMBER + "; Invoice No.: " + invoiceNumber;

                        wisSyncContext.GATShipSupportLists.Attach(inv);
                        wisSyncContext.Entry(inv).Property(u => u.Status).IsModified = true;
                        wisSyncContext.Entry(inv).Property(u => u.UpdatedDt).IsModified = true;
                        wisSyncContext.Entry(inv).Property(u => u.Remarks).IsModified = true;
                        wisSyncContext.SaveChanges();
                        LogManager.Log.Info("Successfully updated record in GATShipSupportList table...");
                    }


                    break;
                }
                catch (Exception ex)
                {
                    LogManager.Log.Info("Failed to update expense record with id " + expId + " in gs_wallem.dbo.Expense ...");
                    LogManager.Log.Info(ex.Message);

                    string errmsg = ex.Message + "<br>" + ex.InnerException + "<br>";

                    if (errmsg.Contains("Could not connect") || errmsg.Contains("The server was not found or was not accessible.") || errmsg.Contains("The underlying provider failed on Open.") || errmsg.Contains("Timeout expired.") || errmsg.Contains("initializing the database") || errmsg.Contains("A transport-level error has occurred"))
                    {
                        LogManager.Log.Info("Retry: " + i);
                        if (i == 3)
                        {
                            LogManager.Log.Error(errmsg);
                            throw new Exception(errmsg);
                        }
                        Thread.Sleep(1000 * 60 * 1);
                    }
                    else
                    {
                        LogManager.Log.Info("Error:" + errmsg);
                        throw new Exception(errmsg);
                    }
                }
            }
        }
    }
}
