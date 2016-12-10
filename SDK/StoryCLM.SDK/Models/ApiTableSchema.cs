using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryCLM.SDK.Models
{
    /// <summary>
    /// Элемент схемы таблицы
    /// </summary>
    public class ApiTableSchema
    {
        /// <summary>
        /// Название поля
        /// </summary>
        public string k { get; set; }

        /// <summary>
        /// Тип поля
        /// </summary>
        public int t { get; set; }
    }
}
