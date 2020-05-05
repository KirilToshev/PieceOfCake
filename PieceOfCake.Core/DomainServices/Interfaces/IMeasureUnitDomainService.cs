using CSharpFunctionalExtensions;
using PieceOfCake.Core.Entities;
using System.Collections.Generic;

namespace PieceOfCake.Core.DomainServices.Interfaces
{
    public interface IMeasureUnitDomainService
    {
        Result<IReadOnlyCollection<MeasureUnit>> Get();
        Result<MeasureUnit> Get(long id);
        Result<MeasureUnit> Create(string name);
        Result<MeasureUnit> Update(long id, string name);
        Result Delete(long id);
    }
}
