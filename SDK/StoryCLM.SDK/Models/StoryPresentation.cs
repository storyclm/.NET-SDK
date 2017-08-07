using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoryCLM.SDK.Models
{
    public class StoryPresentation
    {
        internal SCLM _sclm;

        internal StoryPresentation() { }

        public async Task<StoryMediafile> GetMediafileAsync(int id)
        {
            if (MediaFiles == null) return null;
            if (MediaFiles.Count() == 0 || !MediaFiles.Select(t => t.Id).Contains(id)) return null;
            StoryMediafile mediafile = await _sclm.GetMediafileAsync(id);
            return mediafile;
        }

        public async Task<StorySlide> GetSlideAsync(int id)
        {
            if (Slides == null) return null;
            if (Slides.Count() == 0 || !Slides.Select(t => t.Id).Contains(id)) return null;
            StorySlide slide = await _sclm.GetSlideAsync(id);
            return slide;
        }

        public async Task<StoryPackageSas> GetContentPackageAsync()
        {
            if (SourcesFolder == null) return null;
            StoryPackageSas contentPackage = await _sclm.GetContentPackageAsync(Id);
            return contentPackage;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string ShortDescription { get; set; }

        public string LongDescription { get; set; }

        public string ThumbImgId { get; set; }

        public string ImgId { get; set; }

        public int Order { get; set; }

        public int Revision { get; set; }

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }

        public int ClientId { get; set; }

        public bool DebugModeEnabled { get; set; }

        public bool Skip { get; set; }

        /// <summary>
        /// Карта включина
        /// </summary>
        public bool MapEnabled { get; set; }

        /// <summary>
        /// Тип карты
        /// </summary>
        public int MapType { get; set; }

        /// <summary>
        /// Подтверждение перед выходом
        /// </summary>
        public bool NeedConfirmation { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int PreviewMode { get; set; }

        /// <summary>
        /// Карта 
        /// </summary>
        public string Map { get; set; }

        /// <summary>
        /// Видимость презентации 
        /// </summary>
        public bool Visibility { get; set; }

        public StoryContentPackage SourcesFolder { get; set; }

        /// <summary>
        /// Список медиафайлов
        /// </summary>
        public IEnumerable<StorySimpleModel> MediaFiles { get; set; }

        /// <summary>
        /// Список слайдов
        /// </summary>
        public IEnumerable<StorySimpleModel> Slides { get; set; }

        /// <summary>
        /// Список презентаций, коорые необходимы для корректной работы
        /// </summary>
        public IEnumerable<int> Presentations { get; set; }

        /// <summary>
        /// Пользователи
        /// </summary>
        public IEnumerable<StorySimpleUserForPresentation> Users { get; set; }

    }
}
