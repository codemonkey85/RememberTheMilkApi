using Newtonsoft.Json;
using System.Collections.Generic;

namespace RememberTheMilkApi.Objects
{
    [JsonObject]
    public class RtmRequest
    {
        public SortedDictionary<string, string> Parameters { get; set; }

        public RtmRequest()
        {
            Parameters = new SortedDictionary<string, string>();
        }
    }
}