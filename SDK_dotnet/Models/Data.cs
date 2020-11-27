using Gluwa.SDK_dotnet.Models.Exchange;
using System.ComponentModel.DataAnnotations;

namespace Gluwa.SDK_dotnet.Models
{
    public sealed class Data
    {
        /// <summary>
        /// TransactionConfirmed or TransactionCreated or TransactionFailed or ExchangeSuccess or ExchangeFailed
        /// </summary>
        [Required]
        public EEventType? EventType { get; set; }

        /// <summary>
        /// Always Webhook
        /// </summary>
        [Required]
        public ENotificationType? Type { get; set; }

        /// <summary>
        /// The ID of the resource.
        /// For TransactionConfirmed, TransactionCreated, TransactionFailed, this is the transaction hash.
        /// For ExchangeSuccess, ExchangeFailed, this is the order ID
        /// </summary>
        [Required]
        public string ResourceID { get; set; }

        /// <summary>
        /// (Transactions Related Events Only) The merchant order ID.
        /// </summary>
        public string MerchantOrderID { get; set; }

        /// <summary>
        /// (Transactions Related Events Only) The amount that was received.
        /// </summary>
        public string Amount { get; set; }

        /// <summary>
        /// (Exchanges Related Events Only) Amount remaining in the order.
        /// </summary>
        public string OrderAmountRemaining { get; set; }

        /// <summary>
        /// (Exchanges Related Events Only) Conversion symbol for the order.
        /// </summary>
        public EConversion Conversion { get; set; }

        /// <summary>
        /// (Exchanges Related Events Only) The address where the source amount was sent from.
        /// </summary>
        public string SendingAddress { get; set; }

        /// <summary>
        /// (Exchanges Related Events Only) The address where the exchanged amount was received.
        /// </summary>
        public string ReceivingAddress { get; set; }

        /// <summary>
        /// (Exchanges Related Events Only) The amount that was sent in source currency.
        /// </summary>
        public string SourceAmount { get; set; }

        /// <summary>
        /// (Exchanges Related Events Only) The price used for the exchange.
        /// </summary>
        public string Price { get; set; }

        /// <summary>
        /// (Exchanges Related Events Only) The amount that was received in exchanged currency.
        /// </summary>
        public string ExchangedAmount { get; set; }
    }
}
