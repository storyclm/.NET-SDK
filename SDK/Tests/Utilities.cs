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
        public static string client = "client_18_4";
        public static string secret = "1cdbbf4374634314bfd5607a79a0b5578d05130732dc4a37ac8c046525a27075";
        public static string password = "Qwe510091#";
        public static string username = "rsk-k161@ya.ru";

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
                        SCLM sclm = new SCLM();
                        await sclm.AuthAsync(client, secret, username, password);
                        return sclm;
                    }
            }
            return null;
        }
    }
}
