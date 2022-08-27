using CarniceriaFinal.Core.JWTOKEN.Repository.IRepository;
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
        private readonly IConfiguration Configuration;
        private Timer _timer;
        private readonly ILogger _logger;
        public SaleStateManagement(IServiceProvider IServiceProvider, ILogger<SaleStateManagement> logger, IConfiguration configuration)
        {
            this.IServiceProvider = IServiceProvider;
            _logger = logger;
            Configuration = configuration;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(
            ProcessToHandle,
            null,
            TimeSpan.FromSeconds(20),
            TimeSpan.FromDays(5)
            );

            Timer _t = new Timer(
                HandleStatusSales,
                null,
                TimeSpan.Zero,
                TimeSpan.FromMilliseconds(int.Parse(Configuration["AppConstants:MiliSegToCheckSale"]))
            );

            

            return Task.CompletedTask;
        }


        private async void ProcessToHandle(object state)
        {
            
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
        public async void HandleStatusSales(object state)
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
                        return;

                    var response = ISaleManagementHelper.getPendingSalesIDs(sale);


                    if (response.Count == 0)
                        return;
                    var tasks = new List<Task>();

                    foreach (var item in response)
                    {
                        item.status = 3;
                        var saleToUpdate = ISaleManagementHelper.SaleEntityToUpdate(item);
                        if (saleToUpdate != null)
                        {
                            tasks.Add(Task.Run(async () =>
                            {
                                Context.Venta.Update(saleToUpdate);
                                await Context.SaveChangesAsync();
                                await restoreStockByIdSale(saleToUpdate.IdVenta);
                            }));
                        }


                    }
                    await Task.WhenAll(tasks);
                }
                catch (Exception err)
                {
                    return;
                }
            }
                return;
        }

        public async Task<Boolean> restoreStockByIdSale(int idSale)
        {
            try
            {
                using (var scope = IServiceProvider.CreateScope())
                {
                    var Context = scope.ServiceProvider.GetRequiredService<DBContext>();

                    var salesDetails = await Context
                        .DetalleVenta
                        .Where(x => x.IdVenta == idSale)
                        .AsNoTracking()
                        .ToListAsync();

                    foreach (var sale in salesDetails)
                    {
                        var product = await Context
                        .Productos
                        .Where(x => x.IdProducto == sale.IdProducto)
                        .FirstOrDefaultAsync();

                        product.Stock = product.Stock + sale.Cantidad.Value;

                    }
                    await Context.SaveChangesAsync();

                }

            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

    }
}
