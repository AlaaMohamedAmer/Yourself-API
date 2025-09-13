using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Yourself_App.Models.School;
using yourself_demoAPI.Models.Auth;
using yourself_demoAPI.Models.Home;

namespace yourself_demoAPI.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }
		public DbSet<Account> Accounts { get; set; }
		public DbSet<Admin> Admins { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<School> Schools { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<Item> Items { get; set; }
		public DbSet<HomeCollections> collections { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Account>()
				.HasOne(a => a.Admin)
				.WithOne(ad => ad.Account)
				.HasForeignKey<Admin>(ad => ad.AccountId)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<Account>()
				.HasOne(a => a.User)
				.WithOne(u => u.Account)
				.HasForeignKey<User>(u => u.AccountId)
				.OnDelete(deleteBehavior: DeleteBehavior.Cascade);

			modelBuilder.Entity<Account>()
		   .HasIndex(a => a.Email)
		   .IsUnique();

			modelBuilder.Entity<User>()
				.HasOne(u => u.School)
				.WithMany(s => s.Users)
				.HasForeignKey(u => u.SchoolId)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<Item>()
				.HasOne(i => i.Category)
				.WithMany(c => c.Items)
				.HasForeignKey(i => i.CategoryId)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<Account>().ToTable("Accounts", "identity");
			modelBuilder.Entity<Admin>().ToTable("Admins", "identity");
			modelBuilder.Entity<User>().ToTable("Users", "identity");

			//just for testing
			var engSchoolId = Guid.Parse("11111111-1111-1111-1111-111111111111");
			modelBuilder.Entity<School>().HasData(
				new School { Id = engSchoolId, Name = "Engineering School", Code = "ENG001" }
			);
		}
	}
}
