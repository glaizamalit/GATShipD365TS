using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GATShipD365TS.App_Code;
using GATShipD365TS.GATShip.Entities;
using GATShipD365TS.Helper;
using GATShipD365TS.Models;
using Newtonsoft.Json;
using static GATShipD365TS.xxProgram;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Runtime.Remoting.Contexts;
using Newtonsoft.Json.Converters;
using System.IO;
using System.Net;

namespace GATShipD365TS
{
    public class D365FO
    {
        WIS_Sync context = new WIS_Sync();
        string clientId = string.Empty;
        string clientSecret = string.Empty;
        string grantType = string.Empty;
        string resource = string.Empty;
        string tokenEndpoint = string.Empty;
        string apiUrl = string.Empty;
        string accessToken = string.Empty;
        public async Task<string> GetAccessTokenAsync()
        {
            WIS_Sync context = new WIS_Sync();
            var d365 = (from e in context.Registries
                        where e.RegKey.Contains("D365")
                        select e);

            string tokenEndpoint = d365.Where(c => c.RegKey == "D365TokenEndpoint").FirstOrDefault().RegValue;

            // Your client ID and client secret obtained during OAuth 2.0 registration

            clientId = d365.Where(c => c.RegKey == "D365ClientId").FirstOrDefault().RegValue;
            clientSecret = d365.Where(c => c.RegKey == "D365ClientSecret").FirstOrDefault().RegValue;
            grantType = d365.Where(c => c.RegKey == "D365GrantType").FirstOrDefault().RegValue;
            resource = d365.Where(c => c.RegKey == "D365InstanceUrl").FirstOrDefault().RegValue;
            accessToken = string.Empty;


            // Your API endpoint
            apiUrl = d365.Where(c => c.RegKey == "D365GATShipImport").FirstOrDefault().RegValue;


            using (HttpClient client = new HttpClient())
            {
                //client.DefaultRequestHeaders.Add("Tenant_id", "633c5125-0c78-4d5a-99cf-e6306a750607");
                //client.DefaultRequestHeaders.Add("Content-Type", "application/json; charset=utf-8");

                // Prepare the request content for token endpoint
                var tokenRequestContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", grantType),
                    new KeyValuePair<string, string>("client_id", clientId),
                    new KeyValuePair<string, string>("client_secret", clientSecret),
                    new KeyValuePair<string, string>("resource", resource)
                });

                // Make a POST request to the token endpoint to get the access token
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpResponseMessage tokenResponse = await client.PostAsync(tokenEndpoint, tokenRequestContent);

                // Check if the request was successful (status code 200)
                if (tokenResponse.IsSuccessStatusCode)
                {
                    // Read and parse the response content to get the access token
                    var tokenResponseData = await tokenResponse.Content.ReadAsStringAsync();

                    TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(tokenResponseData);
                    accessToken = token.access_token;
                }
            }

