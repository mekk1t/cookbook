using Dapper;
using KP.Cookbook.Domain.Entities;

namespace KP.Cookbook.Database
{
    public class UsersRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public UsersRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void DeleteById(long id)
        {
            var sql = @"
                DELETE FROM users
                WHERE id = @Id;
            ";

            var parameters = new { Id = id };

            _unitOfWork.Execute((c, t) => c.Execute(sql, parameters, t));
        }

        public User Create(User user)
        {
            var sql = @"
                INSERT INTO users
                    (nickname, avatar, type, joined_at, login, password_hash)
                VALUES
                    (@Nickname, @Avatar, @Type, @JoinedAt, @Login, @PasswordHash)
                RETURNING
                    nickname, avatar, type, joined_at, login, password_hash, id;
            ";

            var parameters = new { user.Nickname, user.Avatar, Type = user.Type, user.JoinedAt, user.Login, user.PasswordHash };

            return _unitOfWork.Execute((c, t) => c.QueryFirst<User>(sql, parameters, t));
        }

        public void Update(User user)
        {
            var sql = @"
                UPDATE users
                SET
                    nickname = @Nickname,
                    avatar = @Avatar,
                    type = @Type,
                    joined_at = @JoinedAt,
                    login = @Login,
                    password_hash = @PasswordHash
                WHERE
                    id = @Id;
            ";

            var parameters = new { user.Nickname, user.Avatar, Type = user.Type, user.JoinedAt, user.Login, user.PasswordHash, user.Id };

            _unitOfWork.Execute((c, t) => c.Execute(sql, parameters, t));
        }

        public User GetById(long id)
        {
            var sql = @"
                SELECT id, nickname, avatar, type, joined_at, login, password_hash FROM users
                WHERE id = @Id;
            ";

            var parameters = new { Id = id };

            return _unitOfWork.Execute((c, t) => c.QueryFirst<User>(sql, parameters, t));
        }

        public User? GetByLoginOrDefault(string login)
        {
            var sql = @"
                SELECT id, nickname, avatar, type, joined_at, login, password_hash FROM users
                WHERE login = @Login;
            ";

            var parameters = new { Login = login };

            return _unitOfWork.Execute((c, t) => c.QueryFirstOrDefault<User?>(sql, parameters, t));
        }

        public List<User> GetAll()
        {
            var sql = "SELECT id, nickname, avatar, type, joined_at, login, password_hash FROM users;";

            return _unitOfWork.Execute((c, t) => c.Query<User>(sql, transaction: t).ToList());
        }
    }
}
