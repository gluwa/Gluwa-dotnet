using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gluwa.SDK_dotnet.Models.Exchange
{
    internal sealed class PutQuoteRequest
    {
        /// <summary>
        /// All orders that will fulfill this quote.
        /// </summary>
        [Required]
        public List<MatchedOrderRequest> MatchedOrders { get; set; }

        /// <summary>
        /// Checksum that was received when quote was created.
        /// </summary>
        [Required]
        public string Checksum { get; set; }
    }
}
