using System.Collections.Generic;
using System.Linq;
using Rubberduck.Model.Internal;

namespace Rubberduck.ContentServices.XmlDoc
{
    public class BeforeAndAfterCodeExample
    {
        public BeforeAndAfterCodeExample(IEnumerable<ExampleModule> modulesBefore, IEnumerable<ExampleModule> modulesAfter)
        {
            ModulesBefore = modulesBefore;
            ModulesAfter = modulesAfter;
        }

        public IEnumerable<ExampleModule> ModulesBefore { get; }
        public IEnumerable<ExampleModule> ModulesAfter { get; }

        public Example AsExample(string description = "", int sortOrder = 0) => 
            Example.FromDTO(new Model.DTO.Example { Description = description, SortOrder = sortOrder }, ModulesBefore.Concat(ModulesAfter));
    }
}
