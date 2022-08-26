using CarniceriaFinal.Core.JWTOKEN.DTOs;

namespace CarniceriaFinal.Core.JWTOKEN.Services.IServices
{
    public interface ILogsServices
    {
        LogsEntity mapperValues(LogsEntity logMapper, string controllName);
        string getMessage(string data);
    }
}
