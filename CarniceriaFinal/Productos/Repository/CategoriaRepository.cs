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
        public readonly ISubInCategoriaRepository ISubInCategoriaRepository;
        public CategoriaRepository(DBContext _Context, ISubInCategoriaRepository ISubInCategoriaRepository)
        {
            Context = _Context;
            this.ISubInCategoriaRepository = ISubInCategoriaRepository;
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

        public async Task<CategoriaProducto> GetCategoryById(int idCategory)
        {
            try
            {
                return await Context.CategoriaProductos
                .Where(x => x.IdCategoria == idCategory)
                .FirstOrDefaultAsync();
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("Obtener las categorias específica");
            }
        }

        public async Task<List<CategoriaProducto>> GetAllCategoriaAndSubCategory()
        {
            try
            {
                return await Context.CategoriaProductos
                .Where(x => x.SubInCategoria.Any(x => x.IdProducto != null && x.IdProductoNavigation.Status == 1))
                .ToListAsync();
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("Obtener las categorias con productos");
            }
        }
        public async Task<List<CategoriaProducto>> GetAllCategories()
        {
            try
            {
                return await Context.CategoriaProductos
                    .Include(x => x.SubInCategoria)
                .ToListAsync();
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("Obtener las categorias");
            }
        }
        public async Task<List<SubInCategorium>> GetAllCategoriesAndSubCategoriesByProductId(int idProduct)
        {
            try
            {
                return await Context.SubInCategoria
                    .Include(x => x.IdCategoriaNavigation)
                    .Include(x => x.IdSubCategoriaNavigation)
                .Where(x => x.IdProducto == idProduct)
                .ToListAsync();
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("Obtener las categorias y subcategorías del producto");
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
        public async Task<List<SubCategorium>> GetSubCategoryByCategoryId(int idCategory)
        {
            try
            {
                return await Context.SubCategoria
                    .Where(x => x.SubInCategoria.Any(x => x.IdCategoria == idCategory && x.IdProductoNavigation.Status == 1))
                    .Distinct()
                    .ToListAsync();
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("Obtener las subcategorias de la categoria");
            }
        }
        public async Task<List<CategoriaProducto>> GetAllCategoriesByIdSubCategory(int idSubCategory)
        {
            try
            {
                return await Context.CategoriaProductos
                    .Where(x => x.SubInCategoria.Any(x => x.IdSubCategoria == idSubCategory))
                    .Distinct()
                    .ToListAsync();
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("Obtener las subcategorias de la categoria");
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
        public async Task<Boolean> UpdateCategoryInSubcategory(int idCategory, List<int> idSubCategories)
        {
            try
            {
                var CategoryInSub = await Context.SubInCategoria
                    .AsNoTracking()
                    .Where(x =>
                    (x.IdCategoria == idCategory
                        && !idSubCategories.Contains(x.IdSubCategoriaNavigation.IdSubCategoria)
                    )).ToListAsync();

                foreach (var item in CategoryInSub)
                {
                    Context.SubInCategoria.Remove(item);
                    Context.SaveChanges();
                }

                CategoryInSub = await Context.SubInCategoria
                    .AsNoTracking()
                    .Where(x => (x.IdCategoria == idCategory)).ToListAsync();

                var CategoryInSubNoContain = idSubCategories.Where(x =>
                            !CategoryInSub.Select(y => y.IdSubCategoria).Contains(x));
                    

                foreach (var item in CategoryInSubNoContain)
                {
                    await ISubInCategoriaRepository.CrearSubInCategoria(idCategory, item);
                }

                return true;

            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("Actualizar la relacion entre categoria y subcategoria");
            }
        }
        public async Task<SubCategorium> UpdateSubCategory(SubCategorium subCategory)
        {
            try
            {
                Context.SubCategoria.Update(subCategory);

                Context.SaveChanges();

                return subCategory;

            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("Obtener las subcategorias de la categoria");
            }
        }
    }
}
