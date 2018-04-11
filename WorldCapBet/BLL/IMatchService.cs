using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorldCapBet.Model;

namespace WorldCapBet.BLL
{
    public interface IMatchService
    {
        IEnumerable<Match> GetAll();
        Match GetById(int id);
        Match Create(Match match);
        void UpdateMatch(Match matchParam);
        void UpdateScore(Match matchParam);
        void Delete(int id);
    }
}
