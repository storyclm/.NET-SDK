using StoryCLM.SDK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StoryCLM.SDK.Users
{
    public class StoryUser
    {
        public SCLM Context { get; set; }

        public StoryUser()
        {
            Groups = Enumerable.Empty<StorySimpleUserItem>();
        }

        Uri GetUri(string query = null) =>
            new Uri($"{Context.GetEndpoint("api")}{UsersExtensions.Version}/{UsersExtensions.Path}/{query}", UriKind.Absolute);

        public string Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Name { get; set; }

        public string Location { get; set; }

        public DateTime? BirthDate { get; set; }

        public bool? Gender { get; set; }

        public IEnumerable<StorySimpleUserItem> Groups { get; set; }

        public IEnumerable<StorySimpleUserItem> Presentations { get; set; }

        public async Task SaveAsync(CancellationToken token = default(CancellationToken)) =>
            await Context.PUTAsync<StoryUser>(GetUri(), this, token);

        public async Task ChangePasswordAsync(string password, CancellationToken token = default(CancellationToken)) =>
           await Context.PUTAsync<Object>(GetUri(Id + "/password"), new StoryPassword { Password = password }, token);

        public async Task AddToGroupAsync(int groupId, CancellationToken token = default(CancellationToken)) =>
            await Context.PUTAsync<Object>(GetUri(Id + "/group/" + groupId), null, token);

        public async Task RemoveFromGroupAsync(int groupId, CancellationToken token = default(CancellationToken)) =>
            await Context.DELETEAsync<Object>(GetUri(Id + "/group/" + groupId), token);

        public async Task AddPresentationAsync(int presentationId, CancellationToken token = default(CancellationToken)) =>
            await Context.PUTAsync<Object>(GetUri(Id + "/presentation/" + presentationId), null, token);

        public async Task<IEnumerable<int>> AddPresentationsAsync(IEnumerable<int> ids, CancellationToken token = default(CancellationToken)) =>
            await Context.POSTAsync<IEnumerable<int>>(GetUri(Id + "/presentations"), ids, token);

        public async Task<IEnumerable<int>> SyncPresentationsAsync(IEnumerable<int> ids, CancellationToken token = default(CancellationToken)) =>
            await Context.PUTAsync<IEnumerable<int>>(GetUri(Id + "/presentations"), ids, token);

        public async Task RemoveFromPresentationAsync(int presentationId, CancellationToken token = default(CancellationToken)) =>
            await Context.DELETEAsync<Object>(GetUri(Id + "/presentation/" + presentationId), token);

        /// <summary>
        /// Загружает пользователей, в которым имеет доступ пользователь.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<IEnumerable<StorySimpleUserItem>> LoadPresentations(CancellationToken token = default(CancellationToken))
        {
            Presentations = await Context.GETAsync<IEnumerable<StorySimpleUserItem>>(GetUri(Id + "/presentations"), token) ?? Enumerable.Empty<StorySimpleUserItem>();
            return Presentations;
        }

        /// <summary>
        /// Загружает группы, в которых состоит пользователь.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<StorySimpleUserItem>> LoadGroups(CancellationToken token = default(CancellationToken))
        {
            Groups = await Context.GETAsync<IEnumerable<StorySimpleUserItem>>(GetUri(Id + "/groups"), token) ?? Enumerable.Empty<StorySimpleUserItem>();
            return Groups;
        }

    }
}
