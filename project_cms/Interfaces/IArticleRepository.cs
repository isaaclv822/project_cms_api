using project_cms.DTOs;
using project_cms.Models;

namespace project_cms.Interfaces
{
    public interface IArticleRepository
    {
        Task<List<Article>> GetAllArticlesAsync();
        Task<Article> GetArticleByIdAsync(int id);
        Task<Article> CreateArticleAsync(Article article);
        Task<Article> UpdateArticleAsync(int id, ArticleRequestDTO dto);
        Task DeleteArticleAsync(int id);

    }
}
