using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Rubberduck.Client.Abstract;
using Rubberduck.Model.Entities;

namespace Rubberduck.Client
{

    public class AdminApiClient : ApiClientBase, IAdminApiClient
    {
        public AdminApiClient(ILogger<ApiClientBase> logger, IConfiguration configuration) 
            : base(logger, configuration)
        {

        }


        protected override HttpClient GetClient()
        {
            // TODO add authentication headers
            return base.GetClient();
        }

        public async Task<Feature> SaveFeatureAsync(Feature dto)
        {
            var endpoint = "Admin/SaveFeature";

            try
            {
                return await Post(endpoint, dto);
            }
            catch (ApiException)
            {
                return null;
            }
        }

        public async Task<FeatureItem> SaveFeatureItemAsync(FeatureItem dto)
        {
            var endpoint = "Admin/SaveFeatureItem";

            try
            {
                return await Post(endpoint, dto);
            }
            catch (ApiException)
            {
                return null;
            }
        }

        public async Task<bool> UpdateTagMetadataAsync()
        {
            var endpoint = "Admin/Update";

            var uri = new Uri($"{BaseUrl}{endpoint}");
            try
            {
                using (var client = GetClient())
                {
                    client.Timeout = TimeSpan.FromMinutes(2);
                    using (var response = await client.PostAsync(uri, new StringContent(null)))
                    {
                        response.EnsureSuccessStatusCode();
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
