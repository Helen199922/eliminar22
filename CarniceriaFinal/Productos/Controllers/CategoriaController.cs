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

        [HttpGet("get-all-products-forAll-categories")]
        public async Task<IActionResult> GetAllProductsToCategory()
        {
            RSEntity<List<SimpleProductInSubCategory>> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await CategoriaService.GetAllProductsToCategory()));

            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
        
        [HttpGet("only-categories")]
        public async Task<IActionResult> GetAllCategories()
        {
            RSEntity<List<CategoryEntity>> rsEntity = new();
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
        [HttpGet("only-categories-admin")]
        public async Task<IActionResult> GetAllAdmCategories()
        {
            RSEntity<List<CategoryEntity>> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await CategoriaService.GetAllAdmCategories()));

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
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateCategory([FromBody] CategoriaProductoEntity categoria)
        {
            RSEntity<CategoriaProductoEntity> rsEntity = new();
            try
            {
                var res = await CategoriaService.UpdateCategory(categoria);
                return Ok(rsEntity.Send(res));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }

        [Authorize]
        [HttpPatch("category-status/{idCategory}/{idStatus}")]
        public async Task<IActionResult> ChangeStatusCategory(int idCategory, int idStatus)
        {
            RSEntity<Boolean> rsEntity = new();
            try
            {
                var res = await CategoriaService.ChangeStatusCategory(idCategory, idStatus);
                return Ok(rsEntity.Send(res));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
    }
}
