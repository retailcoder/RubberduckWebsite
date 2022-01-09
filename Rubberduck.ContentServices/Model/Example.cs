using System.Collections.Generic;
using System.Linq;
using Rubberduck.Model.Abstract;
using PublicModel = Rubberduck.Model.Entities;

namespace Rubberduck.ContentServices.Model
{
    public class Example : Entity, IInternalModel<PublicModel.Example>
    {
        public Example() { }
        public Example(PublicModel.Example model)
        {
            Id = model.Id;
            DateInserted = model.DateInserted;
            DateUpdated = model.DateUpdated;

            FeatureItemId = model.FeatureItemId;
            SortOrder = model.SortOrder;
            Description = model.Description;

            FeatureItem = model.FeatureItem is null ? null : new FeatureItem(model.FeatureItem);
            Modules = model.Modules.Select(m => new ExampleModule(m)).ToList();
        }

        public int FeatureItemId { get; set; }
        public int SortOrder { get; set; }
        public string Description { get; set; }

        public virtual FeatureItem FeatureItem { get; set; }
        public virtual ICollection<ExampleModule> Modules { get; set; } = new List<ExampleModule>();

        public PublicModel.Example ToPublicModel()
        {
            return new PublicModel.Example
            {
                Id = this.Id,
                DateInserted = this.DateInserted,
                DateUpdated = this.DateUpdated,
                
                FeatureItemId = this.FeatureItemId,
                SortOrder = this.SortOrder,
                Description = this.Description,

                Modules = this.Modules.Select(m => m.ToPublicModel()).ToList()
            };
        }
    }
}
