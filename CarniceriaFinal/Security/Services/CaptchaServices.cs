using CarniceriaFinal.Security.DTOs;
using CarniceriaFinal.Security.Services.IServices;
using Newtonsoft.Json;

namespace CarniceriaFinal.Security.Services
{
    public class CaptchaServices : ICaptchaServices
    {
        private readonly IConfiguration Configuration;
        public CaptchaServices(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public async Task<bool> IsCaptchaValid(string token)
        {
            var result = false;

            var googleVerificationUrl = "https://www.google.com/recaptcha/api/siteverify";

            try
            {
                using var client = new HttpClient();
                var serverKey = Configuration["Captcha:ServerKey"];

                var response = await client.PostAsync($"{googleVerificationUrl}?secret={serverKey}&response={token}", null);
                var jsonString = await response.Content.ReadAsStringAsync();
                var captchaVerfication = JsonConvert.DeserializeObject<CaptchaVerificationResponse>(jsonString);

                result = captchaVerfication.Success;
            }
            catch (Exception e)
            {
                // fail gracefully, but log
            }

            return result;
        }
    }
}
