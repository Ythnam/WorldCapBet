using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorldCapBet.ApplicationException;
using WorldCapBet.Data;
using WorldCapBet.Model;

namespace WorldCapBet.BLL
{
    public class PronosticService : IPronosticService
    {
        private WorldCapBetContext context;

        public PronosticService(WorldCapBetContext _context)
        {
            this.context = _context;
        }

        public Pronostic Create(Pronostic pronostic)
        {
            context.Pronostic.Add(pronostic);
            context.SaveChanges();

            return pronostic;
        }

        public void Delete(int id)
        {
            var pronostic = context.Pronostic.Find(id);
            if (pronostic != null)
            {
                context.Pronostic.Remove(pronostic);
                context.SaveChanges();
            }
        }

        public IEnumerable<Pronostic> GetAll()
        {
            return context.Pronostic;
        }

        public Pronostic GetById(int id)
        {
            return context.Pronostic.Find(id);
        }

        public void Update(Pronostic pronosticParam)
        {
            var pronostic = context.Pronostic.Find(pronosticParam.Id);

            if (pronostic == null)
                throw new AppException("Pronostic not found");

            pronostic.ScoreTeam1 = pronosticParam.ScoreTeam1;
            pronostic.ScoreTeam2 = pronosticParam.ScoreTeam2;

            context.Pronostic.Update(pronostic);
            context.SaveChanges();
        }
    }
}
