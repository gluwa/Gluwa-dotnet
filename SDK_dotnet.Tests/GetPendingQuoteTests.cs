using Gluwa.SDK_dotnet.Clients;
using Gluwa.SDK_dotnet.Models.Exchange;
using Gluwa.SDK_dotnet.Tests;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Threading.Tasks;
using Assert = NUnit.Framework.Assert;
using DescriptionAttribute = NUnit.Framework.DescriptionAttribute;
using TestContext = NUnit.Framework.TestContext;

namespace ExchangeClientTests
{
    [TestFixture("Test")]
    //[TestFixture("Production")]
    class GetPendingQuoteTests
    {
        private readonly string useEnvironment;

        /// <summary>
        /// From TestFixture
        /// </summary>
        /// <param name="platform"></param>
        public GetPendingQuoteTests(string useEnvironment)
        {
            this.useEnvironment = useEnvironment;
        }

        [SetUp]
        public void Setup()
        {
            Shared.SetEnvironmentVariables(useEnvironment);
        }

        [Test]
        [Description("TestCaseId: C1676")]
        public async Task GetPendingQuote_BtcsUsdcg_Pos()
        {
            // Arrange
            ExchangeClient exchangeClient = new ExchangeClient(true);

            GetPendingQuoteRequest quoteRequest = new GetPendingQuoteRequest()
            {
                Amount = "1",
                Conversion = EConversion.BtcsUsdcg,
                SendingAddress = Shared.SRC_ADR_BTC,          // requied sender address
                ReceivingAddress = Shared.TRG_ADR_sUSDCG      // requied receiver address
            };

            // Call
            var result = await exchangeClient.GetPendingQuoteAsync(
                Shared.SRC_PRIVATE_BTC,                       // requied sender private key
                Shared.TRG_PRIVATE_sUSDCG,                    // requied receiver private key
                quoteRequest
            );

            // Assert
            TestContext.WriteLine($"Testing against: {useEnvironment}");
            TestContext.WriteLine(JsonConvert.SerializeObject(result));
            Assert.AreEqual(null, result.Error.InnerErrors);
        }


        [Test]
        [Description("TestCaseId: C1676")]
        public async Task GetPendingQuote_sUsdcgBtc_Pos()
        {
            // Arrange
            ExchangeClient exchangeClient = new ExchangeClient(true);

            GetPendingQuoteRequest quoteRequest = new GetPendingQuoteRequest()
            {
                Amount = "1",
                Conversion = EConversion.sUsdcgBtc,
                SendingAddress = Shared.SRC_ADR_sUSDCG,    // requied sender address
                ReceivingAddress = Shared.TRG_ADR_BTC      // requied receiver address
            };

            // Call
            var result = await exchangeClient.GetPendingQuoteAsync(
                Shared.SRC_PRIVATE_sUSDCG,                 // requied sender private key
                Shared.TRG_PRIVATE_BTC,                    // requied receiver private key
                quoteRequest
            );

            // Assert
            TestContext.WriteLine($"Testing against: {useEnvironment}");
            TestContext.WriteLine(JsonConvert.SerializeObject(result));
            if(result.Error.Message.Contains("Could not find orders"))
            {
                Assert.Inconclusive("Could not find orders");
            }
            else {
                Assert.AreEqual(null, result.Error);
            }
        }


