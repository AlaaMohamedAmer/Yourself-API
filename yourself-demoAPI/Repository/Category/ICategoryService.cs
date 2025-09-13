using yourself_demoAPI.DTOs.Home;

namespace yourself_demoAPI.Repository.Home
{
	public interface ICategoryService
	{
		// Admin
		Task<CategoryDTO> CreateCategoryAsync(string name);
		Task UpdateCategoryAsync(Guid categoryId, string name);
		Task DeleteCategoryAsync(Guid categoryId);

		Task<ItemDetailsDTO> CreateItemAsync(CreateItemDTO item);
		Task UpdateItemAsync(UpdateItemDTO item);
		Task DeleteItemAsync(Guid itemId);

		// Users
		Task<List<CategoryDTO>> GetAllCategoriesAsync();
		Task<CategoryWithItemsDTO?> GetCategoryByIdAsync(Guid categoryId);
		Task<ItemDetailsDTO?> GetItemByIdAsync(Guid itemId);
	}
}
