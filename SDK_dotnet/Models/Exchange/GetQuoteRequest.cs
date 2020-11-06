using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gluwa.SDK_dotnet.Models.Exchange
{
    public sealed class GetQuotesRequest
    {
        /// <summary>
        /// datetime.If defined, only quotes created after this datetime are included in the response.
        /// </summary>
        public DateTime? StartDateTime { get; set; }

        /// <summary>
        /// datetime. If defined, only quotes created before this datetime are included in the response.
        /// </summary>
        public DateTime? EndDateTime { get; set; }

        /// <summary>
        /// Pending or Processed.
        /// </summary>
        public EQuoteStatus? Status { get; set; }

        /// <summary>
        /// Number of quotes to skip the beginning of list.Defaults to 0.
        /// </summary>
        public int Offset { get; set; } = 0;

        /// <summary>
        /// Number of quotes to include in the result. Defaults to 25, maximum of 100.
        /// </summary>
        public int Limit { get; set; } = 25;

        public IEnumerable<ValidationResult> Validate()
        {
            if (Limit > 100)
            {
                yield return new ValidationResult($"The maximum value of the {nameof(Limit)} is 100.", new[] { nameof(Limit) });
            }
        }
    }
}
