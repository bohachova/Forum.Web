using Forum.Web.Models.User;
using Forum.Web.Models.Responses;

namespace Forum.Web.Interfaces
{
    public interface IForumAPI
    {
        Task<AuthResponse> CreateUser(UserRegistrationModel model);
        Task<AuthResponse> Authorize (UserAuthorizationModel model);
        Task<bool> CheckIfPasswordSetRequired(string email);
        Task<AuthResponse> SetAdminPassword(UserAuthorizationModel model);
    }
}
