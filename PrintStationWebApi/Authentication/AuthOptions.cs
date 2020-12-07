using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace PrintStationWebApi.Authentication
{
    public class AuthOptions
    {
        const string Key = "xcgtk4r84vgjmt67kmr4u49xdl5";   // ключ для шифрации
        public const int Lifetime = 5; // время жизни токена - 1 минута
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
        }
    }
}
