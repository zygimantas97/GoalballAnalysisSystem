using GoalballAnalysisSystem.API.Contracts.V1.Responses;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.API.Examples.Responses
{
    public class PlayerResponseExample : IExamplesProvider<PlayerResponse>
    {
        public PlayerResponse GetExamples()
        {
            return new PlayerResponse
            {
                Id = 1,
                Name = "Povilas",
                Surname = "Povilaitis",
                Country = "LTU",
                Description = "Very good player",
                PlayerTeams = new List<TeamPlayerResponse>()
                {
                    new TeamPlayerResponse
                    {
                        TeamId = 1,
                        PlayerId = 1,
                        Number = 1,
                        RoleId = 1,
                        Team = new TeamResponse
                        {
                            Id = 1,
                            Name = "Lithuania",
                            Country = "LTU",
                            Description = "Very good team",
                            TeamPlayers = null
                        },
                        Player = null,
                        Role = new PlayerRoleResponse
                        {
                            Id = 1,
                            Name = "LeftStriker"
                        }
                    }
                }
            };
        }
    }
}
