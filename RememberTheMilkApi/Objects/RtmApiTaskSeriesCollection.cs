using Newtonsoft.Json;
using System.Collections.Generic;

namespace RememberTheMilkApi.Objects
{
    [JsonObject("tasks")]
    public class RtmApiTaskSeriesCollection
    {
        [JsonProperty("rev")]
        public string rev { get; set; }

        [JsonProperty("list")]
        public IList<RtmApiTaskSeries> TaskSeries { get; set; }

        public RtmApiTaskSeriesCollection()
        {
            rev = string.Empty;
            TaskSeries = new List<RtmApiTaskSeries>();
        }
    }
}