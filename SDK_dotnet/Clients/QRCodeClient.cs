using Gluwa.Error;
using Gluwa.Models;
using Gluwa.Utils;
using Nethereum.Signer;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Gluwa.Clients
{
    /// <summary>
    /// QRCodeClient generates payment QR code image.
    /// </summary>
    public sealed class QRCodeClient
    {
        private readonly string mApiKey;
        private readonly string mSecret;
        private readonly string mAddress;
        private readonly string mPrivateKey;
        private readonly string mBaseUrl;
        private readonly bool mbSandbox;

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="apiKey">Your API Key.</param>
        /// <param name="secret">Your API Secret.</param>
        /// <param name="address">Your public address.</param>
        /// <param name="privateKey">Your private Key.</param>
        /// <param name="bSandbox">Set to 'true' if using sandbox mode. Otherwise, 'false'</param>
        public QRCodeClient(
            string apiKey,
            string secret,
            string address,
            string privateKey,
            bool bSandbox)
        {
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new ArgumentNullException(nameof(apiKey));
            }
            else if (string.IsNullOrWhiteSpace(secret))
            {
                throw new ArgumentNullException(nameof(secret));
            }
            else if (string.IsNullOrWhiteSpace(address))
            {
                throw new ArgumentNullException(nameof(address));
            }
            else if (string.IsNullOrWhiteSpace(privateKey))
            {
                throw new ArgumentNullException(nameof(privateKey));
            }

            mApiKey = apiKey;
            mSecret = secret;
            mAddress = address;
            mPrivateKey = privateKey;
            mbSandbox = bSandbox;

            if (mbSandbox)
            {
                mBaseUrl = "https://sandbox.api.gluwa.com";
            }
            else
            {
                mBaseUrl = "https://api.gluwa.com";
            }
        }

        /// <summary>
        /// Generates a one-time use QR code for merchants, used for making a payment transaction. Returns an image in a .jpg or .png format.
        /// </summary>
        /// <param name="currency">Currency type.</param>
        /// <param name="amount">Payment amount. Fee will be deducted from this amount when payment request is made.</param>
        /// <param name="format">Desired image format, optional. Defaults to image/png</param>
        /// <param name="note">Additional information, used by the merchant user. optional.</param>
        /// <param name="merchantOrderID">Identifier for the payment, used by the merchant user. optional.</param>
        /// <param name="expiry">Time of expiry for the QR code in seconds. Payment request must be made with this QR code before this time. optional. Defaults to 1800</param>
        /// <response code="200">QR code image in a .png by default or .jpg depending on the format query parameter.</response> 
        /// <response code="400">Validation error. Please see inner errors for more details. or API Key and secret request header is missing or invalid.</response>        
        /// <response code="403">Combination of Api Key and Api Secret was not found.</response>        
        /// <response code="500">Server error.</response>
        /// <response code="503">Service unavailable for the provided currency.</response>
        public async Task<Result<byte[], PublicError>> GetPaymentQRCodeAsync(
            EPaymentCurrency? currency,
            string amount,
            string format = "image/png",
            string note = null,
            string merchantOrderID = null,
            int expiry = 1800
            )
        {
            if (currency == null)
            {
                throw new ArgumentNullException(nameof(currency));
            }
            else if (string.IsNullOrWhiteSpace(amount))
            {
                throw new ArgumentNullException(nameof(amount));
            }

            var result = new Result<byte[], PublicError>();

            var requestUri = $"{mBaseUrl}/v1/QRCode";

            var queryParams = new List<string>();
            if (format != null)
            {
                queryParams.Add($"format={format}");
                requestUri = $"{requestUri}?{string.Join("&", queryParams)}";
            }

            QRCodeRequest bodyParams = new QRCodeRequest()
            {
                Signature = getTimestampSignature(),
                Currency = currency,
                Target = mAddress,
                Amount = amount,
                Expiry = expiry,
                Note = note,
                MerchantOrderID = merchantOrderID
            };

            string json = bodyParams.ToJson();
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            byte[] authenticationBytes = Encoding.ASCII.GetBytes($"{mApiKey}:{mSecret}");
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic",
                     System.Convert.ToBase64String(authenticationBytes));
                using (HttpResponseMessage response = await httpClient.PostAsync(requestUri, content))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        result.IsSuccess = true;
                        result.Data = await response.Content.ReadAsByteArrayAsync();

                        return result;
                    }

                    string errorMessage = await response.Content.ReadAsStringAsync();
                    int statusCode = (int)response.StatusCode;

                    result.Error = new PublicError
                    {
                        Code = statusCode,
                        Message = errorMessage
                    };

                    return result;
                };
            }
        }

        private string getTimestampSignature()
        {
            var signer = new EthereumMessageSigner();
            string Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString();
            string signature = signer.EncodeUTF8AndSign(Timestamp, new EthECKey(mPrivateKey));

            string gluwaSignature = $"{Timestamp}.{signature}";
            byte[] gluwaSignatureByte = Encoding.UTF8.GetBytes(gluwaSignature);
            string encodedData = System.Convert.ToBase64String(gluwaSignatureByte);

            return encodedData;
        }
    }
}