using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KP.Cookbook.RestApi.Controllers.Users.Requests
{
    public class RegisterUserRequest
    {
        public string? Nickname { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [PasswordPropertyText]
        public string Password { get; set; }
    }
}
