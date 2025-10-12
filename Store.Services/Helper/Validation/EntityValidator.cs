using Store.Data.Entities;
using Store.Repositories.Interfaces;

namespace Store.Services.Helper.Validation
{
    public class EntityValidator
    {

        private readonly IUnitOfWork _unitOfWork;

        public EntityValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ValidationResult<T>> ValidateEntityAsync<T>(int? id)
            where T : BaseEntity<int>
        {
            string entityName = typeof(T).Name; // Automatically infer the entity name

            if (id == null || id <= 0)
                return ValidationResult<T>.Fail("400", $"{entityName} ID must be greater than 0.");

            var entity = await _unitOfWork.Repository<T, int>().GetByIdAsync(id.Value);
            if (entity == null)
                return ValidationResult<T>.Fail("404", $"{entityName} with ID '{id}' not found.");

            return ValidationResult<T>.Success(entity);
        }
    }
}
    

