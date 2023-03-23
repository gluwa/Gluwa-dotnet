using Gluwa.SDK_dotnet.Utils;
using NBitcoin;

namespace Gluwa.SDK_dotnet
{
    public class Environment
    {
        public static Environment Production
        {
            get
            {
                Environment env = new Environment(
                    baseUrl: Constants.GLUWA_API_BASE_URL,
                    ngngContractAddress: Constants.GLUWACOIN_NGNG_CONTRACT_ADDRESS,
                    sUsdcgContractAddress: Constants.GLUWACOIN_SUSDCG_CONTRACT_ADDRESS,
                    usdcgContractAddress: Constants.GLUWACOIN_USDCG_CONTRACT_ADDRESS,
                    sNgngContractAddress: Constants.GLUWACOIN_SNGNG_CONTRACT_ADDRESS,
                    network: Network.Main);

                return env;
            }
        }

        public static Environment Test
        {
            get
            {
                Environment env = new Environment(
                    baseUrl: Constants.GLUWA_TEST_API_BASE_URL,
                    ngngContractAddress: Constants.GLUWACOIN_TEST_NGNG_CONTRACT_ADDRESS,
                    sUsdcgContractAddress: Constants.GLUWACOIN_TEST_SUSDCG_CONTRACT_ADDRESS,
                    usdcgContractAddress: Constants.GLUWACOIN_TEST_USDCG_CONTRACT_ADDRESS,
                    sNgngContractAddress: Constants.GLUWACOIN_TEST_SNGNG_CONTRACT_ADDRESS,
                    network: Network.TestNet);

                return env;
            }
        }

        public string BaseUrl { get; }

        public string NgngContractAddress { get; }

        public string SUsdcgContractAddress { get; }

        public string UsdcgContractAddress { get; }

        public string SNgngContractAddress { get; }

        public Network Network { get; }

        protected Environment(
            string baseUrl,
            string ngngContractAddress,
            string sUsdcgContractAddress,
            string usdcgContractAddress,
            string sNgngContractAddress,
            Network network)
        {
            BaseUrl = baseUrl;
            NgngContractAddress = ngngContractAddress;
            SUsdcgContractAddress = sUsdcgContractAddress;
            UsdcgContractAddress = usdcgContractAddress;
            SNgngContractAddress = sNgngContractAddress;
            Network = network;
        }
    }
}