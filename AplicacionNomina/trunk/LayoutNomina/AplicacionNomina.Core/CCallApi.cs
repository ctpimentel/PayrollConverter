using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace AplicacionNomina
{
    public class CConection
    {
        public string connectionString { get; set; }
        //public string tokenId { get; set; }
        //public string id { get; set; }
        //public string reference { get; set; }
        //public string description { get; set; }
        //public string createdDate { get; set; }
        //public string createdBy { get; set; }
        //public string updatedDate { get; set; }
        //public string updatedBy { get; set; }
    }
    public class CParam
    {
        public string paramValue { get; set; }
        public string reference { get; set; }
    }
    public class CAppConfig
    {
        [JsonProperty("connections")]
        public List<CConection> connection { get; set; }
        [JsonProperty("params")]
        public List<CParam> Params { get; set; }
    }
    public class CCallApi
    {
        public static List<CParam> Params { get; set; }
        private static string pConnString = "";
        public static string ConnString

        {
            get { return pConnString; }
            set { pConnString = value; }
        }

        public async Task<CAppConfig> GetApiManagerParams()
        {
            var appConfig = new CAppConfig();

            try
            {

                var config = ConfigurationManager.AppSettings;
                var endpointFinal = config["Endpoint"];
                var token = config["token"];
                var baseUrl = config["BaseUrl"];
                //var endpoint = baseUrl + endpointFinal;
                using (HttpClient client = new HttpClient())
                {

                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Clear();
                    //client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("text/plain"));

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));

                    client.DefaultRequestHeaders.Add("token", token);

                    using (var Response = await client.GetAsync(endpointFinal))
                    {
                        if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var result = await Response.Content.ReadAsStringAsync();

                            //var TEST = Newtonsoft.Json.JsonConvert.
                            //    DeserializeObject<List<CConection>>(result); funcionó

                            //var TEST = Newtonsoft.Json.JsonConvert.
                            //    DeserializeObject<List<CParam>>(result); funcionó

                            appConfig = Newtonsoft.Json.JsonConvert.
                                    DeserializeObject<CAppConfig>(result);


                            CCallApi.ConnString = appConfig.connection.FirstOrDefault().connectionString;
                            CCallApi.Params = appConfig.Params;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return appConfig;
        }

        /// <summary>
        /// Metodo creado para registrar al logueo
        /// </summary>
        public async void insertToLogApi( string message)
        {
            var appConfig = new CAppConfig();

            try
            {
                var requestID = Guid.NewGuid().ToString();

                var config = ConfigurationManager.AppSettings;
                var endpointFinal = config["EndpointLogger"];
                var baseUrl = config["BaseUrllogger"];


                using (HttpClient client = new HttpClient())
                {

                    var HostName = Dns.GetHostName();
                    var ipaddress = Dns.GetHostAddresses(HostName);
                    string myIPV4 = string.Empty;
                    foreach (IPAddress ip4 in ipaddress.Where(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork))
                    {
                        myIPV4 = (ip4.ToString());
                    }



                    var macs =
                    (
                        from nic in NetworkInterface.GetAllNetworkInterfaces()
                        where nic.OperationalStatus == OperationalStatus.Up
                        select nic.GetPhysicalAddress().ToString()
                    ).ToList();



                    var macr = macs.FirstOrDefault();

                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Add("ChannelId", "LayoutNominaChannel");

                    client.DefaultRequestHeaders.Add("ClientApp", "LayoutNomina");
                    client.DefaultRequestHeaders.Add("ClientDt", DateTime.Now.ToShortDateString());
                    //client.DefaultRequestHeaders.Add("Content-Type", "text/plain");
                    client.DefaultRequestHeaders.Add("Destination", "Canalix");

                    client.DefaultRequestHeaders.Add("HostName", HostName);
                    client.DefaultRequestHeaders.Add("IpAddress", myIPV4);

                    client.DefaultRequestHeaders.Add("MacAddress", macr);

                    client.DefaultRequestHeaders.Add("MsgPriority", "High");

                    client.DefaultRequestHeaders.Add("NetworkOwner", "Banesco-DO");

                    client.DefaultRequestHeaders.Add("NetworkRefIdent", "BanescoID");

                    client.DefaultRequestHeaders.Add("Org", "Banesco CA");

                    client.DefaultRequestHeaders.Add("RequestId", requestID);
                    client.DefaultRequestHeaders.Add("RqdOper", "CREATE");
                    client.DefaultRequestHeaders.Add("RqdOperType", "CREATELOGNOMINA");
                    client.DefaultRequestHeaders.Add("RqdService", "Layoutnomina");
                    client.DefaultRequestHeaders.Add("TimeProcess", DateTime.Now.ToString());

                    client.DefaultRequestHeaders.Add("UserId", "ADMIN"); //4d2bece9-39fa-4db5-babc-0eef81ce32e6 ESTE VALOR EN LA PRUEBA POSTMAN FUNCIONA

                    client.DefaultRequestHeaders.Add("UserType", "Admin");


                    var json = JsonConvert.SerializeObject(new stResultReturn() { Mensaje= message} );
                    var data = new StringContent(json, Encoding.UTF8, "text/plain");
                    //client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("text/plain"));

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));

                    //client.DefaultRequestHeaders.Add("token", token);

                    using (var Response = await client.PostAsync(endpointFinal, data))
                    {
                        if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var result = await Response.Content.ReadAsStringAsync();

                            //var TEST = Newtonsoft.Json.JsonConvert.
                            //    DeserializeObject<List<CConection>>(result); funcionó

                            //var TEST = Newtonsoft.Json.JsonConvert.
                            //    DeserializeObject<List<CParam>>(result); funcionó                            
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
