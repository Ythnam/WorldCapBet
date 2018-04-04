using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WorldCapBet.Model;

namespace WorldCapBet.Models
{
    public class WorldCapBetContext : DbContext
    {
        public WorldCapBetContext (DbContextOptions<WorldCapBetContext> options)
            : base(options)
        {
        }

        public DbSet<WorldCapBet.Model.Match> Match { get; set; }

        public DbSet<WorldCapBet.Model.Pronostic> Pronostic { get; set; }

        public DbSet<WorldCapBet.Model.User> User { get; set; }
    }
}
