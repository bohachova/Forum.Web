using Forum.Web.Interfaces;
using Forum.Web.Models.Responses;
using Forum.Web.Models.User;
using Newtonsoft.Json;

namespace Forum.Web.Services
{
    public class ForumAPIClient : IForumAPI
    {
        //Typed HttpClient
        private readonly HttpClient client;
        public ForumAPIClient(HttpClient client)
        {
            this.client = client;
        }
        public async Task<AuthResponse> Authorize(UserAuthorizationModel model)
        {
            HttpContent content = JsonContent.Create(model);
            var response = await client.PostAsync($"{client.BaseAddress}Auth/Authorization", content);
            string s = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<AuthResponse>(s);
        }

        public async Task<bool> CheckIfPasswordSetRequired(string email)
        {
            HttpContent content = JsonContent.Create(email);
            var response = await client.PostAsync($"{client.BaseAddress}Auth/AuthorizationCheck", content);
            string s = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<AuthResponse>(s);
            return !result.IsSuccess;
        }

        public async Task<AuthResponse> CreateUser(UserRegistrationModel model)
        {
            HttpContent content = JsonContent.Create(model);
            var response = await client.PostAsync($"{client.BaseAddress}Auth/Registration", content);
            string s = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<AuthResponse>(s);
        }

        public async Task<AuthResponse> SetAdminPassword(UserAuthorizationModel model)
        {
            HttpContent content = JsonContent.Create(model);
            var response = await client.PostAsync($"{client.BaseAddress}Auth/AdminStart", content);
            string s = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<AuthResponse>(s);
        }
    }
}
