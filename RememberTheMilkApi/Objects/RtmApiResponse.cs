using Newtonsoft.Json;

namespace RememberTheMilkApi.Objects
{
    [JsonObject("rsp")]
    public class RtmApiResponse
    {
        [JsonProperty("stat")]
        public string Status { get; set; }

        [JsonProperty("api_key")]
        public string ApiKey { get; set; }

        [JsonProperty("format")]
        public string Format { get; set; }

        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("frob")]
        public string Frob { get; set; }

        [JsonProperty("auth")]
        public RtmApiAuthResponse Auth { get; set; }

        [JsonProperty("tasks")]
        public dynamic Tasks { get; set; }

        [JsonProperty("err")]
        public RtmApiErrorResponse Error { get; set; }

        public RtmApiResponse()
        {
            Status = string.Empty;
            ApiKey = string.Empty;
            Format = string.Empty;
            Method = string.Empty;
            Frob = string.Empty;
            Auth = new RtmApiAuthResponse();
            Error = new RtmApiErrorResponse();
        }
    }
}