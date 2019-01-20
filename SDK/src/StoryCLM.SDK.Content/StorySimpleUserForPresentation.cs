using StoryCLM.SDK.Users;

namespace StoryCLM.SDK.Content
{
    public class StorySimpleUserForPresentation : StorySimpleUser
    {
        /// <summary>
        /// Версия презентации у пользователя
        /// </summary>
        public int Revision { get; set; }

    }
}
