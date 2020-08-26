using GoalballAnalysisSystem.Domain.Models;
using GoalballAnalysisSystem.Domain.Services;
using GoalballAnalysisSystem.EntityFramework;
using GoalballAnalysisSystem.EntityFramework.Services;
using System;
using System.Linq;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            IDataService<User> dataService = new DataService<User>(new GoalballAnalysisSystemDbContextFactory());
            var user = dataService.Create(new User { Name = "test2", Role = 1 }).Result;
            user.Name = "updated";
            Console.WriteLine(user.Id);
            Console.WriteLine(user.Name);
            Console.WriteLine(dataService.GetAll().Result.Count());
            var user1 = dataService.Update(user).Result;
            Console.WriteLine(user1.Name);
        }
    }
}
