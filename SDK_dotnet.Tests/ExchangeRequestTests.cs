using Gluwa.SDK_dotnet.Clients;
using Gluwa.SDK_dotnet.Models.Exchange;
using Gluwa.SDK_dotnet.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Numerics;
using System.Threading.Tasks;
using Assert = NUnit.Framework.Assert;
using DescriptionAttribute = NUnit.Framework.DescriptionAttribute;
using TestContext = NUnit.Framework.TestContext;

namespace ExchangeClientTests
{
    [TestFixture("Sandbox")]
    //[TestFixture("Production")]
    class ExchangeRequestsTests
    {
        private readonly string useEnvironment;

        /// <summary>
        /// Parsed exchange ID 
        /// </summary>
        private Guid GuidID { get; set; }

        /// <summary>
        /// From TestFixture
        /// </summary>
        /// <param name="platform"></param>
        public ExchangeRequestsTests(string useEnvironment)
        {
            this.useEnvironment = useEnvironment;
        }

        [SetUp]
        public void Setup()
        {
            // Set environment
            Shared.SetEnvironmentVariables(useEnvironment);
        }

        // [TestMethod]
        // [Category("Gluwacoin")]
        // [Description("TestCaseId: C1831")]
        // public async Task ExchangeRequests_Gluwacoin_Pos(EConversion conversion, 
        //                                                  string sourceAddress,
        //                                                  string sourcePrivate)
        // {
        //     // Arrange
        //     ExchangeClient exchangeClient = new ExchangeClient(Shared.BASE_ENV);
        //     string converse = conversion.ToString();

        //     // Get exchange request
        //     var exchangeRequest = Shared.GetExchangeRequest(converse);
        //     var exchangeID = exchangeRequest.ID;
        //     var destAddress = exchangeRequest.DestinationAddress;
        //     var executor = exchangeRequest.Executor;
        //     var exblockNo = exchangeRequest.ExpiryBlockNumber;

        //     // String to Guid 
        //     GuidID = Guid.Parse(exchangeID);

        //     // Add options
        //     AcceptExchangeRequest quoteRequest = new AcceptExchangeRequest()
        //     {
        //         ID = GuidID,                      // required ID
        //         Conversion = conversion,          // required conversion
        //         DestinationAddress = destAddress, // required destination address
        //         Executor = executor,              // optional, included only when the source currency is a Gluwacoin currency.
        //         ExpiryBlockNumber = BigInteger.Parse(exblockNo), // optional, included only when the source currency is a Gluwacoin currency.
        //         SourceAmount = "1",               // the amount in source currency
        //         Fee = "0.061906"                  // the fee amount
        //     };

        //     // Call
        //     var result = await exchangeClient.AcceptExchangeRequestAsync(
        //         Shared.API_KEY,                // required user api key
        //         Shared.API_SECRET,             // required user api secret
        //         sourceAddress,                 // required source address 
        //         sourcePrivate,                 // required source address private key
        //         quoteRequest                   // order request      
        //        );

        //     // Assert
        //     TestContext.WriteLine($"Testing against: {useEnvironment}");
        //     TestContext.WriteLine(JsonConvert.SerializeObject(result));
        //     Assert.AreEqual(null, result.Error.InnerErrors);
        // }


        // [TestMethod]
        // [Category("Btc")]
        // [Description("TestCaseId: C1832")]
        // public async Task ExchangeRequests_Btc_Pos(EConversion conversion, 
        //                                            string sourceAddress,
        //                                            string sourcePrivate)
        // {
        //     // Arrange
        //     ExchangeClient exchangeClient = new ExchangeClient(Shared.BASE_ENV);
        //     string converse = conversion.ToString();

        //     // Get exchange request
        //     var exchangeRequest = Shared.GetExchangeRequest(converse);
        //     var exchangeID = exchangeRequest.ID;
        //     var destAddress = exchangeRequest.DestinationAddress;
        //     var fundsAddress = exchangeRequest.ReservedFundsAddress;
        //     var script = exchangeRequest.ReservedFundsRedeemScript;

