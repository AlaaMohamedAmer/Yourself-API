using yourself_demoAPI.Models.Auth;

namespace Yourself_App.Models.School
{
	public class School
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Code { get; set; }
		public ICollection<User> Users { get; set; } = new List<User>();
	}
}
