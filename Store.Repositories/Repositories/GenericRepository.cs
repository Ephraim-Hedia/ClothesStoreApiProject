using Microsoft.EntityFrameworkCore;
using Store.Data.Context;
using Store.Data.Entities;
using Store.Repositories.Interfaces;
using Store.Repositories.Specification;

namespace Store.Repositories.Repositories
{
    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        private readonly StoreClothesDbContext _context;
        public GenericRepository(StoreClothesDbContext context)
        {
            _context = context; 
        }
        public async Task AddAsync(TEntity entity)
            => await _context.Set<TEntity>().AddAsync(entity);

        public async Task<IReadOnlyList<TEntity>> GetAllAsync()
            => await _context.Set<TEntity>().ToListAsync();

        public async Task<TEntity> GetByIdAsync(TKey? id)
            => await _context.Set<TEntity>().FindAsync(id);

        public void Update(TEntity entity)
            => _context.Set<TEntity>().Update(entity);

        public void Delete(TEntity entity)
            => _context.Set<TEntity>().Remove(entity);


        public async Task<IReadOnlyList<TEntity>> GetAllWithSpecificationAsync(ISpecification<TEntity> specs)
            => await ApplySpecification(specs).ToListAsync();
        public async Task<TEntity> GetByIdWithSpecificationAsync(ISpecification<TEntity> specs)
            => await ApplySpecification(specs).FirstOrDefaultAsync();
        public async Task<int> CountWithSpecificationAsync(ISpecification<TEntity> specs)
            => await ApplySpecification(specs).CountAsync();
        private IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> specs)
        {
            return SpecificationEvaluator<TKey, TEntity>.GetQuery(_context.Set<TEntity>(), specs); 
        }
    }
}
