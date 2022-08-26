﻿using CarniceriaFinal.Core.JWTOKEN.Repository.IRepository;
using CarniceriaFinal.Core.SaleStateHosted.Interface;
using CarniceriaFinal.ModelsEF;
using CarniceriaFinal.Sales.IServices;
using CarniceriaFinal.Sales.Services.IServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CarniceriaFinal.Core
{
    internal class SaleStateManagement : IHostedService
    {
        private readonly IServiceProvider IServiceProvider;
        private Timer _timer;
        private readonly ILogger _logger;
        public SaleStateManagement(IServiceProvider IServiceProvider, ILogger<SaleStateManagement> logger)
        {
            this.IServiceProvider = IServiceProvider;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(
            ProcessToHandle,
            null,
            TimeSpan.FromSeconds(20),
            TimeSpan.FromDays(5)
        );

            return Task.CompletedTask;
        }

        private async void ProcessToHandle(object state)
        {
            await this.HandleStatusSales(state);
            await this.HandleLogs(state);
        }

        public async Task<Boolean> HandleLogs(object state)
        {
            using (var scope = IServiceProvider.CreateScope())
            {
                var ILogs = scope.ServiceProvider.GetRequiredService<ILogsRepository>();

                try
                {
                    await ILogs.DeleteLogs(DateTime.Now.AddDays(-5));
                }
                catch (Exception err)
                {
                    return true;
                }
                _logger.LogInformation("Estado cambiado de forma automática");
                return true;
            }
        }
        public async Task<Boolean> HandleStatusSales(object state)
        {
            using (var scope = IServiceProvider.CreateScope())
            {
                var Context = scope.ServiceProvider.GetRequiredService<DBContext>();
                var ISaleManagementHelper = scope.ServiceProvider.GetRequiredService<ISaleManagementHelper>();

                List<Ventum> sale = null;
                try
                {
                    sale = await Context.Venta.Where(x => x.IdStatus == 1).AsNoTracking().ToListAsync();
                    if (sale == null)
                        return true;

                    var response = ISaleManagementHelper.getPendingSalesIDs(sale);
                    var sales = response
                        .Where(x => DateTime
                                    .Compare(x.fecha.Value, DateTime.Now.AddDays(-15)) <= 0
                              )
                        .ToList();

                    if (sales.Count == 0)
                        return true;
                    var tasks = new List<Task>();

                    foreach (var item in sales)
                    {
                        item.status = 3;
                        var saleToUpdate = ISaleManagementHelper.SaleEntityToUpdate(item);
                        if (saleToUpdate != null)
                        {
                            tasks.Add(Task.Run(async () =>
                            {
                                Context.Venta.Update(saleToUpdate);
                                await Context.SaveChangesAsync();
                            }));
                        }
                    }
                    await Task.WhenAll(tasks);
                }
                catch (Exception err)
                {
                    return false;
                }
                _logger.LogInformation("Estado cambiado de forma automática");
            }
                return true;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

    }
}
