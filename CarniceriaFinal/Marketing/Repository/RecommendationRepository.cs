﻿using CarniceriaFinal.Core.CustomException;
using CarniceriaFinal.Marketing.Repository.IRepository;
using CarniceriaFinal.ModelsEF;
using Microsoft.EntityFrameworkCore;

namespace CarniceriaFinal.Marketing.Repository
{
    public class RecommendationRepository : IRecommendationRepository
    {
        public readonly DBContext Context;
        public RecommendationRepository(DBContext _Context)
        {
            Context = _Context;
        }
        public async Task<List<MomentoDegustacion>> GetAllTimesToEatGeneralUser()
        {
            try
            {
                //ver que este coincida con el anterior valor seleccionado

                return await Context.MomentoDegustacions
                    .Where(x => x.Status == 1 && x.MomentoDegustacionInProductos
                    .Where(x => Context.PreparacionProductoInProductos
                    .Where(z => z.IdProducto == x.IdProducto && z.IdProductoNavigation.Status == 1 && z.IdProductoNavigation.Stock > 0)
                    .FirstOrDefault() != null).FirstOrDefault() != null)
                    .ToListAsync();

                //.MomentoDegustacionInProductos.Where(x => x.Status == 1).ToList().Count > 0
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Lista de Momento de degustación");
            }
        }
        public async Task<MomentoDegustacion> ChangeStatusTimesToEat(int idTimesToEat, int status)
        {
            try
            {
                var response = await Context.MomentoDegustacions
                    .Where(x => x.IdMomentoDegustacion == idTimesToEat)
                    .FirstOrDefaultAsync();

                if (response == null)
                    throw RSException.NoData("No se ha encotrado");

                response.Status = status;

                await Context.SaveChangesAsync();
                return response;
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Momento de degustación");
            }
        }
        public async Task<List<PreparacionProducto>> GetAllPreparationWaysGeneralUser(int idTimeToEat)
        {
            try
            {
                //ver que este coincida 
                return await Context.PreparacionProductos
                    .Where(x => x.Status == 1 &&
                                x.PreparacionProductoInProductos
                                .Where(y => Context.MomentoDegustacionInProductos
                                .Where(z => z.IdProducto == y.IdProducto && z.IdMomentoDegustacion == idTimeToEat 
                                        && z.IdProductoNavigation.Status == 1 && z.IdProductoNavigation.Stock > 0)
                                .FirstOrDefault() != null)
                                .FirstOrDefault() != null
                    ).ToListAsync();
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Lista de formas de preparación");
            }
        }
        public async Task<PreparacionProducto> ChangeStatusPreparationWays(int idPreparationWay, int status)
        {
            try
            {
                var response = await Context.PreparacionProductos
                    .Where(x => x.IdPreparacionProducto == idPreparationWay)
                    .FirstOrDefaultAsync();

                if (response == null)
                    throw RSException.NoData("No se ha encotrado");

                response.Status = status;
                await Context.SaveChangesAsync();

                return response;
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Modo de preparación");
            }
        }