        [Test]
        [Description("TestCaseId: C1677")]
        public async Task GetPendingQuote_InvalidSenderKey_Neg()
        {
            // Arrange
            ExchangeClient exchangeClient = new ExchangeClient(true);

            GetPendingQuoteRequest quoteRequest = new GetPendingQuoteRequest()
            {
                Amount = "1",
                Conversion = EConversion.sUsdcgBtc,
                SendingAddress = Shared.SRC_ADR_sUSDCG,    
                ReceivingAddress = Shared.TRG_ADR_BTC     
            };

            // Call
            var result = await exchangeClient.GetPendingQuoteAsync(
                Shared.INVALID_PRIVATE_KEY,     // invalid sender private key
                Shared.TRG_PRIVATE_BTC,                    
                quoteRequest
            );

            // Assert
            TestContext.WriteLine($"Testing against: {useEnvironment}");
            TestContext.WriteLine(JsonConvert.SerializeObject(result));
            Assert.IsTrue(result.IsFailure);
            Assert.AreEqual("ValidationError", result.Error.Code);
            Assert.AreEqual("There are one or more validation errors. See InnerErrors for more details", result.Error.Message);
            Assert.AreEqual("AddressSignatureInvalid", result.Error.InnerErrors[0].Code);
            Assert.AreEqual("SendingAddressSignature is not valid for the provided SendingAddress.", result.Error.InnerErrors[0].Message);
        }


        [Test]
        [Description("TestCaseId: C1678")]
        public async Task GetPendingQuote_InvalidReceiverKey_Neg()
        {
            // Arrange
            ExchangeClient exchangeClient = new ExchangeClient(true);

            GetPendingQuoteRequest quoteRequest = new GetPendingQuoteRequest()
            {
                Amount = "1",
                Conversion = EConversion.BtcsUsdcg,
                SendingAddress = Shared.SRC_ADR_BTC,          
                ReceivingAddress = Shared.TRG_ADR_sUSDCG      
            };

            // Call
            var result = await exchangeClient.GetPendingQuoteAsync(
                Shared.SRC_PRIVATE_BTC,                     
                Shared.INVALID_PRIVATE_KEY,            // invalid receiver private key
                quoteRequest
            );

            // Assert
            TestContext.WriteLine($"Testing against: {useEnvironment}");
            TestContext.WriteLine(JsonConvert.SerializeObject(result));
            Assert.IsTrue(result.IsFailure);
            Assert.AreEqual("ValidationError", result.Error.Code);
            Assert.AreEqual("There are one or more validation errors. See InnerErrors for more details", result.Error.Message);
            Assert.AreEqual("AddressSignatureInvalid", result.Error.InnerErrors[0].Code);
            Assert.AreEqual("ReceivingAddressSignature is not valid for the provided ReceivingAddress.", result.Error.InnerErrors[0].Message);
        }


        [Test]
        [Description("TestCaseId: C1679")]
        public async Task GetPendingQuote_InvalidSenderAddress_Neg()
        {
            // Arrange
            ExchangeClient exchangeClient = new ExchangeClient(true);

            GetPendingQuoteRequest quoteRequest = new GetPendingQuoteRequest()
            {
                Amount = "1",
                Conversion = EConversion.sUsdcgBtc,
                SendingAddress = Shared.INVALID_ETH_ADDRESS,  // invalid sender address
                ReceivingAddress = Shared.TRG_ADR_BTC
            };

            // Call
            var result = await exchangeClient.GetPendingQuoteAsync(
                Shared.SRC_PRIVATE_sUSDCG,
                Shared.TRG_PRIVATE_BTC,
                quoteRequest
            );

            // Assert
            TestContext.WriteLine($"Testing against: {useEnvironment}");
            TestContext.WriteLine(JsonConvert.SerializeObject(result));
            Assert.IsTrue(result.IsFailure);
            Assert.AreEqual("ValidationError", result.Error.Code);
            Assert.AreEqual("There are one or more validation errors. See InnerErrors for more details", result.Error.Message);
            Assert.AreEqual("AddressSignatureInvalid", result.Error.InnerErrors[0].Code);
            Assert.AreEqual("SendingAddressSignature is not valid for the provided SendingAddress.", result.Error.InnerErrors[0].Message);
        }