        //     // String to Guid 
        //     GuidID = Guid.Parse(exchangeID);

        //     // Add options
        //     AcceptExchangeRequest quoteRequest = new AcceptExchangeRequest()
        //     {
        //         ID = GuidID,                         // required ID
        //         Conversion = conversion,             // required conversion
        //         DestinationAddress = destAddress,    // required destination address
        //         ReservedFundsAddress = fundsAddress, // optional, included only when use Btc currency.
        //         ReservedFundsRedeemScript = script,  // optional, included only when use Btc currency.
        //         SourceAmount = "0.0001",             // the amount in source currency
        //         Fee = "0"                            // the fee amount
        //     };

        //     // Call
        //     var result = await exchangeClient.AcceptExchangeRequestAsync(
        //         Shared.API_KEY,                // required user api key
        //         Shared.API_SECRET,             // required user api secret
        //         sourceAddress,                 // required source address 
        //         sourcePrivate,                 // required source address private key
        //         quoteRequest                   // order request      
        //        );

        //     // Assert
        //     TestContext.WriteLine($"Testing against: {useEnvironment}");
        //     TestContext.WriteLine(JsonConvert.SerializeObject(result));
        //     Assert.AreEqual(null, result.Error.InnerErrors);
        // }


        //[Test]
        //[Category("BTC/sUSDCG")]
        //public async Task ExchangeRequests_BtcsUsdcg_Pos()
        //{
        //    // Call
        //    await ExchangeRequests_Btc_Pos(EConversion.BtcsUsdcg, 
        //                                   Shared.SRC_ADR_BTC,
        //                                   Shared.SRC_PRIVATE_BTC);
        //}


        //[Test]
        //[Category("sUSDCG/BTC")]
        //public async Task ExchangeRequests_sUsdcgBtc_Pos()
        //{
        //    // Call
        //    await ExchangeRequests_Gluwacoin_Pos(EConversion.sUsdcgBtc, 
        //                                         Shared.SRC_ADR_sUSDCG,
        //                                         Shared.SRC_PRIVATE_sUSDCG);
        //}


        //// TODO: Revise after fix. Expected - Code":"ValidationError" with error Msg.; Actual Error:System.FormatException
        //[Test]
        //[Description("TestCaseId: C1833")]
        //public async Task ExchangeRequests_InvalidSourceAddress_Neg()
        //{
        //    // Arrange
        //    ExchangeClient exchangeClient = new ExchangeClient(Shared.BASE_ENV);
        //    EConversion conversion = EConversion.sUsdcgBtc;
        //    string amount = "1";
        //    string fee = "0.1";

        //    // Prepare quote request
        //    var quoteRequest = Shared.PrepareQuoteRequest(conversion, amount, fee);

        //    // Call
        //    var result = await exchangeClient.AcceptExchangeRequestAsync(
        //        Shared.API_KEY,
        //        Shared.API_SECRET,
        //        Shared.INVALID_ETH_ADDRESS,                 // invalid source address
        //        Shared.SRC_PRIVATE_sUSDCG,
        //        quoteRequest
        //        );

        //    // Assert
        //    TestContext.WriteLine($"Testing against: {useEnvironment}");
        //    TestContext.WriteLine(JsonConvert.SerializeObject(result));
        //    Assert.IsTrue(result.IsFailure);
        //    Assert.AreEqual("ValidationError", result.Error.Code);
        //}


        //// TODO: Revise after fix. Expected - Code":"Forbidden" with error Msg.; Actual Success:200
        //[Test]
        //[Description("TestCaseId: C1834")]
        //public async Task ExchangeRequests_InvalidPrivateKey_Neg()
        //{
        //    // Arrange
        //    ExchangeClient exchangeClient = new ExchangeClient(Shared.BASE_ENV);
        //    EConversion conversion = EConversion.sUsdcgBtc;
        //    string amount = "1";
        //    string fee = "0.1";

