using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceCommission.Exceptions
{
    public class HttpException : Exception
    {
        public int Status { get; set; }
        public object Value { get; set; }

        public HttpException(int status, string message)
        {
            Status = status;
            Value = message;
        }
        public HttpException(int status, object value)
        {
            Status = status;
            Value = value;
        }

    }
}
