namespace KP.Cookbook.Domain.Entities
{
    public class User : Entity
    {
        public string? Nickname { get; set; }
        public string? Avatar { get; set; }

        public UserType UserType { get; private set; }
        public DateTime JoinedAt { get; private set; }
        public string Login { get; private set; }
        public string PasswordHash { get; private set; }

        private User()
        {
        }

        private User(string login, string passwordHash)
        {
            Login = login;
            PasswordHash = passwordHash;
            JoinedAt = DateTime.UtcNow;
            UserType = UserType.User;
        }

        public static User Register(string login, string passwordHash, string nickname = "", string avatar = "")
        {
            if (string.IsNullOrEmpty(login))
                throw new InvariantException("Не указан логин");
            if (string.IsNullOrEmpty(passwordHash))
                throw new InvariantException("Не указан пароль");

            return new User(login, passwordHash)
            {
                Nickname = nickname,
                Avatar = avatar
            };
        }
    }

    public enum UserType
    {
        Guest,
        User,
        Admin
    }
}
