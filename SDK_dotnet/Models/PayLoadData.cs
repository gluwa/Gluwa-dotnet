using System.ComponentModel.DataAnnotations;

namespace Gluwa.Models
{
    public sealed class PayLoadData
    {
        /// <summary>
        /// Identifier for the transaction that was provided by the merchant user.
        /// </summary>
        public string MerchantOrderID { get; set; }

        /// <summary>
        /// 1 is TransactionConfirmed.
        /// </summary>
        [Required]
        public int EventType { get; set; }

        /// <summary>
        /// 0 is Webhook.
        /// </summary>
        [Required]
        public int Type { get; set; }

        /// <summary>
        /// Transcation ID
        /// </summary>
        public string ResourceID { get; set; }
    }
}