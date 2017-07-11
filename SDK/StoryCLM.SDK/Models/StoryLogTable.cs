using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryCLM.SDK.Models
{
    /// <summary>
    /// Запись в логе таблицы
    /// </summary>
    public class ApiLog
    {
        /// <summary>
        /// Идендификатор записи лога
        /// </summary>
        public object Id { get; set; }

        /// <summary>
        /// Идендификатор сущности в таблице
        /// </summary>
        public object TableEntityId { get; set; }

        /// <summary>
        /// Дата выполненой операции
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Тип выполненной операции
        /// 0 - insert
        /// 1 - update
        /// 2 - delete
        /// </summary>
        public int OperationType { get; set; }
    }
}
