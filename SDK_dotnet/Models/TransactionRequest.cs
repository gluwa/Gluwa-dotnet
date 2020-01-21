using System;
using System.ComponentModel.DataAnnotations;

namespace Gluwa.SDK_dotnet.Models
{
    internal sealed class TransactionRequest
    {
        /// <summary>
        /// Signature of the transaction to be created. 
        /// </summary>
        [Required]
        public string Signature { get; set; }

        /// <summary>
        /// Transaction amount, not including the fee.
        /// </summary>
        [Required]
        public string Amount { get; set; }

        /// <summary>
        /// Transaction fee amount. Use the Fee API to find out the current minimum amount.
        /// </summary>
        [Required]
        public string Fee { get; set; }

        /// <summary>
        /// Currency of the transaction.
        /// </summary>
        [Required]
        public ECurrency? Currency { get; set; }

        /// <summary>
        /// Address that the transaction will be sent from.
        /// </summary>
        [Required]
        public string Source { get; set; }

        /// <summary>
        /// Address that the transaction will be sent to.
        /// </summary>
        [Required]
        public string Target { get; set; }

        /// <summary>
        /// Identifier for the transaction that was provided by the merchant user.
        /// </summary>
        [StringLength(60)]
        public string MerchantOrderID { get; set; }

        /// <summary>
        /// Additional information about the transaction that a user can provide.
        /// </summary>
        [StringLength(280)]
        public string Note { get; set; }

        /// <summary>
        /// Nonce for the transaction. For Gluwacoin currencies only.
        /// </summary>
        public string Nonce { get; set; }

        /// <summary>
        /// Idempotent key for the transaction to prevent duplicate transactions.
        /// </summary>
        public Guid? Idem { get; set; }

        /// <summary>
        /// ID for the QR code payment.
        /// </summary>
        public Guid? PaymentID { get; set; }

        /// <summary>
        /// Signature of the QR code payment.
        /// Required if PaymentID is not null.
        /// </summary>
        public string PaymentSig { get; set; }
    }
}
