namespace yourself_demoAPI.DTOs.Home
{
	public class CreateItemDTO
	{
		public string Name { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public IFormFile? ImageFile { get; set; }
		public Guid CategoryId { get; set; }
	}
}
