using RememberTheMilkApi;
using RememberTheMilkApi.Objects;
using System;
using System.Diagnostics;
using System.IO;

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
            string authtoken;
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

            if (File.Exists("authtoken.authtoken"))
            {
                using (FileStream fs = new FileStream("authtoken.authtoken", FileMode.Open, FileAccess.Read))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        authtoken = sr.ReadLine();
                        sr.Close();
                        fs.Close();
                    }
                }
                RtmConnection.SetApiAuthToken(authtoken);
            }
            else
            {
                RefreshToken();
            }

            RtmApiResponse tokenResponse = RtmConnection.CheckApiAuthToken();
            if (tokenResponse.Error.Code == 98)
            {
                RefreshToken();
            }

            tokenResponse = RtmConnection.CheckApiAuthToken();
        }

        private static void RefreshToken()
        {
            string url = RtmConnection.GetAuthenticationUrl(RtmConnection.Permissions.Delete);

            Console.WriteLine("Press any key after auth is complete.");
            Process.Start(url);
            Console.ReadKey();

            RtmApiResponse authResponse = RtmConnection.GetApiAuthToken();

            using (FileStream fs = new FileStream("authtoken.authtoken", FileMode.Create, FileAccess.ReadWrite))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(authResponse.Auth.token);
                    sw.Close();
                    fs.Close();
                }
            }
        }
    }
}