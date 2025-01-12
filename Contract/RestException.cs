using System.Net;

namespace NurseryMart.Contract
{
    public class RestException : Exception
    {
        public RestException(HttpStatusCode code, string message) : base(message)
        {
            Code = code;
        }

        public HttpStatusCode Code { get; }
    }
}
