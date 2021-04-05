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
    public class PlayerRolesService : BaseService
    {
        private readonly IIdentityService _identityService;
        private readonly string _serviceUrl;

        public PlayerRolesService(IIdentityService identityService)
        {
            _identityService = identityService;
            _serviceUrl = ApiUrl + "PlayerRoles/";
        }

        public async Task<List<PlayerRoleResponse>> GetPlayerRolesAsync()
        {
            Uri uri = new Uri(_serviceUrl);
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await _identityService.GetToken());
                var response = await client.GetAsync(uri);
                var responseString = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<List<PlayerRoleResponse>>(responseString);
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
