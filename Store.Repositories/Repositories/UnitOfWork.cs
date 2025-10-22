using Microsoft.EntityFrameworkCore.Storage;
using Store.Data.Context;
using Store.Data.Entities;
using Store.Repositories.Interfaces;
using System.Collections;

namespace Store.Repositories.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreClothesDbContext _context;
        private IDbContextTransaction? _transaction;
        private Hashtable _repositories;
        public UnitOfWork(StoreClothesDbContext context)
        {
            _context = context;
        }
        public Task<int> CompleteAsync()
            => _context.SaveChangesAsync();
        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }
        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
                await _transaction.CommitAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
                await _transaction.RollbackAsync();
        }
        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }

        public IGenericRepository<TEntity, TKey> Repository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
            if (_repositories == null)
                _repositories = new Hashtable();

            var entityKey = typeof(TEntity).Name;   // Repository<Product , int>

            if (!_repositories.ContainsKey(entityKey))
            {
                var repositoryType = typeof(GenericRepository<,>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity), typeof(TKey)), _context);
                _repositories.Add(entityKey, repositoryInstance);

            }
            return (IGenericRepository<TEntity, TKey>)_repositories[entityKey];
        }
    }
}
