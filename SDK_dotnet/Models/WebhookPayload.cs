using System.ComponentModel.DataAnnotations;

namespace Gluwa.Models
{
    internal sealed class WebhookPayLoad
    {
        /// <summary>
        /// Identifier for the transaction that was provided by the merchant user.
        /// </summary>
        public string MerchantOrderID { get; set; }

        /// <summary>
        /// TransactionConfirmed or TransactionCreated or TransactionFailed
        /// </summary>
        [Required]
        public string EventType { get; set; }

        /// <summary>
        /// Webhook
        /// </summary>
        [Required]
        public string Type { get; set; }

        /// <summary>
        /// Transcation ID
        /// </summary>
        public string ResourceID { get; set; }
    }
}
