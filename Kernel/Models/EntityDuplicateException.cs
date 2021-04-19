using System;

namespace KitProjects.MasterChef.Kernel.Models
{
    public class EntityDuplicateException : Exception
    {
        public override string Message { get; }

        public EntityDuplicateException(string message)
        {
            Message = message;
        }
    }
}
