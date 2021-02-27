using GoalballAnalysisSystem.API.Contracts.V1.Responses;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.API.Examples.Responses
{
    public class GamePlayerResponseExample : IExamplesProvider<GamePlayerResponse>
    {
        public GamePlayerResponse GetExamples()
        {
            return new GamePlayerResponse
            {
                Id = 1,
                StartTime = DateTime.Now.AddMinutes(-10),
                EndTime = DateTime.Now,
                TeamId = 1,
                PlayerId = 1,
                GameId = 1,
                Game = new GameResponse
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
                        TeamPlayers = null
                    },
                    GuestTeam = new TeamResponse
                    {
                        Id = 2,
                        Name = "Germany",
                        Country = "GER",
                        Description = "Very good team",
                        TeamPlayers = null
                    }
                },
                TeamPlayer = new TeamPlayerResponse
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
            };
        }
    }
}
