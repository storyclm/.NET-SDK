﻿using System.Threading.Tasks;

namespace StoryCLM.SDK.Content
{
    public class StorySimplePresentation : StorySimpleModel<StoryPresentation>
    {
        /// <summary>
        /// Режим разработки
        /// </summary>
        public bool Debug { get; set; }

        /// <summary>
        /// Режим пропуска обновления
        /// </summary>
        public bool Skip { get; set; }

        /// <summary>
        /// Блокировка
        /// </summary>
        public bool Lockout { get; set; }

        public async override Task<StoryPresentation> LoadAsync()
        {
            StoryPresentation presentation = await Context.GetPresentationAsync(Id);
            presentation._sclm = Context;
            return presentation;
        }

    }
}
