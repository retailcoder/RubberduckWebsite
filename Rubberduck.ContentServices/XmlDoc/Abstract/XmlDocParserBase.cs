using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Rubberduck.ContentServices.Model;

namespace Rubberduck.ContentServices.XmlDoc.Abstract
{
    public abstract class XmlDocParserBase : IXmlDocParser
    {
        protected XmlDocParserBase(string assetName)
        {
            AssetName = assetName;
        }

        public string AssetName { get; }

        public async Task<IEnumerable<FeatureItem>> ParseAsync(Rubberduck.Model.Entities.Tag tag)
        {
            var asset = tag.TagAssets.SingleOrDefault(a => a.Name.Contains(AssetName))
                ?? throw new InvalidOperationException($"Asset '{AssetName}' was not found under tag {tag.Name}.");

            if (Uri.TryCreate(asset.DownloadUrl, UriKind.Absolute, out var uri) && uri.Host != "github.com")
            {
                throw new UriFormatException($"Unexpected host in download URL '{uri}' from asset ID {asset.Id} (tag ID {tag.Id}, '{tag.Name}').");
            }

            using (var client = new HttpClient())
            using (var response = await client.GetAsync(uri))
            {
                if (response.IsSuccessStatusCode)
                {
                    using var stream = await response.Content.ReadAsStreamAsync();
                    var document = await XDocument.LoadAsync(stream, LoadOptions.None, CancellationToken.None);

                    var items = await ParseAsync(asset.Id, document, tag.IsPreRelease);
                    return items;
                }
            }

            return Enumerable.Empty<FeatureItem>();
        }

        protected abstract Task<IEnumerable<FeatureItem>> ParseAsync(int assetId, XDocument document, bool isPreRelease);
    }
}
