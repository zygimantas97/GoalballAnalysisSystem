using GoalballAnalysisSystem.API.Contracts.V1.Requests;
using GoalballAnalysisSystem.WPF.Services;
using System;
using System.Threading.Tasks;

namespace ApiServiceTest
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
            Console.ReadLine();
        }

        static async Task MainAsync()
        {
            var identityService = new IdentityService();
            var teamsService = new TeamsService(identityService);
            var playersService = new PlayersService(identityService);
            var teamPlayersService = new TeamPlayersService(identityService);
            try
            {
                await identityService.LoginAsync("user@gas.com", "Password123!");
                var teamResponse = await teamsService.CreateTeamAsync(new TeamRequest
                {
                    Name = "USA",
                    Country = "USA",
                    Description = "Very good team"
                });
                var playerResponse = await playersService.CreatePlayerAsync(new PlayerRequest
                {
                    Name = "Simas",
                    Surname = "Simaitis",
                    Country = "USA",
                    Description = "Good player"
                });
                var createTeamPlayerResponse = await teamPlayersService.CreateTeamPlayerAsync(
                    teamResponse.Id,
                    playerResponse.Id,
                    new TeamPlayerRequest
                    {
                        RoleId = 1,
                        Number = 11
                    });
                var getTeamPlayerResponse = await teamPlayersService.GetTeamPlayerAsync(
                    createTeamPlayerResponse.TeamId,
                    createTeamPlayerResponse.PlayerId);
                Console.WriteLine(getTeamPlayerResponse.Number);
                var getTeamPlayerByTeamResponse = await teamPlayersService.GetTeamPlayersByTeamAsync(teamResponse.Id);
                Console.WriteLine(getTeamPlayerByTeamResponse.Count);
                var getTeamPlayerByPlayerResponse = await teamPlayersService.GetTeamPlayersByPlayerAsync(playerResponse.Id);
                Console.WriteLine(getTeamPlayerByPlayerResponse.Count);
                await teamPlayersService.UpdateTeamPlayerAsync(
                    createTeamPlayerResponse.TeamId,
                    createTeamPlayerResponse.PlayerId,
                    new TeamPlayerRequest
                    {
                        RoleId = 1,
                        Number = 111
                    });
                var deleteTeamPlayerResponse = await teamPlayersService.DeleteTeamPlayerAsync(
                    getTeamPlayerResponse.TeamId,
                    getTeamPlayerResponse.PlayerId);
                Console.WriteLine(deleteTeamPlayerResponse.Number);
                Console.WriteLine("Ok");
            }
            catch(Exception e)
            {
                Console.WriteLine("Error");
                Console.WriteLine(e.Message);
            }
        }
    }
}
