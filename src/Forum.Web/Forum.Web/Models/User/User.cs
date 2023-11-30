using System.ComponentModel.DataAnnotations;
using Forum.Web.Enums;

namespace Forum.Web.Models.User
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 2)]
        public string Username { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        public UserRole UserRole { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string? UserPhoto { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? About { get; set; }
    }
}
