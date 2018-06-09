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
        public static int clientId = 18;
        public static int tableId = 23;

        public static async Task<SCLM> GetContextAsync(int uc)
        {
            switch (uc)
            {
                case 0: // service
                    {
                        string clientId = "client_18_1";
                        string secret = "d17ac10538ec402b9e2355dd3e2be0332b7f9dfa086645f3adcbff8c7208c94d";
                        SCLM sclm = new SCLM();
                        await sclm.AuthAsync(clientId, secret);
                        return sclm;
                    }
                case 1: // application (user)
                    {
                        string client = "client_18_4";
                        string secret = "";
                        string password = "";
                        string username = "rsk-k161@ya.ru";
                        SCLM sclm = new SCLM();
                        await sclm.AuthAsync(client, secret, username, password);
                        return sclm;
                    }
            }
            return null;
        }
    }
}
