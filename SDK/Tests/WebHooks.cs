/*!
* StoryCLM.SDK Library v0.5.0
* Copyright(c) 2016, Vladimir Klyuev, Breffi Inc. All rights reserved.
* License: Licensed under The MIT License.
*/
using Newtonsoft.Json;
using StoryCLM.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Tests.Models;
using Xunit;

namespace Tests
{
   public class WebHooks
    {

        public static IDictionary<string, object> ToTablesDictionary(XElement element)
        {

            IEnumerable<XElement> elements = element.Elements();
            if (elements.Count() == 0) throw new ArgumentException("Elements is empty");
            Dictionary<string, object> result = new Dictionary<string, object>();
            foreach (var t in elements)
                result[t.Name.LocalName] = t.Value;
            return result;
        }

        static XDocument ToXElement(IDictionary<string, object> dictionary)
        {
            return new XDocument(new XDeclaration("1.0", "utf-8", "yes"),
                new XElement("object", dictionary.Select(kv => new XElement(kv.Key, kv.Value))));
        }

        [Theory]
        [InlineData("5883a11045908640c4184eb9", "1146c8d4540d4d22b57d49446f1350d1", "application/xml")]
        public async void SalesforceWebHook(string id, string key, string contentType)
        {
            SCLM sclm = SCLM.Instance;
            await sclm.WebHookAsync(id, key, Properties.Resources.Salesforce, contentType);
        }

        [Theory]
        [InlineData("588df8ae459086281cfc91e1", "7b2e0329f5bd4113b16a7e9ab5fd27b6", "application/xml")]
        public async void TablesXmlRecevierHandler(string id, string key, string contentType)
        {
            SCLM sclm = SCLM.Instance;

            //добавить запись
            var insertData = new Dictionary<string, object>
            {
                ["Name"] = "Insert",
                ["Age"] = 1000,
                ["Gender"] = true,
                ["Rating"] = 11.1,
                ["Created"] = DateTime.Now
            };
            string insertString = ToXElement(insertData).ToString();
            string insertStringResult = await sclm.WebHookAsync(id, key, insertString, contentType);

            //редактируем запись
            var updateData = ToTablesDictionary(XElement.Parse(insertStringResult));
            updateData["Name"] = "Update";
            updateData["Age"] = 2000;
            updateData["Rating"] = 22.2;
            string updateString = ToXElement(updateData).ToString();
            string updateStringResult = await sclm.WebHookAsync(id, key, updateString, contentType);

            var removeData = ToTablesDictionary(XElement.Parse(updateStringResult));
            removeData.Remove("Name");
            removeData.Remove("Age");
            removeData.Remove("Gender");
            removeData.Remove("Rating");
            removeData.Remove("Created");
            string deleteString = ToXElement(removeData).ToString();
            string deleteStringResult = await sclm.WebHookAsync(id, key, deleteString, contentType);

        }


        [Theory]
        [InlineData("588f2134bebec41aa82f7da8", "26920b6c82e84374a5b14bd32f9ed6d6", "application/json")]
        public async void TablesJsonRecevierHandler(string id, string key, string contentType)
        {
            SCLM sclm = SCLM.Instance;
            Profile profile = Profile.CreateProfile();
            string insertProfileResult = await sclm.WebHookAsync(id, key, JsonConvert.SerializeObject(profile, Formatting.Indented), contentType);
            Profile updateProfile =  Profile.UpdateProfile(JsonConvert.DeserializeObject<Profile>(insertProfileResult));
            string updateProfileResult = await sclm.WebHookAsync(id, key, JsonConvert.SerializeObject(updateProfile, Formatting.Indented), contentType);

        }
    }

}
