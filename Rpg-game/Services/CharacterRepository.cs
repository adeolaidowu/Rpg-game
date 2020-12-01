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

        public async Task<Response<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updateCharacter)
        {
            var response = new Response<GetCharacterDto>();
            try
            {
                var character = await _ctx.Characters.FirstOrDefaultAsync(c => c.Id == updateCharacter.Id);
                character.Name = updateCharacter.Name;
                character.HitPoints = updateCharacter.HitPoints;
                character.Strength = updateCharacter.Strength;
                character.Defense = updateCharacter.Defense;
                character.Intelligence = updateCharacter.Intelligence;
                character.Class = updateCharacter.Class;
                _ctx.Update(character);
                await _ctx.SaveChangesAsync();
                response.Data = _mapper.Map<GetCharacterDto>(character);
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = e.Message;
            }
            return response;
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

        public async Task<Response<GetCharacterDto>> GetCharacter(int id)
        {
            var selectedCharacter = await _ctx.Characters.FirstOrDefaultAsync(b => b.Id == id && b.User.Id == GetUserId());
            var response = new Response<GetCharacterDto>();
            response.Data = _mapper.Map<GetCharacterDto>(selectedCharacter);
            return response;

        }

        public async Task<Response<List<GetCharacterDto>>> DeleteCharacter(int Id)
        {
            var response = new Response<List<GetCharacterDto>>();
            try
            {
                var character = await _ctx.Characters.FirstOrDefaultAsync(d => d.Id == Id && d.User.Id == GetUserId());
                if(character != null)
                {
                    _ctx.Remove(character);
                    await _ctx.SaveChangesAsync();
                    response.Data = _ctx.Characters.Where(b => b.User.Id == GetUserId()).Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
                }                
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = e.Message;
            }
            return response;
        }

    }
}
