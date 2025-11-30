using System.Net;

namespace UserService.Application.DTOs
{
    public class ErrorResponce
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
        public string? Details { get; set; }
    }
}