        //    // Prepare quote request
        //    var quoteRequest = Shared.PrepareQuoteRequest(conversion, amount, fee);

        //    // Call
        //    var result = await exchangeClient.AcceptExchangeRequestAsync(
        //        Shared.API_KEY,
        //        Shared.API_SECRET,
        //        Shared.SRC_ADR_sUSDCG,
        //        Shared.INVALID_PRIVATE_KEY,        // invalid private key
        //        quoteRequest
        //       );

        //    // Assert
        //    TestContext.WriteLine($"Testing against: {useEnvironment}");
        //    TestContext.WriteLine(JsonConvert.SerializeObject(result));
        //    Assert.IsTrue(result.IsFailure);
        //    Assert.AreEqual("Forbidden", result.Error.Code);
        //}


        ////TODO: Revise after fix. Expected - Code":"ValidationError" with error Msg.; Actual Error:System.FormatException
        //[Test]
        //[Description("TestCaseId: C1835")]
        //public async Task ExchangeRequests_InvalidDestAddress_Neg()
        //{
        //    // Arrange
        //    ExchangeClient exchangeClient = new ExchangeClient(Shared.BASE_ENV);
        //    EConversion conversion = EConversion.sUsdcgBtc;
        //    string amount = "1";
        //    string fee = "0.1";

        //    // Prepare quote request
        //    var quoteRequest = Shared.PrepareQuoteRequest(conversion, amount, fee);

        //    // Invalid destination address
        //    quoteRequest.DestinationAddress = Shared.INVALID_ETH_ADDRESS;

        //    // Call
        //    var result = await exchangeClient.AcceptExchangeRequestAsync(
        //        Shared.API_KEY,
        //        Shared.API_SECRET,
        //        Shared.SRC_ADR_sUSDCG,
        //        Shared.SRC_PRIVATE_sUSDCG,
        //        quoteRequest
        //       );

        //    // Assert
        //    TestContext.WriteLine($"Testing against: {useEnvironment}");
        //    TestContext.WriteLine(JsonConvert.SerializeObject(result));
        //    Assert.IsTrue(result.IsFailure);
        //    Assert.AreEqual("ValidationError", result.Error.Code);
        //}


        //// TODO: Revise after fix. Expected - Code":"ValidationError" with error Msg.; Actual Error: System.FormatException
        //[Test]
        //[Description("TestCaseId: C1836")]
        //public async Task ExchangeRequests_InvalidExecutor_Neg()
        //{
        //    // Arrange
        //    ExchangeClient exchangeClient = new ExchangeClient(Shared.BASE_ENV);
        //    EConversion conversion = EConversion.sUsdcgBtc;
        //    string amount = "1";
        //    string fee = "0.1";

        //    // Prepare quote request
        //    var quoteRequest = Shared.PrepareQuoteRequest(conversion, amount, fee);

        //    // Invalid Executor
        //    quoteRequest.Executor = Shared.INVALID_BTC_ADDRESS;            

        //    // Call
        //    var result = await exchangeClient.AcceptExchangeRequestAsync(
        //        Shared.API_KEY,
        //        Shared.API_SECRET,
        //        Shared.SRC_ADR_sUSDCG,
        //        Shared.SRC_PRIVATE_sUSDCG,
        //        quoteRequest
        //       );

        //    // Assert
        //    TestContext.WriteLine($"Testing against: {useEnvironment}");
        //    TestContext.WriteLine(JsonConvert.SerializeObject(result));
        //    Assert.IsTrue(result.IsFailure);
        //    Assert.AreEqual("ValidationError", result.Error.Code);
        //}


