using Newtonsoft.Json;
using System.Collections.Generic;

namespace RememberTheMilkApi.Objects
{
    [JsonObject("list")]
    public class RtmApiTaskList
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("taskseries")]
        public IList<RtmApiTaskObject> TaskSeries { get; set; }
    }
}