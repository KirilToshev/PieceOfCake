using CSharpFunctionalExtensions;
using PieceOfCake.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PieceOfCake.Core.DomainServices.Interfaces
{
    public interface IMenuDomainService
    {
        Result<IReadOnlyCollection<Menu>> Get();
        Result<Menu> Get(long id);
        Result<Menu> Create(DateTime? startDate, DateTime? endDate, byte servingsPerDay);
        Result<Menu> Update(DateTime? startDate, DateTime? endDate, byte servingsPerDay);
        Result Delete(long id);
    }
}
