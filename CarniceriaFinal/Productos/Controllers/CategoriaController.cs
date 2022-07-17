using CarniceriaFinal.Core;
using CarniceriaFinal.Core.BlobStorage.DTOs;
using CarniceriaFinal.Core.BlobStorage.Services.IServices;
using CarniceriaFinal.Core.CustomException;
using CarniceriaFinal.Productos.DTOs;
using CarniceriaFinal.Productos.IServicios;
using CarniceriaFinal.Productos.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace CarniceriaFinal.Productos.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        public readonly ICategoriaService CategoriaService;
        public CategoriaController(ICategoriaService CategoriaService)
        {
            this.CategoriaService = CategoriaService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCategoriaAndSubCategory()
        {
            RSEntity<List<CategoriaProductoEntity>> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await CategoriaService.GetAllCategoriaAndSubCategory()));

            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }

        [HttpGet("get-category/{idCategory}")]
        public async Task<IActionResult> GetCategoryById(int idCategory)
        {
            RSEntity<CategoriaProductoEntity> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await CategoriaService.GetCategoryById(idCategory)));

            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }

        [HttpGet("get-subcategory/{idSubCategory}")]
        public async Task<IActionResult> GetSubCategoryById(int idSubCategory)
        {
            RSEntity<CreateSubCategory> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await CategoriaService.GetSubCategoryAndCategoryByIdSubCategory(idSubCategory)));

            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
        [HttpGet("sub-categories")]
        public async Task<IActionResult> GetAllSubCategories()
        {
            RSEntity<List<SubCategoriaAdminEntity>> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await CategoriaService.GetAllSubCategories()));

            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
        [HttpGet("only-categories")]
        public async Task<IActionResult> GetAllCategories()
        {
            RSEntity<List<CategoryAdminEntity>> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await CategoriaService.GetAllCategories()));

            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] List<CategoriaProductoEntity> categoria)
        {
            RSEntity<List<CategoriaProductoEntity>> rsEntity = new();
            try
            {
                var response = await CategoriaService.CreateCategoryProcess(categoria);
                return Ok(rsEntity.Send(response));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }

        [Authorize]
        [HttpPost("Subcategory")]
        public async Task<IActionResult> CreateSubCategories([FromBody] List<CreateSubCategory> subCategories)
        {
            RSEntity<List<CreateSubCategory>> rsEntity = new();
            try
            {
                var response = await CategoriaService.CreateSubCategoriesProcess(subCategories);
                return Ok(rsEntity.Send(response));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.FailWithData(err.MessagesError, (List<CreateSubCategory>)err.DataError));
            }
        }

        [Authorize]
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateCategoryAndSubCategory([FromBody] CategoriaProductoEntity categoria)
        {
            RSEntity<CategoriaProductoEntity> rsEntity = new();
            try
            {
                var res = await CategoriaService.UpdateCategoryAndSubCategory(categoria);
                return Ok(rsEntity.Send(res));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
        
        [Authorize]
        [HttpPut("Subcategory/Update")]
        public async Task<IActionResult> UpdateSubCategoryAndSubCategory([FromBody] CreateSubCategory categoria)
        {
            RSEntity<CreateSubCategory> rsEntity = new();
            try
            {
                var res = await CategoriaService.UpdateSubCategoryAndCategory(categoria);
                return Ok(rsEntity.Send(res));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }

        [Authorize]
        [HttpDelete("{idCategory}")]
        public async Task<IActionResult> DeleteCategoryAndSubCatAndRelationship(int idCategory)
        {
            RSEntity<string> rsEntity = new();
            try
            {
                await CategoriaService.DeleteCategoryAndSubCatAndRelationship(idCategory);
                return Ok(rsEntity.Send("Proceso Ejecutado Correctamente"));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }

        [Authorize]
        [HttpDelete("Subcategory/{idSubCategory}")]
        public async Task<IActionResult> DeleteSubCatAndCategoryRelationship(int idSubCategory)
        {
            RSEntity<string> rsEntity = new();
            try
            {
                await CategoriaService.DeleteSubCatAndCategoryRelationship(idSubCategory);
                return Ok(rsEntity.Send("Proceso Ejecutado Correctamente"));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }

        [Authorize]
        [HttpDelete("sub-in-category/{idProduct}/{idSubCategory}/{idCategory}")]
        public IActionResult DeleteRelationship(int idProduct, int idSubCategory, int idCategory)
        {
            RSEntity<string> rsEntity = new();
            try
            {
                CategoriaService.DeleteSubInCategorByProductIdAndSubCategoryIdAndCategoryId
                    (idProduct, idSubCategory, idCategory);
                return Ok(rsEntity.Send("Elemento eliminado correctamente"));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
    }
}
