using System;
using System.Collections.Generic;
using System.Text;

namespace StoryCLM.SDK
{
    /// <summary>
    /// Устанавливает политику повторов выполнения команды в случае возникновения исключения.
    /// Возможные варианты повторений:
    /// 1. Бесконечное повторение до успешного выполнения.
    /// 2. Заданное количество повторений.
    /// 3. Повторение по условию.
    /// 4. Комбинация выше перечилсенных пунктов.
    /// Возможные варианты задержки:
    /// 1. Без задержки.
    /// 2. Линейная задержка.
    /// 3. Экспоненциальная задержка.
    /// </summary>
    public interface IRetryPolicy
    {
        /// <summary>
        /// Количество попыток.
        /// </summary>
        int RetriesCount { get; set; }

        /// <summary>
        /// Интервал между повторениями в милисекундах.
        /// </summary>
        TimeSpan Interval { get; set; }

        /// <summary>
        /// Экспоненциальная задержка или линейная.
        /// </summary>
        bool ExponentialBackoff { get; set; }

        /// <summary>
        /// Последнее исключение.
        /// </summary>
        Exception LastException { get; set; }

        /// <summary>
        /// Проверка на необходимость сделать повтор.
        /// </summary>
        Predicate<Exception> Predicate { get; set; }
    }
}
