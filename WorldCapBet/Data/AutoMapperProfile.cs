using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorldCapBet.Model;
using WorldCapBet.ModelDTO;

namespace WorldCapBet.Data
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDTO>().ReverseMap();

            CreateMap<Match, MatchDTO>();
            CreateMap<MatchDTO, Match>();

            CreateMap<Pronostic, PronosticDTO>();
            CreateMap<PronosticDTO, Pronostic>();
        }
        
    }
}
