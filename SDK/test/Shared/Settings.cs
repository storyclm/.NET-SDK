using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{
    public class Settings
    {
        static IConfiguration configuration;

        static Settings()
        {
            configuration = new ConfigurationBuilder()
                .AddJsonFile("settings.json")
                .Build();
        }

        public static Settings Get() => configuration.Get<Settings>();

        public string UserClientId { get; set; }

        public string UserSecret { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string ServiceClientId { get; set; }

        public string ServiceSecret { get; set; }

        public string IoTHub { get; set; }

        public string IoTKey { get; set; }

        public string IoTSecret { get; set; }


    }
}
