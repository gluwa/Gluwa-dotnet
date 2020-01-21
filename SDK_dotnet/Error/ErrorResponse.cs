using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gluwa.SDK_dotnet.Error
{
    public sealed class ErrorResponse : IError
    {
        [Required]
        public string Code { get; set; }

        public string Message { get; set; }

        public List<InnerError> InnerErrors { get; set; }
    }

    public sealed class InnerError
    {
        public string Code { get; set; }

        public string Path { get; set; }

        public string Message { get; set; }
    }
}
