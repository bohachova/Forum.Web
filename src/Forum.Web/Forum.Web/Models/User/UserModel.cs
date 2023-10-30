using System.ComponentModel.DataAnnotations;
using Forum.Web.Enums;

namespace Forum.Web.Models.User
{
    public class UserModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 2)]
        public string Username { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        [StringLength(30, MinimumLength = 8)]
        public string Password { get; set; } = string.Empty;
        public UserRole UserRole { get; set; }
        public DateTime RegistrationDate { get; set; } 
        public string UserPhoto { get; set; } = string.Empty;
    }
}
