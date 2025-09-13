namespace yourself_demoAPI.DTOs.Home
{
	public class CreateCollectionDTO
	{
		public string Name { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public IFormFile Image { get; set; }
	}
}
