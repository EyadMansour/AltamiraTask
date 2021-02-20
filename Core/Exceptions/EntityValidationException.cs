using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Exceptions
{
    public class EntityValidationException : ApiException
    {
        private const int Statuscode = StatusCodes.Status400BadRequest;
        public EntityValidationException(string message) : base(message, Statuscode)
        {

        }
    }
}
