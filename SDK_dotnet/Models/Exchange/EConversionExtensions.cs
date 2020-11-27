using System;

namespace Gluwa.SDK_dotnet.Models.Exchange
{
    public static class EConversionExtensions
    {
        public static ECurrency ToSourceCurrency(this EConversion conversion)
        {
            switch (conversion)
            {
                case EConversion.KrwgBtc:
                case EConversion.KrwgUsdg:
                    return ECurrency.KRWG;

                case EConversion.UsdgKrwg:
                case EConversion.UsdgBtc:
                    return ECurrency.USDG;

                case EConversion.BtcKrwg:
                case EConversion.BtcUsdg:
                    return ECurrency.BTC;

                default:
                    throw new ArgumentOutOfRangeException($"No corresponding source currency for {conversion}.");
            }
        }

        public static ECurrency ToTargetCurrency(this EConversion conversion)
        {
            switch (conversion)
            {
                case EConversion.KrwgBtc:
                case EConversion.UsdgBtc:
                    return ECurrency.BTC;

                case EConversion.BtcUsdg:
                case EConversion.KrwgUsdg:
                    return ECurrency.USDG;

                case EConversion.BtcKrwg:
                case EConversion.UsdgKrwg:
                    return ECurrency.KRWG;

                default:
                    throw new ArgumentOutOfRangeException($"No corresponding source currency for {conversion}.");
            }
        }

        public static bool IsSourceCurrencyBtc(this EConversion conversion)
        {
            return conversion.ToSourceCurrency() == ECurrency.BTC;
        }
    }
}
