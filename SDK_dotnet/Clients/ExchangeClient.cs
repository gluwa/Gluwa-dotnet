using Gluwa.SDK_dotnet.Error;
using Gluwa.SDK_dotnet.Models;
using Gluwa.SDK_dotnet.Models.Exchange;
using Gluwa.SDK_dotnet.Utils;
using NBitcoin;
using Nethereum.ABI;
using Nethereum.Signer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Gluwa.SDK_dotnet.Clients
{
    /// <summary>
    /// Client for Exchange APIs
    /// </summary>
    public sealed class ExchangeClient
    {
        private readonly Environment mEnv;
        private readonly string X_REQUEST_SIGNATURE = "X-REQUEST-SIGNATURE";

        private readonly int MAX_UNSPENTOUTPUTS_COUNT = 5;

        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="bSandbox">Set to 'true' if using sandbox mode. Otherwise, 'false'</param>
        public ExchangeClient(
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
        public ExchangeClient(Environment env)
        {
            mEnv = env;
        }

        /// <summary>
        /// Get Quote for currency exchange.
        /// </summary>
        /// <param name="sendingAddressPrivateKey">The privateKey of the sending address.</param>
        /// <param name="receivingAddressPrivateKey">The privateKey of the receiving address.</param>
        /// <param name="quoteRequest">Request body.</param>
        /// <response code="200">Newly generated quote.</response>
        /// <response code="400_InvalidUrlParameters">Invalid URL parameters.</response>
        /// <response code="400_MissingBody">Request body is missing.</response>
        /// <response code="400_InvalidBody">Request validation errors. See InnerErrors.</response>
        /// <response code="400_ValidationError">Request validation errors. See InnerErrors.</response>
        /// <response code="404">No matching orders available.</response>
        /// <response code="500">Server error.</response>
        /// <returns></returns>
        public async Task<Result<QuoteResponse, ErrorResponse>> GetPendingQuoteAsync(
            string sendingAddressPrivateKey,
            string receivingAddressPrivateKey,
            GetPendingQuoteRequest quoteRequest)
        {
            #region
            IEnumerable<ValidationResult> validation = quoteRequest.Validate();

            if (validation.Any())
            {
                foreach (var item in validation)
                {
                    throw new ArgumentNullException(item.ErrorMessage);
                }
            }

            if (string.IsNullOrWhiteSpace(sendingAddressPrivateKey))
            {
                throw new ArgumentNullException(nameof(sendingAddressPrivateKey));
            }
            else if (string.IsNullOrWhiteSpace(receivingAddressPrivateKey))
            {
                throw new ArgumentNullException(nameof(receivingAddressPrivateKey));
            }
            #endregion

            var result = new Result<QuoteResponse, ErrorResponse>();
            string requestUri = $"{mEnv.BaseUrl}/v1/Quote";

            string btcPublicKey = null;

            if (quoteRequest.Conversion.Value.IsSourceCurrencyBtc())
            {
                btcPublicKey = Key.Parse(sendingAddressPrivateKey, mEnv.Network).PubKey.ToString();

            }

            string sendingAddressSignature = GluwaService.GetAddressSignature(sendingAddressPrivateKey, quoteRequest.Conversion.Value.ToSourceCurrency(), mEnv);
            string receivingAddressSignature = GluwaService.GetAddressSignature(receivingAddressPrivateKey, quoteRequest.Conversion.Value.ToTargetCurrency(), mEnv);

            PostQuoteRequest bodyParams = new PostQuoteRequest()
            {
                Amount = quoteRequest.Amount,
                Conversion = quoteRequest.Conversion,
                SendingAddress = quoteRequest.SendingAddress,
                SendingAddressSignature = sendingAddressSignature,
                ReceivingAddress = quoteRequest.ReceivingAddress,
                ReceivingAddressSignature = receivingAddressSignature,
                BtcPublicKey = btcPublicKey
            };

            string json = bodyParams.ToJson();
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                using (HttpClient httpClient = new HttpClient())
                using (HttpResponseMessage response = await httpClient.PostAsync(requestUri, content))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        QuoteResponse quoteResponse = await response.Content.ReadAsAsync<QuoteResponse>();
                        result.IsSuccess = true;
                        result.Data = quoteResponse;

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
        /// Accept quote received from POST /v1/Quote endpoint.
        /// </summary>
        /// <param name="currency">The source currency of the quote.</param>
        /// <param name="address">The sending address of the quote.</param>
        /// <param name="privateKey">The privateKey of the sending address.</param>
        /// <param name="quoteRequest">Request body.</param>
        /// <response code="202">Quote is accepted.</response>
        /// <response code="400_InvalidUrlParameters">Invalid URL parameters.</response>
        /// <response code="400_MissingBody">Request body is missing.</response>
        /// <response code="400_InvalidBody">Request validation errors. See InnerErrors.</response>
        /// <response code="400_ValidationError">Request validation errors. See InnerErrors.</response>
        /// <response code="403">Invalid checksum. Checksum may be wrong or expired.</response>
        /// <response code="404">One of the matched orders are no longer available.</response>
        /// <response code="500">Server error.</response>
        /// <returns></returns>
        public async Task<Result<AcceptQuoteResponse, ErrorResponse>> AcceptQuoteAsync(
            ECurrency currency,
            string address,
            string privateKey,
            AcceptQuoteRequest quoteRequest)
        {
            #region
            if (!currency.IsGluwaExchangeCurrency())
            {
                throw new ArgumentOutOfRangeException($"Unsupported currency: {currency}");
            }

            foreach (var order in quoteRequest.MatchedOrders)
            {
                IEnumerable<ValidationResult> validation = quoteRequest.Validate(currency);

                if (validation.Any())
                {
                    foreach (var item in validation)
                    {
                        throw new ArgumentNullException(item.ErrorMessage);
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(address))
            {
                throw new ArgumentNullException(nameof(address));
            }
            else if (string.IsNullOrWhiteSpace(privateKey))
            {
                throw new ArgumentNullException(nameof(privateKey));
            }
            #endregion

            var result = new Result<AcceptQuoteResponse, ErrorResponse>();
            string requestUri = $"{mEnv.BaseUrl}/v1/Quote";

            List<MatchedOrderRequest> matchedOrders = new List<MatchedOrderRequest>();
            foreach (var matchedOrder in quoteRequest.MatchedOrders)
            {
                if (currency.IsGluwaCoinCurrency())
                {
                    BigInteger nonce = new BigInteger(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
                    BigInteger convertExpiryBlockNumber = BigInteger.Parse(matchedOrder.ExpiryBlockNumber);

                    string signature = getGluwacoinReserveTxnSignature(
                        currency,
                        address,
                        matchedOrder.SourceAmount,
                        matchedOrder.Fee,
                        matchedOrder.DestinationAddress,
                        matchedOrder.Executor,
                        nonce,
                        convertExpiryBlockNumber,
                        privateKey);

                    MatchedOrderRequest matchedOrderRequest = new MatchedOrderRequest()
                    {
                        OrderID = matchedOrder.OrderID,
                        ReserveTxnSignature = signature,
                        Nonce = nonce.ToString()
                    };

                    matchedOrders.Add(matchedOrderRequest);
                }
                else
                {
                    BTCTxnSignature txnSignature = await getBtcTxnSignaturesAsync(
                        currency,
                        address,
                        matchedOrder.SourceAmount,
                        matchedOrder.Fee,
                        matchedOrder.DestinationAddress,
                        matchedOrder.ReservedFundsAddress,
                        matchedOrder.ReservedFundsRedeemScript,
                        privateKey);

                    MatchedOrderRequest matchedOrderRequest = new MatchedOrderRequest()
                    {
                        OrderID = matchedOrder.OrderID,
                        ReserveTxnSignature = txnSignature.ReserveTxnSignature,
                        ExecuteTxnSignature = txnSignature.ExecuteTxnSignature,
                        ReclaimTxnSignature = txnSignature.ReclaimTxnSignature
                    };

                    matchedOrders.Add(matchedOrderRequest);
                }
            }

            PutQuoteRequest bodyParams = new PutQuoteRequest()
            {
                MatchedOrders = matchedOrders,
                Checksum = quoteRequest.Checksum
            };

            string json = bodyParams.ToJson();
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                using (HttpClient httpClient = new HttpClient())
                using (HttpResponseMessage response = await httpClient.PutAsync(requestUri, content))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        AcceptQuoteResponse quoteResponse = await response.Content.ReadAsAsync<AcceptQuoteResponse>();
                        result.IsSuccess = true;
                        result.Data = quoteResponse;

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
        /// Get a list of accepted quotes.
        /// </summary>
        /// <param name="currency">The source currency of the quote.</param>
        /// <param name="address">The sending address of the quote.</param>
        /// <param name="privateKey">The privateKey of the sending address.</param>
        /// <response code="200">array of Quotes.</response>
        /// <response code="400_InvalidUrlParameters">Invalid URL parameters.</response>
        /// <response code="403_SignatureMissing">X-REQUEST-SIGNATURE header is missing.</response>
        /// <response code="403_SignatureExpired">X-REQUEST-SIGNATURE has expired.</response>
        /// <response code="403_InvalidSignature">Invalid X-REQUEST-SIGNATURE.</response>
        /// <response code="500">Server error.</response>
        /// <returns></returns>
        public async Task<Result<List<GetQuotesResponse>, ErrorResponse>> GetQuotesAsync(
            ECurrency currency,
            string address,
            string privateKey)
        {
            #region
            if (!currency.IsGluwaExchangeCurrency())
            {
                throw new ArgumentOutOfRangeException($"Unsupported currency: {currency}");
            }

            if (string.IsNullOrWhiteSpace(address))
            {
                throw new ArgumentNullException(nameof(address));
            }
            #endregion

            var result = new Result<List<GetQuotesResponse>, ErrorResponse>();
            string requestUri = $"{mEnv.BaseUrl}/v1/{currency}/Addresses/{address}/Quotes";

            var queryParams = new List<string>();
            GetQuotesOptions quoteRequest = new GetQuotesOptions();

            queryParams.Add($"offset={quoteRequest.Offset}");
            queryParams.Add($"limit={quoteRequest.Limit}");
            requestUri = $"{requestUri}?{string.Join("&", queryParams)}";

            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add(X_REQUEST_SIGNATURE, GluwaService.GetAddressSignature(privateKey, currency, mEnv));

                    using (HttpResponseMessage response = await httpClient.GetAsync(requestUri))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            List<GetQuotesResponse> quoteResponse = await response.Content.ReadAsAsync<List<GetQuotesResponse>>();
                            result.IsSuccess = true;
                            result.Data = quoteResponse;

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
        /// Get a list of accepted quotes.
        /// </summary>
        /// <param name="currency">The source currency of the quote.</param>
        /// <param name="address">The sending address of the quote.</param>
        /// <param name="privateKey">The privateKey of the sending address.</param>
        /// <param name="quoteRequest">Request body.</param>
        /// <response code="200">array of Quotes.</response>
        /// <response code="400_InvalidUrlParameters">Invalid URL parameters.</response>
        /// <response code="403_SignatureMissing">X-REQUEST-SIGNATURE header is missing.</response>
        /// <response code="403_SignatureExpired">X-REQUEST-SIGNATURE has expired.</response>
        /// <response code="403_InvalidSignature">Invalid X-REQUEST-SIGNATURE.</response>
        /// <response code="500">Server error.</response>
        /// <returns></returns>
        public async Task<Result<List<GetQuotesResponse>, ErrorResponse>> GetQuotesAsync(
            ECurrency currency,
            string address,
            string privateKey,
            GetQuotesOptions quoteRequest)
        {
            #region
            if (!currency.IsGluwaExchangeCurrency())
            {
                throw new ArgumentOutOfRangeException($"Unsupported currency: {currency}");
            }

            IEnumerable<ValidationResult> validation = quoteRequest.Validate();

            if (validation.Any())
            {
                foreach (var item in validation)
                {
                    throw new ArgumentNullException(item.ErrorMessage);
                }
            }

            if (string.IsNullOrWhiteSpace(address))
            {
                throw new ArgumentNullException(nameof(address));
            }
            #endregion

            var result = new Result<List<GetQuotesResponse>, ErrorResponse>();
            string requestUri = $"{mEnv.BaseUrl}/v1/{currency}/Addresses/{address}/Quotes";

            var queryParams = new List<string>();
            if (quoteRequest.StartDateTime.HasValue)
            {
                queryParams.Add($"startDateTime={quoteRequest.StartDateTime.Value.ToString("o")}");
            }
            if (quoteRequest.EndDateTime.HasValue)
            {
                queryParams.Add($"endDateTime={quoteRequest.EndDateTime.Value.ToString("o")}");
            }
            if (quoteRequest.Status.HasValue)
            {
                queryParams.Add($"status={quoteRequest.Status}");
            }

            queryParams.Add($"offset={quoteRequest.Offset}");
            queryParams.Add($"limit={quoteRequest.Limit}");
            requestUri = $"{requestUri}?{string.Join("&", queryParams)}";

            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add(X_REQUEST_SIGNATURE, GluwaService.GetAddressSignature(privateKey, currency, mEnv));

                    using (HttpResponseMessage response = await httpClient.GetAsync(requestUri))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            List<GetQuotesResponse> quoteResponse = await response.Content.ReadAsAsync<List<GetQuotesResponse>>();
                            result.IsSuccess = true;
                            result.Data = quoteResponse;

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
        /// Get an accepted quote with ID.
        /// </summary>
        /// <param name="currency">The source currency of the quote.</param>
        /// <param name="privateKey">The privateKey of the sending address of the quote.</param>
        /// <param name="ID">ID of the accepted quote.</param>
        /// <response code="200">The quote with a specified ID.</response>
        /// <response code="400_InvalidUrlParameters">Invalid URL parameters.</response>
        /// <response code="403_SignatureMissing">X-REQUEST-SIGNATURE header is missing.</response>
        /// <response code="403_SignatureExpired">X-REQUEST-SIGNATURE has expired.</response>
        /// <response code="403_InvalidSignature">Invalid X-REQUEST-SIGNATURE.</response>
        /// <response code="404">Quote is not found.</response>
        /// <response code="500">Server error.</response>
        /// <returns></returns>
        public async Task<Result<GetQuoteResponse, ErrorResponse>> GetQuoteAsync(ECurrency currency, string privateKey, Guid? ID)
        {
            #region
            if (!currency.IsGluwaExchangeCurrency())
            {
                throw new ArgumentOutOfRangeException($"Unsupported currency: {currency}");
            }

            if (ID == null || ID == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(ID));
            }
            #endregion

            var result = new Result<GetQuoteResponse, ErrorResponse>();
            string requestUri = $"{mEnv.BaseUrl}/v1/Quotes/{ID}";

            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add(X_REQUEST_SIGNATURE, GluwaService.GetAddressSignature(privateKey, currency, mEnv));

                    using (HttpResponseMessage response = await httpClient.GetAsync(requestUri))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            GetQuoteResponse quoteResponse = await response.Content.ReadAsAsync<GetQuoteResponse>();
                            result.IsSuccess = true;
                            result.Data = quoteResponse;

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

        private async Task<BTCTxnSignature> getBtcTxnSignaturesAsync(
            ECurrency currency,
            string address,
            string amount,
            string fee,
            string target,
            string reserve,
            string reservedFundsRedeemScript,
            string privateKey
            )
        {
            GluwaClient client = new GluwaClient(mEnv);

            Result<BalanceResponse, ErrorResponse> getUnspentOutput = await client.GetBalanceAsync(currency, address, true);
            List<UnspentOutput> unspentOutputs = getUnspentOutput.Data.UnspentOutputs.OrderByDescending(u => u.Amount).ToList();

            Money amountValue = Money.Parse(amount);
            Money feeValue = Money.Parse(fee);
            Money reserveAmount = amountValue + feeValue;
            Money totalRequiredAmountMoney = reserveAmount + feeValue;

            BigInteger totalRequiredAmount = new BigInteger(totalRequiredAmountMoney.Satoshi);

            BitcoinAddress sourceAddress = BitcoinAddress.Create(address, mEnv.Network);
            BitcoinAddress targetAddress = BitcoinAddress.Create(target, mEnv.Network);
            BitcoinAddress reserveAddress = BitcoinAddress.Create(reserve, mEnv.Network);

            BitcoinSecret secret = new BitcoinSecret(privateKey, mEnv.Network);

            List<UnspentOutput> usingUnspentOutputs = new List<UnspentOutput>();
            BigInteger unspentOutputTotalAmount = BigInteger.Zero;
            for (int i = 0; i < unspentOutputs.Count; i++)
            {
                if (unspentOutputTotalAmount < totalRequiredAmount && i >= MAX_UNSPENTOUTPUTS_COUNT)
                {
                    throw new InvalidOperationException($"Could not find up to {MAX_UNSPENTOUTPUTS_COUNT} BTC unspent outputs that can cover the amount and fee.");
                }

                if (unspentOutputTotalAmount >= totalRequiredAmount)
                {
                    break;
                }

                usingUnspentOutputs.Add(unspentOutputs[i]);
                Money sumAmount = Money.Parse(unspentOutputs[i].Amount);
                unspentOutputTotalAmount += new BigInteger(sumAmount.Satoshi);
            }

            List<Coin> coins = new List<Coin>();
            for (int i = 0; i < usingUnspentOutputs.Count; i++)
            {
                coins.Add(new Coin(
                    fromTxHash: new uint256(usingUnspentOutputs[i].TxHash),
                    fromOutputIndex: (uint)usingUnspentOutputs[i].Index,
                    amount: usingUnspentOutputs[i].Amount,
                    scriptPubKey: Script.FromHex(sourceAddress.ScriptPubKey.ToHex())
                ));
            }

            TransactionBuilder builder = mEnv.Network.CreateTransactionBuilder();

            NBitcoin.Transaction reserveTxSignature = builder
                            .AddKeys(secret)
                            .AddCoins(coins)
                            .Send(reserveAddress, reserveAmount)
                            .SetChange(sourceAddress)
                            .SendFees(fee)
                            .BuildTransaction(true);

            IEnumerable<Coin> reserveTxCoins = reserveTxSignature.Outputs.AsCoins();
            Coin reserveTxCoin = reserveTxCoins.First(
                c => c.TxOut.ScriptPubKey.GetDestinationAddress(mEnv.Network) == reserveAddress);
            Script reservedFundsRedeemScriptValue = new Script(reservedFundsRedeemScript);
            ScriptCoin reservedCoin = new ScriptCoin(reserveTxCoin, reservedFundsRedeemScriptValue);

            builder = mEnv.Network.CreateTransactionBuilder();
            PSBT executePsbt = builder
                .AddKeys(secret)
                .AddCoins(reservedCoin)
                .Send(targetAddress, amount)
                .SendFees(feeValue)
                .SetChange(reserveAddress)
                .BuildPSBT(true);

            builder = mEnv.Network.CreateTransactionBuilder();
            PSBT reclaimPsbt = builder
                .AddKeys(secret)
                .AddCoins(reservedCoin)
                .Send(sourceAddress, amount)
                .SendFees(feeValue)
                .SetChange(reserveAddress)
                .BuildPSBT(true);

            BTCTxnSignature bTCTxnSignature = new BTCTxnSignature()
            {
                ReserveTxnSignature = reserveTxSignature.ToHex(),
                ExecuteTxnSignature = executePsbt.ToHex(),
                ReclaimTxnSignature = reclaimPsbt.ToHex()
            };

            return bTCTxnSignature;
        }

        private string getGluwacoinReserveTxnSignature(
            ECurrency currency,
            string address,
            string amount,
            string fee,
            string target,
            string executor,
            BigInteger nonce,
            BigInteger expiryBlockNumber,
            string privateKey)
        {
            BigInteger convertAmount = GluwacoinConverter.ConvertToGluwacoinBigInteger(amount);
            BigInteger convertFee = GluwacoinConverter.ConvertToGluwacoinBigInteger(fee);

            ABIEncode abiEncode = new ABIEncode();
            byte[] messageHash = abiEncode.GetSha3ABIEncodedPacked(
                new ABIValue("address", GluwaService.getGluwacoinContractAddress(currency, mEnv)),
                new ABIValue("address", address),
                new ABIValue("address", target),
                new ABIValue("address", executor),
                new ABIValue("uint256", convertAmount),
                new ABIValue("uint256", convertFee),
                new ABIValue("uint256", nonce),
                new ABIValue("uint256", expiryBlockNumber)
                );

            EthereumMessageSigner signer = new EthereumMessageSigner();
            string signature = signer.Sign(messageHash, privateKey);

            return signature;
        }
    }
}
