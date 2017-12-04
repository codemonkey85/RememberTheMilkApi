using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace RememberTheMilkApi.Objects
{
    [JsonObject("taskseries")]
    public class RtmApiTaskSeries
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("created")]
        public string Created { get; set; }

        [JsonProperty("modified")]
        public string Modified { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        #region In Development

        [JsonProperty("tags")]
        public dynamic Tags { get; set; }

        [JsonProperty("participants")]
        public dynamic Participants { get; set; }

        [JsonProperty("notes")]
        public dynamic Notes { get; set; }

        [JsonProperty("task")]
        [JsonConverter(typeof(SingleOrArrayConverter<RtmApiTaskObject>))]
        public IList<RtmApiTaskObject> Task { get; set; }

        #endregion In Development

        public RtmApiTaskSeries()
        {
            Id = string.Empty;
            Name = string.Empty;
            Created = string.Empty;
            Modified = string.Empty;
            Source = string.Empty;
            Task = new List<RtmApiTaskObject>();
        }

        public override string ToString()
        {
            return Name ?? string.Empty;
        }
    }

    internal class SingleOrArrayConverter<T> : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(List<T>));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken token = JToken.Load(reader);
            if (token.Type == JTokenType.Array)
            {
                return token.ToObject<List<T>>();
            }
            return new List<T> { token.ToObject<T>() };
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}