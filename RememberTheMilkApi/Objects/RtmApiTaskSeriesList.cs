using Newtonsoft.Json;
using System.Collections.Generic;

namespace RememberTheMilkApi.Objects
{
    [JsonObject("list")]
    public class RtmApiTaskSeriesList
    {
        [JsonProperty("id")]
        public string Id
        {
            get; set;
        }

        [JsonProperty("taskseries")]
        public IList<RtmApiTaskSeries> TaskSeries
        {
            get; set;
        }

        public RtmApiTaskSeriesList()
        {
            Id = string.Empty;
            TaskSeries = new List<RtmApiTaskSeries>();
        }
    }
}