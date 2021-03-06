﻿using Microsoft.EntityFrameworkCore;
using PieceOfCake.Core.Common;
using PieceOfCake.Core.Persistence;
using PieceOfCake.Core.Resources;
using PieceOfCake.Core.ValueObjects;
using PieceOfCake.Persistence.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PieceOfCake.Persistence
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly PocDbContext _context;
        private IMeasureUnitRepository? _measureUnitRepository;
        private IProductRepository? _productRepository;
        private IDishRepository? _dishRepository;
        private IMenuRepository? _menuRepository;

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

        public IProductRepository ProductRepository
        {
            get
            {
                if (_productRepository == null)
                    _productRepository = new ProductRepository(_context);

                return _productRepository;
            }
        }

        public IDishRepository DishRepository
        {
            get
            {
                if (_dishRepository == null)
                    _dishRepository = new DishRepository(_context);

                return _dishRepository;
            }
        }

        public IMenuRepository MenuRepository
        {
            get
            {
                if (_menuRepository == null)
                    _menuRepository = new MenuRepository(_context);

                return _menuRepository;
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

        //NOTE Left with no reason, just a cool way to get repositories by their generic type using reflection
        private IGenericRepository<TEntity> GetRepositoryByType<TEntity>() where TEntity : Entity
        {
            var property = this.GetType().GetProperties()
                .Where(x =>
                x.PropertyType.GetInterfaces().Length > 0
                && x.PropertyType.GetInterfaces().Any(i => i.IsGenericType)
                && x.PropertyType.GetInterfaces().First(i => i.IsGenericType).GetGenericArguments()[0] == typeof(TEntity))
                .First();

            var repo = property.GetValue(this);

            var repository = repo as IGenericRepository<TEntity>;
            if (repository == null)
                throw new ArgumentOutOfRangeException(nameof(TEntity));

            return repository;
        }
    }
}
