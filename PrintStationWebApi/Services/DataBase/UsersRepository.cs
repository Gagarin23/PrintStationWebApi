using Microsoft.EntityFrameworkCore;
using PrintStationWebApi.Models.DataBase;
using System;
using System.Data;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PrintStationWebApi.Logger;

namespace PrintStationWebApi.Services.DataBase
{
    public interface IUsersRepository
    {
        Task<ActionResult<User>> GetUserAsync(string username);
        Task<ActionResult> AddUserAsync(User user);
        public Task<ActionResult> UpdateUserAsync(User user);
        public Task<ActionResult> RemoveUserAsync(string username);
    }

    public class UsersRepository : IUsersRepository
    {
        private readonly PrintStationContext _db;

        public UsersRepository(PrintStationContext db)
        {
            _db = db;
        }

        public async Task<ActionResult<User>> GetUserAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return new BadRequestObjectResult(nameof(username) + " is empty or null.");

            var user = await _db.Users.FindAsync(username);
            if (user != null)
                return user;

            return new NotFoundResult();
        }

        public async Task<ActionResult> AddUserAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException();

            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
            return new OkObjectResult("User has been added.");
        }

        public async Task<ActionResult> UpdateUserAsync(User user)
        {
            if(user == null)
                throw new ArgumentNullException(nameof(user));

            _db.Users.Update(user);
            await _db.SaveChangesAsync();
            return new OkObjectResult("Role has been updated.");
        }

        public async Task<ActionResult> RemoveUserAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return new NotFoundResult();
            
            var user = new User{Login = username};
            _db.Entry(user).State = EntityState.Deleted;
            await _db.SaveChangesAsync();
            return new OkObjectResult("User has been deleted.");
        }
    }
}
