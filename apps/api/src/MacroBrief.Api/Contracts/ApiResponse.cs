public record ApiResponse<T>(T? Data, ApiError? Error, ApiMeta Meta)
{
    public static ApiResponse<T> Ok(T data) => new(data, null, new ApiMeta(DateTime.UtcNow));

    public static ApiResponse<T> Fail(string code, string message) =>
        new(default, new ApiError(code, message), new ApiMeta(DateTime.UtcNow));
}

public record ApiError(string Code, string Message);

public record ApiMeta(DateTime TimestampUtc);
