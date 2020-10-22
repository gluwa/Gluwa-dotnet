namespace Gluwa.SDK_dotnet.Models
{
    public class TransactionResource : IResourceObj
    {
        /// <summary>
        /// Gluwa's internal transaction ID.
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// The created date and time of the webhook.
        /// </summary>
        public string TxHash { get; set; }

        /// <summary>
        /// The address of the sender.
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// The address of the receiver.
        /// </summary>
        public string Target { get; set; }

        /// <summary>
        /// Transaction amount, not including the fee. This is the amount that the receiver receives.
        /// </summary>
        public string Amount { get; set; }

        /// <summary>
        /// The fee amount.
        /// </summary>
        public string Fee { get; set; }

        /// <summary>
        /// The currency of the transaction.
        /// </summary>
        public ECurrency? Currency { get; set; }

        /// <summary>
        /// The status of the transaction.Unconfirmed, Confirmed, or Failed.
        /// </summary>
        public ETransactionStatus? Status { get; set; }
    }
}
