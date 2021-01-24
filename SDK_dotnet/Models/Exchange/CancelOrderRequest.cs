using System.ComponentModel.DataAnnotations;

namespace Gluwa.SDK_dotnet.Models.Exchange
{
    internal sealed class CancelOrderRequest
    {
        /// <summary>
        /// Use Cancel.
        /// </summary>
        [Required]
        public string Action { get; set; } = "Cancel";
    }
}
