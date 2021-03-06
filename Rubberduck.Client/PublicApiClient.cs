using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Rubberduck.Client.Abstract;
using Rubberduck.Model;
using Rubberduck.Model.Abstract;
using Rubberduck.Model.Entities;

namespace Rubberduck.Client
{
    public class PublicApiClient : ApiClientBase, IPublicApiClient
    { 
        public PublicApiClient(ILogger<PublicApiClient> logger, IConfiguration configuration)
            : base(logger, configuration)
        {

        }

        public async Task<IEnumerable<Feature>> GetFeaturesAsync()
        {
            var endpoint = "Public/Features";

            try
            {
                return await GetResponse<Feature[]>(endpoint);
            }
            catch (ApiException)
            {
                return Enumerable.Empty<Feature>();
            }
        }

        public async Task<IEnumerable<Tag>> GetLatestTagsAsync()
        {
            var endpoint = "Public/Tags";

            try
            {
                return await GetResponse<Tag[]>(endpoint);
            }
            catch (ApiException)
            {
                return Enumerable.Empty<Tag>();
            }
        }

        public async Task<string[]> GetIndentedCodeAsync(IIndenterSettings settings)
        {
            var endpoint = "Public/Indent";

            try
            {
                return await Post<IIndenterSettings, string[]>(endpoint, settings);
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (ApiException)
            {
                return null;
            }
        }

        public async Task<Feature> GetFeatureAsync(string name)
        {
            var endpoint = $"Public/Features/{name}";

            try
            {
                return await GetResponse<Feature>(endpoint);
            }
            catch (ApiException)
            {
                return null;
            }
        }

        public async Task<FeatureItem> GetFeatureItemAsync(string name)
        {
            var endpoint = $"Public/FeatureItem/{name}";

            try
            {
                return await GetResponse<FeatureItem>(endpoint);
            }
            catch (ApiException)
            {
                return null;
            }
        }

        public async Task<IndenterViewModel> GetDefaultIndenterSettings()
        {
            var endpoint = $"Public/DefaultIndenterSettings";

            try
            {
                return await GetResponse<IndenterViewModel>(endpoint);
            }
            catch (ApiException)
            {
                return null;
            }
        }

        public async Task<SearchResultsViewModel> SearchContentAsync(SearchViewModel search)
        {
            var endpoint = $"Public/Search";

            try
            {
                return await Post<SearchViewModel, SearchResultsViewModel>(endpoint, search);
            }
            catch (ApiException)
            {
                return new SearchResultsViewModel(search.Query);
            }
        }
    }
}
