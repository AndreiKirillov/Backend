using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using SubscriptionManager.Models;
using SubscriptionManager.Auth;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;
using SubscriptionManager.Data;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace SubscriptionManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public static User user = new User();
        private readonly SubscriptionManagerContext _context;
        //private readonly IConfiguration _configuration;
       // private readonly IUserService _userService;

        public AuthController(SubscriptionManagerContext context)
        {
            _context = context;
        }

        //[HttpGet, Authorize]
        //public ActionResult<string> GetMe()
        //{
        //    var userName = _userService.GetMyName();
        //    return Ok(userName);
        //}

        [HttpPost("register")]    // Регистрация пользователя
        public async Task<ActionResult<User>> Register(AuthData request)
        {
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);  // шифруем пароль

            User new_user = new User();            // создаем юзера
            new_user.Login = request.Login;
            new_user.PasswordHash = passwordHash;
            new_user.PasswordSalt = passwordSalt;

            _context.User.Add(new_user);          // добавляем зарегистрированного юзера в бд
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = new_user.Id }, new_user);
        }

        [HttpPost("login")]              // Вход юзера в систему
        public async Task<ActionResult<string>> Login(AuthData request)
        {
            var user = _context.User.First(u => u.Login == request.Login);
            if (user == null)
            {
                return BadRequest("User not found!");
            }

            if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("Wrong password!");
            }

            string token = CreateToken(user);
            return Ok(token);
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}