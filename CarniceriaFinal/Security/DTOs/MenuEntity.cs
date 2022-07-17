using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Security.DTOs
{
    public class MenuEntity
    {
        public string? module { get; set; }
        public string? icon { get; set; }
        public int id { get; set; }
        public string? route { get; set; }
        public List<Item> items { get; set; }
    }

    public class Item
    {
        public string? option { get; set; }
        public string? icon { get; set; }
        public int id { get; set; }
        public string? route { get; set; }
    }
}
