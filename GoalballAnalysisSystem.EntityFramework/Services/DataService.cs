using GoalballAnalysisSystem.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.EntityFramework.Services
{
    public class DataService<T> : IDataService<T> where T : class
    {
        private readonly GoalballAnalysisSystemDbContextFactory _contextFactory;

        public DataService(GoalballAnalysisSystemDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<T> Create(T entity)
        {
            using(GoalballAnalysisSystemDbContext context = _contextFactory.CreateDbContext())
            {
                EntityEntry<T> result = await context.Set<T>().AddAsync(entity);
                await context.SaveChangesAsync();

                return result.Entity;
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

        
        public async Task<IEnumerable<T>> GetAll()
        {
            using (GoalballAnalysisSystemDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<T> entities = await context.Set<T>().ToListAsync();
                return entities;
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
    }
}
