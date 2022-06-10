using Gluwa.SDK_dotnet.Clients;
using Gluwa.SDK_dotnet.Models.Exchange;
using Gluwa.SDK_dotnet.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Threading.Tasks;
using Assert = NUnit.Framework.Assert;
using DescriptionAttribute = NUnit.Framework.DescriptionAttribute;
using TestContext = NUnit.Framework.TestContext;

namespace ExchangeClientTests
{
    [TestFixture("Sandbox")]
    [TestFixture("Production")]
    class GetOrderBookTests
    {
        private readonly string useEnvironment;

        /// <summary>
        /// From TestFixture
        /// </summary>
        /// <param name="platform"></param>
        public GetOrderBookTests(string useEnvironment)
        {
            this.useEnvironment = useEnvironment;
        }

        [SetUp]
        public void Setup()
        {
            Shared.SetEnvironmentVariables(useEnvironment);
        }

        [TestMethod]
        [Description("TestCaseId: C1674")]
        public async Task GetOrderBook_Pos(EConversion conversion)
        {
            // Arrange
            ExchangeClient exchangeClient = new ExchangeClient(true);

            // Call
            var result = await exchangeClient.GetOrderBook(conversion, 100);

            // Assert
            TestContext.WriteLine($"Testing against: {useEnvironment}");
            TestContext.WriteLine(JsonConvert.SerializeObject(result));
            Assert.IsTrue(result.IsSuccess);
        }


        [Test]
        [Category("BTC/sUSDCG")]
        public async Task GetOrderBook_BtcsUsdcg_Pos()
        {
            await GetOrderBook_Pos(EConversion.BtcsUsdcg);
        }


        [Test]
        [Category("sUSDCG/BTC")]
        public async Task GetOrderBook_sUsdcgBtc_Pos()
        {
            await GetOrderBook_Pos(EConversion.sUsdcgBtc);
        }
    }
}
