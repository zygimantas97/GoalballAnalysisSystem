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
            var gamesService = new GamesService(identityService);
            try
            {
                await identityService.LoginAsync("user@gas.com", "Password123!");

                var createResponse = await gamesService.CreateGameAsync(new GameRequest
                {
                    Title = "Test Game",
                    Comment = "Test Game",
                    HomeTeamId = null,
                    GuestTeamId = null
                });
                var getResponse = await gamesService.GetGameAsync(createResponse.Id);
                Console.WriteLine(getResponse.Title);

                var getAllResponse = await gamesService.GetGamesAsync();
                Console.WriteLine(getAllResponse.Count);

                await gamesService.UpdateGameAsync(getResponse.Id, new GameRequest
                {
                    Title = "Test Game updated",
                    Comment = "Test Game",
                    HomeTeamId = null,
                    GuestTeamId = null
                });
                var deleteResponse = await gamesService.DeleteGameAsync(getResponse.Id);
                Console.WriteLine(deleteResponse.Title);

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
