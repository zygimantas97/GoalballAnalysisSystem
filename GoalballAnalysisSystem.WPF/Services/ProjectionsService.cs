using GoalballAnalysisSystem.API.Contracts.V1.Requests;
using GoalballAnalysisSystem.API.Contracts.V1.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.WPF.Services
{
    public class ProjectionsService : BaseService
    {
        private readonly IIdentityService _identityService;

        public ProjectionsService(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<List<ProjectionResponse>> GetProjectionsByGameAsync(long gameId)
        {
            return null;
        }

        public async Task<List<ProjectionResponse>> GetProjectionsByGamePlayerAsync(long gamePlayerId)
        {
            return null;
        }

        public async Task<ProjectionResponse> GetProjectionAsync(long projectionId)
        {
            return null;
        }

        public async Task UpdateProjectionAsync(long projectionId, ProjectionRequest request)
        {

        }

        public async Task<ProjectionResponse> CreateProjectionAsync(ProjectionRequest request)
        {
            return null;
        }

        public async Task<ProjectionResponse> DeleteProjectionAsync(long projectionId)
        {
            return null;
        }
    }
}
