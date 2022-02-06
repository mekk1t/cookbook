using Dapper;
using KP.Cookbook.Domain.Entities;

namespace KP.Cookbook.Database
{
    public class SourcesRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public SourcesRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<Source> Get()
        {
            var sql = @"
                SELECT id, name, description, link, image, is_approved
                FROM sources;
            ";

            return _unitOfWork.Execute((c, t) => c.Query<Source>(sql, transaction: t).ToList());
        }

        public Source Create(Source source)
        {
            var sql = @"
                INSERT INTO sources
                (
                    name,
                    description,
                    link,
                    image,
                    is_approved
                )
                VALUES 
                (
                    @Name,
                    @Description,
                    @Link,
                    @Image,
                    @IsApproved
                )
                RETURNING
                    id,
                    name,
                    description,
                    link,
                    image,
                    is_approved;
            ";

            var parameters = new { source.Name, source.Description, source.Link, source.Image, source.IsApproved };

            return _unitOfWork.Execute((c, t) => c.QueryFirst<Source>(sql, parameters, t));
        }

        public Source GetById(long id)
        {
            var sql = @"
                SELECT id, name, description, link, image, is_approved
                FROM sources
                WHERE id = @Id;
            ";

            var parameters = new { Id = id };

            return _unitOfWork.Execute((c, t) => c.QueryFirst<Source>(sql, parameters, t));
        }

        public void DeleteById(long id)
        {
            var sql = @"
                DELETE FROM sources
                WHERE id = @Id;
            ";

            var parameters = new { Id = id };

            _unitOfWork.Execute((c, t) => c.Execute(sql, parameters, t));
        }

        public void Update(Source source)
        {
            var sql = @"
                UPDATE 
                    sources
                SET
                    name = @Name,
                    description = @Description,
                    link = @Link,
                    image = @Image,
                    is_approved = @IsApproved
                WHERE
                    id = @Id;
            ";

            var parameters = new { source.Id, source.Name, source.Description, source.Link, source.Image, source.IsApproved };

            _unitOfWork.Execute((c, t) => c.Execute(sql, parameters, t));
        }
    }
}
