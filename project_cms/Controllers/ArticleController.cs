using Microsoft.AspNetCore.Mvc;
using project_cms.DTOs;
using project_cms.Interfaces;
using project_cms.Models;
using project_cms.Services;

namespace project_cms.Controllers
{
    [ApiController]
    [Route("article")]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleRepository _articleRepository;
        private readonly ArticleMapper _articleMapper;

        public ArticleController(IArticleRepository articleRepository, ArticleMapper articleMapper)
        {
            _articleRepository = articleRepository;
            _articleMapper = articleMapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllArticles()
        {
            var articles = await _articleRepository.GetAllArticlesAsync();
            var articleDto = articles.Select(article => _articleMapper.EntityToDto(article)).ToList();
            return Ok(articleDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetArticleById(int id)
        {
            var article = _articleMapper.EntityToDto(await _articleRepository.GetArticleByIdAsync(id));

            if (article == null) return NotFound();

            return Ok(article);
        }

        [HttpPost("add")]
        public async Task<Article> AddArticle([FromBody] ArticleRequestDTO articleDto)
        {
            var article = await _articleMapper.DtoToEntity(articleDto);
            article.PublishedDate = DateTime.UtcNow;
            article.UpdatedDate = DateTime.UtcNow;
            return await _articleRepository.CreateArticleAsync(article);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateArticle(int id, [FromBody] ArticleRequestDTO articleDto)
        {
            var existingArticle = await _articleRepository.GetArticleByIdAsync(id);
            if (existingArticle == null) return NotFound();

            var updatedArticle = await _articleMapper.DtoToEntity(articleDto);
            updatedArticle.Id = id;
            updatedArticle.UpdatedDate = DateTime.UtcNow;

            await _articleRepository.UpdateArticleAsync(updatedArticle);

            return NoContent();
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            var existingArticle = await _articleRepository.GetArticleByIdAsync(id);
            if (existingArticle == null) return NotFound($"Article d\'ID : {id} introuvable.");

            await _articleRepository.DeleteArticleAsync(id);

            return NoContent();
        }
    }
}
