namespace Store.Services.HandleResponse.CommonResponse
{
    public class CommonResponse<T>
    {
        
        public bool IsSuccess { get; set; }
        public Error Errors { get; set; } = new Error();
        public T Data { get; set; }
        public CommonResponse<T> Success(T data)
        {
            IsSuccess = true;
            Data = data;
            return this;
        }

        public CommonResponse<T> Fail(string code, string message)
        {
            IsSuccess = false;
            Errors.Code = code;
            Errors.Message = message;
            return this;
        }

    }
}
