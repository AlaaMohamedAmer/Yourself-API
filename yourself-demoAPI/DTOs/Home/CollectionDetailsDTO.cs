namespace yourself_demoAPI.DTOs.Home
{
	public class CollectionDetailsDTO
	{
		public Guid Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public string? ImageUrl { get; set; }
	}
}
