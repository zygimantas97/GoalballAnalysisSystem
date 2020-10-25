using AutoMapper;
using GoalballAnalysisSystem.API.Contracts.V1.Responses;
using GoalballAnalysisSystem.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.API.Mapping
{
    public class ModelToResponseProfile : Profile
    {
        public ModelToResponseProfile()
        {
            CreateMap<Player, PlayerResponse>();
            CreateMap<Team, TeamResponse>();
            CreateMap<TeamPlayer, TeamPlayerResponse>();
            CreateMap<Game, GameResponse>();
            CreateMap<GamePlayer, GamePlayerResponse>();
        }
    }
}
