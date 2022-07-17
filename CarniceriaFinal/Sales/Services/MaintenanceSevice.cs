using AutoMapper;
using CarniceriaFinal.Sales.Models;
using CarniceriaFinal.Sales.Services.IServices;
using CarniceriaFinal.Security.IRepository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Sales.Services
{
    public class MaintenanceSevice : IMaintenanceSevice
    {
        private readonly ISaleRepository ISaleRepository;
        private readonly IMapper IMapper;
        private readonly ILogger _logger;
        private readonly IConfiguration Configuration;
        public MaintenanceSevice(ISaleRepository ISaleRepository, ILogger<MaintenanceSevice> logger, IConfiguration configuration, IMapper IMapper)
        {
            this.ISaleRepository = ISaleRepository;
            _logger = logger;
            Configuration = configuration;
            this.IMapper = IMapper;
        }
        //Obtener todas las ventas que concuerden con el status pendiente y si ha pasado más de 35 días sin atender mostrar como rechazada
        public async Task<List<int>> getPendingSalesIDs()
        {
            List<int> PendingSalesID = new();
            DateTime DateNow = DateTime.Now;
            try
            {
                var sales = await ISaleRepository.GetAllSalesPending();
                foreach (var item in sales)
                {
                    var sale = IMapper.Map<SaleEntity>(item);
                    TimeSpan difDate = sale.fecha.Value - DateNow;
                    var maximum = Configuration["AppConstants:MaximumDaysState"];
                    if (difDate.TotalDays > Int32.Parse(maximum))
                        PendingSalesID.Add(sale.idVenta.Value);
                }
            }
            catch(Exception)
            {
                _logger.LogInformation("No se ha podido recuperar información de las ventas pedientes");
            }

            return PendingSalesID;
        }
    }
}