        [Test]
        [Description("TestCaseId: C1680")]
        public async Task GetPendingQuote_InvalidRecieverAddress_Neg()
        {
            // Arrange
            ExchangeClient exchangeClient = new ExchangeClient(true);

            GetPendingQuoteRequest quoteRequest = new GetPendingQuoteRequest()
            {
                Amount = "1",
                Conversion = EConversion.BtcsUsdcg,
                SendingAddress = Shared.SRC_ADR_BTC,          
                ReceivingAddress = Shared.INVALID_ETH_ADDRESS      // invalid receiver address
            };

            // Call
            var result = await exchangeClient.GetPendingQuoteAsync(
                Shared.SRC_PRIVATE_BTC,                       
                Shared.TRG_PRIVATE_sUSDCG,                   
                quoteRequest
            );

            // Assert
            TestContext.WriteLine($"Testing against: {useEnvironment}");
            TestContext.WriteLine(JsonConvert.SerializeObject(result));
            Assert.IsTrue(result.IsFailure);
            Assert.AreEqual("ValidationError", result.Error.Code);
            Assert.AreEqual("There are one or more validation errors. See InnerErrors for more details", result.Error.Message);
            Assert.AreEqual("AddressSignatureInvalid", result.Error.InnerErrors[0].Code);
            Assert.AreEqual("ReceivingAddressSignature is not valid for the provided ReceivingAddress.", result.Error.InnerErrors[0].Message);
        }


        [Test]
        [Description("TestCaseId: C1682")]
        public async Task GetPendingQuote_sUsdcg_AmountTooSmall_Neg()
        {
            // Arrange
            ExchangeClient exchangeClient = new ExchangeClient(true);

            GetPendingQuoteRequest quoteRequest = new GetPendingQuoteRequest()
            {
                Amount = "0.1",
                Conversion = EConversion.sUsdcgBtc,
                SendingAddress = Shared.SRC_ADR_sUSDCG,    // requied sender address
                ReceivingAddress = Shared.TRG_ADR_BTC      // requied receiver address
            };

            // Call
            var result = await exchangeClient.GetPendingQuoteAsync(
                Shared.SRC_PRIVATE_sUSDCG,                 // requied sender private key
                Shared.TRG_PRIVATE_BTC,                    // requied receiver private key
                quoteRequest
            );

            // Assert
            TestContext.WriteLine($"Testing against: {useEnvironment}");
            TestContext.WriteLine(JsonConvert.SerializeObject(result));
            Assert.IsTrue(result.IsFailure);
            Assert.AreEqual("ValidationError", result.Error.Code);
            Assert.AreEqual("There are one or more validation errors. See InnerErrors for more details", result.Error.Message);
            Assert.AreEqual("AmountTooSmall", result.Error.InnerErrors[0].Code);
            Assert.AreEqual("Amount must be greater than or equal to 1 sUSDCG.", result.Error.InnerErrors[0].Message);
        }


        [Test]
        [Description("TestCaseId: C1682")]
        public async Task GetPendingQuote_Btc_AmountTooSmall_Neg()
        {
            // Arrange
            ExchangeClient exchangeClient = new ExchangeClient(true);

            GetPendingQuoteRequest quoteRequest = new GetPendingQuoteRequest()
            {
                Amount = "0.00001",
                Conversion = EConversion.BtcsUsdcg,
                SendingAddress = Shared.SRC_ADR_BTC,          // requied sender address
                ReceivingAddress = Shared.TRG_ADR_sUSDCG      // requied receiver address
            };

            // Call
            var result = await exchangeClient.GetPendingQuoteAsync(
                Shared.SRC_PRIVATE_BTC,                       // requied sender private key
                Shared.TRG_PRIVATE_sUSDCG,                    // requied receiver private key
                quoteRequest
            );

            // Assert
            TestContext.WriteLine($"Testing against: {useEnvironment}");
            TestContext.WriteLine(JsonConvert.SerializeObject(result));
            Assert.IsTrue(result.IsFailure);
            Assert.AreEqual("ValidationError", result.Error.Code);
            Assert.AreEqual("There are one or more validation errors. See InnerErrors for more details", result.Error.Message);
            Assert.AreEqual("AmountTooSmall", result.Error.InnerErrors[0].Code);
            Assert.AreEqual("Amount must be greater than or equal to 0.0001 BTC.", result.Error.InnerErrors[0].Message);
        }
    }
}
