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
            CreateMap<Projection, ProjectionResponse>();

            CreateMap<GamePlayer, GamePlayerResponse>()
                .ForMember(dest => dest.Game, opt =>
                    opt.MapFrom(src => new GameResponse
                    {
                        Id = src.Game.Id,
                        Title = src.Game.Title,
                        Date = src.Game.Date,
                        Comment = src.Game.Comment,
                        HomeTeamId = src.Game.HomeTeamId,
                        GuestTeamId = src.Game.GuestTeamId,
                        HomeTeam = src.Game.HomeTeam == null ? null : new TeamResponse
                        {
                            Id = src.Game.HomeTeam.Id,
                            Name = src.Game.HomeTeam.Name,
                            Country = src.Game.HomeTeam.Country,
                            Description = src.Game.HomeTeam.Description,
                            TeamPlayers = null
                        },
                        GuestTeam = src.Game.GuestTeam == null ? null : new TeamResponse
                        {
                            Id = src.Game.GuestTeam.Id,
                            Name = src.Game.GuestTeam.Name,
                            Country = src.Game.GuestTeam.Country,
                            Description = src.Game.GuestTeam.Description,
                            TeamPlayers = null
                        }
                    }))
                .ForMember(dest => dest.TeamPlayer, opt =>
                    opt.MapFrom(src => new TeamPlayerResponse
                    {
                        TeamId = src.TeamPlayer.TeamId,
                        PlayerId = src.TeamPlayer.PlayerId,
                        Number = src.TeamPlayer.Number,
                        RoleId = src.TeamPlayer.RoleId,
                        Team = null,
                        Player = new PlayerResponse
                        {
                            Id = src.TeamPlayer.Player.Id,
                            Name = src.TeamPlayer.Player.Name,
                            Surname = src.TeamPlayer.Player.Surname,
                            Country = src.TeamPlayer.Player.Country,
                            Description = src.TeamPlayer.Player.Description,
                            PlayerTeams = null
                        }
                    }));
            CreateMap<Game, GameResponse>()
                .ForMember(dest => dest.HomeTeam, opt =>
                    opt.MapFrom(src => src.HomeTeam == null ? null : new TeamResponse
                    {
                        Id = src.HomeTeam.Id,
                        Name = src.HomeTeam.Name,
                        Country = src.HomeTeam.Country,
                        Description = src.HomeTeam.Description,
                        TeamPlayers = src.HomeTeam.TeamPlayers.Select(tp => new TeamPlayerResponse
                        {
                            TeamId = tp.TeamId,
                            PlayerId = tp.PlayerId,
                            Number = tp.Number,
                            RoleId = tp.RoleId,
                            Team = null,
                            Player = new PlayerResponse
                            {
                                Id = tp.Player.Id,
                                Name = tp.Player.Name,
                                Surname = tp.Player.Surname,
                                Country = tp.Player.Country,
                                Description = tp.Player.Description,
                                PlayerTeams = null
                            }
                        })
                    }))
                .ForMember(dest => dest.GuestTeam, opt =>
                    opt.MapFrom(src => src.GuestTeam == null ? null : new TeamResponse
                    {
                        Id = src.GuestTeam.Id,
                        Name = src.GuestTeam.Name,
                        Country = src.GuestTeam.Country,
                        Description = src.GuestTeam.Description,
                        TeamPlayers = src.GuestTeam.TeamPlayers.Select(tp => new TeamPlayerResponse
                        {
                            TeamId = tp.TeamId,
                            PlayerId = tp.PlayerId,
                            Number = tp.Number,
                            RoleId = tp.RoleId,
                            Team = null,
                            Player = new PlayerResponse
                            {
                                Id = tp.Player.Id,
                                Name = tp.Player.Name,
                                Surname = tp.Player.Surname,
                                Country = tp.Player.Country,
                                Description = tp.Player.Description,
                                PlayerTeams = null
                            }
                        })
                    }));
            CreateMap<Player, PlayerResponse>()
                .ForMember(dest => dest.PlayerTeams, opt =>
                    opt.MapFrom(src => src.PlayerTeams.Select(tp => new TeamPlayerResponse
                    {
                        TeamId = tp.TeamId,
                        PlayerId = tp.PlayerId,
                        Number = tp.Number,
                        RoleId = tp.RoleId,
                        Team = new TeamResponse
                        {
                            Id = tp.Team.Id,
                            Name = tp.Team.Name,
                            Country = tp.Team.Country,
                            Description = tp.Team.Description,
                            TeamPlayers = null
                        },
                        Player = null
                    })));
            CreateMap<TeamPlayer, TeamPlayerResponse>()
                .ForMember(dest => dest.Team, opt =>
                    opt.MapFrom(src => new TeamResponse
                    {
                        Id = src.Team.Id,
                        Name = src.Team.Name,
                        Country = src.Team.Country,
                        Description = src.Team.Description,
                        TeamPlayers = null
                    }))
                .ForMember(dest => dest.Player, opt =>
                    opt.MapFrom(src => new PlayerResponse
                    {
                        Id = src.Player.Id,
                        Name = src.Player.Name,
                        Surname = src.Player.Surname,
                        Country = src.Player.Country,
                        Description = src.Player.Description,
                        PlayerTeams = null
                    }));
            CreateMap<Team, TeamResponse>()
                .ForMember(dest => dest.TeamPlayers, opt =>
                    opt.MapFrom(src => src.TeamPlayers.Select(tp => new TeamPlayerResponse
                    {
                        TeamId = tp.TeamId,
                        PlayerId = tp.PlayerId,
                        Number = tp.Number,
                        RoleId = tp.RoleId,
                        Team = null,
                        Player = new PlayerResponse
                        {
                            Id = tp.Player.Id,
                            Name = tp.Player.Name,
                            Surname = tp.Player.Surname,
                            Country = tp.Player.Country,
                            Description = tp.Player.Description,
                            PlayerTeams = null
                        }
                    })));
        }
    }
}
