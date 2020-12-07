﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrintStationWebApi.Models.DataBase
{
    public class User
    {
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