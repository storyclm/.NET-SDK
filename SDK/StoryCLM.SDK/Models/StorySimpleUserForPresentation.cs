using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoryCLM.SDK.Models
{
    public class StorySimpleUserForPresentation : StorySimpleUser
    {
        /// <summary>
        /// Версия презентации у пользователя
        /// </summary>
        public int Revision { get; set; }

    }
}
