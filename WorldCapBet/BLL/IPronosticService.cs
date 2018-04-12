using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorldCapBet.Model;

namespace WorldCapBet.BLL
{
    public interface IPronosticService
    {
        IEnumerable<Pronostic> GetAll();
        Pronostic GetById(int id);
        Pronostic Create(Pronostic pronostic);
        void Update(Pronostic pronosticParam);
        void Delete(int id);
    }
}
