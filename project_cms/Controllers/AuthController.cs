using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using project_cms.DTOs;

namespace project_cms.Controllers
{
    /// <summary>
    /// Contrôleur responsable de l'authentification des utilisateurs :
    /// enregistrement, connexion et génération de jetons JWT.
    /// </summary>
    [Route("user")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthController(UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        /// <summary>
        /// Enregistre un nouvel utilisateur à partir des informations fournies.
        /// Crée une instance d'<see cref="IdentityUser"/> et utilise <see cref="UserManager{TUser}.CreateAsync"/> pour persister l'utilisateur.
        /// </summary>
        /// <param name="dto">DTO contenant le nom d'utilisateur (email) et le mot de passe.</param>
        /// <returns>
        /// 200 OK si l'utilisateur est créé avec succès ;
        /// 400 BadRequest avec les erreurs d'Identity si la création échoue.
        /// </returns>
        // POST: api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] LoginDto dto)
        {
            var user = new IdentityUser { UserName = dto.Username, Email = dto.Username };
            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            return Ok();
        }

        /// <summary>
        /// Authentifie un utilisateur et retourne un JWT en cas de succès.
        /// Vérifie d'abord l'existence de l'utilisateur puis la validité du mot de passe.
        /// </summary>
        /// <param name="dto">DTO contenant le nom d'utilisateur (email) et le mot de passe.</param>
        /// <returns>
        /// 200 OK avec un objet contenant le jeton JWT si authentification réussie ;
        /// 401 Unauthorized si l'utilisateur n'existe pas ou si le mot de passe est invalide.
        /// </returns>
        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.Username);
            if (user == null) return Unauthorized();

            var valid = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!valid) return Unauthorized();

            var token = GenerateJwtToken(user);
            return Ok(new { token });
        }

        /// <summary>
        /// Génère un jeton JWT signé HMAC-SHA256 pour l'utilisateur donné.
        /// Les paramètres (Key, Issuer, Audience) sont lus depuis la section "Jwt" de la configuration.
        /// Le jeton contient les claims : sub (username), nameid (user id) et jti (identifiant unique).
        /// </summary>
        /// <param name="user">Utilisateur pour lequel créer le jeton.</param>
        /// <returns>Chaîne représentant le JWT signé.</returns>
        private string GenerateJwtToken(IdentityUser user)
        {
            var jwtSection = _configuration.GetSection("Jwt");
            var key = Encoding.UTF8.GetBytes(jwtSection["Key"]);
            var creds = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName ?? string.Empty),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtSection["Issuer"],
                audience: jwtSection["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}