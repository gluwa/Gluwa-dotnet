using Gluwa.SDK_dotnet.Clients;
using Gluwa.SDK_dotnet.Models;
using Gluwa.SDK_dotnet.Models.Exchange;
using Gluwa.SDK_dotnet.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using Assert = NUnit.Framework.Assert;
using DescriptionAttribute = NUnit.Framework.DescriptionAttribute;
using TestContext = NUnit.Framework.TestContext;

namespace ExchangeClientTests
{
    [TestFixture("Sandbox")]
    [TestFixture("Production")]
    public class GetQuotesTests
    {
        private readonly string useEnvironment;

        /// <summary>
        /// From TestFixture
        /// </summary>
        /// <param name="platform"></param>
        public GetQuotesTests(string useEnvironment)
        {
            this.useEnvironment = useEnvironment;
        }

        [SetUp]
        public void Setup()
        {
            Shared.SetEnvironmentVariables(useEnvironment);
        }

        [TestMethod]
        [Description("TestCaseId: C1671")]
        public async Task GetQuotes_Pos(ECurrency currency, string address, string privateKey)
        {
            // Arrange
            ExchangeClient exchangeClient = new ExchangeClient(Shared.BASE_ENV);

            // Call
            var result = await exchangeClient.GetQuotesAsync(currency, address, privateKey);

            // Assert
            TestContext.WriteLine($"Testing against: {useEnvironment}");
            TestContext.WriteLine(JsonConvert.SerializeObject(result));
            Assert.IsTrue(result.IsSuccess);
        }


        [Test]
        [Category("sUSDCG")]
        public async Task GetQuotes_sUsdcg_Pos()
        {
            await GetQuotes_Pos(ECurrency.sUSDCG, Shared.SRC_ADR_sUSDCG, Shared.SRC_PRIVATE_sUSDCG);
        }


        [Test]
        [Category("BTC")]
        public async Task GetQuotes_Btc_Pos()
        {
            await GetQuotes_Pos(ECurrency.BTC, Shared.SRC_ADR_BTC, Shared.SRC_PRIVATE_BTC);
        }


        [Test]
        [Description("TestCaseId: C1675")]
        public async Task GetQuotes_WithOptions_Pos()
        {
            // Arrange
            ExchangeClient exchangeClient = new ExchangeClient(Shared.BASE_ENV);

            // Add options
            GetQuotesOptions options = new GetQuotesOptions()
            {
                StartDateTime = new DateTime(2021, 01, 01),
                EndDateTime = new DateTime(2021, 12, 01),
                Status = EQuoteStatus.Pending,
                Offset = 0,
                Limit = 100
            };

            // Call
            var result = await exchangeClient.GetQuotesAsync(
                ECurrency.sUSDCG,
                Shared.SRC_ADR_sUSDCG,
                Shared.SRC_PRIVATE_sUSDCG,
                options
            );

            // Assert
            TestContext.WriteLine($"Testing against: {useEnvironment}");
            TestContext.WriteLine(JsonConvert.SerializeObject(result));
            Assert.IsTrue(result.IsSuccess);
        }


        [TestMethod]
        [Description("TestCaseId: C1683")]
        public async Task GetQuote_ByQuoteId_Pos(ECurrency currency, string privateKey, string quoteId)
        {
            // Arrange
            ExchangeClient exchangeClient = new ExchangeClient(Shared.BASE_ENV);

            // String to Guid
            var quoteIdGuid = Guid.Parse(quoteId);

            // Call
            var result = await exchangeClient.GetQuoteAsync(
                currency,                      // requied currency
                privateKey,                    // requied private key
                quoteIdGuid                    // requied quoteId
            );

            // Assert
            TestContext.WriteLine($"Testing against: {useEnvironment}");
            TestContext.WriteLine(JsonConvert.SerializeObject(result));
            Assert.AreEqual(null, result.Error.InnerErrors);
        }


        [Test]
        [Category("sUSDCG")]
        public async Task GetQuote_ByQuoteId_sUsdcg_Pos()
        {
            // Get QuoteId for currency and address
            string quoteId = Shared.GetQuoteByAddress("sUsdcg", Shared.SRC_ADR_sUSDCG, Shared.SRC_PRIVATE_sUSDCG);

            // Call
            await GetQuote_ByQuoteId_Pos(ECurrency.sUSDCG, Shared.SRC_PRIVATE_sUSDCG, quoteId);
        }


