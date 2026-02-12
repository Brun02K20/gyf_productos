using Microsoft.AspNetCore.Http;

namespace backend.Errors;

public class InvalidParamsError : HTTPError
{
    public InvalidParamsError(string message = "Invalid parameters")
        : base(StatusCodes.Status400BadRequest, message)
    {
    }
}
