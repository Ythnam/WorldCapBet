using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WorldCapBet.Model
{
    public class Pronostic
    {
        public int Id { get; set; }

        public string IdUser { get; set; }

        public int IdMatch { get; set; }

        public int ScoreTeam1 { get; set; }

        public int ScoreTeam2 { get; set; }

        public int Scoring { get; set; }
    }
}
