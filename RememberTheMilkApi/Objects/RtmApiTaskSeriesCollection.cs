using Newtonsoft.Json;
using System.Collections.Generic;

namespace RememberTheMilkApi.Objects
{
    [JsonObject("tasks")]
    public class RtmApiTaskSeriesCollection
    {
        [JsonProperty("rev")]
        public string Rev { get; set; }

        [JsonProperty("list")]
        public IList<RtmApiTaskSeriesList> TaskSeriesList { get; set; }

        public RtmApiTaskSeriesCollection()
        {
            Rev = string.Empty;
            TaskSeriesList = new List<RtmApiTaskSeriesList>();
        }
    }
}