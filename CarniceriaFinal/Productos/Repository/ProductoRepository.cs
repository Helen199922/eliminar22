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
        public async Task<List<Producto>> GetSimpleProductsByIdSubCategories(int idSubCategory)
        {
            try
            {
                return await Context.Productos
                    .Include(x => x.IdPromocionNavigation)
                    .Include(x => x.SubInCategoria)
                    .ThenInclude(x => x.IdCategoriaNavigation)
                    .Where(x => x.SubInCategoria.Any(y => y.IdSubCategoria == idSubCategory))
                    .ToListAsync();

            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("Productos para subcategoria administración");
            }
        }
        public async Task<List<Producto>> GetAllProductsSubCategory()
        {
            try
            {
                return await Context.Productos
                    .Include(x => x.SubInCategoria)
                    .ToListAsync();

            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Productos con subcategorias");
            }
        }
        public async Task<List<Producto>> GetSimpleProducts()
        {
            try
            {
                return await Context.Productos
                    .Include(x => x.IdPromocionNavigation)
                    .Include(x => x.SubInCategoria)
                    .ThenInclude(x => x.IdCategoriaNavigation)
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
                    .Include(x => x.IdPromocionNavigation)
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
                    .Include(x => x.IdPromocionNavigation)
                    .Include(x => x.IdUnidadNavigation)
                    .Include(x => x.DetalleProductos)
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
        public async Task<List<Producto>> FindProductsBySubCategory(int idCategory, int idSubCategory)
        {
            try
            {
                
                List<Producto> products = await Context.Productos
                    .Where(y => y.SubInCategoria.Any(x => (x.IdSubCategoria == idSubCategory && x.IdCategoria == idCategory)) && y.Status == 1)
                    .ToListAsync();
                
                return products;
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("obtener productos por categorya");
            }
        }

        public async Task<List<SubCategorium>> FindsubCategoryByCategoryId(int idSubCategory)
        {
            try
            {

                List<SubInCategorium> subCategories = await Context.SubInCategoria
                    .Include(x => x.IdSubCategoriaNavigation)
                    .Where(x => x.IdSubCategoria == idSubCategory)
                    .ToListAsync();

                List<SubCategorium> categories = new();
                foreach (var item in subCategories)
                {
                    categories.Add(item.IdSubCategoriaNavigation);
                }
                return categories;
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("obtener subcategorias por categoria id");
            }
        }

        public async Task<List<Producto>> FindProductByCategoryId(int idCategory)
        {
            try
            {
                List<Producto> products = await Context.Productos
                    .Where(y => y.SubInCategoria.Any(x => x.IdCategoria == idCategory) && y.Status == 1)
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
                productDetail.IdPromocion = product.IdPromocion;
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
        
    }
}
