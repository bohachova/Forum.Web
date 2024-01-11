using Forum.Web.Interfaces;
using Forum.Web.Models.Pagination;
using Forum.Web.Models.Responses;
using Forum.Web.Models.TopicPost;
using Forum.Web.Models.User;
using Newtonsoft.Json;
using System.IO;
using System.IO.Pipes;
using System.Net.Http.Headers;
using System.Reflection;

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

        public async Task<PaginatedList<UserModel>> GetAllProfiles(PaginationSettings settings, string token)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpContent content = JsonContent.Create(settings);
            var response = await client.PostAsync($"{client.BaseAddress}Profiles/AllUsers", content);
            string s = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<PaginatedList<UserModel>>(s);
        }

        public async Task<UserModel> GetUserProfile(int userId, string token)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.GetAsync($"{client.BaseAddress}Profiles/UserProfile/{userId}");
            string s = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<UserModel>(s);
        }

        public async Task<UserModel?> EditUserProfile(UserModel model, string token)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpContent content = JsonContent.Create(model);
            var response = await client.PostAsync($"{client.BaseAddress}Profiles/EditProfile", content);
            if(response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string s = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<UserModel>(s);
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

        public async Task<PaginatedList<Topic>> GetTopicsList(PaginationSettings settings)
        {
            HttpContent content = JsonContent.Create(settings);
            var response = await client.PostAsync($"{client.BaseAddress}Topics/AllTopics", content);
            string s = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<PaginatedList<Topic>>(s);
        }

        public async Task<PaginatedList<Post>> GetTopicPosts(PaginationSettings settings, int topicId, string token)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpContent content = JsonContent.Create(settings);
            var response = await client.PostAsync($"{client.BaseAddress}Topics/Posts/{topicId}", content);
            string s = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<PaginatedList<Post>>(s);
        }

        public async Task<Response> CreateTopic(Topic topic, string token)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpContent content = JsonContent.Create(topic);
            var response = await client.PostAsync($"{client.BaseAddress}Topics/NewTopic", content);
            string s = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Response>(s);
        }

        public async Task<Response> CreatePost(PostCreationModel post, string token)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            

            MultipartFormDataContent content = new()
            {
                { new StringContent(post.Header), "Header" },
                { new StringContent(post.Text), "Text" },
                { new StringContent(post.AuthorId.ToString()), "AuthorId" },
                { new StringContent(post.TopicId.ToString()), "TopicId" }

            };

            if (post.Attachments.Any())
            {
                foreach (var file in post.Attachments)
                {
                    Stream stream = file.OpenReadStream();
                    FormFile formFile = new(stream, 0,  stream.Length, file.FileName, file.FileName)
                    {
                        Headers = new HeaderDictionary(),
                        ContentType = file.ContentType
                    };

                    content.Add(new StreamContent(stream), "Attachments", file.FileName);
                }
            }

            var response = await client.PostAsync($"{client.BaseAddress}Topics/NewPost", content);
            string s = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Response>(s);
        }

        public async Task<Response> DeletePost(int postId, string token)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpContent content = JsonContent.Create(postId);
            var response = await client.PostAsync($"{client.BaseAddress}Topics/DeletePost", content);
            string s = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Response>(s);
        }

        public async Task<Response> LeaveComment(Comment comment, string token)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpContent content = JsonContent.Create(comment);
            var response = await client.PostAsync($"{client.BaseAddress}Comments/NewComment", content);
            string s = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Response>(s);
        }

        public async Task<Response> DeleteComment(int commentId, string token)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpContent content = JsonContent.Create(commentId);
            var response = await client.PostAsync($"{client.BaseAddress}Comments/DeleteComment", content);
            string s = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Response>(s);
        }

        public async Task<Post> ViewPost(int postId, PaginationSettings settings, string token)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpContent content = JsonContent.Create(settings);
            var response = await client.PostAsync($"{client.BaseAddress}Topics/ViewPost/{postId}", content);
            string s = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Post>(s);
        }
    }
}
