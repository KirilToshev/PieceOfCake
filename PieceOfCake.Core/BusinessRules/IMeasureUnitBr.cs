using CSharpFunctionalExtensions;
using PieceOfCake.Core.Entities;
using System.Collections.Generic;

namespace PieceOfCake.Core.BusinessRules
{
    public interface IMeasureUnitBr
    {
        Result<IReadOnlyCollection<MeasureUnit>> Get();
        Result<MeasureUnit> Get(int id);
        Result<MeasureUnit> Create(string name);
        Result<MeasureUnit> Update(int id, string name);
        Result Delete(int id);
    }
}
