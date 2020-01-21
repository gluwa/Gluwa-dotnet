using System.ComponentModel.DataAnnotations;

namespace Gluwa.SDK_dotnet.Models
{
    internal sealed class QRCodeRequest
    {
        /// <summary>
        /// Signature that verifies that the caller is the owner of the Target address.
        /// </summary>
        [Required]
        public string Signature { get; set; }

        /// <summary>
        /// Currency for the payment.
        /// </summary>
        [Required]
        public EPaymentCurrency? Currency { get; set; }

        /// <summary>
        /// Address that will receive the payment.
        /// </summary>
        [Required]
        public string Target { get; set; }

        /// <summary>
        /// Payment amount. Fee will be deducted from this amount when payment request is made.
        /// </summary>
        [Required]
        public string Amount { get; set; }

        /// <summary>
        /// Identifier for the payment, used by the merchant user.
        /// </summary>
        [StringLength(60)]
        public string MerchantOrderID { get; set; }

        /// <summary>
        /// Additional information, used by the merchant user.
        /// </summary>
        [StringLength(280)]
        public string Note { get; set; }

        /// <summary>
        /// Time of expiry for the QR code in seconds. Payment request must be made with this QR code before this time.
        /// </summary>
        [Range(0, int.MaxValue)]
        public int? Expiry { get; set; }
    }
}