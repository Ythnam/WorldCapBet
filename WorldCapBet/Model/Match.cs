using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WorldCapBet.Model
{
    public class Match
    {
        public int Id { get; set; }

        public string Team1 { get; set; }

        public string Team2 { get; set; }

        public int ScoreTeam1 { get; set; }

        public int ScoreTeam2 { get; set; }

        public DateTime Date { get; set; }
    }
}
