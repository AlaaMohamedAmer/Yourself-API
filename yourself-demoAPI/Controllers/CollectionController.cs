using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using yourself_demoAPI.Repository.Collection;

namespace yourself_demoAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[AllowAnonymous]
	public class CollectionController : ControllerBase
	{
		private readonly ICollectionServices _collectionServices;
		public CollectionController(ICollectionServices collectionServices)
		{
			_collectionServices = collectionServices;
		}

		[HttpGet()]
		public async Task<IActionResult> GetAllCollectionsAsync()
		{
			var collections = await _collectionServices.GetAllCollections();
			return Ok(collections);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetCollectionAsync(Guid id)
		{
			var collections = await _collectionServices.GetCollection(id);
			return Ok(collections);
		}
	}
}
