using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using yourself_demoAPI.Data;
using yourself_demoAPI.DTOs.Home;
using yourself_demoAPI.Models.Home;

namespace yourself_demoAPI.Repository.Collection
{
	public class CollectionService : ICollectionServices
	{
		private readonly ApplicationDBContext _dbContext;
		private readonly IHttpContextAccessor _httpContextAccessor;
		public CollectionService(ApplicationDBContext dBContext, IHttpContextAccessor httpContextAccessor)
		{
			_dbContext = dBContext;
			_httpContextAccessor = httpContextAccessor;
		}

		//Admin
		public async Task<CollectionDetailsDTO> CreateCollection(CreateCollectionDTO collectionDTO)
		{
			string imageUrl = null;

			if (collectionDTO.Image != null && collectionDTO.Image.Length > 0)
			{
				var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/collections");
				if (!Directory.Exists(uploadsPath))
					Directory.CreateDirectory(uploadsPath);

				var fileName = $"{Guid.NewGuid()}{Path.GetExtension(collectionDTO.Image.FileName)}";
				var filePath = Path.Combine(uploadsPath, fileName);

				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					await collectionDTO.Image.CopyToAsync(stream);
				}

				imageUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/images/collections/{fileName}";
			}

			var newCollection = new HomeCollections
			{
				Id = Guid.NewGuid(),
				Name = collectionDTO.Name,
				Description = collectionDTO.Description,
				ImageUrl = imageUrl
			};

			_dbContext.collections.Add(newCollection);
			await _dbContext.SaveChangesAsync();

			return new CollectionDetailsDTO
			{
				Id = newCollection.Id,
				Name = newCollection.Name,
				Description = newCollection.Description,
				ImageUrl = newCollection.ImageUrl
			};
		}
		public async Task UpdateCollection(UpdateCollectionDTO collectionDTO)
		{
			var collection = _dbContext.collections.FirstOrDefault(c => c.Id == collectionDTO.Id)
				?? throw new Exception("Collection Not Found");

			if (!string.IsNullOrEmpty(collectionDTO.Name))
				collection.Name = collectionDTO.Name;

			if (!string.IsNullOrEmpty(collectionDTO.Description))
				collection.Description = collectionDTO.Description;


			if (collectionDTO.Image != null && collectionDTO.Image.Length > 0)
			{

				if (!string.IsNullOrEmpty(collection.ImageUrl))
				{
					var oldFileName = Path.GetFileName(new Uri(collection.ImageUrl).AbsolutePath);
					var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/collections", oldFileName);

					if (File.Exists(oldFilePath))
						File.Delete(oldFilePath);
				}

				var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/collections");
				if (!Directory.Exists(uploadsPath))
					Directory.CreateDirectory(uploadsPath);

				var fileName = $"{Guid.NewGuid()}{Path.GetExtension(collectionDTO.Image.FileName)}";
				var filePath = Path.Combine(uploadsPath, fileName);

				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					await collectionDTO.Image.CopyToAsync(stream);
				}

				collection.ImageUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/images/collections/{fileName}";
			}

			await _dbContext.SaveChangesAsync();
		}
		public async Task DeleteCollection(Guid collectionId)
		{
			var collection = await _dbContext.collections.FindAsync(collectionId)
				?? throw new Exception("Collection Not Found");

			if (!string.IsNullOrEmpty(collection.ImageUrl))
			{
				var oldFileName = Path.GetFileName(new Uri(collection.ImageUrl).AbsolutePath);
				var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/collections", oldFileName);

				if (File.Exists(oldFilePath))
					File.Delete(oldFilePath);
			}

			_dbContext.collections.Remove(collection);
			await _dbContext.SaveChangesAsync();
		}


		//User
		public async Task<List<CollectionDTO>> GetAllCollections()
		{
			return await _dbContext.collections
				.Select(c => new CollectionDTO
				{
					Id = c.Id,
					Name = c.Name,
					ImageUrl = c.ImageUrl
				}).ToListAsync();
		}
		public async Task<CollectionDetailsDTO> GetCollection(Guid collectionId)
		{
			var collection = await _dbContext.collections.FindAsync(collectionId)
					?? throw new Exception("Collection not found");

			return new CollectionDetailsDTO
			{
				Id= collection.Id,
				Name = collection.Name,
				Description = collection.Description,
				ImageUrl = collection.ImageUrl
			};
		}
	}
}
