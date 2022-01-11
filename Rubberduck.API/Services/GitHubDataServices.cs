using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Extensions.Configuration;
using Octokit;
using Rubberduck.API.Extensions;
using Rubberduck.API.Services.Abstract;
using Rubberduck.Model;
using Rubberduck.Model.Entities;

namespace Rubberduck.API.Services
{
    internal class GitHubDataServices : IGitHubDataServices
    {
        private static readonly string _owner = "rubberduck-vba";
        private static readonly string _repository = "Rubberduck";

        private readonly string _codeInspectionDefaultsSettingsRawURl;
        private readonly string _userAgent;
        private readonly string _apiKey;

        public GitHubDataServices(IConfiguration configuration)
        {
            _userAgent = configuration.GetSection("GitHub")["UserAgent"];
            _codeInspectionDefaultsSettingsRawURl = configuration.GetSection("GitHub")["CodeInspectionDefaultsSettingsRawUrl"];
            _apiKey = configuration["rdapi_GITHUBAPIKEY"];
        }

        public async Task<IEnumerable<InspectionDefaultConfig>> GetCodeAnalysisDefaultsConfig()
        {
            var url = _codeInspectionDefaultsSettingsRawURl;
            using (var client = new HttpClient())
            using (var response = await client.GetAsync(new Uri(url)))
            {
                if (response.IsSuccessStatusCode)
                {
                    using var xml = await response.Content.ReadAsStreamAsync();
                    var document = await XDocument.LoadAsync(xml, LoadOptions.None, CancellationToken.None)
                        .ContinueWith(t => ParseInspectionSettings(t.Result), TaskContinuationOptions.OnlyOnRanToCompletion);

                    return document.Elements("CodeInspection")
                        .Select(e => new InspectionDefaultConfig
                        {
                            InspectionName = e.Attribute("Name").Value,
                            InspectionType = e.Attribute("InspectionType").Value,
                            DefaultSeverity = e.Attribute("Severity").Value,
                        });
                }
                else
                {
                    Console.WriteLine(response.ToString());
                    throw new HttpRequestException(response.ReasonPhrase);
                }
            }

            static XElement ParseInspectionSettings(XDocument rawSettings)
            {
                var settingNode = (XElement)rawSettings.Root.LastNode;
                var inspectionSettingsNode = (XElement)settingNode.FirstNode;
                var encodedValue = ((XElement)inspectionSettingsNode.FirstNode).Value;
                var decodedValue = WebUtility.HtmlDecode(encodedValue);
                return XDocument.Parse(decodedValue, LoadOptions.None).Root.Element("CodeInspections");
            }
        }

        public async Task<Tag> GetTagAsync(string name = null, int? id = null)
        {
            var tokenAuth = new Credentials(_apiKey);
            var client = new GitHubClient(new ProductHeaderValue(_userAgent)) { Credentials = tokenAuth };

            var release = string.IsNullOrEmpty(name)
                ? await client.Repository.Release.GetLatest(_owner, _repository)
                : await client.Repository.Release.Get(_owner, _repository, name);

            var installer = release.Assets.SingleOrDefault(ReleaseAssetExtensions.IsInstallerAsset);
            var tag = new Tag
            {
                DateCreated = release.CreatedAt.UtcDateTime,
                IsPreRelease = release.Prerelease,
                Name = release.TagName,
                InstallerDownloadUrl = installer?.BrowserDownloadUrl ?? string.Empty,
                InstallerDownloads = installer?.DownloadCount ?? 0,
            };

            tag.Id = id ?? default;

            var assets = release.Assets.Where(ReleaseAssetExtensions.IsXmlDocAsset)
                .Select(a => new TagAsset
                {
                    TagId = id ?? default,
                    Name = a.Name,
                    DownloadUrl = a.BrowserDownloadUrl
                })
                .ToList();

            tag.TagAssets = assets;
            return tag;
        }

        public async Task<IEnumerable<Tag>> GetAllTagsAsync()
        {
            var tokenAuth = new Credentials(_apiKey);
            var client = new GitHubClient(new ProductHeaderValue(_userAgent)) { Credentials = tokenAuth };

            var releases = await client.Repository.Release.GetAll(_owner, _repository);

            return from release in releases
                   let installer = release.Assets.SingleOrDefault(ReleaseAssetExtensions.IsInstallerAsset)
                   select new Tag
                   {
                        DateCreated = release.CreatedAt.UtcDateTime,
                        IsPreRelease = release.Prerelease,
                        Name = release.TagName,
                        InstallerDownloadUrl = installer?.BrowserDownloadUrl ?? string.Empty,
                        InstallerDownloads = installer?.DownloadCount ?? 0,
                   };
        }
    }
}
