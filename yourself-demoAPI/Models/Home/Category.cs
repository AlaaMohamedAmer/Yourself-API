namespace yourself_demoAPI.Models.Home
{
	public class Category
	{
		public Guid Id { get; set; } = Guid.NewGuid();
		public string Name { get; set; } = string.Empty;
		public ICollection<Item> Items { get; set; } = new List<Item>();
	}
}
