using System.ComponentModel.DataAnnotations;

namespace Gluwa.Models
{
    public sealed class PayLoad
    {
        /// <summary>
        /// Identifier for the transaction that was provided by the merchant user.
        /// </summary>
        public string MerchantOrderID { get; set; }

        /// <summary>
        /// Address that will receive the payment.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Payment amount. Fee will be deducted from this amount when payment request is made.
        /// </summary>
        public string Amount { get; set; }

        /// <summary>
        /// USDG = 1, KRWG = 2
        /// </summary>
        public int Currency { get; set; }

        /// <summary>
        /// TransactionConfirmed or TransactionCreated or TransactionFailed
        /// </summary>
        [Required]
        public string EventType { get; set; }

        /// <summary>
        /// Webhook or PushNotification
        /// </summary>
        [Required]
        public string Type { get; set; }

        /// <summary>
        /// Transcation ID
        /// </summary>
        public string ResourceID { get; set; }
    }
}
