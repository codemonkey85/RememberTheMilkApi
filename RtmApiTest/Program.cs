using RememberTheMilkApi;
using RememberTheMilkApi.Objects;
using System.IO;
using System.Net;

namespace RtmApiTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            TestResponse();
        }

        private static void TestResponse()
        {
            string apiKey;
            string secret;

            using (FileStream fs = new FileStream("apikey.apikey", FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    apiKey = sr.ReadLine();
                    sr.Close();
                    fs.Close();
                }
            }

            using (FileStream fs = new FileStream("secret.secret", FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    secret = sr.ReadLine();
                    sr.Close();
                    fs.Close();
                }
            }

            RtmConnection.InitializeRtmConnection(apiKey, secret);
            RtmResponse result = RtmConnection.SendRequest(WebRequestMethods.Http.Get, new RtmRequest
            {
                Parameters = new System.Collections.Generic.SortedDictionary<string, string>()
                {
                { "", "" },
                }
            });
        }
    }
}