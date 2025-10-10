using Microsoft.AspNetCore.Mvc;
using Store.Services.Services.CategoriesService.Dtos;
using Store.Services.Services.SubcategoryService;
using Store.Services.Services.SubcategoryService.Dtos;

namespace Store.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubcategoryController : ControllerBase
    {
        private readonly ISubcategoryService _subcategoryService;
        public SubcategoryController(ISubcategoryService subcategoryService)
        {
            _subcategoryService = subcategoryService;
        }
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> AddCategory(SubcategoryCreateDto dto)
        {
            var result = await _subcategoryService.AddSubcategoryAsync(dto);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllCategories()
        {
            var result = await _subcategoryService.GetAllSubcategoryAsync();
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }
        [HttpGet]
        [Route("{subcategoryId}")]
        public async Task<IActionResult> GetCategory(int subcategoryId)
        {
            var result = await _subcategoryService.GetSubcategoryByIdAsync(subcategoryId);
            return result.IsSuccess ? Ok(result) : result.Errors.Code == "400" ?
                 BadRequest(result) : NotFound(result);
        }
        [HttpDelete]
        [Route("{subcategoryId}")]
        public async Task<IActionResult> DeleteCategory(int subcategoryId)
        {
            var result = await _subcategoryService.DeleteSubcategoryAsync(subcategoryId);
            return result.IsSuccess ? Ok(result) : result.Errors.Code == "400" ?
                 BadRequest(result) : NotFound(result);
        }
        [HttpPut]
        [Route("{subcategoryId}")]
        public async Task<IActionResult> UpdateCategory(int subcategoryId, SubcategoryUpdateDto subcategory)
        {
            var result = await _subcategoryService.UpdateSubcategoryAsync(subcategoryId, subcategory);
            return result.IsSuccess ? Ok(result) : result.Errors.Code == "400" ?
                 BadRequest(result) : NotFound(result);
        }
    }
}
