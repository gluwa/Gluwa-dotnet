using Gluwa.SDK_dotnet.Clients;
using Gluwa.SDK_dotnet.Models.Exchange;
using Gluwa.SDK_dotnet.Tests;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Assert = NUnit.Framework.Assert;
using DescriptionAttribute = NUnit.Framework.DescriptionAttribute;
using TestContext = NUnit.Framework.TestContext;

namespace ExchangeClientTests
{
    [TestFixture("Test")]
    //[TestFixture("Production")]
    class PostOrderTests
    {
        private readonly string useEnvironment;
        private IConfiguration config;

        /// <summary>
        /// From TestFixture
        /// </summary>
        /// <param name="platform"></param>
        public PostOrderTests(string useEnvironment)
        {
            this.useEnvironment = useEnvironment;
        }

        [SetUp]
        public void Setup()
        {
            if (useEnvironment == "Production" && TestContext.CurrentContext.Test.Properties["Category"].Contains("SandboxOnly"))
            {
                Assert.Inconclusive($"Cannot run this test on environment: {useEnvironment}");
            }
            Shared.SetEnvironmentVariables(useEnvironment);
            config = new ConfigurationBuilder().AddJsonFile("appsettings.json").AddJsonFile("appsettings.dev.json", true).Build();
        }

        [TestMethod]
        [Description("TestCaseId: C1846")]
        public async Task PostOrder_Pos(EConversion conversion, 
                                        string sourceAddress, 
                                        string targetAddress,
                                        string sourcePrivate, 
                                        string targetPrivate,
                                        string amount)
        {
            // Arrange
            ExchangeClient exchangeClient = new ExchangeClient(true);

            // Add options
            CreateOrderRequest orderRequest = new CreateOrderRequest()
            {
                Conversion = conversion,
                SendingAddress = sourceAddress,
                ReceivingAddress = targetAddress,
                SourceAmount = amount,
                Price = "1"
            };

            // Call
            var result = await exchangeClient.CreateOrderAsync(
                config["API_KEY_SANDBOX"],     // required user api key
                config["API_SECRET_SANDBOX"],  // required user api secret
                sourcePrivate,                 // required sourece address private key
                targetPrivate,                 // required target address private key
                orderRequest                   // order request      
               );

            // Assert
            TestContext.WriteLine($"Testing against: {useEnvironment}");
            TestContext.WriteLine(JsonConvert.SerializeObject(result));

            if (JsonConvert.SerializeObject(result).Contains("NotEnoughBalance"))
            {
                Assert.Warn("Low account balance prevented this test from completing.");
            }
            else
            {
                Assert.IsTrue(result.IsSuccess);
            }
        }


        [Test]
        [Category("sUSDCG/BTC")]
        [Category("SandboxOnly")]
        public async Task PostOrder_sUsdcgBtc_Pos()
        {
            // Call
            await PostOrder_Pos(EConversion.sUsdcgBtc, 
                                Shared.SRC_ADR_sUSDCG_SANDBOX, 
                                Shared.TRG_ADR_BTC_SANDBOX,
                                config["SRC_PRIVATE_sUSDCG_SANDBOX"],
                                config["TRG_PRIVATE_BTC_SANDBOX"],
                                Shared.DEFAULT_sUSDCG_AMOUNT);
        }


        [Test]
        [Category("BTC/sUSDCG")]
        [Category("SandboxOnly")]
        public async Task PostOrder_BtcsUsdcg_Pos()
        {
            // Call
            await PostOrder_Pos(EConversion.BtcsUsdcg, 
                                Shared.SRC_ADR_BTC_SANDBOX, 
                                Shared.TRG_ADR_sUSDCG_SANDBOX,
                                config["SRC_PRIVATE_BTC_SANDBOX"],
                                config["TRG_PRIVATE_sUSDCG_SANDBOX"],
                                Shared.DEFAULT_BTC_AMOUNT);
        }


