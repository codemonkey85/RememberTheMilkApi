using RememberTheMilkApi.Helpers;
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
            CheckAuthentication();

            RtmApiResponse listResponse = RtmMethodHelper.GetListsList();
            RtmApiResponse taskResponse = RtmMethodHelper.GetTasksList();

            string timeline = RtmConnectionHelper.CreateTimeline().TimeLine;
            RtmApiResponse newTaskResponse = RtmMethodHelper.AddTask(timeline, "try something new at 5 pm", parse: "1");

            //string transactionId = newTaskResponse.Transaction.Id;
            //RtmApiResponse undoResponse = RtmMethodHelper.UndoTransaction(timeline, transactionId);

            //IList<string> tssListIds = listResponse.ListCollection.Lists.Where(list => string.Equals(list.Name, "3ss tasks", StringComparison.OrdinalIgnoreCase)).Select(list => list.Id).ToList();
            //IList<RtmApiTaskSeries> tssTasks = taskResponse.TaskSeriesCollection.TaskSeriesList
            //    .Where(taskSeriesList => taskSeriesList.TaskSeries.Any() && tssListIds.Contains(taskSeriesList.Id))
            //    .SelectMany(taskSeriesList => taskSeriesList.TaskSeries)
            //    .ToList();
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

            RtmConnectionHelper.InitializeRtmConnection(apiKey, secret);

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
                RtmConnectionHelper.SetApiAuthToken(authtoken);
            }
            else
            {
                RefreshToken();
            }

            RtmApiResponse tokenResponse = RtmConnectionHelper.CheckApiAuthToken();
            if (tokenResponse.ErrorResponse.Code == 98)
            {
                RefreshToken();
            }

            /*tokenResponse =*/
            RtmConnectionHelper.CheckApiAuthToken();
        }

        private static void RefreshToken()
        {
            string url = RtmConnectionHelper.GetAuthenticationUrl(RtmConnectionHelper.Permissions.Delete);

            Console.WriteLine("Press any key after auth is complete.");
            Process.Start(url);
            Console.ReadKey();

            RtmApiResponse authResponse = RtmConnectionHelper.GetApiAuthToken();

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