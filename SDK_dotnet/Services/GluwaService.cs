using Gluwa.SDK_dotnet.Models;
using NBitcoin;
using Nethereum.Signer;
using System;
using System.Text;

namespace Gluwa.SDK_dotnet.Utils
{
    internal static class GluwaService
    {
        internal static string GetAddressSignature(string privateKey, ECurrency currency, Environment environment)
        {
            if (string.IsNullOrWhiteSpace(privateKey))
            {
                throw new ArgumentNullException(nameof(privateKey));
            }

            string timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
            string signature = null;

            if (currency == ECurrency.BTC)
            {
                BitcoinSecret secret = new BitcoinSecret(privateKey, environment.Network);
                signature = secret.PrivateKey.SignMessage(timestamp);
            }
            else if (currency.IsGluwaCoinCurrency())
            {
                var signer = new EthereumMessageSigner();
                signature = signer.EncodeUTF8AndSign(timestamp, new EthECKey(privateKey));
            }
            else
            {
                throw new ArgumentOutOfRangeException($"Unsupported currency: {currency}");
            }

            string signatureToEncode = $"{timestamp}.{signature}";
            byte[] signatureByte = Encoding.UTF8.GetBytes(signatureToEncode);
            string encodedData = Convert.ToBase64String(signatureByte);

            return encodedData;
        }

        internal static string getGluwacoinContractAddress(ECurrency currency, Environment environment)
        {
            switch (currency)
            {
                case ECurrency.USDG:
                    return environment.UsdgContractAddress;

                case ECurrency.KRWG:
                    return environment.KrwgContractAddress;

                case ECurrency.NGNG:
                    return environment.NgngContractAddress;

                case ECurrency.sUSDCG:
                    return environment.SUsdcgContractAddress;

                case ECurrency.sNGNG:
                    return environment.SNgngContractAddress;

                default:
                    throw new ArgumentOutOfRangeException($"Unsupported currency: {currency}");
            }
        }
    }
}
