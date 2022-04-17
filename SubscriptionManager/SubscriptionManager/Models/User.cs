using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace SubscriptionManager.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Login { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        public ICollection<Subscription> Subs { get; set; }

        public Subscription GetSubscriptionByID(int id)
        {
            return Subs.FirstOrDefault(s => s.Id == id);
        }
    }
}
