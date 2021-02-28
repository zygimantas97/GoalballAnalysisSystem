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
    public class PlayersService : BaseService
    {
        private readonly IIdentityService _identityService;
        private readonly string _serviceUrl;

        public PlayersService(IIdentityService identityService)
        {
            _identityService = identityService;
            _serviceUrl = ApiUrl + "Players/";
        }

        public async Task<List<PlayerResponse>> GetPlayersAsync()
        {
            Uri uri = new Uri(_serviceUrl);
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await _identityService.GetToken());
                var response = await client.GetAsync(uri);
                var responseString = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<List<PlayerResponse>>(responseString);
                }
                else
                {
                    var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(responseString);
                    throw new Exception(string.Join('\n', errorResponse.Errors.Select(e => e.Message)));
                }
            }
        }

        public async Task<PlayerResponse> GetPlayerAsync(long playerId)
        {
            Uri uri = new Uri(_serviceUrl + playerId);
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await _identityService.GetToken());
                var response = await client.GetAsync(uri);
                var responseString = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<PlayerResponse>(responseString);
                }
                else
                {
                    var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(responseString);
                    throw new Exception(string.Join('\n', errorResponse.Errors.Select(e => e.Message)));
                }
            }
        }

        public async Task UpdatePlayerAsync(long playerId, PlayerRequest request)
        {
            var jsonRequest = JsonConvert.SerializeObject(request);
            HttpContent content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            Uri uri = new Uri(_serviceUrl + playerId);
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

        public async Task<PlayerResponse> CreatePlayerAsync(PlayerRequest request)
        {
            var jsonRequest = JsonConvert.SerializeObject(request);
            HttpContent content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            Uri uri = new Uri(_serviceUrl);
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await _identityService.GetToken());
                var response = await client.PostAsync(uri, content);
                var responseString = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<PlayerResponse>(responseString);
                }
                else
                {
                    var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(responseString);
                    throw new Exception(string.Join('\n', errorResponse.Errors.Select(e => e.Message)));
                }
            }
        }

        public async Task<PlayerResponse> DeletePlayerAsync(long playerId)
        {
            Uri uri = new Uri(_serviceUrl + playerId);
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await _identityService.GetToken());
                var response = await client.DeleteAsync(uri);
                var responseString = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<PlayerResponse>(responseString);
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
