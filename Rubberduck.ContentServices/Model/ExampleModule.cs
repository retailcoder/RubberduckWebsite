using System.Linq;
using Rubberduck.Model.Abstract;
using PublicModel = Rubberduck.Model.Entities;

namespace Rubberduck.ContentServices.Model
{
    public class ExampleModule : Entity, IInternalModel<PublicModel.ExampleModule>
    {
        public static ExampleModule ParseError(string name) => new()
        {
            ModuleName = name,
            HtmlContent = "(error parsing code example from source xmldoc)"
        };

        public ExampleModule() { }
        public ExampleModule(PublicModel.ExampleModule model)
        {
            Id = model.Id;
            DateInserted = model.DateInserted;
            DateUpdated = model.DateUpdated;

            ExampleId = model.ExampleId;
            SortOrder = model.SortOrder;
            ModuleName = model.ModuleName;
            ModuleTypeId = (int)model.ModuleType;
            Description = model.Description;
            HtmlContent = model.HtmlContent;

            Example = new Example(model.Example);
        }

        public int ExampleId { get; set; }
        public int SortOrder { get; set; }
        public string ModuleName { get; set; }
        public int ModuleTypeId { get; set; }
        public string Description { get; set; }
        public string HtmlContent { get; set; }

        public virtual Example Example { get; set; }

        public PublicModel.ExampleModule ToPublicModel()
        {
            return new PublicModel.ExampleModule
            {
                Id = this.Id,
                DateInserted = this.DateInserted,
                DateUpdated = this.DateUpdated,

                ExampleId = this.ExampleId,
                SortOrder = this.SortOrder,
                ModuleName = this.ModuleName,
                ModuleType = (PublicModel.ExampleModuleType)this.ModuleTypeId,
                Description = this.Description,
                HtmlContent = this.HtmlContent
            };
        }
    }
}
