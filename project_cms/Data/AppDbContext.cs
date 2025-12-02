using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using project_cms.Models;

namespace project_cms.Data
{
    public class AppDbContext : DbContext
    {
        protected readonly IConfiguration configuration;

        public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration _configuration) : base(options)
        {
            configuration = _configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(configuration.GetConnectionString("bdd"));
            }
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Article> Articles { get; set; }
    }
}
