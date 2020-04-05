using Microsoft.EntityFrameworkCore;
using PieceOfCake.Core.Persistence;
using PieceOfCake.Core.Resources;
using PieceOfCake.Core.ValueObjects;
using PieceOfCake.Persistence.Repositories;
using System;
using System.Threading.Tasks;

namespace PieceOfCake.Persistence
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly PocDbContext _context;
        private IMeasureUnitRepository? _measureUnitRepository;

        public UnitOfWork(DbContextOptions<PocDbContext> options, IResources resources)
        {
            _context = new PocDbContext(options, resources);
        }
        
        public IMeasureUnitRepository MeasureUnitRepository
        {
            get
            {
                if (_measureUnitRepository == null)
                    _measureUnitRepository = new MeasureUnitRepository(_context);

                return _measureUnitRepository;
            }
        }


        public void Save()
        {
            _context.SaveChanges();
        }

        public Task<int> SaveAsync()
        {
            return _context.SaveChangesAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        
    }
}
