using Forum.Web.Models.User;
using Forum.Web.Models.Responses;
using Forum.Web.Models.Pagination;
using Forum.Web.Models.TopicPost;

namespace Forum.Web.Interfaces
{
    public interface IForumAPI
    {
        Task<AuthResponse> CreateUser(UserRegistrationModel model);
        Task<AuthResponse> Authorize (UserAuthorizationModel model);
        Task<bool> CheckIfPasswordSetRequired(string email);
        Task<AuthResponse> SetAdminPassword(UserAuthorizationModel model);
        Task<PaginatedList<UserModel>> GetAllProfiles(PaginationSettings settings, string token);
        Task<UserModel> GetUserProfile(int userId, string token);
        Task<UserModel?> EditUserProfile(UserModel user, string token);
        Task<Response> SetUserPhoto(int userId, Stream fileStream, string fileName, string imageType, string token);
        Task<Response> DeleteUserPhoto(int userId, string token);
        Task<PaginatedList<Topic>> GetTopicsList(PaginationSettings settings);
        Task<PaginatedList<Post>> GetTopicPosts (PaginationSettings settings, int topicId, string token);
        Task<Response> CreateTopic(Topic topic, string token);
        Task<Response> CreatePost (PostCreationModel post, string token);
        Task<Response> DeletePost(int postId,  string token);
        Task<Response> LeaveComment (Comment comment,  string token);
        Task<Response> DeleteComment (int commentId,  string token);
        Task<Post> ViewPost(int postId, PaginationSettings settings, string token);
        Task<Response> EditPost (PostEditModel post,  string token);
        Task<Response> EditComment (CommentEditModel comment, string token);
        Task<ReactionsResponse> PostReaction (Reaction reaction, string token);
        Task<ReactionsResponse> CommentReaction (Reaction reaction, string token);
    }
}
