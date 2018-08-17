using RememberTheMilkApi;
using RememberTheMilkApi.Extensions;
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

            IList<string> tssListIds = listResponse.ListCollection.Lists.Where(list => list.Name.ToLower() == "bills").Select(list => list.Id).ToList();
            IList<RtmApiTaskSeriesList> test = taskResponse.TaskSeriesCollection.TaskSeriesList.Where(taskSeriesList => taskSeriesList.TaskSeries.Any() && tssListIds.Contains(taskSeriesList.Id)).ToList();

            string billListId = tssListIds.FirstOrDefault();

            Console.WriteLine("Lists:");
            Console.WriteLine(string.Join(Environment.NewLine, listResponse.ListCollection.Lists.Select(list => list.Name)));
            Console.WriteLine("{0}Tasks:", Environment.NewLine);

            IList<string> taskList = (from series in taskResponse.TaskSeriesCollection.TaskSeriesList.Where(series => series.Id == billListId && series.TaskSeries != null && series.TaskSeries.Any()) from taskSeries in series.TaskSeries select taskSeries.Task.First().Due == string.Empty ? taskSeries.Name : $"{taskSeries.Name} due {taskSeries.Task.First().DueLocalTime.ToString("MM/dd/yyyy")}").ToList();

            //var taskListE =
            //    taskResponse.TaskSeriesCollection.TaskSeriesList
            //        .Where(series =>
            //            series.Id == billListId && series.TaskSeries != null && series.TaskSeries.Any())
            //        .Select(series =>
            //            series.TaskSeries
            //                .Select(task =>
            //                    task.Task.First().Due == string.Empty
            //                        ? task.Name
            //                        : string.Format("{0} due {1}",
            //                            task.Name, task.Task.First().DueLocalTime.ToString("MM/dd/yyyy"))));

            string taskListString = string.Join(Environment.NewLine, taskList.OrderBy(task => task));

            Console.WriteLine(taskListString);
            using (FileStream fs = new FileStream(@"C:\Users\mbond\Desktop\ARRIS\bills.txt", FileMode.Create, FileAccess.ReadWrite))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(taskListString);
                    sw.Close();
                    fs.Close();
                }
            }
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