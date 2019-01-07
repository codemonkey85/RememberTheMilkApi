using Newtonsoft.Json;
using RememberTheMilkApi.Extensions;
using RememberTheMilkApi.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace RememberTheMilkApi.Helpers
{
    public static class RtmConnectionHelper
    {
        internal const string AuthUrl = @"https://www.rememberthemilk.com/services/auth/";
        internal const string RestUrl = @"https://api.rememberthemilk.com/services/rest/";
        internal static string ApiKey;
        internal static string Secret;
        internal static string AuthToken;

        internal static string Frob;

        internal const string FormatParameter = @"json";

        internal const string GetFrobMethod = @"rtm.auth.getFrob";
        internal const string GetTokenMethod = @"rtm.auth.getToken";
        internal const string CheckTokenMethod = @"rtm.auth.checkToken";
        internal const string CreateTimelineMethod = @"rtm.timelines.create";

        internal static MD5 Md5;

        public enum Permissions
        {
            Read,
            Write,
            Delete
        }

        public static void InitializeRtmConnection(string apiKey, string secret)
        {
            ApiKey = apiKey;
            Secret = secret;
            Md5 = MD5.Create();
        }

        public static RtmApiResponse CreateTimeline() => SendRequest(CreateTimelineMethod, new Dictionary<string, string>());

        public static string GetAuthenticationUrl(Permissions permissions)
        {
            IDictionary<string, string> parameters = new Dictionary<string, string>();

            RtmApiResponse response = SendRequest(GetFrobMethod, parameters);
            Frob = response.Frob;

            string perms = string.Empty;
            switch (permissions)
            {
                case Permissions.Read:
                    perms = "read";
                    break;

                case Permissions.Write:
                    perms = "write";
                    break;

                case Permissions.Delete:
                    perms = "delete";
                    break;
            }

            parameters.Remove("format");
            parameters.Remove("method");
            parameters.Remove("api_sig");
            parameters.Add("perms", perms);
            parameters.Add("frob", Frob);
            parameters.Add("api_key", ApiKey);
            
            string apiSig = SignApiParameters(parameters).ToLower();
            parameters.CreateNewOrUpdateExisting("api_sig", apiSig);

            return $"{AuthUrl}?{EncodeParameters(parameters)}";
        }

        internal static string SignApiParameters(IDictionary<string, string> parameters) => SignApiParameters(new SortedDictionary<string, string>(parameters));

        internal static string SignApiParameters(SortedDictionary<string, string> parameters) => CalculateMd5Hash($"{Secret}{EncodeParameters(parameters, true)}");

        internal static string EncodeParameters(IDictionary<string, string> parameters, bool signing = false) => string.Join(signing ? "" : "&", parameters.Select(kvp => $"{kvp.Key}{(signing ? kvp.Value : "=" + HttpUtility.UrlEncode(kvp.Value))}"));

        internal static string CalculateMd5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hash = Md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            foreach (byte b in hash)
            {
                sb.Append(b.ToString("X2"));
            }

            return sb.ToString();
        }

        public static void SetApiAuthToken(string apiAuthToken) => AuthToken = apiAuthToken;

        public static RtmApiResponse CheckApiAuthToken()
        {
            return SendRequest(WebRequestMethods.Http.Get, CheckTokenMethod, new RtmApiRequest
            {
                Parameters = new SortedDictionary<string, string>()
            });
        }

        public static RtmApiResponse GetApiAuthToken()
        {
            IDictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "api_key", ApiKey },
                { "format", FormatParameter },
                { "frob", Frob },
                { "method", GetTokenMethod }
            };
            RtmApiRequest request = new RtmApiRequest
            {
                Parameters = new SortedDictionary<string, string>(parameters)
            };
            RtmApiResponse response = SendRequest(WebRequestMethods.Http.Get, GetTokenMethod, request);
            AuthToken = response.Auth.Token;
            return response;
        }

        public static RtmApiResponse SendRequest(string rtmMethodName, IDictionary<string, string> parameters)
        {
            return SendRequest(WebRequestMethods.Http.Get, rtmMethodName, new RtmApiRequest
            {
                Parameters = new SortedDictionary<string, string>(parameters)
            });
        }

        internal static RtmApiResponse SendRequest(string webRequestMethod, string rtmMethod, RtmApiRequest request)
        {
            RtmApiResponse response = null;
            try
            {
                request.Parameters.CreateNewOrUpdateExisting("format", FormatParameter);
                request.Parameters.CreateNewOrUpdateExisting("method", rtmMethod);
                request.Parameters.CreateNewOrUpdateExisting("api_key", ApiKey);
                request.Parameters.CreateNewOrUpdateExisting("auth_token", AuthToken);

                request.Parameters.Remove("api_sig");

                string apiSig = SignApiParameters(request.Parameters);
                request.Parameters.Add("api_sig", apiSig);

                string url = $"{RestUrl}?{EncodeParameters(request.Parameters)}";

                WebRequest webRequest = WebRequest.Create(url);
                webRequest.Method = webRequestMethod;
                WebResponse webResponse = webRequest.GetResponse();
                string data;
                using (StreamReader sr = new StreamReader(webResponse.GetResponseStream()))
                {
                    data = sr.ReadToEnd();
                    sr.Close();
                }
                RtmApiResponseRoot responseRoot = JsonConvert.DeserializeObject<RtmApiResponseRoot>(data);
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