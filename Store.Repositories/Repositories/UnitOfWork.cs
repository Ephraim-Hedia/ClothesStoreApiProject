using Store.Data.Context;
using Store.Data.Entities;
using Store.Repositories.Interfaces;
using System.Collections;

namespace Store.Repositories.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreClothesDbContext _context;
        private Hashtable _repositories;
        public UnitOfWork(StoreClothesDbContext context)
        {
            _context = context;
        }
        public Task<int> CompleteAsync()
            => _context.SaveChangesAsync();

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
