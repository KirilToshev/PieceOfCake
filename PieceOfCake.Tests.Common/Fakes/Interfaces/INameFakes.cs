using PieceOfCake.Core.Common.Entities;

namespace PieceOfCake.Tests.Common.Fakes.Interfaces;
public interface INameFakes<TValue> where TValue : GuidEntity
{
    TValue Create (string? name = null);
}
