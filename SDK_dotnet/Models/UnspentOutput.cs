namespace Gluwa.SDK_dotnet.Models
{
    public sealed class UnspentOutput
    {
        /// <summary>
        /// Amount (in Bitcoin) for the transaction output.
        /// </summary>
        public string Amount { get; set; }

        /// <summary>
        /// Transaction hash of the transaction that contains the output.
        /// </summary>
        public string TxHash { get; set; }

        /// <summary>
        /// Index of the output in the transaction.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Number of confirmations for the transaction.
        /// </summary>
        public int Confirmations { get; set; }
    }
}
