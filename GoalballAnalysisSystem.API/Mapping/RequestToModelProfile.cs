using AutoMapper;
using GoalballAnalysisSystem.API.Contracts.V1.Requests;
using GoalballAnalysisSystem.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.API.Mapping
{
    public class RequestToModelProfile : Profile
    {
        public RequestToModelProfile()
        {
            CreateMap<PlayerRequest, Player>();
            CreateMap<TeamRequest, Team>();
            CreateMap<TeamPlayerRequest, TeamPlayer>();
            CreateMap<GameRequest, Game>();
            CreateMap<CreateGamePlayerRequest, GamePlayer>();
            CreateMap<UpdateGamePlayerRequest, GamePlayer>();
        }
    }
}
