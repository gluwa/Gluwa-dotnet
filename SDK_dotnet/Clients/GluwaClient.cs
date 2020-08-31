using Gluwa.SDK_dotnet.Error;
using Gluwa.SDK_dotnet.Models;
using Gluwa.SDK_dotnet.Utils;
using Nethereum.ABI;
using Nethereum.Signer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Gluwa.SDK_dotnet.Clients
{
    /// <summary>
    /// Client for public APIs
    /// </summary>
    public sealed class GluwaClient
    {
        private readonly Environment mEnv;

        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="bSandbox">Set to 'true' if using sandbox mode. Otherwise, 'false'</param>
        public GluwaClient(
            bool bSandbox = false)
        {
            if (bSandbox)
            {
                mEnv = Environment.Sandbox;
            }
            else
            {
                mEnv = Environment.Production;
            }
        }

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="env"></param>
        public GluwaClient(Environment env)
        {
            mEnv = env;
        }

        /// <summary>
        /// Get balance for specified currency.
        /// </summary>
        /// <param name="currency">Currency type.</param>
        /// <param name="address">Your Gluwacoin public Address.</param>
        /// <response code="200">Balance and associated currency.</response>
        /// <response code="400">Invalid address format.</response>
        /// <response code="500">Server error.</response>
        /// <response code="503">Service unavailable for the specified currency or temporarily.</response>
        public async Task<Result<BalanceResponse, ErrorResponse>> GetBalanceAsync(ECurrency currency, string address)
        {
            if (string.IsNullOrWhiteSpace(address))
            {
                throw new ArgumentNullException(nameof(address));
            }

            var result = new Result<BalanceResponse, ErrorResponse>();
            string requestUri = $"{mEnv.BaseUrl}/v1/{currency}/Addresses/{address}";

            try
            {
                using (HttpClient httpClient = new HttpClient())
                using (HttpResponseMessage response = await httpClient.GetAsync(requestUri))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        BalanceResponse balanceResponse = await response.Content.ReadAsAsync<BalanceResponse>();
                        result.IsSuccess = true;
                        result.Data = balanceResponse;

                        return result;
                    }

                    string contentString = await response.Content.ReadAsStringAsync();
                    result.Error = ResponseHandler.GetError(response.StatusCode, requestUri, contentString);
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
        /// Get a list of transactions for specified currency.
        /// </summary>
        /// <param name="currency">Currency type.</param>
        /// <param name="address">Your Gluwacoin public Address.</param>
        /// <param name="privateKey">Your Gluwacoin Private Key.</param>
        /// <param name="limit">Number of transactions to include in the result. optional. Defaults to 100.</param> 
        /// <param name="status">Filter by transaction status. Optional. Defaults to Confimred.</param>
        /// <param name="offset">Number of transactions to skip; used for pagination. Optional. Default to 0.</param>
        /// <response code="200">List of transactions associated with the address.</response>
        /// <response code="400">Invalid request or Address does not have a valid format.</response>
        /// <response code="403">Request signature header is not valid.</response>
        /// <response code="500">Server error.</response>
        /// <response code="503">Service unavailable.</response>
        public async Task<Result<List<TransactionResponse>, ErrorResponse>> GetTransactionListAsync(
           ECurrency currency,
           string address,
           string privateKey,
           uint limit = 100,
           ETransactionStatusFilter status = ETransactionStatusFilter.Confirmed,
           uint offset = 0)
        {
            if (string.IsNullOrWhiteSpace(address))
            {
                throw new ArgumentNullException(nameof(address));
            }
            else if (string.IsNullOrWhiteSpace(privateKey))
            {
                throw new ArgumentNullException(nameof(privateKey));
            }

            var result = new Result<List<TransactionResponse>, ErrorResponse>();
            string requestUri = $"{mEnv.BaseUrl}/v1/{currency}/Addresses/{address}/Transactions";

            var queryParams = new List<string>();
            if (offset > 0)
            {
                queryParams.Add($"offset={offset}");
            }
            if (limit > 0)
            {
                queryParams.Add($"limit={limit}");
            }
            if (queryParams.Any())
            {
                queryParams.Add($"status={status}");
                requestUri = $"{requestUri}?{string.Join("&", queryParams)}";
            }

            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("X-REQUEST-SIGNATURE", getTimestampSignature(privateKey));
                    using (HttpResponseMessage response = await httpClient.GetAsync(requestUri))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            List<TransactionResponse> transactionResponse = await response.Content.ReadAsAsync<List<TransactionResponse>>();
                            result.IsSuccess = true;
                            result.Data = transactionResponse;

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
        /// Get bitcoin or gluwacoin transaction by hash.
        /// </summary>
        /// <param name="currency">Currency type</param>
        /// <param name="privateKey">Your Gluwacoin Private Key.</param>
        /// <param name="txnHash">Hash of the transaction on the blockchain.</param>
        /// <response code="200">Transaction response.</response>
        /// <response code="400">Invalid transaction hash format.</response>
        /// <response code="403">Request signature header is not valid.</response>
        /// <response code="404">Tranasction not found.</response>
        /// <response code="500">Server error.</response>
        /// <response code="503">Service unavailable.</response>
        public async Task<Result<TransactionResponse, ErrorResponse>> GetTransactionDetailsAsync(ECurrency currency, string privateKey, string txnHash)
        {
            if (string.IsNullOrWhiteSpace(privateKey))
            {
                throw new ArgumentNullException(nameof(privateKey));
            }
            else if (string.IsNullOrWhiteSpace(txnHash))
            {
                throw new ArgumentNullException(nameof(txnHash));
            }

            var result = new Result<TransactionResponse, ErrorResponse>();
            string requestUri = $"{mEnv.BaseUrl}/v1/{currency}/Transactions/{txnHash}";

            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("X-REQUEST-SIGNATURE", getTimestampSignature(privateKey));
                    using (HttpResponseMessage response = await httpClient.GetAsync(requestUri))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            TransactionResponse transactionResponse = await response.Content.ReadAsAsync<TransactionResponse>();
                            result.IsSuccess = true;
                            result.Data = transactionResponse;

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
        /// Create a new Bitcoin or Gluwacoin transaction.
        /// </summary>
        /// <param name="currency">Currency type</param>
        /// <param name="address">Your Gluwacoin public Address.</param>
        /// <param name="privateKey">Your Gluwacoin Private Key.</param>
        /// <param name="amount">Transaction amount, not including the fee.</param>
        /// <param name="target">The address that the transaction will be sent to.</param>
        /// <param name="merchantOrderID">Identifier for the transaction that was provided by the merchant user. Optional.</param>
        /// <param name="note">Additional information about the transaction that a user can provide. Optional.</param>
        /// <param name="nonce">Nonce for the transaction. For Gluwacoin currencies only.</param>
        /// <param name="idem">Idempotent key for the transaction to prevent duplicate transactions.</param>
        /// <param name="paymentID">ID for the QR code payment.</param>
        /// <param name="paymentSig">Signature of the QR code payment.Required if PaymentID is not null.</param>
        /// <response code="202">Newly accepted transaction.</response>
        /// <response code="400">Invalid request. or Validation error. See inner errors for more details. or (BTC only) Signed BTC transaction could not be verified.</response>
        /// <response code="403">For payments, payment signature could not be verified.</response>
        /// <response code="409">A transaction with the same transaction hash, payment ID, or idem already exists.</response>
        /// <response code="500">Server error.</response>
        /// <response code="503">Service unavailable.</response>
        public async Task<Result<bool, ErrorResponse>> CreateTransactionAsync(
            ECurrency currency,
            string address,
            string privateKey,
            string amount,
            string target,
            string merchantOrderID = null,
            string note = null,
            string nonce = null,
            Guid? idem = null,
            Guid? paymentID = null,
            string paymentSig = null)
        {
            if (string.IsNullOrWhiteSpace(address))
            {
                throw new ArgumentNullException(nameof(address));
            }
            else if (string.IsNullOrWhiteSpace(privateKey))
            {
                throw new ArgumentNullException(nameof(privateKey));
            }
            else if (string.IsNullOrWhiteSpace(amount))
            {
                throw new ArgumentNullException(nameof(amount));
            }
            else if (string.IsNullOrWhiteSpace(target))
            {
                throw new ArgumentNullException(nameof(target));
            }
            else if (string.IsNullOrWhiteSpace(getContractAddress(currency)))
            {
                throw new ArgumentNullException(nameof(currency));
            }

            var result = new Result<bool, ErrorResponse>();
            var requestUri = $"{mEnv.BaseUrl}/v1/Transactions";

            Result<FeeResponse, ErrorResponse> getFee = await getFeeAsync(currency);
            if (getFee.IsFailure)
            {
                result.Error = getFee.Error;

                return result;
            }

            BigInteger convertAmount = GluwacoinConverter.ConvertToGluwacoinBigInteger(amount);
            BigInteger convertFee = GluwacoinConverter.ConvertToGluwacoinBigInteger(getFee.Data.MinimumFee.ToString());
            if (nonce == null)
            {
                nonce = ((int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds).ToString();
            }

            ABIEncode abiEncode = new ABIEncode();
            byte[] messageHash = abiEncode.GetSha3ABIEncodedPacked(
                new ABIValue("address", getContractAddress(currency)),
                new ABIValue("address", address),
                new ABIValue("address", target),
                new ABIValue("uint256", convertAmount),
                new ABIValue("uint256", convertFee),
                new ABIValue("uint256", int.Parse(nonce))
                );

            var signer = new EthereumMessageSigner();
            string addressRecovered = signer.Sign(messageHash, privateKey);

            TransactionRequest bodyParams = new TransactionRequest
            {
                Signature = addressRecovered,
                Currency = currency,
                Target = target,
                Amount = amount,
                Fee = getFee.Data.MinimumFee,
                Source = address,
                Nonce = nonce.ToString(),
                MerchantOrderID = merchantOrderID,
                Note = note,
                Idem = idem,
                PaymentID = paymentID,
                PaymentSig = paymentSig
            };

            string json = bodyParams.ToJson();
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                using (HttpClient httpClient = new HttpClient())
                using (var response = await httpClient.PostAsync(requestUri, content))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        result.IsSuccess = true;
                        result.Data = true;

                        return result;
                    }

                    string contentString = await response.Content.ReadAsStringAsync();
                    result.Error = ResponseHandler.GetError(response.StatusCode, requestUri, contentString);
                }
            }
            catch (HttpRequestException)
            {
                result.IsSuccess = false;
                result.Error = ResponseHandler.GetExceptionError();
            }
            return result;
        }

        private string getContractAddress(ECurrency currency)
        {
            switch (currency)
            {
                case ECurrency.USDG:
                    return mEnv.UsdgContractAddress;

                case ECurrency.KRWG:
                    return mEnv.KrwgContractAddress;

                case ECurrency.NGNG:
                    return mEnv.NgngContractAddress;

                default:
                    throw new ArgumentOutOfRangeException($"Unsupported currency: {currency}");
            }
        }

        private string getTimestampSignature(string privateKey)
        {
            if (string.IsNullOrWhiteSpace(privateKey))
            {
                throw new ArgumentNullException(nameof(privateKey));
            }

            var signer = new EthereumMessageSigner();
            string Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString();
            string signature = signer.EncodeUTF8AndSign(Timestamp, new EthECKey(privateKey));

            string gluwaSignature = $"{Timestamp}.{signature}";
            byte[] gluwaSignatureByte = Encoding.UTF8.GetBytes(gluwaSignature);
            string encodedData = Convert.ToBase64String(gluwaSignatureByte);

            return encodedData;
        }

        private async Task<Result<FeeResponse, ErrorResponse>> getFeeAsync(ECurrency currency)
        {
            var result = new Result<FeeResponse, ErrorResponse>();
            string requestUri = $"{mEnv.BaseUrl}/v1/{currency}/Fee";

            try
            {
                using (HttpClient httpClient = new HttpClient())
                using (HttpResponseMessage response = await httpClient.GetAsync(requestUri))
                {
                    FeeResponse feeResponse = await response.Content.ReadAsAsync<FeeResponse>();

                    if (response.IsSuccessStatusCode)
                    {
                        result.IsSuccess = true;
                        result.Data = feeResponse;

                        return result;
                    }

                    string contentString = await response.Content.ReadAsStringAsync();
                    result.Error = ResponseHandler.GetError(response.StatusCode, requestUri, contentString);
                }
            }
            catch (HttpRequestException)
            {
                result.IsSuccess = false;
                result.Error = ResponseHandler.GetExceptionError();
            }
            return result;
        }
    }
}