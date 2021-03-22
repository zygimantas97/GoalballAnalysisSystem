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
            CreateMap<CreateGamePlayerRequest, GamePlayer>();
            CreateMap<GameRequest, Game>();
            CreateMap<PlayerRequest, Player>();
            CreateMap<ProjectionRequest, Projection>();
            CreateMap<TeamPlayerRequest, TeamPlayer>();
            CreateMap<TeamRequest, Team>();
            CreateMap<UpdateGamePlayerRequest, GamePlayer>();
        }
    }
}
