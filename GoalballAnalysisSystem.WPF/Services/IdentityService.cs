using GoalballAnalysisSystem.API.Contracts.V1.Requests;
using GoalballAnalysisSystem.API.Contracts.V1.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.WPF.Services
{
    public class IdentityService : BaseService, IIdentityService
    {
        private string _serviceUrl;
        
        private string _token;
        private string _refreshToken;
        private DateTime _expirityDate;

        public IdentityService()
        {
            _serviceUrl = ApiUrl + "Identity/";
            _expirityDate = DateTime.Now;
        }

        public async Task<AuthenticationResponse> RegisterAsync(string userName, string email, string password)
        {
            var request = new RegistrationRequest
            {
                UserName = userName,
                Email = email,
                Password = password
            };
            var jsonRequest = JsonConvert.SerializeObject(request);
            HttpContent content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            Uri uri = new Uri(_serviceUrl + "Register");
            using (HttpClient client = new HttpClient())
            {
                var response = await client.PostAsync(uri, content);
                var responseString = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    var authenticationResponse = JsonConvert.DeserializeObject<AuthenticationResponse>(responseString);
                    _token = authenticationResponse.Token;
                    _refreshToken = authenticationResponse.RefreshToken;
                    _expirityDate = authenticationResponse.ExpirityDate;
                    return authenticationResponse;
                }
                else
                {
                    var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(responseString);
                    throw new Exception(string.Join('\n', errorResponse.Errors.Select(e => e.Message)));
                }
            }
        }

        public async Task<AuthenticationResponse> LoginAsync(string email, string password)
        {
            var request = new LoginRequest
            {
                Email = email,
                Password = password
            };
            var jsonRequest = JsonConvert.SerializeObject(request);
            HttpContent content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            Uri uri = new Uri(_serviceUrl + "Login");
            using (HttpClient client = new HttpClient())
            {
                var response = await client.PostAsync(uri, content);
                var responseString = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    var authenticationResponse = JsonConvert.DeserializeObject<AuthenticationResponse>(responseString);
                    _token = authenticationResponse.Token;
                    _refreshToken = authenticationResponse.RefreshToken;
                    _expirityDate = authenticationResponse.ExpirityDate;
                    return authenticationResponse;
                }
                else
                {
                    var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(responseString);
                    throw new Exception(string.Join('\n', errorResponse.Errors.Select(e => e.Message)));
                }
            }
        }

        public async Task<string> GetToken()
        {
            if (_expirityDate > DateTime.Now)
            {
                return _token;
            }
            await RefreshTokenAsync();
            return _token;
        }

        private async Task RefreshTokenAsync()
        {
            var request = new RefreshTokenRequest
            {
                Token = _token,
                RefreshToken = _refreshToken
            };
            var jsonRequest = JsonConvert.SerializeObject(request);
            HttpContent content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            Uri uri = new Uri(_serviceUrl + "RefreshToken");
            using (HttpClient client = new HttpClient())
            {
                var response = await client.PostAsync(uri, content);
                var responseString = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    var authenticationResponse = JsonConvert.DeserializeObject<AuthenticationResponse>(responseString);
                    _token = authenticationResponse.Token;
                    _refreshToken = authenticationResponse.RefreshToken;
                    _expirityDate = authenticationResponse.ExpirityDate;
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
