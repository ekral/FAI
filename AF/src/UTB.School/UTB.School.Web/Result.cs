namespace UTB.School.Web
{
    public record Result(bool IsSuccess, string? Error = null)
    {
        public static Result Success() => new(true);
        public static Result Failure(string error) => new(false, error);
    }

    public record Result<T>(bool IsSuccess, T? Value, string? Error = null)
    {
        public static Result<T> Success(T value) => new(true, value);
        public static Result<T> Failure(string error) => new(false, default, error);
    }
}
