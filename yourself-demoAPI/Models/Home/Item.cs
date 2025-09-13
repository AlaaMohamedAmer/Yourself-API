namespace yourself_demoAPI.Models.Home
{
	public class Item
	{
		public Guid Id { get; set; } = Guid.NewGuid();
		public string Name { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty ;
		public string ImageUrl { get; set; } = string.Empty;

		public Guid CategoryId { get; set; }
		public Category Category { get; set; }
	}
}
