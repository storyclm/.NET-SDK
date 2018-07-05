using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{
    public static class Settings
    {
       static IConfiguration configuration;

        static Settings()
        {
            configuration = new ConfigurationBuilder()
                .AddJsonFile("settings.json")
                .Build();
        }

        public static string UserClientId => configuration[nameof(UserClientId)];

        public static string UserSecret => configuration[nameof(UserSecret)];

        public static string Username => configuration[nameof(Username)];

        public static string Password => configuration[nameof(Password)];

        public static string ServiceClientId => configuration[nameof(ServiceClientId)];

        public static string ServiceSecret => configuration[nameof(ServiceSecret)];
    }
}