        //// TODO: Revise after fix. Expected - Code":"ValidationError" with error Msg.; Actual Error: System.FormatException
        //[Test]
        //[Description("TestCaseId: C1837")]
        //public async Task ExchangeRequests_InvalidBlockNo_Neg()
        //{
        //    // Arrange
        //    ExchangeClient exchangeClient = new ExchangeClient(Shared.BASE_ENV);
        //    EConversion conversion = EConversion.sUsdcgBtc;
        //    string amount = "1";
        //    string fee = "0.1";

        //    // Prepare quote request
        //    var quoteRequest = Shared.PrepareQuoteRequest(conversion, amount, fee);

        //    // Invalid block No
        //    quoteRequest.ExpiryBlockNumber = BigInteger.Parse("0");

        //    // Call
        //    var result = await exchangeClient.AcceptExchangeRequestAsync(
        //        Shared.API_KEY,
        //        Shared.API_SECRET,
        //        Shared.SRC_ADR_sUSDCG,
        //        Shared.SRC_PRIVATE_sUSDCG,
        //        quoteRequest
        //       );

        //    // Assert
        //    TestContext.WriteLine($"Testing against: {useEnvironment}");
        //    TestContext.WriteLine(JsonConvert.SerializeObject(result));
        //    Assert.IsTrue(result.IsFailure);
        //    Assert.AreEqual("ValidationError", result.Error.Code);
        //}


        ////TODO: Revise after fix. Expected - Code":"ValidationError" with error Msg.; Actual Error: System.ArgumentOutOfRangeException
        //[Test]
        //[Description("TestCaseId: C1838")]
        //public async Task ExchangeRequests_InvalidAmount_Neg()
        //{
        //    // Arrange
        //    ExchangeClient exchangeClient = new ExchangeClient(Shared.BASE_ENV);
        //    EConversion conversion = EConversion.sUsdcgBtc;
        //    string amount = "1";
        //    string fee = "0.1";

        //    // Prepare quote request
        //    var quoteRequest = Shared.PrepareQuoteRequest(conversion, amount, fee);

        //    // Invalid amount
        //    quoteRequest.SourceAmount = "-1";

        //    // Call
        //    var result = await exchangeClient.AcceptExchangeRequestAsync(
        //        Shared.API_KEY,
        //        Shared.API_SECRET,
        //        Shared.SRC_ADR_sUSDCG,
        //        Shared.SRC_PRIVATE_sUSDCG,
        //        quoteRequest
        //       );

        //    // Assert
        //    TestContext.WriteLine($"Testing against: {useEnvironment}");
        //    TestContext.WriteLine(JsonConvert.SerializeObject(result));
        //    Assert.IsTrue(result.IsFailure);
        //    Assert.AreEqual("ValidationError", result.Error.Code);
        //}


        ////TODO: Revise after fix. Expected - Code":"ValidationError" with error Msg.; Actual Error: System.ArgumentOutOfRangeException
        //[Test]
        //[Description("TestCaseId: C1839")]
        //public async Task ExchangeRequests_InvalidFee_Neg()
        //{
        //    // Arrange
        //    ExchangeClient exchangeClient = new ExchangeClient(Shared.BASE_ENV);
        //    EConversion conversion = EConversion.sUsdcgBtc;
        //    string amount = "1";
        //    string fee = "0.1";

        //    // Prepare quote request
        //    var quoteRequest = Shared.PrepareQuoteRequest(conversion, amount, fee);

        //    // Invalid fee
        //    quoteRequest.Fee = "-0.1";

        //    // Call
        //    var result = await exchangeClient.AcceptExchangeRequestAsync(
        //        Shared.API_KEY,
        //        Shared.API_SECRET,
        //        Shared.SRC_ADR_sUSDCG,
        //        Shared.SRC_PRIVATE_sUSDCG,
        //        quoteRequest
        //       );

        //    // Assert
        //    TestContext.WriteLine($"Testing against: {useEnvironment}");
        //    TestContext.WriteLine(JsonConvert.SerializeObject(result));
        //    Assert.IsTrue(result.IsFailure);
        //    Assert.AreEqual("ValidationError", result.Error.Code);
        //}
    }
}
