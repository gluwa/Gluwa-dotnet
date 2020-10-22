using System.ComponentModel.DataAnnotations;

namespace Gluwa.SDK_dotnet.Models
{
    public sealed class Data
    {
        /// <summary>
        /// The merchant order ID.
        /// </summary>
        public string MerchantOrderID { get; set; }

        /// <summary>
        /// The amount that was received.
        /// </summary>
        public string Amount { get; set; }

        /// <summary>
        /// TransactionConfirmed or TransactionCreated or TransactionFailed
        /// </summary>
        [Required]
        public EEventType? EventType { get; set; }

        /// <summary>
        /// Always Webhook
        /// </summary>
        [Required]
        public ENotificationType? Type { get; set; }

        /// <summary>
        /// The ID of the resource. this is the transaction hash.
        /// </summary>
        [Required]
        public string ResourceID { get; set; }
    }
}
