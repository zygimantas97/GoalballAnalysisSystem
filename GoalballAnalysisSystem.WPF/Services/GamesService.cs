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
    public class GamesService : BaseService
    {
        private readonly IIdentityService _identityService;
        private readonly string _serviceUrl;

        public GamesService(IIdentityService identityService)
        {
            _identityService = identityService;
            _serviceUrl = ApiUrl + "Games/";
        }

        public async Task<List<GameResponse>> GetGamesAsync()
        {
            Uri uri = new Uri(_serviceUrl);
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await _identityService.GetToken());
                var response = await client.GetAsync(uri);
                var responseString = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<List<GameResponse>>(responseString);
                }
                else
                {
                    var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(responseString);
                    throw new Exception(string.Join('\n', errorResponse.Errors.Select(e => e.Message)));
                }
            }
        }

        public async Task<GameResponse> GetGameAsync(long gameId)
        {
            Uri uri = new Uri(_serviceUrl + gameId);
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await _identityService.GetToken());
                var response = await client.GetAsync(uri);
                var responseString = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<GameResponse>(responseString);
                }
                else
                {
                    var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(responseString);
                    throw new Exception(string.Join('\n', errorResponse.Errors.Select(e => e.Message)));
                }
            }
        }

        public async Task UpdateGameAsync(long gameId, GameRequest request)
        {
            var jsonRequest = JsonConvert.SerializeObject(request);
            HttpContent content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            Uri uri = new Uri(_serviceUrl + gameId);
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

        public async Task<GameResponse> CreateGameAsync(GameRequest request)
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
                    return JsonConvert.DeserializeObject<GameResponse>(responseString);
                }
                else
                {
                    var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(responseString);
                    throw new Exception(string.Join('\n', errorResponse.Errors.Select(e => e.Message)));
                }
            }
        }

        public async Task<GameResponse> DeleteGameAsync(long gameId)
        {
            Uri uri = new Uri(_serviceUrl + gameId);
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await _identityService.GetToken());
                var response = await client.DeleteAsync(uri);
                var responseString = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<GameResponse>(responseString);
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
