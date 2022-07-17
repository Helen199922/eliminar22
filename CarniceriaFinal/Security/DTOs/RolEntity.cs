using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Roles.DTOs
{
    public class DetailRole
    {
        public int idRol { get; set; }
        public string? nombre { get; set; }
        public int status { get; set; }
        public int cantOpciones { get; set; }
        public int cantUsuarios { get; set; }

    }
    public class RolOptionModules
    {
        public int idModulo { get; set; }
        public string? titulo { get; set; }
        public List<RolOptions> options { get; set; }
    }
    public class RolOptions
    {
        public int idOption { get; set; }
        public string? opcion { get; set; }
        public Boolean isActivate { get; set; }
    }
    public class RolEntity
    {
        public int idRol { get; set; }
        public string? nombre { get; set; }
        public string? descripcion { get; set; }
        public int status { get; set; }
    }
}
