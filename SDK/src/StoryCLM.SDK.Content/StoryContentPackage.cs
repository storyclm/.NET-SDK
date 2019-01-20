using System;
using System.Threading.Tasks;

namespace StoryCLM.SDK.Content
{
    public class StoryContentPackage : StorySimpleModel<StoryPackageSas>
    {
        internal StoryContentPackage() { }

        /// <summary>
        /// Идентификатор блоба
        /// </summary>
        public string BlobId { get; set; }

        /// <summary>
        /// Размер пакета
        /// </summary>
        public int FileSize { get; set; }
        
        /// <summary>
        /// Тип блоба
        /// </summary>
        public string MIMEType { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Дата обновления
        /// </summary>
        public DateTime Updated { get; set; }

        internal int PresentationId { get; set; }

        public async override Task<StoryPackageSas> LoadAsync() =>
            await Context.GetContentPackageAsync(PresentationId);

    }
}
