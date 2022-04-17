using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace SubscriptionManager.Auth
{
    public class AuthOptions
    {
        public const string Issuer = "KirillovTrustCompany"; // издатель токена
        public const string Audience = "DearClient"; // потребитель токена
        const string Key = "computingverystrongpasswordforgenioushackers";   // ключ для шифрации
        public const int LifetimeInHours = 1; // время жизни токена - 1 минута
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
        }
    }
}
