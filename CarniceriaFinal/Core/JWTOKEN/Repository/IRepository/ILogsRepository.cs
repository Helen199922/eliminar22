using CarniceriaFinal.Core.JWTOKEN.DTOs;

namespace CarniceriaFinal.Core.JWTOKEN.Repository.IRepository
{
    public interface ILogsRepository
    {
        Task<Boolean> SaveLogs(LogsEntity? logs);
    }
}
