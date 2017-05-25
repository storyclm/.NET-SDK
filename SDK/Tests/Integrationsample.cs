/*!
* StoryCLM.SDK Library v1.0.0
* Copyright(c) 2017, Vladimir Klyuev, Breffi Inc. All rights reserved.
* License: Licensed under The MIT License.
*/
using Newtonsoft.Json;
using StoryCLM.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
   public class Integrationsample
    {
        const string clientId = "client_20";
        const string secret = "";

        [Theory]
        [InlineData(44)]
        public async void Insert(int tableId)
        {
            SCLM sclm = SCLM.Instance;
            await sclm.AuthAsync(clientId, secret);
            List<Contact> contacts = new List<Contact>(await sclm.InsertAsync<Contact>(tableId, Contact.CreateContacts()));
            contacts = new List<Contact>(await sclm.FindAsync<Contact>(tableId, skip: 0, take: 100));
            Contact.UpdateContacts(contacts);
            await sclm.UpdateAsync<Contact>(tableId, contacts);
            await sclm.DeleteAsync<Contact>(tableId, contacts.Select(t=>t._id));

        }

        public class Contact
        {
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int Id { get; set; }

            public string _id { get; set; }

            public string Name { get; set; }

            public string Company { get; set; }

            public string Email { get; set; }

            public string Phone { get; set; }

            public string Interest { get; set; }

            public static List<Contact> CreateContacts()
            {
                List<Contact> result = new List<Contact>();
                for (int i = 0; i < 3; i++)
                {
                    result.Add(new Contact() {
                        Company = "iTR",
                        Email = "v@itr.com",
                        Interest = "StoryCLM",
                        Name = "Vladimir Klyuev",
                        Phone = "0000000000"
                    });
                }
                return result;
            }

            public static void UpdateContacts(List<Contact> contacts)
            {
                foreach (var t in contacts)
                {
                    t.Interest = "StoryCLM";
                    t.Name = "Vladimir";
                    t.Phone = "1111111111";
                    t.Company = "Breffi";
                }
            }

        }


    }
}