       [Test]
       [Description("TestCaseId: C1847")]
        public async Task PostOrder_InvalidAddress_Neg()
        {
            // Arrange
            ExchangeClient exchangeClient = new ExchangeClient(Shared.BASE_ENV);

            // Add options
            CreateOrderRequest orderRequest = new CreateOrderRequest()
            {
                Conversion = EConversion.sUsdcgBtc,
                SendingAddress = Shared.INVALID_ETH_ADDRESS,     // Invalid address
                ReceivingAddress = Shared.INVALID_ETH_ADDRESS,
                SourceAmount = Shared.DEFAULT_sUSDCG_AMOUNT,
                Price = "1"
            };

            // Call
            var result = await exchangeClient.CreateOrderAsync(
                Shared.API_KEY,
                Shared.API_SECRET,
                Shared.SRC_PRIVATE_sUSDCG,
                Shared.SRC_PRIVATE_BTC,
                orderRequest
               );

            // Assert
            TestContext.WriteLine($"Testing against: {useEnvironment}");
            TestContext.WriteLine(JsonConvert.SerializeObject(result));
            Assert.IsTrue(result.IsFailure);
            Assert.AreEqual("ValidationError", result.Error.Code);
            Assert.AreEqual("Invalid SourceSignature.", result.Error.InnerErrors[0].Message);
        }


        [Test]
        [Description("TestCaseId: C1848")]
        public async Task PostOrder_InvalidKey_Neg()
        {
            // Arrange
            ExchangeClient exchangeClient = new ExchangeClient(true);

            // Add options
            CreateOrderRequest orderRequest = new CreateOrderRequest()
            {
                Conversion = EConversion.sUsdcgBtc,
                SendingAddress = Shared.SRC_ADR_sUSDCG_SANDBOX,
                ReceivingAddress = Shared.SRC_ADR_BTC_SANDBOX,
                SourceAmount = Shared.DEFAULT_sUSDCG_AMOUNT,
                Price = "1"
            };

            // Call
            var result = await exchangeClient.CreateOrderAsync(
                config["API_KEY_SANDBOX"],
                config["API_SECRET_SANDBOX"],
                Shared.DEFAULT_PRIVATE,
                Shared.SRC_PRIVATE_BTC,
                orderRequest
               );

            // Assert
            TestContext.WriteLine($"Testing against: {useEnvironment}");
            TestContext.WriteLine(JsonConvert.SerializeObject(result));
            Assert.IsTrue(result.IsFailure);
            Assert.AreEqual("ValidationError", result.Error.Code);
            Assert.AreEqual("Invalid SourceSignature.", result.Error.InnerErrors[0].Message);
        }


        [Test]
        public async Task PostOrder_TooSmallAmount_sUsdcg_Neg()
        {
            // Arrange
            ExchangeClient exchangeClient = new ExchangeClient(true);

            // Add options
            CreateOrderRequest orderRequest = new CreateOrderRequest()
            {
                Conversion = EConversion.sUsdcgBtc,
                SendingAddress = Shared.SRC_ADR_sUSDCG_SANDBOX,
                ReceivingAddress = Shared.SRC_ADR_BTC_SANDBOX,
                SourceAmount = "0.99",                           // Too small amount
                Price = "1"
            };

            // Call
            var result = await exchangeClient.CreateOrderAsync(
                config["API_KEY_SANDBOX"],
                config["API_SECRET_SANDBOX"],
                Shared.SRC_PRIVATE_sUSDCG,
                Shared.SRC_PRIVATE_BTC,       
                orderRequest
               );

            // Assert
            TestContext.WriteLine($"Testing against: {useEnvironment}");
            TestContext.WriteLine(JsonConvert.SerializeObject(result));
            Assert.IsTrue(result.IsFailure);
            Assert.AreEqual("ValidationError", result.Error.Code);
            Assert.AreEqual("SourceAmount must be greater than or equal to 1 sUSDCG.", result.Error.InnerErrors[0].Message);
        }


