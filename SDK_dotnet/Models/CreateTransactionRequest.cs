using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Gluwa.SDK_dotnet.Models
{
    public sealed class CreateTransactionRequest
    {
        [Required]
        public string Address { get; set; }

        [Required]
        public ECurrency Currency { get; set; }

        [Required]
        public string PrivateKey { get; set; }

        [Required]
        public string Amount { get; set; }

        [Required]
        public string Target { get; set; }

        public string MerchantOrderID { get; set; }

        public string Note { get; set; }

        public string Nonce { get; set; }

        public Guid? Idem { get; set; }

        public Guid? PaymentID { get; set; }

        public string PaymentSig { get; set; }
    }
}
