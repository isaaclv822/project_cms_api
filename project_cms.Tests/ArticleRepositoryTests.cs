using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using project_cms.Data;
using project_cms.DTOs;
using project_cms.Models;
using project_cms.Repositories;
using Xunit;

namespace project_cms.Tests
{
    // TestAppDbContext override OnConfiguring pour éviter l'appel à UseNpgsql du contexte de production.
    internal class TestAppDbContext : AppDbContext
    {
        public TestAppDbContext(DbContextOptions<AppDbContext> options)
            : base(options, Mock.Of<IConfiguration>())
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // No-op in tests to prevent production DB configuration
        }
    }

    public class ArticleRepositoryTests
    {
        private DbContextOptions<AppDbContext> CreateNewInMemoryOptions(string dbName)
        {
            return new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
        }

        [Fact]
        public async Task CreateArticleAsync_AddsArticle()
        {
            // Arrange
            var options = CreateNewInMemoryOptions("CreateArticleDb");
            await using var context = new TestAppDbContext(options);
            var repo = new ArticleRepository(context);
            var article = new Article { Title = "t", Content = "c" };

            // Act
            var created = await repo.CreateArticleAsync(article);

            // Assert
            Assert.NotNull(created);
            Assert.Equal(1, context.Articles.Count());
            Assert.Equal("t", created.Title);
        }

        [Fact]
        public async Task UpdateArticleAsync_ReturnsNull_WhenNotFound()
        {
            // Arrange
            var options = CreateNewInMemoryOptions("UpdateNotFoundDb");
            await using var context = new TestAppDbContext(options);
            var repo = new ArticleRepository(context);
            var dto = new ArticleRequestDTO { Title = "X", Content = "Y" };

            // Act
            var result = await repo.UpdateArticleAsync(999, dto);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateArticleAsync_Updates_WhenFound()
        {
            // Arrange
            var options = CreateNewInMemoryOptions("UpdateFoundDb");
            await using (var context = new TestAppDbContext(options))
            {
                context.Articles.Add(new Article { Id = 1, Title = "old", Content = "old" });
                await context.SaveChangesAsync();
            }

            await using (var context = new TestAppDbContext(options))
            {
                var repo = new ArticleRepository(context);
                var dto = new ArticleRequestDTO { Title = "new", Content = "new content" };

                // Act
                var updated = await repo.UpdateArticleAsync(1, dto);

                // Assert
                Assert.NotNull(updated);
                Assert.Equal("new", updated.Title);
                Assert.Equal("new content", updated.Content);
            }
        }
    }
}