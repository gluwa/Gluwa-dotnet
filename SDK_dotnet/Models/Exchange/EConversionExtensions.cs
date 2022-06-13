using System;

namespace Gluwa.SDK_dotnet.Models.Exchange
{
    public static class EConversionExtensions
    {
        public static ECurrency ToSourceCurrency(this EConversion conversion)
        {
            switch (conversion)
            {
                case EConversion.BtcUsdcg:
                case EConversion.BtcsUsdcg:
                    return ECurrency.BTC;

                case EConversion.sUsdcgBtc:
                    return ECurrency.sUSDCG;

                case EConversion.UsdcgBtc:
                    return ECurrency.USDCG;

                default:
                    throw new ArgumentOutOfRangeException($"No corresponding source currency for {conversion}.");
            }
        }

        public static ECurrency ToTargetCurrency(this EConversion conversion)
        {
            switch (conversion)
            {
                case EConversion.UsdcgBtc:
                case EConversion.sUsdcgBtc:
                    return ECurrency.BTC;

                case EConversion.BtcsUsdcg:
                    return ECurrency.sUSDCG;

                case EConversion.BtcUsdcg:
                    return ECurrency.USDCG;

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