using GoalballAnalysisSystem.Domain.Services;
using GoalballAnalysisSystem.EntityFramework.Services.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.EntityFramework.Services
{
    public class GenericDataService<T> : IDataService<T> where T : class
    {
        private readonly GoalballAnalysisSystemDbContextFactory _contextFactory;
        private readonly NonQueryDataService<T> _nonQueryDataService;

        public GenericDataService(GoalballAnalysisSystemDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
            _nonQueryDataService = new NonQueryDataService<T>(contextFactory);
        }

        public async Task<T> Create(T entity)
        {
            return await _nonQueryDataService.Create(entity);
        }

        public async Task<bool> Delete(T entity)
        {
            return await _nonQueryDataService.Delete(entity);
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
            return await _nonQueryDataService.Update(entity);
        }
    }
}
