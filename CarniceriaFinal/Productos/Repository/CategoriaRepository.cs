using CarniceriaFinal.Core.CustomException;
using CarniceriaFinal.ModelsEF;
using CarniceriaFinal.Productos.IRepository;
using CarniceriaFinal.Productos.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Productos.Repository
{
    public class CategoriaRepository : ICategoriaRepository
    {
        public readonly DBContext Context;
        public CategoriaRepository(DBContext _Context)
        {
            Context = _Context;
        }

        public Task<CategoriaProducto> GetCategoriaById(int id)
        {
            try
            {
                return Context.CategoriaProductos.Where(x => x.IdCategoria == id).FirstOrDefaultAsync();
            }
            catch(Exception)
            {
                throw RSException.ErrorQueryDB("Categoria");
            }
        }

        public async Task<CategoriaProducto> CreateCategory(CategoriaProducto categoria)
        {
            try
            {
                await Context.CategoriaProductos.AddAsync(categoria);
                await Context.SaveChangesAsync();
                    
                return categoria;
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Crear Categoria");
            }
        }

        public async Task<List<CategoriaProducto>> GetAllCategories()
        {
            try
            {
                return await Context.CategoriaProductos
                .ToListAsync();
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("Obtener las categorias");
            }
        }
        public async Task<Boolean> ChangeStatusCategory(int idCategory, int idStatus)
        {
            try
            {
                var response = await Context.CategoriaProductos
                .Where(x => x.IdCategoria == idCategory)
                .FirstOrDefaultAsync();

                if (response == null) return false;
                response.Status = idStatus;

                return true;

            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("Actualizar estado de categoria");
            }
        }
        public async Task<Boolean> DelateCategoryById(int idCategory)
        {
            try
            {
                var all = await Context.CategoriaProductos.Where(x => x.IdCategoria == idCategory).FirstOrDefaultAsync();

                Context.CategoriaProductos.Remove(all);
                Context.SaveChanges();

                return true;
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("Eliminar las categorias del producto");
            }
        }

        public CategoriaProducto UpdateCategory(CategoriaProducto category)
        {
            try
            {
                Context.CategoriaProductos.Update(category);
                
                Context.SaveChanges();

                return category;

            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("Actualizar las subcategorias de la categoria");
            }
        }

        public async Task<List<CategoriaInProducto>> GetAllProductsInCategory(List<Producto> products, int idCategory)
        {
            try
            {
                List<int> productsId = products.Select(x => x.IdProducto).ToList();

                var productsInSubCategory = await Context.CategoriaInProductos
                    .Where(x => productsId.Contains(x.IdProducto.Value) && x.IdCategoria == idCategory)
                    .AsNoTracking()
                    .ToListAsync();

                return productsInSubCategory;
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("Obtener los productos relacionados con categorias");
            }
        }
        public async Task<List<CategoriaInProducto>> GetAllProductsExistInCategory(int idCategory)
        {
            try
            {
                var productsInSubCategory = await Context.CategoriaInProductos
                    .Include(x => x.IdProductoNavigation)
                    .Where(x => x.IdCategoria == idCategory)
                    .AsNoTracking()
                    .ToListAsync();

                return productsInSubCategory;
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("Obtener las relaciones de productos con categorias");
            }
        }
        public async Task<List<CategoriaProducto>> GetAllCategoriesByProductId(int idProduct)
        {
            try
            {
                var productsInSubCategory = await Context.CategoriaInProductos
                    .Include(x => x.IdCategoriaNavigation)
                    .Include(x => x.IdProductoNavigation)
                    .Where(x => x.IdProducto == idProduct)
                    .AsNoTracking()
                    .ToListAsync();

                return productsInSubCategory.Select(x => x.IdCategoriaNavigation).DistinctBy(x => x.IdCategoria).ToList();
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("Obtener las categorias por idProducto");
            }
        }
        public async Task<CategoriaInProducto> CrearCategoriaInProduct(int idCategory, int idProduct)
        {
            try
            {
                using (var _Context = new DBContext())
                {
                    CategoriaInProducto productInCat = new()
                    {
                        IdCategoria = idCategory,
                        IdProducto = idProduct
                    };
                    await Context.CategoriaInProductos.AddAsync(productInCat);
                    await Context.SaveChangesAsync();

                    return productInCat;

                }
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("Crear Categoria en producto");
            }

        }
        public async Task<Boolean> DeleteCategoriaInProduct(List<CategoriaInProducto> range)
        {
            try
            {
                using (var _Context = new DBContext())
                {
                    _Context.CategoriaInProductos.RemoveRange(range);
                    await _Context.SaveChangesAsync();
                }

                return true;
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("Borrar Categoria en producto");
            }

        }
    }
}
