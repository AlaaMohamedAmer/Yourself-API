namespace yourself_demoAPI.DTOs.Home
{
	public class UpdateCollectionDTO
	{
		public Guid Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public IFormFile? Image { get; set; }
	}
}
