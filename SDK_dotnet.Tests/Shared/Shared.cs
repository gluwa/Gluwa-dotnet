using Gluwa.SDK_dotnet.Tests.Models;
using Gluwa.SDK_dotnet.Models.Exchange;
using NBitcoin;
using Nethereum.Signer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;

namespace Gluwa.SDK_dotnet.Tests
{
    public class Shared
    {
        public static bool BASE_ENV;
        public static string BASE_URL;
        public static string API_KEY;
        public static string API_SECRET;
        public static string TxID_sNGNG;
        public static string TxID_sUSDCG;
        public static string SRC_ADR_NGNG;
        public static string SRC_PRIVATE_NGNG;
        public static string SRC_ADR_sUSDCG;
        public static string SRC_PRIVATE_sUSDCG;
        public static string SRC_ADR_sNGNG;
        public static string SRC_PRIVATE_sNGNG;
        public static string SRC_ADR_BTC;
        public static string SRC_PRIVATE_BTC;
        public static string TRG_ADR_NGNG;
        public static string TRG_PRIVATE_NGNG;
        public static string TRG_ADR_sNGNG;
        public static string TRG_ADR_sUSDCG;
        public static string TRG_PRIVATE_sUSDCG;
        public static string TRG_ADR_BTC;
        public static string TRG_PRIVATE_BTC;
        public static string DEFAULT_PRIVATE;
        
        // ENV
        public static string BASE_URL_TEST = "https://api-test.gluwa.com/";
        public static string BASE_URL_PROD = "https://api.gluwa.com/";
        public static string WEBHOOK_URL = "https://gwebhookreceiver-test.azurewebsites.net/";

        // sUSDC-G
        public static string SRC_ADR_sUSDCG_PROD = "0xBBb8bbAF43fE8b9E5572B1860d5c94aC7ed87Bb9";
        public static string SRC_ADR_sUSDCG_SANDBOX = "0x28a8666e1a79d620395d80741e49101b39ef8d79";
        public static string TRG_ADR_sUSDCG_PROD = "0xA789E9E74559c5fd0AA82dEF71B261F1A18a9457";
        public static string TRG_ADR_sUSDCG_SANDBOX = "0xA789E9E74559c5fd0AA82dEF71B261F1A18a9457";
        public static string TxID_sUSDCG_SANDBOX = "0xd8ae3023dfaebb5bdf5dcf0e4f953fc168dbbcfcdaf2ceadf1df4873e0b91e4f";
        public static string TxID_sUSDCG_PROD = "0xc6a1d6d05349727b12dc604b6ef1239380326c997b0d7604432210bb0a44eb6a";
        public static string DEST_ADR_sUSDCG = "0x28A8666E1A79D620395D80741E49101B39ef8d79";
        public static string DEFAULT_sUSDCG_AMOUNT = "1";

        // NGNG
        public static string SRC_ADR_NGNG_PROD = "0xf04349b4a760f5aed02131e0daa9bb99a1d1d1e5";
        public static string SRC_ADR_NGNG_SANDBOX = "0xf04349b4a760f5aed02131e0daa9bb99a1d1d1e5";
        public static string TRG_ADR_NGNG_PROD = "0x1c7f28011825c047bf9aaeecfbcf65f387de489e";
        public static string TRG_ADR_NGNG_SANDBOX = "0x1c7f28011825c047bf9aaeecfbcf65f387de489e";
        public static string TxID_NGNG_SANDBOX = "0xd2b8d9bdfba88b6b98eb3d370aead3391d09170d2a36fda30586fa0f08c8cf57";

        // sNGNG
        public static string SRC_ADR_sNGNG_PROD = "0xf04349b4a760f5aed02131e0daa9bb99a1d1d1e5";
        public static string SRC_ADR_sNGNG_SANDBOX = "0xf04349b4a760f5aed02131e0daa9bb99a1d1d1e5";
        public static string TxID_sNGNG_PROD = "0x035a89ca4fef40749e7fa2b613720725e49c91523a92ec1a76c37d600c4870ad";
        public static string TxID_sNGNG_SANDBOX = "0xf3661faa3219cf99169800167713c1e6b3b1c4271ec3abf05fb044ac2389b658";
        public static string TRG_ADR_sNGNG_PROD = "0x1c7f28011825c047bf9aaeecfbcf65f387de489e";
        public static string TRG_ADR_sNGNG_SANDBOX = "0x1c7f28011825c047bf9aaeecfbcf65f387de489e";

