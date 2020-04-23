namespace Gluwa.SDK_dotnet.Models
{
    public sealed class UnspentOutput
    {
        /// <summary>
        /// Amount (in Bitcoin) for the transaction output.
        /// </summary>
        public string Amount { get; private set; }

        /// <summary>
        /// Transaction hash of the transaction that contains the output.
        /// </summary>
        public string TxHash { get; private set; }

        /// <summary>
        /// Index of the output in the transaction.
        /// </summary>
        public int Index { get; private set; }

        /// <summary>
        /// Number of confirmations for the transaction.
        /// </summary>
        public int Confirmations { get; private set; }

        /// <summary>
        /// Internal use only.
        /// </summary>
        public string Tx { get; set; }

        public UnspentOutput(
            string amount,
            string txHash,
            int index,
            int confirmations)
        {
            Amount = amount;
            TxHash = txHash;
            Index = index;
            Confirmations = confirmations;
        }
    }
}
