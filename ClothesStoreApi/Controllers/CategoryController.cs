using Microsoft.AspNetCore.Mvc;
using Store.Services.Services.CategoriesService;
using Store.Services.Services.CategoriesService.Dtos;

namespace Store.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> AddCategory(CategoryCreateDto dto)
        {
            var result = await _categoryService.CreateCategoryAsync(dto);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllCategories()
        {
            var result = await _categoryService.GetAllCategoriesAsync();
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }
        [HttpGet]
        [Route("{categoryId}")]
        public async Task<IActionResult> GetCategory(int categoryId)
        {
            var result = await _categoryService.GetCategoryByIdAsync(categoryId);
            return result.IsSuccess ? Ok(result) : result.Errors.Code == "400" ?
                 BadRequest(result) : NotFound(result);
        }
        [HttpDelete]
        [Route("{categoryId}")]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            var result = await _categoryService.DeleteCategoryAsync(categoryId);
            return result.IsSuccess ? Ok(result) : result.Errors.Code == "400" ?
                 BadRequest(result) : NotFound(result);
        }
        [HttpPut]
        [Route("{categoryId}")]
        public async Task<IActionResult> UpdateCategory(int categoryId, CategoryUpdateDto category)
        {
            var result = await _categoryService.UpdateCategoryAsync(categoryId, category);
            return result.IsSuccess ? Ok(result) : result.Errors.Code == "400" ?
                 BadRequest(result) : NotFound(result);
        }
    }
}
