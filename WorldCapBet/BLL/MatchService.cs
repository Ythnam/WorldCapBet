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
        private WorldCapBetContext _context;

        public MatchService(WorldCapBetContext context)
        {
            _context = context;
        }

        public Match Create(Match match)
        {
            if (_context.Match.Any(x => x.Team1 == match.Team1 || x.Team1 == match.Team2))
            {
                if (_context.Match.Any(x => x.Date == match.Date && x.Team2 == match.Team1 || x.Team2 == match.Team2))
                    throw new AppException("A match between " + match.Team1 + " and " + match.Team2 + " at " + match.Date + " already exist");
            }

            _context.Match.Add(match);
            _context.SaveChanges();

            return match;
        }

        public void Delete(int id)
        {
            var match = _context.Match.Find(id);
            if (match != null)
            {
                _context.Match.Remove(match);
                _context.SaveChanges();
            }
        }

        public IEnumerable<Match> GetAll()
        {
            return _context.Match;
        }

        public Match GetById(int id)
        {
            return _context.Match.Find(id);
        }

        public void UpdateMatch(Match matchParam)
        {
            var match = _context.Match.Find(matchParam.Id);

            if (match == null)
                throw new AppException("Match not found");

            // update match score
            match.Team1 = matchParam.Team1;
            match.Team2 = matchParam.Team2;
            match.Date = matchParam.Date;

            _context.Match.Update(match);
            _context.SaveChanges();
        }

        public void UpdateScore(Match matchParam)
        {
            var match = _context.Match.Find(matchParam.Id);

            if (match == null)
                throw new AppException("Match not found");

            // update match score
            match.ScoreTeam1 = matchParam.ScoreTeam1;
            match.ScoreTeam2 = matchParam.ScoreTeam2;

            _context.Match.Update(match);
            _context.SaveChanges();
        }
    }
}
