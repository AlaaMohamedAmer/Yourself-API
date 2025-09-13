using Microsoft.EntityFrameworkCore;
using yourself_demoAPI.Data;
using yourself_demoAPI.DTOs.Home;
using yourself_demoAPI.Models.Auth;
using yourself_demoAPI.Models.Home;

namespace yourself_demoAPI.Repository.Home
{
	public class CategoryService : ICategoryService
	{
		private readonly ApplicationDBContext _context;
		private readonly IHttpContextAccessor _contextAccessor;
		public CategoryService(ApplicationDBContext context, IHttpContextAccessor contextAccessor)
		{
			_context = context;
			_contextAccessor = contextAccessor;
		}

		//Admin
		public async Task<CategoryDTO> CreateCategoryAsync(string name)
		{
			var category = new Category
			{
				Id = Guid.NewGuid(),
				Name = name,
			};

			_context.Categories.Add(category);
			await _context.SaveChangesAsync();

			return new CategoryDTO { Id= category.Id , Name= name };
		}
		public async Task UpdateCategoryAsync(Guid categoryId, string name)
		{
			var category = await _context.Categories.FindAsync(categoryId)
				?? throw new Exception("Category not found");

			category.Name = name;
			await _context.SaveChangesAsync();
		}
		public async Task DeleteCategoryAsync(Guid categoryId)
		{
			var category = await _context.Categories
				.Include(c => c.Items)
				.FirstOrDefaultAsync(c => c.Id == categoryId)
				?? throw new Exception("Category not found");

			_context.Items.RemoveRange(category.Items);
			_context.Categories.Remove(category);

			await _context.SaveChangesAsync();
		}
		public async Task<ItemDetailsDTO> CreateItemAsync(CreateItemDTO item)
		{
			var category = await _context.Categories.FindAsync(item.CategoryId)
					?? throw new Exception("Category not found");

			string imageUrl = null;

			if (item.ImageFile != null && item.ImageFile.Length > 0)
			{
				var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/items");
				if (!Directory.Exists(uploadsPath))
					Directory.CreateDirectory(uploadsPath);

				var fileName = $"{Guid.NewGuid()}{Path.GetExtension(item.ImageFile.FileName)}";
				var filePath = Path.Combine(uploadsPath, fileName);

				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					await item.ImageFile.CopyToAsync(stream);
				}

				imageUrl = $"{_contextAccessor.HttpContext.Request.Scheme}://{_contextAccessor.HttpContext.Request.Host}/images/items/{fileName}";
			}

			var newItem = new Item
			{
				Id = Guid.NewGuid(),
				Name = item.Name,
				Description = item.Description,
				ImageUrl = imageUrl,
				CategoryId = item.CategoryId,
			};

			_context.Items.Add(newItem);
			await _context.SaveChangesAsync();

			return new ItemDetailsDTO
			{
				Id = newItem.Id,
				Name = newItem.Name,
				Description = newItem.Description,
				ImageUrl = newItem.ImageUrl,
			};
		}
		public async Task UpdateItemAsync(UpdateItemDTO item)
		{
			var existing = await _context.Items.FindAsync(item.Id)
				?? throw new Exception("Item Not Found");

			if(!string.IsNullOrEmpty(item.Name)) 
				existing.Name = item.Name;

			if(!string.IsNullOrEmpty(item.Description))
				existing.Description = item.Description;

			if (item.ImageFile != null && item.ImageFile.Length > 0)
			{
				if (!string.IsNullOrEmpty(existing.ImageUrl))
				{
					var oldFileName = Path.GetFileName(new Uri(existing.ImageUrl).AbsolutePath);
					var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/items", oldFileName);

					if (File.Exists(oldFilePath))
						File.Delete(oldFilePath);
				}

				var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/items");
				if (!Directory.Exists(uploadsPath))
					Directory.CreateDirectory(uploadsPath);

				var fileName = $"{Guid.NewGuid()}{Path.GetExtension(item.ImageFile.FileName)}";
				var filePath = Path.Combine(uploadsPath, fileName);

				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					await item.ImageFile.CopyToAsync(stream);
				}

				existing.ImageUrl = $"{_contextAccessor.HttpContext.Request.Scheme}://{_contextAccessor.HttpContext.Request.Host}/images/items/{fileName}";
			}

			await _context.SaveChangesAsync();
		}
		public async Task DeleteItemAsync(Guid itemId)
		{
			var item = await _context.Items.FindAsync(itemId)
				?? throw new Exception("Item Not Found");

			if (!string.IsNullOrEmpty(item.ImageUrl))
			{
				var oldFileName = Path.GetFileName(new Uri(item.ImageUrl).AbsolutePath);
				var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/items", oldFileName);

				if (File.Exists(oldFilePath))
					File.Delete(oldFilePath);
			}

			_context.Items.Remove(item);
			await _context.SaveChangesAsync();
		}

		//User
		public async Task<List<CategoryDTO>> GetAllCategoriesAsync()
		{
			return await _context.Categories
				.Select( c => new CategoryDTO { Id = c.Id, Name = c.Name })
				.ToListAsync();
		}
		public async Task<CategoryWithItemsDTO?> GetCategoryByIdAsync(Guid categoryId)
		{
			return await _context.Categories.Where(c => c.Id == categoryId)
				.Select(c => new CategoryWithItemsDTO
				{
					Id = c.Id,
					Name = c.Name,
					Items = c.Items.Select(i => new ItemDTO
					{
						Id = i.Id,
						Name = i.Name,
						ImageUrl = i.ImageUrl,
					}).ToList()
				})
				.FirstOrDefaultAsync();
		}
		public Task<ItemDetailsDTO?> GetItemByIdAsync(Guid itemId)
		{
			return _context.Items
				.Where(i => i.Id == itemId)
				.Select(i => new ItemDetailsDTO
				{
					Id = i.Id,
					Name = i.Name,
					ImageUrl= i.ImageUrl,
					Description = i.Description,
				})
				.FirstOrDefaultAsync();
		}
	}
}
