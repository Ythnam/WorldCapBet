using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorldCapBet.ModelDTO
{
    public class PronosticDTO
    {
        public int Id { get; set; }

        public string IdUser { get; set; }

        public int IdMatch { get; set; }

        public int ScoreTeam1 { get; set; }

        public int ScoreTeam2 { get; set; }

        public int Scoring { get; set; }
    }
}
