using AutoMapper;
using CarniceriaFinal.Core.SaleStateHosted.Interface;
using CarniceriaFinal.ModelsEF;
using CarniceriaFinal.Sales.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Core.SaleStateHosted
{
    public class SaleManagementHelper : ISaleManagementHelper
    {
        private readonly IMapper IMapper;
        private readonly ILogger _logger;
        private readonly IConfiguration Configuration;
        public SaleManagementHelper( ILogger<SaleManagementHelper> logger, IConfiguration configuration, IMapper IMapper)
        {
            _logger = logger;
            Configuration = configuration;
            this.IMapper = IMapper;
        }
        public List<SaleEntity> getPendingSalesIDs(List<Ventum> sales)
        {
            List<SaleEntity> PendingSalesID = new();
            try
            {
                foreach (var item in sales)
                {
                    DateTime times = DateTime.Now;
                    var sale = IMapper.Map<SaleEntity>(item);
                    DateTime timeEnd = sale.fechaFinal == null ? times : sale.fechaFinal.Value;
                    DateTime timeStart = sale.fecha.Value;

                    //var maximum = Configuration["AppConstants:MaximumDaysState"];
                    if(DateTime.Compare(timeEnd, times) <= 0 && sale.status == 1)
                    {
                        PendingSalesID.Add(sale);
                    }
                }
            }
            catch (Exception)
            {
                _logger.LogInformation("No se ha podido recuperar información de las ventas pedientes");
            }

            return PendingSalesID;
        }
        public Ventum SaleEntityToUpdate(SaleEntity sale)
        {
            Ventum saleToUpdate = null;
            try
            {
                saleToUpdate = IMapper.Map<Ventum>(sale);
                saleToUpdate.IdImpuesto = null;
                return saleToUpdate;
            }
            catch (Exception)
            {
                _logger.LogInformation("No se ha podido recuperar información de las ventas pedientes");
            }
            return saleToUpdate;
        }

    }
}
