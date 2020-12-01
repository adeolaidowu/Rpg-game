using Rpg_game.Dtos;
using Rpg_game.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rpg_game.Services
{
    interface ICharacterRepository
    {
        Task<Response<List<GetCharacterDto>>> GetAllCharacters();

        Task<Response<GetCharacterDto>> GetCharacter(int id);
        Task<Response<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter);
        Task<Response<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updateCharacter);
        Task<Response<List<GetCharacterDto>>> DeleteCharacter();

        
    }
}
