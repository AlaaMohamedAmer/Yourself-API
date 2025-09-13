namespace yourself_demoAPI.DTOs.Home
{
	public class UpdateItemDTO
	{
		public Guid Id { get; set; }
		public string? Name { get; set; } = string.Empty;
		public IFormFile? ImageFile { get; set; } 
		public string? Description { get; set; } = string.Empty;
	}
}
