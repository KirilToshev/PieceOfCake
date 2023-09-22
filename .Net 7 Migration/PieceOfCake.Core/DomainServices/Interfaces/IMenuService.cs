using CSharpFunctionalExtensions;
using PieceOfCake.Core.Entities;

namespace PieceOfCake.Core.DomainServices.Interfaces;

public interface IMenuService : ICRUDService<Menu, Guid>
{
    Result<Menu> Create(DateTime startDate, DateTime endDate, byte servingsPerDay, ushort numberOfPeople);
    Result<Menu> Update(Guid id, DateTime startDate, DateTime endDate, byte servingsPerDay, ushort numberOfPeople);
    Result<Menu> GenerateDishesList(Guid id);
}
