using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Core
{
    public static class Helper
    {
        public static DateTime? getTimeCurrent()
        {
            return DateTime.Now;
        }

        public static string toMaskCedula(string cedula)
        {
            try
            {
                int startIndex = 0;
                int endIndex = cedula.Length - 4;
                string title = cedula.Substring(startIndex, endIndex);
                return title + "XXXX";
            }
            catch (Exception err)
            {
                return "XXXX";
            }
        }
    }
}