        public async Task<List<MomentoDegustacion>> GetAllTimesToEat()
        {
            try
            {
                return await Context.MomentoDegustacions
                    .ToListAsync();
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Lista de Momento de degustación");
            }
        }
        public async Task<List<PreparacionProducto>> GetAllPreparationWays()
        {
            try
            {
                return await Context.PreparacionProductos
                    .ToListAsync();
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Lista de formas de preparación");
            }
        }
        public async Task<MomentoDegustacion> GetTimeToEatDetail(int idTimeToEat)
        {
            try
            {
                return await Context.MomentoDegustacions
                    .Where(x => x.IdMomentoDegustacion == idTimeToEat)
                    .Include(x => x.MomentoDegustacionInProductos)
                    .FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Detalle del momento de desgustación del producto");
            }
        }
        public async Task<PreparacionProducto> GetPreparationWayDetail(int idPreparationWay)
        {
            try
            {
                return await Context.PreparacionProductos
                    .Where(x => x.IdPreparacionProducto == idPreparationWay)
                    .Include(x => x.PreparacionProductoInProductos)
                    .FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Detalle del modo de preparación del producto");
            }
        }
        public async Task<PreparacionProducto> CreatePreparationWay(PreparacionProducto preparation, List<int> productIds)
        {
            try
            {
                preparation.Status = 1;
                await Context.PreparacionProductos.AddAsync(preparation);
                await Context.SaveChangesAsync();
                List<PreparacionProductoInProducto> list = new();

                foreach (var productId in productIds)
                {
                    list.Add(new PreparacionProductoInProducto
                    {
                        IdProducto = productId,
                        IdPreparacionProducto = preparation.IdPreparacionProducto
                    });
                };
                await Context.PreparacionProductoInProductos.AddRangeAsync(list);
                await Context.SaveChangesAsync();
                return preparation;
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Guardar el modo de preparación del producto");
            }
        }
        public async Task<MomentoDegustacion> CreateTimeToEat(MomentoDegustacion tomesToEat, List<int> preparationProducts)
        {
            try
            {
                tomesToEat.Status = 1;
                await Context.MomentoDegustacions.AddAsync(tomesToEat);
                await Context.SaveChangesAsync();
                List<MomentoDegustacionInProducto> list = new();

                foreach (var preparation in preparationProducts)
                {
                    list.Add(new MomentoDegustacionInProducto
                    {
                        IdMomentoDegustacion = tomesToEat.IdMomentoDegustacion,
                        IdProducto = preparation
                    });
                };
                await Context.MomentoDegustacionInProductos.AddRangeAsync(list);
                await Context.SaveChangesAsync();
                return tomesToEat;
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Guardar el momento de degustación del producto");
            }
        }

        //Actualizar correctamente - ambos
        public async Task<PreparacionProducto> UpdatePreparationWay(PreparacionProducto preparation, List<int> productIds)
        {
            try
            {
                var preparationDetail = await Context.PreparacionProductos
                    .Where(x => x.IdPreparacionProducto == preparation.IdPreparacionProducto)
                    .Include(x => x.PreparacionProductoInProductos)
                    .FirstOrDefaultAsync();

                if (preparationDetail == null)
                    throw RSException.NoData("No se ha encontrado información de la preparación");

                preparationDetail.Titulo = preparation.Titulo;
                preparationDetail.Descripcion = preparation.Descripcion;
                preparationDetail.Imagen = preparation.Imagen;

                var listPreparation = preparationDetail.PreparacionProductoInProductos.Select(x => x.IdProducto.Value).ToList();

                //Obtener los productos a agregar
                var productsToAdd = productIds
                    .Where(x => !listPreparation.Contains(x))
                    .ToList();


                //Obtener los productos a eliminar
                var productsToDelete = preparationDetail
                    .PreparacionProductoInProductos
                    .Where(x => !productIds.Contains(x.IdProducto.Value))
                    .ToList();

                Context.PreparacionProductoInProductos.RemoveRange(productsToDelete);
                Context.SaveChanges();

                List<PreparacionProductoInProducto> list = new();

                foreach (var productId in productsToAdd)
                {
                    list.Add(new PreparacionProductoInProducto
                    {
                        IdProducto = productId,
                        IdPreparacionProducto = preparation.IdPreparacionProducto
                    });
                };
                await Context.PreparacionProductoInProductos.AddRangeAsync(list);
                await Context.SaveChangesAsync();

                return preparation;
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Modo de preparación del producto");
            }
        }
        public async Task<MomentoDegustacion> UpdateTimeToEat(MomentoDegustacion tomesToEat, List<int> idProducts)
        {
            try
            {
                var timeToEatDetail = await Context.MomentoDegustacions
                    .Where(x => x.IdMomentoDegustacion == tomesToEat.IdMomentoDegustacion)
                    .Include(x => x.MomentoDegustacionInProductos)
                    .FirstOrDefaultAsync();

                if (timeToEatDetail == null)
                    throw RSException.NoData("No se ha encontrado información del momento de degustación");

                timeToEatDetail.Descripcion = tomesToEat.Descripcion;
                timeToEatDetail.UrlImage = tomesToEat.UrlImage;
                timeToEatDetail.Titulo = tomesToEat.Titulo;


                var listPreparation = timeToEatDetail.MomentoDegustacionInProductos.Select(x => x.IdProducto).ToList();

                //Obtener los productos a agregar
                var productsToAdd = idProducts
                    .Where(x => !listPreparation.Contains(x))
                    .ToList();


                //Obtener los productos a eliminar
                var productsToDelete = timeToEatDetail
                    .MomentoDegustacionInProductos
                    .Where(x => !idProducts.Contains(x.IdProducto))
                    .ToList();

                Context.MomentoDegustacionInProductos.RemoveRange(productsToDelete);
                Context.SaveChanges();

                List<MomentoDegustacionInProducto> list = new();

                foreach (var productId in productsToAdd)
                {
                    list.Add(new MomentoDegustacionInProducto
                    {
                        IdMomentoDegustacion = tomesToEat.IdMomentoDegustacion,
                        IdProducto = productId
                    });
                };
                await Context.MomentoDegustacionInProductos.AddRangeAsync(list);
                await Context.SaveChangesAsync();

                return tomesToEat;
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Momento de preparación del producto");
            }
        }


