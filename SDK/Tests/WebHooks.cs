/*!
* StoryCLM.SDK Library v0.5.0
* Copyright(c) 2016, Vladimir Klyuev, Breffi Inc. All rights reserved.
* License: Licensed under The MIT License.
*/
using StoryCLM.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
   public class WebHooks
    {
        [Theory]
        [InlineData("587b7b2891f2ce5fd4cf5062", "0dc97e117124428c9eb292da2087ba3a", "application/xml")]
        public async void SalesforceWebHook(string id, string key, string contentType)
        {
            SCLM sclm = SCLM.Instance;
            await sclm.WebHookAsync(id, key, Properties.Resources.Salesforce, contentType);

        }
    }

}
