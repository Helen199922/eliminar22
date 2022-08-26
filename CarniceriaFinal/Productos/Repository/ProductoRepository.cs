using CarniceriaFinal.Core.CustomException;
using CarniceriaFinal.ModelsEF;
using CarniceriaFinal.Productos.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Productos
{
    public class ProductoRepository : IProductoRepository
    {
        public readonly DBContext Context;


        public ProductoRepository(DBContext _Context)
        {
            this.Context = _Context;
        }
        public async Task<List<Producto>> GetSimpleProducts()
        {
            try
            {
                return await Context.Productos
                    .ToListAsync();

            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("Producto");
            }
        }
        public async Task<Producto> GetDetailAdminProductoById(int idProduct)
        {
            try
            {
                return await Context.Productos
                    .Include(x => x.IdUnidadNavigation)
                    .Include(x => x.DetalleProductos)
                    .Where(x => x.IdProducto == idProduct)
                    .FirstOrDefaultAsync();

            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Producto");
            }
        }
        public async Task<List<Producto>> GetProductos()
        {
            try
            {
                return await Context.Productos
                    .ToListAsync();

            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Producto");
            }
        }
        public async Task<List<Producto>> GetProductosInCar(List<int> products)
        {
            try
            {
                return await Context.Productos
                    .Where(x => products.Contains(x.IdProducto))
                    .ToListAsync();

            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Producto");
            }
        }
        public async Task<Producto> SaveProduct(Producto producto)
        {
            try
            {
                await Context.Productos.AddAsync(producto);
                await Context.SaveChangesAsync();
                return producto;

            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB(" Guardar Producto");
            }
        }
        public async Task<Producto> ProductById(int id)
        {
            try
            {
                return await Context.Productos.Where(x => x.IdProducto == id).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB(" find Producto by id");
            }
        }
        public async Task<string> SaveDetalles(List<DetalleProducto> detalleProducto)
        {
            try
            {
                foreach (DetalleProducto detalle in detalleProducto)
                {
                    await Context.DetalleProductos.AddAsync(detalle);
                }
                await Context.SaveChangesAsync();
                return "Producto guardado correctamente";
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB(" find Producto by id");
            }
        }
        public async Task<DetalleProducto> SaveOneProductDetail(DetalleProducto detalleProducto)
        {
            try
            {
                await Context.DetalleProductos.AddAsync(detalleProducto);
                await Context.SaveChangesAsync();
                return detalleProducto;
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("gurdar el detalle del producto");
            }
        }


        public async Task<Promocion> promotionConvert(int idProduct)
        {
            try
            {
                using (var _Context = new DBContext())
                {
                    var option = await _Context.PromocionInProductos.Where(x => (
                        DateTime.Compare(x.IdPromocionNavigation.FechaFin, DateTime.Now) >= 0
                        && DateTime.Compare(x.IdPromocionNavigation.FechaInicio, DateTime.Now) <= 0
                        && x.IdProducto == idProduct
                        && x.IdPromocionNavigation.Status == 1
                    ))
                    .Include(x => x.IdPromocionNavigation)
                    .FirstOrDefaultAsync();


                    if (option == null) return null;

                    return option?.IdPromocionNavigation ?? null;
                }
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("obtener promocion de categoria");
            }
        }
        public async Task<List<Producto>> getAllProductsPromotions()
        {
            try
            {
                using (var _Context = new DBContext())
                {
                    List<Producto> productsPromo = new();
                    var products = await _Context.PromocionInProductos.Where(x => (
                        DateTime.Compare(x.IdPromocionNavigation.FechaFin, DateTime.Now) >= 0
                        && DateTime.Compare(x.IdPromocionNavigation.FechaInicio, DateTime.Now) <= 0
                        && x.IdPromocionNavigation.Status == 1
                        && x.IdProductoNavigation.Stock > 0
                    ))
                    .Include(x => x.IdProductoNavigation)
                    .AsNoTracking()
                    .ToListAsync();

                    foreach (var product in products)
                    {
                        productsPromo.Add(product.IdProductoNavigation);
                    }
                    
                    return productsPromo;
                }
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("obtener productos con promocion");
            }
        }

        public async Task<Promocion> getPromotionActivate()
        {
            try
            {
                using (var _Context = new DBContext())
                {
                    var promotion = await _Context.PromocionInProductos.Where(x => (
                        DateTime.Compare(x.IdPromocionNavigation.FechaFin, DateTime.Now) >= 0
                        && DateTime.Compare(x.IdPromocionNavigation.FechaInicio, DateTime.Now) <= 0
                        && x.IdPromocionNavigation.Status == 1
                    ))
                    .Include(x => x.IdPromocionNavigation)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

                    if (promotion == null) return null;
                    return promotion.IdPromocionNavigation;
                }
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("obtener promocio activa");
            }
        }
        public async Task<PromocionInProducto> getPromotionByIdAndProduct(int idPromotion, int idProduct)
        {
            try
            {
                using (var _Context = new DBContext())
                {
                    var promotion = await _Context.PromocionInProductos.Where(x => (
                        x.IdPromocionNavigation.Status == 1
                        && x.IdPromocion == idPromotion
                        && x.IdProducto == idProduct
                        && DateTime.Compare(x.IdPromocionNavigation.FechaFin, DateTime.Now) >= 0
                        && DateTime.Compare(x.IdPromocionNavigation.FechaInicio, DateTime.Now) <= 0
                    ))
                    .Include(x => x.IdProductoNavigation)
                    .Include(x => x.IdPromocionNavigation)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

                    if (promotion == null) return null;
                    return promotion;
                }
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("obtener promocio activa");
            }
        }



        public async Task<List<Producto>> FindProductByCategoryId(int idCategory)
        {
            try
            {
                List<Producto> products = await Context.Productos
                    .Where(y => y.CategoriaInProductos.Any(x => x.IdCategoria == idCategory) && y.Status == 1 && y.Stock > 0)
                    .ToListAsync();

                return products;
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("obtener producto por categoria id");
            }
        }

        public async Task<Producto> UpdateProduct(Producto product)
        {
            try
            {
                
                var productDetail = await Context.Productos
                    .Where(c => c.IdProducto == product.IdProducto)
                    .FirstOrDefaultAsync();

                productDetail.ImgUrl = product.ImgUrl;
                productDetail.Descripcion = product.Descripcion;
                productDetail.Precio = product.Precio;
                productDetail.Titulo = product.Titulo;
                productDetail.Status = product.Status;
                productDetail.IdUnidad = product.IdUnidad;
                productDetail.Stock = product.Stock;
                productDetail.MinimaUnidad = product.MinimaUnidad;

                await Context.SaveChangesAsync();
                return productDetail;
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Actualizar el producto");
            }
        }

        public Producto DeleteProduct(int idProduct)
        {
            try
            {
                Producto product = new() { IdProducto = idProduct,  Status = 0 };
                Context.Productos.Attach(product);
                Context.Entry(product).Property(x => x.Status).IsModified = true;
                Context.SaveChanges();

                return product;
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("Eliminar le producto");
            }
        }

        public async Task<DetalleProducto> UpdateProductDetail(DetalleProducto detail)
        {
            try
            {
                var productDetail = await Context.DetalleProductos
                    .Where(c => c.IdDetalleProducto == detail.IdDetalleProducto)
                    .FirstOrDefaultAsync();

                productDetail.Descripcion = detail.Descripcion;
                productDetail.IdProducto = detail.IdProducto;
                productDetail.TituloDetalle = detail.TituloDetalle;
                productDetail.UrlImg = detail.UrlImg;


                await Context.SaveChangesAsync();
                return productDetail;
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("Editar detalle le producto");
            }
        }
        public async Task<string> DeleteProductDetail(int idProductDetail)
        {
            try
            {
                DetalleProducto detail = new() {IdDetalleProducto = idProductDetail};

                Context.DetalleProductos.Remove(detail);
                await Context.SaveChangesAsync();
                return "Eliminado correctamente";
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("Eliminar detalle le producto");
            }
        }
        public async Task<DetalleProducto> FindProductDetailById(int idProductDetail)
        {
            try
            {
                return await Context.DetalleProductos
                    .Where(x => x.IdDetalleProducto == idProductDetail)
                    .FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("obtener producto por categoria id");
            }
        }
        public async Task<Producto> FindProductById(int idProduct)
        {
            try
            {
                return await Context.Productos
                    .Where(x => x.IdProducto == idProduct)
                    .FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("obtener producto por id");
            }
        }
        public async Task<List<DetalleProducto>> FindDetailsByIdProduct(int idProduct)
        {
            try
            {
                return await Context.DetalleProductos
                    .AsNoTracking()
                    .Where(x => x.IdProducto == idProduct)
                    .ToListAsync();
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("obtener el detalle del producto");
            }
        }
        public async Task<string> EnableProduct(int idProduct)
        {
            try
            {
                Producto product = await Context.Productos
                    .Where(x => x.IdProducto == idProduct)
                    .FirstOrDefaultAsync();
                product.Status = 1;
                await Context.SaveChangesAsync();
                return "Producto activado correctamente";
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("activar el producto");
            }
        }
        public async Task<string> UpdateStock(int idProduct, int stock)
        {
            try
            {
                Producto product = await Context.Productos
                    .Where(x => x.IdProducto == idProduct)
                    .FirstOrDefaultAsync();
                product.Stock = stock;
                await Context.SaveChangesAsync();
                return "Stock de producto actualizado correctamente";
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("el stock del producto");
            }
        }
        public async Task<string> DisminuirStock(int idProduct, int cant)
        {
            try
            {
                Producto product = await Context.Productos
                    .Where(x => x.IdProducto == idProduct)
                    .FirstOrDefaultAsync();

                if(product.Stock < cant)
                    product.Stock = 0;
                else
                    product.Stock = product.Stock - cant;

                await Context.SaveChangesAsync();
                return "Stock de producto actualizado correctamente";
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("el stock del producto");
            }
        }
        public async Task<string> AumentarStock(int idProduct, int cant)
        {
            try
            {
                Producto product = await Context.Productos
                    .Where(x => x.IdProducto == idProduct)
                    .FirstOrDefaultAsync();

                    product.Stock = product.Stock + cant;

                await Context.SaveChangesAsync();
                return "Stock de producto actualizado correctamente";
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("el stock del producto");
            }
        }
    }
}
