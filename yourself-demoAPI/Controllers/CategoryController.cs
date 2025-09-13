using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using yourself_demoAPI.Repository.Home;

namespace yourself_demoAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[AllowAnonymous]
	public class CategoryController : ControllerBase
	{
		private readonly ICategoryService _categoryService;
		public CategoryController(ICategoryService categoryService)
		{
			_categoryService = categoryService;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllCategories()
		{
			var categories = await _categoryService.GetAllCategoriesAsync();
			return Ok(categories);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetCategoryById(Guid id)
		{
			var category = await _categoryService.GetCategoryByIdAsync(id);
			if (category == null)
				return NotFound(new { Message = "Category not found" });

			return Ok(category);
		}

		[HttpGet("item/{id}")]
		public async Task<IActionResult> GetItemById(Guid id)
		{
			var item = await _categoryService.GetItemByIdAsync(id);
			if (item == null)
				return NotFound(new { Message = "Item not found" });

			return Ok(item);
		}
	}

}
