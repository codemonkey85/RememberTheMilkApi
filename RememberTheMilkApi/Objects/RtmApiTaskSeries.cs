using Newtonsoft.Json;
using RememberTheMilkApi.Converters;
using System;
using System.Collections.Generic;

namespace RememberTheMilkApi.Objects
{
    [JsonObject("taskseries")]
    public class RtmApiTaskSeries
    {
        [JsonProperty("id")]
        public string Id
        {
            get; set;
        }

        [JsonProperty("name")]
        public string Name
        {
            get; set;
        }

        [JsonProperty("created")]
        public string Created
        {
            get; set;
        }

        [JsonProperty("modified")]
        public string Modified
        {
            get; set;
        }

        [JsonProperty("source")]
        public string Source
        {
            get; set;
        }

        #region In Development

        [JsonProperty("tags")]
        [JsonConverter(typeof(SingleOrArrayConverter<RtmApiTagObject>))]
        public IList<RtmApiTagObject> Tags
        {
            get; set;
        }

        [JsonProperty("participants")]
        public dynamic Participants
        {
            get; set;
        }

        [JsonProperty("notes")]
        public dynamic Notes
        {
            get; set;
        }

        [JsonProperty("task")]
        [JsonConverter(typeof(SingleOrArrayConverter<RtmApiTaskObject>))]
        public IList<RtmApiTaskObject> Task
        {
            get; set;
        }

        #endregion In Development

        public RtmApiTaskSeries()
        {
            Id = string.Empty;
            Name = string.Empty;
            Created = string.Empty;
            Modified = string.Empty;
            Source = string.Empty;
            Task = new List<RtmApiTaskObject>();
            Tags = new List<RtmApiTagObject>();
        }

        public override string ToString() => Name ?? string.Empty;

        public DateTime? DateCreated => DateTime.TryParse(Created, out DateTime dateCreated) ? dateCreated : (DateTime?)null;
        public DateTime? DateModified => DateTime.TryParse(Modified, out DateTime dateModified) ? dateModified : (DateTime?)null;
    }
}