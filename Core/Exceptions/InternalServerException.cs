using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Core.Exceptions
{
    public class InternalServerException : ApiException
    {

        public InternalServerException(string detail, string title = "Sorry, an unexpected error has occurred") : base(new ProblemDetails()
        {
            Detail = detail,
            Status = StatusCodes.Status500InternalServerError,
            Title = title
        })
        {
        }
    }
}
