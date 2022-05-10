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
using SubscriptionManager.Services;
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
            if(!AuthServices.CheckEmail(request.Email))
                return BadRequest("Email is incorrect!");

            User new_user = new User(request);            // создаем юзера
           
            await _context.User.AddAsync(new_user);          // добавляем зарегистрированного юзера в бд
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

            if (!AuthServices.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt)) // Аутентификация
            {
                return BadRequest("Wrong password!");
            }

            string token = AuthServices.CreateToken(user);
            return Ok(token);
        }
    }
}