        // BTC
        public static string SRC_ADR_BTC_PROD = "18xowhca6m6962Bun9wqYcCteFJc6ei4FT";
        public static string SRC_ADR_BTC_SANDBOX = "mndwpYmfJKamesjwDhRtQRFqdvcKB4R6LM";
        public static string TRG_ADR_BTC_PROD = "1QFCM6czj89szVTbUg7ZxzEz5tSJpzhCXR";
        public static string TRG_ADR_BTC_SANDBOX = "n4m9e9hyY9b8mbwDCF5wnuTJwt31o5j2Lu";
        public static string DEST_ADR_BTC = "n4m9e9hyY9b8mbwDCF5wnuTJwt31o5j2Lu";
        public static string DEFAULT_BTC_AMOUNT = "0.0001";

        // MISC
        public static string ORDER_ID = "93b34bc8-1f04-476d-81c5-9d65362e3274";
        public static string BTC_QUOTE_ID = "d3edb60e-60e4-45d3-a861-ada03a4e2f00";
        public static string WEBHOOK_USER_ID = "3f2b7db6-b51a-4f33-84fa-d48649ba2490";
        public static string EXCHANGE_ID = "F4910B98-903D-4CF3-AA29-F6DD370D8314";
        public static string EXECUTOR = "0x928ef75709Ea7e8D112ed63B5c041Dd9493F8448";
        public static string DEFAULT_TXNId = "0x415d2e4bff39bdf62b66fdb70dc2b0cfc3693a83695d675cf6d20d4d57933044";
        public static string DEFAULT_ADDRESS = "0xf04349B4A760F5Aed02131e0dAA9bB99a1d1d1e5";
        public static string DEFAULT_TRG_ADDRESS = "0xA789E9E74559c5fd0AA82dEF71B261F1A18a9457";
        public static string EXPIRYBLOCK_NO = "7951971";
        public static string FUNDS_ADR = "2N5dEVeDNcounwGvkSZrNUAu2EJJrAxJmaT";
        public static string FUNDS_SCRIPT = "2 03cbd409ce7452e1195c4dc46cf54870afdaadfa96c52ef7a25ac6b4234ef4a1b6 036b33928f204bdd0720ec164696267b4ea04581ba3b8aba292db6f1513f60a007 2 OP_CHECKMULTISIG";

        // INVALID TEST DATA
        public static string INVALID_ETH_ADDRESS = "eeb9eba1c49cda22d4a6b7c052d4ddd263bd90415835173548da";
        public static string INVALID_BTC_ADDRESS = "15uZ15ETVSrbPUZQxeM3WJ14UKrV6kQLby";
        public static string INVALID_PRIVATE_KEY = "0x535336d270b0b68eaa26c218cc11bfb97a4a14aa737fc62c50907d366fddffff";
        public static string INVALIDTXNID = "0xd8ae3023dfaebb5bdf5dcf0e4f953fc168dbbcfcdaf2ceadf1df4873e0bfffff";
        public static string INVALID_API_KEY = "e84a00d9-d732-4208-977a-c8c021f00777";
        public static string INVALID_API_SECRET = Guid.NewGuid().ToString();
        public static string INVALID_QUOTEID = "ee2cd48a-0000-0000-0000-000000000000";
        public static string INVALID_ORDERID = "ee2cd48a-1111-0000-0000-000000000000";
        public static string INVALID_BLOCK_NO = "111100000000000000000000";

        // SET ENVIRONMENT VARIABLES
        public static void SetEnvironmentVariables(string environmment = "Sandbox")
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").AddJsonFile("appsettings.dev.json",true).Build();
            DEFAULT_PRIVATE = config["DEFAULT_PRIVATE"];

