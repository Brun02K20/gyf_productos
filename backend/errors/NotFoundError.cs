using Microsoft.AspNetCore.Http;

namespace backend.Errors;

public class NotFoundError : HTTPError
{
    public NotFoundError(string message = "Not found")
        : base(StatusCodes.Status404NotFound, message)
    {
    }
}
