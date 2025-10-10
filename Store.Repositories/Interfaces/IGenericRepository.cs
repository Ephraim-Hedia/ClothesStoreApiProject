using Store.Data.Entities;
using Store.Repositories.Specification;
namespace Store.Repositories.Interfaces
{
    public interface IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        Task<TEntity> GetByIdAsync(TKey? id);
        Task<IReadOnlyList<TEntity>> GetAllAsync();
        Task<TEntity> GetByIdWithSpecificationAsync(ISpecification<TEntity> specs);
        Task<IReadOnlyList<TEntity>> GetAllWithSpecificationAsync(ISpecification<TEntity> specs);
        Task<int> CountWithSpecificationAsync(ISpecification<TEntity> specs);
        Task AddAsync(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);

    }
}
