using Microsoft.EntityFrameworkCore;
using Rpg_game.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rpg_game.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Character> Characters { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
