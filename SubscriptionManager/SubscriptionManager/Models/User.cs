using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using SubscriptionManager.Data;
using SubscriptionManager.Services;

namespace SubscriptionManager.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Login { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        public List<Subscription> Subs { get; set; }

        public User()
        {
            Id = 0;
            Login = string.Empty;
            PasswordHash = null;
            PasswordSalt = null;
            FirstName = string.Empty;
            LastName = string.Empty;
            Email = string.Empty;
            Subs = new List<Subscription>(0);
        }

        public User(AuthData auth_data)
        {
            Id = 0;
            Login = auth_data.Login;

            AuthServices.CreatePasswordHash(auth_data.Password, out byte[] passwordHash, out byte[] passwordSalt);  // шифруем пароль
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
            
            FirstName = string.Empty;
            LastName = string.Empty;
            Email = auth_data.Email;
            Subs = new List<Subscription>(0);
        }
    }
}
