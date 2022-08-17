using CarniceriaFinal.Core;
using CarniceriaFinal.Core.CustomException;
using CarniceriaFinal.ModelsEF;
using CarniceriaFinal.Productos.DTOs;
using CarniceriaFinal.Productos.IRepository;
using CarniceriaFinal.Productos.Models;
using CarniceriaFinal.Productos.Servicios;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace CarniceriaFinal.Productos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductoController : ControllerBase
    {
        public readonly DBContext Context;

        public readonly IProductoService ProductoService;
        public readonly IUnidadMedidaRepository IUnidadMedidaRepository;
        public ProductoController(IProductoService ProductoServi, IUnidadMedidaRepository IUnidadMedidaRepository)
        {
            ProductoService = ProductoServi;
            this.IUnidadMedidaRepository = IUnidadMedidaRepository;
        }

        [HttpGet(Name = "")]
        public async Task<IActionResult> Get() {
            RSEntity<List<ProductEntity>> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await ProductoService.GetProductos()));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
        [HttpPost("products-in-car")]
        public async Task<IActionResult> GetProductosInCar(List<int> products)
        {
            RSEntity<List<ProductEntity>> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await ProductoService.GetProductosInCar(products)));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
        
        [HttpGet("simple-products")]
        public async Task<IActionResult> GetSimpleProducts()
        {
            RSEntity<List<ProductTableAdminEntity>> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await ProductoService.GetSimpleProductos()));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
        [HttpGet("product-to-subcategory/{idSubCategory}")]
        public async Task<IActionResult> GetSimpleProductsSubCategories(int idSubCategory)
        {
            RSEntity<List<SimpleProductInSubCategory>> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await ProductoService.GetSimpleProductsByIdSubCategories(idSubCategory)));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
        [HttpGet("product-to-subcategory")]
        public async Task<IActionResult> GetAllSimpleProductsSubCategories()
        {
            RSEntity<List<SimpleProductInSubCategory>> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await ProductoService.GetSimpleProductsByIdSubCategories(null)));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
        [HttpGet("product-detail/{idProduct}")]
        public async Task<IActionResult> GetDetailAdminProductoById(int idProduct)
        {
            RSEntity<ProductAdminCompleteEntity> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await ProductoService.GetDetailAdminProductoById(idProduct)));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
        [HttpGet("by-category/{idCategory}/{idSubCategory}")]
        public async Task<IActionResult> FindProductsBySubCategory(int idCategory, int idSubCategory)
        {
            RSEntity<List<ProductEntity>> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await ProductoService.FindProductsBySubCategory(idCategory, idSubCategory)));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
        [HttpGet("products-promotion")]
        public async Task<IActionResult> getAllProductsPromotions()
        {
            RSEntity<PromotionsInProduct> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await ProductoService.getAllProductsPromotions()));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
        [HttpGet("by-category/{idCategory}")]
        public async Task<IActionResult> FindProductsByCategoryId(int idCategory)
        {
            RSEntity<List<ProductEntity>> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await ProductoService.FindProductsByCategoryId(idCategory)));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SaveProduct([FromBody] ProductSaveRequestEntity product)
        {
            RSEntity<string> rsEntity = new();
            try
            {
                await ProductoService.SaveProduct(product);
                return Ok(rsEntity.Send("correctamente"));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductSaveRequestEntity product)
        {
            RSEntity<string> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await ProductoService.UpdateProduct(product)));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
        [HttpDelete("{idProduct}")]
        public IActionResult DeleteProduct(int idProduct)
        {
            RSEntity<string> rsEntity = new();
            try
            {
                ProductoService.DeleteProduct(idProduct);
                return Ok(rsEntity.Send("correctamente"));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
        [HttpPut("enable/{idProduct}")]
        public async Task<IActionResult> EnableProduct(int idProduct)
        {
            RSEntity<string> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await ProductoService.EnableProduct(idProduct)));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
        [HttpPatch("stock/{idProduct}/{stock}")]
        public async Task<IActionResult> UpdateStock(int idProduct, int stock)
        {
            RSEntity<string> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await ProductoService.UpdateStock(idProduct, stock)));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        } 
        [HttpDelete("product-detail/{idProduct}")]
        public IActionResult DeleteProductDetail(int idProduct)
        {
            RSEntity<string> rsEntity = new();
            try
            {
                ProductoService.DeleteProductDetail(idProduct);
                return Ok(rsEntity.Send("correctamente"));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
        
    }
}
