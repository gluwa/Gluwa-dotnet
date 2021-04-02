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
                    usdgContractAddress: Constants.GLUWACOIN_USDG_CONTRACT_ADDRESS,
                    krwgContractAddress: Constants.GLUWACOIN_KRWG_CONTRACT_ADDRESS,
                    ngngContractAddress: Constants.GLUWACOIN_NGNG_CONTRACT_ADDRESS,
                    sUsdcgContractAddress: Constants.GLUWACOIN_SUSDCG_CONTRACT_ADDRESS,
                    sNgngContractAddress: Constants.GLUWACOIN_SNGNG_CONTRACT_ADDRESS,
                    network: Network.Main);

                return env;
            }
        }

        public static Environment Sandbox
        {
            get
            {
                Environment env = new Environment(
                    baseUrl: Constants.GLUWA_SANDBOX_API_BASE_URL,
                    usdgContractAddress: Constants.GLUWACOIN_SANDBOX_USDG_CONTRACT_ADDRESS,
                    krwgContractAddress: Constants.GLUWACOIN_SANDBOX_KRWG_CONTRACT_ADDRESS,
                    ngngContractAddress: Constants.GLUWACOIN_SANDBOX_NGNG_CONTRACT_ADDRESS,
                    sUsdcgContractAddress: Constants.GLUWACOIN_SANDBOX_SUSDCG_CONTRACT_ADDRESS,
                    sNgngContractAddress: Constants.GLUWACOIN_SANDBOX_SNGNG_CONTRACT_ADDRESS,
                    network: Network.TestNet);

                return env;
            }
        }

        public string BaseUrl { get; }

        public string UsdgContractAddress { get; }

        public string KrwgContractAddress { get; }

        public string NgngContractAddress { get; }

        public string SUsdcgContractAddress { get; }

        public string SNgngContractAddress { get; }

        public Network Network { get; }

        protected Environment (
            string baseUrl,
            string usdgContractAddress,
            string krwgContractAddress,
            string ngngContractAddress,
            string sUsdcgContractAddress,
            string sNgngContractAddress,
            Network network)
        {
            BaseUrl = baseUrl;
            UsdgContractAddress = usdgContractAddress;
            KrwgContractAddress = krwgContractAddress;
            NgngContractAddress = ngngContractAddress;
            SUsdcgContractAddress = sUsdcgContractAddress;
            SNgngContractAddress = sNgngContractAddress;
            Network = network;
        }
    }
}
