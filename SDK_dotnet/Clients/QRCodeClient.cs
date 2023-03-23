using Gluwa.SDK_dotnet.Error;
using Gluwa.SDK_dotnet.Models;
using Gluwa.SDK_dotnet.Utils;
using Nethereum.Signer;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Gluwa.SDK_dotnet.Clients
{
    /// <summary>
    /// QRCodeClient generates payment QR code image.
    /// </summary>
    public sealed class QRCodeClient
    {
        private readonly Environment mEnv;

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="bTest">Set to 'true' if using test mode. Otherwise, 'false'</param>
        public QRCodeClient(bool bTest = false)
        {
            mEnv = bTest ? Environment.Test : Environment.Production;
        }

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="env"></param>
        public QRCodeClient(Environment env)
        {
            mEnv = env;
        }

        /// <summary>
        /// Generates a one-time use QR code for merchants, used for making a payment transaction. Returns an image in a .jpg or .png format.
        /// </summary>
        /// <param name="request">Request body.</param>
        /// <response code="200">QR code image in a .png by default or .jpg depending on the format query parameter.</response> 
        /// <response code="400">Validation error. Please see inner errors for more details. or API Key and secret request header is missing or invalid.</response>        
        /// <response code="403">Combination of Api Key and Api Secret was not found.</response>        
        /// <response code="500">Server error.</response>
        /// <response code="503">Service unavailable for the provided currency.</response>
        public async Task<Result<string, ErrorResponse>> GetPaymentQRCodeAsync(QRCodeRequest request)
        {
            request.Expiry = request.Expiry > 0 ? request.Expiry : 1800;
            validateParams(request);

            var result = new Result<string, ErrorResponse>();
            var requestUri = $"{mEnv.BaseUrl}/v1/QRCode";

            var queryParams = new List<string>();
            if (request.Format != null)
            {
                queryParams.Add($"format={request.Format}");
                requestUri = $"{requestUri}?{string.Join("&", queryParams)}";
            }

            QRCodeRequestBase bodyParams =
                generateBody(request.PrivateKey, request.Currency, request.Address, request.Amount, request.Note, request.MerchantOrderID, request.Expiry);

            string json = bodyParams.ToJson();
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            byte[] authenticationBytes = Encoding.ASCII.GetBytes($"{request.ApiKey}:{request.Secret}");
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Basic",
                            System.Convert.ToBase64String(authenticationBytes));
                    using (HttpResponseMessage response = await httpClient.PostAsync(requestUri, content))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            result.IsSuccess = true;
                            result.Data = await response.Content.ReadAsStringAsync();

                            return result;
                        }

                        string contentString = await response.Content.ReadAsStringAsync();
                        result.Error = ResponseHandler.GetError(response.StatusCode, requestUri, contentString);
                    }
                }
            }
            catch (HttpRequestException)
            {
                result.IsSuccess = false;
                result.Error = ResponseHandler.GetExceptionError();
            }

            return result;
        }

        /// <summary>
        /// Generates a one-time use QR code for merchants, used for making a payment transaction. Returns a Base64 string along with the QR code payload.
        /// </summary>
        /// <param name="request">Request body.</param>
        /// <response code="200">Base64 string and a QR code payload.</response> 
        /// <response code="400_ValidationError">Validation error. Please see inner errors for more details.</response>        
        /// <response code="400_BadRequestError">API Key and secret request header is missing or invalid.</response>        
        /// <response code="403_ForbiddenRequestError">Combination of Api Key and Api Secret was not found.</response>        
        /// <response code="500">Server error.</response>
        /// <response code="503">Service unavailable for the provided currency.</response>
        public async Task<Result<QRCodePayloadResponse, ErrorResponse>> GetPaymentQRCodePayLoadAsync(QRCodeRequest request)
        {
            validateParams(request);

            var result = new Result<QRCodePayloadResponse, ErrorResponse>();
            var requestUri = $"{mEnv.BaseUrl}/v1/QRCode/payload";

            QRCodeRequestBase bodyParams = generateBody(request.PrivateKey, request.Currency, request.Address, request.Amount, request.Note, request.MerchantOrderID, request.Expiry);

            string json = bodyParams.ToJson();
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            byte[] authenticationBytes = Encoding.ASCII.GetBytes($"{request.ApiKey}:{request.Secret}");
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic",
                         System.Convert.ToBase64String(authenticationBytes));
                    using (HttpResponseMessage response = await httpClient.PostAsync(requestUri, content))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            result.IsSuccess = true;
                            result.Data = await response.Content.ReadAsAsync<QRCodePayloadResponse>();

                            return result;
                        }

                        string contentString = await response.Content.ReadAsStringAsync();
                        result.Error = ResponseHandler.GetError(response.StatusCode, requestUri, contentString);
                    }
                }
            }
            catch (HttpRequestException)
            {
                result.IsSuccess = false;
                result.Error = ResponseHandler.GetExceptionError();
            }

            return result;
        }

        private void validateParams(QRCodeRequest qrCodeRequest)
        {
            if (string.IsNullOrWhiteSpace(qrCodeRequest.ApiKey))
                throw new InvalidOperationException(nameof(qrCodeRequest.ApiKey));

            if (string.IsNullOrWhiteSpace(qrCodeRequest.Secret))
                throw new InvalidOperationException(nameof(qrCodeRequest.Secret));

            if (string.IsNullOrWhiteSpace(qrCodeRequest.Address))
                throw new InvalidOperationException(nameof(qrCodeRequest.Address));

            if (string.IsNullOrWhiteSpace(qrCodeRequest.PrivateKey))
                throw new InvalidOperationException(nameof(qrCodeRequest.PrivateKey));

            if (string.IsNullOrWhiteSpace(qrCodeRequest.Amount))
                throw new InvalidOperationException(nameof(qrCodeRequest.Amount));
        }

        private QRCodeRequestBase generateBody(string privateKey, EPaymentCurrency? currency, string address, string amount, string note, string merchantOrderID, int? expiry)
        {
            return new QRCodeRequestBase()
            {
                Signature = getTimestampSignature(privateKey),
                Currency = currency,
                Target = address,
                Amount = amount,
                Expiry = expiry,
                Note = note,
                MerchantOrderID = merchantOrderID
            };
        }

        private string getTimestampSignature(string privateKey)
        {
            var signer = new EthereumMessageSigner();
            string Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString();
            string signature = signer.EncodeUTF8AndSign(Timestamp, new EthECKey(privateKey));

            string gluwaSignature = $"{Timestamp}.{signature}";
            byte[] gluwaSignatureByte = Encoding.UTF8.GetBytes(gluwaSignature);
            string encodedData = System.Convert.ToBase64String(gluwaSignatureByte);

            return encodedData;
        }
    }
}