using Microsoft.AspNetCore.Http;

namespace backend.Errors;

public class ConflictError : HTTPError
{
    public ConflictError(string message = "Conflict")
        : base(StatusCodes.Status409Conflict, message)
    {
    }
}
