﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PrintStationWebApi.Authentication;
using PrintStationWebApi.Services.DataBase;

namespace PrintStationWebApi.Services.BL
{
    public interface IAuthenticationService
    {
        string Token(string username, string password);
    }

    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUsersRepository _usersRepository;

        public AuthenticationService(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public string Token(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                throw new ArgumentNullException("username or password null or empty.");

            var identity = GetIdentity(username, password);
            if (identity == null)
                throw new AuthenticationException();
 
            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(AuthOptions.Lifetime)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
 
            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name
            };

            return JsonSerializer.Serialize(response);
        }
 
        private ClaimsIdentity GetIdentity(string username, string password)
        {
            var user = _usersRepository.GetUserAsync(username, password).Result;
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.ToString())
                };
                ClaimsIdentity claimsIdentity =
                    new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                        ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }
 
            return null;
        }
    }
}
