using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using project_cms.DTOs;
using project_cms.Interfaces;
using project_cms.Models;
using project_cms.Services;

namespace project_cms.Controllers
{
    /// <summary>
    /// Contrôleur pour la gestion des articles (CRUD).
    /// Toutes les routes requièrent une authentification grâce à <see cref="AuthorizeAttribute"/>.
    /// </summary>
    [Authorize]
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

        /// <summary>
        /// Récupère tous les articles.
        /// Convertit les entités en DTOs avant de les retourner.
        /// </summary>
        /// <returns>200 OK avec la liste des articles sous forme de <see cref="ArticleResponseDTO"/>.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllArticles()
        {
            var articles = await _articleRepository.GetAllArticlesAsync();
            var articleDto = articles.Select(article => _articleMapper.EntityToDto(article)).ToList();
            return Ok(articleDto);
        }

        /// <summary>
        /// Récupère un article par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant de l'article.</param>
        /// <returns>
        /// 200 OK avec l'article (DTO) si trouvé ;
        /// 404 NotFound si l'article n'existe pas.
        /// </returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetArticleById(int id)
        {
            var article = _articleMapper.EntityToDto(await _articleRepository.GetArticleByIdAsync(id));

            if (article == null) return NotFound($"Article d\'ID : {id}, introuvable.");

            return Ok(article);
        }

        /// <summary>
        /// Ajoute un nouvel article à partir d'un DTO de requête.
        /// Utilise le mapper pour transformer le DTO en entité avant création.
        /// </summary>
        /// <param name="articleDto">DTO contenant les données de l'article à créer.</param>
        /// <returns>
        /// 200 OK avec l'entité créée si succès ;
        /// 400 BadRequest si la création échoue.
        /// </returns>
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

        /// <summary>
        /// Met à jour un article existant.
        /// </summary>
        /// <param name="id">Identifiant de l'article à mettre à jour.</param>
        /// <param name="articleDto">DTO contenant les nouvelles valeurs.</param>
        /// <returns>
        /// 200 OK avec l'article mis à jour si trouvé et modifié ;
        /// 404 NotFound si l'article n'existe pas.
        /// </returns>
        [HttpPut("update/{id}")]
        public async Task<ActionResult<Article>> UpdateArticle(int id, [FromBody] ArticleRequestDTO articleDto)
        {
            var updatedArticle = await _articleRepository.UpdateArticleAsync(id, articleDto);
            if (updatedArticle == null) return NotFound($"Article d\'ID : {id}, introuvable.");

            return Ok(updatedArticle);
        }

        /// <summary>
        /// Supprime un article par son identifiant.
        /// Vérifie d'abord l'existence avant la suppression pour retourner le code HTTP approprié.
        /// </summary>
        /// <param name="id">Identifiant de l'article à supprimer.</param>
        /// <returns>
        /// 204 NoContent si la suppression a réussi ;
        /// 404 NotFound si l'article n'existe pas.
        /// </returns>
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
