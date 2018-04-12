using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorldCapBet.ApplicationException;
using WorldCapBet.Data;
using WorldCapBet.Model;

namespace WorldCapBet.BLL
{
    public class MatchService : IMatchService
    {
        private WorldCapBetContext context;

        public MatchService(WorldCapBetContext _context)
        {
            this.context = _context;
        }

        public Match Create(Match match)
        {
            if (context.Match.Any(x => x.Team1 == match.Team1 || x.Team1 == match.Team2))
            {
                if (context.Match.Any(x => x.Date == match.Date && x.Team2 == match.Team1 || x.Team2 == match.Team2))
                    throw new AppException("A match between " + match.Team1 + " and " + match.Team2 + " at " + match.Date + " already exist");
            }

            context.Match.Add(match);
            context.SaveChanges();

            return match;
        }

        public void Delete(int id)
        {
            var match = context.Match.Find(id);
            if (match != null)
            {
                context.Match.Remove(match);
                context.SaveChanges();
            }
        }

        public IEnumerable<Match> GetAll()
        {
            return context.Match;
        }

        public Match GetById(int id)
        {
            return context.Match.Find(id);
        }

        public void UpdateMatch(Match matchParam)
        {
            var match = context.Match.Find(matchParam.Id);

            if (match == null)
                throw new AppException("Match not found");

            // update match score
            match.Team1 = matchParam.Team1;
            match.Team2 = matchParam.Team2;
            match.Date = matchParam.Date;

            context.Match.Update(match);
            context.SaveChanges();
        }

        public void UpdateScore(Match matchParam)
        {
            var match = context.Match.Find(matchParam.Id);

            if (match == null)
                throw new AppException("Match not found");

            // update match score
            match.ScoreTeam1 = matchParam.ScoreTeam1;
            match.ScoreTeam2 = matchParam.ScoreTeam2;

            context.Match.Update(match);
            context.SaveChanges();
        }
    }
}
