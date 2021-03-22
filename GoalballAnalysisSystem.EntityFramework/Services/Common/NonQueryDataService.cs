using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.EntityFramework.Services.Common
{
    public class NonQueryDataService<T> where T : class
    {
        private readonly GoalballAnalysisSystemDbContextFactory _contextFactory;

        public NonQueryDataService(GoalballAnalysisSystemDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<T> Create(T entity)
        {
            using (GoalballAnalysisSystemDbContext context = _contextFactory.CreateDbContext())
            {
                EntityEntry<T> createdResult = await context.Set<T>().AddAsync(entity);
                await context.SaveChangesAsync();
                return createdResult.Entity;
            }
        }

        public async Task<T> Update(T entity)
        {
            using (GoalballAnalysisSystemDbContext context = _contextFactory.CreateDbContext())
            {
                context.Set<T>().Update(entity);
                await context.SaveChangesAsync();
                return entity;
            }
        }

        public async Task<bool> Delete(T entity)
        {
            using (GoalballAnalysisSystemDbContext context = _contextFactory.CreateDbContext())
            {
                context.Set<T>().Remove(entity);
                await context.SaveChangesAsync();
                return true;
            }
        }
    }
}
