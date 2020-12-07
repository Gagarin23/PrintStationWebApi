using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrintStationWebApi.Models.DataBase
{
    public class User
    {
        [Key]
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public Role Role { get; set; }

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
