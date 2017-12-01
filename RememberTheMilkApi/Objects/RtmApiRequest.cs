using Newtonsoft.Json;
using System.Collections.Generic;

namespace RememberTheMilkApi.Objects
{
    [JsonObject]
    public class RtmApiRequest
    {
        public SortedDictionary<string, string> Parameters { get; set; }
        private string WebRequestMethod { get; set; }
        private string RtmApiMethod { get; set; }

        public RtmApiRequest()
        {
            Parameters = new SortedDictionary<string, string>();
        }

        //public RtmApiResponse SendRequest()
        //{
        //}
    }
}