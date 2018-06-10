using StoryCLM.SDK.Models;
using StoryCLM.SDK.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
