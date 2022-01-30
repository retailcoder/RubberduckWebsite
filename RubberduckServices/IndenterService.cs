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

        public string IndenterVersion()
        {
            return typeof(Rubberduck.SmartIndenter.IIndenterSettings).Assembly.GetName().Version.ToString(3);
        }

        public async Task<string[]> IndentAsync(Rubberduck.Model.Abstract.IIndenterSettings settings)
        {
            var adapter = new IndenterSettingsAdapter(settings);
            return await Task.FromResult(_indenter.Indent(adapter.Code, adapter).ToArray());
        }
    }
}