        //Crear Recomendación
        public async Task<List<EventoEspecial>> GetAllSpecialEvent()
        {
            try
            {
                return await Context.EventoEspecials.ToListAsync();
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Obtener Eventos especiales");
            }
        }
        public async Task<EventoEspecial> GetAllSpecialEventByIdEvent(int idEvent)
        {
            try
            {
                return await Context.EventoEspecials.Where(x => x.IdEventoEspecial == idEvent).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Obtener Evento especiale por Id");
            }
        }
        public async Task<EventoEspecial> CreateSpecialEvent(EventoEspecial specialEvent)
        {
            try
            {
                specialEvent.Status = 1;
                await Context.EventoEspecials
                    .AddAsync(specialEvent);

                await Context.SaveChangesAsync();

                return specialEvent;
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Crear Evento especial");
            }
        }
        public async Task<EventoEspecial> UpdateSpecialEvent(EventoEspecial specialEvent)
        {
            try
            {
                var specialEventResponse = await Context.EventoEspecials
                    .Where(x => x.IdEventoEspecial == specialEvent.IdEventoEspecial)
                    .FirstOrDefaultAsync();

                specialEventResponse.Titulo = specialEvent.Titulo;
                specialEventResponse.FechaFin = specialEvent.FechaFin;
                specialEventResponse.FechaInicio = specialEvent.FechaInicio;
                specialEventResponse.Descripcion = specialEvent.Descripcion;
                specialEventResponse.Imagen = specialEvent.Imagen;

                await Context.SaveChangesAsync();

                return specialEvent;
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Actualizar Evento especial");
            }
        }
        public async Task<EventoEspecial> DisableSpecialEvent(int idSpecialEvent)
        {
            try
            {
                var specialEventResponse = await Context.EventoEspecials
                    .Where(x => x.IdEventoEspecial == idSpecialEvent)
                    .FirstOrDefaultAsync();

                specialEventResponse.Status = 0;

                await Context.SaveChangesAsync();
                return specialEventResponse;
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Desactivar Evento especial");
            }
        }
        public async Task<EventoEspecial> EnableSpecialEvent(int idSpecialEvent)
        {
            try
            {
                var specialEventResponse = await Context.EventoEspecials
                    .Where(x => x.IdEventoEspecial == idSpecialEvent)
                    .FirstOrDefaultAsync();

                specialEventResponse.Status = 1;

                await Context.SaveChangesAsync();
                return specialEventResponse;
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Activar Evento especial");
            }
        }

        //isValidCreateSpecialEvent
        public async Task<Boolean> isValidCreateSpecialEvent(EventoEspecial specialEvent)
        {
            try
            {
                var eventResponse = await Context.EventoEspecials
                    .AsNoTracking()
                    .ToListAsync();

                var activates = eventResponse.Where(x => x.Status == 1 &&
                        (((DateTime.Compare(x.FechaFin, specialEvent.FechaInicio) >= 0) && (DateTime.Compare(x.FechaInicio, specialEvent.FechaInicio) <= 0))
                        || ((DateTime.Compare(x.FechaInicio, specialEvent.FechaFin) <= 0) && (DateTime.Compare(x.FechaFin, specialEvent.FechaInicio) >= 0))
                        || ((DateTime.Compare(specialEvent.FechaInicio, x.FechaInicio) < 0) && (DateTime.Compare(specialEvent.FechaFin, x.FechaFin) > 0)))
                    )
                    .FirstOrDefault();

                if (activates == null)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Consultar validez de crear evento especial");
            }
        }

