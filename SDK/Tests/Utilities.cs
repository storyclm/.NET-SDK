using StoryCLM.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
   public static class Utilities
    {
        public static int uc = 1;
        public static int clientId = 18;
        public static int tableId = 23;

        public static async Task<SCLM> GetContextAsync()
        {
            switch (uc)
            {
                case 0: // service
                    {
                        string clientId = "client_18_1";
                        string secret = "";
                        SCLM sclm = new SCLM();
                        await sclm.AuthAsync(clientId, secret);
                        return sclm;
                    }
                case 1: // application (user)
                    {
                        string username = "rsk-k161@ya.ru";
                        string password = "";
                        string clientId = "client_18_4";
                        string secret = "";
                        SCLM sclm = new SCLM();
                        await sclm.AuthAsync(clientId, secret, username, password);
                        return sclm;
                    }
            }
            return null;
        }
    }
}
