using AutoMapper;
using CarniceriaFinal.Core.JWTOKEN.DTOs;
using CarniceriaFinal.Core.JWTOKEN.Services.IServices;
using Microsoft.Extensions.Options;

namespace CarniceriaFinal.Core.JWTOKEN.Services
{
    public class LogsServices : ILogsServices
    {
        private readonly IMapper IMapper;
        protected readonly IOptions<ModulesConfiguration> modules;
        public LogsServices(IMapper IMapper, IOptions<ModulesConfiguration> Imodules)
        {
            this.IMapper = IMapper;
            this.modules = Imodules;
        }


        public LogsEntity mapperValues(LogsEntity logMapper, string controllName)
        {
            try
            {
                //log.hostname
                //log.timestamp
                //log.metodo
                //log.endpoint


                //idLog
                //hostname
                //timestamp
                //metodo
                //endpoint

                //modulo
                //idModulo


                //mensaje
                //estadoHTTP
                Boolean isActivate = false;

                foreach (var module in modules.Value.Modules)
                {
                    if (module.controllName.ToLower() == controllName.ToLower())
                    {
                        logMapper.idModulo = module.idModule;
                        logMapper.modulo = module.moduleName;
                        logMapper.pathEndPoint = module.baseUrl;
                        isActivate = true;
                        if (module.endpoints.Where(x => x.nameEndpoint == logMapper.endpoint).FirstOrDefault() == null)
                            break;

                        logMapper.pathEndPoint = module.baseUrl + "/" +
                            module.endpoints
                            .Where(x => x.nameEndpoint == logMapper.endpoint)
                            .FirstOrDefault().pathEndpoint;

                        break;
                    }
                }

                return isActivate ? logMapper : null;
            }
            catch (Exception err)
            {

            }
            return null;
        }
        public string getMessage(string data)
        {
            try
            {
                var start = data.Substring(data.IndexOf("[\""), data.Length - data.IndexOf("[\""));
                var ends = start.IndexOf("],");
                return start.Substring(1, ends - 1);


            }
            catch (Exception err)
            {
            }
            return data;
        }
    }
}
