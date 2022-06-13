using System;

namespace Gluwa.SDK_dotnet.Models
{
    public sealed class QRCodeImagePayload
    {
        public Guid? PaymentID { get; set; }

        public EPaymentCurrency? Currency { get; set; }

        public string Target { get; set; }

        public string Amount { get; set; }

        public string Fee { get; set; }

        public string Note { get; set; }

        public string MerchantOrderID { get; set; }

        public string PaymentSig { get; set; }

        public string Environment { get; set; }
    }
}
