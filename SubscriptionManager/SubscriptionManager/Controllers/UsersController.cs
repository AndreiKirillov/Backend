using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SubscriptionManager.Data;
using SubscriptionManager.Models;
using SubscriptionManager.Services;
using System.IdentityModel.Tokens;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;

namespace SubscriptionManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        public static User user = new User();
        private readonly SubscriptionManagerContext _context;

        public UsersController(SubscriptionManagerContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            return await _context.User.ToListAsync();
        }

        // POST: api/Users/forgot_password
        [Authorize]
        [HttpPost("forgot_password")]
        public async Task<IActionResult> RemindPassword(string email)
        {
            var user = _context.User.Where(u => u.Login == User.Identity.Name).FirstOrDefault();
            if (user.Email != email)
                return BadRequest("Wrong E-mail!");

            // Send link to change password to email

            return Ok();
        }

        // PUT: api/Users/change_password
        [Authorize]
        [HttpPut("change_password")]
        public async Task<IActionResult> ChangePassword(string old_password, string new_password)
        {
            var user = _context.User.Where(u => u.Login == User.Identity.Name).FirstOrDefault();

            // Проверяем старый пароль
            if (!AuthServices.VerifyPasswordHash(old_password, user.PasswordHash, user.PasswordSalt))  
                return BadRequest("Wrong E-mail!");

            // Устанавливаем новый
            if (new_password.Length < 8)
                return BadRequest("The password is too short!");

            AuthServices.CreatePasswordHash(new_password, out byte[] passwordHash, out byte[] passwordSalt);  // шифруем пароль

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _context.Entry(user).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }
    }
}
