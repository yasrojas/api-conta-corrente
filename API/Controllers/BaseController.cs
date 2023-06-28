using Domain;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace API.Controllers
{
    [ExcludeFromCodeCoverage]
    public class BaseController : ControllerBase
    {

        protected IActionResult HandleError(Error error)
        => error.StatusCode switch
        {
            "400" => BadRequest(error.Message),
            "404" => NotFound(error.Message),
            _ => StatusCode(HttpStatusCode.InternalServerError.GetHashCode(), error.Message)
        };
    }
}

