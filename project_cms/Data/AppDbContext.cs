using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using project_cms.Models;

namespace project_cms.Data
{
    /// <summary>
    /// Contexte EF Core pour l'application.
    /// Hérite de <see cref="IdentityDbContext{IdentityUser}"/> pour inclure les tables d'identité.
    /// </summary>
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        protected readonly IConfiguration configuration;

        /// <summary>
        /// Constructeur injectant les options EF et la configuration.
        /// </summary>
        /// <param name="options">Options de configuration du DbContext.</param>
        /// <param name="_configuration">Configuration de l'application (utilisée pour la connection string).</param>
        public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration _configuration) : base(options)
        {
            configuration = _configuration;
        }

        /// <summary>
        /// Configure le fournisseur de base de données.
        /// Lit la chaîne de connexion nommée "bdd" dans la configuration et configure Npgsql.
        /// Note : si vous configurez déjà le provider dans <see cref="Program.cs"/>, cette méthode peut être optionnelle.
        /// </summary>
        /// <param name="optionsBuilder">Builder des options EF.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Utilisation de la chaîne de connexion "bdd"
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("bdd"));
        }

        /// <summary>
        /// DbSet pour les entités Article spécifiques au projet.
        /// </summary>
        public DbSet<Article> Articles { get; set; }
    }
}
