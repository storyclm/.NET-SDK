using System.Threading.Tasks;

namespace StoryCLM.SDK.Content
{
    public class StorySimpleSlide : StorySimpleModel<StorySlide>
    {
        public async override Task<StorySlide> LoadAsync() =>
            await Context.GetSlideAsync(Id);
    }
}
