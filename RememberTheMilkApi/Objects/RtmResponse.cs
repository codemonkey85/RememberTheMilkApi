using Newtonsoft.Json;

namespace RememberTheMilkApi.Objects
{
    [JsonObject("rsp")]
    public class RtmResponse
    {
        [JsonProperty("stat")]
        public string Status { get; set; }

        [JsonProperty("api_key")]
        public string ApiKey { get; set; }

        [JsonProperty("format")]
        public string Format { get; set; }

        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("err")]
        public RtmErrorResponse Error { get; set; }

        public RtmResponse()
        {
            Status = string.Empty;
            ApiKey = string.Empty;
            Format = string.Empty;
            Method = string.Empty;
            Error = new RtmErrorResponse();
        }
    }
}