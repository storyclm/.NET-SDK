using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoryCLM.SDK.Models
{
    public class StorySimplePresentation : StorySimpleModel
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
    }
}
