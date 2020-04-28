using System.ComponentModel.DataAnnotations;

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
        /// Current nonce for the address. For Gluwacoin currencies only.
        /// </summary>
        [Range(0, ulong.MaxValue)]
        public ulong? Nonce { get; set; }
    }
}
