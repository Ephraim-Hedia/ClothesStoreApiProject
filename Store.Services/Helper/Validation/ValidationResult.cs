namespace Store.Services.Helper.Validation
{
    public class ValidationResult<T>
        where T : class
    {
        public bool IsValid { get; set; }
        public string? ErrorCode { get; set; }
        public string? ErrorMessage { get; set; }
        public T? Entity { get; set; }

        // Factory helpers for clarity
        public static ValidationResult<T> Success(T entity)
            => new() { IsValid = true, Entity = entity };

        public static ValidationResult<T> Fail(string errorCode, string errorMessage)
            => new() { IsValid = false, ErrorCode = errorCode, ErrorMessage = errorMessage };
    }
}
