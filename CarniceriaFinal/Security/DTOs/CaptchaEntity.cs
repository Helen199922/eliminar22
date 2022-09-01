using Newtonsoft.Json;

namespace CarniceriaFinal.Security.DTOs
{
    public class IsValidCaptchaRequest
    {
        public string Token { get; set; }
    }
    public class CaptchaVerificationResponse
    {
        public bool Success { get; set; }


        [JsonProperty("challenge_ts")]
        public DateTime ChallengeTimestamp { get; set; }


        public string Hostname { get; set; }


        [JsonProperty("error-codes")]
        public string[] Errorcodes { get; set; }


    }
}
