using System.Threading.Tasks;

namespace StoryCLM.SDK.Users
{
    public class StorySimpleUser : ISCLMObject<StoryUser>
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Роль пользователя в системе
        /// </summary>
        public int Role { get; set; }

        public SCLM Context  { get; internal set;}

        public async Task<StoryUser> LoadAsync() =>
            await Context.GetUserAsync(Id);

        public void SetContext(SCLM context) =>
            Context = context;
    }
}
