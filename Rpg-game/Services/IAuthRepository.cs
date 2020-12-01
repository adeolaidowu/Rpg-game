using Rpg_game.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rpg_game.Services
{
    public interface IAuthRepository
    {
        Task<Response<int>> Register(User user, string password);

        Task<Response<string>> Login(string username, string password);
        bool UserExists(string username);
    }
}
