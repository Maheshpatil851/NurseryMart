using NurseryMart.Utility;
using System.Net;

namespace NurseryMart.Contract
{
    public class ResponseDto
    {
        public ResponseDto(dynamic response, int total, string message, string status = "pending")
        {
            if (response == null) throw new RestException(HttpStatusCode.NotFound, ErrorConstant.NotFound);
            Total = total;
            Data = (response is IEnumerable<object> || response is List<object>) ? (response as IEnumerable<object>) : new object[] { response };
            Message = message;
            Status = status;
        }
        public int Total { set; get; }
        public dynamic? Data { set; get; }
        public string? Message { set; get; }
        public string? Status { set; get; }
    }
}
