using System.Threading.Tasks;
using project_cms.DTOs;
using project_cms.Models;
using project_cms.Services;
using Xunit;

namespace project_cms.Tests
{
    public class ArticleMapperTests
    {
        private readonly ArticleMapper _mapper = new ArticleMapper();

        [Fact]
        public void EntityToDto_WhenNull_ReturnsNull()
        {
            var result = _mapper.EntityToDto(null);
            Assert.Null(result);
        }

        [Fact]
        public async Task DtoToEntity_CreatesEntity()
        {
            var dto = new ArticleRequestDTO { Title = "t", Content = "c" };
            var entity = await _mapper.DtoToEntity(dto);
            Assert.NotNull(entity);
            Assert.Equal("t", entity.Title);
            Assert.Equal("c", entity.Content);
        }
    }
}