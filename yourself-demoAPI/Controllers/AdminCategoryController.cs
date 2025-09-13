using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using yourself_demoAPI.DTOs.Home;
using yourself_demoAPI.Repository.Home;

namespace yourself_demoAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Roles = "SuperAdmin")]
	public class AdminCategoryController : ControllerBase
	{
		private readonly ICategoryService _categoryService;
		public AdminCategoryController(ICategoryService categoryService)
		{
			_categoryService = categoryService;
		}

		[HttpPost("create-category")]
		public async Task<IActionResult> CreateCategory([FromBody] string name)
		{
			var result = await _categoryService.CreateCategoryAsync(name);
			return Ok(result);
		}

		[HttpPut("update-category/{categoryId}")]
		public async Task<IActionResult> UpdateCategory(Guid categoryId, [FromBody] string name)
		{
			await _categoryService.UpdateCategoryAsync(categoryId, name);
			return NoContent();
		}

		[HttpDelete("delete-category/{categoryId}")]
		public async Task<IActionResult> DeleteCategory(Guid categoryId)
		{
			await _categoryService.DeleteCategoryAsync(categoryId);
			return NoContent();
		}

		[HttpPost("create-item")]
		public async Task<IActionResult> CreateItemAsync([FromForm] CreateItemDTO itemDTO)
		{
			var result = await _categoryService.CreateItemAsync(itemDTO);
			return Ok(result);
		}

		[HttpPut("update-item")]
		public async Task<IActionResult> UpdateItemAsync([FromForm] UpdateItemDTO itemDTO)
		{
			await _categoryService.UpdateItemAsync(itemDTO);
			return NoContent();
		}

		[HttpDelete("delete-item/{itemId}")]
		public async Task<IActionResult> DeleteItemAsync(Guid itemId)
		{
			await _categoryService.DeleteItemAsync(itemId);
			return NoContent();
		}
	}
}
