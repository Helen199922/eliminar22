using CarniceriaFinal.Core.CustomException;
using CarniceriaFinal.Marketing.DTOs;
using CarniceriaFinal.Marketing.Interfaces.IRepository;
using CarniceriaFinal.ModelsEF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Marketing.Repository
{
    public class PromotionRepository : IPromotionRepository
    {
        public readonly DBContext Context;
        public PromotionRepository(DBContext _Context)
        {
            Context = _Context;
        }
        public async Task<List<Promocion>> GetAll()
        {
            try
            {
                using (var _Context = new DBContext())
                {
                    return await _Context.Promocions
                    .ToListAsync();
                }
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Lista de Promociones");
            }
        }
        public async Task<List<PromotionProductEntity>> GetAllProductsByIdPromotion(int? idPromotion)
        {
            try
            {
                using (var _Context = new DBContext())
                {
                    if (idPromotion != null && idPromotion > 0)
                    {
                        var productByPromotion = await _Context.PromocionInProductos
                            .Include(x => x.IdProductoNavigation)
                            .Where(x => x.IdPromocion == idPromotion
                                        && x.IdProductoNavigation.Status == 1
                                        && x.IdProductoNavigation != null
                            )
                            .Select(y => new PromotionProductEntity
                            {
                                idProduct = y.IdProducto,
                                titulo = y.IdProductoNavigation.Titulo,
                                stock = y.IdProductoNavigation.Stock,
                                isActivate = true
                            })
                            .AsNoTracking()
                            .ToListAsync();

                        var productsToPromo = await GetAllProductsEnableToPromotion();

                        if (productByPromotion.Count == 0)
                            return productsToPromo;

                        var productSelected = productsToPromo
                            .Where(promo => !productByPromotion.Any(y => y.idProduct == promo.idProduct))
                            .ToList();

                        if (productByPromotion != null && productByPromotion.Count > 0)
                            productByPromotion.AddRange(productSelected);

                        return productByPromotion;
                    }
                    
                    return await GetAllProductsEnableToPromotion();
                }
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("Productos por promoción");
            }
        }
        private async Task<List<PromotionProductEntity>> GetAllProductsEnableToPromotion()
        {
            try
            {
                using (var _Context = new DBContext())
                {
                    var products = await _Context.Productos
                            .Where(x => x.Status == 1)
                            .Select(y => new PromotionProductEntity
                            {
                                idProduct = y.IdProducto,
                                titulo = y.Titulo,
                                stock = y.Stock,
                                isActivate = false
                            })
                            .AsNoTracking()
                            .ToListAsync();

                    return products;
                }
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Productos por promoción");
            }
        }
        public async Task<Promocion> getLastPromotion()
        {
            try
            {
                using (var _Context = new DBContext())
                {
                    var option = await _Context.Promocions.Where(x => (
                        x.Status == 1
                    ))
                    .FirstOrDefaultAsync();


                    if (option == null) return null;

                    if (DateTime.Compare(option.FechaInicio, DateTime.Now) <= 0 && DateTime.Compare(option.FechaFin, DateTime.Now) >= 0)
                        return option;

                    return null;
                }
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("obtener promociones disponibles");
            }
        }
        public async Task<Promocion> PromotionIsActivate(int idPromotion)
        {
            try
            {
                using (var _Context = new DBContext())
                {
                    var option = await _Context.Promocions.Where(x => (
                        x.Status == 1
                        && x.IdPromocion == idPromotion
                    ))
                    .FirstOrDefaultAsync();

                    if (option == null) return null;

                    if (DateTime.Compare(option.FechaInicio, DateTime.Now) <= 0 && DateTime.Compare(option.FechaFin, DateTime.Now) >= 0)
                        return option;

                    return null;
                }
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("obtener promocion activa");
            }
        }
        public async Task<Promocion> CreatePromotion(Promocion promotion)
        {
            try
            {
                promotion.Status = 1;
                var lastPromotion = await this.getLastPromotion();
                if (lastPromotion != null) 
                    throw RSException.BadRequest("Ya existe una promoción vigente. Puede cancelar o esperar a que termine");

                await Context.Promocions.AddAsync(promotion);
                await Context.SaveChangesAsync();
                return promotion;
            }
            catch (RSException err){
                throw RSException.BadRequest("Ya existe una promoción vigente. Puede cancelar o esperar a que termine");
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Crear Promocion");
            }
        }
        public async Task<Promocion> UpdatePromotion(Promocion promotion)
        {
            try
            {
                int idPromotion = promotion.IdPromocion;

                using (var _Context = new DBContext())
                {
                    var promotionDB = await _Context.Promocions
                        .Where(x => x.IdPromocion == idPromotion)
                        .FirstOrDefaultAsync();
                    if (promotionDB == null) return promotionDB;

                    promotionDB.FechaInicio = promotion.FechaInicio;
                    promotionDB.FechaFin = promotion.FechaFin;
                    promotionDB.FechaUpdate = DateTime.Now;
                    promotionDB.DsctoMonetario = promotion.DsctoMonetario;
                    promotionDB.Status = promotion.Status;
                    promotionDB.Titulo = promotion.Titulo;
                    promotionDB.MaxParticipantes = promotion.MaxParticipantes;
                    promotionDB.PorcentajePromo = promotion.PorcentajePromo;
                    promotionDB.Imagen = promotion.Imagen;
                    
                    await _Context.SaveChangesAsync();
                }
                return promotion;
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Actualizar promoción");
            }
        }
        public async Task<Promocion> StatusPromotion(int status, int idPromotion)
        {
            try
            {
                using (var _Context = new DBContext())
                {
                    var promotionDB = await _Context.Promocions
                        .Where(x => x.IdPromocion == idPromotion)
                        .FirstOrDefaultAsync();
                    if (promotionDB == null) return promotionDB;

                    promotionDB.Status = status;

                    await _Context.SaveChangesAsync();
                    return promotionDB;
                }
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Cambiar estado de promoción");
            }
        }
    }
}
