using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WorldCapBet.Model
{
    public class Pronostic
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string IdUser { get; set; }

        [Required]
        public int IdMatch { get; set; }

        public int ScoreTeam1 { get; set; }

        public int ScoreTeam2 { get; set; }

        public int Scoring { get; set; }
    }
}
