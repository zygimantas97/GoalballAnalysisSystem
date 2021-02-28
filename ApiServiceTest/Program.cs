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
            try
            {
                await identityService.LoginAsync("user@gas.com", "Password123!");
                var createResponse = await teamsService.CreateTeamAsync(new TeamRequest
                {
                    Name = "Germany",
                    Country = "GER",
                    Description = "Good team"
                });
                var getResponse = await teamsService.GetTeamAsync(createResponse.Id);
                Console.WriteLine(getResponse.Name);
                await teamsService.UpdateTeamAsync(getResponse.Id, new TeamRequest
                {
                    Name = "Germany update",
                    Country = "GER",
                    Description = "Good team"
                });
                var deleteResponse = await teamsService.DeleteTeamAsync(createResponse.Id);
                Console.WriteLine(deleteResponse.Name);

                var getTeamsResponse = await teamsService.GetTeamsAsync();
                Console.WriteLine(getTeamsResponse.Count);
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
