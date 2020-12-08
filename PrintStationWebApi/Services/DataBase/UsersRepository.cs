using Microsoft.EntityFrameworkCore;
using PrintStationWebApi.Models.DataBase;
using System;
using System.Data;
using System.Runtime.CompilerServices;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

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
                return new NotFoundResult();

            try
            {
                return await _db.Users.FindAsync(username);
            }
            catch (Exception e)
            {
                return new ObjectResult(e.Message){StatusCode = 500};
            }
        }

        public async Task<ActionResult> AddUserAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException();

            try
            {
                await _db.Users.AddAsync(user);
                await _db.SaveChangesAsync();
                return new OkObjectResult("User has been added.");
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.Message){StatusCode = 500};
            }
        }

        public async Task<ActionResult> UpdateUserAsync(User user)
        {
            if(user == null)
                throw new ArgumentNullException(nameof(user));

            try
            {
                _db.Users.Update(user);
                await _db.SaveChangesAsync();
                return new OkObjectResult("Role has been updated.");
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.Message){StatusCode = 500};
            }
        }

        public async Task<ActionResult> RemoveUserAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return new NotFoundResult();

            try
            {
                var user = await _db.Users.FindAsync();
                _db.Users.Remove(user);
                await _db.SaveChangesAsync();
                return new OkResult();
            }
            catch (Exception e)
            {
                return new ObjectResult(e.Message){StatusCode = 500};
            }
        }
    }
}
