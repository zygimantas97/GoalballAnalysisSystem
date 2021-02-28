using GoalballAnalysisSystem.API.Contracts.V1.Requests;
using GoalballAnalysisSystem.API.Contracts.V1.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.WPF.Services
{
    public class GamePlayersService : BaseService
    {
        private readonly IIdentityService _identityService;

        public GamePlayersService(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<List<GamePlayerResponse>> GetGamePlayersByGameAsync(long gameId)
        {
            return null;
        }

        public async Task<List<GamePlayerResponse>> GetGamePlayersByTeamPlayerAsync(long teamId, long playerId)
        {
            return null;
        }

        public async Task<GamePlayerResponse> GetGamePlayerAsync(long gamePlayerId)
        {
            return null;
        }

        public async Task UpdateGamePlayerAsync(long gamePlayerId, UpdateGamePlayerRequest request)
        {

        }

        public async Task<GamePlayerResponse> CreateGamePlayerAsync(CreateGamePlayerRequest request)
        {
            return null;
        }

        public async Task<GamePlayerResponse> DeleteGamePlayerAsync(long gamePlayerId)
        {
            return null;
        }
    }
}
