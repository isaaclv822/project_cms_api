using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using project_cms.Models;

namespace project_cms.Data
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        protected readonly IConfiguration configuration;

        public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration _configuration) : base(options)
        {
            configuration = _configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Utilisation de la chaîne de connexion "bdd"
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("bdd"));
        }

        // Conserver les entités spécifiques au projet (articles...)
        public DbSet<Article> Articles { get; set; }
    }
}
