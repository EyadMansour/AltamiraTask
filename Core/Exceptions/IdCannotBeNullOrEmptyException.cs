using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Http;

namespace Core.Exceptions
{
    public class IdCannotBeNullOrEmptyException : ApiException
    {
        private const int Statuscode = StatusCodes.Status409Conflict;

        public IdCannotBeNullOrEmptyException(string title = "Id cannot be null or empty!") : base(title, Statuscode)
        {
        }
    }
}
