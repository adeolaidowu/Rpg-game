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
        Task<Response<IEnumerable<GetCharacterDto>>> GetAllCharacters();

        Task<Response<GetCharacterDto>> GetCharacter(int id);
        Task<Response<IEnumerable<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter);
        Task<Response<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updateCharacter);
        Task<Response<IEnumerable<GetCharacterDto>>> DeleteCharacter(int id);

        
    }
}
