using System.Collections.Generic;
using Rubberduck.Model.Abstract;

namespace Rubberduck.Model.Entities
{
    public class Example : Entity
    {
        public int FeatureItemId { get; set; }
        public int SortOrder { get; set; }
        public string Description { get; set; }

        public virtual FeatureItem FeatureItem { get; set; }
        public virtual ICollection<ExampleModule> Modules { get; set; } = new List<ExampleModule>();
    }
}
