using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace PrintStationWebApi.Authentication
{
    public class AuthOptions
    {
        public const string Issuer = "MyAuthServer";
        public const string Audience = "MyAuthClient";
        const string Key = "xcgtk4r84vgjmt67kmr4u49xdl5";
        public const int Lifetime = 5;
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
        }
    }
}
