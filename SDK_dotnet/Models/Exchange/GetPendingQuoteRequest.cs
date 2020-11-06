using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gluwa.SDK_dotnet.Models.Exchange
{
    public sealed class GetPendingQuoteRequest
    {
        /// <summary>
        /// The amount in source currency you want to exchange.
        /// </summary>
        [Required]
        public string Amount { get; set; }

        /// <summary>
        /// Conversion symbol for the exchange.
        /// </summary>
        [Required]
        public EConversion? Conversion { get; set; }

        /// <summary>
        /// The address that will fund the source amount.
        /// </summary>
        [Required]
        public string SendingAddress { get; set; }

        /// <summary>
        /// The address that the exchanged currency will be received.
        /// </summary>
        [Required]
        public string ReceivingAddress { get; set; }

        public IEnumerable<ValidationResult> Validate()
        {
            if (string.IsNullOrWhiteSpace(Amount))
            {
                yield return new ValidationResult($"{nameof(Amount)} parameter is required.", new[] { nameof(Amount) });
            }
            else if (!Conversion.HasValue)
            {
                yield return new ValidationResult($"{nameof(Conversion)} parameter is required.", new[] { nameof(Conversion) });
            }
            else if (string.IsNullOrWhiteSpace(SendingAddress))
            {
                yield return new ValidationResult($"{nameof(SendingAddress)} parameter is required.", new[] { nameof(SendingAddress) });
            }
            else if (string.IsNullOrWhiteSpace(ReceivingAddress))
            {
                yield return new ValidationResult($"{nameof(ReceivingAddress)} parameter is required.", new[] { nameof(ReceivingAddress) });
            }
        }
    }
}
