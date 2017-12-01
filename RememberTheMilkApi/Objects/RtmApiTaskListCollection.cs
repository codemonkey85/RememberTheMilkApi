using Newtonsoft.Json;
using System.Collections.Generic;

namespace RememberTheMilkApi.Objects
{
    [JsonObject("tasks")]
    public class RtmApiTaskListCollection
    {
        [JsonProperty("rev")]
        public string rev { get; set; }

        [JsonProperty("list")]
        public IList<RtmApiTaskList> ListCollection { get; set; }
    }
}