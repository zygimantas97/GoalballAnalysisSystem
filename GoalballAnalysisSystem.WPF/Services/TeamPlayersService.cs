using GoalballAnalysisSystem.API.Contracts.V1.Requests;
using GoalballAnalysisSystem.API.Contracts.V1.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.WPF.Services
{
    public class TeamPlayersService : BaseService
    {
        private readonly IIdentityService _identityService;
        private readonly string _serviceUrl;

        public TeamPlayersService(IIdentityService identityService)
        {
            _identityService = identityService;
            _serviceUrl = ApiUrl + "TeamPlayers/";
        }

        public async Task<List<TeamPlayerResponse>> GetTeamPlayersByTeamAsync(long teamId)
        {
            Uri uri = new Uri(_serviceUrl + "ByTeam/" + teamId);
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await _identityService.GetToken());
                var response = await client.GetAsync(uri);
                var responseString = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<List<TeamPlayerResponse>>(responseString);
                }
                else
                {
                    var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(responseString);
                    throw new Exception(string.Join('\n', errorResponse.Errors.Select(e => e.Message)));
                }
            }
        }

        public async Task<List<TeamPlayerResponse>> GetTeamPlayersByPlayerAsync(long playerId)
        {
            Uri uri = new Uri(_serviceUrl + "ByPlayer/" + playerId);
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await _identityService.GetToken());
                var response = await client.GetAsync(uri);
                var responseString = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<List<TeamPlayerResponse>>(responseString);
                }
                else
                {
                    var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(responseString);
                    throw new Exception(string.Join('\n', errorResponse.Errors.Select(e => e.Message)));
                }
            }
        }

        public async Task<TeamPlayerResponse> GetTeamPlayerAsync(long teamId, long playerId)
        {
            Uri uri = new Uri(_serviceUrl + teamId + "/" + playerId);
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await _identityService.GetToken());
                var response = await client.GetAsync(uri);
                var responseString = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<TeamPlayerResponse>(responseString);
                }
                else
                {
                    var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(responseString);
                    throw new Exception(string.Join('\n', errorResponse.Errors.Select(e => e.Message)));
                }
            }
        }

        public async Task UpdateTeamPlayerAsync(long teamId, long playerId, TeamPlayerRequest request)
        {
            var jsonRequest = JsonConvert.SerializeObject(request);
            HttpContent content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            Uri uri = new Uri(_serviceUrl + teamId + "/" + playerId);
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await _identityService.GetToken());
                var response = await client.PutAsync(uri, content);
                var responseString = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(responseString);
                    throw new Exception(string.Join('\n', errorResponse.Errors.Select(e => e.Message)));
                }
            }
        }

        public async Task<TeamPlayerResponse> CreateTeamPlayerAsync(long teamId, long playerId, TeamPlayerRequest request)
        {
            var jsonRequest = JsonConvert.SerializeObject(request);
            HttpContent content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            Uri uri = new Uri(_serviceUrl + teamId + "/" + playerId);
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await _identityService.GetToken());
                var response = await client.PostAsync(uri, content);
                var responseString = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<TeamPlayerResponse>(responseString);
                }
                else
                {
                    var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(responseString);
                    throw new Exception(string.Join('\n', errorResponse.Errors.Select(e => e.Message)));
                }
            }
        }

        public async Task<TeamPlayerResponse> DeleteTeamPlayerAsync(long teamId, long playerId)
        {
            Uri uri = new Uri(_serviceUrl + teamId + "/" + playerId);
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await _identityService.GetToken());
                var response = await client.DeleteAsync(uri);
                var responseString = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<TeamPlayerResponse>(responseString);
                }
                else
                {
                    var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(responseString);
                    throw new Exception(string.Join('\n', errorResponse.Errors.Select(e => e.Message)));
                }
            }
        }
    }
}
