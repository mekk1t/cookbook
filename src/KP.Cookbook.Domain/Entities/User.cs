namespace KP.Cookbook.Domain.Entities
{
    public class User : Entity
    {
        public string? Nickname { get; set; }
        public string? Avatar { get; set; }

        public UserType UserType { get; }
        public DateTime JoinedAt { get; }
        public string Login { get; }
        public string PasswordHash { get; }

        public User(UserType userType)
        {
            UserType = userType;
            JoinedAt = DateTime.UtcNow;
            Login = string.Empty;
            PasswordHash = string.Empty;
        }

        private User(string login, string passwordHash)
        {
            Login = login;
            PasswordHash = passwordHash;
            JoinedAt = DateTime.UtcNow;
        }

        public static User Register(string login, string passwordHash)
        {
            if (string.IsNullOrEmpty(login))
                throw new InvariantException("Не указан логин");
            if (string.IsNullOrEmpty(passwordHash))
                throw new InvariantException("Не указан пароль");

            return new User(login, passwordHash);
        }
    }

    public enum UserType
    {
        Guest,
        User,
        Admin
    }
}
