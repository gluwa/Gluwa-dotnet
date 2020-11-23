using System;

namespace Gluwa.SDK_dotnet.Models
{
    internal static class ECurrencyExtensions
    {
        internal static bool IsGluwaCoinCurrency(this ECurrency currency)
        {
            switch (currency)
            {
                case ECurrency.KRWG:
                case ECurrency.USDG:
                case ECurrency.NGNG:
                    return true;

                case ECurrency.BTC:
                    return false;

                default:
                    throw new ArgumentOutOfRangeException($"Unsupported currecny: {currency}");
            }
        }

        internal static bool IsGluwaExchangeCurrency(this ECurrency currency)
        {
            switch (currency)
            {
                case ECurrency.KRWG:
                case ECurrency.USDG:
                case ECurrency.BTC:
                    return true;

                case ECurrency.NGNG:
                    return false;

                default:
                    throw new ArgumentOutOfRangeException($"Unsupported currecny: {currency}");
            }
        }
    }
}
