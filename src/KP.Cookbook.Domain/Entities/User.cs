namespace KP.Cookbook.Domain.Entities
{
    public class User : Entity
    {
        public string Nickname { get; private set; }
        public string Avatar { get; private set; }

        public UserType Type { get; private set; }
        public DateTime JoinedAt { get; private set; }
        public string Login { get; private set; }
        public string PasswordHash { get; private set; }

        private User()
        {
        }

        public static User Register(string login, string passwordHash, string nickname = "", string avatar = "")
        {
            if (string.IsNullOrEmpty(login))
                throw new InvariantException("Не указан логин");
            if (string.IsNullOrEmpty(passwordHash))
                throw new InvariantException("Не указан пароль");

            return new User
            {
                Nickname = nickname,
                Avatar = avatar,
                Login = login,
                PasswordHash = passwordHash,
                JoinedAt = DateTime.UtcNow,
                Type = UserType.User
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
