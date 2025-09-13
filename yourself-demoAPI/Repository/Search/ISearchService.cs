using yourself_demoAPI.DTOs;

namespace yourself_demoAPI.Repository.Search
{
	public interface ISearchService
	{
		Task<List<SearchResultDTO>> SearchAsync(string Keyword);
	}
}
