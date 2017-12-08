using RememberTheMilkApi;
using RememberTheMilkApi.Objects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace RtmApiTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            CheckAuthentication();

            IDictionary<string, string> parameters = new Dictionary<string, string>();

            RtmApiResponse listResponse = RtmConnection.SendRequest("rtm.lists.getList", parameters);
            RtmApiResponse taskResponse = RtmConnection.SendRequest("rtm.tasks.getList", parameters);

            IList<string> tssListIds = listResponse.ListCollection.Lists.Where(list => list.Name.ToLower().Contains("3ss")).Select(list => list.Id).ToList();
            IList<RtmApiTaskSeriesList> test = taskResponse.TaskSeriesCollection.TaskSeriesList.Where(taskSeriesList => taskSeriesList.TaskSeries.Any() && tssListIds.Contains(taskSeriesList.Id)).ToList();

            Console.WriteLine("Lists:");
            Console.WriteLine(string.Join(Environment.NewLine, listResponse.ListCollection.Lists.Select(list => list.Name)));
            Console.WriteLine("{0}Tasks:", Environment.NewLine);
            Console.WriteLine(string.Join(Environment.NewLine, taskResponse.TaskSeriesCollection.TaskSeriesList.Where(series => series.TaskSeries != null && series.TaskSeries.Any()).Select(series => string.Join(Environment.NewLine, series.TaskSeries.Select(task => task.Name)))));
            Console.ReadKey();
        }

        private static void CheckAuthentication()
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

            if (File.Exists("authtoken.authtoken"))
            {
                string authtoken;
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
            if (tokenResponse.ErrorResponse.Code == 98)
            {
                RefreshToken();
            }

            /*tokenResponse =*/
            RtmConnection.CheckApiAuthToken();
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
                    sw.Write(authResponse.Auth.Token);
                    sw.Close();
                    fs.Close();
                }
            }
        }
    }
}