        [Test]
        public async Task PostOrder_TooSmallAmount_Btc_Neg()
        {
            // Arrange
            ExchangeClient exchangeClient = new ExchangeClient(true);

            // Add options
            CreateOrderRequest orderRequest = new CreateOrderRequest()
            {
                Conversion = EConversion.BtcsUsdcg,
                SendingAddress = Shared.SRC_ADR_BTC_SANDBOX,
                ReceivingAddress = Shared.DEFAULT_ADDRESS,
                SourceAmount = "0.000001",                         // Too small amount
                Price = "1"
            };

            // Call
            var result = await exchangeClient.CreateOrderAsync(
                config["API_KEY_SANDBOX"],
                config["API_SECRET_SANDBOX"],
                Shared.SRC_PRIVATE_BTC,
                Shared.DEFAULT_PRIVATE,
                orderRequest
               );

            // Assert
            TestContext.WriteLine($"Testing against: {useEnvironment}");
            TestContext.WriteLine(JsonConvert.SerializeObject(result));
            Assert.IsTrue(result.IsFailure);
            Assert.AreEqual("ValidationError", result.Error.Code);
            Assert.AreEqual("SourceAmount must be greater than or equal to 0.0001 BTC.", result.Error.InnerErrors[0].Message);
        }


        ////TODO: Revise after fix. Expected - Code":"ValidationError" with error Msg.; Actual - Error:System.FormatException
        //[Test]
        //[Description("TestCaseId: C1850")]
        //public async Task PostOrder_EmptyAddress_Neg()
        //{
        //    // Arrange
        //    ExchangeClient exchangeClient = new ExchangeClient(true);

        //    // Add options
        //    CreateOrderRequest orderRequest = new CreateOrderRequest()
        //    {
        //        Conversion = EConversion.sUsdcgBtc,
        //        SendingAddress = null,
        //        ReceivingAddress = Shared.SRC_ADR_BTC_SANDBOX,
        //        SourceAmount = Shared.DEFAULT_sUSDCG_AMOUNT,
        //        Price = "1"
        //    };

        //    // Call
        //    var result = await exchangeClient.CreateOrderAsync(
        //        Shared.API_KEY_SANDBOX,
        //        Shared.API_SECRET_SANDBOX,
        //        Shared.SRC_PRIVATE_sUSDCG,
        //        Shared.SRC_PRIVATE_BTC,
        //        orderRequest
        //       );

        //    // Assert
        //    TestContext.WriteLine($"Testing against: {useEnvironment}");
        //    TestContext.WriteLine(JsonConvert.SerializeObject(result));
        //    Assert.IsTrue(result.IsFailure);
        //    Assert.AreEqual("ValidationError", result.Error.Code);
        //}


        //TODO: Revise after fix. Expected - Code":"ValidationError" with error Msg.; Actual - Error:System.FormatException
        //[Test]
        //[Description("TestCaseId: C1851")]
        //public async Task PostOrder_EmptyPrivateKey_Neg()
        //{
        //    // Arrange
        //    ExchangeClient exchangeClient = new ExchangeClient(true);

        //    // Add options
        //    CreateOrderRequest orderRequest = new CreateOrderRequest()
        //    {
        //        Conversion = EConversion.sUsdcgBtc,
        //        SendingAddress = Shared.SRC_ADR_sUSDCG_SANDBOX,
        //        ReceivingAddress = Shared.SRC_ADR_BTC_SANDBOX,
        //        SourceAmount = Shared.DEFAULT_sUSDCG_AMOUNT,
        //        Price = "1"
        //    };

        //    // Call
        //    var result = await exchangeClient.CreateOrderAsync(
        //        Shared.API_KEY_SANDBOX,
        //        Shared.API_SECRET_SANDBOX,
        //        null,
        //        Shared.SRC_PRIVATE_BTC,
        //        orderRequest
        //       );

        //    // Assert
        //    TestContext.WriteLine($"Testing against: {useEnvironment}");
        //    TestContext.WriteLine(JsonConvert.SerializeObject(result));
        //    Assert.IsTrue(result.IsFailure);
        //    Assert.AreEqual("ValidationError", result.Error.Code);
        //}
    }
}
