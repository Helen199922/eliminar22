using AutoMapper;
using CarniceriaFinal.Core.CustomException;
using CarniceriaFinal.ModelsEF;
using CarniceriaFinal.Productos.DTOs;
using CarniceriaFinal.Productos.IRepository;
using CarniceriaFinal.Productos.IServicios;
using CarniceriaFinal.Productos.Models;
using CarniceriaFinal.Productos.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Productos.Servicios
{
    public class CategoriaService : ICategoriaService
    {
        public readonly ICategoriaRepository ICategoriaRepo;
        private readonly IMapper IMapper;
        public CategoriaService(ICategoriaRepository ICategoriaRepo, IMapper IMapper)
        {
            this.ICategoriaRepo = ICategoriaRepo;
            this.IMapper = IMapper;
        }
        public async Task<List<CategoryEntity>> GetAllCategories()
        {
            try
            {
                var category = await ICategoriaRepo.GetAllCategories();

                var categories = IMapper.Map<List<CategoryEntity>>(category);

                return categories;
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al obtener las categorias.");
            }
        }
        public async Task<List<CategoryEntity>> GetAllAdmCategories()
        {
            try
            {
                var category = await ICategoriaRepo.GetAllAdmCategories();

                var categories = IMapper.Map<List<CategoryEntity>>(category);

                return categories;
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al obtener las categorias.");
            }
        }
       
        public async Task<List<SimpleProductInSubCategory>> GetAllProductsByIdCategory(int idCategory)
        {
            try
            {
                var productResponse = await ICategoriaRepo.GetAllProductsByIdCategory(idCategory);


                return productResponse;
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al obtener los detalles de la categoria.");
            }
        }
        public async Task<List<SimpleProductInSubCategory>> GetAllProductsToCategory()
        {
            try
            {
                var productResponse = await ICategoriaRepo.GetAllProductsToCategory();


                return productResponse;
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al obtener los detalles de la categoria.");
            }
        }
        public async Task<CategoriaProductoEntity> GetCategoryById(int idCategory)
        {
            try
            {
                var category = await ICategoriaRepo.GetCategoriaById(idCategory);
                if(category == null)
                {
                    throw RSException.NoData("No se encontró la categoría");
                }

                CategoriaProductoEntity response =
                    IMapper.Map<CategoriaProductoEntity>(category);

                var productResponse =  await ICategoriaRepo.GetAllProductsExistInCategory(idCategory);
                response.Products = new();


                foreach (var item in productResponse)
                {
                        response.Products.Add(
                       IMapper.Map<SimpleProductInSubCategory>(item.IdProductoNavigation)
                   );
                }

                return response;
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al obtener los detalles de la categoria.");
            }
        }

        public async Task<List<CategoriaProductoEntity>> CreateCategoryProcess(List<CategoriaProductoEntity> categories)
        {
            try
            {
                List<string> problems = new();

                List<CategoriaProductoEntity> response = new();

                foreach (var category in categories)
                {
                    category.status = 1;
                    CategoriaProducto categoryCreate = null;
                    try
                    {
                        categoryCreate = await CreateCategory(category.titulo, category.descripcion, category.UrlImage);
                        category.idCategoria = categoryCreate.IdCategoria;
                        response.Add((CategoriaProductoEntity)category.Clone());
                    }
                    catch (Exception Exp)
                    {
                        problems.Add("No se creado agregado la categoría: " + categoryCreate.Titulo);
                        continue;
                    }
                }


                //foreach (var category in categories)
                //{
                //    await this.UpdateProductsInCategory(category.Products, category.idCategoria.Value);
                //}
                if (problems.Count > 0)
                {
                    throw RSException.WithData(
                        "No se han registrado todas las categorias y/o producto",
                        409,
                        problems,
                        response);
                }

                return response;

            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception err)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al guardar el producto");
            }
        }


        //private async Task<string> UpdateProductsInCategory(List<SimpleProductInSubCategory> products, int idCategory)
        //{
        //    try
        //    {

        //        List<CategoriaInProducto> productsDetected = await ICategoriaRepo.GetAllProductsInCategory(
        //            IMapper.Map<List<Producto>>(products), idCategory
        //        );
        //        List<CategoriaInProducto> allCategoriesInProducts = await ICategoriaRepo.GetAllProductsExistInCategory(
        //            idCategory
        //        );

        //        ///Agregat nuevos productos en categorias
        //        foreach (var product in products)
        //        {
        //            if (productsDetected.Where(x => x.IdProducto.Value == product.idProducto).FirstOrDefault() == null)
        //            {
        //                await ICategoriaRepo.CrearCategoriaInProduct(idCategory, product.idProducto);
        //            }
                    
        //        }

        //        ///Quitar antiguos productos en categorias
        //        var toDelete = allCategoriesInProducts
        //            .Where(x => !products.Select(z => z.idProducto).ToList().Contains(x.IdProducto.Value))
        //            .ToList();

        //        if (toDelete.Count == 0) return "response";

        //        await ICategoriaRepo.DeleteCategoriaInProduct(toDelete);



        //        return "response";
        //    }
        //    catch (RSException err)
        //    {
        //        throw new RSException(err.TypeError, err.Code, err.MessagesError);
        //    }
        //    catch (Exception err)
        //    {
        //        throw new RSException("error", 500).SetMessage("Ha ocurrido un error al crear la sub Categoria y Producto.");
        //    }

        //}

        public async Task<CategoriaProducto> CreateCategory(string titulo, string descripcion, string urlImage)
        {
            try
            {
                CategoriaProducto categoria = new();
                categoria.Descripcion = descripcion;
                categoria.Status = 1;
                categoria.UrlImage = urlImage;
                categoria.Titulo = titulo;

                return await ICategoriaRepo.CreateCategory(categoria);
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al crear la sub Categoria y Producto.");
            }

        }

        
        public async Task<CategoriaProductoEntity> UpdateCategory(CategoriaProductoEntity category)
        {
            try
            {
                ICategoriaRepo.UpdateCategory(IMapper.Map<CategoriaProducto>(category));
                //await this.UpdateProductsInCategory(category.Products, category.idCategoria.Value);

                return await GetCategoryById(category.idCategoria.Value);

            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al guardar el producto");
            }
        }
        
        public async Task<Boolean> ChangeStatusCategory(int idCategory, int idStatus)
        {
            try
            {
                return await this.ICategoriaRepo.ChangeStatusCategory(idCategory, idStatus);
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al obtener al actualizar el estado de la categoría.");
            }
        }

        public Task<CategoriaProductoEntity> GetCategoryByProductId(int idCategory)
        {
            throw new NotImplementedException();
        }
    }
}
