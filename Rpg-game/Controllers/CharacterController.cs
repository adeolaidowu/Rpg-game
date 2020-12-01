using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rpg_game.Models;

namespace Rpg_game.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CharacterController : ControllerBase
    {
        private static Character knight = new Character();
        public IActionResult Get()
        {
            return Ok(knight);
        }
    }
}