        //isValidActivateSpecialEvent
        public async Task<Boolean> isValidActivateSpecialEvent(int idSpecialEvent)
        {
            try
            {
                var eventResponse = await Context.EventoEspecials
                    .Where(x => x.IdEventoEspecial == idSpecialEvent)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

                var events = await Context.EventoEspecials
                    .Where(x => x.IdEventoEspecial != idSpecialEvent)
                    .AsNoTracking()
                    .ToListAsync();

                var activates = events.Where(x => x.Status == 1 &&
                        (((DateTime.Compare(x.FechaFin, eventResponse.FechaInicio) > 0) && (DateTime.Compare(x.FechaInicio, eventResponse.FechaInicio) < 0))
                        || ((DateTime.Compare(x.FechaInicio, eventResponse.FechaFin) < 0) && (DateTime.Compare(x.FechaFin, eventResponse.FechaFin) > 0))
                        || ((DateTime.Compare(eventResponse.FechaInicio, x.FechaInicio) < 0) && (DateTime.Compare(eventResponse.FechaFin, x.FechaFin) > 0)))
                    )
                    .FirstOrDefault();

                if (activates == null)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Consultar validez de crear evento especial");
            }

        }
        public async Task<Boolean> isValidUpdateSpecialEvent(EventoEspecial specialEvent)
        {
            try
            {
                var events = await Context.EventoEspecials
                    .Where(x => x.IdEventoEspecial != specialEvent.IdEventoEspecial && x.Status == 1)
                    .AsNoTracking()
                    .ToListAsync();

                var activates = events.Where(x => ((DateTime.Compare(x.FechaFin, specialEvent.FechaInicio) > 0) && (DateTime.Compare(x.FechaInicio, specialEvent.FechaInicio) < 0))
                        || ((DateTime.Compare(x.FechaInicio, specialEvent.FechaFin) < 0) && (DateTime.Compare(x.FechaFin, specialEvent.FechaFin) > 0))
                        || ((DateTime.Compare(specialEvent.FechaInicio, x.FechaInicio) < 0) && (DateTime.Compare(specialEvent.FechaFin, x.FechaFin) > 0))
                    )
                    .FirstOrDefault();

                if (activates == null)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Consultar validez de crear evento especial");
            }
        }
        public async Task<EventoEspecial> GetCommunicationSpecialEvent()
        {
            try
            {
                //ver que este coincida con el anterior valor seleccionado

                using (var _Context = new DBContext())
                {
                    var events = await _Context.EventoEspecials.Where(x => (
                        x.Status == 1
                    ))
                    .ToListAsync();

                    if (events == null || events.Count == 0)
                        return null;


                    var evento = events.Where(x =>
                        ((DateTime.Compare(x.FechaFin, DateTime.Now) > 0) && (DateTime.Compare(x.FechaInicio, DateTime.Now) < 0))
                        || ((DateTime.Compare(x.FechaInicio, DateTime.Now) < 0) && (DateTime.Compare(x.FechaFin, DateTime.Now) > 0))
                    ).FirstOrDefault();

                    return evento;

                }

            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<List<Producto>> GetProductsRecommendationByPreparationAndTimeToEat(int idPreparationWay, int idTimeToEat)
        {
            try
            {
                using (var _Context = new DBContext())
                {
                    var products = await _Context.Productos
                     .Where(x => x.Status == 1 && x.Stock > 0)
                    .Include(x => x.PreparacionProductoInProductos)
                    .Include(x => x.MomentoDegustacionInProductos)
                    .ToListAsync();

                    List<Producto> productsRespo = new();

                    foreach (var product in products)
                    {
                        if(product.PreparacionProductoInProductos
                            .Where(x => x.IdPreparacionProducto == idPreparationWay)
                            .Select(x => x.IdProducto)
                            .Contains(product.IdProducto)
                            &&
                            product.MomentoDegustacionInProductos
                            .Where(x => x.IdMomentoDegustacion == idTimeToEat)
                            .Select(x => x.IdProducto)
                            .Contains(product.IdProducto)
                            )
                        {
                            productsRespo.Add(product);
                        }
                    }


                    return productsRespo;

                }

            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("Consultar productos que apliquen a los filtros indicados");
            }
        }
    }
}
