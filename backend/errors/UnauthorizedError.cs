using Microsoft.AspNetCore.Http;

namespace backend.Errors;

public class UnauthorizedError : HTTPError
{
    public UnauthorizedError(string message = "Unauthorized")
        : base(StatusCodes.Status401Unauthorized, message)
    {
    }
}
