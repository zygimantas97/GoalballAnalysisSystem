﻿using GoalballAnalysisSystem.API.Contracts.V1.Requests;
using GoalballAnalysisSystem.API.Contracts.V1.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.WPF.Services
{
    public class PlayersService : BaseService
    {
        private readonly IIdentityService _identityService;

        public PlayersService(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<List<PlayerResponse>> GetPlayersAsync()
        {
            return null;
        }

        public async Task<PlayerResponse> GetPlayerAsync(long playerId)
        {
            return null;
        }

        public async Task UpdatePlayerAsync(long playerId, PlayerRequest request)
        {

        }

        public async Task<PlayerResponse> CreatePlayerAsync(PlayerRequest request)
        {
            return null;
        }

        public async Task<PlayerResponse> DeletePlayerAsync(long playerId)
        {
            return null;
        }
    }
}
