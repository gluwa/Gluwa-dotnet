using Gluwa.SDK_dotnet.Clients;
using Gluwa.SDK_dotnet.Models;
using Gluwa.SDK_dotnet.Tests;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using Assert = NUnit.Framework.Assert;
using DescriptionAttribute = NUnit.Framework.DescriptionAttribute;
using TestContext = NUnit.Framework.TestContext;

namespace GluwaClientTests
{
    [TestFixture("Test")]
    [TestFixture("Production")]
    class GetAddressBalanceTests
    {
        private readonly string useEnvironment;

        /// <summary>
        /// From TestFixture
        /// </summary>
        /// <param name="platform"></param>
        public GetAddressBalanceTests(string useEnvironment)
        {
            this.useEnvironment = useEnvironment;
        }

        [SetUp]
        public void Setup()
        {
            Shared.SetEnvironmentVariables(useEnvironment);
        }


        [Test]
        [Description("TestCaseId: C1107")]
        public async Task GetAddressBalance_sNgNg_Pos()
        {
            // Arrange
            GluwaClient gluwaClient = new GluwaClient(Shared.BASE_ENV);

            // Call
            var result = await gluwaClient.GetBalanceAsync(
                ECurrency.sNGNG,             // requied currency:sNGNG
                Shared.SRC_ADR_sNGNG,        // requied sNGNG address
                false                        // (For BTC only) if true
            );

            // Assert
            TestContext.WriteLine($"Testing against: {useEnvironment}");
            TestContext.WriteLine(JsonConvert.SerializeObject(result));
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(ECurrency.sNGNG, result.Data.Currency);
            Assert.That(result.Data.Balance, Is.Not.Null);
        }


        [Test]
        [Description("TestCaseId: C1107")]
        public async Task GetAddressBalance_sUsdcg_Pos()
        {
            // Arrange
            GluwaClient gluwaClient = new GluwaClient(Shared.BASE_ENV);

            // Call
            var result = await gluwaClient.GetBalanceAsync(
                ECurrency.sUSDCG,           // requied currency: sUSDCG
                Shared.SRC_ADR_sUSDCG,      // requied address
                false                       // (For BTC only) if true
            );

            // Assert
            TestContext.WriteLine($"Testing against: {useEnvironment}");
            TestContext.WriteLine(JsonConvert.SerializeObject(result));
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(ECurrency.sUSDCG, result.Data.Currency);
            Assert.That(result.Data.Balance, Is.Not.Null);
        }


        [Test]
        [Description("TestCaseId: C1662")]
        public async Task GetAddressBalance_InvalidAddress_Neg()
        {
            // Arrange
            GluwaClient gluwaClient = new GluwaClient(Shared.BASE_ENV);

            // Call
            var result = await gluwaClient.GetBalanceAsync(
                ECurrency.sUSDCG,
                Shared.INVALID_ETH_ADDRESS,      // invalid address       
                false
            );

            // Assert
            TestContext.WriteLine($"Testing against: {useEnvironment}");
            TestContext.WriteLine(JsonConvert.SerializeObject(result));
            Assert.IsTrue(result.IsFailure);
            Assert.AreEqual("BadRequest", result.Error.Code);
            Assert.AreEqual("Invalid address value.", result.Error.Message);
        }
    }
}
