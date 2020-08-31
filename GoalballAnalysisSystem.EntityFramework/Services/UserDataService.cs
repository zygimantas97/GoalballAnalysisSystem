using GoalballAnalysisSystem.Domain.Models;
using GoalballAnalysisSystem.Domain.Services;
using GoalballAnalysisSystem.EntityFramework.Services.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.EntityFramework.Services
{
    public class UserDataService : IUserDataService
    {
        private readonly GoalballAnalysisSystemDbContextFactory _contextFactory;
        private readonly NonQueryDataService<User> _nonQueryDataService;

        public UserDataService(GoalballAnalysisSystemDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
            _nonQueryDataService = new NonQueryDataService<User>(contextFactory);
        }

        public async Task<User> Create(User entity)
        {
            return await _nonQueryDataService.Create(entity);
        }

        public async Task<bool> Delete(User entity)
        {
            return await _nonQueryDataService.Delete(entity);
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            using (GoalballAnalysisSystemDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<User> entities = await context.Users
                    .Include(u => u.Games)
                    .Include(u => u.Teams)
                    .Include(u => u.Players)
                    .Include(u => u.RoleNavigation)
                    .ToListAsync();
                return entities;
            }
        }

        public async Task<User> GetByEmail(string email)
        {
            using(GoalballAnalysisSystemDbContext context = _contextFactory.CreateDbContext())
            {
                return await context.Users
                    .Include(u => u.Games)
                    .Include(u => u.Teams)
                    .Include(u => u.Players)
                    .Include(u => u.RoleNavigation)
                    .FirstOrDefaultAsync(u => u.Email == email);
            }
        }

        public async Task<User> Update(User entity)
        {
            return await _nonQueryDataService.Update(entity);
        }
    }
}
