using GoalballAnalysisSystem.API.Contracts.V1.Requests;
using GoalballAnalysisSystem.API.Contracts.V1.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.WPF.Services
{
    public class TeamPlayersService : BaseService
    {
        private readonly IIdentityService _identityService;

        public TeamPlayersService(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<List<TeamPlayerResponse>> GetTeamPlayersByTeamAsync(long teamId)
        {
            return null;
        }

        public async Task<List<TeamPlayerResponse>> GetTeamPlayersByPlayerAsync(long playerId)
        {
            return null;
        }

        public async Task<TeamPlayerResponse> GetTeamPlayerAsync(long teamId, long playerId)
        {
            return null;
        }

        public async Task UpdateTeamPlayerAsync(long teamId, long playerId, TeamPlayerRequest request)
        {

        }

        public async Task<TeamPlayerResponse> CreateTeamPlayerAsync(long teamId, long playerId, TeamPlayerRequest request)
        {
            return null;
        }

        public async Task<TeamPlayerResponse> DeleteTeamPlayerAsync(long teamId, long playerId)
        {
            return null;
        }
    }
}
