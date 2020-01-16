using System.ComponentModel.DataAnnotations;

namespace SDK_dotnet.Models
{
    public sealed class PayLoad
    {
        /// <summary>
        /// Identifier for the transaction that was provided by the merchant user.
        /// </summary>
        public string MerchantOrderID { get; set; }

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