        [Test]
        [Category("BTC")]
        public async Task GetQuote_ByQuoteId_Btc_Pos()
        {
            string quoteId;

            // Get QuoteId for currency and address
            if (Shared.BASE_ENV)
            {
                quoteId = Shared.GetQuoteByAddress("btc", Shared.SRC_ADR_BTC, Shared.SRC_PRIVATE_BTC);
            }
            else
                quoteId = Shared.BTC_QUOTE_ID;

            // Call
            await GetQuote_ByQuoteId_Pos(ECurrency.BTC, Shared.SRC_PRIVATE_BTC, quoteId);
        }


        [Test]
        [Description("TestCaseId: C1672")]
        public async Task GetQuotes_InvalidAddress_Neg()
        {
            // Arrange
            ExchangeClient exchangeClient = new ExchangeClient(Shared.BASE_ENV);

            // Call
            var result = await exchangeClient.GetQuotesAsync(
                ECurrency.sUSDCG,
                Shared.INVALID_ETH_ADDRESS,         // invalid address
                Shared.SRC_PRIVATE_sUSDCG
            );


            // Assert
            TestContext.WriteLine($"Testing against: {useEnvironment}");
            TestContext.WriteLine(JsonConvert.SerializeObject(result));
            Assert.IsTrue(result.IsFailure);
            Assert.AreEqual("InvalidSignature", result.Error.Code);
            Assert.AreEqual("The supplied signature is not authorized for this resource.", result.Error.Message);
        }


        [Test]
        [Description("TestCaseId: C1673")]
        public async Task GetQuotes_InvalidPrivateKey_Neg()
        {
            // Arrange
            ExchangeClient exchangeClient = new ExchangeClient(Shared.BASE_ENV);

            // Call
            var result = await exchangeClient.GetQuotesAsync(
                ECurrency.sUSDCG,
                Shared.SRC_ADR_sUSDCG,
                Shared.INVALID_PRIVATE_KEY   // invalid private key
            );

            // Assert
            TestContext.WriteLine($"Testing against: {useEnvironment}");
            TestContext.WriteLine(JsonConvert.SerializeObject(result));
            Assert.IsTrue(result.IsFailure);
            Assert.AreEqual("InvalidSignature", result.Error.Code);
            Assert.AreEqual("The supplied signature is not authorized for this resource.", result.Error.Message);
        }


        [Test]
        [Description("TestCaseId: C1805")]
        public async Task GetQuote_ByQuoteId_InvalidKey_Neg()
        {
            // Arrange
            ExchangeClient exchangeClient = new ExchangeClient(Shared.BASE_ENV);

            // Get QuoteId for currency and address
            string quoteId = Shared.GetQuoteByAddress("sUsdcg", Shared.SRC_ADR_sUSDCG, Shared.SRC_PRIVATE_sUSDCG);

            // String to Guid
            var quoteIdGuid = Guid.Parse(quoteId);

            // Call
            var result = await exchangeClient.GetQuoteAsync(
                ECurrency.sUSDCG,
                Shared.INVALID_PRIVATE_KEY,       // invalid private key
                quoteIdGuid
            );

            // Assert
            TestContext.WriteLine($"Testing against: {useEnvironment}");
            TestContext.WriteLine(JsonConvert.SerializeObject(result));
            Assert.IsTrue(result.IsFailure);
        }


        [Test]
        [Description("TestCaseId: C1806")]
        public async Task GetQuote_ByQuoteId_InvalidQuoteId_Neg()
        {
            // Arrange
            ExchangeClient exchangeClient = new ExchangeClient(Shared.BASE_ENV);

            // String to Guid
            var quoteIdGuid = Guid.Parse(Shared.INVALID_QUOTEID);

            // Call
            var result = await exchangeClient.GetQuoteAsync(
                ECurrency.sUSDCG,
                Shared.SRC_PRIVATE_sUSDCG,
                quoteIdGuid                    // invalid quoteId
            );

            // Assert
            TestContext.WriteLine($"Testing against: {useEnvironment}");
            TestContext.WriteLine(JsonConvert.SerializeObject(result));
            Assert.IsTrue(result.IsFailure);
        }
    }
}
