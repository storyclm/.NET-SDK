using StoryCLM.SDK;
using StoryCLM.SDK.Authentication;
using System.Threading.Tasks;

namespace Shared
{
    public static class Utilities
    {
        public static int clientId = 18;
        public static int tableId = 23;

        public static async Task<SCLM> GetContextAsync(int uc)
        {
            switch (uc)
            {
                case 0: // service
                    {
                        SCLM sclm = new SCLM();
                        await sclm.AuthAsync(Settings.Get().ServiceClientId, Settings.Get().ServiceSecret);
                        return sclm;
                    }
                case 1: // application (user)
                    {
                        SCLM sclm = new SCLM();
                        await sclm.AuthAsync(Settings.Get().UserClientId, Settings.Get().UserSecret, Settings.Get().Username, Settings.Get().Password);
                        return sclm;
                    }
            }
            return null;
        }
    }
}
