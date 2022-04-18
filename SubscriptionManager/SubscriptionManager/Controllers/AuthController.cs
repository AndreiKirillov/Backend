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

        public AuthController(SubscriptionManagerContext context)
        {
            _context = context;
        }

        private bool CheckEmail(string email)
        {
            if (!(email.Contains("@gmail.com") || email.Contains("@yandex.ru") || email.Contains("@mail.ru") ||
                email.Contains("@asugubkin.ru") || 
                (email.Contains("@") && email.Contains(".com")) ||
                (email.Contains("@") && email.Contains(".ru"))
                ))
                return false;

            return true;
        }

        //private async Task<ActionResult> AuthDataIsOK(AuthData request)
        //{
        //    if (_context.User.Any())
        //    {
        //        if (_context.User.FirstOrDefault(u => u.Login == request.Login) != null)
        //        {                                                               // Если уже есть юзер с таким логином
        //            return BadRequest("User with this login already exists!");
        //        }
        //        if (_context.User.FirstOrDefault(u => u.Email == request.Email) != null)
        //        {                                                               // Если уже есть юзер с такой почтой
        //            return BadRequest("User with this E-mail already exists!");
        //        }
        //    }

        //    if (request.Password.Length < 8)
        //        return BadRequest("The password is too short!");

        //    return Ok();
        //}

        [HttpPost("registration")]    // Регистрация пользователя
        public async Task<ActionResult<User>> Register(AuthData request)
        {
            if(_context.User.Any())
            {
                if (_context.User.FirstOrDefault(u => u.Login == request.Login) != null)
                {                                                               // Если уже есть юзер с таким логином
                    return BadRequest("User with this login already exists!");
                }
                if (_context.User.FirstOrDefault(u => u.Email == request.Email) != null)
                {                                                               // Если уже есть юзер с такой почтой
                    return BadRequest("User with this E-mail already exists!");
                }
            }

            if(request.Password.Length < 8)                        
                return BadRequest("The password is too short!");
            if(!CheckEmail(request.Email))
                return BadRequest("Email is incorrect!");

            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);  // шифруем пароль

            User new_user = new User();            // создаем юзера
            new_user.Login = request.Login;
            new_user.PasswordHash = passwordHash;
            new_user.PasswordSalt = passwordSalt;
            new_user.Email = request.Email;
            _context.User.AddAsync(new_user);          // добавляем зарегистрированного юзера в бд
            await _context.SaveChangesAsync();

            return Ok(new_user);
        }

        [HttpPost("login")]              // Вход юзера в систему
        public async Task<ActionResult<string>> Login(AuthData request)
        {
            if (!_context.User.Any())
                return "no elements!";

            var user = _context.User.FirstOrDefault(u => u.Login == request.Login);  // Идентификация юзера
            if (user == null)
            {
                return BadRequest("User not found!");
            }

            if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt)) // Аутентификация
            {
                return BadRequest("Wrong password!");
            }

            string token = CreateToken(user);
            return Ok(token);
        }

        // Функция создания jwt токена
        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Login)
            };
            var key = Auth.AuthOptions.GetSymmetricSecurityKey();

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

                return jwt;
        }
        
        // Функция шифрования пароля
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        // Функция проверки пароля
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