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

namespace RememberTheMilkApi
{
    public static class RtmConnection
    {
        private static string _authUrl = @"https://www.rememberthemilk.com/services/auth/";
        private static string _restUrl = @"https://api.rememberthemilk.com/services/rest/";
        internal static string ApiKey;
        internal static string Secret;
        internal static string AuthToken;

        private static string _frob;

        private static string _getFrob = @"rtm.auth.getFrob";
        private static string _getToken = @"rtm.auth.getToken";
        private static string _checkToken = @"rtm.auth.checkToken";

        private static MD5 _md5;

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
            _md5 = MD5.Create();
        }

        public static string GetAuthenticationUrl(Permissions permissions)
        {
            IDictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "api_key", ApiKey },
                { "format", "json" },
                { "method", _getFrob }
            };

            string apiSig = SignApiParameters(parameters);
            parameters.Add("api_sig", apiSig);
            string url = string.Format("{0}?{1}", _restUrl, EncodeParameters(parameters));

            WebRequest webRequest = WebRequest.Create(url);
            webRequest.Method = WebRequestMethods.Http.Get;
            WebResponse webResponse = webRequest.GetResponse();
            string data;
            using (StreamReader sr = new StreamReader(webResponse.GetResponseStream()))
            {
                data = sr.ReadToEnd();
                sr.Close();
            }
            RtmApiResponseRoot responseRoot = JsonConvert.DeserializeObject<RtmApiResponseRoot>(data);
            RtmApiResponse response = responseRoot.Response;
            _frob = response.Frob;

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
            parameters.Add("frob", _frob);

            apiSig = SignApiParameters(parameters).ToLower();
            parameters.CreateNewOrUpdateExisting("api_sig", apiSig);

            return string.Format("{0}?{1}", _authUrl, EncodeParameters(parameters));
        }

        private static string SignApiParameters(IDictionary<string, string> parameters)
        {
            SortedDictionary<string, string> sortedParameters = new SortedDictionary<string, string>(parameters);
            return SignApiParameters(sortedParameters);
        }

        private static string SignApiParameters(SortedDictionary<string, string> parameters)
        {
            return CalculateMd5Hash(string.Format("{0}{1}", Secret, EncodeParameters(parameters, true)));
        }

        private static string EncodeParameters(IDictionary<string, string> parameters, bool signing = false)
        {
            return string.Join(signing ? "" : "&", parameters.Select(kvp => string.Format("{0}{1}{2}", kvp.Key, signing ? "" : "=", HttpUtility.UrlEncode(kvp.Value))));
        }

        private static string CalculateMd5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hash = _md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }

            return sb.ToString();
        }

        public static void SetApiAuthToken(string apiAuthToken)
        {
            AuthToken = apiAuthToken;
        }

        public static RtmApiResponse CheckApiAuthToken()
        {
            return SendRequest(WebRequestMethods.Http.Get, _checkToken, new RtmApiRequest
            {
                Parameters = new SortedDictionary<string, string>()
            });
        }

        public static RtmApiResponse GetApiAuthToken()
        {
            IDictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "api_key", ApiKey },
                { "format", "json" },
                { "frob", _frob },
                { "method", _getToken }
            };
            RtmApiRequest request = new RtmApiRequest();
            request.Parameters = new SortedDictionary<string, string>(parameters);
            RtmApiResponse response = SendRequest(WebRequestMethods.Http.Get, _getToken, request);
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

        private static RtmApiResponse SendRequest(string webRequestMethod, string rtmMethod, RtmApiRequest request)
        {
            RtmApiResponse response = null;
            try
            {
                request.Parameters.CreateNewOrUpdateExisting("format", "json");
                request.Parameters.CreateNewOrUpdateExisting("method", rtmMethod);
                request.Parameters.CreateNewOrUpdateExisting("api_key", ApiKey);
                request.Parameters.CreateNewOrUpdateExisting("auth_token", AuthToken);

                request.Parameters.Remove("api_sig");

                string apiSig = SignApiParameters(request.Parameters);
                request.Parameters.Add("api_sig", apiSig);

                string url = string.Format("{0}?{1}", _restUrl, EncodeParameters(request.Parameters));

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