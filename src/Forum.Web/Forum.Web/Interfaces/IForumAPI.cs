using Forum.Web.Models.User;
using Forum.Web.Models.Responses;
using Forum.Web.Models.Pagination;

namespace Forum.Web.Interfaces
{
    public interface IForumAPI
    {
        Task<AuthResponse> CreateUser(UserRegistrationModel model);
        Task<AuthResponse> Authorize (UserAuthorizationModel model);
        Task<bool> CheckIfPasswordSetRequired(string email);
        Task<AuthResponse> SetAdminPassword(UserAuthorizationModel model);
        Task<PaginatedList<User>> GetAllProfiles(PaginationSettings settings, string token);
        Task<User> GetUserProfile(int userId, string token);
        Task<User?> EditUserProfile(User user, string token);
        Task<Response> SetUserPhoto(int userId, Stream fileStream, string fileName, string imageType, string token);
        Task<Response> DeleteUserPhoto(int userId, string token);
    }
}
