using System.Threading.Tasks;

namespace PieceOfCake.Core.Persistence
{
    public interface IUnitOfWork
    {
        IMeasureUnitRepository MeasureUnitRepository { get; }

        void Save();
        Task<int> SaveAsync();
    }
}
