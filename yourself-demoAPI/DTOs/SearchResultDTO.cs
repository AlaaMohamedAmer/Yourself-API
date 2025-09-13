namespace yourself_demoAPI.DTOs
{
	public class SearchResultDTO
	{
		public string EntityName { get; set; } = string.Empty;
		public Guid Id {  get; set; }
		public string Name { get; set; } = string.Empty;
		public string? Description { get; set; } = string.Empty ;
		public string? ImageUrl { get; set; } = string.Empty;
	}
}
