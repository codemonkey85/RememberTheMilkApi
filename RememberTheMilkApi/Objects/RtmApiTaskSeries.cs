using Newtonsoft.Json;
using System.Collections.Generic;

namespace RememberTheMilkApi.Objects
{
    [JsonObject("list")]
    public class RtmApiTaskSeries
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("taskseries")]
        public IList<RtmApiTaskObject> TaskSeries { get; set; }

        public RtmApiTaskSeries()
        {
            Id = string.Empty;
            TaskSeries = new List<RtmApiTaskObject>();
        }
    }
}