using Microsoft.AspNetCore.Mvc;
using Store.Services.Services.ProductService;
using Store.Services.Services.ProductService.Dtos;

namespace Store.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> AddProduct(ProductCreateDto dto)
        {
            var result = await _productService.AddProductAsync(dto);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllProducts()
        {
            var result = await _productService.GetAllProductsAsync();
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }
        [HttpGet]
        [Route("{productId}")]
        public async Task<IActionResult> GetColor(int productId)
        {
            var result = await _productService.GetProductByIdAsync(productId);
            return result.IsSuccess ? Ok(result) : result.Errors.Code == "400" ?
                 BadRequest(result) : NotFound(result);
        }
        [HttpDelete]
        [Route("{productId}")]
        public async Task<IActionResult> DeleteColor(int productId)
        {
            var result = await _productService.DeleteProductAsync(productId);
            return result.IsSuccess ? Ok(result) : result.Errors.Code == "400" ?
                 BadRequest(result) : NotFound(result);
        }
        [HttpPut]
        [Route("{productId}")]
        public async Task<IActionResult> UpdateColor(int productId, ProductUpdateDto product)
        {
            var result = await _productService.UpdateProductAsync(productId, product);
            return result.IsSuccess ? Ok(result) : result.Errors.Code == "400" ?
                 BadRequest(result) : NotFound(result);
        }
    }
}
