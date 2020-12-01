using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Rpg_game.Data;
using Rpg_game.Dtos;
using Rpg_game.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Rpg_game.Services
{
    public class CharacterRepository : ICharacterRepository
    {
        private readonly AppDbContext _ctx;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CharacterRepository(AppDbContext ctx, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _ctx = ctx;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public int GetUserId()
        {
            var id = int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            return id;
        }

        public async Task<Response<List<GetCharacterDto>>> GetAllCharacters()
        {
            var characters = await _ctx.Characters.Where(c => c.User.Id == GetUserId()).Select(a => _mapper.Map<GetCharacterDto>(a)).ToListAsync();
            var response = new Response<List<GetCharacterDto>>();
            response.Data = characters;
            return response;
        }

        public Task<Response<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updateCharacter)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            var characterToAdd = _mapper.Map<Character>(newCharacter);
            await _ctx.Characters.AddAsync(characterToAdd);
            await _ctx.SaveChangesAsync();
            var userCharacters = _ctx.Characters.Where(c => c.Id == GetUserId()).ToList();
            var response = new Response<List<GetCharacterDto>>();
            response.Data = userCharacters.Select(a => _mapper.Map<GetCharacterDto>(a)).ToList();
            return response;

        }

        public Task<Response<GetCharacterDto>> GetCharacter(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Response<List<GetCharacterDto>>> DeleteCharacter()
        {
            throw new NotImplementedException();
        }

    }
}
