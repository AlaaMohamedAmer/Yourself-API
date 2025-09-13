namespace yourself_demoAPI.Models.Auth
{
    public class Admin
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public Guid AccountId { get; set; }
        public Account Account { get; set; } = null!;
    }
}
