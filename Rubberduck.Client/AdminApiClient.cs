using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Rubberduck.Client.Abstract;
using Rubberduck.Model.Entities;

namespace Rubberduck.Client
{

    public class AdminApiClient : PublicApiClient, IAdminApiClient
    {
        public AdminApiClient(ILogger<AdminApiClient> logger, IConfiguration configuration) 
            : base(logger, configuration)
        {

        }


        protected override HttpClient GetClient()
        {
            // TODO add authentication headers
            var client = base.GetClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ContentTypeApplicationJson));
            client.DefaultRequestHeaders.AcceptCharset.Add(new StringWithQualityHeaderValue("utf-8"));
            return client;
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
                    client.Timeout = base.PostRequestTimeout;
                    using (var response = await client.PostAsync(uri, null))
                    {
                        response.EnsureSuccessStatusCode();
                        return true;
                    }
                }
            }
            catch (TaskCanceledException)
            {
                throw;
            }
            catch
            {
                return false;
            }
        }
    }
}
