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
            var playersService = new PlayersService(identityService);
            try
            {
                await identityService.LoginAsync("user@gas.com", "Password123!");
                var createResponse = await playersService.CreatePlayerAsync(new PlayerRequest
                {
                    Name = "Simas",
                    Surname = "Simaitis",
                    Country = "GER",
                    Description = "Good player"
                });
                var getResponse = await playersService.GetPlayerAsync(createResponse.Id);
                Console.WriteLine(getResponse.Name);
                await playersService.UpdatePlayerAsync(getResponse.Id, new PlayerRequest
                {
                    Name = "Simas update",
                    Surname = "Simaitis",
                    Country = "GER",
                    Description = "Good player"
                });
                var deleteResponse = await playersService.DeletePlayerAsync(createResponse.Id);
                Console.WriteLine(deleteResponse.Name);

                var getAllResponse = await playersService.GetPlayersAsync();
                Console.WriteLine(getAllResponse.Count);
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
