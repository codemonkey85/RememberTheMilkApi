using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;

namespace RememberTheMilkApi.Objects
{
    [JsonObject("task")]
    public class RtmApiTaskObject
    {
        [JsonProperty("added")]
        public string Added
        {
            get; set;
        }

        [JsonProperty("completed")]
        public string Completed
        {
            get; set;
        }

        [JsonProperty("deleted")]
        public string Deleted
        {
            get; set;
        }

        [JsonProperty("due")]
        public string Due
        {
            get; set;
        }

        public DateTime? DueLocalTime
        {
            get
            {
                if (Due == string.Empty)
                {
                    return null;
                }
                DateTime.TryParse(Due, out DateTime dtLocalTime);
                return dtLocalTime.ToLocalTime();
            }
        }

        [JsonProperty("estimate")]
        public string Estimate
        {
            get; set;
        }

        [JsonProperty("has_due_time")]
        public string HasDueTime
        {
            get; set;
        }

        [JsonProperty("id")]
        public string Id
        {
            get; set;
        }

        [JsonProperty("postponed")]
        public string Postponed
        {
            get; set;
        }

        [JsonProperty("priority")]
        public string Priority
        {
            get; set;
        }

        public RtmApiTaskObject()
        {
            Added = string.Empty;
            Completed = string.Empty;
            Deleted = string.Empty;
            Due = string.Empty;
            Estimate = string.Empty;
            HasDueTime = string.Empty;
            Id = string.Empty;
            Postponed = string.Empty;
            Priority = string.Empty;
        }
    }
}