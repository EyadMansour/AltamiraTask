using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Exceptions
{
    public class ActivityCannotBeSameException : ApiException
    {
        private const int Statuscode = StatusCodes.Status409Conflict;
        public ActivityCannotBeSameException(string title = "Activity cannot be same!") :base(title, Statuscode)
        {

        }
    }
}
