namespace CarniceriaFinal.Security.Services.IServices
{
    public interface ICaptchaServices
    {
        Task<bool> IsCaptchaValid(string token);
    }
}
