using System.Collections.Generic;

namespace StoryCLM.SDK.Content
{
    public class StorySimpleGroup : StorySimpleTable
    {
        /// <summary>
        /// Пользователи группы
        /// </summary>
        public IEnumerable<string> Users { get; set; }
    }
}
