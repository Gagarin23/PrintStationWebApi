using System.ComponentModel.DataAnnotations;

namespace PrintStationWebApi.Models.DataBase
{
    public class User
    {
        [Key]
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }

        public User()
        {

        }

        public User(string login, string passwordHash)
        {
            Login = login;
            PasswordHash = passwordHash;
        }
    }
}
