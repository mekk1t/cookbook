using KP.Cookbook.Domain.Entities;

namespace KP.Cookbook.Features.Abstractions
{
    public interface IAccessValidator
    {
        bool IsCurrentUserOfType(UserType userType);
    }
}
