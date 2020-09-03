using System.Collections.Generic;

namespace Gluwa.SDK_dotnet.Models
{
    public sealed class BalanceResponse
    {
        /// <summary>
        /// The balance.
        /// </summary>
        public string Balance { get; set; }

        /// <summary>
        /// Currency for the address balance.
        /// </summary>
        public ECurrency? Currency { get; set; }

        /// <summary>
        /// Unspent transaction outputs of the address that can be used to create new transactions. For BTC only.
        /// </summary>
        public List<UnspentOutput> UnspentOutputs { get; set; }
    }
}
