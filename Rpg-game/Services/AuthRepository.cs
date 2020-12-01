using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Rpg_game.Data;
using Rpg_game.Helpers;
using Rpg_game.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rpg_game.Services
{
    public class AuthRepository : IAuthRepository
    {
        private readonly AppDbContext _ctx;
        private readonly IConfiguration _config;

        public AuthRepository(AppDbContext ctx, IConfiguration config)
        {
            _ctx = ctx;
            _config = config;
        }
        public async Task<Response<string>> Login(string username, string password)
        {
            var response = new Response<string>();
            try
            {
                var user =  await _ctx.Users.FirstOrDefaultAsync(c => c.Username.ToLower() == username.ToLower());
                if(user == null)
                {
                    response.Message = "User not found.";
                    response.Success = false;
                    return response;
                }
                var passwordCheck = VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt);
                if (!passwordCheck)
                {
                    response.Success = false;
                    response.Message = "Invalid username or password";
                    return response;
                }
                response.Data = JwtTokenConfig.GetToken(user, _config);
                response.Message = "Logged in successfully";
            }
            catch (Exception ex)
            {

                response.Message = ex.Message;
                return response;
            }
            return response;
        }

        public async Task<Response<int>> Register(User user, string password)
        {
            var response = new Response<int>();
            if (UserExists(user.Username))
            {
                response.Success = false;
                response.Message = "User already exists.";
                return response;
            }
            EncryptPassword(password, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            await _ctx.Users.AddAsync(user);
            await _ctx.SaveChangesAsync();
            response.Data = user.Id;
            return response;
        }

        public bool UserExists(string username)
        {
            if (_ctx.Users.Any(c => c.Username.ToLower() == username.ToLower())) return true;
            return false;
        }
        private void EncryptPassword(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i]) return false;

                }
                return true;
            }
        }

    }
}
