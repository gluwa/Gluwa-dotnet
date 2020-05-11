using System.ComponentModel.DataAnnotations;

namespace Gluwa.SDK_dotnet.Models
{
    public sealed class Data
    {
        /// <summary>
        /// Transaction amount.
        /// </summary>
        public string Amount { get; set; }

        /// <summary>
        /// TransactionConfirmed or TransactionCreated or TransactionFailed
        /// </summary>
        [Required]
        public EEventType? EventType { get; set; }

        /// <summary>
        /// Webhook
        /// </summary>
        [Required]
        public ENotificationType? Type { get; set; }

        /// <summary>
        /// Transcation ID
        /// </summary>
        public string ResourceID { get; set; }
    }
}
