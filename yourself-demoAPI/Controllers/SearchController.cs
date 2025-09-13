using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using yourself_demoAPI.Repository.Search;

namespace yourself_demoAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SearchController : ControllerBase
	{
		private readonly ISearchService _searchService;
		public SearchController(ISearchService searchService)
		{
			_searchService = searchService;
		}

		[HttpGet]
		public async Task<IActionResult> Search([FromQuery] string keyword)
		{
			var result = await _searchService.SearchAsync(keyword);
			return Ok(result);
		}
	}
}
