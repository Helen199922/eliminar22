using CarniceriaFinal.Core.CustomException;
using CarniceriaFinal.ModelsEF;
using CarniceriaFinal.Productos.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Productos.Repository
{
    public class SubCategoriaRepository : ISubCategoriaRepository
    {
        public readonly DBContext Context;
        public readonly ISubInCategoriaRepository ISubInCategoriaRepository;
        public SubCategoriaRepository(DBContext _Context, ISubInCategoriaRepository ISubInCategoriaRepository)
        {
            Context = _Context;
            this.ISubInCategoriaRepository = ISubInCategoriaRepository;
        }
        public async Task<SubCategorium> CreateSubCategory(SubCategorium subCat)
        {
            try
            {
                await Context.SubCategoria.AddAsync(subCat);
                await Context.SaveChangesAsync();

                return subCat;
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Crear Sub Categoria");
            }
        }

        public async Task<List<SubCategorium>> GetAllSubCategories()
        {
            try
            {
                return await Context.SubCategoria
                    .Include(x => x.SubInCategoria)
                    .ToListAsync();
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Obtener todas las Sub Categoria");
            }
        }

        public SubCategorium UpdateSubCategory(SubCategorium subCategory)
        {
            try
            {
                Context.SubCategoria.Update(subCategory);

                Context.SaveChanges();

                return subCategory;

            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("Actualizar las subCategory");
            }
        }
        public async Task<Boolean> DeleteSubCategoryById(int idSubCategory)
        {
            try
            {
                var all = await Context.SubCategoria.Where(x => x.IdSubCategoria == idSubCategory).FirstOrDefaultAsync();

                Context.SubCategoria.Remove(all);
                Context.SaveChanges();

                return true;
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("Eliminar las sub categorias del producto");
            }
        }
        public async Task<SubCategorium> GetSubCategoriaById(int idSubCategory)
        {
            try
            {
                return await Context.SubCategoria.Where(x => x.IdSubCategoria == idSubCategory)
                    .FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Obtener SubInCategoria y Categoria");
            }

        }
        public async Task<List<SubCategorium>> GetSubCategoriesByIdCategory(int idCategory)
        {
            try
            {
                return await Context.SubCategoria.Where(x => x.SubInCategoria.Any(y => y.IdCategoria == idCategory))
                    .ToListAsync();
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Obtener SubInCategoria por id Category");
            }

        }
        public async Task<Boolean> UpdateCategoryInSubcategory(int idSubCategory, List<int> idCategories)
        {
            try
            {
                var CategoryInSub = await Context.SubInCategoria
                    .AsNoTracking()
                    .Where(x =>
                    (x.IdSubCategoria == idSubCategory
                        && !idCategories.Contains(x.IdCategoriaNavigation.IdCategoria)
                    )).ToListAsync();

                foreach (var item in CategoryInSub)
                {
                    Context.SubInCategoria.Remove(item);
                    await Context.SaveChangesAsync();
                }

                CategoryInSub = await Context.SubInCategoria
                    .AsNoTracking()
                    .Where(x => (x.IdSubCategoria == idSubCategory)).ToListAsync();

                var CategoryInSubNoContain = idCategories.Where(x =>
                            !CategoryInSub.Select(y => y.IdCategoria).Contains(x));


                foreach (var item in CategoryInSubNoContain)
                {
                    await ISubInCategoriaRepository.CrearSubInCategoria(item, idSubCategory);
                }

                return true;

            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("Actualizar la relacion entre categoria y subcategoria");
            }
        }
    }
}
