using Forum.Web.Interfaces;
using Forum.Web.Models.Pagination;
using Forum.Web.Models.Responses;
using Forum.Web.Models.User;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Forum.Web.Services
{
    public class ForumAPIClient : IForumAPI
    {

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

        public async Task<PaginatedList<User>> GetAllProfiles(PaginationSettings settings, string token)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpContent content = JsonContent.Create(settings);
            var response = await client.PostAsync($"{client.BaseAddress}Profiles/AllUsers", content);
            string s = await response.Content.ReadAsStringAsync();
            var list = JsonConvert.DeserializeObject<PaginatedList<User>>(s);
            return JsonConvert.DeserializeObject<PaginatedList<User>>(s);
        }

        public async Task<User> GetUserProfile(int userId, string token)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync($"{client.BaseAddress}Profiles/UserProfile/{userId}");
            string s = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<User>(s);
        }

        public async Task<User?> EditUserProfile(User model, string token)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpContent content = JsonContent.Create(model);
            var response = await client.PostAsync($"{client.BaseAddress}Profiles/EditProfile", content);
            if(response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string s = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<User>(s);
            }
            else
                return null;
        }

        public async Task<Response> SetUserPhoto(int userId, Stream fileStream, string fileName, string imageType, string token)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            FormFile formFile = new(fileStream, 0, fileStream.Length, fileName, fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = imageType
            };

            MultipartFormDataContent content = new()
            {
                { new StreamContent(fileStream), "file", fileName }
            };

            var response = await client.PostAsync($"{client.BaseAddress}Profiles/SetUserPhoto/{userId}", content);
            string s = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Response>(s);
        }

        public async Task<Response> DeleteUserPhoto (int userId, string token)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.GetAsync($"{client.BaseAddress}Profiles/DeleteUserPhoto/{userId}");
            string s = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Response>(s);
        }
    }
}
