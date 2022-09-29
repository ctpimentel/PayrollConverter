using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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
        public  List<CParam> Params { get; set; }
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
                var endpoint = baseUrl + endpointFinal;
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
    }
}
