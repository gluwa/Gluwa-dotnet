using Gluwa.SDK_dotnet.Clients;
using Gluwa.SDK_dotnet.Models.Exchange;
using Gluwa.SDK_dotnet.Tests;
using Microsoft.Extensions.Configuration;
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
    [TestFixture("Sandbox")]
    //[TestFixture("Production")]
    class PatchOrderTests
    {
        private readonly string useEnvironment;
        private IConfiguration config;

        /// <summary>
        /// From TestFixture
        /// </summary>
        /// <param name="platform"></param>
        public PatchOrderTests(string useEnvironment)
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

        [Test]
        [Category("SandboxOnly")]
        [Description("TestCaseId: C1844")]
        public async Task PatchOrder_ByOrderId_Pos()
        {
            Guid orderIdGuid;

            // Arrange
            ExchangeClient exchangeClient = new ExchangeClient(Shared.BASE_ENV);

            // Post Orderid for conversion
            string orderId = Shared.PostOrder(EConversion.sUsdcgBtc, 
                                              Shared.SRC_ADR_sUSDCG, 
                                              Shared.SRC_PRIVATE_sUSDCG,
                                              Shared.TRG_ADR_BTC, 
                                              Shared.TRG_PRIVATE_BTC,
                                              Shared.BASE_URL_TEST,
                                              Shared.DEFAULT_sUSDCG_AMOUNT);
            // String to Guid 
            orderIdGuid = Guid.Parse(orderId);

            // Call
            var result = await exchangeClient.CancelOrderAsync(
                config["API_KEY_SANDBOX"],
                config["API_SECRET_SANDBOX"],
                orderIdGuid
            );

            // Assert
            TestContext.WriteLine($"Testing against: {useEnvironment}");
            TestContext.WriteLine(JsonConvert.SerializeObject(result));
            Assert.AreEqual(null, result.Error.InnerErrors);
        }


        [Test]
        [Description("TestCaseId: C1845")]
        public async Task PatchOrder_InvalidOrderId_Pos()
        {
            // Arrange
            ExchangeClient exchangeClient = new ExchangeClient(Shared.BASE_ENV);

            // String to Guid 
            Guid orderIdGuid = Guid.Parse(Shared.INVALID_ORDERID);

            // Call
            var result = await exchangeClient.CancelOrderAsync(
                Shared.API_KEY,
                Shared.API_SECRET,
                orderIdGuid                    // Invalid order id
            );

            // Assert
            TestContext.WriteLine($"Testing against: {useEnvironment}");
            TestContext.WriteLine(JsonConvert.SerializeObject(result));
            Assert.AreEqual(null, result.Error.InnerErrors);
        }
    }
}
