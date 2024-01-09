using GATShipD365TS.App_Code;
using GATShipD365TS.GATShip.Entities;
using GATShipD365TS.Helper;
using GATShipD365TS.Models;
using log4net.Repository.Hierarchy;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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
    class xxProgram
    {

        static async Task xxMain()
        {
            WIS_Sync context = new WIS_Sync();
            var d365 = (from e in context.Registries
                        where e.RegKey.Contains("D365")
                        select e);

            string tokenEndpoint = d365.Where(c => c.RegKey == "D365TokenEndpoint").FirstOrDefault().RegValue;

            // Your client ID and client secret obtained during OAuth 2.0 registration
            string clientId = d365.Where(c => c.RegKey == "D365ClientId").FirstOrDefault().RegValue;
            string clientSecret = d365.Where(c => c.RegKey == "D365ClientSecret").FirstOrDefault().RegValue;
            string grantType = d365.Where(c => c.RegKey == "D365GrantType").FirstOrDefault().RegValue;
            string resource = d365.Where(c => c.RegKey == "D365InstanceUrl").FirstOrDefault().RegValue;

            // Your API endpoint
            string apiUrl = d365.Where(c => c.RegKey == "D365GATShipImport").FirstOrDefault().RegValue;

            // Create an HttpClient instance
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
                HttpResponseMessage tokenResponse = await client.PostAsync(tokenEndpoint, tokenRequestContent);

                // Check if the request was successful (status code 200)
                if (tokenResponse.IsSuccessStatusCode)
                {
                    // Read and parse the response content to get the access token
                    var tokenResponseData = await tokenResponse.Content.ReadAsStringAsync();

                    TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(tokenResponseData);
                    string accessToken = token.access_token;

                    // Set the authorization header with the access token
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
                    //client.DefaultRequestHeaders.Add("Content-Type", "application/json; charset=utf-8");
                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                    client.DefaultRequestHeaders.Add("Prefer", "return=representation");




                    D365FOHeader header = new D365FOHeader();

                    // Create instances of the entities that are used in the service and
                    // set the needed fields on those entities.
                    header.CompanyCode = "W97";
                    header.TransType = "ICInv";
                    header.FileBatchID = "GATShip_20230321_0130_ICInv_W97";
                    header.Description = "ICInv WORDR220302 22SGP0147 2023003442 Kim Transport Solutio";

                    List<D365FODetail> details = new List<D365FODetail>();

                    D365FODetail detail = new D365FODetail();
                    detail.FileBatchID = "GATShip_20230321_0130_ICInv_W97";
                    detail.TransDate = DateTime.Now.ToString("yyyy-MM-dd");
                    detail.AccountType = "Ledger";
                    detail.Account = "2000311";                  
                    detail.VesselCustomerCode = "WORDR220302";
                    detail.VoyageCode = "VSL1000";
                    detail.RevenueType = "0000";                   
                    detail.Currency = "SGD";
                    detail.Amount = 70.20M;
                    detail.LineDescription = "CREW TRANSPORT CHARGES 2023003442 Kim Transport Solutions Pt";
                    detail.ExchangeRate = 1.00000M;
                    detail.SalesTaxGroup = "GST";
                    detail.ItemSalesTaxGroup = "GST";
                    detail.SalesTaxAmount = 5.20M;
                    detail.Invoice = "2023003442";
                    detail.Document = "22SGP0147";
                    detail.DocumentDate = DateTime.Now.ToString("yyyy-MM-dd");
                    detail.DueDate = DateTime.Now.ToString("yyyy-MM-dd");
                    detail.Payment = "";
                    detail.BankTransType = "";
                    detail.CreatedByUserId = "WME";
                    detail.UserField1 = "WHQW";
                    detail.UserField1 = "";
                    detail.UserField3 = "";
                    detail.Remark = "";

                    details.Add(detail);

                    detail = new D365FODetail();
                    detail.FileBatchID = "GATShip_20230321_0130_ICInv_W97";
                    detail.TransDate = DateTime.Now.ToString("yyyy-MM-dd");
                    detail.AccountType = "Vendor";
                    detail.Account = "2000311";
                    detail.VesselCustomerCode = "WORDR220302";
                    detail.VoyageCode = "VSL1000";
                    detail.RevenueType = "0000";
                    detail.Currency = "SGD";
                    detail.Amount = 70.20M;
                    detail.LineDescription = "CREW TRANSPORT CHARGES 2023003442 Kim Transport Solutions Pt";
                    detail.ExchangeRate = 1.00000M;
                    detail.SalesTaxGroup = "GST";
                    detail.ItemSalesTaxGroup = "GST";
                    detail.SalesTaxAmount = 5.20M;
                    detail.Invoice = "2023003442";
                    detail.Document = "22SGP0147";
                    detail.DocumentDate = DateTime.Now.ToString("yyyy-MM-dd");
                    detail.DueDate = DateTime.Now.ToString("yyyy-MM-dd");
                    detail.Payment = "";
                    detail.BankTransType = "";
                    detail.CreatedByUserId = "WME";
                    detail.UserField1 = "WHQW";
                    detail.UserField1 = "";
                    detail.UserField3 = "";
                    detail.Remark = "";

                    details.Add(detail);



                    //instantiating THK_AIFA3GLJournal
                    //THK_AIFA3GLJournal a3journal = new THK_AIFA3GLJournal();
                    D365FOJournal a3journal = new D365FOJournal();


                    //adding line details to THK_AIFA3GLLine in an array form then adding it to header
                   // header.THK_AIFA3GLLine = details.ToArray();

                    //adding header in an array form to THK_AIFA3GLJournal
                    //Entity_THK_AIFA3GLHeader[] headerArr = new Entity_THK_AIFA3GLHeader[1] { header };

                    a3journal._gatshipData = header;
                    a3journal._gatshipData.Lines = details;

                    try
                    {
                        var jsonString = "{\"_gatshipData\":{" +
                            "\"CompanyCode\": \"W97\"," +
                            "\"TransType\": \"ICInv\"," +
                            "\"Description\": \"ICInv WORDR220302 22SGP0147 2023003442 Kim Transport Solutio\"," +
                            "\"FileBatchID\": \"GATShip_20230321_0130_ICInv_W97\"," +
                            "\"Line\": [" +
                            "{\"FileBatchID\": \"GATShip_20230321_0130_ICInv_W97\"," +
                                "\"TransDate\": \"2023-03-14\"," +
                                "\"AccountType\": \"Ledger\"," +
                                "\"Account\": \"2000311\"," +
                                "\"VesselCustomerCode\": \"WORDR220302\"," +
                                "\"VoyageCode\": \"VSL1000\"," +
                                "\"RevenueType\": \"0000\"," +                               
                                "\"Currency\": \"SGD\"," +
                                "\"Amount\": \"70.20\"," +
                                "\"LineDescription\": \"CREW TRANSPORT CHARGES 2023003442 Kim Transport Solutions Pt\"," +
                                "\"ExchangeRate  \": \"1.00000\"," +
                                "\"SalesTaxGroup\": \"GST\"," +
                                "\"ItemSalesTaxGroup\": \"GST\"," +
                                "\"SalesTaxAmount\": \"5.20\"," +
                                "\"Invoice\": \"2023003442\"," +
                                "\"Document\": \"22SGP0147 \"," +
                                "\"DocumentDate\": \"2023-02-28\"," +
                                "\"DueDate\": \"2023-04-24\"," +
                                "\"Payment\": \"\"," +
                                "\"BankTransType\": \"\"," +
                                "\"CreatedByUserId\": \"WME\"," +
                                "\"UserField1\": \"WHQW\"," +
                                "\"UserField2\": \"\"," +
                                "\"UserField3\": \"\"," +
                                "\"Remark\": \"\"}," +                              
                                "{\"FileBatchID\": \"GATShip_20230321_0130_ICInv_W97\"," +
                                "\"TransDate\": \"2023-03-14\"," +
                                "\"AccountType\": \"Vendor\"," +
                                "\"Account\": \"K025\"," +
                                "\"DIM1\": \"0000\"," +
                                "\"DIM2\": \"Agency\"," +
                                "\"VesselCustomerCode\": \"WORDR220302\"," +
                                "\"VoyageCode\": \"VSL1000\"," +
                                "\"RevenueType\": \"0000\"," +                               
                                "\"Currency\": \"SGD\"," +
                                "\"Amount\": \"-70.20\"," +
                                "\"LineDescription\": \"CREW TRANSPORT CHARGES 2023003442 Kim Transport Solutions Pt\"," +
                                "\"ExchangeRate  \": \"1.00000\"," +
                                "\"SalesTaxGroup\": \"GST\"," +
                                "\"ItemSalesTaxGroup\": \"GST\"," +
                                "\"SalesTaxAmount\": \"5.20\"," +
                                "\"Invoice\": \"2023003442\"," +
                                "\"Document\": \"22SGP0147 \"," +
                                "\"DocumentDate\": \"2023-02-28\"," +
                                "\"DueDate\": \"2023-04-24\"," +
                                "\"Payment\": \"\"," +
                                "\"BankTransType\": \"\"," +
                                "\"CreatedByUserId\": \"WME\"," +
                                "\"UserField1\": \"WHQW\"," +
                                "\"UserField2\": \"\"," +
                                "\"UserField3\": \"\"," +
                                "\"Remark\": \"\"}" +                               
                                "]}}";


                        // Make a Post request to the API                        
                        // HttpContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                        var js = JsonConvert.SerializeObject(a3journal);
                        HttpContent content = new StringContent(JsonConvert.SerializeObject(a3journal), Encoding.UTF8, "application/json");
                        //HttpContent content = new StringContent(jsonString);
                        HttpResponseMessage apiResponse = await client.PostAsync(apiUrl, content);

                        // Check if the request to the API was successful (status code 200)
                        if (apiResponse.IsSuccessStatusCode)
                        {
                            // Read and print the response content
                            var result = await apiResponse.Content.ReadAsStringAsync();
                            //TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(result);

                            Console.WriteLine(result);
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
                else
                {
                    Console.WriteLine($"Token Request Error: {tokenResponse.StatusCode} - {tokenResponse.ReasonPhrase}");
                }
            }
        }
    }

}