            switch (environmment)
            {
                case "Production":
                    BASE_ENV = false;
                    BASE_URL = BASE_URL_PROD;
                    API_KEY = config["API_KEY_PROD"];
                    API_SECRET = config["API_SECRET_PROD"];
                    TxID_sNGNG = TxID_sNGNG_PROD;
                    TxID_sUSDCG = TxID_sUSDCG_PROD;

                    SRC_ADR_BTC = SRC_ADR_BTC_PROD;
                    SRC_ADR_sNGNG = SRC_ADR_sNGNG_PROD;
                    SRC_ADR_sUSDCG = SRC_ADR_sUSDCG_PROD;

                    SRC_PRIVATE_BTC = config["SRC_PRIVATE_BTC_PROD"];
                    SRC_PRIVATE_sNGNG = config["SRC_PRIVATE_sNGNG_PROD"];
                    SRC_PRIVATE_sUSDCG = config["SRC_PRIVATE_sUSDCG_PROD"];

                    TRG_ADR_BTC = TRG_ADR_BTC_PROD;
                    TRG_ADR_sNGNG = TRG_ADR_sNGNG_PROD;
                    TRG_ADR_sUSDCG = TRG_ADR_sUSDCG_PROD;

                    TRG_PRIVATE_BTC = config["TRG_PRIVATE_BTC_PROD"];
                    TRG_PRIVATE_sUSDCG = config["TRG_PRIVATE_sUSDCG_PROD"];
                    break;

                default:
                    BASE_ENV = true;
                    BASE_URL = BASE_URL_TEST;
                    API_KEY = config["API_KEY_SANDBOX"];
                    API_SECRET = config["API_SECRET_SANDBOX"];
                    TxID_sNGNG = TxID_sNGNG_SANDBOX;
                    TxID_sUSDCG = TxID_sUSDCG_SANDBOX;

                    SRC_ADR_BTC = SRC_ADR_BTC_SANDBOX;
                    SRC_ADR_NGNG = SRC_ADR_NGNG_SANDBOX;
                    SRC_ADR_sNGNG = SRC_ADR_sNGNG_SANDBOX;
                    SRC_ADR_sUSDCG = SRC_ADR_sUSDCG_SANDBOX;

                    SRC_PRIVATE_BTC = config["SRC_PRIVATE_BTC_SANDBOX"];
                    SRC_PRIVATE_NGNG = config["SRC_PRIVATE_NGNG_SANDBOX"];
                    SRC_PRIVATE_sNGNG = config["SRC_PRIVATE_sNGNG_SANDBOX"];
                    SRC_PRIVATE_sUSDCG = config["SRC_PRIVATE_sUSDCG_SANDBOX"];

                    TRG_ADR_BTC = TRG_ADR_BTC_SANDBOX;
                    TRG_ADR_NGNG = TRG_ADR_NGNG_SANDBOX;
                    TRG_ADR_sNGNG = TRG_ADR_sNGNG_SANDBOX;
                    TRG_ADR_sUSDCG = TRG_ADR_sUSDCG_SANDBOX;

                    TRG_PRIVATE_BTC = config["TRG_PRIVATE_BTC_SANDBOX"];
                    TRG_PRIVATE_sUSDCG = config["TRG_PRIVATE_sUSDCG_SANDBOX"];
                    break;
            }
        }

        // SHARED METHODS

        /// <summary>
        /// Returns generated X Request signature 
        /// </summary>
        /// <param name="privateKey"></param>
        /// <returns></returns>
        public static string GetXRequestSignature(string privateKey)
        {
            string xRequestSignature;
            long unixTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            string message = unixTimestamp.ToString();
            bool bGluwacoin = privateKey.StartsWith("0x");

            if (bGluwacoin == true)
            {
                // generate X-RequestSignature for gluwacoin (USD-G/KRW-G/NGN-G)
                EthereumMessageSigner signer = new EthereumMessageSigner();
                string signedMessage = signer.Sign(Encoding.ASCII.GetBytes(message), privateKey);

                byte[] plainTextBytes = System.Text.Encoding.ASCII.GetBytes(message + "." + signedMessage);
                xRequestSignature = System.Convert.ToBase64String(plainTextBytes);
            }
            else
            {
                // for btc check if it's in main or test
                string gluwaApi = $"{BASE_URL_TEST}";
                Network network = Network.Main;
                if (gluwaApi.Contains("test"))
                {
                    network = Network.TestNet;
                }

                // generate XRequestSignature for BTC
                BitcoinSecret secret = new BitcoinSecret(privateKey, network);
                string signedMessage = secret.PrivateKey.SignMessage(message);

                byte[] plainTextBytes = Encoding.ASCII.GetBytes($"{message}.{signedMessage}");
                xRequestSignature = Convert.ToBase64String(plainTextBytes);
            }
            return xRequestSignature;
        }


        /// <summary>
        /// Creates an order and asserts if order has been made
        /// </summary>
        /// <param name="sender">Sending Address information</param>
        /// <param name="receiver">Receiver Address information</param>
        /// <returns></returns>
        public static string PostOrder(EConversion conversion,
                                       string sourceAddress,
                                       string sourcePrivate,
                                       string targetAddress,
                                       string targetPrivate,
                                       string url,
                                       string amount)
        {
            // Get Exchange SetUp Method 
            PostOrderBody body = CreatePostOrderBody(conversion, sourceAddress, sourcePrivate, targetAddress, targetPrivate, amount);

            IRestResponse orderResponse = Api.GetResponse(Api.SetUrl(url, "v1/Orders"), Api.SendRequest(Method.POST, body));

           if (orderResponse.Content.Contains("NotEnoughBalance") || orderResponse.Content.Contains("not have enough funds"))
            {
                TestContext.WriteLine("NotEnoughBalance in SenderAddress. Insufficient funds.");
                Assert.Ignore("NotEnoughBalance in SenderAddress. Insufficient funds.");
            }

            return JObject.Parse(orderResponse.Content).SelectToken("ID").ToString();
        }


