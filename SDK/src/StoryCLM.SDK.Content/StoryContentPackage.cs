using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoryCLM.SDK.Content
{
    public class StoryContentPackage : ISCLMObject<StoryPackageSas>
    {
        internal StoryContentPackage() { }

        /// <summary>
        /// Идентификатор
        /// </summary>
        public int Id { get; set; }

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
        /// Рефизия
        /// </summary>
        public int Revision { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Дата обновления
        /// </summary>
        public DateTime Updated { get; set; }

        public SCLM Context { get; private set; }

        public async Task<StoryPackageSas> LoadAsync() =>
            await Context.GetContentPackageAsync(Id);

        public void SetContext(SCLM context) =>
            Context = context;
    }
}
