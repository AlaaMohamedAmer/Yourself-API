using yourself_demoAPI.Models.Home;

namespace yourself_demoAPI.DTOs.Home
{
	public class CategoryWithItemsDTO:CategoryDTO
	{
		public List<ItemDTO> Items { get; set; }
	}
}
