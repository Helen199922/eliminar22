namespace CarniceriaFinal.Core.JWTOKEN.DTOs
{
    public class LogsEntity
    {
        public LogsEntity()
        {
            this.modulo = "";
            this.metodo = "";
            mensaje = "";
            idModulo = 0;
            estadoHTTP = 0;
            timestamp = DateTime.Now;
            hostname = "";
            endpoint = "";
        }

        public int? idLog { get; set; }
        public string modulo { get; set; }
        public string metodo { get; set; }
        public int idModulo { get; set; }
        public string mensaje { get; set; }
        public int estadoHTTP { get; set; }
        public DateTime timestamp { get; set; }
        public string hostname { get; set; }
        public string endpoint { get; set; }
        public string pathEndPoint { get; set; }
        public int? idUser { get; set; }
    }

    public class ModulesConfiguration
    {
        public List<ModulesInfoConfiguration> Modules { get; set; }
    }

    public class ModulesInfoConfiguration
    {
        public string moduleName { get; set; }
        public string controllName { get; set; }
        public string baseUrl { get; set; }
        public int idModule { get; set; }
        public List<EndPointsConfigurationModules> endpoints { get; set; }
    }
    public class EndPointsConfigurationModules
    {
        public string nameEndpoint { get; set; }
        public string pathEndpoint { get; set; }
        public string method { get; set; }
    }
}
