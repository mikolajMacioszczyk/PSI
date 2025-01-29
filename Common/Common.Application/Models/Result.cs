namespace Common.Application.Models;
public struct Result<T>
{
    public T? Value { get; private set; }
    public bool IsSuccess { get; private set; }
    public readonly bool IsFailure => !IsSuccess && !IsNotFound;
    public bool IsNotFound { get; private set; }
    public string ErrorMessage { get; private set; }

    private Result(T? value, bool isSuccess, bool isNotFound, string errorMessage)
    {
        Value = value;
        IsSuccess = isSuccess;
        IsNotFound = isNotFound;
        ErrorMessage = errorMessage;
    }

    public static implicit operator Result<T>(T value) => new(value, true, false, string.Empty);

    public static implicit operator Result<T>(Failure failure) => new(default, false, false, failure.Message);

    public static implicit operator Result<T>(List<Failure> failures) => new Failure(string.Join(",", failures.Select(f => f.Message)));

    public static implicit operator Result<T>(NotFound notFound) => new(default, false, true, notFound.Message);

    public static bool operator !(Result<T> result) => !result.IsSuccess;

    public static Result<T> MergeResults(Result<T> result1, Result<T> result2, Func<T, T, T> mergeValues)
    {
        bool isNotFound = result1.IsNotFound || result2.IsNotFound;
        bool isSuccess = result1.IsSuccess && result2.IsSuccess;

        var errorMessages = new List<string>();

        if (!result1.IsSuccess)
        {
            errorMessages.Add(result1.ErrorMessage);
        }
        if (!result2.IsSuccess)
        {
            errorMessages.Add(result2.ErrorMessage);
        }

        string errorMessage = string.Join(", ", errorMessages);

        T? value = isSuccess ? mergeValues(result1.Value!, result2.Value!) : default;

        return new Result<T>(value, isSuccess, isNotFound, errorMessage);
    }

    public static Result<T> MergeErrors<R, S>(Result<R> result1, Result<S> result2)
    {
        bool isSuccess = result1.IsSuccess && result2.IsSuccess;
        if (isSuccess)
        {
            throw new InvalidOperationException($"Merging errors of successfull results not allowed");
        }

        bool isNotFound = result1.IsNotFound || result2.IsNotFound;
        var errorMessages = new List<string>();
        if (!result1.IsSuccess)
        {
            errorMessages.Add(result1.ErrorMessage);
        }
        if (!result2.IsSuccess)
        {
            errorMessages.Add(result2.ErrorMessage);
        }
        string errorMessage = string.Join(", ", errorMessages);

        return new Result<T>(default, false, isNotFound, errorMessage);
    }

    public readonly Result<S> MapNonSuccessfullTo<S>()
    {
        if (IsSuccess)
        {
            throw new InvalidOperationException($"Mapping Successfull result to {typeof(S)} not allowed");
        }
        return new Result<S>(default, IsSuccess, IsNotFound, ErrorMessage);
    }
}
