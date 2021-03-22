using GoalballAnalysisSystem.API.Contracts.V1.Responses;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.API.Examples.Responses
{
    public class GameResponseExample : IExamplesProvider<GameResponse>
    {
        public GameResponse GetExamples()
        {
            return new GameResponse
            {
                Id = 1,
                Title = "Rio 2016 Final",
                Date = DateTime.Now,
                Comment = "Very hard game",
                HomeTeamId = 1,
                GuestTeamId = 2,
                HomeTeam = new TeamResponse
                {
                    Id = 1,
                    Name = "Lithuania",
                    Country = "LTU",
                    Description = "Very good team",
                    TeamPlayers = new List<TeamPlayerResponse>()
                    {
                        new TeamPlayerResponse
                        {
                            TeamId = 1,
                            PlayerId = 1,
                            Number = 1,
                            RoleId = 1,
                            Team = null,
                            Player = new PlayerResponse
                            {
                                Id = 1,
                                Name = "Povilas",
                                Surname = "Povilaitis",
                                Country = "LTU",
                                Description = "Very good player",
                                PlayerTeams = null
                            }
                        }
                    }
                },
                GuestTeam = new TeamResponse
                {
                    Id = 2,
                    Name = "Germany",
                    Country = "GER",
                    Description = "Very good team",
                    TeamPlayers = new List<TeamPlayerResponse>()
                    {
                        new TeamPlayerResponse
                        {
                            TeamId = 2,
                            PlayerId = 2,
                            Number = 1,
                            RoleId = 1,
                            Team = null,
                            Player = new PlayerResponse
                            {
                                Id = 2,
                                Name = "Petras",
                                Surname = "Petraitis",
                                Country = "GER",
                                Description = "Very good player",
                                PlayerTeams = null
                            }
                        }
                    }
                }
            };
        }
    }
}
