using AutoMapper;
using Rpg_game.Dtos;
using Rpg_game.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rpg_game
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Character, AddCharacterDto>();
        }
    }
}
