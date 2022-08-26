using CarniceriaFinal.Core.CustomException;
using CarniceriaFinal.Core.JWTOKEN.DTOs;
using CarniceriaFinal.Core.JWTOKEN.Repository.IRepository;
using CarniceriaFinal.ModelsEF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CarniceriaFinal.Core.JWTOKEN.Repository
{
    public class LogsRepository : ILogsRepository
    {
        public readonly DBContext Context;
        protected readonly IOptions<ModulesConfiguration> modules;
        public LogsRepository(DBContext _Context, IOptions<ModulesConfiguration> Imodules)
        {
            Context = _Context;
            modules = Imodules;
        }
        public async Task<Boolean> SaveLogs(LogsEntity? logs)
        {
            try
            {
                if (logs == null) return false;

                using (var _Context = new DBContext())
                {
                    await _Context.Logs.AddAsync(new Log()
                    {
                        Endpoint = logs.endpoint,
                        EstadoHttp = logs.estadoHTTP,
                        Hostname = logs.hostname,
                        IdModulo = logs.idModulo,
                        Mensaje = logs.mensaje,
                        Metodo = logs.metodo,
                        Modulo = logs.modulo,
                        PathEndpoint = logs.pathEndPoint,
                        Timestamp = logs.timestamp,
                        IdUser = logs.idUser != null ? logs.idUser : null,
                    });
                    await _Context.SaveChangesAsync();
                }
                return new();
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("Logs");
            }
        }
        public async Task<Boolean> DeleteLogs(DateTime timeEnd)
        {
            try
            {

                using (var _Context = new DBContext())
                {
                    var logsList = await _Context.Logs.ToListAsync();
                    var logsToDelete = logsList.Where(x => DateTime.Compare(x.Timestamp, timeEnd) <= 0)
                        .ToList();
                    _Context.RemoveRange(logsToDelete);
                    await _Context.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception err)
            {
                return false;
            }
        }
    }
}
