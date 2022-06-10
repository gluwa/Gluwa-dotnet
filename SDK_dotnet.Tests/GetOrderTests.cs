using Gluwa.SDK_dotnet.Clients;
using Gluwa.SDK_dotnet.Models.Exchange;
using Gluwa.SDK_dotnet.Tests;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Assert = NUnit.Framework.Assert;
using DescriptionAttribute = NUnit.Framework.DescriptionAttribute;
using TestContext = NUnit.Framework.TestContext;

namespace ExchangeClientTests
{
    [TestFixture("Sandbox")]
    //[TestFixture("Production")]
    public class GetOrderTests
    {
        private readonly string useEnvironment;

        /// <summary>
        /// From TestFixture
        /// </summary>
        /// <param name="platform"></param>
        public GetOrderTests(string useEnvironment)
        {
            this.useEnvironment = useEnvironment;
        }

        [SetUp]
        public void Setup()
        {
            Shared.SetEnvironmentVariables(useEnvironment);
        }

        [Test]
        [Order(1)]
        [Description("TestCaseId: C1684")]
        public async Task GetOrders_Pos()
        {
            // Arrange
            ExchangeClient exchangeClient = new ExchangeClient(Shared.BASE_ENV);

            // Call
            var result = await exchangeClient.GetOrdersAsync(
                Shared.API_KEY,
                Shared.API_SECRET
            );

            // Assert
            TestContext.WriteLine($"Testing against: {useEnvironment}");
            TestContext.WriteLine(JsonConvert.SerializeObject(result));
            Assert.IsTrue(result.IsSuccess);
        }


        [Test]
        [Order(2)]
        [Description("TestCaseId: C1685")]
        public async Task GetOrders_WithOptions_Pos()
        {
            // Arrange
            ExchangeClient exchangeClient = new ExchangeClient(Shared.BASE_ENV);

            // Add options
            GetOrdersOptions options = new GetOrdersOptions()
            {
                StartDateTime = new DateTime(2015, 01, 01),
                EndDateTime = new DateTime(2021, 12, 01),
                Status = EOrderStatus.Canceled,
                Offset = 10,
                Limit = 100
            };

            // Call
            var result = await exchangeClient.GetOrdersAsync(
                Shared.API_KEY,
                Shared.API_SECRET,
                options
            );

            // Assert
            TestContext.WriteLine($"Testing against: {useEnvironment}");
            TestContext.WriteLine(JsonConvert.SerializeObject(result));
            Assert.IsTrue(result.IsSuccess);
        }


        [Test]
        [Order(3)]
        [Description("TestCaseId: C1842")]
        public async Task GetOrder_ByOrderId_Pos()
        {
            // Arrange
            Guid orderIdGuid;
            ExchangeClient exchangeClient = new ExchangeClient(Shared.BASE_ENV);

            // Post OrderId for conversion
            if (Shared.BASE_ENV)
            {
                string orderId = Shared.PostOrder(EConversion.sUsdcgBtc,
                                                  Shared.SRC_ADR_sUSDCG,
                                                  Shared.SRC_PRIVATE_sUSDCG,
                                                  Shared.TRG_ADR_BTC,
                                                  Shared.TRG_PRIVATE_BTC,
                                                  Shared.BASE_URL_TEST,
                                                  Shared.DEFAULT_sUSDCG_AMOUNT)
                    ;
                // String to Guid 
                orderIdGuid = Guid.Parse(orderId);
            }
            else
                // String to Guid  
                orderIdGuid = Guid.Parse(Shared.ORDER_ID);

            // Call
            var result = await exchangeClient.GetOrderAsync(
                Shared.API_KEY,
                Shared.API_SECRET,
                orderIdGuid
            );

            // Assert
            TestContext.WriteLine($"Testing against: {useEnvironment}");
            TestContext.WriteLine(JsonConvert.SerializeObject(result));
            Assert.AreEqual(null, result.Error.InnerErrors);
        }


        [Test]
        [Order(4)]
        [Description("TestCaseId: C1686")]
        public async Task GetOrders_InvalidKey_Neg()
        {
            // Arrange
            ExchangeClient exchangeClient = new ExchangeClient(Shared.BASE_ENV);

            // Add options
            GetOrdersOptions options = new GetOrdersOptions()
            {
                StartDateTime = new DateTime(2015, 01, 01),
                EndDateTime = new DateTime(2021, 12, 01),
                Status = EOrderStatus.Canceled,
                Offset = 10,
                Limit = 100
            };

            // Call
            var result = await exchangeClient.GetOrdersAsync(
                Shared.INVALID_API_KEY,          // invalid api key
                Shared.API_SECRET,
                options
            );

            // Assert
            TestContext.WriteLine($"Testing against: {useEnvironment}");
            TestContext.WriteLine(JsonConvert.SerializeObject(result));
            Assert.IsTrue(result.IsFailure);
            Assert.AreEqual("Forbidden", result.Error.Code);
            Assert.AreEqual("Not authorized to use this endpoint.", result.Error.Message);
        }


        [Test]
        [Order(5)]
        [Description("TestCaseId: C1687")]
        public async Task GetOrders_InvalidSecretKey_Neg()
        {
            // Arrange
            ExchangeClient exchangeClient = new ExchangeClient(Shared.BASE_ENV);

            // Add options
            GetOrdersOptions options = new GetOrdersOptions()
            {
                StartDateTime = new DateTime(2015, 01, 01),
                EndDateTime = new DateTime(2021, 12, 01),
                Status = EOrderStatus.Canceled,
                Offset = 10,
                Limit = 100
            };

            // Call
            var result = await exchangeClient.GetOrdersAsync(
                Shared.API_KEY,
                Shared.INVALID_API_SECRET,       // invalid api secret
                options
            );

            // Assert
            TestContext.WriteLine($"Testing against: {useEnvironment}");
            TestContext.WriteLine(JsonConvert.SerializeObject(result));
            Assert.IsTrue(result.IsFailure);
            Assert.AreEqual("Forbidden", result.Error.Code);
            Assert.AreEqual("Not authorized to use this endpoint.", result.Error.Message);
        }


        [Test]
        [Order(6)]
        [Description("TestCaseId: C1843")]
        public async Task GetOrder_InvalidOrderId_Neg()
        {
            // Arrange
            Guid orderIdGuid;
            ExchangeClient exchangeClient = new ExchangeClient(Shared.BASE_ENV);

            // String to Guid
            orderIdGuid = Guid.Parse(Shared.INVALID_ORDERID);    // invalid orderId

            // Call
            var result = await exchangeClient.GetOrderAsync(
                Shared.API_KEY,
                Shared.API_SECRET,
                orderIdGuid
            );

            // Assert
            TestContext.WriteLine($"Testing against: {useEnvironment}");
            TestContext.WriteLine(JsonConvert.SerializeObject(result));
            Assert.IsTrue(result.IsFailure);
        }
    }
}
