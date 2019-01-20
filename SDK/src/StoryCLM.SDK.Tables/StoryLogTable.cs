using System;

namespace StoryCLM.SDK.Tables
{
    public enum TableLogOperation
    {
        Insert = 0,
        Update = 1,
        Delete = 2
    }

    /// <summary>
    /// Запись в логе таблицы
    /// </summary>
    public class StoryLogTable
    {
        /// <summary>
        /// Идендификатор записи лога
        /// </summary>
        public object Id { get; set; }

        /// <summary>
        /// Идендификатор сущности в таблице
        /// </summary>
        public object EntityId { get; set; }

        /// <summary>
        /// Внешний ключ
        /// </summary>
        public object ForeignKey { get; set; }


        public object BlobId { get; set; }

        /// <summary>
        /// Дата выполненой операции
        /// </summary>
        public DateTime Created { get; set; }


        public TableLogOperation OperationType { get; set; }


    }
}
