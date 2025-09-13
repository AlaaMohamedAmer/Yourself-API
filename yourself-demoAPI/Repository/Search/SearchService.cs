using Microsoft.EntityFrameworkCore;
using yourself_demoAPI.Data;
using yourself_demoAPI.DTOs;
using yourself_demoAPI.Migrations;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace yourself_demoAPI.Repository.Search
{
	public class SearchService : ISearchService
	{
		private readonly ApplicationDBContext _dbContext;
		public SearchService(ApplicationDBContext dBContext)
		{
			_dbContext = dBContext;
		}
		public async Task<List<SearchResultDTO>> SearchAsync(string keyword)
		{
			string searchPattern = $"%{keyword}%";

			if (string.IsNullOrEmpty(keyword))
				return new List<SearchResultDTO>();

			var Category = await _dbContext.Categories
				.Where(c => EF.Functions.Like(c.Name, searchPattern))
				.Select(c => new SearchResultDTO
				{
					EntityName = "Categorie",
					Id = c.Id,
					Name = c.Name,
				}).ToListAsync();

			var item = await _dbContext.Items
				.Where(i => EF.Functions.Like(i.Name, searchPattern))
				.Select(i => new SearchResultDTO
				{
					EntityName = "Item",
					Id = i.Id,
					Name = i.Name,
					Description = i.Description,
					ImageUrl = i.ImageUrl,
				}).ToListAsync();

			var collection = await _dbContext.collections
				.Where(cat => EF.Functions.Like(cat.Name, searchPattern))
				.Select(cat => new SearchResultDTO
				{
					EntityName = "Collection",
					Id = cat.Id,
					Name = cat.Name,
					Description = cat.Description,
					ImageUrl = cat.ImageUrl,
				}).ToListAsync();

			return Category
				.Concat(item)
				.Concat(collection)
				.ToList();
		}
	}
}
