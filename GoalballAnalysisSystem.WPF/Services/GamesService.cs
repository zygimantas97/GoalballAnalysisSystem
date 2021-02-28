using GoalballAnalysisSystem.API.Contracts.V1.Requests;
using GoalballAnalysisSystem.API.Contracts.V1.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.WPF.Services
{
    public class GamesService : BaseService
    {
        private readonly IIdentityService _identityService;

        public GamesService(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<List<GameResponse>> GetGamesAsync()
        {
            return null;
        }

        public async Task<GameResponse> GetGameAsync(long gameId)
        {
            return null;
        }

        public async Task UpdateGameAsync(long gameId, GameRequest request)
        {
            
        }

        public async Task<GameResponse> CreateGameAsync(GameRequest request)
        {
            return null;
        }

        public async Task<GameResponse> DeleteGameAsync(long gameId)
        {
            return null;
        }
    }
}
