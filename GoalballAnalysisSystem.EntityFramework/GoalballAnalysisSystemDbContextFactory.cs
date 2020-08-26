using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoalballAnalysisSystem.EntityFramework
{
    public class GoalballAnalysisSystemDbContextFactory : IDesignTimeDbContextFactory<GoalballAnalysisSystemDbContext>
    {
        public GoalballAnalysisSystemDbContext CreateDbContext(string[] args = null)
        {
            var options = new DbContextOptionsBuilder<GoalballAnalysisSystemDbContext>();
            options.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=GoalballAnalysisSystemDB;Trusted_Connection=True");

            return new GoalballAnalysisSystemDbContext(options.Options);
        }
    }
}
