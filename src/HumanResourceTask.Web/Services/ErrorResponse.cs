using FluentResults;

namespace HumanResourceTask.Web.Services
{
    public class ErrorResponse : IError
    {
        public int StatusCode { get; set; }

        public required string Message { get; set; }

        public Dictionary<string, List<string>> Errors { get; set; } = [];

        public List<IError> Reasons { get; } = [];

        public Dictionary<string, object> Metadata { get; } = [];
    }
}
