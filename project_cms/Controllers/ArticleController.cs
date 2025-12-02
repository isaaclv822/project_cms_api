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

            if (article == null) return NotFound($"Article d\'ID : {id}, introuvable.");

            return Ok(article);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddArticle([FromBody] ArticleRequestDTO articleDto)
        {
            var article = await _articleMapper.DtoToEntity(articleDto);
            var created = await _articleRepository.CreateArticleAsync(article);
            
            if (created == null)
            {
                return BadRequest("L'opération a échouée.");
            }

            return Ok(article);
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult<Article>> UpdateArticle(int id, [FromBody] ArticleRequestDTO articleDto)
        {
            var updatedArticle = await _articleRepository.UpdateArticleAsync(id, articleDto);
            if (updatedArticle == null) return NotFound($"Article d\'ID : {id}, introuvable.");

            return Ok(updatedArticle);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            var existingArticle = await _articleRepository.GetArticleByIdAsync(id);
            if (existingArticle == null) return NotFound($"Article d\'ID : {id}, introuvable.");

            await _articleRepository.DeleteArticleAsync(id);

            return NoContent();
        }
    }
}
