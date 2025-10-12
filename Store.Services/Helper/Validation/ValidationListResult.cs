namespace Store.Services.Helper.Validation
{
    public class ValidationListResult<T>
    {
        public bool IsValid { get; set; }
        public List<int> ValidIds { get; set; } = new();
        public List<int> MissingIds { get; set; } = new();
        public string? ErrorMessage { get; set; }
    }
}
