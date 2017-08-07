﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoryCLM.SDK.Models
{
    public class StorySimpleGroup : StorySimpleTable
    {
        /// <summary>
        /// Пользователи группы
        /// </summary>
        public IEnumerable<string> Users { get; set; }
    }
}
