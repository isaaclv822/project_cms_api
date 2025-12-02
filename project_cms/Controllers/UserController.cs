using Microsoft.AspNetCore.Mvc;
using project_cms.Interfaces;
using project_cms.Models;

namespace project_cms.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _repository;

        public UserController(IUserRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_repository.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _repository.GetById(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost]
        public IActionResult Create(User user)
        {
            var newUser = _repository.Create(user);
            return CreatedAtAction(nameof(GetById), new { id = newUser.Id }, newUser);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, User user)
        {
            var existing = _repository.GetById(id);
            if (existing == null) return NotFound();

            user.Id = id;
            return Ok(_repository.Update(user));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (!_repository.Delete(id))
                return NotFound();

            return NoContent();
        }
    }
}
