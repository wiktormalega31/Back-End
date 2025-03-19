using LoginAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace MyApi.Controllers
{
    [ApiController]
    [Route("api/Users")]
    public class UsersController : ControllerBase
    {
        private static List<User> _Users = new() { };

        private const string API_KEY = "1234"; // 🔹 Ustaw poprawny klucz API

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_Users);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var user = _Users.FirstOrDefault(p => p.Id == id);
            return user is not null ? Ok(user) : NotFound();
        }

        [HttpPost]
        public IActionResult Create(
            [FromBody] User newUser,
            [FromHeader(Name = "X-API-KEY")] string apiKey
        )
        {
            if (apiKey != API_KEY)
                return Unauthorized();

            if (_Users.Any())
                newUser.Id = _Users.Max(p => p.Id) + 1;
            else
                newUser.Id = 1;

            // 🔹 Hashowanie hasła przed zapisaniem
            newUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newUser.PasswordHash);

            // 🔹 Automatyczne ustawienie daty utworzenia
            newUser.Created = DateTime.UtcNow;

            _Users.Add(newUser);
            return CreatedAtAction(nameof(GetById), new { id = newUser.Id }, newUser);
        }

        [HttpPut("{id}")]
        public IActionResult Update(
            int id,
            [FromBody] User updatedUser,
            [FromHeader(Name = "X-API-KEY")] string apiKey
        )
        {
            if (apiKey != API_KEY)
                return Unauthorized();

            var user = _Users.FirstOrDefault(p => p.Id == id);
            if (user is null)
                return NotFound();

            user.Name = updatedUser.Name;
            user.Email = updatedUser.Email;

            // 🔹 Aktualizacja zahashowanego hasła
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(updatedUser.PasswordHash);

            return Ok(user);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id, [FromHeader(Name = "X-API-KEY")] string apiKey)
        {
            if (apiKey != API_KEY)
                return Unauthorized();

            var user = _Users.FirstOrDefault(p => p.Id == id);
            if (user is null)
                return NotFound();

            _Users.Remove(user);
            return NoContent();
        }
    }
}