            return accessToken;
        }


        public async Task<string> PostData(D365FOJournal journalEntry)
        {
            LogManager.Log.Info("Start posting of journal entry to D365...");
            string retValue = "";
            string jsonRet = "{\"Message\":{\"Status\":\"\",\"Msg\":\"\",\"Submitted\":\"\"}}";
            JsonResponse jsonResponse = new JsonResponse();
            string submittedJson = "";
            A3Helper a3Helper = new A3Helper();
            WIS_Sync context = new WIS_Sync();
            try
            {
                D365FOJournal xjournalEntry = new D365FOJournal();
                D365FOHeader headerx = new D365FOHeader();
                headerx.CompanyCode = "test";

                headerx.Description = "test";
                headerx.FileBatchID = "test";

                List<D365FODetail> journalDetail = new List<D365FODetail>();
                string checknumRetryField = "a3WMECheckNumOfRetry" + "_" + Config.LocationCode;
                var retry = int.Parse((from v in context.Registries
                                       where v.RegKey == checknumRetryField
                                       select v.RegValue).FirstOrDefault().ToString());

                string systemMode = A3Helper.GetSystemMode();
                var success = false;
                //Do Check_Error_System_Status subroutine
                if (systemMode.Trim().ToUpper() == "LOCKED")
                {


                    D365FOHeader header = new D365FOHeader();

                    // Create instances of the entities that are used in the service and
                    // set the needed fields on those entities.
                    header.CompanyCode = journalEntry._gatshipData.CompanyCode;
                    header.TransType = journalEntry._gatshipData.TransType;
                    header.FileBatchID = journalEntry._gatshipData.FileBatchID + Config.batchidcounter;
                    header.Description = journalEntry._gatshipData.Description;

                    List<D365FODetail> details = new List<D365FODetail>();
                    foreach (var line in journalEntry._gatshipData.Lines)
                    {
                        D365FODetail detail = new D365FODetail();
                        detail.FileBatchID = line.FileBatchID + Config.batchidcounter;
                        detail.TransDate = line.TransDate;
                        detail.AccountType = line.AccountType;
                        detail.Account = line.Account;
                        detail.VesselCustomerCode = line.VesselCustomerCode;
                        detail.VoyageCode = line.VoyageCode;
                        detail.RevenueType = line.RevenueType;                                       
                        detail.Currency = line.Currency;
                        detail.Amount = line.Amount;
                        detail.LineDescription = line.LineDescription;
                        detail.ExchangeRate = line.ExchangeRate;
                        detail.SalesTaxGroup = line.SalesTaxGroup;
                        detail.ItemSalesTaxGroup = line.ItemSalesTaxGroup;
                        detail.SalesTaxAmount = line.SalesTaxAmount;
                        detail.Invoice = line.Invoice;
                        detail.Document = line.Document;
                        detail.DocumentDate = line.DocumentDate;
                        detail.DueDate = line.DueDate;
                        detail.Payment = line.Payment;
                        detail.BankTransType = line.BankTransType;
                        detail.CreatedByUserId = line.CreatedByUserId;
                        detail.UserField1 = line.UserField1;
                        detail.UserField1 = line.UserField2;
                        detail.UserField3 = line.UserField3;
                        detail.Remark = line.Remark;                    

                        details.Add(detail);
                    }


                    // Create an instance of the document class.
                    LogManager.Log.Info("Creating document class...");


                    //instantiating THK_AIFA3GLJournal
                    D365FOJournal a3journal = new D365FOJournal();


                    //adding line details to THK_AIFA3GLLine in an array form then adding it to header
                    //header.THK_AIFA3GLLine = details.ToArray();

                    //adding header in an array form to THK_AIFA3GLJournal
                    //Entity_THK_AIFA3GLHeader[] headerArr = new Entity_THK_AIFA3GLHeader[1] { header };

                    a3journal._gatshipData = header;
                    a3journal._gatshipData.Lines = details;


                    for (int i = 1; i <= retry; i++)
                    {
                        // Instantiate an instance of the service client class.
                        LogManager.Log.Info("Connecting to D365FO...");
                        try
                        {
                            // Create an HttpClient instance
                            using (HttpClient client = new HttpClient())
                            {
                                //string accessToken = token.access_token;
                                string accessToken = await GetAccessTokenAsync();

                                // Set the authorization header with the access token
                                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
                                //client.DefaultRequestHeaders.Add("Content-Type", "application/json;charset=utf-8");
                                client.DefaultRequestHeaders.Add("Accept", "application/json");
                                client.DefaultRequestHeaders.Add("Prefer", "return=representation");

                                try
                                {
                                    // Make a Post request to the API                        
                                    LogManager.Log.Info("Posting document class to D365 service...");
                                    HttpContent content = new StringContent(JsonConvert.SerializeObject(a3journal), Encoding.UTF8, "application/json");
                                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                                    HttpResponseMessage apiResponse = await client.PostAsync(apiUrl, content);

                                    LogManager.Log.Info("Response...");

                                    // Check if the request to the API was successful (status code 200)
                                    if (apiResponse.IsSuccessStatusCode)
                                    {
                                        // Read and print the response content
                                        jsonRet = await apiResponse.Content.ReadAsStringAsync();
                                       // jsonResponse = a3Helper.ReadingResponseFromD365(jsonRet);
                                        jsonResponse = JsonConvert.DeserializeObject<JsonResponse>(jsonRet);

                                        LogManager.Log.Info("jsonStatus: " + jsonResponse.Status + "jsonReturnMsg: " + jsonResponse.ReturnMsg);
                                        Console.WriteLine(jsonRet);
                                    }
                                    else
                                    {
                                        Console.WriteLine($"Error: {apiResponse.StatusCode} - {apiResponse.ReasonPhrase}");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"API Exception: {ex.Message}");
                                }
                            }

                            //serialize object to json                            
                            JsonSerializer serializer = new JsonSerializer();
                            serializer.Converters.Add(new JavaScriptDateTimeConverter());
                            serializer.NullValueHandling = NullValueHandling.Ignore;

                            var stringWriter = new System.IO.StringWriter();                                                        
                            serializer.Serialize(stringWriter, a3journal);                          
                            submittedJson = JsonConvert.SerializeObject(stringWriter.ToString(), Newtonsoft.Json.Formatting.Indented);  


                            LogManager.Log.Info("Response...");                                                       
                            jsonRet = "{\"$id\":\"" + jsonResponse.id + "\",\"Status\":\"" + jsonResponse.Status + "\",\"ReturnMsg\":\"" + jsonResponse.ReturnMsg + "\",\"Submitted\":" + submittedJson + "}";
                            
                            success = jsonResponse.Status == 1 ? true : false;
                            if (success)
                            {
                                break;
                            }
                        }
                        catch (Exception e)
                        {
                            string errmsg = "";
                            if (e.Message == null || e.Message == "")
                            {
                                errmsg = e.StackTrace;
                            }
                            else
                            {
                                errmsg = e.Message + " " + e.InnerException;

                            }

                            LogManager.Log.Info("Posting of journal entry to D365 has failed...");
                            LogManager.Log.Info(jsonRet);
                            if (!errmsg.Contains("Could not connect"))
                            {
                                throw new Exception(errmsg);

                            }
                            else
                            {
                                LogManager.Log.Info("Retry: " + i);
                                if (retry == i)
                                {
                                    throw new Exception(errmsg);
                                }
                            }
                        }
                        Thread.Sleep(1000 * 60 * 1);
                    }

                }
                else
                {
                    A3Helper.StopSystem(systemMode);
                }

                return jsonRet;
            }
            catch (Exception ex)
            {
                jsonRet = "{\"Status\":\"Failed\",\"ReturnMsg\":\"" + ex.Message + ex.InnerException + "\",\"Submitted\":\"" + submittedJson + "\"}}";
                return jsonRet;
            }
        }

        //public string PostData(THK_AIFA3GLJournal journalEntry)
        //{
        //    LogManager.Log.Info("Start posting of journal entry to D365...");
        //    string retValue = "";
        //    string jsonRet = "{\"Message\":{\"Status\":\"\",\"Msg\":\"\",\"Submitted\":\"\"}}";
        //    string submittedJson = "";
        //    A3Helper a3Helper = new A3Helper();
        //    WIS_Sync context = new WIS_Sync();
        //    try
        //    {
        //        string checknumRetryField = "a3WMECheckNumOfRetry" + "_" + Config.LocationCode;
        //        var retry = int.Parse((from v in context.Registries
        //                               where v.RegKey == checknumRetryField
        //                               select v.RegValue).FirstOrDefault().ToString());

        //        string systemMode = A3Helper.GetSystemMode();
        //        var success = false;
        //        //Do Check_Error_System_Status subroutine
        //        if (systemMode.Trim().ToUpper() == "LOCKED")
        //        {

        //            for (int i = 1; i <= retry; i++)
        //            {
        //                // Instantiate an instance of the service client class.
        //                LogManager.Log.Info("Instantiating AX web service...");
        //                try
        //                {

        //                    //THK_AIFA3GLJournalServiceClient proxy = new THK_AIFA3GLJournalServiceClient();

        //                    //proxy.ClientCredentials.Windows.ClientCredential.Domain = Config.domain;
        //                    //proxy.ClientCredentials.Windows.ClientCredential.UserName = Config.userName;
        //                    //proxy.ClientCredentials.Windows.ClientCredential.Password = Config.password;
        //                    // Create an instance of the CallContext class.
        //                    // CallContext callContext = new CallContext();
        //                    // Set the value for Company.
        //                    //callContext.Company = Config.company;
        //                    // Set the value for LogonAsUser.
        //                    //callContext.LogonAsUser = Config.domain + "\\" + Config.userName;



        //                    System.Xml.Serialization.XmlSerializer xml = new System.Xml.Serialization.XmlSerializer(journalEntry.GetType());
        //                    var stringWriter = new System.IO.StringWriter();
        //                    xml.Serialize(stringWriter, journalEntry);
        //                    submittedJson = stringWriter.ToString();

        //                    // Call the create method on the service passing in the document.
        //                    LogManager.Log.Info("Posting document class to AX web service...");
        //                    //EntityKey[] returnedEntityKey = proxy.create(callContext, journalEntry);


        //                    // The create method returns an EntityKey.
        //                    //EntityKey returnedEntityData = (EntityKey)returnedEntityKey.GetValue(0);

        //                    //retValue = returnedEntityData.KeyData[0].Value;

        //                    LogManager.Log.Info("Response...");

        //                    jsonRet = "{\"Message\":{\"Status\":\"Success\",\"Msg\":\"" + "Entity Key: " + retValue + "\",\"Submitted\":\"" + submittedJson + "\"}}";
        //                    var response = a3Helper.ReadingResponseFromD365(jsonRet);
        //                    success = response.Message.Status == Config.Status.Success.ToString() ? true : false;
        //                    if (success)
        //                    {
        //                        break;
        //                    }
        //                }
        //                catch (Exception e)
        //                {
        //                    string errmsg = "";
        //                    if (e.Message == null || e.Message == "")
        //                    {
        //                        errmsg = e.StackTrace;
        //                    }
        //                    else
        //                    {
        //                        errmsg = e.Message + " " + e.InnerException;

        //                    }

        //                    LogManager.Log.Info("Posting of journal entry to D365 has failed...");
        //                    LogManager.Log.Info(jsonRet);
        //                    if (!errmsg.Contains("Could not connect"))
        //                    {
        //                        throw new Exception(errmsg);

        //                    }
        //                    else
        //                    {
        //                        LogManager.Log.Info("Retry: " + i);
        //                        if (retry == i)
        //                        {
        //                            throw new Exception(errmsg);
        //                        }
        //                    }
        //                }

        //            }

        //        }
        //        else
        //        {
        //            A3Helper.StopSystem(systemMode);
        //        }

        //        return jsonRet;
        //    }
        //    catch (Exception ex)
        //    {

        //        jsonRet = "{\"Message\":{\"Status\":\"Failed\",\"Msg\":\"" + ex.Message + ex.InnerException + "\",\"Submitted\":\"" + submittedJson + "\"}}";
        //        return jsonRet;
        //    }
        //}
    }
}
