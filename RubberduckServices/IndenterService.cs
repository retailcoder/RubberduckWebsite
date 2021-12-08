using System.Linq;
using System.Threading.Tasks;
using Rubberduck.SmartIndenter;
using RubberduckServices.Abstract;
using RubberduckServices.Internal;

namespace RubberduckServices
{

    public class IndenterService : IIndenterService
    {
        private readonly ISimpleIndenter _indenter;

        public IndenterService(ISimpleIndenter indenter)
        {
            _indenter = indenter;
        }

        public async Task<string[]> IndentAsync(string code, Abstract.IIndenterSettings settings)
        {
            var adapter = new IndenterSettingsAdapter(settings);
            return await Task.FromResult(_indenter.Indent(code, adapter).ToArray());
        }
    }
}
