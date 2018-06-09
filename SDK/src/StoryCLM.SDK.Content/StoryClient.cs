using StoryCLM.SDK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoryCLM.SDK.Content
{
    public class StoryClient
    {
        internal SCLM _sclm;

        internal StoryClient() { }

        public async Task<StoryPresentation> GetPresentationAsync(int id)
        {
            if (Presentations == null) return null;
            if (Presentations.Count() == 0 || !Presentations.Select(t => t.Id).Contains(id)) return null;
            StoryPresentation presentation = await _sclm.GetPresentationAsync(id);
            presentation._sclm = _sclm;
            return presentation;
        }

        /// <summary>
        /// Идентификатор клиента в системе
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название клиента
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Клиент заблокирован
        /// </summary>
        public bool LockoutEnabled { get; set; }

        /// <summary>
        /// Короткое описание
        /// </summary>
        public string ShortDescription { get; set; }

        /// <summary>
        /// Полное описание
        /// </summary>
        public string LongDescription { get; set; }

        /// <summary>
        /// URL логтипа
        /// </summary>
        public string ThumbImgId { get; set; }

        /// <summary>
        /// URL арта
        /// </summary>
        public string ImgId { get; set; }
        
        /// <summary>
        /// URL сслыка на ресурс о компании или продукта
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Адрес электронной почты контактного лица
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Дата создания клиента
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Дата последнего изменения свойст клиента
        /// </summary>
        public DateTime Updated { get; set; }

        /// <summary>
        /// Идентификатор таблицы в которой хранится лог устройства
        /// </summary>
        public string DeviceLogTable { get; set; }

        /// <summary>
        /// Идетификатор таблицы которая хранит базовую статистику
        /// </summary>
        public string BaseStatisticsTable { get; set; }

        /// <summary>
        /// Идентификатор таблицы коорая хранит геолокацию пользователей
        /// </summary>
        public string GeolocationTable { get; set; }

        /// <summary>
        /// Список презентаций клиента
        /// </summary>
        public IEnumerable<StorySimplePresentation> Presentations { get; set; }

        /// <summary>
        /// Список пользователей клиента
        /// </summary>
        public IEnumerable<StorySimpleUser> Users { get; set; }

        /// <summary>
        /// Список групп клиента
        /// </summary>
        public IEnumerable<StorySimpleGroup> Groups { get; set; }

        /// <summary>
        /// Список таблиц клиента
        /// </summary>
        public IEnumerable<StorySimpleTable> Tables { get; set; }
    }
}
