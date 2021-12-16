using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Rubberduck.Client.Abstract;
using Rubberduck.Model.Abstract;
using Rubberduck.Model.DTO;

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
                return await GetResponse<IEnumerable<Feature>>(endpoint);
            }
            catch (ApiException)
            {
                return Enumerable.Empty<Feature>();
            }
        }

        public async Task<FeatureItem> GetFeatureItemAsync(int id)
        {
            var endpoint = $"Public/FeatureItem/{id}";

            try
            {
                return await GetResponse<FeatureItem>(endpoint);
            }
            catch (ApiException)
            {
                return null;
            }
        }

        public async Task<IEnumerable<Tag>> GetLatestTagsAsync()
        {
            var endpoint = "Public/Tags";

            try
            {
                return await GetResponse<IEnumerable<Tag>>(endpoint);
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
    }
}
