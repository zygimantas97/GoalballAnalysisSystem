using GoalballAnalysisSystem.Domain.Models;
using GoalballAnalysisSystem.Domain.Services;
using GoalballAnalysisSystem.EntityFramework;
using GoalballAnalysisSystem.EntityFramework.Services;
using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            IDataService<User> userService = new DataService<User>(new GoalballAnalysisSystemDbContextFactory());
            var users = userService.GetAll().Result;

        }
    }
}
