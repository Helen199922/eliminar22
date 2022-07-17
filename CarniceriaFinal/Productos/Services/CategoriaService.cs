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
        public readonly ISubCategoriaRepository ISubCategoriaRepository;
        public readonly ISubInCategoriaRepository ISubInCategoriaRepository;
        private readonly IMapper IMapper;
        public CategoriaService(ICategoriaRepository ICategoriaRepo, ISubCategoriaRepository ISubCategoriaRepository, ISubInCategoriaRepository ISubInCategoriaRepository, IMapper IMapper)
        {
            this.ICategoriaRepo = ICategoriaRepo;
            this.ISubCategoriaRepository = ISubCategoriaRepository;
            this.ISubInCategoriaRepository = ISubInCategoriaRepository;
            this.IMapper = IMapper;
        }
        public async Task<List<CategoryAdminEntity>> GetAllCategories()
        {
            try
            {
                var category = await ICategoriaRepo.GetAllCategories();

                var categories = IMapper.Map<List<CategoryAdminEntity>>(category);

                foreach (var item in categories)
                    try
                    {
                        var categoriesResult = await ICategoriaRepo
                            .GetSubCategoryByCategoryId(item.idCategoria.Value);

                        item.numSubCategories = categoriesResult.Count;
                    }
                    catch (Exception)
                    {
                        item.numSubCategories = 0;
                    }
                {
                    
                }

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
        public async Task<List<SubCategoriaAdminEntity>> GetAllSubCategories()
        {
            try
            {
                var subCategory = await ISubCategoriaRepository.GetAllSubCategories();

                var subCategories = IMapper.Map<List<SubCategoriaAdminEntity>>(subCategory);

                foreach (var item in subCategories)
                {
                    try
                    {
                        var categoriesResult = await ICategoriaRepo
                            .GetAllCategoriesByIdSubCategory(item.idSubcategoria.Value);

                        item.numCategories = categoriesResult.Count;
                    }
                    catch (Exception)
                    {
                        item.numCategories = 0;
                    }
                }

                return subCategories;
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al obtener todas las sub categorias.");
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

                CategoriaProductoEntity categoryAndSubCategory =
                    IMapper.Map<CategoriaProductoEntity>(category);

                categoryAndSubCategory.subCategoria = 
                IMapper.Map<List<SubCategoriaProductoEntity>>(
                            await ICategoriaRepo.GetSubCategoryByCategoryId(idCategory)
                        );


                return categoryAndSubCategory;
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

                    if (category.subCategoria == null) continue;

                    SubCategorium subCategoryCreate = null;
                    response.Last().subCategoria = new();
                    foreach (var subCategory in category.subCategoria)
                    {
                        try
                        {
                            subCategory.status = 1;
                            if (subCategory.idSubcategoria == null)
                            {
                                subCategoryCreate = await CreateSubCategory(subCategory.titulo, subCategory.descripcion, subCategory.UrlImage);
                                subCategory.idSubcategoria = subCategoryCreate.IdSubCategoria;
                            }
                            var categoriesEntity = (SubCategoriaProductoEntity)subCategory.Clone();
                            response.Last().subCategoria.Add(categoriesEntity);
                        }
                        catch (Exception Exp)
                        {
                            problems.Add("No se creado agregado la sub categoría: " + subCategory.titulo);
                            continue;
                        }
                    }

                    if (response.Last().subCategoria.Count > 0)
                    {
                        foreach (var subCategoryCreateCorrectly in response.Last().subCategoria)
                        {
                            try
                            {
                                await CreteSubInCategoria(category.idCategoria.Value, subCategoryCreateCorrectly.idSubcategoria.Value);
                            }
                            catch (Exception)
                            {
                                problems.Add($"No se realacionado correctamente agregado la sub categoría: {subCategoryCreateCorrectly.titulo} con la categoría: {category.titulo}");
                            }
                        }
                    }
                }

                if (problems.Count > 0)
                {
                    throw RSException.WithData(
                        "No se han registrado todas las categorias y/o sub categorias",
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

        public async Task<List<CreateSubCategory>> CreateSubCategoriesProcess(List<CreateSubCategory> subCategories)
        {
            try
            {
                List<string> problems = new();

                List <CreateSubCategory> response = new();

                foreach (var subCategory in subCategories)
                {
                    subCategory.status = 1;
                    SubCategorium subCategoryCreate = null;
                    try
                    {
                        subCategoryCreate = await CreateSubCategory(subCategory.titulo, subCategory.descripcion, subCategory.UrlImage);
                        subCategory.idSubcategoria = subCategoryCreate.IdSubCategoria;
                        response.Add((CreateSubCategory)subCategory.Clone());
                    }
                    catch (Exception Exp)
                    {
                        problems.Add("No se creado agregado la sub categoría: " + subCategory.titulo);
                        continue;
                    }
                    if (subCategory.Categories == null) continue;
                    //response.Last().Categories = new();
                    //foreach (var category in subCategory.Categories)
                    //{
                    //    CategoriaProducto categoryCreate = null;
                    //    try
                    //    {
                    //        category.status = 1;
                    //        if (category.idCategoria == null)
                    //        {
                    //            categoryCreate = await CreateCategory(category.titulo, category.descripcion, category.UrlImage);
                    //            category.idCategoria = categoryCreate.IdCategoria;
                    //        }
                    //        response.Last().Categories.Add(category);
                    //    }
                    //    catch (Exception Exp)
                    //    {
                    //        problems.Add("No se creado agregado la categoría: " + categoryCreate.Titulo);
                    //        continue;
                    //    }
                    //}

                    //if(response.Last().Categories.Count > 0)
                    //{
                    //    foreach (var categoryCreateCorrectly in response.Last().Categories)
                    //    {
                    //        try
                    //        {
                    //            await CreteSubInCategoria(categoryCreateCorrectly.idCategoria.Value, subCategory.idSubcategoria.Value);
                    //        }
                    //        catch (Exception)
                    //        {
                    //            problems.Add($"No se realacionado correctamente agregado la sub categoría: {subCategory.titulo} con la categoría: {categoryCreateCorrectly.titulo}");
                    //        }
                    //    }
                    //}

                    foreach (var category in subCategory.Categories)
                    {
                        await this.UpdateProductsInSubCategory(subCategory.Products, subCategory.idSubcategoria.Value, category.idCategoria.Value);
                    }
                }
                if (problems.Count > 0)
                {
                    throw RSException.WithData(
                        "No se han registrado todas las categorias y/o sub categorias", 
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
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al crear la sub Categoria y Producto.");
            }

        }

        private async Task<string> UpdateProductsInSubCategory(List<SimpleProductInSubCategory> products, int idSubCategory, int idCategory)
        {
            try
            {
                List<SubInCategorium> ToRemove = new();
                List<SubInCategorium> ToAdd = new();
                List<SubInCategorium> productsDetected = await ISubInCategoriaRepository.GetAllProductsInSubCategory(
                    IMapper.Map<List<Producto>>(products), idSubCategory, idCategory
                );
                List<SubInCategorium> allCategoriesAndSubCategory = await ISubInCategoriaRepository.GetAllProductsExistInSubCategory(
                    idSubCategory, idCategory
                );

                foreach (var product in products)
                {
                    if (productsDetected.Where(x => x.IdProducto.Value == product.idProducto).FirstOrDefault() == null)
                    {
                        await ISubInCategoriaRepository.CrearSubInCategoria(new SubInCategorium()
                        {
                            IdProducto = product.idProducto,
                            IdCategoria = idCategory,
                            IdSubCategoria = idSubCategory

                        });
                    }
                    
                }

                foreach (var allProductRelation in allCategoriesAndSubCategory)
                {
                    if(allProductRelation.IdProducto == null)
                    {
                        await ISubInCategoriaRepository.DeleteProductAndElementInSubCategory(allProductRelation.IdProducto, idSubCategory, idCategory);
                        //ToRemove.Add(new SubInCategorium()
                        //{
                        //    IdProducto = allProductRelation.IdProducto,
                        //    IdCategoria = idCategory,
                        //    IdSubCategoria = idSubCategory
                        //});
                        continue;
                    }

                    if (products.Where(x => x.idProducto == allProductRelation.IdProducto.Value).FirstOrDefault() == null)
                    {
                        await ISubInCategoriaRepository.DeleteProductAndElementInSubCategory(allProductRelation.IdProducto, idSubCategory, idCategory);

                        //ToRemove.Add(new SubInCategorium()
                        //{
                        //    IdProducto = allProductRelation.IdProducto,
                        //    IdCategoria = idCategory,
                        //    IdSubCategoria = idSubCategory
                        //});
                    }


                }
                //if (allCategoriesAndSubCategory.Where(x => x.IdProducto.Value == product.idProducto).FirstOrDefault() == null)
                //{
                //    var valor = product;
                //    ToRemove.Add(new SubInCategorium()
                //    {
                //        IdProducto = product.IdProducto.Value,
                //        IdCategoria = idCategory,
                //        IdSubCategoria = idSubCategory
                //    });

                //}
                //await ISubInCategoriaRepository.CrearSubInCategoria(new SubInCategorium()
                //{
                //    IdProducto = product.idProducto,
                //    IdCategoria = idCategory,
                //    IdSubCategoria = idSubCategory
                //});


                //To remove



                //To Add

                return "response";
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception err)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al crear la sub Categoria y Producto.");
            }

        }

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
        public async Task<SubCategorium> CreateSubCategory(string titulo, string descripcion, string UrlImage)
        {
            try
            {
                SubCategorium subCategoria = new();
                subCategoria.Descripcion = descripcion;
                subCategoria.Status = 1;
                subCategoria.Titulo = titulo;
                subCategoria.UrlImage = UrlImage;
                return await ISubCategoriaRepository.CreateSubCategory(subCategoria);

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

        public async Task<SubInCategorium> CreteSubInCategoria(int idCategoria, int idSubCategoria)
        {
            try
            {
                //Creando el nuevo elemento
                SubInCategorium subInCategoria = new();
                subInCategoria.IdCategoria = idCategoria;
                subInCategoria.IdSubCategoria = idSubCategoria;

                return await ISubInCategoriaRepository.CrearSubInCategoria(subInCategoria);
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
        public async Task<List<CategoriaProductoEntity>> GetAllCategoriaAndSubCategory()
        {
            try
            {
                var category = await ICategoriaRepo.GetAllCategoriaAndSubCategory();
                List<CategoriaProductoEntity> categoryAndSubCategory =
                    IMapper.Map<List<CategoriaProductoEntity>>(category);

                foreach (var itemCategory in categoryAndSubCategory)
                {
                    itemCategory.subCategoria =
                        IMapper.Map<List<SubCategoriaProductoEntity>>(
                            await ICategoriaRepo.GetSubCategoryByCategoryId(itemCategory.idCategoria.Value)
                        );
                }

                return categoryAndSubCategory;
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al obtener la categoria y sub categoria de productos.");
            }
        }
        public async Task<CategoriaProductoEntity> GetCategoryAndSubCategoriesByIdCategory(int idCategory)
        {
            try
            {
                CategoriaProductoEntity category =
                    IMapper.Map<CategoriaProductoEntity>(
                        await ICategoriaRepo.GetCategoriaById(idCategory)
                    );
                category.subCategoria = new();
                category.subCategoria = IMapper.Map<List<SubCategoriaProductoEntity>>(
                        await ISubCategoriaRepository.GetSubCategoriesByIdCategory(idCategory)
                    );

                return category;
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al obtener la categoria y sub categoria de productos.");
            }
        }
        public async Task<CreateSubCategory> GetSubCategoryAndCategoryByIdSubCategory(int idSubCategory)
        {
            try
            {
                CreateSubCategory subCategory =
                    IMapper.Map<CreateSubCategory>(
                        await ISubCategoriaRepository.GetSubCategoriaById(idSubCategory)
                    );

                if(subCategory == null)
                {
                    throw RSException.NoData("No se encontró la subCategoría");
                }

                subCategory.Categories = new();
                subCategory.Categories = 
                    IMapper.Map<List<CategoryEntity>>(
                        await ICategoriaRepo.GetAllCategoriesByIdSubCategory(idSubCategory)
                    );

                return subCategory;
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception err)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al obtener los detalles de la categoria.");
            }
        }
        public async Task<CategoriaProductoEntity> UpdateCategoryAndSubCategory(CategoriaProductoEntity category)
        {
            try
            {
                category.status = 1;
                if (category.subCategoria == null) category.subCategoria = new();

                if (category.subCategoria.Count > 0)
                {
                    foreach (var subCat in category.subCategoria)
                    {
                        if (subCat.idSubcategoria == null)
                        {
                            subCat.status = 1;
                            subCat.idSubcategoria = CreateSubCategory(subCat.titulo, subCat.descripcion, subCat.UrlImage).Result.IdSubCategoria;
                            await CreteSubInCategoria(category.idCategoria.Value, subCat.idSubcategoria.Value);
                        }
                    }
                }
                List<int> arraySubCat = new();
                foreach (var item in category.subCategoria)
                {
                    arraySubCat.Add(item.idSubcategoria.Value);
                }

                await ICategoriaRepo.UpdateCategoryInSubcategory(category.idCategoria.Value, arraySubCat);

                ICategoriaRepo.UpdateCategory( IMapper.Map<CategoriaProducto>(category) );

                return await GetCategoryAndSubCategoriesByIdCategory(category.idCategoria.Value);

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
        public async Task<CreateSubCategory> UpdateSubCategoryAndCategory(CreateSubCategory subCategory)
        {
            try
            {
                subCategory.status = 1;
                if (subCategory.Categories == null) subCategory.Categories = new();

                //Creando las subcategorias
                //if (subCategory.Categories.Count > 0)
                //{
                //    foreach (var category in subCategory.Categories)
                //    {
                //        category.status = 1;
                //        if (category.idCategoria == null)
                //        {
                //            category.idCategoria = CreateCategory(category.titulo, category.descripcion, category.UrlImage).Result.IdCategoria;
                //            await CreteSubInCategoria(category.idCategoria.Value, subCategory.idSubcategoria.Value);
                //        }
                //    }
                //}
                List<int> arraySubCat = new();
                foreach (var item in subCategory.Categories)
                {
                    arraySubCat.Add(item.idCategoria.Value);
                }

                await ISubCategoriaRepository.UpdateCategoryInSubcategory(subCategory.idSubcategoria.Value, arraySubCat);

                ISubCategoriaRepository.UpdateSubCategory(IMapper.Map<SubCategorium>(subCategory));

                foreach (var category in subCategory.Categories)
                {
                    await this.UpdateProductsInSubCategory(subCategory.Products, subCategory.idSubcategoria.Value, category.idCategoria.Value);
                }
                return await GetSubCategoryAndCategoryByIdSubCategory(subCategory.idSubcategoria.Value);

            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al actualizar sub categoría");
            }
        }
        public string DeleteSubInCategorByProductIdAndSubCategoryIdAndCategoryId
            (int idProduct, int idSubCategory, int idCategory)
        {
            try
            {
                ISubInCategoriaRepository.DeleteSubInCategorByProductIdAndSubCategoryIdAndCategoryId
                    (idProduct, idSubCategory, idCategory);

                return "Eliminado correctamente";
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al borrar las subCategorias y/o categorias del producto.");
            }

        }
        public async Task<Boolean> DeleteCategoryAndSubCatAndRelationship(int idCategory)
        {
            try
            {
                List<SubInCategorium> subCatInCategoy = await ISubInCategoriaRepository.GetAllSubInCategoriesByCategoryId(idCategory);
                await ISubInCategoriaRepository.DeleteSubInCategoria(subCatInCategoy);

                await ICategoriaRepo.DelateCategoryById(idCategory);
                return true;
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al borrar las categorias y/o subCategorias.");
            }

        }
        public async Task<Boolean> DeleteSubCatAndCategoryRelationship(int idSubCategory)
        {
            try
            {
                List<SubInCategorium> subCatInCategoy = await ISubInCategoriaRepository.GetAllSubInCategoriesBySubCategoryId(idSubCategory);
                await ISubInCategoriaRepository.DeleteSubInCategoria(subCatInCategoy);

                await ISubCategoriaRepository.DeleteSubCategoryById(idSubCategory);
                return true;
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al borrar las subCategorias y/o categorias.");
            }

        }

        public async Task<List<CategoriaProductoEntity>> GetAllCategoriesAndSubCategoriesByProductId(int idProduct)
        {
            try
            {
                var categoriesRepo = await ICategoriaRepo.GetAllCategoriesAndSubCategoriesByProductId(idProduct);
                List<CategoriaProductoEntity> categories = new();

                foreach (var cetegoryRepo in categoriesRepo)
                {

                    if(categories.Count == 0 || !categories.Select(x => x.idCategoria).Contains(cetegoryRepo.IdCategoria))
                    {

                        categories.Add(IMapper.Map<CategoriaProductoEntity>(cetegoryRepo.IdCategoriaNavigation));
                        categories.Last().subCategoria = new();
                    }

                    var category = categories.Where(x => x.idCategoria == cetegoryRepo.IdCategoria).FirstOrDefault();
                    category.subCategoria.Add(IMapper.Map<SubCategoriaProductoEntity>(cetegoryRepo.IdSubCategoriaNavigation));
                    
                }

                return categories;
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al obtener la categoria y sub categoria del producto especificado.");
            }
        }
    }
}
