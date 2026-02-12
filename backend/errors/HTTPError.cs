using Microsoft.AspNetCore.Http;

namespace backend.Errors;

public abstract class HTTPError : Exception
{
    public int Status { get; }

    protected HTTPError(int status, string message) : base(message)
    {
        Status = status;
    }
}
