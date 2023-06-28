using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Domain
{
    public class Response<T>
    {
        public bool HasError { get; set; }
        public Error Error { get; set; }
        public T Data { get; set; }

        protected Response(T data, Error error, bool hasError)
        {
            Data = data;
            Error = error;
            HasError = hasError;
        } 

        protected Response(Error error)
        {
            Error = error;
            HasError = true;
        }

        public static Response<T> Success(T data) => new Response<T>(data, Error.None, false);
        public static Response<T> Fail(Error error) => new Response<T>(default, error, true);
    }
}
