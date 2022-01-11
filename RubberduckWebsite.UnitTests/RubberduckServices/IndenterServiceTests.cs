using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rubberduck.Model;
using Rubberduck.SmartIndenter;
using RubberduckServices;
using RubberduckServices.Abstract;

namespace RubberduckWebsite.UnitTests.RubberduckServices
{
    [TestClass]
    public class IndenterServiceTests
    {
        private static IIndenterService CreateSUT()
        {
            return new IndenterService(new SimpleIndenter());
        }

        [TestMethod]
        public async Task TestMethod1()
        {
            var code = "Option Explicit";
            var viewModel = new IndenterViewModel { Code = code };

            var sut = CreateSUT();
            var result = await sut.IndentAsync(viewModel);

            Assert.IsNotNull(result);
        }
    }
}
