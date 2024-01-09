using log4net;
using System;
using System.Configuration;
using System.IO;
using System.Net;

namespace GATShipD365TS.App_Code
{
    public class Config
    {
        //for testing purpose only
        public static int id = int.Parse(ConfigurationManager.AppSettings["id"]);
        public static bool bypassValidation = bool.Parse(ConfigurationManager.AppSettings["bypassValidation"]);
        #region Configuration Fields        
        public const string SystemCreatedBy = "WME";

        public static string LocationCode = GetValue(ConfigurationManager.AppSettings["LocationCode"]);   
        //Get Wallem Settings in App Config
        public static string LogFolder = GetValue(ConfigurationManager.AppSettings["LogFolder"]);
        public static string ProcessedFolder = GetValue(ConfigurationManager.AppSettings["ProcessedFolder"]);
        public static string wmeToken = GetValue(ConfigurationManager.AppSettings["variable"]);
        public static string numberOfRecordsInEachQuery = GetValue(ConfigurationManager.AppSettings["NumberOfRecordsInEachQuery"]);
        public static string numberOfMinutesOldOfEvent = GetValue(ConfigurationManager.AppSettings["NumberOfMinutesOldOfEvent"]);
        public static bool isNotifySuffix = bool.Parse(ConfigurationManager.AppSettings["IsNotifySuffix"]);

        //List of Events
        public static string ListOfEvents = GetValue(ConfigurationManager.AppSettings["ListOfEvents"]);

        //Set Parameter values for Log4net
        public static string SystemCode = GetValue(ConfigurationManager.AppSettings["SystemCode"]);
        public static readonly string ProcessName = "Process" + Config.SystemCode + "Events";

        //Sending of Warn Notification
        public static string toWarn = GetValue(ConfigurationManager.AppSettings["toWarn"]);
        public static string fromWarn = GetValue(ConfigurationManager.AppSettings["fromWarn"]);
        public static string ccWarn = GetValue(ConfigurationManager.AppSettings["ccWarn"]);
        public static string bccWarn = GetValue(ConfigurationManager.AppSettings["bccWarn"]);
        public static string subjectWarn = GetValue(ConfigurationManager.AppSettings["subjectWarn"]);
        public static string SMTPHost = GetValue(ConfigurationManager.AppSettings["SMTPHost"]);
        public static string headerWarn = "Hi, <br><br>Good Day, <br><br>This is to notify you that there are unusual data on <b>" + Config.SystemCode + "</b> with Process Name : <b>" + Config.ProcessName + "</b>. <br>Execution Date and Time:&nbsp; " + "<b>" + DateTime.Now.ToString() + "</b>" + "<br><br><b>Information Details</b> : <br><br>";
        public static string footerWarn = "<br><br>This is only a warning message regarding the case.  The WME has processed the event accordingly. Should you need verification or assistance on this, kindly raise Helpdesk case.";

        //AX
        //public static string tokenEndpoint = GetValue(ConfigurationManager.AppSettings["tokenEndpoint"]);
        //public static string clientId = GetValue(ConfigurationManager.AppSettings["clientId"]);
        //public static string clientSecret = GetValue(ConfigurationManager.AppSettings["clientSecret"]);
        //public static string apiUrl = GetValue(ConfigurationManager.AppSettings["apiUrl"]);
        //public static string grantType = GetValue(ConfigurationManager.AppSettings["grantType"]);
        //public static string resource = GetValue(ConfigurationManager.AppSettings["resource"]);

        //For Testing
        public static string batchidcounter = GetValue(ConfigurationManager.AppSettings["batchidcounter"]);

        //for error
        public static string subjectError = GetValue(ConfigurationManager.AppSettings["subjectError"]);
        public static string footerError = GetValue(ConfigurationManager.AppSettings["footerError"]);
        


        #endregion Configuration Fields

        public static void InitConfig()
        {
            string host = Environment.MachineName;
            var ipaddress = Dns.GetHostAddresses(host);

            

            GlobalContext.Properties["date"] = DateTime.Now;
            GlobalContext.Properties["action"] = SystemCode; //Set Action
            GlobalContext.Properties["mailheader"] = "Hi, <br><br>Good Day, <br><br>This is to notify you that there are invalid data on <b>"+SystemCode+"</b> with Process Name : <b>" + ProcessName.ToString() + "</b>. <br>Execution Date and Time:&nbsp; " + "<b>"+DateTime.Now.ToString() +"</b>" + "<br><br><b>Information Details</b> : <br><br>";            
          //GlobalContext.Properties["mailfooter"] = "<br><br> *** This is a system generated message.  Please contact A3Support@wallem.com if needed. ***";                       
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


        private static string GetValue(string appSetting)
        {
            if (string.IsNullOrEmpty(appSetting)) return "";
            else return appSetting.ToString().Trim();
        }

        public enum Status
        {
            Failed,
            Success,
            Active,
            Retry,
            Locked
        }

        public enum A3Folders
        {
            FundsAdditional,
            FundsAdvance,
            FundsFinal,
            FundsExGL_AR,
            FundsExGL_C,
            FundsBkChg,
            Invoice,
            InvoiceCreditNote,
            InvoiceDeditNote,
            DAComplete
        }
    }
}
