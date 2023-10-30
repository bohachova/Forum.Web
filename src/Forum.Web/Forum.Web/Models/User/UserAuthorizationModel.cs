using System.ComponentModel.DataAnnotations;

namespace Forum.Web.Models.User
{
    public class UserAuthorizationModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        [StringLength(30, MinimumLength = 8)]
        public string Password { get; set; } = string.Empty;
    }
}
