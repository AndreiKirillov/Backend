using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using SubscriptionManager.Models;
using SubscriptionManager.Auth;
using System.Security.Cryptography;

namespace SubscriptionManager.Services
{
    public static class AuthServices
    {
        // Функция создания jwt токена
        public static string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Login)
            };
            var key = Auth.AuthOptions.GetSymmetricSecurityKey();

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var now = DateTime.UtcNow;
            var token = new JwtSecurityToken(
                issuer: AuthOptions.Issuer,
                audience: AuthOptions.Audience,
                notBefore: now,
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        // Функция шифрования пароля
        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        // Функция проверки пароля
        public static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        public static bool CheckEmail(string email)
        {
            if (!(email.Contains("@gmail.com") || email.Contains("@yandex.ru") || email.Contains("@mail.ru") ||
                email.Contains("@asugubkin.ru") ||
                (email.Contains("@") && email.Contains(".com")) ||
                (email.Contains("@") && email.Contains(".ru"))
                ))
                return false;

            return true;
        }
    }
}
