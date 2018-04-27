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

            match.ScoreTeam1 = null;
            match.ScoreTeam2 = null;

            context.Match.Add(match);
            context.SaveChanges(); // needed to have the ID of current match object on the following code


            // Create pronostic for each user when a match is created
            foreach(User user in context.User)
            {
                context.Pronostic.Add(new Pronostic
                {
                    IdMatch = match.Id,
                    IdUser = user.Id
                });
            }
            context.SaveChanges();

            return match;
        }

        public void Delete(int id)
        {
            var match = context.Match.Find(id);
            if (match != null)
            {
                var pronostics = context.Pronostic.Where(p => p.IdMatch == match.Id);
                foreach (Pronostic p in pronostics)
                    context.Remove(p);
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

            // After the save because score had to been added to the database
            this.Scoring(matchParam.Id);
        }

        /// <summary>
        /// Scoring pronostic
        /// </summary>
        /// <param name="id"> is the ID of the the match</param>
        private void Scoring(int idMatch)
        {
            //Select pronostic all pronostic done for a match
            var pronostics = context.Pronostic.Where(p => p.IdMatch == idMatch);
            var match = context.Match.Where(m => m.Id == idMatch).SingleOrDefault();

            foreach (Pronostic pronostic in pronostics)
            {
                if (!IsPronosticRight(pronostic, match))
                    pronostic.Scoring = 0;
                else
                {
                    if (pronostic.ScoreTeam1 == match.ScoreTeam1 && pronostic.ScoreTeam2 == match.ScoreTeam2)
                        pronostic.Scoring = 4;
                    else
                    {
                        if (pronostic.ScoreTeam1 == match.ScoreTeam1 || pronostic.ScoreTeam2 == match.ScoreTeam2)
                            pronostic.Scoring = 2;
                        else
                            pronostic.Scoring = 1;
                    }
                }
                context.Pronostic.Update(pronostic);
            }
            context.SaveChanges();
        }

        private bool IsPronosticRight(Pronostic pronostic, Match match)
        {
            if (StateTeam1(pronostic) == StateTeam1(match))
                return true;
            return false;
        }

        private enum MatchStatus { Win = 1, Equal = 0, Loose = -1 };
        private MatchStatus StateTeam1(Match match)
        {
            if (match.ScoreTeam1 > match.ScoreTeam2)
                return MatchStatus.Win;
            else
            {
                if (match.ScoreTeam1 == match.ScoreTeam2)
                    return MatchStatus.Equal;
                else
                    return MatchStatus.Loose;
            }
        }

        private MatchStatus StateTeam1(Pronostic pronostic)
        {
            if (pronostic.ScoreTeam1 > pronostic.ScoreTeam2)
                return MatchStatus.Win;
            else
            {
                if (pronostic.ScoreTeam1 == pronostic.ScoreTeam2)
                    return MatchStatus.Equal;
                else
                    return MatchStatus.Loose;
            }
        }
    }
}
