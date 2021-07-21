using System;

namespace KitProjects.Cookbook.Database
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(Type type, long id) : base($"Сущность {type.GetType().Name} с ID {id} не найдена.")
        {
        }
    }
}
