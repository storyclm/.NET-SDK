using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoryCLM.SDK.Models
{
    public class StorySimpleUser
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Роль пользователя в системе
        /// </summary>
        public int Role { get; set; }
    }
}
