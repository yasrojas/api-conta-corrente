using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Error
    {
        public string StatusCode { get; set; }
        public string Message { get; set; }

        public Error(string statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }

        public static Error None => new Error(string.Empty, string.Empty);
    }
}