        /// <summary>
        /// Create body for POST v1/Orders
        /// </summary>
        /// <param name="conversion"></param>
        /// <param name="sender">AddressItem</param>
        /// <param name="receiver">AddressItem</param>
        /// <returns></returns>
        public static PostOrderBody CreatePostOrderBody(EConversion conversion,
                                                        string sourceAddress,
                                                        string sourcePrivate,
                                                        string targetAddress,
                                                        string targetPrivate,
                                                        string amount)
        {

            PostOrderBody body = new PostOrderBody()
            {
                Conversion = conversion.ToString(),
                SendingAddress = sourceAddress,
                SendingAddressSignature = GetXRequestSignature(sourcePrivate),
                ReceivingAddress = targetAddress,
                ReceivingAddressSignature = GetXRequestSignature(targetPrivate),
                SourceAmount = amount,
                Price = "1"

            };
            return body;
        }


        /// <summary>
        /// Returns a quote ID based on an address
        /// </summary>
        /// <returns></returns>
        public static string GetQuoteByAddress(string currency, string address, string privateKey)
        {
            // Arrange 
            IRestResponse response = Api.GetResponse(Api.SetUrlAndClient($"{BASE_URL_TEST}", $"v1/{currency}/Addresses/{address}/Quotes"),
                                     Api.SendRequest(Method.GET, privateKey));
            TestContext.WriteLine($"Response Content: \"{response.Content}\"");
            TestContext.WriteLine($"Response Status Code: {response.StatusCode}");

            JArray jArray = JArray.Parse(response.Content);
            JObject jObject = jArray.Children<JObject>().LastOrDefault();
            string quoteId = jObject.SelectToken("ID").ToString();

            return quoteId;
        }


        /// <summary>
        /// Calls the exchange request webhook endpoint
        /// </summary>
        /// <returns>Latest exchange made</returns>
        public static GetExchangeRequestResponse GetExchangeRequest(string conversion)
        {
            IRestResponse response = Api.GetResponse(Api.SetUrlAndClient(WEBHOOK_URL, "ExchangeRequest"),
                                                     Api.SendRequest(Method.GET)
                                                     .AddQueryParameter("userID", WEBHOOK_USER_ID)
                                                     .AddQueryParameter("limit", "10000"));

            TestContext.WriteLine($"ExchangeRequest response: {response.Content}");

            try
            {
                List<GetExchangeRequestResponse> exchangeRequestList = JsonConvert.DeserializeObject<List<GetExchangeRequestResponse>>(response.Content);

                List<GetExchangeRequestResponse> exchangeRequestListTest = exchangeRequestList.Where(er => er.Conversion == conversion).ToList();
                return exchangeRequestListTest[0];
            }
            catch (Exception ex)
            {
                TestContext.Error.WriteLine(ex);
            }

            return null;
        }


        /// <summary>
        /// Prepare quote request
        /// </summary>
        /// <param name="converse"></param>
        /// <param name="amount"></param>
        /// <param name="fee"></param>
        /// <returns></returns>
        public static AcceptExchangeRequest PrepareQuoteRequest(EConversion converse, string amount, string fee)
        {
            // Get exchange request
            var exchangeRequest = Shared.GetExchangeRequest(converse.ToString());
            var exchangeID = exchangeRequest.ID;
            var destAddress = exchangeRequest.DestinationAddress;
            var executor = exchangeRequest.Executor;
            var exblockNo = exchangeRequest.ExpiryBlockNumber;

            // String to Guid 
            Guid GuidID = Guid.Parse(exchangeID);

            // Add options
            AcceptExchangeRequest quoteRequest = new AcceptExchangeRequest()
            {
                ID = GuidID,                      
                Conversion = converse,            
                DestinationAddress = destAddress, 
                Executor = executor,              
                ExpiryBlockNumber = BigInteger.Parse(exblockNo), 
                SourceAmount = amount,            
                Fee = fee                  
            };

            return quoteRequest;
        }


        /// <summary>
        /// Get nonce with given length
        /// </summary>
        /// <param name="nonceDigits"></param>
        /// <returns></returns>
        public static string GetNonceString(int nonceDigits)
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buffer = new byte[8];
            StringBuilder sb = new StringBuilder();
            do
            {
                rng.GetBytes(buffer);
                string rngToString = (BitConverter.ToUInt64(buffer, 0)).ToString();
                sb.Append(rngToString);
            } while (sb.Length < nonceDigits);
            rng.Dispose();
            string nonceString = sb.ToString(0, nonceDigits);
            return nonceString;
        }
    }
}
