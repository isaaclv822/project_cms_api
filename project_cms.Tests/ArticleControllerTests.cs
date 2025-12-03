using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using project_cms.Controllers;
using project_cms.DTOs;
using project_cms.Interfaces;
using project_cms.Models;
using project_cms.Services;
using Xunit;

namespace project_cms.Tests
{
    public class ArticleControllerTests
    {
        [Fact]
        public async Task GetAllArticles_ReturnsOkWithDtos()
        {
            // Arrange
            var articles = new List<Article>
            {
                new Article { Id = 1, Title = "T1", Content = "C1" },
                new Article { Id = 2, Title = "T2", Content = "C2" }
            };

            var mockRepo = new Mock<IArticleRepository>();
            mockRepo.Setup(r => r.GetAllArticlesAsync()).ReturnsAsync(articles);

            var mockMapper = new Mock<ArticleMapper>();
            mockMapper.Setup(m => m.EntityToDto(It.IsAny<Article>()))
                      .Returns((Article a) => new ArticleResponseDTO
                      {
                          Id = a.Id,
                          Title = a.Title,
                          Content = a.Content,
                          PublishedDate = a.PublishedDate,
                          UpdatedDate = a.UpdatedDate
                      });

            var controller = new ArticleController(mockRepo.Object, mockMapper.Object);

            // Act
            var result = await controller.GetAllArticles();

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsAssignableFrom<List<ArticleResponseDTO>>(ok.Value);
            Assert.Equal(2, value.Count);
            Assert.Contains(value, v => v.Title == "T1");
        }

        [Fact]
        public async Task GetArticleById_WhenNotFound_ReturnsNotFound()
        {
            // Arrange
            var mockRepo = new Mock<IArticleRepository>();
            mockRepo.Setup(r => r.GetArticleByIdAsync(It.IsAny<int>())).ReturnsAsync((Article)null);

            var mockMapper = new Mock<ArticleMapper>();
            mockMapper.Setup(m => m.EntityToDto((Article)null)).Returns((ArticleResponseDTO)null);

            var controller = new ArticleController(mockRepo.Object, mockMapper.Object);

            // Act
            var result = await controller.GetArticleById(42);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task AddArticle_ReturnsOk_OnSuccess()
        {
            // Arrange
            var dto = new ArticleRequestDTO { Title = "New", Content = "Content" };
            var articleEntity = new Article { Title = dto.Title, Content = dto.Content };

            var mockRepo = new Mock<IArticleRepository>();
            mockRepo.Setup(r => r.CreateArticleAsync(It.IsAny<Article>()))
                    .ReturnsAsync((Article a) => { a.Id = 10; return a; });

            var mockMapper = new Mock<ArticleMapper>();
            mockMapper.Setup(m => m.DtoToEntity(It.IsAny<ArticleRequestDTO>()))
                      .ReturnsAsync(articleEntity);

            var controller = new ArticleController(mockRepo.Object, mockMapper.Object);

            // Act
            var result = await controller.AddArticle(dto);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var returned = Assert.IsType<Article>(ok.Value);
            Assert.Equal(10, returned.Id);
            Assert.Equal("New", returned.Title);
        }
    }
}