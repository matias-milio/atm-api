namespace ATM.Core
{
    public class Result<TValue, TError>
    {
        public readonly TValue? Value;
        public readonly TError? Error;
        public bool IsSuccess;
        private Result(TValue value)
        {
            IsSuccess = true;
            Value = value;
            Error = default;
        }

        private Result(TError error)
        {
            IsSuccess = false;
            Value = default;
            Error = error;
        }

        public static implicit operator Result<TValue, TError>(TValue value) => new(value);

        public static implicit operator Result<TValue, TError>(TError error) => new(error);

        public Result<TValue, TError> Match(Func<TValue, Result<TValue, TError>> success, Func<TError, Result<TValue, TError>> failure)
        {
            if (IsSuccess)            
                return success(Value!);            
            return failure(Error!);
        }
    }
}
