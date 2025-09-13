using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using yourself_demoAPI.DTOs.Home;
using yourself_demoAPI.Repository.Collection;

namespace yourself_demoAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Roles = "SuperAdmin")]

	public class AdminCollectionController : ControllerBase
	{
		private readonly ICollectionServices _collectionServices;
		public AdminCollectionController(ICollectionServices collectionServices)
		{
			_collectionServices = collectionServices;
		}

		[HttpPost("create-collection")]
		public async Task<IActionResult> CreateCollection([FromForm] CreateCollectionDTO createCollectionDTO)
		{
			var result = await _collectionServices.CreateCollection(createCollectionDTO);
			return Ok(result);
		}

		[HttpPut("update-collection/{id}")]
		public async Task<IActionResult> UpdateCollection(Guid id, [FromForm] UpdateCollectionDTO dto)
		{
			dto.Id = id;
			await _collectionServices.UpdateCollection(dto);
			return NoContent();
		}

		[HttpDelete("delete-collection/{id}")]
		public async Task<IActionResult> DeleteCollection(Guid id)
		{
			await _collectionServices.DeleteCollection(id);
			return NoContent();
		}
	}
}
