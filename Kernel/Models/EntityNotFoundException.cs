using System;

namespace KitProjects.MasterChef.Kernel.Models
{
    public class EntityNotFoundException : Exception
    {
        public override string Message { get; }

        public EntityNotFoundException()
        {
        }

        public EntityNotFoundException(string message) : base(message)
        {
        }

        public EntityNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public EntityNotFoundException(Type type)
        {
            Message = $"Сущность типа {type} не была найдена.";
        }

        public EntityNotFoundException(Guid id)
        {
            Message = $"Сущность с ID {id} не была найдена.";
        }

        public EntityNotFoundException(Type type, Guid id)
        {
            Message = $"Сущность типа {type} c ID {id} не была найдена.";
        }
    }
}
