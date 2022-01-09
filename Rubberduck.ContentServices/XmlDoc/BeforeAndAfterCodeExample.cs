using System.Collections.Generic;
using System.Linq;
using Rubberduck.ContentServices.Model;

namespace Rubberduck.ContentServices.XmlDoc
{
    public class BeforeAndAfterCodeExample
    {
        public BeforeAndAfterCodeExample(IEnumerable<ExampleModule> modulesBefore, IEnumerable<ExampleModule> modulesAfter)
        {
            ModulesBefore = modulesBefore ?? Enumerable.Empty<ExampleModule>();
            ModulesAfter = modulesAfter ?? Enumerable.Empty<ExampleModule>();
        }

        public IEnumerable<ExampleModule> ModulesBefore { get; }
        public IEnumerable<ExampleModule> ModulesAfter { get; }

        public Example AsExample(string description = "", int sortOrder = 0) => new()
        {
            Description = description,
            SortOrder = sortOrder,
            Modules = ModulesBefore.Concat(ModulesAfter).ToList()
        };
    }
}
