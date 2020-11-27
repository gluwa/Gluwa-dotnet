using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Gluwa.SDK_dotnet.Models.Exchange
{
    public sealed class AcceptQuoteRequest
    {
        /// <summary>
        /// Checksum that was received when quote was created.
        /// </summary>
        [Required]
        public string Checksum { get; set; }

        /// <summary>
        /// All orders that will fulfill this quote.
        /// </summary>
        [Required]
        public List<AcceptQuoteMatchedOrder> MatchedOrders { get; set; }

        public IEnumerable<ValidationResult> Validate(ECurrency currency)
        {
            if (string.IsNullOrWhiteSpace(Checksum))
            {
                yield return new ValidationResult($"{nameof(Checksum)} parameter is required.", new[] { nameof(Checksum) });
            }
            if (MatchedOrders == null)
            {
                yield return new ValidationResult($"{nameof(MatchedOrders)} is required.", new[] { nameof(MatchedOrders) });
            }
            else
            {
                foreach (var matchedOrder in MatchedOrders)
                {
                    IEnumerable<ValidationResult> validation = matchedOrder.Validate(currency);

                    if (validation.Any())
                    {
                        foreach (var item in validation)
                        {
                            throw new ArgumentNullException(item.ErrorMessage);
                        }
                    }
                }
            }
        }
    }
}
