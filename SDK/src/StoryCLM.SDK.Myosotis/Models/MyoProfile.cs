using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryCLM.SDK.Myosotis.Models
{
    public class MyoProfile
    {
        [JsonProperty(PropertyName = "userId")]
        public string UserId { get; set; }

        [JsonProperty(PropertyName = "xmppLogin")]
        public string XmppLogin { get; set; }

        [JsonProperty(PropertyName = "isDoctor")]
        public bool? IsDoctor { get; set; }

        [JsonProperty(PropertyName = "isValid")]
        public bool? IsValid { get; set; }

        [JsonProperty(PropertyName = "firstName")]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "patronymic")]
        public string Patronymic { get; set; }

        [JsonProperty(PropertyName = "city")]
        public string City { get; set; }

        [JsonProperty(PropertyName = "medicalInstitution")]
        public string MedicalInstitution { get; set; }

        [JsonProperty(PropertyName = "specialization")]
        public string Specialization { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty(PropertyName = "gender")]
        public bool? Gender { get; set; }

        [JsonProperty(PropertyName = "birthDate")]
        public DateTime? BirthDate { get; set; }

    }
}
