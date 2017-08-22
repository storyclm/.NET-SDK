using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoryCLM.SDK.Models
{
    public class StoryUser
    {
        internal SCLM _sclm;

        internal StoryUser() { }

        public string Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Name { get; set; }

        public string Location { get; set; }

        public DateTime? BirthDate { get; set; }

        public bool? Gender { get; set; }

        public async Task SaveAsync() =>
            await _sclm.PUTAsync<StoryUser>(_sclm.kUsers, this);

        public async Task ChangePasswordAsync(string password) =>
           await _sclm.PUTAsync(_sclm.kUsers + Id + "/password", new StoryPassword { Password = password });

        public async Task AddToGroupAsync(int groupId) =>
            await _sclm.PUTAsync(_sclm.kUsers + Id + "/group/" + groupId, null);

        public async Task RemoveFromGroupAsync(int groupId) =>
            await _sclm.DELETEAsync(_sclm.kUsers + Id + "/group/" + groupId, null);

        public async Task AddToPresentationAsync(int presentationId) =>
            await _sclm.PUTAsync(_sclm.kUsers + Id + "/presentation/" + presentationId, null);

        public async Task RemoveFromPresentationAsync(int presentationId) =>
            await _sclm.DELETEAsync(_sclm.kUsers + Id + "/presentation/" + presentationId, null);

        public async Task<IEnumerable<StorySimpleUserItem>> GetPresentations() =>
            await _sclm.GETAsync<IEnumerable<StorySimpleUserItem>>(_sclm.kUsers + Id + "/presentations", string.Empty);

        public async Task<IEnumerable<StorySimpleUserItem>> GetGroups() =>
            await _sclm.GETAsync<IEnumerable<StorySimpleUserItem>>(_sclm.kUsers + Id + "/groups", string.Empty);

    }
}
