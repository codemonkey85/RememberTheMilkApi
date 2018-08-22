using Newtonsoft.Json;
using System.Collections.Generic;

namespace RememberTheMilkApi.Objects
{
    [JsonObject]
    public class RtmApiRequest
    {
        public SortedDictionary<string, string> Parameters
        {
            get; set;
        }

        public RtmApiRequest()
        {
            Parameters = new SortedDictionary<string, string>();
        }
    }
}