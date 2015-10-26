using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.Commands
{
    public interface IFindUserByNameResult
    {
        IUser User { get; } 
    }
}