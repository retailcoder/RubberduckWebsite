using Rubberduck.Model.Abstract;
using PublicModel = Rubberduck.Model.Entities;

namespace Rubberduck.ContentServices.Model
{
    public class TagAsset : Entity, IInternalModel<PublicModel.TagAsset>
    {
        public TagAsset() { }
        public TagAsset(PublicModel.TagAsset model)
        {
            Id = model.Id;
            DateInserted = model.DateInserted;
            DateUpdated = model.DateUpdated;

            TagId = model.TagId;
            Name = model.Name;
            DownloadUrl = model.DownloadUrl;
        }

        public int TagId { get; set; }
        public string Name { get; set; }
        public string DownloadUrl { get; set; }

        public PublicModel.TagAsset ToPublicModel()
        {
            return new PublicModel.TagAsset
            {
                Id = this.Id,
                DateInserted = this.DateInserted,
                DateUpdated = this.DateUpdated,

                TagId = this.TagId,
                Name = this.Name,
                DownloadUrl = this.DownloadUrl
            };
        }
    }
}
