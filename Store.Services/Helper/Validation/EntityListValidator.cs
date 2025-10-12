using Store.Data.Entities;
using Store.Repositories.Interfaces;

namespace Store.Services.Helper.Validation
{
    public class EntityListValidator
    {
        private readonly IUnitOfWork _unitOfWork;

        public EntityListValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ValidationListResult<T>> ValidateEntityIdsAsync<T>(
            IEnumerable<int>? ids)
            where T : BaseEntity<int>
        {
            var result = new ValidationListResult<T>();
            var entityName = typeof(T).Name; // 👈 infer name automatically
            var validIds = ids?.Where(id => id > 0).Distinct().ToList() ?? new();
            if (!validIds.Any())
            {
                result.IsValid = true;
                return result;
            }

            var tasks = validIds.Select(id => _unitOfWork.Repository<T, int>().GetByIdAsync(id));
            var fetched = await Task.WhenAll(tasks);

            var foundIds = fetched.Where(e => e != null).Select(e => e.Id).ToList();
            var missing = validIds.Except(foundIds).ToList();

            if (missing.Any())
            {
                result.IsValid = false;
                result.MissingIds = missing;
                result.ErrorMessage = $"Invalid {entityName} IDs: {string.Join(", ", missing)}";
                return result;
            }

            result.IsValid = true;
            result.ValidIds = foundIds;
            return result;
        }
    }
}
