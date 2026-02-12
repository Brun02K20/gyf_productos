using Microsoft.AspNetCore.Http;

namespace backend.Errors;

public class InternalServerError : HTTPError
{
    public InternalServerError(string message = "Unexpected error")
        : base(StatusCodes.Status500InternalServerError, message)
    {
    }
}
