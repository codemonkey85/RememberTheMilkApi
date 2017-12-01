using Newtonsoft.Json;
using RememberTheMilkApi.Extensions;
using RememberTheMilkApi.Objects;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;

namespace RememberTheMilkApi
{
    public static class RtmConnection
    {
        private static string _authUrl = @"https://www.rememberthemilk.com/services/auth/";
        private static string _restUrl = @"https://api.rememberthemilk.com/services/rest/";
        internal static string _apiKey;
        internal static string _secret;

        public static HttpClient Client = new HttpClient();

        public static void InitializeRtmConnection(string apiKey, string secret)
        {
            _apiKey = apiKey;
            _secret = secret;
        }

        public static RtmResponse SendRequest(string requestMethod, RtmRequest request)
        {
            RtmResponse response = null;
            try
            {
                request.Parameters.CreateNewOrUpdateExisting("format", "json");
                string url = string.Format("{0}?{1}", _restUrl, string.Join("&", request.Parameters.Select(kvp => string.Format("{0}={1}", kvp.Key, HttpUtility.UrlEncode(kvp.Value)))));
                WebRequest webRequest = WebRequest.Create(url);
                webRequest.Method = requestMethod;
                WebResponse webResponse = webRequest.GetResponse();
                StreamReader sr = new StreamReader(webResponse.GetResponseStream());
                string data = sr.ReadToEnd();
                RtmResponseRoot responseRoot = JsonConvert.DeserializeObject<RtmResponseRoot>(data);
                response = responseRoot.Response;
                return response;
            }
            catch (Exception)
            {
                return response;
            }
        }
    }
}