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
            var gamesService = new GamesService(identityService);
            var gamePlayersService = new GamePlayersService(identityService);
            var projectionsService = new ProjectionsService(identityService);
            var playerRolesService = new PlayerRolesService(identityService);
            try
            {
                await identityService.LoginAsync("user@gas.com", "Password123!");
                var playerRoles = await playerRolesService.GetPlayerRolesAsync();

                //var teamResponse = await teamsService.CreateTeamAsync(new TeamRequest
                //{
                //    Name = "Test team",
                //    Country = "LTU",
                //    Description = "Test team"
                //});
                //var playerResponse = await playersService.CreatePlayerAsync(new PlayerRequest
                //{
                //    Name = "Simas",
                //    Surname = "Simauskas",
                //    Country = "LTU",
                //    Description = "Test"
                //});
                //var teamPlayerResponse = await teamPlayersService.CreateTeamPlayerAsync(
                //    teamResponse.Id,
                //    playerResponse.Id,
                //    new TeamPlayerRequest
                //    {
                //        RoleId = 1,
                //        Number = 1
                //    });
                //var gameResponse = await gamesService.CreateGameAsync(new GameRequest
                //{
                //    Title = "Test",
                //    Comment = "Test",
                //    HomeTeamId = teamResponse.Id,
                //    GuestTeamId = null
                //});
                //var gamePlayerResponse = await gamePlayersService.CreateGamePlayerAsync(new CreateGamePlayerRequest
                //{
                //    StartTime = DateTime.Now.AddSeconds(-10),
                //    EndTime = DateTime.Now,
                //    GameId = gameResponse.Id,
                //    TeamId = teamResponse.Id,
                //    PlayerId = playerResponse.Id
                //});

                //// ----------

                //var createResponse = await projectionsService.CreateProjectionAsync(new ProjectionRequest
                //{
                //    X1 = 1,
                //    Y1 = 1,
                //    X2 = 100,
                //    Y2 = 100,
                //    Speed = 0,
                //    GameId = gameResponse.Id,
                //    DefenseGamePlayerId = null,
                //    OffenseGamePlayerId = gamePlayerResponse.Id
                //});
                //var getResponse = await projectionsService.GetProjectionAsync(createResponse.Id);
                //Console.WriteLine("X1: " + getResponse.X1);

                //var getByGameResponse = await projectionsService.GetProjectionsByGameAsync(gameResponse.Id);
                //Console.WriteLine(getByGameResponse.Count);
                //var getByGamePlayerResponse = await projectionsService.GetProjectionsByGamePlayerAsync(gamePlayerResponse.Id);
                //Console.WriteLine(getByGamePlayerResponse.Count);

                //await projectionsService.UpdateProjectionAsync(getResponse.Id, new ProjectionRequest
                //{
                //    X1 = 50,
                //    Y1 = 50,
                //    X2 = 100,
                //    Y2 = 100,
                //    Speed = 0,
                //    GameId = gameResponse.Id,
                //    DefenseGamePlayerId = null,
                //    OffenseGamePlayerId = gamePlayerResponse.Id
                //});
                //var deleteResponse = await projectionsService.DeleteProjectionAsync(getResponse.Id);
                //Console.WriteLine("X1: " + deleteResponse.X1);

                //Console.WriteLine("Ok");
            }
            catch(Exception e)
            {
                Console.WriteLine("Error");
                Console.WriteLine(e.Message);
            }
        }
    }
}
