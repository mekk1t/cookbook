using KP.Cookbook.Domain.Entities;

namespace KP.Cookbook.Database.Models
{
    public class DbUser
    {
        public string Nickname { get; set; }
        public string Avatar { get; set; }
        public UserType Type { get; set; }
        public DateTime JoinedAt { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
    }
}
