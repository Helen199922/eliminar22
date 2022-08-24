using CarniceriaFinal.Core.CustomException;
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
                    .Where(z => z.IdProducto == x.IdProducto).FirstOrDefault() != null).FirstOrDefault() != null)
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
                                .Where( z => z.IdProducto == y.IdProducto && z.IdMomentoDegustacion == idTimeToEat).FirstOrDefault() != null)
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
                timeToEatDetail.HoraInicio = tomesToEat.HoraInicio;
                timeToEatDetail.UrlImage = tomesToEat.UrlImage;
                timeToEatDetail.HoraFin = tomesToEat.HoraFin;
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



        //isValidCreateSpecialEvent
        //isValidActivateSpecialEvent
    }
}
