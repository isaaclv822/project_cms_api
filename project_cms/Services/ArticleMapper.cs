using project_cms.DTOs;
using project_cms.Models;

namespace project_cms.Services
{
    public class ArticleMapper
    {
        public async Task<Article> DtoToEntity(ArticleRequestDTO dto)
        {
            return new Article
            {
                Title = dto.Title,
                Content = dto.Content,
            };
        }

        public ArticleResponseDTO EntityToDto(Article entity)
        {
            if (entity == null) return null;

            return new ArticleResponseDTO
            {
                Id = entity.Id,
                Title = entity.Title,
                Content = entity.Content,
                PublishedDate = entity.PublishedDate,
                UpdatedDate = entity.UpdatedDate
            };
        }
    }
}
