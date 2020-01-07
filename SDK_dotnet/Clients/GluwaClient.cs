using Gluwa.Error;
using Gluwa.Models;
using Gluwa.Utils;
using Nethereum.ABI;
using Nethereum.Signer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Gluwa.Clients
{
    /// <summary>
    /// Client for public APIs
    /// </summary>
    public sealed class GluwaClient
    {
        private readonly bool mbSandbox;
        private readonly string mAddress;
        private readonly string mPrivateKey;
        private readonly string mBaseUrl;

        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="address">Your Gluwacoin public Address.</param>
        /// <param name="privateKey">Your Gluwacoin Private Key.</param>
        /// <param name="bSandbox">Set to 'true' if using sandbox mode. Otherwise, 'false'</param>
        public GluwaClient(
            string address,
            string privateKey,
            bool bSandbox)
        {
            if (string.IsNullOrWhiteSpace(address))
            {
                throw new ArgumentNullException(nameof(address));
            }
            else if (string.IsNullOrWhiteSpace(privateKey))
            {
                throw new ArgumentNullException(nameof(privateKey));
            }

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
        /// Get balance for specified currency.
        /// </summary>
        /// <param name="currency">Currency type.</param>
        /// <response code="200">Balance and associated currency.</response>
        /// <response code="400">Invalid address format.</response>
        /// <response code="500">Server error.</response>
        /// <response code="503">Service unavailable for the specified currency or temporarily.</response>
        public async Task<Result<BalanceResponse, PublicError>> GetBalanceAsync(ECurrency currency)
        {
            var result = new Result<BalanceResponse, PublicError>();
            string requestUri = $"{mBaseUrl}/v1/{currency}/Addressesa/{mAddress}";

            using (HttpClient httpClient = new HttpClient())
            using (HttpResponseMessage message = await httpClient.GetAsync(requestUri))
            {
                if (message.IsSuccessStatusCode)
                {
                    BalanceResponse balanceResponse = await message.Content.ReadAsAsync<BalanceResponse>();
                    result.IsSuccess = true;
                    result.Data = balanceResponse;

                    return result;
                }

                string errorMessage = await message.Content.ReadAsStringAsync();
                int statusCode = (int)message.StatusCode;

                result.Error = new PublicError
                {
                    Code = statusCode,
                    Message = errorMessage
                };

                return result;
            }
        }

        /// <summary>
        /// Get a list of transactions for specified currency.
        /// </summary>
        /// <param name="currency">Currency type.</param>
        /// <param name="limit">Number of transactions to include in the result. optional. Defaults to 100.</param> 
        /// <param name="status">Filter by transaction status. Optional. Defaults to Confimred.</param>
        /// <param name="offset">Number of transactions to skip; used for pagination. Optional. Default to 0.</param>
        /// <response code="200">List of transactions associated with the address.</response>
        /// <response code="400">Invalid request or Address does not have a valid format.</response>
        /// <response code="403">Request signature header is not valid.</response>
        /// <response code="500">Server error.</response>
        /// <response code="503">Service unavailable.</response>
        public async Task<Result<List<TransactionResponse>, PublicError>> GetTransactionListAsync(
            ECurrency currency,
            uint limit = 100,
            ETransactionStatusFilter status = ETransactionStatusFilter.Confirmed,
            uint offset = 0)
        {
            var result = new Result<List<TransactionResponse>, PublicError>();

            string requestUri = $"{mBaseUrl}/v1/{currency}/Addresses/{mAddress}/Transactions";

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

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("X-REQUEST-SIGNATURE", getTimestampSignature());
                using (HttpResponseMessage message = await httpClient.GetAsync(requestUri))
                {
                    if (message.IsSuccessStatusCode)
                    {
                        List<TransactionResponse> transactionResponse = await message.Content.ReadAsAsync<List<TransactionResponse>>();
                        result.IsSuccess = true;
                        result.Data = transactionResponse;

                        return result;
                    }

                    string errorMessage = await message.Content.ReadAsStringAsync();
                    int statusCode = (int)message.StatusCode;

                    result.Error = new PublicError
                    {
                        Code = statusCode,
                        Message = errorMessage
                    };

                    return result;
                }
            }
        }

        /// <summary>
        /// Get bitcoin or gluwacoin transaction by hash.
        /// </summary>
        /// <param name="currency">Currency type</param>
        /// <param name="txnHash">Hash of the transaction on the blockchain.</param>
        /// <response code="200">Transaction response.</response>
        /// <response code="400">Invalid transaction hash format.</response>
        /// <response code="403">Request signature header is not valid.</response>
        /// <response code="404">Tranasction not found.</response>
        /// <response code="500">Server error.</response>
        /// <response code="503">Service unavailable.</response>
        public async Task<Result<TransactionResponse, PublicError>> GetTransactionDetailsAsync(ECurrency currency, string txnHash)
        {
            if (string.IsNullOrWhiteSpace(txnHash))
            {
                throw new ArgumentNullException(nameof(txnHash));
            }
            var result = new Result<TransactionResponse, PublicError>();

            string requestUri = $"{mBaseUrl}/v1/{currency}/Transactions/{txnHash}";

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("X-REQUEST-SIGNATURE", getTimestampSignature());
                using (HttpResponseMessage message = await httpClient.GetAsync(requestUri))
                {
                    if (message.IsSuccessStatusCode)
                    {
                        TransactionResponse transactionResponse = await message.Content.ReadAsAsync<TransactionResponse>();
                        result.IsSuccess = true;
                        result.Data = transactionResponse;

                        return result;
                    }

                    string errorMessage = await message.Content.ReadAsStringAsync();
                    int statusCode = (int)message.StatusCode;

                    result.Error = new PublicError
                    {
                        Code = statusCode,
                        Message = errorMessage
                    };

                    return result;
                }
            }
        }

        /// <summary>
        /// Create a new Bitcoin or Gluwacoin transaction.
        /// </summary>
        /// <param name="currency">Currency type</param>
        /// <param name="amount">Transaction amount, not including the fee.</param>
        /// <param name="target">The address that the transaction will be sent to.</param>
        /// <param name="merchantOrderID">Identifier for the transaction that was provided by the merchant user. Optional.</param>
        /// <param name="note">Additional information about the transaction that a user can provide. Optional.</param>
        /// <param name="expiry">Expiry of the Transfer Request. Optional.</param>
        /// <response code="202">Newly accepted transaction.</response>
        /// <response code="400">Invalid request. or Validation error. See inner errors for more details. or (BTC only) Signed BTC transaction could not be verified.</response>
        /// <response code="403">For payments, payment signature could not be verified.</response>
        /// <response code="409">A transaction with the same transaction hash, payment ID, or idem already exists.</response>
        /// <response code="500">Server error.</response>
        /// <response code="503">Service unavailable.</response>
        public async Task<Result<bool, PublicError>> CreateTransactionAsync(
            ECurrency currency,
            string amount,
            string target,
            string merchantOrderID = null,
            string note = null,
            int expiry = 0)
        {
            if (string.IsNullOrWhiteSpace(amount))
            {
                throw new ArgumentNullException(nameof(amount));
            }
            else if (string.IsNullOrWhiteSpace(target))
            {
                throw new ArgumentNullException(nameof(target));
            }
            else if (string.IsNullOrWhiteSpace(getContractAddress(currency, mbSandbox)))
            {
                throw new ArgumentNullException(nameof(currency));
            }
            var result = new Result<bool, PublicError>();
            var requestUri = $"{mBaseUrl}/v1/Transactions";

            var getFee = getFeeAsync(currency).Result.Data.MinimumFee;

            BigInteger convertAmount = GluwacoinConverter.ConvertToGluwacoinBigInteger(amount);
            BigInteger convertFee = GluwacoinConverter.ConvertToGluwacoinBigInteger(getFee);
            int nonce = ((int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds);

            ABIEncode abiEncode = new ABIEncode();
            byte[] messageHash = abiEncode.GetSha3ABIEncodedPacked(
                new ABIValue("address", getContractAddress(currency, mbSandbox)),
                new ABIValue("address", mAddress),
                new ABIValue("address", target),
                new ABIValue("uint256", convertAmount),
                new ABIValue("uint256", convertFee),
                new ABIValue("uint256", nonce)
                );

            var signer = new EthereumMessageSigner();
            string addressRecovered = signer.Sign(messageHash, mPrivateKey);

            TransactionRequest bodyParams = new TransactionRequest
            {
                Signature = addressRecovered,
                Currency = currency,
                Target = target,
                Amount = amount,
                Fee = getFee,
                Source = mAddress,
                Nonce = nonce.ToString(),
                MerchantOrderID = merchantOrderID,
                Note = note
            };

            string json = bodyParams.ToJson();
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            using (HttpClient httpClient = new HttpClient())
            using (var response = await httpClient.PostAsync(requestUri, content))
            {
                if (response.IsSuccessStatusCode)
                {
                    result.IsSuccess = true;
                    result.Data = true;

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
            }
        }

        private string getContractAddress(ECurrency currency, bool mIsDevEnv)
        {
            switch (currency)
            {
                case ECurrency.USDG:
                    if (mIsDevEnv)
                    {
                        return "0x8e9611f8ebc9323EdDA39eA2d8F31bbb2436adEE";
                    }
                    else
                    {
                        return "0xfb0aaa0432112779d9ac483d9d5e3961ece18eec";
                    }
                case ECurrency.KRWG:
                    if (mIsDevEnv)
                    {
                        return "0x408b7959b3e15b8b1e8495fa9cb123c0180d44db";
                    }
                    else
                    {
                        return "0x4cc8486f2f3dce2d3b5e27057cf565e16906d12d";
                    }
                default:
                    throw new ArgumentOutOfRangeException($"Unsupported currency: {currency}");
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

        private async Task<Result<FeeResponse, PublicError>> getFeeAsync(ECurrency currency)
        {
            var result = new Result<FeeResponse, PublicError>();

            string requestUri = $"{mBaseUrl}/v1/{currency}/Fee";

            using (HttpClient httpClient = new HttpClient())
            using (HttpResponseMessage message = await httpClient.GetAsync(requestUri))
            {
                if (message.IsSuccessStatusCode)
                {
                    FeeResponse feeResponse = await message.Content.ReadAsAsync<FeeResponse>();
                    result.IsSuccess = true;
                    result.Data = feeResponse;

                    return result;
                }

                string errorMessage = await message.Content.ReadAsStringAsync();
                int statusCode = (int)message.StatusCode;

                result.Error = new PublicError
                {
                    Code = statusCode,
                    Message = errorMessage
                };

                return result;
            }
        }
    }
}